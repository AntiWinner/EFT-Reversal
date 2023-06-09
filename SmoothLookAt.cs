using System.Collections;
using UnityEngine;

public class SmoothLookAt : MonoBehaviour
{
	[SerializeField]
	private float _lerpToTargetRotationSpeed;

	[SerializeField]
	private float _angleDifForStartRotation;

	[SerializeField]
	private float _angleDifForStopRotation = 1f;

	[SerializeField]
	private bool _returnStartRotationOnEnd = true;

	[SerializeField]
	private float _returnToStartRotationTime = 1f;

	private Transform m__E000;

	private Transform m__E001;

	private Vector3 m__E002;

	private Vector3 _E003;

	private Coroutine _E004;

	private Coroutine _E005;

	private Quaternion _E006;

	private void OnDestroy()
	{
		_E000();
	}

	private void _E000()
	{
		ToggleWorking(enable: false);
		if (_E005 != null)
		{
			StopCoroutine(_E005);
			_E005 = null;
		}
	}

	public void SetTransformForRotation(Transform forRotation)
	{
		this.m__E000 = forRotation;
	}

	public void SetTransformToLookAt(Transform toLookAt)
	{
		this.m__E001 = toLookAt;
	}

	public void ToggleWorking(bool enable)
	{
		if (!enable)
		{
			if (_E004 != null)
			{
				StopCoroutine(_E004);
				_E004 = null;
				if (_returnStartRotationOnEnd && this.m__E000 != null)
				{
					_E005 = StartCoroutine(_E001());
				}
			}
		}
		else if (this.m__E001 != null && this.m__E000 != null && _E004 == null)
		{
			_E006 = this.m__E000.localRotation;
			this.m__E002 = this.m__E000.position + this.m__E000.forward - this.m__E000.position;
			_E003 = this.m__E002;
			_E004 = StartCoroutine(_E002());
		}
	}

	private IEnumerator _E001()
	{
		Quaternion localRotation = this.m__E000.localRotation;
		float num = 0f;
		while (num <= 1f)
		{
			this.m__E000.localRotation = Quaternion.Lerp(localRotation, _E006, num);
			num += Time.deltaTime / _returnToStartRotationTime;
			yield return null;
		}
		_E005 = null;
	}

	public float GetTimeToLookAt()
	{
		Vector3 to = this.m__E001.position - this.m__E000.position;
		return Vector3.Angle(_E003, to) / _lerpToTargetRotationSpeed;
	}

	private IEnumerator _E002()
	{
		bool flag = false;
		while (!(this.m__E001 == null) && !(this.m__E000 == null))
		{
			this.m__E002 = this.m__E001.position - this.m__E000.position;
			Quaternion b = Quaternion.LookRotation(this.m__E002);
			float num = Quaternion.Angle(this.m__E000.rotation, b);
			if (!flag && num > _angleDifForStartRotation)
			{
				_E003 = Vector3.Lerp(_E003, this.m__E002, Time.deltaTime * _lerpToTargetRotationSpeed);
				this.m__E000.rotation = Quaternion.LookRotation(_E003);
				flag = true;
			}
			else if (flag)
			{
				_E003 = Vector3.Lerp(_E003, this.m__E002, Time.deltaTime * _lerpToTargetRotationSpeed);
				this.m__E000.rotation = Quaternion.LookRotation(_E003);
				if (num <= _angleDifForStopRotation)
				{
					flag = false;
				}
			}
			yield return null;
		}
		_E000();
	}
}
