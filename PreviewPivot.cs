using System;
using EFT.UI.WeaponModding;
using UnityEngine;

public class PreviewPivot : MonoBehaviour
{
	[Serializable]
	public class IconSettings
	{
		public Vector3 position;

		public bool hasOffset;

		public Quaternion rotation;

		public float boundsScale = 1f;

		public float perspective = 15f;

		public bool orthographic;

		public float orthographicSize = 10f;

		public void Apply(IconSettings newSettings)
		{
			position = newSettings.position;
			hasOffset = newSettings.hasOffset;
			rotation = newSettings.rotation;
			boundsScale = newSettings.boundsScale;
			perspective = newSettings.perspective;
			orthographic = newSettings.orthographic;
			orthographicSize = newSettings.orthographicSize;
		}
	}

	public Vector3 pivotPosition = Vector3.zero;

	public Quaternion pivotRotation = Quaternion.identity;

	public Vector3 scale = Vector3.one;

	public Vector3 SpawnPosition = Vector3.zero;

	public IconSettings Icon = new IconSettings();

	public void AutoAdjustPivot()
	{
		pivotPosition = WeaponPreview.GetBounds(base.gameObject).center;
	}
}
