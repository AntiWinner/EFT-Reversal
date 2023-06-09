using UnityEngine;

namespace EFT.InventoryLogic;

public struct ExtraSize
{
	public int Left;

	public int Right;

	public int Up;

	public int Down;

	public int ForcedLeft;

	public int ForcedRight;

	public int ForcedUp;

	public int ForcedDown;

	public static ExtraSize Merge(ExtraSize op1, ExtraSize op2)
	{
		ExtraSize result = default(ExtraSize);
		result.Left = Mathf.Max(op1.Left, op2.Left);
		result.Right = Mathf.Max(op1.Right, op2.Right);
		result.Up = Mathf.Max(op1.Up, op2.Up);
		result.Down = Mathf.Max(op1.Down, op2.Down);
		result.ForcedLeft = op1.ForcedLeft + op2.ForcedLeft;
		result.ForcedRight = op1.ForcedRight + op2.ForcedRight;
		result.ForcedUp = op1.ForcedUp + op2.ForcedUp;
		result.ForcedDown = op1.ForcedDown + op2.ForcedDown;
		return result;
	}

	public _E313 Apply(int width, int height)
	{
		return new _E313(width + Left + Right + ForcedLeft + ForcedRight, height + Up + Down + ForcedUp + ForcedDown);
	}

	public override string ToString()
	{
		return _ED3E._E000(201889) + Left + _ED3E._E000(201944) + Right + _ED3E._E000(201937) + Up + _ED3E._E000(201935) + Down + _ED3E._E000(201927) + ForcedLeft + _ED3E._E000(201973) + ForcedRight + _ED3E._E000(201956) + ForcedUp + _ED3E._E000(202008) + ForcedDown;
	}
}
