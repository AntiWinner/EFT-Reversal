using System.Runtime.CompilerServices;
using Diz.Skinning;
using EFT.InventoryLogic;

namespace EFT.Visual;

public class VestSkin : CustomSkin<VestSkinData>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public VestSkin _003C_003E4__this;

		public PlayerBody playerBody;

		internal void _E000(Item armor)
		{
			_003C_003E4__this.Skin.SkinnedMeshRenderer.sharedMesh = ((armor != null || playerBody.HasIntergratedArmor) ? _003C_003E4__this.Data.Armor : _003C_003E4__this.Data.Body);
		}
	}

	public void Init(Skeleton skeleton, PlayerBody playerBody)
	{
		Skin.Init(skeleton);
		Unsubscribe();
		_unsubscribe = playerBody.SlotViews.GetByKey(EquipmentSlot.ArmorVest).ContainedItem.Bind(delegate(Item armor)
		{
			Skin.SkinnedMeshRenderer.sharedMesh = ((armor != null || playerBody.HasIntergratedArmor) ? Data.Armor : Data.Body);
		});
	}

	public override void Unskin()
	{
		Unsubscribe();
		base.Unskin();
	}
}
