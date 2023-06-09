using System;
using UnityEngine;

public class AnticheatMipMapChecker : MonoBehaviour
{
	[Serializable]
	private class TextureChecker
	{
		[SerializeField]
		public Texture2D CheckTexture;

		[SerializeField]
		public float CheckValue;
	}

	public static AnticheatMipMapChecker Instance;

	[HideInInspector]
	public bool IsMipMapCorrect = true;

	[SerializeField]
	private Shader _mipMapChecker;

	[SerializeField]
	private TextureChecker textureChecker;

	private Material m__E000;

	private RenderTexture m__E001;

	private Texture2D m__E002;

	private Material m__E003;

	private static readonly int m__E004 = Shader.PropertyToID(_ED3E._E000(19728));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(41479));

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		_E000();
		IsMipMapCorrect = _E001();
		_E002(IsMipMapCorrect);
		_E004();
	}

	private void _E000()
	{
		this.m__E001 = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default)
		{
			name = _ED3E._E000(41502),
			autoGenerateMips = true,
			useMipMap = true
		};
		RenderTexture active = RenderTexture.active;
		Graphics.SetRenderTarget(this.m__E001);
		GL.Clear(clearDepth: true, clearColor: true, Color.clear);
		Material material = _E003();
		material.SetTexture(AnticheatMipMapChecker.m__E004, textureChecker.CheckTexture);
		material.SetPass(0);
		float x = -1f;
		float x2 = 1f;
		float y = -1f;
		float y2 = 1f;
		GL.Begin(7);
		material.SetPass(0);
		GL.Begin(7);
		GL.TexCoord2(0f, 0f);
		GL.Vertex3(x, y, 0f);
		GL.TexCoord2(1f, 0f);
		GL.Vertex3(x2, y, 0f);
		GL.TexCoord2(1f, 1f);
		GL.Vertex3(x2, y2, 0f);
		GL.TexCoord2(0f, 1f);
		GL.Vertex3(x, y2, 0f);
		GL.End();
		this.m__E002 = new Texture2D(512, 512, TextureFormat.RGBAFloat, mipChain: true);
		this.m__E002.name = _ED3E._E000(41488);
		this.m__E002.ReadPixels(new Rect(0f, 0f, 512f, 512f), 0, 0);
		this.m__E002.Apply();
		RenderTexture.active = active;
	}

	private bool _E001()
	{
		Color[] pixels = this.m__E002.GetPixels();
		float num = 0f;
		for (int i = 0; i < pixels.Length; i++)
		{
			num += pixels[i].a;
		}
		return Math.Abs(textureChecker.CheckValue - num) < 20f;
	}

	private void _E002(bool isMipMapCorrect)
	{
		Shader.SetGlobalFloat(_E005, (!isMipMapCorrect) ? 1 : 0);
	}

	private Material _E003()
	{
		if (this.m__E000 != null)
		{
			return this.m__E000;
		}
		return this.m__E000 = new Material(_mipMapChecker);
	}

	private void _E004()
	{
		UnityEngine.Object.DestroyImmediate(this.m__E001);
		this.m__E001 = null;
		UnityEngine.Object.DestroyImmediate(this.m__E002);
		this.m__E002 = null;
		UnityEngine.Object.DestroyImmediate(this.m__E000);
	}
}
