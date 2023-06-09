using UnityEngine;

public class ExternalTerrainCollider : MonoBehaviour
{
	public Terrain Terrain;

	[Range(2f, 2048f)]
	public int GridX = 1024;

	[Range(2f, 2048f)]
	public int GridY = 1024;
}
