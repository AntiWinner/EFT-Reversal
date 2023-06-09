using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/TopLight")]
[RequireComponent(typeof(Camera))]
public class TopLight : MonoBehaviour
{
	public Shader TheShader;

	public Color Color;

	private Material _E000;

	private Color _E001;

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(36528));

	private Material _E003
	{
		get
		{
			if (_E000 == null)
			{
				_E000 = new Material(TheShader);
				_E000.hideFlags = HideFlags.HideAndDontSave;
				_E000.SetColor(_E002, Color);
			}
			return _E000;
		}
	}

	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
		else if (TheShader == null)
		{
			string text = GetType().Name;
			Debug.Log(text + _ED3E._E000(83891) + text + _ED3E._E000(83919));
			base.enabled = false;
		}
		else if (!TheShader.isSupported)
		{
			string text2 = GetType().Name;
			Debug.Log(text2 + _ED3E._E000(83904) + text2 + _ED3E._E000(83919));
			base.enabled = false;
		}
	}

	protected void OnDisable()
	{
		if ((bool)_E000)
		{
			Object.DestroyImmediate(_E000);
		}
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Material material = _E003;
		if (_E001 != Color)
		{
			_E001 = Color;
			material.SetColor(_E002, Color);
		}
		source.anisoLevel = 0;
		source.filterMode = FilterMode.Point;
		Graphics.Blit(source, destination, material);
	}
}
