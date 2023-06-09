using JetBrains.Annotations;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class HoverTooltipAreaClick : HoverTooltipArea, IPointerClickHandler, IEventSystemHandler
{
	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
	}
}
