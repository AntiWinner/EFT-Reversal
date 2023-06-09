using System;
using UnityEngine;

[Serializable]
public class MaskDescription
{
	public Texture Mask;

	public float MaskSize = 1.5f;

	public Texture ThermalMaskTexture;

	public Texture AnvisMaskTexture;

	public Texture BinocularMaskTexture;

	public Texture GasMaskTexture;

	public Texture OldMonocularMaskTexture;
}
