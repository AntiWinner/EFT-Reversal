using EFT.BlitDebug;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Pixelation : MonoBehaviour
{
	public enum PixelationMode
	{
		Point = 0,
		CRT = 1,
		None = 10
	}

	public bool On;

	public PixelationMode Mode;

	[SerializeField]
	private Shader _pixelationShader;

	[SerializeField]
	[Range(2f, 2048f)]
	private float _blockCount = 256f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _alpha = 1f;

	[SerializeField]
	private Texture _pixelationMask;

	private Camera m__E000;

	private Material m__E001;

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(88716));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(88765));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(88749));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(88797));

	private void Awake()
	{
		this.m__E000 = GetComponent<Camera>();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destanation)
	{
		if (!On || Mode == PixelationMode.None)
		{
			_E3A1.BlitOrCopy(source, destanation);
			return;
		}
		_E001();
		if (Mode != PixelationMode.None)
		{
			DebugGraphics.Blit(source, destanation, _E000(), (int)Mode);
		}
	}

	private Material _E000()
	{
		if (this.m__E001 != null)
		{
			return this.m__E001;
		}
		return this.m__E001 = new Material(_pixelationShader);
	}

	private void _E001()
	{
		Material material = _E000();
		Vector2 vector = new Vector2(_blockCount, _blockCount / this.m__E000.aspect);
		Vector2 vector2 = new Vector2(1f / vector.x, 1f / vector.y);
		material.SetVector(_E002, vector);
		material.SetVector(_E003, vector2);
		material.SetTexture(_E004, _pixelationMask);
		material.SetFloat(_E005, _alpha);
	}
}
