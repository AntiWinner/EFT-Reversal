using TMPro;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public sealed class MatchmakerRaidSettingView : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _valueText;

	private CanvasGroup _E000;

	private void Awake()
	{
		_E000 = GetComponent<CanvasGroup>();
	}

	public void Refresh(string value, bool isActive)
	{
		_valueText.text = value;
		_E000.SetUnlockStatus(isActive, setRaycast: false);
	}
}
