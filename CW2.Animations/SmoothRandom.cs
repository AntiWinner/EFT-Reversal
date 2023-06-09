using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CW2.Animations;

[Serializable]
public class SmoothRandom
{
	[FormerlySerializedAs("Amlitude")]
	[Range(0f, 5f)]
	public float Amplitude = 0.1f;

	[_E2BD(0f, 1f, -1f)]
	public Vector2 MinMaxDelay = new Vector2(0.3f, 0.6f);

	[Range(0f, 2f)]
	public float Hardness = 0.1f;

	private float _countDown;

	private float _currentVal;

	private float _increase;

	public float GetValue(float deltaTime)
	{
		_countDown -= deltaTime;
		if (_countDown < 0f)
		{
			float num = (_countDown = UnityEngine.Random.Range(MinMaxDelay.x, MinMaxDelay.y));
			float num2 = (UnityEngine.Random.Range(0f - Amplitude, Amplitude) - _currentVal) / num;
			_increase += (num2 - _increase) * Hardness;
		}
		_currentVal += _increase * deltaTime;
		return _currentVal;
	}
}
