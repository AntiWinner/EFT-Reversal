using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class UIJittingStabilizer : MonoBehaviour
{
	[Serializable]
	public class JittedUiElement
	{
		public GameObject GameObjectCached;

		public RectTransform RectTransformCached;

		public Vector2 LastFramePosition;

		public bool Stabilize = true;

		public bool IsJitted;

		public JittedUiElement(RectTransform rectTransform)
		{
			GameObjectCached = rectTransform.gameObject;
			RectTransformCached = rectTransform;
			LastFramePosition = rectTransform.anchoredPosition;
		}

		public void UpdatePosition()
		{
			Vector2 anchoredPosition = RectTransformCached.anchoredPosition;
			float num = Vector2.Distance(anchoredPosition, LastFramePosition);
			if (!RectTransformCached.hasChanged)
			{
				IsJitted = false;
				LastFramePosition = anchoredPosition;
			}
			else if (RectTransformCached.hasChanged && (IsJitted = num > Mathf.Epsilon && num < MinDeltaDistance))
			{
				if (Stabilize)
				{
					RectTransformCached.anchoredPosition = LastFramePosition;
				}
				RectTransformCached.hasChanged = false;
			}
			else
			{
				LastFramePosition = anchoredPosition;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public Transform thisTransform;

		public Func<RectTransform, bool> _003C_003E9__0;

		internal bool _E000(RectTransform t)
		{
			return t.parent == thisTransform;
		}
	}

	public static float MinDeltaDistance = 0.01f;

	public ScrollRect ScrollRectCached;

	public List<JittedUiElement> ControlledElements = new List<JittedUiElement>();

	private ScrollRect.MovementType m__E000;

	private bool m__E001;

	private float m__E002;

	private void Start()
	{
		ScrollRectCached = GetComponent<ScrollRect>();
		this.m__E000 = ScrollRectCached.movementType;
		ScrollRectCached.onValueChanged.AddListener(_E000);
		RectTransform[] componentsInChildren = GetComponentsInChildren<RectTransform>(includeInactive: true);
		Transform thisTransform = base.transform;
		foreach (RectTransform item in componentsInChildren.Where((RectTransform t) => t.parent == thisTransform))
		{
			ControlledElements.Add(new JittedUiElement(item));
		}
	}

	private void _E000(Vector2 arg0)
	{
		if (base.gameObject.activeInHierarchy && !this.m__E001)
		{
			_E001();
			ScrollRectCached.movementType = this.m__E000;
		}
	}

	private void _E001()
	{
		this.m__E002 = Time.realtimeSinceStartup + 3f;
	}

	private bool _E002()
	{
		return Time.realtimeSinceStartup > this.m__E002;
	}

	private void OnEnable()
	{
		StartCoroutine(_E003());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator _E003()
	{
		while (true)
		{
			yield return new _EC77();
			this.m__E001 = false;
			foreach (JittedUiElement item in ControlledElements.Where((JittedUiElement uiElement) => (bool)uiElement.GameObjectCached && uiElement.GameObjectCached.activeInHierarchy))
			{
				item.UpdatePosition();
				if (item.IsJitted && _E002())
				{
					this.m__E001 = true;
				}
			}
			if (this.m__E001 && _E004())
			{
				ScrollRectCached.movementType = ScrollRect.MovementType.Unrestricted;
			}
		}
	}

	private bool _E004()
	{
		return new Rect(0f - MinDeltaDistance, 0f - MinDeltaDistance, 1f + MinDeltaDistance * 2f, 1f + MinDeltaDistance * 2f).Contains(ScrollRectCached.normalizedPosition);
	}
}
