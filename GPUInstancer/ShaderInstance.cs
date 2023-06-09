using System;
using System.Globalization;
using UnityEngine;

namespace GPUInstancer;

[Serializable]
public class ShaderInstance
{
	public string name;

	public Shader instancedShader;

	public string modifiedDate;

	public bool isOriginalInstanced;

	public string extensionCode;

	public ShaderInstance(string name, Shader instancedShader, bool isOriginalInstanced, string extensionCode = null)
	{
		this.name = name;
		this.instancedShader = instancedShader;
		modifiedDate = _E5AD.Now.ToString(_ED3E._E000(77760), CultureInfo.InvariantCulture);
		this.isOriginalInstanced = isOriginalInstanced;
		this.extensionCode = extensionCode;
	}

	public virtual void Regenerate()
	{
		if (isOriginalInstanced)
		{
			instancedShader = _E4C8.CreateInstancedShader(instancedShader, useOriginal: true);
			return;
		}
		Shader shader = Shader.Find(name);
		if (shader != null)
		{
			instancedShader = _E4C8.CreateInstancedShader(shader);
			modifiedDate = _E5AD.Now.ToString(_ED3E._E000(77760), CultureInfo.InvariantCulture);
		}
	}
}
