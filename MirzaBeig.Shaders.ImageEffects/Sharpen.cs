using System;
using UnityEngine;

namespace MirzaBeig.Shaders.ImageEffects;

[Serializable]
[ExecuteInEditMode]
public class Sharpen : IEBase
{
	[Range(-2f, 2f)]
	public float strength = 0.5f;

	[Range(0f, 8f)]
	public float edgeMult = 0.2f;

	private static readonly int _strength = Shader.PropertyToID(_ED3E._E000(128333));

	private static readonly int _edgeMult = Shader.PropertyToID(_ED3E._E000(128327));

	private void Awake()
	{
		base.shader = _E3AC.Find(_ED3E._E000(38137));
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat(_strength, strength);
		base.material.SetFloat(_edgeMult, edgeMult);
		blit(source, destination);
	}
}
