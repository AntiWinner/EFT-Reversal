namespace EFT.Hideout;

public enum EAreaStatus
{
	NotSet,
	LockedToConstruct,
	ReadyToConstruct,
	Constructing,
	ReadyToInstallConstruct,
	LockedToUpgrade,
	ReadyToUpgrade,
	Upgrading,
	ReadyToInstallUpgrade,
	NoFutureUpgrades,
	AutoUpgrading
}
