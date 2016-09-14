using UnityEngine;
using System.Collections;
//using Windows.Kinect;


public class LocalOrientationView : MonoBehaviour 
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
					Quaternion qRotJoint = manager.GetJointOrientation(userId, iJointIndex, !mirroredView);

					int iPrevJoint = (int)manager.GetParentJoint(trackedJoint);
					if (manager.IsJointTracked(userId, iPrevJoint)) 
					{
						Quaternion qRotParent = manager.GetJointOrientation(userId, iPrevJoint, !mirroredView);
						Quaternion qRotLocal = Quaternion.Inverse(qRotParent) * qRotJoint;

						Vector3 vRotAngles = qRotLocal.eulerAngles + initialRotation.eulerAngles;
						qRotLocal = Quaternion.Euler(vRotAngles);

						if(debugText)
						{
							debugText.GetComponent<GUIText>().text = string.Format("{0} - L({1:000}, {2:000}, {3:000})", trackedJoint, 
							                                       vRotAngles.x, vRotAngles.y, vRotAngles.z);
						}
						
						transform.rotation = Quaternion.Slerp(transform.rotation, qRotLocal, smoothFactor * Time.deltaTime);
					}

				}
				
			}
			
		}
	}
}
