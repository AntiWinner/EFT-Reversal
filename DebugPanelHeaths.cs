using UnityEngine;
using UnityEngine.UI;

public class DebugPanelHeaths : MonoBehaviour
{
	public Image IconsHealthHead;

	public Image IconsHealthBody;

	public Image IconsHealthStomach;

	public Image IconsHealthLeftArm;

	public Image IconsHealthRightArm;

	public Image IconsHealthLeftLeg;

	public Image IconsHealthRightLeg;

	private RectTransform _E000;

	public RectTransform RectTransform => _E000 ?? (_E000 = base.transform.RectTransform());

	public void UpdateHealth(_E109 heathsData)
	{
		IconsHealthHead.color = GetHealthColor(heathsData.HealthHead, heathsData.MaxHealthHead);
		IconsHealthBody.color = GetHealthColor(heathsData.HealthBody, heathsData.MaxHealthBody);
		IconsHealthStomach.color = GetHealthColor(heathsData.HealthStomach, heathsData.MaxHealthStomach);
		IconsHealthLeftArm.color = GetHealthColor(heathsData.HealthLeftArm, heathsData.MaxHealthLeftArm);
		IconsHealthRightArm.color = GetHealthColor(heathsData.HealthRightArm, heathsData.MaxHealthRightArm);
		IconsHealthLeftLeg.color = GetHealthColor(heathsData.HealthLeftLeg, heathsData.MaxHealthLeftLeg);
		IconsHealthRightLeg.color = GetHealthColor(heathsData.HealthRightLeg, heathsData.MaxHealthRightLeg);
	}

	public Color GetHealthColor(int currentHealth, int maxHealth)
	{
		if (currentHealth == 0)
		{
			return Color.black;
		}
		if (currentHealth * 2 > maxHealth)
		{
			return Color.Lerp(Color.yellow, Color.green, ((float)currentHealth - (float)maxHealth * 0.5f) / (float)maxHealth);
		}
		return Color.Lerp(Color.yellow, Color.red, ((float)maxHealth * 0.5f - (float)currentHealth) / ((float)maxHealth * 0.5f));
	}
}
