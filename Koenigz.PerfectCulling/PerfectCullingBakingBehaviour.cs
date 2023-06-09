using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Koenigz.PerfectCulling.EFT;
using UnityEngine;

namespace Koenigz.PerfectCulling;

public abstract class PerfectCullingBakingBehaviour : MonoBehaviour, _E49C
{
	public class _E000
	{
		private List<ushort>[] m__E000;

		public int NumCells => m__E000.Length;

		public List<ushort>[] RawCells => m__E000;

		public List<ushort> GetCellData(int index)
		{
			return m__E000[index];
		}

		public _E000(int numCells)
		{
			m__E000 = new List<ushort>[numCells];
			for (int i = 0; i < m__E000.Length; i++)
			{
				m__E000[i] = new List<ushort>();
			}
		}

		public void Dispose()
		{
			m__E000 = null;
		}
	}

	public sealed class _E001
	{
		public enum Mode
		{
			Overwrite,
			Append,
			Union,
			SuperSample
		}

		[CompilerGenerated]
		private int[] _E000;

		[CompilerGenerated]
		private Vector3[] m__E001;

		[CompilerGenerated]
		private _E000 _E002;

		[CompilerGenerated]
		private PerfectCullingSuperSamplingVolume _E003;

		[CompilerGenerated]
		private Mode _E004;

		public int[] CellIndices
		{
			[CompilerGenerated]
			get
			{
				return _E000;
			}
			[CompilerGenerated]
			private set
			{
				_E000 = value;
			}
		}

		public Vector3[] SamplingPoints
		{
			[CompilerGenerated]
			get
			{
				return m__E001;
			}
			[CompilerGenerated]
			private set
			{
				m__E001 = value;
			}
		}

		public _E000 OutputData
		{
			[CompilerGenerated]
			get
			{
				return _E002;
			}
			[CompilerGenerated]
			private set
			{
				_E002 = value;
			}
		}

		public PerfectCullingSuperSamplingVolume OptionalVolume
		{
			[CompilerGenerated]
			get
			{
				return _E003;
			}
			[CompilerGenerated]
			set
			{
				_E003 = value;
			}
		}

		public Mode WriteMode
		{
			[CompilerGenerated]
			get
			{
				return _E004;
			}
			[CompilerGenerated]
			private set
			{
				_E004 = value;
			}
		}

		public void RemapIds()
		{
		}

		public _E001(int[] cellIndices, Vector3[] samplingPoints, Mode writeMode, PerfectCullingSuperSamplingVolume optionalVolume = null)
		{
			OptionalVolume = optionalVolume;
			CellIndices = cellIndices;
			SamplingPoints = samplingPoints;
			OutputData = new _E000(CellIndices.Length);
			WriteMode = writeMode;
		}

		public void Dispose()
		{
			OptionalVolume = null;
			CellIndices = null;
			SamplingPoints = null;
			OutputData.Dispose();
			OutputData = null;
		}
	}

	private static _E4A5 m__E000 = new _E4A4();

	public HashSet<_E4A5> SamplingProviders = new HashSet<_E4A5> { PerfectCullingBakingBehaviour.m__E000 };

	[HideInInspector]
	[SerializeField]
	public PerfectCullingBakeGroup[] _bakeGroups = Array.Empty<PerfectCullingBakeGroup>();

	[SerializeField]
	public List<Renderer> _additionalOccluders = new List<Renderer>();

	[CompilerGenerated]
	private readonly PerfectCullingBakeData m__E001;

	[NonSerialized]
	public int TotalVertexCount;

	[CompilerGenerated]
	private static bool _E002;

	public virtual PerfectCullingBakeGroup[] bakeGroups
	{
		get
		{
			return _bakeGroups;
		}
		set
		{
			_bakeGroups = value;
		}
	}

	public virtual List<Renderer> additionalOccluders
	{
		get
		{
			return _additionalOccluders;
		}
		set
		{
			_additionalOccluders = value;
		}
	}

	public virtual PerfectCullingBakeData BakeData
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
	}

	public static bool IsBakeActive
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}

	public void AddSamplingProvider(_E4A5 samplingProvider)
	{
		SamplingProviders.Add(samplingProvider);
	}

	public void RemoveSamplingProvider(_E4A5 samplingProvider)
	{
		SamplingProviders.Remove(samplingProvider);
	}

	public void InitializeAllSamplingProviders()
	{
		foreach (_E4A5 samplingProvider in SamplingProviders)
		{
			samplingProvider.InitializeSamplingProvider();
		}
	}

	public bool SamplingProvidersIsPositionActive(Vector3 pos)
	{
		foreach (_E4A5 samplingProvider in SamplingProviders)
		{
			if (!samplingProvider.IsSamplingPositionActive(this, pos))
			{
				return false;
			}
		}
		return true;
	}

	public virtual void Start()
	{
	}

	public void ToggleAllRenderers(bool state, bool forceNullCheck = false)
	{
		PerfectCullingBakeGroup[] array = bakeGroups;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Toggle(state);
		}
	}

	public virtual void SetBakeData(PerfectCullingBakeData bakeData)
	{
		throw new NotImplementedException();
	}

	public virtual List<Vector3> GetSamplingPositions(Space space = Space.Self)
	{
		throw new NotImplementedException();
	}

	public virtual Vector3 GetWorldPositionForIndex(int index)
	{
		return Vector3.zero;
	}

	public virtual void GetIndicesForWorldPos(Vector3 worldPos, List<ushort> indices)
	{
		throw new NotImplementedException();
	}

	public virtual int GetIndexForWorldPos(Vector3 worldPos, out bool isOutOfBounds)
	{
		throw new NotImplementedException();
	}

	public virtual void GetIndicesForIndex(int index, List<ushort> indices)
	{
		BakeData.SampleAtIndex(index, indices);
	}

	public virtual bool PreBake()
	{
		throw new NotImplementedException();
	}

	public virtual void PostBake()
	{
		throw new NotImplementedException();
	}

	public virtual int GetBakeHash()
	{
		throw new NotImplementedException();
	}

	protected virtual void CullAdditionalOccluders(ref HashSet<Renderer> additionalOccluders)
	{
		throw new NotImplementedException();
	}

	public virtual void InitializeBake()
	{
	}

	public virtual void PostProcessBakeData(PerfectCullingBakeData data)
	{
	}
}
