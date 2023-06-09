using System.Collections.Generic;
using UnityEngine;

namespace EFT.AssetsManager;

public class PoolContainerObject : MonoBehaviour
{
	[SerializeField]
	private GameObject _originGameObject;

	public List<AssetPoolObject> PoolObjects = new List<AssetPoolObject>(20);

	public GameObject OriginGameObject
	{
		get
		{
			return _originGameObject;
		}
		set
		{
			_originGameObject = value;
		}
	}

	public void AddToContainer(AssetPoolObject poolObject)
	{
		if (!(this == null))
		{
			PoolObjects.Add(poolObject);
			poolObject.transform.SetParent(base.transform);
		}
	}

	public void RemoveFromContainer(AssetPoolObject poolObject)
	{
		PoolObjects.Remove(poolObject);
	}
}
