using UnityEngine;

public class PreviewFilter : MonoBehaviour
{
	public Material material;

	private void OnEnable()
	{
		Camera component = GetComponent<Camera>();
		component.clearFlags = CameraClearFlags.Color;
		component.backgroundColor = new Color(1f, 0f, 1f, 0f);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}
}
