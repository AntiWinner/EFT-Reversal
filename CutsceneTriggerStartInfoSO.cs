using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneTriggerStartInfoSO", menuName = "ScriptableObjects/CutsceneTriggerStartInfoSO")]
public class CutsceneTriggerStartInfoSO : ScriptableObject
{
	[Serializable]
	public class SceneCutscenesInfoRelation
	{
		public string sceneName;

		public List<CutsceneIDWithStartValues> cutsceneTriggersStartInfo = new List<CutsceneIDWithStartValues>();
	}

	[Serializable]
	public class CutsceneIDWithStartValues
	{
		public int cutsceneID;

		public StartPlayerValues startValues;
	}

	[Serializable]
	public class StartPlayerValues
	{
		public Vector3 startPosition;

		public Vector3 startViewDirection;

		public float startPlayerPosLevel;

		public bool needToProneAtStart;

		public Vector3 cutsceneEndPlayerPos;
	}

	private static CutsceneTriggerStartInfoSO m__E000;

	[SerializeField]
	private List<SceneCutscenesInfoRelation> infoByScenes = new List<SceneCutscenesInfoRelation>();

	private Dictionary<string, Dictionary<int, StartPlayerValues>> _E001 = new Dictionary<string, Dictionary<int, StartPlayerValues>>();

	private Dictionary<int, Dictionary<int, StartPlayerValues>> _E002 = new Dictionary<int, Dictionary<int, StartPlayerValues>>();

	public static CutsceneTriggerStartInfoSO Instance
	{
		get
		{
			if (CutsceneTriggerStartInfoSO.m__E000 == null)
			{
				CutsceneTriggerStartInfoSO.m__E000 = Resources.Load<CutsceneTriggerStartInfoSO>(_ED3E._E000(53628));
				CutsceneTriggerStartInfoSO.m__E000._E000();
			}
			return CutsceneTriggerStartInfoSO.m__E000;
		}
	}

	private void _E000()
	{
		_E001.Clear();
		foreach (SceneCutscenesInfoRelation infoByScene in infoByScenes)
		{
			if (!_E001.ContainsKey(infoByScene.sceneName))
			{
				_E001[infoByScene.sceneName] = new Dictionary<int, StartPlayerValues>();
			}
			foreach (CutsceneIDWithStartValues item in infoByScene.cutsceneTriggersStartInfo)
			{
				_E001[infoByScene.sceneName].Add(item.cutsceneID, item.startValues);
			}
		}
		_E002.Clear();
		foreach (SceneCutscenesInfoRelation infoByScene2 in infoByScenes)
		{
			int hashCode = infoByScene2.sceneName.GetHashCode();
			if (!_E002.ContainsKey(hashCode))
			{
				_E002[hashCode] = new Dictionary<int, StartPlayerValues>();
			}
			foreach (CutsceneIDWithStartValues item2 in infoByScene2.cutsceneTriggersStartInfo)
			{
				_E002[hashCode].Add(item2.cutsceneID, item2.startValues);
			}
		}
	}

	public StartPlayerValues GetPlayerStartInfo(string sceneName, int cutsceneID)
	{
		if (_E001.TryGetValue(sceneName, out var value) && value.TryGetValue(cutsceneID, out var value2))
		{
			return value2;
		}
		return null;
	}

	public StartPlayerValues GetPlayerStartInfo(int sceneNameHash, int cutsceneID)
	{
		if (_E002.TryGetValue(sceneNameHash, out var value) && value.TryGetValue(cutsceneID, out var value2))
		{
			return value2;
		}
		return null;
	}
}
