using UnityEngine;

namespace EFT.Animations;

public class OneOffWeaponSettings : MonoBehaviour
{
	[SerializeField]
	private GameObject _ammo;

	[SerializeField]
	private bool _canAim;

	public GameObject SpawneAmmo => _ammo;

	public bool CanAim => _canAim;
}
