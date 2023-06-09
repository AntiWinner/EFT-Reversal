using UnityEngine;

namespace Systems.Effects;

[RequireComponent(typeof(Camera))]
public class ScreenColorBlender : MonoBehaviour
{
	[SerializeField]
	private Shader _shader;

	[SerializeField]
	private Color _color1;

	[SerializeField]
	private Color _color2;

	[SerializeField]
	private AnimationCurve _curve;

	[Range(0f, 12f)]
	[SerializeField]
	private int _downsamplingValue;

	[SerializeField]
	private bool _useExistingTexture;

	[SerializeField]
	private Texture2D _blendTexture;

	[Range(0f, 1f)]
	[SerializeField]
	private float _blendValue;

	private Material _E000;

	private Texture _E001;

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(92304));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(92292));

	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (_shader == null)
		{
			Debug.Log(_ED3E._E000(92200));
			base.enabled = false;
		}
		else if (!_shader.isSupported)
		{
			base.enabled = false;
		}
		_E000 = new Material(_shader)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		GenerateTextureAndReassignMaterial();
	}

	private void OnDestroy()
	{
		if (_E000 != null)
		{
			Object.DestroyImmediate(_E000);
		}
		if (!_useExistingTexture)
		{
			_blendTexture = null;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destanation)
	{
		_E000.SetFloat(_E002, Mathf.Clamp01(_blendValue));
		Graphics.Blit(source, destanation, _E000);
	}

	private void GenerateTextureAndReassignMaterial()
	{
		if (_E000 == null)
		{
			_E000 = new Material(_shader)
			{
				hideFlags = HideFlags.HideAndDontSave
			};
		}
		if (!_useExistingTexture)
		{
			int num = (int)Mathf.Pow(2f, _downsamplingValue);
			_E001 = _E441.GetRadialTexture(Screen.width / num, Screen.height / num, _color1, _color2, _curve);
		}
		else
		{
			_E001 = _blendTexture;
		}
		_E000.SetTexture(_E003, _E001);
	}
}
