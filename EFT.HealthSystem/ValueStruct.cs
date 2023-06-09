namespace EFT.HealthSystem;

public struct ValueStruct
{
	public float Current;

	public float Maximum;

	public float Minimum;

	public float OverDamageReceivedMultiplier;

	public float Normalized => Current / Maximum;

	public bool AtMinimum => !(Current - Minimum).Positive();

	public bool AtMaximum => !(Maximum - Current).Positive();
}
