using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using ManageLog;

/// <summary>
/// Script launched when the user has to select an avatar
/// </summary>
public class SelectModel : MonoBehaviour
{
    /// <summary>
    /// Refers to the positionAvatar Gameobject of the scene
    /// </summary>
    [SerializeField]
    private GameObject _posAvatar;

    /// <summary>
    /// An array containing all the models ( = avatars )
    /// </summary>
    private GameObject[] _go_models;

    /// <summary>
    /// The model shown in the scene
    /// </summary>
    private GameObject _avatar;

    /// <summary>
    /// The index of the avatar in the avatar list
    /// </summary>
    private int _avatarIndex = 0;

    /// <summary>
    /// The _gender chosen by the user
    /// </summary>
    private int _gender;

    private float _offsetYPosAvatar = 0.68f;

    void Start()
    {
        _gender = PlayerPrefs.GetInt(Utils.PREFS_AVATAR_GENDER); // 0: man ; 1: woman


        // Non trié
        //_go_models = Resources.LoadAll<GameObject> (Utils.MODELS_DIRECTORY [_gender]);

        //trié
        _go_models = new GameObject[24];
        if (_gender == 0)
        { // man
            _go_models[0] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "04MaleHCLM") as GameObject;
            _go_models[1] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "09MaleLHCLM") as GameObject;
            _go_models[2] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "03MaleHCHLM") as GameObject;
            _go_models[3] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "08MaleLHCHLM") as GameObject;
            _go_models[4] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "01MaleHCLHM") as GameObject;
            _go_models[5] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "14MaleMCLM") as GameObject;
            _go_models[6] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "05MaleLHCHM") as GameObject;
            _go_models[7] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "02MaleHCMM") as GameObject;
            _go_models[8] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "13MaleMCHLM") as GameObject;
            _go_models[9] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "06MaleLHCLHM") as GameObject;
            _go_models[10] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "07MaleLHCMM") as GameObject;
            _go_models[11] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "12MaleMCMM") as GameObject;
            _go_models[12] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "17MaleHLCMM") as GameObject;
            _go_models[13] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "22MaleLCMM") as GameObject;
            _go_models[14] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "11MaleMCLHM") as GameObject;
            _go_models[15] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "18MaleHLCHLM") as GameObject;
            _go_models[16] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "16MaleHLCLHM") as GameObject;
            _go_models[17] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "21MaleLCLHM") as GameObject;
            _go_models[18] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "10MaleMCHM") as GameObject;
            _go_models[19] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "19MaleHLCLM") as GameObject;
            _go_models[20] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "15MaleHLCHM") as GameObject;
            _go_models[21] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "20MaleLCHM") as GameObject;
            _go_models[22] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "23MaleLCHLM") as GameObject;
            _go_models[23] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "24MaleLCLM") as GameObject;
        }
        else
        { // woman
            _go_models[0] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "04FemaleHCLM") as GameObject;
            _go_models[1] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "09FemaleLHCLM") as GameObject;
            _go_models[2] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "03FemaleHCHLM") as GameObject;
            _go_models[3] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "08FemaleLHCHLM") as GameObject;
            _go_models[4] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "14FemaleMCLM") as GameObject;
            _go_models[5] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "01FemaleHCLHM") as GameObject;
            _go_models[6] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "13FemaleMCHLM") as GameObject;
            _go_models[7] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "02FemaleHCMM") as GameObject;
            _go_models[8] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "07FemaleLHCMM") as GameObject;
            _go_models[9] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "12FemaleMCMM") as GameObject;
            _go_models[10] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "16FemaleHLCLHM") as GameObject;
            _go_models[11] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "17FemaleHLCMM") as GameObject;
            _go_models[12] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "11FemaleMCLHM") as GameObject;
            _go_models[13] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "10FemaleMCHM") as GameObject;
            _go_models[14] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "05FemaleLHCHM") as GameObject;
            _go_models[15] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "06FemaleLHCLHM") as GameObject;
            _go_models[16] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "22FemaleLCMM") as GameObject;
            _go_models[17] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "21FemaleLCLHM") as GameObject;
            _go_models[18] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "20FemaleLCHM") as GameObject;
            _go_models[19] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "15FemaleHLCHM") as GameObject;
            _go_models[20] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "18FemaleHLCHLM") as GameObject;
            _go_models[21] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "19FemaleHLCLM") as GameObject;
            _go_models[22] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "23FemaleLCHLM") as GameObject;
            _go_models[23] = Resources.Load(Utils.MODELS_DIRECTORY[_gender] + "24FemaleLCLM") as GameObject;

        }






        _avatar = (GameObject)Instantiate(_go_models[_avatarIndex]);
        _avatar.name = Utils.MODELS_DIRECTORY[_gender] + _go_models[_avatarIndex].name;

        _avatar.transform.parent = posAvatar.transform;
        applyOffsetY();
        _avatar.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        initAvatar();
    }

    /// <summary>
    /// Initialize the avatar
    /// </summary>
    void initAvatar()
    {
        _avatar.transform.localPosition = Vector3.zero;
        _avatar.transform.localScale = new Vector3(8, 8, 8);

        GameObject modelRoot = _avatar.transform.FindChild("python").gameObject;

        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder").transform.localRotation = new Quaternion(-0.5f, 0.3f, 0.3f, 0.8f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra").transform.localRotation = new Quaternion(-0.6f, 0.3f, 0.2f, 0.7f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm").transform.localRotation = new Quaternion(-0.3f, 0.9f, 0.3f, -0.3f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm").transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand").transform.localRotation = new Quaternion(0.1f, -0.6f, 0.0f, 0.8f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftHandThumb1").transform.localRotation = new Quaternion(-0.1f, -0.5f, -0.4f, 0.8f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandIndex").transform.localRotation = new Quaternion(-0.1f, -0.2f, -0.1f, 1.0f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandMiddle").transform.localRotation = new Quaternion(-0.1f, 0.0f, 0.0f, 1.0f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandPinky").transform.localRotation = new Quaternion(-0.1f, -0.2f, 0.1f, 1.0f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandRing").transform.localRotation = new Quaternion(-0.1f, -0.1f, 0.1f, 1.0f);

        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder").transform.localRotation = new Quaternion(-0.5f, -0.3f, -0.3f, 0.8f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra").transform.localRotation = new Quaternion(-0.6f, -0.3f, -0.2f, 0.7f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm").transform.localRotation = new Quaternion(0.3f, 0.9f, 0.3f, 0.3f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm").transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand").transform.localRotation = new Quaternion(0.1f, 0.6f, 0.0f, 0.8f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightHandThumb1").transform.localRotation = new Quaternion(-0.1f, 0.5f, 0.4f, 0.8f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandIndex").transform.localRotation = new Quaternion(-0.1f, 0.2f, 0.1f, 1.0f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandMiddle").transform.localRotation = new Quaternion(-0.1f, 0.0f, 0.0f, 1.0f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandPinky").transform.localRotation = new Quaternion(-0.1f, 0.2f, -0.1f, 1.0f);
        modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandRing").transform.localRotation = new Quaternion(-0.1f, 0.1f, 0.0f, 1.0f);
    }

    void Update()
    {
        _avatar.transform.Rotate(0.0f, 1.0f, 0.0f);
        if (Input.GetMouseButtonDown(0))
        {   // If right button of the mouse is pressed
            _avatarIndex--;
            if (_avatarIndex < 0)
                _avatarIndex = _go_models.Length - 1;
            ReloadAvatar();
        }
        else if (Input.GetMouseButtonDown(1))
        {   // If left button of the mouse is pressed
            _avatarIndex++;
            if (_avatarIndex >= _go_models.Length)
                _avatarIndex = 0;
            ReloadAvatar();
        }
        if (!string.Empty.Equals(PlayerPrefs.GetString(Utils.PREFS_VALIDATE_AVATAR)))
        {   // If the user validates the current avatar in the Web interface
            int difference = int.Parse(PlayerPrefs.GetString(Utils.PREFS_VALIDATE_AVATAR));
            PlayerPrefs.DeleteKey(Utils.PREFS_VALIDATE_AVATAR);
            Validate(difference);
        }
    }

    /// <summary>
    /// Delete the current avatar in the scene and instantiate a new avatar
    /// </summary>
    void ReloadAvatar()
    {
        Quaternion srcRotation = _avatar.transform.localRotation;
        Destroy(_avatar);
        _avatar = (GameObject)Instantiate(_go_models[_avatarIndex]);
        _avatar.name = Utils.MODELS_DIRECTORY[_gender] + _go_models[_avatarIndex].name;
        _avatar.transform.parent = posAvatar.transform;
        applyOffsetY();
        _avatar.transform.localRotation = srcRotation;
        initAvatar();
        Debug.Log("avatarIndex");
        Debug.Log(_avatarIndex);
    }

    /// <summary>
    /// Validate the selected avatar
    /// </summary>
    /// <param name="difference">The difference between the chosen avatar and the one chosen by the experimentator</param>
    void Validate(int difference)
    {
        GameObject src = _go_models[_avatarIndex];
        GameObject dst = SelectOtherAvatar(src, difference);
        Debug.Log("VALIDATE");
        if (src != null && dst != null)
        {
            //CreateNewDirectory();
            string models = Utils.MODELS_DIRECTORY[_gender] + src.name + ";" + Utils.MODELS_DIRECTORY[_gender] + dst.name;
            PlayerPrefs.SetString(Utils.PREFS_MODEL, models);
            Debug.Log(models);
            PlayerPrefs.SetInt(Utils.PREFS_CONDITION, 1);

            //Creation du fichier de log pour les données de l'avatar sélectionné
            FileLog fl = new FileLog();
            fl.createAvatarLogFile(difference, _go_models, _avatarIndex);

            Utils.CurrentState = State.WAITING;
            //Application.LoadLevel(Utils.WAITING_SCENE);
        }
    }

   /* /// <summary>
    /// Create the directory which will contains the user's files
    /// </summary>
    void CreateNewDirectory()
    {
        if (!Directory.Exists(FilesConst.SAVE_FILES_DIRECTORY))
        {   // Si le répertoire contenant les résultats n'existent pas
            Directory.CreateDirectory(FilesConst.SAVE_FILES_DIRECTORY); // On le crée
        }
        int dirIndex = 0;
        foreach (string directory in Directory.GetDirectories(FilesConst.SAVE_FILES_DIRECTORY))
        {
            string dir = directory.Remove(0, FilesConst.SAVE_FILES_DIRECTORY.Length + 1);
            if (dir.Contains(FilesConst.USER_PREFIX_DIRECTORY) && int.Parse(dir.Remove(0, FilesConst.USER_PREFIX_DIRECTORY.Length).Split('_')[0]) > dirIndex)
                dirIndex = int.Parse(dir.Remove(0, FilesConst.USER_PREFIX_DIRECTORY.Length).Split('_')[0]);
        }
        string time = System.DateTime.Now.ToString().Replace("/", "-").Replace(":", "-");
        PlayerPrefs.SetString(Utils.PREFS_PATH_FOLDER, Directory.CreateDirectory(FilesConst.SAVE_FILES_DIRECTORY + "/" + FilesConst.USER_PREFIX_DIRECTORY + (dirIndex + 1).ToString() + "_" + time).FullName);
    }*/

    /// <summary>
    /// Selects the experimentator's avatar, taking care with the difference between the two models
    /// </summary>
    /// <returns>The experimentator's avatar</returns>
    /// <param name="avatar">The user's avatar</param>
    /// <param name="difference">The difference between the two models</param>
    GameObject SelectOtherAvatar(GameObject avatar, int difference)
    {
        string[] weight = { "HC", "LHC", "MC", "HLC", "LC" };
        string[] muscle = { "HM", "LHM", "MM", "HLM", "LM" };

        string suffixeAvatar = "";

        for (int i = 0; i < weight.Length; i++)
        {
            for (int j = 0; j < muscle.Length; j++)
            {
                if (avatar.name.Substring(avatar.name.LastIndexOf("ale") + 3).Equals(weight[i] + muscle[j]))
                {
                    if (i + difference > weight.Length - 1)
                        suffixeAvatar = weight[weight.Length - 1];
                    else
                        suffixeAvatar = weight[i + difference];
                    if (j + difference > muscle.Length - 1)
                        suffixeAvatar += muscle[muscle.Length - 1];
                    else
                        suffixeAvatar += muscle[j + difference];
                }
            }
        }
        foreach (GameObject character in _go_models)
        {
            if (character.name.Substring(character.name.LastIndexOf("ale") + 3).Equals(suffixeAvatar))
                return character;
        }
        return null;
    }

    void applyOffsetY()
    {
        Vector3 vector = Vector3.zero;
        vector.y -= _offsetYPosAvatar;
        _avatar.transform.localPosition = vector;
    }

    public GameObject posAvatar
    {
        set
        {
            _posAvatar = value;
        }
        get
        {
            return _posAvatar;
        }
    }
}
