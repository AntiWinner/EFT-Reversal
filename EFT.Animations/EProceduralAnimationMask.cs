using System;

namespace EFT.Animations;

[Flags]
public enum EProceduralAnimationMask
{
	Breathing = 1,
	Walking = 2,
	MotionReaction = 4,
	ForceReaction = 8,
	Shooting = 0x10,
	DrawDown = 0x20,
	Aiming = 0x40
}
