using System;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerSettingsExtension : ScriptableObject
{
	public float extensionVersionNo;

	public virtual string GetExtensionCode()
	{
		throw new NotImplementedException();
	}
}
