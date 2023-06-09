using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

public class TransformLinks : MonoBehaviour
{
	[Serializable]
	public struct CachedTransform
	{
		public Vector3 Position;

		public Quaternion Rotation;

		public Transform Transform;

		public void Reset()
		{
			Transform.localPosition = Position;
			Transform.localRotation = Rotation;
			Transform.gameObject.SetActive(value: true);
		}
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public Transform parent;

		internal Transform _E000(string x)
		{
			return parent.FindTransform(x);
		}
	}

	public Transform[] Transforms;

	public Transform Self;

	[SerializeField]
	private CachedTransform[] _cachedTransforms;

	[CanBeNull]
	public Transform GetTransform(ECharacterWeaponBones bone)
	{
		return Transforms[(int)bone];
	}

	[CanBeNull]
	public Transform GetTransformOutOfRangeSafe(ECharacterWeaponBones bone)
	{
		if ((int)bone >= Transforms.Length)
		{
			return null;
		}
		return Transforms[(int)bone];
	}

	public Transform GetCachedTransform(int index)
	{
		return _cachedTransforms[index].Transform;
	}

	public void GatherIK(Transform[] markers, Transform[] gripReferences, Transform[] elbowBends)
	{
		elbowBends[0] = GetTransform(ECharacterWeaponBones.Bend_Goal_Left);
		elbowBends[1] = GetTransform(ECharacterWeaponBones.Bend_Goal_Right);
		markers[0] = GetTransform(ECharacterWeaponBones.weapon_L_IK_marker);
		markers[1] = GetTransform(ECharacterWeaponBones.weapon_R_IK_marker);
		gripReferences[0] = GetTransform(ECharacterWeaponBones.weapon_L_hand_marker);
		gripReferences[1] = GetTransform(ECharacterWeaponBones.weapon_R_hand_marker);
	}

	public void GatherUnderbarrelWeaponIK(Transform launcherRoot, Transform[] elbowBends)
	{
		elbowBends[0] = launcherRoot.FindTransform(_ED3E._E000(63941));
	}

	public void CacheTransforms(Transform parent, IEnumerable<string> cachedBoneNames)
	{
		Self = base.gameObject.transform;
		string[] names = Enum.GetNames(typeof(ECharacterWeaponBones));
		Transforms = new Transform[names.Length];
		foreach (string text in names)
		{
			int num = (int)Enum.Parse(typeof(ECharacterWeaponBones), text);
			string text2 = (text.Contains(_ED3E._E000(63988)) ? (_ED3E._E000(63986) + text) : text);
			Transforms[num] = parent.FindTransform(text2);
		}
		_cachedTransforms = (from x in cachedBoneNames
			select parent.FindTransform(x) into x
			where x != null
			select x).Select(delegate(Transform t)
		{
			CachedTransform result = default(CachedTransform);
			result.Position = t.localPosition;
			result.Rotation = t.localRotation;
			result.Transform = t;
			return result;
		}).ToArray();
	}

	public void ResetPositions()
	{
		CachedTransform[] cachedTransforms = _cachedTransforms;
		foreach (CachedTransform cachedTransform in cachedTransforms)
		{
			cachedTransform.Reset();
		}
	}
}
