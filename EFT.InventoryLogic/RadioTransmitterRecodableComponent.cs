using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;

namespace EFT.InventoryLogic;

public sealed class RadioTransmitterRecodableComponent : RecodableComponent
{
	private const string _E010 = "RadioTransmitter/AttributeValues/Blocked";

	private RadioTransmitterStatus _E011;

	private _E637 _E012;

	[CompilerGenerated]
	private Action<bool> _E013;

	[CompilerGenerated]
	private Action<RadioTransmitterStatus> _E014;

	public RadioTransmitterStatus Status => _E011;

	public _E637 Handler => _E012;

	public event Action<bool> OnRadioTransmitterStateChanged
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = _E013;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E013, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = _E013;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E013, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<RadioTransmitterStatus> OnRadioTransmitterStatusChanged
	{
		[CompilerGenerated]
		add
		{
			Action<RadioTransmitterStatus> action = _E014;
			Action<RadioTransmitterStatus> action2;
			do
			{
				action2 = action;
				Action<RadioTransmitterStatus> value2 = (Action<RadioTransmitterStatus>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E014, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<RadioTransmitterStatus> action = _E014;
			Action<RadioTransmitterStatus> action2;
			do
			{
				action2 = action;
				Action<RadioTransmitterStatus> value2 = (Action<RadioTransmitterStatus>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E014, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public RadioTransmitterRecodableComponent(Item item, _EA83 template)
		: base(item, template)
	{
		if (IsEncoded)
		{
			_E011 = RadioTransmitterStatus.Green;
		}
		else
		{
			_E011 = RadioTransmitterStatus.Red;
		}
	}

	public override void SetEncoded(bool value)
	{
		base.SetEncoded(value);
		if (_E012 == null)
		{
			_E011 = (IsEncoded ? RadioTransmitterStatus.Green : RadioTransmitterStatus.Red);
		}
	}

	public void SetStatus(RadioTransmitterStatus status)
	{
		_E011 = status;
		_E014?.Invoke(_E011);
	}

	public void InitializeHandler(Player player)
	{
		if (_E012 == null)
		{
			_E012 = new _E637(this, player);
		}
	}

	protected override string GetAttributeValue()
	{
		bool flag = Singleton<AbstractGame>.Instance is HideoutGame;
		if (_E012 == null || flag)
		{
			return base.GetAttributeValue();
		}
		return ((_EA87)Item).RecodableComponent.Status switch
		{
			RadioTransmitterStatus.Green => _ED3E._E000(103076) + _ED3E._E000(215810).Localized() + _ED3E._E000(59467), 
			RadioTransmitterStatus.Yellow => _ED3E._E000(103041) + _ED3E._E000(215896).Localized() + _ED3E._E000(59467), 
			_ => _ED3E._E000(103088) + _ED3E._E000(215921).Localized() + _ED3E._E000(59467), 
		};
	}
}
