using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class AssembleModPanel : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _EAED inventoryController;

		public AssembleModPanel _003C_003E4__this;

		internal void _E000()
		{
			inventoryController.ExamineEvent -= _003C_003E4__this._E000;
		}
	}

	private const int _E090 = 50;

	private const int _E091 = 10;

	[SerializeField]
	private AssembleBuildItemView _itemView;

	[SerializeField]
	private TextMeshProUGUI _itemName;

	[SerializeField]
	private TextMeshProUGUI _itemCategory;

	private _EBAB _E086;

	private Item _E085;

	private _EAED _E092;

	public void Show(_EBAB node, Item item, _EAED inventoryController, ItemUiContext itemUiContext, _ECB1 insuranceCompany)
	{
		_E086 = node;
		_E085 = item;
		_E092 = inventoryController;
		ShowGameObject();
		_itemView.Show(item, ItemRotation.Horizontal, inventoryController, itemUiContext, insuranceCompany);
		_E001();
		inventoryController.ExamineEvent += _E000;
		UI.AddDisposable(delegate
		{
			inventoryController.ExamineEvent -= _E000;
		});
		_itemCategory.text = string.Join(_ED3E._E000(197193), _E086.Category.Select((string x) => x.Localized()).ToArray());
	}

	private void _E000(_EAF6 args)
	{
		_E001();
	}

	private void _E001()
	{
		string @string = (_E092.Examined(_E085) ? Singleton<_E63B>.Instance.BriefItemName(_E085, _E085.Name.Localized()) : _ED3E._E000(193009).Localized());
		@string = @string.SubstringIfNecessary(50);
		string text = string.Format(_ED3E._E000(250183), @string, _E085.StackObjectsCount);
		MedKitComponent itemComponent = _E085.GetItemComponent<MedKitComponent>();
		if (itemComponent != null)
		{
			text += string.Format(_ED3E._E000(250265), itemComponent.HpResource, _ED3E._E000(250281), itemComponent.MaxHpResource);
		}
		else
		{
			RepairableComponent[] array = _E085.GetItemComponentsInChildren<RepairableComponent>().ToArray();
			if (array.Length != 0)
			{
				double num = Math.Round(array.Sum((RepairableComponent x) => x.Durability), 1);
				double num2 = Math.Round(array.Sum((RepairableComponent x) => x.MaxDurability), 1);
				string arg = ((num * 100.0 / num2 <= 10.0) ? _ED3E._E000(250331) : _ED3E._E000(250273));
				text += string.Format(_ED3E._E000(250323), num, arg, num2);
			}
		}
		_itemName.text = text;
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
	}

	public override void Close()
	{
		_itemView.Kill();
		base.Close();
	}
}
