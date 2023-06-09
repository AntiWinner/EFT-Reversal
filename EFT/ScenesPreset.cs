using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace EFT;

public class ScenesPreset : ScriptableObject
{
	[Serializable]
	public class Guid : ISceneResource
	{
		public string guid;

		[SerializeField]
		private bool _onlyOffline;

		public bool onlyOffline
		{
			get
			{
				return _onlyOffline;
			}
			set
			{
				_onlyOffline = value;
			}
		}

		public Guid(string guid, bool onlyOffline)
		{
			this.guid = guid;
			this.onlyOffline = onlyOffline;
		}
	}

	public Guid ActiveSceneGuid;

	public string ServerName;

	public List<Guid> ScenesGuids;

	private bool m__E000;

	public ScenesPreset[] ChildPresets;

	[SerializeField]
	[FormerlySerializedAs("ActiveSceneName")]
	private string _activeSceneName;

	[SerializeField]
	[FormerlySerializedAs("ScenesResourceKeys")]
	private List<SceneResourceKey> _scenesResourceKeys;

	public string ActiveSceneName => _activeSceneName;

	public string PresetName => base.name.Replace(_ED3E._E000(135139), string.Empty);

	public SceneResourceKey[] ScenesResourceKeys => _scenesResourceKeys.Where(_E000).ToArray();

	public void DisableServerScenes(bool val)
	{
		this.m__E000 = val;
		for (int i = 0; i < ChildPresets.Length; i++)
		{
			ChildPresets[i].m__E000 = val;
		}
	}

	private bool _E000(ISceneResource scene)
	{
		if (!this.m__E000)
		{
			return true;
		}
		return !scene.onlyOffline;
	}

	public int GetTotalSceneCount()
	{
		return ScenesGuids.Where(_E000).Count() + ChildPresets.Sum((ScenesPreset x) => x.GetTotalSceneCount());
	}
}
