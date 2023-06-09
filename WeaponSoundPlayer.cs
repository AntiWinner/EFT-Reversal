using Audio.SpatialSystem;
using Comfort.Common;
using EFT;
using UnityEngine;
using UnityEngine.Audio;

public class WeaponSoundPlayer : BaseSoundPlayer
{
	public SoundBank Body;

	public SoundBank Tail;

	public SoundBank Doublet;

	public SoundBank BodySilenced;

	public SoundBank TailSilenced;

	public SoundBank DoubletSilenced;

	private bool _isSilenced;

	private Player.FirearmController _firearm;

	private float _prevDistance;

	private _E3BD _queue;

	private bool _isFiring;

	private float _firingLoopLength;

	private const int BEATS = 16;

	public bool Non_auto;

	private float _releaseTime;

	private float _occlusionReleaseTime;

	public const float SOUND_SPEED = 340.29f;

	private float _pitch = 1f;

	private float _balance = 1f;

	private float _tailLn;

	private float _start;

	private float _delay;

	private AudioMixerGroup _group;

	private float _prevPitchMult;

	public bool IsSilenced
	{
		get
		{
			return _isSilenced;
		}
		set
		{
			_isSilenced = value;
			if (BodySilenced == null)
			{
				BodySilenced = Body;
			}
			if (TailSilenced == null)
			{
				TailSilenced = Tail;
			}
			_firingLoopLength = (IsSilenced ? BodySilenced.ClipLength : Body.ClipLength);
		}
	}

	public bool IsAutoWeapon => !Non_auto;

	public float BeatLn => _firingLoopLength / 16f;

	public bool IsOccludedToListener => _group != Singleton<BetterAudio>.Instance.GunshotMixerGroup;

	private float Distance => _E8A8.Instance.Distance(_weaponHierarchy.position);

	public float ReleaseTime
	{
		set
		{
			_releaseTime = value + 0.1f;
		}
	}

	public override void Init(_E6C7 firearmController, BifacialTransform weaponHierarchy, Player player)
	{
		base.Init(firearmController, weaponHierarchy, player);
		_firearm = firearmController as Player.FirearmController;
		if (_firearm != null && IsAutoWeapon)
		{
			_firearm.BreakLoop += OnBreakLoop;
			CompositeDisposable.AddDisposable(delegate
			{
				_firearm.BreakLoop -= OnBreakLoop;
			});
		}
		if (!(BodySilenced == null) || !(Body == null))
		{
			_firingLoopLength = (IsSilenced ? BodySilenced.ClipLength : Body.ClipLength);
			_group = MonoBehaviourSingleton<BetterAudio>.Instance.GunshotMixerGroup;
		}
	}

	public override void Update()
	{
		base.Update();
		if (_queue != null)
		{
			UpdateOcclusion();
			_queue.Pose(_weaponHierarchy.position);
			if (!_isFiring && Time.time > _releaseTime)
			{
				Release();
			}
		}
	}

	protected override void OnDisable()
	{
		if (_isFiring)
		{
			StopFiringLoop();
		}
		Release(_releaseTime - Time.time);
		base.OnDisable();
	}

	protected override void OnDestroy()
	{
		if (_isFiring)
		{
			StopFiringLoop();
		}
		Release(_releaseTime - Time.time);
		base.OnDestroy();
		_firearm = null;
	}

	private void Release(float timeLeftToPlay = 0f)
	{
		if (_queue != null && Singleton<BetterAudio>.Instantiated)
		{
			Singleton<BetterAudio>.Instance.ReleaseQueueDelayed(_queue, timeLeftToPlay);
			_queue = null;
		}
	}

	private void UpdateOcclusion()
	{
		if (Player.PointOfView != 0 && _queue != null && (object)Body != null)
		{
			if ((!_isFiring && Time.time > _occlusionReleaseTime) || Distance > Body.Rolloff / 2f)
			{
				EnableSourceOcclusion(enabledOcclusion: false);
				return;
			}
			_group = _E487.GetGunshotOcclusionMixerGroup(_queue.AudioSources[0].LowPassFilterLevel);
			_queue.SetMixerGroup(_group);
		}
	}

	public void FireBullet(_EA12 ammo, Vector3 shotPosition, Vector3 shotDirection, float pitchMult, bool malfunctioned = false, bool multiShot = false, bool burstOf2Start = false)
	{
		SoundBank soundBank = ((!multiShot) ? (IsSilenced ? BodySilenced : Body) : (IsSilenced ? DoubletSilenced : Doublet));
		if (soundBank == null)
		{
			return;
		}
		if (_isFiring)
		{
			if (_queue != null)
			{
				if (IsAutoWeapon)
				{
					Balance(soundBank);
				}
				if (Mathf.Abs(pitchMult - _prevPitchMult) > Mathf.Epsilon)
				{
					_pitch = pitchMult;
					_prevPitchMult = pitchMult;
					_queue.SetPitch(_pitch);
				}
			}
			return;
		}
		float distance = Distance;
		_prevDistance = Distance;
		if (_queue == null)
		{
			_queue = Singleton<BetterAudio>.Instance.BorrowWeaponAudioQueue(BetterAudio.AudioSourceGroupType.Gunshots);
			if (_queue == null)
			{
				return;
			}
			_queue.Pose(_weaponHierarchy.position);
			_queue.SetRolloff(soundBank.Rolloff);
			_queue.SetMixerGroup(MonoBehaviourSingleton<BetterAudio>.Instance.GunshotMixerGroup);
			RegisterSourceForOcclusionProcess();
		}
		EnableSourceOcclusion(enabledOcclusion: true);
		if (!(_queue.AudioSources[0].OcclusionVolumeFactor <= 0f))
		{
			_queue.SetMixerGroup(_group);
			if (IsAutoWeapon)
			{
				_isFiring = true;
			}
			AudioClip clip = null;
			AudioClip clip2 = null;
			soundBank.PickClipsByDistance(ref clip, ref clip2, ref _balance, (int)Player.Environment, distance);
			_pitch = pitchMult;
			_prevPitchMult = pitchMult;
			float num = Mathf.Max((clip != null) ? clip.length : 0f, (clip2 != null) ? clip2.length : 0f);
			_delay = distance / 340.29f;
			_start = Time.time;
			_releaseTime = _start + num + 1f;
			_occlusionReleaseTime = float.MaxValue;
			float sonicDelay = _delay;
			_queue.Enqueue(clip, clip2, _balance, _start + _delay, Non_auto ? (num / _pitch) : 0f, soundBank.BaseVolume, _pitch);
			if ((!_firearm.IsTriggerPressed || malfunctioned || burstOf2Start) && IsAutoWeapon)
			{
				SoundBank soundBank2 = (IsSilenced ? TailSilenced : Tail);
				AudioClip clip3 = null;
				AudioClip clip4 = null;
				soundBank2.PickClipsByDistance(ref clip3, ref clip4, ref _balance, (int)Player.Environment, distance);
				float num2 = Mathf.Max((clip3 != null) ? clip3.length : 0f, (clip4 != null) ? clip4.length : 0f);
				float num3 = (burstOf2Start ? (BeatLn * 2f) : BeatLn);
				sonicDelay = _delay + num3 / _pitch;
				_queue.Enqueue(clip3, clip4, _balance, _start + _delay + num3 / _pitch, num2 / _pitch, soundBank2.BaseVolume, _pitch);
				_isFiring = false;
				_releaseTime = _start + _delay + num3 / _pitch + num2 / _pitch;
				_occlusionReleaseTime = _start + _delay + num3 / _pitch;
			}
			FireSonicSound(sonicDelay, soundBank.Rolloff, ammo, shotPosition, shotDirection);
		}
	}

	private void EnableSourceOcclusion(bool enabledOcclusion)
	{
		if (Player.PointOfView != 0)
		{
			_E3BD queue = _queue;
			if (queue != null)
			{
				queue.AudioSources[0]?.IncludeInOcclusionProcess(enabledOcclusion);
			}
			_E3BD queue2 = _queue;
			if (queue2 != null)
			{
				queue2.AudioSources[0]?.IncludeInOcclusionProcess(enabledOcclusion);
			}
		}
	}

	private void RegisterSourceForOcclusionProcess()
	{
		if (Player.PointOfView != 0 && MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
		{
			MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ProcessSourceOcclusion(Player, _queue.AudioSources[0]);
			MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ProcessSourceOcclusion(Player, _queue.AudioSources[1]);
		}
	}

	private void Balance(SoundBank bank)
	{
		if (_queue != null && (object)Body != null && !(Mathf.Abs(Distance - _prevDistance) < Mathf.Epsilon))
		{
			_prevDistance = Distance;
			BetterSource[] audioSources = _queue.AudioSources;
			for (int i = 0; i < audioSources.Length; i++)
			{
				bank._E001(audioSources[i], Distance);
			}
		}
	}

	private void OnBreakLoop()
	{
		if (base.gameObject.activeSelf && _isFiring)
		{
			StopFiringLoop();
		}
	}

	private void StopFiringLoop()
	{
		int num = (int)((Time.time - _start) / BeatLn) + 1;
		AudioClip clip = null;
		AudioClip clip2 = null;
		SoundBank soundBank = (IsSilenced ? TailSilenced : Tail);
		soundBank.PickClipsByDistance(ref clip, ref clip2, ref _balance, (int)Player.Environment, Distance);
		float num2 = Mathf.Max((clip != null) ? clip.length : 0f, (clip2 != null) ? clip2.length : 0f);
		_queue?.Enqueue(clip, clip2, _balance, _start + _delay + (float)num * BeatLn / _pitch, num2 / _pitch, soundBank.BaseVolume, _pitch);
		_isFiring = false;
		_releaseTime = _start + _delay + (float)num * BeatLn / _pitch + num2 / _pitch;
	}

	private void FireSonicSound(float sonicDelay, float rolloff, _EA12 ammo, Vector3 shotPosition, Vector3 shotDirection)
	{
		if (!(_E8A8.Instance.Camera == null) && Player.PointOfView != 0 && ammo.ProjectileCount == 1)
		{
			_E3BF.Shoot(new SonicBulletSoundPlayer._E001(ammo, shotPosition, shotDirection, _E8A8.Instance.Camera, rolloff, sonicDelay, IsOccludedToListener));
		}
	}
}
