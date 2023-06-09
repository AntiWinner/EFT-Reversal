using UnityEngine;

[DisallowMultipleComponent]
public class UsableHandsPrefab : WeaponPrefab
{
	[Header("Bone name for item")]
	public string UsableItemBoneName = _ED3E._E000(63976);

	public Transform ItemSpawnTransform;

	protected override void CacheInternalObjects()
	{
		base.CacheInternalObjects();
		ItemSpawnTransform = _E38B.FindTransformRecursive(base.transform, UsableItemBoneName);
	}
}
