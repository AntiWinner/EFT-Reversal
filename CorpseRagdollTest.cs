using System.Collections.Generic;
using EFT.AssetsManager;
using RootMotion.FinalIK;
using UnityEngine;

public class CorpseRagdollTest : MonoBehaviour
{
	public _EBFC Ragdoll;

	public void Init()
	{
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		IK[] componentsInChildren2 = GetComponentsInChildren<IK>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].enabled = false;
		}
		int layer = LayerMask.NameToLayer(_ED3E._E000(55338));
		_E38B.SetLayersRecursively(base.gameObject, layer);
	}

	public void Drop(Vector3 velocity, float maxDepenetrationVelocity, CollisionDetectionMode collisionDetectionMode, bool keepRigidbody, bool putToSleep)
	{
		CharacterJointSpawner[] componentsInChildren = base.gameObject.GetComponentsInChildren<CharacterJointSpawner>();
		RigidbodySpawner[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<RigidbodySpawner>();
		List<PlayerRigidbodySleepHierarchy> rigidbodySleepHierarchy = PlayerPoolObject.CreatePlayerRigidbodySleepHierarchy(componentsInChildren2);
		Ragdoll = new _EBFC(componentsInChildren2, componentsInChildren, rigidbodySleepHierarchy, velocity, maxDepenetrationVelocity, collisionDetectionMode, this, _E000, null, _E001, keepRigidbody, putToSleep);
	}

	private bool _E000(bool sleeping, float timePass)
	{
		if (!sleeping)
		{
			return timePass >= 15f;
		}
		return true;
	}

	private void _E001()
	{
		base.gameObject.name += _ED3E._E000(55331);
	}
}
