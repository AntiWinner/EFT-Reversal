using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EFT.Interactive;

public sealed class LootPoint_LEGACY : MonoBehaviour
{
	[Serializable]
	public sealed class WeightedSpawnPosition
	{
		public Transform Transform;

		public float Weight;
	}

	[SerializeField]
	private List<WeightedSpawnPosition> _groupPositions = new List<WeightedSpawnPosition>();

	public LootPointParameters Settings;

	public List<WeightedSpawnPosition> GroupPositions => _groupPositions;

	private void OnValidate()
	{
		Debug.LogError(_ED3E._E000(211918));
	}

	public void Convert()
	{
		Debug.Log(_ED3E._E000(211947) + base.gameObject.name + _ED3E._E000(59476));
		LootPoint lootPoint = base.gameObject.AddComponent<LootPoint>();
		LootPointParameters parameters = AsLootPointParameters();
		lootPoint.ApplyLootPointParameters(parameters);
		foreach (WeightedSpawnPosition groupPosition in GroupPositions)
		{
			UnityEngine.Object.DestroyImmediate(groupPosition.Transform.gameObject);
		}
		UnityEngine.Object.DestroyImmediate(this);
		Debug.Log(_ED3E._E000(144933));
	}

	public LootPointParameters AsLootPointParameters()
	{
		Settings.Position = ClassVector3.FromUnityVector3(base.transform.position);
		Settings.Rotation = ClassVector3.FromUnityVector3(base.transform.rotation.eulerAngles);
		Settings.FilterInclusive = Settings.FilterInclusive.Except(new string[2]
		{
			_ED3E._E000(211548),
			_ED3E._E000(211525)
		}).ToArray();
		Settings.GroupPositions = _groupPositions.Select((WeightedSpawnPosition x) => new WeightedLootPointSpawnPosition(x.Transform.position, x.Transform.rotation.eulerAngles, x.Weight, x.Transform.name)).ToArray();
		return Settings;
	}
}
