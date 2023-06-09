using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using EFT.UI.WeaponModding;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class AddOfferWindow : UIElement, _E640, _E63F, _E641
{
	private enum EAddOfferWarning
	{
		TooLittle,
		TooMuch,
		Pack,
		None
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public AddOfferWindow _003C_003E4__this;

		public RectTransform requirementsWindowContainer;

		public ItemUiContext itemUiContext;

		public Action<RequirementView> _003C_003E9__0;

		internal void _E000(RequirementView arg)
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001
			{
				CS_0024_003C_003E8__locals1 = this,
				arg = arg
			};
			if (_003C_003E4__this._E346 != null)
			{
				if (_003C_003E4__this._E346.gameObject.activeSelf)
				{
					_003C_003E4__this._E346.Close();
				}
			}
			else
			{
				_003C_003E4__this._E346 = UnityEngine.Object.Instantiate(_003C_003E4__this._addRequirementsWindow, requirementsWindowContainer);
			}
			_003C_003E4__this._E346.Show(_003C_003E4__this._E328, _003C_003E4__this._E34B, itemUiContext.ContextMenu, _003C_003E4__this._E081, delegate(HandoverRequirement requirement, _EBAB node)
			{
				foreach (RequirementView item in CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._E343)
				{
					if (item.SelectedRequirement != null && item.SelectedRequirement.TemplateId == node.Data.Id)
					{
						item.UpdateRequirementInformation(null, null);
					}
				}
				CS_0024_003C_003E8__locals0.arg.UpdateRequirementInformation(requirement, node);
			}, _003C_003E4__this._E19F);
			_003C_003E4__this._E346.SetRequirementsCountLabel(_003C_003E4__this._E000);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public RequirementView arg;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000(HandoverRequirement requirement, _EBAB node)
		{
			foreach (RequirementView item in CS_0024_003C_003E8__locals1._003C_003E4__this._E343)
			{
				if (item.SelectedRequirement != null && item.SelectedRequirement.TemplateId == node.Data.Id)
				{
					item.UpdateRequirementInformation(null, null);
				}
			}
			arg.UpdateRequirementInformation(requirement, node);
		}
	}

	private const int _E33D = 3;

	private const int _E33E = 3;

	private const float _E33F = 2f;

	private const float _E340 = 4f;

	private const string _E341 = "Ragfair/No selected item";

	private const string _E342 = "No available item";

	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private ItemInfoWindowLabels _labels;

	[SerializeField]
	private Button _refreshButton;

	[SerializeField]
	private CameraImage _cameraImage;

	[SerializeField]
	private DefaultUIButton _addOfferButton;

	[SerializeField]
	private CustomTextMeshProUGUI _selectedCountLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _categoryLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _myOffersCount;

	[SerializeField]
	private CustomTextMeshProUGUI _durationLabel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private GridView _gridView;

	[SerializeField]
	private ItemMarketPricesPanel _pricesPanel;

	[SerializeField]
	private AddRequirementsWindow _addRequirementsWindow;

	[SerializeField]
	private RequirementView _requirementViewTemplate;

	[SerializeField]
	private RectTransform _requirementsContainer;

	[SerializeField]
	private CustomTextMeshProUGUI _estimatedRequirementsPrice;

	[SerializeField]
	private CustomTextMeshProUGUI _taxPrice;

	[SerializeField]
	private CustomTextMeshProUGUI _perItemLabel;

	[SerializeField]
	private UpdatableToggle _autoSelectSimilar;

	[SerializeField]
	private UpdatableToggle _sellInOnePieceToggle;

	[SerializeField]
	private CanvasGroup _sellInOnePieceGroup;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private GameObject _selectedItemPanel;

	[SerializeField]
	private GameObject _noOfferPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _noItemsText;

	private readonly List<RequirementView> _E343 = new List<RequirementView>();

	private readonly List<EAddOfferWarning> _E344 = new List<EAddOfferWarning>();

	private _EAED _E092;

	private ItemUiContext _E089;

	private _EBA8 _E081;

	private _ECBD _E328;

	private Item _E19F;

	private bool _E345;

	private AddRequirementsWindow _E346;

	private bool _E072;

	private float _E070;

	private double _E347;

	private List<Slot> _E348 = new List<Slot>();

	private WeaponPreviewPool _E13E;

	private WeaponPreview _E130;

	private TaskCompletionSource<object> _E349;

	private bool _E34A;

	private _ECA2 _E133;

	private _EC69 _E34B;

	private int _E000 => Mathf.Clamp(_E343.Count((RequirementView x) => x.SelectedRequirement != null) + 1, 0, 3);

	private int _E001 => _E34B.OfferItemCount;

	private TimeSpan _E002 => TimeSpan.FromHours(_ECBD.Settings.offerDurationTimeInHour);

	private _E79F[] _E003
	{
		get
		{
			List<_E79F> list = new List<_E79F>();
			foreach (RequirementView item in _E343)
			{
				HandoverRequirement selectedRequirement = item.SelectedRequirement;
				if (selectedRequirement != null)
				{
					list.Add(new _E79F
					{
						_tpl = selectedRequirement.TemplateId,
						count = selectedRequirement.IntCount,
						onlyFunctional = selectedRequirement.OnlyFunctional
					});
				}
			}
			return list.ToArray();
		}
	}

	private double _E004
	{
		get
		{
			if (_E345)
			{
				return _E347 / (double)this._E001;
			}
			return _E347;
		}
	}

	private void Awake()
	{
		_captionPanel.AddComponent<UIDragComponent>().Init(base.RectTransform, putOnTop: false);
		_refreshButton.onClick.AddListener(_E007);
		_addOfferButton.OnClick.AddListener(_E001);
		_sellInOnePieceToggle.Bind(delegate(bool arg)
		{
			_E004(arg);
			_E003();
		});
		_closeButton.onClick.AddListener(Close);
		_autoSelectSimilar.onValueChanged.AddListener(_E008);
	}

	public Task Show(_EAED inventoryController, _EA40[] lootItems, RectTransform requirementsWindowContainer, _E796 session, _ECBD ragfair, _EBA8 handbook, ItemUiContext itemUiContext, WeaponPreviewPool weaponPreviewPool)
	{
		ShowGameObject();
		CorrectPosition();
		if (PlayerPrefs.HasKey(_ED3E._E000(242971)))
		{
			base.transform.position = JsonUtility.FromJson<Vector2>(PlayerPrefs.GetString(_ED3E._E000(242971)));
			CorrectPosition();
		}
		else
		{
			base.RectTransform.SetInCenter();
		}
		_E092 = inventoryController;
		_E092.RegisterView(this);
		_E328 = ragfair;
		_E081 = handbook;
		_E089 = itemUiContext;
		_E13E = weaponPreviewPool;
		_E130 = _E13E.GetWeaponPreview();
		if (_E130 == null)
		{
			throw new Exception(_ED3E._E000(242946));
		}
		_EA40 obj = lootItems[0];
		_E34B = new _EC69(session.Profile, obj.Grids[0]);
		_E34B.OnItemSelectionChanged += _E00A;
		_E34B.OnItemChanged += _E009;
		_E34B.AutoSelectSimilar = _autoSelectSimilar.CurrentValue();
		_E133 = _E34B.ItemContext;
		_E328.OnNodePriceUpdated += _E00C;
		_myOffersCount.text = string.Format(_ED3E._E000(243029).Localized() + _ED3E._E000(243015), _E328.MyOffersCount + 1, _E328.GetMaxOffersCount(_E328.MyRating));
		_labels.Show(_E130);
		_pricesPanel.Show(_E328);
		_gridView.Show(obj.Grids[0], _E133, _E092, _E089);
		for (int i = 0; i < 3; i++)
		{
			RequirementView requirementView = UnityEngine.Object.Instantiate(_requirementViewTemplate, _requirementsContainer);
			requirementView.Show(_E089.Tooltip, _E003, delegate(RequirementView arg)
			{
				if (_E346 != null)
				{
					if (_E346.gameObject.activeSelf)
					{
						_E346.Close();
					}
				}
				else
				{
					_E346 = UnityEngine.Object.Instantiate(_addRequirementsWindow, requirementsWindowContainer);
				}
				_E346.Show(_E328, _E34B, itemUiContext.ContextMenu, _E081, delegate(HandoverRequirement requirement, _EBAB node)
				{
					foreach (RequirementView item in _E343)
					{
						if (item.SelectedRequirement != null && item.SelectedRequirement.TemplateId == node.Data.Id)
						{
							item.UpdateRequirementInformation(null, null);
						}
					}
					arg.UpdateRequirementInformation(requirement, node);
				}, _E19F);
				_E346.SetRequirementsCountLabel(this._E000);
			});
			_E343.Add(requirementView);
		}
		bool flag = _E34B.GetFirstAvailableItemOrNull() != null;
		_noItemsText.text = (flag ? _ED3E._E000(243044).Localized() : _ED3E._E000(243066).Localized());
		_E003();
		_E006(hasItemForRagfairSale: false);
		_E328.RefreshItemPrices();
		_E34A = false;
		_E349 = new TaskCompletionSource<object>();
		return _E349.Task;
	}

	private EAddOfferWarning _E000()
	{
		if (_pricesPanel.Minimum > 0f && this._E004 * 2.0 <= (double)_pricesPanel.Minimum && !_E344.Contains(EAddOfferWarning.TooLittle))
		{
			_E344.Add(EAddOfferWarning.TooLittle);
			return EAddOfferWarning.TooLittle;
		}
		if (_pricesPanel.Maximum > 0f && this._E004 >= (double)(_pricesPanel.Maximum * 4f) && !_E344.Contains(EAddOfferWarning.TooMuch))
		{
			_E344.Add(EAddOfferWarning.TooMuch);
			return EAddOfferWarning.TooMuch;
		}
		if (_E345 && this._E001 > 1 && !_E344.Contains(EAddOfferWarning.Pack))
		{
			_E344.Add(EAddOfferWarning.Pack);
			return EAddOfferWarning.Pack;
		}
		return EAddOfferWarning.None;
	}

	private void _E001()
	{
		if (!_E34B.IsAllSelectedItemSame)
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_ED3E._E000(243085), _ED3E._E000(243073), delegate
			{
				_E34B.ClearSelectedList();
			});
			return;
		}
		EAddOfferWarning eAddOfferWarning = _E000();
		if (eAddOfferWarning != EAddOfferWarning.None)
		{
			_E089.ShowMessageWindow(string.Concat(_ED3E._E000(232326), eAddOfferWarning, _ED3E._E000(243157)).Localized(), _E001, delegate
			{
				_E344.Clear();
			});
			return;
		}
		_E34A = true;
		Mathf.CeilToInt((float)_E8CD.CalculateTaxPrice(_E19F, this._E001, _E347, _E345));
		_EB0E.GetMoneySums(_E092.Inventory.Stash.Grid.ContainedItems.Keys);
		_E328.AddOffer(_E345 && this._E001 > 1, _E34B.SelectedItemsIds, this._E003, delegate
		{
			_E34A = false;
			_E349.SetResult(null);
		});
		Close();
	}

	private void _E002(int taxPrice, int rubAmountBefore)
	{
		int num = _EB0E.GetMoneySums(_E092.Inventory.Stash.Grid.ContainedItems.Keys)[ECurrencyType.RUB];
		int num2 = rubAmountBefore - taxPrice;
		Debug.Log(string.Format(_ED3E._E000(243149), taxPrice));
		if (num != num2)
		{
			int num3 = rubAmountBefore - num;
			Debug.LogError(string.Format(_ED3E._E000(243188), taxPrice, num3));
			ItemUiContext.Instance.ShowMessageWindow(string.Format(_ED3E._E000(243188), taxPrice, num3), delegate
			{
			});
		}
	}

	private void _E003()
	{
		if (_E19F == null)
		{
			return;
		}
		_E347 = _E343.Where((RequirementView x) => x.SelectedNode != null).Sum((RequirementView view) => (double)view.SelectedNode.Data.Price * view.SelectedRequirement.PreciseCount);
		_E347 = Math.Ceiling(_E347);
		_estimatedRequirementsPrice.text = _ED3E._E000(243214) + _E347.FormatSeparate(_ED3E._E000(18502)) + _ED3E._E000(27308);
		_EBAB obj = _E081.StructuredItems[_E19F.TemplateId];
		if (obj != null)
		{
			double num = _E8CD.CalculateTaxPrice(_E19F, this._E001, _E347, _E345);
			_categoryLabel.text = string.Join(_ED3E._E000(197193), obj.Category.Select((string x) => x.Localized()).ToArray());
			_taxPrice.text = num.FormatSeparate(_ED3E._E000(18502)) + _ED3E._E000(260492);
			_selectedCountLabel.text = _ED3E._E000(96989) + this._E001.FormatSeparate(_ED3E._E000(18502));
			int num2 = ((_E34B.SelectedItem != null) ? ((_E34B.SelectedItem is _EA12) ? (this._E001 / 120) : (this._E001 - 1)) : 0);
			TimeSpan timeSpan = this._E002.MultiplyByPercent(25 * num2);
			_durationLabel.text = timeSpan.RagfairDateFormatLong();
			Dictionary<ECurrencyType, int> moneySums = _EB0E.GetMoneySums(_E092.Inventory.Stash.Grid.ContainedItems.Keys);
			bool flag = num <= (double)moneySums[ECurrencyType.RUB];
			_addOfferButton.Interactable = flag && num > 0.0 && _E343.Any((RequirementView x) => x.SelectedRequirement != null);
			_sellInOnePieceGroup.SetUnlockStatus(this._E001 > 1);
		}
	}

	private void _E004(bool value)
	{
		_E345 = value;
		_perItemLabel.text = (value ? _ED3E._E000(243240) : _ED3E._E000(243201)).Localized();
	}

	public void OnItemAdded(_EAF2 args)
	{
		if (_E19F != null && args.Status == CommandStatus.Succeed && args.To is _EB20 obj)
		{
			if (args.To.Container.ParentItem == _E19F || args.To.Container.Items.Contains(_E19F))
			{
				_E003();
			}
			if (_E348 != null && _E348.Contains(obj.Slot))
			{
				_E00B(_E19F);
			}
		}
	}

	public void OnItemRemoved(_EAF3 args)
	{
		if (_E19F != null && args.Status == CommandStatus.Succeed)
		{
			if (args.From.Container.ParentItem == _E19F || args.From.Container.Items.Contains(_E19F))
			{
				_E003();
			}
			if (args.From is _EB20 obj && _E348 != null && _E348.Contains(obj.Slot))
			{
				_E00B(_E19F);
			}
		}
	}

	private void _E005()
	{
		bool flag = _E34B.TrySelectFirstAvailableItem();
		if (!flag)
		{
			_E003();
		}
		_E006(flag);
	}

	private void _E006(bool hasItemForRagfairSale)
	{
		_noOfferPanel.SetActive(!hasItemForRagfairSale);
		_selectedItemPanel.SetActive(hasItemForRagfairSale);
	}

	private void Update()
	{
		if (!_E072)
		{
			_E070 += Time.deltaTime;
			if (!(_E070 < 3f))
			{
				_E072 = true;
				_refreshButton.interactable = true;
			}
		}
	}

	private void _E007()
	{
		if (_E072)
		{
			_E070 = 0f;
			_E072 = false;
			_refreshButton.interactable = false;
			if (_E19F != null)
			{
				_pricesPanel.UpdatePrices(_E19F.TemplateId);
			}
		}
	}

	private void _E008(bool value)
	{
		_E34B.AutoSelectSimilar = value;
	}

	private void _E009(Item item)
	{
		if (_E34B.SelectedItem == null)
		{
			_E005();
		}
	}

	private void _E00A(Item item, bool selected)
	{
		Item selectedItem = _E34B.SelectedItem;
		bool hasItemForRagfairSale = selectedItem != null;
		_E006(hasItemForRagfairSale);
		if (item is _EA76)
		{
			_sellInOnePieceToggle.isOn = true;
		}
		else if (this._E001 <= 1)
		{
			_sellInOnePieceToggle.isOn = false;
		}
		_E348 = ((item is _EA40 obj) ? obj.AllSlots.ToList() : new List<Slot>());
		if (!selected)
		{
			_E19F = selectedItem;
			_E003();
			return;
		}
		if (_E19F == null || item.TemplateId != _E19F.TemplateId)
		{
			_E130.Hide();
			_labels._E000(item.Name.Localized());
			_E00B(item);
			_pricesPanel.UpdatePrices(item.TemplateId);
		}
		if (item.CanBeRagfairForbidden && _E19F != null && item.TemplateId != _E19F.TemplateId)
		{
			for (int num = _E343.Count - 1; num >= 0; num--)
			{
				RequirementView requirementView = _E343[num];
				if (requirementView.SelectedRequirement != null && !(requirementView.SelectedRequirement.Item.TemplateId != item.TemplateId))
				{
					requirementView.ResetRequirementInformation();
				}
			}
		}
		_E19F = selectedItem;
		_E003();
	}

	private void _E00B(Item item)
	{
		_E130.SetupItemPreview(item, delegate
		{
			_loader.SetActive(value: true);
		}, delegate
		{
			_loader.SetActive(value: false);
		}, delegate
		{
			_cameraImage.InitCamera(_E130.WeaponPreviewCamera);
		});
	}

	private void _E00C(Dictionary<string, float> dictionary)
	{
		if (_E19F != null && dictionary.ContainsKey(_E19F.TemplateId))
		{
			_E003();
		}
	}

	public override void Close()
	{
		if (!_E34A)
		{
			_E349.SetResult(null);
		}
		_E34B.OnItemSelectionChanged -= _E00A;
		_E34B.OnItemChanged -= _E009;
		_E133?.CloseDependentWindows();
		_E34B.ClearSelectedList();
		_E34B.Dispose();
		_E34B = null;
		if (_E130 != null)
		{
			_E130.Hide();
			_E13E.ReturnToPool(_E130);
			_E130 = null;
		}
		_E328.OnNodePriceUpdated -= _E00C;
		_E092.UnregisterView(this);
		_cameraImage.InitCamera(null);
		_gridView.Hide();
		if (_E130 != null)
		{
			_E130.Hide();
		}
		if (_E346 != null)
		{
			if (_E346.gameObject.activeSelf)
			{
				_E346.Close();
			}
			UnityEngine.Object.Destroy(_E346.gameObject);
			_E346 = null;
		}
		foreach (RequirementView item in _E343)
		{
			item.Close();
		}
		_E343.Clear();
		_E344.Clear();
		_E19F = null;
		Vector3 position = base.transform.position;
		position.x = (int)position.x;
		position.y = (int)position.y;
		PlayerPrefs.SetString(_ED3E._E000(242971), JsonUtility.ToJson(position));
		base.Close();
	}

	[CompilerGenerated]
	private void _E00D(bool arg)
	{
		_E004(arg);
		_E003();
	}

	[CompilerGenerated]
	private void _E00E()
	{
		_E34B.ClearSelectedList();
	}

	[CompilerGenerated]
	private void _E00F()
	{
		_E344.Clear();
	}

	[CompilerGenerated]
	private void _E010()
	{
		_E34A = false;
		_E349.SetResult(null);
	}

	[CompilerGenerated]
	private void _E011()
	{
		_loader.SetActive(value: true);
	}

	[CompilerGenerated]
	private void _E012()
	{
		_loader.SetActive(value: false);
	}

	[CompilerGenerated]
	private void _E013(IResult arg)
	{
		_cameraImage.InitCamera(_E130.WeaponPreviewCamera);
	}
}
