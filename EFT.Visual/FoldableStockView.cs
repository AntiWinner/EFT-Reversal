using System;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Visual;

public class FoldableStockView : MonoBehaviour, _E7F4
{
	[Serializable]
	public class BonePosition
	{
		public string BoneName;

		public Transform Bone;

		public Quaternion FoldedRotation;

		public Quaternion UnfoldedRotation;
	}

	[SerializeField]
	private BonePosition[] _bonePositions;

	private FoldableComponent m__E000;

	private Action _E001;

	public void Init(Item item, bool isAnimated)
	{
		if (isAnimated)
		{
			return;
		}
		this.m__E000 = item.GetItemComponent<FoldableComponent>();
		if (this.m__E000 == null)
		{
			Debug.LogError(_ED3E._E000(168732) + base.name);
			return;
		}
		_E001 = this.m__E000.OnChanged.Bind(delegate
		{
			BonePosition[] bonePositions = _bonePositions;
			foreach (BonePosition bonePosition in bonePositions)
			{
				Transform transform = ((bonePosition.Bone == null) ? _E38B.FindTransformRecursiveWidthFirst(base.transform, bonePosition.BoneName) : bonePosition.Bone);
				if (!(transform == null))
				{
					transform.localRotation = (this.m__E000.Folded ? bonePosition.FoldedRotation : bonePosition.UnfoldedRotation);
				}
			}
		});
	}

	public void Deinit()
	{
		if (_E001 != null)
		{
			_E001();
			_E001 = null;
		}
	}

	[CompilerGenerated]
	private void _E000()
	{
		BonePosition[] bonePositions = _bonePositions;
		foreach (BonePosition bonePosition in bonePositions)
		{
			Transform transform = ((bonePosition.Bone == null) ? _E38B.FindTransformRecursiveWidthFirst(base.transform, bonePosition.BoneName) : bonePosition.Bone);
			if (!(transform == null))
			{
				transform.localRotation = (this.m__E000.Folded ? bonePosition.FoldedRotation : bonePosition.UnfoldedRotation);
			}
		}
	}
}
