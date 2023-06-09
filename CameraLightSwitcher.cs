using System.Collections.Generic;
using UnityEngine;

public class CameraLightSwitcher : MonoBehaviour
{
	public List<Light> Lights;

	protected virtual void OnPreCull()
	{
		TurnLights(on: true);
	}

	protected virtual void OnPostRender()
	{
		TurnLights(on: false);
	}

	protected virtual void OnEnable()
	{
		TurnLights(on: false);
	}

	protected void TurnLights(bool on)
	{
		for (int i = 0; i < Lights.Count; i++)
		{
			Lights[i].enabled = on;
		}
	}
}
