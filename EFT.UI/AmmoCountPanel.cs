using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI;

[RequireComponent(typeof(BattleUIComponentAnimation))]
public sealed class AmmoCountPanel : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _ammoCount;

	[SerializeField]
	private CustomTextMeshProUGUI _ammoDetails;

	private BattleUIComponentAnimation _E093;

	private static readonly string[] _E0B9 = new string[4]
	{
		_ED3E._E000(250716),
		_ED3E._E000(250705),
		_ED3E._E000(250688),
		_ED3E._E000(250747)
	};

	public void ShowFireMode(Weapon.EFireMode fireMode)
	{
		Show(fireMode.ToString().Localized());
	}

	public void Show(string message, string details = null)
	{
		ShowGameObject();
		if (_E093 == null)
		{
			_E093 = base.gameObject.GetComponent<BattleUIComponentAnimation>();
		}
		_ammoCount.text = message;
		_ammoDetails.gameObject.SetActive(details != null);
		_ammoDetails.text = details;
		_E093.Show(autoHide: true).HandleExceptions();
	}

	public static string GetAmmoCountByLevel(int ammoCount, int maxAmmoCount, int level)
	{
		if (ammoCount >= maxAmmoCount - 1)
		{
			return _ED3E._E000(250683).Localized();
		}
		if (ammoCount == 0)
		{
			return _ED3E._E000(104396).Localized();
		}
		switch (level)
		{
		case 0:
			return _E0B9[(int)(3.33f * (float)ammoCount / (float)maxAmmoCount + 0.5f)].Localized();
		case 1:
			if (maxAmmoCount <= 10)
			{
				return ammoCount.ToString();
			}
			if (ammoCount >= 5)
			{
				return _ED3E._E000(250672).Localized() + _ED3E._E000(18502) + (int)((float)ammoCount / 5f + 0.5f) * 5;
			}
			return _ED3E._E000(250664).Localized();
		default:
			return ammoCount.ToString();
		}
	}

	public static string GetAmmoCountByLevelForFoldingMechanismWeapon(int ammoCount, int maxAmmoCount)
	{
		if (ammoCount >= maxAmmoCount)
		{
			return _ED3E._E000(250683).Localized();
		}
		if (ammoCount == 0)
		{
			return _ED3E._E000(104396).Localized();
		}
		return ammoCount.ToString();
	}

	public void Hide()
	{
		if (_E093 != null)
		{
			_E093.Hide().HandleExceptions();
		}
	}

	public override void Close()
	{
		if (_E093 != null)
		{
			_E093.Close();
		}
		base.Close();
	}
}
