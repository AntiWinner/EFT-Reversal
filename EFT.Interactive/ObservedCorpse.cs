using System.Collections;
using UnityEngine;

namespace EFT.Interactive;

public class ObservedCorpse : Corpse
{
	private bool _E050;

	private bool _E051;

	private IEnumerator _E052;

	private bool _E053;

	public override float PhysicsQuality => 0f;

	protected override CollisionDetectionMode CollisionDetectionMode => CollisionDetectionMode.Continuous;

	protected override bool CheckCorpseIsStill(bool sleeping, float timePass)
	{
		_E051 = (_E050 && sleeping) || timePass >= 30f;
		return _E051;
	}

	public void ApplyNetPacket(_E5C4 packet)
	{
		_E050 = packet.Done;
		if (!packet.IsNotValidPosition)
		{
			_E000(packet.Position, _E2B6.Config.CorpseSyncThreshold);
			_E003();
			if (_E050 && _E052 == null)
			{
				_E052 = _E001(packet.Position, _E2B6.Config.CorpseSyncThreshold, packet.TransformSyncs);
				StartCoroutine(_E052);
			}
		}
		else if (_E050 && _E052 == null)
		{
			_E052 = _E002(packet.TransformSyncs);
			StartCoroutine(_E052);
		}
	}

	private float _E000(Vector3 position, float corpseSyncThreshold)
	{
		Vector3 vector = position - _pelvis.position;
		float num = vector.magnitude;
		float num2 = num - corpseSyncThreshold;
		if (num2 > 0f)
		{
			_pelvis.position += vector.normalized * num2;
			num = corpseSyncThreshold;
			Ragdoll.WakeUp();
		}
		return num;
	}

	private IEnumerator _E001(Vector3 desiredPosition, float corpseSyncThreshold, _E53B[] packetTransformSyncs)
	{
		while (_E050 && !_E051)
		{
			yield return null;
			float num = _E000(desiredPosition, corpseSyncThreshold);
			if (num < corpseSyncThreshold)
			{
				corpseSyncThreshold = num;
			}
			_E003();
		}
		if (!_E053)
		{
			yield return _E002(packetTransformSyncs);
		}
	}

	private IEnumerator _E002(_E53B[] packetTransformSyncs)
	{
		while (!_E051 || PlayerBody.IsVisible())
		{
			yield return null;
		}
		ApplyTransformSync(packetTransformSyncs);
	}

	private void _E003()
	{
		if (!_E053 && PlayerBody.IsVisible() && _E8A8.Instance.SqrDistance(base.transform.position) < EFTHardSettings.Instance.CorpseApplyHardSyncMinDistanceSqr)
		{
			_E053 = true;
		}
	}
}
