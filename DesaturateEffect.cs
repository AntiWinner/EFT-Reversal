using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("Image Effects/Desaturate")]
[ExecuteInEditMode]
public class DesaturateEffect : ImageEffectBase
{
	public Texture textureRamp;

	public float rampOffsetR;

	public float rampOffsetG;

	public float rampOffsetB;

	[FormerlySerializedAs("desaturateAmount")]
	public float WeatherDesaturate;

	public float HealthDesaturate;

	private static readonly int _rampTex = Shader.PropertyToID("_RampTex");

	private static readonly int _desat = Shader.PropertyToID("_Desat");

	private static readonly int _rampOffset = Shader.PropertyToID("_RampOffset");

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetTexture(_rampTex, textureRamp);
		base.material.SetFloat(_desat, Mathf.Clamp01(WeatherDesaturate + HealthDesaturate));
		base.material.SetVector(_rampOffset, new Vector4(rampOffsetR, rampOffsetG, rampOffsetB, 0f));
		Graphics.Blit(source, destination, base.material);
	}
}
