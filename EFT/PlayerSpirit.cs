using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT;

[DisallowMultipleComponent]
public class PlayerSpirit : PlayerSpiritBase
{
	protected SpriteRenderer _spiritDebugImage;

	protected readonly Vector3 _unseenPlace = new Vector3(0f, -1000f, 0f);

	protected readonly List<string> _slotIdsWithVisibleWeapons = new List<string>
	{
		EquipmentSlot.FirstPrimaryWeapon.ToString(),
		EquipmentSlot.SecondPrimaryWeapon.ToString(),
		EquipmentSlot.Holster.ToString()
	};

	private int _E01B;

	private const int _E01C = 10;

	public override void Init(Player player, Vector3 position, bool isBodyAnimatorUpdating, CharacterControllerSpawner.Mode characterControllerMode, CharacterControllerSpawner.Mode disconnectedCharacterControllerMode)
	{
		base.Init(player, position, isBodyAnimatorUpdating, characterControllerMode, disconnectedCharacterControllerMode);
		InitDebugImage();
	}

	public override void PlayerSync()
	{
		_E71E footprint = CreateFootprint();
		PlayerSync(footprint);
		HideUnusedPartOnNextFrame();
	}

	protected void InitDebugImage()
	{
		if ((_E2B6.Config.ShowSpiritImage || _E2B6.Config.UseSpiritHack) && _player is ObservedPlayer)
		{
			GameObject gameObject = new GameObject(_ED3E._E000(137766));
			gameObject.transform.SetParent(_bodyTransform, worldPositionStays: false);
			gameObject.transform.localPosition = new Vector3(0f, 1.1f, 0f);
			gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			_spiritDebugImage = gameObject.AddComponent<SpriteRenderer>();
			_spiritDebugImage.sprite = _E3A2.Load<Sprite>(_ED3E._E000(137820));
			gameObject.AddComponent<PrimitiveBillboard>();
			if (_E2B6.Config.UseSpiritHack)
			{
				_spiritDebugImage.color = new Color(1f, 1f, 1f, 0.01f);
			}
		}
	}

	public void ConnectToPlayerEvents()
	{
		_player._E0DE.AddItemEvent += InventoryControllerOnAddItemEvent;
	}

	public void RecheckSwitch()
	{
		Switch(base.IsActive);
	}

	public void Switch(bool toSpirit)
	{
		if (toSpirit)
		{
			toSpirit = CanSwitchToSpirit();
		}
		if (toSpirit != base.IsActive)
		{
			if (toSpirit)
			{
				TranslateStateToSpirit();
			}
			else
			{
				TranslateStateToPlayer();
			}
			SetActive(toSpirit);
			HideUnusedPart();
			_player.MouseLook(forceApplyToOriginalRibcage: true);
			if (!toSpirit)
			{
				_player.ProceduralWeaponAnimation.MotionReact.Reset();
			}
		}
	}

	protected void TranslateStateToPlayer()
	{
		TranslateAnimatorState(_player._animators[0], BodyAnimatorWrapper, _player.BodyUpdateMode);
		_player._characterController.CopyFields(base.CharacterController);
		_player.Transform.SyncOriginal();
		TranslateAnimatorState(_player._animators[1], ArmsAnimator, _player.ArmsUpdateMode);
		_player.MovementContext.SetDirectlyLookRotations(LookRotation, PreviousLookRotation);
	}

	protected void TranslateStateToSpirit()
	{
		TranslateAnimatorState(BodyAnimatorWrapper, _player._animators[0], _player.BodyUpdateMode);
		base.CharacterController.CopyFields(_player._characterController);
		_player.Transform.SyncImitators();
		TranslateAnimatorState(ArmsAnimator, _player._animators[1], _player.ArmsUpdateMode);
		LookRotation = _player.MovementContext.Rotation;
		PreviousLookRotation = _player.MovementContext.PreviousRotation;
	}

	protected void TranslateAnimatorState(IAnimator animatorRecipient, IAnimator animatorDonor, Player.EUpdateMode updateMode)
	{
		if (updateMode == Player.EUpdateMode.Auto)
		{
			animatorDonor.enabled = false;
		}
		TranslateAnimatorState(animatorRecipient, animatorDonor);
		if (updateMode == Player.EUpdateMode.Auto)
		{
			animatorRecipient.enabled = true;
		}
	}

	protected bool CanSwitchToSpirit()
	{
		if (HasActiveLightMode())
		{
			return false;
		}
		return true;
	}

	public void HideUnusedPart()
	{
		if (base.IsActive)
		{
			_player.PlayerBones.BodyTransform.Original.localPosition = _player.PlayerBones.BodyTransform.position + _unseenPlace;
		}
		else
		{
			_bodyTransform.localPosition = _player.PlayerBones.BodyTransform.position + _unseenPlace;
		}
	}

	public void HideUnusedPartOnNextFrame()
	{
		StartCoroutine(HideUnusedPartOnNextFrameWorker());
	}

	protected IEnumerator HideUnusedPartOnNextFrameWorker()
	{
		yield return null;
		HideUnusedPart();
	}

	protected bool HasActiveLightMode()
	{
		foreach (Slot item in _player._E0DE.Inventory.Equipment.AllSlots.Where((Slot slot) => _slotIdsWithVisibleWeapons.Contains(slot.ID)))
		{
			if (item.ContainedItem is Weapon weapon && HasActiveLightMode(weapon))
			{
				return true;
			}
		}
		return false;
	}

	protected bool HasActiveLightMode(Weapon weapon)
	{
		foreach (LightComponent component in weapon.AllSlots.Select((Slot slot) => slot.ContainedItem).GetComponents<LightComponent>())
		{
			if (component.IsActive)
			{
				return true;
			}
		}
		return false;
	}

	protected void InventoryControllerOnAddItemEvent(_EAF2 addItemEventArgs)
	{
		if (addItemEventArgs.Status == CommandStatus.Succeed)
		{
			bool flag = false;
			IContainer[] array = addItemEventArgs.Item.GetPathOfContainers().ToArray();
			if (array.Length >= 2 && array[1] is Slot slot && _slotIdsWithVisibleWeapons.Contains(slot.ID))
			{
				flag = true;
			}
			if (flag)
			{
				RecheckSwitch();
			}
		}
	}

	private void OnRenderObject()
	{
		if (_E2B6.Config == null || !_E2B6.Config.UseSpiritHack || Camera.current != _E8A8.Instance.Camera)
		{
			return;
		}
		if (_player is ObservedPlayer && base.IsActive && _spiritDebugImage.isVisible)
		{
			_E01B++;
			if (_E01B >= 10)
			{
				Switch(toSpirit: false);
				Debug.LogError(_ED3E._E000(137819) + _E01B + _ED3E._E000(137795));
				_E01B = 0;
			}
		}
		else
		{
			_E01B = 0;
		}
	}

	[CompilerGenerated]
	private bool _E000(Slot slot)
	{
		return _slotIdsWithVisibleWeapons.Contains(slot.ID);
	}
}
