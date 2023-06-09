using EFT;
using UnityEngine;

public class AudioTester : MonoBehaviour
{
	private Coroutine _E000;

	public SoundBank SoundBank;

	public TagBank Phrases;

	public EOcclusionTest EOcclusionTest = EOcclusionTest.Regular;

	public bool JustTest;

	public bool Talking;

	private float _E001;

	public float Freq = 3f;

	public float Times = 1f;

	public void Update()
	{
		if (!Talking || Time.time < _E001 + Freq)
		{
			return;
		}
		if (!JustTest)
		{
			if (Phrases != null)
			{
				TaggedClip taggedClip = Phrases.Clips[Random.Range(0, Phrases.Clips.Length)];
				for (int i = 0; (float)i < Times; i++)
				{
					MonoBehaviourSingleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, taggedClip.Clip, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Speech, taggedClip.Falloff, taggedClip.Volume, EOcclusionTest);
				}
			}
			else if (SoundBank != null)
			{
				for (int j = 0; (float)j < Times; j++)
				{
					MonoBehaviourSingleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, SoundBank, _E8A8.Instance.Distance(base.transform.position), 1f, -1f, EnvironmentType.Outdoor, EOcclusionTest);
				}
			}
		}
		_E001 = Time.time;
	}
}
