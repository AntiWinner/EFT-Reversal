using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.Animations;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace EFT;

public sealed class BotOwner : MonoBehaviour, _E5B4
{
	public const float DIST_CHECK_NAVMESH = 0.2f;

	public static readonly Vector3 STAY_HEIGHT = new Vector3(0f, 1.15f, 0f);

	public const string PATH_TO_AI = "AI";

	public const string PATH_TO_AI_DEBUG = "AIDebug";

	public static int BotCount;

	private Action<BotOwner> m__E000;

	private EBotState m__E001;

	private bool m__E002;

	private float m__E003 = 2f;

	private float m__E004;

	private float m__E005;

	private float m__E006;

	public CustomPath CurrPath;

	public _E10F DecisionProxy;

	public _E102 DebugMemory;

	public Transform LookedTransform;

	public _E5B2 Memory;

	public _E2A1 Settings;

	private bool m__E007;

	[CompilerGenerated]
	private _E179 m__E008;

	[CompilerGenerated]
	private float m__E009;

	[CompilerGenerated]
	private _E296 m__E00A;

	[CompilerGenerated]
	private _E170 m__E00B;

	[CompilerGenerated]
	private _E14D _E00C;

	[CompilerGenerated]
	private _E15E _E00D;

	[CompilerGenerated]
	private _E1F6 _E00E;

	[CompilerGenerated]
	private _E20D _E00F;

	[CompilerGenerated]
	private _E216 _E010;

	[CompilerGenerated]
	private _E219 _E011;

	[CompilerGenerated]
	private _E14F _E012;

	[CompilerGenerated]
	private _E1FE _E013;

	[CompilerGenerated]
	private _E126 _E014;

	[CompilerGenerated]
	private _E14E _E015;

	[CompilerGenerated]
	private _E128 _E016;

	[CompilerGenerated]
	private _E1FD _E017;

	[CompilerGenerated]
	private _E151 _E018;

	[CompilerGenerated]
	private _E134 _E019;

	[CompilerGenerated]
	private _E158 _E01A;

	[CompilerGenerated]
	private _E276 _E01B;

	[CompilerGenerated]
	private _E215 _E01C;

	[CompilerGenerated]
	private _E142 _E01D;

	[CompilerGenerated]
	private _E1FB _E01E;

	[CompilerGenerated]
	private _E204 _E01F;

	[CompilerGenerated]
	private _E138 _E020;

	[CompilerGenerated]
	private _E245 _E021;

	[CompilerGenerated]
	private _E246 _E022;

	[CompilerGenerated]
	private _E218 _E023;

	[CompilerGenerated]
	private _E13E _E024;

	[CompilerGenerated]
	private _E13F _E025;

	[CompilerGenerated]
	private _E21B _E026;

	[CompilerGenerated]
	private _E242 _E027;

	[CompilerGenerated]
	private _E214 _E028;

	[CompilerGenerated]
	private _E137 _E029;

	[CompilerGenerated]
	private _E145 _E02A;

	[CompilerGenerated]
	private _E20E _E02B;

	[CompilerGenerated]
	private _E26D _E02C;

	[CompilerGenerated]
	private _E143 _E02D;

	[CompilerGenerated]
	private _E1FC _E02E;

	[CompilerGenerated]
	private _E205 _E02F;

	[CompilerGenerated]
	private _E247 _E030;

	[CompilerGenerated]
	private _E13D _E031;

	[CompilerGenerated]
	private _E152 _E032;

	[CompilerGenerated]
	private _E15A _E033;

	[CompilerGenerated]
	private _E24B _E034;

	[CompilerGenerated]
	private _E24C _E035;

	[CompilerGenerated]
	private _E1EF _E036;

	[CompilerGenerated]
	private _E148 _E037;

	[CompilerGenerated]
	private _E13B _E038;

	[CompilerGenerated]
	private _E24A _E039;

	[CompilerGenerated]
	private _E290 _E03A;

	[CompilerGenerated]
	private _E1EE _E03B;

	[CompilerGenerated]
	private _E28E _E03C;

	[CompilerGenerated]
	private _E259 _E03D;

	[CompilerGenerated]
	private _E15B _E03E;

	[CompilerGenerated]
	private _E14C _E03F;

	[CompilerGenerated]
	private _E241 _E040;

	[CompilerGenerated]
	private _E248 _E041;

	[CompilerGenerated]
	private _E206 _E042;

	[CompilerGenerated]
	private _E28F _E043;

	[CompilerGenerated]
	private _E16D _E044;

	[CompilerGenerated]
	private _E16E _E045;

	[CompilerGenerated]
	private _E133 _E046;

	[CompilerGenerated]
	private _E243 _E047;

	[CompilerGenerated]
	private _E150 _E048;

	[CompilerGenerated]
	private _E139 _E049;

	[CompilerGenerated]
	private _E169 _E04A;

	[CompilerGenerated]
	private _E252 _E04B;

	[CompilerGenerated]
	private _E141 _E04C;

	[CompilerGenerated]
	private _E244 _E04D;

	[CompilerGenerated]
	private _E13A _E04E;

	[CompilerGenerated]
	private _E135 _E04F;

	[CompilerGenerated]
	private _E156 _E050;

	[CompilerGenerated]
	private _E257 _E051;

	[CompilerGenerated]
	private _E255 _E052;

	[CompilerGenerated]
	private _E256 _E053;

	[CompilerGenerated]
	private _E1ED _E054;

	[CompilerGenerated]
	private _E2AA _E055;

	[CompilerGenerated]
	private _E159 _E056;

	[CompilerGenerated]
	private _E295 _E057;

	[CompilerGenerated]
	private _E157 _E058;

	[CompilerGenerated]
	private _E620 _E059;

	[CompilerGenerated]
	private BifacialTransform _E05A;

	[CompilerGenerated]
	private _E172 _E05B;

	[CompilerGenerated]
	private _E144 _E05C;

	[CompilerGenerated]
	private _E629 _E05D;

	[CompilerGenerated]
	private Vector2 _E05E;

	[CompilerGenerated]
	private _E136 _E05F;

	[CompilerGenerated]
	private _E20F _E060;

	[CompilerGenerated]
	private _E15D _E061;

	[CompilerGenerated]
	private _E301 _E062;

	[CompilerGenerated]
	private Action<EBotState> _E063;

	[CompilerGenerated]
	private _E267 _E064;

	[CompilerGenerated]
	private _E23F _E065;

	[CompilerGenerated]
	private string _E066;

	[CompilerGenerated]
	private int _E067;

	[CompilerGenerated]
	private Player _E068;

	[CompilerGenerated]
	private Dictionary<BodyPartType, _E1FF> _E069;

	[CompilerGenerated]
	private bool _E06A;

	public _E179 Brain
	{
		[CompilerGenerated]
		get
		{
			return this.m__E008;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E008 = value;
		}
	}

	public float ENEMY_LOOK_AT_ME
	{
		[CompilerGenerated]
		get
		{
			return this.m__E009;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E009 = value;
		}
	}

	public float ActivateTime => this.m__E005;

	public _E296 LookSensor
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00A;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E00A = value;
		}
	}

	public _E170 DeadBodyData
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00B;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E00B = value;
		}
	}

	public _E14D BotLay
	{
		[CompilerGenerated]
		get
		{
			return _E00C;
		}
		[CompilerGenerated]
		private set
		{
			_E00C = value;
		}
	}

	public _E15E Tilt
	{
		[CompilerGenerated]
		get
		{
			return _E00D;
		}
		[CompilerGenerated]
		private set
		{
			_E00D = value;
		}
	}

	public _E1F6 GoalCulculator
	{
		[CompilerGenerated]
		get
		{
			return _E00E;
		}
		[CompilerGenerated]
		private set
		{
			_E00E = value;
		}
	}

	public _E20D GoToSomePointData
	{
		[CompilerGenerated]
		get
		{
			return _E00F;
		}
		[CompilerGenerated]
		private set
		{
			_E00F = value;
		}
	}

	public _E216 TrianglePosition
	{
		[CompilerGenerated]
		get
		{
			return _E010;
		}
		[CompilerGenerated]
		private set
		{
			_E010 = value;
		}
	}

	public _E219 LookData
	{
		[CompilerGenerated]
		get
		{
			return _E011;
		}
		[CompilerGenerated]
		private set
		{
			_E011 = value;
		}
	}

	public _E14F BotLight
	{
		[CompilerGenerated]
		get
		{
			return _E012;
		}
		[CompilerGenerated]
		private set
		{
			_E012 = value;
		}
	}

	public _E1FE BotTurnAwayLight
	{
		[CompilerGenerated]
		get
		{
			return _E013;
		}
		[CompilerGenerated]
		private set
		{
			_E013 = value;
		}
	}

	public _E126 Boss
	{
		[CompilerGenerated]
		get
		{
			return _E014;
		}
		[CompilerGenerated]
		private set
		{
			_E014 = value;
		}
	}

	public _E14E LeaveData
	{
		[CompilerGenerated]
		get
		{
			return _E015;
		}
		[CompilerGenerated]
		private set
		{
			_E015 = value;
		}
	}

	public _E128 BotFollower
	{
		[CompilerGenerated]
		get
		{
			return _E016;
		}
		[CompilerGenerated]
		private set
		{
			_E016 = value;
		}
	}

	public _E1FD NightVision
	{
		[CompilerGenerated]
		get
		{
			return _E017;
		}
		[CompilerGenerated]
		private set
		{
			_E017 = value;
		}
	}

	public _E151 LoyaltyData
	{
		[CompilerGenerated]
		get
		{
			return _E018;
		}
		[CompilerGenerated]
		private set
		{
			_E018 = value;
		}
	}

	public _E134 AssaultBuildingData
	{
		[CompilerGenerated]
		get
		{
			return _E019;
		}
		[CompilerGenerated]
		private set
		{
			_E019 = value;
		}
	}

	public _E158 BotRun
	{
		[CompilerGenerated]
		get
		{
			return _E01A;
		}
		[CompilerGenerated]
		private set
		{
			_E01A = value;
		}
	}

	public _E276 SuspiciousPlaceData
	{
		[CompilerGenerated]
		get
		{
			return _E01B;
		}
		[CompilerGenerated]
		private set
		{
			_E01B = value;
		}
	}

	public _E215 Steering
	{
		[CompilerGenerated]
		get
		{
			return _E01C;
		}
		[CompilerGenerated]
		private set
		{
			_E01C = value;
		}
	}

	public _E142 EnemyLookData
	{
		[CompilerGenerated]
		get
		{
			return _E01D;
		}
		[CompilerGenerated]
		private set
		{
			_E01D = value;
		}
	}

	public _E1FB FindPlaceToShoot
	{
		[CompilerGenerated]
		get
		{
			return _E01E;
		}
		[CompilerGenerated]
		private set
		{
			_E01E = value;
		}
	}

	public _E204 HealAnotherTarget
	{
		[CompilerGenerated]
		get
		{
			return _E01F;
		}
		[CompilerGenerated]
		private set
		{
			_E01F = value;
		}
	}

	public _E138 CellData
	{
		[CompilerGenerated]
		get
		{
			return _E020;
		}
		[CompilerGenerated]
		private set
		{
			_E020 = value;
		}
	}

	public _E245 PeaceHardAim
	{
		[CompilerGenerated]
		get
		{
			return _E021;
		}
		[CompilerGenerated]
		private set
		{
			_E021 = value;
		}
	}

	public _E246 PeaceLook
	{
		[CompilerGenerated]
		get
		{
			return _E022;
		}
		[CompilerGenerated]
		private set
		{
			_E022 = value;
		}
	}

	public _E218 UnityEditorRunChecker
	{
		[CompilerGenerated]
		get
		{
			return _E023;
		}
		[CompilerGenerated]
		private set
		{
			_E023 = value;
		}
	}

	public _E13E DelayActions
	{
		[CompilerGenerated]
		get
		{
			return _E024;
		}
		[CompilerGenerated]
		private set
		{
			_E024 = value;
		}
	}

	public _E13F DogFight
	{
		[CompilerGenerated]
		get
		{
			return _E025;
		}
		[CompilerGenerated]
		private set
		{
			_E025 = value;
		}
	}

	public _E21B PriorityAxeTarget
	{
		[CompilerGenerated]
		get
		{
			return _E026;
		}
		[CompilerGenerated]
		private set
		{
			_E026 = value;
		}
	}

	public _E242 FriendChecker
	{
		[CompilerGenerated]
		get
		{
			return _E027;
		}
		[CompilerGenerated]
		private set
		{
			_E027 = value;
		}
	}

	public _E214 NavMeshCutterController
	{
		[CompilerGenerated]
		get
		{
			return _E028;
		}
		[CompilerGenerated]
		private set
		{
			_E028 = value;
		}
	}

	public _E137 BewareGrenade
	{
		[CompilerGenerated]
		get
		{
			return _E029;
		}
		[CompilerGenerated]
		private set
		{
			_E029 = value;
		}
	}

	public _E145 PlayerFollowData
	{
		[CompilerGenerated]
		get
		{
			return _E02A;
		}
		[CompilerGenerated]
		private set
		{
			_E02A = value;
		}
	}

	public _E20E MoveToEnemyData
	{
		[CompilerGenerated]
		get
		{
			return _E02B;
		}
		[CompilerGenerated]
		private set
		{
			_E02B = value;
		}
	}

	public _E26D ArtilleryDangerPlace
	{
		[CompilerGenerated]
		get
		{
			return _E02C;
		}
		[CompilerGenerated]
		private set
		{
			_E02C = value;
		}
	}

	public _E143 FlashGrenade
	{
		[CompilerGenerated]
		get
		{
			return _E02D;
		}
		[CompilerGenerated]
		private set
		{
			_E02D = value;
		}
	}

	public _E1FC GrenadeSuicide
	{
		[CompilerGenerated]
		get
		{
			return _E02E;
		}
		[CompilerGenerated]
		private set
		{
			_E02E = value;
		}
	}

	public _E205 HealingBySomebody
	{
		[CompilerGenerated]
		get
		{
			return _E02F;
		}
		[CompilerGenerated]
		private set
		{
			_E02F = value;
		}
	}

	public _E247 PeacefulActions
	{
		[CompilerGenerated]
		get
		{
			return _E030;
		}
		[CompilerGenerated]
		private set
		{
			_E030 = value;
		}
	}

	public _E13D DeadBodyWork
	{
		[CompilerGenerated]
		get
		{
			return _E031;
		}
		[CompilerGenerated]
		private set
		{
			_E031 = value;
		}
	}

	public _E152 MagazineChecker
	{
		[CompilerGenerated]
		get
		{
			return _E032;
		}
		[CompilerGenerated]
		private set
		{
			_E032 = value;
		}
	}

	public _E15A SmokeGrenade
	{
		[CompilerGenerated]
		get
		{
			return _E033;
		}
		[CompilerGenerated]
		private set
		{
			_E033 = value;
		}
	}

	public _E24B SuppressShoot
	{
		[CompilerGenerated]
		get
		{
			return _E034;
		}
		[CompilerGenerated]
		private set
		{
			_E034 = value;
		}
	}

	public _E24C SuppressStationary
	{
		[CompilerGenerated]
		get
		{
			return _E035;
		}
		[CompilerGenerated]
		private set
		{
			_E035 = value;
		}
	}

	public _E1EF EnemyChooser
	{
		[CompilerGenerated]
		get
		{
			return _E036;
		}
		[CompilerGenerated]
		private set
		{
			_E036 = value;
		}
	}

	public _E148 GiftData
	{
		[CompilerGenerated]
		get
		{
			return _E037;
		}
		[CompilerGenerated]
		private set
		{
			_E037 = value;
		}
	}

	public _E13B DangerPointsData
	{
		[CompilerGenerated]
		get
		{
			return _E038;
		}
		[CompilerGenerated]
		private set
		{
			_E038 = value;
		}
	}

	public _E24A SuppressGrenade
	{
		[CompilerGenerated]
		get
		{
			return _E039;
		}
		[CompilerGenerated]
		private set
		{
			_E039 = value;
		}
	}

	public _E290 ShootData
	{
		[CompilerGenerated]
		get
		{
			return _E03A;
		}
		[CompilerGenerated]
		private set
		{
			_E03A = value;
		}
	}

	public _E1EE EnemiesController
	{
		[CompilerGenerated]
		get
		{
			return _E03B;
		}
		[CompilerGenerated]
		private set
		{
			_E03B = value;
		}
	}

	public _E28E AimingData
	{
		[CompilerGenerated]
		get
		{
			return _E03C;
		}
		[CompilerGenerated]
		private set
		{
			_E03C = value;
		}
	}

	public _E259 PlanDropItem
	{
		[CompilerGenerated]
		get
		{
			return _E03D;
		}
		[CompilerGenerated]
		private set
		{
			_E03D = value;
		}
	}

	public _E15B StandBy
	{
		[CompilerGenerated]
		get
		{
			return _E03E;
		}
		[CompilerGenerated]
		private set
		{
			_E03E = value;
		}
	}

	public _E14C HeadData
	{
		[CompilerGenerated]
		get
		{
			return _E03F;
		}
		[CompilerGenerated]
		private set
		{
			_E03F = value;
		}
	}

	public _E241 EatDrinkData
	{
		[CompilerGenerated]
		get
		{
			return _E040;
		}
		[CompilerGenerated]
		private set
		{
			_E040 = value;
		}
	}

	public _E248 SecondWeaponData
	{
		[CompilerGenerated]
		get
		{
			return _E041;
		}
		[CompilerGenerated]
		private set
		{
			_E041 = value;
		}
	}

	public _E206 Medecine
	{
		[CompilerGenerated]
		get
		{
			return _E042;
		}
		[CompilerGenerated]
		private set
		{
			_E042 = value;
		}
	}

	public _E28F RecoilData
	{
		[CompilerGenerated]
		get
		{
			return _E043;
		}
		[CompilerGenerated]
		private set
		{
			_E043 = value;
		}
	}

	public _E16D CallForHelp
	{
		[CompilerGenerated]
		get
		{
			return _E044;
		}
		[CompilerGenerated]
		private set
		{
			_E044 = value;
		}
	}

	public _E16E CalledData
	{
		[CompilerGenerated]
		get
		{
			return _E045;
		}
		[CompilerGenerated]
		private set
		{
			_E045 = value;
		}
	}

	public _E133 Ambush
	{
		[CompilerGenerated]
		get
		{
			return _E046;
		}
		[CompilerGenerated]
		private set
		{
			_E046 = value;
		}
	}

	public _E243 FriendlyTilt
	{
		[CompilerGenerated]
		get
		{
			return _E047;
		}
		[CompilerGenerated]
		private set
		{
			_E047 = value;
		}
	}

	public _E150 LootOpener
	{
		[CompilerGenerated]
		get
		{
			return _E048;
		}
		[CompilerGenerated]
		private set
		{
			_E048 = value;
		}
	}

	public _E139 Covers
	{
		[CompilerGenerated]
		get
		{
			return _E049;
		}
		[CompilerGenerated]
		private set
		{
			_E049 = value;
		}
	}

	public _E169 WeaponManager
	{
		[CompilerGenerated]
		get
		{
			return _E04A;
		}
		[CompilerGenerated]
		private set
		{
			_E04A = value;
		}
	}

	public _E252 Tactic
	{
		[CompilerGenerated]
		get
		{
			return _E04B;
		}
		[CompilerGenerated]
		private set
		{
			_E04B = value;
		}
	}

	public _E141 DoorOpener
	{
		[CompilerGenerated]
		get
		{
			return _E04C;
		}
		[CompilerGenerated]
		private set
		{
			_E04C = value;
		}
	}

	public _E244 Gesture
	{
		[CompilerGenerated]
		get
		{
			return _E04D;
		}
		[CompilerGenerated]
		private set
		{
			_E04D = value;
		}
	}

	public _E13A DangerArea
	{
		[CompilerGenerated]
		get
		{
			return _E04E;
		}
		[CompilerGenerated]
		private set
		{
			_E04E = value;
		}
	}

	public _E135 AssaultDangerArea
	{
		[CompilerGenerated]
		get
		{
			return _E04F;
		}
		[CompilerGenerated]
		private set
		{
			_E04F = value;
		}
	}

	public _E156 Receiver
	{
		[CompilerGenerated]
		get
		{
			return _E050;
		}
		[CompilerGenerated]
		private set
		{
			_E050 = value;
		}
	}

	public _E257 ItemTaker
	{
		[CompilerGenerated]
		get
		{
			return _E051;
		}
		[CompilerGenerated]
		private set
		{
			_E051 = value;
		}
	}

	public _E255 ExternalItemsController
	{
		[CompilerGenerated]
		get
		{
			return _E052;
		}
		[CompilerGenerated]
		private set
		{
			_E052 = value;
		}
	}

	public _E256 ItemDropper
	{
		[CompilerGenerated]
		get
		{
			return _E053;
		}
		[CompilerGenerated]
		private set
		{
			_E053 = value;
		}
	}

	public _E1ED SearchData
	{
		[CompilerGenerated]
		get
		{
			return _E054;
		}
		[CompilerGenerated]
		private set
		{
			_E054 = value;
		}
	}

	public _E2AA BotPersonalStats
	{
		[CompilerGenerated]
		get
		{
			return _E055;
		}
		[CompilerGenerated]
		private set
		{
			_E055 = value;
		}
	}

	public _E159 ShootFromPlace
	{
		[CompilerGenerated]
		get
		{
			return _E056;
		}
		[CompilerGenerated]
		private set
		{
			_E056 = value;
		}
	}

	public _E295 HearingSensor
	{
		[CompilerGenerated]
		get
		{
			return _E057;
		}
		[CompilerGenerated]
		private set
		{
			_E057 = value;
		}
	}

	public _E157 BotRequestController
	{
		[CompilerGenerated]
		get
		{
			return _E058;
		}
		[CompilerGenerated]
		private set
		{
			_E058 = value;
		}
	}

	public _E620 BotsController
	{
		[CompilerGenerated]
		get
		{
			return _E059;
		}
		[CompilerGenerated]
		private set
		{
			_E059 = value;
		}
	}

	public BifacialTransform MyHead
	{
		[CompilerGenerated]
		get
		{
			return _E05A;
		}
		[CompilerGenerated]
		private set
		{
			_E05A = value;
		}
	}

	public _E172 DecisionQueue
	{
		[CompilerGenerated]
		get
		{
			return _E05B;
		}
		[CompilerGenerated]
		private set
		{
			_E05B = value;
		}
	}

	public _E144 GameEventsData
	{
		[CompilerGenerated]
		get
		{
			return _E05C;
		}
		[CompilerGenerated]
		private set
		{
			_E05C = value;
		}
	}

	public _E629 GameDateTime
	{
		[CompilerGenerated]
		get
		{
			return _E05D;
		}
		[CompilerGenerated]
		private set
		{
			_E05D = value;
		}
	}

	public Vector2 Lean
	{
		[CompilerGenerated]
		get
		{
			return _E05E;
		}
		[CompilerGenerated]
		set
		{
			_E05E = value;
		}
	}

	public Vector3 Destination => Mover.CurPathLastPoint;

	public _E136 BotAttackManager
	{
		[CompilerGenerated]
		get
		{
			return _E05F;
		}
		[CompilerGenerated]
		private set
		{
			_E05F = value;
		}
	}

	public _E20F Mover
	{
		[CompilerGenerated]
		get
		{
			return _E060;
		}
		[CompilerGenerated]
		private set
		{
			_E060 = value;
		}
	}

	public _E15D BotTalk
	{
		[CompilerGenerated]
		get
		{
			return _E061;
		}
		[CompilerGenerated]
		private set
		{
			_E061 = value;
		}
	}

	public _E301 SpawnProfileData
	{
		[CompilerGenerated]
		get
		{
			return _E062;
		}
		[CompilerGenerated]
		set
		{
			_E062 = value;
		}
	}

	public bool HasPathAndNotComplete => Mover.HasPathAndNoComplete;

	public _E267 BotsGroup
	{
		[CompilerGenerated]
		get
		{
			return _E064;
		}
		[CompilerGenerated]
		set
		{
			_E064 = value;
		}
	}

	public EBotState BotState
	{
		get
		{
			return this.m__E001;
		}
		private set
		{
			this.m__E001 = value;
			if (_E063 != null)
			{
				_E063(this.m__E001);
			}
		}
	}

	public BifacialTransform Fireport
	{
		get
		{
			if (GetPlayer.MultiBarrelFireports != null && GetPlayer.MultiBarrelFireports.Length != 0)
			{
				return GetPlayer.MultiBarrelFireports[0];
			}
			if (GetPlayer.Fireport != null)
			{
				return GetPlayer.Fireport;
			}
			return WeaponRoot;
		}
	}

	public bool CanSprintPlayer => GetPlayer.Physical.CanSprint;

	[Obsolete("Use Player.Transform instead!", true)]
	public new Transform transform => base.transform;

	public _E23F PatrollingData
	{
		[CompilerGenerated]
		get
		{
			return _E065;
		}
		[CompilerGenerated]
		private set
		{
			_E065 = value;
		}
	}

	public Vector3 Position => Transform.position;

	public string GroupId => Profile.Info.GroupId;

	public string TeamId => Profile.Info.TeamId;

	public string Infiltration => Profile.Info.EntryPoint;

	public string ProfileId
	{
		[CompilerGenerated]
		get
		{
			return _E066;
		}
		[CompilerGenerated]
		private set
		{
			_E066 = value;
		}
	}

	public int Id
	{
		[CompilerGenerated]
		get
		{
			return _E067;
		}
		[CompilerGenerated]
		private set
		{
			_E067 = value;
		}
	}

	public EPlayerSide Side => GetPlayer.Profile.Info.Side;

	public BifacialTransform Transform => GetPlayer.PlayerBones.BodyTransform;

	public BifacialTransform WeaponRoot => GetPlayer.PlayerBones.WeaponRoot;

	public _E9C4 HealthController => GetPlayer.HealthController;

	public Profile Profile => GetPlayer.Profile;

	public Player GetPlayer
	{
		[CompilerGenerated]
		get
		{
			return _E068;
		}
		[CompilerGenerated]
		private set
		{
			_E068 = value;
		}
	}

	public _E279 AIData => GetPlayer.AIData;

	public bool IsAI
	{
		get
		{
			if (AIData != null)
			{
				return AIData.IsAI;
			}
			return false;
		}
	}

	public _E27C Loyalty => GetPlayer.Loyalty;

	public Dictionary<BodyPartType, _E1FF> MainParts
	{
		[CompilerGenerated]
		get
		{
			return _E069;
		}
		[CompilerGenerated]
		private set
		{
			_E069 = value;
		}
	}

	public Vector3 LookDirection => GetPlayer.LookDirection;

	public bool IsDead
	{
		[CompilerGenerated]
		get
		{
			return _E06A;
		}
		[CompilerGenerated]
		set
		{
			_E06A = value;
		}
	}

	public event Action<EBotState> OnBotStateChange
	{
		[CompilerGenerated]
		add
		{
			Action<EBotState> action = _E063;
			Action<EBotState> action2;
			do
			{
				action2 = action;
				Action<EBotState> value2 = (Action<EBotState>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E063, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<EBotState> action = _E063;
			Action<EBotState> action2;
			do
			{
				action2 = action;
				Action<EBotState> value2 = (Action<EBotState>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E063, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void SayGroupAboutEnemy(_E5B4 person, Vector3? partPos = null)
	{
	}

	public void GoToPoint(CustomNavigationPoint targetPoint, string debugdata = "")
	{
		Memory.GoToPoint(targetPoint, debugdata);
	}

	public static BotOwner Create(Player player, GameObject behaviourTreePrefab, _E629 gameDataTime, _E620 botsController, bool isLocalGame)
	{
		player.ProceduralWeaponAnimation.Mask = EProceduralAnimationMask.DrawDown;
		player.Profile.UncoverAll();
		BotDifficulty difficulty;
		WildSpawnType role;
		if (player.Profile.Info != null && player.Profile.Info.Settings != null)
		{
			difficulty = player.Profile.Info.Settings.BotDifficulty;
			role = player.Profile.Info.Settings.Role;
		}
		else
		{
			difficulty = BotDifficulty.normal;
			role = WildSpawnType.assault;
		}
		_E2A1 settings = Singleton<_E2A3>.Instance.GetSettings(difficulty, role);
		BotOwner botOwner = player.gameObject.AddComponent<BotOwner>();
		botOwner.m__E007 = isLocalGame;
		botOwner.Settings = settings;
		botOwner.BotTalk = new _E15D(botOwner);
		botOwner.Tactic = new _E252(botOwner);
		botOwner.name = string.Format(_ED3E._E000(136634), ++BotCount);
		player.SetOwnerToAIData(botOwner);
		botOwner.ENEMY_LOOK_AT_ME = Mathf.Cos(botOwner.Settings.FileSettings.Mind.ENEMY_LOOK_AT_ME_ANG * ((float)Math.PI / 180f));
		botOwner.BotsController = botsController;
		botOwner.GetPlayer = player;
		botOwner.Id = player.Id;
		botOwner.ProfileId = player.Profile.Id;
		botOwner.GetPlayer.ActiveHealthController.SetDamageCoeff(botOwner.Settings.FileSettings.Core.DamageCoeff);
		botOwner.MyHead = player.PlayerBones.Head;
		botOwner.Brain = new _E179(botOwner);
		botOwner.DecisionProxy = new _E10F(botOwner);
		botOwner.DecisionQueue = new _E172(botOwner);
		botOwner.BotLight = new _E14F(botOwner);
		botOwner.BotTurnAwayLight = new _E1FE(botOwner);
		botOwner.LookData = new _E219(botOwner);
		botOwner.HeadData = new _E14C(botOwner);
		botOwner.NavMeshCutterController = new _E214(botOwner);
		botOwner.GrenadeSuicide = new _E1FC(botOwner);
		botOwner.EatDrinkData = new _E241(botOwner);
		botOwner.SecondWeaponData = new _E248(botOwner);
		botOwner.MagazineChecker = new _E152(botOwner);
		botOwner.FriendlyTilt = new _E243(botOwner);
		botOwner.GoalCulculator = new _E1F6(botOwner);
		botOwner.PlanDropItem = new _E259(botOwner);
		botOwner.ItemTaker = new _E257(botOwner);
		botOwner.ExternalItemsController = new _E255(botOwner);
		botOwner.PeaceHardAim = new _E245(botOwner);
		botOwner.PeaceLook = new _E246(botOwner);
		botOwner.MoveToEnemyData = new _E20E(botOwner);
		botOwner.DangerArea = new _E13A(botOwner);
		botOwner.AssaultDangerArea = new _E135(botOwner);
		botOwner.ItemDropper = new _E256(botOwner);
		botOwner.PlayerFollowData = new _E145(botOwner);
		botOwner.LoyaltyData = new _E151(botOwner);
		botOwner.AssaultBuildingData = new _E134(botOwner);
		botOwner.FindPlaceToShoot = new _E1FB(botOwner);
		botOwner.GoToSomePointData = new _E20D(botOwner);
		botOwner.FriendChecker = new _E242(botOwner, botsController.Bots.GetConnector());
		botOwner.PeacefulActions = new _E247(botOwner);
		botOwner.SuspiciousPlaceData = new _E276(botOwner);
		botOwner.AimingData = new _E28B(botOwner);
		botOwner.Covers = new _E139(botOwner);
		botOwner.StandBy = new _E15B(botOwner);
		botOwner.SuppressStationary = new _E24C(botOwner);
		botOwner.EnemyLookData = new _E142(botOwner, onlyIfVisible: true);
		botOwner.HealingBySomebody = new _E205(botOwner);
		botOwner.DelayActions = new _E13E(botOwner);
		botOwner.Medecine = new _E206(botOwner);
		botOwner.LeaveData = new _E14E(botOwner);
		botOwner.BotFollower = _E128.Create(botOwner);
		botOwner.UnityEditorRunChecker = new _E218(botOwner);
		botOwner.EnemiesController = _E1EE.Create(botOwner);
		botOwner.Boss = new _E126(botOwner);
		botOwner.DoorOpener = new _E141(botOwner);
		botOwner.RecoilData = new _E28F(botOwner);
		botOwner.LootOpener = new _E150(botOwner);
		botOwner.DeadBodyWork = new _E13D(botOwner);
		botOwner.WeaponManager = new _E169(botOwner);
		botOwner.BotRun = new _E158(botOwner);
		botOwner.HealAnotherTarget = new _E204(botOwner);
		botOwner.Steering = new _E215(botOwner);
		botOwner.ShootData = new _E290(botOwner, botOwner.RecoilData);
		botOwner.DeadBodyData = new _E170(botOwner);
		botOwner.BotLay = new _E14D(botOwner);
		botOwner.Tilt = new _E15E(botOwner);
		botOwner.TrianglePosition = new _E216(botOwner);
		botOwner.GoToSomePointData = new _E20D(botOwner);
		botOwner.Receiver = new _E156(botOwner);
		botOwner.NightVision = new _E1FD(botOwner);
		botOwner.SearchData = _E1ED.Create(botOwner);
		botOwner.GoToSomePointData = new _E20D(botOwner);
		botOwner.Gesture = new _E244(botOwner);
		botOwner.GameDateTime = gameDataTime;
		botOwner.LookSensor = new _E296(botOwner);
		botOwner.BotAttackManager = new _E136(botOwner);
		botOwner.HearingSensor = new _E295(botOwner);
		botOwner.BotRequestController = new _E157(botOwner);
		botOwner.MainParts = _E1FF.Create(botOwner, player.PlayerBones);
		botOwner.BotPersonalStats = new _E2AA();
		botOwner.ShootFromPlace = new _E159(botOwner);
		botOwner.DebugMemory = new _E102(botOwner);
		botOwner.BewareGrenade = new _E137(botOwner);
		botOwner.ArtilleryDangerPlace = new _E26D(botOwner);
		botOwner.FlashGrenade = new _E143(botOwner);
		botOwner.CellData = new _E138(botOwner);
		botOwner.DogFight = new _E13F(botOwner);
		botOwner.CallForHelp = new _E16D(botOwner);
		botOwner.CalledData = new _E16E(botOwner);
		botOwner.Ambush = new _E133(botOwner);
		botOwner.PriorityAxeTarget = new _E21B(botOwner);
		botOwner.SmokeGrenade = new _E15A(botOwner);
		botOwner.SuppressShoot = new _E24B(botOwner);
		botOwner.SuppressGrenade = new _E24A(botOwner);
		botOwner.SuppressStationary = new _E24C(botOwner);
		botOwner.DangerPointsData = new _E13B(botOwner);
		botOwner.EnemyChooser = _E1EF.Create(botOwner);
		botOwner.GiftData = new _E148(botOwner);
		if (botOwner.Settings.FileSettings.Move.ETERNITY_STAMINA)
		{
			botOwner.GetPlayer.Physical.Stamina.ForceMode = true;
			botOwner.GetPlayer.Physical.HandsStamina.ForceMode = true;
		}
		return botOwner;
	}

	public void PreActivate(BotZone zone, _E629 time, _E267 group, bool autoActivate = true)
	{
		this.m__E004 = Time.time;
		GameDateTime = time;
		BotsGroup = group;
		LookSensor.UpdateZoneValue(zone);
		Covers.Init(zone);
		if (GetPlayer.CharacterControllerCommon is _E347)
		{
			Mover = new _E211(this, GetPlayer);
		}
		else
		{
			Mover = new _E213(this, GetPlayer);
		}
		WeaponManager.PreActivate();
		Memory = new _E5B2(this, BotsGroup);
		PatrollingData = new _E23F(this);
		GameEventsData = new _E144(this);
		_E004();
		DebugMemory.Init();
		if (autoActivate)
		{
			BotState = EBotState.PreActive;
		}
	}

	public void PostActivate()
	{
		if (BotState == EBotState.NonActive)
		{
			BotState = EBotState.PreActive;
		}
	}

	private void _E000()
	{
		Collider collider = GetPlayer.CharacterControllerCommon.GetCollider();
		foreach (BotOwner botOwner in BotsGroup.BotGame.BotsController.Bots.BotOwners)
		{
			_E320.IgnoreCollision(botOwner.GetPlayer.CharacterControllerCommon.GetCollider(), collider);
		}
	}

	[CanBeNull]
	public _E07F CurrentEnemyTargetPosition(bool sensPosition)
	{
		if (Memory.GoalEnemy == null)
		{
			return null;
		}
		Vector3 point = ((!sensPosition) ? Memory.GoalEnemy.BodyPart.Position : (Memory.GoalEnemy.EnemyLastPosition + STAY_HEIGHT));
		return new _E07F(point);
	}

	private void _E001(Func<int, bool> condition, _E5B4 person)
	{
		BotsGroup.RemoveInfo(person);
		if (Memory.GoalEnemy.Person.Id == person.Id)
		{
			Memory.GoalEnemy = null;
		}
	}

	public void Sprint(bool val, bool withDebugCallback = true)
	{
		if (val)
		{
			SetPose(1f);
			AimingData.LoseTarget();
		}
		Mover.Sprint(val, withDebugCallback);
	}

	private void _E002()
	{
		GetPlayer.OnPlayerDead += _E003;
		GetPlayer.BeingHitAction += _E009;
	}

	private void _E003(Player player, Player lastAggressor, _EC23 lastDamageInfo, EBodyPart lastBodyPart)
	{
		if (lastAggressor != null)
		{
			BotsGroup.ReportAboutEnemy(lastAggressor, EEnemyPartVisibleType.visible);
			_E009(lastDamageInfo, EBodyPart.Chest, 0f);
		}
	}

	private void _E004()
	{
		GetPlayer.HealthController.ApplyDamageEvent += _E007;
		GetPlayer.HealthController.DiedEvent += _E006;
		if (Singleton<_E307>.Instantiated)
		{
			Singleton<_E307>.Instance.OnBodyBotDead += _E005;
		}
	}

	private void _E005(Vector3 obj)
	{
		if (Memory.IsPeace && (obj - Transform.position).sqrMagnitude < Settings.FileSettings.Hearing.DEAD_BODY_SOUND_RAD)
		{
			BotsGroup.AddPointToSearch(obj, 80f, this);
		}
	}

	private void _E006(EDamageType damageType)
	{
		if (Singleton<_E307>.Instantiated)
		{
			Vector3 position = Transform.position;
			Singleton<_E307>.Instance.DeadBodySound(position);
		}
		BotsController.BotDied(this);
		BotPersonalStats.Death();
		Dispose();
		IsDead = true;
		BotsGroup.BotZone.ZoneDangerAreas.BotDied(Position);
		if (this.m__E000 != null)
		{
			this.m__E000(this);
		}
		else
		{
			UnityEngine.Debug.LogError(_ED3E._E000(136625) + this.m__E001);
		}
	}

	public void Dispose()
	{
		_E063 = null;
		_ = Time.time;
		_ = this.m__E004;
		if (this.m__E001 == EBotState.PreActive)
		{
			return;
		}
		BotState = EBotState.Disposed;
		try
		{
			Brain?.Dispose();
			ArtilleryDangerPlace?.Dispose();
			AIData?.AskRequests?.DisposeAll();
			Mover?.Dispose();
			SuppressGrenade?.Dispose();
			SuppressShoot?.Dispose();
			SuppressStationary?.Dispose();
			BotLay?.Dispose();
			AssaultBuildingData?.Dispose();
			LoyaltyData?.Dispose();
			Boss?.Dispose();
			Tactic?.Dispose();
			FriendlyTilt?.Dispose();
			ExternalItemsController?.Dispose();
			DangerArea?.Dispose();
			LeaveData?.Dispose();
			EnemyChooser?.Dispose();
			Covers?.Dispose();
			DebugMemory?.Dispose();
			SearchData?.Dispose();
			PlayerFollowData?.Dispose();
			WeaponManager?.Dispose();
			LookSensor?.Dispose();
		}
		catch (Exception)
		{
		}
		try
		{
			UnityEditorRunChecker?.Dispose();
			Medecine?.Dispose();
			NavMeshCutterController?.Dispose();
			FlashGrenade?.Dispose();
			PatrollingData?.Dispose();
			PeacefulActions?.Dispose();
			BotFollower?.Dispose();
			ShootData?.Dispose();
			BotRequestController?.Dispose();
			DelayActions?.Dispose();
			EatDrinkData?.Dispose();
			PeaceHardAim?.Dispose();
			ItemTaker?.Dispose();
		}
		catch (Exception)
		{
		}
		try
		{
			_E008();
		}
		catch (Exception)
		{
		}
		try
		{
			PatrollingData?.Disable();
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].isTrigger = false;
			}
			BotState = EBotState.NonActive;
			HearingSensor?.Dispose();
			Receiver?.Dispose();
		}
		catch (Exception)
		{
		}
		try
		{
			Memory?.Dispose();
			BotPersonalStats?.Dispose();
		}
		catch (Exception)
		{
		}
		PeaceLook = null;
		EnemyChooser = null;
		Brain = null;
		ArtilleryDangerPlace = null;
		Mover = null;
		SuppressGrenade = null;
		AssaultBuildingData = null;
		SuppressShoot = null;
		BotLay = null;
		LoyaltyData = null;
		FriendlyTilt = null;
		ExternalItemsController = null;
		DangerArea = null;
		LeaveData = null;
		Covers = null;
		DebugMemory = null;
		SearchData = null;
		UnityEditorRunChecker = null;
		Medecine = null;
		PatrollingData = null;
		BotFollower = null;
		DelayActions = null;
		EatDrinkData = null;
		PeaceHardAim = null;
		ItemTaker = null;
		PatrollingData = null;
		HearingSensor = null;
		Receiver = null;
	}

	public bool IsRole(WildSpawnType role)
	{
		if (Profile == null || Profile.Info == null || Profile.Info.Settings == null)
		{
			return false;
		}
		return Profile.Info.Settings.Role == role;
	}

	public bool IsFollower()
	{
		if (Profile == null || Profile.Info == null || Profile.Info.Settings == null)
		{
			return false;
		}
		return Profile.Info.Settings.IsFollower();
	}

	public float DistTo(Vector3 v)
	{
		return (Transform.position - v).magnitude;
	}

	public float SDistTo(Vector3 v)
	{
		return (Transform.position - v).sqrMagnitude;
	}

	private void _E007(EBodyPart bodyPart, float damage, _EC23 damageInfo)
	{
		damageInfo.DamageType.IsSelfInflicted();
	}

	public bool IsEnemyLookingAtMe(_E1F0 goalEnemy)
	{
		if (goalEnemy == null)
		{
			return false;
		}
		return IsEnemyLookingAtMe(goalEnemy.Person);
	}

	public bool IsEnemyLookingAtMe(_E5B4 gamePerson)
	{
		Vector3 position = WeaponRoot.position;
		BifacialTransform weaponRoot = gamePerson.WeaponRoot;
		return _E39C.IsAngLessNormalized(_E39C.NormalizeFastSelf(position - weaponRoot.position), gamePerson.LookDirection, 0.9659258f);
	}

	private void _E008()
	{
		this.m__E002 = true;
		GetPlayer.OnPlayerDead -= _E003;
		GetPlayer.BeingHitAction -= _E009;
		GetPlayer.HealthController.ApplyDamageEvent -= _E007;
		GetPlayer.HealthController.DiedEvent -= _E006;
		if (Singleton<_E307>.Instantiated)
		{
			Singleton<_E307>.Instance.OnBodyBotDead -= _E005;
		}
	}

	private void _E009(_EC23 damageInfo, EBodyPart bodyType, float damageReducedByArmor)
	{
		StandBy.GetHit();
		if (!(damageInfo.Player == null))
		{
			if (!damageInfo.Player.IsAI && damageInfo.Player.Side == EPlayerSide.Savage && !Profile.Info.Settings.Role.IsHostileToEverybody() && !damageInfo.Player.Loyalty.CanBeFreeKilled)
			{
				damageInfo.Player.Loyalty.MarkAsCanBeFreeKilled();
			}
			BotPersonalStats.GetHit(damageInfo, bodyType);
			Memory.GetHit(damageInfo);
			if (damageInfo.Player != null && damageInfo.Player.Side == Side)
			{
				BotTalk.TrySay(EPhraseTrigger.FriendlyFire);
			}
		}
	}

	private void _E00A()
	{
		try
		{
			LookSensor.Activate();
			Settings.Activate();
			ExternalItemsController.Activate();
			ItemTaker.Activate();
			PlanDropItem.Activate();
			ItemDropper.Activate();
			NavMeshCutterController.Activate();
			AimingData.Activate();
			BotFollower.Activate();
			FriendlyTilt.Activate();
			Tactic.Activate();
			EnemiesController.Activate(BotsGroup.BotGame.BotsController.OnlineDependenceSettings.CanPersueAxeman);
			HearingSensor.Init();
			LeaveData.Activate();
			Receiver.Init();
			Mover.Activate();
			BotTalk.Activate();
			LoyaltyData.Activate();
			AssaultDangerArea.Activate();
			DangerArea.Activate();
			TrianglePosition.Activate(BotsGroup.BotZone.CachePathLength);
			BotPersonalStats.Init(this, BotsGroup.BotZone.name);
			StandBy.InitPoints(BotsGroup.BotZone.Modifier.DistToActivate, BotsGroup.BotZone.Modifier.DistToSleep);
			_E002();
			FlashGrenade.Activate();
			PeaceHardAim.Activate();
			ShootData.Activate();
			PeaceLook.Activate();
			CellData.Activate();
			UnityEditorRunChecker.Activate();
			NightVision.Activate();
			SearchData.Activate();
			Medecine.Activate();
			BotState = EBotState.Active;
			Memory.Activate();
			SuppressShoot.Activate();
			EatDrinkData.Activate();
			SecondWeaponData.Activate();
			SuppressGrenade.Activate();
			ArtilleryDangerPlace.Activate();
			_E00B();
			Brain.Activate();
			PatrollingData.Activate();
			WeaponManager.Activate();
			BotFollower.TryFindBoss();
			this.m__E005 = Time.time;
		}
		catch (Exception)
		{
			BotState = EBotState.ActiveFail;
		}
	}

	private void _E00B()
	{
		if (Settings.FileSettings.Boss.EFFECT_PAINKILLER)
		{
			GetPlayer.ActiveHealthController.DoPainKiller();
		}
		if (Settings.FileSettings.Boss.EFFECT_REGENERATION_PER_MIN > 0f)
		{
			GetPlayer.ActiveHealthController.DoScavRegeneration(Settings.FileSettings.Boss.EFFECT_REGENERATION_PER_MIN);
		}
	}

	public void Disable()
	{
		BotState = EBotState.NonActive;
	}

	public void UpdateManual()
	{
		if (BotState == EBotState.Active && GetPlayer.HealthController.IsAlive)
		{
			StandBy.Update();
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			LookSensor.UpdateLook();
			stopwatch.Stop();
			if (StandBy.StandByType != BotStandByType.paused)
			{
				if (this.m__E003 < Time.time)
				{
					CalcGoal();
				}
				HeadData.ManualUpdate();
				ShootData.ManualUpdate();
				Tilt.ManualUpdate();
				NightVision.ManualUpdate();
				CellData.Update();
				DogFight.ManualUpdate();
				FriendChecker.ManualUpdate();
				RecoilData.LosingRecoil();
				Mover.ManualUpdate();
				AimingData.PermanentUpdate();
				TrianglePosition.ManualUpdate();
				Medecine.ManualUpdate();
				Boss.ManualUpdate();
				BotTalk.ManualUpdate();
				WeaponManager.ManualUpdate();
				BotRequestController.Update();
				Tactic.UpdateChangeTactics();
				Memory.ManualUpdate(Time.deltaTime);
				Settings.UpdateManual();
				BotRequestController.TryToFind();
				if (GetPlayer.UpdateQueue == EUpdateQueue.Update)
				{
					Mover.ManualFixedUpdate();
					Steering.ManualFixedUpdate();
				}
				UnityEditorRunChecker.ManualLateUpdate();
			}
		}
		else if (BotState == EBotState.PreActive && WeaponManager.IsReady)
		{
			if (NavMesh.SamplePosition(GetPlayer.Position, out var _, 0.2f, -1))
			{
				_E00A();
			}
			else if (this.m__E006 < Time.time)
			{
				this.m__E006 = Time.time + 1f;
				Transform.position = BotsGroup.BotZone.SpawnPoints.RandomElement().Position + Vector3.up * 0.5f;
				_E00A();
			}
		}
	}

	public void Deactivate()
	{
		BotState = EBotState.NonActive;
	}

	public void CalcGoal()
	{
		this.m__E003 = _E2A0.Core.UPDATE_GOAL_TIMER_SEC + Time.time;
		BotsGroup.CalcGoalForBot(this);
	}

	private void FixedUpdate()
	{
		if (BotState == EBotState.Active && GetPlayer.UpdateQueue == EUpdateQueue.FixedUpdate)
		{
			Steering.ManualFixedUpdate();
			Mover.ManualFixedUpdate();
		}
	}

	public void SetLean(Vector2 lean)
	{
		Lean = lean;
	}

	public void SetYAngle(float angle)
	{
		Steering.SetYAngle(angle);
	}

	public void StopMove()
	{
		Mover.Stop();
	}

	public void SetTargetMoveSpeed(float speed)
	{
		Mover.SetTargetMoveSpeed(speed);
	}

	public void MovementResume()
	{
		Mover.MovementResume();
	}

	public void SetPose(float targetPose)
	{
		Mover.SetPose(targetPose);
	}

	public void MovementPause(float pauseTime)
	{
		Mover.MovementPause(pauseTime);
	}

	public void SetDieCallback(Action<BotOwner> botDied)
	{
		this.m__E000 = botDied;
	}

	public void GoToByWay(Vector3[] way, float reachDist = -1f)
	{
		if (reachDist < 0f)
		{
			reachDist = Settings.FileSettings.Move.REACH_DIST;
		}
		Mover.GoToByWay(way, reachDist, Vector3.zero);
	}

	public NavMeshPathStatus GoToPoint(Vector3 position, bool slowAtTheEnd = true, float reachDist = -1f, bool getUpWithCheck = false, bool mustHaveWay = true, bool mustGetUp = true)
	{
		if (reachDist < 0f)
		{
			reachDist = Settings.FileSettings.Move.REACH_DIST;
		}
		return Mover.GoToPoint(position, slowAtTheEnd, reachDist, getUpWithCheck: true, mustHaveWay, mustGetUp);
	}

	private void OnDrawGizmos()
	{
	}

	public void OnDrawGizmosSelected()
	{
		_E103.DrawBotOwnerGizmosSelected(this);
	}

	public void SetHandle(string toString)
	{
	}

	public HashSet<Vector3> CarePositions()
	{
		return Covers.CarePositions();
	}
}
