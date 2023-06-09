using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using Diz.Binding;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.Ragfair;
using EFT.UI.Screens;
using EFT.UI.WeaponBuilds;
using EFT.UI.WeaponModding;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class EditBuildScreen : ItemObserveScreen<EditBuildScreen._E000, EditBuildScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, EditBuildScreen>
	{
		public readonly _EAED InventoryController;

		public readonly _E796 Session;

		private new bool m__E000;

		private Item _E001;

		public Item BuildItem;

		public override EEftScreenType ScreenType => EEftScreenType.EditBuild;

		public Profile Profile => Session.Profile;

		public Item Item
		{
			get
			{
				return _E001;
			}
			set
			{
				_E001 = value;
				this.m__E000 = _E001 != null;
			}
		}

		protected override bool QueuePreviousScreen => this.m__E000;

		protected override EStateSwitcher EnvironmentOverlay => EStateSwitcher.Disabled;

		public _E000(Item item, _EAED inventoryController, _E796 session)
		{
			Item = item;
			BuildItem = item?.CloneItem();
			InventoryController = inventoryController;
			Session = session;
		}

		protected override async Task<bool> CloseScreenInterruption(bool moveForward)
		{
			bool flag = _E002 != null;
			if (flag)
			{
				flag = !(await _E002._E016());
			}
			if (flag)
			{
				return false;
			}
			return await base.CloseScreenInterruption(moveForward);
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private Task<bool> _E000(bool moveForward)
		{
			return base.CloseScreenInterruption(moveForward);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public bool inProgress;

		internal void _E000(Transform _, ModdingScreenSlotView view)
		{
			view.SetLockedStatus(inProgress);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _ECAC build;

		public EditBuildScreen _003C_003E4__this;

		internal void _E000()
		{
			_003C_003E4__this.m__E004.Value = false;
			if (build == null)
			{
				throw new Exception(_ED3E._E000(249788));
			}
			if (build.Item == null)
			{
				throw new Exception(_ED3E._E000(249815));
			}
			_003C_003E4__this._E012(build);
			_003C_003E4__this._openBuildWindow.Close();
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public _ECAC foundBuild;

		public string nextBuildId;

		public string newBuildName;

		public EditBuildScreen _003C_003E4__this;

		internal void _E000()
		{
			if (foundBuild != null)
			{
				nextBuildId = foundBuild.Id;
				_003C_003E4__this.m__E00E.RemoveBuild(foundBuild.Id, sendRequest: false, null);
			}
			_003C_003E4__this.m__E004.Value = false;
			_003C_003E4__this._E004(new _ECAC(nextBuildId, newBuildName, newBuildName, _003C_003E4__this.Item, fromFromPreset: false));
			if (_003C_003E4__this.m__E00F == null)
			{
				throw new Exception(_ED3E._E000(249853));
			}
			_003C_003E4__this.m__E00E.SaveBuild(_003C_003E4__this.m__E00F, delegate(Result<_ECAC> result)
			{
				if (_003C_003E4__this.m__E00F != null && !result.Failed)
				{
					_003C_003E4__this._E004(result.Value);
				}
			});
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public Dictionary<string, int> buildItems;

		public Func<Item, bool> _003C_003E9__1;

		internal bool _E000(Item item)
		{
			return !buildItems.ContainsKey(item.TemplateId);
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public Item item;

		internal bool _E000(string z)
		{
			return z == item.TemplateId;
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public EditBuildScreen _003C_003E4__this;

		public List<Mod> alreadyInstalledMods;

		internal void _E000()
		{
			_003C_003E4__this._E00F(alreadyInstalledMods).HandleExceptions();
		}

		internal void _E001(bool arg)
		{
			_003C_003E4__this._E006(arg, alreadyInstalledMods);
		}
	}

	[CompilerGenerated]
	private sealed class _E008
	{
		public Item newItem;

		internal bool _E000(Item x)
		{
			return newItem.TemplateId == x.TemplateId;
		}
	}

	[CompilerGenerated]
	private sealed class _E009
	{
		public TaskCompletionSource<bool> taskSource;

		internal void _E000()
		{
			taskSource.SetResult(result: true);
		}

		internal void _E001()
		{
			taskSource.SetResult(result: false);
		}
	}

	private new const string m__E000 = "Are you sure you want to discard your changes?";

	private const string m__E001 = "Build with the same name will be replaced. Continue?";

	private const string m__E002 = "Are you sure you want to delete this build?";

	private const string m__E003 = "Weapon has been built";

	[SerializeField]
	private Toggle _onlyAvailableToggle;

	[SerializeField]
	private Button _newBuildButton;

	[SerializeField]
	private Button _openBuildButton;

	[SerializeField]
	private HoverTooltipArea _saveAsHoverArea;

	[SerializeField]
	private Button _saveAsBuildButton;

	[SerializeField]
	private CanvasGroup _selectItemHoverArea;

	[SerializeField]
	private Button _selectItemButton;

	[SerializeField]
	private CanvasGroup _findPartsHoverArea;

	[SerializeField]
	private Button _findPartsButton;

	[SerializeField]
	private Button _publishButton;

	[SerializeField]
	private Button _deleteBuildButton;

	[SerializeField]
	private CanvasGroup _deleteCanvasGroup;

	[SerializeField]
	private ButtonWithHint _saveButton;

	[SerializeField]
	private HoverTooltipArea _saveHoverArea;

	[SerializeField]
	private ButtonWithHint _assembleButton;

	[SerializeField]
	private HoverTooltipArea _assembleHoverArea;

	[SerializeField]
	private ValidationInputField _buildName;

	[SerializeField]
	private EditBuildNameWindow _editBuildNameWindow;

	[SerializeField]
	private GameObject _itemReadyToAssemble;

	[SerializeField]
	private Button _itemReadyToAssembleButton;

	[SerializeField]
	private OpenBuildWindow _openBuildWindow;

	[SerializeField]
	private SelectWeaponBodyWindow _selectWeaponBodyWindow;

	[SerializeField]
	private AssembleBuildWindow _assembleBuildWindow;

	private readonly _ECF5<bool> m__E004 = new _ECF5<bool>(initialValue: false);

	private readonly _ECF5<bool> m__E005 = new _ECF5<bool>(initialValue: false);

	private readonly _ECF5<bool> m__E006 = new _ECF5<bool>(initialValue: false);

	private readonly _ECF5<bool> m__E007 = new _ECF5<bool>(initialValue: false);

	private readonly _ECF5<bool> m__E008 = new _ECF5<bool>(initialValue: false);

	private readonly _ECF5<bool> m__E009 = new _ECF5<bool>(initialValue: false);

	[CanBeNull]
	private Item m__E00A;

	private _E63B m__E00B;

	private _ECBD m__E00C;

	private _EBA8 m__E00D;

	private _ECAD m__E00E;

	[CanBeNull]
	private _ECAC m__E00F;

	private _EB1E m__E010;

	private _EB6B m__E011;

	private _EB6A m__E012;

	private _E796 m__E013;

	private List<Item> m__E014;

	private Item[] m__E015;

	private _EB1F m__E016;

	private readonly _EB6D m__E017 = new _EB6D();

	private Profile m__E018;

	private _E63B _E019 => this.m__E00B ?? (this.m__E00B = Singleton<_E63B>.Instance);

	private string _E01A
	{
		get
		{
			string text = string.Empty;
			if (!this.m__E004.Value)
			{
				text += _ED3E._E000(249371).Localized();
			}
			if (!this.m__E005.Value)
			{
				return text;
			}
			if (!string.IsNullOrEmpty(text))
			{
				text += _ED3E._E000(2540);
			}
			return text + _ED3E._E000(249350).Localized();
		}
	}

	private string _E01B
	{
		get
		{
			Item[] array = this.m__E015;
			if (array != null && array.Length != 0)
			{
				if (this.m__E00A != null)
				{
					return _ED3E._E000(249376);
				}
				return _ED3E._E000(249453);
			}
			return _ED3E._E000(249477);
		}
	}

	private IBindable<bool> _E01C => _ECF3.Combine(this.m__E004, this.m__E005, this.m__E007, delegate(bool changed, bool missingParts, bool isSaving)
	{
		_saveHoverArea.SetMessageText(this._E01A, rawText: true);
		return changed && !missingParts && !isSaving;
	});

	private IBindable<bool> _E01D => _ECF3.Combine(this.m__E008, this.m__E009, delegate(bool isAssembling, bool isAssemblingAvailable)
	{
		bool num = !isAssembling && isAssemblingAvailable;
		if (!num)
		{
			_assembleHoverArea.SetMessageText(isAssembling ? string.Empty : this._E01B);
		}
		return num;
	});

	protected override void Awake()
	{
		base.Awake();
		_newBuildButton.onClick.AddListener(_E002);
		_openBuildButton.onClick.AddListener(_E003);
		_saveAsBuildButton.onClick.AddListener(_E005);
		_selectItemButton.onClick.AddListener(_E00B);
		_findPartsButton.onClick.AddListener(delegate
		{
			_E006(onlyContained: false, new List<Mod>());
		});
		_publishButton.onClick.AddListener(delegate
		{
			UnityEngine.Debug.Log(_ED3E._E000(249739));
		});
		_deleteBuildButton.onClick.AddListener(_E009);
		_saveButton.Show(_E00A);
		_assembleButton.Show(_E00D);
		_itemReadyToAssembleButton.onClick.AddListener(_E00D);
		_onlyAvailableToggle.onValueChanged.AddListener(delegate(bool arg)
		{
			UpdateManipulation(arg ? this.m__E012 : this.m__E011);
			RefreshWeapon();
		});
	}

	public override void Show(_E000 controller)
	{
		Show(controller.Item, controller.BuildItem, controller.InventoryController, controller.Session);
	}

	private void Show(Item itemBody, Item buildItem, _EAED inventoryController, _E796 session)
	{
		this.m__E018 = ScreenController.Profile;
		this.m__E015 = null;
		this.m__E00C = session.RagFair;
		this.m__E00D = Singleton<_EBA8>.Instance;
		this.m__E00E = session.WeaponBuildsStorage;
		this.m__E013 = session;
		if (this.m__E016 == null)
		{
			this.m__E016 = new _EB1F(_E010(Array.Empty<Item>()));
		}
		Show(buildItem, inventoryController);
		if (itemBody?.CurrentAddress != null)
		{
			this.m__E00A = itemBody;
		}
		if (buildItem != null)
		{
			_ECAC obj = session.WeaponBuildsStorage.FindBuild(buildItem);
			if (obj != null)
			{
				_E004(obj);
			}
		}
		else
		{
			this.m__E004.Value = false;
			_E004(null);
			_E011(readyToAssemble: false);
			_E003();
		}
		UI.AddDisposable(this._E01C.Bind(delegate(bool arg)
		{
			_saveHoverArea.SetUnlockStatus(arg);
		}));
		UI.AddDisposable(this._E01D.Bind(delegate(bool arg)
		{
			_assembleHoverArea.SetUnlockStatus(arg);
		}));
		UI.AddDisposable(this.m__E006.Bind(delegate(bool arg)
		{
			_deleteCanvasGroup.SetUnlockStatus(arg);
		}));
		UI.AddDisposable(this.m__E005.Bind(delegate(bool anyMissing)
		{
			_saveAsHoverArea.SetUnlockStatus(!anyMissing);
		}));
		UI.AddDisposable(this.m__E008.Bind(delegate(bool inProgress)
		{
			ModIcons.ExecuteForEach(delegate(Transform _, ModdingScreenSlotView view)
			{
				view.SetLockedStatus(inProgress);
			});
		}));
		_itemUiContext.Configure(inventoryController, this.m__E018, session, session.InsuranceCompany, this.m__E00C, null, null, new _EA40[1] { this.m__E018.Inventory.Stash }, EItemUiContextType.RagfairScreen, ECursorResult.ShowCursor);
		UpdateItem(buildItem);
	}

	protected override _EB6A CreateBuildManipulation()
	{
		this.m__E014 = base.InventoryController.Inventory.AllPlayerItems.ToList();
		Item[] playerItems = this.m__E014.Where(_E001).ToArray();
		if (this.m__E010 == null)
		{
			this.m__E010 = _E010(this._E019.CreateAllModsEver());
		}
		_EAED controller = new _EAED(this.m__E018, examined: false);
		this.m__E011 = new _EB6B(controller, new _EA40[1] { (_EA40)this.m__E010.RootItem }, playerItems);
		_EA40[] topLevelCollections = new _EA40[1] { this.m__E018.Inventory.Stash };
		_EB1E obj = _E010((from x in topLevelCollections.GetAllItemsFromCollections()
			select x.CloneItem()).ToArray());
		this.m__E012 = new _EB6B(controller, new _EA40[1] { (_EA40)obj.RootItem }, null);
		return this.m__E011;
	}

	private void _E000()
	{
		Item[] playerItems = this.m__E014.Where(_E001).ToArray();
		this.m__E011 = this.m__E011.CreateOtherManipulation(playerItems);
	}

	private bool _E001(Item itemToCheck)
	{
		if (itemToCheck == null)
		{
			return false;
		}
		if (base.InventoryController.IsItemEquipped(itemToCheck))
		{
			return false;
		}
		if (!itemToCheck.Parent.Container.ParentItem.IsContainer)
		{
			return itemToCheck.IsChildOf(this.m__E00A);
		}
		return true;
	}

	private void _E002()
	{
		_E014(!this.m__E004.Value || Item == null, _ED3E._E000(249562).Localized(), delegate
		{
			_buildName.text = string.Empty;
			UpdateItem(Item?.CloneBodyItem());
			_E004(null);
		}, delegate
		{
		});
	}

	private void _E003()
	{
		if (_openBuildWindow.gameObject.activeSelf)
		{
			_openBuildWindow.Close();
		}
		_openBuildWindow.Show(this.m__E00C, this.m__E00D, this.m__E00E, delegate(_ECAC build)
		{
			if (build == this.m__E00F)
			{
				_openBuildWindow.Close();
			}
			_E014(!this.m__E004.Value || Item == null, _ED3E._E000(249562).Localized(), delegate
			{
				this.m__E004.Value = false;
				if (build == null)
				{
					throw new Exception(_ED3E._E000(249788));
				}
				if (build.Item == null)
				{
					throw new Exception(_ED3E._E000(249815));
				}
				_E012(build);
				_openBuildWindow.Close();
			}, delegate
			{
			});
		});
	}

	private void _E004([CanBeNull] _ECAC build)
	{
		this.m__E00F = build;
		bool flag = this.m__E00F != null;
		bool flag2 = flag && !this.m__E00F.FromPreset;
		this.m__E006.Value = flag2;
		this.m__E004.Value = !flag;
		_buildName.text = (flag2 ? this.m__E00F.HandbookName : string.Empty);
	}

	private void _E005()
	{
		if (_editBuildNameWindow.gameObject.activeSelf)
		{
			_editBuildNameWindow.Close();
		}
		_editBuildNameWindow.Show(_buildName.text, delegate(string newBuildName)
		{
			string nextBuildId = string.Empty;
			_ECAC foundBuild = this.m__E00E.FindByName(newBuildName);
			bool flag = foundBuild != null;
			_E014(!flag, _ED3E._E000(249726).Localized(), delegate
			{
				if (foundBuild != null)
				{
					nextBuildId = foundBuild.Id;
					this.m__E00E.RemoveBuild(foundBuild.Id, sendRequest: false, null);
				}
				this.m__E004.Value = false;
				_E004(new _ECAC(nextBuildId, newBuildName, newBuildName, Item, fromFromPreset: false));
				if (this.m__E00F == null)
				{
					throw new Exception(_ED3E._E000(249853));
				}
				this.m__E00E.SaveBuild(this.m__E00F, delegate(Result<_ECAC> result)
				{
					if (this.m__E00F != null && !result.Failed)
					{
						_E004(result.Value);
					}
				});
			}, delegate
			{
			});
		});
	}

	private void _E006(bool onlyContained, List<Mod> alreadyInstalledMods)
	{
		List<Item> list = (onlyContained ? _E008(alreadyInstalledMods) : _E007());
		if (list.Count <= 0)
		{
			UnityEngine.Debug.LogError(_ED3E._E000(249577));
			return;
		}
		string[] source = list.Select((Item y) => y.TemplateId).ToArray();
		Dictionary<string, int> buildItems = new Dictionary<string, int>();
		foreach (Item item2 in list.Where((Item item) => !buildItems.ContainsKey(item.TemplateId)))
		{
			buildItems.Add(item2.TemplateId, source.Count((string z) => z == item2.TemplateId));
		}
		BuildItemSearchValue value = new BuildItemSearchValue
		{
			BuildName = this.m__E00F?.HandbookName,
			BuildItems = buildItems,
			BuildCount = 1
		};
		this.m__E013.RagFair.CancellableFilters.Store();
		this.m__E013.RagFair.CancellableFilters.Clear();
		this.m__E013.RagFair.ExternalRagfairSearch(new _ECC4(EFilterType.BuildItems, value));
	}

	private List<Item> _E007()
	{
		if (!(Item is Weapon weapon))
		{
			return new List<Item>();
		}
		List<Item> list = (from slot in weapon.AllSlots
			select slot.ContainedItem into x
			where x is Mod
			select x).ToList();
		list.Add(weapon);
		return list;
	}

	private List<Item> _E008(List<Mod> alreadyInstalledMods)
	{
		List<Item> itemsToCheck = _E007();
		List<Item> list = this.m__E011.GetMissingItems(itemsToCheck, alreadyInstalledMods).ToList();
		if (this.m__E00A != null)
		{
			list.RemoveAll((Item x) => x.TemplateId == this.m__E00A.TemplateId);
		}
		return list;
	}

	private void _E009()
	{
		if (this.m__E00F == null)
		{
			UnityEngine.Debug.LogError(_ED3E._E000(249625));
			return;
		}
		if (_openBuildWindow.gameObject.activeSelf)
		{
			_openBuildWindow.Close();
		}
		_E014(autoAccept: false, _ED3E._E000(249661).Localized(), delegate
		{
			this.m__E00E.RemoveBuild(this.m__E00F.Id, sendRequest: true, _E002);
		}, delegate
		{
		});
	}

	private void _E00A()
	{
		if (this.m__E00F == null || this.m__E00F.FromPreset)
		{
			_E005();
			return;
		}
		this.m__E007.Value = true;
		this.m__E004.Value = false;
		this.m__E00F.Item = Item;
		if (this.m__E00F.FromPreset)
		{
			this.m__E00F.ChangeId(string.Empty);
		}
		this.m__E00F.HandbookName = _buildName.text;
		if ((bool)_buildName.HasError)
		{
			return;
		}
		this.m__E00E.SaveBuild(this.m__E00F, delegate(Result<_ECAC> result)
		{
			if (!(base.gameObject == null) && base.gameObject.activeSelf && !result.Failed)
			{
				_E004(result.Value);
				this.m__E007.Value = false;
			}
		});
	}

	private void _E00B()
	{
		_E00C(force: true);
	}

	private void _E00C(bool force)
	{
		if (this.m__E013.Profile == null || Item == null || (!force && this.m__E00A?.TemplateId == Item.TemplateId))
		{
			return;
		}
		if (_selectWeaponBodyWindow.gameObject.activeSelf)
		{
			_selectWeaponBodyWindow.Close();
		}
		if (this.m__E015.Length == 0)
		{
			return;
		}
		_selectWeaponBodyWindow.Show(this.m__E015, this.m__E00A, this.m__E013.Profile, base.InventoryController, delegate(Weapon selectedWeapon)
		{
			if (selectedWeapon != null)
			{
				_E013(selectedWeapon);
				CheckForVitalParts();
			}
		}, delegate
		{
		});
	}

	private void _E00D()
	{
		if (this.m__E00A == null)
		{
			_E00B();
			return;
		}
		List<Mod> installedMods = this.m__E017.GetInstalledMods(this.m__E00A as Weapon);
		List<Item> list = _E008(installedMods);
		if (list.Count > 0)
		{
			_E00E(list, _E007(), installedMods);
		}
		else
		{
			_E00F(installedMods).HandleExceptions();
		}
	}

	private void _E00E(List<Item> outOfCollectionItems, List<Item> itemsInBuild, List<Mod> alreadyInstalledMods)
	{
		_assembleBuildWindow.Show(this.m__E00D, outOfCollectionItems, itemsInBuild, base.InventoryController, _itemUiContext, this.m__E013.InsuranceCompany, delegate
		{
			_E00F(alreadyInstalledMods).HandleExceptions();
		}, delegate(bool arg)
		{
			_E006(arg, alreadyInstalledMods);
		});
	}

	private async Task _E00F(List<Mod> alreadyInstalledMods)
	{
		if (Item == null)
		{
			return;
		}
		Weapon weapon;
		Weapon weapon2 = (weapon = Item as Weapon);
		if (weapon == null)
		{
			throw new Exception(_ED3E._E000(203648));
		}
		_selectWeaponBodyWindow.Close();
		_assembleBuildWindow.Close();
		if (this.m__E017.CheckIfAlreadyBuilt(alreadyInstalledMods, weapon2))
		{
			_E857.DisplayMessageNotification(_ED3E._E000(247834).Localized());
			return;
		}
		this.m__E008.Value = true;
		await TasksExtensions.WaitUntil(() => !this.m__E007.Value);
		if (await this.m__E017.Assemble((Weapon)this.m__E00A, weapon2, base.InventoryController, this.m__E014))
		{
			_E857.DisplayMessageNotification(_ED3E._E000(247834).Localized());
		}
		CheckForVitalParts();
		this.m__E008.Value = false;
	}

	private _EB1E _E010(Item[] items)
	{
		_EAA0 obj = this._E019.CreateFakeStash();
		obj.Grids[0] = new _E9EE(Guid.NewGuid().ToString(), 30, 1, canStretchVertically: true, Array.Empty<ItemFilter>(), obj);
		string text = Guid.NewGuid().ToString();
		_EB1E result = new _EB1E(obj, _ED3E._E000(249681), text, canBeLocalized: false);
		foreach (Item item in items)
		{
			item.StackObjectsCount = item.StackMaxSize;
			obj.Grid.Add(item);
		}
		return result;
	}

	protected override void WeaponUpdate()
	{
		_E011(readyToAssemble: false);
		base.WeaponUpdate();
		ScreenController.BuildItem = Item;
		_E015();
	}

	protected override void CheckForVitalParts()
	{
		if (!RefreshingWeapon)
		{
			if (!(Item is Weapon weapon))
			{
				this.m__E005.Value = true;
				_E011(readyToAssemble: false);
				return;
			}
			this.m__E005.Value = weapon.MissingVitalParts.Any();
			List<Mod> installedMods = this.m__E017.GetInstalledMods(this.m__E00A as Weapon);
			List<Item> list = _E008(installedMods);
			bool flag = this.m__E017.CheckIfAlreadyBuilt(installedMods, Item as Weapon);
			_E011(!flag && this.m__E00A != null, list.Count <= 0);
		}
	}

	private void _E011(bool readyToAssemble, bool noMissingMods = false)
	{
		this.m__E009.Value = readyToAssemble;
		_itemReadyToAssemble.SetActive(readyToAssemble && noMissingMods);
	}

	private void _E012(_ECAC build)
	{
		_E004(build);
		if (this.m__E00F != null)
		{
			UpdateItem(this.m__E00F.Item.CloneItem());
		}
	}

	private void _E013(Item newBody)
	{
		ScreenController.Item = newBody;
		this.m__E00A = newBody;
		_E000();
	}

	protected override void UpdateItem(Item newItem)
	{
		if (Item != null)
		{
			this.m__E016.Remove(Item, string.Empty);
		}
		ScreenController.BuildItem = newItem;
		if (newItem != null)
		{
			this.m__E016.Add(ScreenController.BuildItem, new string[0]);
		}
		_findPartsHoverArea.SetUnlockStatus(newItem != null && !this.m__E00C.Disabled);
		if (newItem == null || this.m__E00A?.TemplateId != newItem.TemplateId)
		{
			_E013(null);
		}
		if (newItem?.TemplateId != Item?.TemplateId || this.m__E015 == null)
		{
			this.m__E015 = ((newItem != null) ? (from x in this.m__E014.Where(_E001)
				where newItem.TemplateId == x.TemplateId
				select x).ToArray() : Array.Empty<Item>());
		}
		base.UpdateItem(newItem);
		if (this.m__E00A == null)
		{
			_E00C(force: false);
		}
		_selectItemHoverArea.SetUnlockStatus(this.m__E015.Length != 0);
	}

	private void _E014(bool autoAccept, string message, [CanBeNull] Action acceptAction, [CanBeNull] Action cancelAction = null)
	{
		if (autoAccept)
		{
			acceptAction?.Invoke();
		}
		else
		{
			_itemUiContext.ShowMessageWindow(message, acceptAction, cancelAction);
		}
	}

	private void _E015()
	{
		this.m__E004.Value = true;
		_ECAC obj = this.m__E013.WeaponBuildsStorage.FindBuild(Item);
		if (obj != null)
		{
			_E004(obj);
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			if (_openBuildWindow.gameObject.activeSelf)
			{
				_openBuildWindow.Close();
				return ETranslateResult.Block;
			}
			if (_selectWeaponBodyWindow.gameObject.activeSelf)
			{
				_selectWeaponBodyWindow.Close();
				return ETranslateResult.Block;
			}
			if (_assembleBuildWindow.gameObject.activeSelf)
			{
				_assembleBuildWindow.Close();
				return ETranslateResult.Block;
			}
			if (_editBuildNameWindow.gameObject.activeSelf)
			{
				_editBuildNameWindow.Close();
				return ETranslateResult.Block;
			}
			ScreenController.CloseScreen();
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	private Task<bool> _E016()
	{
		if (!this.m__E004.Value || Item == null)
		{
			return Task.FromResult(result: true);
		}
		TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
		_E014(autoAccept: false, _ED3E._E000(249562).Localized(), delegate
		{
			taskSource.SetResult(result: true);
		}, delegate
		{
			taskSource.SetResult(result: false);
		});
		return taskSource.Task;
	}

	public override void Close()
	{
		if ((bool)_openBuildWindow)
		{
			_openBuildWindow.Close();
		}
		_selectWeaponBodyWindow.Close();
		_assembleBuildWindow.Close();
		_editBuildNameWindow.Close();
		this.m__E004.Value = false;
		this.m__E005.Value = false;
		_E011(readyToAssemble: false);
		_E004(null);
		if (MonoBehaviourSingleton<PreloaderUI>.Instantiated)
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.CloseErrorScreen();
		}
		base.Close();
	}

	[CompilerGenerated]
	private bool _E017(bool changed, bool missingParts, bool isSaving)
	{
		_saveHoverArea.SetMessageText(this._E01A, rawText: true);
		if (changed && !missingParts)
		{
			return !isSaving;
		}
		return false;
	}

	[CompilerGenerated]
	private bool _E018(bool isAssembling, bool isAssemblingAvailable)
	{
		bool num = !isAssembling && isAssemblingAvailable;
		if (!num)
		{
			_assembleHoverArea.SetMessageText(isAssembling ? string.Empty : this._E01B);
		}
		return num;
	}

	[CompilerGenerated]
	private void _E019()
	{
		_E006(onlyContained: false, new List<Mod>());
	}

	[CompilerGenerated]
	private void _E01A(bool arg)
	{
		UpdateManipulation(arg ? this.m__E012 : this.m__E011);
		RefreshWeapon();
	}

	[CompilerGenerated]
	private void _E01B(bool arg)
	{
		_saveHoverArea.SetUnlockStatus(arg);
	}

	[CompilerGenerated]
	private void _E01C(bool arg)
	{
		_assembleHoverArea.SetUnlockStatus(arg);
	}

	[CompilerGenerated]
	private void _E01D(bool arg)
	{
		_deleteCanvasGroup.SetUnlockStatus(arg);
	}

	[CompilerGenerated]
	private void _E01E(bool anyMissing)
	{
		_saveAsHoverArea.SetUnlockStatus(!anyMissing);
	}

	[CompilerGenerated]
	private void _E01F(bool inProgress)
	{
		ModIcons.ExecuteForEach(delegate(Transform _, ModdingScreenSlotView view)
		{
			view.SetLockedStatus(inProgress);
		});
	}

	[CompilerGenerated]
	private void _E020()
	{
		_buildName.text = string.Empty;
		UpdateItem(Item?.CloneBodyItem());
		_E004(null);
	}

	[CompilerGenerated]
	private void _E021(_ECAC build)
	{
		if (build == this.m__E00F)
		{
			_openBuildWindow.Close();
		}
		_E014(!this.m__E004.Value || Item == null, _ED3E._E000(249562).Localized(), delegate
		{
			this.m__E004.Value = false;
			if (build == null)
			{
				throw new Exception(_ED3E._E000(249788));
			}
			if (build.Item == null)
			{
				throw new Exception(_ED3E._E000(249815));
			}
			_E012(build);
			_openBuildWindow.Close();
		}, delegate
		{
		});
	}

	[CompilerGenerated]
	private void _E022(string newBuildName)
	{
		string nextBuildId = string.Empty;
		_ECAC foundBuild = this.m__E00E.FindByName(newBuildName);
		bool flag = foundBuild != null;
		_E014(!flag, _ED3E._E000(249726).Localized(), delegate
		{
			if (foundBuild != null)
			{
				nextBuildId = foundBuild.Id;
				this.m__E00E.RemoveBuild(foundBuild.Id, sendRequest: false, null);
			}
			this.m__E004.Value = false;
			_E004(new _ECAC(nextBuildId, newBuildName, newBuildName, Item, fromFromPreset: false));
			if (this.m__E00F == null)
			{
				throw new Exception(_ED3E._E000(249853));
			}
			this.m__E00E.SaveBuild(this.m__E00F, delegate(Result<_ECAC> result)
			{
				if (this.m__E00F != null && !result.Failed)
				{
					_E004(result.Value);
				}
			});
		}, delegate
		{
		});
	}

	[CompilerGenerated]
	private void _E023(Result<_ECAC> result)
	{
		if (this.m__E00F != null && !result.Failed)
		{
			_E004(result.Value);
		}
	}

	[CompilerGenerated]
	private bool _E024(Item x)
	{
		return x.TemplateId == this.m__E00A.TemplateId;
	}

	[CompilerGenerated]
	private void _E025()
	{
		this.m__E00E.RemoveBuild(this.m__E00F.Id, sendRequest: true, _E002);
	}

	[CompilerGenerated]
	private void _E026(Result<_ECAC> result)
	{
		if (!(base.gameObject == null) && base.gameObject.activeSelf && !result.Failed)
		{
			_E004(result.Value);
			this.m__E007.Value = false;
		}
	}

	[CompilerGenerated]
	private void _E027(Weapon selectedWeapon)
	{
		if (selectedWeapon != null)
		{
			_E013(selectedWeapon);
			CheckForVitalParts();
		}
	}

	[CompilerGenerated]
	private bool _E028()
	{
		return !this.m__E007.Value;
	}
}
