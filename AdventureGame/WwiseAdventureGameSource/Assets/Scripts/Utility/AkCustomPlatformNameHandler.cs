public class CustomBasePathGetter
{
	[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void RegisterCustomPlatformName()
	{
		AkBasePathGetter.GetCustomPlatformName = GetCustomPlatformName;
	}

	public static void GetCustomPlatformName(ref string platformName)
	{
#if UNITY_ANDROID
		platformName = "Android_High";
#endif
	}
}
