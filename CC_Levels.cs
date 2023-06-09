using UnityEngine;

[AddComponentMenu("Colorful/Levels")]
[ExecuteInEditMode]
public class CC_Levels : CC_Base
{
	public bool isRGB;

	public float inputMinL;

	public float inputMaxL = 255f;

	public float inputGammaL = 1f;

	public float inputMinR;

	public float inputMaxR = 255f;

	public float inputGammaR = 1f;

	public float inputMinG;

	public float inputMaxG = 255f;

	public float inputGammaG = 1f;

	public float inputMinB;

	public float inputMaxB = 255f;

	public float inputGammaB = 1f;

	public float outputMinL;

	public float outputMaxL = 255f;

	public float outputMinR;

	public float outputMaxR = 255f;

	public float outputMinG;

	public float outputMaxG = 255f;

	public float outputMinB;

	public float outputMaxB = 255f;

	public int currentChannel;

	public bool logarithmic;

	private static readonly int _E02E = Shader.PropertyToID(_ED3E._E000(16814));

	private static readonly int _E02F = Shader.PropertyToID(_ED3E._E000(16800));

	private static readonly int _E030 = Shader.PropertyToID(_ED3E._E000(16858));

	private static readonly int _E031 = Shader.PropertyToID(_ED3E._E000(16846));

	private static readonly int _E032 = Shader.PropertyToID(_ED3E._E000(16833));

	public int mode
	{
		get
		{
			if (!isRGB)
			{
				return 0;
			}
			return 1;
		}
		set
		{
			isRGB = value > 0;
		}
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!isRGB)
		{
			base.material.SetVector(_E02E, new Vector4(inputMinL / 255f, inputMinL / 255f, inputMinL / 255f, 1f));
			base.material.SetVector(_E02F, new Vector4(inputMaxL / 255f, inputMaxL / 255f, inputMaxL / 255f, 1f));
			base.material.SetVector(_E030, new Vector4(inputGammaL, inputGammaL, inputGammaL, 1f));
			base.material.SetVector(_E031, new Vector4(outputMinL / 255f, outputMinL / 255f, outputMinL / 255f, 1f));
			base.material.SetVector(_E032, new Vector4(outputMaxL / 255f, outputMaxL / 255f, outputMaxL / 255f, 1f));
		}
		else
		{
			base.material.SetVector(_E02E, new Vector4(inputMinR / 255f, inputMinG / 255f, inputMinB / 255f, 1f));
			base.material.SetVector(_E02F, new Vector4(inputMaxR / 255f, inputMaxG / 255f, inputMaxB / 255f, 1f));
			base.material.SetVector(_E030, new Vector4(inputGammaR, inputGammaG, inputGammaB, 1f));
			base.material.SetVector(_E031, new Vector4(outputMinR / 255f, outputMinG / 255f, outputMinB / 255f, 1f));
			base.material.SetVector(_E032, new Vector4(outputMaxR / 255f, outputMaxG / 255f, outputMaxB / 255f, 1f));
		}
		Graphics.Blit(source, destination, base.material);
	}
}
