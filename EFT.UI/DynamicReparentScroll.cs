using UnityEngine;

namespace EFT.UI;

public class DynamicReparentScroll : DynamicScroll
{
	[SerializeField]
	private RectTransform _temporaryContainer;

	[SerializeField]
	private RectTransform _originalContainer;

	protected override void Enable(DynamicScrollElement scrollElement, EArrangement arrangement)
	{
		base.Enable(scrollElement, arrangement);
		scrollElement.transform.SetParent(_originalContainer);
	}

	protected override void Disable(DynamicScrollElement scrollElement, EArrangement arrangement)
	{
		base.Disable(scrollElement, arrangement);
		scrollElement.transform.SetParent(_temporaryContainer);
	}
}
