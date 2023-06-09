using System.Collections.Generic;
using Diz.Skinning;
using UnityEngine;

namespace EFT.Visual;

public sealed class SkinDress : Dress
{
	[SerializeField]
	private AbstractSkin[] _lods;

	public override void Init(PlayerBody playerBody)
	{
		base.Init(playerBody);
		Skeleton skeletonRootJoint = playerBody.SkeletonRootJoint;
		AbstractSkin[] lods = _lods;
		foreach (AbstractSkin abstractSkin in lods)
		{
			if ((object)abstractSkin == null)
			{
				continue;
			}
			if (!(abstractSkin is Skin skin))
			{
				if (!(abstractSkin is TorsoSkin torsoSkin))
				{
					if (!(abstractSkin is BackpackSkin backpackSkin))
					{
						if (abstractSkin is VestSkin vestSkin)
						{
							vestSkin.Init(skeletonRootJoint, playerBody);
						}
					}
					else
					{
						backpackSkin.Init(skeletonRootJoint, playerBody);
					}
				}
				else
				{
					torsoSkin.Init(skeletonRootJoint, playerBody);
				}
			}
			else
			{
				skin.Init(skeletonRootJoint);
			}
		}
	}

	protected override void OnSkin(Transform playerTransform, Transform meshTransform)
	{
		AbstractSkin[] lods = _lods;
		for (int i = 0; i < lods.Length; i++)
		{
			lods[i].ApplySkin();
		}
	}

	public override void Unskin()
	{
		AbstractSkin[] lods = _lods;
		for (int i = 0; i < lods.Length; i++)
		{
			lods[i].Unskin();
		}
		base.Unskin();
	}

	protected override IEnumerable<Renderer> GetRenderers()
	{
		AbstractSkin[] lods = _lods;
		foreach (AbstractSkin abstractSkin in lods)
		{
			yield return abstractSkin.SkinnedMeshRenderer;
		}
	}
}
