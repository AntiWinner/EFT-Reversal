using System.ComponentModel;

namespace EFT.InventoryLogic;

public class TagComponent : _EB19
{
	[DefaultValue(0)]
	[_E63C]
	public int Color;

	[_E63C]
	[DefaultValue("")]
	public string Name = string.Empty;

	public bool IsEmpty
	{
		get
		{
			if (string.IsNullOrEmpty(Name))
			{
				return Color == 0;
			}
			return false;
		}
	}

	public TagComponent(Item item)
		: base(item)
	{
	}

	public _ECD8<_EB5A> Set(string name, int color, bool simulate)
	{
		string name2 = Name;
		int color2 = Color;
		if (!simulate)
		{
			Name = name;
			Color = color;
		}
		return new _EB5A(this, name, color, name2, color2);
	}
}
