using System;

[Serializable]
[Obsolete]
public class Speed
{
	public float Forward;

	public float Side;

	public float Backward;

	public Speed(float forward, float side, float backward)
	{
		Forward = forward;
		Side = side;
		Backward = backward;
	}

	public static Speed operator *(Speed speed, float multiplier)
	{
		return new Speed(speed.Forward * multiplier, speed.Side * multiplier, speed.Backward * multiplier);
	}
}
