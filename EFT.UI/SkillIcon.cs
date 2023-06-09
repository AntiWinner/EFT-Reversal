using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.HealthSystem;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class SkillIcon : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E9C4 healthController;

		public SkillIcon _003C_003E4__this;

		public _E751 skill;

		internal void _E000()
		{
			healthController.StimulatorBuffEvent -= _003C_003E4__this._E001;
		}

		internal void _E001()
		{
			bool isEliteLevel = _003C_003E4__this._E1A6.IsEliteLevel;
			float num = skill.LevelProgress * (float)skill.LevelExp;
			_003C_003E4__this._progress.text = (isEliteLevel ? (_ED3E._E000(258924) + _ED3E._E000(258972).Localized() + _ED3E._E000(59467)) : (_ED3E._E000(258899) + num.ToString(_ED3E._E000(229344)) + _ED3E._E000(258942) + skill.LevelExp));
			_003C_003E4__this._border.color = (isEliteLevel ? _E1A7 : Color.white);
			_003C_003E4__this._levelPanel.SetLevel(_003C_003E4__this._E1A6);
		}
	}

	[SerializeField]
	private Image _border;

	[SerializeField]
	private CustomTextMeshProUGUI _progress;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private SkillLevelPanel _levelPanel;

	[SerializeField]
	private SkillClassIcon _classIcon;

	[SerializeField]
	private HoverTooltipArea _alphaBlockedSkill;

	[SerializeField]
	private SkillBuffIcon _skillBuffIcon;

	private _E751 _E1A6;

	private static readonly Color _E1A7 = new Color32(183, 112, 0, byte.MaxValue);

	private Action<bool, PointerEventData> _E0F1;

	private bool _E1A8;

	private void Awake()
	{
		HoverTrigger orAddComponent = _icon.gameObject.GetOrAddComponent<HoverTrigger>();
		orAddComponent.OnHoverStart += delegate(PointerEventData eventData)
		{
			if (_E1A8)
			{
				_E0F1(arg1: true, eventData);
			}
		};
		orAddComponent.OnHoverEnd += delegate(PointerEventData eventData)
		{
			if (_E1A8)
			{
				_E0F1(arg1: false, eventData);
			}
		};
	}

	[CanBeNull]
	private static Sprite _E000(_E751 skill)
	{
		return EFTHardSettings.Instance.StaticIcons.SkillIdSprites.GetValueOrDefault(skill.Id);
	}

	public void Show(_E751 skill, [CanBeNull] _E9C4 healthController, Action<bool, PointerEventData> onHover)
	{
		ShowGameObject();
		_E1A8 = !skill.Locked;
		_alphaBlockedSkill.SetUnlockStatus(_E1A8);
		_E0F1 = onHover;
		_E1A6 = skill;
		if (healthController != null)
		{
			healthController.StimulatorBuffEvent += _E001;
			UI.AddDisposable(delegate
			{
				healthController.StimulatorBuffEvent -= _E001;
			});
			foreach (_E9C0 item in healthController.GetAllActiveEffects(EBodyPart.Head).OfType<_E9C0>())
			{
				foreach (_E986 item2 in item.ActiveBuffs.Where((_E986 x) => x.Active))
				{
					_E001(item2);
				}
			}
		}
		_classIcon.Set(skill.Class);
		_icon.sprite = _E000(skill);
		UI.BindEvent(skill.SkillLevelChanged, delegate
		{
			bool isEliteLevel = _E1A6.IsEliteLevel;
			float num = skill.LevelProgress * (float)skill.LevelExp;
			_progress.text = (isEliteLevel ? (_ED3E._E000(258924) + _ED3E._E000(258972).Localized() + _ED3E._E000(59467)) : (_ED3E._E000(258899) + num.ToString(_ED3E._E000(229344)) + _ED3E._E000(258942) + skill.LevelExp));
			_border.color = (isEliteLevel ? _E1A7 : Color.white);
			_levelPanel.SetLevel(_E1A6);
		});
	}

	private void _E001(_E986 buff)
	{
		if (buff.Settings.BuffType == EStimulatorBuffType.SkillRate && !(buff.Settings.SkillName != _E1A6.Id.ToString()))
		{
			if (buff.Active)
			{
				_skillBuffIcon.Show(buff);
			}
			else
			{
				_skillBuffIcon.Close();
			}
			_levelPanel.SetLevel(_E1A6);
		}
	}

	[CompilerGenerated]
	private void _E002(PointerEventData eventData)
	{
		if (_E1A8)
		{
			_E0F1(arg1: true, eventData);
		}
	}

	[CompilerGenerated]
	private void _E003(PointerEventData eventData)
	{
		if (_E1A8)
		{
			_E0F1(arg1: false, eventData);
		}
	}
}
