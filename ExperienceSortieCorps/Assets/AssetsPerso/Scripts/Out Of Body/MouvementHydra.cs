using UnityEngine;
using System.Collections;

/// <summary>
/// Script used to update position and rotation of the razer hydra during the scene "baton".
/// </summary>
public class MouvementHydra : MonoBehaviour
{

    // Instance of joystick (right or left hand)
    [SerializeField]
    private SixenseHands _hand;
    // position of the caracter on the scene.
    [SerializeField]
    private GameObject _avatarPosition;

    // Sensitivity of the razer hydra.
    private Vector3 _sensitivity = new Vector3(0.001f, 0.001f, 0.001f);
    // Quaternion used to calculate hand's rotation
    private Quaternion m_initialRotation;
    // Vector used to calculate hand's position
    private Vector3 m_initialPosition;
    // temporary position to update hand position
    private Vector3 _tmpPosition;


    void Start()
    {
        m_initialRotation = this.gameObject.transform.localRotation;

        _avatarPosition = _avatarPosition.transform.FindChild(PlayerPrefs.GetString(Utils.PREFS_MODEL).Split(';')[0].Split('/')[2]).gameObject;
        GetInitialPositionAvatar();

        _tmpPosition = m_initialPosition;


        gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    void Update()
    {
        if (_hand != SixenseHands.UNKNOWN)
        {
            SixenseInput.Controller controller = SixenseInput.GetController(_hand);
            if (controller != null && controller.Enabled)
            {
                UpdatePosition(controller);
                UpdateRotation(controller);
            }
        }
    }

    /// <summary>
    /// Gets the initial position of the avatar.
    /// </summary>
    void GetInitialPositionAvatar()
    {
        m_initialPosition = _avatarPosition.transform.localPosition;
        m_initialPosition.y += 0.3f;
        m_initialPosition.z -= 0.28f;
    }

    /// <summary>
    /// Updates the position of the razer controller on the scene.
    /// </summary>
    /// <param name="controller">razer hydra.</param>
    void UpdatePosition(SixenseInput.Controller controller)
    {
        Vector3 controllerPosition = new Vector3(controller.Position.x * _sensitivity.x, controller.Position.y * _sensitivity.y, controller.Position.z * _sensitivity.z);

        if (controller.GetButtonDown(SixenseButtons.TRIGGER))
        {
            GetInitialPositionAvatar();
            _tmpPosition = m_initialPosition - controllerPosition;
        }
        this.gameObject.transform.localPosition = _tmpPosition + controllerPosition;
    }

    /// <summary>
    /// Updates the rotation of the razer controller on the scene
    /// </summary>
    /// <param name="controller">razer hydra.</param>
    void UpdateRotation(SixenseInput.Controller controller)
    {
        Quaternion controllerRotation = new Quaternion(controller.Rotation.x, controller.Rotation.y, controller.Rotation.z, controller.Rotation.w);

        this.gameObject.transform.localRotation = m_initialRotation * controllerRotation;
    }

    public GameObject avatarPosition
    {
        get
        {
            return _avatarPosition;
        }
        set
        {
            _avatarPosition = value;
        }
    }

    public SixenseHands hand
    {
        get
        {
            return _hand;
        }
        set
        {
            _hand = value;
        }
    }
}
