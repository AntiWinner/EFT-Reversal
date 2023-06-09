using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using BSG.CameraEffects;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.CameraControl;

public class PlayerCameraController : MonoBehaviour
{
	private delegate _E8AA _E000(PlayerCameraController playerCameraController);

	[SerializeField]
	private EUpdateQueue _playerUpdateQueue;

	[CompilerGenerated]
	private static Action<PlayerCameraController, Camera> m__E000;

	[CompilerGenerated]
	private static Action m__E001;

	[CompilerGenerated]
	private Player m__E002;

	private _E8AA m__E003;

	private readonly Dictionary<Type, _E8AA> m__E004 = new Dictionary<Type, _E8AA>();

	private Dictionary<Type, _E000> m__E005;

	private VisorEffect m__E006;

	private Action _E007;

	private bool _E008;

	private Action _E009;

	private Action _E00A;

	private Action _E00B;

	private Action _E00C;

	private HysteresisFilter _E00D;

	public Camera Camera => _E8A8.Instance.Camera;

	public Player Player
	{
		[CompilerGenerated]
		get
		{
			return this.m__E002;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E002 = value;
		}
	}

	private _E8A8 _E00E => _E8A8.Instance;

	public static event Action<PlayerCameraController, Camera> OnPlayerCameraControllerCreated
	{
		[CompilerGenerated]
		add
		{
			Action<PlayerCameraController, Camera> action = PlayerCameraController.m__E000;
			Action<PlayerCameraController, Camera> action2;
			do
			{
				action2 = action;
				Action<PlayerCameraController, Camera> value2 = (Action<PlayerCameraController, Camera>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref PlayerCameraController.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<PlayerCameraController, Camera> action = PlayerCameraController.m__E000;
			Action<PlayerCameraController, Camera> action2;
			do
			{
				action2 = action;
				Action<PlayerCameraController, Camera> value2 = (Action<PlayerCameraController, Camera>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref PlayerCameraController.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action OnPlayerCameraControllerDestroyed
	{
		[CompilerGenerated]
		add
		{
			Action action = PlayerCameraController.m__E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref PlayerCameraController.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = PlayerCameraController.m__E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref PlayerCameraController.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static PlayerCameraController Create(Player player)
	{
		PlayerCameraController playerCameraController = player.gameObject.GetComponent<PlayerCameraController>();
		if (playerCameraController == null)
		{
			playerCameraController = player.gameObject.AddComponent<PlayerCameraController>()._E000(player);
		}
		return playerCameraController;
	}

	private PlayerCameraController _E000(Player player)
	{
		Player = player;
		_playerUpdateQueue = Player.UpdateQueue;
		LevelSettings instance = Singleton<LevelSettings>.Instance;
		GameObject cameraFromPrefab = ((instance != null && instance.CameraPrefab != null) ? instance.CameraPrefab.gameObject : null);
		_E00E.SetCameraFromPrefab(cameraFromPrefab);
		if (Player.PointOfView == EPointOfView.FirstPerson)
		{
			InitiateOperation<_E8AD>();
		}
		else if (Player.PointOfView == EPointOfView.FreeCamera)
		{
			InitiateOperation<_E8AC>();
		}
		_E008 = false;
		this.m__E006 = _E00E.VisorEffect;
		_E00D = _E00E.VisorSwitcher;
		_E009 = Player.NightVisionObserver.Changed.Bind(_E004);
		_E00A = Player.ThermalVisionObserver.Changed.Bind(_E005);
		_E00B = Player.FaceShieldObserver.Changed.Bind(_E001);
		_E00C = Player.FaceCoverObserver.Changed.Bind(_E002);
		_E007 = Player.PointOfViewChanged.Bind(UpdatePointOfView);
		PlayerCameraController.m__E000(this, Camera);
		if (!Singleton<PlayerCameraController>.Instantiated)
		{
			Singleton<PlayerCameraController>.Create(this);
		}
		return this;
	}

	private void _E001()
	{
		FaceShieldComponent component = Player.FaceShieldObserver.Component;
		if (Player.FaceCoverObserver.Component == null)
		{
			_E003(component);
		}
	}

	private void _E002()
	{
		FaceShieldComponent component = Player.FaceCoverObserver.Component;
		if (Player.FaceShieldObserver.Component == null)
		{
			_E003(component);
		}
	}

	private void _E003(FaceShieldComponent faceShield)
	{
		bool flag = _E008;
		_E008 = true;
		if (faceShield != null)
		{
			RepairableComponent itemComponent = faceShield.Item.GetItemComponent<RepairableComponent>();
			if (itemComponent != null)
			{
				this.m__E006.BreakCompletely(faceShield.HitSeed, Math.Abs(itemComponent.Durability) < 0.001f);
			}
			this.m__E006.SetMask((VisorEffect.EMask)faceShield.Mask);
			this.m__E006.DrawScratches(faceShield.HitSeed, Mathf.Min(faceShield.Hits, 20));
		}
		bool isOn = faceShield != null && (faceShield.Togglable == null || faceShield.Togglable.On);
		_E00D.Set(isOn, !flag);
	}

	private void _E004()
	{
		NightVision nightVision = _E8A8.Instance.NightVision;
		if (!nightVision.enabled)
		{
			return;
		}
		NightVisionComponent component = Player.NightVisionObserver.Component;
		bool flag = component?.Togglable.On ?? false;
		if (component != null)
		{
			ThermalVision thermalVision = _E8A8.Instance.ThermalVision;
			if ((object)thermalVision != null && thermalVision.InProcessSwitching)
			{
				thermalVision.FastForwardSwitch();
				if (thermalVision.On && flag)
				{
					thermalVision.StartSwitch(on: false);
					thermalVision.FastForwardSwitch();
				}
			}
			nightVision.Intensity = component.Template.Intensity;
			nightVision.MaskSize = component.Template.MaskSize;
			nightVision.NoiseIntensity = component.Template.NoiseIntensity;
			nightVision.NoiseScale = component.Template.NoiseScale;
			nightVision.Color = component.Template.Color;
			nightVision.SetMask(component.Template.Mask);
			nightVision.ApplySettings();
		}
		nightVision.StartSwitch(flag);
	}

	private void _E005()
	{
		ThermalVision thermalVision = _E8A8.Instance.ThermalVision;
		if (!thermalVision.enabled)
		{
			return;
		}
		ThermalVisionComponent component = Player.ThermalVisionObserver.Component;
		bool flag = component?.Togglable.On ?? false;
		if (component != null)
		{
			NightVision nightVision = _E8A8.Instance.NightVision;
			if ((object)nightVision != null && nightVision.InProcessSwitching)
			{
				nightVision.FastForwardSwitch();
				if (nightVision.On && flag)
				{
					nightVision.StartSwitch(on: false);
					nightVision.FastForwardSwitch();
				}
			}
			thermalVision.ThermalVisionUtilities.CurrentRampPalette = component.Template.RampPalette;
			thermalVision.ThermalVisionUtilities.DepthFade = component.Template.DepthFade;
			ValuesCoefs valuesCoefs = thermalVision.ThermalVisionUtilities.ValuesCoefs;
			valuesCoefs.MainTexColorCoef = component.Template.MainTexColorCoef;
			valuesCoefs.MinimumTemperatureValue = component.Template.MinimumTemperatureValue;
			valuesCoefs.RampShift = component.Template.RampShift;
			thermalVision.IsNoisy = component.Template.IsNoisy;
			thermalVision.ThermalVisionUtilities.NoiseParameters.NoiseIntensity = component.Template.NoiseInstensity;
			thermalVision.IsFpsStuck = component.Template.IsFpsStuck;
			thermalVision.IsGlitch = component.Template.IsGlitch;
			thermalVision.IsMotionBlurred = component.Template.IsMotionBlurred;
			MaskDescription maskDescription = thermalVision.ThermalVisionUtilities.MaskDescription;
			thermalVision.SetMask(component.Template.Mask);
			maskDescription.MaskSize = component.Template.MaskSize;
			thermalVision.IsPixelated = component.Template.IsPixelated;
			thermalVision.PixelationUtilities.BlockCount = component.Template.PixelationBlockCount;
			thermalVision.SetMaterialProperties();
		}
		thermalVision.StartSwitch(flag);
	}

	public void InitiateOperation<TCreateOperation>() where TCreateOperation : _E8AA
	{
		if (this.m__E005 == null)
		{
			this.m__E005 = _E006();
		}
		Type typeFromHandle = typeof(TCreateOperation);
		if (!this.m__E004.ContainsKey(typeFromHandle))
		{
			this.m__E004[typeFromHandle] = this.m__E005[typeFromHandle](this);
		}
		this.m__E003?.OnExit();
		this.m__E003 = this.m__E004[typeFromHandle];
		this.m__E003.OnEnter();
		this.m__E003.ManualLateUpdate(0f);
	}

	private Dictionary<Type, _E000> _E006()
	{
		return new Dictionary<Type, _E000>
		{
			{
				typeof(_E8AD),
				(PlayerCameraController playerCamera) => new _E8AD(playerCamera)
			},
			{
				typeof(_E8AC),
				(PlayerCameraController playerCamera) => new _E8AC(playerCamera)
			}
		};
	}

	public void UpdatePointOfView()
	{
		this.m__E003?.SetPointOfView(Player.PointOfView);
	}

	private void LateUpdate()
	{
		this.m__E003?.ManualLateUpdate(Time.deltaTime);
	}

	private void FixedUpdate()
	{
		if (_playerUpdateQueue == EUpdateQueue.FixedUpdate)
		{
			this.m__E003?.ManualFixedUpdate(Time.deltaTime);
		}
	}

	private void Update()
	{
		if (_playerUpdateQueue == EUpdateQueue.Update)
		{
			this.m__E003?.ManualFixedUpdate(Time.deltaTime);
		}
		if (this.m__E006 != null)
		{
			this.m__E006.StepFrequency = Player.ProceduralWeaponAnimation.VisorStepFrequency;
			Vector2 vector = new Vector2(Player.MovementContext.Velocity.x, Player.MovementContext.Velocity.z);
			this.m__E006.Velocity = vector.magnitude;
		}
	}

	private void OnDestroy()
	{
		_E007();
		_E009();
		_E00A();
		_E00B();
		_E00C();
		_E00D.Deinit();
		if (Singleton<AudioListenerConsistencyManager>.Instantiated)
		{
			Singleton<AudioListenerConsistencyManager>.Instance.Reset();
		}
		if (Singleton<PlayerCameraController>.Instantiated)
		{
			Singleton<PlayerCameraController>.Release(Singleton<PlayerCameraController>.Instance);
		}
		PlayerCameraController.m__E001?.Invoke();
	}
}
