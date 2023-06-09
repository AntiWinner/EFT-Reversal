using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.Hideout;

public sealed class AreaWorldPanel : AreaPanel
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _E001
	{
		public Vector3 pos;

		public Vector2 screenSize;

		public Vector2 restrictedBorder;
	}

	public const float FADE_DURATION = 0.8f;

	private const float _E040 = 10f;

	[SerializeField]
	private CanvasGroup _labelsGroup;

	[SerializeField]
	private CanvasGroup _panelCanvasGroup;

	[SerializeField]
	private Vector2 _restrictedBorder = new Vector2(0.1f, 0.185f);

	[SerializeField]
	private Vector2 _cameraOffset = new Vector2(0.42f, 0.139f);

	private bool _E041;

	private Coroutine _E042;

	private Vector3 _E043;

	private bool _E044;

	private bool _E045;

	private float _E046;

	private float _E047;

	private bool _E000
	{
		get
		{
			return _E045;
		}
		set
		{
			if (base.gameObject.activeSelf && _E045 != value)
			{
				_E045 = value;
				if (_E042 != null)
				{
					StopCoroutine(_E042);
				}
				_E042 = StartCoroutine(_E000(_E045));
			}
		}
	}

	private void Awake()
	{
		_E001(visible: false);
		_panelCanvasGroup.alpha = 0f;
		_E046 = 0f;
		_E047 = 0f;
	}

	private void Update()
	{
		float value = _E046 - _E047;
		if (!value.IsZero())
		{
			float num = Time.deltaTime / 0.8f;
			value = Mathf.Clamp(value, 0f - num, num);
			_E047 += value;
			_panelCanvasGroup.alpha = _E047;
		}
	}

	protected override void SetInfo()
	{
		IEnumerable<AreaRequirement> source = base.Data.NextStage.Requirements.OfType<AreaRequirement>();
		_E044 = (source.All((AreaRequirement x) => x.Fulfilled) || base.Data.CurrentLevel >= 1) && base.Data.Requirements.All((Requirement x) => x.Fulfilled);
		_panelCanvasGroup.interactable = _E044;
		_panelCanvasGroup.blocksRaycasts = _E044;
		if (_E044)
		{
			base.SetInfo();
		}
	}

	private IEnumerator _E000(bool offscreen)
	{
		if (_E044)
		{
			Vector3 vector = (offscreen ? new Vector3(0.5f, 0.5f, 0.5f) : Vector3.one);
			Vector3 localScale = (offscreen ? Vector3.one : new Vector3(0.5f, 0.5f, 0.5f));
			base.transform.localScale = localScale;
			while (base.transform.localScale != vector && this._E000 == offscreen)
			{
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, vector, 10f * Time.deltaTime);
				yield return null;
			}
			base.transform.localScale = vector;
		}
	}

	public void SetPointOfView(bool firstPerson)
	{
		_E041 = firstPerson;
		_E002();
		if (!_E041)
		{
			_E046 = (_E044 ? 1f : 0f);
		}
	}

	public void SetPosition(Vector3 pos)
	{
		_E001 obj = default(_E001);
		obj.pos = pos;
		if (!_E044 || _E043 == obj.pos)
		{
			return;
		}
		_E043 = obj.pos;
		obj.screenSize = new Vector2(Screen.width, Screen.height);
		obj.restrictedBorder = _restrictedBorder * obj.screenSize;
		Vector2 vector = _cameraOffset * obj.screenSize;
		float num = 0f;
		float y = vector.y;
		float num2 = obj.screenSize.x - vector.x;
		float y2 = obj.screenSize.y;
		obj.pos.x = Mathf.Clamp(obj.pos.x, num, num2);
		obj.pos.y = Mathf.Clamp(obj.pos.y, y, y2);
		bool flag = false;
		if (_E003(ref obj))
		{
			flag = true;
			obj.pos.x = obj.restrictedBorder.x;
		}
		if (_E003(ref obj))
		{
			flag = true;
			obj.pos.y = obj.screenSize.y - obj.restrictedBorder.y;
		}
		Transform obj2 = base.transform;
		bool flag2 = obj.pos.z < 0.5f;
		this._E000 = obj.pos.x <= num || obj.pos.x >= num2 || obj.pos.y <= y || obj.pos.y >= y2 || flag || flag2;
		if (flag2)
		{
			float a = obj.pos.x - num;
			float b = num2 - obj.pos.x;
			float a2 = obj.pos.y - y;
			float b2 = y2 - obj.pos.y;
			float num3 = Mathf.Min(a, b);
			float num4 = Mathf.Min(a2, b2);
			float num5 = Mathf.Max(a, b);
			float num6 = Mathf.Max(a2, b2);
			if (num3 < num4)
			{
				obj.pos.y = y;
			}
			else if (num3 > num4)
			{
				obj.pos.x = num;
			}
			else if (num5 > num6)
			{
				obj.pos.x = num2;
			}
			else if (num5 < num6)
			{
				obj.pos.y = y2;
			}
		}
		obj2.position = obj.pos;
		if (_E041)
		{
			_E046 = Mathf.Clamp01(1f - Mathf.Abs(obj.pos.z) / 10f);
		}
	}

	private void _E001(bool visible)
	{
		_labelsGroup.gameObject.SetActive(visible);
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (!_E041)
		{
			base.OnPointerClick(eventData);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (!_E041)
		{
			base.OnPointerEnter(eventData);
			_E001(visible: true);
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (!_E041)
		{
			_E002();
		}
	}

	protected override void Init(AreaData data, Action<AreaPanel> onSelected)
	{
		base.Init(data, onSelected);
		UI.AddDisposable(base.Data.IconStatusUpdated.Subscribe(SetInfo));
	}

	private void _E002()
	{
		base.OnPointerExit((PointerEventData)null);
		_E001(visible: false);
	}

	[CompilerGenerated]
	internal static bool _E003(ref _E001 P_0)
	{
		if (P_0.pos.y > P_0.screenSize.y - P_0.restrictedBorder.y)
		{
			return P_0.pos.x < P_0.restrictedBorder.x;
		}
		return false;
	}
}
