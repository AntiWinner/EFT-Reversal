using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace EFT;

public class SoundController : MonoBehaviour
{
	public AudioMixerGroup OccludedGroup;

	public AudioMixerGroup DirectGroup;

	public AudioMixerGroup ObstructedGroup;

	public Transform AudioListenerTransform;

	private static SoundController m__E000;

	public static SoundController Instance
	{
		get
		{
			if (SoundController.m__E000 == null)
			{
				SoundController.m__E000 = _E3AA.FindUnityObjectOfType<SoundController>();
			}
			if (SoundController.m__E000 == null)
			{
				SoundController.m__E000 = new GameObject(_ED3E._E000(139838)).AddComponent<SoundController>();
				SoundController.m__E000.SetAudioListener(_E3AA.FindUnityObjectOfType<AudioListener>());
			}
			return SoundController.m__E000;
		}
	}

	private void Start()
	{
		StartCoroutine(_E000());
	}

	private IEnumerator _E000()
	{
		while (AudioListenerTransform == null)
		{
			SetAudioListener(_E3AA.FindUnityObjectOfType<AudioListener>());
			yield return new WaitForSeconds(1f);
		}
	}

	public float Distance(Vector3 target)
	{
		return Vector3.Distance(AudioListenerTransform.position, target);
	}

	public void SetAudioListener(AudioListener listener)
	{
		if (listener != null)
		{
			AudioListenerTransform = listener.gameObject.transform;
		}
	}
}
