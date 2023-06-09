using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Timeline;

namespace Cutscene;

public class CutsceneDirectorBlender : MonoBehaviour
{
	[SerializeField]
	private List<BlendingDirector> _blendingDirectors = new List<BlendingDirector>();

	[SerializeField]
	[Header("Debug test loop blending")]
	private bool _playAll;

	[CompilerGenerated]
	private BlendingDirector m__E000;

	private Dictionary<TimelineAsset, BlendingDirector> _E001;

	private Queue<BlendingDirector> _E002 = new Queue<BlendingDirector>();

	public bool IsTimelinePlaying
	{
		get
		{
			if (_E002.Count <= 0)
			{
				return _E001.Values.Any((BlendingDirector d) => d.IsPlaying);
			}
			return true;
		}
	}

	public BlendingDirector Current
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		set
		{
			this.m__E000 = value;
		}
	}

	public void Start()
	{
		_E001.Clear();
		foreach (BlendingDirector blendingDirector in _blendingDirectors)
		{
			_E001.Add(blendingDirector.PlayableDirector.playableAsset as TimelineAsset, blendingDirector);
		}
	}

	public void Update()
	{
		if (_playAll)
		{
			_playAll = false;
			_E002 = new Queue<BlendingDirector>(_blendingDirectors);
		}
		if (_E002 != null && _E002.Count != 0 && (Current == null || !Current.IsPlaying || (Current.IsBlendingOut && _E002.Any())))
		{
			Current = _E002.Dequeue();
			Current.Play();
		}
	}

	public void Play(TimelineAsset timeline, bool playImmediately = true, float transitionTime = 0.5f)
	{
		if (timeline == null || !_E001.ContainsKey(timeline))
		{
			return;
		}
		BlendingDirector blendingDirector = _E001[timeline];
		_E002.Clear();
		if (Current == null)
		{
			_E002.Enqueue(blendingDirector);
			return;
		}
		if (Current == blendingDirector && Current.IsPlaying)
		{
			Current.Seek(0f, transitionTime);
			return;
		}
		_E002.Enqueue(blendingDirector);
		if (playImmediately && Current.IsPlaying)
		{
			Current.Stop(transitionTime);
		}
	}

	public void Play(List<TimelineAsset> timelineSequence, bool playImmediately = true, float transitionTime = 0.5f)
	{
		_E002.Clear();
		foreach (TimelineAsset item in timelineSequence.Where((TimelineAsset t) => _E001.ContainsKey(t)))
		{
			_E002.Enqueue(_E001[item]);
		}
		if (playImmediately && Current != null && !Current.IsBlendingOut)
		{
			Current.Stop(transitionTime);
		}
	}

	[CompilerGenerated]
	private bool _E000(TimelineAsset t)
	{
		return _E001.ContainsKey(t);
	}
}
