using System;
using UnityEngine;

public class TimerText : MonoBehaviour
{
	[SerializeField]
	private CustomTextMeshProUGUI _text;

	private float _E000;

	public void Show(float startTime)
	{
		base.gameObject.SetActive(value: true);
		_E000 = startTime;
	}

	private void Update()
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time - _E000);
		_text.text = string.Format(_ED3E._E000(48740), (int)timeSpan.TotalMinutes, timeSpan.Seconds);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
