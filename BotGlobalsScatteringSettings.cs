using System;

[Serializable]
public class BotGlobalsScatteringSettings
{
	[_E2A4(" [метры на расстояние]\tМинимальный угол разброса")]
	public float MinScatter = 0.03f;

	[_E2A4(" [метры на расстояние]\tРабочий угол разброса")]
	public float WorkingScatter = 0.15f;

	[_E2A4("[метры на расстояние]\tМаксимальный угол разброса")]
	public float MaxScatter = 0.4f;

	[_E2A4(" [метры на расстояние]/сек\tСкорость схождения угла разброса")]
	public float SpeedUp = 0.3f;

	[_E2A4("  Float\tКоэффициент на который умножается скорость схождения угла разброса при прицеливании. Больше лучше")]
	public float SpeedUpAim = 1.4f;

	[_E2A4("   Попугаи/сек\tСкорость расхождения угла разброса. Больше лучше")]
	public float SpeedDown = -0.3f;

	[_E2A4("   Попугаи/сек\tСкорость бота после которой начинается замедление схождения угла разброса")]
	public float ToSlowBotSpeed = 1.5f;

	[_E2A4("   Попугаи/сек\tСкорость бота после которой начинается останавливается схождение угла разброса")]
	public float ToLowBotSpeed = 2.4f;

	[_E2A4(" Попугаи/сек Скорость бота после которой начинается расхождение угла разброса")]
	public float ToUpBotSpeed = 3.6f;

	[_E2A4("Коэфициент. Больше хуже. Насколько изменится скорсоть сведения если удельная скорость (ToSlowBotSpeed,ToLowBotSpeed) в этом промежутке")]
	public float MovingSlowCoef = 1.5f;

	[_E2A4("  Градусы/сек\tСкорость поворота бота после которой начинается расхождение угла разброса")]
	public float ToLowBotAngularSpeed = 80f;

	[_E2A4("")]
	public float ToStopBotAngularSpeed = 40f;

	[_E2A4("   Градусы\tНа сколько расходится угол разброса бота при попадании по нему умноженное на урон.")]
	public float FromShot = 0.001f;

	[_E2A4(" Float\tМножитель на сколько быстрей будет сходиться значение ScatterSpeed при использовании трассирующих пуль")]
	public float TracerCoef = 1.3f;

	[_E2A4(" Float\tКоэффициент изменения минимального круга точности  при выбитой руке")]
	public float HandDamageScatteringMinMax = 0.7f;

	[_E2A4(" Float kоэффициент скорости схождения угла прицеливания при выбитой руке")]
	public float HandDamageAccuracySpeed = 1.3f;

	[_E2A4("   Float\tКоэффициент изменения рабочего круга точности при кровотечении")]
	public float BloodFall = 1.45f;

	[_E2A4(" В процентах\tКоличество оставшихся патронов для перехода в состояние экономии патронов 0_1")]
	public float Caution = 0.3f;

	[_E2A4("  Float\tКоэффициент изменения предпочтительного круга точности в режиме экономии патронов")]
	public float ToCaution = 0.6f;

	[_E2A4("  Float\tКоэффициент контроля отдачи зависящий от отдачи оружия. Увеличивает текущий круг при вылете пули из ствола. Для одиночных выстрелов.")]
	public float RecoilControlCoefShootDone = 0.0003f;

	[_E2A4("  Float\tКоэффициент контроля отдачи зависящий от отдачи оружия. Увеличивает текущий круг при вылете пули из ствола. для автоматического огня")]
	public float RecoilControlCoefShootDoneAuto = 0.00015f;

	[_E2A4("Попугаи. Как высоко подскакивает при амплитуде прицел")]
	public float AMPLITUDE_FACTOR = 0.25f;

	[_E2A4("Попугаи. Скорость амплитуды прицеливания")]
	public float AMPLITUDE_SPEED = 0.1f;

	[_E2A4("Метры. Дистанция от новой точки прицеливая до старой, если больше чем Х то бот автоматически считает что он неприцелился независимо не от чего остального.")]
	public float DIST_FROM_OLD_POINT_TO_NOT_AIM = 15f;

	[_E2A4("")]
	public float DIST_FROM_OLD_POINT_TO_NOT_AIM_SQRT;

	[_E2A4("Метры. Если точка прицеливания ближе чем Х то бот не будет стрелять")]
	public float DIST_NOT_TO_SHOOT = 0.3f;

	[_E2A4("В момент смены положения текущий круг сведения увеличится на Х*степень смены положения")]
	public float PoseChnageCoef = 0.1f;

	[_E2A4(" В момент смены положения на лежа/нележа текущий круг сведения увеличится на Х")]
	public float LayFactor = 0.1f;

	[_E2A4("насколько вверх подбрасывает ствол. Коэфициент от отдачи оружия.")]
	public float RecoilYCoef = 0.0005f;

	[_E2A4("Скорость снижения отдачи вверх")]
	public float RecoilYCoefSppedDown = -0.52f;

	[_E2A4("насколько максимально может подняться отдача.")]
	public float RecoilYMax = 1f;

	public BotGlobalsScatteringSettings()
	{
		Update();
	}

	public void Update()
	{
		DIST_FROM_OLD_POINT_TO_NOT_AIM_SQRT = DIST_FROM_OLD_POINT_TO_NOT_AIM * DIST_FROM_OLD_POINT_TO_NOT_AIM;
	}
}
