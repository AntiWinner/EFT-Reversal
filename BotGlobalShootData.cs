using System;

[Serializable]
public class BotGlobalShootData
{
	[_E2A4("Время за которое отдача уйдет")]
	public float RECOIL_TIME_NORMALIZE = 4f;

	[_E2A4("НАсколько высоко подскочит отдача вверх в метрах в зависимостии от расстояния")]
	public float RECOIL_PER_METER = 0.3f;

	[_E2A4("НАсколько высоко подскочит отдача вверх в метрах в зависимостии от расстояния")]
	public float MAX_RECOIL_PER_METER = 0.2f;

	[_E2A4("НАсколько высоко подскочит отдача вбок в зависимостии от вертикальной отдачи")]
	public float HORIZONT_RECOIL_COEF = 0.4f;

	[_E2A4("Перерыв между выстрелами.")]
	public float WAIT_NEXT_SINGLE_SHOT = 0.3f;

	[_E2A4("Перерыв между выстрелами стационарка пулевая.")]
	public float WAIT_NEXT_STATIONARY_BULLET = 0.3f;

	[_E2A4("Перерыв между выстрелами стационарко гранатомет.")]
	public float WAIT_NEXT_STATIONARY_GRENADE = 0.3f;

	[_E2A4("Максимальный перерыв между выстремами для снайперов базовый")]
	public float WAIT_NEXT_SINGLE_SHOT_LONG_MAX = 3.3f;

	[_E2A4("Минимальный")]
	public float WAIT_NEXT_SINGLE_SHOT_LONG_MIN = 0.8f;

	[_E2A4("Коэфициент зависимости частоты выстрелов снайперов (каждые Х метров - примерно сек)")]
	public float MARKSMAN_DIST_SEK_COEF = 44f;

	[_E2A4("Сколько будет зажат палец курсе при одиночном огне")]
	public float FINGER_HOLD_SINGLE_SHOT = 0.14f;

	[_E2A4("Сколько будет зажат палец  курсе при огне стационарке пулемете")]
	public float FINGER_HOLD_STATIONARY_BULLET = 0.14f;

	[_E2A4("Сколько будет зажат палец  курсе при огне стационарке гранатомете")]
	public float FINGER_HOLD_STATIONARY_GRENADE = 0.14f;

	[_E2A4("Сколько будем зажат палец на курсе при автоматическом огне")]
	public float BASE_AUTOMATIC_TIME = 0.1f;

	[_E2A4("Коэфициент разлета при автоматической стрельбе")]
	public float AUTOMATIC_FIRE_SCATTERING_COEF = 2.5f;

	[_E2A4("Шанс переключится в автоматический огонь еа старте игры")]
	public float CHANCE_TO_CHANGE_TO_AUTOMATIC_FIRE_100 = 76f;

	[_E2A4("Минимальная дельта по стрельбе оружия для укрытий")]
	public float FAR_DIST_ENEMY = 20f;

	[_E2A4("Если выстрелов из укрытия было сделно больше чем Х то возвращается в укрытие")]
	public int SHOOT_FROM_COVER = 4;

	[_E2A4("")]
	public float FAR_DIST_ENEMY_SQR;

	[_E2A4("Коэфициент на каоторы домнажается эффективная дистанция стрельбы что бы получить максимальную дистанцию стельбы.")]
	public float MAX_DIST_COEF = 1.35f;

	[_E2A4("типа скорострельность 600 в мин => 10 в сек => 1 в 0.1 сек.")]
	public float RECOIL_DELTA_PRESS = 0.15f;

	[_E2A4("Ессли враг дальше чем Х и у бота закончились патроны то он побежит в укрытие и перезарядится там.")]
	public float RUN_DIST_NO_AMMO = 25f;

	[_E2A4("")]
	public float RUN_DIST_NO_AMMO_SQRT;

	[_E2A4("Сколько раз надо несмочь выстрелить что бы не пытаться дальше стрелять, а пойти попрятаться.")]
	public int CAN_SHOOTS_TIME_TO_AMBUSH = 3;

	[_E2A4("Если враг был виден более NOT_TO_SEE_ENEMY_TO_WANT_RELOAD_SEC сек назад и в магазине меньше чем Х патронов то перезарядится")]
	public float NOT_TO_SEE_ENEMY_TO_WANT_RELOAD_PERCENT = 0.4f;

	[_E2A4("")]
	public float NOT_TO_SEE_ENEMY_TO_WANT_RELOAD_SEC = 2f;

	[_E2A4("Если врагов давно небыло и в магазине меньше чем Х процентов патронов то перезарядится")]
	public float RELOAD_PECNET_NO_ENEMY = 0.6f;

	[_E2A4("Шанс поменять оружие если кончились патроны.")]
	public float CHANCE_TO_CHANGE_WEAPON = 100f;

	[_E2A4("Шанс поменять оружие если кончились патроны иу врага есть шлем.")]
	public float CHANCE_TO_CHANGE_WEAPON_WITH_HELMET = 100f;

	[_E2A4("бот будет менять оружие только если враг дальше чем Х.")]
	public float LOW_DIST_TO_CHANGE_WEAPON = 10f;

	[_E2A4("бот будет менять оружие только если враг ближе чем Х.")]
	public float FAR_DIST_TO_CHANGE_WEAPON = 50f;

	[_E2A4("Настолько противник будет считаться засапрешеным если его сапрсить пулями")]
	public float SUPPRESS_BY_SHOOT_TIME = 6f;

	[_E2A4("Сколько раз надо нажать на спуск что бы противник стал засапрешеным")]
	public int SUPPRESS_TRIGGERS_DOWN = 3;

	[_E2A4("Сколько раз надо нажать на спуск что бы противник стал засапрешеным при условии списка точек")]
	public int SUPPRESS_TRIGGERS_DOWN_AS_LIST = 6;

	[_E2A4("")]
	public float DIST_TO_CHANGE_TO_MAIN = 15f;

	[_E2A4("Дистанция от врага при которой бот покинет АГС_17. ")]
	public float AGS_17_DIST_TO_LEAVE = 25f;

	[_E2A4("Дистанция с которой можно бить/начать комбо")]
	public float DIST_TO_HIT_MELEE = 2f;

	[_E2A4("Дистанция с которой можно продолжить комбо")]
	public float DIST_TO_HIT_MELEE_CONTINUE_COMBO = 1.8f;

	[_E2A4("Дистанция с которой нужно остановить спринт")]
	public float DIST_TO_STOP_SPRINT_MELEE = 2.4f;

	[_E2A4("Переодичность удара")]
	public float TRY_HIT_PERIOD_MELEE = 0.5f;

	[_E2A4("насколько блокировать стрельбу когда ложишься")]
	public float BLOCK_PERIOD_WHEN_LAY = 1.25f;

	[_E2A4("как часто может менять оружие в руках")]
	public float CHANGE_WEAPON_PERIOD = 5f;

	[_E2A4("флаг для комбо атак в OneMeleeAttackNode")]
	public bool USE_MELEE_COMBOS;

	[_E2A4("100% - оружие ломается как у обычных игроков, 50% - в 2 раза реже, 0 - никогда")]
	public int VALIDATE_MALFUNCTION_CHANCE = 100;

	[_E2A4("шанс чинить оружие сразу в момент поломки, не убегая в укрытие. Если не срабатывает - бот сначала прячется, только потом чинит")]
	public int REPAIR_MALFUNCTION_IMMEDIATE_CHANCE = 25;

	[_E2A4("время в секундах между перехода в ноду малфанкшена и осмотром оружия")]
	public float DELAY_BEFORE_EXAMINE_MALFUNCTION = 1f;

	[_E2A4("время в секундах между осмотром оружия и непосредственно починкой")]
	public float DELAY_BEFORE_FIX_MALFUNCTION = 1.5f;

	[_E2A4("Бот попытается сменить оружие вместо перезарядки в бою")]
	public bool TRY_CHANGE_WEAPON_INSTEAD_RELOAD;

	[_E2A4("Минимальная дистанция до противника когда бот попытается сменить оружие вместо перезарядки в бою")]
	public float MIN_DIST_TO_ENEMY_TO_CHANGE_WEAPON_INSTEAD_RELOAD = 30f;

	[_E2A4("Шанс сменить оружие вместо перезарядки в бою")]
	public float CHANCE_TO_CHANGE_WEAPON_INSTEAD_RELOAD = 60f;

	[_E2A4("Шанс сменить оружие вместо перезарядки в бою когда у противника нет шлема")]
	public float CHANCE_TO_CHANGE_WEAPON_INSTEAD_RELOAD_ENEMY_WITHOUT_HELM = 90f;

	[_E2A4("дистанция для остановки перед рукопашной атакой (положительная == промежать за спину игрока)")]
	public float MELEE_STOP_DIST = 0.3f;

	[_E2A4("меняет ли бот оружие на основное во время патруля")]
	public bool CHANGE_TO_MAIN_WEAPON_WHEN_PATROL;

	[_E2A4("")]
	public float SHOOT_IMMEDIATELY_DIST = 25f;

	public bool CAN_STOP_SHOOT_CAUSE_ANIMATOR;

	public bool TRY_CHANGE_WEAPON_WHEN_RELOAD = true;

	public bool CHANGE_TO_MAIN_WHEN_SUPPORT_NO_AMMO = true;

	public void Update()
	{
		FAR_DIST_ENEMY_SQR = FAR_DIST_ENEMY * FAR_DIST_ENEMY;
		RUN_DIST_NO_AMMO_SQRT = RUN_DIST_NO_AMMO * RUN_DIST_NO_AMMO;
	}
}
