using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using EFT.UI.Map;
using EFT.UI.Screens;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

[UsedImplicitly]
public sealed class InventoryScreen : EftScreen<InventoryScreen._E000, InventoryScreen>
{
	public enum EInventoryTab
	{
		Unchanged,
		Overall,
		Gear,
		Health,
		Skills,
		Map,
		Notes
	}

	public new class _E000 : _EC92._E000<_E000, InventoryScreen>
	{
		public readonly _E796 Session;

		public readonly _E9C4 HealthController;

		public readonly _EAED InventoryController;

		public readonly _E935 QuestController;

		public readonly _EA40[] LootItems;

		public readonly EInventoryTab InventoryTab;

		[CompilerGenerated]
		private new EItemViewType m__E000;

		public override EEftScreenType ScreenType => EEftScreenType.Inventory;

		public EItemViewType ViewType
		{
			[CompilerGenerated]
			get
			{
				return this.m__E000;
			}
			[CompilerGenerated]
			protected set
			{
				this.m__E000 = value;
			}
		}

		public virtual EItemUiContextType ContextType => EItemUiContextType.InventoryScreen;

		public virtual Profile Profile => Session.Profile;

		public virtual bool InRaid => false;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Enabled;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Enabled;

		public _E000(_E796 session, _E9C4 healthController, _EAED inventoryController, _E935 questController, _EA40[] lootItems, EInventoryTab inventoryTab)
		{
			Session = session;
			HealthController = healthController;
			InventoryController = inventoryController;
			QuestController = questController;
			LootItems = lootItems;
			InventoryTab = inventoryTab;
			ViewType = EItemViewType.Inventory;
		}

		protected override async Task<bool> CloseScreenInterruption(bool moveForward)
		{
			bool flag = _E002 != null && _E002._E00D;
			if (flag)
			{
				flag = !(await _E002._sortingTable.TryClose());
			}
			if (flag)
			{
				return false;
			}
			if (!(await base.CloseScreenInterruption(moveForward)))
			{
				return false;
			}
			if (_E002._playBackpackSounds)
			{
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.BackpackClose);
			}
			return true;
		}

		public void ShowMap(MapComponent map)
		{
			if (_E002 != null && !base.Closed)
			{
				_E002._E000(map);
			}
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private Task<bool> _E000(bool moveForward)
		{
			return base.CloseScreenInterruption(moveForward);
		}
	}

	public class _E001 : _E000
	{
		[CompilerGenerated]
		private readonly Profile m__E001;

		public override Profile Profile
		{
			[CompilerGenerated]
			get
			{
				return m__E001;
			}
		}

		public override bool InRaid => true;

		protected override EStateSwitcher UnrestrictedFrameRate => EStateSwitcher.Enabled;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher EnvironmentOverlay => EStateSwitcher.Disabled;

		public _E001(_E796 session, Profile profile, _E9C4 healthController, _EAED inventoryController, _E935 questController, _EA40[] lootItems, EInventoryTab inventoryTab)
			: base(session, healthController, inventoryController, questController, lootItems, inventoryTab)
		{
			m__E001 = profile;
		}
	}

	public sealed class _E002 : _E000
	{
		protected override EStateSwitcher UnrestrictedFrameRate => EStateSwitcher.Disabled;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Enabled;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Enabled;

		protected override EStateSwitcher EnvironmentOverlay => EStateSwitcher.Disabled;

		public _E002(_E796 session, _E9C4 healthController, _EAED inventoryController, _E935 questController, _EA40[] lootItems, EInventoryTab inventoryTab)
			: base(session, healthController, inventoryController, questController, lootItems, inventoryTab)
		{
		}
	}

	[SerializeField]
	private SimpleStashPanel _simpleStashPanel;

	[SerializeField]
	private ComplexStashPanel _complexStashPanel;

	[SerializeField]
	private Tab _overallTab;

	[SerializeField]
	private Tab _gearTab;

	[SerializeField]
	private Tab _healthTab;

	[SerializeField]
	private Tab _skillsTab;

	[SerializeField]
	private Tab _mapTab;

	[SerializeField]
	private Tab _tasksTab;

	[SerializeField]
	private ItemsPanel _itemsPanel;

	[SerializeField]
	private MapScreen _mapScreen;

	[SerializeField]
	private TasksScreen _tasksScreen;

	[SerializeField]
	private SkillsAndMasteringScreen _skillsAndMasteringScreen;

	[SerializeField]
	private OverallScreen _overallScreen;

	[SerializeField]
	private bool _playBackpackSounds;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private SortingTableWindow _sortingTable;

	private new ItemUiContext m__E000;

	private _EC67 m__E001;

	private Dictionary<EInventoryTab, Tab> m__E002;

	private _EAED m__E003;

	private _E9C4 m__E004;

	private _EA40[] m__E005;

	private Profile m__E006;

	private _E796 m__E007;

	private _ECB1 _E008;

	private _E935 _E009;

	private _EC60 _E00A;

	private readonly Dictionary<EBodyModelPart, _EBE6> _E00B = new Dictionary<EBodyModelPart, _EBE6>();

	private Dictionary<EBodyModelPart, string> _E00C;

	private bool _E00D
	{
		get
		{
			if (_sortingTable != null)
			{
				return _sortingTable.IsVisible;
			}
			return false;
		}
	}

	[UsedImplicitly]
	private void Awake()
	{
		this.m__E002 = new Dictionary<EInventoryTab, Tab>
		{
			{
				EInventoryTab.Overall,
				_overallTab
			},
			{
				EInventoryTab.Gear,
				_gearTab
			},
			{
				EInventoryTab.Health,
				_healthTab
			},
			{
				EInventoryTab.Skills,
				_skillsTab
			},
			{
				EInventoryTab.Map,
				_mapTab
			},
			{
				EInventoryTab.Notes,
				_tasksTab
			}
		};
		this.m__E001 = new _EC67(this.m__E002.Values.ToArray(), _gearTab);
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_overallScreen.OnCustomizationChanged += _E003;
		_overallScreen.OnBackButtonClick += delegate
		{
			ScreenController.CloseScreen();
		};
		_itemsPanel.OnBackButtonClick += delegate
		{
			ScreenController.CloseScreen();
		};
		_tasksScreen.OnBackButtonClick += delegate
		{
			ScreenController.CloseScreen();
		};
	}

	private void _E000(MapComponent map)
	{
		_mapScreen.Init(map);
		this.m__E001.TryHide();
		this.m__E001.Show(_mapTab);
	}

	public override void Show(_E000 controller)
	{
		Show(controller.HealthController, controller.InventoryController, controller.QuestController, controller.LootItems, controller.InventoryTab, controller.Session);
	}

	private void Show(_E9C4 healthController, _EAED controller, _E935 questController, _EA40[] lootItems, EInventoryTab tab, _E796 session)
	{
		if (Singleton<_E90A>.Instantiated)
		{
			Singleton<_E90A>.Instance.PauseFrameRelatedMetrics(pause: true);
		}
		_E00A = null;
		this.m__E004 = healthController;
		this.m__E007 = session;
		this.m__E006 = ScreenController.Profile;
		this.m__E003 = controller;
		_E009 = questController;
		this.m__E005 = lootItems;
		_E008 = this.m__E007.InsuranceCompany;
		this.m__E000 = ItemUiContext.Instance;
		_E001();
		_EA40 obj = lootItems?.FirstOrDefault();
		if (obj is _EB0B item)
		{
			_complexStashPanel.Configure(this.m__E003, new _EB63<_EB0B>(item, ScreenController.ViewType), this.m__E006.Skills, _E008, this.m__E000);
			_E00A = _complexStashPanel;
		}
		else if (obj != null)
		{
			_simpleStashPanel.Configure(obj, this.m__E003, ScreenController.ViewType);
			_E00A = _simpleStashPanel;
		}
		_simpleStashPanel.OnSortingTableTabSelected += _E004;
		UI.Dispose();
		if (this.m__E003 is Player.PlayerInventoryController playerInventoryController)
		{
			playerInventoryController.SetNextProcessLocked(status: false);
			_E8A8.Instance.Blur(isActive: true);
		}
		ShowGameObject();
		StartCoroutine(_E002(tab, splitInFrames: true));
		_E00B.Clear();
		_E00C = new Dictionary<EBodyModelPart, string>(this.m__E006.Customization);
		if (_playBackpackSounds)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.BackpackOpen);
		}
	}

	private void _E001()
	{
		this.m__E000.Configure(this.m__E003, this.m__E006, this.m__E007, _E008, this.m__E007.RagFair, null, this.m__E004, this.m__E005, ScreenController.ContextType, ECursorResult.ShowCursor);
	}

	private IEnumerator _E002(EInventoryTab arg, bool splitInFrames)
	{
		CancellationToken cancellationToken = UI.CancellationToken;
		if (splitInFrames)
		{
			yield return null;
		}
		if (cancellationToken.IsCancellationRequested)
		{
			yield break;
		}
		_overallTab.Init(new OverallScreen._E000(_overallScreen, this.m__E006, ScreenController.InRaid ? this.m__E003 : null));
		_itemsPanel.SplitInFrames = splitInFrames;
		_gearTab.Init(new ItemsPanel._E000(_itemsPanel, _E00A, this.m__E003, this.m__E004, this.m__E006.Skills, _E008 ?? new _ECB1(null), _sortingTable, ItemsPanel.EItemsTab.Gear, ScreenController.ViewType));
		_healthTab.Init(new ItemsPanel._E000(_itemsPanel, _E00A, this.m__E003, this.m__E004, this.m__E006.Skills, _E008 ?? new _ECB1(null), null, ItemsPanel.EItemsTab.Health, ScreenController.ViewType));
		_skillsTab.Init(new SkillsAndMasteringScreen._E000(_skillsAndMasteringScreen, this.m__E006, this.m__E003, this.m__E004));
		_mapTab.Init(new MapScreen._E000(_mapScreen, this.m__E003, this.m__E005));
		_tasksTab.Init(new TasksScreen.TasksTabController(_tasksScreen, this.m__E003, _E009, this.m__E007, this.m__E006, this.m__E006.Notes));
		if (splitInFrames)
		{
			yield return null;
		}
		if (cancellationToken.IsCancellationRequested)
		{
			yield break;
		}
		this.m__E001.Show((arg == EInventoryTab.Unchanged) ? null : this.m__E002[arg]);
		if (splitInFrames)
		{
			yield return null;
			if (!cancellationToken.IsCancellationRequested)
			{
				_itemsPanel.SplitInFrames = false;
			}
		}
	}

	private void _E003(_EBE6 suite)
	{
		_E00B[suite.MainBodyPart] = suite;
	}

	private void _E004()
	{
		_EA98 sortingTable = this.m__E003.Inventory.SortingTable;
		_EB60 obj = this.m__E003 as _EB60;
		if (sortingTable == null || obj == null)
		{
			_simpleStashPanel.ChangeSortingTableTabState(isVisible: false);
			return;
		}
		_sortingTable.Show(new _EB63<_EA98>(sortingTable, ScreenController.ViewType), this.m__E007, obj, this.m__E000, _sortingTable.Close, delegate
		{
			_simpleStashPanel.ChangeSortingTableTabState(isVisible: false);
		});
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			if (_E00D)
			{
				_sortingTable.Close();
			}
			else
			{
				ScreenController.CloseScreen();
			}
			return ETranslateResult.BlockAll;
		}
		if (command.IsCommand(ECommand.ToggleInventory))
		{
			ScreenController.CloseScreen();
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	public override void Close()
	{
		if (_E00B.Count > 0)
		{
			string[] suites = _E00B.Select((KeyValuePair<EBodyModelPart, _EBE6> quipped) => quipped.Value.Id).ToArray();
			this.m__E007.ApplyCustomizationWear(suites, delegate(IResult response)
			{
				if (response.Failed)
				{
					this.m__E006.Customization = new _E72D(_E00C);
				}
			});
		}
		if (this.m__E003 is Player.PlayerInventoryController playerInventoryController)
		{
			playerInventoryController.SetNextProcessLocked(status: true);
			_E8A8.Instance.Blur(isActive: false);
		}
		this.m__E001?.TryHide().HandleExceptions();
		this.m__E003?.StopProcesses();
		base.Close();
		if (Singleton<_E90A>.Instantiated)
		{
			Singleton<_E90A>.Instance.PauseFrameRelatedMetrics(pause: false);
		}
		_simpleStashPanel.OnSortingTableTabSelected -= _E004;
		_complexStashPanel.UnConfigure();
		this.m__E007 = null;
		_E008 = null;
		this.m__E006 = null;
		this.m__E003 = null;
		this.m__E004 = null;
		_E009 = null;
		this.m__E005 = null;
	}

	[CompilerGenerated]
	private void _E005()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E006()
	{
		_simpleStashPanel.ChangeSortingTableTabState(isVisible: false);
	}

	[CompilerGenerated]
	private void _E007(IResult response)
	{
		if (response.Failed)
		{
			this.m__E006.Customization = new _E72D(_E00C);
		}
	}
}
