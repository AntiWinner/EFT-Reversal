using System;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;
using UnityEngine.Audio;

public class SourceOccluder : MonoBehaviour
{
	public AudioSource[] _sources;

	private int m__E000;

	public int UpdateEveryFrames = 20;

	public Action<AudioSource, Vector3> OutputSwitcher;

	public AudioMixerGroup CustomOutput;

	public AudioMixerGroup CustomOutputOccluded;

	private Action m__E001;

	private void Start()
	{
		OutputSwitcher = ((CustomOutput == null || CustomOutputOccluded == null) ? new Action<AudioSource, Vector3>(_E002) : new Action<AudioSource, Vector3>(_E001));
		if (!(_E8A8.Instance.Camera != null))
		{
			_E8A8.Instance.OnCameraChanged += _E000;
			base.enabled = false;
			this.m__E001 = delegate
			{
				_E8A8.Instance.OnCameraChanged -= _E000;
				this.m__E001 = null;
			};
		}
	}

	private void _E000()
	{
		base.enabled = true;
		this.m__E001?.Invoke();
	}

	private void _E001(AudioSource source, Vector3 vector3)
	{
		if (Singleton<BetterAudio>.Instantiated)
		{
			bool flag = _E486.SimpleOcclusionTest(vector3, _E8A8.Instance.Camera.transform.position, _E8A8.Instance.Distance(vector3), _E486.HighPolyOcclusionMask);
			source.outputAudioMixerGroup = (flag ? CustomOutputOccluded : CustomOutput);
		}
	}

	private void _E002(AudioSource source, Vector3 vector3)
	{
		if (Singleton<BetterAudio>.Instantiated)
		{
			source.outputAudioMixerGroup = Singleton<BetterAudio>.Instance.GetOcclusionGroupSimple(vector3);
		}
	}

	private void OnDestroy()
	{
		OutputSwitcher = null;
		this.m__E001?.Invoke();
	}

	public virtual void Update()
	{
		this.m__E000 = (this.m__E000 + 1) % UpdateEveryFrames;
		for (int i = this.m__E000; i < _sources.Length; i += UpdateEveryFrames)
		{
			AudioSource audioSource = _sources[i];
			if (audioSource.isActiveAndEnabled)
			{
				Vector3 position = audioSource.transform.position;
				OutputSwitcher(audioSource, position);
			}
		}
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E8A8.Instance.OnCameraChanged -= _E000;
		this.m__E001 = null;
	}
}
