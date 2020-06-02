using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PresentationScript : MonoBehaviour 
{
  
	public float smoothFactor = 5f;
	//public bool autoChangeAlfterDelay = false;
	//public float slideChangeAfterDelay = 10;
	public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
	//public List<Texture> slideTextures;
	public GUIText debugText;
	//public List<GameObject> horizontalSides;

	private float distanceToCamera = 10f;
	public GUITexture backgroundImage;

	private GestureListener gestureListener;
	public GameObject OverlayObject;

	private bool basket = false;


	void Start() 
	{
		OverlayObject.active = false; //basket is invisible

		// get the gestures listener
		gestureListener = Camera.main.GetComponent<GestureListener>();
	}
	
	void Update() 
	{

		KinectManager manager = KinectManager.Instance;
		// dont run Update() if there is no user
		KinectManager kinectManager = KinectManager.Instance;

		//(copied from example: Overlay)
		if (backgroundImage && (backgroundImage.texture == null))
		{
			backgroundImage.texture = manager.GetUsersClrTex();
		}



		if ( gestureListener) 
		{
			if ( gestureListener.IsSquat()) //if user squats basket will stick to hand/release from hand
			{
				print(basket);
                if (!basket)
                {
					OverlayObject.active = true;
				}
                else
                {
					OverlayObject.active = false;
				}
				
				basket = !basket;
			}
			
		}


		//tracks right Hand (copied from example: Overlay)
		int iJointIndex = (int)TrackedJoint;

		uint user1Id = manager.GetPlayer1ID();


		if (manager.IsJointTracked(user1Id, iJointIndex))
		{
			Vector3 posJoint = manager.GetRawSkeletonJointPos(user1Id, iJointIndex);

			if (posJoint != Vector3.zero)
			{
				// 3d position to depth
				Vector2 posDepth = manager.GetDepthMapPosForJointPos(posJoint);

				// depth pos to color pos
				Vector2 posColor = manager.GetColorMapPosForDepthPos(posDepth);

				float scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
				float scaleY = 1.0f - (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;


				if (debugText)
				{
					debugText.GetComponent<GUIText>().text = "Tracked user ID: " + user1Id;  // new Vector2(scaleX, scaleY).ToString();
				}

				if (OverlayObject)
				{
					Vector3 vPosOverlay = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
					OverlayObject.transform.position = Vector3.Lerp(OverlayObject.transform.position, vPosOverlay, smoothFactor * Time.deltaTime);
				}
			}
		}
		
	}




}
