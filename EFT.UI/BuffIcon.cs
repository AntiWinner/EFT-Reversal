using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class BuffIcon : BuffThumb, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private Sprite _normalBackgroundSprite;

	[SerializeField]
	private Sprite _eliteBackgroundSprite;

	private SimpleTooltip _E02A;

	[CanBeNull]
	private static Sprite _E000(_E74F._E003 buff)
	{
		if (buff.Id == EBuffId.None)
		{
			return null;
		}
		return EFTHardSettings.Instance.StaticIcons.BuffIdSprites.GetValueOrDefault(buff.Id);
	}

	public static string GetBuffDescription(_E74F._E003 buff)
	{
		string text = buff.Id.ToString().Localized().Replace(_ED3E._E000(147704), _ED3E._E000(215238))
			.Replace(_ED3E._E000(11164), _ED3E._E000(59467));
		if (text.Contains(_ED3E._E000(258178)))
		{
			return string.Format(text, buff.ValueObj);
		}
		return text;
	}

	public void Show(_E74F._E003 buff, _E751 skill)
	{
		Buff = buff;
		Skill = skill;
		_E02A = ItemUiContext.Instance.Tooltip;
		_icon.sprite = _E000(buff);
		UpdateBuff();
	}

	public void UpdateBuff()
	{
		bool isEliteLevel = Skill.IsEliteLevel;
		bool flag = Buff.BuffType != _E74F.EBuffType.Simple && isEliteLevel;
		_background.sprite = (flag ? _eliteBackgroundSprite : _normalBackgroundSprite);
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		bool flag = Buff.BuffType != 0 && Skill.IsEliteLevel;
		_E02A.Show((flag ? _ED3E._E000(258237) : "") + GetBuffDescription(Buff));
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_E02A.Close();
	}
}
