using System;
using System.Collections.Generic;
using EFT.Hideout;

namespace EFT;

[Serializable]
public class HideoutInfo
{
	public Dictionary<string, ProductionData> Production;

	public Dictionary<string, _E81D> Improvements;

	public AreaInfo[] Areas;

	public HideoutCounters HideoutCounters;

	public int Seed;
}
