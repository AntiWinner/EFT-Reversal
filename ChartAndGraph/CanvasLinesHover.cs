using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph;

public class CanvasLinesHover : MaskableGraphic
{
	private float m__E000;

	private UIVertex[] _E001 = new UIVertex[4];

	public void Init(float thickness)
	{
		this.m__E000 = thickness * 0.5f;
		SetAllDirty();
		Rebuild(CanvasUpdate.PreRender);
	}

	private IEnumerable<UIVertex> _E000()
	{
		UIVertex uIVertex = default(UIVertex);
		uIVertex.position = new Vector3(0f - this.m__E000, 0f - this.m__E000);
		uIVertex.uv0 = new Vector2(0f, 0f);
		yield return uIVertex;
		uIVertex.position = new Vector3(0f - this.m__E000, this.m__E000);
		uIVertex.uv0 = new Vector2(0f, 1f);
		yield return uIVertex;
		uIVertex.position = new Vector3(this.m__E000, this.m__E000);
		uIVertex.uv0 = new Vector2(1f, 1f);
		yield return uIVertex;
		uIVertex.position = new Vector3(this.m__E000, 0f - this.m__E000);
		uIVertex.uv0 = new Vector2(1f, 0f);
		yield return uIVertex;
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		base.OnPopulateMesh(vh);
		vh.Clear();
		int num = 0;
		foreach (UIVertex item in _E000())
		{
			_E001[num++] = item;
			if (num == 4)
			{
				UIVertex uIVertex = _E001[2];
				_E001[2] = _E001[3];
				_E001[3] = uIVertex;
				num = 0;
				vh.AddUIVertexQuad(_E001);
			}
		}
	}

	protected override void OnPopulateMesh(Mesh m)
	{
		_ED21 obj = new _ED21(1);
		int num = 0;
		foreach (UIVertex item in _E000())
		{
			_E001[num++] = item;
			if (num == 4)
			{
				num = 0;
				obj.AddQuad(_E001[0], _E001[1], _E001[2], _E001[3]);
			}
		}
		obj.ApplyToMesh(m);
	}
}
