using UnityEngine;

namespace EFT.UI;

public sealed class QuestsScreen : UIElement
{
	[SerializeField]
	private QuestView _questView;

	[SerializeField]
	private QuestsListView _questsListView;

	public void Show(_E796 backendSession, _EAE6 controller, _E935 questController, _E8B2 trader)
	{
		ShowGameObject();
		_questsListView.Show(backendSession, controller, questController, trader, _questView);
	}

	public override void Close()
	{
		_questsListView.Close();
		_questView.Close();
		base.Close();
	}
}
