using System.Collections.Generic;
using UnityEngine;

namespace Audio.SpatialSystem;

[RequireComponent(typeof(GuidComponent))]
public sealed class SpatialAudioCrossSceneGroup : MonoBehaviour
{
	public static readonly List<SpatialAudioCrossSceneGroup> AllCrossGroups = new List<SpatialAudioCrossSceneGroup>();

	[HideInInspector]
	public GuidReference crossSceneGroup;

	public List<SpatialAudioRoom> AudioRooms;

	public List<SpatialAudioPortal> AudioPortals;

	public void Awake()
	{
		AllCrossGroups.Add(this);
	}

	public void OnDestroy()
	{
		AllCrossGroups.Clear();
	}
}
