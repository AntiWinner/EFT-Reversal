using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public sealed class MultiFlareLight : MonoBehaviour
{
	private sealed class _E000
	{
		private readonly Light m__E000;

		private readonly CullingLightObject _E001;

		public bool IsEnabled
		{
			get
			{
				if (_E001 != null && _E001.TakeFlareParametersFromCullingLight)
				{
					return _E001.IsLightEnabled;
				}
				return m__E000.enabled;
			}
		}

		public float Intensity
		{
			get
			{
				if (_E001 != null && _E001.TakeFlareParametersFromCullingLight)
				{
					return _E001.CurrentLightIntensity;
				}
				return m__E000.intensity;
			}
		}

		public bool IsVisible
		{
			get
			{
				if (!(_E001 == null))
				{
					return _E001.IsVisible;
				}
				return true;
			}
		}

		public _E000(Light light)
		{
			m__E000 = light;
			_E001 = light.gameObject.GetComponent<CullingLightObject>();
		}
	}

	[Serializable]
	public sealed class Flare
	{
		public int TextureId;

		public MultiFlare.EFlareType Type;

		public Vector2 Scale;

		public float Alpha;

		public float CenterShift = 1f;

		public float MinDist;

		public float MaxDist;

		public float MinScale;

		public float MaxScale;

		public float MinAlpha;

		public float MaxAlpha;

		public float SvWidth;

		public float SvShift;

		public MultiFlare.ERotationType RotType;

		public Color Color;

		public void DrawSelf(in int pI, ProFlareAtlas atlas, MultiFlareLight light, IList<Vector4> tangents, IList<Vector3> normals, IList<Vector2> uv0, IList<Vector2> uv1, IList<Vector2> uv2, IList<Vector2> uv3, IList<Color32> colors)
		{
			Vector2 vector = MinScale * light.Scale * Scale;
			Vector3 checkFadeOffset = light.CheckFadeOffset;
			float num = Alpha * MinAlpha;
			float y = 1f / (MaxDist - MinDist);
			int index = pI << 2;
			int index2 = index++;
			int index3 = index++;
			int index4 = index++;
			Rect uV = atlas.elementsList[TextureId].UV;
			switch (RotType)
			{
			case MultiFlare.ERotationType.Normal:
				uV.xMin -= 2f;
				uV.xMax -= 2f;
				break;
			case MultiFlare.ERotationType.Inverse:
				uV.yMin -= 2f;
				uV.yMax -= 2f;
				break;
			}
			uv0[index2] = new Vector2(uV.xMin, uV.yMax);
			uv0[index3] = new Vector2(uV.xMin, uV.yMin);
			uv0[index4] = new Vector2(uV.xMax, uV.yMin);
			uv0[index] = new Vector2(uV.xMax, uV.yMax);
			uv1[index2] = new Vector2(0f - vector.x, vector.y);
			uv1[index3] = new Vector2(0f - vector.x, 0f - vector.y);
			uv1[index4] = new Vector2(vector.x, 0f - vector.y);
			uv1[index] = new Vector2(vector.x, vector.y);
			uv2[index2] = new Vector2(checkFadeOffset.x, checkFadeOffset.y);
			uv2[index3] = new Vector2(checkFadeOffset.x, checkFadeOffset.y);
			uv2[index4] = new Vector2(checkFadeOffset.x, checkFadeOffset.y);
			uv2[index] = new Vector2(checkFadeOffset.x, checkFadeOffset.y);
			uv3[index2] = new Vector2(checkFadeOffset.z, 0f);
			uv3[index3] = new Vector2(checkFadeOffset.z, 0f);
			uv3[index4] = new Vector2(checkFadeOffset.z, 0f);
			uv3[index] = new Vector2(checkFadeOffset.z, 0f);
			Color32 color = Color * light.Color * num;
			color.a = (byte)(light.CurrentAlpha * 255f);
			Color32 color3 = (colors[index] = color);
			Color32 color5 = (colors[index4] = color3);
			Color32 value = (colors[index3] = color5);
			colors[index2] = value;
			Vector3 vector3 = (normals[index] = new Vector3(MinDist, y, CenterShift));
			Vector3 vector5 = (normals[index4] = vector3);
			Vector3 value2 = (normals[index3] = vector5);
			normals[index2] = value2;
			Vector4 vector8 = (tangents[index] = new Vector4((MaxScale - MinScale) / MinScale, (MaxAlpha - MinAlpha) / MinAlpha, SvShift, SvWidth));
			Vector4 vector10 = (tangents[index4] = vector8);
			Vector4 value3 = (tangents[index3] = vector10);
			tangents[index2] = value3;
		}
	}

	public MultiFlare Parent;

	public bool TrackPosition;

	public Light LightObject;

	public float Scale = 1f;

	public Vector3 CheckFadeOffset;

	public float Alpha = 1f;

	public Color Color = Color.white;

	public Flare[] Flares;

	[CompilerGenerated]
	private float m__E000;

	[CompilerGenerated]
	private float m__E001;

	private bool m__E002;

	private float m__E003 = 1f;

	private float _E004;

	private bool _E005;

	private bool _E006;

	private Vector3 _E007;

	private int _E008 = -1;

	private int _E009 = -1;

	private int _E00A = -1;

	private int _E00B = -1;

	private _E000 _E00C;

	public bool IsGenerating
	{
		get
		{
			if (base.enabled)
			{
				return base.gameObject.activeSelf;
			}
			return false;
		}
	}

	public bool IsVisible
	{
		get
		{
			if (_E00C != null)
			{
				return _E00C.IsVisible;
			}
			return true;
		}
	}

	public float DefaultAlpha
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E000 = value;
		}
	}

	public float CurrentAlpha
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		set
		{
			this.m__E001 = value;
		}
	}

	private bool _E00D => _E00A != -1;

	private bool _E00E => _E008 != -1;

	private void Awake()
	{
		this.m__E002 = LightObject != null;
		float currentAlpha = (DefaultAlpha = Alpha);
		CurrentAlpha = currentAlpha;
	}

	private void OnEnable()
	{
		if (Parent != null)
		{
			Parent.RegisterLight(this);
		}
		if (LightObject != null)
		{
			_E00C = new _E000(LightObject);
		}
	}

	private void OnDisable()
	{
		if (Parent != null)
		{
			Parent.RemoveLight(this);
		}
		_E00C = null;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.transform.position, _ED3E._E000(88319), allowScaling: true);
	}

	public void DrawSelf(ref int pI, in MultiFlare.EFlareType type, ProFlareAtlas atlas, IList<Vector3> vertices, IList<Vector4> tangents, IList<Vector3> normals, IList<Vector2> uv0, IList<Vector2> uv1, IList<Vector2> uv2, IList<Vector2> uv3, IList<Color32> colors, in Space space = Space.World)
	{
		if (_E00C == null && LightObject != null)
		{
			_E00C = new _E000(LightObject);
		}
		int num = pI << 2;
		switch (type)
		{
		case MultiFlare.EFlareType.Normal:
			_E008 = num;
			break;
		case MultiFlare.EFlareType.Shit:
			_E00A = num;
			break;
		}
		Vector3 vector = ((space == Space.World) ? base.transform.position : base.transform.localPosition);
		for (int i = 0; i < Flares.Length; i++)
		{
			Flare flare = Flares[i];
			if (flare.Type == type)
			{
				int index = num++;
				int index2 = num++;
				int index3 = num++;
				Vector3 vector3 = (vertices[num++] = vector);
				Vector3 vector5 = (vertices[index3] = vector3);
				Vector3 value = (vertices[index2] = vector5);
				vertices[index] = value;
				flare.DrawSelf(in pI, atlas, this, tangents, normals, uv0, uv1, uv2, uv3, colors);
				pI++;
			}
		}
		switch (type)
		{
		case MultiFlare.EFlareType.Normal:
			_E009 = num;
			break;
		case MultiFlare.EFlareType.Shit:
			_E00B = num;
			break;
		}
		this.m__E003 = CurrentAlpha;
		_E004 = _E00C?.Intensity ?? 8f;
		_E005 = _E00C?.IsEnabled ?? false;
		_E006 = base.enabled;
	}

	public void RefreshState(MultiFlare._E000 screen, MultiFlare._E000 shit)
	{
		if (this.m__E002 && LightObject == null)
		{
			base.enabled = false;
		}
		if (base.enabled)
		{
			base.enabled = base.gameObject.activeSelf;
		}
		if (LightObject != null && _E00C == null)
		{
			_E00C = new _E000(LightObject);
		}
		else if (LightObject == null && _E00C != null)
		{
			_E00C = null;
		}
		_E000(screen, shit);
		_E002(screen, shit);
	}

	private void _E000(MultiFlare._E000 screen, MultiFlare._E000 shit)
	{
		if (!_E001())
		{
			return;
		}
		byte a = ((_E00C != null) ? ((byte)(this.m__E003 * _E004 * 31.875f)) : ((byte)(this.m__E003 * 255f)));
		if (_E00E && screen.Colors != null)
		{
			for (int i = _E008; i < _E009; i++)
			{
				screen.Colors[i].a = a;
			}
			screen.ColorsWasChanged = true;
		}
		if (_E00D && shit.Colors != null)
		{
			for (int j = _E00A; j < _E00B; j++)
			{
				shit.Colors[j].a = a;
			}
			shit.ColorsWasChanged = true;
		}
	}

	private bool _E001()
	{
		if (Math.Abs(this.m__E003 - CurrentAlpha) > 0.001f || base.enabled != _E006)
		{
			_E006 = base.enabled;
			this.m__E003 = (_E006 ? CurrentAlpha : 0f);
			return true;
		}
		if (_E00C != null)
		{
			float intensity = _E00C.Intensity;
			bool isEnabled = _E00C.IsEnabled;
			if (Math.Abs(intensity - _E004) > 0.001f || isEnabled != _E005)
			{
				_E005 = isEnabled;
				_E004 = (_E005 ? intensity : 0f);
				return true;
			}
		}
		return LightObject == null;
	}

	private void _E002(MultiFlare._E000 screen, MultiFlare._E000 shit)
	{
		if (!_E003())
		{
			return;
		}
		Vector3 vector = _E007;
		if (_E00E && screen.Positions != null)
		{
			for (int i = _E008; i < _E009; i++)
			{
				screen.Positions[i] = vector;
			}
			screen.PositionsWasChanged = true;
		}
		if (_E00D && shit.Positions != null)
		{
			for (int j = _E00A; j < _E00B; j++)
			{
				shit.Positions[j] = vector;
			}
			shit.PositionsWasChanged = true;
		}
	}

	private bool _E003()
	{
		if (!TrackPosition)
		{
			return false;
		}
		Transform transform = base.transform;
		if (LightObject != null)
		{
			transform = LightObject.transform;
		}
		Vector3 position = transform.position;
		if (position == _E007)
		{
			return false;
		}
		_E007 = position;
		return true;
	}
}
