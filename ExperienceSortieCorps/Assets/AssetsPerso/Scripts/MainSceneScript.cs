using UnityEngine;
using System.Collections;

public class MainSceneScript : MonoBehaviour
{

    // Reference pour les différents exercices
    [SerializeField]
    private GameObject _DoorsExercices;

    [SerializeField]
    private GameObject _CreatePicture;

    [SerializeField]
    private GameObject _OutOfBody;

   // public bool IsExerciceEnded;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Initialisation de la scène principale");
        Debug.Log(Utils.CurrentState);

        // On désactive tout à l'initialisation
        _DoorsExercices.SetActive(false);
        _CreatePicture.SetActive(false);
        _OutOfBody.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Activation de la scène adéquate

        switch (Utils.CurrentState)
        {
            case State.CREATE_PICTURE:
                // Activation des objets de la scène
                _CreatePicture.SetActive(true);
                //IsExerciceEnded = false;
                break;
            case State.OUT_OF_BODY:
                _OutOfBody.SetActive(true);
               // IsExerciceEnded = false;
                break;
            case State.DOORS:
                _DoorsExercices.SetActive(true);
                //IsExerciceEndedEnded = false;
                break;
            default:
                _DoorsExercices.SetActive(false);
                _CreatePicture.SetActive(false);
                _OutOfBody.SetActive(false);
                break;
        }

        // TO DO : Récupérer un signal STOP des différents scripts


    }


}