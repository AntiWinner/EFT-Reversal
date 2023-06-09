using System;
using UnityEngine;

[Serializable]
public class BotLocationModifier
{
	public float AccuracySpeed = 1f;

	public float Scattering = 1f;

	public float GainSight = 1f;

	public float MarksmanAccuratyCoef = 1f;

	public float VisibleDistance = 1f;

	public float MagnetPower = 10f;

	public float DistToSleep = 240f;

	public float DistToActivate = 220f;

	public float DistToPersueAxemanCoef = 1f;

	public float KhorovodChance;

	public void Validate()
	{
		_E000(ref AccuracySpeed);
		_E000(ref Scattering);
		_E000(ref GainSight);
		_E000(ref MarksmanAccuratyCoef);
		_E000(ref VisibleDistance);
		_E000(ref DistToSleep);
		_E000(ref DistToActivate);
		if (MagnetPower < 0f)
		{
			Debug.LogError(_ED3E._E000(16011));
			MagnetPower = 0f;
		}
	}

	private void _E000(ref float val)
	{
		if (val <= 0.001f)
		{
			Debug.LogError(_ED3E._E000(16119));
			val = 1f;
		}
	}
}
