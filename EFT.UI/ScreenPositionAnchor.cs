using System;
using UnityEngine;

namespace EFT.UI;

public sealed class ScreenPositionAnchor : MonoBehaviour
{
	private enum EAnchorType
	{
		RelativeByHeight,
		RelativeByWidth,
		Absolute
	}

	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private EAnchorType _type;

	[SerializeField]
	private TextAnchor _alignment;

	[SerializeField]
	private Vector2 _position;

	private int _E000;

	private int _E001;

	public void OnEnable()
	{
		_E001 = 0;
		_E000 = 0;
	}

	public void SetCamera(Camera newCam)
	{
		_camera = newCam;
	}

	public void Update()
	{
		if (!(_camera == null) && (Screen.width != _E001 || Screen.height != _E000))
		{
			_E001 = Screen.width;
			_E000 = Screen.height;
			_camera.gameObject.SetActive(value: true);
			_camera.enabled = true;
			Vector2 vector = new Vector2(Screen.width, Screen.height);
			Vector2 vector2 = _type switch
			{
				EAnchorType.Absolute => _position, 
				EAnchorType.RelativeByWidth => _position * vector.x, 
				EAnchorType.RelativeByHeight => _position * vector.y, 
				_ => throw new ArgumentOutOfRangeException(), 
			};
			switch (_alignment)
			{
			case TextAnchor.UpperCenter:
			case TextAnchor.MiddleCenter:
			case TextAnchor.LowerCenter:
				vector2.x += vector.x / 2f;
				break;
			case TextAnchor.UpperRight:
			case TextAnchor.MiddleRight:
			case TextAnchor.LowerRight:
				vector2.x += vector.x;
				break;
			}
			switch (_alignment)
			{
			case TextAnchor.UpperLeft:
			case TextAnchor.UpperCenter:
			case TextAnchor.UpperRight:
				vector2.y += vector.y;
				break;
			case TextAnchor.MiddleLeft:
			case TextAnchor.MiddleCenter:
			case TextAnchor.MiddleRight:
				vector2.y += vector.y / 2f;
				break;
			}
			Vector3 position = base.transform.position;
			float z = _camera.transform.InverseTransformPoint(position).z;
			Vector3 position2 = _camera.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y, z));
			position2.z = position.z;
			base.transform.position = position2;
			_camera.gameObject.SetActive(value: false);
		}
	}
}
