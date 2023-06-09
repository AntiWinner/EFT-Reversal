using UnityEngine;

namespace CW2;

public class AdditiveMeshBaker : MonoBehaviour
{
	public bool SaveCollider;

	public bool DontDesttroyComponents = true;

	[HideInInspector]
	public Rigidbody Rigidbody;

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (Rigidbody != null && !Rigidbody.isKinematic && Rigidbody.IsSleeping())
		{
			_E44F.Add(base.gameObject, SaveCollider, DontDesttroyComponents);
		}
	}
}
