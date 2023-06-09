using System;

[Serializable]
public class BotGlobalsGrenadeSettings
{
	[_E2A4("Частота попыток проверить можно ли бросить гранату из за укрытия")]
	public float DELTA_NEXT_ATTEMPT_FROM_COVER = 5f;

	[_E2A4("")]
	public float DELTA_NEXT_ATTEMPT = 10f;

	[_E2A4("дистанция проверки нет ли рядом с точкой броска друзей или автора броска")]
	public float MIN_DIST_NOT_TO_THROW = 8f;

	[_E2A4("Время когда считается что недавно бот бросал гранату (Нужно что бы знать что пора возвращатся в укрытие)")]
	public float NEAR_DELTA_THROW_TIME_SEC = 2f;

	[_E2A4("Мин дистанция броска гранаты")]
	public float MIN_THROW_GRENADE_DIST = 12f;

	[_E2A4("    public float MAX_THROW_GRENADE_DIST_SQRT;")]
	public float MIN_THROW_GRENADE_DIST_SQRT;

	[_E2A4("")]
	public float MIN_DIST_NOT_TO_THROW_SQR;

	[_E2A4("Дистанция если бот биже нее к предполагаемому месту  попадания гранаты то он запишет точку в опастность")]
	public float RUN_AWAY = 15f;

	[_E2A4("")]
	public float RUN_AWAY_SQR;

	[_E2A4("Дистанция если бот биже нее к предполагаемому месту  попадания гранаты то он запишет точку в опастность")]
	public float ADD_GRENADE_AS_DANGER = 65f;

	[_E2A4("")]
	public float ADD_GRENADE_AS_DANGER_SQR;

	[_E2A4("Шанс увидеть летящую в бота гранату")]
	public float CHANCE_TO_NOTIFY_ENEMY_GR_100 = 95f;

	[_E2A4("Уровень разлета броска гранаты (Меньше == точнее)")]
	public float GrenadePerMeter = 0.25f;

	[_E2A4("")]
	public float REQUEST_DIST_MUST_THROW_SQRT;

	[_E2A4("Если дистанция меньше чем Х метров до точки броска и враг виден то реквест не прервется.")]
	public float REQUEST_DIST_MUST_THROW = 2f;

	[_E2A4("2 - Убегает от точного места гранаты   .  3 - убегает от предположительного места гранаты. остальное - убегает от средней точки.")]
	public int BEWARE_TYPE = 2;

	[_E2A4("Вероятность желания пострелять в дым.")]
	public float SHOOT_TO_SMOKE_CHANCE_100 = 50f;

	[_E2A4("Шанс что будет будет убегать будучи зафлешенным, а не стрелять по точке если может.")]
	public float CHANCE_RUN_FLASHED_100;

	[_E2A4("Если точка последней опастности ближе чем Х и бот в блайнде то он может по ней посрелять")]
	public float MAX_FLASHED_DIST_TO_SHOOT = 10f;

	[_E2A4("")]
	public float MAX_FLASHED_DIST_TO_SHOOT_SQRT;

	[_E2A4("Коэфицие на которое умножается время которое бот получит после расчетов слеповой грены")]
	public float FLASH_GRENADE_TIME_COEF = 0.2f;

	[_E2A4("дымовой гранаты. Насколько бльше считается радиус для точек которые бот не будет занимать пока идет дым чем коллайдер дыма")]
	public float SIZE_SPOTTED_COEF = 2f;

	[_E2A4("Коэфициент бота для привлечение внимания гранатой дымовой")]
	public float BE_ATTENTION_COEF = 4f;

	[_E2A4("Сколько секунд бот будет стрелять будучи зафлешеным")]
	public float TIME_SHOOT_TO_FLASH = 2f;

	[_E2A4("Как близко к источнику дыма должна быть подозрительность что бы туда начать стрелять.")]
	public float CLOSE_TO_SMOKE_TO_SHOOT = 5f;

	[_E2A4("")]
	public float CLOSE_TO_SMOKE_TO_SHOOT_SQRT;

	[_E2A4("Как давно в смоках должен был быть шум что бы туда пострелять")]
	public float CLOSE_TO_SMOKE_TIME_DELTA = 7f;

	[_E2A4("Частота проверки смоков.")]
	public float SMOKE_CHECK_DELTA = 1f;

	[_E2A4("Через стлько сек после броска точка опастности станет активной (бот будет на нее реагировать => убегать если надо) (раньше было в глобалсах)")]
	public float DELTA_GRENADE_START_TIME = 0.7f;

	[_E2A4("Бот перейдет в состояние Ambush если в его зоне кинули смока")]
	public float AMBUSH_IF_SMOKE_IN_ZONE_100 = 40f;

	[_E2A4("Через сколько секунд бот переейдет в свое прежнее состояние после смоков.")]
	public float AMBUSH_IF_SMOKE_RETURN_TO_ATTACK_SEC = 30f;

	[_E2A4("Бот НЕ будет убегать от гранат кинутых другими ботами")]
	public bool NO_RUN_FROM_AI_GRENADES = true;

	[_E2A4("")]
	public float MAX_THROW_POWER = 25f;

	[_E2A4("точность броска гранаты . меньше-точнее")]
	public float GrenadePrecision;

	[_E2A4("Остановится ли бот для броска гранаты. Если остановится бросит точнее, если нет - может кинуть не совсем туда куда хочет.")]
	public bool STOP_WHEN_THROW_GRENADE = true;

	[_E2A4("Сколько секунд ождать посл броска своей световой гранаты что бы отвернуться")]
	public float WAIT_TIME_TURN_AWAY = 1.2f;

	[_E2A4("На сколько оказывает подавление дымовая граната")]
	public float SMOKE_SUPPRESS_DELTA = 20f;

	[_E2A4("На сколько оказывает подавление дамажащая граната")]
	public float DAMAGE_GRENADE_SUPPRESS_DELTA = 8f;

	[_E2A4("На сколько оказывает подавление светошумовая граната")]
	public float STUN_SUPPRESS_DELTA = 9f;

	[_E2A4("Если включено то граната будет вылетать не из руки а оттуда откуда бот предпосчитал что её надо броситть")]
	public bool CHEAT_START_GRENADE_PLACE;

	[_E2A4("Может ли бот кидать гранаты при прямом контакте с врагом")]
	public bool CAN_THROW_STRAIGHT_CONTACT = true;

	[_E2A4("Делей для прямого броска гранаты после первого контакта")]
	public float STRAIGHT_CONTACT_DELTA_SEC = -1f;

	[_E2A4("Базовый угл для расчета броска гранаты   ,1 - 45 градусов ,  2 - 25 градусов ,   3 - 65 градусов  , 4 - 15 градусов, 5 - 35, 6 - 55")]
	public int ANG_TYPE = 1;

	[_E2A4("Если цель находится на проценте пути дальше чем Х то бросок будет")]
	public float MIN_THROW_DIST_PERCENT_0_1 = 0.6f;

	[_E2A4("Насколько умножится длительность ослепления если бот был в найтвижене")]
	public float FLASH_MODIF_IS_NIGHTVISION = 2f;

	[_E2A4("Через сколько сек после первого контакта можно кидать гранату?")]
	public float FIRST_TIME_SEEN_DELTA_CAN_THROW;

	[_E2A4("Должен ли бот вставать когда кидает грену")]
	public bool SHALL_GETUP = true;

	[_E2A4("Может ли бот кидать грену лежа")]
	public bool CAN_LAY;

	[_E2A4("бот не будет убегать от дымовой гранаты")]
	public bool IGNORE_SMOKE_GRENADE;

	public BotGlobalsGrenadeSettings()
	{
		Update();
	}

	public void Update()
	{
		CLOSE_TO_SMOKE_TO_SHOOT_SQRT = CLOSE_TO_SMOKE_TO_SHOOT * CLOSE_TO_SMOKE_TO_SHOOT;
		MAX_FLASHED_DIST_TO_SHOOT_SQRT = MAX_FLASHED_DIST_TO_SHOOT * MAX_FLASHED_DIST_TO_SHOOT;
		REQUEST_DIST_MUST_THROW_SQRT = REQUEST_DIST_MUST_THROW * REQUEST_DIST_MUST_THROW;
		RUN_AWAY_SQR = RUN_AWAY * RUN_AWAY;
		ADD_GRENADE_AS_DANGER_SQR = ADD_GRENADE_AS_DANGER * ADD_GRENADE_AS_DANGER;
		MIN_DIST_NOT_TO_THROW_SQR = MIN_DIST_NOT_TO_THROW * MIN_DIST_NOT_TO_THROW;
		MIN_THROW_GRENADE_DIST_SQRT = MIN_THROW_GRENADE_DIST * MIN_THROW_GRENADE_DIST;
	}
}
