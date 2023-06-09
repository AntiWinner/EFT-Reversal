using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotInfoIconsPanel : MonoBehaviour
{
	public BotInfoIcon GrenadeObj;

	public BotInfoIcon CoverObj;

	public BotInfoIcon EnemyVisibleObj;

	public BotInfoIcon LookThoughtGrass;

	public BotInfoIcon CanShoot;

	public BotInfoIcon InBuildingTarget;

	public BotInfoIcon InBuildingMe;

	public BotInfoIcon UnderFire;

	public BotInfoIcon HealObj;

	public BotInfoIcon SurgicalObj;

	public BotInfoIcon StimulatorObj;

	public BotInfoIcon ParametersChange;

	public TextMeshProUGUI AmmoRemainField;

	public Image IconsBackImageUpper;

	public Image IconsBackImageLower;

	private Color _E000 = new Color(0.5f, 0.5f, 0.5f, 0.15f);

	private RectTransform _E001;

	public RectTransform RectTransform => _E001 ?? (_E001 = base.transform.RectTransform());

	public void SetBackgroundColor(Color color)
	{
		IconsBackImageUpper.color = color;
		IconsBackImageLower.color = color;
	}

	public void UpdateIcons(_E10B actorDataStruct)
	{
		_E10A botData = actorDataStruct.BotData;
		AmmoRemainField.text = botData.Ammo.ToString();
		AmmoRemainField.color = (botData.Shooting ? Color.red : Color.white);
		CoverObj.SetBool(botData.InCover);
		UnderFire.SetBool(botData.UnderFire);
		ParametersChange.SetBool(botData.IsParametesChanged);
		EnemyVisibleObj.SetInt((int)botData.IsEnemyVisibleOnlyBySense);
		LookThoughtGrass.SetBool(botData.CanLookThoughtGrass);
		CanShoot.SetBool(botData.CanShoot);
		InBuildingTarget.SetBool(botData.InBuildingTarget);
		InBuildingMe.SetBool(botData.InBuildingMe);
		if (botData.HaveSurgical)
		{
			SurgicalObj.SetBool(botData.UseSurgical);
		}
		else
		{
			SurgicalObj.SetColor(_E000);
		}
		if (botData.HaveMeds)
		{
			HealObj.SetBool(botData.UseMeds);
		}
		else
		{
			HealObj.SetColor(_E000);
		}
		if (botData.HaveStimulator)
		{
			StimulatorObj.SetBool(botData.UseStimulator);
		}
		else
		{
			StimulatorObj.SetColor(_E000);
		}
		GrenadeObj.SetBool(botData.HaveGrenade);
	}
}
