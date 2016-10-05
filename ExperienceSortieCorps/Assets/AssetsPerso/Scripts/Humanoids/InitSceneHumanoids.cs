using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitSceneHumanoids : MonoBehaviour {

    public GameObject _humanoidRight;
    public GameObject _humanoidLeft;

    public int nb_distance;
    public int distance_step;

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

            _distanceMinimal = (float)(_initialDistX  - distance_step/100.0 * _initialDistX);

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
            _distanceMaximal = (float)(_initialDistX + (distance_step / 100.0) * _initialDistX);
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

        intitDistances();
             
        applyScaleDistance();


	}

    private void intitDistances()
    {
        if(nb_distance > 0)
        {
            RangeDistances = getListOfDistanceValue();
        }

    }

    private List<float> getListOfDistanceValue()
    {
        List<float> rangeDistanceValue = new List<float>();

        // if nbWidth is an odd number
        if (nb_distance % 2 != 0)
        {   
            for (int i = -nb_distance/2; i < (nb_distance + 1) / 2; i++)
            {
                Debug.LogWarning("i " + i);

                float value = (float)((distance_step * i / 100.0 + 1.0) * _initialDistX);
                Debug.LogWarning("value " + value);

                rangeDistanceValue.Add(value);
            }
        }

        else
        {
            for (int i = -nb_distance/2; i < nb_distance/2; i++)
            {
                Debug.LogWarning("i " + i);

                float value = (float)((distance_step * i / 100.0 + 1.0) * _initialDistX + distance_step * _initialDistX / (2 * 100));
                Debug.LogWarning("value ajoutée : " + value);
                rangeDistanceValue.Add(value);
            }
        }        

        return rangeDistanceValue;
    }


    private List<float> getAleatoireDistance()
    {
        List<float> rangeValeurAleatoire = new List<float>();
        System.Random rnd = new System.Random();

        while (rangeValeurAleatoire.Count <= nb_distance)
        {
            float aleatoireDistance = (float)(rnd.NextDouble() * (DistanceMaximal - DistanceMinimal) + DistanceMinimal);

            if (!rangeValeurAleatoire.Contains(aleatoireDistance))
            {
                rangeValeurAleatoire.Add(aleatoireDistance);
                string value = "Value added : " + aleatoireDistance;
                Debug.LogWarning(value);
            }
        }       

        return rangeValeurAleatoire;
    }

    private void applyScaleDistance()
    {
        if (RangeDistances.Count > 0)
        {
            _distanceIndex = Random.Range(0, RangeDistances.Count);

            Vector3 posLeft = _humanoidLeft.transform.localPosition;
            Vector3 posRight = _humanoidRight.transform.localPosition;
     

            float newPosX = (float)(RangeDistances[_distanceIndex] / 2.0);
            Debug.LogWarning("Range appliqué:" + newPosX);

            float step = System.Math.Abs(System.Math.Abs(posLeft.x) - newPosX);

            if(newPosX < System.Math.Abs(posLeft.x))
            {
                step = -step;
            }

            _posHumanoidLeft.x =  step;
            _posHumanoidRight.x = -step;

            _humanoidLeft.transform.Translate(_posHumanoidLeft);
            _humanoidRight.transform.Translate(_posHumanoidRight);

            RangeDistances.RemoveAt(_distanceIndex);
            nb_distance--;
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
                if (nb_distance> 0)
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
