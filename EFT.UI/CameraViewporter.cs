using System;
using EFT.UI.WeaponModding;
using UnityEngine;

namespace EFT.UI;

[ExecuteInEditMode]
public sealed class CameraViewporter : MonoBehaviour
{
	[SerializeField]
	private WeaponPreview _weaponPreview;

	private Canvas _E000;

	private RectTransform _E001;

	public Camera TargetCamera => _weaponPreview.WeaponPreviewCamera;

	private void OnEnable()
	{
		_E001 = base.transform as RectTransform;
		_E000 = GetComponentInParent<Canvas>();
		if (TargetCamera != null)
		{
			TargetCamera.enabled = true;
		}
	}

	private void Update()
	{
		if (!(_E001 == null) && !(_E000 == null) && !(TargetCamera == null))
		{
			switch (_E000.renderMode)
			{
			case RenderMode.ScreenSpaceOverlay:
			{
				Vector3 vector3 = base.transform.TransformPoint(_E001.rect.min);
				Vector3 vector4 = base.transform.TransformPoint(_E001.rect.max);
				Vector2 vector5 = new Vector2(vector3.x / (float)Screen.width, vector3.y / (float)Screen.height);
				Vector2 vector6 = new Vector2(vector4.x / (float)Screen.width, vector4.y / (float)Screen.height);
				TargetCamera.rect = new Rect(vector5.x, vector5.y, (vector6 - vector5).x, (vector6 - vector5).y);
				break;
			}
			case RenderMode.ScreenSpaceCamera:
			{
				Vector3 position = base.transform.TransformPoint(_E001.rect.min);
				Vector3 position2 = base.transform.TransformPoint(_E001.rect.max);
				Vector3 vector = _E000.worldCamera.WorldToViewportPoint(position);
				Vector3 vector2 = _E000.worldCamera.WorldToViewportPoint(position2);
				TargetCamera.rect = new Rect(vector.x, vector.y, (vector2 - vector).x, (vector2 - vector).y);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			case RenderMode.WorldSpace:
				break;
			}
		}
	}

	public Vector2 WorldToLocalScreenPosition(Vector3 worldPosition)
	{
		Vector3 vector = TargetCamera.WorldToViewportPoint(worldPosition);
		Vector2 size = _E001.rect.size;
		return new Vector2(size.x * vector.x, size.y * vector.y);
	}

	private void OnDisable()
	{
		if (TargetCamera != null)
		{
			TargetCamera.enabled = false;
		}
	}
}
