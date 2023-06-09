using UnityEngine;

public class ShowDepth : MonoBehaviour
{
	private Material _E000;

	private void Start()
	{
		_E000 = new Material(_E3AC.Find(_ED3E._E000(38408)));
	}

	private void OnRenderImage(RenderTexture s, RenderTexture d)
	{
		Graphics.Blit(s, d, _E000);
	}
}
