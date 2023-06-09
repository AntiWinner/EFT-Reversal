using UnityEngine;

public class CirclePacker : MonoBehaviour
{
	public class _E000
	{
		public Vector2 Pos;

		public float Radius;

		public float Alpha;
	}

	public bool Generate;

	public int Seed;

	public int Count = 16;

	[_E2BD(0f, 1f, -1f)]
	public Vector2 Radius;

	[Range(0.8f, 1.2f)]
	public float TuneSize = 1f;

	[Range(1f, 255f)]
	public int Iterations;

	[Range(0f, 1f)]
	public float AlphaRandomness = 1f;

	public int TextureSize = 128;

	public bool FadeMips;

	[_E2BD(0f, 1f, -1f)]
	public Vector2 FadeMinMax;

	public Material Material;

	public Material ViewMaterial;

	public bool ExportTexture;

	private RenderTexture m__E000;

	private _E000[] m__E001;

	private static readonly int m__E002 = Shader.PropertyToID(_ED3E._E000(44102));

	private void OnValidate()
	{
		if (Generate)
		{
			Random.InitState(Seed);
			Vector2 vector = Radius / Mathf.Sqrt(Count);
			this.m__E001 = new _E000[Count];
			for (int i = 0; i < this.m__E001.Length; i++)
			{
				this.m__E001[i] = new _E000();
				this.m__E001[i].Pos = new Vector2(Random.value, Random.value);
				this.m__E001[i].Radius = Random.Range(vector.x, vector.y);
			}
			_E000(this.m__E001, Iterations);
			_E003(this.m__E001, 1f);
			if (this.m__E000 == null || TextureSize != this.m__E000.width)
			{
				this.m__E000 = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32)
				{
					name = _ED3E._E000(44077),
					wrapMode = TextureWrapMode.Repeat
				};
			}
		}
		if (this.m__E001 != null)
		{
			Graphics.SetRenderTarget(this.m__E000);
			GL.Clear(clearDepth: true, clearColor: true, new Color(0f, 0f, 0f, 0f));
			_E004(this.m__E001, Material, 0, TuneSize);
			ViewMaterial.mainTexture = this.m__E000;
		}
		if (ExportTexture)
		{
			Texture2D texture = _E006(this.m__E000, FadeMips);
			if (FadeMips)
			{
				_E007(texture, FadeMinMax.x, FadeMinMax.y);
			}
			_E008(texture, FadeMips);
			ExportTexture = false;
		}
	}

	private static void _E000(_E000[] circles, int iterationCount)
	{
		for (int i = 0; i < iterationCount; i++)
		{
			foreach (_E000 obj in circles)
			{
				foreach (_E000 obj2 in circles)
				{
					if (obj != obj2)
					{
						Vector2 v = obj2.Pos - obj.Pos;
						v = _E002(v);
						float num = obj.Radius + obj2.Radius;
						float num2 = v.x * v.x + v.y * v.y;
						if (num2 < num * num * 1.02f)
						{
							Vector2 normalized = v.normalized;
							normalized *= (num - Mathf.Sqrt(num2)) * 0.5f;
							obj2.Pos += normalized;
							obj.Pos -= normalized;
							obj2.Pos = _E001(obj2.Pos);
							obj.Pos = _E001(obj.Pos);
						}
					}
				}
			}
		}
	}

	private static Vector2 _E001(Vector2 v)
	{
		if (v.x < 0f)
		{
			v.x += 1f;
		}
		if (v.y < 0f)
		{
			v.y += 1f;
		}
		if (v.x > 1f)
		{
			v.x -= 1f;
		}
		if (v.y > 1f)
		{
			v.y -= 1f;
		}
		return v;
	}

	private static Vector2 _E002(Vector2 v)
	{
		if (v.x < -0.5f)
		{
			v.x += 1f;
		}
		if (v.y < -0.5f)
		{
			v.y += 1f;
		}
		if (v.x > 0.5f)
		{
			v.x -= 1f;
		}
		if (v.y > 0.5f)
		{
			v.y -= 1f;
		}
		return v;
	}

	private static void _E003(_E000[] circles, float randomness)
	{
		float[] array = new float[circles.Length];
		float num = 1f / (float)array.Length;
		float num2 = num * 0.5f;
		float num3 = num2 * randomness;
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = num2 + (float)i * num + Random.Range(0f - num3, num3);
		}
		for (int j = 0; j < array.Length; j++)
		{
			int num4 = Random.Range(0, array.Length - 1);
			if (j != num4)
			{
				float num5 = array[j];
				array[j] = array[num4];
				array[num4] = num5;
			}
		}
		for (int k = 0; k < circles.Length; k++)
		{
			circles[k].Alpha = array[k];
		}
	}

	private static void _E004(_E000[] circles, Material material, int pass, float tuneSize)
	{
		material.SetPass(pass);
		material.SetFloat(CirclePacker.m__E002, tuneSize);
		GL.Begin(7);
		Vector2 vector = default(Vector2);
		foreach (_E000 obj in circles)
		{
			Vector2 pos = obj.Pos;
			vector.x = ((pos.x < 0.5f) ? 1f : (-1f));
			vector.y = ((pos.y < 0.5f) ? 1f : (-1f));
			GL.Color(new Color(0f, 0f, 0f, obj.Alpha));
			_E005(pos, obj.Radius);
			_E005(pos + new Vector2(0f, vector.y), obj.Radius);
			_E005(pos + new Vector2(vector.x, 0f), obj.Radius);
			_E005(pos + new Vector2(vector.x, vector.y), obj.Radius);
		}
		GL.End();
	}

	private static void _E005(Vector2 center, float size)
	{
		center = center * 2f - Vector2.one;
		size *= 2f;
		Vector2 vector = new Vector2(center.x - size, center.y - size);
		Vector2 vector2 = new Vector2(center.x + size, center.y + size);
		GL.TexCoord2(0f, 0f);
		GL.Vertex3(vector.x, vector.y, 0f);
		GL.TexCoord2(0f, 1f);
		GL.Vertex3(vector.x, vector2.y, 0f);
		GL.TexCoord2(1f, 1f);
		GL.Vertex3(vector2.x, vector2.y, 0f);
		GL.TexCoord2(1f, 0f);
		GL.Vertex3(vector2.x, vector.y, 0f);
	}

	private static Texture2D _E006(RenderTexture renderTexture, bool mipMaps)
	{
		RenderTexture.active = renderTexture;
		Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, mipMaps);
		texture2D.name = _ED3E._E000(44113);
		texture2D.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0, mipMaps);
		texture2D.Apply(mipMaps, makeNoLongerReadable: false);
		return texture2D;
	}

	private static void _E007(Texture2D texture, float a, float b)
	{
		int mipmapCount = texture.mipmapCount;
		for (int i = 0; i < mipmapCount; i++)
		{
			float value = (float)i / (float)(mipmapCount - 1);
			Color32[] pixels = texture.GetPixels32(i);
			float num = Mathf.InverseLerp(b, a, value);
			for (int j = 0; j < pixels.Length; j++)
			{
				pixels[j].r = (byte)((float)(int)pixels[j].r * num);
			}
			texture.SetPixels32(pixels, i);
		}
		texture.Apply(updateMipmaps: false);
	}

	private static void _E008(Texture2D texture, bool asAsset)
	{
	}
}
