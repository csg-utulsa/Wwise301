	[UnityEditor.InitializeOnLoad]
public class AkWAGBuildPreprocessor
{
	static AkWAGBuildPreprocessor()
	{
		AkBuildPreprocessor.GetCustomPlatformName = GetCustomPlatformName;
	}

	static void GetCustomPlatformName(ref string platformName, UnityEditor.BuildTarget target)
	{
		if(target == UnityEditor.BuildTarget.Android)
		{
			platformName = "Android_High";
		}
	}
}
