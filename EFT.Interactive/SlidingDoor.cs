using UnityEngine;

namespace EFT.Interactive;

public class SlidingDoor : Door
{
	public AnimationCurve CustomProgressCurve;

	public Vector3 ShutPosition;

	public Vector3 OpenPosition;

	public override float CurrentAngle
	{
		get
		{
			return base.CurrentAngle;
		}
		protected set
		{
			_E000(value);
		}
	}

	public override AnimationCurve ProgressCurve => CustomProgressCurve;

	private void _E000(float value)
	{
		base.transform.localPosition = Vector3.Lerp(ShutPosition, OpenPosition, value / OpenAngle);
	}
}
