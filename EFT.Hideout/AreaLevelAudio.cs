using UnityEngine;

namespace EFT.Hideout;

[RequireComponent(typeof(AudioArray))]
public sealed class AreaLevelAudio : MonoBehaviour
{
	private RelatedSounds m__E000;

	private EAreaStatus m__E001;

	private EAreaStatus _E002;

	private ELightStatus _E003;

	private ELightStatus _E004;

	private AudioSequence _E005;

	private AudioArray _E006;

	public EAreaStatus AreaStatus
	{
		get
		{
			return this.m__E001;
		}
		set
		{
			_E000(_E003, value);
		}
	}

	public ELightStatus LightStatus
	{
		get
		{
			return _E003;
		}
		set
		{
			_E000(value, this.m__E001);
		}
	}

	private AudioArray _E007
	{
		get
		{
			if (_E006 == null)
			{
				_E006 = base.gameObject.GetComponent<AudioArray>();
			}
			return _E006;
		}
	}

	public void Init(RelatedSounds soundsData, ELightStatus lightStatus = ELightStatus.None, EAreaStatus areaStatus = EAreaStatus.NotSet)
	{
		this.m__E000 = soundsData;
		_E003 = lightStatus;
		this.m__E001 = areaStatus;
		_E005 = soundsData.WorkingSequence;
	}

	public void Resume()
	{
		ELightStatus lightStatus = _E003;
		EAreaStatus areaStatus = this.m__E001;
		_E001();
		_E000(lightStatus, areaStatus);
	}

	public void Select()
	{
		EAreaActivityType soundType = EAreaActivityType.Select;
		switch (this.m__E001)
		{
		case EAreaStatus.Constructing:
			soundType = EAreaActivityType.ConstructionBegin;
			break;
		case EAreaStatus.Upgrading:
			soundType = EAreaActivityType.UpgradeBegin;
			break;
		}
		_E007.PlayOneShot(this.m__E000.GetSound(soundType), volumetric: false);
	}

	public async void LevelChanged()
	{
		if (this.m__E001 != 0)
		{
			Pause();
			await _E007.PlayOneShotAsync(this.m__E000.GetSound(EAreaActivityType.Install), volumetric: false);
			bool num = _E003 == ELightStatus.Working;
			EAudioSequenceType sequenceType = (num ? EAudioSequenceType.OnWorking : EAudioSequenceType.OffDisabled);
			EAreaActivityType soundType = (num ? EAreaActivityType.TurnOn : EAreaActivityType.TurnOff);
			_E007.PlaySequence(_E005, sequenceType, volumetric: true);
			_E007.PlayOneShot(this.m__E000.GetSound(soundType), volumetric: false);
		}
	}

	public void PlayOneShot(AudioClip sound, bool volumetric)
	{
		_E007.PlayOneShot(sound, volumetric);
	}

	public void PlayOneShot(EAreaActivityType soundType, bool volumetric)
	{
		PlayOneShot(this.m__E000.GetSound(soundType), volumetric);
	}

	private void _E000(ELightStatus lightStatus, EAreaStatus areaStatus)
	{
		if (_E003 == lightStatus && this.m__E001 == areaStatus)
		{
			return;
		}
		_E002 = this.m__E001;
		_E004 = _E003;
		this.m__E001 = areaStatus;
		_E003 = lightStatus;
		if (_E002 != this.m__E001 && _E002 != 0)
		{
			switch (this.m__E001)
			{
			case EAreaStatus.Constructing:
				_E007.PlayOneShot(this.m__E000.GetSound(EAreaActivityType.ConstructionBegin), volumetric: false);
				break;
			case EAreaStatus.Upgrading:
				_E007.PlayOneShot(this.m__E000.GetSound(EAreaActivityType.UpgradeBegin), volumetric: false);
				break;
			case EAreaStatus.ReadyToInstallConstruct:
				_E007.PlayOneShot(this.m__E000.GetSound(EAreaActivityType.ConstructionComplete), volumetric: false);
				break;
			case EAreaStatus.ReadyToInstallUpgrade:
				_E007.PlayOneShot(this.m__E000.GetSound(EAreaActivityType.UpgradeComplete), volumetric: false);
				break;
			}
		}
		if (_E004 != _E003)
		{
			bool flag = _E003 == ELightStatus.Working;
			if (_E004 != 0)
			{
				EAudioSequenceType sequenceType = (flag ? EAudioSequenceType.OnWorking : EAudioSequenceType.OffDisabled);
				EAreaActivityType soundType = (flag ? EAreaActivityType.TurnOn : EAreaActivityType.TurnOff);
				_E007.PlaySequence(_E005, sequenceType, volumetric: true);
				_E007.PlayOneShot(this.m__E000.GetSound(soundType), volumetric: false);
			}
			else
			{
				_E007.PlaySequence(_E005, (!flag) ? EAudioSequenceType.Disabled : EAudioSequenceType.Working, volumetric: true);
			}
		}
	}

	private void _E001()
	{
		_E003 = ELightStatus.None;
		this.m__E001 = EAreaStatus.NotSet;
	}

	public void Pause()
	{
		_E007.Stop(volumetric: true);
	}
}
