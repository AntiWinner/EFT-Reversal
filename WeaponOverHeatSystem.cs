using System.Collections.Generic;
using Systems.Effects;
using Comfort.Common;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class WeaponOverHeatSystem : ComponentSystem<WeaponPrefab, WeaponOverHeatSystem>
{
	protected override bool HasUpdate => true;

	protected override bool HasLateUpdate => true;

	protected override void LateUpdate()
	{
		if (Singleton<Effects>.Instantiated && _E8A8.Exist && !(_E8A8.Instance.Camera == null) && EFTHardSettings.Instance.HEAT_EMITTER_ENABLED)
		{
			base.LateUpdate();
		}
	}

	protected override void UpdateComponent(WeaponPrefab component)
	{
		component.ManualUpdate();
	}

	protected override void LateUpdateComponent(WeaponPrefab component)
	{
		bool flag = HotObject.NeedProcessEffects(_E8A8.Instance.Camera.transform.position, component.transform.position);
		List<HotObject> hotObjects = component.HotObjects;
		for (int i = 0; i < hotObjects.Count; i++)
		{
			HotObject hotObject = hotObjects[i];
			if (hotObject.UseHeatHaze && flag)
			{
				hotObject.ManualSyncEffects();
				hotObject.SyncPosition();
			}
		}
	}
}
