using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Systems.Effects;
using Comfort.Common;
using EFT.AssetsManager;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT;

public class GrenadeCartridge : Throwable
{
	public delegate void _E000(Player player, _EA12 grenade, Item weapon, Vector3 position, GrenadeCartridge grenadeCartridge);

	private Player _E019;

	private _EA12 _E037;

	private Item _E01B;

	[CompilerGenerated]
	private _E000 _E038;

	private bool _E039;

	private WeaponSounds _E03A;

	private const float _E03B = 76f;

	public override int Id => _E037.Id.GetHashCode();

	public override bool HasNetData => false;

	public event _E000 BlowUpEvent
	{
		[CompilerGenerated]
		add
		{
			_E000 obj = _E038;
			_E000 obj2;
			do
			{
				obj2 = obj;
				_E000 value2 = (_E000)Delegate.Combine(obj2, value);
				obj = Interlocked.CompareExchange(ref _E038, value2, obj2);
			}
			while ((object)obj != obj2);
		}
		[CompilerGenerated]
		remove
		{
			_E000 obj = _E038;
			_E000 obj2;
			do
			{
				obj2 = obj;
				_E000 value2 = (_E000)Delegate.Remove(obj2, value);
				obj = Interlocked.CompareExchange(ref _E038, value2, obj2);
			}
			while ((object)obj != obj2);
		}
	}

	private void Awake()
	{
	}

	public void Init(Player player, _EA12 grenadeCartridgeAmmo, Item weapon)
	{
		_E037 = grenadeCartridgeAmmo;
		_E01B = weapon;
		_E019 = player;
		CapsuleCollider capsuleCollider = base.gameObject.AddComponent<CapsuleCollider>();
		capsuleCollider.radius = 0.03f;
		capsuleCollider.center = new Vector3(0f, 0f, -0.005f);
		capsuleCollider.height = 0.12f;
		capsuleCollider.direction = 2;
		Rigidbody = base.gameObject.GetComponent<Rigidbody>();
		if (Rigidbody == null)
		{
			Rigidbody = base.gameObject.AddComponent<Rigidbody>();
		}
		_E320._E002.SupportRigidbody(Rigidbody);
		Rigidbody.isKinematic = false;
		Rigidbody.mass = _E037.Weight;
		Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		_E03A = Singleton<_ED0A>.Instance.InstantiateAsset<WeaponSounds>(_ED3E._E000(35307));
		_E039 = false;
		base.enabled = true;
	}

	public void Launch()
	{
		Rigidbody.velocity = 76f * base.transform.forward;
	}

	private void _E000()
	{
		Singleton<Effects>.Instance.Emit(_ED3E._E000(138958), base.transform.position, Vector3.up);
		if (_E038 != null)
		{
			_E038(_E019, _E037, _E01B, base.transform.position, this);
		}
		AssetPoolObject.ReturnToPool(base.gameObject, immediate: false);
		base.enabled = false;
		Singleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, _E03A.GrenadeDropSoundBank, _E8A8.Instance.Distance(base.transform.position));
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (base.enabled)
		{
			_E039 = true;
		}
	}

	private void Update()
	{
		if (_E039)
		{
			_E000();
			_E039 = false;
		}
	}
}
