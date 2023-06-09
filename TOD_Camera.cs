using UnityEngine;

[AddComponentMenu("Time of Day/Camera Main Script")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class TOD_Camera : MonoBehaviour
{
	public TOD_Sky sky;

	public bool DomePosToCamera = true;

	public Vector3 DomePosOffset = Vector3.zero;

	public bool DomeScaleToFarClip = true;

	public float DomeScaleFactor = 0.95f;

	private Camera _E000;

	private Transform _E001;

	public bool HDR
	{
		get
		{
			if (!_E000)
			{
				return false;
			}
			return _E000.allowHDR;
		}
	}

	protected void OnValidate()
	{
		DomeScaleFactor = Mathf.Clamp(DomeScaleFactor, 0.01f, 1f);
	}

	protected void OnEnable()
	{
		_E000 = GetComponent<Camera>();
		_E001 = GetComponent<Transform>();
		if (!sky)
		{
			sky = _E3AA.FindUnityObjectOfType(typeof(TOD_Sky)) as TOD_Sky;
		}
	}

	protected void Update()
	{
		if ((bool)sky && sky.Initialized)
		{
			sky.Components.Camera = this;
			if (_E000.clearFlags == CameraClearFlags.Skybox)
			{
				Debug.LogWarning(_ED3E._E000(17057), _E000);
				_E000.clearFlags = CameraClearFlags.Color;
			}
		}
	}

	protected void OnPreCull()
	{
		if ((bool)sky && sky.Initialized)
		{
			if (DomeScaleToFarClip)
			{
				DoDomeScaleToFarClip();
			}
			if (DomePosToCamera)
			{
				DoDomePosToCamera();
			}
		}
	}

	public void DoDomeScaleToFarClip()
	{
		float num = DomeScaleFactor * _E000.farClipPlane;
		Vector3 localScale = new Vector3(num, num, num);
		sky.Components.DomeTransform.localScale = localScale;
	}

	public void DoDomePosToCamera()
	{
		Vector3 position = _E001.position + _E001.rotation * DomePosOffset;
		sky.Components.DomeTransform.position = position;
	}
}
