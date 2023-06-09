using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Cutscene;

[RequireComponent(typeof(PlayableDirector))]
public class BlendingDirector : MonoBehaviour
{
	public enum BlendType
	{
		None,
		In,
		Out,
		Seek
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public Action onBlendInFinished;

		public BlendingDirector _003C_003E4__this;

		internal void _E000()
		{
			onBlendInFinished?.Invoke();
			_003C_003E4__this._E00B = null;
			_003C_003E4__this.CurrentBlend = BlendType.None;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Action onFinished;

		public BlendingDirector _003C_003E4__this;

		internal void _E000()
		{
			onFinished?.Invoke();
			_003C_003E4__this.CurrentBlend = BlendType.None;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public Action onFinished;

		public BlendingDirector _003C_003E4__this;

		internal void _E000()
		{
			onFinished?.Invoke();
			_003C_003E4__this.CurrentBlend = BlendType.None;
		}
	}

	[SerializeField]
	private PlayableDirector _playableDirector;

	[CompilerGenerated]
	private BlendType m__E000;

	private AnimationPlayableOutput m__E001;

	private Playable m__E002;

	private Playable m__E003;

	private AnimationMixerPlayable m__E004;

	private int m__E005;

	private int _E006;

	private float _E007;

	private float _E008;

	private TrackAsset _E009;

	private double _E00A = -1.0;

	private Action _E00B;

	private float _E00C;

	private AnimationPlayableOutput _E00D;

	private int _E00E;

	private Stopwatch _E00F = new Stopwatch();

	private static bool _E010;

	private Coroutine _E011;

	private Coroutine _E012;

	private Coroutine _E013;

	private string _E014;

	public PlayableDirector PlayableDirector => _playableDirector;

	public bool IsPlaying => _playableDirector.state == PlayState.Playing;

	public BlendType CurrentBlend
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

	public bool IsBlending => CurrentBlend != BlendType.None;

	public bool IsBlendingOut => CurrentBlend == BlendType.Out;

	public bool IsBlendingIn => CurrentBlend == BlendType.In;

	public double TimeRemaining => _playableDirector.time - _playableDirector.duration;

	public string Name => _playableDirector.name;

	private void Awake()
	{
		_E014 = _ED3E._E000(71407) + GetInstanceID();
	}

	private void Update()
	{
		if (IsPlaying && CurrentBlend == BlendType.None && _E00A >= 0.0 && _playableDirector.time >= _E00A)
		{
			CurrentBlend = BlendType.Out;
			_E00F.Restart();
			if (_E011 != null)
			{
				StopCoroutine(_E011);
			}
			_E011 = StartCoroutine(_E002(_playableDirector, this.m__E001, _E00C, this.m__E001.GetWeight(), delegate
			{
				_E00F.Stop();
				_E00B?.Invoke();
				_E00B = null;
				CurrentBlend = BlendType.None;
			}));
		}
	}

	private void OnDestroy()
	{
		_E000();
	}

	private void OnDisable()
	{
		_E000();
	}

	private void _E000()
	{
		if (_E011 != null)
		{
			StopCoroutine(_E011);
			_E011 = null;
		}
		if (_E012 != null)
		{
			StopCoroutine(_E012);
			_E012 = null;
		}
		if (_E013 != null)
		{
			StopCoroutine(_E013);
			_E013 = null;
		}
	}

	private void _E001()
	{
		_playableDirector.Evaluate();
		if (_playableDirector.playableGraph.IsValid())
		{
			_E00E = 0;
			_E009 = (_playableDirector.playableAsset as TimelineAsset)?.GetOutputTrack(_E00E);
			_E00D = (AnimationPlayableOutput)_playableDirector.playableGraph.GetOutputByType<AnimationPlayableOutput>(_E00E);
			this.m__E002 = _E00D.GetSourcePlayable();
			this.m__E003 = _playableDirector.playableAsset.CreatePlayable(_playableDirector.playableGraph, _playableDirector.gameObject);
			this.m__E004 = AnimationMixerPlayable.Create(_playableDirector.playableGraph, 2);
			this.m__E005 = this.m__E004.AddInput(this.m__E003, 0);
			_E006 = this.m__E004.AddInput(this.m__E002, 0, 1f);
			if (_E00D.IsOutputValid() && _E00D.GetTarget() != null)
			{
				this.m__E001 = AnimationPlayableOutput.Create(_playableDirector.playableGraph, _E014, _E00D.GetTarget());
				this.m__E001.SetSourcePlayable(this.m__E004);
				this.m__E001.SetSourceOutputPort(_E00D.GetSourceOutputPort());
				this.m__E001.SetWeight(1f);
				_E00D.SetTarget(null);
			}
		}
	}

	public void Play(float blendInDuration = 0.5f, float blendOutDuration = 0.5f, float startTime = -1f, float endTime = -1f, Action onBlendInFinished = null, Action onFinished = null)
	{
		if (CurrentBlend != 0)
		{
			return;
		}
		CurrentBlend = BlendType.In;
		if (!this.m__E001.IsOutputValid())
		{
			_playableDirector.RebuildGraph();
			_E001();
		}
		if (!this.m__E001.IsOutputValid())
		{
			return;
		}
		_E00B = onFinished;
		_E00C = blendOutDuration;
		_E00A = ((endTime < 0f) ? (_E009.start + _E009.duration - (double)blendOutDuration) : ((double)Math.Max(0f, endTime - blendInDuration))) - (double)Time.deltaTime;
		if (blendInDuration == 0f)
		{
			_playableDirector.Play();
			return;
		}
		if (_E011 != null)
		{
			StopCoroutine(_E011);
		}
		_E011 = StartCoroutine(_E003(_playableDirector, this.m__E001, blendInDuration, startTime, delegate
		{
			onBlendInFinished?.Invoke();
			_E00B = null;
			CurrentBlend = BlendType.None;
		}));
	}

	public void Stop(float blendDuration = 0.5f, Action onFinished = null)
	{
		if (blendDuration <= 0f)
		{
			_playableDirector.Pause();
		}
		if (CurrentBlend != 0 && CurrentBlend != BlendType.In)
		{
			return;
		}
		if (CurrentBlend == BlendType.In)
		{
			_E010 = true;
		}
		CurrentBlend = BlendType.Out;
		if (!this.m__E001.IsOutputValid())
		{
			_playableDirector.RebuildGraph();
			_E001();
		}
		if (this.m__E001.IsOutputValid())
		{
			if (_E012 != null)
			{
				StopCoroutine(_E012);
			}
			_E012 = StartCoroutine(_E002(_playableDirector, this.m__E001, blendDuration, this.m__E001.GetWeight(), delegate
			{
				onFinished?.Invoke();
				CurrentBlend = BlendType.None;
			}));
		}
	}

	public void Seek(float toTime, float blendDuration, Action onFinished = null)
	{
		if (CurrentBlend != 0)
		{
			return;
		}
		CurrentBlend = BlendType.Seek;
		if (this.m__E001.IsOutputValid())
		{
			if (_E013 != null)
			{
				StopCoroutine(_E013);
			}
			_E013 = StartCoroutine(_E004(blendDuration, toTime, delegate
			{
				onFinished?.Invoke();
				CurrentBlend = BlendType.None;
			}));
		}
	}

	private IEnumerator _E002(PlayableDirector director, AnimationPlayableOutput output, float blendTime, float fromWeight = 1f, Action onFinished = null)
	{
		for (float num = blendTime - blendTime * fromWeight; num < blendTime; num += Time.deltaTime)
		{
			float value = 1f - Mathf.Clamp01(num / blendTime);
			if (!output.IsOutputValid())
			{
				break;
			}
			output.SetWeight(value);
			yield return null;
		}
		if (output.IsOutputValid())
		{
			output.SetWeight(0f);
		}
		onFinished?.Invoke();
		if (director.isActiveAndEnabled)
		{
			director.Pause();
		}
		_E012 = null;
	}

	private IEnumerator _E003(PlayableDirector director, AnimationPlayableOutput output, float blendTime, float startTime = -1f, Action onFinished = null)
	{
		director.time = ((startTime > 0f) ? startTime : 0f);
		director.Play();
		output.SetWeight(0f);
		_E010 = false;
		for (float num = 0f; num < blendTime; num += Time.deltaTime)
		{
			if (_E010)
			{
				_E010 = false;
				break;
			}
			float value = Mathf.Clamp01(num / blendTime);
			output.SetWeight(value);
			yield return null;
		}
		output.SetWeight(1f);
		onFinished?.Invoke();
		_E011 = null;
	}

	private IEnumerator _E004(float blendTime, float startTime = -1f, Action onFinished = null)
	{
		float num = 0f;
		this.m__E003.SetTime(_playableDirector.time);
		this.m__E003.Play();
		this.m__E001.SetWeight(1f);
		this.m__E004.SetInputWeight(this.m__E005, 1f);
		this.m__E004.SetInputWeight(_E006, 0f);
		_playableDirector.time = startTime;
		_playableDirector.Play();
		_playableDirector.Evaluate();
		for (; num < blendTime; num += Time.deltaTime)
		{
			float num2 = Mathf.Clamp01(num / blendTime);
			_E007 = 1f - num2;
			_E008 = num2;
			this.m__E004.SetInputWeight(this.m__E005, _E007);
			this.m__E004.SetInputWeight(_E006, _E008);
			yield return null;
		}
		this.m__E004.SetInputWeight(this.m__E005, 0f);
		this.m__E004.SetInputWeight(_E006, 1f);
		this.m__E003.Pause();
		onFinished?.Invoke();
		_E013 = null;
	}

	[CompilerGenerated]
	private void _E005()
	{
		_E00F.Stop();
		_E00B?.Invoke();
		_E00B = null;
		CurrentBlend = BlendType.None;
	}
}
