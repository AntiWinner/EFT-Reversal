namespace EFT.Interactive;

public enum EExfiltrationStatus : byte
{
	NotPresent = 1,
	UncompleteRequirements,
	Countdown,
	RegularMode,
	Pending,
	AwaitsManualActivation
}
