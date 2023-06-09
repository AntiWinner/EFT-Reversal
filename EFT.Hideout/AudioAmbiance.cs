using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace EFT.Hideout;

[RequireComponent(typeof(AudioArray))]
public sealed class AudioAmbiance : InteractiveAmbianceObject<IAudioPlaybackSettings>
{
	[Serializable]
	public sealed class MuteSettings : IAudioPlaybackSettings
	{
		public void Play(AudioArray audioArray, ELightStatus status, bool firstStart)
		{
			audioArray.Stop(volumetric: true);
		}

		public void Pause(AudioArray audioArray)
		{
		}
	}

	[Serializable]
	public sealed class SimpleAudioSettings : IAudioPlaybackSettings
	{
		[SerializeField]
		private AudioClip _audioClip;

		[SerializeField]
		private bool _loop;

		public void Play(AudioArray audioArray, ELightStatus status, bool firstStart)
		{
			audioArray.Stop(volumetric: true);
			if (_loop)
			{
				audioArray.PlayLoop(_audioClip, volumetric: true);
			}
			else
			{
				audioArray.PlayOneShot(_audioClip, volumetric: true);
			}
		}

		public void Pause(AudioArray audioArray)
		{
		}
	}

	[Serializable]
	public sealed class PlaybackSettings : IAudioPlaybackSettings
	{
		[SerializeField]
		private AudioSequence _audioSequence;

		public AudioSequence AudioSequence => _audioSequence;

		public void Play(AudioArray audioArray, ELightStatus status, bool firstStart)
		{
			audioArray.Stop(volumetric: true);
			EAudioSequenceType sequenceType;
			switch (status)
			{
			default:
				return;
			case ELightStatus.Working:
				sequenceType = ((!firstStart) ? EAudioSequenceType.OnWorking : EAudioSequenceType.Working);
				break;
			case ELightStatus.OutOfFuel:
				sequenceType = (firstStart ? EAudioSequenceType.Disabled : EAudioSequenceType.OffDisabled);
				break;
			}
			audioArray.PlaySequence(AudioSequence, sequenceType, volumetric: true);
		}

		public void Pause(AudioArray audioArray)
		{
		}
	}

	[Serializable]
	public sealed class ContinuingPlaybackSettings : IAudioPlaybackSettings
	{
		[CompilerGenerated]
		private sealed class _E000
		{
			public ContinuingPlaybackSettings _003C_003E4__this;

			public AudioArray audioArray;

			public ELightStatus status;

			internal void _E000()
			{
				_003C_003E4__this._currentTime = 0f;
				_003C_003E4__this._currentClip = _003C_003E4__this.TrackList.GetRandomItem(_003C_003E4__this._currentClip);
				_003C_003E4__this.Play(audioArray, status, firstStart: false);
			}
		}

		public List<AudioClip> TrackList = new List<AudioClip>();

		private float _currentTime;

		private Stopwatch _stopwatch = Stopwatch.StartNew();

		private AudioClip _currentClip;

		public void Pause(AudioArray audioArray)
		{
			_currentTime += ((float?)_stopwatch?.ElapsedMilliseconds / 1000f) ?? 0f;
		}

		public void Play(AudioArray audioArray, ELightStatus status, bool firstStart)
		{
			audioArray.Stop(volumetric: true);
			_stopwatch = Stopwatch.StartNew();
			if (_currentClip == null || firstStart)
			{
				_currentTime = 0f;
				_currentClip = TrackList.GetRandomItem();
			}
			if (TrackList.Count > 0)
			{
				audioArray.PlayWithOffset(_currentClip, volumetric: true, _currentTime, delegate
				{
					_currentTime = 0f;
					_currentClip = TrackList.GetRandomItem(_currentClip);
					Play(audioArray, status, firstStart: false);
				});
			}
		}
	}

	private AudioArray m__E000;

	private void Awake()
	{
		this.m__E000 = GetComponent<AudioArray>();
		this.m__E000.Init();
		OnEnable();
	}

	protected override void OnEnable()
	{
		if (this.m__E000 != null)
		{
			base.OnEnable();
		}
	}

	public override async Task<bool> PerformInteraction(ELightStatus status)
	{
		if (!(await base.PerformInteraction(status)) || this.m__E000 == null || !Patterns.TryGetValue(base.CombinedStatus, out var value))
		{
			return false;
		}
		if (Patterns.TryGetValue(base.PreviousStatus, out var value2))
		{
			value2.Value.Pause(this.m__E000);
		}
		value.Value.Play(this.m__E000, base.CombinedStatus, base.PreviousStatus == ELightStatus.None);
		return true;
	}

	internal void OnDestroy()
	{
		if (!(this.m__E000 == null))
		{
			this.m__E000.Stop(volumetric: true);
			this.m__E000.Stop(volumetric: false);
		}
	}

	[DebuggerHidden]
	[CompilerGenerated]
	private Task<bool> _E000(ELightStatus status)
	{
		return base.PerformInteraction(status);
	}
}
