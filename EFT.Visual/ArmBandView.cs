using System;
using System.Collections.Generic;
using System.Linq;
using Diz.Skinning;
using UnityEngine;

namespace EFT.Visual;

public sealed class ArmBandView : Dress
{
	[Serializable]
	private sealed class MeshCustomizationIdPair
	{
		public string CustomizationId;

		public Mesh[] Meshes;
	}

	[SerializeField]
	private MeshCustomizationIdPair[] _customizationIdPair = Array.Empty<MeshCustomizationIdPair>();

	private Dictionary<string, Mesh[]> _E000;

	[SerializeField]
	private Skin[] _main;

	public override void Init(PlayerBody playerBody)
	{
		base.Init(playerBody);
		Skin[] main = _main;
		for (int i = 0; i < main.Length; i++)
		{
			main[i].Init(playerBody.SkeletonRootJoint);
		}
		this._E000 = _customizationIdPair.ToDictionary((MeshCustomizationIdPair p) => p.CustomizationId, (MeshCustomizationIdPair p) => p.Meshes);
	}

	public void SetBodyHandler(string bodyCustomizationId)
	{
		if (!this._E000.TryGetValue(bodyCustomizationId, out var value))
		{
			_E5DA.Instance.LogError(_ED3E._E000(168614) + bodyCustomizationId);
			value = this._E000.Values.First();
		}
		for (int i = 0; i < _main.Length; i++)
		{
			_main[i].SkinnedMeshRenderer.sharedMesh = value[i];
			_main[i].ApplySkin();
		}
	}

	protected override void OnSkin(Transform playerTransform, Transform meshTransform)
	{
		CompositeDisposable.BindState(PlayerBody.BodyCustomizationId, SetBodyHandler);
		Skin[] main = _main;
		for (int i = 0; i < main.Length; i++)
		{
			main[i].ApplySkin();
		}
	}

	public override void Unskin()
	{
		Skin[] main = _main;
		for (int i = 0; i < main.Length; i++)
		{
			main[i].Unskin();
		}
		base.Unskin();
	}

	protected override IEnumerable<Renderer> GetRenderers()
	{
		Skin[] main = _main;
		foreach (Skin skin in main)
		{
			yield return skin.SkinnedMeshRenderer;
		}
	}
}
