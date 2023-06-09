using System.Collections;
using UnityEngine;

public class WheelDrive : MonoBehaviour
{
	public enum EGear
	{
		Back,
		Fwd
	}

	[SerializeField]
	private RigidbodySpawner _rigidSpawner;

	[SerializeField]
	private Transform Ballistics;

	[SerializeField]
	private RigidbodySpawner _bumperSpawner;

	[SerializeField]
	private Collider _chassisCollider;

	[SerializeField]
	private Collider _bumperCollider;

	public EGear Gear;

	public Vector3 SteerTarget;

	public Transform SteerTargetTransform;

	public float Torque;

	public Vector2 MinSpeedAtDistance;

	public Vector2 MaxpeedAtDistance;

	public float BrakeTorque = 400f;

	public float SteerSpeed = 1000f;

	public Vector2 SpeedByTurnAngel = new Vector2(1f, 1f);

	private float m__E000;

	private bool _E001;

	public EasySuspension ES;

	private Rigidbody _E002;

	private Rigidbody _E003;

	[Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
	public float criticalSpeed = 5f;

	[Tooltip("Simulation sub-steps when the speed is above critical.")]
	public int stepsBelow = 5;

	[Tooltip("Simulation sub-steps when the speed is below critical.")]
	public int stepsAbove = 1;

	private WheelCollider[] _E004 = new WheelCollider[0];

	private Vector3 _E005;

	public WheelCollider[] frontWheels;

	public AudioSource AudioSource;

	public AudioClip OnStartSound;

	public Transform[] wheelGraphics;

	private void Start()
	{
		_E005 = Vector3.Lerp(frontWheels[0].transform.position, frontWheels[1].transform.position, 0.5f);
		_E005 = base.transform.InverseTransformPoint(_E005);
		_E004 = GetComponentsInChildren<WheelCollider>();
		wheelGraphics = new Transform[_E004.Length];
		for (int i = 0; i < _E004.Length; i++)
		{
			WheelCollider wheelCollider = _E004[i];
			wheelGraphics[i] = wheelCollider.transform.GetChild(0);
		}
		StartCoroutine(Sleep());
	}

	public IEnumerator Sleep()
	{
		_E002.drag = 3f;
		yield return new WaitForSeconds(3f);
		base.enabled = false;
	}

	private void OnEnable()
	{
		_E002 = _rigidSpawner.Create();
		_E002.drag = 0f;
		_E320._E002.SupportRigidbody(_E002);
		if (_bumperSpawner != null)
		{
			_E003 = _bumperSpawner.Create();
			_E320._E002.SupportRigidbody(_E003);
		}
		if (_chassisCollider != null && _bumperCollider != null)
		{
			Physics.IgnoreCollision(_chassisCollider, _bumperCollider);
		}
		WheelCollider[] array = _E004;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		if (ES != null)
		{
			ES.SetRigidbody(_E002);
			ES.enabled = true;
		}
	}

	private void OnDisable()
	{
		_rigidSpawner.Remove();
		_E002 = null;
		if (_bumperSpawner != null)
		{
			_bumperSpawner.Remove();
			_E003 = null;
		}
		WheelCollider[] array = _E004;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		if (ES != null)
		{
			ES.SetRigidbody(null);
			ES.enabled = false;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawSphere(SteerTarget, 0.5f);
	}

	public void Steer(Vector3 point)
	{
		SteerTarget = point;
		Vector3 lhs = new Vector3(_E002.velocity.x, 0f, _E002.velocity.z);
		Vector3 vector = SteerTarget - base.transform.TransformPoint(_E005);
		Debug.DrawRay(base.transform.TransformPoint(_E005), vector, Color.red, Time.deltaTime);
		Vector3 forward = base.transform.forward;
		float num = Vector3.Dot(forward, vector);
		Gear = ((num > 0f) ? EGear.Fwd : EGear.Back);
		float value = Vector3.Distance(base.transform.TransformPoint(_E005), point);
		float t = Mathf.InverseLerp(MinSpeedAtDistance.y, MaxpeedAtDistance.y, value);
		Torque = Mathf.Lerp(MinSpeedAtDistance.x, MaxpeedAtDistance.x, t);
		Torque *= SpeedByTurnAngel.Evaluate(Mathf.Abs(num));
		Vector3 a = ((Gear == EGear.Fwd) ? new Vector3(forward.x, 0f, forward.z) : new Vector3(0f - forward.x, 0f, 0f - forward.z));
		Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
		_E001 = lhs.magnitude > 1f && Vector3.Dot(lhs, vector2) < 0f;
		float num2 = Mathf.Clamp(_E000(a, vector2), -30f, 30f);
		if (Gear == EGear.Back)
		{
			num2 = 0f - num2;
			Torque = 0f - Torque;
		}
		this.m__E000 = Mathf.Lerp(this.m__E000, num2, SteerSpeed * Time.deltaTime);
		WheelCollider[] array = frontWheels;
		foreach (WheelCollider obj in array)
		{
			obj.steerAngle = this.m__E000;
			obj.motorTorque = Torque;
		}
		array = _E004;
		foreach (WheelCollider wheelCollider in array)
		{
			wheelCollider.brakeTorque = (_E001 ? BrakeTorque : 0f);
			if (_E001)
			{
				wheelCollider.motorTorque = 0f;
			}
		}
	}

	private float _E000(Vector3 a, Vector3 b)
	{
		return Vector3.Angle(a, b) * Mathf.Sign(Vector3.Cross(a, b).y);
	}

	private void Update()
	{
		_E004[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
		for (int i = 0; i < _E004.Length; i++)
		{
			_E004[i].GetWorldPose(out var pos, out var quat);
			Transform obj = wheelGraphics[i];
			obj.position = pos;
			obj.rotation = quat;
		}
		Ballistics.SetPositionAndRotation(base.transform.position, base.transform.rotation);
	}

	public void ManualBrake()
	{
		WheelCollider[] array = _E004;
		foreach (WheelCollider obj in array)
		{
			obj.brakeTorque = BrakeTorque;
			obj.motorTorque = 0f;
		}
	}
}
