using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using DG.Tweening;
using EFT.InputSystem;
using EFT.UI;
using EFT.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.Hideout;

public sealed class AreaScreenSubstrate : UIScreen
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E823 producer;

		public AreaScreenSubstrate _003C_003E4__this;

		internal void _E000()
		{
			producer.OnProduceStatusChanged -= _003C_003E4__this._E000;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public AreaScreenSubstrate _003C_003E4__this;

		public ELevelType state;

		public CanvasGroup oldStageGroup;

		internal void _E000()
		{
			if (!(_003C_003E4__this == null) && _003C_003E4__this._E034 == state)
			{
				oldStageGroup.gameObject.SetActive(value: false);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public AreaScreenSubstrate _003C_003E4__this;

		public bool stateChanged;

		public RectTransform container;

		public List<Task> showTasks;

		public Stage stage;

		public ELevelType levelType;

		internal void _E000()
		{
			// Found self-referencing delegate construction. Abort transformation to avoid stack overflow.
			_003C_003E4__this._E000 -= _E000;
			stateChanged = true;
		}

		internal Transform _E001(RelatedData arg)
		{
			return container;
		}

		internal void _E002(RelatedData item, _E833 view)
		{
			showTasks.Add(view.Show(item, stage, levelType, _003C_003E4__this._E02E, _003C_003E4__this._E030, _003C_003E4__this._E031));
		}

		internal bool _E003()
		{
			if (!(container.rect.height >= _003C_003E4__this._maxHeight))
			{
				return Task.WhenAll(showTasks).IsCompleted;
			}
			return true;
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public AreaScreenSubstrate _003C_003E4__this;

		public ELevelType levelType;

		internal bool _E000(KeyValuePair<EPanelType, RelatedData> x)
		{
			AreaScreenSubstrate areaScreenSubstrate = _003C_003E4__this;
			KeyValuePair<EPanelType, RelatedData> keyValuePair = x;
			return areaScreenSubstrate._E008(keyValuePair.Value.Type, levelType);
		}
	}

	public const float FADE_DELAY = 0.3f;

	public const float CONTENT_HEIGHT_DELTA = 80f;

	[SerializeField]
	private float _maxHeight;

	[SerializeField]
	private AreaSubstrateSettings _areaSubstrateSettings;

	[SerializeField]
	private RectTransform _nextStageContainer;

	[SerializeField]
	private CanvasGroup _nextStageGroup;

	[SerializeField]
	private RectTransform _currentStageContainer;

	[SerializeField]
	private CanvasGroup _currentStageGroup;

	[SerializeField]
	private DefaultUIButton _nextLevelButton;

	[SerializeField]
	private DefaultUIButton _currentLevelButton;

	[SerializeField]
	private AreaPanel _areaPanel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private AreaPanelSettings _areaPanelSettings;

	[SerializeField]
	private Image _areaStatusImage;

	[SerializeField]
	private CanvasGroup _buttonCanvasGroup;

	[SerializeField]
	private DefaultUIButton _actionButton;

	[SerializeField]
	private AreaDetails _areaDetailsPanel;

	[SerializeField]
	private LayoutElement _contentLayout;

	private Action<AreaData> _E02C;

	private Action _E02D;

	private AreaData _E02E;

	private int _E02F;

	private Player _E030;

	private _E796 _E031;

	private _EC6D<RelatedData, _E833> _E032;

	private _EC6D<RelatedData, _E833> _E033;

	private ELevelType _E034;

	private bool _E035;

	private bool _E036;

	private _EC76 _E037 = new _EC76();

	[CompilerGenerated]
	private Action _E038;

	private event Action _E000
	{
		[CompilerGenerated]
		add
		{
			Action action = _E038;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E038, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E038;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E038, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		_nextLevelButton.OnClick.AddListener(delegate
		{
			_E004(ELevelType.Next, force: false);
		});
		_currentLevelButton.OnClick.AddListener(delegate
		{
			_E004(ELevelType.Current, force: false);
		});
		_actionButton.OnClick.AddListener(delegate
		{
			_E02C?.Invoke(_E02E);
		});
		_closeButton.onClick.AddListener(_E00A);
	}

	public void Init(Player player, _E796 session, Action<AreaData> onButtonClicked, Action onClosed)
	{
		_E030 = player;
		_E031 = session;
		_E02C = onButtonClicked;
		_E02D = onClosed;
	}

	public void SelectArea(AreaData areaData)
	{
		if (_E02E == areaData)
		{
			_E00A();
			return;
		}
		_E035 = true;
		_E037.Dispose();
		_E02E = areaData;
		_E02F = _E02E.CurrentLevel;
		_E034 = ELevelType.NotSet;
		_E823 producer = Singleton<_E815>.Instance.ProductionController.GetProducer(areaData);
		if (producer != null)
		{
			producer.OnProduceStatusChanged += _E000;
			_E037.AddDisposable(delegate
			{
				producer.OnProduceStatusChanged -= _E000;
			});
			producer.SetErrorsChecked();
		}
		_E037.AddDisposable(_E02E.LevelUpdated.Subscribe(_E002));
		_E037.AddDisposable(_E02E.StatusUpdated.Subscribe(_E003));
		_areaPanel.Close();
		base.CanvasGroup.alpha = 0f;
		ShowGameObject(instant: true);
		_areaPanel.Show(_E02E);
		_E002();
	}

	private void _E000()
	{
		Singleton<_E815>.Instance.ProductionController.GetProducer(_E02E).SetErrorsChecked();
	}

	private ELevelType _E001()
	{
		switch (_E02E.Status)
		{
		case EAreaStatus.NoFutureUpgrades:
		case EAreaStatus.AutoUpgrading:
			return ELevelType.Current;
		case EAreaStatus.LockedToConstruct:
		case EAreaStatus.ReadyToConstruct:
		case EAreaStatus.Constructing:
		case EAreaStatus.ReadyToInstallConstruct:
		case EAreaStatus.ReadyToUpgrade:
		case EAreaStatus.ReadyToInstallUpgrade:
			return ELevelType.Next;
		case EAreaStatus.LockedToUpgrade:
		case EAreaStatus.Upgrading:
			return ELevelType.Current;
		default:
			if (_E034 != 0)
			{
				return _E034;
			}
			return ELevelType.Current;
		}
	}

	private void _E002()
	{
		_E02F = _E02E.CurrentLevel;
		_E003();
	}

	private void _E003()
	{
		if (_E02F == _E02E.CurrentLevel)
		{
			ELevelType state = _E001();
			var (eButtonDisplayStatus, text) = _E02E.ActionButtonStatus;
			_actionButton.enabled = eButtonDisplayStatus != EButtonDisplayStatus.Disabled;
			_actionButton.SetHeaderText(text, 24);
			_actionButton.Interactable = eButtonDisplayStatus == EButtonDisplayStatus.Enabled;
			_areaStatusImage.sprite = _areaPanelSettings[_E02E.Status];
			_E004(state, force: true);
			_areaDetailsPanel.Close();
			_areaDetailsPanel.Show(_E02E);
		}
	}

	private void _E004(ELevelType state, bool force)
	{
		if (state == ELevelType.NotSet || (_E034 == state && !force))
		{
			return;
		}
		UI.DisposeReference(ref _E033);
		UI.DisposeReference(ref _E032);
		_E034 = state;
		CanvasGroup obj = ((_E034 == ELevelType.Current) ? _currentStageGroup : _nextStageGroup);
		CanvasGroup oldStageGroup = ((_E034 == ELevelType.Next) ? _currentStageGroup : _nextStageGroup);
		obj.alpha = 0f;
		obj.gameObject.SetActive(value: true);
		switch (_E034)
		{
		case ELevelType.Current:
			_E033 = new _EC6D<RelatedData, _E833>();
			break;
		case ELevelType.Next:
			_E032 = new _EC6D<RelatedData, _E833>();
			break;
		}
		_EC6D<RelatedData, _E833> obj2 = ((_E034 == ELevelType.Current) ? _E033 : _E032);
		UI.AddDisposable(obj2);
		_E036 = false;
		_E005(obj2, _E034).HandleExceptions();
		bool flag = _E02E.Status == EAreaStatus.NoFutureUpgrades || _E02E.Status == EAreaStatus.AutoUpgrading;
		oldStageGroup.DOFade(0f, 0.3f).OnComplete(delegate
		{
			if (!(this == null) && _E034 == state)
			{
				oldStageGroup.gameObject.SetActive(value: false);
			}
		});
		_nextLevelButton.gameObject.SetActive(!flag);
		if (!flag)
		{
			bool flag2 = _E02E.Status == EAreaStatus.Constructing || _E02E.Status == EAreaStatus.LockedToConstruct || _E02E.Status == EAreaStatus.ReadyToConstruct || _E02E.Status == EAreaStatus.ReadyToInstallConstruct;
			_currentLevelButton.gameObject.SetActive(!flag2);
			string text = (_E02E.DisplayLevel ? string.Format(_ED3E._E000(53834), _ED3E._E000(165317).Localized(), _E02E.CurrentLevel + 1) : _ED3E._E000(165330).Localized());
			_nextLevelButton.SetRawText(text, 24);
		}
	}

	private async Task _E005(_EC6D<RelatedData, _E833> viewList, ELevelType levelType)
	{
		_E002 CS_0024_003C_003E8__locals0 = new _E002();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.levelType = levelType;
		CS_0024_003C_003E8__locals0.stateChanged = false;
		_E038?.Invoke();
		this._E000 += delegate
		{
			// Found self-referencing delegate construction. Abort transformation to avoid stack overflow.
			CS_0024_003C_003E8__locals0._003C_003E4__this._E000 -= CS_0024_003C_003E8__locals0._E000;
			CS_0024_003C_003E8__locals0.stateChanged = true;
		};
		switch (CS_0024_003C_003E8__locals0.levelType)
		{
		case ELevelType.Current:
			CS_0024_003C_003E8__locals0.stage = _E02E.CurrentStage;
			CS_0024_003C_003E8__locals0.container = _currentStageContainer;
			break;
		case ELevelType.Next:
			CS_0024_003C_003E8__locals0.stage = _E02E.NextStage;
			CS_0024_003C_003E8__locals0.container = _nextStageContainer;
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(165313), CS_0024_003C_003E8__locals0.levelType, null);
		}
		Dictionary<EPanelType, RelatedData> dictionary = _E007(CS_0024_003C_003E8__locals0.stage, CS_0024_003C_003E8__locals0.levelType);
		await Task.Yield();
		if (CS_0024_003C_003E8__locals0.stateChanged)
		{
			return;
		}
		if (this == null || _E02E == null)
		{
			this._E000 -= delegate
			{
				// Found self-referencing delegate construction. Abort transformation to avoid stack overflow.
				CS_0024_003C_003E8__locals0._003C_003E4__this._E000 -= CS_0024_003C_003E8__locals0._E000;
				CS_0024_003C_003E8__locals0.stateChanged = true;
			};
			return;
		}
		CS_0024_003C_003E8__locals0.showTasks = new List<Task>();
		await viewList.InitAsync(dictionary.Values, _E009, (RelatedData arg) => CS_0024_003C_003E8__locals0.container, delegate(RelatedData item, _E833 view)
		{
			CS_0024_003C_003E8__locals0.showTasks.Add(view.Show(item, CS_0024_003C_003E8__locals0.stage, CS_0024_003C_003E8__locals0.levelType, CS_0024_003C_003E8__locals0._003C_003E4__this._E02E, CS_0024_003C_003E8__locals0._003C_003E4__this._E030, CS_0024_003C_003E8__locals0._003C_003E4__this._E031));
		});
		if (CS_0024_003C_003E8__locals0.stateChanged)
		{
			return;
		}
		await Task.Yield();
		if (CS_0024_003C_003E8__locals0.stateChanged)
		{
			return;
		}
		await TasksExtensions.WaitUntil(() => CS_0024_003C_003E8__locals0.container.rect.height >= CS_0024_003C_003E8__locals0._003C_003E4__this._maxHeight || Task.WhenAll(CS_0024_003C_003E8__locals0.showTasks).IsCompleted);
		if (!CS_0024_003C_003E8__locals0.stateChanged)
		{
			this._E000 -= delegate
			{
				// Found self-referencing delegate construction. Abort transformation to avoid stack overflow.
				CS_0024_003C_003E8__locals0._003C_003E4__this._E000 -= CS_0024_003C_003E8__locals0._E000;
				CS_0024_003C_003E8__locals0.stateChanged = true;
			};
			if (!base.gameObject.activeInHierarchy)
			{
				_E036 = true;
			}
			else
			{
				await _E006();
			}
		}
	}

	private void OnEnable()
	{
		if (!(this == null) && _E02E != null && _E036)
		{
			_E006().HandleExceptions();
		}
	}

	private async Task _E006()
	{
		if (_E036)
		{
			await Task.Yield();
		}
		if (this == null || _E02E == null)
		{
			return;
		}
		base.CanvasGroup.DOKill();
		base.CanvasGroup.DOFade(1f, 0.3f);
		RectTransform rectTransform = ((_E034 == ELevelType.Current) ? _currentStageContainer : _nextStageContainer);
		((_E034 == ELevelType.Current) ? _currentStageGroup : _nextStageGroup).DOFade(1f, 0.3f);
		Vector2 zero = Vector2.zero;
		zero.y = Math.Min(rectTransform.rect.height + 80f, _maxHeight);
		if (!(zero.y < 1f) && Math.Abs(_contentLayout.preferredHeight - zero.y) > 1f)
		{
			if (_E035)
			{
				_contentLayout.preferredHeight = zero.y;
				_E035 = false;
			}
			else
			{
				_contentLayout.DOKill();
				_contentLayout.DOPreferredSize(zero, 0.3f).SetEase(Ease.OutSine);
			}
		}
	}

	private Dictionary<EPanelType, RelatedData> _E007(Stage stage, ELevelType levelType)
	{
		return stage.Data.ToDictionary((KeyValuePair<EPanelType, Func<RelatedData>> kvp) => kvp.Key, (KeyValuePair<EPanelType, Func<RelatedData>> kvp) => kvp.Value()).Where(delegate(KeyValuePair<EPanelType, RelatedData> x)
		{
			KeyValuePair<EPanelType, RelatedData> keyValuePair3 = x;
			return keyValuePair3.Value != null;
		}).Where(delegate(KeyValuePair<EPanelType, RelatedData> x)
		{
			KeyValuePair<EPanelType, RelatedData> keyValuePair2 = x;
			return keyValuePair2.Value.IsActive;
		})
			.Where(delegate(KeyValuePair<EPanelType, RelatedData> x)
			{
				AreaScreenSubstrate areaScreenSubstrate = this;
				KeyValuePair<EPanelType, RelatedData> keyValuePair = x;
				return areaScreenSubstrate._E008(keyValuePair.Value.Type, levelType);
			})
			.ToDictionary((KeyValuePair<EPanelType, RelatedData> x) => x.Key, (KeyValuePair<EPanelType, RelatedData> x) => x.Value);
	}

	private bool _E008(EPanelType panelType, ELevelType levelType)
	{
		PanelVisibilitySettings panelVisibilitySettings = _areaSubstrateSettings.VisibilitySettings[_E02E.Status];
		switch (levelType)
		{
		case ELevelType.Current:
			return panelVisibilitySettings.CurrentLevel.Contains(panelType);
		case ELevelType.Next:
			if (panelVisibilitySettings.NextLevel == null)
			{
				return false;
			}
			return panelVisibilitySettings.NextLevel.Contains(panelType);
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(165313), levelType, null);
		}
	}

	private _E833 _E009(RelatedData data)
	{
		return _areaSubstrateSettings.Panels[data.Type];
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command == ECommand.ToggleGoggles)
		{
			return InputNode.GetDefaultBlockResult(command);
		}
		return ETranslateResult.Ignore;
	}

	private void _E00A()
	{
		_E02D?.Invoke();
	}

	public override void Close()
	{
		_E036 = false;
		_E037.Dispose();
		_areaPanel.Close();
		_areaDetailsPanel.Close();
		base.Close();
		_E02E = null;
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuInspectorWindowClose);
	}

	[CompilerGenerated]
	private void _E00B()
	{
		_E004(ELevelType.Next, force: false);
	}

	[CompilerGenerated]
	private void _E00C()
	{
		_E004(ELevelType.Current, force: false);
	}

	[CompilerGenerated]
	private void _E00D()
	{
		_E02C?.Invoke(_E02E);
	}
}
