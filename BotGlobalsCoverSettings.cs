using System;

[Serializable]
public class BotGlobalsCoverSettings
{
	[_E2A4("min Время после которогобог убежавший в засаду может вернутся в режим атаки")]
	public float RETURN_TO_ATTACK_AFTER_AMBUSH_MIN = 20f;

	[_E2A4("max")]
	public float RETURN_TO_ATTACK_AFTER_AMBUSH_MAX = 50f;

	[_E2A4("Дистанция на которой бот пугается выстрела и перестает считать свое укрытие надежным")]
	public float SOUND_TO_GET_SPOTTED = 10f;

	[_E2A4("Время которое после послейдней перестрелки бот будет сидеть в укрытии (При атакующем состоянии) по любому")]
	public float TIME_TO_MOVE_TO_COVER = 15f;

	[_E2A4("Максимальная дистанция при которйо бот считается в укрытии, после перепоиска точки укрытия")]
	public float MAX_DIST_OF_COVER = 4f;

	[_E2A4("Дельта для перерасчета поиска укрытия во время бега.")]
	public float CHANGE_RUN_TO_COVER_SEC = 5f;

	[_E2A4("Дельта для перерасчета поиска укрытия во время бега от гранаты.")]
	public float CHANGE_RUN_TO_COVER_SEC_GREANDE = 0.6f;

	[_E2A4("")]
	public float MIN_DIST_TO_ENEMY = 9f;

	[_E2A4("Если мы уже приблизились к нашей точке на Х м. То она не может быть сменена")]
	public float DIST_CANT_CHANGE_WAY = 5f;

	[_E2A4("Если игрок смотрит на меня и ближе чем Х то укрытия считается ненадежным и будет покинуто")]
	public float DIST_CHECK_SFETY = 20f;

	[_E2A4("Как часто крытие проверяется на сохранность")]
	public float TIME_CHECK_SAFE = 2f;

	[_E2A4("Время за которое бот выглядывает и прячется за укрытиям")]
	public float HIDE_TO_COVER_TIME = 1.5f;

	[_E2A4("")]
	public float MAX_DIST_OF_COVER_SQR;

	[_E2A4("")]
	public float DIST_CANT_CHANGE_WAY_SQR;

	[_E2A4("Если бот был обнаружет то все точки укрытий в этом радиусе будут помеченны как обнаруженные")]
	public float SPOTTED_COVERS_RADIUS = 5f;

	[_E2A4("Если последний увиденный враг был меньше стольки секунд назад, то бот не в укрытии позиции будет смотреть на последнюю узвестную точку")]
	public float LOOK_LAST_ENEMY_POS_MOVING = 3f;

	[_E2A4("Если последний увиденный враг был меньше стольки секунд назад, и LOOK_LAST_ENEMY_POS_DIST то бот смотр на него ")]
	public float LOOK_LAST_ENEMY_POS_LONG = 10f;

	[_E2A4("Если последний увиденный враг был меньше стольки секунд назад, то бот не в укрытии позиции будет смотреть на последнюю узвестную точку")]
	public float LOOK_LAST_ENEMY_POS_DIST = 17f;

	[_E2A4("Если последний увиденный враг был меньше больше X секунд назад, и по боту попали недавно то бот взглянет в сторону урона")]
	public float LOOK_TO_HIT_POINT_IF_LAST_ENEMY = 2f;

	[_E2A4("Время если последняя увиденная точка была меньше стольки сек. Юот при осмотре места подозрительного места.")]
	public float LOOK_LAST_ENEMY_POS_LOOKAROUND = 45f;

	[_E2A4("Максимальный угл отклонения когда бот сидит в укрытии и смотри вдоль стены")]
	public int OFFSET_LOOK_ALONG_WALL_ANG = 20;

	[_E2A4("При замечании гранаты все укрытия в этом радиусе будут помеченны как ненадежные.")]
	public float SPOTTED_GRENADE_RADIUS = 23f;

	[_E2A4("Столько сек укрытия будет оставаться ненадежным.")]
	public float MAX_SPOTTED_TIME_SEC = 45f;

	[_E2A4("Столько бот сидит в укрытиях передвигаясь перебежками ко врагу который потерялся из виду")]
	public float WAIT_INT_COVER_FINDING_ENEMY = 2f;

	[_E2A4("")]
	public float CLOSE_DIST_POINT_SQRT = 4f;

	[_E2A4("Через сколько секунд для выглядывания из за угрытия позция точно будет считаться непростреливаемой.")]
	public float DELTA_SEEN_FROM_COVE_LAST_POS = 15f;

	[_E2A4("При")]
	public bool MOVE_TO_COVER_WHEN_TARGET;

	[_E2A4("")]
	public bool RUN_COVER_IF_CAN_AND_NO_ENEMIES;

	[_E2A4("Столько укрытие будет ненадежным после броска гранаты рядом.")]
	public float SPOTTED_GRENADE_TIME = 7f;

	[_E2A4("При поске точек на максимальную близость к бот будет учитываться дельта по У")]
	public bool DEPENDS_Y_DIST_TO_BOT;

	[_E2A4("Бот будет убегать в укрытие если оно ближе чем Х")]
	public float RUN_IF_FAR = 15f;

	[_E2A4("")]
	public float RUN_IF_FAR_SQRT;

	[_E2A4("Бот будет идти отстреливаясь в укрытие если оно ближе чем Х но больше чем RUN_IF_FAR")]
	public float STAY_IF_FAR = 25f;

	[_E2A4("")]
	public float STAY_IF_FAR_SQRT;

	[_E2A4("проверять безопастность укрытия по тому смотрит ли на него враг.")]
	public bool CHECK_COVER_ENEMY_LOOK;

	[_E2A4("Если рядом с ботом который сидит в укрытии сделанно больше чем Х выстрелов то он посчитает это укрытие невалидным")]
	public int SHOOT_NEAR_TO_LEAVE = 3;

	[_E2A4("Если рядом с ботом который сидит в укрытии был сделан выстрел то он будет считать не чаще чем раз в Х сек")]
	public float SHOOT_NEAR_SEC_PERIOD = 1f;

	[_E2A4("Если вражины в бот попали больше чем Х то он посчитает укрытие невалидным")]
	public int HITS_TO_LEAVE_COVER = 2;

	[_E2A4("Если не вражины в бота попали больше чем Х то он посчитает укрытие невалидным")]
	public int HITS_TO_LEAVE_COVER_UNKNOWN = 2;

	[_E2A4("Столько времени посл того как бота выкинут из укрытия он будет в догфайте")]
	public float DOG_FIGHT_AFTER_LEAVE = 4f;

	[_E2A4("Игноировать стены при сидении в укртии при реакции на выстрелы.")]
	public bool NOT_LOOK_AT_WALL_IS_DANGER = true;

	[_E2A4("Если указано число больше чем 0 то бот будет стараться выбирать укрытия с уровнем защиты более Х")]
	public float MIN_DEFENCE_LEVEL;

	[_E2A4("Если в результате поика в ближайшем радиусе укрытий откуда можно стрелять нет то след итерация будет без обязательной стрельбы")]
	public bool REWORK_NOT_TO_SHOOT = true;

	[_E2A4("Удалять ли точки ЗА точками угрозы")]
	public bool DELETE_POINTS_BEHIND_ENEMIES = true;

	[_E2A4("Если блиэе к точке чем GOOD_DIST_TO_POINT*Х - то точка хорошая")]
	public float GOOD_DIST_TO_POINT_COEF = 1.8f;

	[_E2A4("Если враг подошел ближе чем Х и враг был виден то бот покинет укрытие")]
	public float ENEMY_DIST_TO_GO_OUT = 1f;

	[_E2A4("Проверка при поиске укрытий на наличие рядом ближайшего друга")]
	public bool CHECK_CLOSEST_FRIEND;

	[_E2A4("")]
	public float MIN_TO_ENEMY_TO_BE_NOT_SAFE_SQRT;

	[_E2A4("Если враг ближе чем Х к этой точке то бот будет считать что там нельзя спрятаться")]
	public float MIN_TO_ENEMY_TO_BE_NOT_SAFE = 8f;

	[_E2A4("Если нельзя выглядывать, то надо ли сидеть")]
	public bool SIT_DOWN_WHEN_HOLDING;

	[_E2A4("Если врага не видно больше чем Х сек , то бот покинет стационарку")]
	public float STATIONARY_WEAPON_NO_ENEMY_GETUP = 6f;

	[_E2A4("Если стационарка дальше чем Х то бот точно не будет сипользовать эту стационарку")]
	public float STATIONARY_WEAPON_MAX_DIST_TO_USE = 50f;

	[_E2A4("Сколько раз бот должен почуствовать пулю что бы покинуть стационарку")]
	public int STATIONARY_SPOTTED_TIMES_TO_LEAVE = 3;

	[_E2A4("Глобальный переключатель возможности использования стационарки")]
	public bool STATIONARY_CAN_USE = true;

	[_E2A4("Может ли бот находять в хорошем укрытии решить перейти на стационарку")]
	public bool CAN_END_SHOOT_FROM_COVER_CAUSE_STATIONARY = true;

	[_E2A4("Дельта проверки желания пойти к стационарки")]
	public float CAN_END_SHOOT_FROM_COVER_CAUSE_STATIONARY_DELTA = 5f;

	[_E2A4("Радиус проверки желания пойти к стационарки")]
	public float CAN_END_SHOOT_FROM_COVER_CAUSE_STATIONARY_RADIUS = 30f;

	[_E2A4("Если врга виден (не мы подвластен обстрелу) и ближе чем Х метров то кончаем холдить. ")]
	public float END_HOLD_IF_ENEMY_CLOSE_AND_VISIBLE = 15f;

	[_E2A4("Если враг дальше чем X то  прекращать поиск укрытий откуда можно стрелять")]
	public float DIST_MAX_REWORK_NOT_TO_SHOOT = 50f;

	[_E2A4("Если враг дальше чем X то  прекращать поиск укрытий откуда можно стрелять")]
	public float SDIST_MAX_REWORK_NOT_TO_SHOOT = 7225f;

	[_E2A4("Использовать опасные зоны и замечать их")]
	public bool USE_DANGER_AREAS = true;

	[_E2A4("")]
	public int MAX_ITERATIONS = 50;

	[_E2A4("Через сколько секунд бот покинет укрытия если цель видна а стрелять не может.")]
	public float CHANGE_COVER_IF_CANT_SHOOT_SEC = 8f;

	[_E2A4("")]
	public bool SHALL_CHANGE_COVER_IF_CAN_SHOOT;

	[_E2A4("дистанция до ближайшего друга что бы укрытие считалось не оч хорошим.")]
	public float CHECK_CLOSEST_FRIEND_DIST = 12f;

	[_E2A4("Может ли ложиться в укрытия если оно лежачего типа")]
	public bool CAN_LAY_TO_COVER;

	public BotGlobalsCoverSettings()
	{
		Update();
	}

	public void Update()
	{
		SDIST_MAX_REWORK_NOT_TO_SHOOT = DIST_MAX_REWORK_NOT_TO_SHOOT * DIST_MAX_REWORK_NOT_TO_SHOOT;
		MIN_TO_ENEMY_TO_BE_NOT_SAFE_SQRT = MIN_TO_ENEMY_TO_BE_NOT_SAFE * MIN_TO_ENEMY_TO_BE_NOT_SAFE;
		STAY_IF_FAR_SQRT = STAY_IF_FAR * STAY_IF_FAR;
		RUN_IF_FAR_SQRT = RUN_IF_FAR * RUN_IF_FAR;
		DIST_CANT_CHANGE_WAY_SQR = DIST_CANT_CHANGE_WAY * DIST_CANT_CHANGE_WAY;
		MAX_DIST_OF_COVER_SQR = MAX_DIST_OF_COVER * MAX_DIST_OF_COVER;
	}
}
