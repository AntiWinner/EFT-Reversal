using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class AreaLightManager : MonoBehaviour
{
	[SerializeField]
	private Shader ClearStencilShader;

	private const int _E000 = 192;

	private Material _E001;

	private CommandBuffer _E002;

	private Camera _E003;

	private Mesh _E004;

	private void Awake()
	{
		_E001 = new Material(ClearStencilShader);
		_E001.SetFloat(_ED3E._E000(42517), 192f);
		_E003 = GetComponent<Camera>();
		_E004 = AmbientLight.GetQuadMesh();
		_E002 = new CommandBuffer
		{
			name = _ED3E._E000(42507)
		};
		_E002.DrawMesh(_E004, Matrix4x4.identity, _E001);
		_E003.AddCommandBuffer(CameraEvent.AfterImageEffectsOpaque, _E002);
	}
}
