using UnityEngine;

namespace EFT.Airdrop;

public class FlarePatronSound : MonoBehaviour
{
	[SerializeField]
	private AudioSource _audioSource;

	[SerializeField]
	private float _volumeStepToFading = 0.01f;

	private float _E000;

	private const float _E001 = 3f;

	public void Init(float lifetime)
	{
		_E000 = Time.time + lifetime;
		_audioSource.Play();
	}

	private void Update()
	{
		if (Time.time + 3f >= _E000 && _audioSource.volume != 0f)
		{
			_audioSource.volume -= _volumeStepToFading;
		}
	}
}
