using System;
using System.Linq;
using Comfort.Common;
using EFT.Animations;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ShotEffector : IEffector
{
	[Serializable]
	public class ShotVal
	{
		public ComponentType From;

		public Target To;

		public ComponentType Component;

		public float Intensity;

		private Spring _target;

		private int _component;

		private int _from;

		public void Initialize(Spring rootRotation, Spring handsRotation, Spring handsPosition)
		{
			switch (To)
			{
			case Target.RootRotation:
				_target = rootRotation;
				break;
			case Target.HandsPosition:
				_target = handsPosition;
				break;
			case Target.HandsRotation:
				_target = handsRotation;
				break;
			}
			_component = (int)Component;
			_from = (int)From;
			if (_target == null)
			{
				Debug.LogError(_ED3E._E000(46676));
			}
		}

		public void Process(Vector3 rnd)
		{
			_target.AddAccelerationLimitless(_component, rnd[_from] * Intensity);
		}
	}

	[_E2BD(0f, 100f, -1f)]
	[FormerlySerializedAs("RecoilStrengthXY")]
	public Vector2 RecoilStrengthXy;

	[_E2BD(0f, 100f, -1f)]
	public Vector2 RecoilStrengthZ;

	[_E2BD(-360f, 360f, -1f)]
	public Vector2 RecoilDegree;

	public Vector3 RecoilDirection;

	public Vector2 RecoilRadian;

	[_E2BE("From 60;To 120;Component 60;Intensity 80", false)]
	public ShotVal[] ShotVals;

	public float Stiffness = 1f;

	public _E2BD Recoil;

	public _E2BD RecoilPower;

	public float Intensity = 1f;

	private Vector3 _separateIntensityFactors = new Vector3(1f, 1f, 1f);

	private IWeapon _weapon;

	private Weapon _mainWeaponInHands;

	private _E5CB._E024 _aimingConfig;

	private _E74F._E000 _buffs = new _E74F._E000();

	private int _pose = -1;

	private _E5CB._E024 _E000
	{
		get
		{
			if (_aimingConfig == null)
			{
				_aimingConfig = Singleton<_E5CB>.Instance.Aiming;
			}
			return _aimingConfig;
		}
	}

	public int Pose
	{
		get
		{
			return _pose;
		}
		set
		{
			_pose = value;
			float z = ((_weapon == null) ? this._E000.RecoilZIntensityByPose[_pose] : Mathf.Min(_weapon.WeaponTemplate.RecoilPosZMult, this._E000.RecoilZIntensityByPose[_pose]));
			_separateIntensityFactors = new Vector3(this._E000.RecoilXIntensityByPose[_pose], this._E000.RecoilYIntensityByPose[_pose], z);
		}
	}

	public float ConvertFromTaxanomy(float f)
	{
		return f * 0.1399f;
	}

	public void Initialize(PlayerSpring playerSpring)
	{
		ShotVal[] shotVals = ShotVals;
		for (int i = 0; i < shotVals.Length; i++)
		{
			shotVals[i].Initialize(playerSpring.CameraRotation, playerSpring.Recoil, playerSpring.HandsPosition);
		}
		RecoilRadian = RecoilDegree * ((float)Math.PI / 180f);
	}

	public void Process(float str = 1f)
	{
		RecoilRadian = RecoilDegree * ((float)Math.PI / 180f);
		float f = UnityEngine.Random.Range(RecoilRadian.x, RecoilRadian.y);
		float num = UnityEngine.Random.Range(RecoilStrengthXy.x, RecoilStrengthXy.y) * str;
		float num2 = UnityEngine.Random.Range(RecoilStrengthZ.x, RecoilStrengthZ.y) * str;
		RecoilDirection = new Vector3((0f - Mathf.Sin(f)) * num * _separateIntensityFactors.x, Mathf.Cos(f) * num * _separateIntensityFactors.y, num2 * _separateIntensityFactors.z) * Intensity;
		Vector2 vector = _weapon?.MalfState.OverheatBarrelMoveDir ?? Vector2.zero;
		float num3 = _weapon?.MalfState.OverheatBarrelMoveMult ?? 0f;
		float num4 = (RecoilRadian.x + RecoilRadian.y) / 2f * ((RecoilStrengthXy.x + RecoilStrengthXy.y) / 2f) * num3;
		RecoilDirection.x += vector.x * num4;
		RecoilDirection.y += vector.y * num4;
		ShotVal[] shotVals = ShotVals;
		for (int i = 0; i < shotVals.Length; i++)
		{
			shotVals[i].Process(RecoilDirection);
		}
	}

	internal void _E000(IWeapon activeWeapon, _E74F._E000 buffInfo, Weapon mainWeaponInHands)
	{
		_buffs = buffInfo;
		if (_weapon != null)
		{
			_weapon.Item.Attributes.ElementAt(0).OnUpdate -= OnWeaponParametersChanged;
		}
		_weapon = activeWeapon;
		_weapon.Item.Attributes.ElementAt(0).OnUpdate += OnWeaponParametersChanged;
		_weapon.Item.UpdateAttributes();
		_mainWeaponInHands = mainWeaponInHands;
		float y = _buffs.RecoilSupression.y;
		float num = Mathf.LerpAngle(_weapon.WeaponTemplate.RecoilAngle, 90f, y);
		float num2 = (float)_weapon.WeaponTemplate.RecolDispersion / (1f + y);
		RecoilDegree = new Vector2(num - num2, num + num2);
		RecoilRadian = RecoilDegree * ((float)Math.PI / 180f);
	}

	private float _E001()
	{
		float num = _weapon.RecoilDelta;
		if (_weapon.IsUnderbarrelWeapon)
		{
			num += _mainWeaponInHands.StockRecoilDelta;
		}
		return num;
	}

	public void OnWeaponParametersChanged()
	{
		WeaponTemplate weaponTemplate = _weapon.WeaponTemplate;
		float num = Mathf.Max(0f, (1f + _E001()) * (1f - _buffs.RecoilSupression.x - _buffs.RecoilSupression.y * 0.1f));
		RecoilStrengthXy = new Vector2(0.9f, 1.15f) * ConvertFromTaxanomy(weaponTemplate.RecoilForceUp * num + this._E000.RecoilVertBonus);
		RecoilStrengthZ = new Vector2(0.65f, 1.05f) * ConvertFromTaxanomy(weaponTemplate.RecoilForceBack * num + this._E000.RecoilBackBonus);
		float cameraRecoil = weaponTemplate.CameraRecoil;
		ShotVals[3].Intensity = cameraRecoil;
		ShotVals[4].Intensity = 0f - cameraRecoil;
	}

	public string DebugOutput()
	{
		throw new NotImplementedException();
	}
}
