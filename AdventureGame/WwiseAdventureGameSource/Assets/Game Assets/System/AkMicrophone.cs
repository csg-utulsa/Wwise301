#if !UNITY_WEBGL
using UnityEngine.Events;
[UnityEngine.RequireComponent(typeof(UnityEngine.AudioSource))]
public class AkMicrophone : UnityEngine.MonoBehaviour
{
    public AK.Wwise.Event MicrophoneEvent;
    public AK.Wwise.RTPC MicrophoneLevelRTPC;
    public int SampleRate = 48000;
    public int InitialReadDelayInSamples = 4096;
    [UnityEngine.Header("Invoked on exceeding -48dB")]
    public UnityEvent OnMicrophoneAction;

    public static AkMicrophone Instance
	{
		get;
		private set;
	}

	public bool IsAboveThreshold
	{
		get
		{
			bool Above = currentMicLevel > ATTACK_LEVEL_THRESHOLD;
			currentMicLevel = -48;
			return Above;
		}
	}

    // Buffering consts
    private const int NUMBER_OF_CHANNELS = 1;
	private const int BUFFER_SIZE_IN_SECONDS = 2;
	private int BufferSizeInSamples { get { return BUFFER_SIZE_IN_SECONDS * NUMBER_OF_CHANNELS * SampleRate; } }

	// Level monitoring
	private float ATTACK_LEVEL_THRESHOLD = -24.0f;
	private float currentMicLevel = -48;
	
	// Unity Microphone input handling
    private UnityEngine.AudioSource MicrophoneSource;
    private int ReadPosition = 0;
	private bool IsPlaying { get { return m_PlayingID != AkSoundEngine.AK_INVALID_PLAYING_ID; } }
	private float[] SamplesBuffer = null;
	private object BufferLock = new object();

	private uint m_PlayingID = AkSoundEngine.AK_INVALID_PLAYING_ID;

    void AudioFormatDelegate(uint playingID, AkAudioFormat audioFormat)
    {
        audioFormat.channelConfig.uNumChannels = NUMBER_OF_CHANNELS;
        audioFormat.uSampleRate = (uint)SampleRate;
    }

	bool AudioSamplesDelegate(uint playingID, uint channelIndex, float[] samples)
    {
		if (IsPlaying)
		{
			try
			{
				lock (BufferLock)
				{
					System.Array.Copy(SamplesBuffer, 0, samples, 0, samples.Length);
					ReadPosition = (ReadPosition + samples.Length) % BufferSizeInSamples;
				}
			}
			catch (System.Exception e)
			{
				UnityEngine.Debug.LogError("Wwise Audio Input, exception occured: " + e.ToString());
				return false;
			}
		}

		// Return false to indicate that there is no more data to provide. This will also stop the associated event.
		return IsPlaying;
    }

	void GetMicrophoneSamples()
	{
		if (MicrophoneSource.clip != null)
		{
			lock (BufferLock)
			{
				MicrophoneSource.clip.GetData(SamplesBuffer, ReadPosition);
			}
		}
	}

	void FixedUpdate()
    {
		GetMicrophoneSamples();

        if (IsPlaying)
        {
            int RTPCValue = (int)AkQueryRTPCValue.RTPCValue_GameObject;
            AkSoundEngine.GetRTPCValue((uint)MicrophoneLevelRTPC.Id, gameObject, m_PlayingID, out currentMicLevel, ref RTPCValue);
            if (IsAboveThreshold)
                OnMicrophoneAction.Invoke();
        }
	}

	void Start()
	{
		if (Instance != null)
		{
			UnityEngine.Debug.LogError("Wwise Microphone input: Attempted to add a second AkMicrophone component.");
			return;
		}
		Instance = this;

		SamplesBuffer = new float[BufferSizeInSamples];
		StartSound();
	}

	public void StartSound()
	{
		if (!IsPlaying)
		{
			MicrophoneSource = GetComponent<UnityEngine.AudioSource>();
			MicrophoneSource.clip = UnityEngine.Microphone.Start(null, true, BUFFER_SIZE_IN_SECONDS, SampleRate);
			GetMicrophoneSamples();

			// Since GetData gives a LOT of data, move our ReadPosition closer to the current samples upon first read
			ReadPosition = UnityEngine.Microphone.GetPosition(null) - InitialReadDelayInSamples;
			ReadPosition = (ReadPosition + BufferSizeInSamples) % BufferSizeInSamples;

			m_PlayingID = AkAudioInputManager.PostAudioInputEvent(MicrophoneEvent.Id, gameObject, AudioSamplesDelegate, AudioFormatDelegate);
		}
	}
	public void StopSound()
    {
		m_PlayingID = AkSoundEngine.AK_INVALID_PLAYING_ID;
		MicrophoneEvent.Stop(gameObject);
	}

    private void OnDestroy()
    {
		StopSound();
    }
}
#endif
