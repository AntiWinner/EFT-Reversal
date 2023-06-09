using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RainCondensator : MonoBehaviour
{
	private const float _E000 = 8f;

	private const float _E001 = 0.25f;

	private const string _E002 = "USERAIN";

	private const string _E003 = "SMap";

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(84066));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(84114));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(84159));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(84146));

	private bool _E008;

	private MaterialPropertyBlock _E009;

	private List<Material> _E00A = new List<Material>();

	private Renderer _E00B;

	private Vector3 _E00C;

	private void OnEnable()
	{
		if (base.gameObject.name.Contains(_ED3E._E000(84093)) || base.gameObject.name.Contains(_ED3E._E000(84091)))
		{
			base.enabled = false;
			return;
		}
		_E00B = GetComponent<Renderer>();
		_E008 = _E00B is SkinnedMeshRenderer;
		_E009 = new MaterialPropertyBlock();
		Material[] materials = _E00B.materials;
		foreach (Material material in materials)
		{
			if (material.shader.name.Contains(_ED3E._E000(84077)))
			{
				material.SetFloat(_E004, _E008 ? 0.25f : 8f);
				material.SetFloat(_E005, Convert.ToInt32(_E008));
				_E00A.Add(material);
				material.EnableKeyword(_ED3E._E000(84074));
			}
		}
		RainController.AddRainCondensator(this);
	}

	public void UpdateValues()
	{
		Vector3 vector = (RainController.IsCameraUnderRain ? RainController.FallingVectorV3 : (0.9f * _E00C));
		_E00C = Vector3.Slerp(_E00C, vector, 0.3f);
		Vector3 vector2 = -base.transform.InverseTransformDirection(vector);
		Vector3 vector3 = -base.transform.InverseTransformDirection(_E00C);
		_E009.SetVector(_E006, vector2);
		_E009.SetVector(_E007, vector3);
		for (int i = 0; i < _E00A.Count; i++)
		{
			_E00B.SetPropertyBlock(_E009, i);
		}
	}

	private void OnDisable()
	{
		Material[] materials = _E00B.materials;
		foreach (Material material in materials)
		{
			if (material.shader.name.Contains(_ED3E._E000(84077)))
			{
				material.DisableKeyword(_ED3E._E000(84074));
			}
		}
		_E00A.Clear();
		RainController.RemoveRainCondensator(this);
	}
}
