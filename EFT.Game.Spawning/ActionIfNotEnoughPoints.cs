namespace EFT.Game.Spawning;

public enum ActionIfNotEnoughPoints
{
	ReturnFoundPoints,
	ReturnNothing,
	DuplicateIfAtLeastOne,
	FillWithDiscardedPointsAndDuplicates
}
