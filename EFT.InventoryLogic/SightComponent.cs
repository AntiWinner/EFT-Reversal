using UnityEngine;

namespace EFT.InventoryLogic;

public class SightComponent : _EB19
{
	public Vector3[][] OpticCalibrationPoints;

	private _E9E9 m__E000;

	[_E63C]
	public int[] ScopesCurrentCalibPointIndexes;

	[_E63C]
	public int[] ScopesSelectedModes;

	[_E63C]
	public int SelectedScope;

	public int ScopesCount => this.m__E000.ScopesCount;

	public string CustomAimPlane => this.m__E000.CustomAimPlane;

	public int SelectedScopeIndex
	{
		get
		{
			return SelectedScope;
		}
		set
		{
			if (ScopesCount > 0)
			{
				SelectedScope = value % ScopesCount;
			}
		}
	}

	public int SelectedScopeMode
	{
		get
		{
			if (SelectedScope >= ScopesSelectedModes.Length)
			{
				return 0;
			}
			return ScopesSelectedModes[SelectedScope];
		}
		set
		{
			ScopesSelectedModes[SelectedScope] = value;
			if (ScopesSelectedModes[SelectedScope] >= this.m__E000.ModesCount[SelectedScope])
			{
				ScopesSelectedModes[SelectedScope] = 0;
			}
		}
	}

	public float GetCurrentSensitivity
	{
		get
		{
			float[][] aimSensitivity = this.m__E000.AimSensitivity;
			if (SelectedScope >= aimSensitivity.Length)
			{
				return 1f;
			}
			int selectedScopeMode = SelectedScopeMode;
			if (selectedScopeMode < aimSensitivity[SelectedScope].Length)
			{
				return aimSensitivity[SelectedScope][selectedScopeMode];
			}
			return 1f;
		}
	}

	public void SetScopeMode(int scopeIndex, int mode)
	{
		if (scopeIndex >= 0 && scopeIndex < ScopesSelectedModes.Length)
		{
			ScopesSelectedModes[scopeIndex] = mode;
			if (ScopesSelectedModes[scopeIndex] >= this.m__E000.ModesCount[scopeIndex])
			{
				ScopesSelectedModes[scopeIndex] = 0;
			}
		}
	}

	public int GetScopeModesCount(int scopeIndex)
	{
		return this.m__E000.ModesCount[scopeIndex];
	}

	public int[] GetScopeCalibrationDistances(int scopeIndex)
	{
		if (scopeIndex < 0 || scopeIndex >= ScopesCount)
		{
			return null;
		}
		return this.m__E000.CalibrationDistances[scopeIndex];
	}

	public SightComponent(Item item, _E9E9 template)
		: base(item)
	{
		this.m__E000 = template;
		ScopesSelectedModes = new int[this.m__E000.ScopesCount];
		ScopesCurrentCalibPointIndexes = new int[this.m__E000.ScopesCount];
	}

	private int _E000(int scopeIndex)
	{
		if (scopeIndex >= ScopesCurrentCalibPointIndexes.Length)
		{
			return 0;
		}
		int num = ScopesCurrentCalibPointIndexes[scopeIndex];
		if (HasOpticCalibrationPoints(scopeIndex) && (num < 0 || num >= OpticCalibrationPoints[scopeIndex].Length))
		{
			ScopesCurrentCalibPointIndexes[scopeIndex] = 0;
		}
		return ScopesCurrentCalibPointIndexes[scopeIndex];
	}

	public bool HasOpticCalibrationPoints(int scopeIndex)
	{
		if (OpticCalibrationPoints != null && OpticCalibrationPoints.Length != 0 && OpticCalibrationPoints[scopeIndex] != null)
		{
			return OpticCalibrationPoints[scopeIndex].Length != 0;
		}
		return false;
	}

	public void SetScopeCaliabrationPoints(int scopeIndex, Vector3[] points)
	{
		OpticCalibrationPoints[scopeIndex] = points;
	}

	public Vector3 GetCurrentOpticCalibrationPoint()
	{
		if (!HasOpticCalibrationPoints(SelectedScope))
		{
			return Vector3.zero;
		}
		int num = _E000(SelectedScope);
		if (OpticCalibrationPoints[SelectedScope].Length <= num)
		{
			return Vector3.zero;
		}
		return OpticCalibrationPoints[SelectedScope][num];
	}

	public void SetSelectedOpticCalibrationPoint(int scope, int index)
	{
		if (HasOpticCalibrationPoints(scope))
		{
			ScopesCurrentCalibPointIndexes[scope] = Mathf.Clamp(index, 0, OpticCalibrationPoints[scope].Length - 1);
		}
	}

	public int GetCurrentOpticCalibrationDistance()
	{
		int[] array = this.m__E000.CalibrationDistances[SelectedScope];
		if (array.Length == 0)
		{
			return 0;
		}
		return array[_E000(SelectedScope)];
	}

	public float GetCurrentOpticZoom()
	{
		float[] array = this.m__E000.Zooms[SelectedScope];
		if (array.Length == 0)
		{
			return 0f;
		}
		return array[SelectedScopeMode];
	}

	public bool HasCurrentZoomGreaterThenOne()
	{
		float[] array = this.m__E000.Zooms[SelectedScope];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] > 1f)
			{
				return true;
			}
		}
		return false;
	}

	public bool OpticCalibrationPointUp()
	{
		if (!HasOpticCalibrationPoints(SelectedScope))
		{
			return false;
		}
		int num = ScopesCurrentCalibPointIndexes[SelectedScope];
		ScopesCurrentCalibPointIndexes[SelectedScope]++;
		if (ScopesCurrentCalibPointIndexes[SelectedScope] >= OpticCalibrationPoints[SelectedScope].Length)
		{
			ScopesCurrentCalibPointIndexes[SelectedScope] = OpticCalibrationPoints[SelectedScope].Length - 1;
		}
		return num != ScopesCurrentCalibPointIndexes[SelectedScope];
	}

	public bool OpticCalibrationPointDown()
	{
		if (!HasOpticCalibrationPoints(SelectedScope))
		{
			return false;
		}
		int num = ScopesCurrentCalibPointIndexes[SelectedScope];
		ScopesCurrentCalibPointIndexes[SelectedScope]--;
		if (ScopesCurrentCalibPointIndexes[SelectedScope] <= 0)
		{
			ScopesCurrentCalibPointIndexes[SelectedScope] = 0;
		}
		return num != ScopesCurrentCalibPointIndexes[SelectedScope];
	}
}
