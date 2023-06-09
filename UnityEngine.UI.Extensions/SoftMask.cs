namespace UnityEngine.UI.Extensions;

[ExecuteInEditMode]
public class SoftMask : MonoBehaviour
{
	private Material m__E000;

	private Canvas m__E001;

	public Shader ImageMaskShader;

	[Tooltip("The area that is to be used as the container.")]
	public RectTransform MaskArea;

	private RectTransform m__E002;

	[Tooltip("A Rect Transform that can be used to scale and move the mask - Does not apply to Text UI Components being masked")]
	public RectTransform maskScalingRect;

	[Tooltip("Texture to be used to do the soft alpha")]
	public Texture AlphaMask;

	[Tooltip("At what point to apply the alpha min range 0-1")]
	[Range(0f, 1f)]
	public float CutOff;

	[Tooltip("Implement a hard blend based on the Cutoff")]
	public bool HardBlend;

	[Tooltip("Flip the masks alpha value")]
	public bool FlipAlphaMask;

	[Tooltip("If Mask Scaling Rect is given and this value is true, the area around the mask will not be clipped")]
	public bool DontClipMaskScalingRect;

	[Tooltip("If set to true, this mask is applied to all child Graphic objects belonging to this object.")]
	public bool CascadeToALLChildren;

	private Vector3[] _E003;

	private Vector2 _E004;

	private Vector2 _E005;

	private Vector2 _E006 = Vector2.one;

	private Vector2 _E007;

	private Vector2 _E008;

	private Vector2 _E009 = new Vector2(0.5f, 0.5f);

	private bool _E00A;

	private Rect _E00B;

	private Rect _E00C;

	private Vector2 _E00D;

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(242649));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(242636));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(242633));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(242630));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(242677));

	private static readonly int _E013 = Shader.PropertyToID(_ED3E._E000(242664));

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(242657));

	private static readonly int _E015 = Shader.PropertyToID(_ED3E._E000(115324));

	private void Start()
	{
		this.m__E002 = base.transform as RectTransform;
		if (!MaskArea)
		{
			MaskArea = this.m__E002;
		}
		if (ImageMaskShader == null)
		{
			ImageMaskShader = _E3AC.Find(_ED3E._E000(37545));
		}
		if (GetComponent<Graphic>() != null)
		{
			this.m__E000 = new Material(ImageMaskShader);
			GetComponent<Graphic>().material = this.m__E000;
		}
		if (CascadeToALLChildren)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				_E000(base.transform.GetChild(i));
			}
		}
		_E00A = this.m__E000 == null;
	}

	private void _E000(Transform t)
	{
		SoftMask softMask = t.gameObject.GetComponent<SoftMask>();
		if (softMask == null)
		{
			softMask = t.gameObject.AddComponent<SoftMask>();
		}
		softMask.MaskArea = MaskArea;
		softMask.AlphaMask = AlphaMask;
		softMask.CutOff = CutOff;
		softMask.HardBlend = HardBlend;
		softMask.FlipAlphaMask = FlipAlphaMask;
		softMask.maskScalingRect = maskScalingRect;
		softMask.DontClipMaskScalingRect = DontClipMaskScalingRect;
		softMask.CascadeToALLChildren = CascadeToALLChildren;
	}

	private void _E001()
	{
		Transform parent = base.transform;
		int num = 100;
		int num2 = 0;
		while (this.m__E001 == null && num2 < num)
		{
			this.m__E001 = parent.gameObject.GetComponent<Canvas>();
			if (this.m__E001 == null)
			{
				parent = parent.parent;
			}
			num2++;
		}
	}

	private void Update()
	{
		_E002();
	}

	private void _E002()
	{
		if (!_E00A)
		{
			_E00B = MaskArea.rect;
			_E00C = this.m__E002.rect;
			if (maskScalingRect != null)
			{
				_E00B = maskScalingRect.rect;
			}
			if (maskScalingRect != null)
			{
				_E00D = this.m__E002.transform.InverseTransformPoint(maskScalingRect.transform.TransformPoint(maskScalingRect.rect.center));
			}
			else
			{
				_E00D = this.m__E002.transform.InverseTransformPoint(MaskArea.transform.TransformPoint(MaskArea.rect.center));
			}
			_E00D += (Vector2)this.m__E002.transform.InverseTransformPoint(this.m__E002.transform.position) - this.m__E002.rect.center;
			_E004 = new Vector2(_E00B.width / _E00C.width, _E00B.height / _E00C.height);
			_E005 = _E00D;
			_E006 = _E005;
			_E008 = new Vector2(_E00B.width, _E00B.height) * 0.5f;
			_E005 -= _E008;
			_E006 += _E008;
			_E005 = new Vector2(_E005.x / _E00C.width, _E005.y / _E00C.height) + _E009;
			_E006 = new Vector2(_E006.x / _E00C.width, _E006.y / _E00C.height) + _E009;
			this.m__E000.SetFloat(_E00E, HardBlend ? 1 : 0);
			this.m__E000.SetVector(_E00F, _E005);
			this.m__E000.SetVector(_E010, _E006);
			this.m__E000.SetInt(_E011, FlipAlphaMask ? 1 : 0);
			this.m__E000.SetTexture(_E012, AlphaMask);
			this.m__E000.SetVector(_E013, _E004);
			this.m__E000.SetInt(_E014, (DontClipMaskScalingRect && maskScalingRect != null) ? 1 : 0);
			this.m__E000.SetFloat(_E015, CutOff);
		}
	}
}
