using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[Obsolete]
public class AnimatorEventReceiver : MonoBehaviour
{
	[CompilerGenerated]
	private Action _E000;

	[CompilerGenerated]
	private Action<int> _E001;

	[CompilerGenerated]
	private Action _E002;

	[CompilerGenerated]
	private Action _E003;

	[CompilerGenerated]
	private Action _E004;

	[CompilerGenerated]
	private Action _E005;

	[CompilerGenerated]
	private Action _E006;

	[CompilerGenerated]
	private Action _E007;

	[CompilerGenerated]
	private Action _E008;

	[CompilerGenerated]
	private Action _E009;

	public event Action OnFire
	{
		[CompilerGenerated]
		add
		{
			Action action = _E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<int> OnChangeFireMode
	{
		[CompilerGenerated]
		add
		{
			Action<int> action = _E001;
			Action<int> action2;
			do
			{
				action2 = action;
				Action<int> value2 = (Action<int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int> action = _E001;
			Action<int> action2;
			do
			{
				action2 = action;
				Action<int> value2 = (Action<int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnDelAmmoChamber
	{
		[CompilerGenerated]
		add
		{
			Action action = _E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnDelAmmoFromMag
	{
		[CompilerGenerated]
		add
		{
			Action action = _E003;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E003;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnShellEject
	{
		[CompilerGenerated]
		add
		{
			Action action = _E004;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E004, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E004;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E004, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnAddAmmoInChamber
	{
		[CompilerGenerated]
		add
		{
			Action action = _E005;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E005, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E005;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E005, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnOnBoltCatch
	{
		[CompilerGenerated]
		add
		{
			Action action = _E006;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E006, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E006;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E006, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnOffBoltCatch
	{
		[CompilerGenerated]
		add
		{
			Action action = _E007;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E007, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E007;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E007, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnMagOut
	{
		[CompilerGenerated]
		add
		{
			Action action = _E008;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E008, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E008;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E008, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnMagIn
	{
		[CompilerGenerated]
		add
		{
			Action action = _E009;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E009, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E009;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E009, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void FiringBullet()
	{
		if (_E000 != null)
		{
			_E000();
		}
		Debug.Log(_ED3E._E000(62109));
	}

	public void DelAmmoChamber()
	{
		if (_E002 != null)
		{
			_E002();
		}
	}

	public void ShellEject()
	{
		if (_E004 != null)
		{
			_E004();
		}
	}

	public void DelAmmoFromMag()
	{
		if (_E003 != null)
		{
			_E003();
		}
	}

	public void AddAmmoInChamber()
	{
		if (_E005 != null)
		{
			_E005();
		}
	}

	public void OnBoltCatch()
	{
		if (_E006 != null)
		{
			_E006();
		}
	}

	public void OffBoltCatch()
	{
		if (_E007 != null)
		{
			_E007();
		}
	}

	public void SetFiremode1()
	{
		if (_E001 != null)
		{
			_E001(1);
		}
	}

	public void SetFiremode0()
	{
		if (_E001 != null)
		{
			_E001(0);
		}
	}

	public void MagOut()
	{
		if (_E008 != null)
		{
			_E008();
		}
	}

	public void MagIn()
	{
		if (_E009 != null)
		{
			_E009();
		}
	}
}
