using System.Runtime.CompilerServices;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace EFT.Hideout;

public sealed class HideoutCamera : MonoBehaviour
{
	private const int m__E000 = 10;

	private const float m__E001 = 0.5f;

	private const float m__E002 = 10f;

	[SerializeField]
	private float _delta = 200f;

	[SerializeField]
	private float _normalizedTime;

	[SerializeField]
	private Animation _animation;

	[SerializeField]
	private CinemachineVirtualCamera _virtualCamera;

	[SerializeField]
	private float _animationSpeed;

	public static bool NegativeDirection;

	private float _E003;

	private float _E004;

	private float _E005;

	private float _E006;

	private string _E007 => _animation.clip.name;

	public Transform Transform => _virtualCamera.transform;

	private float _E008
	{
		get
		{
			return _virtualCamera.m_Lens.FieldOfView;
		}
		set
		{
			LensSettings lens = _virtualCamera.m_Lens;
			lens.FieldOfView = value;
			_virtualCamera.m_Lens = lens;
		}
	}

	private void Awake()
	{
		_E005 = _virtualCamera.m_Lens.FieldOfView;
		_E006 = _E005;
		_E003 = _normalizedTime;
	}

	public void Zoom(float? zoom)
	{
		if (!zoom.HasValue)
		{
			zoom = _E005;
		}
		_E006 = Mathf.Clamp(_E006 + zoom.Value, 10f, _E005);
		DOTween.To(() => _E008, delegate(float x)
		{
			_E008 = x;
		}, _E006, 0.5f).SetEase(Ease.Linear);
	}

	public void Show()
	{
		_E004 = Mathf.Abs(_animationSpeed);
		_animation[_E007].speed = 0f;
		_animation.Play(_E007);
	}

	public void MoveAside(EMovementDirection direction)
	{
		float num = _animationSpeed * Time.deltaTime;
		switch (direction)
		{
		case EMovementDirection.Left:
			if (NegativeDirection)
			{
				_normalizedTime += num;
			}
			else
			{
				_normalizedTime -= num;
			}
			break;
		case EMovementDirection.Right:
			if (NegativeDirection)
			{
				_normalizedTime -= num;
			}
			else
			{
				_normalizedTime += num;
			}
			break;
		default:
			return;
		}
		_E000(_normalizedTime);
	}

	public void CameraUpdate()
	{
		float x = Input.mousePosition.x;
		int width = Screen.width;
		float num = (float)width - _delta;
		if (x < 0f || x > (float)width)
		{
			return;
		}
		if (x >= num)
		{
			float num2 = (x - num) * (_animationSpeed / _delta);
			if (NegativeDirection)
			{
				num2 *= -1f;
			}
			_normalizedTime += Mathf.Clamp(num2 * Time.deltaTime, 0f - _E004, _E004);
		}
		else if (x <= _delta)
		{
			float num3 = (_delta - x) * (_animationSpeed / _delta);
			if (NegativeDirection)
			{
				num3 *= -1f;
			}
			_normalizedTime -= Mathf.Clamp(num3 * Time.deltaTime, 0f - _E004, _E004);
		}
		_E000(_normalizedTime);
	}

	public void SetAnimationTimePercent(int percent)
	{
		_E000(Mathf.Abs(_E003 - (float)percent / 100f));
	}

	private void _E000(float normalizedTime)
	{
		_normalizedTime = Mathf.Clamp01(normalizedTime);
		_animation[_E007].normalizedTime = _normalizedTime;
	}

	public void SetCameraActive(bool value)
	{
		_virtualCamera.Priority = (value ? 10 : 0);
	}

	[CompilerGenerated]
	private float _E001()
	{
		return _E008;
	}

	[CompilerGenerated]
	private void _E002(float x)
	{
		_E008 = x;
	}
}
