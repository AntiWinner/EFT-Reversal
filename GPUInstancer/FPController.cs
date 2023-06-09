using UnityEngine;

namespace GPUInstancer;

[RequireComponent(typeof(CharacterController))]
public class FPController : MonoBehaviour
{
	[SerializeField]
	public float m_WalkSpeed;

	[SerializeField]
	public float m_RunSpeed;

	[SerializeField]
	public float m_JumpSpeed;

	private bool m__E000;

	private _E4BC m__E001;

	private Camera _E002;

	private bool _E003;

	private float _E004;

	private Vector2 _E005;

	private Vector3 _E006 = Vector3.zero;

	private CharacterController _E007;

	private CollisionFlags _E008;

	private bool _E009;

	private bool _E00A;

	private float _E00B = 10f;

	private float _E00C = 2f;

	private void Start()
	{
		_E007 = GetComponent<CharacterController>();
		_E002 = Camera.main;
		_E00A = false;
		this.m__E001 = new _E4BC();
		this.m__E001.Init(base.transform, _E002.transform);
	}

	private void Update()
	{
		_E001();
		if (!_E003 && Cursor.lockState == CursorLockMode.Locked)
		{
			_E003 = Input.GetButtonDown(_ED3E._E000(77053));
		}
		if (!_E009 && _E007.isGrounded)
		{
			_E006.y = 0f;
			_E00A = false;
		}
		if (!_E007.isGrounded && !_E00A && _E009)
		{
			_E006.y = 0f;
		}
		_E009 = _E007.isGrounded;
	}

	private void FixedUpdate()
	{
		_E000(out var speed);
		Vector3 vector = base.transform.forward * _E005.y + base.transform.right * _E005.x;
		Physics.SphereCast(base.transform.position, _E007.radius, Vector3.down, out var hitInfo, _E007.height / 2f, -1, QueryTriggerInteraction.Ignore);
		vector = Vector3.ProjectOnPlane(vector, hitInfo.normal).normalized;
		_E006.x = vector.x * speed;
		_E006.z = vector.z * speed;
		if (_E007.isGrounded)
		{
			_E006.y = 0f - _E00B;
			if (_E003)
			{
				_E006.y = m_JumpSpeed;
				_E003 = false;
				_E00A = true;
			}
		}
		else
		{
			_E006 += Physics.gravity * _E00C * Time.fixedDeltaTime;
		}
		_E008 = _E007.Move(_E006 * Time.fixedDeltaTime);
		this.m__E001.UpdateCursorLock();
	}

	private void _E000(out float speed)
	{
		float axis = Input.GetAxis(_ED3E._E000(23415));
		float axis2 = Input.GetAxis(_ED3E._E000(23422));
		this.m__E000 = !Input.GetKey(KeyCode.LeftShift);
		speed = (this.m__E000 ? m_WalkSpeed : m_RunSpeed);
		_E005 = new Vector2(axis, axis2);
		if (_E005.sqrMagnitude > 1f)
		{
			_E005.Normalize();
		}
	}

	private void _E001()
	{
		this.m__E001.LookRotation(base.transform, _E002.transform);
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
		if (_E008 != CollisionFlags.Below && !(attachedRigidbody == null) && !attachedRigidbody.isKinematic)
		{
			attachedRigidbody.AddForceAtPosition(_E007.velocity * 0.1f, hit.point, ForceMode.Impulse);
		}
	}
}
