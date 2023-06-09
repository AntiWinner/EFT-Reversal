using System;
using EFT.Animations;
using UnityEngine;

[Serializable]
public class MotionEffector : IEffector
{
	public Vector3 Motion;

	public Vector3 Velocity;

	public float RotationInputClamp = 300f;

	private Vector3 _lastPosition;

	private Vector3 _lastForward;

	public Vector3 PositionVelocity;

	public Vector2 RotationVelocity;

	public Vector3 PositionAcceleration;

	public Vector2 RotationAcceleration;

	public Vector3 SwayFactors = Vector3.one;

	private Vector3 _lastPositionVelocity;

	private Vector2 _lastRotationVelocity;

	private Vector2 _rotVelSum;

	private Vector2 _rotAccSum;

	private Vector3 lastRotation;

	public float Intensity = 0.45f;

	private Vector3 _platformMovement;

	private Vector2 v;

	private Vector2 v4;

	private _E3CA[] _mouseProcessors;

	private _E3C9[] _movementProcessors;

	public MotionEffectorParameters MouseParameters;

	public MotionEffectorParameters MovementParameters;

	private bool _initialized;

	private Vector3 v2;

	private Vector3 v3;

	private bool _needReset;

	public Transform Transform { get; set; }

	public void Initialize(PlayerSpring playerSpring)
	{
		Transform = playerSpring.TrackingTransform;
		_movementProcessors = _E3C9.CreateInstance(MovementParameters);
		_mouseProcessors = _E3CA.CreateInstance(MouseParameters);
		_E3CA[] mouseProcessors = _mouseProcessors;
		for (int i = 0; i < mouseProcessors.Length; i++)
		{
			mouseProcessors[i].Initialize(playerSpring.SwaySpring);
		}
		_E3C9[] movementProcessors = _movementProcessors;
		for (int i = 0; i < movementProcessors.Length; i++)
		{
			movementProcessors[i].Initialize(playerSpring.CameraRotation, playerSpring.HandsPosition, playerSpring.HandsRotation);
		}
	}

	public void AddPlatformMovement(Vector3 movement)
	{
		_platformMovement += movement;
	}

	public void FixedTracking(float deltaTime)
	{
		if (_needReset)
		{
			_E001();
			_needReset = false;
		}
		Vector3 motion = Motion;
		float num = Mathf.Abs(Velocity.y);
		motion.y = Mathf.Clamp(motion.y, 0f - num, num);
		PositionVelocity = Vector3.SmoothDamp(PositionVelocity, Transform.InverseTransformDirection(motion), ref v2, MovementParameters.VelSmooth);
		_lastPosition = Transform.position;
		PositionAcceleration = Vector3.SmoothDamp(PositionAcceleration, PositionVelocity - _lastPositionVelocity, ref v3, MovementParameters.AccSmooth);
		_lastPositionVelocity = PositionVelocity;
		Vector2 b = _E000() / deltaTime;
		RotationVelocity.x = Mathf.Clamp(RotationVelocity.x, 0f - RotationInputClamp, RotationInputClamp);
		RotationVelocity.y = Mathf.Clamp(RotationVelocity.y, 0f - RotationInputClamp, RotationInputClamp);
		_rotAccSum = Vector2.Lerp(_rotAccSum, RotationVelocity - _lastRotationVelocity, 1f / 3f);
		_lastRotationVelocity = RotationVelocity;
		_rotVelSum = Vector2.Lerp(_rotVelSum, b, 0.2f);
		RotationVelocity = Vector2.SmoothDamp(RotationVelocity, _rotVelSum, ref v4, MouseParameters.VelSmooth, float.MaxValue, deltaTime);
		RotationAcceleration = Vector2.SmoothDamp(RotationAcceleration, _rotAccSum, ref v, MouseParameters.AccSmooth, float.MaxValue, deltaTime);
	}

	private Vector2 _E000()
	{
		Vector3 vector = Transform.InverseTransformDirection(_lastForward);
		Vector2 result = default(Vector2);
		result.y = Mathf.Atan2(vector.z, vector.y) * 57.29578f - 90f;
		result.x = (0f - Mathf.Atan2(vector.x, vector.z)) * 57.29578f;
		_lastForward = Transform.forward;
		return result;
	}

	public void Process(float deltaTime)
	{
		_E3C9[] movementProcessors = _movementProcessors;
		for (int i = 0; i < movementProcessors.Length; i++)
		{
			movementProcessors[i].Process(this, deltaTime * Intensity);
		}
		_E3CA[] mouseProcessors = _mouseProcessors;
		for (int i = 0; i < mouseProcessors.Length; i++)
		{
			mouseProcessors[i].Process(this, deltaTime, SwayFactors);
		}
	}

	public string DebugOutput()
	{
		return string.Format(_ED3E._E000(46582), RotationVelocity, RotationAcceleration, PositionAcceleration * 100f, PositionVelocity * 100f);
	}

	public void Reset()
	{
		_needReset = true;
	}

	private void _E001()
	{
		_lastPosition = Transform.position;
		PositionVelocity = Transform.InverseTransformDirection(Vector3.zero);
		_lastPositionVelocity = Vector3.zero;
		PositionAcceleration = Vector3.zero;
		_lastForward = Transform.forward;
		_lastRotationVelocity = Vector2.zero;
		RotationAcceleration = Vector2.zero;
		v = Vector2.zero;
		v2 = Vector3.zero;
		v3 = Vector3.zero;
		_rotAccSum = Vector2.zero;
		_rotVelSum = Vector2.zero;
	}
}
