using System;

[Serializable]
public class BotGlobalLookData
{
	[_E2A4("Время жизни точки до которой бот будет обращать внимание на нее при стандартном алгоритме осмотра.")]
	public float OLD_TIME_POINT = 11f;

	[_E2A4("дельта через которую бот обновит точки в стандартном алгоритме куда ему смотреть")]
	public float WAIT_NEW_SENSOR = 2.1f;

	[_E2A4("Дельта при которой бот может повернутся в другую сторону вдоль стены в стандартном алгоритме")]
	public float WAIT_NEW__LOOK_SENSOR = 7.8f;

	[_E2A4("Время просмотра одного направления, когда бот осматривает место.")]
	public float LOOK_AROUND_DELTA = 1.1f;

	[_E2A4("Дистанция через на которую бот видит в листву")]
	public float MAX_VISION_GRASS_METERS = 1.1f;

	[_E2A4("Коэфициент вспышки от оружия. Влияет на то как быстро бот заметит стрелявшего")]
	public float MAX_VISION_GRASS_METERS_FLARE = 8f;

	[_E2A4("")]
	public float MAX_VISION_GRASS_METERS_OPT;

	[_E2A4("")]
	public float MAX_VISION_GRASS_METERS_FLARE_OPT;

	[_E2A4("TODO фонарики")]
	public float LightOnVisionDistance = 30f;

	[_E2A4("Дистанция выше которой объект считается дальеко")]
	public float FAR_DISTANCE = 160f;

	[_E2A4("Дельта апдейта следущего времни на дальнем расстоянии")]
	public float FarDeltaTimeSec = 3f;

	[_E2A4("Дистанция выше которой объект считается на среднем расстоянии")]
	public float MIDDLE_DIST = 90f;

	[_E2A4("Дельта апдейта следущего времни на среднем расстоянии")]
	public float MiddleDeltaTimeSec = 1f;

	[_E2A4("Дельта апдейта следущего времни на близком расстоянии")]
	public float CloseDeltaTimeSec = 0.1f;

	[_E2A4("Время при котором не сбрасывается набор видимости.")]
	public float POSIBLE_VISION_SPACE = 1.2f;

	[_E2A4("После пропадания из видимости бот еще столько секуннд будет \"Видеть\" врага")]
	public float GOAL_TO_FULL_DISSAPEAR = 6.5f;

	[_E2A4("После пропадания из видимости бот еще столько секуннд будет \"Видеть\" врага если мешает только зелень")]
	public float GOAL_TO_FULL_DISSAPEAR_GREEN = 6.5f;

	[_E2A4("После пропадания из видимости бот еще столько секуннд сможет стрелять во врага")]
	public float GOAL_TO_FULL_DISSAPEAR_SHOOT = 2.5f;

	[_E2A4("Частота поиска новых тел")]
	public float BODY_DELTA_TIME_SEARCH_SEC = 1.7f;

	[_E2A4("")]
	public float COME_TO_BODY_DIST = 1.2f;

	[_E2A4("")]
	public float MARKSMAN_VISIBLE_DIST_COEF = 1.15f;

	[_E2A4("Дальности видимости с фонариком")]
	public float VISIBLE_DISNACE_WITH_LIGHT = 33f;

	[_E2A4("Настолько дальше в метрах будет виден враг если у него включен фонарь.")]
	public float ENEMY_LIGHT_ADD = 35f;

	[_E2A4("Если дальность зрения бота меньше Х то начинает действовать параметр ENEMY_LIGHT_COEF")]
	public float ENEMY_LIGHT_START_DIST = 40f;

	[_E2A4("Если точка на которую смотрит бот ближе чем Х. Он может будет игрорить стены")]
	public float DIST_NOT_TO_IGNORE_WALL = 15f;

	[_E2A4("Если стена ближе чем Х то бот будет смотреть по ходу движения когда идет к подозрительной точке.")]
	public float DIST_CHECK_WALL = 20f;

	[_E2A4("Если небыло никаких других точек интереса, то бот будет смотреть на последнюю точку где они видел своего врага")]
	public float LOOK_LAST_POSENEMY_IF_NO_DANGER_SEC = 25f;

	[_E2A4("")]
	public float MIN_LOOK_AROUD_TIME = 20f;

	[_E2A4("")]
	public bool LOOK_THROUGH_GRASS;

	[_E2A4("Если расстояние между первой точкой попадания в лист и обратной точкой попадания в листу меньше чем Х то бот видит. Работает только при Х > 0")]
	public float LOOK_THROUGH_GRASS_DIST_METERS;

	[_E2A4("Меньше скольки секунд  последний раз был замечен бот что бы сработл коэфициент ускоренного замечания")]
	public float SEC_REPEATED_SEEN = 10f;

	[_E2A4("")]
	public double DIST_SQRT_REPEATED_SEEN;

	[_E2A4("Меньше скольки метров относительно своей старой точки долежн быть бот что бы сработл коэфициент ускоренного замечания")]
	public double DIST_REPEATED_SEEN = 15.0;

	[_E2A4("Коэфициент на сколько быстрее увидет бот при условии DIST_REPEATED_SEEN и SEC_REPEATED_SEEN Меньше - быстрее. 1==Также")]
	public float COEF_REPEATED_SEEN = 1E-05f;

	[_E2A4("Если враг дальеш чем Х метров то будет учитываться эта дистанця")]
	public float MAX_DIST_CLAMP_TO_SEEN_SPEED = 100f;

	[_E2A4(" Если дальность видимости бот меньше Х вкл ПНВ")]
	public float NIGHT_VISION_ON = 100f;

	[_E2A4("   Если дальность видимости бот больше Х выкл ПНВ")]
	public float NIGHT_VISION_OFF = 110f;

	[_E2A4(" При включенном ПНВ бот видит на Х метров")]
	public float NIGHT_VISION_DIST = 105f;

	[_E2A4("Угл видимости когда включен фонарик")]
	public float VISIBLE_ANG_LIGHT = 60f;

	[_E2A4("Угл видимости когда включен ПНВ")]
	public float VISIBLE_ANG_NIGHTVISION = 120f;

	[_E2A4("если расстояние между игроками меньше Х то трава и листва игноряться на уровне Слоёв. ")]
	public float NO_GREEN_DIST = 1.5f;

	[_E2A4("если расстояние между игроками меньше Х то трава игноряться на уровне Слоёв. ")]
	public float NO_GRASS_DIST = 15f;

	[_E2A4("Коэфициент когда бот смотрит из куста на дистанцию зелени. (чем меньше тем лучше бот видит сквозь зелень находясь внутри неё)")]
	public float INSIDE_BUSH_COEF = 1f;

	[_E2A4("какую кривую использовать для дальности зрения по времени суток")]
	public bool SELF_NIGHTVISION;

	[_E2A4("Сколько секунд после попадния во себя будет иметь возможность смотреть через зелень")]
	public float LOOK_THROUGH_PERIOD_BY_HIT = 10f;

	public bool CHECK_HEAD_ANY_DIST = true;

	public bool MIDDLE_DIST_CAN_SHOOT_HEAD;

	[_E2A4("Может ли использовать фонарик")]
	public bool CAN_USE_LIGHT = true;

	public BotGlobalLookData()
	{
		Update();
	}

	public void Update()
	{
		DIST_SQRT_REPEATED_SEEN = DIST_REPEATED_SEEN * DIST_REPEATED_SEEN;
		MAX_VISION_GRASS_METERS_OPT = 1f / MAX_VISION_GRASS_METERS;
		MAX_VISION_GRASS_METERS_FLARE_OPT = 1f / MAX_VISION_GRASS_METERS_FLARE;
	}
}
