using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class MeshProjector : MonoBehaviour
{
	private LayerMask _E000 = -1;

	[SerializeField]
	[Range(0f, 1f)]
	private float _offset = 0.05f;

	[SerializeField]
	private float _raycastOffset = 10f;

	[HideInInspector]
	public bool UpdateByTimer;

	[HideInInspector]
	public float UpdateTime = 2f;

	private Transform _E001;

	private MeshFilter _E002;

	private const float _E003 = 50f;

	private float _E004;

	public static Action<int> OnMeshUpdated;
}
