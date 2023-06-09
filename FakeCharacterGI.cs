using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class FakeCharacterGI : MonoBehaviour
{
	public Shader FakeCharacterGIShader;

	public Color GIColor;

	public Vector3 GIDirection;

	public float GIPower;

	public Color SkylightColor;

	public Vector3 SkylightDirection;

	public float SkylightPower;

	private Material m__E000;

	private Camera m__E001;

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(33493));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(43529));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(43522));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(43575));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(43560));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(43615));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(43594));

	private void Awake()
	{
		this.m__E001 = GetComponent<Camera>();
		_E001();
	}

	private void OnValidate()
	{
		_E001();
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Matrix4x4 cameraToWorldMatrix = this.m__E001.cameraToWorldMatrix;
		_E000().SetMatrix(_E002, cameraToWorldMatrix);
		Graphics.Blit(src, dest, _E000());
	}

	private Material _E000()
	{
		if (this.m__E000 != null)
		{
			return this.m__E000;
		}
		return this.m__E000 = new Material(FakeCharacterGIShader);
	}

	private void _E001()
	{
		Material material = _E000();
		material.SetColor(_E003, GIColor);
		material.SetVector(_E004, GIDirection);
		material.SetFloat(_E005, GIPower);
		material.SetColor(_E006, SkylightColor);
		material.SetVector(_E007, SkylightDirection);
		material.SetFloat(_E008, SkylightPower);
	}
}
