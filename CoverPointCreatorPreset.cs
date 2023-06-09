using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CoverPointCreatorPreset
{
	[SerializeField]
	private string _name;

	[SerializeField]
	private DirectionCalculationType _directionCalculation;

	[SerializeField]
	private float _angleToRemovePointOnEdge = 6f;

	[SerializeField]
	private float _distanceBetweenCoversOnEdge = 3f;

	[SerializeField]
	private float _smallEdgeSize = 1f;

	[SerializeField]
	private float _clusterLargeDist = 0.8f;

	[SerializeField]
	private float _clusterAmbushDist = 1.8f;

	[SerializeField]
	private float _clusterLargeAngle = 50f;

	[SerializeField]
	private float _clusterNearDist = 0.4f;

	[SerializeField]
	private float _clusterFirePosDist = 0.75f;

	[SerializeField]
	private float _fireStepDist = 0.85f;

	[SerializeField]
	private bool _bothFireposIsBadCover;

	[SerializeField]
	private float _distanceRaycastMultiplier = 1f;

	[SerializeField]
	private float _distanceFireposMultiplier = 1f;

	[SerializeField]
	private float _ambushMinSegment = 1.5f;

	[SerializeField]
	private float _legWallCheck = 0.17f;

	[SerializeField]
	private float _legsCheckMinDist = 1.2f;

	[SerializeField]
	private bool _highQualityBorder;

	[FormerlySerializedAs("_checkLineCast")]
	[SerializeField]
	private bool _ignoreRayCast;

	[SerializeField]
	private bool _withCheckPointStay = true;

	[SerializeField]
	private float _checkOnNavMeshDist = 0.04f;

	[SerializeField]
	private float _sphereCastRadius = 0.1f;

	[SerializeField]
	private bool _removeCoversNearDoorAfterCalc = true;

	public string Name => _name;

	public DirectionCalculationType DirectionCalculation => _directionCalculation;

	public float ANGLE_TO_REMOVE_POINT_ON_EDGE => _angleToRemovePointOnEdge;

	public float DISTANCE_BETWEEN_COVERS_ON_EDGE => _distanceBetweenCoversOnEdge;

	public float SMALL_EDGE_SIZE => _smallEdgeSize;

	public float CLUSTER_LARGE_DIST => _clusterLargeDist;

	public float CLUSTER_AMBUSH_DIST => _clusterAmbushDist;

	public float CLUSTER_LARGE_ANGLE => _clusterLargeAngle;

	public float CLUSTER_NEAR_DIST => _clusterNearDist;

	public float CLUSTER_FIREPOS_DIST => _clusterFirePosDist;

	public float FIRE_STEP_DIST => _fireStepDist;

	public bool BOTH_FIREPOS_IS_BAD_COVER => _bothFireposIsBadCover;

	public float DISTANCE_RAYCAST_MULTIPLIER => _distanceRaycastMultiplier;

	public float DISTANCE_FIREPOS_MULTIPLIER => _distanceFireposMultiplier;

	public float AmbushMinSegment => _ambushMinSegment;

	public float LEG_WALL_CHECK => _legWallCheck;

	public float LegsCheckMinDist => _legsCheckMinDist;

	public bool HighQualityBorder => _highQualityBorder;

	public bool IgnoreLineCast => _ignoreRayCast;

	public bool WithCheckPointStay => _withCheckPointStay;

	public float CheckOnNavMeshDist => _checkOnNavMeshDist;

	public float SphereCastRadius => _sphereCastRadius;

	public bool RemoveCoversNearDoorAfterCalc => _removeCoversNearDoorAfterCalc;

	public static CoverPointCreatorPreset LoadDefault()
	{
		return ((CoverPointCreatorPresetCollection)Resources.Load(_ED3E._E000(30782))).Presets[0];
	}
}
