using System;
using System.Linq;
using UnityEngine;

namespace EFT;

public class SoundBank : ScriptableObject
{
	public float CustomLength;

	public float BaseVolume = 1f;

	public float DeltaVolume;

	public float Rolloff = 100f;

	public bool IgnoreOcclusion;

	public bool Physical;

	public bool _noEnvironment;

	public BetterAudio.AudioSourceGroupType SourceType = BetterAudio.AudioSourceGroupType.Environment;

	[SerializeField]
	private float _clipLiength;

	public EnvironmentVariety[] Environments = new EnvironmentVariety[Enum.GetNames(typeof(EnvironmentType)).Length];

	public DistanceBlendOptions BlendOptions;

	private byte[] m__E000;

	private byte m__E001;

	public float RandomVolume
	{
		get
		{
			if (!(DeltaVolume > 0f))
			{
				return BaseVolume;
			}
			return BaseVolume + UnityEngine.Random.Range(0f - DeltaVolume, 0f);
		}
	}

	public float ClipLength
	{
		get
		{
			if (CustomLength != 0f)
			{
				return CustomLength;
			}
			return _clipLiength;
		}
		set
		{
			_clipLiength = value;
		}
	}

	public float[] BlendValues => BlendOptions.Values;

	public void Shuffle(byte ln)
	{
		if (this.m__E000 == null || this.m__E000.Length < ln)
		{
			this.m__E000 = new byte[ln];
		}
		for (byte b = 0; b < ln; b = (byte)(b + 1))
		{
			this.m__E000[b] = b;
		}
		for (byte b2 = 0; b2 < ln; b2 = (byte)(b2 + 1))
		{
			int num = UnityEngine.Random.Range(0, ln - 1);
			byte b3 = this.m__E000[num];
			this.m__E000[num] = this.m__E000[b2];
			this.m__E000[b2] = b3;
		}
		this.m__E001 = ln;
	}

	public int GetRandomClipIndex(int ln)
	{
		if (ln > 1)
		{
			if (this.m__E001 < 1)
			{
				Shuffle((byte)ln);
			}
			this.m__E001--;
			return this.m__E000[this.m__E001];
		}
		return 0;
	}

	public AudioClip PickSingleClip(int environment)
	{
		EnvironmentVariety environmentVariety = Environments[(!_noEnvironment) ? environment : 0];
		AudioClip clip = null;
		_E000(environmentVariety[0], ref clip);
		return clip;
	}

	public void PickClipsByDistance(ref AudioClip clip1, ref AudioClip clip2, ref float proportions, int environment, float distance)
	{
		EnvironmentVariety environmentVariety = Environments[(!_noEnvironment) ? environment : 0];
		if (BlendOptions == null)
		{
			_E000(environmentVariety[0], ref clip1);
			return;
		}
		int i;
		for (i = 0; i < BlendValues.Length && !(distance < BlendValues[i]); i++)
		{
		}
		switch (i)
		{
		case 0:
			_E000(environmentVariety[0], ref clip1);
			break;
		case 1:
			_E000(environmentVariety[0], ref clip1);
			_E000(environmentVariety[1], ref clip2);
			break;
		case 2:
			_E000(environmentVariety[1], ref clip1);
			break;
		case 3:
			_E000(environmentVariety[1], ref clip1);
			_E000(environmentVariety[2], ref clip2);
			break;
		case 4:
			_E000(environmentVariety[2], ref clip1);
			break;
		}
		if (i % 2 != 0)
		{
			proportions = Mathf.InverseLerp(BlendValues[i], BlendValues[i - 1], distance);
		}
		else
		{
			proportions = 1f;
		}
	}

	private void _E000(DistanceVarity clips, ref AudioClip clip)
	{
		int length = clips.Length;
		if (length != 0)
		{
			int i = GetRandomClipIndex(length) % length;
			clip = clips[i];
		}
	}

	public float PlayOn(BetterSource source, EnvironmentType pos = EnvironmentType.Outdoor, float distance = 0f, float volume = 1f, bool forceStereo = false)
	{
		AudioClip clip = null;
		AudioClip clip2 = null;
		float proportions = 1f;
		PickClipsByDistance(ref clip, ref clip2, ref proportions, (int)pos, distance);
		if (clip == null && clip2 == null)
		{
			return 0f;
		}
		float maxDistance = source.MaxDistance;
		if (distance > maxDistance)
		{
			return 0f;
		}
		source.SetRolloff(Rolloff);
		source.Play(clip, clip2, proportions, volume, forceStereo);
		return Mathf.Max((clip != null) ? clip.length : 0f, (clip2 != null) ? clip2.length : 0f);
	}

	public float PlayWithConstantRolloffDistance(BetterSource source, EnvironmentType pos = EnvironmentType.Outdoor, float distance = 0f, float volume = 1f, float blendParameter = 0f, bool forceStereo = false)
	{
		AudioClip clip = null;
		AudioClip clip2 = null;
		float proportions = 1f;
		PickClipsByDistance(ref clip, ref clip2, ref proportions, (int)pos, blendParameter);
		if (clip == null && clip2 == null)
		{
			return 0f;
		}
		float maxDistance = source.source1.maxDistance;
		if (distance > maxDistance)
		{
			return 0f;
		}
		volume = volume * Rolloff / maxDistance;
		source.Play(clip, clip2, proportions, volume, forceStereo);
		return Mathf.Max((clip != null) ? clip.length : 0f, (clip2 != null) ? clip2.length : 0f);
	}

	public float PlayWithCustomRolloffDistance(BetterSource source, EnvironmentType pos = EnvironmentType.Outdoor, float distance = 0f, float volume = 1f, float blendParameter = 0f, bool forceStereo = false, float customRolloff = 120f)
	{
		AudioClip clip = null;
		AudioClip clip2 = null;
		float proportions = 1f;
		PickClipsByDistance(ref clip, ref clip2, ref proportions, (int)pos, blendParameter);
		if (clip == null && clip2 == null)
		{
			return 0f;
		}
		float maxDistance = source.source1.maxDistance;
		if (distance > maxDistance)
		{
			return 0f;
		}
		volume = volume * customRolloff / maxDistance;
		source.Play(clip, clip2, proportions, volume, forceStereo);
		return Mathf.Max((clip != null) ? clip.length : 0f, (clip2 != null) ? clip2.length : 0f);
	}

	public float PlayOnScheduled(BetterSource source, double time, EnvironmentType pos = EnvironmentType.Outdoor, float distance = 0f, float volume = 1f)
	{
		AudioClip clip = null;
		AudioClip clip2 = null;
		float proportions = 1f;
		PickClipsByDistance(ref clip, ref clip2, ref proportions, (int)pos, distance);
		if (clip == null && clip2 == null)
		{
			return 0f;
		}
		source.SetRolloff(Rolloff);
		source.Play(clip, clip2, proportions, volume);
		source.PlayScheduled(time);
		return Mathf.Max((clip != null) ? clip.length : 0f, (clip2 != null) ? clip2.length : 0f);
	}

	public float PickClips(float distance, ref AudioClip clip1, ref AudioClip clip2, ref float proportions, EnvironmentType pos = EnvironmentType.Outdoor)
	{
		PickClipsByDistance(ref clip1, ref clip2, ref proportions, (int)pos, distance);
		return Mathf.Max((clip1 != null) ? clip1.length : 0f, (clip2 != null) ? clip2.length : 0f);
	}

	internal void _E001(BetterSource source, float d)
	{
		if (source == null)
		{
			return;
		}
		AudioClip clip = source.GetClip(1);
		if (clip == null)
		{
			return;
		}
		int num;
		if (Environments[0].Clips[1].Clips.Contains(clip))
		{
			num = 1;
		}
		else
		{
			if (!Environments[0].Clips[2].Clips.Contains(clip))
			{
				return;
			}
			num = 3;
		}
		source.Balance(Mathf.InverseLerp(BlendValues[num], BlendValues[num - 1], d));
	}

	private int _E002(AudioClip audioClip)
	{
		for (int i = 0; i < 2; i++)
		{
			if (Environments[0].Clips[i].Clips.Contains(audioClip))
			{
				return i;
			}
		}
		return -1;
	}
}
