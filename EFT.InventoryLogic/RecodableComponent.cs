using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace EFT.InventoryLogic;

public class RecodableComponent : _EB19
{
	protected const string ENCODED = "RecodableItem/AttributeValues/Encoded";

	protected const string DECODED = "RecodableItem/AttributeValues/Decoded";

	[_E63C]
	public bool IsEncoded;

	private readonly _EA83 m__E000;

	[CompilerGenerated]
	private Action<bool> _E015;

	public event Action<bool> OnRecodableStateChanged
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = _E015;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E015, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = _E015;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E015, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public RecodableComponent(Item item, _EA83 template)
		: base(item)
	{
		this.m__E000 = template;
		AddRecodableAttribute(item);
	}

	protected void AddRecodableAttribute(Item item)
	{
		item.Attributes.Add(new _EB10(EItemAttributeId.EncodeState)
		{
			Name = EItemAttributeId.EncodeState.GetName(),
			StringValue = () => GetAttributeValue(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
	}

	protected virtual string GetAttributeValue()
	{
		if (!IsEncoded)
		{
			return _ED3E._E000(103088) + _ED3E._E000(215921).Localized() + _ED3E._E000(59467);
		}
		return _ED3E._E000(103076) + _ED3E._E000(215810).Localized() + _ED3E._E000(59467);
	}

	public virtual void SetEncoded(bool value)
	{
		if (IsEncoded != value)
		{
			IsEncoded = value;
			Item.UpdateAttributes();
			_E015?.Invoke(value);
		}
	}

	[CompilerGenerated]
	private string _E000()
	{
		return GetAttributeValue();
	}
}
