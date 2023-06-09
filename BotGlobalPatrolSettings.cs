using System;

[Serializable]
public class BotGlobalPatrolSettings
{
	[_E2A4("Время которое бот проводит на точке для осмотра на обычном пути")]
	public float LOOK_TIME_BASE = 12f;

	[_E2A4(" Время которое бот проводит на точки Резервного пути")]
	public float RESERVE_TIME_STAY = 72f;

	[_E2A4(" Время которое бот проводит на точке лута Резервного пути")]
	public float RESERVE_LOOT_TIME_STAY = 15f;

	[_E2A4("Дельта поиска длуга на резервном пути для душевной беседы")]
	public float FRIEND_SEARCH_SEC = 12f;

	[_E2A4("Дельта фразы в беседе")]
	public float TALK_DELAY = 1.1f;

	[_E2A4("Дельта на говорливость")]
	public float MIN_TALK_DELAY = 10f;

	[_E2A4("Если друзей нет то будем говорить с такой дельтой")]
	public float TALK_DELAY_BIG = 15.1f;

	[_E2A4("базовое время когда бот может решить сменить пути")]
	public float CHANGE_WAY_TIME = 125.1f;

	[_E2A4("")]
	public float MIN_DIST_TO_CLOSE_TALK = 5f;

	[_E2A4("Коэфициент на который режется дистанция в \"мирном\" режиме")]
	public float VISION_DIST_COEF_PEACE = 0.5f;

	[_E2A4("")]
	public float MIN_DIST_TO_CLOSE_TALK_SQR;

	[_E2A4("Шанс срезать путь при патрулировании")]
	public float CHANCE_TO_CUT_WAY_0_100 = 75f;

	[_E2A4("Минимальный процент на который дистанци пути при патруле мб срезанна")]
	public float CUT_WAY_MIN_0_1 = 0.4f;

	[_E2A4("Максимальный")]
	public float CUT_WAY_MAX_0_1 = 0.65f;

	[_E2A4("Шанс сменить путь патрулирования")]
	public float CHANCE_TO_CHANGE_WAY_0_100 = 50f;

	[_E2A4("Вероятность того что бот попытается сделать контрольный выстрел в тело врага")]
	public int CHANCE_TO_SHOOT_DEADBODY = 52;

	[_E2A4("Время жизни подозрительной точки.")]
	public float SUSPETION_PLACE_LIFETIME = 7f;

	[_E2A4("Через сколько времени если бот встал на резервный маршрут он попробует найти новый.")]
	public float RESERVE_OUT_TIME = 30f;

	[_E2A4("Дистанция до срединной точки пути для резервных путей что бы путь можно было выбрать.")]
	public float CLOSE_TO_SELECT_RESERV_WAY = 25f;

	[_E2A4("Если бот захочет выполнить запрос на предупреждение он должен быть ближе чем Х к цели по ОСИ Y - это для этажности, но если земля кривая то лучше ставить побольше.")]
	public float MAX_YDIST_TO_START_WARN_REQUEST_TO_REQUESTER = 4f;

	[_E2A4("Может ли вставать на альтернативные пути")]
	public bool CAN_CHOOSE_RESERV;

	[_E2A4("использует ли ТОЛЬКО на альтернативные пути")]
	public bool USE_ONLY_RESERV;

	[_E2A4("Переодичность смены направления взгляда головы.")]
	public float HEAD_PERIOD_TIME = 13f;

	[_E2A4("Переодичность смены направления взгляда головы.")]
	public float HEAD_FRONT_PERIOD_TIME = 1f;

	[_E2A4("Шанс проиграть жест при встречи")]
	public float CHANCE_TO_PLAY_GESTURE_WHEN_CLOSE = 50f;

	[_E2A4("Скорость поворота головы")]
	public float HEAD_TURN_SPEED = 41f;

	[_E2A4("Угл поворота")]
	public float HEAD_ANG_ROTATE = 25f;

	[_E2A4("шанс сказать фразу во время приветствия")]
	public float CHANCE_TO_PLAY_VOICE_WHEN_CLOSE = 50f;

	[_E2A4("")]
	public float GO_TO_NEXT_POINT_DELTA = 90f;

	[_E2A4("")]
	public float GO_TO_NEXT_POINT_DELTA_RESERV_WAY = 15f;

	[_E2A4("С какого расстояния ощущают труп")]
	public float DEAD_BODY_SEE_DIST = 20f;

	[_E2A4("Если бот оказался дальше чем Х метров то он забудет о теле")]
	public float DEAD_BODY_LEAVE_DIST = 50f;

	[_E2A4("Может ли бот смотреть на трупы в мирном режиме")]
	public bool CAN_LOOK_TO_DEADBODIES;

	[_E2A4("Сколько бот будет делать PeacefulAction")]
	public float GESTURE_LENGTH = 2f;

	[_E2A4("Надо ли остановиться для PeacefulAction")]
	public bool SHALL_STOP_IN_PEACEFUL_ACTION;

	[_E2A4("Зафорсить ли оппонента на акшен")]
	public bool FORCE_OPPONENT_TO_PEAEFUL;

	[_E2A4("Сколько секунд бот будет стоять на точке применения набора хирурга")]
	public float RESERVE_USE_SURGE_TIME_STAY = 40f;

	[_E2A4("")]
	public bool RESERV_CAN_USE_MEDS;

	[_E2A4("Надо ли остановиться для PeacefulAction")]
	public bool USE_PATROL_POINT_ACTION_MOVE_BY_RESERVE_WAY = true;

	[_E2A4("Шанс начать использовать хир набор если бот смотрит на тело.")]
	public float USE_SURGIAL_KIT_OVER_THE_BODY_CAHNCE_100 = 50f;

	[_E2A4("Шанс начать использовать хир набор второй раз если бот смотрит на тело.")]
	public float USE_SURGIAL_KIT_OVER_THE_BODY_SECOND_CAHNCE_100 = 50f;

	[_E2A4("Если у фолловера стоит режим изди вместе с боссом то он задержится на Х")]
	public float FOLLOWER_START_MOVE_DELAY;

	[_E2A4("Использование закешированных путей для патруля")]
	public bool USE_CHACHE_WAYS = true;

	[_E2A4("Список вещей доступных для дропа")]
	public string ITEMS_TO_DROP = "";

	[_E2A4("Шанс что бот побежит между точками на патруле если это предусмотрено другими условиями и бот использует кешированные маршруты")]
	public float SPRINT_BETWEEN_CACHED_POINTS = 150f;

	[_E2A4("Базовая Переодичность проверки магазина")]
	public float CHECK_MAGAZIN_PERIOD = 30f;

	[_E2A4("Переодичность с которой бот может есть/пить")]
	public float EAT_DRINK_PERIOD = 30f;

	[_E2A4("")]
	public float WATCH_SECOND_WEAPON_PERIOD = 30f;

	[_E2A4("")]
	public bool CAN_WATCH_SECOND_WEAPON;

	[_E2A4("Базовое время осмотра трупа")]
	public float DEAD_BODY_LOOK_PERIOD = 17f;

	[_E2A4("")]
	public bool CAN_HARD_AIM;

	[_E2A4("")]
	public bool CAN_PEACEFUL_LOOK = true;

	[_E2A4("")]
	public bool CAN_FRIENDLY_TILT;

	[_E2A4("может ли бот жестикулировать")]
	public bool CAN_GESTUS = true;

	[_E2A4("При рождении будет пытаться выбрать резервный путь")]
	public bool TRY_CHOOSE_RESERV_WAY_ON_START;

	[_E2A4("")]
	public bool CAN_CHECK_MAGAZINE = true;

	[_E2A4(" При поднятии предмета всегда отправлять в рюкзак или контейнер")]
	public bool PICKUP_ITEMS_TO_BACKPACK_OR_CONTAINER;

	public BotGlobalPatrolSettings()
	{
		Update();
	}

	public void Update()
	{
		MIN_DIST_TO_CLOSE_TALK_SQR = MIN_DIST_TO_CLOSE_TALK * MIN_DIST_TO_CLOSE_TALK;
	}
}
