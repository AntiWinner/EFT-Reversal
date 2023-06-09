using System;
using System.Collections.Generic;
using EFT.InventoryLogic;

[Serializable]
public class ThermalVisionUtilities
{
	public ThermalVisionComponent.SelectablePalette CurrentRampPalette;

	public float DepthFade = 0.03f;

	public List<RampTexPalletteConnector> RampTexPalletteConnectors;

	public ValuesCoefs ValuesCoefs;

	public Noise NoiseParameters;

	public MaskDescription MaskDescription;
}
