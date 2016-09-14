/// <summary>
/// Class containing all the values used by the scripts
/// </summary>
public static class Utils {
	public readonly static string PREFS_LAUNCH_MODEL = "launchModel";
	public readonly static string PREFS_AVATAR_GENDER = "avatarGender";

	public readonly static string PREFS_GHOST = "ghost";

	public readonly static string PREFS_VALIDATE_AVATAR = SOCKET_VALIDATE;
	public readonly static string PREFS_PATH_FOLDER = "path_folder";

	public readonly static string PREFS_CONDITION = "Condition";
	public readonly static string PREFS_SESSION = "Session";

	public readonly static string PREFS_MODEL = "Model";
	public readonly static string PREFS_DOORS = "doors";
	public readonly static string PREFS_PARAM_DOORS = "param_doors";
	public readonly static string[] MODELS_DIRECTORY = {"Models/Homme/", "Models/Femme/"};

	public readonly static string FULL_DOORS = "fullDoors";
	public readonly static string BOTTOM_DOORS = "bottomDoors";
	public readonly static string TOP_DOORS = "topDoors";

	public readonly static string NO_PARAMETER = "noParameters";
	public readonly static string MORPHING_PARAMETER = "morphingParameter";
	public readonly static string STICK_PARAMETER = "stickParameter";
	public readonly static string ALL_PARAMETERS = "allParameters";

	public readonly static string DOORS_SCENE = "Doors";
	public readonly static string WAITING_SCENE = "Waiting Scene";
	public readonly static string OUTOFBODY_SCENE = "Out Of Body";

	public readonly static string WIDTH_KEY = "width";
	public readonly static string HEIGHT_KEY = "height";

	public readonly static string SERVER_IP = @"127.0.0.1";
	public readonly static int SERVER_PORT = 3000;
	public readonly static int SOCKET_PORT = 8000;

	public readonly static string SOCKET_AVATAR = "avatar";
	public readonly static string SOCKET_NOTHING = "nothing";
	public readonly static string SOCKET_BATON = "baton";
	public readonly static string SOCKET_MORPHING = "morphing";
	public readonly static string SOCKET_GHOST = "ghost";
	public readonly static string SOCKET_BATON_MORPHING = "baton_morphing";
	public readonly static string SOCKET_STOP = "stop";
	public readonly static string SOCKET_EXIT = "exit";
	public readonly static string SOCKET_OUT_OF_BODY = "oob";
	
	public readonly static string SOCKET_PORTE_ENTIERE = "e/";
	public readonly static string SOCKET_PORTE_DEMIHAUT = "dh/";
	public readonly static string SOCKET_PORTE_DEMIBAS = "db/";

	public readonly static string[] SOCKET_GENDER = {"M", "F"};
	public readonly static string SOCKET_VALIDATE = "validerAvatar";

	public readonly static string SOCKET_END_DOOR = "door_finish";
}