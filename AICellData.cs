using System;
using UnityEngine;

[Serializable]
public class AICellData
{
	[SerializeField]
	public AICell[] List;

	public float CellSize;

	public float StartX;

	public float StartZ;

	public int MaxIx;

	public int MaxIz;

	public void ValidateOnStart()
	{
		if (CellSize < 3f)
		{
			Debug.LogError(_ED3E._E000(25561));
		}
		if (MaxIx < 3)
		{
			Debug.LogError(_ED3E._E000(27679));
		}
		if (MaxIz < 3)
		{
			Debug.LogError(_ED3E._E000(27681));
		}
		if (List.Length < 3)
		{
			Debug.LogError(_ED3E._E000(27755));
		}
	}

	public void SetCell(int i, int j, AICell cell)
	{
		List[i + j * MaxIx] = cell;
	}

	public AICell GetCell(int i, int j)
	{
		return List[i + j * MaxIx];
	}

	public void SetCellSize(float val)
	{
		if (val < 3f)
		{
			Debug.LogError(_ED3E._E000(27820));
			val = 10f;
		}
		CellSize = val;
	}
}
