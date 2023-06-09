using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class CharacteristicsPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000<_E0B7> where _E0B7 : CompactCharacteristicPanel
	{
		public _EB10 attribute;

		internal bool _E000(_EB10 attr)
		{
			return attribute.Id.Equals(attr.Id);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public float allDurability;

		internal float _E000()
		{
			return allDurability;
		}

		internal string _E001()
		{
			return Math.Round(allDurability, 1).ToString(CultureInfo.InvariantCulture);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public CharacteristicsPanel _003C_003E4__this;

		public List<_EB10> newAttributes;

		internal bool _E000(_EB10 attribute)
		{
			_E003 CS_0024_003C_003E8__locals0 = new _E003
			{
				attribute = attribute
			};
			return _003C_003E4__this._E085.Attributes.Find((_EB10 a) => a.Id.Equals(CS_0024_003C_003E8__locals0.attribute.Id)) == null;
		}

		internal bool _E001(_EB10 attribute)
		{
			_E004 CS_0024_003C_003E8__locals0 = new _E004
			{
				attribute = attribute
			};
			if (_003C_003E4__this._E085.Attributes.Find((_EB10 a) => a.Id.Equals(CS_0024_003C_003E8__locals0.attribute.Id)) == null)
			{
				return newAttributes.Find((_EB10 a) => a.Id.Equals(CS_0024_003C_003E8__locals0.attribute.Id)) == null;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public _EB10 attribute;

		internal bool _E000(_EB10 a)
		{
			return a.Id.Equals(attribute.Id);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public _EB10 attribute;

		internal bool _E000(_EB10 a)
		{
			return a.Id.Equals(attribute.Id);
		}

		internal bool _E001(_EB10 a)
		{
			return a.Id.Equals(attribute.Id);
		}
	}

	[SerializeField]
	private GameObject _infoButtonPanel;

	[SerializeField]
	private GameObject _characteristics;

	[SerializeField]
	private Button _infoButton;

	[SerializeField]
	private Button _charPanelButton;

	[SerializeField]
	private CharacteristicPanel _charPanelTemplate;

	[SerializeField]
	private CharacteristicPanel _charDurability;

	[SerializeField]
	private CharacteristicPanel _charWeight;

	[SerializeField]
	private CompactCharacteristicPanel _compactCharTemplate;

	[SerializeField]
	private Transform _charPanelContainer;

	[SerializeField]
	private bool _expanded;

	private Item _E085;

	private Item _E16E;

	private _EB11 _E16F;

	private SimpleTooltip _E02A;

	private _EC79<_EB10, CharacteristicPanel> _E13C;

	private _EC79<_EB10, CompactCharacteristicPanel> _E13B;

	private _E3F3 _E170 = new _E3F3(0f, 0f, 0f, 28f);

	private void Awake()
	{
		base.gameObject.AddComponent<UIDragComponent>().Init(base.RectTransform, putOnTop: false);
		_E003(_expanded);
		_infoButton.onClick.AddListener(_E002);
		_charPanelButton.onClick.AddListener(_E002);
	}

	public void Show([CanBeNull] Item item, Item compareItem)
	{
		UI.Dispose();
		_E085 = item;
		_E16E = compareItem;
		if (item == null || item.Attributes.Count == 0)
		{
			Close();
			return;
		}
		_E02A = ItemUiContext.Instance.Tooltip;
		ShowGameObject();
		_E000(item, compareItem);
	}

	private void _E000([CanBeNull] Item item, Item compareItem)
	{
		if (_E13C != null && _E13B != null)
		{
			_E13C.Dispose();
			_E13B.Dispose();
			_charDurability.Close();
			_charWeight.Close();
		}
		if (item == null)
		{
			return;
		}
		if (item.Attributes.Count > 0)
		{
			item.UpdateAttributes();
			List<_EB10> changedList = compareItem.Attributes.Where((_EB10 x) => x.DisplayType() == EItemAttributeDisplayType.FullBar).ToList();
			List<_EB10> changedList2 = compareItem.Attributes.Where((_EB10 x) => x.DisplayType() == EItemAttributeDisplayType.Compact).ToList();
			List<_EB10> list = new List<_EB10>();
			List<_EB10> list2 = new List<_EB10>();
			foreach (_EB10 attribute in item.Attributes)
			{
				switch (attribute.DisplayType())
				{
				case EItemAttributeDisplayType.Compact:
					list2.Add(attribute);
					break;
				case EItemAttributeDisplayType.FullBar:
					list.Add(attribute);
					break;
				}
			}
			_E13C = new _EC79<_EB10, CharacteristicPanel>(list, _charPanelTemplate, _charPanelContainer, delegate(_EB10 attribute, CharacteristicPanel full)
			{
				full.Show(attribute, _E02A);
			});
			_E13B = new _EC79<_EB10, CompactCharacteristicPanel>(list2, _compactCharTemplate, _charPanelContainer, delegate(_EB10 attribute, CompactCharacteristicPanel compact)
			{
				compact.Show(attribute, _E02A);
			});
			_E006(_E13C, changedList);
			_E006(_E13B, changedList2);
		}
		if (item != null)
		{
			if (!(item is Weapon))
			{
				if (item is _EA16)
				{
					RepairableComponent[] source = (from x in item.GetItemComponentsInChildren<ArmorComponent>()
						select x.Repairable).ToArray();
					float allDurability = source.Sum((RepairableComponent x) => x.Durability);
					float num = source.Sum((RepairableComponent x) => x.MaxDurability);
					_charDurability.Show(new _EB11(EItemAttributeId.ArmorPoints)
					{
						Name = EItemAttributeId.ArmorPoints.GetName(),
						Base = () => allDurability,
						StringValue = () => Math.Round(allDurability, 1).ToString(CultureInfo.InvariantCulture),
						Range = new Vector2(0f, num),
						DisplayType = () => EItemAttributeDisplayType.Special
					}, _E02A, examined: true, (int)num);
				}
			}
			else
			{
				_charDurability.Show(item.Attributes.Find((_EB10 x) => x.Id.Equals(EItemAttributeId.Durability)), _E02A);
			}
		}
		_charDurability.CompareWith(null);
		Vector2 range = new Vector2(0f, 15f);
		string text = EItemAttributeId.Weight.GetName();
		_EB11 itemAttribute = new _EB11(EItemAttributeId.Weight)
		{
			Name = text,
			Range = range,
			Base = item.GetSingleItemTotalWeight,
			StringValue = item.GetTruncatedWeightString,
			LessIsGood = true
		};
		_E16F = new _EB11(EItemAttributeId.Weight)
		{
			Name = text,
			Range = range,
			Base = _E16E.GetSingleItemTotalWeight,
			StringValue = _E16E.GetTruncatedWeightString,
			LessIsGood = true
		};
		_charWeight.Show(itemAttribute, _E02A);
		_charWeight.CompareWith(_E16F);
	}

	public void ResetCompare(Slot slot)
	{
		int index = ((_EA40)_E085).AllSlots.ToList().IndexOf(slot);
		Slot slot2 = ((_EA40)_E16E).AllSlots.ToList()[index];
		if (slot.ContainedItem != null)
		{
			slot2.Add(slot.ContainedItem.CloneItem());
		}
		else if (slot2.ContainedItem != null)
		{
			slot2.RemoveItem();
		}
		_E001();
		slot2.ApplyContainedItem();
		_E16E.UpdateAttributes();
		_E16F.Update();
	}

	public void OnCompareRequired(Item compareWith, Slot slot)
	{
		int index = ((_EA40)_E085).AllSlots.ToList().IndexOf(slot);
		Slot slot2 = ((_EA40)_E16E).AllSlots.ToList()[index];
		if (compareWith != null)
		{
			slot2.Add(compareWith.CloneItem());
		}
		else if (slot2.ContainedItem != null)
		{
			slot2.RemoveItem();
		}
		_E001();
		slot2.ApplyContainedItem();
		_E16E.UpdateAttributes();
		_E16F.Update();
	}

	private void _E001()
	{
		List<_EB10> newAttributes = _E16E.Attributes.FindAll((_EB10 attribute) => _E085.Attributes.Find((_EB10 a) => a.Id.Equals(attribute.Id)) == null);
		foreach (_EB10 item in newAttributes)
		{
			if (item.DisplayType() == EItemAttributeDisplayType.FullBar)
			{
				_E13C.Add(item.Clone());
			}
		}
		foreach (_EB10 item2 in _E13C.Keys.FindAll((_EB10 attribute) => _E085.Attributes.Find((_EB10 a) => a.Id.Equals(attribute.Id)) == null && newAttributes.Find((_EB10 a) => a.Id.Equals(attribute.Id)) == null))
		{
			_E13C.Remove(item2);
		}
	}

	private void _E002()
	{
		_E003(!_expanded);
	}

	private void _E003(bool expanded)
	{
		_expanded = expanded;
		_infoButtonPanel.gameObject.SetActive(!expanded);
		_characteristics.gameObject.SetActive(expanded);
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)base.transform);
		CorrectPosition(_E170);
	}

	public override void Close()
	{
		SimpleTooltip simpleTooltip = _E02A;
		if ((object)simpleTooltip != null && simpleTooltip.isActiveAndEnabled)
		{
			_E02A.Close();
		}
		foreach (Transform item in _charPanelContainer)
		{
			item.gameObject.SetActive(value: false);
		}
		base.Close();
	}

	[CompilerGenerated]
	private void _E004(_EB10 attribute, CharacteristicPanel full)
	{
		full.Show(attribute, _E02A);
	}

	[CompilerGenerated]
	private void _E005(_EB10 attribute, CompactCharacteristicPanel compact)
	{
		compact.Show(attribute, _E02A);
	}

	[CompilerGenerated]
	internal static void _E006<_E0B7>(_EC79<_EB10, _E0B7> viewList, List<_EB10> changedList) where _E0B7 : CompactCharacteristicPanel
	{
		foreach (KeyValuePair<_EB10, _E0B7> view in viewList)
		{
			_E39D.Deconstruct(view, out var key, out var value);
			_EB10 attribute = key;
			_E0B7 val = value;
			_EB10 obj = changedList.FirstOrDefault((_EB10 attr) => attribute.Id.Equals(attr.Id));
			if (obj != null)
			{
				val.CompareWith(obj);
			}
		}
	}
}
