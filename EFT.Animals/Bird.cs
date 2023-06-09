using UnityEngine;

namespace EFT.Animals;

public class Bird : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	public Transform Carina;

	[SerializeField]
	private float _shaftAngleSmooth = 0.1f;

	[SerializeField]
	private float _directionChangeSensitivity = 360f;

	[SerializeField]
	private float _switchToSoarPoint = -0.05f;

	[SerializeField]
	private float _speedToFlap = 0.1f;

	[SerializeField]
	private float _directionToFlap = 1f;

	private Vector3 _E000;

	private float _E001;

	private float _E002;

	private static readonly int _E003 = Animator.StringToHash(_ED3E._E000(242359));

	private static readonly int _E004 = Animator.StringToHash(_ED3E._E000(242350));

	private static readonly int _E005 = Animator.StringToHash(_ED3E._E000(242347));

	public void SetDirection(Vector3 direction, float commonSpeedMult)
	{
		if (commonSpeedMult != 0f && !(direction == Vector3.zero))
		{
			Carina.forward = direction;
			float target = (_E000.x - direction.x) * _directionChangeSensitivity;
			_E001 = Mathf.SmoothDampAngle(_E001, target, ref _E002, _shaftAngleSmooth);
			Carina.Rotate(direction, _E001);
			float num = 1f;
			if (direction.y > _switchToSoarPoint)
			{
				_animator.SetTrigger(_E003);
				num = 1f + Mathf.InverseLerp(_switchToSoarPoint, 1f, direction.y) * _directionToFlap;
			}
			else
			{
				_animator.SetTrigger(_E004);
				num = 1f + Mathf.InverseLerp(_switchToSoarPoint, -1f, direction.y) * _directionToFlap;
			}
			_animator.SetFloat(_E005, num * commonSpeedMult * _speedToFlap);
			_E000 = direction;
		}
	}
}
