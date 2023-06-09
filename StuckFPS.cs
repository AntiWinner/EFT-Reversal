using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class StuckFPS : MonoBehaviour
{
	[SerializeField]
	private int _minFramerate = 60;

	[SerializeField]
	private int _maxFramerate = 60;

	private RenderTexture _E000;

	private float _E001;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (_E000 == null)
		{
			_E000 = new RenderTexture(source.width, source.height, source.depth)
			{
				name = _ED3E._E000(83348)
			};
		}
		int num = Random.Range(_minFramerate, _maxFramerate);
		if (Time.time - _E001 > 1f / (float)num)
		{
			Graphics.Blit(source, _E000);
			_E001 = Time.time;
		}
		Graphics.Blit(_E000, destination);
	}

	private void OnDestroy()
	{
		Object.DestroyImmediate(_E000);
	}
}
