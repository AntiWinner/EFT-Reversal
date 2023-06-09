using UnityEngine;

[RequireComponent(typeof(Light))]
public class MuzzleLight : MonoBehaviour
{
	[_E2BD(0f, 10f, -1f)]
	public Vector2 Range = new Vector2(6f, 12f);

	public Light[] Lights;

	public AnimationCurve LightIntensityCurve = AnimationCurve.Linear(0f, 1f, 0.5f, 0f);

	public void Start()
	{
		if (Lights == null || Lights.Length == 0)
		{
			Lights = new Light[1];
			Lights[0] = GetComponent<Light>();
			Lights[0].shadows = LightShadows.None;
		}
		Light[] lights = Lights;
		for (int i = 0; i < lights.Length; i++)
		{
			lights[i].color = new Color(1f, 0.65f, 0.45f);
		}
		SetIntensity(0f);
	}

	public void SetIntensity(float intensity)
	{
		if (Lights == null || Lights.Length == 0)
		{
			return;
		}
		bool flag = intensity > 0.001f;
		Light[] lights = Lights;
		foreach (Light light in lights)
		{
			if ((bool)light)
			{
				light.intensity = intensity * 2f;
				light.enabled = flag;
			}
		}
	}

	internal void _E000()
	{
		float range = Random.Range(Range.x, Range.y);
		Light[] lights = Lights;
		for (int i = 0; i < lights.Length; i++)
		{
			lights[i].range = range;
		}
	}
}
