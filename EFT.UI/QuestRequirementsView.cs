using EFT.Quests;
using UnityEngine;

namespace EFT.UI;

public class QuestRequirementsView : UIElement
{
	[SerializeField]
	private GameObject _itemPrefab;

	[SerializeField]
	private GameObject _container;

	public void Show(_E933 quest, _E91B conditions)
	{
		ShowGameObject();
		_container.DestroyAllChildren(onlyActive: true);
		foreach (Condition condition in conditions)
		{
			_container.InstantiatePrefab<QuestRequirementView>(_itemPrefab).Init(quest, condition);
		}
	}
}
