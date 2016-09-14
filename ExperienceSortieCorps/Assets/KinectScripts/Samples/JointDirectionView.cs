using UnityEngine;
using System.Collections;
//using Windows.Kinect;


public class JointDirectionView : MonoBehaviour 
{
	public KinectInterop.JointType trackedJoint1 = KinectInterop.JointType.WristRight;
	public KinectInterop.JointType trackedJoint2 = KinectInterop.JointType.HandRight;
	public bool mirroredView = false;
	public bool normalizedView = false;
	//public float smoothFactor = 5f;

	public LineRenderer lineDir;
	public LineRenderer lineBase;
	public LineRenderer lineCross;
	
	public GUIText debugText;



	void Start()
	{
//		lineRenderer = this.gameObject.GetComponent<LineRenderer>();
//
//		if(lineRenderer)
//		{
//			lineRenderer.SetVertexCount(2);
//
//			lineBase = Instantiate(lineRenderer) as LineRenderer;
//			//lineBase.transform.parent = transform;
//			
//			lineCross = Instantiate(lineRenderer) as LineRenderer;
//			//lineCross.transform.parent = transform;
//		}
	}
	
	void Update () 
	{
		KinectManager manager = KinectManager.Instance;
		
		if(lineDir && manager && manager.IsInitialized())
		{
			if(manager.IsUserDetected())
			{
				long userId = manager.GetPrimaryUserID();
				
				if(manager.IsJointTracked(userId, (int)trackedJoint1) && 
				   manager.IsJointTracked(userId, (int)trackedJoint2))
				{
					Vector3 posJoint1 = manager.GetJointPosition(userId, (int)trackedJoint1);
					posJoint1.z = !mirroredView ? -posJoint1.z : posJoint1.z;

					Vector3 posJoint2 = manager.GetJointPosition(userId, (int)trackedJoint2);
					posJoint2.z = !mirroredView ? -posJoint2.z : posJoint2.z;

					Vector3 dirJoints = posJoint2 - posJoint1;
					Vector3 baseDir = KinectInterop.JointBaseDir[(int)trackedJoint1];

					Vector3 crossDir = Vector3.Cross(baseDir, dirJoints.normalized);

					if(debugText)
					{
						debugText.GetComponent<GUIText>().text = string.Format("Dir: {0}; Mag2: {1:F3}, Dot={2:F3}", 
						                                       dirJoints, dirJoints.sqrMagnitude, 
						                                       Vector3.Dot(baseDir, dirJoints.normalized));
					}

					if(normalizedView)
					{
						posJoint2 = posJoint1 + dirJoints.normalized;
					}

					Vector3 posLine1 = transform.position;
					Vector3 posLine2 = transform.position + (posJoint2 - posJoint1);

					lineDir.SetPosition(0, posLine1);
					lineDir.SetPosition(1, posLine2);
					
					posLine1 = transform.position;
					posLine2 = transform.position + baseDir;
					
					lineBase.SetPosition(0, posLine1);
					lineBase.SetPosition(1, posLine2);

					posLine1 = transform.position;
					posLine2 = transform.position + crossDir;
					
					lineCross.SetPosition(0, posLine1);
					lineCross.SetPosition(1, posLine2);
				}
				
			}
			
		}
	}
}
