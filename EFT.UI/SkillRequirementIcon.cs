using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class SkillRequirementIcon : UIElement
{
	[SerializeField]
	private Image _border;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private SkillLevelPanel _levelPanel;

	private _E751 _E1A6;

	private Action<bool, PointerEventData> _E0F1;

	private readonly Color _E1A7 = new Color32(183, 112, 0, byte.MaxValue);

	private void Awake()
	{
		HoverTrigger orAddComponent = _icon.gameObject.GetOrAddComponent<HoverTrigger>();
		orAddComponent.OnHoverStart += delegate(PointerEventData eventData)
		{
			_E0F1(arg1: true, eventData);
		};
		orAddComponent.OnHoverEnd += delegate(PointerEventData eventData)
		{
			_E0F1(arg1: false, eventData);
		};
	}

	public void Show(_E751 skill, Action<bool, PointerEventData> onHover)
	{
		ShowGameObject();
		_E0F1 = onHover;
		_E1A6 = skill;
		_icon.sprite = _E000(skill);
		UI.BindEvent(skill.SkillLevelChanged, delegate
		{
			bool isEliteLevel = _E1A6.IsEliteLevel;
			_border.color = (isEliteLevel ? _E1A7 : Color.white);
			_levelPanel.SetLevel(_E1A6);
		});
	}

	[CanBeNull]
	private static Sprite _E000(_E751 skill)
	{
		return EFTHardSettings.Instance.StaticIcons.SkillIdSprites.GetValueOrDefault(skill.Id);
	}

	[CompilerGenerated]
	private void _E001(PointerEventData eventData)
	{
		_E0F1(arg1: true, eventData);
	}

	[CompilerGenerated]
	private void _E002(PointerEventData eventData)
	{
		_E0F1(arg1: false, eventData);
	}

	[CompilerGenerated]
	private void _E003()
	{
		bool isEliteLevel = _E1A6.IsEliteLevel;
		_border.color = (isEliteLevel ? _E1A7 : Color.white);
		_levelPanel.SetLevel(_E1A6);
	}
}
