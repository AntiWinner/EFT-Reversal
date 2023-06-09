using UnityEngine;

namespace GPUInstancer;

public class GrassPrefabVariant : MonoBehaviour
{
	[SerializeField]
	public int ArrayIndex;

	[SerializeField]
	private Color DryColor = new Color(0.47f, 0.5f, 0.3f, 1f);

	[SerializeField]
	private Color HealthyColor = Color.white;

	private static readonly string m__E000 = _ED3E._E000(117275);

	private static readonly string _E001 = _ED3E._E000(115083);

	private static readonly string _E002 = _ED3E._E000(115093);

	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
			_E000();
		}
	}

	private void _E000()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		materialPropertyBlock.SetInt(GrassPrefabVariant.m__E000, ArrayIndex);
		materialPropertyBlock.SetColor(_E001, DryColor);
		materialPropertyBlock.SetColor(_E002, HealthyColor);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetPropertyBlock(materialPropertyBlock);
		}
	}
}
