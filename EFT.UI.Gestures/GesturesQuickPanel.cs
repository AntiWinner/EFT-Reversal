using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.Interactive;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI.Gestures;

public sealed class GesturesQuickPanel : UIElement
{
	private sealed class _E000
	{
		public EPhraseTrigger Phrase;

		public float Expires;
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public GesturesQuickPanel _003C_003E4__this;

		public EPhraseTrigger trigger;

		internal bool? _E000(Task _)
		{
			return _003C_003E4__this._E307?.TrySetResult(result: true);
		}

		internal bool _E001(_E000 x)
		{
			return x.Phrase == trigger;
		}

		internal bool _E002(_E000 x)
		{
			return x.Phrase == trigger;
		}
	}

	[SerializeField]
	private CustomTextMeshProUGUI _textField;

	[SerializeField]
	private GameObject _quickCommandObject;

	[SerializeField]
	private GesturesMenu _gesturesMenu;

	[SerializeField]
	private GesturesVoipPanel _gesturesVoipPanel;

	[SerializeField]
	private GesturesDropdownPanel _dropdownPanel;

	[SerializeField]
	private Animation _animation;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private CustomTextMeshProUGUI Hotkey;

	[SerializeField]
	private CustomTextMeshProUGUI DropdownHotkey;

	private EPhraseTrigger _E304 = EPhraseTrigger.PhraseNone;

	public static readonly Dictionary<EPhraseTrigger, int> PhrasePriorities = new Dictionary<EPhraseTrigger, int>
	{
		{
			EPhraseTrigger.OnEnemyGrenade,
			100
		},
		{
			EPhraseTrigger.OnGrenade,
			95
		},
		{
			EPhraseTrigger.OnFriendlyDown,
			95
		},
		{
			EPhraseTrigger.OnWeaponReload,
			95
		},
		{
			EPhraseTrigger.NeedAmmo,
			95
		},
		{
			EPhraseTrigger.ExitLocated,
			77
		},
		{
			EPhraseTrigger.LootKey,
			70
		},
		{
			EPhraseTrigger.LockedDoor,
			70
		},
		{
			EPhraseTrigger.LootBody,
			70
		},
		{
			EPhraseTrigger.LootContainer,
			70
		},
		{
			EPhraseTrigger.LootGeneric,
			70
		},
		{
			EPhraseTrigger.LootMoney,
			70
		},
		{
			EPhraseTrigger.LootWeapon,
			70
		},
		{
			EPhraseTrigger.Cooperation,
			87
		},
		{
			EPhraseTrigger.LootNothing,
			70
		},
		{
			EPhraseTrigger.WeaponBroken,
			88
		},
		{
			EPhraseTrigger.OnWeaponJammed,
			86
		},
		{
			EPhraseTrigger.HandBroken,
			80
		},
		{
			EPhraseTrigger.LegBroken,
			80
		},
		{
			EPhraseTrigger.Dehydrated,
			70
		},
		{
			EPhraseTrigger.Exhausted,
			70
		},
		{
			EPhraseTrigger.Bleeding,
			85
		},
		{
			EPhraseTrigger.HurtLight,
			90
		},
		{
			EPhraseTrigger.HurtMedium,
			90
		},
		{
			EPhraseTrigger.HurtHeavy,
			90
		},
		{
			EPhraseTrigger.HurtNearDeath,
			90
		},
		{
			EPhraseTrigger.OpenDoor,
			75
		},
		{
			EPhraseTrigger.CheckHim,
			75
		}
	};

	private readonly Dictionary<EPhraseTrigger, int> _E305 = new Dictionary<EPhraseTrigger, int>();

	private _E9C4 _E0AE;

	private Player _E094;

	private readonly List<_E000> _E306 = new List<_E000>();

	private HashSet<EPhraseTrigger> _E2FF;

	private TaskCompletionSource<bool> _E307;

	private bool _E308;

	public GesturesDropdownPanel DropdownPanel => _dropdownPanel;

	private EPhraseTrigger _E000 => (from x in _E305
		orderby x.Value, _E3A5<EPhraseTrigger>.GetName(x.Key)
		select x).LastOrDefault().Key;

	public bool DropdownPanelActive => _dropdownPanel.DropdownPanelActive;

	public void Show(Player player)
	{
		_E009();
		ShowGameObject();
		_ECAA gesturesStorage = new _ECAA(autoSave: true, player.Profile.Info.Side);
		_E2FF = new HashSet<EPhraseTrigger>(player.Speaker.PhrasesBanks.Keys);
		_gesturesMenu.Init(gesturesStorage, _E2FF);
		UI.AddDisposable(_gesturesMenu.OnShow.Subscribe(_E00B));
		UI.AddDisposable(_gesturesMenu.OnClose.Subscribe(_E00C));
		UI.AddDisposable(_gesturesMenu.OnItemSelected.Subscribe(delegate
		{
			_gesturesMenu.Close();
		}));
		_E094 = player;
		_E0AE = player.HealthController;
		_E094.HealthController.HealthChangedEvent += _E002;
		_E094.HealthController.EffectStartedEvent += _E003;
		_E094.HealthController.EffectResidualEvent += _E003;
		_E00A();
		UI.BindEvent(_E094.OnExitTriggerVisited, _E006);
		_E094.PhraseSituation += _E000;
		_E094.PossibleInteractionsChanged += _E001;
		_E001();
		_E7F2 settings = Singleton<_E7DE>.Instance.Control.Settings;
		Hotkey.text = settings.GetKeyName(EGameKey.MumbleQuick);
		DropdownHotkey.text = settings.GetKeyName(EGameKey.MumbleDropdown);
		UI.AddDisposable(_gesturesMenu.OnItemSelected.Subscribe(delegate((int actionId, bool aggressive) x)
		{
			if (_E094 != null)
			{
				_E094.PlayPhraseOrGesture(x.actionId, x.aggressive);
			}
		}));
	}

	private void _E000(EPhraseTrigger ePhraseTrigger, int delay)
	{
		_E000 obj = null;
		foreach (_E000 item in _E306)
		{
			if (item.Phrase != ePhraseTrigger)
			{
				continue;
			}
			obj = item;
			break;
		}
		if (obj != null)
		{
			obj.Expires = Time.time + (float)delay;
		}
		else
		{
			_E306.Add(new _E000
			{
				Phrase = ePhraseTrigger,
				Expires = Time.time + (float)delay
			});
		}
		_E306.Sort((_E000 p1, _E000 p2) => p1.Expires.CompareTo(p2.Expires));
		_E007(ePhraseTrigger, active: true);
	}

	public static bool IsSituationalPhrase(EPhraseTrigger phrase)
	{
		if (phrase != EPhraseTrigger.NeedAmmo)
		{
			return PhrasePriorities.ContainsKey(phrase);
		}
		return false;
	}

	public static bool IsPhraseAvailable(EPhraseTrigger phrase)
	{
		if (IsSituationalPhrase(phrase))
		{
			if (MonoBehaviourSingleton<GameUI>.Instantiated)
			{
				return MonoBehaviourSingleton<GameUI>.Instance.GesturesQuickPanel._E305.ContainsKey(phrase);
			}
			return false;
		}
		return true;
	}

	private void Start()
	{
		EPhraseTrigger[] array = new EPhraseTrigger[8]
		{
			EPhraseTrigger.OnWeaponReload,
			EPhraseTrigger.OnWeaponJammed,
			EPhraseTrigger.Hit,
			EPhraseTrigger.OnGrenade,
			EPhraseTrigger.OnBeingHurt,
			EPhraseTrigger.OnFight,
			EPhraseTrigger.OnEnemyDown,
			EPhraseTrigger.OnEnemyShot
		};
		foreach (EPhraseTrigger phrase in array)
		{
			_E007(phrase, active: true);
			_E007(phrase, active: false);
		}
	}

	private void Update()
	{
		for (int num = _E306.Count - 1; num > -1; num--)
		{
			if (_E306[num].Expires < Time.time)
			{
				_E007(_E306[num].Phrase, active: false);
				_E306.RemoveAt(num);
			}
		}
	}

	public void ToggleBindPanel(Player player)
	{
		if (_gesturesMenu.IsShowing)
		{
			CloseBindPanel();
			return;
		}
		_gesturesMenu.Show();
		_gesturesVoipPanel.Show(player.VoipController);
		_E00A();
		_E006();
		_E001();
	}

	public void CloseBindPanel()
	{
		_gesturesMenu.CloseWithSelected();
		_gesturesVoipPanel.Close();
	}

	public EPhraseTrigger ActivateCommand()
	{
		EPhraseTrigger ePhraseTrigger = this._E000;
		if (!_E305.ContainsKey(ePhraseTrigger) || _E308)
		{
			return EPhraseTrigger.PhraseNone;
		}
		_E304 = ePhraseTrigger;
		_E305[ePhraseTrigger] -= 10;
		return ePhraseTrigger;
	}

	private void _E001()
	{
		LootItem lootItem = _E094.InteractableObject as LootItem;
		bool flag = lootItem != null && lootItem.ItemOwner.RootItem.GetItemComponent<KeyComponent>() != null;
		bool flag2 = lootItem != null && lootItem.ItemOwner.RootItem is _EA76;
		bool flag3 = lootItem != null && (lootItem.ItemOwner.RootItem is Weapon || lootItem.ItemOwner.RootItem.GetItemComponent<KnifeComponent>() != null);
		_E007(EPhraseTrigger.LootKey, flag);
		_E007(EPhraseTrigger.LootMoney, flag2);
		_E007(EPhraseTrigger.LootWeapon, flag3);
		_E007(EPhraseTrigger.LootGeneric, lootItem != null && !flag && !flag2 && !flag3);
		Corpse corpse = _E094.InteractableObject as Corpse;
		_E007(EPhraseTrigger.LootBody, corpse != null);
		_E007(EPhraseTrigger.CheckHim, corpse != null);
		_E007(EPhraseTrigger.LootContainer, _E094.InteractableObject as LootableContainer != null);
		Door door = _E094.InteractableObject as Door;
		_E007(EPhraseTrigger.OpenDoor, door != null);
		_E007(EPhraseTrigger.LockedDoor, door != null && (door.DoorState == EDoorState.Locked || door.DoorState == EDoorState.Shut));
		_E007(EPhraseTrigger.Cooperation, _E094.InteractablePlayer != null);
	}

	private void _E002(EBodyPart bodyPart = EBodyPart.Chest, float diff = 0f, _EC23 damageStats = default(_EC23))
	{
		float num = 100f * _E0AE.GetBodyPartHealth(EBodyPart.Common).Normalized;
		_E007(EPhraseTrigger.HurtNearDeath, num >= 0f && num <= 15f);
		_E007(EPhraseTrigger.HurtHeavy, num > 15f && num <= 40f);
		_E007(EPhraseTrigger.HurtMedium, num > 40f && num <= 70f);
		_E007(EPhraseTrigger.HurtLight, num > 70f && num < 90f);
	}

	private void _E003(_E992 effect = null)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (effect != null)
		{
			if (!(effect is _E9A1))
			{
				if (!(effect is _E9A2))
				{
					if (!(effect is _E9A3))
					{
						if (effect is _E9A4)
						{
							flag4 = true;
						}
					}
					else
					{
						flag3 = true;
					}
				}
				else
				{
					flag2 = true;
				}
			}
			else
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
			flag2 = true;
			flag3 = true;
			flag4 = true;
		}
		if (flag)
		{
			_E007(EPhraseTrigger.Bleeding, _E004<_E9A1>());
		}
		if (flag2)
		{
			_E007(EPhraseTrigger.LegBroken, _E005<_E9A2>(new EBodyPart[2]
			{
				EBodyPart.LeftLeg,
				EBodyPart.RightLeg
			}));
			_E007(EPhraseTrigger.HandBroken, _E005<_E9A2>(new EBodyPart[2]
			{
				EBodyPart.LeftArm,
				EBodyPart.RightArm
			}));
		}
		if (flag3)
		{
			_E007(EPhraseTrigger.Dehydrated, _E004<_E9A3>());
		}
		if (flag4)
		{
			_E007(EPhraseTrigger.Exhausted, _E004<_E9A4>());
		}
	}

	private bool _E004<_E0AC>() where _E0AC : class, _E992
	{
		return _E0AE.FindActiveEffect<_E0AC>() != null;
	}

	private bool _E005<_E0AC>(params EBodyPart[] bodyParts) where _E0AC : class, _E992
	{
		if (bodyParts.Length == 0)
		{
			return _E0AE.FindActiveEffect<_E0AC>() != null;
		}
		return bodyParts.Any((EBodyPart bp) => _E0AE.FindActiveEffect<_E0AC>(bp) != null);
	}

	private void _E006()
	{
		_E007(EPhraseTrigger.ExitLocated, _E094.ExitTriggerZone);
	}

	private void _E007(EPhraseTrigger phrase, bool active)
	{
		active &= _E2FF.Contains(phrase);
		if (active)
		{
			AddQuickCommand(phrase);
		}
		else
		{
			RemoveQuickCommand(phrase);
		}
		_gesturesMenu.SetPhraseActive(phrase, active);
	}

	public void AddQuickCommand(EPhraseTrigger trigger)
	{
		if (PhrasePriorities.ContainsKey(trigger))
		{
			if (!_E305.ContainsKey(trigger))
			{
				_dropdownPanel.AddCommand(trigger);
				_E305.Add(trigger, PhrasePriorities[trigger]);
				_quickCommandObject.SetActive(value: true);
				_E008();
			}
			else if (trigger != EPhraseTrigger.HurtLight && trigger != EPhraseTrigger.HurtMedium && trigger != EPhraseTrigger.HurtHeavy && trigger != EPhraseTrigger.HurtNearDeath)
			{
				_E305[trigger] = PhrasePriorities[trigger];
			}
		}
	}

	public void RemoveQuickCommand(EPhraseTrigger trigger)
	{
		if (PhrasePriorities.ContainsKey(trigger) && _E305.ContainsKey(trigger))
		{
			_E305.Remove(trigger);
			_dropdownPanel.RemoveCommand(trigger);
			if (_E305.Count <= 0)
			{
				_quickCommandObject.SetActive(value: false);
				CloseDropdown(null);
			}
			else
			{
				_E008();
			}
		}
	}

	public void ShowDropdown()
	{
		MonoBehaviourSingleton<GameUI>.Instance.BattleUiScreen.ActionPanel.HideGameObject();
		_dropdownPanel.ShowDropdown();
	}

	public void CloseDropdown(Action<EPhraseTrigger> onPhraseSelected)
	{
		MonoBehaviourSingleton<GameUI>.Instance.BattleUiScreen.ActionPanel.ShowGameObject(instant: true);
		_dropdownPanel.CloseDropdown();
		if (_E305.ContainsKey(_dropdownPanel._E0BE))
		{
			onPhraseSelected?.Invoke(_dropdownPanel._E0BE);
		}
	}

	public async Task TryNotifyPhrasePlayed(EPhraseTrigger trigger, float time)
	{
		if (trigger != _E304)
		{
			return;
		}
		_E308 = true;
		_animation.Play();
		_E307?.TrySetResult(result: false);
		_E307 = new TaskCompletionSource<bool>();
		TasksExtensions.Delay(time).ContinueWith((Task _) => _E307?.TrySetResult(result: true)).HandleExceptions();
		bool num = await _E307.Task;
		_E307 = null;
		if (num)
		{
			_E308 = false;
			_animation.Stop();
			_canvasGroup.alpha = 1f;
			if (_E306.Any((_E000 x) => x.Phrase == trigger))
			{
				_E007(trigger, active: false);
			}
			_E306.RemoveAll((_E000 x) => x.Phrase == trigger);
			_E008();
		}
	}

	private void _E008()
	{
		_textField.text = this._E000.LocalizedShort(EStringCase.Upper);
	}

	private void _E009()
	{
		if (!(_E094 == null))
		{
			_E094.PossibleInteractionsChanged -= _E001;
			_E094.PhraseSituation -= _E000;
			_E094.HealthController.HealthChangedEvent -= _E002;
			_E094.HealthController.EffectStartedEvent -= _E003;
			_E094.HealthController.EffectRemovedEvent -= _E003;
			_E094 = null;
		}
	}

	private void OnDestroy()
	{
		_E307?.TrySetResult(result: false);
	}

	public override void Close()
	{
		_E009();
		_gesturesMenu.Close();
		_gesturesVoipPanel.Close();
		_E307?.TrySetResult(result: true);
		base.Close();
	}

	private void _E00A()
	{
		_E002();
		_E003();
	}

	private static void _E00B()
	{
		UIEventSystem.Instance.Enable();
		_E8A8.Instance.Blur(isActive: true, 0.35f);
		MonoBehaviourSingleton<GameUI>.Instance.BattleUiScreen.ActionPanel.HideGameObject();
	}

	private void _E00C()
	{
		MonoBehaviourSingleton<GameUI>.Instance.BattleUiScreen.ActionPanel.ShowGameObject(instant: true);
		_E8A8.Instance.Blur(isActive: false);
		UIEventSystem.Instance.Disable();
		_gesturesVoipPanel.Close();
	}

	[CompilerGenerated]
	private void _E00D((int actionId, bool aggressive) x)
	{
		_gesturesMenu.Close();
	}

	[CompilerGenerated]
	private void _E00E((int actionId, bool aggressive) x)
	{
		if (_E094 != null)
		{
			_E094.PlayPhraseOrGesture(x.actionId, x.aggressive);
		}
	}

	[CompilerGenerated]
	private bool _E00F<_E0AC>(EBodyPart bp) where _E0AC : class, _E992
	{
		return _E0AE.FindActiveEffect<_E0AC>(bp) != null;
	}
}
