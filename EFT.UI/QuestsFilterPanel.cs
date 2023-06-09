using System.Runtime.CompilerServices;

namespace EFT.UI;

public sealed class QuestsFilterPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public QuestsFilterPanel _003C_003E4__this;

		public EQuestsSortType sortType;

		public FilterButton button;

		internal void _E000(FilterButton _)
		{
			_003C_003E4__this._E001(sortType, button);
		}
	}

	public QuestSortToButtonDictionary QuestFilterButtons = new QuestSortToButtonDictionary();

	[CompilerGenerated]
	private bool _E1A0;

	private TasksScreen _E1A1;

	private FilterButton _E1A2;

	private bool _E000
	{
		[CompilerGenerated]
		get
		{
			return _E1A0;
		}
		[CompilerGenerated]
		set
		{
			_E1A0 = value;
		}
	}

	private FilterButton _E001 => QuestFilterButtons[_E1A1.SortType];

	public void Show(TasksScreen tasksScreen)
	{
		UI.Dispose();
		ShowGameObject();
		_E1A1 = tasksScreen;
		foreach (var (sortType, button) in QuestFilterButtons)
		{
			_E000(sortType, button);
		}
		this._E000 = _E1A1.SortAscend;
		_E001(_E1A1.SortType, this._E001);
	}

	private void _E000(EQuestsSortType sortType, FilterButton button)
	{
		button.Show(delegate
		{
			_E001(sortType, button);
		});
		UI.AddDisposable(button);
	}

	private void _E001(EQuestsSortType sortType, FilterButton button)
	{
		if (_E1A2 == button)
		{
			this._E000 = !this._E000;
		}
		_E1A2 = button;
		if (this._E000)
		{
			button.ApplyDescend();
		}
		else
		{
			button.ApplyAscend();
		}
		button.ShowIcon();
		foreach (var (_, filterButton2) in QuestFilterButtons)
		{
			if (!(filterButton2 == button))
			{
				filterButton2.HideIcon();
			}
		}
		_E1A1.SetQuestsFilter(sortType, this._E000);
	}

	public override void Close()
	{
		base.Close();
		_E1A2 = null;
	}
}
