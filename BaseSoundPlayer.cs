using System;
using System.Collections.Generic;
using Audio.SpatialSystem;
using Comfort.Common;
using EFT;
using UnityEngine;

public class BaseSoundPlayer : MonoBehaviour, _E324
{
	[Serializable]
	public class SoundElement
	{
		public string EventName = "";

		public int RollOff = 30;

		public float Volume = 1f;

		public AudioClip[] SoundClips;

		public AudioClip RandomSoundClip
		{
			get
			{
				if (SoundClips == null || SoundClips.Length == 0)
				{
					return null;
				}
				return SoundClips[UnityEngine.Random.Range(0, SoundClips.Length)];
			}
		}
	}

	private BetterSource _clipsSource;

	protected BifacialTransform _weaponHierarchy;

	private const string UberviewPattern = "EventName 150;SoundClips 180;";

	[SerializeField]
	public List<SoundElement> AdditionalSounds = new List<SoundElement>();

	[SerializeField]
	public SoundElement[] MainSounds = new SoundElement[8]
	{
		new SoundElement
		{
			EventName = "ShellEject"
		},
		new SoundElement
		{
			EventName = "AddAmmoInChamber"
		},
		new SoundElement
		{
			EventName = "OnBoltCatch"
		},
		new SoundElement
		{
			EventName = "OffBoltCatch"
		},
		new SoundElement
		{
			EventName = "SetFiremode1"
		},
		new SoundElement
		{
			EventName = "SetFiremode0"
		},
		new SoundElement
		{
			EventName = "MagOut"
		},
		new SoundElement
		{
			EventName = "MagIn"
		}
	};

	protected readonly _E3A4 CompositeDisposable = new _E3A4();

	protected Player Player;

	private FirearmsAnimator _firearmsAnimator;

	public virtual void Init(_E6C7 handsController, BifacialTransform weaponHierarchy, Player player)
	{
		Player = player;
		_weaponHierarchy = weaponHierarchy;
		_firearmsAnimator = handsController.FirearmsAnimator;
		_firearmsAnimator.AddEventsConsumer(this);
		CompositeDisposable.AddDisposable(delegate
		{
			_firearmsAnimator.RemoveEventsConsumer(this);
		});
	}

	private void SoundEventHandler(string soundName)
	{
		SoundElement soundElement2 = AdditionalSounds.Find((SoundElement soundElement) => soundElement.EventName == soundName || soundElement.EventName == "Snd" + soundName);
		if (soundElement2 != null)
		{
			PlayRandomClip(soundElement2);
		}
	}

	private void SoundAtPointEventHandler(string soundName)
	{
		SoundElement soundElement = AdditionalSounds.Find((SoundElement elem) => elem.EventName == soundName || elem.EventName == "Snd" + soundName);
		if (soundElement != null)
		{
			AudioClip randomSoundClip = soundElement.RandomSoundClip;
			if (!(randomSoundClip == null))
			{
				Vector3 position = _weaponHierarchy.position;
				EOcclusionTest occlusionTest = ((Player.PointOfView != 0) ? EOcclusionTest.Fast : EOcclusionTest.None);
				bool forceStereo = Player.PointOfView == EPointOfView.FirstPerson;
				MonoBehaviourSingleton<BetterAudio>.Instance.PlayAtPoint(position, randomSoundClip, _E8A8.Instance.Distance(position), BetterAudio.AudioSourceGroupType.Weaponry, soundElement.RollOff, soundElement.Volume, occlusionTest, null, forceStereo);
			}
		}
	}

	public void EventReceiver(string eventName)
	{
		SoundElement soundElement2 = Array.Find(MainSounds, (SoundElement soundElement) => soundElement.EventName == eventName);
		if (soundElement2 != null)
		{
			PlayRandomClip(soundElement2);
		}
		SoundElement soundElement3 = AdditionalSounds.Find((SoundElement soundElement) => soundElement.EventName == eventName);
		if (soundElement3 != null)
		{
			PlayRandomClip(soundElement3);
		}
	}

	private void PlayRandomClip(SoundElement soundElement)
	{
		if (soundElement == null)
		{
			return;
		}
		AudioClip randomSoundClip = soundElement.RandomSoundClip;
		if (randomSoundClip == null || ((Player.PointOfView == EPointOfView.FirstPerson) ? 0f : _E8A8.Instance.Distance(_weaponHierarchy.position)) > (float)soundElement.RollOff)
		{
			return;
		}
		if (_clipsSource == null)
		{
			_clipsSource = Singleton<BetterAudio>.Instance.GetSource(BetterAudio.AudioSourceGroupType.Weaponry, activateSource: false);
			if (_clipsSource == null)
			{
				Debug.LogWarning("GetSource(BetterAudio.AudioSourceGroupType.Weaponry) returned null, overflow?");
				return;
			}
			_clipsSource.Awake();
			_clipsSource.gameObject.name = randomSoundClip.name + " " + _weaponHierarchy.Original.gameObject.name;
			_clipsSource.Position = _weaponHierarchy.position;
		}
		float volume = soundElement.Volume;
		if (Player.PointOfView == EPointOfView.FirstPerson)
		{
			_clipsSource.SetMixerGroup(Singleton<BetterAudio>.Instance.VeryStandartMixerGroup);
		}
		else
		{
			if (MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
			{
				MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ProcessSourceOcclusion(Player, _clipsSource);
			}
			_clipsSource.SetMixerGroup(MonoBehaviourSingleton<BetterAudio>.Instance.VeryStandartMixerGroup);
		}
		if (_clipsSource.OcclusionVolumeFactor > 0f)
		{
			_clipsSource.gameObject.SetActive(value: true);
			_clipsSource.SetRolloff(soundElement.RollOff);
			_clipsSource.Play(randomSoundClip, null, 1f, volume);
		}
	}

	public virtual void Update()
	{
		if (_clipsSource != null && !_clipsSource.source1.isPlaying)
		{
			ReleaseClipsSource();
		}
		if (_clipsSource != null && _weaponHierarchy != null)
		{
			_clipsSource.transform.position = _weaponHierarchy.position;
		}
	}

	protected virtual void OnDestroy()
	{
		CompositeDisposable.Dispose();
		_firearmsAnimator = null;
		Player = null;
		ReleaseClipsSource();
	}

	protected virtual void OnDisable()
	{
		ReleaseClipsSource();
	}

	private void ReleaseClipsSource()
	{
		if (!(_clipsSource == null))
		{
			_clipsSource.Release();
			_clipsSource = null;
		}
	}

	void _E324.OnUseProp(bool boolParam)
	{
		PlayRandomClip(boolParam ? Player.PropIn : Player.PropOut);
	}

	void _E324.OnAddAmmoInChamber()
	{
	}

	void _E324.OnAddAmmoInMag()
	{
	}

	void _E324.OnArm()
	{
	}

	void _E324.OnCook()
	{
	}

	void _E324.OnDelAmmoChamber()
	{
	}

	void _E324.OnDelAmmoFromMag()
	{
	}

	void _E324.OnDisarm()
	{
	}

	void _E324.OnFireEnd()
	{
	}

	void _E324.OnFiringBullet()
	{
	}

	void _E324.OnFoldOff()
	{
	}

	void _E324.OnFoldOn()
	{
	}

	void _E324.OnIdleStart()
	{
	}

	void _E324.OnMalfunctionOff()
	{
	}

	void _E324.OnMagHide()
	{
	}

	void _E324.OnMagIn()
	{
	}

	void _E324.OnMagOut()
	{
	}

	void _E324.OnMagShow()
	{
	}

	void _E324.OnMessageName()
	{
	}

	void _E324.OnModChanged()
	{
	}

	void _E324.OnOffBoltCatch()
	{
	}

	void _E324.OnOnBoltCatch()
	{
	}

	void _E324.OnPutMagToRig()
	{
	}

	void _E324.OnRemoveShell()
	{
	}

	void _E324.OnReplaceSecondMag()
	{
	}

	void _E324.OnShellEject()
	{
	}

	void _E324.OnShowAmmo(bool BoolParam)
	{
	}

	public void OnSliderOut()
	{
	}

	void _E324.OnSound(string StringParam)
	{
		SoundEventHandler(StringParam);
	}

	void _E324.OnSoundAtPoint(string StringParam)
	{
		SoundAtPointEventHandler(StringParam);
	}

	void _E324.OnStartUtilityOperation()
	{
	}

	void _E324.OnThirdAction(int IntParam)
	{
	}

	void _E324.OnUseSecondMagForReload()
	{
	}

	void _E324.OnWeapIn()
	{
	}

	void _E324.OnWeapOut()
	{
	}

	void _E324.OnBackpackDrop()
	{
	}

	void _E324.OnComboPlanning()
	{
	}

	void _E324.OnLauncherAppeared()
	{
	}

	void _E324.OnLauncherDisappeared()
	{
	}

	void _E324.OnShowMag()
	{
	}

	void _E324.OnCurrentAnimStateEnded()
	{
	}

	void _E324.OnSetActiveObject(int objectID)
	{
	}

	void _E324.OnDeactivateObject(int objectID)
	{
	}
}
