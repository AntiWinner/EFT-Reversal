using System;
using UnityEngine;

[Serializable]
public class TOD_WorldParameters
{
	[Range(-90f, 90f)]
	[Tooltip("Latitude of the current location in degrees.")]
	public float Latitude;

	[Range(-180f, 180f)]
	[Tooltip("Longitude of the current location in degrees.")]
	public float Longitude;

	[Tooltip("UTC/GMT time zone of the current location in hours.")]
	[Range(-14f, 14f)]
	public float UTC;
}
