using System;

[Serializable]
public class BotGlobalLayData
{
	[_E2A4("Когда ложится проверяет может ли из это позиции стрелять в последнюю известную позицию врага. (Если нет то может ложится за углом и т.п.)")]
	public bool CHECK_SHOOT_WHEN_LAYING;

	[_E2A4("Главный ограничитель на тайминги попытки проверки лечь")]
	public float DELTA_LAY_CHECK = 2f;

	[_E2A4("Бот может встать только через Х после того как лег")]
	public float DELTA_GETUP = 2.7f;

	public float DIST_LAY_CHECK = 50f;

	[_E2A4("После того как встал опять лечь можно только через Х")]
	public float DELTA_AFTER_GETUP = 10f;

	[_E2A4("если бот пролежит Х сек то все точки страха обнулятся")]
	public float CLEAR_POINTS_OF_SCARE_SEC = 20f;

	[_E2A4("Если бот лежит больше Х то он встанет")]
	public float MAX_LAY_TIME = 35f;

	[_E2A4("Дельта проверки во время атаки: стоит ли ложится")]
	public float DELTA_WANT_LAY_CHECL_SEC = 5f;

	[_E2A4("Шанс лечь если все уловия выполнены")]
	public float ATTACK_LAY_CHANCE = 25f;

	[_E2A4("если до укрытия больше Х. одно из необходимых условий что бы лечь")]
	public float DIST_TO_COVER_TO_LAY = 3.5f;

	[_E2A4("если между землей и травой меньше Х то можно лечь")]
	public float DIST_TO_COVER_TO_LAY_SQRT;

	[_E2A4("если между землей и травой меньше Х то можно лечь")]
	public float DIST_GRASS_TERRAIN_SQRT = 0.16000001f;

	[_E2A4("Если враг подошел ближе чем Х то обнуляем все точки страха")]
	public float DIST_ENEMY_NULL_DANGER_LAY = 15f;

	[_E2A4("")]
	public float DIST_ENEMY_NULL_DANGER_LAY_SQRT;

	[_E2A4("Если враг ближе чем Х то пора вставать")]
	public float DIST_ENEMY_GETUP_LAY = 10f;

	[_E2A4("")]
	public float DIST_ENEMY_GETUP_LAY_SQRT;

	[_E2A4("Если враг ближе чем Х то запрещает ложится если есть видимый врга ближе чем Х")]
	public float DIST_ENEMY_CAN_LAY = 15f;

	[_E2A4("")]
	public float DIST_ENEMY_CAN_LAY_SQRT;

	[_E2A4("На Х умножается коэфициент разлета при прицеливание когда бот лежит.")]
	public float LAY_AIM = 0.6f;

	[_E2A4("")]
	public float MIN_CAN_LAY_DIST_SQRT;

	[_E2A4("Если точка напугания была дальше чем Х то бот может попытаться лечь")]
	public float MIN_CAN_LAY_DIST = 11f;

	[_E2A4("")]
	public float MAX_CAN_LAY_DIST_SQRT;

	[_E2A4("Если точка напугания была ближе чем Х то бот может попытаться лечь")]
	public float MAX_CAN_LAY_DIST = 200f;

	[_E2A4("Шанс лечь вместо того что бы убежать (0..100)")]
	public float LAY_CHANCE_DANGER = 40f;

	[_E2A4("")]
	public int DAMAGE_TIME_TO_GETUP = 3;

	[_E2A4("Будет ли бот вставать когда ему мешает повернуться рельеф")]
	public bool SHALL_GETUP_ON_ROTATE = true;

	[_E2A4("Может ли бот ложится без проверки")]
	public bool SHALL_LAY_WITHOUT_CHECK;

	[_E2A4("Запрещено ли ложится без наличия врага")]
	public bool IF_NO_ENEMY = true;

	public BotGlobalLayData()
	{
		Update();
	}

	public void Update()
	{
		DIST_ENEMY_GETUP_LAY_SQRT = DIST_ENEMY_GETUP_LAY * DIST_ENEMY_GETUP_LAY;
		DIST_ENEMY_NULL_DANGER_LAY_SQRT = DIST_ENEMY_NULL_DANGER_LAY * DIST_ENEMY_NULL_DANGER_LAY;
		DIST_TO_COVER_TO_LAY_SQRT = DIST_TO_COVER_TO_LAY * DIST_TO_COVER_TO_LAY;
		MIN_CAN_LAY_DIST_SQRT = MIN_CAN_LAY_DIST * MIN_CAN_LAY_DIST;
		MAX_CAN_LAY_DIST_SQRT = MAX_CAN_LAY_DIST * MAX_CAN_LAY_DIST;
		DIST_ENEMY_CAN_LAY_SQRT = DIST_ENEMY_CAN_LAY * DIST_ENEMY_CAN_LAY;
	}
}
