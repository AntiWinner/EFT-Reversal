using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("")]
public sealed class AmplifyMotionPostProcess : MonoBehaviour
{
	private AmplifyMotionEffectBase _E000;

	public AmplifyMotionEffectBase Instance
	{
		get
		{
			return _E000;
		}
		set
		{
			_E000 = value;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (_E000 != null)
		{
			_E000.PostProcess(source, destination);
		}
	}
}
