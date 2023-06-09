using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Matchmaker;

public sealed class ComradeView : UIElement
{
	[SerializeField]
	private Image _comradePlaceholder;

	public void TogglePlaceholder(bool active)
	{
		_comradePlaceholder.gameObject.SetActive(active);
	}
}
