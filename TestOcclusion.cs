using System.Collections;
using Audio.SpatialSystem;
using Comfort.Common;
using EFT;
using UnityEngine;

public class TestOcclusion : MonoBehaviour
{
	public SoundBank TestSoundBank;

	public TagBank VoiceSoundBank;

	public BetterSource EnvironmentSource;

	public BetterAudio.AudioSourceGroupType SourceType = BetterAudio.AudioSourceGroupType.Character;

	private float m__E000 = 1f;

	private AudioClip m__E001;

	private void Start()
	{
		EnvironmentSource = Singleton<BetterAudio>.Instance.GetSource(BetterAudio.AudioSourceGroupType.Speech);
		if ((object)EnvironmentSource == null)
		{
			Debug.LogError(_ED3E._E000(35613));
		}
		else
		{
			StartCoroutine(_E001());
		}
	}

	private IEnumerator _E000()
	{
		while (!Singleton<BetterAudio>.Instantiated)
		{
			yield return null;
		}
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
			Singleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, TestSoundBank, TestSoundBank.Rolloff, 1f, -1f, EnvironmentType.Outdoor, EOcclusionTest.Regular);
		}
	}

	private IEnumerator _E001()
	{
		while (!MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated || !MonoBehaviourSingleton<BetterAudio>.Instantiated)
		{
			yield return null;
		}
		if ((object)EnvironmentSource == null)
		{
			EnvironmentSource = Singleton<BetterAudio>.Instance.GetSource(BetterAudio.AudioSourceGroupType.Speech);
		}
		if (MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
		{
			MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ProcessSourceOcclusion(EnvironmentSource, EOcclusionTest.Continuous);
		}
		while (true)
		{
			yield return new WaitForSeconds((this.m__E001 == null) ? 1.5f : this.m__E001.length);
			int num = Random.Range(0, VoiceSoundBank.Clips.Length);
			TaggedClip taggedClip = VoiceSoundBank.Clips[num];
			if (taggedClip == null)
			{
				yield return null;
			}
			_E002(taggedClip);
		}
	}

	private void _E002(TaggedClip clip, bool looped = false)
	{
		float volume = clip.Volume;
		this.m__E000 = 1f;
		if (!EnvironmentSource.source1.isPlaying)
		{
			EnvironmentSource.transform.parent = base.gameObject.transform;
			EnvironmentSource.transform.position = base.gameObject.transform.position;
			this.m__E001 = clip.Clip;
			EnvironmentSource.gameObject.SetActive(value: true);
			EnvironmentSource.SetRolloff(70f);
			EnvironmentSource.source1.clip = clip.Clip;
			EnvironmentSource.source1.loop = looped;
			_ = EnvironmentSource.source1.transform.position;
			float volume2 = EnvironmentSource.source1.volume * volume;
			EnvironmentSource.source1.outputAudioMixerGroup = Singleton<BetterAudio>.Instance.VeryStandartMixerGroup;
			EnvironmentSource.Play(clip.Clip, null, 1f, volume2);
		}
	}
}
