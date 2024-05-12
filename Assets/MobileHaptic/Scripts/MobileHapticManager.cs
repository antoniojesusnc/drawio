using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
//#else if UNITY_ANDROID
//using UnityEngine.and
#endif

public class MobileHapticManager : MonoBehaviour {

	public enum E_FeedBackType {
		SelectionChange = 0,
		ImpactLight = 1,
		ImpactMedium = 2,
		ImpactHeavy = 3,
		Success = 4,
		Warning = 5,
		Failure = 6,
		None = 7
	};

	private static MobileHapticManager _instance;
	public static MobileHapticManager Instance{
		get{
			if (_instance == null) {
				_instance = FindObjectOfType<MobileHapticManager> ();
				if (_instance == null) {
					Resources.Load<MobileHapticManager> ("./_MobileHapticManager");
					if (_instance == null) {
						GameObject go = new GameObject ("_MobileHapticManager");
						_instance = go.AddComponent<MobileHapticManager> ();
						DontDestroyOnLoad (go);
					}
				}
			}
			return _instance;
		}
	}

    public static bool s_Vibrate = true;
		
	public void Vibrate (E_FeedBackType type){

        if (s_Vibrate == false)
            return;

		#if UNITY_EDITOR

		Debug.Log("Vibrate");

#elif UNITY_IOS

		if (Device.generation == DeviceGeneration.iPhone7 
			|| Device.generation == DeviceGeneration.iPhone7Plus 
            || Device.generation == DeviceGeneration.iPhone8
            || Device.generation == DeviceGeneration.iPhone8Plus
            || Device.generation == DeviceGeneration.iPhoneX
			|| Device.generation.ToString() == "iPhoneUnknown"){
			iOSHapticFeedback.Instance.Trigger( (iOSHapticFeedback.iOSFeedbackType)type);
		}else if(type == E_FeedBackType.ImpactHeavy || type == E_FeedBackType.Failure){
			Handheld.Vibrate();
		}

		//iOSHapticFeedback.Instance.Trigger
#elif UNITY_ANDROID
			AndroidVibration.Vibrate(androidForces[type]);
#endif


    }
}
