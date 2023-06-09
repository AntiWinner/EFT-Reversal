using UnityEngine;

public class CullingLightObjectInverse : CullingLightObject
{
	protected override void UpdateCullingObject(float multiplier, float shadowMultiplier)
	{
		CullDistance = GetFadEndDistance();
		bool num = CullingManager.Instance.IsOpticEnabled();
		bool flag = Mathf.Approximately(multiplier, 0f);
		float num2 = ((num && flag) ? 1f : multiplier);
		num2 *= base.IntensityMultiplier;
		if (GetFlicker() != null && GetFlicker().enabled)
		{
			GetFlicker().CullingCoef = 1f - num2;
		}
		else
		{
			GetLight().intensity = getMaxIntensityFinal() * (1f - num2);
		}
	}
}
