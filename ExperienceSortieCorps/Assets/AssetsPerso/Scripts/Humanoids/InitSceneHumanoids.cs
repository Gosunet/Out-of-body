using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class InitSceneHumanoids : MonoBehaviour
{

    public GameObject _humanoidRight;
    public GameObject _humanoidLeft;

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
    private bool _next = false;
    private bool _stop = false;

    // Use this for initialization
    void Start()
    {
        // Activate the exercice
        _humanoidLeft.SetActive(true);
        _humanoidRight.SetActive(true);
        
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
        //GameObject modelRootLeft = _humanoidLeft.transform.FindChild("python").gameObject;
        //GameObject modelRootRight= _humanoidRight.transform.FindChild("python").gameObject;


        //_humanoidLeft.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        //modelRootLeft.transform.FindChild("hips/spine/chest/clavicle.L/upper_arm.L").transform.localRotation = new Quaternion(20, -180, 140, 0);
        //modelRootLeft.transform.FindChild("hips/spine/chest/clavicle.R/upper_arm.R").transform.localRotation = new Quaternion(-20, -180, 140, 0);
        //modelRootRight.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder").transform.localRotation = new Quaternion(20, -180.0f, 140.0f, 0.0f);
        //modelRootRight.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder").transform.localRotation = new Quaternion(-20, -180.0f, 140.0f, 0.0f);

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
}
