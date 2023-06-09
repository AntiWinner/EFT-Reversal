using UnityEngine;

public class SimpleSparksTest : MonoBehaviour
{
	public SimpleSparksRenderer Renderer;

	public float Speed;

	public float Gravity;

	[Range(0f, 0.9999999f)]
	public float Drag;

	public float Radius;

	public float LifeTime;

	[Range(0f, 255f)]
	public byte Emission;

	[Range(0f, 255f)]
	public byte Size;

	[Range(0f, 255f)]
	public byte TurbulenceAmp;

	[Range(0f, 255f)]
	public byte TurbulenceFreq;

	public int Count;

	public int CountRnd;

	public bool Check;

	public bool Once;

	public void Awake()
	{
	}

	private void Update()
	{
		if (Check && !(Renderer == null))
		{
			if (Once)
			{
				Check = false;
			}
			int num = Count + Random.Range(0, CountRnd);
			float num2 = Drag * Drag;
			num2 *= num2;
			num2 *= num2;
			for (int i = 0; i < num; i++)
			{
				Renderer.EmitSeg(base.transform.position, (base.transform.forward + Random.insideUnitSphere * Radius) * Speed, Time.time, Gravity, num2, LifeTime, Emission, Size, TurbulenceAmp, TurbulenceFreq);
			}
		}
	}
}
