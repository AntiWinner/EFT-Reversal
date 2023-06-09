using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace EFT.Hideout;

[RequireComponent(typeof(VideoPlayer))]
public sealed class VideoAmbiance : InteractiveAmbianceObject<VideoAmbiance.VideoPlaybackSettings>
{
	private sealed class _E000
	{
		public VideoClip VideoClip;

		public float CurrentTime;

		public _E000(VideoClip clip)
		{
			VideoClip = clip;
			CurrentTime = 0f;
		}
	}

	[Serializable]
	public sealed class VideoPlaybackSettings
	{
		public List<VideoClip> VideoClips = new List<VideoClip>();

		public bool PlayerEnabled = true;

		public VideoClip GetRandomClip(VideoClip lastClip)
		{
			int count = VideoClips.Count;
			switch (count)
			{
			case 0:
				return null;
			case 1:
				return VideoClips[0];
			}
			VideoClip videoClip;
			do
			{
				videoClip = VideoClips[UnityEngine.Random.Range(0, count)];
			}
			while (videoClip != lastClip);
			return videoClip;
		}
	}

	private VideoPlayer m__E000;

	private Stopwatch m__E001 = Stopwatch.StartNew();

	private Dictionary<ELightStatus, _E000> m__E002 = new Dictionary<ELightStatus, _E000>();

	private void Awake()
	{
		this.m__E000 = GetComponent<VideoPlayer>();
		this.m__E000.loopPointReached += _E001;
		OnEnable();
	}

	protected override void OnEnable()
	{
		if (!(this.m__E000 == null))
		{
			base.OnEnable();
		}
	}

	protected override void OnDisable()
	{
		if (!(this.m__E000 == null))
		{
			base.OnDisable();
		}
	}

	private void _E000(VideoClip lastClip)
	{
		if (lastClip == null && this.m__E002.TryGetValue(base.PreviousStatus, out var value))
		{
			value.CurrentTime += (float)this.m__E001.ElapsedMilliseconds / 1000f;
		}
		if (Patterns.TryGetValue(base.CombinedStatus, out var value2))
		{
			this.m__E001 = Stopwatch.StartNew();
			if (!this.m__E002.TryGetValue(base.CombinedStatus, out var value3))
			{
				value3 = new _E000(value2.Value.GetRandomClip(lastClip));
				this.m__E002.Add(base.CombinedStatus, value3);
			}
			this.m__E000.enabled = value2.Value.PlayerEnabled;
			this.m__E000.targetTexture.Release();
			this.m__E000.clip = value3.VideoClip;
			if (value3.VideoClip != null)
			{
				this.m__E000.time = Mathf.Clamp(value3.CurrentTime, 0f, (float)value3.VideoClip.length);
			}
		}
	}

	private void _E001(VideoPlayer source)
	{
		if (this.m__E002.TryGetValue(base.CombinedStatus, out var value))
		{
			this.m__E002.Remove(base.CombinedStatus);
			_E000(value.VideoClip);
		}
	}

	public override async Task<bool> PerformInteraction(ELightStatus status)
	{
		if (!(await base.PerformInteraction(status)) || this.m__E000 == null)
		{
			return false;
		}
		_E000(null);
		return true;
	}

	private void OnDestroy()
	{
		this.m__E000.loopPointReached -= _E001;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<bool> _E002(ELightStatus status)
	{
		return base.PerformInteraction(status);
	}
}
