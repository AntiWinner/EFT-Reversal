using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio.SpatialSystem;
using Comfort.Common;
using UnityEngine;
using UnityEngine.Audio;

namespace EFT.Interactive;

public class TreeInteractive : MonoBehaviour, _E31B, IPhysicsTrigger
{
	public Terrain Terrain;

	public int InstanceIndex;

	[SerializeField]
	public SoundBank _soundBank;

	private Dictionary<Collider, BetterSource> m__E000 = new Dictionary<Collider, BetterSource>();

	private const float _E001 = 1f;

	private Dictionary<Collider, Player> _E002 = new Dictionary<Collider, Player>();

	private float _E003 = 1f;

	private const float _E004 = -0.33f;

	private const float _E005 = 4.5f;

	private const float _E006 = 0.1f;

	private const float _E007 = 0.0233f;

	private const float _E008 = 0.5f;

	private float _E009;

	[CompilerGenerated]
	private readonly string _E00A = _ED3E._E000(212968);

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E00A;
		}
	}

	public void OnTriggerEnter(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		float num = _E8A8.Instance.Distance(col.transform.position);
		if (playerByCollider == null || _soundBank == null || num > _soundBank.Rolloff || this.m__E000.ContainsKey(col) || _E002.ContainsKey(col) || Singleton<AbstractGame>.Instance.GameTimer.Status == _E62B.EGameTimerStatus.Unknown)
		{
			return;
		}
		_E002.Add(col, playerByCollider);
		BetterSource source = MonoBehaviourSingleton<BetterAudio>.Instance.GetSource(_soundBank, activateSource: false);
		if (!(source == null))
		{
			if (playerByCollider.AIData != null)
			{
				playerByCollider.AIData.EnterTree(this);
			}
			this.m__E000[col] = source;
			_E003 = 0f;
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if (this.m__E000.ContainsKey(col))
		{
			BetterSource betterSource = this.m__E000[col];
			if (!betterSource.VolumeFadeOut(1f, betterSource.Release))
			{
				betterSource.Release();
			}
			Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
			if (playerByCollider != null && playerByCollider.AIData != null)
			{
				playerByCollider.AIData.ExitTree(this);
			}
			this.m__E000.Remove(col);
			_E002.Remove(col);
		}
	}

	public void OnTriggerStay(Collider col)
	{
		if (!this.m__E000.ContainsKey(col))
		{
			return;
		}
		Player player = _E002[col];
		if (!(_E8A8.Instance.Distance(col.transform.position) > _soundBank.Rolloff * player.Physical.SoundRadius))
		{
			BetterSource betterSource = this.m__E000[col];
			Vector3 velocity = player.MovementContext.CharacterController.velocity;
			float value = Mathf.Abs(player.MovementContext.AverageRotationSpeed.Avarage);
			float num = Mathf.Max(Mathf.InverseLerp(-0.33f, 4.5f, velocity.magnitude), Mathf.InverseLerp(45f, 540f, value));
			float num2 = num - _E003;
			_E003 = ((num2 > 0f) ? num : Mathf.Clamp(_E003 - 0.0233f, num, 1f));
			bool forceStereo = player.PointOfView == EPointOfView.FirstPerson && Vector3.Distance(col.transform.position, base.transform.position) < 0.1f;
			Vector3 soundPosition = ((player.PointOfView == EPointOfView.FirstPerson) ? col.bounds.ClosestPoint(base.transform.position) : col.transform.position);
			if ((!betterSource.source1.isPlaying || num2 > 0.5f) && num > 0.1f)
			{
				_E000(soundPosition, betterSource, player, forceStereo);
			}
			betterSource.SetBaseVolume(_soundBank.BaseVolume * _E003);
		}
	}

	private void _E000(Vector3 soundPosition, BetterSource source, Player player, bool forceStereo = false)
	{
		source.gameObject.SetActive(value: true);
		source.transform.position = soundPosition;
		if (player.PointOfView == EPointOfView.FirstPerson)
		{
			source.SetMixerGroup(MonoBehaviourSingleton<BetterAudio>.Instance.GetOcclusionGroupSimple(source.transform.position));
			source.source1.spatialBlend = 0.7f;
		}
		else
		{
			if (MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
			{
				MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ProcessSourceOcclusion(source, EOcclusionTest.Fast);
			}
			float distance = _E8A8.Instance.Distance(soundPosition);
			AudioMixerGroup mixerGroup = _E487.VolumeDependentOcclusion(soundPosition, ref _E003, distance);
			source.SetMixerGroup(mixerGroup);
			source.source1.spatialBlend = 1f;
			_E003 = source.source1.volume;
			if (_E003 == 0f || source.OcclusionVolumeFactor <= 0f)
			{
				return;
			}
		}
		float num = _E003;
		float num2 = 1f - (float)player.Skills.BotSoundCoef;
		num *= num2;
		source.SetRolloff(_soundBank.Rolloff * player.Physical.SoundRadius);
		source.SetPitch(1f + Random.Range(-0.1f, 0.1f));
		_soundBank.PlayOn(source, EnvironmentType.Outdoor, 0f, num, forceStereo);
	}

	[SpecialName]
	bool IPhysicsTrigger.get_enabled()
	{
		return base.enabled;
	}

	[SpecialName]
	void IPhysicsTrigger.set_enabled(bool value)
	{
		base.enabled = value;
	}
}
