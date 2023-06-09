using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Dithering")]
public class Dithering : MonoBehaviour
{
	public Texture DitTex;

	public Shader shader;

	private Material _E000;

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(18568));

	private Material _E002
	{
		get
		{
			if (_E000 == null)
			{
				_E000 = new Material(shader);
				_E000.hideFlags = HideFlags.HideAndDontSave;
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
		else if (shader == null)
		{
			Debug.Log(_ED3E._E000(41960));
			base.enabled = false;
		}
		else if (!shader.isSupported)
		{
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

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Material material = _E002;
		material.SetTexture(_E001, DitTex);
		Graphics.Blit(source, destination, material);
	}
}
