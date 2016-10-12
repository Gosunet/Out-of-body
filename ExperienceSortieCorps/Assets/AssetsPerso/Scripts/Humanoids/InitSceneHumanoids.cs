using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InitSceneHumanoids : MonoBehaviour {

    public GameObject _humanoidRight;
    public GameObject _humanoidLeft;

    // doors counter on the scene.
    [SerializeField]
    private GameObject _text;

    // Value got from the socket
    private int _nbRepetition;
    private int _nbDistance;
    private int _percentageDiff;

    // Value of initial position of each humanoid
    private Vector3 _posHumanoidRight;
    private Vector3 _posHumanoidLeft;
        
    // Value of initial distance between humanoids 
    private float _initialDistX;
    private float _centerDistanceX;

    // Attribute to know the time response of the patient
    private System.DateTime _time;

    // Management of the distance between the humanoids
    private int _nbTests = 0;

    private int _distanceIndex;
    private int _randomDistance;

    private List<float> _rangeDistances;
    public List<float> RangeDistances
    {
        get
        {
            if(_rangeDistances == null)
            {
                _rangeDistances = new List<float>();
            }
            return _rangeDistances;
        }

        set
        {
            _rangeDistances = value;
        }
    }

    private float _distanceMinimal;
    private float DistanceMinimal
    {
        get
        {

            _distanceMinimal = (float)(_initialDistX  - _percentageDiff/100.0 * _initialDistX);

            return _distanceMinimal;
        }

        set
        {
            _distanceMinimal = value;
        }
    }

    private float _distanceMaximal;
    public float DistanceMaximal
    {
        get
        {
            _distanceMaximal = (float)(_initialDistX + (_percentageDiff / 100.0) * _initialDistX);
            return _distanceMaximal;
        }

        set
        {
            _distanceMaximal = value;
        }
    }

    private int _testIndex;
    private int _currentDistance;

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
    void Start () {

        _humanoidLeft.SetActive(true);
        _humanoidRight.SetActive(true);

        _posHumanoidLeft = _humanoidLeft.transform.localPosition;
        _posHumanoidRight = _humanoidRight.transform.localPosition;

        string testPosHumGauche = "Position humanoid gauche: " + _posHumanoidLeft.x + ", " + _posHumanoidLeft.y + ", " + _posHumanoidLeft.z;
        string testPosHumDroite = "Position humanoid droit: " + _posHumanoidRight.x + "," + _posHumanoidRight.y + ", " + _posHumanoidRight.z;

        _initialDistX = _posHumanoidRight.x - _posHumanoidLeft.x;
        _centerDistanceX = _initialDistX / 2;

        // Debug to test 
        Debug.ClearDeveloperConsole();
        Debug.LogWarning("Distance initiale : " + _initialDistX);
        Debug.LogWarning(testPosHumGauche);
        Debug.LogWarning(testPosHumDroite);

        initParameters();

        intitDistances();
             
        applyScaleDistance();

	}

    private void initParameters()
    {
        string resSocket = PlayerPrefs.GetString(Utils.PREFS_PARAM_HUMANOID);

        string[] parameters = resSocket.Split('_');

        _nbRepetition = int.Parse(parameters[0]);
        _nbDistance = int.Parse(parameters[1]);
        _nbTests = _nbDistance;
        _percentageDiff = int.Parse(parameters[2]);

        Debug.Log("nb de repetition : " + _nbRepetition);
        Debug.Log(" nb de test : " + _nbDistance);
        Debug.Log("pourcentage de diff :" + _percentageDiff);

    }

    private void intitDistances()
    {
        if(_nbDistance > 0)
        {
            RangeDistances = getListOfDistanceValue();
        }

    }

    private List<float> getListOfDistanceValue()
    {
        List<float> rangeDistanceValue = new List<float>();

        // if nbWidth is an odd number
        if (_nbDistance % 2 != 0)
        {   
            for (int i = -_nbDistance/2; i < (_nbDistance + 1) / 2; i++)
            {
                Debug.LogWarning("i " + i);

                float value = (float)((_percentageDiff * i / 100.0 + 1.0) * _initialDistX);
                Debug.LogWarning("value " + value);

                rangeDistanceValue.Add(value);
            }
        }

        else
        {
            for (int i = -_nbDistance/2; i < _nbDistance/2; i++)
            {
                float value = (float)((_percentageDiff * i / 100.0 + 1.0) * _initialDistX + _percentageDiff * _initialDistX / (2 * 100));
                Debug.LogWarning("value ajoutée : " + value);
                rangeDistanceValue.Add(value);
            }
        }        

        return rangeDistanceValue;
    }

    //private List<float> getAleatoireDistance()
    //{
    //    List<float> rangeValeurAleatoire = new List<float>();
    //    System.Random rnd = new System.Random();

    //    while (rangeValeurAleatoire.Count <= _nbDistance)
    //    {
    //        float aleatoireDistance = (float)(rnd.NextDouble() * (DistanceMaximal - DistanceMinimal) + DistanceMinimal);

    //        if (!rangeValeurAleatoire.Contains(aleatoireDistance))
    //        {
    //            rangeValeurAleatoire.Add(aleatoireDistance);
    //            string value = "Value added : " + aleatoireDistance;
    //            Debug.LogWarning(value);
    //        }
    //    }       

    //    return rangeValeurAleatoire;
    //}

    private void applyScaleDistance()
    {
        if (RangeDistances.Count > 0)
        {
            _distanceIndex = Random.Range(0, RangeDistances.Count);

            Vector3 posLeft = _humanoidLeft.transform.localPosition;
            Vector3 posRight = _humanoidRight.transform.localPosition;
     
            float newPosX = (float)(RangeDistances[_distanceIndex] / 2.0);
            Debug.LogWarning("Range appliqué:" + newPosX);

            float step = System.Math.Abs(System.Math.Abs(posLeft.x) - System.Math.Abs(newPosX));

            if(newPosX < System.Math.Abs(posLeft.x))
            {
                step = -step;
            }

            _posHumanoidLeft.x =  step;
            _posHumanoidRight.x = -step;

            _humanoidLeft.transform.Translate(_posHumanoidLeft);
            _humanoidRight.transform.Translate(_posHumanoidRight);

            RangeDistances.RemoveAt(_distanceIndex);
            _nbDistance--;
            _nbAnswers++;

            _text.GetComponent<Text>().text = _nbAnswers.ToString() + "/" + _nbTests.ToString();

        }

    }

    // Update is called once per frame
    void Update ()
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
                if (_nbDistance> 0)
                {
                    applyScaleDistance();
                    _next = false;
                }
                else
                {
                    _stop = true;
                }
            }
        }


    }
}
