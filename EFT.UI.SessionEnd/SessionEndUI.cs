using EFT.UI.Screens;

namespace EFT.UI.SessionEnd;

public sealed class SessionEndUI : MonoBehaviourSingleton<SessionEndUI>
{
	public SessionResultExitStatus SessionResultExitStatus;

	public SessionResultKillList SessionResultKillList;

	public SessionResultStatistics SessionResultStatistics;

	public SessionResultExperienceCount SessionResultExperienceCount;

	public HealthTreatmentScreen HealthTreatmentScreen;

	public override void Awake()
	{
		base.Awake();
		_EC92 instance = _EC92.Instance;
		instance.RegisterScreen(EEftScreenType.ExitStatus, SessionResultExitStatus);
		instance.RegisterScreen(EEftScreenType.KillList, SessionResultKillList);
		instance.RegisterScreen(EEftScreenType.SessionStatistics, SessionResultStatistics);
		instance.RegisterScreen(EEftScreenType.SessionExperience, SessionResultExperienceCount);
		instance.RegisterScreen(EEftScreenType.HealthTreatment, HealthTreatmentScreen);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_EC92 instance = _EC92.Instance;
		instance.ReleaseScreen(EEftScreenType.ExitStatus, SessionResultExitStatus);
		instance.ReleaseScreen(EEftScreenType.KillList, SessionResultKillList);
		instance.ReleaseScreen(EEftScreenType.SessionStatistics, SessionResultStatistics);
		instance.ReleaseScreen(EEftScreenType.SessionExperience, SessionResultExperienceCount);
		instance.ReleaseScreen(EEftScreenType.HealthTreatment, HealthTreatmentScreen);
	}
}
