using Comfort.Common;
using EFT.GlobalEvents;
using UnityEngine;

namespace EFT;

public class MuteSpeechToggler : MonoBehaviour
{
	[SerializeField]
	private BaseEventFilter _muteSpeechEventFilter;

	[SerializeField]
	private BaseEventFilter _unmuteSpeechEventFilter;

	private void Awake()
	{
		_muteSpeechEventFilter.OnFilterPassed += _E000;
		_unmuteSpeechEventFilter.OnFilterPassed += _E001;
	}

	private void _E000(BaseEventFilter filter, _EBAD passedEvent)
	{
		foreach (Player allPlayer in Singleton<GameWorld>.Instance.AllPlayers)
		{
			allPlayer.ToggleMuteSpeechSource(muteSpeech: true);
		}
	}

	private void _E001(BaseEventFilter filter, _EBAD passedEvent)
	{
		foreach (Player allPlayer in Singleton<GameWorld>.Instance.AllPlayers)
		{
			allPlayer.ToggleMuteSpeechSource(muteSpeech: false);
		}
	}
}
