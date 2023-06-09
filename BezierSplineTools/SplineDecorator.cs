using UnityEngine;

namespace BezierSplineTools;

public class SplineDecorator : MonoBehaviour
{
	public BezierSpline spline;

	public int frequency;

	public bool lookForward;

	public Transform[] items;

	private void Awake()
	{
		if (frequency <= 0 || items == null || items.Length == 0)
		{
			return;
		}
		float num = frequency * items.Length;
		num = ((!spline.Loop && num != 1f) ? (1f / (num - 1f)) : (1f / num));
		int num2 = 0;
		for (int i = 0; i < frequency; i++)
		{
			int num3 = 0;
			while (num3 < items.Length)
			{
				Transform transform = Object.Instantiate(items[num3]);
				Vector3 point = spline.GetPoint((float)num2 * num);
				transform.transform.localPosition = point;
				if (lookForward)
				{
					transform.transform.LookAt(point + spline.GetDirection((float)num2 * num));
				}
				transform.transform.parent = base.transform;
				num3++;
				num2++;
			}
		}
	}
}
