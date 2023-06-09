using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph;

[ExecuteInEditMode]
public class TextDirection : MonoBehaviour
{
	public Material PointMaterial;

	public Material LineMaterial;

	public float Length = 20f;

	public float Gap = 5f;

	public float Thickness = 2f;

	public float PointSize = 10f;

	public CanvasLines Lines;

	public CanvasLines Point;

	public Text Text;

	private Transform m__E000;

	private Transform _E001;

	private TextController _E002;

	public void SetTextController(TextController control)
	{
		_E002 = control;
	}

	public void SetRelativeTo(Transform from, Transform to)
	{
		this.m__E000 = to;
		_E001 = from;
	}

	public void LateUpdate()
	{
		if (!(_E001 == null) && !(this.m__E000 == null) && !(_E002 == null) && !(_E002.Camera == null))
		{
			Vector3 vector = (this.m__E000.position - _E001.position).normalized * Length;
			vector = Quaternion.Inverse(_E002.Camera.transform.rotation) * vector;
			_E000(vector);
		}
	}

	public void SetDirection(float angle)
	{
		_E000(_ED15._E00A(angle, Length));
	}

	private void _E000(Vector3 dir)
	{
		float num = Mathf.Sign(dir.x);
		Vector3 vector = new Vector3(1f, 0f, 0f) * num * Length;
		Vector3 vector2 = new Vector3(1f, 0f, 0f) * num * Gap;
		if (LineMaterial != null)
		{
			List<CanvasLines._E001> list = new List<CanvasLines._E001>();
			list.Add(new CanvasLines._E001(new Vector3[3]
			{
				Vector3.zero,
				dir,
				dir + vector
			}));
			List<CanvasLines._E001> lines = list;
			Lines.Thickness = Thickness;
			Lines.Tiling = 1f;
			Lines.material = LineMaterial;
			Lines._E002(lines);
		}
		if (PointMaterial != null)
		{
			List<CanvasLines._E001> list = new List<CanvasLines._E001>();
			list.Add(new CanvasLines._E001(new Vector3[1] { Vector3.zero }));
			List<CanvasLines._E001> lines2 = list;
			Point.MakePointRender(PointSize);
			Point.material = PointMaterial;
			Point._E002(lines2);
		}
		Vector2 vector3 = new Vector2(0.5f, 0.5f);
		Vector2 pivot = new Vector2((num > 0f) ? 0f : 1f, 0.5f);
		Text.rectTransform.anchorMin = vector3;
		Text.rectTransform.anchorMax = vector3;
		Text.rectTransform.pivot = pivot;
		Text.alignment = ((num > 0f) ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight);
		Text.rectTransform.anchoredPosition = dir + vector + vector2;
	}
}
