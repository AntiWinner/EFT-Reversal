using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainNeighbors : MonoBehaviour
{
	public Terrain Left;

	public Terrain Right;

	public Terrain Top;

	public Terrain Bottom;

	private void Awake()
	{
		_E000();
	}

	private void OnValidate()
	{
		_E000();
	}

	private void _E000()
	{
		GetComponent<Terrain>().SetNeighbors(Left, Top, Right, Bottom);
	}
}
