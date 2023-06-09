using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.Hideout;

public sealed class HideoutAreasInfo : SerializedScriptableObject
{
	[SerializeField]
	private Dictionary<EAreaType, AreaTemplate> _areas;

	public IEnumerable<AreaTemplate> Templates => _areas.Values;

	public static async void Load()
	{
		await LoadAsync();
	}

	public static async Task<HideoutAreasInfo> LoadAsync()
	{
		ResourceRequest resourceRequest = Resources.LoadAsync<HideoutAreasInfo>(_ED3E._E000(165603));
		await resourceRequest.Await();
		return (HideoutAreasInfo)resourceRequest.asset;
	}
}
