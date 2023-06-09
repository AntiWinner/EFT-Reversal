using UnityEngine;

namespace GPUInstancer;

public class SpaceshipCameraRig : MonoBehaviour
{
	public Transform m_Target;

	public float m_MoveSpeed = 3f;

	public float m_TurnSpeed = 1f;

	public float m_RollSpeed = 0.2f;

	public bool m_FollowVelocity;

	public bool m_FollowTilt = true;

	public float m_SpinTurnLimit = 90f;

	public float m_TargetVelocityLowerLimit = 4f;

	public float m_SmoothTurnTime = 0.2f;

	private Vector3 m__E000;

	private Rigidbody _E001;

	private float _E002;

	private float _E003;

	private float _E004;

	private Vector3 _E005 = Vector3.up;

	private void Start()
	{
		if (!(m_Target == null))
		{
			_E001 = m_Target.GetComponent<Rigidbody>();
		}
	}

	private void FixedUpdate()
	{
		_E000(Time.deltaTime);
	}

	private void _E000(float deltaTime)
	{
		if (!(deltaTime > 0f) || m_Target == null)
		{
			return;
		}
		Vector3 forward = m_Target.forward;
		Vector3 up = m_Target.up;
		if (m_FollowVelocity && Application.isPlaying)
		{
			if (_E001.velocity.magnitude > m_TargetVelocityLowerLimit)
			{
				forward = _E001.velocity.normalized;
				up = Vector3.up;
			}
			else
			{
				up = Vector3.up;
			}
			_E003 = Mathf.SmoothDamp(_E003, 1f, ref _E004, m_SmoothTurnTime);
		}
		else
		{
			float num = Mathf.Atan2(forward.x, forward.z) * 57.29578f;
			if (m_SpinTurnLimit > 0f)
			{
				float value = Mathf.Abs(Mathf.DeltaAngle(_E002, num)) / deltaTime;
				float num2 = Mathf.InverseLerp(m_SpinTurnLimit, m_SpinTurnLimit * 0.75f, value);
				float smoothTime = ((_E003 > num2) ? 0.1f : 1f);
				if (Application.isPlaying)
				{
					_E003 = Mathf.SmoothDamp(_E003, num2, ref _E004, smoothTime);
				}
				else
				{
					_E003 = num2;
				}
			}
			else
			{
				_E003 = 1f;
			}
			_E002 = num;
		}
		base.transform.position = Vector3.Lerp(base.transform.position, m_Target.position, deltaTime * m_MoveSpeed);
		if (!m_FollowTilt)
		{
			forward.y = 0f;
			if (forward.sqrMagnitude < float.Epsilon)
			{
				forward = base.transform.forward;
			}
		}
		Quaternion b = Quaternion.LookRotation(forward, _E005);
		_E005 = ((m_RollSpeed > 0f) ? Vector3.Slerp(_E005, up, m_RollSpeed * deltaTime) : Vector3.up);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, m_TurnSpeed * _E003 * deltaTime);
	}
}
