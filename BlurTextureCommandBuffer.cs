using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class BlurTextureCommandBuffer : MonoBehaviour
{
	public Shader BlurShader;

	private Material m__E000;

	private Camera _E001;

	private CommandBuffer _E002;

	private void _E000()
	{
		if (_E001 != null && _E002 != null)
		{
			_E001.RemoveCommandBuffer(CameraEvent.AfterImageEffectsOpaque, _E002);
		}
		Object.DestroyImmediate(this.m__E000);
	}

	public void OnEnable()
	{
		_E000();
	}

	public void OnDisable()
	{
		_E000();
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		_E001 = Camera.current;
		if ((bool)_E001)
		{
			_E000();
			if (!this.m__E000)
			{
				this.m__E000 = new Material(BlurShader);
				this.m__E000.hideFlags = HideFlags.HideAndDontSave;
			}
			_E002 = new CommandBuffer();
			_E002.name = _ED3E._E000(41905);
			int num = Shader.PropertyToID(_ED3E._E000(41950));
			_E002.GetTemporaryRT(num, -1, -1, 0, FilterMode.Bilinear);
			_E002.Blit(BuiltinRenderTextureType.CurrentActive, num);
			int num2 = Shader.PropertyToID(_ED3E._E000(41929));
			int num3 = Shader.PropertyToID(_ED3E._E000(41920));
			_E002.GetTemporaryRT(num2, -2, -2, 0, FilterMode.Bilinear);
			_E002.GetTemporaryRT(num3, -2, -2, 0, FilterMode.Bilinear);
			_E002.Blit(num, num2);
			_E002.ReleaseTemporaryRT(num);
			_E002.SetGlobalVector(_ED3E._E000(41913), new Vector4(2f / (float)Screen.width, 0f, 0f, 0f));
			_E002.Blit(num2, num3, this.m__E000);
			_E002.SetGlobalVector(_ED3E._E000(41913), new Vector4(0f, 2f / (float)Screen.height, 0f, 0f));
			_E002.Blit(num3, num2, this.m__E000);
			_E002.SetGlobalVector(_ED3E._E000(41913), new Vector4(4f / (float)Screen.width, 0f, 0f, 0f));
			_E002.Blit(num2, num3, this.m__E000);
			_E002.SetGlobalVector(_ED3E._E000(41913), new Vector4(0f, 4f / (float)Screen.height, 0f, 0f));
			_E002.Blit(num3, num2, this.m__E000);
			_E002.SetGlobalVector(_ED3E._E000(41913), new Vector4(8f / (float)Screen.width, 0f, 0f, 0f));
			_E002.Blit(num2, num3, this.m__E000);
			_E002.SetGlobalVector(_ED3E._E000(41913), new Vector4(0f, 8f / (float)Screen.height, 0f, 0f));
			_E002.Blit(num3, num2, this.m__E000);
			_E002.SetGlobalTexture(_ED3E._E000(41983), num2);
			_E001.AddCommandBuffer(CameraEvent.AfterImageEffectsOpaque, _E002);
			Graphics.Blit(src, dest);
		}
	}
}
