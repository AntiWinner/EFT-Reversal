using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.UI;
using UnityEngine;

public class TabElement : UIElement
{
	[SerializeField]
	private List<RectTransform> _sizeDefiners;

	[SerializeField]
	private float _widthAdjustment;

	public int OrderNumber;

	[CompilerGenerated]
	private float _E002;

	private _ECEC _E003;

	private RectTransform _E004;

	public float Width
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		private set
		{
			_E002 = value;
		}
	}

	public _ECEC OnLayoutUpdatedEvent => _E003 ?? (_E003 = new _ECEC());

	public void SetWidth(float width, float gap)
	{
		Vector3 localPosition = _E004.localPosition;
		_E004.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
		_E004.localPosition = new Vector3((width + gap) * (float)OrderNumber, localPosition.y, localPosition.z);
	}

	private float _E000()
	{
		float num = 0f;
		foreach (RectTransform sizeDefiner in _sizeDefiners)
		{
			if (sizeDefiner != null && sizeDefiner.gameObject.activeInHierarchy)
			{
				num = Math.Max(num, sizeDefiner.rect.width);
			}
		}
		return num + _widthAdjustment;
	}

	private void Update()
	{
		float num = _E000();
		if (num != Width)
		{
			Width = num;
			OnLayoutUpdatedEvent.Invoke();
		}
	}

	private void Awake()
	{
		_E004 = base.gameObject.GetComponent<RectTransform>();
	}

	private void OnDestroy()
	{
		Dispose();
	}
}
