using UnityEngine;
using System.Collections;
using System.IO;

public class CreatePicture : MonoBehaviour
{

    private GameObject _avatar;

    private GameObject[] _go_modelsMale;
    private GameObject[] _go_modelsFemale;

    private GameObject[] _currentGoModels;

    private int _avatarIndex;

    private string _avatarName;

    private bool _isFront;
    private bool _stop = false;

    private bool _init = false;

    void Start()
    {
        _go_modelsMale = Resources.LoadAll<GameObject>("Models/Homme/");
        _go_modelsFemale = Resources.LoadAll<GameObject>("Models/Femme/");
        _currentGoModels = _go_modelsMale;

        if (!Directory.Exists("AvatarImg"))
            Directory.CreateDirectory("AvatarImg");
        if (!Directory.Exists("AvatarImg/Hommes"))
            Directory.CreateDirectory("AvatarImg/Hommes");
        if (!Directory.Exists("AvatarImg/Femmes"))
            Directory.CreateDirectory("AvatarImg/Femmes");
        _isFront = false;
        _avatarIndex = 0;
    }

    void Update()
    {
        if (!_init)
            _init = true;
        else
        {
            if (!_isFront)
            {
                if (_avatarIndex > _currentGoModels.Length - 1)
                {
                    if (_currentGoModels.Equals(_go_modelsMale))
                    {
                        _avatarIndex = 0;
                        _currentGoModels = _go_modelsFemale;
                    }
                    else
                    {
                        Application.Quit();
                        return;
                    }
                }
                if (_avatar != null)
                    Destroy(_avatar);
                _avatar = (GameObject)Instantiate(_currentGoModels[_avatarIndex]);
                _avatarName = _currentGoModels[_avatarIndex].name;
                _avatar.transform.localRotation = new Quaternion(0.0f, -0.7f, 0.0f, 0.7f);
                _avatar.transform.parent = transform;
                initAvatar();
                _isFront = true;

            }
            else
            {
                _avatar.transform.rotation = new Quaternion(0.0f, 0.7f, 0.0f, 0.7f);
                _isFront = false;
                _avatarIndex++;
            }
        }
    }

    void LateUpdate()
    {
        if (!string.IsNullOrEmpty(_avatarName))
        {
            string filename;
            if (_currentGoModels.Equals(_go_modelsMale))
                filename = "../AvatarImg/Hommes/" + _avatarName;
            else
                filename = "../AvatarImg/Femmes/" + _avatarName;
            if (_isFront)
                filename += "_back";
            else
                filename += "_front";
            Application.CaptureScreenshot(filename + ".png");
        }
    }

    void initAvatar()
    {
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
}
