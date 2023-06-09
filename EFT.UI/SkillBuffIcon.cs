using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class SkillBuffIcon : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private Image _image;

	[SerializeField]
	private Sprite _buffSprite;

	[SerializeField]
	private Sprite _debuffSprite;

	private SimpleTooltip _E02A;

	private _E986 _E1A3;

	public void Show(_E986 buff)
	{
		_E02A = ItemUiContext.Instance.Tooltip;
		ShowGameObject();
		_E1A3 = buff;
		_image.sprite = (buff.Settings.IsBuff ? _buffSprite : _debuffSprite);
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		if (!_E02A.gameObject.activeSelf)
		{
			_E02A.Show(_E1A3.BuffName);
		}
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		if (_E02A.gameObject.activeSelf)
		{
			_E02A.Close();
		}
	}

	public override void Close()
	{
		if (_E02A.gameObject.activeSelf)
		{
			_E02A.Close();
		}
		base.Close();
	}
}
