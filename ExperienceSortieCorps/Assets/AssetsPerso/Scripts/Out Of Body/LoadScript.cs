using UnityEngine;
using System.Collections;

/// <summary>
/// Script used to enable the morphing, the stick, or nothing. Depending with the message received on the socket
/// </summary>
public class LoadScript : MonoBehaviour {

	/// <summary>
	/// The stick (Razor Hydra)
	/// </summary>
	[SerializeField]
	private GameObject _baton;

	/// <summary>
	/// The _camera of the scene
	/// </summary>
	[SerializeField]
	private Camera _camera;

	/// <summary>
	/// The gameObject in which the avatar will be instantiated
	/// </summary>
	[SerializeField]
	private GameObject _posAvatar;
	
	[SerializeField]
	private Material _jeanGhost;

	[SerializeField]
	private Material _shirtGhost;

	/// <summary>
	/// The gameObject containing the script who will instantiate the avatar
	/// </summary>
	[SerializeField]
	private GameObject _initModel;

	void Start () {
		_camera.gameObject.SetActive (false);
		if (PlayerPrefs.GetInt (Utils.PREFS_LAUNCH_MODEL) == 0) {
			gameObject.AddComponent<SelectModel> ().posAvatar = _posAvatar;
		} else {
			_initModel.SetActive (true);
			switch (PlayerPrefs.GetInt (Utils.PREFS_CONDITION)) {
			case 1:
				break;
			case 2:
				EnableMorphing();
				break;
			case 3:
				EnableBaton();
				break;
			case 4:
				EnableMorphing();
				EnableBaton();
				break;
			}
		}
		_camera.gameObject.SetActive (true);
	}

	/// <summary>
	/// Add the stick to the scene and enables the Razor Hydra
	/// </summary>
	void EnableBaton(){
		_baton.SetActive (true);
	}

	/// <summary>
	/// Enables the morphing on the avatar
	/// </summary>
	void EnableMorphing(){
		InitMorphing initMorphing = gameObject.AddComponent<InitMorphing> ();
		initMorphing.jeanGhost = _jeanGhost;
		initMorphing.shirtGhost = _shirtGhost;
	}
	
	public GameObject baton {
		set {
			_baton = value;
		}
	}
	
	public Camera camera {
		set {
			_camera = value;
		}
	}

	public GameObject posAvatar {
		set {
			_posAvatar = value;
		}
	}
	
	public Material jeanGhost {
		set {
			_jeanGhost = value;
		}
	}
	
	public Material shirtGhost {
		set {
			_shirtGhost = value;
		}
	}
	
	public GameObject initModel {
		set {
			_initModel = value;
		}
	}
}