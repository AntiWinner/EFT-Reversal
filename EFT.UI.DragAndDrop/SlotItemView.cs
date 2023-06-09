using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public class SlotItemView : GridItemView
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E750 mastered;

		internal float _E000()
		{
			return (mastered != null) ? ((int)(mastered.LevelProgress * 100f)) : 0;
		}
	}

	[SerializeField]
	private Slider _masteringSlider;

	[SerializeField]
	private Image _masteringIcon;

	[SerializeField]
	private DurabilitySlider _durabilitySlider;

	[SerializeField]
	private CustomTextMeshProUGUI _masteringLabel;

	private const float _E05D = 10f;

	private _EB10 _E060;

	protected override string ValueFormat => _ED3E._E000(236402);

	protected override void OnBeingExaminedChanged(bool isBeingExamined)
	{
		base.OnBeingExaminedChanged(isBeingExamined);
		bool active = base.Item is Weapon && !isBeingExamined;
		_masteringSlider.gameObject.SetActive(active);
		_masteringIcon.gameObject.SetActive(active);
		_masteringLabel.gameObject.SetActive(active);
		_durabilitySlider.ShowDurability = (base.Item is Weapon || base.Item.GetItemComponent<KnifeComponent>() != null) && !isBeingExamined;
	}

	public static SlotItemView Create(Item item, _EB68 sourceContext, _EAED inventoryController, IItemOwner itemOwner, ItemUiContext itemUiContext, _E74F skills, _ECB1 insurance)
	{
		SlotItemView slotItemView = ItemViewFactory.CreateFromPool<SlotItemView>(_ED3E._E000(236421)).NewSlotItemView(item, sourceContext, ItemRotation.Horizontal, inventoryController, itemOwner, itemUiContext, skills, insurance);
		slotItemView.Init();
		return slotItemView;
	}

	protected SlotItemView NewSlotItemView(Item item, _EB68 sourceContext, ItemRotation rotation, _EAED inventoryController, IItemOwner itemOwner, ItemUiContext itemUiContext, _E74F skills, _ECB1 insurance)
	{
		NewGridItemView(item, sourceContext, rotation, inventoryController, itemOwner, null, null, itemUiContext, insurance);
		RepairableComponent itemComponent = item.GetItemComponent<RepairableComponent>();
		if (itemComponent != null)
		{
			_durabilitySlider._E000(itemComponent, 10f);
			_E060 = item.Attributes.Find((_EB10 x) => x.Id.Equals(EItemAttributeId.Durability));
			_E060.OnUpdate -= _E000;
			_E060.OnUpdate += _E000;
			if (skills != null)
			{
				_E750 mastered = skills.GetMastering(item.TemplateId);
				_E750 obj = mastered;
				int num = ((obj == null) ? 1 : (obj.Level + 1));
				_EB11 obj2 = new _EB11(EItemAttributeId.Mastering)
				{
					Name = _ED3E._E000(236473) + num + _ED3E._E000(27308),
					Base = () => (mastered != null) ? ((int)(mastered.LevelProgress * 100f)) : 0
				};
				_masteringLabel.text = _ED3E._E000(27314) + num;
				_masteringSlider.value = obj2.Base();
			}
		}
		_E001(base.RectTransform);
		return this;
	}

	public void SetupAddress()
	{
		if (ItemController is _EAED inventoryOwner)
		{
			SetItemBinding(ItemView.GetBindingForAddress(inventoryOwner, base.Item.CurrentAddress));
		}
	}

	private void _E000()
	{
		_durabilitySlider._E001(_E060.Base());
	}

	private static void _E001(RectTransform rectTransform)
	{
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		rectTransform.pivot = new Vector2(0f, 0f);
	}
}
