using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Globalization;
using UnityEngine.SceneManagement;
using ManageLog;

/// <summary>
/// Script called when the door scene is starting.
/// </summary>
public class InitSceneDoors : MonoBehaviour
{

    // PORTE ENTIERE
    [SerializeField]
    private GameObject _fullDoors;

    [SerializeField]
    private GameObject _topWall;

    // PORTE BAS
    [SerializeField]
    private GameObject _bottomDoors;

    // PORTE HAUT
    [SerializeField]
    private GameObject _topDoors;

    // doors counter on the scene.
    [SerializeField]
    private GameObject _text;

    // const value of the scene 
    private const float _maxHeight = 30;
    private const float _maxWidth = 50;
    private const float _standardHeight = 15;

    // Value of initial doors scales
    private float _initialDistX;
    private float _initialDistY;

    // Attribute to know time response of the patient
    private System.DateTime _time;

    // management width of the doors
    private int _nbTest;
    private int _nbRepeat;

    // liste des mesures réalisées
    private List<Measure> _listResultDistance = new List<Measure>();

    private List<Distance> _listRandomDistance = new List<Distance>();
    private List<float> _listHeight = new List<float>();
    private List<float> _listWidth = new List<float>();

    // Index used in scales list
    private int _doorIndex;     

    // Number of doors played (visible on the scene)
    private int _nbAnswers = 0;

    // Chosen model by the patient
    private float[] _modelSrcValues;
    // Chosen model by the psychologue
    private float[] _modelDstValues;
    // difference of morphology
    private float[] _differenceModels;

    // results file.
    private string _fileName;
    private XmlDocument _xmlModel;

    private List<bool> _answers = new List<bool>();
    private bool _next = false;
    private bool _stop;

    private int _doorType;

    private int initialOffset = 0;
    private GameObject LeftWall
    {
        get
        {
            if (_doorType == FilesConst.FULL_DOOR)
            {
                initialOffset = 0;
                return (GameObject)_fullDoors.transform.FindChild("MurGauche").gameObject;
            }
            else if (_doorType == FilesConst.BOTTOM_DOOR)
            {
                initialOffset = 14;
                return (GameObject)_bottomDoors.transform.FindChild("CubeBottomLeft").gameObject;
            }
            else if(_doorType == FilesConst.TOP_DOOR)
            {
                initialOffset = 14;
                return (GameObject)_topDoors.transform.FindChild("CubeTopLeft").gameObject;
            }
            else
            {
                return null;
            }
        }

    }

    private GameObject RightWall
    {
        get
        {
            if (_doorType == FilesConst.FULL_DOOR)
            {
                return (GameObject)_fullDoors.transform.FindChild("MurDroit").gameObject;
            }
            else if (_doorType == FilesConst.BOTTOM_DOOR)
            {
                return (GameObject)_bottomDoors.transform.FindChild("CubeBottomRight").gameObject;
            }
            else if (_doorType == FilesConst.TOP_DOOR)
            {
                return (GameObject)_topDoors.transform.FindChild("CubeTopRight").gameObject;
            }
            else
            {
                return null;
            }
        }
    }  

    string SEPARATOR = "\t";


    void Start()
    {
        string doors = PlayerPrefs.GetString(Utils.PREFS_DOORS);

		_topWall.SetActive(false);

        if (doors.Equals(Utils.BOTTOM_DOORS))
        {
            _bottomDoors.SetActive(true);       
            _doorType = FilesConst.BOTTOM_DOOR;
        }
        else if (doors.Equals(Utils.TOP_DOORS))
        {
            _topDoors.SetActive(true);
            _doorType = FilesConst.TOP_DOOR;
        }
        else
        {
            _fullDoors.SetActive(true);
            _topWall.SetActive(true);		
            _doorType = FilesConst.FULL_DOOR;
        }

        RightWall.SetActive(true);
        LeftWall.SetActive(true);


        // creation results file
        _fileName = System.DateTime.Now.ToString();
        _fileName = _fileName.Replace("/", "-");
        _fileName = _fileName.Replace(":", "-");

        // Init. array of doors scales.
        initListDistance();

        // Apply the first distance 
        applyDistance();

        _stop = false;      
    }

    void Update()
    {
        if (!_stop)
        {
            if (Input.GetKeyDown(KeyCode.O) || Input.GetMouseButtonDown(0))
            {                
                Reponse(true);
                _next = true;
            }
            else if (Input.GetKeyDown(KeyCode.N) || Input.GetMouseButtonDown(1))
            {              
                Reponse(false);
                _next = true;
            }

            if (_next)
            {
                // Ajout de la mesure de temps dans la liste
                _listResultDistance[_listResultDistance.Count - 1].time = (System.DateTime.Now - _time).TotalMilliseconds;

                if (_listRandomDistance.Count > 0)
                {
                    applyDistance();
                    _next = false;
                }
                else
                {
                    _stop = true;

                    string directory = PlayerPrefs.GetString(Utils.PREFS_EXPERIMENT_PATH_FOLDER);
                    if (!string.Empty.Equals(directory))
                    {
                        string username = directory.Remove(0, directory.LastIndexOf('\\') + 1).Split('_')[0];
                        Debug.Log("Avant création log - DOORS");
                        int numeroEx = recupNumeroExecice();
                        FileLog fl = new FileLog();
                        fl.createConfigFile(numeroEx);
                        fl.createResultFile(directory, username, numeroEx, _listResultDistance, _answers);
                        Debug.Log("Apres création log - DOORS");

                        //CallPythonScript(username, PlayerPrefs.GetInt(Utils.PREFS_CONDITION), Path.Combine(directory, username + ".txt"));
                    }
					RightWall.SetActive(false);
					LeftWall.SetActive(false);

                    SocketClient.GetInstance().Write(Utils.SOCKET_END_DOOR);  // Send message "doors end" to the server

                    Utils.CurrentState = State.WAITING;
                }
            }
        }
    }

    #region Initialisation des listes

    private void initListDistance()
    {     
        string resSocket = PlayerPrefs.GetString(Utils.PREFS_PARAM_DOORS);
        string[] parameters = resSocket.Split('_');

        // Récupération des paramètres envoyé via la socket
        _nbRepeat = int.Parse(parameters[0]);

        int nbWidth = int.Parse(parameters[1]);
        float minWidth = float.Parse(parameters[2])/10;
        float maxWidth = float.Parse(parameters[3])/10;

        if (maxWidth > _maxWidth) maxWidth = _maxWidth;

        int nbHeight = 0;
        if (_doorType == FilesConst.FULL_DOOR) nbHeight = int.Parse(parameters[4]);

        // liste des distances 
        List<Distance> listDistance = new List<Distance>();

        // Définition des valeurs possibles en largeur
        if (nbWidth > 0)
        {
            // Définition du pas
            float step = (float)((maxWidth - minWidth) / nbWidth);

            for (int i = 0; i < nbWidth; i++)
            {
                float value = (float)(step * i + minWidth);
                listDistance.Add(new Distance(value,_standardHeight));
            }
        }

        // La gestion de la liste en hauteur s ion est en FULL DOORS 
        if(_doorType == FilesConst.FULL_DOOR && nbHeight>1)
        {    
                float minHeight = float.Parse(parameters[5]) / 10;
                float maxHeight = float.Parse(parameters[6]) / 10;

                if (maxHeight > _maxHeight) maxHeight = _maxHeight;

                // Définition du pas
                float step = (float)((maxHeight - minHeight) / nbHeight);

                List<Distance> listDistanceTmp = new List<Distance>();
                foreach(var distance in listDistance)
                {
                    for (int j = 0; j < nbHeight; j++)
                    {
                        float value = (float)(step * j + minHeight);
                        listDistanceTmp.Add(new Distance(distance.width, value));
                    }
                }

                listDistance = listDistanceTmp;    
        }
        
        // nombre de répétition du test 
        for (int i = 0; i < _nbRepeat; i++)
        {
            _listRandomDistance.AddRange(listDistance);
        }

        // Le nombre de test total qu'il faut faire.
        _nbTest = _listRandomDistance.Count;
    }

    #endregion
    
    private void applyDistance()
    {
        _doorIndex = Random.Range(0, _listRandomDistance.Count);
        Distance newDistance = _listRandomDistance[_doorIndex];

        float newPosX = (float)(newDistance.width / 2.0);
        float newPosY = (float)(newDistance.height);

        if (_doorType == FilesConst.FULL_DOOR)
        {
            _topWall.transform.localPosition = new Vector3(_topWall.transform.localPosition.x, newPosY, _topWall.transform.localPosition.z);
            
            // Gestion des bordures des portes
            //LeftBordure.transform.localPosition = new Vector3(LeftBordure.transform.localPosition.x, 0, LeftBordure.transform.localPosition.z);
           // RightBordure.transform.localPosition = new Vector3(RightBordure.transform.localPosition.x, 0S, RightBordure.transform.localPosition.z);
        }
        RightWall.transform.localPosition = new Vector3(newPosX + initialOffset, RightWall.transform.localPosition.y, RightWall.transform.localPosition.z);
        LeftWall.transform.localPosition = new Vector3(-newPosX - initialOffset, LeftWall.transform.localPosition.y, LeftWall.transform.localPosition.z);

        _listRandomDistance.RemoveAt(_doorIndex);
        _nbAnswers++;

        _text.GetComponent<Text>().text = _nbAnswers.ToString() + "/" + _nbTest.ToString();

        _time = System.DateTime.Now;

        // Le temps est à zéro : il sera récupéré durant l'appui sur la touche par l'utilisateur.
        _listResultDistance.Add(new Measure(newPosX, newPosY, 0));
    }





    #region Gestion des fichiers de resultats
    int recupNumeroExecice()
    {
        int n = PlayerPrefs.GetInt(Utils.PREFS_NUMERO_EXERCICE);
        PlayerPrefs.SetInt(Utils.PREFS_NUMERO_EXERCICE, n + 1);
        return n;
    }

    /// <summary>
    /// Call python script
    /// </summary>
    /// <param name="username">Patient name</param>
    /// <param name="condition">Experimentation condition</param>
    /// <param name="resultFilename">Result files who's generate for the patient</param>
    void CallPythonScript(string username, int condition, string resultFilename)
    {
        new System.Diagnostics.Process()
        {
            StartInfo =
            {
                FileName = "cmd.exe",
                Arguments = "/c py drawGraphe.py " + username + " " + condition + " \"" + resultFilename + " \"",
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            }
        }.Start();
    }

    /// <summary>
    /// Reads the models value on the xml.
    /// </summary>
    /// <returns>The models value.</returns>
    /// <param name="name">Name.</param>
    public float[] readModelsValue(string name)
    {
        string nameNode = "", waist = "", hips = "", chest = "";
        foreach (XmlElement node in _xmlModel.SelectNodes("Models/Model"))
        {
            nameNode = node.GetAttribute("name");
            if (nameNode.Equals(name))
            {
                XmlNode waistNode = node.SelectSingleNode("Waist");
                waist = waistNode.InnerText;
                XmlNode chestNode = node.SelectSingleNode("Chest");
                chest = chestNode.InnerText;
                XmlNode hipsNode = node.SelectSingleNode("Hips");
                hips = hipsNode.InnerText;
            }
        }
        float[] values = new float[3];
        values[0] = float.Parse(waist);
        values[1] = float.Parse(chest);
        values[2] = float.Parse(hips);
        return values;
    }


    /// <summary>
    /// Add response into the array of _answers.
    /// </summary>
    /// <param name="rep">Patient response</param>
    void Reponse(bool rep)
    {
        _answers.Add(rep);
        _next = true;
    }


    #endregion

    #region Definition des classes

    public class Distance
    {
        public Distance(float width, float height)
        {
            _width = width;
            _height = height;
        }

        private float _width;
        private float _height;
        public float width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public float height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

    }
    #endregion

}