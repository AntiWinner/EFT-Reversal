using System;
using System.Collections.Generic;
using Comfort.Common;
using UnityEngine;

namespace EFT.NPC;

public class NPCAdditionalSoundPlayer : MonoBehaviour
{
	[Serializable]
	public struct AudioClipWithID
	{
		public string ID;

		public AudioClip clip;
	}

	[SerializeField]
	private NPCAnimationsEventReceiver _animationsEventReceiver;

	[SerializeField]
	private Transform _transformForPlaySounds;

	[SerializeField]
	private List<AudioClipWithID> _clips = new List<AudioClipWithID>();

	private Dictionary<string, AudioClip> m__E000 = new Dictionary<string, AudioClip>();

	private void Awake()
	{
		_animationsEventReceiver.OnNeedToPlaySomeSound += _E000;
		this.m__E000.Clear();
		foreach (AudioClipWithID clip in _clips)
		{
			this.m__E000[clip.ID] = clip.clip;
		}
	}

	private void OnDestroy()
	{
		if (_animationsEventReceiver != null)
		{
			_animationsEventReceiver.OnNeedToPlaySomeSound -= _E000;
		}
	}

	private void _E000(string soundID)
	{
		if (this.m__E000.TryGetValue(soundID, out var value))
		{
			Singleton<BetterAudio>.Instance.PlayAtPoint(_transformForPlaySounds.position, value, 10f, BetterAudio.AudioSourceGroupType.Environment, 15);
		}
	}
}
