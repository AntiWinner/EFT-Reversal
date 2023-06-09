using UnityEngine;
using UnityEngine.Rendering;

public class MaskAndShadow : MonoBehaviour
{
	public Shader Shader;

	public Texture2D Mask;

	[Header("Shadow")]
	public Vector2 ShadowShift;

	public int ShadowBlurIterations = 1;

	public float ShadowBlurStrength = 1f;

	[Range(0f, 1f)]
	public float ShadowStrength = 1f;

	private Camera m__E000;

	private Material m__E001;

	private CommandBuffer m__E002;

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(88197));

	private Material _E000()
	{
		if (this.m__E001 != null)
		{
			return this.m__E001;
		}
		return this.m__E001 = new Material(Shader);
	}

	private void OnValidate()
	{
		if (this.m__E000 == null)
		{
			this.m__E000 = GetComponent<Camera>();
		}
		if (this.m__E002 == null)
		{
			this.m__E002 = new CommandBuffer
			{
				name = _ED3E._E000(90084)
			};
		}
		_E001();
		_E000().SetTexture(_E003, Mask);
	}

	private void Awake()
	{
		OnValidate();
		this.m__E000 = GetComponent<Camera>();
		CommandBuffer commandBuffer = new CommandBuffer
		{
			name = _ED3E._E000(88080)
		};
		CameraEvent evt = ((this.m__E000.actualRenderingPath == RenderingPath.DeferredShading) ? CameraEvent.BeforeGBuffer : CameraEvent.BeforeForwardOpaque);
		int num = Shader.PropertyToID(_ED3E._E000(88064));
		commandBuffer.GetTemporaryRT(num, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
		commandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, num);
		commandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.clear);
		commandBuffer.SetGlobalTexture(_ED3E._E000(88064), num);
		this.m__E000.AddCommandBuffer(evt, commandBuffer);
		_E001();
	}

	private void _E001()
	{
		if (this.m__E002 == null)
		{
			this.m__E002 = new CommandBuffer
			{
				name = _ED3E._E000(90084)
			};
		}
		this.m__E000.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.m__E002);
		this.m__E002.Clear();
		Material mat = _E000();
		int num = Shader.PropertyToID(_ED3E._E000(88112));
		this.m__E002.GetTemporaryRT(num, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
		this.m__E002.Blit(BuiltinRenderTextureType.CurrentActive, num);
		int num2 = _E002(this.m__E002, mat, num);
		this.m__E002.SetGlobalTexture(_ED3E._E000(88107), num2);
		this.m__E002.SetGlobalVector(_ED3E._E000(88148), ShadowShift);
		this.m__E002.SetGlobalFloat(_ED3E._E000(88133), ShadowStrength);
		this.m__E002.Blit(num, BuiltinRenderTextureType.CameraTarget, mat, 2);
		this.m__E000.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.m__E002);
	}

	private int _E002(CommandBuffer buffer, Material mat, int blurTex)
	{
		int pixelWidth = this.m__E000.pixelWidth;
		int pixelHeight = this.m__E000.pixelHeight;
		int num = Shader.PropertyToID(_ED3E._E000(88177));
		int num2 = Shader.PropertyToID(_ED3E._E000(88167));
		buffer.GetTemporaryRT(num, -2, -2, 0, FilterMode.Bilinear, RenderTextureFormat.R8);
		buffer.GetTemporaryRT(num2, -2, -2, 0, FilterMode.Bilinear, RenderTextureFormat.R8);
		buffer.Blit(blurTex, num, mat, 1);
		for (int i = 0; i < ShadowBlurIterations; i++)
		{
			float num3 = (float)(1 << i) * ShadowBlurStrength;
			buffer.SetGlobalVector(_ED3E._E000(88213), new Vector4(num3 / (float)pixelWidth, 0f, 0f, 0f));
			buffer.Blit(num, num2, mat, 0);
			buffer.SetGlobalVector(_ED3E._E000(88213), new Vector4(0f, num3 / (float)pixelHeight, 0f, 0f));
			buffer.Blit(num2, num, mat, 0);
		}
		buffer.ReleaseTemporaryRT(num2);
		return num;
	}
}
