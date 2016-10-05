using UnityEngine;
using System.Collections;

/// <summary>
/// Init morphing.
/// </summary>
public class InitMorphing : MonoBehaviour
{

    [SerializeField]
    private Material _jeanGhost;
    [SerializeField]
    private Material _shirtGhost;

    /// <summary>
    /// The morphing's speed
    /// </summary>
    private float _speed = 0.016f;

    /// <summary>
    /// The avatar in the scene
    /// </summary>
    private GameObject _goSrc;

    private GameObject _jean;
    private GameObject _shirt;

    void Start()
    {
        string[] model = PlayerPrefs.GetString(Utils.PREFS_MODEL).Split(';');
        _goSrc = GameObject.Find(model[0].Split('/')[2]);
        GameObject goDst = (GameObject)Resources.Load(model[1]);

        Debug.Log("goSrc");
        Debug.Log(model[0]);
        Debug.Log("goDest");
        Debug.Log(model[1]);

        _jean = (GameObject)Instantiate(_goSrc.transform.FindChild("jeans01Mesh").gameObject);
        _jean.name = "jeanGhost";
        _jean.transform.parent = _goSrc.transform;
        _jean.GetComponent<Renderer>().material = _jeanGhost;

        _shirt = (GameObject)Instantiate(_goSrc.transform.FindChild("shirt01Mesh").gameObject);
        _shirt.name = "shirtGhost";
        _shirt.transform.parent = _goSrc.transform;
        _shirt.GetComponent<Renderer>().material = _shirtGhost;

        bool activateGhost = (PlayerPrefs.GetInt(Utils.PREFS_GHOST) == 1);
        _jean.SetActive(activateGhost);
        _shirt.SetActive(activateGhost);

        init("high-polyMesh");
        init("jeans01Mesh");
        init("shirt01Mesh");

        if (model[0].Contains(Utils.MODELS_DIRECTORY[0]))
        {   // If the avatar is a man
            init("male1591Mesh");
        }
        else
        {   // If the avatar is a woman
            init("female1605Mesh");
        }

        foreach (MorphingAvatar morph in _goSrc.GetComponentsInChildren<MorphingAvatar>())
        {
            Mesh meshSrc = morph.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            Mesh meshDst = goDst.transform.FindChild(morph.gameObject.name).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            morph.dstMesh = meshDst;
            morph.srcMesh = meshSrc;
        }

        SetLayerRecursively(_goSrc, 8);
    }

    /// <summary>
    /// Initializes the morphing for the avatar's limb specified by his name
    /// </summary>
    /// <param name="name">The name of the avatar's limb</param>
    void init(string name)
    {
        MorphingAvatar morph = _goSrc.transform.FindChild(name).gameObject.AddComponent<MorphingAvatar>();
        morph.speed = _speed;
    }

    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    public Material jeanGhost
    {
        set
        {
            _jeanGhost = value;
        }
    }

    public Material shirtGhost
    {
        set
        {
            _shirtGhost = value;
        }
    }
}
