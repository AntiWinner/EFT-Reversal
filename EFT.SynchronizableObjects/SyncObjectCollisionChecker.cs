using UnityEngine;

namespace EFT.SynchronizableObjects;

public class SyncObjectCollisionChecker : MonoBehaviour
{
	private SynchronizableObject _E000;

	private void Awake()
	{
		_E000 = base.transform.parent.GetComponent<SynchronizableObject>();
	}

	public void OnTriggerEnter(Collider collider)
	{
		_E000.CollisionEnter(collider);
	}
}
