using EFT.InputSystem;
using UnityEngine;

namespace EFT.UI;

public abstract class UIInputNode : InputNode, IShowable
{
	protected readonly _EC76 UI = new _EC76();

	private RectTransform _E0AC;

	protected RectTransform RectTransform => _E0AC ?? (_E0AC = (RectTransform)base.transform);

	public virtual void Display()
	{
		ShowGameObject();
	}

	public virtual void Close()
	{
		UI.Dispose();
		HideGameObject();
	}

	public virtual void ShowGameObject(bool instant = false)
	{
		base.gameObject.SetActive(value: true);
	}

	public virtual void HideGameObject()
	{
		base.gameObject.SetActive(value: false);
	}

	protected void CorrectPosition(_E3F3 margins = default(_E3F3))
	{
		if ((bool)this && base.transform != null)
		{
			RectTransform.CorrectPositionResolution(margins);
		}
	}
}
