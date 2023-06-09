namespace EFT;

public interface IProfileDataContainer
{
	string ProfileId { get; }

	string Nickname { get; }

	EPlayerSide Side { get; }
}
