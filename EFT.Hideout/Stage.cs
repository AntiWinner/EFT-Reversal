using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace EFT.Hideout;

[Serializable]
public class Stage
{
	private Dictionary<EPanelType, Func<RelatedData>> _data;

	[JsonProperty("requirements")]
	public RelatedRequirements Requirements = new RelatedRequirements();

	[JsonProperty("bonuses")]
	public RelatedBonuses Bonuses = new RelatedBonuses();

	[JsonProperty("constructionTime")]
	public RelatedConstructionTime ConstructionTime = new RelatedConstructionTime();

	public RelatedBoost Boost = new RelatedBoost();

	public RelatedFuelSupply FuelSupply = new RelatedFuelSupply();

	public RelatedProduction Production = new RelatedProduction();

	public RelatedImprovement Improvements = new RelatedImprovement();

	public RelatedSounds Sounds = new RelatedSounds();

	public RelatedQteData QteData = new RelatedQteData();

	[_E3FB(0f)]
	public float HideoutProgress = 10f;

	[JsonIgnore]
	public Dictionary<EPanelType, Func<RelatedData>> Data
	{
		get
		{
			Dictionary<EPanelType, Func<RelatedData>> dictionary = _data;
			if (dictionary == null)
			{
				Dictionary<EPanelType, Func<RelatedData>> obj = new Dictionary<EPanelType, Func<RelatedData>>
				{
					{
						EPanelType.Description,
						() => new RelatedDescription()
					},
					{
						EPanelType.Requirements,
						() => Requirements
					},
					{
						EPanelType.ConstructionTime,
						() => ConstructionTime
					},
					{
						EPanelType.Bonuses,
						() => Bonuses
					},
					{
						EPanelType.Boost,
						() => Boost
					},
					{
						EPanelType.FuelSupply,
						() => FuelSupply
					},
					{
						EPanelType.Production,
						() => Production
					},
					{
						EPanelType.Improvement,
						() => Improvements
					},
					{
						EPanelType.UnitReady,
						() => new RelatedUnitReady()
					}
				};
				Dictionary<EPanelType, Func<RelatedData>> dictionary2 = obj;
				_data = obj;
				dictionary = dictionary2;
			}
			return dictionary;
		}
	}

	public DateTime StartTime { get; set; }

	public int Level { get; set; }

	public bool ActionGoing { get; set; }

	public bool Waiting { get; set; }

	public bool ActionReady { get; set; }

	public bool AutoUpgrade { get; set; }

	public bool DisplayInterface { get; set; }

	public void UpdateData(Stage stage)
	{
		Requirements = stage.Requirements;
		Bonuses = stage.Bonuses;
		ConstructionTime = stage.ConstructionTime;
		Boost = stage.Boost;
		FuelSupply = stage.FuelSupply;
		Production = stage.Production;
		Improvements = stage.Improvements;
		AutoUpgrade = stage.AutoUpgrade;
		DisplayInterface = stage.DisplayInterface;
		if (QteData == null)
		{
			QteData = stage.QteData;
		}
		QteData.UpdateClient(stage.QteData);
	}

	[CompilerGenerated]
	private RelatedData _E000()
	{
		return Requirements;
	}

	[CompilerGenerated]
	private RelatedData _E001()
	{
		return ConstructionTime;
	}

	[CompilerGenerated]
	private RelatedData _E002()
	{
		return Bonuses;
	}

	[CompilerGenerated]
	private RelatedData _E003()
	{
		return Boost;
	}

	[CompilerGenerated]
	private RelatedData _E004()
	{
		return FuelSupply;
	}

	[CompilerGenerated]
	private RelatedData _E005()
	{
		return Production;
	}

	[CompilerGenerated]
	private RelatedData _E006()
	{
		return Improvements;
	}
}
