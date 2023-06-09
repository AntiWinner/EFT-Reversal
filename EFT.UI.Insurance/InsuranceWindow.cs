using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.WeaponModding;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.Insurance;

public sealed class InsuranceWindow : Window<_EC7C>, IPointerClickHandler, IEventSystemHandler, _E640, _E63F, _E641
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E750 mastered;

		public InsuranceWindow _003C_003E4__this;

		internal float _E000()
		{
			return _003C_003E4__this._E00B ? ((int)(mastered.LevelProgress * (float)mastered.LevelingThreshold)) : 0;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public List<_ECB4> itemsToInsure;

		public InsuranceWindow _003C_003E4__this;

		internal void _E000(IResult result)
		{
			if (result.Succeed)
			{
				ItemUiContext.Instance.RedrawContextMenus(itemsToInsure.Select((_ECB4 item) => item.Item.TemplateId));
				_003C_003E4__this.Close();
			}
			_003C_003E4__this.m__E002 = false;
		}
	}

	[SerializeField]
	private TextMeshProUGUI _masteringLabel;

	[SerializeField]
	private InsurerParametersPanel _insurerParameters;

	[SerializeField]
	private ItemInfoWindowLabels _itemLabels;

	[SerializeField]
	private CameraImage _cameraImage;

	[SerializeField]
	private DefaultUIButton _insureButton;

	[SerializeField]
	private GameObject _loader;

	private WeaponPreview m__E000;

	private Item m__E001;

	private bool m__E002;

	private _EAE6 m__E003;

	private IItemOwner m__E004;

	private Action m__E005;

	private _EBAB m__E006;

	private WeaponPreviewPool m__E007;

	private _ECB1 m__E008;

	private MessageWindow m__E009;

	private List<_ECB4> _E00A
	{
		get
		{
			_ECB4 obj = _ECB4.FindOrCreate(this.m__E001);
			return (from x in this.m__E008.GetItemChildren(obj).Flatten(this.m__E008.GetItemChildren).Concat(new _ECB4[1] { obj })
				where this.m__E008.ItemTypeAvailableForInsurance(x) && !this.m__E008.InsuredItems.Contains(x)
				select x).ToList();
		}
	}

	private bool _E00B => this.m__E003.Examined(this.m__E001);

	private _ECB1._E000 _E00C
	{
		get
		{
			List<_ECB4> source = this._E00A;
			int num = (from x in source
				select this.m__E008.InsureSummary[this.m__E008.SelectedInsurerId][x] into x
				where x.Loaded
				select x).Sum((_ECAE x) => x.Amount);
			_ECB1.EInsuranceError? error = null;
			if (num <= 0)
			{
				error = (source.Any() ? _ECB1.EInsuranceError.InvalidItem : _ECB1.EInsuranceError.NothingToInsure);
			}
			return new _ECB1._E000(num, error);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_insureButton.OnClick.AddListener(_E004);
	}

	public void Show(Item item, _EAE7 inventory, _EAE6 itemController, _ECB1 insuranceCompany, MessageWindow messageWindow, [CanBeNull] _E74F skills, Action onSelectedAction, WeaponPreviewPool weaponPreviewPool, Action onClosedAction)
	{
		Show(onClosedAction);
		CorrectPosition();
		this.m__E001 = item;
		this.m__E001.UpdateAttributes();
		this.m__E008 = insuranceCompany;
		this.m__E006 = Singleton<_EBA8>.Instance.StructuredItems[this.m__E001.TemplateId];
		_insurerParameters.Show(inventory, this.m__E008, _E001, skills);
		this.m__E007 = weaponPreviewPool;
		this.m__E000 = this.m__E007.GetWeaponPreview();
		this.m__E009 = messageWindow;
		_cameraImage.InitCamera(this.m__E000.WeaponPreviewCamera);
		_itemLabels.Show(this.m__E000);
		this.m__E005 = onSelectedAction;
		this.m__E003 = itemController;
		this.m__E004 = this.m__E001.Parent.GetOwner();
		this.m__E004.RegisterView(this);
		this.m__E000.SetupItemPreview(this.m__E001, delegate
		{
			_loader.SetActive(value: true);
		}, delegate
		{
			_loader.SetActive(value: false);
		});
		this.m__E000.ResetRotator(-1.5f);
		Weapon weapon = this.m__E001 as Weapon;
		_masteringLabel.gameObject.SetActive(weapon != null);
		if (skills != null && weapon != null)
		{
			_E750 mastered = skills.GetMastering(weapon.TemplateId);
			_EB11 obj = new _EB11(EItemAttributeId.Mastering);
			string format = _ED3E._E000(247107);
			_E750 obj2 = mastered;
			obj.Name = string.Format(format, (obj2 == null) ? 1 : (obj2.Level + 1));
			_EB11 obj3 = obj;
			if (mastered != null)
			{
				obj3.Base = () => this._E00B ? ((int)(mastered.LevelProgress * (float)mastered.LevelingThreshold)) : 0;
			}
			TextMeshProUGUI masteringLabel = _masteringLabel;
			string format2 = _ED3E._E000(247147);
			_E750 obj4 = mastered;
			masteringLabel.text = string.Format(format2, (obj4 == null) ? 1 : (obj4.Level + 1));
		}
		_E002(new _ECB1._E000(-1, _ECB1.EInsuranceError.NothingToInsure));
		_E001();
		_E003();
		this.m__E004.RefreshItemEvent += _E000;
		this.m__E004.AddItemEvent += _E000;
		this.m__E004.RemoveItemEvent += _E000;
		UI.AddDisposable(delegate
		{
			this.m__E004.RefreshItemEvent -= _E000;
			this.m__E004.AddItemEvent -= _E000;
			this.m__E004.RemoveItemEvent -= _E000;
		});
	}

	private void _E000(EventArgs _)
	{
		_E001();
	}

	private void _E001()
	{
		this.m__E008.GetInsurePriceAsync(this._E00A, delegate
		{
			_E002(_E00C);
		});
	}

	private void _E002(_ECB1._E000 priceData)
	{
		if (!(this == null) && !(base.gameObject == null) && base.gameObject.activeSelf)
		{
			_insurerParameters.UpdateLabels(priceData.Price);
			_insurerParameters.UpdateInsureButtonStatus(priceData.Error);
		}
	}

	private void _E003()
	{
		string text = string.Join(_ED3E._E000(197193), this.m__E006?.Category.Select((string x) => x.Localized()).ToArray() ?? new string[0]);
		_itemLabels._E000(_ED3E._E000(233063).Localized() + (this._E00B ? this.m__E001.Name.Localized() : _ED3E._E000(193009).Localized()));
		_itemLabels._E001(this._E00B ? text : _ED3E._E000(91186));
		_itemLabels._E003(this.m__E001.GetTruncatedWeightString() + _ED3E._E000(214011).Localized());
	}

	private void _E004()
	{
		List<_ECB4> itemsToInsure = this._E00A;
		if (!itemsToInsure.Any())
		{
			return;
		}
		if (_E00C.Price > this.m__E008.SelectedInsurer.Settings.Insurance.MinPayment)
		{
			if (this.m__E002)
			{
				return;
			}
			this.m__E002 = true;
			this.m__E008.InsureItems(itemsToInsure, delegate(IResult result)
			{
				if (result.Succeed)
				{
					ItemUiContext.Instance.RedrawContextMenus(itemsToInsure.Select((_ECB4 item) => item.Item.TemplateId));
					Close();
				}
				this.m__E002 = false;
			});
		}
		else
		{
			_E857.DisplayWarningNotification(_ED3E._E000(233747).Localized());
		}
	}

	public void OnItemAdded(_EAF2 eventArgs)
	{
		_EB20 obj = eventArgs.To as _EB20;
		if (eventArgs.Status == CommandStatus.Succeed && obj != null)
		{
			_ECB1._E000 obj2 = _E00C;
			_insurerParameters.UpdateLabels(obj2.Price, updateMoneyToPay: false, updateMoneySums: false);
			_insurerParameters.UpdateInsureButtonStatus(obj2.Error);
			_E005(eventArgs.Item);
		}
	}

	public void OnItemRemoved(_EAF3 eventArgs)
	{
		_EB20 obj = eventArgs.From as _EB20;
		if (eventArgs.Status == CommandStatus.Succeed)
		{
			_ECB1._E000 obj2 = _E00C;
			_insurerParameters.UpdateLabels(obj2.Price, updateMoneyToPay: false, updateMoneySums: false);
			_insurerParameters.UpdateInsureButtonStatus(obj2.Error);
			_E005(eventArgs.Item);
			if (obj != null)
			{
				_E005(obj.Slot.ParentItem);
			}
		}
	}

	private void _E005(Item item)
	{
		if (item == this.m__E001)
		{
			Close();
			return;
		}
		foreach (Item itemChild in this.m__E008.GetItemChildren(this.m__E001))
		{
			if (itemChild == item)
			{
				Close();
			}
		}
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		this.m__E005?.Invoke();
	}

	public override void Close()
	{
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuInspectorWindowClose);
		base.Close();
		this.m__E004?.UnregisterView(this);
		if (this.m__E009.isActiveAndEnabled)
		{
			this.m__E009.Decline();
		}
		if (this.m__E000 != null)
		{
			this.m__E000.Hide();
			this.m__E007.ReturnToPool(this.m__E000);
		}
		_insurerParameters.Close();
		_cameraImage.InitCamera(null);
		_EC45.SetCursor(ECursorType.Idle);
	}

	[CompilerGenerated]
	private bool _E006(_ECB4 x)
	{
		if (this.m__E008.ItemTypeAvailableForInsurance(x))
		{
			return !this.m__E008.InsuredItems.Contains(x);
		}
		return false;
	}

	[CompilerGenerated]
	private _ECAE _E007(_ECB4 x)
	{
		return this.m__E008.InsureSummary[this.m__E008.SelectedInsurerId][x];
	}

	[CompilerGenerated]
	private void _E008()
	{
		_loader.SetActive(value: true);
	}

	[CompilerGenerated]
	private void _E009()
	{
		_loader.SetActive(value: false);
	}

	[CompilerGenerated]
	private void _E00A()
	{
		this.m__E004.RefreshItemEvent -= _E000;
		this.m__E004.AddItemEvent -= _E000;
		this.m__E004.RemoveItemEvent -= _E000;
	}

	[CompilerGenerated]
	private void _E00B(_ECB4[] arg)
	{
		_E002(_E00C);
	}
}
