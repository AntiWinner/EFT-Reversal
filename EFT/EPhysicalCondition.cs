using System;

namespace EFT;

[Flags]
public enum EPhysicalCondition
{
	None = 0,
	OnPainkillers = 1,
	LeftLegDamaged = 2,
	RightLegDamaged = 4,
	ProneDisabled = 8,
	LeftArmDamaged = 0x10,
	RightArmDamaged = 0x20,
	Tremor = 0x40,
	UsingMeds = 0x80,
	HealingLegs = 0x100,
	JumpDisabled = 0x200,
	SprintDisabled = 0x400,
	ProneMovementDisabled = 0x800
}
