using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.UI;
using TMPro;
using UnityEngine;

namespace EFT.Hideout;

public sealed class RequirementsPanel : AbstractPanel<List<Requirement>>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public RequirementsPanel _003C_003E4__this;

		public ItemUiContext itemUiContext;

		internal bool _E000()
		{
			switch (_003C_003E4__this.AreaData.Status)
			{
			case EAreaStatus.Constructing:
			case EAreaStatus.ReadyToInstallConstruct:
			case EAreaStatus.Upgrading:
			case EAreaStatus.ReadyToInstallUpgrade:
			case EAreaStatus.AutoUpgrading:
				return true;
			default:
				return false;
			}
		}

		internal void _E001(Requirement requirement, _E83B view)
		{
			view.Show(itemUiContext, _003C_003E4__this.Player._E0DE, requirement, _003C_003E4__this.AreaData.Template.Type, _E000());
		}
	}

	private const string m__E000 = "LEVEL {0} UPGRADE REQUIREMENTS";

	private const string m__E001 = "NEXT LEVEL UPGRADE REQUIREMENTS";

	private const string _E002 = "CONSTRUCT REQUIREMENTS";

	[SerializeField]
	private TextMeshProUGUI _requirementsLabel;

	[SerializeField]
	private AreaRequirementPanel _areaRequirementPanel;

	[SerializeField]
	private ItemRequirementPanel _itemRequirementPanel;

	[SerializeField]
	private TraderRequirementPanel _traderRequirementPanel;

	[SerializeField]
	private SkillRequirementPanel _skillRequirementPanel;

	[SerializeField]
	private RectTransform _areaContainer;

	[SerializeField]
	private RectTransform _itemContainer;

	[SerializeField]
	private RectTransform _traderContainer;

	[SerializeField]
	private RectTransform _skillContainer;

	private _EC6D<Requirement, _E83B> _E003;

	public override async Task ShowContents()
	{
		_E000 CS_0024_003C_003E8__locals0 = new _E000();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		_areaContainer.gameObject.SetActive(value: false);
		_itemContainer.gameObject.SetActive(value: false);
		_traderContainer.gameObject.SetActive(value: false);
		_skillContainer.gameObject.SetActive(value: false);
		IOrderedEnumerable<Requirement> items = base.Info.OrderBy((Requirement x) => x.Type);
		int num = base.AreaData.CurrentLevel + 1;
		_requirementsLabel.text = _ED3E._E000(165835).Localized();
		if (num > 1)
		{
			_requirementsLabel.text = (base.AreaData.DisplayLevel ? string.Format(_ED3E._E000(163858).Localized(), num.LevelFormat()) : _ED3E._E000(165874).Localized());
		}
		_E003 = UI.AddDisposable(new _EC6D<Requirement, _E83B>());
		CS_0024_003C_003E8__locals0.itemUiContext = ItemUiContext.Instance;
		await _E003.InitAsync(items, _E000, _E001, delegate(Requirement requirement, _E83B view)
		{
			view.Show(CS_0024_003C_003E8__locals0.itemUiContext, CS_0024_003C_003E8__locals0._003C_003E4__this.Player._E0DE, requirement, CS_0024_003C_003E8__locals0._003C_003E4__this.AreaData.Template.Type, CS_0024_003C_003E8__locals0._E000());
		});
	}

	public override void SetInfo()
	{
		_E003?.Dispose();
	}

	private _E83B _E000(Requirement requirement)
	{
		switch (requirement.Type)
		{
		case ERequirementType.Area:
			return _areaRequirementPanel;
		case ERequirementType.Item:
			return _itemRequirementPanel;
		case ERequirementType.TraderUnlock:
		case ERequirementType.TraderLoyalty:
			return _traderRequirementPanel;
		case ERequirementType.Skill:
			return _skillRequirementPanel;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private Transform _E001(Requirement requirement)
	{
		switch (requirement.Type)
		{
		case ERequirementType.Area:
			_areaContainer.gameObject.SetActive(value: true);
			return _areaContainer;
		case ERequirementType.Item:
			_itemContainer.gameObject.SetActive(value: true);
			return _itemContainer;
		case ERequirementType.TraderUnlock:
		case ERequirementType.TraderLoyalty:
			_traderContainer.gameObject.SetActive(value: true);
			return _traderContainer;
		case ERequirementType.Skill:
			_skillContainer.gameObject.SetActive(value: true);
			return _skillContainer;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}
}
