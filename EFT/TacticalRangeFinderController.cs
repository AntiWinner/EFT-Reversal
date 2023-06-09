using System;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

namespace EFT;

public class TacticalRangeFinderController : MonoBehaviour
{
	private enum DistanceOutputFormat
	{
		FourDigits,
		ThreeDigitsAndDot
	}

	[SerializeField]
	private DistanceOutputFormat _distanceOutputFormat;

	[SerializeField]
	private string _noDistanceText = _ED3E._E000(138998);

	[SerializeField]
	private float _delayInSecsBetweenCasts = 0.5f;

	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private TMP_Text _textOnDisplay;

	[SerializeField]
	private Transform _boneToCastRay;

	[SerializeField]
	private float _rayStartOffset = 0.1f;

	[SerializeField]
	private float _maxCastDistance = 2500f;

	[SerializeField]
	private float _cullDistance = 1f;

	[SerializeField]
	private LayerMask _mask;

	private float m__E000;

	private readonly CustomSampler _E001 = CustomSampler.Create(_ED3E._E000(138995));

	private void OnEnable()
	{
		this.m__E000 = 0f;
		if (_canvas != null)
		{
			_canvas.worldCamera = _E8A8.Instance.OpticCameraManager.Camera;
			_canvas.planeDistance = 2f;
		}
		_E000();
	}

	private void OnDisable()
	{
		if (_canvas != null)
		{
			_canvas.worldCamera = null;
		}
	}

	private void Update()
	{
		this.m__E000 += Time.deltaTime;
		if (!(this.m__E000 < _delayInSecsBetweenCasts))
		{
			this.m__E000 = 0f;
			bool flag = _E8A8.Instance.Distance(base.transform.parent.position) < _cullDistance;
			if (_canvas != null)
			{
				_canvas.enabled = flag;
			}
			if (flag)
			{
				_E000();
			}
		}
	}

	private void _E000()
	{
		Vector3 position = _boneToCastRay.position;
		Vector3 forward = _boneToCastRay.forward;
		if (Physics.Raycast(position + forward * _rayStartOffset, forward, out var hitInfo, _maxCastDistance, _mask))
		{
			switch (_distanceOutputFormat)
			{
			case DistanceOutputFormat.FourDigits:
			{
				int num2 = Mathf.RoundToInt(hitInfo.distance);
				_textOnDisplay.SetMonospaceText(num2.ToString(_ED3E._E000(139005)));
				break;
			}
			case DistanceOutputFormat.ThreeDigitsAndDot:
			{
				float num = Mathf.Clamp(hitInfo.distance, 0f, 999f);
				_textOnDisplay.SetMonospaceText(num.ToString(_ED3E._E000(139000)));
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		else
		{
			_textOnDisplay.SetMonospaceText(_noDistanceText);
		}
	}
}
