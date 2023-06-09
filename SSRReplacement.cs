using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class SSRReplacement : MonoBehaviour
{
	public Shader ssrReplacementShader;

	public RenderTexture tex;

	private GameObject m__E000;

	private Camera _E001;

	private void _E000()
	{
		this.m__E000 = new GameObject(_ED3E._E000(88959), typeof(Camera));
		Camera component = GetComponent<Camera>();
		_E001 = this.m__E000.GetComponent<Camera>();
		_E001.CopyFrom(component);
		_E001.aspect = component.aspect;
		_E001.SetReplacementShader(ssrReplacementShader, _ED3E._E000(18067));
		_E001.targetTexture = tex;
		_E001.renderingPath = RenderingPath.Forward;
		_E001.backgroundColor = Color.black;
		_E001.clearFlags = CameraClearFlags.Color;
		this.m__E000.hideFlags = HideFlags.HideAndDontSave;
		this.m__E000.transform.parent = base.transform;
		this.m__E000.transform.localEulerAngles = Vector3.zero;
		this.m__E000.transform.localPosition = Vector3.zero;
	}

	private void OnDestroy()
	{
		Object.DestroyImmediate(_E001);
	}

	private void OnPostRender()
	{
		if (_E001 == null)
		{
			_E000();
		}
		_E001.RenderWithShader(ssrReplacementShader, _ED3E._E000(18067));
		this.m__E000.transform.position = base.transform.position;
		this.m__E000.transform.rotation = base.transform.rotation;
	}
}
