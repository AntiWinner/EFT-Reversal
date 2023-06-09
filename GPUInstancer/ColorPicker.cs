using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace GPUInstancer;

public class ColorPicker : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Texture2D satvalTex;

		public Color[] satvalColors;

		public float Hue;

		public Color[] hueColors;

		public Action resetSatValTexture;

		public float Saturation;

		public float Value;

		public GameObject result;

		public ColorPicker _003C_003E4__this;

		public GameObject hueGO;

		public Action dragH;

		public GameObject satvalGO;

		public Action dragSV;

		public Vector2 hueSz;

		public Action applyHue;

		public Action applySaturationValue;

		public GameObject hueKnob;

		public Action idle;

		public Vector2 satvalSz;

		public GameObject satvalKnob;

		internal void _E000()
		{
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					satvalTex.SetPixel(j, i, satvalColors[j + i * 2]);
				}
			}
			satvalTex.Apply();
		}

		internal void _E001()
		{
			int num = Mathf.Clamp((int)Hue, 0, 5);
			int num2 = (num + 1) % 6;
			Color color = Color.Lerp(hueColors[num], hueColors[num2], Hue - (float)num);
			satvalColors[3] = color;
			resetSatValTexture();
		}

		internal void _E002()
		{
			Vector2 vector = new Vector2(Saturation, Value);
			Vector2 vector2 = new Vector2(1f - vector.x, 1f - vector.y);
			Color color = vector2.x * vector2.y * satvalColors[0];
			Color color2 = vector.x * vector2.y * satvalColors[1];
			Color color3 = vector2.x * vector.y * satvalColors[2];
			Color color4 = vector.x * vector.y * satvalColors[3];
			Color color5 = color + color2 + color3 + color4;
			result.GetComponent<Image>().color = color5;
			if (_003C_003E4__this.m__E000 != color5)
			{
				if (_003C_003E4__this.m__E001 != null)
				{
					_003C_003E4__this.m__E001(color5);
				}
				if (_003C_003E4__this.m__E002 != null)
				{
					_003C_003E4__this.m__E002();
				}
				_003C_003E4__this.m__E000 = color5;
			}
		}

		internal void _E003()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (ColorPicker._E001(hueGO, out Vector2 vector))
				{
					_003C_003E4__this.m__E003 = dragH;
				}
				else if (ColorPicker._E001(satvalGO, out vector))
				{
					_003C_003E4__this.m__E003 = dragSV;
				}
			}
		}

		internal void _E004()
		{
			ColorPicker._E001(hueGO, out Vector2 vector);
			Hue = vector.y / hueSz.y * 6f;
			applyHue();
			applySaturationValue();
			hueKnob.transform.localPosition = new Vector2(hueKnob.transform.localPosition.x, vector.y);
			if (Input.GetMouseButtonUp(0))
			{
				_003C_003E4__this.m__E003 = idle;
			}
		}

		internal void _E005()
		{
			ColorPicker._E001(satvalGO, out Vector2 vector);
			Saturation = vector.x / satvalSz.x;
			Value = vector.y / satvalSz.y;
			applySaturationValue();
			satvalKnob.transform.localPosition = vector;
			if (Input.GetMouseButtonUp(0))
			{
				_003C_003E4__this.m__E003 = idle;
			}
		}
	}

	private Color m__E000 = Color.red;

	private Action<Color> m__E001;

	private Action m__E002;

	private Action m__E003;

	public Color Color
	{
		get
		{
			return this.m__E000;
		}
		set
		{
			_E004(value);
		}
	}

	public void SetOnValueChangeCallback(Action<Color> onValueChange)
	{
		this.m__E001 = onValueChange;
	}

	public void SetOnValueChangeCallback(Action onValueChange)
	{
		this.m__E002 = onValueChange;
	}

	private static void _E000(Color color, out float h, out float s, out float v)
	{
		float num = Mathf.Min(color.r, color.g, color.b);
		float num2 = Mathf.Max(color.r, color.g, color.b);
		float num3 = num2 - num;
		if (num3 == 0f)
		{
			h = 0f;
		}
		else if (num2 == color.r)
		{
			h = Mathf.Repeat((color.g - color.b) / num3, 6f);
		}
		else if (num2 == color.g)
		{
			h = (color.b - color.r) / num3 + 2f;
		}
		else
		{
			h = (color.r - color.g) / num3 + 4f;
		}
		s = ((num2 == 0f) ? 0f : (num3 / num2));
		v = num2;
	}

	private static bool _E001(GameObject go, out Vector2 result)
	{
		RectTransform rectTransform = (RectTransform)go.transform;
		Vector3 point = rectTransform.InverseTransformPoint(Input.mousePosition);
		result.x = Mathf.Clamp(point.x, rectTransform.rect.min.x, rectTransform.rect.max.x);
		result.y = Mathf.Clamp(point.y, rectTransform.rect.min.y, rectTransform.rect.max.y);
		return rectTransform.rect.Contains(point);
	}

	private static Vector2 _E002(GameObject go)
	{
		return ((RectTransform)go.transform).rect.size;
	}

	private GameObject _E003(string name)
	{
		return base.transform.Find(name).gameObject;
	}

	private void _E004(Color inputColor)
	{
		GameObject satvalGO = _E003(_ED3E._E000(76980));
		GameObject satvalKnob = _E003(_ED3E._E000(76964));
		GameObject hueGO = _E003(_ED3E._E000(77009));
		GameObject hueKnob = _E003(_ED3E._E000(77005));
		GameObject result = _E003(_ED3E._E000(76998));
		Color[] hueColors = new Color[6]
		{
			Color.red,
			Color.yellow,
			Color.green,
			Color.cyan,
			Color.blue,
			Color.magenta
		};
		Color[] satvalColors = new Color[4]
		{
			new Color(0f, 0f, 0f),
			new Color(0f, 0f, 0f),
			new Color(1f, 1f, 1f),
			hueColors[0]
		};
		Texture2D texture2D = new Texture2D(1, 7);
		for (int i = 0; i < 7; i++)
		{
			texture2D.SetPixel(0, i, hueColors[i % 6]);
		}
		texture2D.Apply();
		hueGO.GetComponent<Image>().sprite = Sprite.Create(texture2D, new Rect(0f, 0.5f, 1f, 6f), new Vector2(0.5f, 0.5f));
		Vector2 hueSz = _E002(hueGO);
		Texture2D satvalTex = new Texture2D(2, 2);
		satvalGO.GetComponent<Image>().sprite = Sprite.Create(satvalTex, new Rect(0.5f, 0.5f, 1f, 1f), new Vector2(0.5f, 0.5f));
		Action resetSatValTexture = delegate
		{
			for (int j = 0; j < 2; j++)
			{
				for (int k = 0; k < 2; k++)
				{
					satvalTex.SetPixel(k, j, satvalColors[k + j * 2]);
				}
			}
			satvalTex.Apply();
		};
		Vector2 satvalSz = _E002(satvalGO);
		_E000(inputColor, out var Hue, out var Saturation, out var Value);
		Action applyHue = delegate
		{
			int num = Mathf.Clamp((int)Hue, 0, 5);
			int num2 = (num + 1) % 6;
			Color color6 = Color.Lerp(hueColors[num], hueColors[num2], Hue - (float)num);
			satvalColors[3] = color6;
			resetSatValTexture();
		};
		Action applySaturationValue = delegate
		{
			Vector2 vector = new Vector2(Saturation, Value);
			Vector2 vector2 = new Vector2(1f - vector.x, 1f - vector.y);
			Color color = vector2.x * vector2.y * satvalColors[0];
			Color color2 = vector.x * vector2.y * satvalColors[1];
			Color color3 = vector2.x * vector.y * satvalColors[2];
			Color color4 = vector.x * vector.y * satvalColors[3];
			Color color5 = color + color2 + color3 + color4;
			result.GetComponent<Image>().color = color5;
			if (this.m__E000 != color5)
			{
				if (this.m__E001 != null)
				{
					this.m__E001(color5);
				}
				if (this.m__E002 != null)
				{
					this.m__E002();
				}
				this.m__E000 = color5;
			}
		};
		applyHue();
		applySaturationValue();
		satvalKnob.transform.localPosition = new Vector2(Saturation * satvalSz.x, Value * satvalSz.y);
		hueKnob.transform.localPosition = new Vector2(hueKnob.transform.localPosition.x, Hue / 6f * satvalSz.y);
		Action dragH = null;
		Action dragSV = null;
		Action idle = delegate
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (_E001(hueGO, out var result4))
				{
					this.m__E003 = dragH;
				}
				else if (_E001(satvalGO, out result4))
				{
					this.m__E003 = dragSV;
				}
			}
		};
		dragH = delegate
		{
			_E001(hueGO, out var result3);
			Hue = result3.y / hueSz.y * 6f;
			applyHue();
			applySaturationValue();
			hueKnob.transform.localPosition = new Vector2(hueKnob.transform.localPosition.x, result3.y);
			if (Input.GetMouseButtonUp(0))
			{
				this.m__E003 = idle;
			}
		};
		dragSV = delegate
		{
			_E001(satvalGO, out var result2);
			Saturation = result2.x / satvalSz.x;
			Value = result2.y / satvalSz.y;
			applySaturationValue();
			satvalKnob.transform.localPosition = result2;
			if (Input.GetMouseButtonUp(0))
			{
				this.m__E003 = idle;
			}
		};
		this.m__E003 = idle;
	}

	public void SetRandomColor()
	{
		System.Random random = new System.Random();
		float r = (float)(random.Next() % 1000) / 1000f;
		float g = (float)(random.Next() % 1000) / 1000f;
		float b = (float)(random.Next() % 1000) / 1000f;
		Color = new Color(r, g, b);
	}

	private void Awake()
	{
		Color = Color.red;
	}

	private void Update()
	{
		this.m__E003();
	}
}
