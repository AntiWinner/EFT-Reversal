using System;
using UnityEngine;

[Serializable]
public class CoverPointPlaceSerializable
{
	public int Id = -1;

	public Vector3 Origin;

	public int[] NeighbourhoodIds;

	public float DefenceLevel;

	public CoverPointDefenceInfo DefenceInfo;

	public CoverType CoverType;

	public bool IsGoodInsideBuilding;

	public ECoverPointSpecial Special;

	public EnvironmentType EnvironmentType;

	public int IdEnvironment;

	public CoverPointPlaceSerializable(Vector3 origin, CoverPointDefenceInfo defenceInfo, CoverType coverType, bool isGoodInside)
	{
		Special = (ECoverPointSpecial)0;
		IsGoodInsideBuilding = isGoodInside;
		DefenceInfo = defenceInfo;
		Origin = origin;
		CoverType = coverType;
	}
}
