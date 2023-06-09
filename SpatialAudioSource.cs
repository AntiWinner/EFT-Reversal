using EFT;
using UnityEngine;

public sealed class SpatialAudioSource : MonoBehaviour
{
	[SerializeField]
	private ONSPAudioSource _oculusAudioSource;

	private AudioSource m__E000;

	private void Awake()
	{
		_E000();
	}

	private void Update()
	{
		_E002();
	}

	private void _E000()
	{
		if ((object)_oculusAudioSource == null)
		{
			_oculusAudioSource = GetComponent<ONSPAudioSource>();
		}
		if ((object)this.m__E000 == null)
		{
			this.m__E000 = GetComponent<AudioSource>();
		}
	}

	public void EnableSpatialization(bool enable)
	{
		if (!(_oculusAudioSource == null))
		{
			_oculusAudioSource.EnableSpatialization = enable;
		}
	}

	public void SetParameters(AudioGroupPreset preset)
	{
		_E001(preset);
	}

	private void _E001(AudioGroupPreset preset)
	{
		if ((object)_oculusAudioSource != null)
		{
			if (!_oculusAudioSource.isActiveAndEnabled)
			{
				_oculusAudioSource.enabled = true;
			}
			_oculusAudioSource.EnableSpatialization = preset.SteamSpatialize;
			_oculusAudioSource.VolumetricRadius = preset.OcclusionSourceRadius;
			_oculusAudioSource.Far = preset.DefaultMaxDistance;
		}
	}

	private void _E002()
	{
		if ((object)this.m__E000 != null && (object)_oculusAudioSource != null)
		{
			_oculusAudioSource.Far = this.m__E000.maxDistance;
		}
	}
}
