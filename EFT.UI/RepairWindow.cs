using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.WeaponModding;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class RepairWindow : Window<_EC7C>, IPointerClickHandler, IEventSystemHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public RepairWindow _003C_003E4__this;

		public IItemOwner itemOwner;

		internal void _E000()
		{
			_003C_003E4__this._loader.SetActive(value: true);
		}

		internal void _E001()
		{
			_003C_003E4__this._loader.SetActive(value: false);
		}

		internal void _E002()
		{
			itemOwner.RemoveItemEvent -= _003C_003E4__this.ItemRemovedHandler;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E750 mastered;

		public _E000 CS_0024_003C_003E8__locals1;

		internal float _E000()
		{
			return CS_0024_003C_003E8__locals1._003C_003E4__this._E00B ? ((int)(mastered.LevelProgress * (float)mastered.LevelingThreshold)) : 0;
		}
	}

	[SerializeField]
	private TextMeshProUGUI _masteringLabel;

	[SerializeField]
	private RepairerParametersPanel _repairerParameters;

	[SerializeField]
	private RepairWarningStatusPanel _repairWarningStatusPanel;

	[SerializeField]
	private ItemInfoWindowLabels _itemLabels;

	[SerializeField]
	private CameraImage _cameraImage;

	[SerializeField]
	private DefaultUIButton _repairButton;

	[SerializeField]
	private GameObject _repairProcess;

	[SerializeField]
	private GameObject _loader;

	private WeaponPreview m__E000;

	private Item m__E001;

	private _EB68 m__E002;

	private _EAE6 m__E003;

	private Action _E004;

	private _EBAB _E005;

	private WeaponPreviewPool _E006;

	private MessageWindow _E007;

	private bool _E008;

	private _E8D0 _E009;

	private _EA8D _E00A;

	private bool _E00B => this.m__E003.Examined(this.m__E001);

	protected override void Awake()
	{
		base.Awake();
		_repairButton.OnClick.AddListener(delegate
		{
			_E002().HandleExceptions();
		});
	}

	public void Show(_E8D0 repairController, _EB68 itemContext, _EAE7 inventory, MessageWindow messageWindow, _EAE6 itemController, IEnumerable<_E8B2> repairers, _E74F skills, Action onSelectedAction, WeaponPreviewPool weaponPreviewPool, Action onClosedAction)
	{
		Show(onClosedAction);
		CorrectPosition();
		_E00A = ((itemContext is _EB67 obj && obj.RepairKit != null && obj.RepairKit.Resource.Positive()) ? obj.RepairKit : null);
		_E009 = repairController;
		this.m__E002 = itemContext;
		this.m__E001 = itemContext.Item;
		this.m__E001.UpdateAttributes();
		_E005 = Singleton<_EBA8>.Instance.StructuredItems[this.m__E001.TemplateId];
		_E007 = messageWindow;
		UI.SubscribeEvent(_repairWarningStatusPanel.OnRepairStatusChanged, _E000);
		_repairerParameters.Show(_E009, this.m__E001, inventory, _E00A);
		_E006 = weaponPreviewPool;
		this.m__E000 = _E006.GetWeaponPreview();
		_cameraImage.InitCamera(this.m__E000.WeaponPreviewCamera);
		_itemLabels.Show(this.m__E000);
		_E004 = onSelectedAction;
		this.m__E003 = itemController;
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
		if (weapon != null)
		{
			_E750 mastered = skills.GetMastering(weapon.TemplateId);
			_EB11 obj2 = new _EB11(EItemAttributeId.Mastering);
			string format = _ED3E._E000(247107);
			_E750 obj3 = mastered;
			obj2.Name = string.Format(format, (obj3 == null) ? 1 : (obj3.Level + 1));
			_EB11 obj4 = obj2;
			if (mastered != null)
			{
				obj4.Base = () => _E00B ? ((int)(mastered.LevelProgress * (float)mastered.LevelingThreshold)) : 0;
			}
			TextMeshProUGUI masteringLabel = _masteringLabel;
			string format2 = _ED3E._E000(247147);
			_E750 obj5 = mastered;
			masteringLabel.text = string.Format(format2, (obj5 == null) ? 1 : (obj5.Level + 1));
		}
		IItemOwner itemOwner = this.m__E001.Parent.GetOwner();
		itemOwner.RemoveItemEvent += ItemRemovedHandler;
		UI.AddDisposable(delegate
		{
			itemOwner.RemoveItemEvent -= ItemRemovedHandler;
		});
		_E001();
	}

	private void _E000(ERepairStatusWarning? status)
	{
		_repairButton.Interactable = !status.HasValue || status == ERepairStatusWarning.BrokenItem;
		if (status.HasValue)
		{
			_repairButton.SetDisabledTooltip(status.Value.ToString());
		}
	}

	private void _E001()
	{
		string text = string.Join(_ED3E._E000(197193), _E005.Category.Select((string x) => x.Localized()).ToArray());
		_itemLabels._E000(_ED3E._E000(247489).Localized() + (_E00B ? this.m__E001.Name.Localized() : _ED3E._E000(193009).Localized()));
		_itemLabels._E001(_E00B ? text : _ED3E._E000(91186));
		_itemLabels._E003(this.m__E001.GetTruncatedWeightString() + _ED3E._E000(214011).Localized());
	}

	private async Task _E002()
	{
		RepairableComponent itemComponent = this.m__E001.GetItemComponent<RepairableComponent>();
		if (itemComponent == null)
		{
			Debug.LogError(_ED3E._E000(247546) + this.m__E001.GetType());
		}
		else
		{
			if (_repairerParameters.RepairAmount <= 0f)
			{
				return;
			}
			_repairProcess.SetActive(value: true);
			if (_E008)
			{
				return;
			}
			_E008 = true;
			_E8CF currentRepairer = _repairerParameters.CurrentRepairer;
			float num = _repairerParameters.RepairAmount;
			if (currentRepairer is _E3C6 obj)
			{
				num = (float)obj.GetRepairPrice(num, this.m__E001);
			}
			IResult obj2 = await currentRepairer.RepairItems(new RepairItem(this.m__E001.Id, num), _E00A);
			_E008 = false;
			if (obj2.Succeed)
			{
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.RepairComplete);
				_E857.DisplayMessageNotification(_ED3E._E000(247575).Localized() + _ED3E._E000(18502) + itemComponent.Durability.ToString(_ED3E._E000(229344)));
				if (!(this == null))
				{
					_repairProcess.SetActive(value: false);
					this.m__E001.UpdateAttributes();
					this.m__E001.RaiseRefreshEvent(refreshIcon: true);
					this.m__E002.UpdateView();
					_repairerParameters.UpdateTraderLabels();
					_repairerParameters.UpdateRepairKitLabels();
					_repairerParameters.UpdateBuffState();
					_repairerParameters.UpdateConditionSlider();
				}
			}
		}
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E004?.Invoke();
	}

	public void ItemRemovedHandler(_EAF3 eventArgs)
	{
		if (eventArgs.Status == CommandStatus.Succeed && eventArgs.Item == this.m__E001)
		{
			Close();
		}
	}

	public override void Close()
	{
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuInspectorWindowClose);
		if (_E007.isActiveAndEnabled)
		{
			_E007.Decline();
		}
		if (this.m__E000 != null)
		{
			this.m__E000.Hide();
			_E006.ReturnToPool(this.m__E000);
		}
		_repairerParameters.Close();
		_cameraImage.InitCamera(null);
		_EC45.SetCursor(ECursorType.Idle);
		base.Close();
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E002().HandleExceptions();
	}
}
