using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Undithering")]
public class Undithering : MonoBehaviour
{
	public Shader shader;

	public bool useTriangleBlit = true;

	[SerializeField]
	private bool _distortion;

	private bool _E000;

	private Material _E001;

	private MaterialPropertyBlock _E002;

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(16473));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(82022));

	private CommandBuffer _E005;

	private Material _E006
	{
		get
		{
			if (_E001 == null)
			{
				_E001 = new Material(shader)
				{
					hideFlags = HideFlags.HideAndDontSave
				};
			}
			return _E001;
		}
	}

	private void Awake()
	{
		_E005 = new CommandBuffer();
		_E005.name = _ED3E._E000(81959);
	}

	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (shader == null)
		{
			Debug.Log(_ED3E._E000(82011));
			base.enabled = false;
		}
		else if (!shader.isSupported)
		{
			base.enabled = false;
		}
		TOD_Scattering component = base.gameObject.GetComponent<TOD_Scattering>();
		_E000 = component != null;
	}

	private void OnDestroy()
	{
		if ((bool)_E001)
		{
			Object.DestroyImmediate(_E001);
		}
		_E005?.Dispose();
		_E005 = null;
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		_E006.SetFloat(_E003, _distortion ? 1f : 0f);
		_E006.SetFloat(_E004, _E000 ? 1f : 0f);
		if (useTriangleBlit)
		{
			if (_E002 == null)
			{
				_E002 = new MaterialPropertyBlock();
			}
			_E005.Clear();
			_E005.BlitFullscreenTriangle(source, destination, _E006, 0, _E002);
			Graphics.ExecuteCommandBuffer(_E005);
		}
		else
		{
			Graphics.Blit(source, destination, _E006);
		}
	}
}
