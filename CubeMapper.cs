using UnityEngine;

public class CubeMapper : MonoBehaviour
{
	[SerializeField]
	protected string _weaponShaderNameToReplace = _ED3E._E000(45238);

	protected ShaderDataReplacer _sdr;

	private static readonly int m__E000 = Shader.PropertyToID(_ED3E._E000(84062));

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(45489));

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(84052));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(84042));

	protected virtual void Update()
	{
		if (Input.GetKeyDown(KeyCode.Keypad4))
		{
			_E000();
		}
	}

	private void _E000()
	{
		_sdr = _E3AA.FindUnityObjectOfType<ShaderDataReplacer>();
		if (_sdr == null)
		{
			return;
		}
		Renderer[] componentsInChildren = base.transform.parent.GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer renderer in componentsInChildren)
		{
			if (!(renderer.name != _ED3E._E000(88327)))
			{
				continue;
			}
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				if (material.shader.name == _sdr._shaderNameToReplaceData || material.shader.name == _ED3E._E000(37394))
				{
					material.SetTexture(CubeMapper.m__E000, _sdr._cubemap);
					material.SetColor(_E001, _sdr._reflectColor);
					material.SetFloat(_E002, _sdr._specularness);
					material.SetFloat(_E003, _sdr._glossness);
				}
			}
		}
	}
}
