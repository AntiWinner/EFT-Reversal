using System.Collections.Generic;
using UnityEngine;

public class ConvexHull : MonoBehaviour
{
	private static List<_E41A> m__E000;

	private static List<_E41A> _E001;

	private static List<_E41A> _E002;

	public static int[] ComputeIncremental(Vector3[] points)
	{
		if (points.Length < 3)
		{
			return null;
		}
		_E41A.Points = points;
		_E41A obj = new _E41A(0, 1, 2);
		Vector3 p = _E000(points, 3, obj);
		if (obj.IsVisible(p))
		{
			obj.Flip();
		}
		_E41A obj2 = new _E41A(3, obj.I0, obj.I1);
		if (obj2.IsVisible(p))
		{
			obj2.Flip();
		}
		_E41A obj3 = new _E41A(3, obj.I1, obj.I2);
		if (obj3.IsVisible(p))
		{
			obj3.Flip();
		}
		_E41A obj4 = new _E41A(3, obj.I2, obj.I0);
		if (obj4.IsVisible(p))
		{
			obj4.Flip();
		}
		ConvexHull.m__E000 = new List<_E41A> { obj, obj2, obj3, obj4 };
		_E001 = new List<_E41A>(points.Length);
		_E002 = new List<_E41A>(points.Length);
		for (int i = 4; i < points.Length; i++)
		{
			Vector3 p2 = points[i];
			_E001.Clear();
			for (int j = 0; j < ConvexHull.m__E000.Count; j++)
			{
				if (ConvexHull.m__E000[j].IsVisible(p2))
				{
					_E001.Add(ConvexHull.m__E000[j]);
				}
			}
			if (_E001.Count == 0)
			{
				continue;
			}
			for (int k = 0; k < _E001.Count; k++)
			{
				ConvexHull.m__E000.Remove(_E001[k]);
			}
			if (_E001.Count == 1)
			{
				_E41A obj5 = _E001[0];
				ConvexHull.m__E000.Add(new _E41A(i, obj5.I0, obj5.I1));
				ConvexHull.m__E000.Add(new _E41A(i, obj5.I1, obj5.I2));
				ConvexHull.m__E000.Add(new _E41A(i, obj5.I2, obj5.I0));
				continue;
			}
			if (_E001.Count > 2000)
			{
				Debug.LogWarning(_ED3E._E000(85332) + _E001.Count + _ED3E._E000(85375));
				return new int[0];
			}
			_E002.Clear();
			for (int l = 0; l < _E001.Count; l++)
			{
				_E002.Add(new _E41A(i, _E001[l].I0, _E001[l].I1));
				_E002.Add(new _E41A(i, _E001[l].I1, _E001[l].I2));
				_E002.Add(new _E41A(i, _E001[l].I2, _E001[l].I0));
			}
			if (_E002.Count > 8000)
			{
				Debug.LogWarning(_ED3E._E000(85350) + _E002.Count + _ED3E._E000(85375));
				return new int[0];
			}
			for (int m = 0; m < _E002.Count; m++)
			{
				_E41A obj6 = _E002[m];
				for (int n = 0; n < _E002.Count; n++)
				{
					if (obj6 != _E002[n] && obj6.IsVisible(_E002[n].Centroid))
					{
						obj6 = null;
						break;
					}
				}
				if (obj6 != null)
				{
					ConvexHull.m__E000.Add(obj6);
				}
			}
		}
		int[] array = new int[ConvexHull.m__E000.Count * 3];
		int num = 0;
		for (int num2 = 0; num2 < ConvexHull.m__E000.Count; num2++)
		{
			int[] array2 = array;
			int num3 = num;
			int num4 = 1;
			int num5 = num3 + num4;
			int i2 = ConvexHull.m__E000[num2].I0;
			array2[num3] = i2;
			int[] array3 = array;
			int num6 = num5;
			int num7 = 1;
			int num8 = num6 + num7;
			int i3 = ConvexHull.m__E000[num2].I1;
			array3[num6] = i3;
			int[] array4 = array;
			int num9 = num8;
			int num10 = 1;
			num = num9 + num10;
			int i4 = ConvexHull.m__E000[num2].I2;
			array4[num9] = i4;
		}
		return array;
	}

	private static Vector3 _E000(Vector3[] points, int index, _E41A face)
	{
		return (points[index] + points[face.I0] + points[face.I1] + points[face.I2]) / 4f;
	}
}
