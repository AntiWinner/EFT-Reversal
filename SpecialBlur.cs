using EFT.BlitDebug;
using UnityEngine;

public class SpecialBlur : MonoBehaviour
{
	public Shader Shader;

	public RenderTexture From;

	public RenderTexture To;

	public RectTransform Target;

	public int BlurIterations = 1;

	public float Snrength = 1f;

	public float Aspect = 1f;

	private Material m__E000;

	private static readonly int m__E001 = Shader.PropertyToID(_ED3E._E000(41913));

	private void Start()
	{
		OnValidate();
	}

	private void OnValidate()
	{
		GetComponent<Camera>().aspect = Aspect;
		_E001(From, To);
	}

	private Material _E000()
	{
		if (this.m__E000 != null)
		{
			return this.m__E000;
		}
		return this.m__E000 = new Material(Shader);
	}

	private void _E001(RenderTexture from, RenderTexture to)
	{
		if (Target != null)
		{
			int num = (int)Target.rect.width;
			int num2 = (int)Target.rect.height;
			if (num != from.width || num2 != from.height)
			{
				from.Release();
				from.width = num;
				from.height = num2;
				from.Create();
				to.Release();
				to.width = num;
				to.height = num2;
				to.Create();
			}
		}
	}

	private void _E002(RenderTexture from, RenderTexture to)
	{
		_E001(from, to);
		int num = to.width >> 1;
		int num2 = to.height >> 1;
		RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0, RenderTextureFormat.ARGB32);
		temporary.name = _ED3E._E000(41866);
		Graphics.Blit(from, to);
		Material material = _E000();
		for (int i = 0; i < BlurIterations; i++)
		{
			float num3 = 1 << i;
			material.SetVector(SpecialBlur.m__E001, new Vector4(Snrength * num3 / (float)num, 0f, 0f, 0f));
			DebugGraphics.Blit(to, temporary, material, 0);
			material.SetVector(SpecialBlur.m__E001, new Vector4(0f, Snrength * num3 / (float)num2, 0f, 0f));
			DebugGraphics.Blit(temporary, to, material, 0);
		}
		RenderTexture.ReleaseTemporary(temporary);
	}

	public void OnPostRender()
	{
		_E002(From, To);
	}
}
