using System;
using EFT.Animations;

[Serializable]
public class Val
{
	public enum Source
	{
		MouseXVel,
		MouseXAcc,
		MouseYVel,
		MouseYAcc,
		MoveXVel,
		MoveYVel,
		MoveZVel,
		MoveXAcc,
		MoveYAcc,
		MoveZAcc
	}

	public Source From;

	public Target To;

	public ComponentType Component;

	public float Intensity;
}
