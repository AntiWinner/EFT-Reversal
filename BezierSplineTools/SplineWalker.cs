using UnityEngine;

namespace BezierSplineTools;

public class SplineWalker : MonoBehaviour
{
	public BezierSpline spline;

	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	private float _E000;

	private bool _E001 = true;

	private void Update()
	{
		if (_E001)
		{
			_E000 += Time.deltaTime / duration;
			if (_E000 > 1f)
			{
				if (mode == SplineWalkerMode.Once)
				{
					_E000 = 1f;
				}
				else if (mode == SplineWalkerMode.Loop)
				{
					_E000 -= 1f;
				}
				else
				{
					_E000 = 2f - _E000;
					_E001 = false;
				}
			}
		}
		else
		{
			_E000 -= Time.deltaTime / duration;
			if (_E000 < 0f)
			{
				_E000 = 0f - _E000;
				_E001 = true;
			}
		}
		Vector3 point = spline.GetPoint(_E000);
		base.transform.localPosition = point;
		if (lookForward)
		{
			base.transform.LookAt(point + spline.GetDirection(_E000));
		}
	}
}
