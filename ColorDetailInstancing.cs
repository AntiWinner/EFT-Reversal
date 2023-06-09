using System;
using UnityEngine;

public class ColorDetailInstancing : MonoBehaviour
{
	[Serializable]
	public struct MaterialLink
	{
		public MeshRenderer MeshRenderer;

		public int MaterialIndex;

		public MaterialLink(MeshRenderer meshRenderer, int materialIndex)
		{
			MeshRenderer = meshRenderer;
			MaterialIndex = materialIndex;
		}
	}

	[Serializable]
	public struct MaterialControlls
	{
		public Material material;

		public Color Color;

		public int DetailTexIndex;

		public float DetailScale;

		[HideInInspector]
		public MaterialLink[] Links;
	}

	private static Shader m__E000;

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(88940));

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(88929));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(88983));

	private static MaterialPropertyBlock _E004;

	[SerializeField]
	private MaterialControlls[] _materials;

	private static void _E000(MaterialControlls[] controlls)
	{
		if (_E004 == null)
		{
			_E004 = new MaterialPropertyBlock();
		}
		for (int i = 0; i < controlls.Length; i++)
		{
			_E004.SetColor(_E001, controlls[i].Color);
			_E004.SetVector(_E002, new Vector4(controlls[i].DetailTexIndex, controlls[i].DetailScale));
			for (int j = 0; j < controlls[i].Links.Length; j++)
			{
				controlls[i].Links[j].MeshRenderer.SetPropertyBlock(_E004, controlls[i].Links[j].MaterialIndex);
			}
		}
	}

	private void Awake()
	{
		_E000(_materials);
	}
}
