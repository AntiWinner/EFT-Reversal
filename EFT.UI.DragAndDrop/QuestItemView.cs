using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.DragAndDrop;

public sealed class QuestItemView : StaticGridItemView
{
	[SerializeField]
	private GameObject _selectedQuestItemBorder;

	public static QuestItemView Create(Item item, _EB68 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		QuestItemView questItemView = ItemViewFactory.CreateFromPool<QuestItemView>(_ED3E._E000(236071))._E000(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		questItemView.Init();
		return questItemView;
	}

	private QuestItemView _E000(Item item, _EB68 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		NewGridItemView(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		return this;
	}

	protected override void OnClick(PointerEventData.InputButton button, Vector2 position, bool doubleClick)
	{
		switch (button)
		{
		case PointerEventData.InputButton.Left:
			TasksScreen.OnQuestItemSelected(this);
			if (doubleClick)
			{
				base.NewContextInteractions.ExecuteInteraction(EItemInfoButton.Inspect);
			}
			break;
		case PointerEventData.InputButton.Right:
			ShowContextMenu(position);
			break;
		}
	}

	public void SetSelectedStatus(bool value)
	{
		_selectedQuestItemBorder.SetActive(value);
	}
}
