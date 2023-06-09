using UnityEngine;

public class ShaderDataReplacer : MonoBehaviour
{
	[SerializeField]
	public string _shaderNameToReplaceData = _ED3E._E000(45238);

	[SerializeField]
	public Cubemap _cubemap;

	[SerializeField]
	public Color _reflectColor = new Color(1f, 1f, 1f, 0.5f);

	[Range(0.01f, 10f)]
	[SerializeField]
	public float _specularness = 5f / 64f;

	[Range(0.01f, 10f)]
	[SerializeField]
	public float _glossness = 1f;
}
