using UnityEngine;

public class CurveRotator : MonoBehaviour
{
	public Transform RotatedTransform;

	public float RotationSpeed;

	public Quaternion OffRotation;

	public Quaternion OnRotation;

	public AnimationCurve AnimationCurve;

	private float m__E000;

	private bool _E001;

	private float _E002;

	public void Set(bool isOn, bool initial)
	{
		if (initial)
		{
			this.m__E000 = (isOn ? 1f : 0f);
			RotatedTransform.localRotation = (isOn ? OnRotation : OffRotation);
			_E001 = false;
			return;
		}
		_E002 = (isOn ? 1f : 0f);
		if (!_E001 && _E000())
		{
			_E001 = true;
		}
	}

	private bool _E000()
	{
		return !Mathf.Approximately(this.m__E000, _E002);
	}

	private void Update()
	{
		if (_E001)
		{
			if (_E000())
			{
				this.m__E000 = Mathf.MoveTowards(this.m__E000, _E002, Time.deltaTime * RotationSpeed);
				RotatedTransform.localRotation = Quaternion.Lerp(OffRotation, OnRotation, AnimationCurve.Evaluate(this.m__E000));
			}
			else
			{
				_E001 = false;
			}
		}
	}
}
