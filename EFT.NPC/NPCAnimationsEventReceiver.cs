using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using AnimationEventSystem;
using UnityEngine;

namespace EFT.NPC;

public class NPCAnimationsEventReceiver : MonoBehaviour, _E324
{
	[CompilerGenerated]
	private Action m__E000;

	[CompilerGenerated]
	private Action<int, bool> _E001;

	[CompilerGenerated]
	private Action<string> _E002;

	private _E570 _E003;

	private IAnimator _E004;

	private List<_E324> _E005 = new List<_E324>();

	public event Action OnCurrentAnimationEnded
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<int, bool> OnNeedChangeObjectVisibility
	{
		[CompilerGenerated]
		add
		{
			Action<int, bool> action = _E001;
			Action<int, bool> action2;
			do
			{
				action2 = action;
				Action<int, bool> value2 = (Action<int, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int, bool> action = _E001;
			Action<int, bool> action2;
			do
			{
				action2 = action;
				Action<int, bool> value2 = (Action<int, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnNeedToPlaySomeSound
	{
		[CompilerGenerated]
		add
		{
			Action<string> action = _E002;
			Action<string> action2;
			do
			{
				action2 = action;
				Action<string> value2 = (Action<string>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<string> action = _E002;
			Action<string> action2;
			do
			{
				action2 = action;
				Action<string> value2 = (Action<string>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Initialize(Animator npcAnimator)
	{
		_E005.Add(this);
		_E004 = _E563.CreateAnimator(npcAnimator);
		_E003 = new _E570();
		_E003.SetAnimator(_E004, _E570.EEmitType.EmitOnAnimatorUpdate, _ED3E._E000(50105));
		_E003.OnEventAction += _E000;
	}

	private void _E000(int functionHashName, AnimationEventParameter parameters)
	{
		_E325.AnimatorEventHandler(_E005, functionHashName, parameters);
	}

	void _E324.OnCurrentAnimStateEnded()
	{
		this.m__E000?.Invoke();
	}

	void _E324.OnSetActiveObject(int objectID)
	{
		_E001?.Invoke(objectID, arg2: true);
	}

	void _E324.OnDeactivateObject(int objectID)
	{
		_E001?.Invoke(objectID, arg2: false);
	}

	void _E324.OnSound(string StringParam)
	{
		_E002?.Invoke(StringParam);
	}

	void _E324.OnAddAmmoInChamber()
	{
	}

	void _E324.OnAddAmmoInMag()
	{
	}

	void _E324.OnArm()
	{
	}

	void _E324.OnCook()
	{
	}

	void _E324.OnDelAmmoChamber()
	{
	}

	void _E324.OnDelAmmoFromMag()
	{
	}

	void _E324.OnDisarm()
	{
	}

	void _E324.OnFireEnd()
	{
	}

	void _E324.OnFiringBullet()
	{
	}

	void _E324.OnFoldOff()
	{
	}

	void _E324.OnFoldOn()
	{
	}

	void _E324.OnIdleStart()
	{
	}

	void _E324.OnMagHide()
	{
	}

	void _E324.OnMagIn()
	{
	}

	void _E324.OnMagOut()
	{
	}

	void _E324.OnMagShow()
	{
	}

	void _E324.OnMessageName()
	{
	}

	void _E324.OnMalfunctionOff()
	{
	}

	void _E324.OnModChanged()
	{
	}

	void _E324.OnOffBoltCatch()
	{
	}

	void _E324.OnOnBoltCatch()
	{
	}

	void _E324.OnPutMagToRig()
	{
	}

	void _E324.OnRemoveShell()
	{
	}

	void _E324.OnReplaceSecondMag()
	{
	}

	void _E324.OnShellEject()
	{
	}

	void _E324.OnShowAmmo(bool BoolParam)
	{
	}

	void _E324.OnSoundAtPoint(string StringParam)
	{
	}

	void _E324.OnStartUtilityOperation()
	{
	}

	void _E324.OnUseSecondMagForReload()
	{
	}

	void _E324.OnWeapIn()
	{
	}

	void _E324.OnWeapOut()
	{
	}

	void _E324.OnLauncherAppeared()
	{
	}

	void _E324.OnLauncherDisappeared()
	{
	}

	void _E324.OnShowMag()
	{
	}

	void _E324.OnSliderOut()
	{
	}

	void _E324.OnUseProp(bool BoolParam)
	{
	}

	void _E324.OnBackpackDrop()
	{
	}

	void _E324.OnComboPlanning()
	{
	}

	void _E324.OnThirdAction(int objectID)
	{
	}
}
