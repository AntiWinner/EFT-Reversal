using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[ExecuteInEditMode]
public class StaticDeferredDecal : MonoBehaviour
{
	[HideInInspector]
	public Vector2 LowerLeftPixel = Vector2.zero;

	[HideInInspector]
	public Vector2 UpperRightPixel = Vector2.zero;

	[SerializeField]
	public Material _material;

	[Range(0f, 9f)]
	[SerializeField]
	private int _decalQueue;

	[SerializeField]
	[Range(-1f, 1f)]
	private float _normalClip = 0.8f;

	[SerializeField]
	private float _normalAlphaFadeoff = 10f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _edgeClip;

	[SerializeField]
	private float _edgeAlphaFadeoff = 1f;

	[SerializeField]
	private Color _tint = Color.white;

	private Action m__E000;

	private Action _E001;

	private Vector3 _E002;

	private Quaternion _E003;

	private Vector3 _E004;

	[CompilerGenerated]
	private static Action<StaticDeferredDecal, bool> _E005;

	[CompilerGenerated]
	private static Action<StaticDeferredDecal, bool> _E006;

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(19728));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(44504));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(44499));

	[CompilerGenerated]
	private Texture2D _E00A;

	[HideInInspector]
	public Vector2 LastTexSize = Vector2.zero;

	public int DecalQueue => _decalQueue;

	public Material DecalMaterial
	{
		get
		{
			return _material;
		}
		private set
		{
			_material = value;
		}
	}

	public Texture2D Tex
	{
		[CompilerGenerated]
		get
		{
			return _E00A;
		}
		[CompilerGenerated]
		private set
		{
			_E00A = value;
		}
	}

	public Color Tint => _tint;

	public static event Action<StaticDeferredDecal, bool> OnDecalRegister
	{
		[CompilerGenerated]
		add
		{
			Action<StaticDeferredDecal, bool> action = _E005;
			Action<StaticDeferredDecal, bool> action2;
			do
			{
				action2 = action;
				Action<StaticDeferredDecal, bool> value2 = (Action<StaticDeferredDecal, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E005, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<StaticDeferredDecal, bool> action = _E005;
			Action<StaticDeferredDecal, bool> action2;
			do
			{
				action2 = action;
				Action<StaticDeferredDecal, bool> value2 = (Action<StaticDeferredDecal, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E005, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<StaticDeferredDecal, bool> OnDecalUnregister
	{
		[CompilerGenerated]
		add
		{
			Action<StaticDeferredDecal, bool> action = _E006;
			Action<StaticDeferredDecal, bool> action2;
			do
			{
				action2 = action;
				Action<StaticDeferredDecal, bool> value2 = (Action<StaticDeferredDecal, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E006, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<StaticDeferredDecal, bool> action = _E006;
			Action<StaticDeferredDecal, bool> action2;
			do
			{
				action2 = action;
				Action<StaticDeferredDecal, bool> value2 = (Action<StaticDeferredDecal, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E006, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Awake()
	{
		if (_material == null)
		{
			base.enabled = false;
			return;
		}
		_E000();
		if ((double)(UpperRightPixel - LowerLeftPixel).sqrMagnitude < 0.0001)
		{
			UpperRightPixel = new Vector2(Tex.width - 1, Tex.height - 1);
			LowerLeftPixel = Vector2.zero;
		}
	}

	private void _E000()
	{
		Tex = _material.GetTexture(_E007) as Texture2D;
	}

	public Vector4 GetUVStartEnd()
	{
		if (Tex == null)
		{
			_E000();
		}
		if (Tex == null)
		{
			return Vector4.zero;
		}
		int width = Tex.width;
		int height = Tex.height;
		return new Vector4(LowerLeftPixel.x / (float)width, LowerLeftPixel.y / (float)height, UpperRightPixel.x / (float)width, UpperRightPixel.y / (float)height);
	}

	public Vector4 GetNormalEdgeClipAndFadeoff()
	{
		return new Vector4(_normalClip, _normalAlphaFadeoff, _edgeClip * 0.5f, _edgeAlphaFadeoff);
	}

	private void OnEnable()
	{
		_E005?.Invoke(this, arg2: true);
	}

	private void OnDisable()
	{
		_E006?.Invoke(this, arg2: true);
	}

	public void Refresh()
	{
		Awake();
	}
}
