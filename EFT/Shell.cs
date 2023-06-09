using System.Runtime.CompilerServices;
using EFT.Ballistics;
using UnityEngine;

namespace EFT;

public class Shell : BouncingObject
{
	private const float _E00F = 100f;

	[SerializeField]
	private ECaliber _caliber;

	private Vector3 _E010;

	[CompilerGenerated]
	private _E6FC _E011;

	public _E6FC CollisionListener
	{
		[CompilerGenerated]
		get
		{
			return _E011;
		}
		[CompilerGenerated]
		set
		{
			_E011 = value;
		}
	}

	private void Awake()
	{
		base.enabled = false;
	}

	public void ActivatePhysics(Vector3 beginPoint, Vector3 velocity, Vector3 rotationVector, Vector3 weaponForward)
	{
		_E010 = rotationVector;
		EFTHardSettings.ShellsSettings shells = EFTHardSettings.Instance.Shells;
		if (shells.velocityRotation > 0f)
		{
			velocity = Quaternion.AngleAxis(shells.velocityRotation, weaponForward) * velocity;
		}
		Init(beginPoint, velocity * EFTHardSettings.Instance.Shells.velocityMult, EFTHardSettings.Instance.Shells.radius, EFTHardSettings.Instance.Shells.playMult, _E37B.ShellsCollisionsMask, EFTHardSettings.Instance.Shells.maxCastCount, EFTHardSettings.Instance.Shells.deltaTimeStep, EFTHardSettings.Instance.Shells.randomReboundSpread, EFTHardSettings.Instance.Shells.bounceSpeedMult, EFTHardSettings.Instance.Shells.showDebug);
		base.enabled = true;
	}

	public void DisablePhysics()
	{
		base.enabled = false;
	}

	public void SetCaliber(ECaliber caliber)
	{
		_caliber = caliber;
	}

	protected override void OnBounce(Collider collider)
	{
		base.OnBounce(collider);
		_E010 = base.VelocitySqrMagnitude * new Vector3(EFTHardSettings.Instance.Shells.ReboundRotationX.Random(randomizeSign: true), EFTHardSettings.Instance.Shells.ReboundRotationY.Random(randomizeSign: true), 0f);
		if (CollisionListener != null)
		{
			BallisticCollider component = collider.gameObject.GetComponent<BallisticCollider>();
			if (component != null)
			{
				CollisionListener.InvokeShellCollision(base.transform.position, component.GetSurfaceSound(base.transform.position), _caliber);
			}
			else
			{
				CollisionListener.InvokeShellCollision(base.transform.position, BaseBallistic.ESurfaceSound.Soil, _caliber);
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		base.transform.localRotation *= Quaternion.Euler(_E010 * (Time.deltaTime * 100f));
	}
}
