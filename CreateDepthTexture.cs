using UnityEngine;

public class CreateDepthTexture : MonoBehaviour
{
	private Material _E000;

	public RenderTexture _rt;

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(84798));

	private void Start()
	{
		_E000 = new Material(_E3AC.Find(_ED3E._E000(38408)));
		_rt = new RenderTexture(1024, 1024, 0)
		{
			name = _ED3E._E000(84759),
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point,
			format = RenderTextureFormat.RHalf
		};
	}

	private void OnRenderImage(RenderTexture s, RenderTexture d)
	{
		Graphics.Blit(s, _rt, _E000);
		Shader.SetGlobalTexture(_E001, _rt);
		Graphics.CopyTexture(s, d);
	}
}
