using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;

namespace EFT.UI;

public sealed class GUISounds : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E002
	{
		public string fileName;

		internal bool _E000(AudioClip x)
		{
			return x.name == fileName;
		}
	}

	private const float m__E000 = 0.15f;

	private AudioSource m__E001;

	private AudioSource m__E002;

	private AudioSource m__E003;

	private AudioSource m__E004;

	private AudioSource m__E005;

	private AudioMixer m__E006;

	private Coroutine m__E007;

	private const string m__E008 = "Audio/Music/";

	private AudioClip[] m__E009;

	private int _E00A;

	private ItemSounds _E00B;

	private UISoundsWrapper _E00C;

	private CancellationTokenSource _E00D;

	private AudioMixerGroup _E00E;

	private AudioMixerGroup _E00F;

	[CompilerGenerated]
	private AudioMixer _E010;

	[CompilerGenerated]
	private bool _E011;

	[CompilerGenerated]
	private bool _E012;

	public AudioMixer MasterMixer
	{
		[CompilerGenerated]
		get
		{
			return _E010;
		}
		[CompilerGenerated]
		private set
		{
			_E010 = value;
		}
	}

	public bool BackgroundMusicActive
	{
		[CompilerGenerated]
		get
		{
			return _E011;
		}
		[CompilerGenerated]
		private set
		{
			_E011 = value;
		}
	}

	public bool HideoutSoundActive
	{
		[CompilerGenerated]
		get
		{
			return _E012;
		}
		[CompilerGenerated]
		private set
		{
			_E012 = value;
		}
	}

	internal async Task _E000(AudioSource audioSource)
	{
		ResourceRequest resourceRequest = Resources.LoadAsync<AudioMixer>(_ED3E._E000(35435));
		await resourceRequest.Await();
		ResourceRequest resourceRequest2 = Resources.LoadAsync<UISoundsWrapper>(_ED3E._E000(251911));
		await resourceRequest2.Await();
		this.m__E006 = (AudioMixer)resourceRequest.asset;
		_E00C = (UISoundsWrapper)resourceRequest2.asset;
		this.m__E001 = audioSource;
		this.m__E004 = audioSource.gameObject.AddComponent<AudioSource>();
		this.m__E002 = audioSource.gameObject.AddComponent<AudioSource>();
		this.m__E003 = audioSource.gameObject.AddComponent<AudioSource>();
		this.m__E005 = audioSource.gameObject.AddComponent<AudioSource>();
		MasterMixer = this.m__E006;
		_E00E = this.m__E006.FindMatchingGroups(_ED3E._E000(103244)).First();
		_E00F = this.m__E006.FindMatchingGroups(_ED3E._E000(251949)).First();
		this.m__E001.outputAudioMixerGroup = _E00E;
		this.m__E002.outputAudioMixerGroup = _E00E;
		this.m__E004.outputAudioMixerGroup = this.m__E006.FindMatchingGroups(_ED3E._E000(251938)).First();
		this.m__E003.outputAudioMixerGroup = this.m__E006.FindMatchingGroups(_ED3E._E000(251992)).First();
		this.m__E005.outputAudioMixerGroup = this.m__E006.FindMatchingGroups(_ED3E._E000(103244)).First();
		this.m__E004.playOnAwake = false;
		this.m__E009 = Resources.LoadAll<AudioClip>(_ED3E._E000(251989));
	}

	internal void _E001()
	{
		_E612.WaitForAllBundles(Singleton<_ED0A>.Instance.Retain(new string[1] { _ED3E._E000(109921) }), delegate
		{
			_E00B = Singleton<_ED0A>.Instance.GetAsset<ItemSounds>(_ED3E._E000(109921));
		});
	}

	public void PlaySound(AudioClip clip, bool single = false, bool commonUiSound = false)
	{
		AudioMixerGroup outputAudioMixerGroup = _E00E;
		float volumeScale = 1f;
		if (commonUiSound)
		{
			outputAudioMixerGroup = _E007();
			volumeScale = _E008();
		}
		if (!single)
		{
			if (commonUiSound)
			{
				this.m__E002.outputAudioMixerGroup = outputAudioMixerGroup;
				this.m__E002.PlayOneShot(clip, volumeScale);
			}
			else
			{
				this.m__E001.outputAudioMixerGroup = _E00E;
				this.m__E001.PlayOneShot(clip);
			}
		}
		else
		{
			this.m__E005.Stop();
			this.m__E005.outputAudioMixerGroup = outputAudioMixerGroup;
			this.m__E005.PlayOneShot(clip, volumeScale);
		}
	}

	public async Task ForcePlaySound(AudioClip clip)
	{
		_E00D?.Cancel();
		CancellationTokenSource cancellationTokenSource = (_E00D = new CancellationTokenSource());
		_E7E0 settings = Singleton<_E7DE>.Instance.Sound.Settings;
		bool flag = settings.InterfaceVolumeValue < -17;
		bool flag2 = settings.MusicVolumeValue > -40;
		if (flag)
		{
			MasterMixer.SetFloat(_ED3E._E000(251978), -17f);
		}
		if (flag2)
		{
			MasterMixer.SetFloat(_ED3E._E000(192313), -40f);
		}
		this.m__E005.Stop();
		this.m__E005.PlayOneShot(clip);
		await Task.Delay(Mathf.CeilToInt(clip.length * 1000f));
		if (!cancellationTokenSource.IsCancellationRequested)
		{
			if (flag)
			{
				MasterMixer.SetFloat(_ED3E._E000(251978), settings.InterfaceVolumeValue);
			}
			if (flag2)
			{
				MasterMixer.SetFloat(_ED3E._E000(192313), settings.MusicVolumeValue);
			}
		}
	}

	public void PlayItemSound(string soundGroup, EInventorySoundType soundType, bool single = false)
	{
		AudioClip itemClip = GetItemClip(soundGroup, soundType);
		if (itemClip == null)
		{
			Debug.LogWarning(_ED3E._E000(253888) + soundGroup + _ED3E._E000(48793) + soundType);
		}
		else
		{
			PlaySound(itemClip, single, commonUiSound: true);
		}
	}

	public bool PlayItemSound(EModClass itemClass)
	{
		EUISoundType soundType;
		switch (itemClass)
		{
		case EModClass.None:
			return false;
		case EModClass.Master:
			soundType = EUISoundType.MenuInstallModVital;
			break;
		case EModClass.Gear:
			soundType = EUISoundType.MenuInstallModGear;
			break;
		default:
			soundType = EUISoundType.MenuInstallModFunc;
			break;
		}
		PlayUISound(soundType);
		return true;
	}

	internal void _E002(ExitStatus exitStatus)
	{
		bool flag = exitStatus == ExitStatus.Killed || exitStatus == ExitStatus.MissingInAction || exitStatus == ExitStatus.Left;
		PlayEndGameSound(flag ? EEndGameSoundType.ArenaLose : EEndGameSoundType.ArenaWin);
	}

	internal void _E003()
	{
		int num;
		do
		{
			num = Random.Range(0, this.m__E009.Length);
		}
		while (_E00A == num);
		_E00A = num;
		AudioClip audioClip = this.m__E009[_E00A];
		_E004();
		this.m__E004.clip = audioClip;
		this.m__E004.Play();
		this.m__E007 = StaticManager.Instance.WaitSeconds(audioClip.length, _E003);
	}

	internal void _E004()
	{
		if (this.m__E007 != null)
		{
			StaticManager.Instance.StopCoroutine(this.m__E007);
		}
		this.m__E004.Stop();
		this.m__E004.clip = null;
	}

	internal void _E005(bool isActive)
	{
		BackgroundMusicActive = isActive;
		this.m__E004.DOFade(isActive ? 1 : 0, 2f);
	}

	internal void _E006(bool active)
	{
		HideoutSoundActive = active;
		int num = ((!active) ? (-80) : Singleton<_E7DE>.Instance.Sound.Settings.HideoutVolumeValue);
		MasterMixer.DOKill();
		MasterMixer.DOSetFloat(_ED3E._E000(192185), num, 1f);
	}

	[CanBeNull]
	public AudioClip GetItemClip(string soundGroup, EInventorySoundType soundType)
	{
		if (!(_E00B == null))
		{
			return _E00B.GetClip(soundGroup, soundType);
		}
		return null;
	}

	[CanBeNull]
	public AudioClip GetLootingClip(string fileName)
	{
		if (!(_E00B == null))
		{
			return _E00B.LootingClips.FirstOrDefault((AudioClip x) => x.name == fileName);
		}
		return null;
	}

	public void PlayUILoadSound()
	{
		AudioClip randomClip = _E00C.LoadSounds.GetRandomClip();
		if (randomClip != null)
		{
			PlaySound(randomClip, single: false, commonUiSound: true);
		}
	}

	public void PlayUIUnloadSound()
	{
		AudioClip randomClip = _E00C.UnloadSounds.GetRandomClip();
		if (randomClip != null)
		{
			PlaySound(randomClip, single: false, commonUiSound: true);
		}
	}

	public void PlayUISound(EUISoundType soundType)
	{
		AudioClip uIClip = _E00C.GetUIClip(soundType);
		if (uIClip != null)
		{
			PlaySound(uIClip);
		}
	}

	public void PlayChatSound(ESocialNetworkSoundType soundType)
	{
		AudioClip socialNetworkClip = _E00C.GetSocialNetworkClip(soundType);
		if (socialNetworkClip != null)
		{
			this.m__E003.PlayOneShot(socialNetworkClip);
		}
	}

	public void PlayEndGameSound(EEndGameSoundType soundType)
	{
		AudioClip endGameClip = _E00C.GetEndGameClip(soundType);
		if (endGameClip != null)
		{
			PlaySound(endGameClip);
		}
	}

	public void PlayNotificationSound()
	{
		AudioClip audioClip = _E905.Pop<AudioClip>(_ED3E._E000(253935));
		if (!(audioClip == null))
		{
			this.m__E001.outputAudioMixerGroup = _E00E;
			this.m__E001.PlayOneShot(audioClip);
		}
	}

	private AudioMixerGroup _E007()
	{
		if (!Singleton<AbstractGame>.Instantiated || !Singleton<AbstractGame>.Instance.InRaid)
		{
			return _E00E;
		}
		return _E00F;
	}

	private float _E008()
	{
		if (!Singleton<AbstractGame>.Instantiated || !Singleton<AbstractGame>.Instance.InRaid)
		{
			return 1f;
		}
		return 0.15f;
	}

	[CompilerGenerated]
	private void _E009()
	{
		_E00B = Singleton<_ED0A>.Instance.GetAsset<ItemSounds>(_ED3E._E000(109921));
	}
}
