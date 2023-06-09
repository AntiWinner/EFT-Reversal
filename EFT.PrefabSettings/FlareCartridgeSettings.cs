using System.Collections.Generic;
using UnityEngine;

namespace EFT.PrefabSettings;

public sealed class FlareCartridgeSettings : ThrowableSettings
{
	[SerializeField]
	private GameObject _projectileAmmo;

	[SerializeField]
	private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();

	[SerializeField]
	private float _succesfullHeight = 35f;

	[SerializeField]
	private FlareColorType _flareColorType;

	[SerializeField]
	private FlareEventType _flareEventType;

	[SerializeField]
	private float _initialSpeed;

	[SerializeField]
	private float _flareTimeAfterStart;

	[SerializeField]
	private float _flareLifetime;

	[SerializeField]
	private float _weight;

	[SerializeField]
	private float _rigidbodyDrag;

	[SerializeField]
	private float _collisionDrag;

	[SerializeField]
	private GameObject _flareEffectPrefab;

	public GameObject ProjectileAmmo => _projectileAmmo;

	public float SuccesfullHeight => _succesfullHeight;

	public FlareColorType FlareColorType => _flareColorType;

	public FlareEventType FlareEventType => _flareEventType;

	public float InitialSpeed => _initialSpeed;

	public float FlareTimeAfterStart => _flareTimeAfterStart;

	public float FlareLifetime => _flareLifetime;

	public float Weight => _weight;

	public float RigidbodyDrag => _rigidbodyDrag;

	public float CollisionDrag => _collisionDrag;

	public GameObject FlareEffectPrefab => _flareEffectPrefab;

	public void SwitchMeshRenderersActive(bool value)
	{
		foreach (MeshRenderer meshRenderer in _meshRenderers)
		{
			meshRenderer.enabled = value;
		}
	}
}
