using System.Collections.Generic;
using UnityEngine;

namespace EFT.Visual;

public sealed class JackOLantern : Dress
{
	[SerializeField]
	private VolumetricLight _volumetricLight;

	private Transform _E001;

	protected override void OnSkin(Transform playerTransform, Transform meshTransform)
	{
		Transform transform = base.transform;
		_E001 = transform.parent;
		transform.SetParent(PlayerBody.PlayerBones.Head.Original, worldPositionStays: false);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	public override void Unskin()
	{
		base.transform.SetParent(_E001, worldPositionStays: false);
		base.Unskin();
	}

	protected override IEnumerable<Renderer> GetRenderers()
	{
		return GetComponentsInChildren<MeshRenderer>(includeInactive: true);
	}

	protected override void AdjustShadowMode(EPointOfView pointOfView)
	{
		base.AdjustShadowMode(pointOfView);
		if (pointOfView == EPointOfView.ThirdPerson)
		{
			_volumetricLight.enabled = true;
			return;
		}
		_volumetricLight.enabled = false;
		_volumetricLight.Light.RemoveAllCommandBuffers();
	}
}
