using System;
using UnityEngine;

[Serializable]
public class ValuesCoefs
{
	[Range(0.01f, 10f)]
	public float MainTexColorCoef = 0.2f;

	[Range(-10f, 10f)]
	public float MinimumTemperatureValue = 0.25f;

	[Range(-1f, 1f)]
	public float RampShift;
}
