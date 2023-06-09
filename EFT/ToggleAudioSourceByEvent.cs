using EFT.GlobalEvents;
using UnityEngine;

namespace EFT;

public class ToggleAudioSourceByEvent : MonoBehaviour
{
	[SerializeField]
	private AudioSource _audioSource;

	[SerializeField]
	private BaseEventFilter _enableAudioByFilter;

	[SerializeField]
	private BaseEventFilter _disableAudioByFilter;

	private void Awake()
	{
		_enableAudioByFilter.OnFilterPassed += _E000;
		_disableAudioByFilter.OnFilterPassed += _E001;
	}

	private void OnDestroy()
	{
		_enableAudioByFilter.OnFilterPassed -= _E000;
		_disableAudioByFilter.OnFilterPassed -= _E001;
	}

	private void _E000(BaseEventFilter eventFilter, _EBAD invokedEvent)
	{
		_audioSource.enabled = true;
	}

	private void _E001(BaseEventFilter eventFilter, _EBAD invokedEvent)
	{
		_audioSource.enabled = false;
	}
}
