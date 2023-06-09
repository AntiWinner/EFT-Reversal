using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.Map;
using EFT.UI.Screens;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public sealed class MatchmakerMapPointsScreen : MatchmakerEftScreen<MatchmakerMapPointsScreen._E000, MatchmakerMapPointsScreen>
{
	public new sealed class _E000 : _EC8F<_E000, MatchmakerMapPointsScreen>
	{
		public readonly bool AllowSelection;

		public readonly MapPoints MapPoints;

		public readonly _E554.Location Location;

		public readonly _EAE6 ItemController;

		public override EEftScreenType ScreenType => EEftScreenType.MapPoints;

		public override bool KeyScreen => true;

		public _E000(bool allowSelection, MapPoints mapPoints, _E554.Location location, _EAE6 itemController, _EC99 matchmakerPlayersController)
			: base(matchmakerPlayersController)
		{
			AllowSelection = allowSelection;
			MapPoints = mapPoints;
			Location = location;
			ItemController = itemController;
		}
	}

	[SerializeField]
	private CustomTextMeshProUGUI _substring;

	[SerializeField]
	private CustomTextMeshProUGUI _warningDescription;

	[SerializeField]
	private GameObject[] _enableWhenNoMap;

	[SerializeField]
	private GameObject[] _enableWhenMap;

	[SerializeField]
	private GameObject _mapIcon;

	[SerializeField]
	private SelectEntryPointPanel _entryPointPanel;

	[SerializeField]
	private MapPointsManager _mapPointsManager;

	[SerializeField]
	private DefaultUIButton _acceptButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private DefaultUIButton _readyButton;

	[SerializeField]
	private SimplePocketMap _pocketMap;

	[SerializeField]
	private CustomToggle _legendButton;

	[SerializeField]
	private GameObject _legend;

	private new MapPoints m__E000;

	private _E554.Location m__E001;

	private readonly Dictionary<string, string> m__E002 = new Dictionary<string, string>
	{
		{
			_ED3E._E000(124548),
			_ED3E._E000(234448)
		},
		{
			_ED3E._E000(124512),
			_ED3E._E000(234489)
		},
		{
			_ED3E._E000(124565),
			_ED3E._E000(234489)
		},
		{
			_ED3E._E000(124522),
			_ED3E._E000(234466)
		},
		{
			_ED3E._E000(124547),
			_ED3E._E000(236555)
		},
		{
			_ED3E._E000(124597),
			_ED3E._E000(236588)
		},
		{
			_ED3E._E000(124530),
			_ED3E._E000(234466)
		}
	};

	private void Awake()
	{
		_acceptButton.OnClick.AddListener(delegate
		{
			((_EC90<_E000, MatchmakerMapPointsScreen>)ScreenController)._E000();
		});
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_readyButton.OnClick.AddListener(delegate
		{
			ScreenController._E000();
		});
		_legendButton.onValueChanged.AddListener(delegate
		{
			_legend.SetActive(!_legend.activeSelf);
		});
	}

	public override void Show(_E000 controller)
	{
		base.Show(controller);
		Show(controller.AllowSelection, controller.MapPoints, controller.Location, controller.ItemController);
	}

	private void Show(bool allowSelection, MapPoints mapPoints, _E554.Location location, _EAE6 itemController)
	{
		_substring.text = string.Format((allowSelection ? _ED3E._E000(234261) : _ED3E._E000(234225)).Localized(), (location._Id + _ED3E._E000(70087)).Localized().ToUpper());
		_warningDescription.text = (allowSelection ? _ED3E._E000(234359) : _ED3E._E000(234293)).Localized();
		this.m__E000 = mapPoints;
		this.m__E001 = location;
		ShowGameObject();
		_readyButton.gameObject.SetActive(value: true);
		MapComponent[] maps = itemController.Inventory.GetAllEquipmentItems().GetComponents<MapComponent>().ToArray();
		MapComponent mapComponent = _E000(maps);
		if (mapComponent != null && itemController.Examined(mapComponent.Item))
		{
			_pocketMap.Show(mapComponent);
			_mapPointsManager.Show(allowSelection, this.m__E000);
			_entryPointPanel.Show(allowSelection, this.m__E000.EntryPoints.ToArray(), delegate(EFT.UI.Map.EntryPoint arg)
			{
				_mapPointsManager.SelectPoint(arg);
			});
		}
		else
		{
			_entryPointPanel.Show(allowSelection, this.m__E000.EntryPoints.ToArray());
		}
		GameObject[] enableWhenNoMap = _enableWhenNoMap;
		for (int i = 0; i < enableWhenNoMap.Length; i++)
		{
			enableWhenNoMap[i].SetActive(mapComponent == null);
		}
		enableWhenNoMap = _enableWhenMap;
		for (int i = 0; i < enableWhenNoMap.Length; i++)
		{
			enableWhenNoMap[i].SetActive(mapComponent != null);
		}
		_mapIcon.SetActive(mapComponent != null);
	}

	[CanBeNull]
	private MapComponent _E000(IEnumerable<MapComponent> maps)
	{
		if (this.m__E002.ContainsKey(this.m__E001.Id))
		{
			return maps.FirstOrDefault((MapComponent map) => this.m__E002[this.m__E001.Id] == map.Item.TemplateId);
		}
		return null;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			ScreenController.CloseScreen();
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	public override void Close()
	{
		_pocketMap.Close();
		_mapPointsManager.Close();
		_entryPointPanel.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E001()
	{
		((_EC90<_E000, MatchmakerMapPointsScreen>)ScreenController)._E000();
	}

	[CompilerGenerated]
	private void _E002()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E003()
	{
		ScreenController._E000();
	}

	[CompilerGenerated]
	private void _E004(bool arg)
	{
		_legend.SetActive(!_legend.activeSelf);
	}

	[CompilerGenerated]
	private void _E005(EFT.UI.Map.EntryPoint arg)
	{
		_mapPointsManager.SelectPoint(arg);
	}

	[CompilerGenerated]
	private bool _E006(MapComponent map)
	{
		return this.m__E002[this.m__E001.Id] == map.Item.TemplateId;
	}
}
