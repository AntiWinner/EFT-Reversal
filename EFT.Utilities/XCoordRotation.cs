using UnityEngine;

namespace EFT.Utilities;

public class XCoordRotation : MonoBehaviour
{
	[SerializeField]
	private float _rotationSpeed = 0.3f;

	private Transform _rotator;

	public float CurrentYaw { get; private set; }

	public void Init(Transform model)
	{
		_rotator = model;
		CurrentYaw = model.eulerAngles.y;
	}

	public void Rotate(float angle)
	{
		float rotation = CurrentYaw - angle * _rotationSpeed;
		SetRotation(rotation);
	}

	public void SetRotation(float yaw)
	{
		if (!(_rotator == null))
		{
			CurrentYaw = _E000(yaw);
			_rotator.localRotation = Quaternion.Euler(0f, CurrentYaw, 0f);
		}
	}

	private float _E000(float value)
	{
		if (value >= 360f)
		{
			value -= 360f;
		}
		else if (value < 0f)
		{
			value += 360f;
		}
		return value;
	}
}
