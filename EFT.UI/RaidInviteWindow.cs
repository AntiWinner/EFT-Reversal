using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.Matchmaker;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class RaidInviteWindow : DialogWindow<_EC7B>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public RaidInviteWindow _003C_003E4__this;

		public string[] accessKeys;

		internal bool _E000(Item equippedItem)
		{
			if (_003C_003E4__this._E002.Examined(equippedItem))
			{
				return accessKeys.Contains(equippedItem.TemplateId);
			}
			return false;
		}
	}

	[SerializeField]
	private TextMeshProUGUI _description;

	[SerializeField]
	private TextMeshProUGUI _locationName;

	[SerializeField]
	private LocationConditionsPanel _locationConditionsPanel;

	[SerializeField]
	private GameObject _footer;

	[SerializeField]
	private Image _redLight;

	[SerializeField]
	private TextMeshProUGUI _footerText;

	private ChatSpecialIconSettings m__E000;

	private RaidSettings m__E001;

	private _EB61 _E002;

	public _EC7B Show(_E796 session, _E550 player, RaidSettings raidSettings, _EB61 inventoryController)
	{
		UIEventSystem.Instance.Enable();
		_EC7B result = Show(null, null, delegate
		{
		});
		this.m__E001 = raidSettings;
		_E002 = inventoryController;
		if (this.m__E000 == null)
		{
			this.m__E000 = _E3A2.Load<ChatSpecialIconSettings>(_ED3E._E000(250600));
		}
		_E54F info = player.Info;
		string arg = _E5AE.SetColor(color: this.m__E000.GetDataByMemberCategory(info.MemberCategory).IconColor, s: info.Nickname);
		_description.SetText(string.Format(_ED3E._E000(255670).Localized(), arg, raidSettings.IsPmc ? _ED3E._E000(255697).Localized() : _ED3E._E000(255708).Localized()));
		_locationConditionsPanel.Set(session, raidSettings, takeFromCurrent: false);
		_locationName.text = (raidSettings.SelectedLocation._Id + _ED3E._E000(70087)).Localized();
		string names;
		bool flag = _E001(out names);
		_acceptButton.Interactable = flag;
		if (!flag)
		{
			_footerText.SetText(string.Format(_ED3E._E000(255685).Localized(), _ED3E._E000(103088) + names + _ED3E._E000(59467)));
		}
		_E000(!flag);
		return result;
	}

	private void _E000(bool active)
	{
		_redLight.gameObject.SetActive(active);
		_footer.gameObject.SetActive(active);
	}

	private bool _E001(out string names)
	{
		names = string.Empty;
		string[] accessKeys = this.m__E001.SelectedLocation?.AccessKeys;
		if (accessKeys == null || accessKeys.Length == 0 || this.m__E001.IsScav)
		{
			return true;
		}
		IEnumerable<Item> allEquipmentItems = _E002.Inventory.GetAllEquipmentItems();
		names = string.Join(_ED3E._E000(255730).Localized(), this.m__E001.SelectedLocation?.AccessKeys.Select((string item) => item.LocalizedName()));
		return allEquipmentItems?.FirstOrDefault((Item equippedItem) => _E002.Examined(equippedItem) && accessKeys.Contains(equippedItem.TemplateId)) != null;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		axes = null;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		base.TranslateCommand(command);
		return ETranslateResult.Block;
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.ShowCursor;
	}
}
