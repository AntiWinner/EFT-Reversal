using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.EventSystems;

namespace EFT.UI;

public class TMP_InputFieldNoScroll : TMP_InputField
{
	private Action<PointerEventData> _E000;

	public void Show(Action<PointerEventData> onScroll)
	{
		_E000 = onScroll;
	}

	public override void OnScroll([NotNull] PointerEventData eventData)
	{
		if (_E000 != null)
		{
			_E000(eventData);
		}
	}
}
