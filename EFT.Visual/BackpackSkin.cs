using System;
using System.Runtime.CompilerServices;
using Diz.Skinning;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Visual;

public class BackpackSkin : CustomSkin<BackpackSkinData>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public PlayerBody playerBody;

		public BackpackSkin _003C_003E4__this;

		internal void _E000(Tuple<Item, Item> tuple)
		{
			Item item = tuple.Item1;
			Item item2 = tuple.Item2;
			bool flag = item != null || playerBody.HasIntergratedArmor;
			bool flag2 = item2 != null;
			Mesh sharedMesh = ((flag && flag2) ? _003C_003E4__this.Data.ArmorVest : (flag ? _003C_003E4__this.Data.Armor : ((!flag2) ? _003C_003E4__this.Data.Body : _003C_003E4__this.Data.Vest)));
			_003C_003E4__this.Skin.SkinnedMeshRenderer.sharedMesh = sharedMesh;
		}
	}

	public void Init(Skeleton skeleton, PlayerBody playerBody)
	{
		Skin.Init(skeleton);
		Unsubscribe();
		_unsubscribe = _ECF3.Combine(playerBody.SlotViews.GetByKey(EquipmentSlot.ArmorVest).ContainedItem, playerBody.SlotViews.GetByKey(EquipmentSlot.TacticalVest).ContainedItem, Tuple.Create).Bind(delegate(Tuple<Item, Item> tuple)
		{
			Item item = tuple.Item1;
			Item item2 = tuple.Item2;
			bool flag = item != null || playerBody.HasIntergratedArmor;
			bool flag2 = item2 != null;
			Mesh sharedMesh = ((flag && flag2) ? Data.ArmorVest : (flag ? Data.Armor : ((!flag2) ? Data.Body : Data.Vest)));
			Skin.SkinnedMeshRenderer.sharedMesh = sharedMesh;
		});
	}

	public override void Unskin()
	{
		Unsubscribe();
		base.Unskin();
	}
}
