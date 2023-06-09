using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.Hideout;

public class AreasScrollRect : ScrollRect
{
	private bool _E000;

	public readonly UnityEvent OnStartDrag = new UnityEvent();

	public void ResetDrag()
	{
		_E000 = true;
	}

	public override void OnInitializePotentialDrag(PointerEventData eventData)
	{
		_E000 = false;
		base.OnInitializePotentialDrag(eventData);
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		_E000 = false;
		base.OnBeginDrag(eventData);
		OnStartDrag.Invoke();
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if (!_E000)
		{
			base.OnDrag(eventData);
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (!_E000)
		{
			base.OnEndDrag(eventData);
		}
	}
}
