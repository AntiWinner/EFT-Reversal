using System;
using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.Quests;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class TasksScreen : UIElement
{
	private sealed class QuestLocationComparer : IComparer<_E933>
	{
		private readonly string _locationId;

		public QuestLocationComparer(string locationId)
		{
			_locationId = locationId;
		}

		public int Compare(_E933 x, _E933 y)
		{
			if (x == y)
			{
				return 0;
			}
			if (y == null)
			{
				return 1;
			}
			if (x == null)
			{
				return -1;
			}
			string locationId = x.Template.LocationId;
			string locationId2 = y.Template.LocationId;
			if (string.Equals(locationId, locationId2))
			{
				return x.StartTime.CompareTo(y.StartTime);
			}
			if (locationId2 == _locationId)
			{
				return 1;
			}
			if (locationId == _locationId)
			{
				return -1;
			}
			if (locationId2 == "any")
			{
				return 1;
			}
			if (locationId == "any")
			{
				return -1;
			}
			int num = string.CompareOrdinal(locationId.Localized(), locationId2.Localized());
			if (num == 0)
			{
				return x.StartTime.CompareTo(y.StartTime);
			}
			return num;
		}
	}

	private sealed class QuestStatusComparer : IComparer<_E933>
	{
		public int Compare(_E933 x, _E933 y)
		{
			if (x == y)
			{
				return 0;
			}
			if (y == null)
			{
				return 1;
			}
			if (x == null)
			{
				return -1;
			}
			EQuestStatus questStatus = x.QuestStatus;
			EQuestStatus questStatus2 = y.QuestStatus;
			if (questStatus == questStatus2)
			{
				return x.StartTime.CompareTo(y.StartTime);
			}
			if (questStatus2 == EQuestStatus.MarkedAsFailed || questStatus == EQuestStatus.AvailableForFinish)
			{
				return 1;
			}
			if (questStatus2 == EQuestStatus.AvailableForFinish || questStatus == EQuestStatus.MarkedAsFailed)
			{
				return -1;
			}
			return x.StartTime.CompareTo(y.StartTime);
		}
	}

	private sealed class QuestProgressComparer : IComparer<_E933>
	{
		public int Compare(_E933 x, _E933 y)
		{
			if (x == y)
			{
				return 0;
			}
			if (y == null)
			{
				return 1;
			}
			if (x == null)
			{
				return -1;
			}
			float value = x.Progress.current / x.Progress.absolute;
			float num = y.Progress.current / y.Progress.absolute;
			if (!value.ApproxEquals(num))
			{
				return value.CompareTo(num);
			}
			return x.StartTime.CompareTo(y.StartTime);
		}
	}

	private sealed class QuestStringFieldComparer : IComparer<_E933>
	{
		private readonly EQuestsSortType _sortType;

		private string _xField;

		private string _yField;

		public QuestStringFieldComparer(EQuestsSortType sortType)
		{
			_sortType = sortType;
		}

		public int Compare(_E933 x, _E933 y)
		{
			if (x == y)
			{
				return 0;
			}
			if (y == null)
			{
				return 1;
			}
			if (x == null)
			{
				return -1;
			}
			switch (_sortType)
			{
			case EQuestsSortType.Trader:
				_xField = x.Template.TraderId.Localized();
				_yField = y.Template.TraderId.Localized();
				break;
			case EQuestsSortType.Type:
				_xField = x.Template.QuestType.ToStringNoBox();
				_yField = y.Template.QuestType.ToStringNoBox();
				break;
			case EQuestsSortType.Task:
				_xField = x.Template.Name;
				_yField = y.Template.Name;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (!string.Equals(_xField, _yField))
			{
				return string.CompareOrdinal(_xField, _yField);
			}
			return x.StartTime.CompareTo(y.StartTime);
		}
	}

	public sealed class TasksTabController : _EC64<TasksScreen>
	{
		private readonly _EAED _inventoryController;

		private readonly _E935 _questController;

		private readonly _E796 _session;

		private readonly Profile _activeProfile;

		private readonly _E9CE _notesManager;

		public TasksTabController(TasksScreen tasksTab, _EAED inventoryController, _E935 questController, _E796 session, Profile curActiveProfile, _E9CE notesManager)
			: base(tasksTab)
		{
			_inventoryController = inventoryController;
			_questController = questController;
			_session = session;
			_activeProfile = curActiveProfile;
			_notesManager = notesManager;
		}

		public override void Show()
		{
			_E000.Show(_inventoryController, _questController, _session, _activeProfile, _notesManager);
		}
	}

	private const string ANY_LOCATION_KEY = "any";

	[SerializeField]
	private UIAnimatedToggleSpawner _defaultQuestsToggleSpawner;

	[SerializeField]
	private UIAnimatedToggleSpawner _dailyQuestsToggleSpawner;

	[SerializeField]
	private UIAnimatedToggleSpawner _notesToggleSpawner;

	[SerializeField]
	private UIAnimatedToggleSpawner _questItemsToggleSpawner;

	[SerializeField]
	private GameObject _notesPart;

	[SerializeField]
	private GameObject _questItemsPart;

	[SerializeField]
	[Space]
	private NotesTask _notesTaskTemplate;

	[SerializeField]
	private GameObject _notesTaskDescriptionTemplate;

	[SerializeField]
	private GameObject _notesTaskDescription;

	[SerializeField]
	private QuestsFilterPanel _questsFilterPanel;

	[SerializeField]
	private RectTransform _notesTaskContent;

	[SerializeField]
	private NoteWindow _noteWindow;

	[SerializeField]
	private GameObject _notesIsBusySpinner;

	[SerializeField]
	private CanvasGroup _notesCanvasGroup;

	[SerializeField]
	private JournalNote _noteTemplate;

	[SerializeField]
	private RectTransform _notesContent;

	[SerializeField]
	private DefaultUIButton _addNoteButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	[Space]
	private GridView _questRaidGrid;

	[SerializeField]
	private GridView _questStashGrid;

	[SerializeField]
	private CanvasGroup _transferCanvasGroup;

	[SerializeField]
	private DefaultUIButton _transferButtonSpawner;

	[SerializeField]
	private Image _transferIcon;

	[SerializeField]
	private Sprite _downTransferSprite;

	[SerializeField]
	private Sprite _upTransferSprite;

	[SerializeField]
	private Image _warningImage;

	[SerializeField]
	private GameObject _noActiveTasksObject;

	[SerializeField]
	private TextMeshProUGUI _notesCounter;

	[SerializeField]
	private TMP_InputField _searchField;

	private readonly Dictionary<_E933, bool> _questsAvailability = new Dictionary<_E933, bool>();

	private SimpleTooltip _tooltip;

	public static Action<QuestItemView> OnQuestItemSelected;

	private QuestItemView _selectedItemView;

	private _EAED _inventoryController;

	private _E9CE _notesManager;

	private readonly List<JournalNote> _notes = new List<JournalNote>();

	private _EA40 _questRaidItem;

	private _EA40 _questStashItem;

	private string _currentLocationId;

	private _E796 _session;

	private Profile _activeProfile;

	private _E935 _questController;

	private Func<_E933, bool> _questsAdditionalFilter;

	public EQuestsSortType SortType { get; private set; }

	public bool SortAscend { get; private set; }

	private bool SelectedItemInRaidStash
	{
		get
		{
			if (_selectedItemView != null)
			{
				return _inventoryController.Inventory.QuestRaidItems.Grid.Contains(_selectedItemView.Item);
			}
			return false;
		}
	}

	private string RaidQuestWarningText
	{
		get
		{
			if (_E7A3.InRaid)
			{
				return "You need to survive and exit from the location to save and move these items to a special stash for quest items.".Localized();
			}
			return "These items will be lost if you do not survive in the next raid. You can move them to a special stash for quest items.".Localized();
		}
	}

	public event Action OnBackButtonClick;

	private void Awake()
	{
		_defaultQuestsToggleSpawner.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg && !(_questsAdditionalFilter == new Func<_E933, bool>(IsRegularQuest)))
			{
				_questsAdditionalFilter = IsRegularQuest;
				ShowQuests(_questController, _session);
			}
		});
		_dailyQuestsToggleSpawner.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg && !(_questsAdditionalFilter == new Func<_E933, bool>(IsDailyQuest)))
			{
				_questsAdditionalFilter = IsDailyQuest;
				ShowQuests(_questController, _session);
			}
		});
		_notesToggleSpawner.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			_notesPart.SetActive(arg);
			_questItemsPart.SetActive(!arg);
		});
		_questItemsToggleSpawner.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			_questItemsPart.SetActive(arg);
			_notesPart.SetActive(!arg);
		});
		_backButton.OnClick.AddListener(delegate
		{
			this.OnBackButtonClick?.Invoke();
		});
		_addNoteButton.OnClick.AddListener(StartAddNote);
		HoverTrigger orAddComponent = _warningImage.gameObject.GetOrAddComponent<HoverTrigger>();
		orAddComponent.OnHoverStart += delegate
		{
			_tooltip.Show(RaidQuestWarningText);
		};
		orAddComponent.OnHoverEnd += delegate
		{
			_tooltip.Close();
		};
		_transferButtonSpawner.OnClick.AddListener(delegate
		{
			Item item = _selectedItemView.Item;
			_EB22 to = (SelectedItemInRaidStash ? _inventoryController.Inventory.QuestStashItems.Grid.FindLocationForItem(item) : _inventoryController.Inventory.QuestRaidItems.Grid.FindLocationForItem(item));
			_inventoryController.TryRunNetworkTransaction(_EB29.Move(item, to, _inventoryController, simulate: true));
			QuestItemSelectedHandler(null);
		});
		_searchField.onValueChanged.AddListener(OnSearchTextChanged);
	}

	private void Show(_EAED inventoryController, _E935 questController, _E796 session, Profile profile, _E9CE notesManager)
	{
		ItemUiContext instance = ItemUiContext.Instance;
		_tooltip = instance.Tooltip;
		_inventoryController = inventoryController;
		_inventoryController.StopProcesses();
		instance.CloseAllWindows();
		ShowGameObject();
		_notesManager = notesManager;
		_questRaidItem = _inventoryController.Inventory.QuestRaidItems;
		_questStashItem = _inventoryController.Inventory.QuestStashItems;
		_questController = questController;
		_session = session;
		_activeProfile = profile;
		_questsAdditionalFilter = IsRegularQuest;
		CleanNotes();
		CreateNotes();
		OnQuestItemSelected = (Action<QuestItemView>)Delegate.Combine(OnQuestItemSelected, new Action<QuestItemView>(QuestItemSelectedHandler));
		QuestItemSelectedHandler(null);
		_currentLocationId = ((_E7A3.InRaid && Singleton<AbstractGame>.Instantiated) ? Singleton<AbstractGame>.Instance.LocationObjectId : null);
		if (string.IsNullOrEmpty(_currentLocationId))
		{
			SortType = EQuestsSortType.Status;
			SortAscend = true;
		}
		else
		{
			SortType = EQuestsSortType.Location;
			SortAscend = false;
		}
		_questsFilterPanel.Show(this);
		if (_questRaidItem != null)
		{
			_questRaidGrid.Show(_questRaidItem.Grids[0], new _EB64(_questRaidItem, EItemViewType.Quest), _inventoryController, instance);
		}
		if (_questStashItem != null)
		{
			_questStashGrid.Show(_questStashItem.Grids[0], new _EB64(_questStashItem, EItemViewType.Quest), _inventoryController, instance);
		}
		_questItemsToggleSpawner._E001 = true;
		_defaultQuestsToggleSpawner._E001 = true;
		_transferCanvasGroup.SetUnlockStatus(!_E7A3.InRaid);
		UI.AddDisposable(_notesManager.TransactionInProcess.Bind(delegate(bool newValue)
		{
			_notesIsBusySpinner.SetActive(newValue);
			_notesCanvasGroup.alpha = (newValue ? 0.6f : 1f);
			_notesCanvasGroup.interactable = !newValue;
			_addNoteButton.Interactable = !newValue && _notesManager.Notes.Count < 50;
			_notesCounter.text = _notesManager.Notes.Count + "/" + 50;
		}));
	}

	private bool FilterInGame(_E935 questController, _E933 quest)
	{
		if (InRaid())
		{
			return questController.IsQuestForCurrentProfile(quest);
		}
		return true;
	}

	private bool InRaid()
	{
		AbstractGame instance = Singleton<AbstractGame>.Instance;
		if (!(instance is _E7A6))
		{
			return instance is LocalGame;
		}
		return true;
	}

	private void ShowQuests(_E935 questController, _E796 session)
	{
		UI.Dispose();
		List<_E933> list = (from quest in questController.Quests.Where((_E933 x) => x.Template != null && (x.QuestStatus == EQuestStatus.Started || x.QuestStatus == EQuestStatus.AvailableForFinish || x.QuestStatus == EQuestStatus.MarkedAsFailed)).Where(_questsAdditionalFilter)
			where FilterInGame(questController, quest)
			select quest).ToList();
		if (!list.Any())
		{
			_noActiveTasksObject.SetActive(value: true);
			return;
		}
		_noActiveTasksObject.SetActive(value: false);
		switch (SortType)
		{
		case EQuestsSortType.Trader:
			list.Sort(new QuestStringFieldComparer(EQuestsSortType.Trader));
			break;
		case EQuestsSortType.Type:
			list.Sort(new QuestStringFieldComparer(EQuestsSortType.Type));
			break;
		case EQuestsSortType.Task:
			list.Sort(new QuestStringFieldComparer(EQuestsSortType.Task));
			break;
		case EQuestsSortType.Location:
			list.Sort(new QuestLocationComparer(_currentLocationId));
			break;
		case EQuestsSortType.Status:
			list.Sort(new QuestStatusComparer());
			break;
		case EQuestsSortType.Progress:
			list.Sort(new QuestProgressComparer());
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (SortAscend)
		{
			list.Reverse();
		}
		foreach (_E933 item in list)
		{
			bool value = string.IsNullOrEmpty(_currentLocationId) || ((item.Template.LocationId == "any" || item.Template.LocationId == _currentLocationId) && item.Template.PlayerGroup == _activeProfile.Side.ToPlayerGroup());
			if (!_questsAvailability.ContainsKey(item))
			{
				_questsAvailability.Add(item, value);
			}
		}
		list.Sort((_E933 questX, _E933 questY) => _questsAvailability[questY].CompareTo(_questsAvailability[questX]));
		NotesTaskDescriptionShort description = _notesTaskDescription.InstantiatePrefab<NotesTaskDescriptionShort>(_notesTaskDescriptionTemplate);
		UI.AddViewList(list, _notesTaskTemplate, _notesTaskContent, delegate(_E933 quest, NotesTask view)
		{
			view.Show(quest, session, _inventoryController, questController, description, _questsAvailability[quest]);
		});
	}

	public void SetQuestsFilter(EQuestsSortType sortType, bool sortDirection)
	{
		SortType = sortType;
		SortAscend = sortDirection;
		ShowQuests(_questController, _session);
	}

	private bool IsRegularQuest(_E933 quest)
	{
		return !(quest is _E931);
	}

	private bool IsDailyQuest(_E933 quest)
	{
		return quest is _E931;
	}

	private void QuestItemSelectedHandler([CanBeNull] QuestItemView itemView)
	{
		if (_selectedItemView != null)
		{
			_selectedItemView.SetSelectedStatus(value: false);
		}
		_selectedItemView = itemView;
		if (_selectedItemView != null)
		{
			_selectedItemView.SetSelectedStatus(value: true);
		}
		_transferButtonSpawner.gameObject.SetActive(_selectedItemView != null);
		_transferIcon.gameObject.SetActive(_selectedItemView != null);
		if (_selectedItemView != null)
		{
			_transferIcon.sprite = (SelectedItemInRaidStash ? _downTransferSprite : _upTransferSprite);
		}
	}

	private void CreateNotes()
	{
		foreach (_E9CD note in _notesManager.Notes)
		{
			CreateNote(note);
		}
	}

	private void CleanNotes()
	{
		foreach (JournalNote note in _notes)
		{
			UnityEngine.Object.Destroy(note.gameObject);
		}
		_notes.Clear();
	}

	private void StartAddNote()
	{
		_noteWindow.Show(AddNote, delegate
		{
			_noteWindow.Hide();
		});
		async void AddNote(string text)
		{
			_noteWindow.Hide();
			if (!string.IsNullOrEmpty(text))
			{
				using (new _E578<_E9CE>(_notesManager))
				{
					_E9CD note = new _E9CD
					{
						Text = text,
						Time = (float)_E5AD.UtcNowUnix
					};
					await _inventoryController.TryRunNetworkTransaction(_notesManager.AddNote(note, simulate: true));
					CreateNote(note);
				}
			}
		}
	}

	private void CreateNote(_E9CD note)
	{
		GameObject obj = UnityEngine.Object.Instantiate(_noteTemplate.gameObject, _notesContent, worldPositionStays: false);
		obj.transform.SetAsFirstSibling();
		JournalNote component = obj.GetComponent<JournalNote>();
		component.Init(StartEditNote, DestroyNote);
		component.Show(note);
		_notes.Add(component);
	}

	[CanBeNull]
	private JournalNote GetNoteView(_E9CD note)
	{
		return _notes.FirstOrDefault((JournalNote journalNote) => journalNote.Note == note);
	}

	private void StartEditNote(_E9CD note)
	{
		_noteWindow.Show(EditNote, delegate
		{
			_noteWindow.Hide();
		});
		_noteWindow.FillData(note);
		async void EditNote(string text)
		{
			_noteWindow.Hide();
			if (string.IsNullOrEmpty(text))
			{
				DestroyNote(note);
			}
			else
			{
				using (new _E578<_E9CE>(_notesManager))
				{
					note.Text = text;
					int index = _notesManager.Notes.IndexOf(note);
					await _inventoryController.TryRunNetworkTransaction(_notesManager.EditNote(index, note, simulate: true));
					GetNoteView(note).Show(note);
				}
			}
		}
	}

	private async void DestroyNote(_E9CD note)
	{
		using (new _E578<_E9CE>(_notesManager))
		{
			int index = _notesManager.Notes.IndexOf(note);
			await _inventoryController.TryRunNetworkTransaction(_notesManager.DeleteNote(index, simulate: true));
			JournalNote noteView = GetNoteView(note);
			UnityEngine.Object.Destroy(noteView.gameObject);
			_notes.Remove(noteView);
		}
	}

	private void OnSearchTextChanged(string newText)
	{
		foreach (JournalNote note in _notes)
		{
			note.gameObject.SetActive(string.IsNullOrEmpty(newText) || note.Note.Text.IndexOf(newText, StringComparison.OrdinalIgnoreCase) >= 0);
		}
		_notesCounter.text = (string.IsNullOrEmpty(newText) ? (_notes.Count + "/" + 50) : (_notes.Count((JournalNote x) => x.isActiveAndEnabled) + "/" + _notes.Count));
	}

	public override void Close()
	{
		if (_questRaidItem != null)
		{
			_questRaidGrid.Hide();
		}
		if (_questStashItem != null)
		{
			_questStashGrid.Hide();
		}
		OnQuestItemSelected = (Action<QuestItemView>)Delegate.Remove(OnQuestItemSelected, new Action<QuestItemView>(QuestItemSelectedHandler));
		QuestItemSelectedHandler(null);
		_questsFilterPanel.Close();
		_tooltip.Close();
		base.Close();
	}
}
