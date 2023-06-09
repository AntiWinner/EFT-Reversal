using UnityEngine;

[AddComponentMenu("Colorful/Wiggle")]
[ExecuteInEditMode]
public class CC_Wiggle : CC_Base
{
	public float timer;

	public float speed = 1f;

	public float scale = 12f;

	public float str = 1f;

	public bool autoTimer = true;

	private static readonly int _E046 = Shader.PropertyToID(_ED3E._E000(17069));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(16510));

	private static readonly int _E047 = Shader.PropertyToID(_ED3E._E000(17060));

	protected virtual void Update()
	{
		if (autoTimer)
		{
			timer += speed * Time.deltaTime;
		}
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat(_E046, timer);
		base.material.SetFloat(_E007, scale);
		base.material.SetFloat(_E047, str);
		Graphics.Blit(source, destination, base.material);
	}
}
