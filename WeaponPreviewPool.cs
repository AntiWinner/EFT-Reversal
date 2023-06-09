using System.Collections.Generic;
using System.Linq;
using EFT.UI.WeaponModding;
using JetBrains.Annotations;
using UnityEngine;

public class WeaponPreviewPool : MonoBehaviour
{
	[SerializeField]
	private WeaponPreview _weaponPreviewTemplate;

	[SerializeField]
	private int _count = 8;

	private readonly List<WeaponPreview> _E000 = new List<WeaponPreview>();

	private void Awake()
	{
		for (int i = 0; i < _count; i++)
		{
			WeaponPreview weaponPreview = Object.Instantiate(_weaponPreviewTemplate, base.transform, worldPositionStays: false);
			weaponPreview.name = string.Format(_ED3E._E000(49047), i + 1);
			weaponPreview.transform.localPosition = new Vector3(0f, 10f * (float)(i + 1), 0f);
			_E000.Add(weaponPreview);
			weaponPreview.gameObject.SetActive(value: false);
		}
	}

	[CanBeNull]
	public WeaponPreview GetWeaponPreview()
	{
		WeaponPreview weaponPreview = _E000.FirstOrDefault();
		if (weaponPreview == null)
		{
			Debug.LogError(_ED3E._E000(49025));
			return null;
		}
		_E000.Remove(weaponPreview);
		weaponPreview.Init();
		return weaponPreview;
	}

	public void ReturnToPool(WeaponPreview weaponPreview)
	{
		weaponPreview.gameObject.SetActive(value: false);
		_E000.Add(weaponPreview);
	}
}
