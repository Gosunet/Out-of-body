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

    [SerializeField]
    private GameObject _HumanoidsExercice;

    //public bool IsExerciceEnded;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Initialisation de la scène principale");
        Debug.Log(Utils.CurrentState);

        // On désactive tout à l'initialisation
        _DoorsExercices.SetActive(false);
        _CreatePicture.SetActive(false);
        _OutOfBody.SetActive(false);
        _HumanoidsExercice.SetActive(false);
        
        Debug.Log("Fin de l'initialisation");
        Debug.Log(Utils.CurrentState);
    }

    // Update is called once per frame
    void Update()
    {
        // Activation de la scène adéquate
        switch (Utils.CurrentState)
        {
            // Cet exercice sert à prendre des captures d'écrans, mais n'est pas utilisé pour le moment.
            case State.CREATE_PICTURE:
                _CreatePicture.SetActive(true);
                break;
            // Permet de choisir les avatars ainsi de faire les exercices de morphing.
            case State.OUT_OF_BODY:
                _OutOfBody.SetActive(true);
                break;
            // Exercice de choix des portes
            case State.DOORS:
                _DoorsExercices.SetActive(true);
                break;
            case State.HUMANOID:
                _HumanoidsExercice.SetActive(true);
                break;

            // Par défaut, on désactive tout
            default:
                _DoorsExercices.SetActive(false);
                _CreatePicture.SetActive(false);
                _OutOfBody.SetActive(false);
                _HumanoidsExercice.SetActive(false);

                break;
        }
    }




}