using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace EFT.InventoryLogic;

public class FaceShieldComponent : _EB19
{
	public enum EMask
	{
		NoMask,
		Narrow,
		Wide
	}

	[CanBeNull]
	public readonly TogglableComponent Togglable;

	[_E63C]
	public byte HitSeed;

	[NonSerialized]
	public readonly _ECEC HitsChanged = new _ECEC();

	private readonly _E9D7 m__E000;

	private byte _E00A;

	[_E63C]
	public byte Hits
	{
		get
		{
			return _E00A;
		}
		set
		{
			if (_E00A != value)
			{
				_E00A = value;
				HitsChanged.Invoke();
			}
		}
	}

	public EMask Mask => this.m__E000.Mask;

	public float BlindnessProtection => this.m__E000.BlindnessProtection;

	public void StoreValidationTimestamp()
	{
	}

	public bool TimestampIsValid(int frameCount)
	{
		return true;
	}

	public FaceShieldComponent(Item item, [CanBeNull] TogglableComponent togglable, _E9D7 template)
		: base(item)
	{
		this.m__E000 = template;
		Togglable = togglable;
		item.SafelyAddAttributeToList(new _EB11(EItemAttributeId.BlindnessProtection)
		{
			Name = EItemAttributeId.BlindnessProtection.GetName(),
			Base = () => this.m__E000.BlindnessProtection,
			StringValue = () => string.Format(_ED3E._E000(215391), this.m__E000.BlindnessProtection),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		if (HitSeed == 0)
		{
			HitSeed = (byte)(_E5AD.Now.Ticks & 0xFF);
		}
	}

	[CompilerGenerated]
	private float _E000()
	{
		return this.m__E000.BlindnessProtection;
	}

	[CompilerGenerated]
	private string _E001()
	{
		return string.Format(_ED3E._E000(215391), this.m__E000.BlindnessProtection);
	}
}
