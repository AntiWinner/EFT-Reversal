using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class TacticalClothingView : ServiceView
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public TacticalClothingView _003C_003E4__this;

		public TaskCompletionSource<bool> showPreviewSource;

		public Action _003C_003E9__1;

		internal void _E000()
		{
			_003C_003E4__this._rotator.Init(_003C_003E4__this._playerModelView.ModelPlayerPoser.transform);
			_003C_003E4__this._dragTrigger.onDrag -= _003C_003E4__this._E00F;
			_003C_003E4__this._dragTrigger.onDrag += _003C_003E4__this._E00F;
			_003C_003E4__this.UI.AddDisposable(delegate
			{
				_003C_003E4__this._dragTrigger.onDrag -= _003C_003E4__this._E00F;
			});
			showPreviewSource.SetResult(result: true);
		}

		internal void _E001()
		{
			_003C_003E4__this._dragTrigger.onDrag -= _003C_003E4__this._E00F;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public int requestIndex;

		public TacticalClothingView _003C_003E4__this;

		internal void _E000()
		{
			_E003 CS_0024_003C_003E8__locals0 = new _E003
			{
				CS_0024_003C_003E8__locals1 = this
			};
			if (requestIndex == _003C_003E4__this._E1F4 && _003C_003E4__this.gameObject.activeSelf && _003C_003E4__this._E1EB.Count != 0)
			{
				CS_0024_003C_003E8__locals0.currentBodyPath = _003C_003E4__this._E1EE.GetBundle(_003C_003E4__this._E0B7.Customization[EBodyModelPart.Body]).path;
				CS_0024_003C_003E8__locals0.currentFeetPath = _003C_003E4__this._E1EE.GetBundle(_003C_003E4__this._E0B7.Customization[EBodyModelPart.Feet]).path;
				_003C_003E4__this.UI.AddViewList(_003C_003E4__this._E1EB[EBodyModelPart.Body], _003C_003E4__this._clothingItemTemplate, _003C_003E4__this._upperBodyList, delegate(ClothingItem._E000 offer, ClothingItem bodyPartView)
				{
					CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._E007(offer, bodyPartView, EBodyModelPart.Body, CS_0024_003C_003E8__locals0.currentBodyPath);
				});
				_003C_003E4__this.UI.AddViewList(_003C_003E4__this._E1EB[EBodyModelPart.Feet], _003C_003E4__this._clothingItemTemplate, _003C_003E4__this._lowerBodyList, delegate(ClothingItem._E000 offer, ClothingItem bodyPartView)
				{
					CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._E007(offer, bodyPartView, EBodyModelPart.Feet, CS_0024_003C_003E8__locals0.currentFeetPath);
				});
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public string currentBodyPath;

		public string currentFeetPath;

		public _E002 CS_0024_003C_003E8__locals1;

		internal void _E000(ClothingItem._E000 offer, ClothingItem bodyPartView)
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this._E007(offer, bodyPartView, EBodyModelPart.Body, currentBodyPath);
		}

		internal void _E001(ClothingItem._E000 offer, ClothingItem bodyPartView)
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this._E007(offer, bodyPartView, EBodyModelPart.Feet, currentFeetPath);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public TacticalClothingView _003C_003E4__this;

		public EBodyModelPart bodyPart;

		internal void _E000(ClothingItem item)
		{
			_003C_003E4__this._E008(bodyPart, item);
		}

		internal void _E001(ClothingItem item)
		{
			_003C_003E4__this._E00B(bodyPart, item);
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public TacticalClothingView _003C_003E4__this;

		public ClothingItem clothing;

		internal void _E000()
		{
			_003C_003E4__this._E009(clothing);
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public ClothingItem clothing;

		public TacticalClothingView _003C_003E4__this;

		public List<_E557> reqItems;

		internal void _E000(IResult response)
		{
			if (response.Failed)
			{
				if (clothing != null)
				{
					clothing.UpdateLock(isLocked: true);
				}
				_003C_003E4__this._E003();
			}
			else
			{
				_003C_003E4__this._E1EE.SetAvailableSuites(clothing.Offer.Suite.Id);
			}
		}

		internal void _E001()
		{
			using List<_E557>.Enumerator enumerator = reqItems.GetEnumerator();
			while (enumerator.MoveNext())
			{
				_E008 CS_0024_003C_003E8__locals0 = new _E008
				{
					item = enumerator.Current
				};
				Item item = _003C_003E4__this._E1EC.FirstOrDefault((Item stashItem) => stashItem.Id == CS_0024_003C_003E8__locals0.item.id);
				if (item != null)
				{
					if (item is _EA9E obj && obj.StackObjectsCount > CS_0024_003C_003E8__locals0.item.count)
					{
						obj.StackObjectsCount -= CS_0024_003C_003E8__locals0.item.count;
					}
					else
					{
						_003C_003E4__this._E1EC.Remove(item);
					}
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public _EBE5 itemRequirement;

		internal bool _E000(Item x)
		{
			return x.TemplateId == itemRequirement._tpl;
		}
	}

	[CompilerGenerated]
	private sealed class _E008
	{
		public _E557 item;

		internal bool _E000(Item stashItem)
		{
			return stashItem.Id == item.id;
		}
	}

	[SerializeField]
	private ServicesListView.ServiceInfo _info;

	[SerializeField]
	private PlayerModelView _playerModelView;

	[SerializeField]
	private RectTransform _upperBodyList;

	[SerializeField]
	private RectTransform _lowerBodyList;

	[SerializeField]
	private ClothingItem _clothingItemTemplate;

	[SerializeField]
	private XCoordRotation _rotator;

	[SerializeField]
	private DragTrigger _dragTrigger;

	[SerializeField]
	private TextMeshProUGUI _sideTitle;

	[SerializeField]
	private HoverTrigger _previewHoverArea;

	private readonly Dictionary<EBodyModelPart, ClothingItem> _E1D1 = new Dictionary<EBodyModelPart, ClothingItem>();

	private readonly Dictionary<EBodyModelPart, ClothingItem> _E1D2 = new Dictionary<EBodyModelPart, ClothingItem>();

	private readonly Dictionary<EBodyModelPart, ClothingItem._E000[]> _E1EB = new Dictionary<EBodyModelPart, ClothingItem._E000[]>();

	private _E8B2 _E1BB;

	private Profile _E0B7;

	private _EAED _E092;

	private _E934 _E1BC;

	private _E9EF _E1B2;

	private List<Item> _E1EC;

	private _ED0E<_ED08>._E002 _E1ED;

	private _E60E _E1EE;

	private _E63B _E1EF;

	private _EBE6 _E1F0;

	private _EBE6 _E1F1;

	private ItemUiContext _E118;

	private bool _E1F2;

	private _E72D _E1F3;

	private int _E1F4;

	private bool _E1F5;

	private Profile _E1F6;

	private Profile _E1F7;

	private Task _E1F8;

	private Profile _E000
	{
		get
		{
			if (!_E1F5)
			{
				return _E1F6;
			}
			return _E1F7;
		}
	}

	private Task _E001 => _E1F8 ?? Task.CompletedTask;

	public override void Show(_E8B2 trader, Profile profile, _EAED inventoryController, _E981 healthController, _E934 quests, ItemUiContext context, _E796 session)
	{
		_E0B7 = profile;
		_E092 = inventoryController;
		_E1B2 = inventoryController.Inventory.Stash.Grid;
		_E1BB = trader;
		_E1BC = quests;
		_E1EE = Singleton<_E60E>.Instance;
		_E118 = context;
		ShowGameObject();
		_sideTitle.text = profile.Info.Side.ToString().Localized();
		_E005(trader);
		_E1F5 = false;
		_E1F3 = new _E72D(profile.Customization);
		_E1F7 = new Profile
		{
			Customization = _E1F3,
			Info = profile.Info,
			Inventory = _E672.CloneInventory(Singleton<_E63B>.Instance, profile.Inventory)
		};
		_E1F6 = new Profile
		{
			Customization = _E1F3,
			Info = profile.Info,
			Inventory = _E672.CloneInventory(Singleton<_E63B>.Instance, profile.Inventory)
		};
		foreach (Slot item in _E1F6.Inventory.Equipment.Slots.Where((Slot _) => _.ContainedItem != null && !(_.ContainedItem is Weapon)))
		{
			item.RemoveItem();
		}
		_E004();
		UI.AddDisposable(_playerModelView);
		_E092.AddItemEvent += _E002;
		_E092.RemoveItemEvent += _E001;
		_E092.RefreshItemEvent += _E000;
		_E003();
		_previewHoverArea.Init(delegate
		{
			_E1F2 = true;
		}, delegate
		{
			_E1F2 = false;
		});
	}

	private void _E000(_EAFF obj)
	{
		_E003();
	}

	private void _E001(_EAF3 obj)
	{
		if (obj.Status == CommandStatus.Succeed)
		{
			_E003();
		}
	}

	private void _E002(_EAF2 obj)
	{
		if (obj.Status == CommandStatus.Succeed)
		{
			_E003();
		}
	}

	private void _E003()
	{
		_E1EC = _E1B2.ContainedItems.SelectMany((KeyValuePair<Item, LocationInGrid> x) => x.Key.GetAllItems()).ToList();
	}

	public override bool CheckAvailable(_E8B2 trader)
	{
		return trader.Settings.CustomizationSeller;
	}

	private void _E004()
	{
		TaskCompletionSource<bool> showPreviewSource = new TaskCompletionSource<bool>();
		_E1F8 = showPreviewSource.Task;
		_playerModelView.Show(this._E000, null, delegate
		{
			_rotator.Init(_playerModelView.ModelPlayerPoser.transform);
			_dragTrigger.onDrag -= _E00F;
			_dragTrigger.onDrag += _E00F;
			UI.AddDisposable(delegate
			{
				_dragTrigger.onDrag -= _E00F;
			});
			showPreviewSource.SetResult(result: true);
		}).HandleExceptions();
	}

	private async void _E005(_E8B2 trader)
	{
		int num = ++_E1F4;
		_EBE2[] array = await trader.GetOffers();
		if (!base.gameObject.activeSelf || num != _E1F4)
		{
			return;
		}
		ClothingItem._E001 comparer = new ClothingItem._E001(_E1EE);
		SortedSet<ClothingItem._E000> sortedSet = new SortedSet<ClothingItem._E000>(comparer);
		SortedSet<ClothingItem._E000> sortedSet2 = new SortedSet<ClothingItem._E000>(comparer);
		_EBE2[] array2 = array;
		foreach (_EBE2 obj in array2)
		{
			_EBE6 suite = _E1EE.GetSuite(obj.suiteId);
			if (suite == null)
			{
				continue;
			}
			_EBDF item = _E1EE.GetItem(suite.MainBodyPartItem);
			if (item != null)
			{
				ClothingItem._E000 item2 = new ClothingItem._E000
				{
					Offer = obj,
					Suite = suite,
					Clothing = item
				};
				if (suite is _EBE8)
				{
					sortedSet2.Add(item2);
				}
				else if (suite is _EBE7)
				{
					sortedSet.Add(item2);
				}
			}
		}
		ClothingItem._E000[] array3 = new ClothingItem._E000[sortedSet.Count];
		sortedSet.CopyTo(array3);
		_E1EB[EBodyModelPart.Body] = array3;
		ClothingItem._E000[] array4 = new ClothingItem._E000[sortedSet2.Count];
		sortedSet2.CopyTo(array4);
		_E1EB[EBodyModelPart.Feet] = array4;
		_E006(from _ in sortedSet.Concat(sortedSet2)
			select _.Clothing.Prefab.path, num);
	}

	private void _E006(IEnumerable<string> clothing, int requestIndex)
	{
		_E1ED = Singleton<_ED0A>.Instance.Retain(clothing);
		Singleton<_E3DE>.Instance.Begin();
		_E612.WaitForAllBundles(_E1ED, delegate
		{
			if (requestIndex == _E1F4 && base.gameObject.activeSelf && _E1EB.Count != 0)
			{
				string currentBodyPath = _E1EE.GetBundle(_E0B7.Customization[EBodyModelPart.Body]).path;
				string currentFeetPath = _E1EE.GetBundle(_E0B7.Customization[EBodyModelPart.Feet]).path;
				UI.AddViewList(_E1EB[EBodyModelPart.Body], _clothingItemTemplate, _upperBodyList, delegate(ClothingItem._E000 offer, ClothingItem bodyPartView)
				{
					_E007(offer, bodyPartView, EBodyModelPart.Body, currentBodyPath);
				});
				UI.AddViewList(_E1EB[EBodyModelPart.Feet], _clothingItemTemplate, _lowerBodyList, delegate(ClothingItem._E000 offer, ClothingItem bodyPartView)
				{
					_E007(offer, bodyPartView, EBodyModelPart.Feet, currentFeetPath);
				});
			}
		});
	}

	private void _E007(ClothingItem._E000 offer, ClothingItem bodyPartView, EBodyModelPart bodyPart, string bundlePath)
	{
		bodyPartView.Init(delegate(ClothingItem item)
		{
			_E008(bodyPart, item);
		}, delegate(ClothingItem item)
		{
			_E00B(bodyPart, item);
		}, offer.Suite.NameLocalizationKey.Localized(), offer, !_E1EE.IsSuiteAvailable(offer.Suite.Id));
		bool flag = offer.Clothing.Prefab.path == bundlePath;
		bodyPartView.UpdateView(flag, flag);
		if (flag)
		{
			_E1D1[bodyPart] = bodyPartView;
			_E1D2[bodyPart] = bodyPartView;
			if (bodyPart == EBodyModelPart.Body)
			{
				_E1F0 = offer.Suite;
			}
			else
			{
				_E1F1 = offer.Suite;
			}
		}
	}

	private void _E008(EBodyModelPart bodyPart, ClothingItem clothing)
	{
		if (clothing.IsItemLocked)
		{
			_E118.ShowClothingConfirmation(clothing.Offer.Suite.NameLocalizationKey.Localized(), _E0B7, _E1BB.Info, _E1BC, clothing.Offer.Offer.requirements, _E1B2, delegate
			{
				_E009(clothing);
			});
		}
		else
		{
			_E00A(bodyPart, clothing).HandleExceptions();
		}
	}

	private void _E009(ClothingItem clothing)
	{
		_E006 CS_0024_003C_003E8__locals0 = new _E006();
		CS_0024_003C_003E8__locals0.clothing = clothing;
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		_EBE2 offer = CS_0024_003C_003E8__locals0.clothing.Offer.Offer;
		CS_0024_003C_003E8__locals0.reqItems = new List<_E557>();
		_EBE5[] itemRequirements = offer.requirements.itemRequirements;
		foreach (_EBE5 itemRequirement in itemRequirements)
		{
			IEnumerable<Item> enumerable = _E1EC.Where((Item x) => x.TemplateId == itemRequirement._tpl);
			float num = itemRequirement.count;
			foreach (Item item2 in enumerable)
			{
				int num2 = ((!(item2 is _EA9E obj)) ? 1 : ((int)Mathf.Min(obj.StackObjectsCount, num)));
				_E557 item = new _E557(item2, num2);
				CS_0024_003C_003E8__locals0.reqItems.Add(item);
				num -= (float)num2;
				if (num <= 0f)
				{
					break;
				}
			}
			if (num > 0f)
			{
				Debug.LogError(_ED3E._E000(260780));
				return;
			}
		}
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.TacticalClothingBuy);
		_E1BB.BuyWear(CS_0024_003C_003E8__locals0.clothing.Offer.Offer._id, CS_0024_003C_003E8__locals0.reqItems, delegate(IResult response)
		{
			if (response.Failed)
			{
				if (CS_0024_003C_003E8__locals0.clothing != null)
				{
					CS_0024_003C_003E8__locals0.clothing.UpdateLock(isLocked: true);
				}
				CS_0024_003C_003E8__locals0._003C_003E4__this._E003();
			}
			else
			{
				CS_0024_003C_003E8__locals0._003C_003E4__this._E1EE.SetAvailableSuites(CS_0024_003C_003E8__locals0.clothing.Offer.Suite.Id);
			}
		});
		CS_0024_003C_003E8__locals0.clothing.UpdateLock(isLocked: false);
		CS_0024_003C_003E8__locals0._E001();
	}

	private async Task _E00A(EBodyModelPart bodyPart, ClothingItem clothing)
	{
		bool flag = !_E013(_E1D1, bodyPart, clothing);
		bool flag2 = !_E013(_E1D2, bodyPart, clothing);
		if (!(flag && flag2))
		{
			clothing.UpdateView(true, true);
			_E1D2[bodyPart] = clothing;
			_E1D1[bodyPart] = clothing;
			_E00C();
			await this._E001;
			_E00D(bodyPart, clothing.Offer.Clothing.Id);
		}
	}

	private async void _E00B(EBodyModelPart bodyPart, ClothingItem item)
	{
		_E1D1.TryGetValue(bodyPart, out var value);
		if (!(value == item))
		{
			_E00C();
			await this._E001;
			if (value != null)
			{
				value.UpdateView(false, null);
			}
			item.UpdateView(true, null);
			_E1D1[bodyPart] = item;
			_E00D(bodyPart, item.Offer.Clothing.Id);
		}
	}

	private void _E00C()
	{
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.TacticalClothingApply);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse1) && _E1F2)
		{
			_E1F5 = !_E1F5;
			_E00E();
		}
	}

	private void _E00D(EBodyModelPart bodyPart, string customizationId)
	{
		_E1F3[bodyPart] = customizationId;
		_E00E();
	}

	private void _E00E()
	{
		_playerModelView.Close();
		_E004();
	}

	private void _E00F(PointerEventData pointerData)
	{
		Task task = _E1F8;
		if (task != null && task.IsCompleted && pointerData.button == PointerEventData.InputButton.Left)
		{
			_rotator.Rotate(pointerData.delta.x);
		}
	}

	private void _E010()
	{
		int num = 0;
		if (_E1D2.TryGetValue(EBodyModelPart.Body, out var value) && _E1F0 != value.Offer.Suite)
		{
			num++;
		}
		else
		{
			value = null;
		}
		if (_E1D2.TryGetValue(EBodyModelPart.Feet, out var value2) && _E1F1 != value2.Offer.Suite)
		{
			num++;
		}
		else
		{
			value2 = null;
		}
		if (num != 0)
		{
			_EBE6[] array = new _EBE6[num];
			int num2 = 0;
			if (value != null)
			{
				array[num2] = value.Offer.Suite;
				num2++;
			}
			if (value2 != null)
			{
				array[num2] = value2.Offer.Suite;
			}
			_E1BB.ApplyWear(array);
		}
	}

	public override void Close()
	{
		_E010();
		_E092.AddItemEvent -= _E002;
		_E092.RemoveItemEvent -= _E001;
		_E092.RefreshItemEvent -= _E000;
		_E1D1.Clear();
		_E1D2.Clear();
		_E1EB.Clear();
		Singleton<_E3DE>.Instance.End();
		if (_E1ED != null)
		{
			_E1ED.Release();
			_E1ED = null;
		}
		_previewHoverArea.Init(null, null);
		base.Close();
	}

	[CompilerGenerated]
	private void _E011(PointerEventData data)
	{
		_E1F2 = true;
	}

	[CompilerGenerated]
	private void _E012(PointerEventData data)
	{
		_E1F2 = false;
	}

	[CompilerGenerated]
	internal static bool _E013(Dictionary<EBodyModelPart, ClothingItem> views, EBodyModelPart bp, ClothingItem newClothingItem)
	{
		views.TryGetValue(bp, out var value);
		if (value != null)
		{
			if (value == newClothingItem)
			{
				return false;
			}
			value.UpdateView(false, false);
		}
		return true;
	}
}
