using System;

namespace EFT;

[Serializable]
public sealed class PlayerVisualRepresentation
{
	public _E54F Info;

	public _E72D Customization;

	public _EB0B Equipment;

	public bool IsEmpty()
	{
		if (Customization != null)
		{
			return Equipment == null;
		}
		return true;
	}
}
