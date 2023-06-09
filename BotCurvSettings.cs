using UnityEngine;

public class BotCurvSettings : ScriptableObject
{
	private static BotCurvSettings _E000;

	public AnimationCurve NightVisionSettings;

	public AnimationCurve StandartVisionSettings;

	public AnimationCurve VisionAngCoef;

	public AnimationCurve AimTime2Dist;

	public AnimationCurve AimAngCoef;

	public AnimationCurve AgsAimUp;

	public AnimationCurve MoveStartCurv;

	public static BotCurvSettings Instance => _E000 ?? (_E000 = _E3A2.Load<BotCurvSettings>(_ED3E._E000(15625)));

	public BotCurvSettings Copy()
	{
		return Object.Instantiate(this);
	}
}
