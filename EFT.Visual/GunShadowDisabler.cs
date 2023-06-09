using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace EFT.Visual;

public class GunShadowDisabler : MonoBehaviour
{
	[SerializeField]
	private List<Light> Lights;

	private CommandBuffer _E000;

	private CommandBuffer _E001;

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(168817));

	private void Awake()
	{
		_E001 = new CommandBuffer
		{
			name = _ED3E._E000(168784)
		};
		_E000 = new CommandBuffer
		{
			name = _ED3E._E000(168769)
		};
		_E001.SetGlobalFloat(_E002, 1f);
		_E000.SetGlobalFloat(_E002, 0f);
	}

	[ContextMenu("DisableGunShadow")]
	public void DisableGunShadow()
	{
		for (int i = 0; i < Lights.Count; i++)
		{
			Light light = Lights[i];
			light.RemoveCommandBuffer(LightEvent.BeforeShadowMap, _E001);
			light.RemoveCommandBuffer(LightEvent.AfterShadowMap, _E000);
			light.AddCommandBuffer(LightEvent.BeforeShadowMap, _E001);
			light.AddCommandBuffer(LightEvent.AfterShadowMap, _E000);
		}
	}

	[ContextMenu("EnableGunShadow")]
	public void EnableGunShadow()
	{
		for (int i = 0; i < Lights.Count; i++)
		{
			Light light = Lights[i];
			light.RemoveCommandBuffer(LightEvent.BeforeShadowMap, _E001);
			light.RemoveCommandBuffer(LightEvent.AfterShadowMap, _E000);
		}
	}
}
