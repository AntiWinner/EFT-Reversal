using System;
using UnityEngine;

namespace EFT.Interactive;

[Serializable]
public class ExitTriggerSettings
{
	public string Name;

	public EExfiltrationType ExfiltrationType;

	public float ExfiltrationTime;

	public int PlayersCount;

	[Header("Presence settings")]
	public float Chance;

	public float MinTime;

	public float MaxTime;

	public int StartTime;

	public string EntryPoints;

	public void Load(_E556 settings, ExfiltrationPoint point)
	{
		MinTime = settings.MinTime;
		MaxTime = settings.MaxTime;
		StartTime = UnityEngine.Random.Range((int)MinTime, (int)MaxTime);
		PlayersCount = settings.PlayersCount;
		ExfiltrationTime = settings.ExfiltrationTime;
		ExfiltrationType = settings.ExfiltrationType;
		Chance = settings.Chance;
	}
}
