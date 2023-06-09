using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Systems.Effects;
using Comfort.Common;
using EFT;
using UnityEngine;
using UnityEngine.Networking;

public class MineDirectional : MonoBehaviour, IPhysicsTrigger
{
	public class _E000
	{
		private ArrayPool<byte> m__E000;

		public _E000()
		{
			m__E000 = ArrayPool<byte>.Shared;
			m__E000.Preallocate(65536, 2);
		}

		public byte[] Serialize(MineDirectional mineDirectional)
		{
			NetworkWriter networkWriter = new NetworkWriter();
			networkWriter.Write(mineDirectional.transform.position);
			return networkWriter.AsArray();
		}

		public static Vector3 Deserialize(byte[] data)
		{
			return new NetworkReader(data).ReadVector3();
		}
	}

	[Serializable]
	public struct MineSettings : IExplosiveItem
	{
		[SerializeField]
		private Vector3 _blindness;

		[SerializeField]
		private Vector3 _contusion;

		[SerializeField]
		private Vector3 _armorDistanceDistanceDamage;

		[SerializeField]
		private float _minExplosionDistance;

		[SerializeField]
		private float _maxExplosionDistance;

		[SerializeField]
		private int _fragmentsCount;

		[SerializeField]
		private float _strength;

		[SerializeField]
		private string _tag;

		[SerializeField]
		private float _armorDamage;

		[SerializeField]
		private float _staminaBurnRate;

		[SerializeField]
		private float _penetrationPower;

		[SerializeField]
		private string _fragmentType;

		[SerializeField]
		private string _fxName;

		[SerializeField]
		private WildSpawnType _ignoreRole;

		[SerializeField]
		private float _directionalDamageAngle;

		[SerializeField]
		private float _directionalDamageMultiplier;

		public WildSpawnType IgnoreRole => _ignoreRole;

		public float ArmorDamage => _armorDamage;

		public float StaminaBurnRate => _staminaBurnRate;

		public float PenetrationPower => _penetrationPower;

		public string FXName => _fxName;

		public Vector3 Blindness => _blindness;

		public Vector3 Contusion => _contusion;

		public Vector3 ArmorDistanceDistanceDamage => _armorDistanceDistanceDamage;

		public float MinExplosionDistance => _minExplosionDistance;

		public float MaxExplosionDistance => _maxExplosionDistance;

		public int FragmentsCount => _fragmentsCount;

		public float GetStrength => _strength;

		public float GetDirectionalDamageAngle => _directionalDamageAngle;

		public float GetDirectionalDamageMultiplier => _directionalDamageMultiplier;

		public string Tag => _tag;

		public string FragmentType => _fragmentType;

		public bool IsDummy { get; }

		public _EA12 CreateFragment()
		{
			if (!Singleton<_E63B>.Instance.ItemTemplates.ContainsKey(FragmentType))
			{
				return null;
			}
			return (_EA12)Singleton<_E63B>.Instance.CreateItem(Guid.NewGuid().ToString(), FragmentType, null);
		}
	}

	private static int m__E000;

	public static List<MineDirectional> Mines = new List<MineDirectional>();

	private static Lazy<_EC1E> m__E001 = new Lazy<_EC1E>(() => Singleton<_E5CE>.Instance.CreateBallisticCalculator(0));

	[SerializeField]
	private MineSettings _mineData;

	private bool m__E002;

	private bool m__E003;

	public string Tag => _mineData.Tag;

	public string Description => _ED3E._E000(55426);

	private void Awake()
	{
		Mines.Add(this);
	}

	private bool _E000(Player player)
	{
		return false;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!this.m__E002 && !this.m__E003 && !_E001(other) && Singleton<GameWorld>.Instance is ClientLocalGameWorld)
		{
			Explosion();
		}
	}

	private bool _E001(Collider other)
	{
		try
		{
			Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(other);
			if (playerByCollider == null)
			{
				return true;
			}
			if (!playerByCollider.IsAI)
			{
				return _E000(playerByCollider);
			}
			_E279 aIData = playerByCollider.AIData;
			if (aIData == null)
			{
				return false;
			}
			if (_mineData.IgnoreRole.Equals(null))
			{
				return false;
			}
			return aIData.BotOwner.IsRole(_mineData.IgnoreRole);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		return false;
	}

	public void OnTriggerExit(Collider collider)
	{
	}

	public void SetArmed(bool isArmed)
	{
		this.m__E002 = !isArmed;
	}

	public void Explosion()
	{
		Singleton<GameWorld>.Instance.MineManager.OnMineExplosion(this);
		if (!string.IsNullOrEmpty(_mineData.FXName))
		{
			Singleton<Effects>.Instance.EmitGrenade(_mineData.FXName, base.transform.position, Vector3.up);
		}
		_E002(_mineData, base.transform.position, MineDirectional.m__E001.Value);
		this.m__E002 = true;
		this.m__E003 = true;
		base.gameObject.SetActive(value: false);
	}

	private void _E002(MineSettings mineData, Vector3 explosionPosition, _EC1E ballisticsCalculator)
	{
		if (!mineData.IsDummy && !this.m__E002 && !this.m__E003)
		{
			mineData.Explosion(explosionPosition, null, ballisticsCalculator, null, _E003, mineData.GetDirectionalDamageMultiplier, mineData.GetDirectionalDamageAngle, base.transform.forward);
		}
	}

	private _EC23 _E003()
	{
		_EC23 result = default(_EC23);
		result.DamageType = EDamageType.Landmine;
		result.ArmorDamage = _mineData.ArmorDamage;
		result.StaminaBurnRate = _mineData.StaminaBurnRate;
		result.PenetrationPower = _mineData.PenetrationPower;
		result.Direction = Vector3.zero;
		result.Player = null;
		result.IsForwardHit = true;
		return result;
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
