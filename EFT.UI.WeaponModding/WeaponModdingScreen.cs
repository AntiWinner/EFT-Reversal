using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.WeaponModding;

public sealed class WeaponModdingScreen : ItemObserveScreen<WeaponModdingScreen._E000, WeaponModdingScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, WeaponModdingScreen>
	{
		public readonly Item Item;

		public readonly _EAED InventoryController;

		public readonly _EA40[] Collections;

		public override EEftScreenType ScreenType => EEftScreenType.WeaponModding;

		protected override EStateSwitcher EnvironmentOverlay => EStateSwitcher.Disabled;

		public _E000(Item item, _EAED inventoryController, _EA40[] collections)
		{
			Item = item;
			InventoryController = inventoryController;
			Collections = collections;
		}
	}

	[SerializeField]
	private CustomTextMeshProUGUI _headerLabel;

	[SerializeField]
	private Toggle _allClassesToggle;

	[SerializeField]
	private GameObject _warningPanel;

	[SerializeField]
	private LocalizedText _warningLabel;

	private new _EA40[] m__E000;

	protected override void Awake()
	{
		base.Awake();
		_allClassesToggle.onValueChanged.AddListener(delegate
		{
			ToggleChangedHandler();
			UpdatePositions();
		});
	}

	public override void Show(_E000 controller)
	{
		Show(controller.Item, controller.InventoryController, controller.Collections);
	}

	private void Show(Item item, _EAED inventoryController, _EA40[] collections)
	{
		this.m__E000 = collections;
		Show(item, inventoryController);
		UpdateItem(item);
		bool flag = Item is Weapon || Item is Mod;
		Toggle[] modClassToggles = ModClassToggles;
		for (int i = 0; i < modClassToggles.Length; i++)
		{
			modClassToggles[i].gameObject.SetActive(flag);
		}
		_allClassesToggle.gameObject.SetActive(!flag);
		_headerLabel.text = ((item is Weapon) ? _ED3E._E000(257841).Localized() : _ED3E._E000(257795).Localized());
	}

	protected override _EB6A CreateBuildManipulation()
	{
		return new _EB6C(base.InventoryController, this.m__E000);
	}

	protected override void ToggleChangedHandler()
	{
		VisibleModClasses = ((!_allClassesToggle.gameObject.activeSelf) ? ((ModClassToggles[0].isOn ? EModClass.Master : EModClass.None) | (ModClassToggles[1].isOn ? EModClass.Functional : EModClass.None) | (ModClassToggles[2].isOn ? EModClass.Gear : EModClass.None)) : (_allClassesToggle.isOn ? EModClass.All : EModClass.None));
		foreach (ModdingScreenSlotView value in ModIcons.Values)
		{
			value.CheckVisibility(VisibleModClasses);
		}
	}

	protected override void CheckForVitalParts()
	{
		_warningPanel.SetActive(value: false);
		if (Item is _EA40 obj)
		{
			Slot slot = obj.MissingVitalParts.LastOrDefault();
			if (slot != null)
			{
				_warningPanel.SetActive(value: true);
				_warningLabel.SetFormatValues(slot.ID.Localized());
			}
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape) && !ScreenController.Closed)
		{
			ScreenController.CloseScreen();
			return ETranslateResult.Block;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	[CompilerGenerated]
	private void _E000(bool x)
	{
		ToggleChangedHandler();
		UpdatePositions();
	}
}
