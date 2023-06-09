using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChartAndGraph;

internal class ChartMaterialController : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	private static bool m__E000;

	public bool HandleEvents = true;

	[SerializeField]
	private ChartDynamicMaterial materials;

	internal ChartItemMaterialLerpEffect _E001;

	private Color m__E002 = Color.clear;

	private Color m__E003;

	private Renderer m__E004;

	private Material m__E005;

	private Graphic m__E006;

	private bool _E007;

	private bool _E008;

	private int? _E009;

	private float _E00A;

	private bool _E00B;

	internal ChartDynamicMaterial _E00C
	{
		get
		{
			return materials;
		}
		set
		{
			materials = value;
			if (materials != null)
			{
				if (materials.Normal == null)
				{
					_E001();
				}
				else
				{
					_E000(materials.Normal, null);
				}
			}
		}
	}

	private void _E000(Material m, Material fallback)
	{
		if (this.m__E004 == null)
		{
			this.m__E004 = GetComponent<Renderer>();
		}
		if (this.m__E004 != null)
		{
			_ED15._E00C(this.m__E004, m, fallback);
			return;
		}
		if (this.m__E006 == null)
		{
			this.m__E006 = GetComponent<Graphic>();
		}
		if (this.m__E005 == null)
		{
			this.m__E005 = new Material(materials.Normal);
			this.m__E005.hideFlags = HideFlags.DontSave;
		}
		if (this.m__E006 != null)
		{
			this.m__E006.material = this.m__E005;
		}
	}

	private void _E001()
	{
		if (!ChartMaterialController.m__E000)
		{
			Debug.LogWarning(_ED3E._E000(243771));
			ChartMaterialController.m__E000 = true;
		}
	}

	private void Start()
	{
		this._E001 = GetComponent<ChartItemMaterialLerpEffect>();
		Refresh();
	}

	private int _E002()
	{
		if (!_E009.HasValue)
		{
			_E009 = Shader.PropertyToID(_ED3E._E000(243801));
		}
		return _E009.Value;
	}

	public void TriggerOn()
	{
		_E007 = true;
		Refresh();
	}

	public void TriggerOff()
	{
		_E007 = false;
		Refresh();
	}

	private void OnMouseEnter()
	{
		if (HandleEvents)
		{
			_E007 = true;
			Refresh();
		}
	}

	private void OnMouseExit()
	{
		if (HandleEvents)
		{
			_E007 = false;
			Refresh();
		}
	}

	private void OnMouseDown()
	{
		if (HandleEvents)
		{
			_E008 = true;
			Refresh();
		}
	}

	private void OnMouseUp()
	{
		if (HandleEvents)
		{
			_E008 = false;
			Refresh();
		}
	}

	private void Update()
	{
		if (this._E001 != null && _E00B)
		{
			_E00A += Time.deltaTime;
			if (_E00A > this._E001.LerpTime)
			{
				_E005(this.m__E002);
			}
			else
			{
				_E005(Color.Lerp(this.m__E003, this.m__E002, _E00A / this._E001.LerpTime));
			}
		}
	}

	private Color _E003(Material m)
	{
		if (m.HasProperty(_E002()))
		{
			return m.GetColor(_E002());
		}
		return m.color;
	}

	private void _E004(Material m, Color c)
	{
		if (m.HasProperty(_E002()))
		{
			m.SetColor(_E002(), c);
		}
		else
		{
			m.color = c;
		}
	}

	private void _E005(Color c)
	{
		if (this.m__E004 == null)
		{
			this.m__E004 = GetComponent<Renderer>();
		}
		if (this.m__E004 != null)
		{
			if (c == _E003(materials.Normal) && !_E007 && !_E008)
			{
				_ED15.SafeDestroy(this.m__E004.material);
				this.m__E004.material = null;
				this.m__E004.sharedMaterial = materials.Normal;
				_E00B = false;
			}
			else
			{
				_E004(this.m__E004.material, c);
			}
			return;
		}
		if (this.m__E006 == null)
		{
			this.m__E006 = GetComponent<Graphic>();
		}
		if (this.m__E006 != null)
		{
			if (this.m__E005 == null)
			{
				this.m__E005 = new Material(materials.Normal);
				this.m__E005.hideFlags = HideFlags.DontSave;
				this.m__E006.material = this.m__E005;
			}
			if (this.m__E006.material != this.m__E005)
			{
				this.m__E006.material = this.m__E005;
			}
			_E004(this.m__E005, c);
			if (c == _E003(materials.Normal) && !_E007 && !_E008)
			{
				_E00B = false;
				this.m__E006.material = materials.Normal;
			}
		}
	}

	private void _E006(Color c)
	{
		if (_ED15._E003)
		{
			return;
		}
		if (c == Color.clear)
		{
			c = _E003(materials.Normal);
		}
		if (this._E001 == null)
		{
			_E005(c);
			return;
		}
		if (this.m__E005 == null)
		{
			this.m__E005 = new Material(materials.Normal);
			this.m__E005.hideFlags = HideFlags.DontSave;
			if (this.m__E006 != null)
			{
				this.m__E006.material = this.m__E005;
			}
		}
		if (this.m__E004 != null)
		{
			this.m__E003 = _E003(this.m__E004.material);
		}
		else if (this.m__E006 != null && this.m__E005 != null)
		{
			this.m__E003 = _E003(this.m__E005);
		}
		else
		{
			this.m__E003 = _E003(materials.Normal);
		}
		this.m__E002 = c;
		_E00B = true;
		_E00A = 0f;
	}

	private void OnDestroy()
	{
		if (this.m__E004 != null)
		{
			_ED15.SafeDestroy(this.m__E004.material);
		}
		_ED15.SafeDestroy(this.m__E005);
	}

	public void Refresh()
	{
		if (_E00C != null && !(materials.Normal == null))
		{
			if (!_E007)
			{
				_E006(Color.clear);
			}
			else if (!_E008 || _E00C.Selected == Color.clear)
			{
				_E006(materials.Hover);
			}
			else
			{
				_E006(materials.Selected);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		OnMouseEnter();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		OnMouseExit();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		OnMouseDown();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		OnMouseUp();
	}
}
