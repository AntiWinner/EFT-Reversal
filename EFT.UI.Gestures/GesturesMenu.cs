using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.Gestures;

public sealed class GesturesMenu : UIInputNode
{
	public readonly struct _E000
	{
		public readonly ECommand Command;

		public readonly int Index;

		public readonly string Caption;

		public _E000(ECommand command, int index, string caption)
		{
			Command = command;
			Index = index;
			Caption = caption;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public bool isSituational;

		public GesturesMenu _003C_003E4__this;

		internal void _E000(GestureBaseItem._E000 x)
		{
			_003C_003E4__this._E002(x, isSituational);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public GesturesMenu _003C_003E4__this;

		public int index;

		internal void _E000(ECommand command)
		{
			_003C_003E4__this._E006(command, index);
			_003C_003E4__this._E005();
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public int index;

		internal bool _E000(KeyValuePair<ECommand, int> x)
		{
			return x.Value == index;
		}
	}

	public static readonly _ECED<_E000> OnBindUpdated = new _ECED<_E000>();

	[SerializeField]
	[Space]
	private Transform _audioContainer;

	[SerializeField]
	private GesturesAudioItem _commonGroupTemplate;

	[SerializeField]
	private GesturesAudioItem _situationalGroupTemplate;

	private readonly List<GesturesAudioItem> _E0C0 = new List<GesturesAudioItem>();

	[Space]
	[SerializeField]
	private Transform _gestureContainer;

	[SerializeField]
	private GesturesMenuItem _gestureItemTemplate;

	private readonly List<GesturesMenuItem> _E0C1 = new List<GesturesMenuItem>();

	[SerializeField]
	[Space]
	private GesturesBindPanel _gesturesBindPanel;

	[SerializeField]
	private SimpleContextMenu _gesturesContextMenu;

	[SerializeField]
	[Space]
	private Sprite _defaultSubItemBack;

	[SerializeField]
	private Sprite _selectedSubItemBack;

	[SerializeField]
	private Sprite _defaultGestureBack;

	[SerializeField]
	private Sprite _selectedGestureBack;

	[SerializeField]
	private ColorMap _colorMap;

	[NonSerialized]
	public readonly _ECEC OnShow = new _ECEC();

	[NonSerialized]
	public readonly _ECEC OnClose = new _ECEC();

	[NonSerialized]
	public readonly _ECED<(int actionId, bool aggressive)> OnItemSelected = new _ECED<(int, bool)>();

	private readonly List<GestureBaseItem> _E0C2 = new List<GestureBaseItem>();

	private GesturesAudioItem _E0C3;

	private _ECAA _E0C4;

	private HashSet<EPhraseTrigger> _E0C5;

	[CompilerGenerated]
	private _ECAB _E0C6;

	[CompilerGenerated]
	private bool _E0C7;

	[CompilerGenerated]
	private bool _E0C8;

	public _ECAB Binds
	{
		[CompilerGenerated]
		get
		{
			return _E0C6;
		}
		[CompilerGenerated]
		private set
		{
			_E0C6 = value;
		}
	}

	public bool IsShowing
	{
		[CompilerGenerated]
		get
		{
			return _E0C7;
		}
		[CompilerGenerated]
		private set
		{
			_E0C7 = value;
		}
	}

	public bool HideOnlySituationalGroups
	{
		[CompilerGenerated]
		private get
		{
			return _E0C8;
		}
		[CompilerGenerated]
		set
		{
			_E0C8 = value;
		}
	}

	private void Awake()
	{
		_commonGroupTemplate.gameObject.SetActive(value: false);
		_situationalGroupTemplate.gameObject.SetActive(value: false);
		_gestureItemTemplate.gameObject.SetActive(value: false);
	}

	public void Init(_ECAA gesturesStorage, HashSet<EPhraseTrigger> availablePhrases)
	{
		_E0C4 = gesturesStorage;
		_E0C5 = availablePhrases;
		Binds = _E0C4.GetCommands();
		if (_E0C0.IsNullOrEmpty())
		{
			_E008();
		}
		if (_E0C1.IsNullOrEmpty())
		{
			_E007();
		}
	}

	public void Show()
	{
		ShowGameObject();
		IsShowing = true;
		Color defaultAudioColor = _colorMap[_ED3E._E000(229605)];
		Color selectedAudioColor = _colorMap[_ED3E._E000(229634)];
		foreach (GesturesAudioItem item in _E0C0)
		{
			if (HideOnlySituationalGroups && item.HasOnlySituational)
			{
				item.Close();
			}
			else
			{
				item.Show(_defaultSubItemBack, _selectedSubItemBack, defaultAudioColor, selectedAudioColor, Binds, _E0C5);
			}
		}
		foreach (GesturesMenuItem item2 in _E0C1)
		{
			item2.Show(_defaultGestureBack, _selectedGestureBack, Binds);
		}
		_gesturesBindPanel.Show(Binds, _colorMap, _E0C5);
		UI.AddDisposable(_gesturesBindPanel.OnItemSelected.Subscribe(delegate(int item)
		{
			OnItemSelected.Invoke((item, false));
		}));
		UI.AddDisposable(_gesturesContextMenu.OnMenuClosed.Subscribe(_E005));
		OnShow.Invoke();
	}

	public void SetPhraseActive(EPhraseTrigger phrase, bool active)
	{
		foreach (GesturesAudioItem item in _E0C0)
		{
			foreach (GesturesAudioSubItem audioSubItem in item.AudioSubItems)
			{
				if (audioSubItem.PhraseTrigger != phrase)
				{
					continue;
				}
				audioSubItem.gameObject.SetActive(active);
				return;
			}
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		switch (command)
		{
		case ECommand.MumbleEnd:
		case ECommand.MumbleToggle:
			return ETranslateResult.Ignore;
		default:
			return InputNode.GetDefaultBlockResult(command);
		case ECommand.Escape:
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			Close();
			return InputNode.GetDefaultBlockResult(command);
		}
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		axes = null;
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.ShowCursor;
	}

	public void CloseWithSelected()
	{
		GestureBaseItem gestureBaseItem = _E0C2.FirstOrDefault((GestureBaseItem x) => x.IsUnderPointer);
		int? num = (((object)gestureBaseItem != null) ? new int?(gestureBaseItem.ItemIndex) : _gesturesBindPanel.Items.FirstOrDefault((GesturesBindItem x) => x.IsUnderPointer)?.ItemIndex);
		if (num.HasValue)
		{
			OnItemSelected.Invoke((num.Value, false));
		}
		Close();
	}

	public override void Close()
	{
		if (_E0C0 != null)
		{
			foreach (GesturesAudioItem item in _E0C0)
			{
				item.Close();
			}
		}
		if (_E0C1 != null)
		{
			foreach (GesturesMenuItem item2 in _E0C1)
			{
				item2.Close();
			}
		}
		_gesturesBindPanel.Close();
		_gesturesContextMenu.Close();
		OnClose.Invoke();
		IsShowing = false;
		base.Close();
	}

	private void _E000(EGesture gesture)
	{
		GesturesMenuItem gesturesMenuItem = UnityEngine.Object.Instantiate(_gestureItemTemplate, _gestureContainer);
		gesturesMenuItem.gameObject.name = gesture.ToString();
		gesturesMenuItem.Gesture = gesture;
		if (EFTHardSettings.Instance.StaticIcons.GestureSprites.TryGetValue(gesture, out var value))
		{
			gesturesMenuItem.Icon = value;
		}
		gesturesMenuItem.gameObject.SetActive(value: true);
		_E0C1.Add(gesturesMenuItem);
		_E0C2.Add(gesturesMenuItem);
		gesturesMenuItem.OnPointerClicked.Subscribe(delegate(GestureBaseItem._E000 x)
		{
			_E002(x, isSituational: false);
		});
	}

	private GesturesAudioItem _E001(string localizationKey, GesturesAudioItem groupTemplate, params EPhraseTrigger[] phrases)
	{
		GesturesAudioItem gesturesAudioItem = UnityEngine.Object.Instantiate(groupTemplate, _audioContainer);
		gesturesAudioItem.gameObject.name = localizationKey;
		gesturesAudioItem.LocalizationKey = localizationKey;
		foreach (EPhraseTrigger phrase in phrases)
		{
			bool isSituational = GesturesQuickPanel.IsSituationalPhrase(phrase);
			GestureBaseItem gestureBaseItem = gesturesAudioItem.CreateNewPhrase(phrase, isSituational);
			gestureBaseItem.OnPointerClicked.Subscribe(delegate(GestureBaseItem._E000 x)
			{
				_E002(x, isSituational);
			});
			_E0C2.Add(gestureBaseItem);
		}
		gesturesAudioItem.gameObject.SetActive(value: true);
		_E0C0.Add(gesturesAudioItem);
		return gesturesAudioItem;
	}

	private void _E002(GestureBaseItem._E000 click, bool isSituational)
	{
		switch (click.Button)
		{
		case PointerEventData.InputButton.Left:
			OnItemSelected.Invoke((click.ItemIndex, false));
			break;
		case PointerEventData.InputButton.Middle:
			OnItemSelected.Invoke((click.ItemIndex, true));
			break;
		case PointerEventData.InputButton.Right:
			if (!isSituational)
			{
				_E003(click.ItemIndex, click.Position);
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void _E003(int index, Vector2 pos)
	{
		_E004();
		_gesturesContextMenu.Show(pos, new _EC50(delegate(ECommand command)
		{
			_E006(command, index);
			_E005();
		}), Binds.GetCommandNames());
	}

	private void _E004()
	{
		_E0C3 = _E0C0.FirstOrDefault((GesturesAudioItem x) => !x.GroupIsAlwaysOpen && x.IsOpen);
		if (_E0C3 != null)
		{
			_E0C3.GroupIsAlwaysOpen = true;
		}
	}

	private void _E005()
	{
		if (_E0C3 != null)
		{
			_E0C3.GroupIsAlwaysOpen = false;
			_E0C3.IsOpen = false;
			_E0C3 = null;
		}
	}

	private void _E006(ECommand command, int index)
	{
		if (Binds.ContainsValue(index))
		{
			ECommand key = Binds.FirstOrDefault((KeyValuePair<ECommand, int> x) => x.Value == index).Key;
			Binds.Remove(key);
		}
		Binds[command] = index;
		_E0C4.SetChanges(Binds);
		OnBindUpdated.Invoke(new _E000(command, index, _ECAB.GetCommandName(index)));
	}

	private void _E007()
	{
		_E000(EGesture.Bad);
		_E000(EGesture.Good);
		_E000(EGesture.Hello);
		_E000(EGesture.Stop);
		_E000(EGesture.ThatDirection);
		_E000(EGesture.ComeToMe);
		_E000(EGesture.FuckYou);
	}

	private void _E008()
	{
		_E001(_ED3E._E000(229714), _commonGroupTemplate, EPhraseTrigger.Fire, EPhraseTrigger.HoldFire, EPhraseTrigger.Suppress, EPhraseTrigger.Spreadout, EPhraseTrigger.GetInCover, EPhraseTrigger.KnifesOnly, EPhraseTrigger.Regroup, EPhraseTrigger.HoldPosition, EPhraseTrigger.Attention, EPhraseTrigger.Gogogo, EPhraseTrigger.GoForward, EPhraseTrigger.GetBack, EPhraseTrigger.FollowMe, EPhraseTrigger.CoverMe, EPhraseTrigger.GoLoot, EPhraseTrigger.Stop, EPhraseTrigger.Silence, EPhraseTrigger.OnYourOwn);
		_E001(_ED3E._E000(229706), _commonGroupTemplate, EPhraseTrigger.OnPosition, EPhraseTrigger.OnLoot, EPhraseTrigger.GoodWork, EPhraseTrigger.Roger, EPhraseTrigger.Repeat, EPhraseTrigger.Ready, EPhraseTrigger.Going, EPhraseTrigger.Negative, EPhraseTrigger.BadWork, EPhraseTrigger.Covering, EPhraseTrigger.Clear, EPhraseTrigger.DontKnow);
		_E001(_ED3E._E000(229699), _commonGroupTemplate, EPhraseTrigger.Dehydrated, EPhraseTrigger.Exhausted, EPhraseTrigger.HurtLight, EPhraseTrigger.HurtMedium, EPhraseTrigger.HurtHeavy, EPhraseTrigger.HurtNearDeath, EPhraseTrigger.Bleeding, EPhraseTrigger.LegBroken, EPhraseTrigger.HandBroken);
		_E001(_ED3E._E000(229745), _commonGroupTemplate, EPhraseTrigger.RightFlank, EPhraseTrigger.InTheFront, EPhraseTrigger.OnSix, EPhraseTrigger.SniperPhrase, EPhraseTrigger.Scav, EPhraseTrigger.LeftFlank);
		_E001(_ED3E._E000(229737), _commonGroupTemplate, EPhraseTrigger.EnemyHit, EPhraseTrigger.KnifeKill, EPhraseTrigger.NoisePhrase, EPhraseTrigger.LostVisual, EPhraseTrigger.ScavDown, EPhraseTrigger.EnemyDown);
		_E001(_ED3E._E000(229735), _commonGroupTemplate, EPhraseTrigger.NeedFrag, EPhraseTrigger.NeedSniper, EPhraseTrigger.NeedAmmo, EPhraseTrigger.NeedHelp, EPhraseTrigger.NeedWeapon, EPhraseTrigger.NeedMedkit);
		_E001(_ED3E._E000(229788), _commonGroupTemplate, EPhraseTrigger.Down, EPhraseTrigger.Hit, EPhraseTrigger.Rat, EPhraseTrigger.FriendlyFire);
		_E001(_ED3E._E000(229776), _situationalGroupTemplate, EPhraseTrigger.MumblePhrase).GroupIsAlwaysOpen = true;
		_E001(_ED3E._E000(229775), _situationalGroupTemplate, EPhraseTrigger.LootKey, EPhraseTrigger.LootMoney, EPhraseTrigger.LootWeapon, EPhraseTrigger.LootGeneric, EPhraseTrigger.LootBody, EPhraseTrigger.LootContainer, EPhraseTrigger.LockedDoor, EPhraseTrigger.CheckHim, EPhraseTrigger.OpenDoor);
	}

	[CompilerGenerated]
	private void _E009(int item)
	{
		OnItemSelected.Invoke((item, false));
	}

	[CompilerGenerated]
	private void _E00A(GestureBaseItem._E000 x)
	{
		_E002(x, isSituational: false);
	}
}
