using System.Collections.Generic;
using UnityEngine;

namespace EFT;

public class CorpseRagdollTestApplication : AbstractApplication
{
	[SerializeField]
	private GameObject _corpsePrefab;

	[SerializeField]
	private GameObject _weaponPrefab;

	private List<CorpseRagdollTest> _E003 = new List<CorpseRagdollTest>();

	[SerializeField]
	private Vector3 _velocity;

	[SerializeField]
	private float _maxDepenetrationVelocity = 2f;

	[SerializeField]
	private CollisionDetectionMode _collisionDetectionMode = CollisionDetectionMode.Continuous;

	[SerializeField]
	private bool _keepRigidbody;

	[SerializeField]
	private bool _putToSleep;

	[SerializeField]
	private bool _dropWeapon = true;

	[SerializeField]
	private bool _weaponFixedHinge;

	protected override EUpdateQueue PlayerUpdateQueue => EUpdateQueue.Update;

	protected override _E316 CreateLogConfigurator()
	{
		return _E778.Create();
	}

	public void SpawnCorpse(int count = 1)
	{
		for (int i = 0; i < count; i++)
		{
			CorpseRagdollTest corpseRagdollTest = Object.Instantiate(_corpsePrefab, base.transform.position, base.transform.rotation).AddComponent<CorpseRagdollTest>();
			corpseRagdollTest.Init();
			_E003.Add(corpseRagdollTest);
		}
	}

	public void DropAll()
	{
		foreach (CorpseRagdollTest item in _E003)
		{
			item.Drop(item.transform.TransformVector(_velocity), _maxDepenetrationVelocity, _collisionDetectionMode, _keepRigidbody, _putToSleep);
			if (_dropWeapon && _weaponPrefab != null)
			{
				GameObject gameObject = Object.Instantiate(_weaponPrefab);
				Rigidbody weaponRigidbody = gameObject.AddComponent<Rigidbody>();
				Collider[] componentsInChildren = gameObject.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = true;
				}
				int layer = LayerMask.NameToLayer(_ED3E._E000(55338));
				_E38B.SetLayersRecursively(gameObject, layer);
				item.Ragdoll.AttachWeapon(weaponRigidbody, item.gameObject, item.gameObject.GetComponentInChildren<PlayerBones>(), gameObject.GetComponentInChildren<TransformLinks>(), _weaponFixedHinge, _velocity);
			}
		}
		_E003.Clear();
	}

	public void ResetAVGSimulationDeltaTime()
	{
		_E320._E001.SetAVGSimulationDeltaTime(Time.deltaTime);
	}

	private void LateUpdate()
	{
		_E320.SyncTransforms();
	}
}
