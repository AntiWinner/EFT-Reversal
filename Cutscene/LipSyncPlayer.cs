using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using uLipSync;
using UnityEngine;

namespace Cutscene;

public class LipSyncPlayer : MonoBehaviour
{
	private SkinnedMeshRenderer m__E000;

	private BakedData m__E001;

	private BakedDataWithCurves.CurveData[] m__E002;

	private List<uLipSyncBlendShape.BlendShapeInfo> m__E003;

	[SerializeField]
	private float minVolume = -2.5f;

	[SerializeField]
	private float maxVolume = -1.5f;

	[SerializeField]
	private float smoothness = 0.05f;

	[SerializeField]
	private float timeOffset = 0.1f;

	[SerializeField]
	private float playVolume = 1f;

	private AudioClip m__E004;

	private AudioSource m__E005;

	private Animator m__E006;

	private bool m__E007 = true;

	private float m__E008;

	private float m__E009;

	private bool m__E00A;

	private float m__E00B;

	private float _E00C = 1f / 60f;

	private float _E00D;

	private Dictionary<string, int> _E00E;

	private Action<string> _E00F;

	private Action _E010;

	private Coroutine _E011;

	private LipSyncInfo _E012;

	[CompilerGenerated]
	private Action _E013;

	[CompilerGenerated]
	private Action _E014;

	[CompilerGenerated]
	private static Action<string> _E015;

	[CompilerGenerated]
	private bool _E016;

	public bool IsPlaying
	{
		[CompilerGenerated]
		get
		{
			return _E016;
		}
		[CompilerGenerated]
		private set
		{
			_E016 = value;
		}
	}

	public bool IsAllowPlaySound
	{
		get
		{
			return this.m__E007;
		}
		set
		{
			this.m__E007 = value;
			if (!this.m__E007)
			{
				_E008();
			}
		}
	}

	public event Action OnStartPlay
	{
		[CompilerGenerated]
		add
		{
			Action action = _E013;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E013, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E013;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E013, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnStopPlay
	{
		[CompilerGenerated]
		add
		{
			Action action = _E014;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E014, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E014;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E014, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<string> TestOnStartPlay
	{
		[CompilerGenerated]
		add
		{
			Action<string> action = _E015;
			Action<string> action2;
			do
			{
				action2 = action;
				Action<string> value2 = (Action<string>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E015, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<string> action = _E015;
			Action<string> action2;
			do
			{
				action2 = action;
				Action<string> value2 = (Action<string>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E015, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void ApplySettings(GameObject source, GameObject target)
	{
		uLipSyncBlendShape component = source.GetComponent<uLipSyncBlendShape>();
		if (!(component == null) && !(component.skinnedMeshRenderer == null))
		{
			this.m__E003 = component.blendShapes;
			minVolume = component.minVolume;
			maxVolume = component.maxVolume;
			smoothness = component.smoothness;
			component.updateMethod = UpdateMethod.External;
			this.m__E000 = base.transform.Find(component.skinnedMeshRenderer.gameObject.name).GetComponent<SkinnedMeshRenderer>();
			uLipSyncBakedDataPlayer component2 = source.GetComponent<uLipSyncBakedDataPlayer>();
			if (component2 != null)
			{
				playVolume = component2.volume;
				timeOffset = component2.timeOffset;
				component2.playOnAwake = false;
				this.m__E005 = component2.audioSource;
			}
			this.m__E006 = target.GetComponent<Animator>();
			if (this.m__E005 == null)
			{
				this.m__E005 = target.GetComponent<AudioSource>();
			}
			this.m__E00A = this.m__E006 != null && this.m__E003 != null;
		}
	}

	public void StartPlay(BakedData bakedData)
	{
		if (this.m__E00A)
		{
			bool num = StopPlay();
			this.m__E002 = Array.Empty<BakedDataWithCurves.CurveData>();
			if (bakedData is BakedDataWithCurves bakedDataWithCurves && bakedDataWithCurves.curves != null)
			{
				this.m__E002 = bakedDataWithCurves.curves.ToArray();
			}
			this.m__E001 = bakedData;
			this.m__E004 = bakedData.audioClip;
			_E00F?.Invoke(this.m__E004.name);
			_E00F = null;
			if (num)
			{
				this.m__E009 = 0f;
				this.m__E008 = 0f;
			}
			_E00D = 0f;
			this.m__E00B = 0f;
			IsPlaying = true;
			if (IsAllowPlaySound)
			{
				_E005();
			}
			_E00A();
			UpdatePlay(0f);
			_E013?.Invoke();
			_E015?.Invoke(bakedData.name);
		}
	}

	public void UpdatePlay(float normalizedTime)
	{
		if (this.m__E00A)
		{
			if (Math.Abs(this.m__E00B - normalizedTime) > Mathf.Epsilon)
			{
				float num = Mathf.Abs(this.m__E00B - normalizedTime);
				_E00C = num * this.m__E001.duration;
				this.m__E00B = normalizedTime;
			}
			else
			{
				_E00C = Time.deltaTime;
			}
			BakedFrame frame = this.m__E001.GetFrame(normalizedTime * this.m__E001.duration);
			frame.volume *= playVolume;
			LipSyncInfo lipSyncInfo = BakedData.GetLipSyncInfo(frame);
			_E009(normalizedTime);
			ApplyBakeFrame(lipSyncInfo);
			_E00D = this.m__E001.duration * normalizedTime;
		}
	}

	private void LateUpdate()
	{
		if (_E012.phonemeRatios != null)
		{
			_E000();
			_E001();
			_E002();
			if (Mathf.Approximately(this.m__E008, 0f))
			{
				_E003();
				_E012 = default(LipSyncInfo);
			}
		}
	}

	public void ApplyBakeFrame(LipSyncInfo info)
	{
		_E012 = info;
	}

	public void ContinueDetachedPlay()
	{
		_E011 = StartCoroutine(_E007(_E00D));
	}

	public bool StopPlay()
	{
		if (!this.m__E00A)
		{
			return false;
		}
		bool result = false;
		if (_E011 != null)
		{
			StopCoroutine(_E011);
			_E011 = null;
			result = true;
		}
		if (_E010 != null)
		{
			_E010();
			_E010 = null;
		}
		if (IsPlaying)
		{
			_E014?.Invoke();
		}
		_E00A();
		_E008();
		_E00D = 0f;
		this.m__E00B = 0f;
		IsPlaying = false;
		return result;
	}

	private void _E000()
	{
		float target = 0f;
		if (_E012.rawVolume > 0f)
		{
			target = Mathf.Log10(_E012.rawVolume);
			target = (target - minVolume) / Mathf.Max(maxVolume - minVolume, 0.0001f);
			target = Mathf.Clamp(target, 0f, 1f);
		}
		this.m__E008 = _E004(this.m__E008, target, ref this.m__E009);
	}

	private void _E001()
	{
		float num = 0f;
		Dictionary<string, float> phonemeRatios = _E012.phonemeRatios;
		foreach (uLipSyncBlendShape.BlendShapeInfo item in this.m__E003)
		{
			float value = 0f;
			if (phonemeRatios != null && !string.IsNullOrEmpty(item.phoneme))
			{
				phonemeRatios.TryGetValue(item.phoneme, out value);
			}
			float velocity = item.weightVelocity;
			item.weight = _E004(item.weight, value, ref velocity);
			item.weightVelocity = velocity;
			num += item.weight;
		}
		foreach (uLipSyncBlendShape.BlendShapeInfo item2 in this.m__E003)
		{
			item2.weight = ((num > 0f) ? (item2.weight / num) : 0f);
		}
	}

	private void _E002()
	{
		if (!this.m__E000)
		{
			return;
		}
		foreach (uLipSyncBlendShape.BlendShapeInfo item in this.m__E003)
		{
			if (item.index >= 0)
			{
				this.m__E000.SetBlendShapeWeight(item.index, 0f);
			}
		}
		foreach (uLipSyncBlendShape.BlendShapeInfo item2 in this.m__E003)
		{
			if (item2.index >= 0)
			{
				float blendShapeWeight = this.m__E000.GetBlendShapeWeight(item2.index);
				blendShapeWeight += item2.weight * item2.maxWeight * this.m__E008 * 100f;
				this.m__E000.SetBlendShapeWeight(item2.index, blendShapeWeight);
			}
		}
	}

	private void _E003()
	{
		foreach (uLipSyncBlendShape.BlendShapeInfo item in this.m__E003)
		{
			this.m__E000.SetBlendShapeWeight(item.index, 0f);
		}
	}

	private float _E004(float value, float target, ref float velocity)
	{
		return Mathf.SmoothDamp(value, target, ref velocity, smoothness);
	}

	private void _E005()
	{
		this.m__E005.clip = this.m__E004;
		this.m__E005.volume = playVolume;
		this.m__E005.loop = false;
		this.m__E005.PlayDelayed(0.01f);
	}

	private void _E006(AnimationTrack data, Action<string> onStart, Action onFinish)
	{
		_E00F = onStart;
		StartPlay(data.GetBackedDataForPlay());
		_E010 = onFinish;
		_E011 = StartCoroutine(_E007(0f));
	}

	private IEnumerator _E007(float startTime)
	{
		_E00D = startTime;
		float duration = this.m__E001.duration;
		double dspTime = AudioSettings.dspTime;
		while (_E00D < duration)
		{
			float normalizedTime = _E00D / duration;
			UpdatePlay(normalizedTime);
			_E00D = startTime + (float)(AudioSettings.dspTime - dspTime);
			yield return null;
		}
		_E011 = null;
		StopPlay();
	}

	private void _E008()
	{
		this.m__E005.Stop();
	}

	private void _E009(float normalizedTime)
	{
		if (this.m__E002 == null)
		{
			return;
		}
		BakedDataWithCurves.CurveData[] array = this.m__E002;
		foreach (BakedDataWithCurves.CurveData curveData in array)
		{
			float value = curveData.curve.Evaluate(normalizedTime * this.m__E001.duration);
			if (curveData.isBlendShape)
			{
				this.m__E000.SetBlendShapeWeight(curveData.blendShapeIndex, value);
			}
			else
			{
				this.m__E006.SetFloat(_E00B(curveData.paramName), value);
			}
		}
	}

	private void _E00A()
	{
		if (this.m__E002 == null)
		{
			return;
		}
		BakedDataWithCurves.CurveData[] array = this.m__E002;
		foreach (BakedDataWithCurves.CurveData curveData in array)
		{
			if (curveData.isBlendShape)
			{
				this.m__E000.SetBlendShapeWeight(curveData.blendShapeIndex, curveData.resetValue);
			}
			else
			{
				this.m__E006.SetFloat(_E00B(curveData.paramName), curveData.resetValue);
			}
		}
	}

	private int _E00B(string param)
	{
		if (_E00E == null)
		{
			_E00E = new Dictionary<string, int>();
		}
		if (_E00E.TryGetValue(param, out var value))
		{
			return value;
		}
		return _E00E[param] = Animator.StringToHash(param);
	}

	public static LipSyncPlayer GetPlayer(GameObject target)
	{
		LipSyncPlayer lipSyncPlayer = target.GetComponent<LipSyncPlayer>();
		if (lipSyncPlayer == null)
		{
			lipSyncPlayer = target.AddComponent<LipSyncPlayer>();
			lipSyncPlayer.ApplySettings(target, target);
		}
		return lipSyncPlayer;
	}

	public static void Play(AnimationTrack data, GameObject target, Action<string> onStart, Action onFinish)
	{
		GetPlayer(target)._E006(data, onStart, onFinish);
	}
}
