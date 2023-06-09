using System;
using System.Collections.Generic;
using UnityEngine;

namespace Koenigz.PerfectCulling;

public abstract class PerfectCullingBakeData : ScriptableObject
{
	[SerializeField]
	public int bakeDataVersion = 1;

	[SerializeField]
	public bool bakeCompleted;

	[SerializeField]
	public int bakeHash;

	[SerializeField]
	public string strBakeDate;

	[SerializeField]
	public long bakeDurationMilliseconds;

	public virtual ushort[] GetRawData(int index)
	{
		return null;
	}

	public virtual void SetRawData(int index, _E493 indices)
	{
		throw new NotImplementedException();
	}

	public virtual void SetRawData(int index, ushort[] indices)
	{
	}

	public unsafe virtual void SetRawDataMT(int index, int* buffer, int count)
	{
	}

	public virtual void SampleAtIndex(int index, List<ushort> indices)
	{
		throw new NotImplementedException();
	}

	public virtual void PrepareForBake(PerfectCullingBakingBehaviour bakingBehaviour)
	{
		throw new NotImplementedException();
	}

	public virtual void CompleteBake()
	{
		throw new NotImplementedException();
	}

	public virtual void DrawInspectorGUI()
	{
		throw new NotImplementedException();
	}
}
