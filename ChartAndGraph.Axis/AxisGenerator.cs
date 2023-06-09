using System;
using UnityEngine;

namespace ChartAndGraph.Axis;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
internal class AxisGenerator : MonoBehaviour, _ED2E
{
	private MeshRenderer m__E000;

	private MeshFilter _E001;

	private Mesh _E002;

	private AxisBase _E003;

	private Material _E004;

	private Material _E005;

	private float _E006 = 1f;

	private _ED21 _E007;

	private Mesh _E008;

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(245686));

	private void OnDestroy()
	{
		_ED15._E00D(null, ref _E002);
		_ED15.SafeDestroy(_E004);
	}

	private float _E000(AnyChart parent, ChartOrientation orientation, ChartDivisionInfo inf)
	{
		MaterialTiling materialTiling = inf.MaterialTiling;
		if (!materialTiling.EnableTiling || materialTiling.TileFactor <= 0f)
		{
			return 1f;
		}
		float num = Math.Abs(_ED15._E006(parent, orientation, inf));
		float num2 = _ED15._E004(parent, orientation);
		float num3 = _ED15._E005(parent, orientation, inf);
		if (!inf.MarkBackLength.Automatic)
		{
			num2 = inf.MarkBackLength.Value;
		}
		if (num2 != 0f && num3 > 0f)
		{
			num += Math.Abs(num2) + Math.Abs(num3);
		}
		return num / materialTiling.TileFactor;
	}

	public void SetAxis(double scrollOffset, AnyChart parent, AxisBase axis, ChartOrientation axisOrientation, bool isSubDivisions)
	{
		_E003 = axis;
		if (_E007 == null)
		{
			_E007 = new _ED21(2);
			_E007.RecycleText = true;
		}
		_E007.Clear();
		_E007.Orientation = axisOrientation;
		_E003 = axis;
		if (isSubDivisions)
		{
			axis._E005(scrollOffset, parent, base.transform, _E007, axisOrientation);
		}
		else
		{
			axis._E007(scrollOffset, parent, base.transform, _E007, axisOrientation);
		}
		if (_E007.TextObjects != null)
		{
			foreach (BillboardText textObject in _E007.TextObjects)
			{
				((IInternalUse)parent).InternalTextController.AddText(textObject);
			}
		}
		Mesh mesh = (_E008 = _E007.Generate(_E008));
		mesh.hideFlags = HideFlags.DontSave;
		if (_E001 == null)
		{
			_E001 = GetComponent<MeshFilter>();
		}
		_E001.sharedMesh = mesh;
		MeshCollider component = GetComponent<MeshCollider>();
		if (component != null)
		{
			component.sharedMesh = mesh;
		}
		_ED15._E00D(mesh, ref _E002);
		MeshRenderer component2 = GetComponent<MeshRenderer>();
		if (component2 != null)
		{
			Material material = _E003.MainDivisions.Material;
			float num = _E000(parent, axisOrientation, _E003.MainDivisions);
			if (isSubDivisions)
			{
				material = _E003.SubDivisions.Material;
				num = _E000(parent, axisOrientation, _E003.SubDivisions);
			}
			_E005 = material;
			if (material != null)
			{
				_ED15.SafeDestroy(_E004);
				_E004 = new Material(material);
				_E004.hideFlags = HideFlags.DontSave;
				component2.sharedMaterial = _E004;
				_E006 = num;
				if (_E004.HasProperty(_ED3E._E000(245686)))
				{
					_E004.SetFloat(_E009, _E006);
				}
			}
		}
		_E007.DestoryRecycled();
	}

	protected virtual void Update()
	{
		if (_E005 != null && _E004 != null && _E004.HasProperty(_ED3E._E000(245686)))
		{
			if (_E004 != _E005)
			{
				_E004.CopyPropertiesFromMaterial(_E005);
			}
			_E004.SetFloat(_E009, _E006);
		}
	}

	public UnityEngine.Object This()
	{
		return this;
	}

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}
}
