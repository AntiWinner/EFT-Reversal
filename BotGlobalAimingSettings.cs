using System;

[Serializable]
public class BotGlobalAimingSettings
{
	[_E2A4("Время для Максимальное улучшение стрельбы в зависимости от того как долго бот целиться")]
	public float MAX_AIM_PRECICING = 5f;

	[_E2A4("Коэфициент скорость пристрелки. Больше == Лучше")]
	public float BETTER_PRECICING_COEF = 0.7f;

	[_E2A4("дистанции при сдвижение на который прицеливание не прерветься по Y")]
	public float RECLC_Y_DIST = 1.2f;

	[_E2A4("дистанции при сдвижение на который прицеливание не прерветься по XZ")]
	public float RECALC_DIST = 0.7f;

	[_E2A4("    public float BASE_ANF_COEF = 7;")]
	public float RECALC_SQR_DIST;

	[_E2A4("усиленное прицеливание когда выглядываешь за укрытия")]
	public float COEF_FROM_COVER = 0.85f;

	[_E2A4("Время прицеливания умножается на этот коэфициент если чар паникует")]
	public float PANIC_COEF = 3.5f;

	[_E2A4("Коэфициент увеличения разлета при панике")]
	public float PANIC_ACCURATY_COEF = 3f;

	[_E2A4("Коэфициент улучшенного прицеливания")]
	public float HARD_AIM = 0.75f;

	[_E2A4("шанс прицеливания по время стрельбы [0;100]")]
	public int HARD_AIM_CHANCE_100 = 100;

	[_E2A4("Время паники при обычное")]
	public float PANIC_TIME = 6f;

	[_E2A4("После скольки попыток доприцелится бот все равно переприцелится даже если было цель очень близко")]
	public int RECALC_MUST_TIME = 3;

	[_E2A4("После скольки попыток доприцелится бот все равно переприцелится даже если было цель очень близко  min max")]
	public int RECALC_MUST_TIME_MIN = 1;

	[_E2A4("После скольки попыток доприцелится бот все равно переприцелится даже если было цель очень близко  min max")]
	public int RECALC_MUST_TIME_MAX = 2;

	[_E2A4("Время паники при попадании по боту.")]
	public float DAMAGE_PANIC_TIME = 25f;

	[_E2A4("уровень стрельбы по опасной точке")]
	public float DANGER_UP_POINT = 1.3f;

	[_E2A4("насколько лучше может стать стрельба от пристрелки - 0.15 == 85%.  0.5 == 50%  . 1 == 0%")]
	public float MAX_AIMING_UPGRADE_BY_TIME = 0.7f;

	[_E2A4("это вероятность того что бот скосит стрельбу при попадании по нему. Альтернатива - ухудшить время прицеливания.")]
	public float DAMAGE_TO_DISCARD_AIM_0_100 = 86f;

	[_E2A4("Минимальное ухудшение времени прицеливания")]
	public float MIN_TIME_DISCARD_AIM_SEC = 0.3f;

	[_E2A4("Макс ухудшение времени прицеливания")]
	public float MAX_TIME_DISCARD_AIM_SEC = 1.3f;

	[_E2A4("Коэфициент зависимости прицеливания в горизонтальной плоскости в зависимости от угла до цели")]
	public float XZ_COEF = 0.15f;

	[_E2A4("Коэфициент зависимости прицеливания в горизонтальной плоскости в зависимости от угла до цели")]
	public float XZ_COEF_STATIONARY_BULLET = 0.15f;

	[_E2A4("Коэфициент зависимости прицеливания в горизонтальной плоскости в зависимости от угла до цели")]
	public float XZ_COEF_STATIONARY_GRENADE = 0.25f;

	[_E2A4("Сколько примерно нужно выстрелов по цели что бы поменять приоритет на стрельбу по ногам")]
	public int SHOOT_TO_CHANGE_PRIORITY = 5525;

	[_E2A4("Базовое время прицеливания. Прибавляетя к результату полученному по формуле")]
	public float BOTTOM_COEF = 0.3f;

	[_E2A4("Прибавляется к первому времени прицеливания в игрока ботом")]
	public float FIRST_CONTACT_ADD_SEC = 0.1f;

	[_E2A4("шанс срабатывания задержки указанной в FIRST_CONTACT_ADD_SEC")]
	public float FIRST_CONTACT_ADD_CHANCE_100 = 80f;

	[_E2A4("Базовое время, через которое бот отойдет от попадания по нему и перестанет афектить прицеливание")]
	public float BASE_HIT_AFFECTION_DELAY_SEC = 0.77f;

	[_E2A4("Минимально насколько может отойти прицел от попадания в бота в градусах")]
	public float BASE_HIT_AFFECTION_MIN_ANG = 4f;

	[_E2A4("Максимально насколько может отойти прицел от попадания в бота в градусах")]
	public float BASE_HIT_AFFECTION_MAX_ANG = 18f;

	[_E2A4("Базовый сдвиг в метрах для прицеливания (пример: BASE_SHIEF=5 => значит на расстоянии 20 метров прицеливаться будет как на 20+5=25)")]
	public float BASE_SHIEF = 1f;

	[_E2A4("Базовый сдвиг в метрах для прицеливания (пример: BASE_SHIEF=5 => значит на расстоянии 20 метров прицеливаться будет как на 20+5=25)")]
	public float BASE_SHIEF_STATIONARY_BULLET = 0.05f;

	[_E2A4("Базовый сдвиг в метрах для прицеливания (пример: BASE_SHIEF=5 => значит на расстоянии 20 метров прицеливаться будет как на 20+5=25)")]
	public float BASE_SHIEF_STATIONARY_GRENADE = 0.05f;

	[_E2A4("Настолько бот становится косее если он был поврежден")]
	public float SCATTERING_HAVE_DAMAGE_COEF = 2f;

	[_E2A4("Модификатор зависимости прицеливанипя от дистанции не линейног (за линейность отвечает др. параметр).   //рекумендуемые знаечни 0.2..1.3.     //Меньше 1 - значит чем дальше тем будет точнее чем лнейная зависимость. Больше - косее.")]
	public float SCATTERING_DIST_MODIF = 0.8f;

	[_E2A4(" Модификатор зависимости прицеливанипя от дистанции не линейног (за линейность отвечает др. параметр).   //рекумендуемые знаечни 0.2..1.3.     //Меньше 1 - значит чем дальше тем будет точнее чем лнейная зависимость. Больше - косее.")]
	public float SCATTERING_DIST_MODIF_CLOSE = 0.6f;

	[_E2A4("default - в центр тушки // 1 - рандщомно + центр // 2 - рандомно кроме ног + центр // 3 - первая замеченная часть тела // 4 - рандщомно + центр + без головы // 5 - рандомно кроме ног + центр + без головы //6 - сначала голову")]
	public int AIMING_TYPE = 2;

	[_E2A4("Если враг ближе чем Х то прицеливатся будем только в тело.")]
	public float DIST_TO_SHOOT_TO_CENTER = 3f;

	[_E2A4("Если враг ближе чем Х то разлета не будет.")]
	public float DIST_TO_SHOOT_NO_OFFSET = 3f;

	[_E2A4("Если больше 0 то используется сферкаст с указанным радиусом. Если меньше 0 то используется лайнкаст.")]
	public float SHPERE_FRIENDY_FIRE_SIZE = -1f;

	[_E2A4("Настолько больше будет разлет если бот стреляет сходу")]
	public float COEF_IF_MOVE = 1.5f;

	[_E2A4("Настолько дольше будет прицеливание если бот стреляет сходу")]
	public float TIME_COEF_IF_MOVE = 1.5f;

	[_E2A4("Бот считается что двигается если он прошел за кадр больше чем Х")]
	public float BOT_MOVE_IF_DELTA = 0.01f;

	[_E2A4("Шанс что следущем выстрелом бот промажет")]
	public float NEXT_SHOT_MISS_CHANCE_100 = 100f;

	[_E2A4("Насколько выше выстрелит бот если захочет стрелять мимо")]
	public float NEXT_SHOT_MISS_Y_OFFSET = 1f;

	[_E2A4("Шанс что бот будет включать фонарик когда целится")]
	public float ANYTIME_LIGHT_WHEN_AIM_100 = -1f;

	[_E2A4("Через сколько секундр после первого замечания врага можно будет стрелять в любую часть тела")]
	public float ANY_PART_SHOOT_TIME = 900f;

	[_E2A4("Дальность отступления назад для проверки возможности выстрела (Что бы нельзябыло просунуть ствол через дверь)")]
	public float WEAPON_ROOT_OFFSET = 0.35f;

	[_E2A4("Минимальный урон что бы бот получил дебаффы от урона")]
	public float MIN_DAMAGE_TO_GET_HIT_AFFETS = 1f;

	[_E2A4("Максимальное время прицеливания")]
	public float MAX_AIM_TIME = 1.5f;

	[_E2A4("")]
	public float OFFSET_RECAL_ANYWAY_TIME = 1f;

	[_E2A4("Уровень сжатия прицельной сферы сверху")]
	public float Y_TOP_OFFSET_COEF = 0.2f;

	[_E2A4("Уровень сжатия прицельной сферы снизу")]
	public float Y_BOTTOM_OFFSET_COEF = 0.2f;

	[_E2A4("Если враг уйдет дальше чем Х градусов в 1 сторону то  бот покинет стационар")]
	public float STATIONARY_LEAVE_HALF_DEGREE = 45f;

	[_E2A4("Базовое кл-во попадаий мимо МИН")]
	public int BAD_SHOOTS_MIN;

	[_E2A4("Базовое кл-во попадаий мимо МАКС")]
	public int BAD_SHOOTS_MAX;

	[_E2A4("сли мы стреляем мимо, то бот настолько отлоит свой прицел от цели в метрах")]
	public float BAD_SHOOTS_OFFSET = 1f;

	[_E2A4("Базовый коэфициент из формулы == N        N*ln(x/5+1.2)")]
	public float BAD_SHOOTS_MAIN_COEF = 1f;

	[_E2A4("")]
	public float START_TIME_COEF = 1f;

	public BotGlobalAimingSettings()
	{
		Update();
	}

	public void Update()
	{
		RECALC_SQR_DIST = RECALC_DIST * RECALC_DIST;
	}
}
