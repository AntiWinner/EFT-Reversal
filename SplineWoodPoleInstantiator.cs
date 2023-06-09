using System.Collections.Generic;
using UnityEngine;

public class SplineWoodPoleInstantiator : SplinePrefabInstantiator
{
	public Vector2 RandomOffset;

	private List<Transform> _E002 = new List<Transform>();

	public float WireShapeSize = 1f;

	public int WireShapeDetails = 5;

	public int WireRestart = 5;

	public int WireParts = 15;

	public float Gravity = 1f;

	public Vector2 GravityRandom = new Vector2(0.01f, 0.3f);

	public Material WireMaterial;

	public bool AutoGenerateWires = true;

	public bool AddLods = true;

	public float CullingRate = 1f;

	public bool OrientationByWorldUp = true;

	public List<WireGenerator> WireGenerators = new List<WireGenerator>();

	public float WireSpawnProbability = 1f;

	public bool DontGenerateWires;

	public bool CastShadows = true;

	private int _E003 = -1;

	private string[] _E004;

	private Transform _E005;

	private bool _E006;

	public bool Hold;
}
