using UnityEngine;
using UnityStandardAssets.Water;

[ExecuteInEditMode]
[RequireComponent(typeof(WaterBase))]
public class WaterController : MonoBehaviour
{
	public Color WaterColor;

	public Color SpecularColor;

	[Range(0f, 500f)]
	public float Shininess;

	private WaterBase _E000;

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(89167));

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(95540));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(44830));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(95531));

	private void Update()
	{
		if (!_E000)
		{
			_E000 = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
		}
		if (_E000 == null)
		{
			return;
		}
		TOD_Sky instance = TOD_Sky.Instance;
		if (!(instance == null))
		{
			Vector3 sunDirection = instance.SunDirection;
			Color sunLightColor = instance.SunLightColor;
			float num = Mathf.Clamp01(sunDirection.y * 4f);
			Material sharedMaterial = _E000.sharedMaterial;
			if (!(sharedMaterial == null))
			{
				Color value = sunLightColor * WaterColor * num;
				value.a = WaterColor.a;
				sharedMaterial.SetColor(_E001, value);
				sharedMaterial.SetVector(_E002, -sunDirection);
				sharedMaterial.SetColor(_E003, SpecularColor * sunLightColor);
				sharedMaterial.SetFloat(_E004, Shininess);
			}
		}
	}
}
