using System.Collections.Generic;
using UnityEngine;

public class PreviewMaterialSetter : MonoBehaviour
{
	[SerializeField]
	private Color _availableColor = new Color(0f, 1f, 0f, 0.2f);

	[SerializeField]
	private Color _availableReflectionColor = new Color(0.25f, 1f, 0.25f, 1f);

	[SerializeField]
	private Color _unvailableColor = new Color(1f, 0f, 0f, 0.2f);

	[SerializeField]
	private Color _unvailableReflectionColor = new Color(1f, 0.25f, 0.25f, 1f);

	private Dictionary<MeshRenderer, Material[]> m__E000;

	private Material m__E001;

	private static readonly int m__E002 = Shader.PropertyToID(_ED3E._E000(36528));

	private static readonly int m__E003 = Shader.PropertyToID(_ED3E._E000(45489));

	private void Awake()
	{
		_E000();
		_E002();
		foreach (KeyValuePair<MeshRenderer, Material[]> item in this.m__E000)
		{
			_E003(item.Key, item.Value);
		}
	}

	private void OnDestroy()
	{
		foreach (KeyValuePair<MeshRenderer, Material[]> item in this.m__E000)
		{
			item.Key.sharedMaterials = item.Value;
		}
		this.m__E000.Clear();
	}

	public void SetAvailable(bool isAvailable)
	{
		Color value = (isAvailable ? _availableColor : _unvailableColor);
		Color value2 = (isAvailable ? _availableReflectionColor : _unvailableReflectionColor);
		this.m__E001.SetColor(PreviewMaterialSetter.m__E002, value);
		this.m__E001.SetColor(PreviewMaterialSetter.m__E003, value2);
	}

	private void _E000()
	{
		this.m__E000 = new Dictionary<MeshRenderer, Material[]>();
		MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.m__E000.Add(componentsInChildren[i], _E001(componentsInChildren[i]));
		}
	}

	private Material[] _E001(MeshRenderer renderer)
	{
		Material[] array = new Material[renderer.sharedMaterials.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = renderer.sharedMaterials[i];
		}
		return array;
	}

	private void _E002()
	{
		this.m__E001 = new Material(_E3AC.Find(_ED3E._E000(37298)));
		this.m__E001.SetColor(PreviewMaterialSetter.m__E002, _availableColor);
	}

	private void _E003(MeshRenderer renderer, Material[] savedMaterials)
	{
		Material[] array = new Material[renderer.sharedMaterials.Length];
		for (int i = 0; i < savedMaterials.Length; i++)
		{
			array[i] = this.m__E001;
		}
		renderer.sharedMaterials = array;
	}
}
