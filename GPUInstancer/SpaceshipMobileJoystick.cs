using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GPUInstancer;

public class SpaceshipMobileJoystick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
{
	[HideInInspector]
	public Vector3 inputDirection;

	private Image _E000;

	private Image _E001;

	private Vector2 _E002;

	private void Start()
	{
		_E000 = GetComponent<Image>();
		_E001 = base.transform.GetChild(0).GetComponent<Image>();
		inputDirection = Vector3.zero;
	}

	public virtual void OnDrag(PointerEventData data)
	{
		_E002 = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_E000.rectTransform, data.position, data.pressEventCamera, out _E002))
		{
			inputDirection.x = _E002.x / _E000.rectTransform.sizeDelta.x * 2f + (float)((_E000.rectTransform.pivot.x == 1f) ? 1 : (-1));
			inputDirection.z = _E002.y / _E000.rectTransform.sizeDelta.y * 2f + (float)((_E000.rectTransform.pivot.y == 1f) ? 1 : (-1));
			inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);
			_E001.rectTransform.anchoredPosition = new Vector3(inputDirection.x * (_E000.rectTransform.sizeDelta.x / 3f), inputDirection.z * (_E000.rectTransform.sizeDelta.y / 3f));
		}
	}

	public virtual void OnPointerDown(PointerEventData data)
	{
		OnDrag(data);
	}

	public virtual void OnPointerUp(PointerEventData data)
	{
		inputDirection = Vector3.zero;
		_E001.rectTransform.anchoredPosition = Vector3.zero;
	}
}
