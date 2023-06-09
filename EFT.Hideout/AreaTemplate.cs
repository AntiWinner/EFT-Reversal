using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.Hideout;

public sealed class AreaTemplate : SerializedScriptableObject
{
	public string Id;

	public bool Enabled;

	public bool NeedsFuel;

	public bool CraftGivesExp;

	public bool TakeFromSlotLocked;

	public EAreaType Type;

	public bool DisplayLevel;

	[JsonIgnore]
	public _E831 AreaBehaviour;

	[JsonIgnore]
	public Sprite AreaIcon;

	[JsonIgnore]
	public int CameraTimePosition;

	[JsonIgnore]
	public bool IsElite;

	[JsonIgnore]
	public readonly Dictionary<ELightStatus, float> PowerDelays = new Dictionary<ELightStatus, float>();

	[JsonIgnore]
	public Dictionary<EAreaActivityType, AudioClip> DefaultSounds = new Dictionary<EAreaActivityType, AudioClip>();

	[HideInInspector]
	public RelatedRequirements Requirements = new RelatedRequirements();

	public Dictionary<int, Stage> Stages = new Dictionary<int, Stage>();

	public int MaxLevel
	{
		get
		{
			if (Stages.Count <= 0)
			{
				return 0;
			}
			return Stages.Count - 1;
		}
	}

	public string Name => Type.LocalizeAreaName();

	public void Init()
	{
		foreach (var (level, stage2) in Stages)
		{
			stage2.Sounds.FallbackSounds = DefaultSounds;
			stage2.Level = level;
		}
	}

	public void UpdateData(AreaTemplate template)
	{
		NeedsFuel = template.NeedsFuel;
		Enabled = template.Enabled;
		TakeFromSlotLocked = template.TakeFromSlotLocked;
		CraftGivesExp = template.CraftGivesExp;
		DisplayLevel = template.DisplayLevel;
		Requirements = template.Requirements;
	}
}
