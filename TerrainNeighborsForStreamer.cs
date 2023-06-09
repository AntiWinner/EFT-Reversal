using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainNeighborsForStreamer : MonoBehaviour
{
	public int X;

	public int Y;

	public int ArrayWidth;

	public int ArrayHeight;

	[CompilerGenerated]
	private Terrain m__E000;

	[CompilerGenerated]
	private Terrain m__E001;

	[CompilerGenerated]
	private Terrain _E002;

	[CompilerGenerated]
	private Terrain _E003;

	private Terrain _E004;

	private static TerrainNeighborsForStreamer[,] _E005;

	public Terrain Left
	{
		[CompilerGenerated]
		private get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		set
		{
			this.m__E000 = value;
		}
	}

	public Terrain Right
	{
		[CompilerGenerated]
		private get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		set
		{
			this.m__E001 = value;
		}
	}

	public Terrain Top
	{
		[CompilerGenerated]
		private get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}

	public Terrain Bottom
	{
		[CompilerGenerated]
		private get
		{
			return _E003;
		}
		[CompilerGenerated]
		set
		{
			_E003 = value;
		}
	}

	private void Awake()
	{
		if (_E005 == null)
		{
			_E005 = new TerrainNeighborsForStreamer[ArrayWidth, ArrayHeight];
		}
		_E004 = GetComponent<Terrain>();
		_E000(remove: false);
	}

	private void OnDestroy()
	{
		_E000(remove: true);
	}

	private void _E000(bool remove)
	{
		_E005[X, Y] = (remove ? null : this);
		TerrainNeighborsForStreamer terrainNeighborsForStreamer = _E001(X - 1, Y);
		TerrainNeighborsForStreamer terrainNeighborsForStreamer2 = _E001(X + 1, Y);
		TerrainNeighborsForStreamer terrainNeighborsForStreamer3 = _E001(X, Y + 1);
		TerrainNeighborsForStreamer terrainNeighborsForStreamer4 = _E001(X, Y - 1);
		Terrain terrain = (remove ? null : _E004);
		if (terrainNeighborsForStreamer != null)
		{
			terrainNeighborsForStreamer.Right = terrain;
			terrainNeighborsForStreamer.UpdateNeighbors();
			Left = terrainNeighborsForStreamer._E004;
		}
		if (terrainNeighborsForStreamer2 != null)
		{
			terrainNeighborsForStreamer2.Left = terrain;
			terrainNeighborsForStreamer2.UpdateNeighbors();
			Right = terrainNeighborsForStreamer2._E004;
		}
		if (terrainNeighborsForStreamer3 != null)
		{
			terrainNeighborsForStreamer3.Bottom = terrain;
			terrainNeighborsForStreamer3.UpdateNeighbors();
			Top = terrainNeighborsForStreamer3._E004;
		}
		if (terrainNeighborsForStreamer4 != null)
		{
			terrainNeighborsForStreamer4.Top = terrain;
			terrainNeighborsForStreamer4.UpdateNeighbors();
			Bottom = terrainNeighborsForStreamer4._E004;
		}
		if (!remove)
		{
			UpdateNeighbors();
		}
	}

	private TerrainNeighborsForStreamer _E001(int x, int y)
	{
		if (x < 0 || y < 0 || x >= ArrayWidth || y >= ArrayHeight)
		{
			return null;
		}
		return _E005[x, y];
	}

	public void UpdateNeighbors()
	{
		_E004.SetNeighbors(Left, Top, Right, Bottom);
	}
}
