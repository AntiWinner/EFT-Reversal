using System;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class MapZoomer : MonoBehaviour
{
	[SerializeField]
	private float minCof = 5f;

	[SerializeField]
	private float maxCof = 2f;

	[SerializeField]
	private float scrollSensitivity = 2f;

	[SerializeField]
	private GameObject _fullMap;

	[SerializeField]
	private Scrollbar _zoomScroll;

	private RectTransform m__E000;

	private Vector3 _E001;

	private Vector3 _E002;

	private float _E003 = 0.35f;

	private float _E004 = 0.9f;

	private float _E005 = 30f;

	private void Start()
	{
		if (_fullMap != null)
		{
			this.m__E000 = (RectTransform)_fullMap.transform;
		}
		if (!(this.m__E000 == null))
		{
			this.m__E000.localScale = new Vector3(_E003, _E003, 1f);
			this.m__E000.localPosition = Vector3.zero;
			_zoomScroll.value = Mathf.InverseLerp(0.95f, -1f, this.m__E000.localScale.y);
		}
	}

	private void FixedUpdate()
	{
		if (!this.m__E000.Equals(null))
		{
			float num = Input.GetAxis(_ED3E._E000(205973)) / scrollSensitivity;
			float axis = Input.GetAxis(_ED3E._E000(23374));
			float axis2 = Input.GetAxis(_ED3E._E000(23366));
			Vector3 localScale = this.m__E000.localScale;
			float num2 = localScale.x + num;
			float num3 = localScale.y + num;
			if (num2 > _E003 && num2 < _E004 && (_E002.x != num2 || _E002.y != num3))
			{
				this.m__E000.localScale = new Vector3(num2, num3, 1f);
				_E002 = this.m__E000.localScale;
				_zoomScroll.value = Mathf.InverseLerp(0.95f, -1f, localScale.y);
			}
			if (Input.GetMouseButton(0))
			{
				this.m__E000.localPosition = _E000(axis, axis2);
			}
		}
	}

	private Vector3 _E000(float h, float v)
	{
		Vector3 localPosition = this.m__E000.localPosition;
		float num = localPosition.x + h * _E005;
		float num2 = localPosition.y + v * _E005;
		Vector3 localScale = this.m__E000.localScale;
		float num3 = ((localScale.x <= _E003 + 0.1f) ? minCof : maxCof);
		float num4 = ((localScale.y <= _E003 + 0.1f) ? minCof : maxCof);
		if (Math.Abs(num) > Mathf.Abs(this.m__E000.sizeDelta.x * this.m__E000.localScale.x / num3 / 2f))
		{
			num = Mathf.Sign(num) * this.m__E000.sizeDelta.x * this.m__E000.localScale.x / num3 / 2f;
		}
		if (Math.Abs(num2) > Mathf.Abs(this.m__E000.sizeDelta.y * this.m__E000.localScale.y / num4 / 2f))
		{
			num2 = Mathf.Sign(num2) * this.m__E000.sizeDelta.y * this.m__E000.localScale.y / num4 / 2f;
		}
		return new Vector3(num, num2);
	}
}
