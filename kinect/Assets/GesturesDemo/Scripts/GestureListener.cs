using UnityEngine;
using System.Collections;
using System;

public class GestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
	// GUI Text to display the gesture messages.
	public GUIText GestureInfo;
	
	private bool swipeLeft; //raiseLeftHand
	private bool swipeRight; //raiseRightHand
	
	

	
	public bool IsSwipeRight() //IsRaiseRightHand
	{
		
		if (swipeRight) //raiseRightHand
		{
			print("IsSwipeRight");
			swipeRight = false; //raiseRightHand
            return true;
		}
		
		return false;
	}

	public void UserDetected(uint userId, int userIndex)
	{
		print("UserDetected");
		// detect these user specific gestures
		KinectManager manager = KinectManager.Instance;
		
		manager.DetectGesture(userId, KinectGestures.Gestures.Squat);

		if(GestureInfo != null)
		{
			GestureInfo.GetComponent<GUIText>().text = "Raise left or right hand to change the slides.";
		}
	}
	
	public void UserLost(uint userId, int userIndex)
	{
		print("UserLost");
		if (GestureInfo != null)
		{
			GestureInfo.GetComponent<GUIText>().text = string.Empty;
		}
	}

	public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		print("GestureInProgress");
		// don't do anything here
	}

	public bool GestureCompleted (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		print("GestureCompleted");
		string sGestureText = gesture + " detected";
		if(GestureInfo != null)
		{
			GestureInfo.GetComponent<GUIText>().text = sGestureText;
		}
		
		if(gesture == KinectGestures.Gestures.Squat)
			swipeRight = true;
			
	

		return true;
	}

	public bool GestureCancelled (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint)
	{
		print("GestureCancelled");
		// don't do anything here, just reset the gesture state
		return true;
	}
	
}
