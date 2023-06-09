using UnityEngine;

public class CameraPixelPerfect : MonoBehaviour
{
	private void Start()
	{
		if (Application.isPlaying)
		{
			_E000();
		}
	}

	private void OnEnable()
	{
		_E000();
	}

	private void _E000()
	{
		Camera component = GetComponent<Camera>();
		if (component != null)
		{
			component.orthographic = true;
			component.orthographicSize = (float)Screen.height * 0.5f;
		}
	}
}
