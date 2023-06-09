using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public abstract class TOD_ImageEffect : MonoBehaviour
{
	public TOD_Sky sky;

	protected Camera cam;

	private static readonly int _E000 = Shader.PropertyToID(_ED3E._E000(19728));

	private bool _E001;

	private bool _E002;

	protected Material CreateMaterial(Shader shader)
	{
		if (!shader)
		{
			Debug.Log(_ED3E._E000(19475) + ToString());
			base.enabled = false;
			return null;
		}
		if (!shader.isSupported)
		{
			Debug.LogError(_ED3E._E000(19518) + shader.ToString() + _ED3E._E000(19506) + ToString() + _ED3E._E000(19494));
			base.enabled = false;
			return null;
		}
		return new Material(shader)
		{
			hideFlags = HideFlags.DontSave
		};
	}

	protected void Awake()
	{
		_E001 = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth);
		_E002 = SystemInfo.SupportsRenderTextureFormat(RuntimeUtilities.defaultHDRRenderTextureFormat);
		if (!cam)
		{
			cam = GetComponent<Camera>();
		}
		if (!sky)
		{
			sky = TOD_Sky.Instance;
		}
	}

	protected bool CheckSupport(bool needDepth = false, bool needHdr = false)
	{
		if (!cam)
		{
			return false;
		}
		if (!sky || !sky.Initialized)
		{
			return false;
		}
		if (!SystemInfo.supportsImageEffects)
		{
			Debug.LogWarning(_ED3E._E000(19522) + ToString() + _ED3E._E000(19564));
			base.enabled = false;
			return false;
		}
		if (needDepth && !_E001)
		{
			Debug.LogWarning(_ED3E._E000(19522) + ToString() + _ED3E._E000(19630));
			base.enabled = false;
			return false;
		}
		if (needHdr && !_E002)
		{
			Debug.LogWarning(_ED3E._E000(19522) + ToString() + _ED3E._E000(19705));
			base.enabled = false;
			return false;
		}
		if (needDepth)
		{
			cam.depthTextureMode |= DepthTextureMode.Depth;
		}
		if (needHdr)
		{
			cam.allowHDR = true;
		}
		return true;
	}

	protected void DrawBorder(RenderTexture dest, Material material)
	{
		RenderTexture.active = dest;
		bool flag = true;
		GL.PushMatrix();
		GL.LoadOrtho();
		for (int i = 0; i < material.passCount; i++)
		{
			material.SetPass(i);
			float y;
			float y2;
			if (flag)
			{
				y = 1f;
				y2 = 0f;
			}
			else
			{
				y = 0f;
				y2 = 1f;
			}
			float x = 0f + 1f / ((float)dest.width * 1f);
			float y3 = 0f;
			float y4 = 1f;
			GL.Begin(7);
			GL.TexCoord2(0f, y);
			GL.Vertex3(0f, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(0f, y4, 0.1f);
			float x2 = 1f - 1f / ((float)dest.width * 1f);
			x = 1f;
			y3 = 0f;
			y4 = 1f;
			GL.TexCoord2(0f, y);
			GL.Vertex3(x2, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(x2, y4, 0.1f);
			x = 1f;
			y3 = 0f;
			y4 = 0f + 1f / ((float)dest.height * 1f);
			GL.TexCoord2(0f, y);
			GL.Vertex3(0f, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(0f, y4, 0.1f);
			x = 1f;
			y3 = 1f - 1f / ((float)dest.height * 1f);
			y4 = 1f;
			GL.TexCoord2(0f, y);
			GL.Vertex3(0f, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(0f, y4, 0.1f);
			GL.End();
		}
		GL.PopMatrix();
	}

	protected void CustomBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr = 0)
	{
		RenderTexture.active = dest;
		fxMaterial.SetTexture(_E000, source);
		GL.PushMatrix();
		GL.LoadOrtho();
		fxMaterial.SetPass(passNr);
		GL.Begin(7);
		GL.MultiTexCoord2(0, 0f, 0f);
		GL.Vertex3(0f, 0f, 3f);
		GL.MultiTexCoord2(0, 1f, 0f);
		GL.Vertex3(1f, 0f, 2f);
		GL.MultiTexCoord2(0, 1f, 1f);
		GL.Vertex3(1f, 1f, 1f);
		GL.MultiTexCoord2(0, 0f, 1f);
		GL.Vertex3(0f, 1f, 0f);
		GL.End();
		GL.PopMatrix();
	}
}
