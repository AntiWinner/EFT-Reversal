using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

public class SonicBulletSoundPlayer : BulletSoundPlayer
{
	private class _E000 : _E3F7._E000
	{
		private _E3F1<_E000> _E000;

		private Vector3 _E001;

		private AudioClip _E002;

		private float _E003;

		private int _E004;

		private float _E005;

		public void Initialize(_E3F1<_E000> pool, Vector3 start, AudioClip clip, float distance, int rollOff, float volume)
		{
			_E000 = pool;
			_E001 = start;
			_E002 = clip;
			_E003 = distance;
			_E004 = rollOff;
			_E005 = volume;
		}

		public void Release()
		{
			Singleton<BetterAudio>.Instance.PlayAtPoint(_E001, _E002, _E003, BetterAudio.AudioSourceGroupType.Environment, _E004, _E005);
			_E002 = null;
			_E000.PutObject(this);
		}
	}

	public enum SonicType
	{
		Sonic9,
		Sonic545,
		Sonic762,
		SonicShotgun
	}

	public class _E001
	{
		public _EA12 Ammo;

		public Vector3 ShotPosition;

		public Vector3 ShotDirection;

		public Camera Camera;

		public float Rolloff;

		public float Delay;

		public bool IsOccluded;

		public _E001(_EA12 ammo, Vector3 shotPosition, Vector3 shotDirection, Camera camera, float rolloff, float delay, bool isOccluded)
		{
			Ammo = ammo;
			ShotPosition = shotPosition;
			ShotDirection = shotDirection;
			Camera = camera;
			Rolloff = rolloff;
			Delay = delay;
			IsOccluded = isOccluded;
		}
	}

	[Serializable]
	public class SonicAudio
	{
		public SonicType Type;

		public AudioClip Clip;
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public SonicType type;

		internal bool _E000(SonicAudio x)
		{
			return x.Type == type;
		}
	}

	private readonly float _E009 = 340.29f;

	[SerializeField]
	private List<SonicAudio> _sources;

	[SerializeField]
	private AnimationCurve _soundCurve;

	[SerializeField]
	private float _minDistance = 1.5f;

	private readonly _E3F1<_E000> _E00A = new _E3F1<_E000>(5, 2);

	private void Awake()
	{
		_E3BF.OnShoot += FireBullet;
	}

	private void OnDestroy()
	{
		_E3BF.OnShoot -= FireBullet;
	}

	public void FireBullet(_E001 sonicInfo)
	{
		if (sonicInfo.IsOccluded || sonicInfo.Ammo.InitialSpeed < _E009)
		{
			return;
		}
		Vector3 shotPosition = sonicInfo.ShotPosition;
		Vector3 end = shotPosition + sonicInfo.ShotDirection * 1000f;
		Vector3 vector = _E3BF.CalculateNormalFromPoint(shotPosition, end, sonicInfo.Camera.transform.position);
		if (vector == Vector3.zero)
		{
			return;
		}
		float num = _E8A8.Instance.Distance(shotPosition);
		if (!(num < _minDistance))
		{
			int rollOff = _E001(sonicInfo, num);
			float magnitude = vector.magnitude;
			float num2 = num * 0.125f;
			if (!(magnitude > num2))
			{
				SonicType sonicType = sonicInfo.Ammo.SonicType;
				SonicAudio sonicAudio = _E000(sonicType);
				float time = magnitude / num2;
				float volume = _soundCurve.Evaluate(time);
				_E000 @object = _E00A.GetObject();
				@object.Initialize(_E00A, shotPosition, sonicAudio.Clip, num, rollOff, volume);
				float time2 = Time.time;
				Singleton<BetterAudio>.Instance.AddToAudioSourceQueue(@object, time2 + sonicInfo.Delay);
			}
		}
	}

	private SonicAudio _E000(SonicType type)
	{
		return _sources.First((SonicAudio x) => x.Type == type);
	}

	private int _E001(_E001 sonicInfo, float distance)
	{
		float t = distance / sonicInfo.Rolloff;
		return (int)Mathf.Lerp(2000f, sonicInfo.Rolloff, t);
	}
}
