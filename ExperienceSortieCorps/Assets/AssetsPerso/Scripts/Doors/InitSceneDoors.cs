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
            else if (_doorType == FilesConst.TOP_DOOR)
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


    //string SEPARATOR = "\t";

    void Start()
    {
        string doors = PlayerPrefs.GetString(Utils.PREFS_DOORS);
        //_topWall.SetActive(false);

        if (doors.Equals(Utils.BOTTOM_DOORS))
        {
            _bottomDoors.SetActive(true);
            _topWall.SetActive(false);

            _doorType = FilesConst.BOTTOM_DOOR;
        }
        else if (doors.Equals(Utils.TOP_DOORS))
        {
            _topDoors.SetActive(true);
            _topWall.SetActive(false);

            _doorType = FilesConst.TOP_DOOR;
        }
        else
        {
            _fullDoors.SetActive(true);
            //_rightWall.SetActive(true);
            //_leftWall.SetActive(true);
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

        // Load XML from assets
        //loadXMLFromAssest();

        // Init the model
        //initModel();

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

                    string directory = PlayerPrefs.GetString(Utils.PREFS_PATH_FOLDER);
                    if (!string.Empty.Equals(directory))
                    {
                        string username = directory.Remove(0, directory.LastIndexOf('\\') + 1).Split('_')[0];

                        int numeroEx = recupNumeroExecice();
                        FileLog fl = new FileLog();
                        fl.createConfigFile(numeroEx);
                        fl.createResultFile(directory, username, numeroEx, _listResultDistance, _answers);

                        //CreateResultFile(directory, username);
                        //createTxT(username);

                        CallPythonScript(username, PlayerPrefs.GetInt(Utils.PREFS_CONDITION), Path.Combine(directory, username + ".txt"));
                    }
                    SocketClient.GetInstance().Write(Utils.SOCKET_END_DOOR);  // Send message "doors end" to the server

                    Utils.CurrentState = State.WAITING;
                }
            }
        }
    }

    #region Initialisation des listes
    /*private void initModel()
    {
        // Get name of the current model
        string[] modelName = PlayerPrefs.GetString(Utils.PREFS_MODEL).Split(';');
        if (!modelName[0].Equals(""))
        {
            _modelSrcValues = ReadModelsValue(modelName[0].Split('/')[2]);  //Patient model save at the index 0 of the "modelName" array
            _modelDstValues = ReadModelsValue(modelName[1].Split('/')[2]);  //Psychologue model save at the index 0 of the "modelName" array
            _differenceModels = calculEcart(_modelSrcValues, _modelDstValues);
        }

    }*/

    private void initListDistance()
    {
        string resSocket = PlayerPrefs.GetString(Utils.PREFS_PARAM_DOORS);
        string[] parameters = resSocket.Split('_');

        // Récupération des paramètres envoyé via la socket
        _nbRepeat = int.Parse(parameters[0]);

        int nbWidth = int.Parse(parameters[1]);
        float minWidth = float.Parse(parameters[2]) / 10;
        float maxWidth = float.Parse(parameters[3]) / 10;

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
                listDistance.Add(new Distance(value, _standardHeight));
            }
        }

        // La gestion de la liste en hauteur s ion est en FULL DOORS 
        if (_doorType == FilesConst.FULL_DOOR && nbHeight > 1)
        {
            float minHeight = float.Parse(parameters[5]) / 10;
            float maxHeight = float.Parse(parameters[6]) / 10;

            if (maxHeight > _maxHeight) maxHeight = _maxHeight;

            // Définition du pas
            float step = (float)((maxHeight - minHeight) / nbHeight);

            List<Distance> listDistanceTmp = new List<Distance>();
            foreach (var distance in listDistance)
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

        //if (_doorType == FilesConst.BOTTOM_DOOR)
        //{
        //    _cubeBottomLeft.transform.localPosition = new Vector3(-newPosX - 14, _cubeBottomLeft.transform.localPosition.y, _cubeBottomLeft.transform.localPosition.z);
        //    _cubeBottomRight.transform.localPosition = new Vector3(newPosX + 14, _cubeBottomRight.transform.localPosition.y, _cubeBottomRight.transform.localPosition.z);
        //}
        //else if (_doorType == FilesConst.TOP_DOOR)
        //{ 
        //    // _____________ ENTOURLOUPE _________________ Le 14 sauvage !! Mais ça marche.
        //    _cubeTopLeft.transform.localPosition = new Vector3(-newPosX - 14, _cubeTopLeft.transform.localPosition.y, _cubeTopLeft.transform.localPosition.z);
        //    _cubeTopRight.transform.localPosition = new Vector3(newPosX + 14, _cubeTopRight.transform.localPosition.y, _cubeTopRight.transform.localPosition.z);
        //}
        //else
        //{
        //     // Positionnement des murs
        //    _leftWall.transform.localPosition = new Vector3(-newPosX, _leftWall.transform.localPosition.y, _leftWall.transform.localPosition.z);
        //    _rightWall.transform.localPosition = new Vector3(newPosX, _rightWall.transform.localPosition.y, _rightWall.transform.localPosition.z);
        //    _topWall.transform.localPosition = new Vector3(_topWall.transform.localPosition.x, newPosY, _topWall.transform.localPosition.z);
        //    Debug.Log("Nouvelle position Y : " + newPosY.ToString());
        //}


        if (_doorType == FilesConst.FULL_DOOR)
        {
            _topWall.transform.localPosition = new Vector3(_topWall.transform.localPosition.x, newPosY, _topWall.transform.localPosition.z);
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

    /// <summary>
    /// Loads the XML from assest.
    /// </summary>
    /*void loadXMLFromAssest()
    {
        _xmlModel = new XmlDocument();
        TextAsset textXml = (TextAsset)Resources.Load("Models", typeof(TextAsset));
        _xmlModel.LoadXml(textXml.text);
    }*/

    /// <summary>
    /// Creates the result file.
    /// </summary>
    /// <param name="dir">directory</param>
    /// <param name="username">Name of the patient</param>
    /*void CreateResultFile(string dir, string username)
    {
        string modelSrcvalue;
        string modelDstvalue;
        string modelDiffvalue;
        if (_modelSrcValues != null)
        {
            modelSrcvalue = _modelSrcValues[0].ToString() + SEPARATOR + _modelSrcValues[1].ToString() + SEPARATOR + _modelSrcValues[2].ToString();
            modelDstvalue = _modelDstValues[0].ToString() + SEPARATOR + _modelDstValues[1].ToString() + SEPARATOR + _modelDstValues[2].ToString();
            modelDiffvalue = _differenceModels[0].ToString() + SEPARATOR + _differenceModels[1].ToString() + SEPARATOR + _differenceModels[2].ToString();
        }
        else
        {
            modelSrcvalue = "0" + SEPARATOR + "0" + SEPARATOR + "0";
            modelDstvalue = "0" + SEPARATOR + "0" + SEPARATOR + "0";
            modelDiffvalue = "0" + SEPARATOR + "0" + SEPARATOR + "0";
        }
        string filename = Path.Combine(dir, username + ".txt");
        StreamWriter file = null;
        if (!File.Exists(filename))
        {
            file = new StreamWriter(filename, true);
            file.WriteLine("Essai" + SEPARATOR +
                            "Condition" + SEPARATOR +
                            "Type de porte" + SEPARATOR +
                            "Largeur de porte" + SEPARATOR +
                            "Hauteur de porte" + SEPARATOR +
                            "Reponse" + SEPARATOR +
                            "Corpulence utilisateur" + SEPARATOR + SEPARATOR + SEPARATOR +
                            "Corpulence docteur" + SEPARATOR + SEPARATOR + SEPARATOR +
                            "Difference de corpulence" + SEPARATOR + SEPARATOR + SEPARATOR +
                            "Temps de reponse");
        }
        else
        {
            file = new StreamWriter(filename, true);
        }
        int condition = PlayerPrefs.GetInt(Utils.PREFS_CONDITION);

        for (int nbDoor = 0; nbDoor < _listResultDistance.Count; nbDoor++)
        {
            file.WriteLine((nbDoor + 1).ToString() + SEPARATOR + condition.ToString() + SEPARATOR + _doorType.ToString() + SEPARATOR + _listResultDistance[nbDoor].width.ToString() + SEPARATOR + _listResultDistance[nbDoor].height.ToString() + SEPARATOR + (_answers[nbDoor] == true ? "1" : "0") + SEPARATOR + modelSrcvalue + SEPARATOR + modelDstvalue + SEPARATOR + modelDiffvalue + SEPARATOR + _listResultDistance[nbDoor].time.ToString());
        }
        file.Close();
    }/*

    /// <summary>
    /// Creates the text file.
    /// </summary>
    /// <param name="username">Patient name</param>
    /*void createTxT(string username)
    {
        string modelSrcvalue;
        string modelDstvalue;
        string modelDiffvalue;
        if (_modelSrcValues != null)
        {
            modelSrcvalue = _modelSrcValues[0].ToString() + SEPARATOR + _modelSrcValues[1].ToString() + SEPARATOR + _modelSrcValues[2].ToString();
            modelDstvalue = _modelDstValues[0].ToString() + SEPARATOR + _modelDstValues[1].ToString() + SEPARATOR + _modelDstValues[2].ToString();
            modelDiffvalue = _differenceModels[0].ToString() + SEPARATOR + _differenceModels[1].ToString() + SEPARATOR + _differenceModels[2].ToString();
        }
        else
        {
            modelSrcvalue = "0" + SEPARATOR + "0 " + SEPARATOR + "0";
            modelDstvalue = "0" + SEPARATOR + "0 " + SEPARATOR + "0";
            modelDiffvalue = "0" + SEPARATOR + "0 " + SEPARATOR + "0";
        }

        string fileName = Path.Combine(FilesConst.SAVE_FILES_DIRECTORY, FilesConst.FILENAME_RESULT_TXT);
        StreamWriter fileWritter = null;

        bool newFile = false;

        if (!File.Exists(fileName))
        {
            fileWritter = new StreamWriter(fileName);
            fileWritter.WriteLine("Participant" + SEPARATOR +
                                   "Choix patient" + SEPARATOR + SEPARATOR + SEPARATOR +
                                   "Choix experimentateur" + SEPARATOR + SEPARATOR + SEPARATOR +
                                   "Difference corpulence" + SEPARATOR + SEPARATOR + SEPARATOR +
                                   "Type porte" + SEPARATOR +
                                   "Moyenne largeur OUI" + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR +
                                   "PSE");
            fileWritter.WriteLine(SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR +
                                   "C1" + SEPARATOR + "C2" + SEPARATOR + "C3" + SEPARATOR + "C4" + SEPARATOR +
                                   "C1" + SEPARATOR + "C2" + SEPARATOR + "C3" + SEPARATOR + "C4");

            newFile = true;
            fileWritter.Close();
        }

        int nbOui = 0;
        float moyenne = 0;
        for (int i = 0; i < _answers.Count; i++)
        {
            if (_answers[i])
            {
                moyenne += _listResultDistance[i].width;
                nbOui++;
            }
        }
        moyenne = (nbOui > 0) ? moyenne / nbOui : 0;

        float pse = 0;

        string[] lines = File.ReadAllLines(fileName);

        string[] parameters = lines[lines.Length - 1].Split('\t');

        int condition = PlayerPrefs.GetInt(Utils.PREFS_CONDITION);

        string emptyParam = "/";

        if (!parameters[0].Equals(username) || newFile)
        {   // Add a new line
            fileWritter = new StreamWriter(fileName, true);
            string res = username + SEPARATOR + modelSrcvalue + SEPARATOR + modelDstvalue + SEPARATOR + modelDiffvalue + SEPARATOR + _doorType + SEPARATOR;
            for (int i = 1; i < 5; i++)
            {
                if (condition == i) res += moyenne + SEPARATOR;
                else res += emptyParam + SEPARATOR;
            }
            for (int i = 1; i < 5; i++)
            {
                if (condition == i) res += pse.ToString() + SEPARATOR;
                else res += emptyParam + SEPARATOR;
            }
            fileWritter.WriteLine(res);
            fileWritter.Close();
        }
        else
        {   // Update the last line.
            string res = "";

            for (int i = 0; i < 11; i++)
                res += parameters[i] + SEPARATOR;

            res += WriteOrUpdateMoyenne(condition, moyenne, parameters);
            res += WriteOrUpdatePSE(condition, pse, parameters);

            lines[lines.Length - 1] = res;
            File.WriteAllLines(fileName, lines);
        }
    }*/

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
    /// Writes the or update moyenne.
    /// </summary>
    /// <returns>The or update moyenne.</returns>
    /// <param name="condition">Condition.</param>
    /// <param name="moyenne">Moyenne.</param>
    /// <param name="parameters">Parameters.</param>
    /*string WriteOrUpdateMoyenne(int condition, float moyenne, string[] parameters)
    {
        return WriteOrUpdateParameter(condition, parameters, moyenne, 10);
    }*/

    /*string WriteOrUpdatePSE(int condition, float pse, string[] parameters)
    {
        return WriteOrUpdateParameter(condition, parameters, pse, 14);
    }*/

    /// <summary>
    /// Prepare or update string who's write on the result file.
    /// </summary>
    /// <returns>The new or update string parameter's.</returns>
    /// <param name="condition">Condition.</param>
    /// <param name="parameters">Parameters.</param>
    /// <param name="newValue">New value.</param>
    /// <param name="baseIndex">Base index.</param>
    /*string WriteOrUpdateParameter(int condition, string[] parameters, float newValue, int baseIndex)
    {
        string res = "";
        for (int i = 1; i < 5; i++)
        {
            if (condition == i)
                res += newValue.ToString() + SEPARATOR;
            else
                res += parameters[baseIndex + i] + SEPARATOR;
        }
        return res;
    }*/

    /// <summary>
    /// Add response into the array of _answers.
    /// </summary>
    /// <param name="rep">Patient response</param>
    void Reponse(bool rep)
    {
        _answers.Add(rep);
        _next = true;
    }

    int recupNumeroExecice()
    {
        int n = PlayerPrefs.GetInt(Utils.PREFS_NUMERO_EXERCICE);
        PlayerPrefs.SetInt(Utils.PREFS_NUMERO_EXERCICE, n + 1);
        return n;
    }

    /// <summary>
    /// Reads the models value on the xml.
    /// </summary>
    /// <returns>The models value.</returns>
    /// <param name="name">Name.</param>
    /*float[] ReadModelsValue(string name)
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
    }*/

    /// <summary>
    /// calculate the difference bewteen patient and psycholog model.
    /// </summary>
    /// <returns>The ecart.</returns>
    /// <param name="src">Source.</param>
    /// <param name="dst">Dst.</param>
    /*float[] calculEcart(float[] src, float[] dst)
    {
        float[] resultat = new float[3];
        for (int i = 0; i < resultat.Length; i++)
            resultat[i] = (dst[i] - src[i]) / src[i] * 100;
        return resultat;
    }*/

    #region Definition des classes
    /*public class Measure
    {
        private float _width;
        private float _height;
        private double _time;

        public Measure(float width, float height, double time)
        {
            _width = width;
            _height = height;
            _time = time;
        }


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

        public double time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = ((int)value) / 1000.0;
            }
        }
    }*/



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