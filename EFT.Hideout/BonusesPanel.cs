using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace EFT.Hideout;

public sealed class BonusesPanel : AbstractPanel<List<_E5EA>>
{
	private const string m__E000 = "CURRENT BONUSES:";

	private const string m__E001 = "FUTURE BONUSES:";

	private const string _E002 = "CURRENT NEGATIV BONUSES:";

	private const string _E003 = "FUTURE NEGATIV BONUSES:";

	private readonly string[] _E004 = new string[2]
	{
		_ED3E._E000(165469),
		_ED3E._E000(165454)
	};

	private readonly string[] _E005 = new string[2]
	{
		_ED3E._E000(165502),
		_ED3E._E000(165479)
	};

	[SerializeField]
	private GroupBonusPanel _positiveBonusesPanel;

	[SerializeField]
	private GroupBonusPanel _negativeBonusesPanel;

	public override async Task Show(RelatedData relatedData, Stage stage, ELevelType levelType, AreaData areaData, Player player, _E796 session)
	{
		await base.Show(relatedData, stage, levelType, areaData, player, session);
		UI.AddDisposable(areaData.ImprovementsUpdated.Subscribe(delegate
		{
			ShowContents();
		}));
	}

	public override async Task ShowContents()
	{
		Dictionary<EBonusType, _E5EA> dictionary = new Dictionary<EBonusType, _E5EA>();
		List<_E5EA> list = new List<_E5EA>();
		switch (base.LevelType)
		{
		case ELevelType.Current:
		{
			int currentLevel = base.AreaData.CurrentLevel;
			Dictionary<int, Stage> stages = base.AreaData.Template.Stages;
			for (int i = 0; i <= currentLevel; i++)
			{
				list.AddRange(stages[i].Bonuses.Where((_E5EA b) => b.IsVisible));
			}
			IEnumerable<_E5EA> collection = from b in (from v in base.Stage.Improvements
					where v.Completed
					select v.Bonuses).Flatten()
				where b.IsVisible
				select b;
			list.AddRange(collection);
			break;
		}
		case ELevelType.Next:
			list = base.AreaData.NextStage.Bonuses.Where((_E5EA v) => v.IsVisible).ToList();
			break;
		}
		foreach (_E5EA item in list)
		{
			if (dictionary.ContainsKey(item.BonusType))
			{
				dictionary[item.BonusType].Add(item);
			}
			else
			{
				dictionary.Add(item.BonusType, item.Clone());
			}
		}
		IEnumerable<_E5EA> source = dictionary.Values.Where((_E5EA x) => x.IsPositive);
		IEnumerable<_E5EA> source2 = dictionary.Values.Where((_E5EA x) => !x.IsPositive);
		RelatedBonuses relatedData = new RelatedBonuses
		{
			Data = source.ToList()
		};
		RelatedBonuses relatedData2 = new RelatedBonuses
		{
			Data = source2.ToList()
		};
		await _positiveBonusesPanel.Show(relatedData, base.Stage, base.LevelType, base.AreaData, base.Player, base.Session, _E004);
		await _negativeBonusesPanel.Show(relatedData2, base.Stage, base.LevelType, base.AreaData, base.Player, base.Session, _E005);
	}

	public override void SetInfo()
	{
	}

	[CompilerGenerated]
	private void _E000()
	{
		ShowContents();
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E001(RelatedData relatedData, Stage stage, ELevelType levelType, AreaData areaData, Player player, _E796 session)
	{
		return base.Show(relatedData, stage, levelType, areaData, player, session);
	}
}
