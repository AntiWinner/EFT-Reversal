using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ChatShared;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using JetBrains.Annotations;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class MenuTaskBar : UIInputNode, ISerializationCallbackReceiver, ISupportsPrefabSerialization
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public MenuTaskBar _003C_003E4__this;

		public _EC92 screenManager;

		internal void _E000()
		{
			_003C_003E4__this._E097.ReadWholeEncyclopedia();
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public EMenuType menuType;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000(bool arg)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonBottomBarClick);
			CS_0024_003C_003E8__locals1._003C_003E4__this._E095?.Invoke(menuType, arg);
			if (CS_0024_003C_003E8__locals1.screenManager.CurrentScreenController != null)
			{
				CS_0024_003C_003E8__locals1._003C_003E4__this.OnScreenChanged(CS_0024_003C_003E8__locals1.screenManager.CurrentScreenController.ScreenType);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public BonusController bonusController;

		public MenuTaskBar _003C_003E4__this;

		internal void _E000()
		{
			bonusController.OnBonusChanged -= _003C_003E4__this._E003;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public _E826 productionController;

		public MenuTaskBar _003C_003E4__this;

		internal void _E000()
		{
			productionController.OnProducedItemCountChanged -= _003C_003E4__this.SetProducedItemsCount;
		}

		internal void _E001()
		{
			productionController.OnProductionStatusChanged -= _003C_003E4__this.SetProducedItemsCount;
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public _EC99 matchmakerPlayersController;

		public MenuTaskBar _003C_003E4__this;

		internal void _E000()
		{
			matchmakerPlayersController.OnGroupStatusChanged -= _003C_003E4__this._E000;
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public MenuTaskBar _003C_003E4__this;

		public AnimatedToggle toggleToSelect;

		internal async Task _E000()
		{
			await Task.Yield();
			_003C_003E4__this._E009();
			if (toggleToSelect != null)
			{
				toggleToSelect.ToggleSilent(value: true);
			}
		}
	}

	private const string _E093 = "Not available in raid";

	[SerializeField]
	private GameObject _newInformation;

	[SerializeField]
	private Dictionary<EMenuType, HoverTooltipArea> _hoverTooltipAreas;

	[SerializeField]
	private Dictionary<EMenuType, AnimatedToggle> _toggleButtons;

	[SerializeField]
	private GameObject _failedItemsObject;

	[SerializeField]
	private CustomTextMeshProUGUI _failedItemsLabel;

	[SerializeField]
	private GameObject _producedItemsObject;

	[SerializeField]
	private CustomTextMeshProUGUI _producedItemsLabel;

	[SerializeField]
	private GameObject _newMessagesObject;

	[SerializeField]
	private CustomTextMeshProUGUI _newMessagesLabel;

	[SerializeField]
	private GameObject _newAttachmentsMessagesObject;

	[SerializeField]
	private CustomTextMeshProUGUI _newAttachmentsMessagesLabel;

	[SerializeField]
	private GameObject _newFriendRequestsObject;

	[SerializeField]
	private CustomTextMeshProUGUI _newFriendsRequestsLabel;

	[SerializeField]
	private GameObject _newNodesObject;

	[SerializeField]
	private CustomTextMeshProUGUI _newNodesLabel;

	[SerializeField]
	private LocalizedText[] _labels;

	[SerializeField]
	private Button _readAllButton;

	[SerializeField]
	private GroupPanel _groupPanel;

	private readonly Dictionary<EEftScreenType, EMenuType> _E094 = new Dictionary<EEftScreenType, EMenuType>(_E3A5<EEftScreenType>.EqualityComparer)
	{
		{
			EEftScreenType.MainMenu,
			EMenuType.MainMenu
		},
		{
			EEftScreenType.Hideout,
			EMenuType.Hideout
		},
		{
			EEftScreenType.Inventory,
			EMenuType.Player
		},
		{
			EEftScreenType.Traders,
			EMenuType.Trade
		},
		{
			EEftScreenType.FleaMarket,
			EMenuType.RagFair
		},
		{
			EEftScreenType.EditBuild,
			EMenuType.EditBuild
		},
		{
			EEftScreenType.Handbook,
			EMenuType.Handbook
		},
		{
			EEftScreenType.Settings,
			EMenuType.Settings
		}
	};

	[CompilerGenerated]
	private Action<EMenuType, bool> _E095;

	private _E79D _E096;

	private _EBA8 _E097;

	private _ECBD _E089;

	private _EC99 _E098;

	private int _E099;

	private int _E09A;

	private int _E09B = -1;

	private int _E09C = -1;

	private int _E09D = -1;

	private bool _E09E;

	private bool _E09F;

	private readonly List<Action> _E0A0 = new List<Action>();

	private readonly List<Action> _E0A1 = new List<Action>();

	private readonly List<Action> _E0A2 = new List<Action>();

	[SerializeField]
	[HideInInspector]
	private SerializationData _serializationData;

	public AnimatedToggle ChatToggle => _toggleButtons[EMenuType.Chat];

	private int _E001
	{
		get
		{
			return _E099;
		}
		set
		{
			int num = (_E099 = ((value > 0) ? value : 0));
			_newMessagesObject.SetActive(num > 0);
			_newMessagesLabel.text = num.SubstringIfNecessary();
		}
	}

	private int _E002
	{
		get
		{
			return _E09A;
		}
		set
		{
			int num = (_E09A = ((value > 0) ? value : 0));
			_newAttachmentsMessagesObject.SetActive(num > 0);
			_newAttachmentsMessagesLabel.text = num.SubstringIfNecessary();
		}
	}

	public int FailedItemsCount
	{
		set
		{
			if (_E09D != value)
			{
				_E09D = value;
				_failedItemsObject.SetActive(value > 0);
				_failedItemsLabel.text = value.SubstringIfNecessary();
			}
		}
	}

	public int ProducedItemsCount
	{
		get
		{
			return _E09C;
		}
		set
		{
			if (_E09C != value)
			{
				_E09C = value;
				_producedItemsObject.SetActive(value > 0);
				_producedItemsLabel.text = value.SubstringIfNecessary();
			}
		}
	}

	SerializationData ISupportsPrefabSerialization.SerializationData
	{
		get
		{
			return _serializationData;
		}
		set
		{
			_serializationData = value;
		}
	}

	internal event Action<EMenuType, bool> _E000
	{
		[CompilerGenerated]
		add
		{
			Action<EMenuType, bool> action = _E095;
			Action<EMenuType, bool> action2;
			do
			{
				action2 = action;
				Action<EMenuType, bool> value2 = (Action<EMenuType, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E095, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<EMenuType, bool> action = _E095;
			Action<EMenuType, bool> action2;
			do
			{
				action2 = action;
				Action<EMenuType, bool> value2 = (Action<EMenuType, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E095, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		_EC92 screenManager = _EC92.Instance;
		screenManager.OnScreenChanged += OnScreenChanged;
		foreach (KeyValuePair<EMenuType, AnimatedToggle> toggleButton in _toggleButtons)
		{
			var (menuType, animatedToggle2) = toggleButton;
			animatedToggle2.onValueChanged.AddListener(delegate(bool arg)
			{
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonBottomBarClick);
				_E095?.Invoke(menuType, arg);
				if (screenManager.CurrentScreenController != null)
				{
					OnScreenChanged(screenManager.CurrentScreenController.ScreenType);
				}
			});
		}
		_readAllButton.onClick.AddListener(delegate
		{
			_E097.ReadWholeEncyclopedia();
		});
		if (screenManager.CurrentScreenController != null)
		{
			OnScreenChanged(screenManager.CurrentScreenController.ScreenType);
		}
	}

	private void OnDestroy()
	{
		_toggleButtons.Clear();
		_hoverTooltipAreas.Clear();
		_EC92.Instance.OnScreenChanged -= OnScreenChanged;
	}

	public void PreparingRaid()
	{
		_E00C(_E0A0);
	}

	public void InitSocial(_E79D social)
	{
		if (_E0A0.Count > 0 && _E096 != social)
		{
			_E00C(_E0A0);
		}
		_E096 = social;
		if (_E0A0.Count == 0)
		{
			UI.BindEvent(social.InputFriendsInvitations.ItemsChanged, _E00A);
			_E0A0.Add(_E096.OnLastMessageUpdatedGlobal.Subscribe(_E005));
			_E0A0.Add(_E096.OnDialogueSelectedGlobal.Subscribe(_E006));
			_E004();
			_E0A0.Add(_E7AD._E010.AddLocaleUpdateListener(_E004));
		}
		_E007();
	}

	public void InitHandbook(_EBA8 handbook)
	{
		if (_E0A1.Count > 0 && _E097 != handbook)
		{
			_E00C(_E0A1);
		}
		_E097 = handbook;
		if (_E0A1.Count == 0)
		{
			_E097.OnNodesCountChanged += _E00B;
			_E0A1.Add(delegate
			{
				_E097.OnNodesCountChanged -= _E00B;
			});
		}
		_E00B();
	}

	public void InitRagfair(_ECBD ragfair)
	{
		_E089 = ragfair;
		UI.AddDisposable(_E089.OnStatusChanged.Bind(delegate
		{
			_E002();
		}, _E089.Status));
	}

	public void InitBonusController()
	{
		_E00C(_E0A2);
		BonusController bonusController = Singleton<BonusController>.Instance;
		bonusController.OnBonusChanged += _E003;
		_E0A2.Add(delegate
		{
			bonusController.OnBonusChanged -= _E003;
		});
		_E003(bonusController.HasBonus(EBonusType.UnlockWeaponModification), EBonusType.UnlockWeaponModification);
	}

	public void InitHideout(_E815 hideout)
	{
		_E826 productionController = hideout.ProductionController;
		productionController.OnProducedItemCountChanged += SetProducedItemsCount;
		productionController.OnProductionStatusChanged += SetProducedItemsCount;
		SetProducedItemsCount(productionController);
		UI.AddDisposable(delegate
		{
			productionController.OnProducedItemCountChanged -= SetProducedItemsCount;
		});
		UI.AddDisposable(delegate
		{
			productionController.OnProductionStatusChanged -= SetProducedItemsCount;
		});
	}

	public void InitGroupPanel(_EC99 matchmakerPlayersController, _E79D socialNetwork)
	{
		_E098 = matchmakerPlayersController;
		_E096 = socialNetwork;
		matchmakerPlayersController.OnGroupStatusChanged += _E000;
		_E000();
		UI.AddDisposable(delegate
		{
			matchmakerPlayersController.OnGroupStatusChanged -= _E000;
		});
	}

	private void _E000()
	{
		if (!_E098.Initialized)
		{
			_groupPanel.Close();
			return;
		}
		_groupPanel.Show(_E098, _E096);
		_groupPanel.RaidReadyButtonPressed += _E001;
		UI.AddDisposable(delegate
		{
			_groupPanel.RaidReadyButtonPressed -= _E001;
		});
	}

	private void _E001()
	{
		_E095?.Invoke(EMenuType.GoInRaid, arg2: true);
	}

	public void SetProducedItemsCount(_E826 productionController)
	{
		ProducedItemsCount = productionController.TotalProducedItemsCount;
		FailedItemsCount = productionController.TotalFailedItemsCount;
	}

	public void OnScreenChanged(EEftScreenType eftScreenType)
	{
		_E005 obj = new _E005();
		obj._003C_003E4__this = this;
		obj.toggleToSelect = null;
		obj._E000().HandleExceptions();
		if (_E094.TryGetValue(eftScreenType, out var value))
		{
			obj.toggleToSelect = _toggleButtons[value];
			if ((eftScreenType == EEftScreenType.MainMenu || eftScreenType == EEftScreenType.Hideout) && obj.toggleToSelect != null)
			{
				obj.toggleToSelect.enabled = false;
			}
		}
	}

	private void _E002()
	{
		HoverTooltipArea hoverTooltipArea = _hoverTooltipAreas[EMenuType.RagFair];
		hoverTooltipArea.SetMessageText((_E089 != null) ? new Func<string>(_E089.GetFormattedStatusDescription) : null);
		_ECBD obj = _E089;
		hoverTooltipArea.SetUnlockStatus(obj != null && !obj.Disabled);
	}

	private void _E003(bool isActive, EBonusType bonusType)
	{
		if (bonusType == EBonusType.UnlockWeaponModification)
		{
			_E09E = isActive;
			HoverTooltipArea hoverTooltipArea = _hoverTooltipAreas[EMenuType.EditBuild];
			hoverTooltipArea.SetUnlockStatus(isActive);
			hoverTooltipArea.SetMessageText(isActive ? string.Empty : _ED3E._E000(252465));
		}
	}

	private void _E004()
	{
		LocalizedText[] labels = _labels;
		foreach (LocalizedText localizedText in labels)
		{
			if (localizedText == null || !localizedText.gameObject.activeSelf)
			{
				break;
			}
			localizedText._E001();
		}
	}

	private void _E005(KeyValuePair<_E466, _E464> pair)
	{
		if (_E096.CanReadDialogue(pair.Key))
		{
			if (pair.Value.HasRewards)
			{
				this._E002++;
			}
			else
			{
				this._E001++;
			}
		}
	}

	private void _E006([CanBeNull] _E466 dialogue)
	{
		if (dialogue != null)
		{
			dialogue.New = 0;
			dialogue.AttachmentsNew = 0;
		}
		_E007();
	}

	private void _E007()
	{
		_E466[] source = _E096.Dialogues.Where((_E466 x) => x.Type != EMessageType.GlobalChat).ToArray();
		this._E001 = source.Sum((_E466 item) => item.New);
		this._E002 = source.Sum((_E466 item) => item.AttachmentsNew);
	}

	internal void _E008(bool value)
	{
		if (!value)
		{
			_E009();
			ChatToggle._E001 = false;
		}
		base.gameObject.SetActive(value);
	}

	private void _E009()
	{
		foreach (var (eMenuType2, animatedToggle2) in _toggleButtons)
		{
			if (eMenuType2 != EMenuType.Chat)
			{
				if (eMenuType2 == EMenuType.Hideout || eMenuType2 == EMenuType.MainMenu)
				{
					animatedToggle2.enabled = true;
				}
				animatedToggle2.ToggleSilent(value: false);
			}
		}
	}

	public void SetButtonsAvailable(bool available)
	{
		_E09F = available;
		_newInformation.SetActive(available);
		if (!available)
		{
			ChatToggle._E001 = false;
		}
		foreach (KeyValuePair<EMenuType, HoverTooltipArea> hoverTooltipArea2 in _hoverTooltipAreas)
		{
			_E39D.Deconstruct(hoverTooltipArea2, out var key, out var value);
			EMenuType num = key;
			HoverTooltipArea hoverTooltipArea = value;
			key = num;
			int num2 = (int)key;
			if (num2 != 6 && (num2 != 10 || _E09E))
			{
				hoverTooltipArea.SetUnlockStatus(available);
				hoverTooltipArea.SetMessageText(available ? string.Empty : _ED3E._E000(249325));
			}
		}
		_groupPanel.SetGroupsAvailability(available);
		if (available)
		{
			_E002();
		}
	}

	private void _E00A()
	{
		int count = _E096.InputFriendsInvitations.Count;
		if (_E09B != count)
		{
			_newFriendRequestsObject.SetActive(count > 0);
			_newFriendsRequestsLabel.text = count.SubstringIfNecessary();
			_E09B = count;
		}
	}

	private void _E00B()
	{
		int newNodesCount = _E097.NewNodesCount;
		_newNodesObject.SetActive(newNodesCount > 0);
		_newNodesLabel.text = newNodesCount.SubstringIfNecessary();
	}

	private void _E00C(List<Action> disposableCollection)
	{
		foreach (Action item in disposableCollection)
		{
			item();
		}
		disposableCollection.Clear();
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (!_E09F || !command.IsCommand(ECommand.ToggleInventory))
		{
			return InputNode.GetDefaultBlockResult(command);
		}
		_E095?.Invoke(EMenuType.Player, arg2: true);
		return ETranslateResult.BlockAll;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.Ignore;
	}

	public override void Close()
	{
		_E00C(_E0A0);
		_E00C(_E0A1);
		_E00C(_E0A2);
		base.Close();
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		UnitySerializationUtility.DeserializeUnityObject(this, ref _serializationData);
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		UnitySerializationUtility.SerializeUnityObject(this, ref _serializationData);
	}

	[CompilerGenerated]
	private void _E00D()
	{
		_E097.OnNodesCountChanged -= _E00B;
	}

	[CompilerGenerated]
	private void _E00E(_ECBD.ERagFairStatus status)
	{
		_E002();
	}

	[CompilerGenerated]
	private void _E00F()
	{
		_groupPanel.RaidReadyButtonPressed -= _E001;
	}
}
