using EFT.Ballistics;
using UnityEngine;

namespace EFT.Interactive;

public sealed class BrokenWindowPieceTemplate : MonoBehaviour
{
	[SerializeField]
	private MeshFilter _meshFilter;

	[SerializeField]
	private MeshRenderer _meshRenderer;

	[SerializeField]
	private MeshCollider _meshCollider;

	[SerializeField]
	private BallisticCollider _ballisticCollider;

	[SerializeField]
	private GameObject _child;

	[SerializeField]
	private BoxCollider _childBoxCollider;

	[SerializeField]
	private BrokenWindowPieceCollider _childFragileGlassCollider;

	public _EC12 GetPiece()
	{
		_EC12 result = default(_EC12);
		result.GameObject = base.gameObject;
		result.Transform = base.transform;
		result.MeshFilter = _meshFilter;
		result.MeshRenderer = _meshRenderer;
		result.MeshCollider = _meshCollider;
		result.BallisticCollider = _ballisticCollider;
		result.ChildTransform = _child.transform;
		result.ChildBoxCollider = _childBoxCollider;
		result.ChildFragileCollider = _childFragileGlassCollider;
		return result;
	}
}
