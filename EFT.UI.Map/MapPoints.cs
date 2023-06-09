using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Interactive;
using UnityEngine;

namespace EFT.UI.Map;

public class MapPoints : ScriptableObject
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ExfiltrationPoint point;

		internal bool _E000(EntryPoint x)
		{
			return x.OpenExtractionPoints.Contains(point.Settings.Name);
		}
	}

	public List<EntryPoint> EntryPoints;

	public List<ExtractionPoint> ExtractionPoints;

	private const string m__E000 = "MapPointConfigs/";

	private static readonly Dictionary<string, MapPoints> _E001 = new Dictionary<string, MapPoints>();

	public static MapPoints Load(string locationId)
	{
		if (!_E001.ContainsKey(locationId))
		{
			_E001.Add(locationId, _E3A2.Load<MapPoints>(_ED3E._E000(140841) + locationId));
		}
		return _E001[locationId];
	}

	public static void FillEligablePoints(ExfiltrationPoint point, string locationId)
	{
		MapPoints mapPoints = Load(locationId);
		if (mapPoints == null)
		{
			Debug.LogError(_ED3E._E000(233233));
			return;
		}
		point.EligibleEntryPoints = (from x in mapPoints.EntryPoints
			where x.OpenExtractionPoints.Contains(point.Settings.Name)
			select x.Name).ToArray();
	}
}
