using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
	public static class _E000
	{
		public const float PRONE_POSE = 0f;

		public const float CROUCH_POSE = 0.5f;

		public const float STAND_POSE = 1f;

		public const float SPEED_MIN = 0f;

		public const float SPEED_MAX = 1f;

		public const string POSE_LEVEL_PARAM_NAME = "Level";

		public const string DIRECTION_X_PARAM_NAME = "Direct_X";

		public const string DIRECTION_Y_PARAM_NAME = "Direct_Y";

		public const string SPEED_PARAM_NAME = "Speed";
	}

	public Vector3 LocalVelocity;

	private RaycastHit m__E000;

	[SerializeField]
	private float _checkGroundedRayDistance = 1f;

	private Animator _E001;

	private Vector2 _E002;

	private float _E003;

	private float _E004 = 1f;

	private Rigidbody _E005;

	private float _E006;

	public bool IsGrounded => this.m__E000.collider != null;

	public Vector2 MovementDirection
	{
		get
		{
			return _E002;
		}
		set
		{
			_E002 = Vector2.ClampMagnitude(value, 1f);
			_E001.SetFloat(_ED3E._E000(63706), _E002.x);
			_E001.SetFloat(_ED3E._E000(63699), _E002.y);
		}
	}

	public float PoseLevel
	{
		get
		{
			return _E004;
		}
		set
		{
			_E004 = Mathf.Clamp(value, 0f, 1f);
			_E001.SetFloat(_ED3E._E000(63684), _E004);
		}
	}

	public float Speed
	{
		get
		{
			return _E003;
		}
		set
		{
			_E003 = Mathf.Clamp(value, 0f, 1f);
			_E001.SetFloat(_ED3E._E000(63682), _E003);
		}
	}

	protected virtual void Awake()
	{
		_E001 = GetComponent<Animator>();
		_E005 = GetComponent<Rigidbody>();
		PoseLevel = 1f;
		Speed = 0f;
	}

	public void CheckGrounded()
	{
		Physics.Raycast(new Ray(base.transform.position + Vector3.up * 0.5f, Vector3.down), out this.m__E000, _checkGroundedRayDistance);
	}

	public virtual void Move(Vector2 direction, float speed)
	{
		MovementDirection = direction;
		Speed = speed;
	}

	private void FixedUpdate()
	{
		Speed = 0f;
	}

	public void SetYaw(float yaw)
	{
		float y = base.transform.eulerAngles.y;
		float value = Mathf.DeltaAngle(base.transform.eulerAngles.y, yaw);
		_E001.SetFloat(_ED3E._E000(63736), value);
		float num = Mathf.DeltaAngle(y, base.transform.rotation.eulerAngles.y);
		_E006 = Mathf.Lerp(_E006, num / Time.deltaTime, Time.deltaTime);
		_E001.SetFloat(_ED3E._E000(63726), _E006);
	}

	protected Vector3 ConvertToWorldXZCoordintaes(Vector3 vector)
	{
		Vector3 forward = base.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		return Quaternion.LookRotation(forward) * vector;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(base.transform.position, base.transform.position + _E005.velocity);
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 10f, 200f, 40f), _E005.velocity.ToString());
		GUI.Label(new Rect(10f, 50f, 200f, 40f), (this.m__E000.collider != null) ? this.m__E000.collider.ToString() : _ED3E._E000(63714));
	}
}
