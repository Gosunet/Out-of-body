using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.IO;

public class InitSceneHumanoids : MonoBehaviour
{
    [SerializeField]
    private GameObject _humanoidRight;

    [SerializeField]
    private GameObject _humanoidLeft;

    // doors counter on the scene.
    [SerializeField]
    private GameObject _text;

    private const float _minDistance = 4;
    private const float _maxDistance = 50;

    // Value got from the socket
    private int _nbRepetition;
    private int _nbDistance;
    private int _percentageDiff;

    // Value of initial position of each humanoid
    private Vector3 _posHumanoidRight;
    private Vector3 _posHumanoidLeft;

    // Attribute to know the time response of the patient
    private System.DateTime _time;

    // Management of the distance between the humanoids
    private int _nbTests = 0;

    private int _distanceIndex;
    private int _randomDistance;

    private List<float> _listRangeDistance = new List<float>();
    private List<float> _listRandomDistance = new List<float>();

    private float _distanceMinimal;
    private float _distanceMaximal;

    // Number of tests achived
    private int _nbAnswers = 0;

    // Chosen model by the patient
    private float[] _modelSrcValues;
    // Chosen model by the psychologue
    private float[] _modelDstValues;
    // difference of morphology
    private float[] _differenceModels;

    // Variables for results 
    private string _fileName;
    private bool _next = false;
    private bool _stop = false;
    // results file.
    private string _fileName;
    private XmlDocument _xmlModel;

    private List<bool> _answers = new List<bool>();

    string SEPARATOR = "\t";

    // Use this for initialization
    void Start()
    {
        // Activate the exercice
        _humanoidLeft.SetActive(true);
        _humanoidRight.SetActive(true);

        // Creation of results file 
        _fileName = System.DateTime.Now.ToString();

        // Initialize the models 
        initModels();

        //  initialize the paramaters received from the socket
        initParameters();

        // Initialize the range of distance
        initDistances();

        // Apply the distance scale 
        applyScaleDistance();

    }

    private void initModels()
    {
        // --------- Initialisation de l'humanoide de droite 
        // Position
        _humanoidRight.transform.localPosition = new Vector3(4, 0, 2);

        // Scale
        _humanoidRight.transform.localScale = new Vector3(8, 8, 8);

        // Rotation des bras 
        _humanoidRight.transform.FindChild("python/Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra").transform.localRotation = Quaternion.Euler(-58f, -32f, 39.2f);  
        _humanoidRight.transform.FindChild("python/Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra").transform.localRotation = Quaternion.Euler(-58f, 32f, -39.2f); 

        // --------- Initilisation de l'humanoide de gauche 
        // Position
        _humanoidLeft.transform.localPosition = new Vector3(-4, 0, 2);

        // Scale
        _humanoidLeft.transform.localScale = new Vector3(8, 8, 8);

        // Rotation des bras 
        _humanoidLeft.transform.FindChild("python/Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra").transform.localRotation = Quaternion.Euler(-98.2f, -55f, 39.2f); 
        _humanoidLeft.transform.FindChild("python/Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra").transform.localRotation = Quaternion.Euler(-98, 55f, -39.2f); 

    }

    private void initParameters()
    {
        string resSocket = PlayerPrefs.GetString(Utils.PREFS_PARAM_HUMANOID);

        string[] parameters = resSocket.Split('_');

        // Parameters receivied from the socket
        _nbRepetition = int.Parse(parameters[0]);
        _nbDistance = int.Parse(parameters[1]);
        _nbTests = _nbDistance;
        _distanceMinimal = int.Parse(parameters[2]) / 10;
        if (_distanceMinimal < _minDistance) _distanceMinimal = _minDistance;

        _distanceMaximal = int.Parse(parameters[3]) / 10;
        if (_distanceMaximal > _maxDistance) _distanceMaximal = _maxDistance;

    }

    private void initDistances()
    {
        System.Random rnd = new System.Random();

        if (_nbDistance > 0)
        {
            List<float> listDistance = new List<float>();

            // Définition du pas
            float step = (float)((_distanceMaximal - _distanceMinimal) / _nbDistance);

            // initialiser une liste à pas fixe
            for (int i = 0; i < _nbDistance; i++)
            {
                float value = (float)(step * i + _distanceMinimal);
                listDistance.Add(value);
            }

            // Création de la liste de répétition 
            for (int i = 0; i < _nbRepetition; i++)
            {
                _listRangeDistance.AddRange(listDistance);
            }

            _nbTests = _listRangeDistance.Count;
        }
    }

    private void applyScaleDistance()
    {
        if (_listRangeDistance.Count > 0)
        {
            _distanceIndex = UnityEngine.Random.Range(0, _listRangeDistance.Count);

            float newPosX = (float)(_listRangeDistance[_distanceIndex] / 2.0);

            // Définition de la nouvelle position des humanoides 
            _humanoidLeft.transform.localPosition = new Vector3(-newPosX, _humanoidLeft.transform.localPosition.y, _humanoidLeft.transform.localPosition.z);
            _humanoidRight.transform.localPosition = new Vector3(newPosX, _humanoidLeft.transform.localPosition.y, _humanoidLeft.transform.localPosition.z);

            _listRangeDistance.RemoveAt(_distanceIndex);
            _nbDistance--;
            _nbAnswers++;

            _text.GetComponent<Text>().text = _nbAnswers.ToString() + "/" + _nbTests.ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!_stop)
        {
            if (Input.GetKeyDown(KeyCode.O) || Input.GetMouseButtonDown(0))
            {
                _next = true;
            }
            else if (Input.GetKeyDown(KeyCode.N) || Input.GetMouseButtonDown(1))
            {
                _next = true;
            }

            if (_next)
            {
                if (_listRangeDistance.Count > 0)
                {
                    applyScaleDistance();
                    _next = false;
                }
                else
                {
                    _stop = true;
                    SocketClient.GetInstance().Write(Utils.SOCKET_END_DOOR);  // Send message "end of exercice" to the server
                    Utils.CurrentState = State.WAITING;
                }
            }
        }


    }

    /*
    // Load the XML from the assets 
    void loadXMLFromAssest()
    {
        _xmlModel = new XmlDocument();
        TextAsset textXml = (TextAsset)Resources.Load("Models", typeof(TextAsset));
        _xmlModel.LoadXml(textXml.text);
    }

    /// <summary>
    /// Creates the result file.
    /// </summary>
    /// <param name="dir">directory</param>
    /// <param name="username">Name of the patient</param>
    void CreateResultFile(string dir, string username)
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

        //for (int nbDoor = 0; nbDoor < _listResultDistance.Count; nbDoor++)
        //{
           //file.WriteLine((nbDoor + 1).ToString() + SEPARATOR + condition.ToString() + SEPARATOR + _doorType.ToString() + SEPARATOR + _listResultDistance[nbDoor].width.ToString() + SEPARATOR + _listResultDistance[nbDoor].height.ToString() + SEPARATOR + (_answers[nbDoor] == true ? "1" : "0") + SEPARATOR + modelSrcvalue + SEPARATOR + modelDstvalue + SEPARATOR + modelDiffvalue + SEPARATOR + _listResultDistance[nbDoor].time.ToString());
        //}
        //file.Close();
    }

    
   // void createTxT(string username)
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
    /// Writes the or update moyenne.
    /// </summary>
    /// <returns>The or update moyenne.</returns>
    /// <param name="condition">Condition.</param>
    /// <param name="moyenne">Moyenne.</param>
    /// <param name="parameters">Parameters.</param>
    string WriteOrUpdateMoyenne(int condition, float moyenne, string[] parameters)
    {
        return WriteOrUpdateParameter(condition, parameters, moyenne, 10);
    }

    string WriteOrUpdatePSE(int condition, float pse, string[] parameters)
    {
        return WriteOrUpdateParameter(condition, parameters, pse, 14);
    }

    /// <summary>
    /// Prepare or update string who's write on the result file.
    /// </summary>
    /// <returns>The new or update string parameter's.</returns>
    /// <param name="condition">Condition.</param>
    /// <param name="parameters">Parameters.</param>
    /// <param name="newValue">New value.</param>
    /// <param name="baseIndex">Base index.</param>
    string WriteOrUpdateParameter(int condition, string[] parameters, float newValue, int baseIndex)
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

    /// <summary>
    /// Reads the models value on the xml.
    /// </summary>
    /// <returns>The models value.</returns>
    /// <param name="name">Name.</param>
    float[] ReadModelsValue(string name)
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
    /// calculate the difference bewteen patient and psycholog model.
    /// </summary>
    /// <returns>The ecart.</returns>
    /// <param name="src">Source.</param>
    /// <param name="dst">Dst.</param>
    float[] calculEcart(float[] src, float[] dst)
    {
        float[] resultat = new float[3];
        for (int i = 0; i < resultat.Length; i++)
            resultat[i] = (dst[i] - src[i]) / src[i] * 100;
        return resultat;
    }
    */


}
