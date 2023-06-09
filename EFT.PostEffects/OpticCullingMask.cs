using UnityEngine;
using UnityEngine.Rendering;

namespace EFT.PostEffects;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class OpticCullingMask : MonoBehaviour
{
	[SerializeField]
	private Shader _cullingMaskShader;

	[SerializeField]
	[Range(0f, 2f)]
	public float _maskScale = 1f;

	private static Mesh _E000;

	private static readonly Matrix4x4 _E001 = Matrix4x4.identity;

	private Camera _E002;

	private Material _E003;

	private CameraEvent _E004 = CameraEvent.BeforeGBuffer;

	private CommandBuffer _E005;

	private int _E006;

	private void Awake()
	{
		if (_E000 == null)
		{
			_E000 = SSAOMask.GetQuadMesh(0f);
		}
		if (_cullingMaskShader == null)
		{
			_cullingMaskShader = _E3AC.Find(_ED3E._E000(40051));
		}
		_E003 = new Material(_cullingMaskShader);
		_E006 = Shader.PropertyToID(_ED3E._E000(191796));
		UpdateParameters();
		_E005 = new CommandBuffer
		{
			name = _ED3E._E000(191791)
		};
		_E005.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
		_E005.DrawMesh(_E000, _E001, _E003);
		_E002 = GetComponent<Camera>();
		_E002.AddCommandBuffer(_E004, _E005);
	}

	private void OnDestroy()
	{
		_E005?.Release();
	}

	public void UpdateParameters()
	{
		if (_E003 != null)
		{
			_E003.SetFloat(_E006, _maskScale);
		}
	}
}
