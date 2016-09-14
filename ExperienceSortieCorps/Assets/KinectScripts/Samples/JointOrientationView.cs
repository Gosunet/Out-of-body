using UnityEngine;
using System.Collections;
//using Windows.Kinect;


public class JointOrientationView : MonoBehaviour 
{
	public KinectInterop.JointType trackedJoint = KinectInterop.JointType.WristRight;
	public bool mirroredView = false;
	public float smoothFactor = 5f;
	public GUIText debugText;
	
	private Quaternion initialRotation = Quaternion.identity;

	
	void Start()
	{
		initialRotation = transform.rotation;
		transform.rotation = Quaternion.identity;
	}
	
	void Update () 
	{
		KinectManager manager = KinectManager.Instance;
		
		if(manager && manager.IsInitialized())
		{
			int iJointIndex = (int)trackedJoint;

			if(manager.IsUserDetected())
			{
				long userId = manager.GetPrimaryUserID();
				
				if(manager.IsJointTracked(userId, iJointIndex))
				{
					Quaternion qRotObject = manager.GetJointOrientation(userId, iJointIndex, !mirroredView);
					Vector3 vRotAngles = qRotObject.eulerAngles + initialRotation.eulerAngles;
					qRotObject = Quaternion.Euler(vRotAngles);
					
					if(debugText)
					{
						debugText.GetComponent<GUIText>().text = string.Format("{0} - W({1:000}, {2:000}, {3:000})", trackedJoint, 
						                                       vRotAngles.x, vRotAngles.y, vRotAngles.z);
					}
					
					transform.rotation = Quaternion.Slerp(transform.rotation, qRotObject, smoothFactor * Time.deltaTime);
				}
				
			}
			
		}
	}
}
