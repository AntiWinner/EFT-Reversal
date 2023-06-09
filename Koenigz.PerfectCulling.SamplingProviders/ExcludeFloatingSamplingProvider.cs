using UnityEngine;

namespace Koenigz.PerfectCulling.SamplingProviders;

[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(PerfectCullingBakingBehaviour))]
public class ExcludeFloatingSamplingProvider : SamplingProviderBase
{
	[SerializeField]
	[Tooltip("Which layers will be used as colliders")]
	private LayerMask layerMask = -1;

	[SerializeField]
	[Tooltip("Box XZ half size\nShould equal to max jump distance in horizontal plane from infinite height")]
	private float areaSize = 6f;

	[Tooltip("Player height")]
	[SerializeField]
	private float playerHeight = 3f;

	[SerializeField]
	private bool terrainVerticalCheck;

	[SerializeField]
	private float nearSurfaceCheckRadius;

	[SerializeField]
	[HideInInspector]
	private Vector3 boxSize;

	[SerializeField]
	private float _verticalVoidCheckDistance;

	private TerrainCollider[] _E001;

	private const float _E002 = 400f;

	public LayerMask CollisionMask => layerMask;

	public bool UseTerrainVerticalCheck
	{
		get
		{
			return terrainVerticalCheck;
		}
		set
		{
			terrainVerticalCheck = value;
		}
	}

	public override string Name => _ED3E._E000(66686) + string.Format(_ED3E._E000(66718), layerMask.value.ToString(), boxSize);

	public override void InitializeSamplingProvider()
	{
		_E001 = Object.FindObjectsOfType<TerrainCollider>();
	}

	private void OnValidate()
	{
		boxSize = new Vector3(areaSize, playerHeight, areaSize);
	}

	public override bool IsSamplingPositionActive(PerfectCullingBakingBehaviour bakingBehaviour, Vector3 pos)
	{
		if (terrainVerticalCheck)
		{
			TerrainCollider[] array = _E001;
			foreach (TerrainCollider obj in array)
			{
				Vector3 vector = pos + Vector3.up * 400f;
				if (obj.Raycast(new Ray(vector, -Vector3.up), out var hitInfo, 400f) && hitInfo.distance < (pos - vector).magnitude)
				{
					return false;
				}
			}
		}
		if (nearSurfaceCheckRadius > 0f && Physics.CheckSphere(pos, nearSurfaceCheckRadius, layerMask.value, QueryTriggerInteraction.Ignore))
		{
			return false;
		}
		if (_verticalVoidCheckDistance > 0f && Physics.Raycast(pos, -Vector3.up, out var hitInfo2, _verticalVoidCheckDistance, layerMask.value, QueryTriggerInteraction.Ignore) && hitInfo2.collider.gameObject.name == _ED3E._E000(66689))
		{
			return false;
		}
		return Physics.CheckBox(pos, boxSize, Quaternion.identity, layerMask.value, QueryTriggerInteraction.Ignore);
	}
}
