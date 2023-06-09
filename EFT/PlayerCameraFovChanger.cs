using System.Collections;
using Comfort.Common;
using EFT.CameraControl;
using UnityEngine;

namespace EFT;

public class PlayerCameraFovChanger : MonoBehaviour
{
	[SerializeField]
	[Header("Use only two keys")]
	private AnimationCurve _changeFovCurve;

	[SerializeField]
	private float _changingTime;

	private float m__E000;

	private Camera _E001;

	private Coroutine _E002;

	private readonly float _E003 = 0.65f;

	public void ChangeFov(float targetFov, float changeTime)
	{
		_changingTime = Mathf.Clamp(changeTime, 0.3f, 3f);
		_changingTime *= _E003;
		ChangeFov(targetFov);
	}

	public void ChangeFov(float targetFov)
	{
		if (Singleton<PlayerCameraController>.Instantiated)
		{
			_E001 = Singleton<PlayerCameraController>.Instance.Camera;
			this.m__E000 = Singleton<PlayerCameraController>.Instance.Camera.fieldOfView;
			if (_E002 != null)
			{
				StopCoroutine(_E002);
			}
			_E002 = StartCoroutine(_E000(this.m__E000, targetFov));
		}
	}

	public void ReturnFov(float changeTime)
	{
		if (Singleton<PlayerCameraController>.Instantiated)
		{
			_changingTime = changeTime;
			if (_E002 != null)
			{
				StopCoroutine(_E002);
			}
			_E002 = StartCoroutine(_E000(_E001.fieldOfView, this.m__E000));
		}
	}

	private IEnumerator _E000(float from, float to)
	{
		float num = 0f;
		while (num <= 1f)
		{
			float t = _changeFovCurve.Evaluate(num);
			_E001.fieldOfView = Mathf.Lerp(from, to, t);
			num += Time.deltaTime / _changingTime;
			yield return null;
		}
	}

	private void OnDestroy()
	{
		if (_E002 != null)
		{
			StopCoroutine(_E002);
			_E002 = null;
		}
	}
}
