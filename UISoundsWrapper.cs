using System;
using System.Collections.Generic;
using EFT.UI;
using JetBrains.Annotations;
using NLog;
using UnityEngine;

public class UISoundsWrapper : ScriptableObject
{
	[Serializable]
	public class UISound : SoundElement<EUISoundType>
	{
	}

	[Serializable]
	public class SotialNetworkSound : SoundElement<ESocialNetworkSoundType>
	{
	}

	[Serializable]
	public class EndGameSound : SoundElement<EEndGameSoundType>
	{
	}

	[Serializable]
	public abstract class SoundElement<TSoundTypeEnum>
	{
		[SerializeField]
		private TSoundTypeEnum _soundType;

		[SerializeField]
		private AudioClip _sound;

		public TSoundTypeEnum TSoundType => _soundType;

		public AudioClip TSound => _sound;
	}

	[Serializable]
	public class UIClipSounds
	{
		[SerializeField]
		private AudioClip[] _audioClips;

		public AudioClip GetRandomClip()
		{
			AudioClip[] audioClips = _audioClips;
			if (audioClips == null)
			{
				return null;
			}
			return audioClips[UnityEngine.Random.Range(0, _audioClips.Length - 1)];
		}
	}

	private class _E000 : _E315
	{
		public _E000()
			: base(LogManager.GetLogger(_ED3E._E000(48908)), LoggerMode.Add)
		{
		}

		public void LogMissingSound(Enum soundType)
		{
			LogError(_ED3E._E000(48902), soundType);
		}
	}

	[SerializeField]
	private UISound[] _UIAudioClips;

	[SerializeField]
	private SotialNetworkSound[] _socialNetworkAudioClips;

	[SerializeField]
	private EndGameSound[] _endGameAudioClips;

	[SerializeField]
	private UIClipSounds _loadSounds;

	[SerializeField]
	private UIClipSounds _unloadSounds;

	private static readonly _E000 m__E000 = new _E000();

	private readonly Dictionary<EUISoundType, AudioClip> _E001 = new Dictionary<EUISoundType, AudioClip>(_E3A5<EUISoundType>.EqualityComparer);

	private readonly Dictionary<ESocialNetworkSoundType, AudioClip> _E002 = new Dictionary<ESocialNetworkSoundType, AudioClip>(_E3A5<ESocialNetworkSoundType>.EqualityComparer);

	private readonly Dictionary<EEndGameSoundType, AudioClip> _E003 = new Dictionary<EEndGameSoundType, AudioClip>(_E3A5<EEndGameSoundType>.EqualityComparer);

	public UIClipSounds LoadSounds => _loadSounds;

	public UIClipSounds UnloadSounds => _unloadSounds;

	[CanBeNull]
	public AudioClip GetSocialNetworkClip(ESocialNetworkSoundType soundType)
	{
		if (!_E002.ContainsKey(soundType))
		{
			UISoundsWrapper.m__E000.LogMissingSound(soundType);
			return null;
		}
		return _E002[soundType];
	}

	[CanBeNull]
	public AudioClip GetUIClip(EUISoundType soundType)
	{
		if (!_E001.ContainsKey(soundType))
		{
			UISoundsWrapper.m__E000.LogMissingSound(soundType);
			return null;
		}
		return _E001[soundType];
	}

	[CanBeNull]
	public AudioClip GetEndGameClip(EEndGameSoundType soundType)
	{
		if (!_E003.ContainsKey(soundType))
		{
			UISoundsWrapper.m__E000.LogMissingSound(soundType);
			return null;
		}
		return _E003[soundType];
	}

	protected void OnEnable()
	{
		SoundElement<EUISoundType>[] uIAudioClips = _UIAudioClips;
		_E000(uIAudioClips, _E001);
		SoundElement<ESocialNetworkSoundType>[] socialNetworkAudioClips = _socialNetworkAudioClips;
		_E000(socialNetworkAudioClips, _E002);
		SoundElement<EEndGameSoundType>[] endGameAudioClips = _endGameAudioClips;
		_E000(endGameAudioClips, _E003);
	}

	private static void _E000<_E08C>(SoundElement<_E08C>[] clipList, Dictionary<_E08C, AudioClip> clipsIndex)
	{
		clipsIndex.Clear();
		if (clipList == null)
		{
			return;
		}
		foreach (SoundElement<_E08C> soundElement in clipList)
		{
			if (soundElement != null)
			{
				_E08C tSoundType = soundElement.TSoundType;
				if (!clipsIndex.ContainsKey(tSoundType))
				{
					clipsIndex.Add(tSoundType, soundElement.TSound);
				}
			}
		}
	}
}
