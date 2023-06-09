using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;
using UnityEngine;

namespace EFT.AssetsManager;

[DisallowMultipleComponent]
public class AssetPoolObject : MonoBehaviour
{
	private sealed class _E000 : PooledObjectPolicy<List<AssetPoolObject>>
	{
		public override List<AssetPoolObject> Create()
		{
			return new List<AssetPoolObject>(10);
		}

		public override bool Return(List<AssetPoolObject> obj)
		{
			obj.Clear();
			return true;
		}
	}

	public class _E001
	{
		public Collider Collider;

		public int Layer;

		public bool WasEnabled;

		public void Reset()
		{
			Collider.gameObject.layer = Layer;
			Collider.enabled = WasEnabled;
		}
	}

	private static readonly ObjectPool<List<AssetPoolObject>> _E028;

	public List<string> PoolHistory = new List<string>();

	public List<Collider> Colliders = new List<Collider>();

	public List<_EC3C> Components = new List<_EC3C>();

	[SerializeField]
	private List<Component> _originallyEnabledComponents = new List<Component>();

	[CompilerGenerated]
	private bool _E029;

	[CanBeNull]
	public _E333 ContainerCollectionView;

	[CompilerGenerated]
	private bool _E02A;

	protected _E54C ResourceType;

	private _EC37 _E02B;

	private bool _E02C;

	public List<Component> RegisteredComponentsToClean = new List<Component>();

	public List<_E001> RegisteredCollidersToDisable = new List<_E001>();

	public bool IsStub
	{
		[CompilerGenerated]
		get
		{
			return _E029;
		}
		[CompilerGenerated]
		set
		{
			_E029 = value;
		}
	}

	public bool IsInPool
	{
		[CompilerGenerated]
		get
		{
			return _E02A;
		}
		[CompilerGenerated]
		private set
		{
			_E02A = value;
		}
	}

	public string Name
	{
		get
		{
			if (_E02B == null)
			{
				return base.name;
			}
			return _E02B.PoolShortName;
		}
	}

	static AssetPoolObject()
	{
		_E028 = new DefaultObjectPool<List<AssetPoolObject>>(new _E000());
		_E028.Preallocate(10);
	}

	public static void ReturnToPool(GameObject gameObject, bool immediate = true)
	{
		if (gameObject == null)
		{
			_E760.Logger.LogError(_ED3E._E000(208885));
			return;
		}
		AssetPoolObject component = gameObject.GetComponent<AssetPoolObject>();
		if (component == null)
		{
			_E002(gameObject, immediate);
		}
		else
		{
			ReturnToPool(component, immediate);
		}
	}

	public static void ReturnToPool(AssetPoolObject assetPoolObject, bool immediate = true)
	{
		if (assetPoolObject != null)
		{
			if (!assetPoolObject.IsStub)
			{
				_E001(assetPoolObject);
				_E000(assetPoolObject.gameObject, assetPoolObject, immediate);
			}
			else
			{
				_E000(assetPoolObject.gameObject, assetPoolObject, immediate);
				_E002(assetPoolObject.gameObject, immediate);
			}
		}
	}

	private static void _E000(GameObject gameObject, AssetPoolObject parentAssetPoolObject, bool immediate)
	{
		if (gameObject == null)
		{
			return;
		}
		List<AssetPoolObject> list = _E028.Get();
		gameObject.GetComponentsInChildren(includeInactive: true, list);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			AssetPoolObject assetPoolObject = list[num];
			if (!(assetPoolObject == parentAssetPoolObject))
			{
				if (assetPoolObject.IsStub)
				{
					_E002(assetPoolObject.gameObject, immediate);
				}
				else
				{
					_E001(assetPoolObject);
				}
			}
		}
		_E028.Return(list);
	}

	private static void _E001(AssetPoolObject assetPoolObject)
	{
		if (assetPoolObject._E02B != null)
		{
			if (!assetPoolObject._E02B.IsWillBeDestroyedByParent)
			{
				if (!assetPoolObject.IsInPool)
				{
					assetPoolObject.ReturnToPool();
					return;
				}
				_E760.Logger.LogWarnFields(_ED3E._E000(206908), assetPoolObject.Name, _ED3E._E000(206891), assetPoolObject.GetHashCode());
			}
			return;
		}
		AssetPoolObject[] components = assetPoolObject.GetComponents<AssetPoolObject>();
		_E760.Logger.LogErrorFields(_ED3E._E000(206886), assetPoolObject.Name, _ED3E._E000(206963), components.Length > 1, _ED3E._E000(206891), assetPoolObject.GetHashCode());
		if (components.Length > 1)
		{
			Object.DestroyImmediate(assetPoolObject);
		}
		else
		{
			_E002(assetPoolObject.gameObject, immediate: true);
		}
	}

	private static void _E002(GameObject gameObject, bool immediate)
	{
		if (!(gameObject == null))
		{
			_E760.Logger.LogTraceFields(_ED3E._E000(206996), gameObject.name);
			if (immediate)
			{
				Object.DestroyImmediate(gameObject);
			}
			else
			{
				Object.Destroy(gameObject);
			}
		}
	}

	public void StoreCollider(Collider coll)
	{
		RegisteredCollidersToDisable.Add(new _E001
		{
			Collider = coll,
			Layer = coll.gameObject.layer,
			WasEnabled = coll.enabled
		});
	}

	public virtual void OnGetFromPool()
	{
		foreach (_EC3C component in Components)
		{
			component.OnGetFromPool(this);
		}
		IsInPool = false;
	}

	protected virtual void ReturnToPool()
	{
		base.transform.localScale = Vector3.one;
		if (IsStub)
		{
			return;
		}
		foreach (_EC3C component in Components)
		{
			component.OnReturnToPool(this);
			component.enabled = false;
		}
		foreach (Component item in RegisteredComponentsToClean)
		{
			if (item is Rigidbody)
			{
				Object.Destroy(item);
			}
			else
			{
				Object.DestroyImmediate(item);
			}
		}
		RegisteredComponentsToClean.Clear();
		foreach (_E001 item2 in RegisteredCollidersToDisable)
		{
			item2.Reset();
		}
		RegisteredCollidersToDisable.Clear();
		if (_E02B == null)
		{
			_E760.Logger.LogWarn(_ED3E._E000(207033));
			GameObject obj = base.gameObject;
			obj.SetActive(value: false);
			Object.Destroy(obj);
			return;
		}
		DisposeContainerCollectionView();
		if (_E02B.IsDisposed)
		{
			_E760.Logger.LogError(_ED3E._E000(207068));
			return;
		}
		_E02B.Push(this);
		IsInPool = true;
	}

	private void _E003()
	{
		Components = base.gameObject.GetComponentsInChildren<_EC3C>().ToList();
	}

	public void RegisterComponent(_EC3C component)
	{
		Components.Add(component);
	}

	public void UnregisterComponent(_EC3C component)
	{
		Components.Remove(component);
	}

	public virtual void Init(_E54C resourceType, bool isStub = false)
	{
		ResourceType = resourceType;
		IsStub = isStub;
	}

	public virtual void OnCreatePoolRoleModel()
	{
		_E003();
		foreach (_EC3C component in Components)
		{
			component.enabled = false;
			component.OnCreatePoolRoleModel(this);
		}
		Collider[] componentsInChildren = base.gameObject.GetComponentsInChildren<Collider>(includeInactive: true);
		foreach (Collider collider in componentsInChildren)
		{
			if (!collider.isTrigger)
			{
				Colliders.Add(collider);
			}
		}
		IsInPool = true;
	}

	public virtual void InheritRoleModelData<TAssetPoolObject>(TAssetPoolObject roleModel) where TAssetPoolObject : AssetPoolObject
	{
		_E003();
		for (int i = 0; i < Components.Count; i++)
		{
			_EC3C obj = Components[i];
			_EC3C roleModel2 = roleModel.Components[i];
			obj.InheritRoleModelData(roleModel2);
		}
	}

	public virtual void OnCreatePoolObject<TAssetPoolObject>([CanBeNull] _EC3A<TAssetPoolObject> assetsPoolParent) where TAssetPoolObject : AssetPoolObject
	{
		_E02B = assetsPoolParent;
		foreach (_EC3C component in Components)
		{
			component.OnCreatePoolObject(this);
		}
		IsInPool = true;
	}

	public void Destroy<TAssetPoolObject>(_EC3A<TAssetPoolObject> ownerAssetsPool) where TAssetPoolObject : AssetPoolObject
	{
		if (ownerAssetsPool == _E02B)
		{
			_E02C = true;
			Object.Destroy(base.gameObject);
		}
	}

	protected virtual void OnDestroy()
	{
		DisposeContainerCollectionView();
	}

	public List<Collider> GetColliders(bool includeNestedAssetPoolObjects)
	{
		if (includeNestedAssetPoolObjects)
		{
			AssetPoolObject[] componentsInChildren = base.gameObject.GetComponentsInChildren<AssetPoolObject>(includeInactive: true);
			if (componentsInChildren.Length != 0)
			{
				List<Collider> list = new List<Collider>();
				list.AddRange(Colliders);
				AssetPoolObject[] array = componentsInChildren;
				foreach (AssetPoolObject assetPoolObject in array)
				{
					list.AddRange(assetPoolObject.Colliders);
				}
				return list;
			}
		}
		return Colliders;
	}

	public void DisposeContainerCollectionView()
	{
		if (ContainerCollectionView != null)
		{
			ContainerCollectionView.Dispose();
			ContainerCollectionView = null;
		}
	}

	public void CollectOriginallyEnabledComponents()
	{
		_originallyEnabledComponents.Clear();
		Component[] componentsInChildren = GetComponentsInChildren<Component>(includeInactive: true);
		foreach (Component component in componentsInChildren)
		{
			if (component.IsEnabledUniversal())
			{
				_originallyEnabledComponents.Add(component);
			}
		}
	}

	public void SetOriginalComponentsEnabled(bool enabled)
	{
		for (int i = 0; i < _originallyEnabledComponents.Count; i++)
		{
			Component component = _originallyEnabledComponents[i];
			if (component != null)
			{
				component.SetEnabledUniversal(enabled);
			}
		}
	}
}
