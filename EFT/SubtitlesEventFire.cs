using UnityEngine;

namespace EFT;

public class SubtitlesEventFire : MonoBehaviour
{
	public const string SUBTITLES_PREFIX = "Subtitles";

	[SerializeField]
	private AudioSource _audioSource;

	[SerializeField]
	private ESubtitlesSource _source;

	private bool _E000;

	private void Awake()
	{
		if (_audioSource == null)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (!_audioSource.isPlaying)
		{
			if (_E000)
			{
				_E000 = false;
				_EBAF.Instance.CreateCommonEvent<_EBB8>().Invoke(_source);
			}
		}
		else if (!_E000)
		{
			_E000 = true;
			_EBAF.Instance.CreateCommonEvent<_EBC3>().Invoke(_source, _ED3E._E000(139896) + _audioSource.clip.name, _audioSource.clip.name);
		}
	}
}
