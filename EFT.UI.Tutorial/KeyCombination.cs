using System;
using EFT.InputSystem;

namespace EFT.UI.Tutorial;

[Serializable]
public class KeyCombination
{
	public KeyAxis Axis;

	public EGameKey[] Keys;

	public string LocalizationKey;
}
