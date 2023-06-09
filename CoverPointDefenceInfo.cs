using System;
using UnityEngine;

[Serializable]
public class CoverPointDefenceInfo
{
	[SerializeField]
	private float _defenceLevel;

	[SerializeField]
	private int distanceCheckSum;

	public float DefenceLevel => _defenceLevel;

	public int DangerCoeff => distanceCheckSum;

	public CoverPointDefenceInfo(int defLevel)
	{
		_defenceLevel = defLevel;
		distanceCheckSum = 999;
	}

	public CoverPointDefenceInfo(Vector3 position)
	{
		float num = (float)_E079.AllBaseSides.Count * 8f;
		float num2 = 0f;
		BotZoneDebug botZoneDebug = UnityEngine.Object.FindObjectOfType<BotZoneDebug>();
		Vector3 vector = position + Vector3.up * 1.26f;
		distanceCheckSum = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		for (int i = 0; i < _E079.AllBaseSides.Count; i++)
		{
			Vector3 vector2 = _E079.AllBaseSides[i];
			if (!_E079.TestDir(vector, vector2, 8f, out var outPos) && outPos.HasValue)
			{
				float magnitude = (outPos.Value - position).magnitude;
				num2 += magnitude;
				if (magnitude <= 2f)
				{
					botZoneDebug?.AddLine(_ED3E._E000(30800), vector, vector + vector2);
					num3++;
				}
				else if (magnitude <= 6f)
				{
					botZoneDebug?.AddLine(_ED3E._E000(30847), vector, vector + vector2);
					distanceCheckSum++;
					num4++;
				}
				else
				{
					botZoneDebug?.AddLine(_ED3E._E000(30820), vector, vector + vector2);
					distanceCheckSum += 2;
					num5++;
				}
			}
			else
			{
				num2 += 8f;
				botZoneDebug?.AddLine(_ED3E._E000(30865), vector, vector + vector2);
				distanceCheckSum += 4;
				num6++;
			}
		}
		botZoneDebug?.Add(_ED3E._E000(30910) + distanceCheckSum, vector);
		_defenceLevel = num - num2;
	}

	public void OverrideData(float defenceLevel, int dangerCoeff)
	{
		_defenceLevel = defenceLevel;
		distanceCheckSum = dangerCoeff;
	}

	public bool IsSafe()
	{
		return distanceCheckSum < 8;
	}
}
