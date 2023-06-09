using System;
using System.Collections.Generic;
using UnityEngine;

namespace Diz.Skinning;

public class Skin : AbstractSkin
{
	[SerializeField]
	[HideInInspector]
	private SkinnedMeshRenderer _skinnedMeshRenderer;

	[SerializeField]
	[HideInInspector]
	private string[] _bonePaths;

	[HideInInspector]
	[SerializeField]
	private string _rootBonePath;

	private Skeleton _E001;

	public override SkinnedMeshRenderer SkinnedMeshRenderer => _skinnedMeshRenderer;

	public void Init(Skeleton skeleton)
	{
		_E001 = skeleton;
	}

	public override void ApplySkin()
	{
		int num = _bonePaths.Length;
		Transform[] array = new Transform[num];
		Dictionary<string, Transform> bones = _E001.Bones;
		SkinnedMeshRenderer skinnedMeshRenderer = _skinnedMeshRenderer;
		for (int i = 0; i < num; i++)
		{
			string text = _bonePaths[i];
			if (bones.TryGetValue(text, out var value))
			{
				array[i] = value;
				continue;
			}
			throw new Exception(string.Format(_ED3E._E000(244808), text, _E001, _skinnedMeshRenderer.name));
		}
		skinnedMeshRenderer.rootBone = bones[_rootBonePath];
		skinnedMeshRenderer.bones = array;
	}

	public override void Unskin()
	{
		SkinnedMeshRenderer skinnedMeshRenderer = _skinnedMeshRenderer;
		skinnedMeshRenderer.bones = new Transform[_bonePaths.Length];
		skinnedMeshRenderer.rootBone = null;
	}
}
