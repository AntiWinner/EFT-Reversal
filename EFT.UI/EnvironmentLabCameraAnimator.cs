using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Sirenix.Utilities;
using UnityEngine;

namespace EFT.UI;

public sealed class EnvironmentLabCameraAnimator : MonoBehaviour
{
	[Serializable]
	public struct RangeFloat
	{
		public float MinValue;

		public float MaxValue;

		public RangeFloat(float minValue, float maxValue)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

		public float GetRandomValue()
		{
			return UnityEngine.Random.Range(MinValue, MaxValue);
		}
	}

	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private Transform _horizontalJoint;

	[SerializeField]
	private Transform _verticalJoint;

	[SerializeField]
	private Transform _lens;

	[SerializeField]
	private Vector3 _steadyVerticalPosition;

	[SerializeField]
	private float _autoDegreesPerSecond = 7f;

	[SerializeField]
	private RangeFloat _autoRotationRate = new RangeFloat(10f, 13f);

	[SerializeField]
	private Vector3 _leftHorizontalPosition;

	[SerializeField]
	private Vector3 _rightHorizontalPosition;

	[SerializeField]
	private float _followDegreesPerSecond = 14f;

	[SerializeField]
	private float _followDelay = 0.3f;

	[SerializeField]
	private Vector3 _horizontalFollowDelta;

	[SerializeField]
	private Vector3 _verticalFollowDelta;

	[SerializeField]
	private float _lensSpeed = 0.1f;

	[SerializeField]
	private Vector3 _lensClosePosition;

	[SerializeField]
	private Vector3 _lensFarPosition;

	[SerializeField]
	private float _blinkInterval = 1f;

	[SerializeField]
	private GameObject _redLight;

	private Stopwatch m__E000;

	private float m__E001;

	private Vector3 m__E002;

	private Vector3 m__E003;

	private Vector3 _E004;

	private Vector3 _E005;

	private bool _E006;

	public void OnEnable()
	{
		this.m__E002 = _horizontalJoint.localRotation.eulerAngles;
		this.m__E000 = Stopwatch.StartNew();
	}

	public void Update()
	{
		_E003().HandleExceptions();
		_E002();
		_E001();
		_E000();
	}

	private void _E000()
	{
		_redLight.SetActive(Time.realtimeSinceStartup % (_blinkInterval * 2f) < _blinkInterval);
	}

	private void _E001()
	{
		float deltaTime = Time.deltaTime;
		float num = (_E006 ? _autoDegreesPerSecond : _followDegreesPerSecond);
		Vector3 euler = _horizontalJoint.localRotation.eulerAngles.DeltaAngle(this.m__E002 + this.m__E003);
		float num2 = euler.magnitude / num;
		if (num2.Positive())
		{
			float num3 = ((num2 < deltaTime) ? num2 : deltaTime);
			euler *= num3 / num2;
			_horizontalJoint.localRotation *= Quaternion.Euler(euler);
		}
		else
		{
			_E006 = false;
		}
		Vector3 euler2 = _verticalJoint.localRotation.eulerAngles.DeltaAngle(_steadyVerticalPosition + _E004);
		float num4 = euler2.magnitude / _followDegreesPerSecond;
		if (num4.Positive())
		{
			float num5 = ((num4 < deltaTime) ? num4 : deltaTime);
			euler2 *= num5 / num4;
			_verticalJoint.localRotation *= Quaternion.Euler(euler2);
		}
		Vector3 vector = _E005 - _lens.localPosition;
		float num6 = vector.magnitude / _lensSpeed;
		if (num6.Positive())
		{
			float num7 = ((num6 < deltaTime) ? num6 : deltaTime);
			vector *= num7 / num6;
			_lens.localPosition += vector;
		}
	}

	private void _E002()
	{
		if (!((float)this.m__E000.ElapsedMilliseconds < this.m__E001))
		{
			Vector3 vector = this.m__E002;
			_E006 = true;
			this.m__E002 = ((vector.DeltaAngle(_leftHorizontalPosition).magnitude > vector.DeltaAngle(_rightHorizontalPosition).magnitude) ? _leftHorizontalPosition : _rightHorizontalPosition);
			float num = this.m__E002.DeltaAngle(vector).magnitude / _autoDegreesPerSecond;
			this.m__E001 = (_autoRotationRate.GetRandomValue() + num) * 1000f;
			this.m__E000.Restart();
		}
	}

	private async Task _E003()
	{
		Vector3 mousePosition = Input.mousePosition;
		Vector2 vector = new Vector2(Screen.width, Screen.height);
		Vector2 vector2 = (mousePosition / vector).Clamp(Vector2.zero, Vector2.one);
		Vector2 vector3 = (vector2 - new Vector2(0.5f, 0.5f)) * new Vector2(2f, 2f);
		Vector2 vector4 = _camera.WorldToScreenPoint(_lens.position) / vector;
		if (vector2.x > vector4.x)
		{
			vector2.x = 1f - vector2.x;
			vector4.x = 1f - vector4.x;
		}
		if (vector2.y > vector4.y)
		{
			vector2.y = 1f - vector2.y;
			vector4.y = 1f - vector4.y;
		}
		float magnitude = ((vector4 - vector2) / vector4).magnitude;
		_E005 = Vector3.Lerp(_lensFarPosition, _lensClosePosition, magnitude);
		await Task.Delay((int)(_followDelay * 1000f));
		this.m__E003 = _horizontalFollowDelta * vector3.x;
		_E004 = _verticalFollowDelta * vector3.y;
	}
}
