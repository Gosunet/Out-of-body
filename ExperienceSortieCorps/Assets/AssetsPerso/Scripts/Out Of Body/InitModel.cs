using UnityEngine;
using System.Collections;

/// <summary>
/// Instantiate the avatar in the scene
/// </summary>
public class InitModel : MonoBehaviour {
	/// <summary>
	/// The gameObject in which the avatar will be instantiated
	/// </summary>
	[SerializeField]
	private GameObject _posAvatar;

	/// <summary>
	/// The instantiated avatar
	/// </summary>
	private GameObject _goSrc;

	void Awake(){
		string[] model = PlayerPrefs.GetString (Utils.PREFS_MODEL).Split(';');
		_goSrc = (GameObject)Instantiate(Resources.Load(model[0]));
		_goSrc.transform.parent = _posAvatar.transform;
		_goSrc.name = model [0].Split ('/') [2];
		_goSrc.transform.localPosition = Vector3.zero;
		_goSrc.transform.localRotation = new Quaternion(0.0f,0.0f,0.0f,0.0f);
		initAvatar();
		initKinect();
	}

	/// <summary>
	/// Initialize the Kinect
	/// </summary>
	void initKinect(){
		GameObject modelRoot = _goSrc.transform.FindChild ("python").gameObject;

		AvatarControllerClassic ctrl = _goSrc.AddComponent <AvatarControllerClassic>();
		
		ctrl.verticalMovement = true;
		
		ctrl.HipCenter = modelRoot.transform.FindChild("Hips");
		ctrl.Spine = modelRoot.transform.FindChild ("Hips/Spine");
		ctrl.ShoulderCenter = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/Neck");
		ctrl.Neck = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/Neck/Head");
		
		ctrl.ClavicleLeft = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra");
		ctrl.ShoulderLeft = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm");
		ctrl.ElbowLeft = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm");
		ctrl.HandLeft = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand");
		
		
		ctrl.ClavicleRight = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra");
		ctrl.ShoulderRight = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm");
		ctrl.ElbowRight = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm");
		ctrl.HandRight = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand");
		
		ctrl.HipLeft = modelRoot.transform.FindChild("Hips/LeftUpLeg");
		ctrl.KneeLeft = modelRoot.transform.FindChild("Hips/LeftUpLeg/LeftLeg");
		ctrl.FootLeft = modelRoot.transform.FindChild("Hips/LeftUpLeg/LeftLeg/LeftFoot");
		
		ctrl.HipRight = modelRoot.transform.FindChild("Hips/RightUpLeg");
		ctrl.KneeRight = modelRoot.transform.FindChild("Hips/RightUpLeg/RightLeg");
		ctrl.FootRight = modelRoot.transform.FindChild("Hips/RightUpLeg/RightLeg/RightFoot");
		
		ctrl.BodyRoot = modelRoot.transform.FindChild("Hips");
		ctrl.OffsetNode = modelRoot;

		ctrl.Init ();
	}

	/// <summary>
	/// Init the avatar
	/// </summary>
	void initAvatar(){
		GameObject modelRoot = _goSrc.transform.FindChild ("python").gameObject;

		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder").transform.localRotation = new Quaternion (-0.5f, 0.3f, 0.3f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra").transform.localRotation = new Quaternion (-0.6f, 0.3f, 0.2f, 0.7f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm").transform.localRotation = new Quaternion (-0.3f, 0.9f, 0.3f, -0.3f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm").transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand").transform.localRotation = new Quaternion (0.1f, -0.6f, 0.0f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftHandThumb1").transform.localRotation = new Quaternion (-0.1f, -0.5f, -0.4f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandIndex").transform.localRotation = new Quaternion (-0.1f, -0.2f, -0.1f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandMiddle").transform.localRotation = new Quaternion (-0.1f, 0.0f, 0.0f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandPinky").transform.localRotation = new Quaternion (-0.1f, -0.2f, 0.1f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandRing").transform.localRotation = new Quaternion (-0.1f, -0.1f, 0.1f, 1.0f);

		modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder").transform.localRotation = new Quaternion (-0.5f, -0.3f, -0.3f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra").transform.localRotation = new Quaternion (-0.6f, -0.3f, -0.2f, 0.7f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm").transform.localRotation = new Quaternion (0.3f, 0.9f, 0.3f, 0.3f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm").transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand").transform.localRotation = new Quaternion (0.1f, 0.6f, 0.0f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightHandThumb1").transform.localRotation = new Quaternion (-0.1f, 0.5f, 0.4f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandIndex").transform.localRotation = new Quaternion (-0.1f, 0.2f, 0.1f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandMiddle").transform.localRotation = new Quaternion (-0.1f, 0.0f, 0.0f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandPinky").transform.localRotation = new Quaternion (-0.1f, 0.2f, -0.1f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandRing").transform.localRotation = new Quaternion (-0.1f, 0.1f, 0.0f, 1.0f);
	}

	public GameObject posAvatar {
		set {
			_posAvatar = value;
		}
		get {
			return _posAvatar;
		}
	}
}
