using EFT.UI;
using UnityEngine;
using UnityEngine.UI;

public class FilterTab : Tab
{
	[SerializeField]
	private Image _filterIcon;

	private RectTransform _E000;

	protected override void OnAwake()
	{
		_E000 = _filterIcon.GetComponent<RectTransform>();
		base.OnAwake();
	}

	public void SetIcon(Sprite icon)
	{
		if (!(icon == null))
		{
			_filterIcon.sprite = icon;
			if (_E000 == null)
			{
				_E000 = _filterIcon.GetComponent<RectTransform>();
			}
			_E000.sizeDelta = new Vector2(_filterIcon.preferredWidth, _filterIcon.preferredHeight);
		}
	}

	internal override void _E001(bool active)
	{
		base._E001(active);
		_filterIcon.ChangeImageAlpha(active ? 1f : 0.15f);
		GetComponent<ButtonFeedback>().enabled = active;
	}
}
