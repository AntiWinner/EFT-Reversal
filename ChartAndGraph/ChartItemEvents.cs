using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ChartAndGraph;

internal class ChartItemEvents : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, _ED1D
{
	[Serializable]
	public class Event : UnityEvent<GameObject>
	{
	}

	[SerializeField]
	private ChartItemTextBlend _textBlend;

	private bool _E000;

	private bool _E001;

	private IInternalUse _E002;

	private object _E003;

	IInternalUse _ED1D.Parent
	{
		get
		{
			return _E002;
		}
		set
		{
			_E002 = value;
		}
	}

	object _ED1D.UserData
	{
		get
		{
			return _E003;
		}
		set
		{
			_E003 = value;
		}
	}

	public void OnMouseEnter()
	{
		if (!_E000)
		{
			_textBlend.Grow();
		}
		if (_E002 != null)
		{
			_E002.InternalItemHovered(_E003);
		}
		_E000 = true;
	}

	private void OnMouseExit()
	{
		if (_E000)
		{
			_textBlend.Shrink();
		}
		if (_E002 != null)
		{
			_E002.InternalItemLeave(_E003);
		}
		_E000 = false;
	}

	public void OnMouseDown()
	{
		if (!_E001)
		{
			_textBlend.Grow();
		}
		if (_E002 != null)
		{
			_E002.InternalItemSelected(_E003);
		}
		_E001 = true;
	}

	private void OnMouseUp()
	{
		_E001 = false;
	}

	public void OnPointerEnter([CanBeNull] PointerEventData eventData)
	{
		OnMouseEnter();
	}

	public void OnPointerExit([CanBeNull] PointerEventData eventData)
	{
		OnMouseExit();
	}

	public void OnPointerDown([CanBeNull] PointerEventData eventData)
	{
		OnMouseDown();
	}

	public void OnPointerUp([CanBeNull] PointerEventData eventData)
	{
		OnMouseUp();
	}
}
