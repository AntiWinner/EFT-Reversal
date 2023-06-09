using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

public class ShootingScoreInterface : MonoBehaviour
{
	[SerializeField]
	private GameObject _beforeTraining;

	[SerializeField]
	private GameObject _training;

	[SerializeField]
	private TextMeshProUGUI _countdownValue;

	[SerializeField]
	private Color _defaultCountdownColor = Color.white;

	[SerializeField]
	private Color _finalCountdownColor = Color.red;

	[SerializeField]
	private int _finalCountdownSeconds = 10;

	[SerializeField]
	private GameObject _timeLeft;

	[SerializeField]
	private TextMeshProUGUI _timeLeftValue;

	[SerializeField]
	private TextMeshProUGUI _currentScoreValue;

	[SerializeField]
	private TextMeshProUGUI _bestScoreValue;

	[SerializeField]
	private AudioSource _audioSignal;

	private Color _E000;

	private CancellationTokenSource _E001;

	public async Task StartBeforeTraining(int seconds, CancellationToken cancellationToken)
	{
		ShowBeforeTraining();
		while (seconds > 0 && !cancellationToken.IsCancellationRequested)
		{
			SetCountdown(seconds--);
			await Task.Delay(1000, cancellationToken);
		}
	}

	public async Task StartTraining(int seconds, CancellationToken cancellationToken)
	{
		ShowTraining();
		ShowTimeLeft();
		while (seconds > 0 && !cancellationToken.IsCancellationRequested)
		{
			SetTimeLeftColor((seconds < _finalCountdownSeconds) ? _finalCountdownColor : _defaultCountdownColor);
			SetTimeLeft(seconds--);
			await Task.Delay(1000, cancellationToken);
		}
		HideTimeLeft();
	}

	public async void StartCountdown(float delay, float duration)
	{
		_ = 1;
		try
		{
			ResetCountdown();
			await StartBeforeTraining((int)delay, this._E001.Token);
			_audioSignal.Play();
			await StartTraining((int)duration, this._E001.Token);
		}
		catch (Exception)
		{
		}
	}

	public void StopCountdown()
	{
		_audioSignal.Play();
		ResetCountdown();
		ShowTraining();
		HideTimeLeft();
	}

	public void ResetCountdown()
	{
		this._E001?.Cancel();
		this._E001 = new CancellationTokenSource();
	}

	public void ShowBeforeTraining()
	{
		_beforeTraining.gameObject.SetActive(value: true);
		_training.gameObject.SetActive(value: false);
	}

	public void ShowTraining()
	{
		_beforeTraining.gameObject.SetActive(value: false);
		_training.gameObject.SetActive(value: true);
	}

	public void SetCountdown(int value)
	{
		_countdownValue.text = FormatSeconds(value);
	}

	public void ShowTimeLeft()
	{
		_timeLeft.gameObject.SetActive(value: true);
	}

	public void HideTimeLeft()
	{
		_timeLeft.gameObject.SetActive(value: false);
	}

	public void SetTimeLeft(int value)
	{
		_timeLeftValue.text = FormatTime(value);
	}

	public void SetTimeLeftColor(Color value)
	{
		if (!(this._E000 == value))
		{
			this._E000 = value;
			_timeLeftValue.color = value;
		}
	}

	public void SetBestScore(int value)
	{
		_bestScoreValue.text = FormatScore(value);
	}

	public void SetCurrentScore(int value)
	{
		_currentScoreValue.text = FormatScore(value);
	}

	public string FormatScore(int value)
	{
		return value.ToString(_ED3E._E000(103414));
	}

	public string FormatTime(int value)
	{
		return TimeSpan.FromSeconds(value).ToString(_ED3E._E000(164663));
	}

	public string FormatSeconds(int value)
	{
		return TimeSpan.FromSeconds(value).ToString(_ED3E._E000(164654));
	}
}
