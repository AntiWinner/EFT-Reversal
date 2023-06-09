using TMPro;
using UnityEngine;

public class BotDebugMenuItemTemplate : MonoBehaviour
{
	public TMP_Text ItemLabel;

	public TMP_Text HintText;

	[SerializeField]
	private GameObject _selector;

	[SerializeField]
	private GameObject _leftArrow;

	[SerializeField]
	private GameObject _rightArrow;

	[SerializeField]
	private GameObject _hintRoot;

	public void ChangeSelection(bool selected)
	{
		_selector.SetActive(selected);
	}

	public void SetArrowsVisible(bool left, bool right)
	{
		_leftArrow.SetActive(left);
		_rightArrow.SetActive(right);
	}

	public void SetHitActive(bool active)
	{
		_hintRoot.SetActive(active);
	}
}
