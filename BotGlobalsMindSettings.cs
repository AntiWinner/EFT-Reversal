using System;
using EFT;

[Serializable]
public class BotGlobalsMindSettings
{
	[_E2A4("Мин число выстрелов наугад по позиции откуда велся огонь с близкого расстония")]
	public int MIN_SHOOTS_TIME = 2;

	[_E2A4("Маккс число выстрелов наугад по позиции откуда велся огонь с близкого расстония")]
	public int MAX_SHOOTS_TIME = 4;

	[_E2A4("Бот сможет напугатся только через это время после пропадения из поля последнего замеченного врага")]
	public float TIME_TO_RUN_TO_COVER_CAUSE_SHOOT_SEC = 15f;

	[_E2A4("Время через которое бот восстановит своих характеристики после получения повреждений")]
	public float DAMAGE_REDUCTION_TIME_SEC = 30f;

	[_E2A4("Минимальный урон который должен пулучить бот что бы получить точку опастности")]
	public float MIN_DAMAGE_SCARE = 20f;

	[_E2A4("Вероятность того что бот побежит если в него попасть пока он в укрытии и не может/видет стрелять во врага")]
	public float CHANCE_TO_RUN_CAUSE_DAMAGE_0_100 = 35f;

	[_E2A4("Через Х сек враг перестает выдваться раздачиком задач ботам.")]
	public float TIME_TO_FORGOR_ABOUT_ENEMY_SEC = 52f;

	[_E2A4("Через Х сек бот будет искать врага придя на место его последнего видения! должен быть меньше чем TIME_TO_FORGOR_ABOUT_ENEMY_SEC")]
	public float TIME_TO_FIND_ENEMY = 22f;

	[_E2A4("")]
	public float MAX_AGGRO_BOT_DIST = 100f;

	[_E2A4("Коэфицент точности восприятия позиции ткуда по игроку попали больше - точнее;")]
	public float HIT_POINT_DETECTION = 4f;

	[_E2A4("Коэфициент точки опастности при поиске укрытия. Опасная точка")]
	public float DANGER_POINT_CHOOSE_COEF = 1f;

	[_E2A4("Коэфициент точки опастности при поиске укрытия. Простая точка")]
	public float SIMPLE_POINT_CHOOSE_COEF = 0.4f;

	[_E2A4("Какойто коэфициент при поиске точки укрытия")]
	public float LASTSEEN_POINT_CHOOSE_COEF = 0.2f;

	[_E2A4("Какойто коэфициент при поиске точки укрытия")]
	public float COVER_DIST_COEF = 1.5f;

	[_E2A4("")]
	public float DIST_TO_FOUND_SQRT = 400f;

	[_E2A4("Ищет ли игрок противника при наличии GoalTarget")]
	public bool SEARCH_TARGET = true;

	[_E2A4("являются ли бировцы врагами по умолчанию для этого бота")]
	public bool DEFAULT_ENEMY_BEAR = true;

	[_E2A4("являются ли юсеки врагами по умолчанию для этого бота")]
	public bool DEFAULT_ENEMY_USEC = true;

	[_E2A4("являются ли дикие врагами по умолчанию для этого бота")]
	public bool DEFAULT_ENEMY_SAVAGE;

	[_E2A4("если флаг установлен хотя бы у одного бота в ботгруппе, то враждебной становится вся группа, в которой есть один враждебный игрок-ЧВК")]
	public bool ENEMY_BY_GROUPS_PMC_PLAYERS = true;

	[_E2A4("если флаг установлен хотя бы у одного бота в ботгруппе, то враждебной становится вся группа, в которой есть один враждебный дикий игрок")]
	public bool ENEMY_BY_GROUPS_SAVAGE_PLAYERS;

	[_E2A4("Если true то боссы не меняют своё поведение для игрока с высокой репутаций скупщика")]
	public bool BOSS_IGNORE_LOYALTY;

	[_E2A4("являются ли бировцы врагами по умолчанию для этого бота")]
	public EWarnBehaviour DEFAULT_BEAR_BEHAVIOUR = EWarnBehaviour.Attack;

	[_E2A4("являются ли юсеки врагами по умолчанию для этого бота")]
	public EWarnBehaviour DEFAULT_USEC_BEHAVIOUR = EWarnBehaviour.Attack;

	[_E2A4("являются ли дикие врагами по умолчанию для этого бота")]
	public EWarnBehaviour DEFAULT_SAVAGE_BEHAVIOUR = EWarnBehaviour.Ignore;

	[_E2A4("список дружественных типов ботов")]
	public WildSpawnType[] FRIENDLY_BOT_TYPES = new WildSpawnType[0];

	[_E2A4("список нейтральных типов ботов. Если бот не может предупреждать, то все боты из этого списка становятся дружественными или враждебными в зависимости от значения DEFAULT_ENEMY_SAVAGE")]
	public WildSpawnType[] WARN_BOT_TYPES = new WildSpawnType[0];

	[_E2A4("список враждебных типов ботов")]
	public WildSpawnType[] ENEMY_BOT_TYPES = new WildSpawnType[0];

	[_E2A4("список ботов, при атаке на которых бот будет считать игрока враждебным")]
	public WildSpawnType[] REVENGE_BOT_TYPES = new WildSpawnType[19]
	{
		WildSpawnType.assault,
		WildSpawnType.marksman,
		WildSpawnType.bossBully,
		WildSpawnType.followerBully,
		WildSpawnType.bossKilla,
		WildSpawnType.bossKojaniy,
		WildSpawnType.followerKojaniy,
		WildSpawnType.cursedAssault,
		WildSpawnType.bossGluhar,
		WildSpawnType.followerGluharAssault,
		WildSpawnType.followerGluharSecurity,
		WildSpawnType.followerGluharScout,
		WildSpawnType.followerGluharSnipe,
		WildSpawnType.followerSanitar,
		WildSpawnType.bossSanitar,
		WildSpawnType.assaultGroup,
		WildSpawnType.bossTagilla,
		WildSpawnType.followerTagilla,
		WildSpawnType.gifter
	};

	public float MAX_AGGRO_BOT_DIST_UPPER_LIMIT = 400f;

	[_E2A4("")]
	public float MAX_AGGRO_BOT_DIST_SQR_UPPER_LIMIT = 40000f;

	[_E2A4("")]
	public float MAX_AGGRO_BOT_DIST_SQR;

	[_E2A4("при беге когда до точки укрытия остается меньше Х то базовые опасные точки обнуляются")]
	public float DIST_TO_STOP_RUN_ENEMY = 15f;

	[_E2A4("Угл по которому бот понимает что противник смотриит на него")]
	public float ENEMY_LOOK_AT_ME_ANG = 15f;

	[_E2A4("min  Уровень агрессивности. [0....inf] На этот коеф умножается собственная сила => вероятность тактических моделей.")]
	public float MIN_START_AGGRESION_COEF = 1f;

	[_E2A4("max")]
	public float MAX_START_AGGRESION_COEF = 3f;

	[_E2A4("Расстояние с которого бот может \"ощутить\" пулю")]
	public float BULLET_FEEL_DIST = 160f;

	[_E2A4("Квадрат расстояния если ближе которого пуля ударится рядом с ботов то он ее воспримет (и если бы в укрытии то посчитает укрытии плохим)")]
	public float BULLET_FEEL_CLOSE_SDIST = 1f;

	[_E2A4("Шанс на то что после потери из виду врага и не имея нового бот пойдет искать врага сразу. Применимо только при атакующей тактики. (дальше идет проверка на свою силу и силу врага.)")]
	public float ATTACK_IMMEDIATLY_CHANCE_0_100 = 40f;

	[_E2A4("Шанс показать фак в момент когда бот увидит игрока")]
	public float CHANCE_FUCK_YOU_ON_CONTACT_100 = 10f;

	[_E2A4("Насколько падает агресия бота если убьют когонить в его группе.")]
	public float FRIEND_DEAD_AGR_LOW = -0.2f;

	[_E2A4("Насколько растет агресия если кто то умер недалеко от них")]
	public float FRIEND_AGR_KILL = 0.2f;

	[_E2A4("")]
	public float LAST_ENEMY_LOOK_TO = 40f;

	[_E2A4("")]
	public bool CAN_RECEIVE_PLAYER_REQUESTS_BEAR;

	[_E2A4("")]
	public bool CAN_RECEIVE_PLAYER_REQUESTS_USEC;

	[_E2A4("")]
	public bool CAN_RECEIVE_PLAYER_REQUESTS_SAVAGE;

	[_E2A4("Если бота атакует группа и параметр равен FALSE, то бот в ответ атакует тлько агрессора. Если TRUE - то всю группу агрессора")]
	public bool REVENGE_TO_GROUP;

	[_E2A4("Если флаг установлен, то бот атакует агрессора против игрока-дикого")]
	public bool REVENGE_FOR_SAVAGE_PLAYERS = true;

	[_E2A4("")]
	public bool CAN_USE_MEDS = true;

	[_E2A4("")]
	public float SUSPETION_POINT_CHANCE_ADD100 = 90f;

	[_E2A4("")]
	public bool AMBUSH_WHEN_UNDER_FIRE = true;

	[_E2A4("Псоле того как чела напугали выстрелами и загнали в амбуш - он будет иметь к такому резист на Х сек")]
	public float AMBUSH_WHEN_UNDER_FIRE_TIME_RESIST = 60f;

	[_E2A4("Если чел выл виден кем то менее Х сек назад - то мы его идем атаковать")]
	public float ATTACK_ENEMY_IF_PROTECT_DELTA_LAST_TIME_SEEN = 2.5f;

	[_E2A4("Если чел выл виден кем то менее Х сек назад - и мы вне укрытия и укрытия нет то мы похлдим и подожем его.")]
	public float HOLD_IF_PROTECT_DELTA_LAST_TIME_SEEN = 8.5f;

	[_E2A4("Когда бот ищет укрытие (в некоторых случаях) если поселедний видимый враг был виден меньше чем Х времени назад то будет пытаться найти позицию со стрельбой")]
	public float FIND_COVER_TO_GET_POSITION_WITH_SHOOT = 2f;

	[_E2A4("какой тип времени браться для того что бы решить атаковать или нет. Реальный видимы или \"по ощущениям\"")]
	public bool PROTECT_TIME_REAL = true;

	[_E2A4("Шанс что бот когда предупреждает игрока он посреляет рядом а не говорит")]
	public float CHANCE_SHOOT_WHEN_WARN_PLAYER_100 = 25f;

	[_E2A4("")]
	public bool CAN_PANIC_IS_PROTECT;

	[_E2A4("Не убегать для защиты себя")]
	public bool NO_RUN_AWAY_FOR_SAFE;

	[_E2A4("Если в части тела меньше чем Х то она будет подвержена лечению")]
	public float PART_PERCENT_TO_HEAL = 0.65f;

	[_E2A4("Когда бот защищает когото и последний врга был виден больше чем Х сек назад то можно попытаться похиляться.")]
	public float PROTECT_DELTA_HEAL_SEC = 10f;

	[_E2A4("")]
	public bool CAN_STAND_BY = true;

	[_E2A4("Может ли бот кдать реквесты другим ботам.")]
	public bool CAN_THROW_REQUESTS = true;

	[_E2A4("После того как бот скажет фразу следущий бот из этойже группы сможет сказать фразу только через Х. Если Х<0 то задержки нет.")]
	public float GROUP_ANY_PHRASE_DELAY = -1f;

	[_E2A4("Тоже самое что и GROUP_ANY_PHRASE_DELAY только относится к конкретному типу фразы")]
	public float GROUP_EXACTLY_PHRASE_DELAY = -1f;

	[_E2A4("При наличие врага что бы бот начал хилится враг должен быть дальше чем Х метров")]
	public float DIST_TO_ENEMY_YO_CAN_HEAL = 30f;

	[_E2A4("Шанс что после первых 2 действий при предупреждении бот будет стоять и ждать след 4 сек.")]
	public float CHANCE_TO_STAY_WHEN_WARN_PLAYER_100 = 80f;

	[_E2A4("Выйдет из догфайта")]
	public float DOG_FIGHT_OUT = 5f;

	[_E2A4("Войдет в догфайт")]
	public float DOG_FIGHT_IN = 3f;

	[_E2A4("Если бот вошел в догфайт более 2 раз за Х сек он вместо этого будет стрелять с места.")]
	public float SHOOT_INSTEAD_DOG_FIGHT = 6f;

	[_E2A4("Дистанция для перехода в амбушь для пистолетов и шотганов")]
	public float PISTOL_SHOTGUN_AMBUSH_DIST = 30f;

	[_E2A4("Дистанция для перехода в амбушь для остального")]
	public float STANDART_AMBUSH_DIST = 100f;

	[_E2A4("Коэфициент для пересчета силы игрока в амбуш дистанс")]
	public float AI_POWER_COEF = 120f;

	[_E2A4("")]
	public float COVER_SECONDS_AFTER_LOSE_VISION = 10f;

	[_E2A4("При поиске укрытий бот будет всегда ныкаться если он имеет повреждения")]
	public bool COVER_SELF_ALWAYS_IF_DAMAGED;

	[_E2A4("Если врга был виден менее Х сек назад то дист до укрытия для бега будет больше на 1.5")]
	public float SEC_TO_MORE_DIST_TO_RUN = 10f;

	[_E2A4("Перерыв между лечением.")]
	public float HEAL_DELAY_SEC = 5f;

	[_E2A4("Задержка для ощущения попадания если бот настороже")]
	public float HIT_DELAY_WHEN_HAVE_SMT = -1f;

	[_E2A4("Задержка для ощущения попадания если бот в мирном")]
	public float HIT_DELAY_WHEN_PEACE = -1f;

	[_E2A4("Бот говорит только через очередь фраз и по приоритетам")]
	public bool TALK_WITH_QUERY = true;

	[_E2A4("Сколько времени бот будет паниковать min")]
	public float DANGER_EXPIRE_TIME_MIN = 0.4f;

	[_E2A4("Сколько времени бот будет паниковать max")]
	public float DANGER_EXPIRE_TIME_MAX = 1.2f;

	[_E2A4("Вес шанса бега при сильной паники")]
	public float PANIC_RUN_WEIGHT = 1f;

	[_E2A4("Вес шанса сесть при сильной паники")]
	public float PANIC_SIT_WEIGHT = 80f;

	[_E2A4("Вес шанса лечь при сильной паники")]
	public float PANIC_LAY_WEIGHT = 20f;

	[_E2A4("Вес шанса ничего не делать при легкой панике")]
	public float PANIC_NONE_WEIGHT = 40f;

	[_E2A4("Шанс сесть если паника была легкой (выстрел не в бота или рядом)")]
	public float PANIC_SIT_WEIGHT_PEACE = 60f;

	[_E2A4("Может ли бот исполнять реквесты")]
	public bool CAN_EXECUTE_REQUESTS = true;

	[_E2A4("Если урон был нанесет с дистанции меньше чем Х то бот этого врага \"заметит\", даже если он к нему спиной.")]
	public float DIST_TO_ENEMY_SPOTTED_ON_HIT = 20f;

	[_E2A4("Сколько бот находитьcя в режиме \"под огнем\"")]
	public float UNDER_FIRE_PERIOD = 2.5f;

	[_E2A4("Использовать медецину только из СейфКонтейнера")]
	public bool MEDS_ONLY_SAFE_CONTAINER;

	[_E2A4("Может ли бот выкидывать вещи")]
	public bool CAN_DROP_ITEMS = true;

	[_E2A4("Может ли бот поднимать вещи выброшеные другими игроками")]
	public bool CAN_TAKE_ITEMS;

	[_E2A4("Дистанция с которой бот увидит и запомнит брошенный предмет")]
	public float THROW_DIST_TO_SEE = 30f;

	[_E2A4("")]
	public bool CAN_TAKE_ANY_ITEM;

	[_E2A4("Будет ли бот преследовать топористов")]
	public bool WILL_PERSUE_AXEMAN;

	[_E2A4("Максимальная дистанция на которой бот бдует преследовать топориста бегом.")]
	public float MAX_DIST_TO_RUN_PERSUE_AXEMAN = 200f;

	[_E2A4("Максимальная дистанция на которой бот бдует преследовать топориста")]
	public float MAX_DIST_TO_PERSUE_AXEMAN = 120f;

	[_E2A4("Использование хирург набора только из сейф контейнера")]
	public bool SURGE_KIT_ONLY_SAFE_CONTAINER = true;

	[_E2A4("")]
	public bool CAN_USE_LONG_COVER_POINTS = true;

	[_E2A4("Можно ли есть-пить")]
	public bool CAN_USE_FOOD_DRINK = true;

	[_E2A4("Минимальная переодичность для есть-пить")]
	public float FOOD_DRINK_DELAY_SEC = 40f;

	[_E2A4("Что именно делать когда приходишь к трупу  1 - использовать набор медециню 2 - попробовать взять пушку  Дефолт - смотреть. public bool CAN_TALK = true;")]
	public int HOW_WORK_OVER_DEAD_BODY;

	[_E2A4("може ли говорить    ")]
	public bool CAN_TALK = true;

	public bool ACTIVE_FORCE_ATTACK_EVENTS = true;

	public bool ACTIVE_FOLLOW_PLAYER_EVENTS = true;

	public BotGlobalsMindSettings()
	{
		Update();
	}

	public void Update()
	{
		MAX_AGGRO_BOT_DIST_SQR = MAX_AGGRO_BOT_DIST * MAX_AGGRO_BOT_DIST;
		if (DEFAULT_BEAR_BEHAVIOUR.HasFlag(EWarnBehaviour.Attack))
		{
			DEFAULT_BEAR_BEHAVIOUR |= EWarnBehaviour.Warn;
		}
		if (DEFAULT_SAVAGE_BEHAVIOUR.HasFlag(EWarnBehaviour.Attack))
		{
			DEFAULT_SAVAGE_BEHAVIOUR |= EWarnBehaviour.Warn;
		}
		if (DEFAULT_USEC_BEHAVIOUR.HasFlag(EWarnBehaviour.Attack))
		{
			DEFAULT_USEC_BEHAVIOUR |= EWarnBehaviour.Warn;
		}
	}
}
