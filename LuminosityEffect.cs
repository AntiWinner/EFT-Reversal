using UnityEngine;

[AddComponentMenu("Image Effects/Luminosity")]
[ExecuteInEditMode]
public class LuminosityEffect : ImageEffectBase
{
	public float desaturateAmount;

	public Texture textureRamp;

	public float rampOffsetR;

	public float rampOffsetG;

	public float rampOffsetB;

	private static readonly int _E000 = Shader.PropertyToID(_ED3E._E000(16726));

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(17016));

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(17015));

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetTexture(_E000, textureRamp);
		base.material.SetFloat(_E001, desaturateAmount);
		base.material.SetVector(_E002, new Vector4(rampOffsetR, rampOffsetG, rampOffsetB, 0f));
		Graphics.Blit(source, destination, base.material);
	}
}
