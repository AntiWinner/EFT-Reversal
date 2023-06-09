using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class RainControllerOld : MonoBehaviour
{
	[SerializeField]
	private AudioSource _waterAudio;

	[SerializeField]
	private float _maxAudioVolume = 0.5f;

	[SerializeField]
	private float _minAudioVolume;

	[SerializeField]
	private AnimationCurve _fadeCurve;

	[SerializeField]
	private float _evaluationTime = 2f;

	private float _E000;

	private int _E001 = 1;

	private void OnTriggerEnter(Collider col)
	{
		_E001 = -1;
	}

	private void OnTriggerExit(Collider col)
	{
		_E001 = 1;
	}

	private void Update()
	{
		float num = 1f / _evaluationTime * Time.deltaTime;
		_E000 = Mathf.Clamp01(_E000 - num * (float)_E001);
		float t = _fadeCurve.Evaluate(_E000);
		_waterAudio.volume = Mathf.Lerp(_minAudioVolume, _maxAudioVolume, t);
	}
}
