using System;

[Serializable]
public class BotGlobalsChangeSettings
{
	[_E2A4("коэфициент дальности видимости когда внутри дыма")]
	public float SMOKE_VISION_DIST = 0.6f;

	[_E2A4("коэфициент скорости замечания когда внутри дыма")]
	public float SMOKE_GAIN_SIGHT = 1.6f;

	[_E2A4("коэфициент точности когда внутри дыма")]
	public float SMOKE_SCATTERING = 1.6f;

	[_E2A4("коэфициент скорости прицеливания когда внутри дыма")]
	public float SMOKE_PRECICING = 1.6f;

	[_E2A4("коэфициент слышимости дальности когда внутри дыма")]
	public float SMOKE_HEARING = 1f;

	[_E2A4("коэфициент скорости прицеливания когда внутри дыма")]
	public float SMOKE_ACCURATY = 1.6f;

	[_E2A4("коэфициент шанса лечь в случае опастности внезапной когда внутри дыма")]
	public float SMOKE_LAY_CHANCE = 1.6f;

	[_E2A4("коэфициент дальности видимости когда ослеплен")]
	public float FLASH_VISION_DIST = 0.2f;

	[_E2A4("коэфициент скорости замечания когда ослеплен")]
	public float FLASH_GAIN_SIGHT = 1.8f;

	[_E2A4("коэфициент точности когда ослеплен")]
	public float FLASH_SCATTERING = 1.6f;

	[_E2A4("коэфициент скорости прицеливания когда ослеплен")]
	public float FLASH_PRECICING = 1.6f;

	[_E2A4("коэфициент слышимости дальности когда ослеплен")]
	public float FLASH_HEARING = 1f;

	[_E2A4("коэфициент скорости прицеливания когда ослеплен")]
	public float FLASH_ACCURATY = 1.6f;

	[_E2A4("коэфициент шанса лечь в случае опастности внезапной когда ослеплен")]
	public float FLASH_LAY_CHANCE = 1f;

	[_E2A4("коэфициент дальности слышимости когда оглушен")]
	public float STUN_HEARING = 0.6f;

	public void Update()
	{
	}
}
