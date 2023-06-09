using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.Hideout;

public sealed class HideoutAudioBackground : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public HideoutAudioBackground _003C_003E4__this;

		public AreaData areaData;

		internal void _E000()
		{
			_003C_003E4__this._E000(areaData);
			_003C_003E4__this._E001();
		}
	}

	[SerializeField]
	private AudioSource _hollowAudioSource;

	[SerializeField]
	private List<AudioSource> _inhabitedAudioSources;

	[SerializeField]
	private AudioClip _hollowAmbiance;

	[Range(0f, 1f)]
	[SerializeField]
	private float _masterVolume = 1f;

	private float m__E000;

	private float m__E001;

	private float _E002;

	private float _E003 = 1f;

	private readonly Dictionary<EAreaType, float> _E004 = new Dictionary<EAreaType, float>();

	private readonly List<Action> _E005 = new List<Action>();

	private bool _E006;

	private void Update()
	{
		if (base.gameObject.activeInHierarchy && !(_E003 - _masterVolume).IsZero())
		{
			_E003 = _masterVolume;
			_E001();
		}
	}

	private void OnEnable()
	{
		_E001();
	}

	public void Play()
	{
		_E006 = true;
		_E001();
	}

	public void InitArea(AreaData areaData)
	{
		if (!areaData.Enabled)
		{
			return;
		}
		EAreaType type = areaData.Template.Type;
		if (_E004.ContainsKey(type))
		{
			return;
		}
		float num = 0f;
		Dictionary<int, Stage> stages = areaData.Template.Stages;
		foreach (KeyValuePair<int, Stage> item in stages)
		{
			_E39D.Deconstruct(item, out var _, out var value);
			Stage stage = value;
			num += stage.HideoutProgress;
		}
		_E004.Add(type, 0f);
		this.m__E001 += num;
		_E002 += stages[0].HideoutProgress;
		_E005.Add(areaData.LevelUpdated.Subscribe(delegate
		{
			_E000(areaData);
			_E001();
		}));
		_E000(areaData);
	}

	private void _E000(AreaData areaData)
	{
		if (areaData.Enabled)
		{
			float num = 0f;
			Dictionary<int, Stage> stages = areaData.Template.Stages;
			for (int i = 0; i <= areaData.CurrentLevel; i++)
			{
				num += stages[i].HideoutProgress;
			}
			this.m__E000 -= _E004[areaData.Template.Type];
			_E004[areaData.Template.Type] = num;
			this.m__E000 += num;
		}
	}

	private void _E001()
	{
		if (this.m__E000 > _E002 && _hollowAudioSource.clip != _hollowAmbiance)
		{
			_hollowAudioSource.clip = _hollowAmbiance;
		}
		float num = this.m__E000 / this.m__E001;
		float volume = (1f - num) * _masterVolume;
		num *= _masterVolume;
		_hollowAudioSource.volume = volume;
		if (!_hollowAudioSource.isPlaying && _E006)
		{
			_hollowAudioSource.Play();
		}
		foreach (AudioSource inhabitedAudioSource in _inhabitedAudioSources)
		{
			if (!(inhabitedAudioSource == null))
			{
				inhabitedAudioSource.volume = num;
				if (!inhabitedAudioSource.isPlaying && _E006)
				{
					inhabitedAudioSource.Play();
				}
			}
		}
	}

	private void OnDestroy()
	{
		foreach (Action item in _E005)
		{
			item();
		}
	}
}
