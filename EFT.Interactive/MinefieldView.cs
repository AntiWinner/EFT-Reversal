using Systems.Effects;
using Comfort.Common;
using UnityEngine;

namespace EFT.Interactive;

public sealed class MinefieldView : MonoBehaviour
{
	private const float m__E000 = 3f;

	[SerializeField]
	private Minefield _minefield;

	[SerializeField]
	private string _effectName;

	[SerializeField]
	private float _cameraShakeRange = 30f;

	[SerializeField]
	private Vector3 _explosionLocalPosition;

	[SerializeField]
	private bool _raycastToTheGround;

	[SerializeField]
	private LayerMask _raycastMask;

	private void Awake()
	{
		_minefield.PlayerShotEvent += _E000;
	}

	private void OnDestroy()
	{
		_minefield.PlayerShotEvent -= _E000;
	}

	private void _E000(Player player, BorderZone zone, float arg3, bool willHit)
	{
		Vector3 vector = player.Transform?.TransformPoint(_explosionLocalPosition) ?? player.Position;
		if (_raycastToTheGround && Physics.Raycast(vector + Vector3.up, Vector3.down, out var hitInfo, 3f, _raycastMask))
		{
			vector = hitInfo.point;
		}
		Singleton<Effects>.Instance.EmitGrenade(_effectName, vector, Vector3.up);
		Player[] getAllPlayersInArea = zone.GetAllPlayersInArea;
		foreach (Player player2 in getAllPlayersInArea)
		{
			if (player2.PointOfView == EPointOfView.FirstPerson)
			{
				StartCoroutine(player2.ProceduralWeaponAnimation.ForceReact.GrenadeShake_CO(1f - Vector3.Distance(vector, player2.Position) / _cameraShakeRange));
				continue;
			}
			Collider collider = player.PlayerBones.BodyPartCollidersDictionary[EBodyPartColliderType.Ribcage].Collider;
			Transform transform = collider.transform;
			Vector3 normalized = (transform.position - vector).normalized;
			Vector3 point = collider.ClosestPoint(transform.position + normalized);
			player.HitReaction.Hit(collider, normalized, point);
		}
	}
}
