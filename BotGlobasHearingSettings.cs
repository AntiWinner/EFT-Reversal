using System;

[Serializable]
public class BotGlobasHearingSettings
{
	[_E2A4("Если вытрелить рядом с ботом когда он ничем не обеспокоен с дистанции меньше Х то он начнет паниковать")]
	public float BOT_CLOSE_PANIC_DIST = 15f;

	[_E2A4("Шанс услышать простой звук")]
	public float CHANCE_TO_HEAR_SIMPLE_SOUND_0_1 = 0.5f;

	[_E2A4("Коэфицент точности восприятия звука от не опасных звуков - больше - точнее")]
	public float DISPERSION_COEF = 1.6f;

	[_E2A4("Коэфицент точности восприятия звука от опасных звуков - больше - точнее")]
	public float DISPERSION_COEF_GUN = 40.6f;

	[_E2A4("Ближе этой дистанции простой звук слышен всегда")]
	public float CLOSE_DIST = 6f;

	[_E2A4("Дальше эой дистанции простой звук не слышен всегда")]
	public float FAR_DIST = 30f;

	[_E2A4("Угл При котором выстрел считается произведенным в бота")]
	public float SOUND_DIR_DEEFREE = 30f;

	[_E2A4("")]
	public float DIST_PLACE_TO_FIND_POINT = 70f;

	[_E2A4("")]
	public float DEAD_BODY_SOUND_RAD = 30f;

	[_E2A4("Когда юот смотрит на слух. Он убдет оборачиватся только на \"опасные\" точки.")]
	public bool LOOK_ONLY_DANGER;

	[_E2A4("")]
	public float RESET_TIMER_DIST = 17f;

	[_E2A4("Задержка для слуха когда бот в мирном режиме.")]
	public float HEAR_DELAY_WHEN_PEACE = 0.5f;

	[_E2A4("Задержка для слуха когда бот в режиме подозрения.")]
	public float HEAR_DELAY_WHEN_HAVE_SMT = 0.3f;

	[_E2A4("Через сколько секунд после пропадния врага из вида бот входит в режим карауливания")]
	public float LOOK_ONLY_DANGER_DELTA = 15f;

	[_E2A4("Если бот почуствовал выстрел с дистанции боле Х то бот скажет \"снайпер\"")]
	public float ENEMY_SNIPER_SHOOT_DIST = 100f;

	public void Update()
	{
	}
}
