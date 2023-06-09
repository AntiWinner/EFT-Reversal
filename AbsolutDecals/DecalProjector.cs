using System;
using System.Collections;
using EFT.Ballistics;
using UnityEngine;

namespace AbsolutDecals;

[ExecuteInEditMode]
public class DecalProjector : MonoBehaviour
{
	[Serializable]
	public enum ProjectionDirections
	{
		PositiveX,
		NegativeX,
		PositiveY,
		NegativeY,
		PositiveZ,
		NegativeZ
	}

	public enum ProjectorState
	{
		Unbaked,
		BakedToUniqueMesh,
		BakedToCommonMesh,
		BakedInProjectorSpace,
		WaitingForSaveOnDisc
	}

	[SerializeField]
	public DecalSystem.SingleDecal Decal;

	private ProjectionDirections _projectionDirection = ProjectionDirections.NegativeY;

	[SerializeField]
	public bool ShowSelf = true;

	[SerializeField]
	private bool _updateOnlyWhenChange = true;

	[SerializeField]
	private bool _bakeToReusableMesh;

	private Vector3 _bakedPosition;

	private MeshRenderer _meshRenderer;

	private MeshFilter _meshFilter;

	private Vector3 _prevPosition;

	private Vector3 _prevRotation;

	[HideInInspector]
	public ProjectorState CurrentState;

	private static int _index;

	private static bool _locked;

	public bool DontBakeInEditor { get; set; }

	public Transform Transform { get; private set; }

	public ProjectionDirections ProjectionDirection
	{
		get
		{
			return _projectionDirection;
		}
		set
		{
			_projectionDirection = value;
		}
	}

	public bool BakedInReusableMesh
	{
		get
		{
			return _bakeToReusableMesh;
		}
		set
		{
			_bakeToReusableMesh = value;
		}
	}

	public Vector3 ProjectionDirectionVector => -Transform.up;

	public Vector3 ProjectionDirectionVectorLocal => -Vector3.up;

	private void Update()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (CurrentState != 0)
		{
			if (CurrentState != ProjectorState.BakedInProjectorSpace)
			{
				Transform.position = _bakedPosition;
			}
			return;
		}
		if (_updateOnlyWhenChange)
		{
			if ((_prevPosition - Transform.position).sqrMagnitude < Mathf.Epsilon && (_prevRotation - Transform.eulerAngles).sqrMagnitude < Mathf.Epsilon)
			{
				return;
			}
			_prevPosition = Transform.position;
			_prevRotation = Transform.eulerAngles;
		}
		_E449 decalMesh = MonoBehaviourSingleton<DecalSystem>.Instance.GetDecalMesh(this, ShowSelf);
		if (ShowSelf && _meshFilter != null && decalMesh != null)
		{
			_meshFilter.sharedMesh = decalMesh.ToMesh();
			_meshRenderer.enabled = true;
		}
		else if (ShowSelf && _meshFilter != null && decalMesh == null)
		{
			_meshRenderer.enabled = false;
		}
	}

	private void DrawGizmo(bool selected)
	{
		Color color = new Color(0f, 0.5f, 1f, 1f);
		if (CurrentState == ProjectorState.BakedToCommonMesh)
		{
			color = ((!_bakeToReusableMesh) ? new Color(0f, 1f, 0f, 1f) : new Color(0f, 1f, 0.8f, 1f));
		}
		if (CurrentState == ProjectorState.BakedToUniqueMesh)
		{
			color = new Color(1f, 1f, 0f, 1f);
		}
		if (CurrentState == ProjectorState.BakedInProjectorSpace)
		{
			color = new Color(0.3f, 0f, 0.7f, 1f);
		}
		if (!Application.isPlaying && DontBakeInEditor)
		{
			color = new Color(0.7f, 0f, 0f);
		}
		color.a = (selected ? 0.3f : 0.1f);
		Gizmos.color = color;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		color.a = (selected ? 0.5f : 0.2f);
		Gizmos.color = color;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}

	private void Start()
	{
		Transform = base.transform;
		MonoBehaviourSingleton<DecalSystem>.Instance.RegisterProjector(this);
		DecalSystem.CallProjectors = (Action)Delegate.Combine(DecalSystem.CallProjectors, (Action)delegate
		{
			MonoBehaviourSingleton<DecalSystem>.Instance.RegisterProjector(this);
		});
		if (CurrentState != 0)
		{
			MonoBehaviourSingleton<DecalSystem>.Instance.UnbakeDecal(this);
			ResetState();
		}
		if (ShowSelf)
		{
			InitializeMesh();
		}
	}

	private void InitializeMesh()
	{
		if (base.gameObject.GetComponent<MeshRenderer>() != null)
		{
			_meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
			_meshFilter = base.gameObject.GetComponent<MeshFilter>();
		}
		else
		{
			_meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			_meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		_meshRenderer.sharedMaterial = Decal.DecalMaterial;
	}

	public void OnDrawGizmos()
	{
		DrawGizmo(selected: false);
	}

	public void OnDrawGizmosSelected()
	{
		DrawGizmo(selected: true);
	}

	private void OnDestroy()
	{
		if (MonoBehaviourSingleton<DecalSystem>.Instance != null)
		{
			UnbakeFromMesh();
			MonoBehaviourSingleton<DecalSystem>.Instance.UnregisterProjector(this, destroyMesh: true);
		}
		ResetState();
	}

	public void BakeToMesh()
	{
		if (CurrentState != 0)
		{
			return;
		}
		DecalSystem.BakeResult bakeResult = MonoBehaviourSingleton<DecalSystem>.Instance.BakeDecal(this, _bakeToReusableMesh);
		if (bakeResult == DecalSystem.BakeResult.Unsuccess || bakeResult == DecalSystem.BakeResult.BakedToSkinnedMesh)
		{
			if (Application.isPlaying)
			{
				base.gameObject.SetActive(value: false);
			}
			return;
		}
		CurrentState = ProjectorState.BakedToCommonMesh;
		_bakedPosition = Transform.position;
		if (_meshRenderer != null)
		{
			_meshRenderer.enabled = false;
		}
	}

	public void BakeToUniqueMesh(bool inProjectorSpace = false)
	{
		if (CurrentState != 0)
		{
			return;
		}
		if (!inProjectorSpace)
		{
			if (MonoBehaviourSingleton<DecalSystem>.Instance.BakeUniqueDecal(this))
			{
				_bakedPosition = Transform.position;
				CurrentState = ProjectorState.BakedToUniqueMesh;
				if (_meshRenderer != null)
				{
					_meshRenderer.enabled = false;
				}
			}
			return;
		}
		_E449 decalMesh = MonoBehaviourSingleton<DecalSystem>.Instance.GetDecalMesh(this, inProjectorSpace: true);
		if (decalMesh != null && decalMesh.IsSkinned)
		{
			MonoBehaviourSingleton<DecalSystem>.Instance.BakeToSkinnedMesh(decalMesh, this);
			CurrentState = ProjectorState.BakedInProjectorSpace;
			return;
		}
		if (_meshFilter == null || _meshRenderer == null)
		{
			InitializeMesh();
		}
		if (!(_meshFilter == null) && decalMesh != null)
		{
			_meshFilter.mesh = decalMesh.ToMesh();
			CurrentState = ProjectorState.BakedInProjectorSpace;
			_bakedPosition = Transform.position;
		}
	}

	public void ResetState()
	{
		CurrentState = ProjectorState.Unbaked;
	}

	public void UnbakeFromMesh()
	{
		if (CurrentState != 0)
		{
			MonoBehaviourSingleton<DecalSystem>.Instance.UnbakeDecal(this);
			CurrentState = ProjectorState.Unbaked;
			if (_meshRenderer != null)
			{
				_meshRenderer.enabled = true;
			}
		}
	}

	public static void CreateAndBakeProjector(_EC26 hit)
	{
		if (_locked || MonoBehaviourSingleton<DecalSystem>.Instance == null)
		{
			return;
		}
		DecalSystem.SingleDecal decal = MonoBehaviourSingleton<DecalSystem>.Instance.GetDecal(hit.HittedBallisticCollider.TypeOfMaterial);
		BakeDecal(hit, decal);
		if (hit.HittedBallisticCollider.TypeOfMaterial == MaterialType.Body)
		{
			LayerMask layerMask = 1 << LayerMask.NameToLayer("HighPolyCollider");
			if (Physics.Raycast(hit.HitPoint, hit.Direction, out var hitInfo, 3f, layerMask))
			{
				DrawBloodOnWall(hitInfo);
			}
		}
		_locked = true;
	}

	private static void DrawBloodOnWall(RaycastHit hit)
	{
		DecalSystem.SingleDecal bloodOnWallsMaterial = MonoBehaviourSingleton<DecalSystem>.Instance.BloodOnWallsMaterial;
		BakeDecal(hit.point, hit.normal, hit.collider, bloodOnWallsMaterial);
	}

	private static void BakeDecal(Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider, DecalSystem.SingleDecal decal)
	{
		bool flag = true;
		if (hitCollider != null)
		{
			flag = hitCollider.gameObject.isStatic;
		}
		Material decalMaterial = decal.DecalMaterial;
		GameObject gameObject = new GameObject("Projector" + decalMaterial.name + _index);
		_index++;
		gameObject.transform.position = hitPoint;
		gameObject.transform.up = hitNormal;
		gameObject.transform.localScale = decal.GetScale();
		if (decal.CanRotate)
		{
			gameObject.transform.Rotate(Vector3.up * UnityEngine.Random.Range(0f, 360f), Space.Self);
		}
		if (flag)
		{
			gameObject.transform.parent = MonoBehaviourSingleton<DecalSystem>.Instance.ProjectorsParent.transform;
		}
		else
		{
			gameObject.transform.parent = hitCollider.transform;
		}
		DecalProjector decalProjector = gameObject.AddComponent<DecalProjector>();
		decalProjector.ShowSelf = false;
		decalProjector.BakedInReusableMesh = true;
		decalProjector.ProjectionDirection = ProjectionDirections.NegativeY;
		decalProjector.Decal = decal;
		decalProjector.Start();
		decalProjector.BakeDelayed(flag);
	}

	private static void BakeDecal(_EC26 hit, DecalSystem.SingleDecal decal)
	{
		BakeDecal(hit.HitPoint, hit.HitNormal, hit.HitCollider, decal);
	}

	private void BakeDelayed(bool isStatic)
	{
		StartCoroutine(WaitAndBake(isStatic));
		Invoke("Unlock", 0.1f);
	}

	private void Unlock()
	{
		_locked = false;
	}

	private IEnumerator WaitAndBake(bool isStaic)
	{
		yield return new WaitForEndOfFrame();
		if (isStaic)
		{
			BakeToMesh();
		}
		else
		{
			BakeToUniqueMesh(inProjectorSpace: true);
		}
	}
}
