using System.Collections.Generic;
using System.Linq;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.Rendering;

namespace EFT.Visual;

public class Dress : MonoBehaviour, _E7F4
{
	public EDecalTextureType DecalTextureType;

	protected PlayerBody PlayerBody;

	protected readonly _E3A4 CompositeDisposable = new _E3A4();

	[SerializeField]
	protected Renderer[] Renderers;

	public virtual void Init(PlayerBody playerBody)
	{
		CompositeDisposable.Dispose();
		PlayerBody = playerBody;
		if (Renderers == null || Renderers.Length == 0)
		{
			Renderers = GetRenderers().ToArray();
		}
		CompositeDisposable.BindState(PlayerBody.PointOfView, AdjustShadowMode);
	}

	public void Skin(Transform playerTransform, Transform meshTransform)
	{
		OnSkin(playerTransform, meshTransform);
		LODGroup[] componentsInChildren = GetComponentsInChildren<LODGroup>();
		foreach (LODGroup obj in componentsInChildren)
		{
			float size = obj.size;
			obj.RecalculateBounds();
			obj.size = size;
		}
	}

	public virtual void Init(Item item, bool isAnimated)
	{
	}

	public virtual void Deinit()
	{
		AdjustShadowMode(EPointOfView.ThirdPerson);
	}

	protected virtual void OnSkin(Transform playerTransform, Transform meshTransform)
	{
	}

	public virtual void Unskin()
	{
		CompositeDisposable.Dispose();
		PlayerBody = null;
	}

	public _E3D2 GetBodyRenderer()
	{
		_E3D2 result = default(_E3D2);
		result.DecalType = DecalTextureType;
		result.Renderers = Renderers;
		return result;
	}

	protected virtual IEnumerable<Renderer> GetRenderers()
	{
		return Renderers;
	}

	protected virtual void AdjustShadowMode(EPointOfView pointOfView)
	{
		foreach (Renderer renderer in GetRenderers())
		{
			renderer.shadowCastingMode = ((pointOfView != 0) ? ShadowCastingMode.On : ShadowCastingMode.ShadowsOnly);
		}
	}
}
