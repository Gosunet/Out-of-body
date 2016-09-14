using UnityEngine;
using System.Collections;

/// <summary>
/// Script used to treat the messages coming from the socket to do actions in Unity's scenes
/// </summary>
public class ReceiveSocket : MonoBehaviour
{
	/// <summary>
	/// The instance of SocketClient
	/// </summary>
	private SocketClient _socketClient;
	
	void Start ()
	{
		_socketClient = SocketClient.GetInstance();
	}
	
	void Update() {

		// If Q Button pressed, finish the application
		if(Input.GetKeyDown (KeyCode.Q))
			Application.Quit();

		// If message received on the socket
		if (_socketClient.message != null) {

			// Get the message
			string message = _socketClient.message;

			// Finish the application
			if (message.Equals(Utils.SOCKET_EXIT)) {
				Application.Quit();
			}

			// If the user selects the avatar from the Web interface
			else if(message.Contains(Utils.SOCKET_VALIDATE)){
				PlayerPrefs.SetString(Utils.PREFS_VALIDATE_AVATAR, message.Split('/')[1]);
			}

			// If the user selects the gender of the avatar
			else if (message.Contains (Utils.SOCKET_AVATAR)) {
				for(int i=0; i<Utils.SOCKET_GENDER.Length; i++){
					if(message.Substring(0, 1).Equals(Utils.SOCKET_GENDER[i])) {
						PlayerPrefs.SetInt (Utils.PREFS_AVATAR_GENDER, i);
					}
				}
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 0);
				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}

			// If the user stops the running exercice
			else if (message.Equals (Utils.SOCKET_STOP)) {
				Application.LoadLevel (Utils.WAITING_SCENE);
			}

			// If the user selects a full door
			else if (message.Contains (Utils.SOCKET_PORTE_ENTIERE)) {
				PlayerPrefs.SetString (Utils.PREFS_PARAM_DOORS, message.Split('/')[1]);
				PlayerPrefs.SetString (Utils.PREFS_DOORS, Utils.FULL_DOORS);
				Application.LoadLevel (Utils.DOORS_SCENE);
			}

			// If the user selects a bottom door
			else if (message.Contains (Utils.SOCKET_PORTE_DEMIBAS)) {
				PlayerPrefs.SetString (Utils.PREFS_PARAM_DOORS, message.Split('/')[1]);
				PlayerPrefs.SetString (Utils.PREFS_DOORS, Utils.BOTTOM_DOORS);
				Application.LoadLevel (Utils.DOORS_SCENE);
			}

			// If the user selects a top door
			else if (message.Contains (Utils.SOCKET_PORTE_DEMIHAUT)) {
				PlayerPrefs.SetString (Utils.PREFS_PARAM_DOORS, message.Split('/')[1]);
				PlayerPrefs.SetString (Utils.PREFS_DOORS, Utils.TOP_DOORS);
				Application.LoadLevel (Utils.DOORS_SCENE);
			}

			// If the user runs the scene "Out of Body"
			else if(message.Contains(Utils.SOCKET_OUT_OF_BODY)){
				string[] parameters = message.Remove (0, Utils.SOCKET_OUT_OF_BODY.Length + 1).Split('_');
				int baton = int.Parse(parameters[0]);
				int morphing = int.Parse(parameters[1]);
				int ghost = int.Parse(parameters[2]);

				PlayerPrefs.SetInt(Utils.PREFS_GHOST, ghost);
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 1);

				if(morphing == 0 && baton == 0) 
					PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 1);
				else if(morphing == 1 && baton == 0) 
					PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 2);
				else if(morphing == 0 && baton == 1) 
					PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 3);
				else if(morphing == 1 && baton == 1) 
					PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 4);

				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}
			_socketClient.message = null;
		}
	}

	/// <summary>
	/// Method called before the application finishes
	/// </summary>
	void OnApplicationQuit(){
		_socketClient.stopThread = true;	// Stop the reading socket thread  
		_socketClient.socket.Close ();		// Close the socket
		_socketClient.StopAllProcess ();
		PlayerPrefs.DeleteKey (Utils.PREFS_MODEL);
		PlayerPrefs.DeleteKey (Utils.PREFS_PATH_FOLDER);
	}
}