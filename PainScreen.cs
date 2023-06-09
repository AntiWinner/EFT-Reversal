using UnityEngine;

public class PainScreen : MonoBehaviour
{
	[Range(0f, 1f)]
	[SerializeField]
	private float _value;

	[SerializeField]
	private Material _mat;

	private static readonly int _E000 = Shader.PropertyToID(_ED3E._E000(44277));

	private void Start()
	{
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		_mat.SetFloat(_E000, _value);
		Graphics.Blit(source, destination, _mat);
	}
}
