using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public enum InputAxisEnum
	{
		X,
		Y
	}

	public float MaximumY = 90f;

	public float MinimumY = -90f;

	public Vector3 Axis = Vector3.right;

	public Transform Root;

	public float SensitivityY = 5f;

	public InputAxisEnum InputAxis = InputAxisEnum.Y;

	private Quaternion _E000;

	public bool UseLocalUpdate;

	private float _E001;

	public float RotationY => _E001;

	private void Start()
	{
		_E000 = Root.transform.localRotation;
		_E001 = _E000.eulerAngles.y;
	}

	private void Update()
	{
		if (UseLocalUpdate)
		{
			OnUpdate();
		}
	}

	public void OnUpdate()
	{
		_E001 += Input.GetAxis(_ED3E._E000(46415) + InputAxis) * SensitivityY;
		_E001 = ClampAngle(_E001, MinimumY, MaximumY);
		Quaternion quaternion = Quaternion.AngleAxis(0f - _E001, Axis);
		Root.localRotation = _E000 * quaternion;
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
