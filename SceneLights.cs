using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneLights : MonoBehaviour
{
	public static List<Light> AllLights = new List<Light>();

	[SerializeField]
	private Light[] _lights;

	private void Awake()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		Light[] lights = _lights;
		foreach (Light light in lights)
		{
			if (light != null)
			{
				AllLights.Add(light);
			}
		}
	}

	public void RecacheLights()
	{
		_lights = (from x in _E3AA.FindUnityObjectsOfType<Light>()
			where x.gameObject.scene == base.gameObject.scene
			select x).ToArray();
	}

	[CompilerGenerated]
	private bool _E000(Light x)
	{
		return x.gameObject.scene == base.gameObject.scene;
	}
}
