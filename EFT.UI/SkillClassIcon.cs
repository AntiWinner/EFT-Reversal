using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class SkillClassIcon : UIElement
{
	[SerializeField]
	private Image _background;

	[SerializeField]
	private CustomTextMeshProUGUI _symbol;

	private static readonly Dictionary<ESkillClass, Color> _E1A4 = new Dictionary<ESkillClass, Color>
	{
		{
			ESkillClass.Physical,
			new Color32(103, 177, 184, byte.MaxValue)
		},
		{
			ESkillClass.Combat,
			new Color32(184, 87, 57, byte.MaxValue)
		},
		{
			ESkillClass.Mental,
			new Color32(220, 215, 185, byte.MaxValue)
		},
		{
			ESkillClass.Special,
			new Color32(184, 156, 71, byte.MaxValue)
		},
		{
			ESkillClass.Practical,
			new Color32(175, 220, 169, byte.MaxValue)
		}
	};

	private static readonly Dictionary<ESkillClass, string> _E1A5 = new Dictionary<ESkillClass, string>
	{
		{
			ESkillClass.Physical,
			_ED3E._E000(258896)
		},
		{
			ESkillClass.Combat,
			_ED3E._E000(47712)
		},
		{
			ESkillClass.Mental,
			_ED3E._E000(47696)
		},
		{
			ESkillClass.Special,
			_ED3E._E000(47743)
		},
		{
			ESkillClass.Practical,
			_ED3E._E000(47685)
		}
	};

	public void Set(ESkillClass skillClass)
	{
		_background.color = _E1A4[skillClass];
		_symbol.text = _E1A5[skillClass];
	}
}
