using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceCulling : MonoBehaviour
{
	[Serializable]
	private struct AudioSourceData
	{
		public AudioSource AudioSource;

		public Transform Transform;

		public float SqrCullDistance;
	}

	private const int m__E000 = 10;

	private int _E001;

	[SerializeField]
	private List<AudioSourceData> _audioSources = new List<AudioSourceData>();

	private Transform _E002;

	private void Start()
	{
		_E002 = base.transform;
		StartCoroutine(_E000());
	}

	public void Register(AudioSource audioSource)
	{
		_audioSources.Add(new AudioSourceData
		{
			AudioSource = audioSource,
			Transform = audioSource.transform,
			SqrCullDistance = audioSource.maxDistance * audioSource.maxDistance
		});
		_E001 = Math.Min(_audioSources.Count, 10);
	}

	public void Remove(AudioSource audioSource)
	{
		for (int i = 0; i < _audioSources.Count; i++)
		{
			if (_audioSources[i].AudioSource == audioSource)
			{
				_audioSources.RemoveAt(i);
				break;
			}
		}
	}

	public void CleanRegisteredAudioSources()
	{
		_audioSources.Clear();
	}

	private IEnumerator _E000()
	{
		int num = 0;
		while (_audioSources.Count > 0)
		{
			for (int i = 0; i < _audioSources.Count; i++)
			{
				AudioSourceData audioSourceData = _audioSources[i];
				float num2 = Vector3.SqrMagnitude(audioSourceData.Transform.position - _E002.position);
				audioSourceData.AudioSource.enabled = num2 <= audioSourceData.SqrCullDistance;
				num++;
				if (num > _E001)
				{
					num = 0;
					yield return null;
				}
			}
		}
	}
}
