using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace EFT;

public sealed class AudioArray : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public List<AudioSource> sources;

		public AudioClip sequenceClip;

		public bool cancelled;

		internal void _E000(float time, bool init)
		{
			foreach (AudioSource source in sources)
			{
				if (init)
				{
					source.DOKill();
				}
				if (source == null)
				{
					break;
				}
				if (init && source.clip != sequenceClip)
				{
					source.loop = false;
					source.clip = sequenceClip;
				}
				source.time = time;
				if (init)
				{
					source.Play();
				}
			}
		}

		internal void _E001()
		{
			cancelled = true;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public bool cancelled;

		internal void _E000()
		{
			cancelled = true;
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public bool isVolumetric;

		public AudioArray _003C_003E4__this;

		public Action onCancelCallback;

		public Action originalCallback;

		internal void _E000(bool volumetric)
		{
			// Found self-referencing delegate construction. Abort transformation to avoid stack overflow.
			if (isVolumetric == volumetric)
			{
				_003C_003E4__this._E000 -= _E000;
				onCancelCallback?.Invoke();
				originalCallback?.Invoke();
			}
		}
	}

	[SerializeField]
	private AudioSource _audioSource2D;

	private List<AudioSource> m__E001 = new List<AudioSource>();

	[CompilerGenerated]
	private Action<bool> m__E002;

	private event Action<bool> _E000
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = this.m__E002;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = this.m__E002;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Awake()
	{
		Init();
	}

	public void OnDisable()
	{
		this.m__E002?.Invoke(obj: true);
		this.m__E002?.Invoke(obj: false);
	}

	public void OnDestroy()
	{
		this.m__E002?.Invoke(obj: true);
		this.m__E002?.Invoke(obj: false);
	}

	public void Init(float? overlap = null)
	{
		if (this.m__E001.Count == 0)
		{
			this.m__E001 = (from source in base.gameObject.GetComponentsInChildren<AudioSource>()
				where source != _audioSource2D
				select source).ToList();
		}
	}

	public async void PlaySequence(AudioSequence sequence, EAudioSequenceType sequenceType, bool volumetric, Action onCancel = null)
	{
		_E000 CS_0024_003C_003E8__locals0 = new _E000();
		Stop(volumetric);
		if (sequence == null || sequence.SequenceClip == null)
		{
			return;
		}
		CS_0024_003C_003E8__locals0.sources = _E001(volumetric);
		if (CS_0024_003C_003E8__locals0.sources.Count == 0)
		{
			return;
		}
		CS_0024_003C_003E8__locals0.sequenceClip = sequence.SequenceClip;
		if (CS_0024_003C_003E8__locals0.sequenceClip.loadState != AudioDataLoadState.Loaded)
		{
			CS_0024_003C_003E8__locals0.sequenceClip.LoadAudioData();
		}
		AudioSequence._E000 sequenceSettings = sequence.GetSequenceSettings(sequenceType);
		bool flag = sequenceSettings.LoopLength >= double.Epsilon;
		if (sequenceSettings.Delay >= float.Epsilon)
		{
			await Task.Delay((int)(sequenceSettings.Delay * 1000f));
		}
		CS_0024_003C_003E8__locals0.cancelled = false;
		_E000(volumetric, delegate
		{
			CS_0024_003C_003E8__locals0.cancelled = true;
		}, onCancel);
		CS_0024_003C_003E8__locals0._E000((float)sequenceSettings.StartTime, init: true);
		if (sequenceSettings.SkipTime >= double.Epsilon)
		{
			await Task.Delay((int)Math.Ceiling(sequenceSettings.SkipTime));
			if (CS_0024_003C_003E8__locals0.cancelled)
			{
				return;
			}
			CS_0024_003C_003E8__locals0._E000((float)sequenceSettings.JumpTime, init: false);
		}
		Stopwatch stopwatch = Stopwatch.StartNew();
		await Task.Delay((int)Math.Ceiling(sequenceSettings.InitialLength * 1000.0));
		if (flag)
		{
			int value = (int)((double)stopwatch.ElapsedMilliseconds - sequenceSettings.InitialLength * 1000.0);
			int num = (int)(sequenceSettings.LoopLength * 1000.0);
			while (!CS_0024_003C_003E8__locals0.cancelled)
			{
				value = Mathf.Clamp(value, 0, num);
				CS_0024_003C_003E8__locals0._E000((float)(sequenceSettings.LoopStartTime + (double)value / 1000.0), init: false);
				stopwatch.Restart();
				await Task.Delay(num - value);
				value = (int)(stopwatch.ElapsedMilliseconds + value - num);
			}
		}
	}

	public async void PlayWithOffset(AudioClip sound, bool volumetric, float offset, Action onFinish = null, Action onCancel = null)
	{
		Stop(volumetric);
		if (sound == null)
		{
			return;
		}
		bool cancelled = false;
		_E000(volumetric, delegate
		{
			cancelled = true;
		}, onCancel);
		List<AudioSource> list = _E001(volumetric);
		if (list.Count == 0)
		{
			return;
		}
		offset = Mathf.Clamp(offset, 0f, sound.length);
		foreach (AudioSource item in list)
		{
			item.clip = sound;
			item.loop = false;
			item.time = offset;
			item.Play();
		}
		await Task.Delay((int)((sound.length - offset) * 1000f));
		if (!cancelled)
		{
			onFinish?.Invoke();
		}
	}

	public async void PlayOneShot(AudioClip sound, bool volumetric, Action onFinish = null)
	{
		await PlayOneShotAsync(sound, volumetric, onFinish);
	}

	public async Task PlayOneShotAsync(AudioClip sound, bool volumetric, Action onFinish = null)
	{
		if (sound == null)
		{
			return;
		}
		List<AudioSource> list = _E001(volumetric);
		if (list.Count == 0)
		{
			return;
		}
		foreach (AudioSource item in list)
		{
			item.PlayOneShot(sound);
		}
		await Task.Delay((int)(sound.length * 1000f));
		onFinish?.Invoke();
	}

	public void PlayLoop(AudioClip sound, bool volumetric)
	{
		Stop(volumetric);
		if (sound == null)
		{
			return;
		}
		List<AudioSource> list = _E001(volumetric);
		if (list.Count == 0)
		{
			return;
		}
		foreach (AudioSource item in list)
		{
			item.clip = sound;
			item.loop = true;
			item.Play();
		}
	}

	private void _E000(bool isVolumetric, Action onCancelCallback, Action originalCallback = null)
	{
		_E006 CS_0024_003C_003E8__locals0 = new _E006();
		CS_0024_003C_003E8__locals0.isVolumetric = isVolumetric;
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.onCancelCallback = onCancelCallback;
		CS_0024_003C_003E8__locals0.originalCallback = originalCallback;
		this._E000 += delegate(bool volumetric)
		{
			// Found self-referencing delegate construction. Abort transformation to avoid stack overflow.
			if (CS_0024_003C_003E8__locals0.isVolumetric == volumetric)
			{
				CS_0024_003C_003E8__locals0._003C_003E4__this._E000 -= CS_0024_003C_003E8__locals0._E000;
				CS_0024_003C_003E8__locals0.onCancelCallback?.Invoke();
				CS_0024_003C_003E8__locals0.originalCallback?.Invoke();
			}
		};
	}

	public void Stop(bool volumetric)
	{
		this.m__E002?.Invoke(volumetric);
		foreach (AudioSource item in _E001(volumetric))
		{
			item.Stop();
		}
	}

	private List<AudioSource> _E001(bool volumetric)
	{
		if (volumetric)
		{
			return this.m__E001;
		}
		if (_audioSource2D != null)
		{
			return new List<AudioSource> { _audioSource2D };
		}
		return new List<AudioSource>();
	}

	[CompilerGenerated]
	private bool _E002(AudioSource source)
	{
		return source != _audioSource2D;
	}
}
