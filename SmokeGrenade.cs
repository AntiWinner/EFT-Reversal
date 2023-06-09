using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT;
using EFT.MovingPlatforms;
using UnityEngine;

public class SmokeGrenade : Grenade, MovingPlatform._E000
{
	private float m__E000;

	private float _E001;

	private float _E002;

	private Vector3 _E003 = new Vector3(1f, 1f, 1f);

	public Action<Grenade> EmissionEnd;

	private const float _E004 = 60f;

	[CompilerGenerated]
	private MovingPlatform _E005;

	public float EmitTime => _E002;

	private SmokeGrenadeSettings _E006 => _grenadeSettings as SmokeGrenadeSettings;

	public SphereCollider Area => _E006._emissionArea;

	public float Radius => this._E001;

	public MovingPlatform Platform
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		set
		{
			_E005 = value;
		}
	}

	public _E335 NetworkData
	{
		get
		{
			_E335 result = default(_E335);
			result.PlatformId = (short)((Platform == null) ? (-1) : ((short)Array.IndexOf(Singleton<GameWorld>.Instance.Platforms, Platform)));
			result.Position = ((Platform == null) ? base.transform.position : base.transform.localPosition);
			result.Template = base.WeaponSource.TemplateId;
			result.Id = base.WeaponSource.Id;
			result.Time = (int)(Time.time - this.m__E000);
			result.Orientation = ((Platform == null) ? base.transform.rotation : base.transform.localRotation);
			return result;
		}
	}

	private Vector3 _E007 => base.transform.position;

	public float LifeTime
	{
		get
		{
			Keyframe keyframe = _E006._sizeOverTime[_E006._sizeOverTime.length - 1];
			return EmitTime * keyframe.time;
		}
	}

	public override void Init(GrenadeSettings settings, Player player, _EADF throwWeap, float timeSpent, _EC1E calculator)
	{
		_E002 = throwWeap.EmitTime;
		if (_E002 <= 0f)
		{
			Debug.LogError(throwWeap.ShortName.Localized() + _ED3E._E000(63799));
			_E002 = 30f;
		}
		_E002 -= timeSpent;
		base.Init(settings, player, throwWeap, timeSpent, calculator);
		StartEmissionBehaviour();
		base.VelocityBelowThreshold += RemoveRigidBodyOnVelocityDrop;
		this.StartBehaviourTimer(60f, RemoveRigidbody);
	}

	private void _E000()
	{
		for (int i = 0; i < _E006._sizeOverTime.length; i++)
		{
			Keyframe keyframe = _E006._sizeOverTime[i];
			if (keyframe.value > this._E001)
			{
				this._E001 = keyframe.value;
			}
		}
	}

	public void SetEmission(float time)
	{
		_E000();
		this.m__E000 = time;
	}

	[ContextMenu("Update Emission Area")]
	public void UpdateEmissionArea(float normalTime)
	{
		float num = Mathf.Clamp01(_E006._sizeOverTime.Evaluate(normalTime));
		bool flag = num > 0.1f;
		_E006._emissionArea.gameObject.SetActive(flag);
		_E006._emissionArea.transform.parent = (flag ? null : base.transform);
		float num2 = ((normalTime < 0.5f) ? Mathf.Lerp(_E006._areaStartPosNorm, 1f, num) : 1f);
		if (flag)
		{
			_E006._emissionArea.transform.position = base.transform.position + base.transform.rotation * _E006._pivot * num2;
			_E006._emissionArea.transform.localScale = _E003 * _E006._initialRadius * num * _E006._radiusMultiplier;
			_E006._emissionArea.transform.rotation = Quaternion.identity;
		}
	}

	public override void OnExplosion()
	{
		SetEmission(Time.time);
		StartCoroutine(EmissionAreaHandler(2f));
		StartCoroutine(DelayForEmitEvent(3f));
	}

	public override void SetThrowForce(Vector3 force)
	{
		base.SetThrowForce(force);
		Rigidbody.AddTorque(_E006._torque + _E006._torque * UnityEngine.Random.Range(0f - _E006._torqueDelta, _E006._torqueDelta), ForceMode.Acceleration);
	}

	public void StartEmissionBehaviour()
	{
		this.StartBehaviourTimer(EmitTime, InvokeEmissionEndEvent);
	}

	protected virtual void RemoveRigidBodyOnVelocityDrop(Throwable grenade)
	{
		RemoveRigidbody();
	}

	public void RemoveRigidbody()
	{
		if (Rigidbody != null)
		{
			UnityEngine.Object.Destroy(Rigidbody);
			Rigidbody = null;
		}
		if (Platform == null)
		{
			Singleton<GameWorld>.Instance.BoardIfOnPlatform(this, base.transform.position);
		}
	}

	[ContextMenu("EmissionEnd")]
	public void InvokeEmissionEndEvent()
	{
		if (EmissionEnd != null)
		{
			EmissionEnd(this);
		}
	}

	public IEnumerator DelayForEmitEvent(float time)
	{
		yield return new WaitForSeconds(time);
		if (Singleton<_E307>.Instantiated)
		{
			SmokeGrenade smokeGrenade = this;
			float smokeRadius = smokeGrenade?.Radius ?? 0f;
			float smokeLifeTime = smokeGrenade?.LifeTime ?? 0f;
			Singleton<_E307>.Instance.GrenadeExplosion(_E007, base.Player, isSmoke: true, smokeRadius, smokeLifeTime);
		}
	}

	public IEnumerator EmissionAreaHandler(float rate)
	{
		float num = 0f;
		while (num < _E006._sizeOverTime[_E006._sizeOverTime.length - 1].time)
		{
			num = (Time.time - this.m__E000) / EmitTime;
			UpdateEmissionArea(num);
			yield return new WaitForSeconds(rate);
		}
		UpdateEmissionArea(0f);
		base.enabled = false;
	}

	public void Board(MovingPlatform platform)
	{
		Platform = platform;
		base.transform.parent = platform.transform;
	}

	public void GetOff(MovingPlatform platform)
	{
		if (!(Platform != platform))
		{
			Platform = null;
			base.transform.parent = null;
		}
	}
}
