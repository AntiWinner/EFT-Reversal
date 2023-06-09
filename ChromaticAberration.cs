using EFT.BlitDebug;
using UnityEngine;

[AddComponentMenu("Image Effects/ChromaticAberration")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ChromaticAberration : MonoBehaviour
{
	public float Shift = 0.01f;

	public int Aniso = 3;

	public bool Simple;

	public Shader shader;

	private Material _E000;

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(44086));

	private Material _E002
	{
		get
		{
			if (_E000 == null)
			{
				_E000 = new Material(shader)
				{
					hideFlags = HideFlags.HideAndDontSave
				};
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

	protected void OnDestroy()
	{
		if ((bool)_E000)
		{
			Object.DestroyImmediate(_E000);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Material material = _E002;
		material.SetVector(_E001, new Vector4(1f - Shift, 1f + Shift, 1f - Shift * 0.5f, 1f + Shift * 0.5f));
		source.anisoLevel = Aniso;
		source.filterMode = FilterMode.Bilinear;
		DebugGraphics.Blit(source, destination, material, (!Simple) ? 1 : 0);
	}
}
