namespace EFT.Quests;

public enum EQuestStatus
{
	Locked,
	AvailableForStart,
	Started,
	AvailableForFinish,
	Success,
	Fail,
	FailRestartable,
	MarkedAsFailed,
	Expired,
	AvailableAfter
}
