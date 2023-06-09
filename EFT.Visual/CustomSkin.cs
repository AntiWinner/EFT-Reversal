using Diz.Skinning;
using UnityEngine;

namespace EFT.Visual;

public class CustomSkin<TSkin> : AbstractSkin
{
	public Skin Skin;

	public TSkin Data;

	public override SkinnedMeshRenderer SkinnedMeshRenderer => Skin.SkinnedMeshRenderer;

	public override void ApplySkin()
	{
		Skin.ApplySkin();
	}

	public override void Unskin()
	{
		Skin.Unskin();
	}
}
