using System.Collections.Generic;
using Comfort.Common;
using EFT.AssetsManager;
using JetBrains.Annotations;
using UnityEngine;

[DisallowMultipleComponent]
public class MagVisualController : WeaponModPoolObject
{
	private class _E000
	{
		private readonly GameObject m__E000;

		public readonly Transform Transform;

		public readonly int Id;

		public bool IsActive;

		public _E000(Transform transform, int id)
		{
			m__E000 = transform.gameObject;
			Transform = transform;
			Id = id;
			IsActive = m__E000.activeSelf;
		}

		public void SetActive(bool isActive)
		{
			if (isActive != IsActive)
			{
				m__E000.SetActive(isActive);
				IsActive = isActive;
			}
		}
	}

	private const string m__E000 = "patron_";

	private new _E000[] m__E001;

	private _EA6A _E002;

	private List<AmmoPoolObject> _E003 = new List<AmmoPoolObject>();

	private Animation _E004;

	private int _E005 = -1;

	private bool _E006;

	private void _E000()
	{
		if (this.m__E001 == null)
		{
			List<Transform> list = _E38B._E004(base.transform, _ED3E._E000(63896), false);
			this.m__E001 = new _E000[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				Transform transform = list[i];
				int id = int.Parse(transform.name.Replace(_ED3E._E000(63896), ""));
				this.m__E001[i] = new _E000(transform, id);
			}
		}
		_E004 = GetComponent<Animation>();
	}

	public override void OnCreatePoolObject<TAssetPoolObject>([CanBeNull] _EC3A<TAssetPoolObject> assetsPoolParent)
	{
		base.OnCreatePoolObject(assetsPoolParent);
		_E000();
	}

	protected override void ReturnToPool()
	{
		ReturnAmmo();
		_E002 = null;
		_E005 = -1;
		base.ReturnToPool();
	}

	public void ReturnAmmo()
	{
		_E003.ForEach(delegate(AmmoPoolObject ammo)
		{
			AssetPoolObject.ReturnToPool(ammo);
		});
		_E003.Clear();
	}

	public void InitMagazine(_EA6A mag, bool isAnimated = true)
	{
		_E002 = mag;
		_E006 = isAnimated;
		if (mag.Count > 0)
		{
			for (int i = 0; i < this.m__E001.Length; i++)
			{
				_E000 obj = this.m__E001[i];
				if (obj.Id > _E002.Count)
				{
					obj.SetActive(isActive: false);
					continue;
				}
				obj.SetActive(isActive: true);
				_EA12 bulletAtPosition = _E002.GetBulletAtPosition(_E002.Count - 1 - i);
				if (!Singleton<_E760>.Instantiated)
				{
					return;
				}
				GameObject obj2 = Singleton<_E760>.Instance.CreateItem(bulletAtPosition, isAnimated: false);
				AmmoPoolObject component = obj2.GetComponent<AmmoPoolObject>();
				_E003.Add(component);
				if (component != null)
				{
					component.SetUsed(isUsed: false);
				}
				obj2.transform.SetParent(obj.Transform, worldPositionStays: false);
				obj2.transform.localPosition = Vector3.zero;
				obj2.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
				obj2.SetActive(value: true);
			}
		}
		if (!_E006)
		{
			_E001();
		}
		Update();
	}

	public virtual void Update()
	{
		if (_E002 == null)
		{
			return;
		}
		int count = _E002.Count;
		bool flag = _E004 != null && _E006;
		if (flag && _E8A8.Instance.Distance(base.transform.position) > 10f)
		{
			flag = false;
		}
		if (_E004 != null)
		{
			_E004.enabled = flag;
		}
		if (flag)
		{
			_E001();
		}
		int num = this.m__E001.Length;
		for (int i = 0; i < num; i++)
		{
			_E000 obj = this.m__E001[i];
			if (obj.Id > count && obj.IsActive)
			{
				obj.SetActive(isActive: false);
			}
		}
	}

	private void _E001()
	{
		if (_E004 == null || _E002 == null || _E005 == _E002.Count)
		{
			return;
		}
		_E005 = _E002.Count;
		foreach (AnimationState item in _E004)
		{
			item.speed = 0f;
			item.time = Mathf.Clamp((float)_E002.Count * (1f / item.clip.frameRate), 0f, item.clip.length);
		}
	}
}
