using System;

[Serializable]
public class BotGlobalsBossSettings
{
	[_E2A4("Дистанция ближе которой босс будет предупреждать")]
	public float BOSS_DIST_TO_WARNING = 34f;

	[_E2A4("")]
	public float BOSS_DIST_TO_WARNING_SQRT = 576f;

	[_E2A4("Дистанция ближе которой босс будет предупреждать игроков фракции юсек")]
	public float BOSS_DIST_TO_WARNING_USEC = 34f;

	[_E2A4("")]
	public float BOSS_DIST_TO_WARNING_SQRT_USEC = 576f;

	[_E2A4("Дистанция ближе которой босс будет предупреждать игроков фракции bear")]
	public float BOSS_DIST_TO_WARNING_BEAR = 34f;

	[_E2A4("")]
	public float BOSS_DIST_TO_WARNING_SQRT_BEAR = 576f;

	[_E2A4("Дистанция выйдя за которую босс перестает обращать внимание на дикого")]
	public float BOSS_DIST_TO_WARNING_OUT = 43f;

	[_E2A4("")]
	public float BOSS_DIST_TO_WARNING_OUT_SQRT = 1089f;

	[_E2A4("Дистанция ближе которой босс будет стрелять.")]
	public float BOSS_DIST_TO_SHOOT = 16f;

	[_E2A4("")]
	public float BOSS_DIST_TO_SHOOT_SQRT = 9f;

	[_E2A4("")]
	public float CHANCE_TO_SEND_GRENADE_100 = 100f;

	[_E2A4("Дистанция до босса, если до от потенциального укрытия больше чем Х то боты будут не будут занимать его.")]
	public float MAX_DIST_COVER_BOSS = 25f;

	[_E2A4("")]
	public float MAX_DIST_COVER_BOSS_SQRT;

	[_E2A4("Дистанция если враг ближе Х то будет отправленна проверка или запрошен бросок грены")]
	public float MAX_DIST_DECIDER_TO_SEND = 35f;

	[_E2A4("")]
	public float MAX_DIST_DECIDER_TO_SEND_SQRT;

	[_E2A4("Через столько после потери из видения босс решала будет отправлять челов на проверку")]
	public float TIME_AFTER_LOSE = 15f;

	[_E2A4("Но не больше чем Х")]
	public float TIME_AFTER_LOSE_DELTA = 60f;

	[_E2A4("Столько он попробудет отправить")]
	public int PERSONS_SEND = 2;

	[_E2A4("Через столько он попробует отправить еще раз")]
	public float DELTA_SEARCH_TIME = 18f;

	[_E2A4("Является ли нахождение всех в укрытиях необходимым условием что бы отправлять на проверку")]
	public bool COVER_TO_SEND = true;

	[_E2A4("Столько босс не будет стрелять во врага дикого пока с ним разбирается свита если этот дикий не стреляет в самого босса")]
	public float WAIT_NO_ATTACK_SAVAGE = 10f;

	[_E2A4("Шанс что при выборе пути для патрулирования босс выберет Маршрут Отдыха а не основной")]
	public float CHANCE_USE_RESERVE_PATROL_100 = 50f;

	[_E2A4("время, через которое предупреждение будет считаться завершённым")]
	public float WARN_PLAYER_PERIOD = 15f;

	[_E2A4("объём регенерации хп в бою за минуту (применяется раз в 3 секунды)")]
	public float EFFECT_REGENERATION_PER_MIN;

	[_E2A4("использовать ли обезболивающие на старте")]
	public bool EFFECT_PAINKILLER;

	[_E2A4("Дистанция по высоте в рамках которой игрок считается врагом киллы")]
	public float KILLA_Y_DELTA_TO_BE_ENEMY_BOSS = 5f;

	[_E2A4("Дистанция в рамках которой игрок считается врагом киллы")]
	public float KILLA_DITANCE_TO_BE_ENEMY_BOSS = 45f;

	[_E2A4("Через сколько секунд босс пойдет искать")]
	public float KILLA_START_SEARCH_SEC = 40f;

	[_E2A4("Сколько длиться контузия.")]
	public float KILLA_CONTUTION_TIME = 5f;

	[_E2A4("ближняя дистанция")]
	public float KILLA_CLOSE_ATTACK_DIST = 8f;

	[_E2A4("средняя дистанция")]
	public float KILLA_MIDDLE_ATTACK_DIST = 22f;

	[_E2A4("дальняя дистанция")]
	public float KILLA_LARGE_ATTACK_DIST = 41f;

	[_E2A4("для остановки и ожидания поиска дистанция")]
	public float KILLA_SEARCH_METERS = 30f;

	[_E2A4("Дистанция для поиска укрытий в режиме защиты")]
	public float KILLA_DEF_DIST_SQRT = 225f;

	[_E2A4("Если бот в течении этой дельты никого не нашел и побывал ближе чем KILLA_SEARCH_METERS то он пойдет посидит отдохнет.")]
	public float KILLA_SEARCH_SEC_STOP_AFTER_COMING = 25f;

	[_E2A4("Если место с которого надо сапресить дальше чем Х то он не будет сапресить.")]
	public float KILLA_DIST_TO_GO_TO_SUPPRESS = 6f;

	[_E2A4("Сколько ждать и не бежать после подавления гранатой")]
	public float KILLA_AFTER_GRENADE_SUPPRESS_DELAY = 2f;

	[_E2A4("После скольки штурмов долен настать перерыв")]
	public int KILLA_CLOSEATTACK_TIMES = 3;

	[_E2A4("Длительность перерыва")]
	public float KILLA_CLOSEATTACK_DELAY = 10f;

	[_E2A4("Килла базовое время ожидания в укрытии.")]
	public float KILLA_HOLD_DELAY = 5f;

	[_E2A4("Если перед набегом на врага пуль меньше чем Х то килла перезарядится")]
	public int KILLA_BULLET_TO_RELOAD = 15;

	[_E2A4("Если перед набегом на врага пуль меньше чем Х то килла перезарядится")]
	public float PERCENT_BULLET_TO_RELOAD = 0.6f;

	[_E2A4("Будет ли босс предупреждать (с учетом дистанции)")]
	public bool SHALL_WARN = true;

	[_E2A4("")]
	public int KILLA_ENEMIES_TO_ATTACK = 3;

	[_E2A4("Если враги оказываются на короткой дистанции (менее 30м) босс штурмует их.")]
	public float KILLA_ONE_IS_CLOSE = 30f;

	[_E2A4("НАсколько дольше килла зажимает от пролетающих мима пуль")]
	public float KILLA_TRIGGER_DOWN_DELAY = 1f;

	[_E2A4("НАсколько дольше килла выглядывает из укрытия")]
	public float KILLA_WAIT_IN_COVER_COEF = 1f;

	[_E2A4("Дистанция по высоте в рамках которой игрок считается врагом тагиллы")]
	public float TAGILLA_Y_DELTA_TO_BE_ENEMY_BOSS = 2f;

	[_E2A4("квадрат дистанции, с которой тагилле на помощь будут приходить дикие.")]
	public float TAGILLA_SAVAGE_HELP_SQR_DIST = 10000f;

	[_E2A4("ближняя дистанция с игнорированием безопасности")]
	public float TAGILLA_FORCED_CLOSE_ATTACK_DIST = 7f;

	[_E2A4("Если враги оказываются на короткой дистанции (менее 15м) босс штурмует их с бОльшим шансом при срабатывании триггера. Расстояние считается по навмешу!")]
	public float TAGILLA_FIRST_ASSAULT_RADIUS = 15f;

	[_E2A4("шанс штурма при успешном срабатывании триггера на меньшем радиусе.")]
	public float TAGILLA_FIRST_ASSAULT_CHANCE = 100f;

	[_E2A4("Если враги оказываются на короткой дистанции (менее 30м) босс штурмует их с меньшим шансом при срабатывании триггера. Расстояние считается по навмешу!")]
	public float TAGILLA_SECOND_ASSAULT_RADIUS = 30f;

	[_E2A4("шанс штурма при успешном срабатывании триггера на большем радиусе.")]
	public float TAGILLA_SECOND_ASSAULT_CHANCE = 30f;

	[_E2A4("квадрат дистанции, начиная с которой Тагилла будет считать врага замеченным, даже если не видел его. В идеале - TAGILLA_CLOSE_ATTACK_DIST")]
	public float TAGILLA_FEEL_ENEMY_DIST_SQR = 900f;

	[_E2A4("прекратить преследование, если не нанёс удар в течение такого количества секунд")]
	public float TAGILLA_TIME_TO_PURSUIT_WITHOUT_HITS = 6f;

	[_E2A4("не принимать решение преследовать чаще чем раз в X секунд")]
	public float TAGILLA_MELEE_ATTACK_NEXT_DECISION_PERIOD = 10.5f;

	[_E2A4("шанс штурма при перезарядке")]
	public float TAGILLA_MELEE_CHANCE_RELOAD = 100f;

	[_E2A4("шанс штурма при использовании предмета")]
	public float TAGILLA_MELEE_CHANCE_INTERACTION = 100f;

	[_E2A4("шанс штурма при использовании инвентаря")]
	public float TAGILLA_MELEE_CHANCE_INVENTORY = 100f;

	[_E2A4("шанс штурма при использовании аптечки")]
	public float TAGILLA_MELEE_CHANCE_MEDS = 100f;

	[_E2A4("шанс штурма при близком приближении")]
	public float TAGILLA_MELEE_CHANCE_FORCED = 100f;

	[_E2A4("минимальное время между окончанием предыдущего штурма и началом следующего")]
	public float TAGILLA_MIN_TIME_TO_REPEAT_MELEE_ASSAULT = 8f;

	[_E2A4("дистанция хотя бы 1 врага до лута меньше чем Х когда все заняли позиции то атаковать")]
	public float KOJANIY_DIST_WHEN_READY = 40f;

	[_E2A4("дляу учета количества врагов учитывается этот радиус.")]
	public float KOJANIY_DIST_TO_BE_ENEMY = 200f;

	[_E2A4("Дистанция до лута что бы можно было атаковать")]
	public float KOJANIY_MIN_DIST_TO_LOOT = 20f;

	[_E2A4("")]
	public float KOJANIY_MIN_DIST_TO_LOOT_SQRT = 100f;

	[_E2A4("Если враг подошел ближе чем Х к свите/боссу то атаковать.")]
	public float KOJANIY_DIST_ENEMY_TOO_CLOSE = 15f;

	[_E2A4("Коэфициент допуска к луту по дистанции для врагов больше 1")]
	public float KOJANIY_MANY_ENEMIES_COEF = 1.5f;

	[_E2A4("Стартовая позиция при драки - true- отсчитывается от бота. false- отсчитывается от Позции для окружения игрока")]
	public bool KOJANIY_FIGHT_CENTER_POS_ME;

	[_E2A4("Дистанция для пересчета. Если последняя точка пересчета больше чем Х относительно новой.")]
	public float KOJANIY_DIST_CORE_SPOS_RECALC = 25f;

	[_E2A4("")]
	public float KOJANIY_DIST_CORE_SPOS_RECALC_SQRT;

	[_E2A4("Через сколько кожаный или один из помошников после потери цели из виду дадут выстрелов")]
	public float KOJANIY_START_SUPPERS_SHOOTS_SEC = 30f;

	[_E2A4("Через сколько после первого ложного обстрела можно будет начать второй")]
	public float KOJANIY_START_NEXT_SUPPERS_SHOOTS_SEC = 90f;

	[_E2A4("если врагов строго больше чем Х то будет использованна защитная тактика")]
	public int KOJANIY_SAFE_ENEMIES = 1;

	[_E2A4("Через сколько секунд после пропадения из виду боссу/фолловеру становится пох на врага.")]
	public float KOJANIY_TAKE_CARE_ABOULT_ENEMY_DELTA = 2f;

	[_E2A4("Через сколько секунд после пропадения из виду боссу/фолловеру пойдут в ближайшее укрытие")]
	public float KOJANIY_WANNA_GO_TO_CLOSEST_COVER = 15f;

	[_E2A4("название желаемого типа пути")]
	public string GLUHAR_FOLLOWER_PATH_NAME = _ED3E._E000(7133);

	[_E2A4("")]
	public float GLUHAR_FOLLOWER_SCOUT_DIST_START_ATTACK = 80f;

	[_E2A4("")]
	public float GLUHAR_FOLLOWER_SCOUT_DIST_END_ATTACK = 120f;

	[_E2A4("")]
	public float GLUHAR_BOSS_WANNA_ATTACK_CHANCE_0_100 = 150f;

	[_E2A4("Дистанция ДО босса с которой можно атаковать ассаутерам.")]
	public float GLUHAR_ASSAULT_ATTACK_DIST = 80f;

	[_E2A4("Дистанция если првевысить её то асаутеры перестанут атаковать")]
	public float GLUHAR_STOP_ASSAULT_ATTACK_DIST = 260f;

	[_E2A4("")]
	public float GLUHAR_TIME_TO_ASSAULT = 10f;

	[_E2A4("Если бот должен охранять босса, но в текщем укрытии дистанция больше чем Х     то он постарается поменять укртие на поближе")]
	public float DIST_TO_PROTECT_BOSS = 15f;

	[_E2A4("Через какое время босс проверит и захочет вызвать подкрепу . Если <0 то не работает")]
	public float GLUHAR_SEC_TO_REINFORSMENTS = -1f;

	[_E2A4("Может ли глухать вызывать подкрепу по этим ивентам")]
	public bool GLUHAR_REINFORSMENTS_BY_EXIT;

	[_E2A4("Может ли глухать вызывать подкрепу по этим ивентам")]
	public bool GLUHAR_REINFORSMENTS_BY_EVENT;

	[_E2A4("Может ли глухать вызывать подкрепу по этим ивентам")]
	public bool GLUHAR_REINFORSMENTS_BY_PLAYER_COME_TO_ZONE;

	[_E2A4("Если фоловеров меньше чем Х то можно вызывать подкрепу   . Если <0 то не работает")]
	public int GLUHAR_FOLLOWERS_TO_REINFORSMENTS = -1;

	[_E2A4("Целевое кол-во по роли")]
	public int GLUHAR_FOLLOWERS_SECURITY = 3;

	[_E2A4("Целевое кол-во по роли")]
	public int GLUHAR_FOLLOWERS_ASSAULT = 2;

	[_E2A4("Целевое кол-во по роли")]
	public int GLUHAR_FOLLOWERS_SCOUT = 2;

	[_E2A4("Целевое кол-во по роли")]
	public int GLUHAR_FOLLOWERS_SNIPE;

	[_E2A4("Дистанция до босса когда он начинает хотеть идти убивать если штурмовики дереться")]
	public float GLUHAR_BOSS_DIST_TO_ENEMY_WANT_KILL = 40f;

	[_E2A4("Сколько секунд отходить в укрытия если тебя концули и враг не виден   ")]
	public float IF_I_HITTED_GO_AWAY_SEC_HIT = 2f;

	[_E2A4("На сколько бот должен отстать от боса что бы перейти на бег если он использует передвижение с остановками.")]
	public float DIST_TO_START_RUN_FOR_COVER_WITH_STOP = 14f;

	[_E2A4("На сколько оставшаяся дистанция пути бота должна отличаться от оставшийся дистанции босса что бы он мог перейти на бег если он использует передвижение с остановками.")]
	public float DELTA_DIST_DEST_BOSS_START_RUN_FOR_COVER_WITH_STOP = 8f;

	[_E2A4("Использовать только точки со стрельбой")]
	public bool SANITAR_ONLY_FIGHT_COVERS = true;

	[_E2A4("Тактика 2 укрытий")]
	public bool SANITAR_TWO_COVER_TACTIC = true;

	[_E2A4("Если фоловров меньше чем Х то предупреждать не будут")]
	public int COUNT_FOLLOWERS_TO_WARN = 2;

	[_E2A4("Может ли босс при тактике укрытий использовать кусты")]
	public bool RUN_HIDE_CAN_USE_TREE_COVRES;

	[_E2A4("Если расстояния меньше Х то будет игнорироваться видимость врага для атаки ножом")]
	public float SECTANT_INDOOR_DIST_NOT_TO_ATTACK = 6f;

	[_E2A4("При добавлении бота во враги из-за пересечения радиуса босса, даёт читерную видимость на кадр")]
	public bool SET_CHEAT_VISIBLE_WHEN_ADD_TO_ENEMY;

	[_E2A4("Сколько надо прстоять в круге что бы стать врагом")]
	public float TOTAL_TIME_KILL = 50f;

	[_E2A4("Через столько после предупрежедния станет врагом     ")]
	public float TOTAL_TIME_KILL_AFTER_WARN = 5f;

	[_E2A4("После скольки пересечений границы круга чел будет атакован")]
	public int COME_INSIDE_TIMES = 5;

	[_E2A4("Насколько метров надо отойти от начала предупреждания ботов что бы он прекратил предупреждать")]
	public float BOSS_DIST_TO_WARNING_OUT_DELTA = 15f;

	[_E2A4("ЧЕрез сколько сек после первого дейтсивя педупреждения будет становится враго")]
	public float TOTAL_TIME_KILL_AFTER_START_WARN = 15f;

	[_E2A4("Сколько врагов видно что бы попытатья поливать гранатометом")]
	public int BIG_PIPE_ARTILLERY_COUNT = 1;

	[_E2A4("Шанс что босс вместо перемещения лежа будет телепортироваться")]
	public float BOSS_ZRYACHIY_TELEPORT_CHANCE = 25f;

	[_E2A4("минимальная дистанция до фрага что бы можно было телепортироватся")]
	public float BOSS_ZRYACHIY_MIN_DIST_TO_TELEPORT = 150f;

	[_E2A4("сколько секунд проходит с рождения бота что бы можно было телепортироватся")]
	public float BOSS_ZRYACHIY_TELEPORT_CAN_SECONDS_FROM_START = 99999f;

	[_E2A4("минимальная дистанция для выбора следущего укрытия")]
	public float BOSS_ZRYACHIY_MIN_DIST_TO_NEXT_COVER = 3f;

	[_E2A4("минимальный уровень тумана что бы можно было перепортиться")]
	public float BOSS_ZRYACHIY_POSSIBLE_FOG = 0.025f;

	public BotGlobalsBossSettings()
	{
		Update();
	}

	public void Update()
	{
		KOJANIY_DIST_CORE_SPOS_RECALC_SQRT = KOJANIY_DIST_CORE_SPOS_RECALC * KOJANIY_DIST_CORE_SPOS_RECALC;
		BOSS_DIST_TO_WARNING_OUT_SQRT = BOSS_DIST_TO_WARNING_OUT * BOSS_DIST_TO_WARNING_OUT;
		BOSS_DIST_TO_SHOOT_SQRT = BOSS_DIST_TO_SHOOT * BOSS_DIST_TO_SHOOT;
		BOSS_DIST_TO_WARNING_SQRT = BOSS_DIST_TO_WARNING * BOSS_DIST_TO_WARNING;
		BOSS_DIST_TO_WARNING_SQRT_USEC = BOSS_DIST_TO_WARNING_USEC * BOSS_DIST_TO_WARNING_USEC;
		BOSS_DIST_TO_WARNING_SQRT_BEAR = BOSS_DIST_TO_WARNING_BEAR * BOSS_DIST_TO_WARNING_BEAR;
		MAX_DIST_DECIDER_TO_SEND_SQRT = MAX_DIST_DECIDER_TO_SEND * MAX_DIST_DECIDER_TO_SEND;
		MAX_DIST_COVER_BOSS_SQRT = MAX_DIST_COVER_BOSS * MAX_DIST_COVER_BOSS;
		KOJANIY_MIN_DIST_TO_LOOT_SQRT = KOJANIY_MIN_DIST_TO_LOOT * KOJANIY_MIN_DIST_TO_LOOT;
	}
}
