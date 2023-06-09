using System;
using EFT.InputSystem;

namespace EFT.UI.Tutorial;

[Serializable]
public class KeyAxis
{
	public EAxis AxisPairName;

	public bool UsePositiveAxis;

	public bool UseNegativeAxis;
}
