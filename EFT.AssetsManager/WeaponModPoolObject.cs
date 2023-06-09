using EFT.CameraControl;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.AssetsManager;

[DisallowMultipleComponent]
public class WeaponModPoolObject : AssetPoolObject
{
	private OpticSight[] _E027;

	public override void OnCreatePoolObject<TAssetPoolObject>([CanBeNull] _EC3A<TAssetPoolObject> assetsPoolParent)
	{
		base.OnCreatePoolObject(assetsPoolParent);
		_E41B.CreateRainCondensators(base.gameObject, enabled: false, cleanOld: false);
		if (ResourceType.ItemTemplate is _E9E1)
		{
			base.gameObject.GetOrAddComponent<TacticalComboVisualController>().Init();
		}
		if (ResourceType.ItemTemplate is _E9E9)
		{
			base.gameObject.GetOrAddComponent<SightModVisualControllers>().Init();
			_E027 = base.gameObject.GetComponentsInChildren<OpticSight>(includeInactive: true);
		}
		if (ResourceType.ItemTemplate is _EA61)
		{
			base.gameObject.GetOrAddComponent<LauncherViauslController>().Init();
		}
	}

	protected override void ReturnToPool()
	{
		if (_E027 != null)
		{
			OpticSight[] array = _E027;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
		}
		base.ReturnToPool();
	}
}
