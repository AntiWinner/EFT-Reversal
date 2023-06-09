using EFT.BlitDebug;
using UnityEngine;

[AddComponentMenu("Colorful/Hue, Saturation, Value")]
[ExecuteInEditMode]
public class CC_HueSaturationValue : CC_Base
{
	[Range(-180f, 180f)]
	public float masterHue;

	[Range(-100f, 100f)]
	public float masterSaturation;

	[Range(-100f, 100f)]
	public float masterValue;

	[Range(-180f, 180f)]
	public float redsHue;

	[Range(-100f, 100f)]
	public float redsSaturation;

	[Range(-100f, 100f)]
	public float redsValue;

	[Range(-180f, 180f)]
	public float yellowsHue;

	[Range(-100f, 100f)]
	public float yellowsSaturation;

	[Range(-100f, 100f)]
	public float yellowsValue;

	[Range(-180f, 180f)]
	public float greensHue;

	[Range(-100f, 100f)]
	public float greensSaturation;

	[Range(-100f, 100f)]
	public float greensValue;

	[Range(-180f, 180f)]
	public float cyansHue;

	[Range(-100f, 100f)]
	public float cyansSaturation;

	[Range(-100f, 100f)]
	public float cyansValue;

	[Range(-180f, 180f)]
	public float bluesHue;

	[Range(-100f, 100f)]
	public float bluesSaturation;

	[Range(-100f, 100f)]
	public float bluesValue;

	[Range(-180f, 180f)]
	public float magentasHue;

	[Range(-100f, 100f)]
	public float magentasSaturation;

	[Range(-100f, 100f)]
	public float magentasValue;

	public bool advanced;

	public int currentChannel;

	private static readonly int _E026 = Shader.PropertyToID(_ED3E._E000(16748));

	private static readonly int _E027 = Shader.PropertyToID(_ED3E._E000(16740));

	private static readonly int _E028 = Shader.PropertyToID(_ED3E._E000(16738));

	private static readonly int _E029 = Shader.PropertyToID(_ED3E._E000(16795));

	private static readonly int _E02A = Shader.PropertyToID(_ED3E._E000(16787));

	private static readonly int _E02B = Shader.PropertyToID(_ED3E._E000(16778));

	private static readonly int _E02C = Shader.PropertyToID(_ED3E._E000(16769));

	public float hue
	{
		get
		{
			return masterHue;
		}
		set
		{
			masterHue = value;
		}
	}

	public float saturation
	{
		get
		{
			return masterSaturation;
		}
		set
		{
			masterSaturation = value;
		}
	}

	public float value
	{
		get
		{
			return masterValue;
		}
		set
		{
			masterValue = value;
		}
	}

	[ImageEffectTransformsToLDR]
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector(_E026, new Vector3(masterHue / 360f, masterSaturation * 0.01f, masterValue * 0.01f));
		if (advanced)
		{
			base.material.SetVector(_E027, new Vector3(redsHue / 360f, redsSaturation * 0.01f, redsValue * 0.01f));
			base.material.SetVector(_E028, new Vector3(yellowsHue / 360f, yellowsSaturation * 0.01f, yellowsValue * 0.01f));
			base.material.SetVector(_E029, new Vector3(greensHue / 360f, greensSaturation * 0.01f, greensValue * 0.01f));
			base.material.SetVector(_E02A, new Vector3(cyansHue / 360f, cyansSaturation * 0.01f, cyansValue * 0.01f));
			base.material.SetVector(_E02B, new Vector3(bluesHue / 360f, bluesSaturation * 0.01f, bluesValue * 0.01f));
			base.material.SetVector(_E02C, new Vector3(magentasHue / 360f, magentasSaturation * 0.01f, magentasValue * 0.01f));
			DebugGraphics.Blit(source, destination, base.material, 1);
		}
		else
		{
			DebugGraphics.Blit(source, destination, base.material, 0);
		}
	}
}
