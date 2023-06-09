using System;
using UnityEngine;

[Serializable]
public class CustomLensFlare
{
	[Serializable]
	public class Element
	{
		public float Position;

		public float Size;

		public float Rotation;

		public Color Color;

		public int X;

		public int Y;

		public int Width;

		public int Heigth;

		private Vector3[] _texCoords;

		private Vector2 _halfSize;

		private float _screenAspect;

		public void Calc(Vector2 texScale, float screenAspect)
		{
			float x = (float)X * texScale.x;
			float y = (float)Y * texScale.y;
			float x2 = (float)(X + Width) * texScale.x;
			float y2 = (float)(Y + Heigth) * texScale.y;
			_texCoords = new Vector3[4]
			{
				new Vector3(x, y, 0f),
				new Vector3(x, y2, 0f),
				new Vector3(x2, y2, 0f),
				new Vector3(x2, y, 0f)
			};
			_screenAspect = screenAspect;
			_halfSize = new Vector2(1f, screenAspect) * Size * 0.5f;
		}

		public void Draw(Vector2 pos, float distance, float alpha)
		{
			GL.Color(Color * alpha);
			if (Rotation == 0f)
			{
				GL.TexCoord(_texCoords[0]);
				GL.Vertex3(pos.x - _halfSize.x, pos.y - _halfSize.y, 0f);
				GL.TexCoord(_texCoords[1]);
				GL.Vertex3(pos.x - _halfSize.x, pos.y + _halfSize.y, 0f);
				GL.TexCoord(_texCoords[2]);
				GL.Vertex3(pos.x + _halfSize.x, pos.y + _halfSize.y, 0f);
				GL.TexCoord(_texCoords[3]);
				GL.Vertex3(pos.x + _halfSize.x, pos.y - _halfSize.y, 0f);
			}
			else
			{
				float f = distance * Rotation;
				float num = Mathf.Cos(f);
				float num2 = Mathf.Sin(f);
				float num3 = (num - num2) * _halfSize.x;
				float num4 = (num + num2) * _halfSize.x;
				float num5 = num3 * _screenAspect;
				float num6 = num4 * _screenAspect;
				GL.TexCoord(_texCoords[0]);
				GL.Vertex3(pos.x - num3, pos.y - num6, 0f);
				GL.TexCoord(_texCoords[1]);
				GL.Vertex3(pos.x - num4, pos.y + num5, 0f);
				GL.TexCoord(_texCoords[2]);
				GL.Vertex3(pos.x + num3, pos.y + num6, 0f);
				GL.TexCoord(_texCoords[3]);
				GL.Vertex3(pos.x + num4, pos.y - num5, 0f);
			}
		}
	}

	public Material FlareMaterial;

	public int GridSize = 64;

	public Element[] Elements;

	private Vector2 _center;

	public void Initialize()
	{
		if (!(FlareMaterial == null) && Elements.Length != 0)
		{
			Vector2 texScale = new Vector2((float)GridSize / (float)FlareMaterial.mainTexture.width, (float)GridSize / (float)FlareMaterial.mainTexture.height);
			float screenAspect = (float)Screen.width / (float)Screen.height;
			_center = new Vector2(0.5f, 0.5f);
			Element[] elements = Elements;
			for (int i = 0; i < elements.Length; i++)
			{
				elements[i].Calc(texScale, screenAspect);
			}
		}
	}

	public void Draw(Vector2 positionOnScreen)
	{
		if (!(FlareMaterial == null) && Elements.Length != 0)
		{
			Vector2 vector = _center - positionOnScreen;
			float magnitude = vector.magnitude;
			float alpha = 1f - magnitude * 0.65f;
			magnitude = Mathf.Sqrt(magnitude * 0.333333f);
			GL.PushMatrix();
			FlareMaterial.SetPass(0);
			GL.LoadOrtho();
			GL.Begin(7);
			Element[] elements = Elements;
			foreach (Element element in elements)
			{
				Vector2 pos = _center + vector * element.Position;
				element.Draw(pos, magnitude, alpha);
			}
			GL.End();
			GL.PopMatrix();
		}
	}
}
