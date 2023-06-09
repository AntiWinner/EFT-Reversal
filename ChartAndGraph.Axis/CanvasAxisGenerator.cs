using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph.Axis;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
internal class CanvasAxisGenerator : Image, _ED2E
{
	private MeshRenderer m__E000;

	private MeshFilter m__E001;

	private Mesh _E002;

	private AxisBase _E003;

	private AnyChart _E004;

	private ChartOrientation _E005;

	private bool _E006;

	private Material _E007;

	private Material _E008;

	private _ED1E _E009;

	private float _E00A = 1f;

	private double _E00B;

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(245686));

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		base.OnPopulateMesh(vh);
		vh.Clear();
		if (!(_E003 == null) && !(_E004 == null))
		{
			_ED1E mesh = new _ED1E(vh);
			_E000(mesh);
		}
	}

	private void _E000(_ED1E mesh)
	{
		mesh.Orientation = _E005;
		if (_E006)
		{
			_E003._E005(_E00B, _E004, base.transform, mesh, _E005);
		}
		else
		{
			_E003._E007(_E00B, _E004, base.transform, mesh, _E005);
		}
	}

	protected override void OnPopulateMesh(Mesh m)
	{
		m.Clear();
		if (!(_E003 == null) && !(_E004 == null))
		{
			_ED21 obj = new _ED21(isCanvas: true);
			obj.Orientation = _E005;
			if (_E006)
			{
				_E003._E007(_E00B, _E004, base.transform, obj, _E005);
			}
			else
			{
				_E003._E005(_E00B, _E004, base.transform, obj, _E005);
			}
			obj.ApplyToMesh(m);
		}
	}

	protected override void UpdateMaterial()
	{
		base.UpdateMaterial();
		if (!(material == null))
		{
			if (material.mainTexture != null)
			{
				base.canvasRenderer.SetTexture(material.mainTexture);
			}
			base.canvasRenderer.SetColor(Color.white);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (_E007 != null)
		{
			_ED15.SafeDestroy(_E007);
		}
	}

	private float _E001(MaterialTiling tiling)
	{
		if (!tiling.EnableTiling || tiling.TileFactor <= 0f)
		{
			return 1f;
		}
		ChartDivisionInfo info = _E003.MainDivisions;
		if (_E006)
		{
			info = _E003.SubDivisions;
		}
		return _ED15._E006(_E004, _E005, info) / tiling.TileFactor;
	}

	public void SetAxis(double scrollOffset, AnyChart parent, AxisBase axis, ChartOrientation axisOrientation, bool isSubDivisions)
	{
		_E00B = scrollOffset;
		raycastTarget = false;
		color = Color.white;
		_E003 = axis;
		_E004 = parent;
		_E006 = isSubDivisions;
		_E005 = axisOrientation;
		if (_E009 == null)
		{
			_E009 = new _ED1E(forText: true);
			_E009.RecycleText = true;
		}
		_E009.Clear();
		if (_E006)
		{
			_E003._E007(_E00B, _E004, base.transform, _E009, _E005);
		}
		else
		{
			_E003._E005(_E00B, _E004, base.transform, _E009, _E005);
		}
		if (_E009.TextObjects != null)
		{
			foreach (BillboardText textObject in _E009.TextObjects)
			{
				((IInternalUse)parent).InternalTextController.AddText(textObject);
			}
		}
		base.canvasRenderer.materialCount = 1;
		if (_E007 != null)
		{
			_ED15.SafeDestroy(_E007);
		}
		float num = 1f;
		if (isSubDivisions)
		{
			if (axis.SubDivisions.Material != null)
			{
				_E007 = new Material(_E008 = axis.SubDivisions.Material);
				_E007.hideFlags = HideFlags.DontSave;
				material = _E007;
				num = _E001(axis.SubDivisions.MaterialTiling);
			}
		}
		else if (axis.MainDivisions.Material != null)
		{
			_E007 = new Material(_E008 = axis.MainDivisions.Material);
			_E007.hideFlags = HideFlags.DontSave;
			material = _E007;
			num = _E001(axis.MainDivisions.MaterialTiling);
		}
		_E00A = num;
		if (_E007 != null && _E007.HasProperty(_ED3E._E000(245686)))
		{
			_E007.SetFloat(_E00C, num);
		}
		SetAllDirty();
		Rebuild(CanvasUpdate.PreRender);
		_E009.DestoryRecycled();
	}

	protected virtual void Update()
	{
		if (_E008 != null && _E007 != null && _E007.HasProperty(_ED3E._E000(245686)))
		{
			if (_E007 != _E008)
			{
				_E007.CopyPropertiesFromMaterial(_E008);
			}
			_E007.SetFloat(_E00C, _E00A);
		}
	}

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public Object This()
	{
		return this;
	}
}
