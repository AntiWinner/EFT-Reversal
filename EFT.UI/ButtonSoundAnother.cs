using Comfort.Common;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public class ButtonSoundAnother : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonOver);
	}
}
