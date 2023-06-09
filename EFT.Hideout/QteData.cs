using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace EFT.Hideout;

[Serializable]
public class QteData
{
	public enum EQteActivityType
	{
		Gym
	}

	public enum EQteEffectType
	{
		FinishEffect,
		SingleSuccessEffect,
		SingleFailEffect
	}

	[JsonProperty("Id")]
	public string Id { get; private set; }

	[JsonProperty("Type")]
	[SerializeField]
	public EQteActivityType Type { get; private set; }

	[JsonProperty("Area")]
	public EAreaType AreaType { get; private set; }

	[JsonProperty("AreaLevel")]
	public int AreaLevel { get; private set; }

	[JsonProperty("QuickTimeEvents")]
	public QuickTimeEvent[] QuickTimeEvents { get; private set; }

	[JsonProperty("Requirements")]
	public Requirement[] Requirements { get; private set; }

	[JsonProperty("Results")]
	public Dictionary<EQteEffectType, QteResult> Results { get; private set; }

	public void Update(QteData backendData)
	{
		Id = backendData.Id;
		Type = backendData.Type;
		AreaType = backendData.AreaType;
		AreaLevel = backendData.AreaLevel;
		QuickTimeEvents = backendData.QuickTimeEvents;
		Requirements = backendData.Requirements;
		Results = backendData.Results;
	}
}
