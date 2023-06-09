using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public sealed class PixelPerfectSpriteScaler : UIBehaviour
{
	private Image m__E000;

	private RectTransform m__E001;

	private Vector2 _E002;

	private Vector2 _E003;

	protected override void Awake()
	{
		base.Awake();
		this.m__E000 = base.gameObject.GetComponent<Image>();
		this.m__E001 = base.gameObject.GetComponent<RectTransform>();
		_E002 = this.m__E001.offsetMin;
		_E003 = this.m__E001.offsetMax;
		if (this.m__E001.anchorMax.x.ApproxEquals(this.m__E001.anchorMin.x) || this.m__E001.anchorMax.y.ApproxEquals(this.m__E001.anchorMin.y) || this.m__E001.childCount > 0)
		{
			base.enabled = false;
			Debug.LogError(_ED3E._E000(255219) + base.gameObject.name + _ED3E._E000(2540) + this.GetFullPath());
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_EC74.OnResolutionChanged += _E000;
		_E001();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		_EC74.OnResolutionChanged -= _E000;
	}

	private void _E000()
	{
		_E001();
	}

	private void _E001()
	{
		Vector3 lossyScale = base.transform.lossyScale;
		float num = Math.Min(lossyScale.x, lossyScale.y);
		if (!num.ApproxEquals(this.m__E000.pixelsPerUnitMultiplier))
		{
			this.m__E000.pixelsPerUnitMultiplier = num;
			this.m__E000.SetVerticesDirty();
			this.m__E001.offsetMin = _E002 / num;
			this.m__E001.offsetMax = _E003 / num;
		}
	}
}
