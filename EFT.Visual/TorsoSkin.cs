using System;
using System.Runtime.CompilerServices;
using Diz.Skinning;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Visual;

public class TorsoSkin : AbstractSkin
{
	[SerializeField]
	private Skin _skin;

	[SerializeField]
	private Mesh _base;

	[SerializeField]
	private Mesh _armor;

	[SerializeField]
	private Mesh _vest;

	private PlayerBody m__E000;

	public override SkinnedMeshRenderer SkinnedMeshRenderer => _skin.SkinnedMeshRenderer;

	public void Init(Skeleton skeleton, PlayerBody playerBody)
	{
		this.m__E000 = playerBody;
		_skin.Init(skeleton);
	}

	public override void ApplySkin()
	{
		_skin.ApplySkin();
		Unsubscribe();
		_unsubscribe = _ECF3.Combine(this.m__E000.SlotViews.GetByKey(EquipmentSlot.ArmorVest).ContainedItem, this.m__E000.SlotViews.GetByKey(EquipmentSlot.TacticalVest).ContainedItem, this.m__E000.SlotViews.GetByKey(EquipmentSlot.Backpack).ContainedItem, this.m__E000.SlotViews.GetByKey(EquipmentSlot.ArmorVest).MainDress, Tuple.Create).Bind(delegate(Tuple<Item, Item, Item, Dress> tuple)
		{
			Item item = tuple.Item1;
			Item item2 = tuple.Item2;
			Item item3 = tuple.Item3;
			Dress item4 = tuple.Item4;
			VestlikeArmor vestlikeArmor = ((item4 != null) ? item4.GetComponent<VestlikeArmor>() : null);
			Mesh sharedMesh = ((item != null && vestlikeArmor == null) ? _armor : (((item == null || !(vestlikeArmor != null)) && item2 == null && item3 == null) ? _base : _vest));
			_skin.SkinnedMeshRenderer.sharedMesh = sharedMesh;
		});
	}

	public override void Unskin()
	{
		Unsubscribe();
		_skin.Unskin();
	}

	[CompilerGenerated]
	private void _E000(Tuple<Item, Item, Item, Dress> tuple)
	{
		Item item = tuple.Item1;
		Item item2 = tuple.Item2;
		Item item3 = tuple.Item3;
		Dress item4 = tuple.Item4;
		VestlikeArmor vestlikeArmor = ((item4 != null) ? item4.GetComponent<VestlikeArmor>() : null);
		Mesh sharedMesh = ((item != null && vestlikeArmor == null) ? _armor : (((item == null || !(vestlikeArmor != null)) && item2 == null && item3 == null) ? _base : _vest));
		_skin.SkinnedMeshRenderer.sharedMesh = sharedMesh;
	}
}
