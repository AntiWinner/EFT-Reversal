using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.AssetsManager;

public abstract class PoolSafeMonoBehaviour : MonoBehaviour, _EC3C
{
	[_E368]
	public _E367 OriginalState;

	public static readonly Type[] ExcludedBaseClasses = new Type[1] { typeof(UnityEngine.Object) };

	private void _E000()
	{
		if (OriginalState == null)
		{
			OriginalState = new _E367();
			OriginalState.StoreState(this, ExcludedBaseClasses);
		}
	}

	public virtual void OnCreatePoolRoleModel(AssetPoolObject assetPoolObject)
	{
		_E000();
	}

	public virtual void OnCreatePoolObject(AssetPoolObject assetPoolObject)
	{
		_E000();
	}

	public virtual void OnGetFromPool(AssetPoolObject assetPoolObject)
	{
	}

	public virtual void OnReturnToPool(AssetPoolObject assetPoolObject)
	{
		OriginalState.RestoreState(this);
	}

	public virtual void InheritRoleModelData(_EC3C roleModel)
	{
		PoolSafeMonoBehaviour poolSafeMonoBehaviour = roleModel as PoolSafeMonoBehaviour;
		OriginalState = poolSafeMonoBehaviour.OriginalState;
	}

	[SpecialName]
	bool _EC3C.get_enabled()
	{
		return base.enabled;
	}

	[SpecialName]
	void _EC3C.set_enabled(bool value)
	{
		base.enabled = value;
	}
}
