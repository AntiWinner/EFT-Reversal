using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Systems.Effects;
using AnimationSystem.RootMotionTable;
using Audio.SpatialSystem;
using Bsg.GameSettings;
using Comfort.Common;
using CommonAssets.Scripts;
using Dissonance;
using Diz.Jobs;
using EFT.Animations;
using EFT.AssetsManager;
using EFT.Ballistics;
using EFT.ClientItems.ClientSpecItems;
using EFT.EnvironmentEffect;
using EFT.Game.Spawning;
using EFT.HealthSystem;
using EFT.InputSystem;
using EFT.Interactive;
using EFT.InventoryLogic;
using EFT.MovingPlatforms;
using EFT.PrefabSettings;
using EFT.UI;
using EFT.UI.Gestures;
using JetBrains.Annotations;
using NLog;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace EFT;

public class Player : MonoBehaviour, _E5B4, _E640, _E63F, _E641, _E64C, MovingPlatform._E000, IDissonancePlayer
{
	public class _E000 : _ECD1
	{
	}

	public class _E001 : _E000
	{
		public readonly string ItemControllerId;

		public _E001(string itemControllerId)
		{
			ItemControllerId = itemControllerId;
		}

		public override string ToString()
		{
			return _ED3E._E000(163103) + ItemControllerId;
		}
	}

	public class _E002 : _ECD1
	{
		public readonly Item Item;

		public readonly Type ComponentType;

		public _E002(Item item, Type componentType)
		{
			Item = item;
			ComponentType = componentType;
		}

		public override string ToString()
		{
			return string.Concat(Item, _ED3E._E000(163120), ComponentType.Name);
		}
	}

	public class _E003 : _ECD1
	{
		public readonly Item Item;

		public readonly ItemAddress ItemAddress;

		public _E003(Item item, ItemAddress itemAddress)
		{
			Item = item;
			ItemAddress = itemAddress;
		}

		public override string ToString()
		{
			return string.Concat(Item, _ED3E._E000(163154), ItemAddress, _ED3E._E000(163198), Item.Parent);
		}
	}

	public class _E004 : _ECD1
	{
		public readonly _EB19 Component;

		public readonly bool Value;

		public _E004(_EB19 component, bool value)
		{
			Component = component;
			Value = value;
		}

		public override string ToString()
		{
			return string.Concat(Component.GetType().Name, _ED3E._E000(163185), Component.Item, _ED3E._E000(163182), Value.ToString());
		}
	}

	protected internal class PlayerInventoryController : _EAED, _EAEA
	{
		private new sealed class _E000
		{
			private readonly _EA6A m__E000;

			private readonly float _E001;

			private readonly Callback _E002;

			private bool _E003;

			private Coroutine _E004;

			public _E000(_EA6A magazine, float duration, Callback callback)
			{
				this.m__E000 = magazine;
				_E001 = duration;
				_E002 = callback;
				if (_E004 != null)
				{
					StaticManager.KillCoroutine(_E004);
				}
				_E004 = StaticManager.BeginCoroutine(_E000());
			}

			public void Proceed()
			{
				if (_E004 != null)
				{
					if (_E003)
					{
						_E002.Succeed();
					}
					else
					{
						_E002.Fail(_ED3E._E000(163308));
					}
					StaticManager.KillCoroutine(_E004);
					_E004 = null;
				}
			}

			public void TryProceedForItem(_EA6A magazine)
			{
				if (this.m__E000 == magazine)
				{
					Proceed();
				}
			}

			private IEnumerator _E000()
			{
				_E003 = false;
				yield return new WaitForSeconds(_E001);
				_E003 = true;
				Proceed();
			}
		}

		private new sealed class _E001 : _EB5D
		{
			[CompilerGenerated]
			private sealed class _E000
			{
				public TaskCompletionSource<IResult> cancellationHandlerSource;

				internal void _E000()
				{
					cancellationHandlerSource.Succeed();
				}
			}

			[CompilerGenerated]
			private sealed class _E002
			{
				public TaskCompletionSource<IResult> executionSource;

				internal void _E000(IResult res)
				{
					executionSource.SetResult(res);
				}
			}

			private readonly _EAED m__E000;

			private readonly _EA6A m__E001;

			private readonly _EA12 m__E002;

			private readonly int m__E003;

			private readonly bool m__E004;

			private readonly float m__E005;

			private readonly IItemOwner _E006;

			private readonly IItemOwner _E007;

			private CancellationTokenSource _E008;

			private float _E009;

			private bool _E00A => _E008?.IsCancellationRequested ?? true;

			public _E001(_EAED inventoryController, _EA6A magazine, _EA12 sourceAmmo, int count, bool elite, float loadOneAmmoSpeed)
			{
				this.m__E000 = inventoryController;
				this.m__E001 = magazine;
				this.m__E002 = sourceAmmo;
				this.m__E003 = count;
				this.m__E004 = elite;
				this.m__E005 = loadOneAmmoSpeed;
				_E009 = loadOneAmmoSpeed;
				_E006 = this.m__E001.Parent.GetOwner();
				_E007 = this.m__E002.Parent.GetOwner();
			}

			public async Task<IResult> Start()
			{
				_E000();
				_E008 = new CancellationTokenSource();
				TaskCompletionSource<IResult> cancellationHandlerSource = new TaskCompletionSource<IResult>();
				_E008.Token.Register(delegate
				{
					cancellationHandlerSource.Succeed();
				});
				_E001(CommandStatus.Begin);
				IResult result = (await Task.WhenAny<IResult>(_E002(), cancellationHandlerSource.Task)).Result;
				Proceed(result.Succeed);
				return result;
			}

			public void Proceed(bool success = true)
			{
				CancellationTokenSource cancellationTokenSource = _E008;
				if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
				{
					_E000();
					_E001(success ? CommandStatus.Succeed : CommandStatus.Failed);
					_E003();
				}
			}

			private void _E000()
			{
				if (_E008 != null)
				{
					_E008.Cancel(throwOnFirstException: false);
					_E008.Dispose();
					_E008 = null;
				}
			}

			public void TryProceedForItem(Item item)
			{
				if (this.m__E001 == item || this.m__E002 == item)
				{
					Proceed();
				}
			}

			private void _E001(CommandStatus status)
			{
				_EAF7 args = new _EAF7(this.m__E002, this.m__E001, this.m__E003, this.m__E005, status);
				_E006.RaiseLoadMagazineEvent(args);
				if (_E006 != this.m__E000)
				{
					this.m__E000.RaiseLoadMagazineEvent(args);
				}
				if (_E007 != _E006)
				{
					_E007.RaiseLoadMagazineEvent(args);
				}
			}

			private async Task<IResult> _E002()
			{
				Task task = _E005();
				for (int i = 0; i < this.m__E003; i++)
				{
					await task;
					if (_E00A)
					{
						break;
					}
					task = _E005();
					if (this.m__E004)
					{
						_E009 = Mathf.Clamp(_E009 - this.m__E005 * Singleton<_E5CB>.Instance.LoadTimeSpeedProgress / 100f, this.m__E005 * 40f / 100f, 10f);
					}
					_ECD7 sourceOption = this.m__E001.ApplyWithoutRestrictions(this.m__E000, this.m__E002, 1, simulate: true);
					if (sourceOption.Failed)
					{
						return sourceOption.ToResult();
					}
					_EB73 operation = this.m__E000.ConvertOperationResultToOperation(sourceOption.Value);
					TaskCompletionSource<IResult> executionSource = new TaskCompletionSource<IResult>();
					this.m__E000.Execute(operation, delegate(IResult res)
					{
						executionSource.SetResult(res);
					});
					IResult result = await executionSource.Task;
					if (result.Failed)
					{
						return result;
					}
					_E004();
					_E003(i == this.m__E003 - 1);
					if (_E00A)
					{
						break;
					}
				}
				return SuccessfulResult.New;
			}

			private void _E003(bool refreshIcon = false)
			{
				if (this.m__E002.CurrentAddress != null)
				{
					this.m__E002.RaiseRefreshEvent(refreshIcon);
				}
				this.m__E001.RaiseRefreshEvent(refreshIcon);
			}

			private void _E004()
			{
				if (Singleton<GUISounds>.Instantiated)
				{
					Singleton<GUISounds>.Instance.PlayUILoadSound();
				}
			}

			[CompilerGenerated]
			private Task _E005()
			{
				return Task.Delay(Mathf.CeilToInt(_E009 * 1000f));
			}
		}

		private new sealed class _E002 : _EB5D
		{
			[CompilerGenerated]
			private sealed class _E000
			{
				public TaskCompletionSource<IResult> cancellationHandlerSource;

				internal void _E000()
				{
					cancellationHandlerSource.Succeed();
				}
			}

			[CompilerGenerated]
			private sealed class _E002
			{
				public TaskCompletionSource<IResult> executionSource;

				internal void _E000(IResult executeResult)
				{
					executionSource.SetResult(executeResult);
				}
			}

			private readonly _EAED m__E000;

			private readonly _EA6A m__E001;

			private readonly bool m__E002;

			private readonly float m__E003;

			private readonly int m__E004;

			private float _E005;

			private int _E006;

			private CancellationTokenSource _E007;

			private Item _E008;

			private Item _E009;

			private bool _E00A => _E007?.IsCancellationRequested ?? true;

			public _E002(_EAED inventoryController, _EA6A magazine, float loadOneAmmoSpeed, bool elite)
			{
				this.m__E000 = inventoryController;
				this.m__E001 = magazine;
				this.m__E003 = loadOneAmmoSpeed;
				this.m__E002 = elite;
				_E005 = loadOneAmmoSpeed;
				this.m__E004 = this.m__E001.Cartridges.Items.Sum((Item item) => item.StackObjectsCount);
				_E006 = this.m__E004;
			}

			public async Task<IResult> Start()
			{
				_E000();
				if (_E006 == 0)
				{
					return new _EA06(this.m__E001).ToResult();
				}
				_E007 = new CancellationTokenSource();
				TaskCompletionSource<IResult> cancellationHandlerSource = new TaskCompletionSource<IResult>();
				_E007.Token.Register(delegate
				{
					cancellationHandlerSource.Succeed();
				});
				IResult result = (await Task.WhenAny<IResult>(_E002(), cancellationHandlerSource.Task)).Result;
				Proceed(result.Succeed);
				return result;
			}

			private void _E000()
			{
				if (_E007 != null)
				{
					_E007.Cancel(throwOnFirstException: false);
					_E007.Dispose();
					_E007 = null;
				}
			}

			public void Proceed(bool success)
			{
				CancellationTokenSource cancellationTokenSource = _E007;
				if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
				{
					_E000();
					_E003(success ? CommandStatus.Succeed : CommandStatus.Failed);
				}
			}

			public void TryProceedForItem(Item item)
			{
				if (this.m__E001 == item || _E008 == item || _E009 == item)
				{
					Proceed(success: true);
				}
			}

			private void _E001()
			{
				if (Singleton<GUISounds>.Instantiated)
				{
					Singleton<GUISounds>.Instance.PlayUIUnloadSound();
				}
			}

			private async Task<IResult> _E002()
			{
				Task task = _E004();
				while (!_E00A)
				{
					_EA12 obj = (_EA12)this.m__E001.Cartridges.Items.LastOrDefault();
					if (obj == null)
					{
						break;
					}
					_ECD8<_EB2E> sourceOption = _EB29.QuickFindAppropriatePlace(obj, this.m__E000, this.m__E000.Inventory.Equipment.ToEnumerable(), _EB29.EMoveItemOrder.UnloadAmmo, simulate: true);
					if (sourceOption.Failed)
					{
						return sourceOption.ToResult();
					}
					ItemAddress itemAddress = null;
					Item item = null;
					_EB2E value = sourceOption.Value;
					if (value != null)
					{
						if (!(value is _EB31 obj2))
						{
							if (value is _EB32 obj3)
							{
								item = obj3.TargetItem;
							}
						}
						else
						{
							_EB31 obj4 = obj2;
							itemAddress = obj4.To;
						}
					}
					if (itemAddress == null && item == null)
					{
						break;
					}
					if (_E008 != obj || item != _E009)
					{
						if (_E008 != null)
						{
							_E003(CommandStatus.Succeed);
						}
						_E008 = obj;
						_E009 = item;
						_E003(CommandStatus.Begin);
					}
					await task;
					if (_E00A)
					{
						break;
					}
					task = _E004();
					if (this.m__E002)
					{
						_E005 = Mathf.Clamp(_E005 - this.m__E003 * Singleton<_E5CB>.Instance.LoadTimeSpeedProgress / 100f, this.m__E003 * 40f / 100f, 10f);
					}
					_ECD8<_EB41> sourceOption2 = ((_E009 != null) ? _EA12.ApplyToAmmo(_E008, _E009, 1, this.m__E000, simulate: true) : _EA12.ApplyToAddress(_E008, itemAddress, 1, this.m__E000, simulate: true));
					if (sourceOption2.Failed)
					{
						return sourceOption2.ToResult();
					}
					_EB40 operationResult = new _EB43(sourceOption2.Value);
					_EB8E operation = this.m__E000.ConvertOperationResultToOperation(operationResult) as _EB8E;
					TaskCompletionSource<IResult> executionSource = new TaskCompletionSource<IResult>();
					this.m__E000.Execute(operation, delegate(IResult executeResult)
					{
						executionSource.SetResult(executeResult);
					});
					IResult result = await executionSource.Task;
					if (result.Failed)
					{
						return result;
					}
					_E006--;
					_E008.RaiseRefreshEvent();
					this.m__E001.RaiseRefreshEvent(_E006 == 0);
					_E009?.RaiseRefreshEvent();
					_E001();
				}
				return SuccessfulResult.New;
			}

			private void _E003(CommandStatus status)
			{
				_EAF8 args = new _EAF8(_E008, _E009, this.m__E001, this.m__E004 - _E006, _E006, this.m__E003, status);
				IItemOwner owner = this.m__E001.Parent.GetOwner();
				owner.RaiseUnloadMagazineEvent(args);
				if (owner != this.m__E000)
				{
					this.m__E000.RaiseUnloadMagazineEvent(args);
				}
			}

			[CompilerGenerated]
			private Task _E004()
			{
				return Task.Delay(Mathf.CeilToInt(_E005 * 1000f));
			}
		}

		[CompilerGenerated]
		private new sealed class _E003
		{
			public PlayerInventoryController _003C_003E4__this;

			public bool ignoreRestrictions;

			internal bool _E000(ContainerCollection localItem)
			{
				if (!_003C_003E4__this.m__E005.Contains(localItem))
				{
					if (_003C_003E4__this._E001(localItem, ignoreRestrictions))
					{
						return localItem.NotShownInSlot;
					}
					return true;
				}
				return false;
			}
		}

		[CompilerGenerated]
		private new sealed class _E005
		{
			public PlayerInventoryController _003C_003E4__this;

			public IEnumerable<_EAE9> destroyedItems;

			internal void _E000(Item itemToSubtract)
			{
				if (itemToSubtract.LimitedDiscard)
				{
					string templateId = itemToSubtract.TemplateId;
					_ = _003C_003E4__this.DiscardLimits[templateId];
					int preservedNumber;
					int num = (_E001(itemToSubtract, out preservedNumber) ? preservedNumber : itemToSubtract.StackObjectsCount);
					if (num != 0)
					{
						_003C_003E4__this.DiscardLimits[templateId] -= num;
						_003C_003E4__this.LogDiscardLimitsChange(itemToSubtract.Template, -num);
					}
				}
			}

			internal bool _E001(Item localItem, out int preservedNumber)
			{
				foreach (var (item2, _, num3) in destroyedItems)
				{
					if (localItem != item2)
					{
						continue;
					}
					preservedNumber = num3;
					return true;
				}
				preservedNumber = 0;
				return false;
			}

			internal bool _E002(Item localItem)
			{
				foreach (var (item2, _, num3) in destroyedItems)
				{
					if (localItem != item2)
					{
						continue;
					}
					return num3 > 0;
				}
				return true;
			}
		}

		[CompilerGenerated]
		private new sealed class _E006
		{
			public IEnumerable<_EAE9> destroyedItems;

			public PlayerInventoryController _003C_003E4__this;

			internal void _E000(Item itemToAdd)
			{
				if (!itemToAdd.LimitedDiscard)
				{
					return;
				}
				int num = itemToAdd.StackObjectsCount;
				foreach (var (item2, _, num4) in destroyedItems)
				{
					if (item2 != itemToAdd)
					{
						continue;
					}
					num = num4;
					break;
				}
				if (num != 0)
				{
					_003C_003E4__this.DiscardLimits[itemToAdd.TemplateId] += num;
					_003C_003E4__this.LogDiscardLimitsChange(itemToAdd.Template, num);
				}
			}

			internal bool _E001(Item localItem)
			{
				foreach (var (item2, _, num3) in destroyedItems)
				{
					if (localItem != item2)
					{
						continue;
					}
					return num3 > 0;
				}
				return true;
			}
		}

		[CompilerGenerated]
		private new sealed class _E007
		{
			public PlayerInventoryController _003C_003E4__this;

			public _EA6A magazine;

			public bool notify;

			public float speed;

			internal void _E000(IResult result)
			{
				if (result.Succeed)
				{
					_003C_003E4__this.StrictCheckMagazine(magazine, status: true, _003C_003E4__this.Profile.MagDrillsMastering, notify);
					if (Singleton<GUISounds>.Instantiated)
					{
						Singleton<GUISounds>.Instance.PlayItemSound(magazine.ItemSound, EInventorySoundType.drop);
					}
				}
				else
				{
					UnityEngine.Debug.Log(_ED3E._E000(163302));
				}
				magazine.Parent.GetOwner().RaiseInventoryCheckMagazine(magazine, speed, status: false);
			}
		}

		[CompilerGenerated]
		private new sealed class _E008
		{
			public _EA12 containedAmmo;

			public TaskCompletionSource<IResult> taskSource;

			internal _EB22 _E000(_E9EF grid)
			{
				return grid.FindLocationForItem(containedAmmo);
			}

			internal void _E001(IResult result)
			{
				taskSource.SetResult(result);
			}
		}

		[CompilerGenerated]
		private new sealed class _E00A
		{
			public TaskCompletionSource<IResult> taskSource;

			internal void _E000(IResult result)
			{
				taskSource.SetResult(result);
			}

			internal void _E001(IResult result)
			{
				taskSource.SetResult(result);
			}
		}

		[CompilerGenerated]
		private sealed class _E00F
		{
			public Callback callback;

			internal void _E000(IResult result)
			{
				if (result.Failed)
				{
					UnityEngine.Debug.LogError(result.Error);
				}
				callback(result);
			}
		}

		[CompilerGenerated]
		private sealed class _E010
		{
			public PlayerInventoryController _003C_003E4__this;

			public Item item;

			public ItemAddress to;

			internal bool _E000(Item x)
			{
				return _003C_003E4__this.ItemInHands == x;
			}

			internal bool _E001(Item y)
			{
				return y == item;
			}

			internal bool _E002(Slot x)
			{
				return x == to.Container;
			}
		}

		internal new readonly Player _E000;

		private new _EB5D m__E001;

		private new _E000 m__E002;

		private new bool m__E003 = true;

		private new readonly Dictionary<string, int> m__E004 = new Dictionary<string, int>();

		private new readonly List<Item> m__E005 = new List<Item>();

		[CompilerGenerated]
		private new Dictionary<string, int> m__E006;

		protected Dictionary<string, int> DiscardLimits
		{
			[CompilerGenerated]
			get
			{
				return this.m__E006;
			}
			[CompilerGenerated]
			private set
			{
				this.m__E006 = value;
			}
		}

		protected virtual bool HasDiscardLimits => base.Profile.Info.Side != EPlayerSide.Savage;

		public override Item ItemInHands
		{
			get
			{
				if (!(this._E000.HandsController != null))
				{
					return null;
				}
				return this._E000.HandsController.Item;
			}
		}

		public override bool Locked
		{
			get
			{
				return this._E000.ProcessStatus != EProcessStatus.None;
			}
			set
			{
				this._E000.ProcessStatus = (value ? EProcessStatus.Internal : EProcessStatus.None);
				UpdateLockedStatus();
			}
		}

		public override Task<IResult> TryRunNetworkTransaction(_ECD7 operationResult, Callback callback = null)
		{
			if (this._E000.HealthController.IsAlive)
			{
				return base.TryRunNetworkTransaction(operationResult, callback);
			}
			TaskCompletionSource<IResult> taskCompletionSource = new TaskCompletionSource<IResult>();
			taskCompletionSource.Fail(_ED3E._E000(163220));
			return taskCompletionSource.Task;
		}

		internal PlayerInventoryController(Player player, Profile profile, bool examined)
			: base(profile, examined)
		{
			this._E000 = player;
			DiscardLimits = base.Profile.Inventory.DiscardLimits;
			base.RootItem.CurrentAddress = new _EB1F(this);
			if (base.QuestStashItem != null)
			{
				base.QuestStashItem.CurrentAddress = new _EB1F(this);
			}
			if (base.QuestRaidItem != null)
			{
				base.QuestRaidItem.CurrentAddress = new _EB1F(this);
			}
			if (base.Inventory.Stash != null)
			{
				base.Inventory.Stash.CurrentAddress = new _EB1F(this);
			}
		}

		public virtual bool HasDiscardLimit(Item item, out int limit)
		{
			if (HasDiscardLimits && item.LimitedDiscard)
			{
				limit = DiscardLimits[item.TemplateId];
				return true;
			}
			limit = 0;
			return false;
		}

		public virtual void ResetDiscardLimits()
		{
			if (HasDiscardLimits)
			{
				DiscardLimits = Singleton<_E63B>.Instance.GetDiscardLimits();
			}
		}

		public virtual IEnumerable<_EAE9> GetItemsOverDiscardLimit(Item item)
		{
			if (!HasDiscardLimits)
			{
				yield break;
			}
			foreach (var (key, value) in DiscardLimits)
			{
				this.m__E004[key] = value;
			}
			bool ignoreRestrictions = this._E000.HealthController?.IsAlive ?? false;
			this.m__E005.Clear();
			foreach (Item allItem in item.GetAllItems((ContainerCollection localItem) => !this.m__E005.Contains(localItem) && (!_E001(localItem, ignoreRestrictions) || localItem.NotShownInSlot)))
			{
				if (!_E001(allItem, ignoreRestrictions) && _E000(allItem, out var overLimit))
				{
					yield return overLimit;
				}
			}
		}

		private bool _E000(Item item, out _EAE9 overLimit)
		{
			overLimit = default(_EAE9);
			if (!item.LimitedDiscard)
			{
				return false;
			}
			string templateId = item.TemplateId;
			int stackObjectsCount = item.StackObjectsCount;
			if (!this.m__E004.TryGetValue(templateId, out var value))
			{
				Logger.LogError(string.Format(_ED3E._E000(163211), item));
				return false;
			}
			int num = value - stackObjectsCount;
			if (num >= 0)
			{
				this.m__E004[templateId] = num;
				return false;
			}
			this.m__E004[templateId] = 0;
			this.m__E005.Add(item);
			int num2 = Math.Min(-num, stackObjectsCount);
			overLimit = new _EAE9(item, num2, stackObjectsCount - num2);
			return true;
		}

		private bool _E001(Item item, bool ignoreRestrictions)
		{
			if (ignoreRestrictions || !(item.CurrentAddress?.Container is Slot slot) || !(slot.ParentItem is _EB0B))
			{
				return false;
			}
			if (item.TryGetItemComponent<UnlootableComponent>(out var component) && component.IsUnlootableFrom(slot))
			{
				return true;
			}
			if (!(item is ContainerCollection))
			{
				return false;
			}
			if (item.TryGetItemComponent<CantRemoveFromSlotsDuringRaidComponent>(out var component2))
			{
				return !component2.CanRemoveFromSlotDuringRaid(slot.ID);
			}
			return false;
		}

		public virtual void SubtractFromDiscardLimits(Item rootItem, IEnumerable<_EAE9> destroyedItems)
		{
			_E005 CS_0024_003C_003E8__locals0 = new _E005();
			CS_0024_003C_003E8__locals0._003C_003E4__this = this;
			CS_0024_003C_003E8__locals0.destroyedItems = destroyedItems;
			if (!HasDiscardLimits)
			{
				return;
			}
			foreach (Item allItem in rootItem.GetAllItems((Predicate<ContainerCollection>)delegate(Item localItem)
			{
				foreach (var (item2, _, num3) in CS_0024_003C_003E8__locals0.destroyedItems)
				{
					if (localItem == item2)
					{
						return num3 > 0;
					}
				}
				return true;
			}))
			{
				CS_0024_003C_003E8__locals0._E000(allItem);
			}
		}

		protected virtual void LogDiscardLimitsChange(ItemTemplate template, int delta)
		{
		}

		public virtual void AddDiscardLimits(Item rootItem, IEnumerable<_EAE9> destroyedItems)
		{
			_E006 CS_0024_003C_003E8__locals0 = new _E006();
			CS_0024_003C_003E8__locals0.destroyedItems = destroyedItems;
			CS_0024_003C_003E8__locals0._003C_003E4__this = this;
			if (!HasDiscardLimits)
			{
				return;
			}
			foreach (Item allItem in rootItem.GetAllItems((Predicate<ContainerCollection>)delegate(Item localItem)
			{
				foreach (var (item2, _, num3) in CS_0024_003C_003E8__locals0.destroyedItems)
				{
					if (localItem == item2)
					{
						return num3 > 0;
					}
				}
				return true;
			}))
			{
				CS_0024_003C_003E8__locals0._E000(allItem);
			}
		}

		protected override void OutProcess(_EB1E executor, Item item, ItemAddress from, ItemAddress to, _EB72 operation, Callback callback)
		{
			if (!executor.CheckTransferOwners(item, to, out var error))
			{
				callback.Fail(error.ToString());
			}
			else
			{
				_E006(item, to, operation, callback);
			}
		}

		protected override void InProcess(_EB1E executor, Item item, ItemAddress to, bool succeed, _EB72 operation, Callback callback)
		{
			if (!succeed)
			{
				callback.Succeed();
				return;
			}
			if (!executor.CheckTransferOwners(item, to, out var error))
			{
				callback.Fail(error.ToString());
				return;
			}
			_E005(item, to, operation, callback);
			this._E000.StatisticsManager.OnGrabLoot(item);
		}

		public void SetNextProcessLocked(bool status)
		{
			this.m__E003 = status;
		}

		public override void InventoryCheckMagazine(_EA6A magazine, bool notify)
		{
			StopProcesses();
			float num = 100f - (float)base.Profile.Skills.MagDrillsInventoryCheckSpeed + magazine.CheckTimeModifier;
			float speed = Singleton<_E5CB>.Instance.BaseCheckTime * num / 100f;
			UnityEngine.Debug.Log(_ED3E._E000(163235) + speed + _ED3E._E000(163322));
			magazine.Parent.GetOwner().RaiseInventoryCheckMagazine(magazine, speed, status: true);
			this.m__E002 = new _E000(magazine, speed, delegate(IResult result)
			{
				if (result.Succeed)
				{
					StrictCheckMagazine(magazine, status: true, base.Profile.MagDrillsMastering, notify);
					if (Singleton<GUISounds>.Instantiated)
					{
						Singleton<GUISounds>.Instance.PlayItemSound(magazine.ItemSound, EInventorySoundType.drop);
					}
				}
				else
				{
					UnityEngine.Debug.Log(_ED3E._E000(163302));
				}
				magazine.Parent.GetOwner().RaiseInventoryCheckMagazine(magazine, speed, status: false);
			});
		}

		public override async Task<IResult> LoadMultiBarrelWeapon(Weapon weapon, _EA12 ammo, int ammoCount)
		{
			if (!(this._E000.HandsController is _E6CB obj) || obj.Item != weapon)
			{
				return await base.LoadMultiBarrelWeapon(weapon, ammo, ammoCount);
			}
			if (!obj.CanStartReload())
			{
				return new FailedResult(_ED3E._E000(163365));
			}
			if (!weapon.IsMultiBarrel)
			{
				return new FailedResult(_ED3E._E000(163418));
			}
			TaskCompletionSource<IResult> taskSource = new TaskCompletionSource<IResult>();
			_EA12 containedAmmo;
			_EB22 placeToPutContainedAmmoMagazine = (((containedAmmo = weapon.FirstFreeChamberSlot.ContainedItem as _EA12) != null && !containedAmmo.IsUsed) ? (from grid in base.Inventory.Equipment.GetPrioritizedGridsForUnloadedObject()
				select grid.FindLocationForItem(containedAmmo) into g
				where g != null
				orderby g.Grid.GridWidth.Value * g.Grid.GridHeight.Value
				select g).FirstOrDefault((_EB22 x) => x != null) : null);
			obj.ReloadBarrels(new _E9CF(new List<_EA12> { ammo }), placeToPutContainedAmmoMagazine, delegate(IResult result)
			{
				taskSource.SetResult(result);
			});
			return await taskSource.Task;
		}

		public override async Task<IResult> LoadWeaponWithAmmo(Weapon weapon, _EA12 ammo, int ammoCount)
		{
			if (!(this._E000.HandsController is _E6CB obj) || obj.Item != weapon)
			{
				return await base.LoadWeaponWithAmmo(weapon, ammo, ammoCount);
			}
			if (!obj.CanStartReload())
			{
				return new FailedResult(_ED3E._E000(163365));
			}
			if (!weapon.SupportsInternalReload)
			{
				return new FailedResult(_ED3E._E000(163436));
			}
			TaskCompletionSource<IResult> taskSource = new TaskCompletionSource<IResult>();
			if (weapon is _EAD1)
			{
				obj.ReloadRevolverDrum(new _E9CF(new List<_EA12> { ammo }), delegate(IResult result)
				{
					taskSource.SetResult(result);
				}, quickReload: false);
			}
			else
			{
				obj.ReloadWithAmmo(new _E9CF(new List<_EA12> { ammo }), delegate(IResult result)
				{
					taskSource.SetResult(result);
				});
			}
			return await taskSource.Task;
		}

		public override void StrictCheckMagazine(_EA6A magazine, bool status, int skill = 0, bool notify = false, bool useOperation = true)
		{
			if (status)
			{
				if (magazine.Count <= 0 || magazine.Count >= magazine.MaxCount)
				{
					skill = 2;
				}
				if (notify && !base.Profile.CheckedMagazines.ContainsKey(magazine.Id))
				{
					NotifyMagazineChecked(magazine.ShortName);
				}
			}
			_E001(magazine, status, skill, useOperation);
		}

		protected override async Task<IResult> LoadMagazine(_EA12 sourceAmmo, _EA6A magazine, int loadCount, bool ignoreRestrictions)
		{
			if (loadCount <= 0)
			{
				return new FailedResult(_ED3E._E000(163464));
			}
			StopProcesses();
			float num = 100f - (float)base.Profile.Skills.MagDrillsLoadSpeed + magazine.LoadUnloadModifier;
			float num2 = Singleton<_E5CB>.Instance.BaseLoadTime * num / 100f;
			UnityEngine.Debug.LogFormat(_ED3E._E000(163504), num2);
			_ECD7 sourceOption = (ignoreRestrictions ? magazine.ApplyWithoutRestrictions(this, sourceAmmo, 1, simulate: true) : magazine.Apply(this, sourceAmmo, 1, simulate: true));
			if (sourceOption.Failed || !CanExecute(sourceOption.Value))
			{
				return sourceOption.ToResult();
			}
			IResult result = await _E002();
			if (result.Failed)
			{
				return result;
			}
			this.m__E001 = new _E001(this, magazine, sourceAmmo, loadCount, base.Profile.Skills.MagDrillsLoadProgression, num2);
			IResult result2 = await this.m__E001.Start();
			this.m__E001 = null;
			return result2;
		}

		public override async Task<IResult> UnloadMagazine(_EA6A magazine)
		{
			StopProcesses();
			float num = 100f - (float)base.Profile.Skills.MagDrillsUnloadSpeed + magazine.LoadUnloadModifier;
			float num2 = Singleton<_E5CB>.Instance.BaseUnloadTime * num / 100f;
			UnityEngine.Debug.LogFormat(_ED3E._E000(163522), num2);
			IResult result = await _E002();
			if (result.Failed)
			{
				return result;
			}
			this.m__E001 = new _E002(this, magazine, num2, elite: false);
			IResult result2 = await this.m__E001.Start();
			this.m__E001 = null;
			return result2;
		}

		private async Task<IResult> _E002()
		{
			await TasksExtensions.WaitUntil(() => this.m__E001 == null);
			IResult result;
			if (!this.m__E003)
			{
				result = SuccessfulResult.New;
			}
			else
			{
				IResult result2 = new FailedResult(_ED3E._E000(163598));
				result = result2;
			}
			return result;
		}

		public override void StopProcesses()
		{
			this.m__E001?.Proceed();
			this.m__E002?.Proceed();
		}

		private void _E003(Item magazineOrAmmo)
		{
			this.m__E001?.TryProceedForItem(magazineOrAmmo);
		}

		private void _E004(_EA6A magazine)
		{
			this.m__E002?.TryProceedForItem(magazine);
		}

		public override void ThrowItem(Item item, IEnumerable<_EAE9> destroyedItems, Callback callback = null, bool downDirection = false)
		{
			if (item is Weapon weapon)
			{
				CheckChamber(weapon, status: false);
				_EA6A currentMagazine = weapon.GetCurrentMagazine();
				if (currentMagazine != null)
				{
					StrictCheckMagazine(currentMagazine, status: false);
					_E004(currentMagazine);
					_E003(currentMagazine);
				}
			}
			if (item is ContainerCollection topLevelCollection)
			{
				foreach (Item item2 in topLevelCollection.GetAllItemsFromCollection())
				{
					if (item2 is _EA6A obj)
					{
						_E004(obj);
						_E003(obj);
					}
					if (item2 is _EA12 magazineOrAmmo)
					{
						_E003(magazineOrAmmo);
					}
				}
			}
			Execute(new _EB9B(_E026++, this, item, destroyedItems, this._E000, downDirection), callback);
		}

		protected override _EB9C ToggleItem(TogglableComponent togglable)
		{
			if (this._E000.HealthController.FindActiveEffect<_E9BF>() != null || !(Time.time - togglable.LastToggleTime > 2f))
			{
				return base.ToggleItem(togglable);
			}
			togglable.LastToggleTime = Time.time;
			return new _EB9D(_E026++, this, togglable, this._E000);
		}

		public override void SetupItem(Item item, string zone, Vector3 position, Quaternion rotation, float setupTime, Callback callback = null)
		{
			Execute(new _EB95(_E026++, this, item, zone, position, rotation, this._E000, setupTime), callback);
		}

		public override void CheckMagazineAmmoDepend(_EA6A magazine, Action callback, bool useOperation, bool allowUncheck = false)
		{
			if (magazine.Count > 0 && magazine.Count < magazine.MaxCount)
			{
				if (allowUncheck && this._E000.Profile.CheckedMagazines.ContainsKey(magazine.Id))
				{
					StrictCheckMagazine(magazine, status: false, 0, notify: false, useOperation);
				}
			}
			else if (!this._E000.Profile.CheckedMagazines.ContainsKey(magazine.Id))
			{
				StrictCheckMagazine(magazine, status: true, 2, notify: false, useOperation);
			}
			callback();
		}

		public override bool CheckItemAction(Item item, ItemAddress location)
		{
			if (!IsInventoryBlocked())
			{
				return base.CheckItemAction(item, location);
			}
			return false;
		}

		private void _E005(Item item, ItemAddress to, _EB72 operation, Callback callback)
		{
			this._E000.TrySetInHands(item, to, operation, delegate(IResult result)
			{
				if (result.Failed)
				{
					UnityEngine.Debug.LogError(result.Error);
				}
				callback(result);
			});
		}

		private void _E006(Item item, ItemAddress to, _EB72 abstractOperation, Callback callback)
		{
			Item item2 = _E007(item, to);
			if (item2 != null)
			{
				this._E000.TryRemoveFromHands(item2, abstractOperation, callback);
			}
			else
			{
				callback.Succeed();
			}
		}

		internal override bool _E01E(_EB73 operation)
		{
			return this._E000.HandsController.CanExecute(operation);
		}

		private Item _E007(Item item, ItemAddress to)
		{
			List<Item> list = new List<Item> { item };
			if (item is ContainerCollection containerCollection)
			{
				list.AddRange(containerCollection.Containers.SelectMany((IContainer x) => x.Items));
			}
			Item item2 = list.FirstOrDefault((Item x) => ItemInHands == x);
			if (item2 == null && ItemInHands is Weapon weapon)
			{
				item2 = (from x in weapon.AllSlots
					where x.ContainedItem != null
					select x.ContainedItem).FirstOrDefault((Item y) => y == item);
				if (item2 == null && to != null && weapon.AllSlots.Any((Slot x) => x == to.Container))
				{
					item2 = item;
				}
			}
			if (item2 == null && to == null)
			{
				item2 = (this._E000._inventoryController.IsInAnimatedSlot(item) ? item : null);
			}
			return item2;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private Task<IResult> _E008(Weapon weapon, _EA12 ammo, int ammoCount)
		{
			return base.LoadMultiBarrelWeapon(weapon, ammo, ammoCount);
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private Task<IResult> _E009(Weapon weapon, _EA12 ammo, int ammoCount)
		{
			return base.LoadWeaponWithAmmo(weapon, ammo, ammoCount);
		}

		[CompilerGenerated]
		private bool _E00A()
		{
			return this.m__E001 == null;
		}
	}

	public static class _E005
	{
		public const float SPEED_MIN = 0f;

		public const float SPEED_MAX = 0.7f;

		public const float MAX_SPRINTING_SPEED = 2f;

		public const float SPEED_MAX_DELTA = 0.3f;

		public static readonly int FIRST_PERSON_ACTION = Animator.StringToHash(_ED3E._E000(130762));

		public static readonly Vector2 STAND_POSE_ROTATION_PITCH_RANGE = new Vector2(-90f, 90f);

		public static readonly Vector2 PRONE_POSE_ROTATION_PITCH_RANGE = new Vector2(-16f, 25f);

		public static readonly Vector2 ROLL_POSE_ROTATION_PITCH_RANGE = new Vector2(-16f, 2f);

		public static readonly Vector2 FULL_YAW_RANGE = new Vector2(-360f, 360f);

		public const float POSE_RANGE_MIN = 0f;

		public const float POSE_RANGE_MAX = 1f;

		public const float POSE_THRESHOLD = 0.5f;

		public const float TILT_RANGE_MIN = -5f;

		public const float TILT_RANGE_MAX = 5f;

		public const int STEP_RANGE_MIN = -1;

		public const int STEP_RANGE_MAX = 1;
	}

	public enum EMouseSensitivityModifier
	{
		Armor
	}

	public enum LeanType
	{
		NormalLean,
		SlowLean
	}

	public enum ESpeedLimit
	{
		BarbedWire,
		HealthCondition,
		Aiming,
		Weight,
		SurfaceNormal,
		Swamp,
		Shot,
		Armor,
		Fall
	}

	[Serializable]
	public class ValueBlender
	{
		protected float _target;

		protected float _startTime;

		protected float _startValue;

		public float Speed = 1f;

		public virtual float Target
		{
			get
			{
				return _target;
			}
			set
			{
				if (Target != value)
				{
					_startValue = Value;
					_startTime = Time.time;
					_target = value;
				}
			}
		}

		public virtual float Value
		{
			get
			{
				return Mathf.Clamp01(_startValue + (Time.time - _startTime) * Speed * Mathf.Sign(_target - 0.5f));
			}
			set
			{
				_startTime = Time.time;
				_startValue = value;
			}
		}

		public ValueBlender(int defaultValue = 0)
		{
			_target = defaultValue;
			_startValue = defaultValue;
		}
	}

	[Serializable]
	public class BetterValueBlender : ValueBlender
	{
		public override float Value
		{
			get
			{
				float num = Mathf.Max(0f, Time.time - _startTime);
				if (!(_startValue > Target))
				{
					return Mathf.Clamp(_startValue + num * Speed * Mathf.Sign(Target - _startValue), _startValue, Target);
				}
				return Mathf.Clamp(_startValue + num * Speed * Mathf.Sign(Target - _startValue), Target, _startValue);
			}
			set
			{
				_startValue = value;
				_target = value;
				_startTime = Time.time;
			}
		}

		public void ChangeValue(float value, float delay)
		{
			_startTime = Time.time + delay;
			_startValue = value;
		}
	}

	[Serializable]
	public class ValueBlenderDelay : ValueBlender
	{
		public float Delay;

		public override float Target
		{
			get
			{
				return _target;
			}
			set
			{
				if (Target != value)
				{
					_startValue = Value;
					_startTime = Time.time + Delay;
					_target = value;
				}
			}
		}
	}

	public delegate void _E006(float damage, EBodyPart part, EDamageType type, float absorbed, MaterialType special);

	private class _E007
	{
		public Vector3 Shift;

		public Transform Transform;

		public CommonTransportee Transportee;

		public void RemovePhysics()
		{
			if ((bool)Transportee)
			{
				UnityEngine.Object.Destroy(Transportee);
			}
		}

		public void RestoreShift()
		{
			foreach (Transform item in Transform)
			{
				item.localPosition += Shift;
			}
		}

		public void Destroy()
		{
			RemovePhysics();
			RestoreShift();
			Transform = null;
			Transportee = null;
		}
	}

	internal class EmptyHandsController : ItemHandsController, _E6C9, _E6C7, _E6C8
	{
		public new class _E000 : _E001
		{
			private new Callback m__E000;

			public _E000(EmptyHandsController controller)
				: base(controller)
			{
			}

			public virtual void Start(Item item, Callback callback)
			{
				m__E000 = callback;
				Start();
				_E002.SetInventoryOpened(opened: false);
				_E002.m__E001.SetInteract(p: true, 300);
			}

			public override void Reset()
			{
				m__E000 = null;
				base.Reset();
			}

			public override void OnBackpackDrop()
			{
				State = EOperationState.Finished;
				_E002.m__E001.SetInteract(p: false, 300);
				_E002.m__E001.SetInventory(_E002.m__E002);
				_E326.ResetTriggerHandReady(_E002.m__E001.Animator);
				_E002.InitiateOperation<_E002>().Start();
				m__E000.Succeed();
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.m__E002 = opened;
			}
		}

		public new abstract class _E001 : _E00E
		{
			protected new readonly Player _E001;

			protected readonly EmptyHandsController _E002;

			protected _E001(EmptyHandsController controller)
				: base(controller)
			{
				_E002 = controller;
				_E001 = _E002._player;
			}

			public new void Start()
			{
				base.Start();
			}

			public virtual void HideWeaponComplete()
			{
				_E000();
			}

			public virtual void WeaponAppeared()
			{
				_E000();
			}

			public virtual void OnBackpackDrop()
			{
				_E000();
			}

			public virtual void HideWeapon(Action onHidden)
			{
				_E000();
			}

			public virtual void ExamineWeapon()
			{
				_E000();
			}

			public virtual void SetEmptyHandsCompassState(bool active)
			{
				_E000();
			}

			public virtual void FastForward()
			{
			}

			public virtual void SetInventoryOpened(bool opened)
			{
				_E002.m__E002 = opened;
				_E002.m__E001.SetInventory(opened);
			}

			public virtual bool CanExecute(_EB72 operation)
			{
				if (!(operation is _EB75 obj))
				{
					return true;
				}
				if (obj.From1 != null && _E002._player._E0DE.IsInAnimatedSlot(obj.Item1))
				{
					return false;
				}
				return true;
			}

			public virtual void Execute(_EB72 operation, Callback callback)
			{
				_E000();
				if (!(operation is _EB75 obj))
				{
					callback.Succeed();
				}
				else if (obj.From1 != null && _E002._player._E0DE.IsInAnimatedSlot(obj.Item1))
				{
					callback?.Fail(string.Format(_ED3E._E000(163626), GetType()));
				}
				else
				{
					callback.Succeed();
				}
			}
		}

		public new class _E002 : _E001
		{
			private const float _E003 = 300f;

			private float _E004;

			public _E002(EmptyHandsController controller)
				: base(controller)
			{
			}

			public new void Start()
			{
				base.Start();
				_E004 = 0f;
			}

			public override void Reset()
			{
				_E004 = 0f;
				base.Reset();
			}

			public override void HideWeapon(Action onHidden)
			{
				State = EOperationState.Finished;
				_E002.InitiateOperation<_E003>().Start(onHidden);
			}

			public override void OnEnd()
			{
				_E002.SetCompassState(active: false);
			}

			public override bool CanExecute(_EB72 operation)
			{
				return true;
			}

			public override void Execute(_EB72 operation, Callback callback)
			{
				if (operation != null && operation is _EB75 obj)
				{
					_EB75 obj2 = obj;
					if (_E002._player._E0DE.IsInAnimatedSlot(obj2.Item1) && obj2.From1 != null && !obj2.From1.Equals(obj2.To1))
					{
						State = EOperationState.Finished;
						_E002.InitiateOperation<_E000>().Start(obj2.Item1, callback);
						return;
					}
				}
				callback.Succeed();
			}

			public override void Update(float deltaTime)
			{
				_E004 += deltaTime;
				if (_E004 > 300f)
				{
					_E002.m__E001.Idle();
					_E004 = 0f;
				}
			}

			public override void ExamineWeapon()
			{
				_E002.m__E001.LookTrigger();
			}

			public override void SetEmptyHandsCompassState(bool active)
			{
				_E002.CompassState.Value = active;
			}
		}

		private new sealed class _E003 : _E001
		{
			private Action _E005;

			public _E003(EmptyHandsController controller)
				: base(controller)
			{
			}

			public void Start(Action onHidden)
			{
				_E005 = onHidden;
				Start();
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				HideWeaponComplete();
			}

			public override void Reset()
			{
				_E005 = null;
				base.Reset();
			}

			public override bool CanExecute(_EB72 operation)
			{
				return true;
			}

			public override void Execute(_EB72 operation, Callback callback)
			{
				if (operation != null && operation is _EB75 obj)
				{
					_EB75 obj2 = obj;
					if (_E002._player._E0DE.IsInAnimatedSlot(obj2.Item1) && obj2.From1 != null && !obj2.From1.Equals(obj2.To1))
					{
						State = EOperationState.Finished;
						_E002.InitiateOperation<_E000>().Start(obj2.Item1, callback);
						return;
					}
				}
				callback.Succeed();
			}

			public override void HideWeaponComplete()
			{
				State = EOperationState.Finished;
				_E005();
			}

			public override void HideWeapon(Action onHidden)
			{
				_E005 = (Action)Delegate.Combine(_E005, onHidden);
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					HideWeaponComplete();
				}
			}
		}

		public sealed class _E004 : _E000
		{
			private const float _E006 = 0.01f;

			private float _E007;

			private bool _E008;

			public _E004(EmptyHandsController controller)
				: base(controller)
			{
			}

			public override void Start(Item item, Callback callback)
			{
				_E007 = 0f;
				_E008 = false;
				base.Start(item, callback);
			}

			public override void FastForward()
			{
				if (!_E008)
				{
					_E008 = true;
					OnBackpackDrop();
				}
			}

			public override void Update(float deltaTime)
			{
				base.Update(deltaTime);
				if (!_E008 && _E007 > 0.01f)
				{
					_E008 = true;
					OnBackpackDrop();
				}
				_E007 += deltaTime;
			}
		}

		public sealed class _E005 : _E001
		{
			private Action _E009;

			private bool _E00A;

			private Action _E00B;

			public _E005(EmptyHandsController controller)
				: base(controller)
			{
			}

			public void Start(Action callback)
			{
				_E00B = callback;
				_E00A = false;
				_E001.BodyAnimatorCommon.SetFloat(_E712.WEAPON_SIZE_MODIFIER_PARAM_HASH, 1f);
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				Start();
				_E002.m__E001.SetActiveParam(active: true);
			}

			public override void Reset()
			{
				base.Reset();
				_E00B = null;
				_E009 = null;
				_E00A = false;
			}

			public override void WeaponAppeared()
			{
				_E002.SetupProp();
				State = EOperationState.Finished;
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 0f);
				if (!_E00A)
				{
					_E002.InitiateOperation<_E002>().Start();
				}
				else
				{
					_E002.InitiateOperation<_E003>().Start(_E009);
				}
				Action action = _E00B;
				_E00B = null;
				action?.Invoke();
			}

			public override void HideWeapon(Action onHidden)
			{
				_E009 = onHidden;
				_E00A = true;
				WeaponAppeared();
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					WeaponAppeared();
				}
			}
		}

		[CompilerGenerated]
		private sealed class _E007<_E077> where _E077 : EmptyHandsController
		{
			public _E077 controller;

			internal void _E000()
			{
				((EmptyHandsController)controller).m__E001.RemoveEventsConsumer(controller);
			}
		}

		[CompilerGenerated]
		private sealed class _E008
		{
			public EmptyHandsController _003C_003E4__this;

			public Action callback;

			internal void _E000()
			{
				_003C_003E4__this._player.StartCoroutine(_003C_003E4__this._E006(callback));
			}
		}

		[CompilerGenerated]
		private sealed class _E009
		{
			public Action callback;

			internal void _E000()
			{
				callback();
			}
		}

		private new _E6FB m__E000;

		private new FirearmsAnimator m__E001;

		private new bool m__E002;

		protected new _E001 _E003 => base.CurrentHandsOperation as _E001;

		protected virtual bool _E004 => true;

		public override FirearmsAnimator FirearmsAnimator => this.m__E001;

		public override string LoggerDistinctId => string.Format(_ED3E._E000(163638), _player.ProfileId, _player.Profile.Info.Nickname, this);

		public new _EB15 Item => base.Item as _EB15;

		protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
		{
			return new Dictionary<Type, OperationFactoryDelegate>
			{
				{
					typeof(_E005),
					() => new _E005(this)
				},
				{
					typeof(_E002),
					() => new _E002(this)
				},
				{
					typeof(_E003),
					() => new _E003(this)
				},
				{
					typeof(_E000),
					() => new _E000(this)
				}
			};
		}

		public override float GetAnimatorFloatParam(int hash)
		{
			return this.m__E001.GetAnimatorParameter(hash);
		}

		public override bool SupportPickup()
		{
			return true;
		}

		public override void Pickup(bool p)
		{
			this.m__E001.SetPickup(p);
		}

		public override void Interact(bool isInteracting, int actionIndex)
		{
			this.m__E001.SetInteract(isInteracting, actionIndex);
		}

		public override void SetInventoryOpened(bool opened)
		{
			if (opened)
			{
				SetCompassState(active: false);
			}
			this._E003.SetInventoryOpened(opened);
			_player.CurrentState.OnInventory(opened);
		}

		public override void Loot(bool p)
		{
			this.m__E001.SetLooting(p);
		}

		public override bool IsInInteraction()
		{
			return this.m__E001.IsInInteraction;
		}

		public override bool IsInInteractionStrictCheck()
		{
			if (!IsInInteraction() && !(this.m__E001.GetLayerWeight(this.m__E001.LACTIONS_LAYER_INDEX) >= float.Epsilon))
			{
				return this.m__E001.Animator.IsInTransition(this.m__E001.LACTIONS_LAYER_INDEX);
			}
			return true;
		}

		public override void Destroy()
		{
			SetPropVisibility(isVisible: false);
			_player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			base.Destroy();
			this.m__E001 = null;
			AssetPoolObject.ReturnToPool(_controllerObject.gameObject);
		}

		public override bool CanExecute(_EB72 operation)
		{
			return this._E003.CanExecute(operation);
		}

		public override void Execute(_EB72 operation, Callback callback)
		{
			this._E003.Execute(operation, callback);
		}

		public virtual void ExamineWeapon()
		{
			this._E003.ExamineWeapon();
		}

		public override void SetCompassState(bool active)
		{
			if (CanChangeCompassState(active))
			{
				this._E003.SetEmptyHandsCompassState(active);
			}
		}

		public override bool CanRemove()
		{
			return true;
		}

		public override void ShowGesture(EGesture gesture)
		{
			if (gesture != 0)
			{
				this.m__E001.Gesture(gesture);
			}
		}

		internal static _E077 _E000<_E077>(Player player) where _E077 : EmptyHandsController
		{
			_EB15 item = new _EB15(Guid.NewGuid().ToString(), new _EB14());
			_E077 val = ItemHandsController._E000<_E077>(player, item);
			_E002(val, player);
			return val;
		}

		internal static async Task<_E077> _E001<_E077>(Player player) where _E077 : EmptyHandsController
		{
			_EB15 item = new _EB15(Guid.NewGuid().ToString(), new _EB14());
			_E077 obj = await ItemHandsController._E002<_E077>(player, item);
			_E002(obj, player);
			return obj;
		}

		private static void _E002<_E077>(_E077 controller, Player player) where _E077 : EmptyHandsController
		{
			WeaponPrefab componentInChildren = controller._controllerObject.GetComponentInChildren<WeaponPrefab>();
			((EmptyHandsController)controller).m__E000 = componentInChildren.ObjectInHands;
			controller._controllerObject.transform.SetParent(player.PlayerBones.WeaponRoot.Original.parent);
			player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			player.ProceduralWeaponAnimation.InitTransforms(controller.HandsHierarchy);
			((EmptyHandsController)controller).m__E000.AfterGetFromPoolInit(controller._player.ProceduralWeaponAnimation, null, player.IsYourPlayer);
			((EmptyHandsController)controller).m__E001 = componentInChildren.FirearmsAnimator;
			((EmptyHandsController)controller).m__E001.AddEventsConsumer(controller);
			controller.CompositeDisposable.AddDisposable(delegate
			{
				((EmptyHandsController)controller).m__E001.RemoveEventsConsumer(controller);
			});
			controller._player.HandsAnimator = ((EmptyHandsController)controller).m__E001;
		}

		protected override void IEventsConsumerOnWeapIn()
		{
			_E004();
		}

		protected override void IEventsConsumerOnWeapOut()
		{
			_E003();
		}

		protected override void IEventsConsumerOnThirdAction(int intParam)
		{
			TranslateAnimatorParameter(intParam);
		}

		protected override void IEventsOnBackpackDrop()
		{
			_E005();
		}

		protected override void IEventsConsumerOnOnUseProp(bool boolParam)
		{
			SetPropVisibility(boolParam);
		}

		public override bool IsInventoryOpen()
		{
			return _objectInHandsAnimator.IsInInventory;
		}

		public override void FastForwardCurrentState()
		{
			this._E003.FastForward();
		}

		private void _E003()
		{
			this._E003.HideWeaponComplete();
		}

		private void _E004()
		{
			this._E003.WeaponAppeared();
		}

		private void _E005()
		{
			this._E003.OnBackpackDrop();
		}

		public override void Spawn(float animationSpeed, Action callback)
		{
			this.m__E001.SetAnimationSpeed(animationSpeed);
			Action callback2 = delegate
			{
				_player.StartCoroutine(_E006(callback));
			};
			InitiateOperation<_E005>().Start(callback2);
		}

		public override void ManualUpdate(float deltaTime)
		{
			base.ManualUpdate(deltaTime);
			this.m__E001?.SetAimAngle(_player.Pitch);
		}

		public override void Drop(float animationSpeed, Action callback, bool fastDrop, Item nextControllerItem = null)
		{
			if (base.Destroyed)
			{
				this._E003.HideWeapon(callback);
				return;
			}
			base.Destroyed = true;
			Action onHidden = delegate
			{
				callback();
			};
			this._E003.HideWeapon(onHidden);
		}

		private IEnumerator _E006(Action callback)
		{
			while (!this._E004)
			{
				yield return null;
			}
			callback();
		}

		[CompilerGenerated]
		private _E00E _E007()
		{
			return new _E005(this);
		}

		[CompilerGenerated]
		private _E00E _E008()
		{
			return new _E002(this);
		}

		[CompilerGenerated]
		private _E00E _E009()
		{
			return new _E003(this);
		}

		[CompilerGenerated]
		private _E00E _E00A()
		{
			return new _E000(this);
		}
	}

	public class FirearmController : ItemHandsController, _E6CB, _E6CA, _E6C7, _E6C8
	{
		public new class _E000 : _E00E
		{
			private _EA12 _E00C;

			private ItemAddress _E00D;

			private new Callback m__E000;

			private bool _E00E;

			private bool _E00F;

			private int _E010 = -1;

			public _E000(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(_EA12 item, int camoraIndex, ItemAddress itemAddress, Callback callback)
			{
				_E00C = item;
				this.m__E000 = callback;
				_E010 = camoraIndex;
				_E00D = itemAddress;
				Start();
				_E002.IsAiming = false;
				_E037.Discharge(discharge: true);
				_E037.SetFire(fire: false);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}

			public override void Reset()
			{
				base.Reset();
				_E00C = null;
				this.m__E000 = null;
				_E00E = false;
				_E00F = false;
				_E010 = -1;
				_E00D = null;
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					OnMagAppeared();
					OnMagPuttedToRig();
				}
			}

			public override void RemoveAmmoFromChamber()
			{
				_E002.m__E01F.DestroyPatronInWeapon();
			}

			public override void OnMagAppeared()
			{
				if (!_E00E)
				{
					_E00E = true;
					_E037.Discharge(discharge: false);
					_E037.SetShellsInWeapon(_E002.UnderbarrelWeapon.ShellsInLauncherCount);
					_E037.SetCanReload(canReload: false);
				}
			}

			public override void OnMagPuttedToRig()
			{
				if (!_E00F)
				{
					_E00F = true;
					_E000();
				}
			}

			private new void _E000()
			{
				_E037.SetAmmoInChamber(0f);
				State = EOperationState.Finished;
				_E002.InitiateOperation<_E015>().Start();
				this.m__E000.Succeed();
				_E037.SetAmmoInChamber(_E002.UnderbarrelWeapon.ChamberAmmoCount);
				_E037.SetShellsInWeapon(_E002.UnderbarrelWeapon.ShellsInLauncherCount);
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}
		}

		private new sealed class _E001 : _E00E
		{
			private Item _E011;

			private Slot _E012;

			private new Callback m__E000;

			private bool _E013;

			public _E001(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(Item item, Slot slot, Callback callback)
			{
				_E011 = item;
				_E012 = slot;
				this.m__E000 = callback;
				Start();
				_E037.SetupMod(modSet: true);
				_E037.SetFire(fire: false);
				_E002.SetAim(value: false);
			}

			public override void Reset()
			{
				_E011 = null;
				_E012 = null;
				this.m__E000 = null;
				_E013 = false;
				base.Reset();
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					OnModChanged();
				}
			}

			public override void SetAiming(bool isAiming)
			{
				if (!isAiming)
				{
					_E002.IsAiming = false;
				}
			}

			public override void OnModChanged()
			{
				if (!_E013)
				{
					_E013 = true;
					_E037.SetupMod(modSet: false);
					GameObject gameObject = Singleton<_E760>.Instance.CreateItem(_E011, isAnimated: true);
					_E038.SetupMod(_E012, gameObject);
					_E037.Fold(_E039.Folded);
					State = EOperationState.Finished;
					this.m__E000.Succeed();
					base._E001.BodyAnimatorCommon.SetFloat(_E712.WEAPON_SIZE_MODIFIER_PARAM_HASH, _E039.CalculateCellSize().X);
					base._E001.UpdateFirstPersonGrip(GripPose.EGripType.Common, _E002.HandsHierarchy);
					_E002.WeaponModified();
					_E000(gameObject);
					_E002.InitiateOperation<_E011>().Start();
				}
			}

			private void _E000(GameObject createdItem)
			{
				if (_E011 is _EA62 underbarrelWeapon)
				{
					_E002._E007(underbarrelWeapon, createdItem);
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}

			public override bool CanChangeLightState(_E6C5[] lightsStates)
			{
				return false;
			}
		}

		public new class _E002 : _E00D
		{
			private const float _E014 = 0f;

			private const float _E015 = 0.25f;

			private const float _E016 = 0.75f;

			private const float _E017 = 0.75f;

			private const float _E018 = 0.99f;

			protected const int _E019 = 3;

			protected int _E01A;

			protected float _E01B;

			protected float _E01C;

			protected float _E007;

			protected SingleShotData _E01D;

			private bool _E01E;

			private int _E01F;

			protected float _E020;

			public _E002(FirearmController controller)
				: base(controller)
			{
				_E01B = 60f / (float)controller.Item.FireRate;
				_E01F = _E039.GetItemComponent<FireModeComponent>()?.BurstShotsCount ?? 3;
			}

			public new virtual void Start()
			{
				base.Start();
				_E007 = 0.0001f;
				_E01A = 0;
				_E01C = 0f;
				_E01E = false;
				((_E00E)this)._E002.m__E00F = false;
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				_E037.Animator.Play(_E037.FullFireStateName, 1, 0.2f);
				InternalOnFireEvent();
			}

			public override void Reset()
			{
				base.Reset();
				_E020 = -1f;
			}

			public override void SetTriggerPressed(bool pressed)
			{
				((_E00E)this)._E002.IsTriggerPressed = pressed;
			}

			public override void OnFireEvent()
			{
				_E01E = true;
			}

			public override void Update(float deltaTime)
			{
				if (!_E01E)
				{
					return;
				}
				try
				{
					base.Update(deltaTime);
					float num = _E007 % _E01B / _E01B;
					float num2 = num + deltaTime / _E01B;
					if (num2 <= 1f)
					{
						_E000(num, num2);
					}
					else
					{
						_E000(num, 1f);
						float num3 = num2 - 1f;
						while (num3 > 0f)
						{
							if (num3 <= 1f)
							{
								_E000(0f, num3);
								num3 -= 1f;
								continue;
							}
							double num4 = Math.Floor(num3);
							for (int i = 0; (double)i < num4; i++)
							{
								_E000(0f, 1f);
								num3 -= 1f;
							}
						}
					}
					Weapon item = ((_E00E)this)._E002.Item;
					float overheatFirerateMult = item.MalfState.OverheatFirerateMult;
					_E007 += deltaTime;
					if (overheatFirerateMult > 0f)
					{
						if (Mathf.Abs(overheatFirerateMult - _E01C) > Mathf.Epsilon)
						{
							_E007 = _E01B * (num2 % 1f);
							_E01B = 60f / ((float)item.FireRate * item.MalfState.OverheatFirerateMult);
						}
						_E01C = overheatFirerateMult;
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
					InternalOnFireEndEvent();
				}
			}

			protected void _E000(float normalizedStartFrameTime, float normalizedEndFrameTime)
			{
				if (State != EOperationState.Executing)
				{
					return;
				}
				if (normalizedStartFrameTime <= 0f && 0f < normalizedEndFrameTime)
				{
					InternalOnFireEvent();
				}
				if (State == EOperationState.Executing)
				{
					if (normalizedStartFrameTime <= 0.25f && 0.25f < normalizedEndFrameTime)
					{
						InternalOnShellEjectEvent();
					}
					if (normalizedStartFrameTime <= 0.75f && 0.75f < normalizedEndFrameTime && _E039.IsBoltCatch && _E039.ChamberAmmoCount == 0 && _E039.GetCurrentMagazine() != null && _E039.GetCurrentMagazineCount() == 0 && !_E039.ManualBoltCatch)
					{
						_E037.SetBoltCatch(active: true);
					}
					if (normalizedStartFrameTime <= 0.75f && 0.75f < normalizedEndFrameTime)
					{
						InternalRemoveAmmoFromChamber();
						InternalOnAddAmmoInChamber();
					}
					if (normalizedStartFrameTime <= 0.99f && 0.99f < normalizedEndFrameTime)
					{
						InternalOnFireEndEvent();
					}
					_E020 = normalizedEndFrameTime;
				}
			}

			protected virtual void InternalOnFireEvent()
			{
				_E01D = _E000(out var malfState, out var malfSource);
				ShowUncompatibleNotification();
				_E039.MalfState.State = malfState;
				if (malfState != 0)
				{
					_E039.MalfState.LastMalfunctionTime = _E62A.PastTime;
					if (((_E00E)this)._E001.Skills.TroubleFixingDurElite.Value)
					{
						_E039.MalfState.AddMalfReduceChance(((_E00E)this)._E001.ProfileId, malfSource);
					}
					_E037.MisfireSlideUnknown(val: false);
					if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
					{
						((_E00E)this)._E001._E0DE.ExamineMalfunction(_E039, clearRest: true);
					}
				}
				if (malfState == Weapon.EMalfunctionState.Misfire)
				{
					((_E00E)this)._E002.m__E00F = true;
					_E003();
				}
				else
				{
					if (malfState == Weapon.EMalfunctionState.None)
					{
						((_E00E)this)._E002.m__E00F = false;
						_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
					}
					else
					{
						((_E00E)this)._E002.m__E00F = true;
					}
					FireModeComponent fireMode = ((_E00E)this)._E002.Item.FireMode;
					((_E00E)this)._E002.IsBirstOf2Start = fireMode.FireMode == Weapon.EFireMode.burst && _E01A == 0 && fireMode.BurstShotsCount == 2;
					MakeShot(_E01D.AmmoToFire);
					_E01A++;
					((_E00E)this)._E002.IsBirstOf2Start = false;
					if (_E039.HasChambers)
					{
						if (_E039.MalfState.State == Weapon.EMalfunctionState.Feed)
						{
							((_E00E)this)._E002._E00C.SetRoundIntoWeapon(_E01D.FedAmmo);
							((_E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(ammoIsUsed: false);
						}
						else
						{
							((_E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(_E01D.AmmoToFire.IsUsed);
						}
					}
					if (_E039.MalfState.State == Weapon.EMalfunctionState.Jam || _E039.MalfState.State == Weapon.EMalfunctionState.SoftSlide || _E039.MalfState.State == Weapon.EMalfunctionState.HardSlide || _E039.MalfState.State == Weapon.EMalfunctionState.Feed)
					{
						if (true)
						{
							((_E00E)this)._E001._E0DE.ExamineMalfunction(_E039, clearRest: true);
						}
						_E003();
					}
				}
				((_E00E)this)._E002._player.MouseLook();
			}

			public void InternalOnShellEjectEvent()
			{
				if (!_E039.HasChambers && _E01D.AmmoToFire != null)
				{
					((_E00E)this)._E002._E00C.SetRoundIntoWeapon(_E01D.AmmoToFire);
					((_E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(_E01D.AmmoToFire.IsUsed);
				}
				_ = _E01D;
				base._E003();
			}

			public override void OnShellEjectEvent()
			{
			}

			public override void OnAddAmmoInChamber()
			{
			}

			public override void RemoveAmmoFromChamber()
			{
			}

			public override void OnOnOffBoltCatchEvent(bool isCaught)
			{
			}

			public void InternalRemoveAmmoFromChamber()
			{
				if (_E039.HasChambers)
				{
					_E037.SetAmmoInChamber(0f);
				}
				if (_E01D.AmmoWillBeLoadedToChamber == null || _E039.MalfState.State != 0)
				{
					((_E00E)this)._E002.m__E00F = true;
				}
			}

			public void InternalOnAddAmmoInChamber()
			{
				if (_E039.HasChambers)
				{
					_E037.SetAmmoInChamber(_E01D.AmmoCountInChamberAfterShot);
				}
				_E037.SetAmmoOnMag(_E01D.AmmoCountInMagAfterShot);
				if (_E039.HasChambers && _E01D.AmmoWillBeLoadedToChamber != null)
				{
					((_E00E)this)._E002._E00C.SetRoundIntoWeapon(_E01D.AmmoWillBeLoadedToChamber);
				}
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
			}

			protected virtual void InternalOnFireEndEvent()
			{
				if (_E039.HasChambers)
				{
					_E037.SetAmmoInChamber(_E01D.AmmoCountInChamberAfterShot);
				}
				_E037.SetAmmoOnMag(_E01D.AmmoCountInMagAfterShot);
				if (_E039.SelectedFireMode == Weapon.EFireMode.burst)
				{
					if (_E01A < _E01F && _E01D.AmmoCountInChamberAfterShot > 0)
					{
						((_E00E)this)._E002.IsTriggerPressed = true;
						return;
					}
					((_E00E)this)._E002.IsTriggerPressed = false;
				}
				if (_E036 != null)
				{
					((_E00E)this)._E002.IsTriggerPressed = false;
					_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
					SetAiming(isAiming: false);
					_E002();
				}
				else if (!((_E00E)this)._E002.IsTriggerPressed || _E035)
				{
					_E001();
				}
				else if ((_E039.HasChambers && _E01D.AmmoCountInChamberAfterShot == 0) || (!_E039.HasChambers && _E01D.AmmoCountInMagAfterShot == 0))
				{
					if (_E039.IsBoltCatch)
					{
						_E001();
						return;
					}
					_E001();
					((_E00E)this)._E002.SetTriggerPressed(pressed: true);
				}
			}

			public override void FastForward()
			{
				_E001();
			}

			protected new void _E001()
			{
				SetTriggerPressed(pressed: false);
				_E037.SetInventory(_E035);
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				State = EOperationState.Finished;
				_E037.Animator.Play(_E037.FullIdleStateName, 1, 0.1f);
				((_E00E)this)._E002.EmitEvents();
				State = EOperationState.Finished;
				((_E00E)this)._E002.InitiateOperation<_E011>().Start();
			}

			private new void _E002()
			{
				SetTriggerPressed(pressed: false);
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				_E037.Animator.Play(_E037.FullIdleStateName, 1, 0.1f);
				((_E00E)this)._E002.EmitEvents();
				SetAiming(isAiming: false);
				State = EOperationState.Finished;
				_E036();
			}

			protected new void _E003()
			{
				SetTriggerPressed(pressed: false);
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				_E037.Malfunction((int)_E039.MalfState.State);
				State = EOperationState.Finished;
				switch (_E039.MalfState.State)
				{
				case Weapon.EMalfunctionState.Jam:
					_E037.Animator.Play(_ED3E._E000(61976), 1, 0f);
					break;
				case Weapon.EMalfunctionState.Misfire:
					_E037.Animator.Play(_ED3E._E000(52717), 1, 0f);
					break;
				case Weapon.EMalfunctionState.Feed:
					_E037.Animator.Play(_ED3E._E000(62929), 1, 0f);
					break;
				case Weapon.EMalfunctionState.SoftSlide:
					_E037.Animator.Play(_ED3E._E000(63339), 1, 0f);
					break;
				case Weapon.EMalfunctionState.HardSlide:
					_E037.Animator.Play(_ED3E._E000(51229), 1, 0f);
					break;
				}
				((_E00E)this)._E002.EmitEvents();
				_E039.MalfState.AmmoToFire = _E01D.AmmoToFire;
				_E039.MalfState.AmmoWillBeLoadedToChamber = _E01D.AmmoWillBeLoadedToChamber;
				_E039.MalfState.MalfunctionedAmmo = _E01D.FedAmmo ?? _E01D.AmmoToFire;
				((_E00E)this)._E002.InitiateOperation<_E031>().Start();
			}
		}

		public new abstract class _E003 : _E00E
		{
			protected bool _E021;

			protected Action _E022;

			private Callback _E023;

			[CompilerGenerated]
			private Slot _E024;

			protected new Slot _E000
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

			public override EOperationState State
			{
				get
				{
					return base.State;
				}
				protected set
				{
					base.State = value;
					switch (value)
					{
					case EOperationState.Finished:
						base._E001.CurrentState.OnReload(enable: false);
						break;
					case EOperationState.Executing:
						base._E001.CurrentState.OnReload(enable: true);
						break;
					}
				}
			}

			protected _E003(FirearmController controller)
				: base(controller)
			{
			}

			protected void _E000(bool isAiming)
			{
				if (base._E002.CurrentMasteringLevel < 2)
				{
					return;
				}
				base._E001.ProceduralWeaponAnimation.TacticalReload = isAiming;
				if (!isAiming || EFTHardSettings.Instance.CanAimInState(base._E001.CurrentState.Name))
				{
					if (base._E002.m__E010 > EFTHardSettings.Instance.STOP_AIMING_AT && isAiming)
					{
						base._E002.AimingInterruptedByOverlap = false;
					}
					else
					{
						base._E002.IsAiming = isAiming;
					}
				}
			}

			protected void Start([CanBeNull] Callback callback)
			{
				_E023 = callback;
				this._E000 = (_E039.HasChambers ? _E039.Chambers[0] : null);
				base._E001.ProceduralWeaponAnimation.TacticalReload = true;
				base._E001.ExecuteSkill((Action)delegate
				{
					base._E001.Skills.WeaponReloadAction.Complete(_E039);
				});
				_E037.SetInventory(open: false);
				base._E002.SetCompassState(active: false);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				Start();
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				_E021 = fastDrop;
				_E022 = onHidden;
			}

			public override void Reset()
			{
				_E022 = null;
				_E021 = false;
				_E023 = null;
				this._E000 = null;
				base.Reset();
			}

			protected new void _E001()
			{
				_E023?.Succeed();
			}

			public override void SetInventoryOpened(bool opened)
			{
				base._E002.InventoryOpened = opened;
			}

			protected new void _E002()
			{
				_E037.SetInventory(base._E002.InventoryOpened);
			}

			[CompilerGenerated]
			private void _E003()
			{
				base._E001.Skills.WeaponReloadAction.Complete(_E039);
			}
		}

		public sealed class _E004 : _E00C
		{
			public _E004(FirearmController controller)
				: base(controller)
			{
			}

			public override void Start()
			{
				base.Start();
				_E037.SetBoltActionReload(!((_E00E)this)._E002.IsTriggerPressed);
			}

			public override void SetTriggerPressed(bool pressed)
			{
				base.SetTriggerPressed(pressed);
				_E037.SetBoltActionReload(!((_E00E)this)._E002.IsTriggerPressed);
			}

			public override void SetInventoryOpened(bool opened)
			{
				base.SetInventoryOpened(opened);
				_E037.SetBoltActionReload(boltActionReload: true);
			}

			public override void ReloadMag(_EA6A magazine, _EB22 gridItemAddress, Callback finishCallback, Callback startCallback)
			{
				base.ReloadMag(magazine, gridItemAddress, finishCallback, startCallback);
				_E037.SetBoltActionReload(boltActionReload: true);
			}

			public override void QuickReloadMag(_EA6A magazine, Callback finishCallback, Callback startCallback)
			{
				base.QuickReloadMag(magazine, finishCallback, startCallback);
				_E037.SetBoltActionReload(boltActionReload: true);
			}

			public override void ReloadWithAmmo(_E9CF ammoPack, Callback finishCallback, Callback startCallback)
			{
				base.ReloadWithAmmo(ammoPack, finishCallback, startCallback);
				_E037.SetBoltActionReload(boltActionReload: true);
			}

			public override void ReloadRevolverDrum(_E9CF ammoPack, Callback finishCallback, Callback startCallback, bool quickReload = false)
			{
				base.ReloadRevolverDrum(ammoPack, finishCallback, startCallback, quickReload);
				_E037.SetBoltActionReload(boltActionReload: true);
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				SetAiming(isAiming: false);
				SetTriggerPressed(pressed: false);
				base.HideWeapon(onHidden, fastDrop, (Item)null);
			}

			public override void OnAimingDisabled()
			{
				SetAiming(isAiming: false);
				SetTriggerPressed(pressed: false);
				base.OnAimingDisabled();
			}
		}

		public class _E005 : _E00E
		{
			private _EA12 _E00C;

			private _EB13 _E025;

			private ItemAddress _E00D;

			private new Callback m__E000;

			private bool _E00E;

			private bool _E00F;

			private int _E026 = -1;

			public _E005(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(_EA12 item, int camoraIndex, ItemAddress itemAddress, Callback callback)
			{
				_E00C = item;
				this.m__E000 = callback;
				_E026 = camoraIndex;
				_E025 = _E039.GetCurrentMagazine() as _EB13;
				_E00D = itemAddress;
				Start();
				_E002.IsAiming = false;
				_E037.Discharge(discharge: true);
				_E037.SetFire(fire: false);
				_E037.SetCamoraIndexForUnloadAmmo(camoraIndex);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}

			public override void Reset()
			{
				base.Reset();
				_E00C = null;
				this.m__E000 = null;
				_E00E = false;
				_E00F = false;
				_E026 = -1;
				_E025 = null;
				_E00D = null;
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					OnMagAppeared();
					OnMagPuttedToRig();
				}
			}

			public override void OnMagAppeared()
			{
				if (!_E00E)
				{
					_E00E = true;
					_E037.Discharge(discharge: false);
					_E037.SetShellsInWeapon(_E039.ShellsInChamberCount);
					_E037.SetAmmoOnMag(_E025.Count);
					_E038.DestroyPatronInWeapon(_E026);
					_E037.SetCanReload(canReload: false);
					if (_E00D != null)
					{
						_E000();
					}
				}
			}

			public override void OnMagPuttedToRig()
			{
				if (!_E00F)
				{
					_E00F = true;
					if (_E00D == null)
					{
						_E000();
					}
				}
			}

			private new void _E000()
			{
				_E025.ResetCamoraIndex();
				_E037.SetCamoraIndex(_E025.CurrentCamoraIndex);
				State = EOperationState.Finished;
				_E001();
				_E002.InitiateOperation<_E011>().Start();
				this.m__E000.Succeed();
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}

			private new void _E001()
			{
				_E037.LoadOneTrigger(_E00D != null);
			}
		}

		private sealed class _E006 : _E007
		{
			public _E006(FirearmController controller)
				: base(controller)
			{
			}

			public override void RemoveAmmoFromChamber()
			{
				_E000.Succeed();
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
				_E037.SetCanReload(canReload: false);
				if (_E027)
				{
					_E038.RemoveShellInWeapon();
				}
				else
				{
					_E038.DestroyPatronInWeapon(_E010);
				}
			}

			public override void OnMagPuttedToRig()
			{
				if (!_E00F)
				{
					_E00F = true;
					State = EOperationState.Finished;
					_E037.Discharge(discharge: false);
					_E002.InitiateOperation<_E011>().Start();
					if (base._E001)
					{
						RemoveAmmoFromChamber();
					}
				}
			}
		}

		private class _E007 : _E00E
		{
			protected new Callback _E000;

			protected bool _E027;

			protected bool _E00F;

			protected int _E010 = -1;

			private ItemAddress _E028;

			protected new bool _E001 => _E028 != null;

			public _E007(FirearmController controller)
				: base(controller)
			{
			}

			internal void Start(_EA12 item, int chamberIndex, ItemAddress destinationAddress, Callback callback)
			{
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					base._E000();
				}
				this._E000 = callback;
				_E027 = item.IsUsed;
				_E010 = chamberIndex;
				_E028 = destinationAddress;
				Start();
				_E002.IsAiming = false;
				_E037.Discharge(discharge: true);
				_E037.SetFire(fire: false);
				_E037.SetChamberIndexForLoadUnloadAmmo(_E010);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				_E000();
				_E001();
			}

			public override void Reset()
			{
				base.Reset();
				_E028 = null;
				this._E000 = null;
				_E027 = false;
				_E00F = false;
				_E010 = -1;
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					OnMagPuttedToRig();
				}
			}

			public override void OnMagPuttedToRig()
			{
				if (!_E00F)
				{
					_E00F = true;
					State = EOperationState.Finished;
					_E037.Discharge(discharge: false);
					_E002.InitiateOperation<_E011>().Start();
					this._E000.Succeed();
					_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
					_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
					_E037.SetCanReload(canReload: false);
					if (_E027)
					{
						_E038.RemoveShellInWeapon();
					}
					else
					{
						_E038.DestroyPatronInWeapon(_E010);
					}
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}

			private new void _E000()
			{
				_E037.LoadOneTrigger(_E001);
			}
		}

		public class _E008 : _E00E
		{
			private new Callback _E000;

			private bool _E029;

			public _E008(FirearmController controller)
				: base(controller)
			{
			}

			public virtual void Start(Item item, Callback callback)
			{
				_E000 = callback;
				Start();
				_E002.SetCompassState(active: false);
				_E037.SetFire(fire: false);
				_E037.SetInventory(open: false);
				_E002.SetAim(value: false);
				_E037.SetInteract(p: true, 300);
			}

			public override void Reset()
			{
				_E000 = null;
				_E029 = false;
				base.Reset();
			}

			public override void SetAiming(bool isAiming)
			{
				if (!isAiming)
				{
					_E002.IsAiming = false;
				}
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					OnBackpackDropEvent();
				}
			}

			public override void OnBackpackDropEvent()
			{
				if (!_E029)
				{
					_E029 = true;
					State = EOperationState.Finished;
					_E037.SetInteract(p: false, 300);
					_E326.ResetTriggerHandReady(_E002._E001.Animator);
					_E037.SetInventory(_E002.m__E002);
					_E002.InitiateOperation<_E011>().Start();
					_E000.Succeed();
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.m__E002 = opened;
			}

			public override bool CanChangeLightState(_E6C5[] lightsStates)
			{
				return false;
			}
		}

		public sealed class _E009 : _E01B
		{
			private bool _E02A;

			public _E009(FirearmController controller)
				: base(controller)
			{
			}

			public new void Start()
			{
				_E002.IsAiming = false;
				_E037.SetFire(fire: false);
				((_E00E)this)._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				base.Start();
			}

			public override void Reset()
			{
				_E02A = false;
				base.Reset();
			}

			public override void OnMalfunctionOffEvent()
			{
				if (!_E02A)
				{
					_E02A = true;
					((_E00E)this)._E001._E0DE.ExamineMalfunctionType(_E039);
					_E000();
				}
			}

			public override void FastForward()
			{
				OnMalfunctionOffEvent();
			}
		}

		public class _E00A : _E00C
		{
			private _EB13 _E025;

			private int _E026 = -1;

			protected bool _E02B;

			public _E00A(FirearmController controller)
				: base(controller)
			{
			}

			public override void Start()
			{
				_E025 = _E039.GetCurrentMagazine() as _EB13;
				_E026 = _E025.GetCamoraFireOrLoadStartIndex(!_E039.CylinderHammerClosed);
				_E037.SetCamoraFireIndex(_E025.CurrentCamoraIndex);
				if (_E025.GetFirstAmmo(!_E039.CylinderHammerClosed) == null)
				{
					State = EOperationState.Executing;
					_E000();
				}
				else
				{
					base.Start();
				}
			}

			private new void _E000()
			{
				_E02B = true;
				_E037.SetCamoraFireIndex(_E025.CurrentCamoraIndex);
				if ((_E039.CylinderHammerClosed && _E039.FireMode.FireMode == Weapon.EFireMode.doubleaction) || (!_E039.CylinderHammerClosed && _E039.FireMode.FireMode == Weapon.EFireMode.single))
				{
					_E025.DryFireIncrementCamoraIndex();
				}
				_E037.SetDoubleAction(Convert.ToSingle(_E039.CylinderHammerClosed));
				_E037.SetCamoraIndex(_E025.CurrentCamoraIndex);
				_E039.CylinderHammerClosed = _E039.FireMode.FireMode == Weapon.EFireMode.doubleaction;
				_E037.SetFire(fire: true);
				_E030 = true;
				((_E00E)this)._E002.DryShot(_E026);
			}

			public override void Reset()
			{
				base.Reset();
				_E02B = false;
				_E026 = -1;
				_E025 = null;
				_E02F = default(SingleShotData);
				_E030 = false;
			}

			public override void OnFireEvent()
			{
				if (!_E030)
				{
					_E030 = true;
					MakeShot(_E02F.AmmoToFire, _E026);
					((_E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(_E02F.AmmoToFire.IsUsed, _E026);
				}
			}

			public override void OnAddAmmoInChamber()
			{
				_E037.SetAmmoOnMag(_E025.Count);
				_E039.CylinderHammerClosed = _E039.FireMode.FireMode == Weapon.EFireMode.doubleaction;
				_E037.SetDoubleAction(Convert.ToSingle(_E039.CylinderHammerClosed));
			}

			public override void SetTriggerPressed(bool pressed)
			{
				((_E00E)this)._E002.IsTriggerPressed &= pressed;
			}

			public override void SetInventoryOpened(bool opened)
			{
				base.SetInventoryOpened(opened);
				((_E00E)this)._E002.InventoryOpened = opened;
			}

			public override void OnFireEndEvent()
			{
				if (_E02B)
				{
					_E037.SetDoubleAction(Convert.ToSingle(_E039.CylinderHammerClosed));
				}
			}

			public override void OnIdleStartEvent()
			{
				if (_E030)
				{
					((_E00E)this)._E002.IsTriggerPressed = false;
					_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
					if (_E036 != null)
					{
						_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
						SetAiming(isAiming: false);
						_E003();
					}
					else
					{
						_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
						_E001();
					}
				}
			}

			public override void OnShellEjectEvent()
			{
			}

			public override void FastForward()
			{
				OnFireEvent();
				_E001();
				_E037.Animator.Play(_E037.FullIdleStateName, 1, 0.2f);
			}

			protected override void PrepareShot()
			{
				_EA12 firstAmmo = _E025.GetFirstAmmo(!_E039.CylinderHammerClosed);
				_E02F = default(SingleShotData);
				_E025.RemoveAmmoInCamora(firstAmmo, ((_E00E)this)._E002._player._E0DE);
				((_E00E)this)._E001._E0DE.CheckChamber(_E039, status: false);
				if (firstAmmo == null)
				{
					UnityEngine.Debug.LogError(_ED3E._E000(161997));
					return;
				}
				_E02F.AmmoToFire = firstAmmo;
				_E02F.AmmoToFire.IsUsed = true;
				_E039.ShellsInChambers[_E026] = (AmmoTemplate)_E02F.AmmoToFire.Template;
				if (_E039.CylinderHammerClosed || _E039.FireMode.FireMode != Weapon.EFireMode.doubleaction)
				{
					_E025.IncrementCamoraIndex();
				}
				_E037.SetCamoraIndex(_E025.CurrentCamoraIndex);
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				ShowUncompatibleNotification();
				((_E00E)this)._E002._player.MouseLook();
			}

			protected new void _E001()
			{
				_E037.SetAmmoOnMag(_E025.Count);
				_E037.SetInventory(_E035);
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				State = EOperationState.Finished;
				((_E00E)this)._E002.InitiateOperation<_E011>().Start();
			}

			protected new void _E002()
			{
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				_E037.Malfunction((int)_E039.MalfState.State);
				State = EOperationState.Finished;
				_E039.MalfState.AmmoToFire = _E02F.AmmoToFire;
				_E039.MalfState.AmmoWillBeLoadedToChamber = _E02F.AmmoWillBeLoadedToChamber;
				_E039.MalfState.MalfunctionedAmmo = _E02F.FedAmmo ?? _E02F.AmmoToFire;
				((_E00E)this)._E002.InitiateOperation<_E031>().Start();
			}

			private new void _E003()
			{
				_E037.SetAmmoOnMag(_E025.Count);
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				SetAiming(isAiming: false);
				State = EOperationState.Finished;
				_E036();
			}
		}

		public class _E00B : _E00C
		{
			private List<int> _E02C = new List<int>();

			private List<SingleShotData> _E02D = new List<SingleShotData>();

			private int _E02E;

			public _E00B(FirearmController controller)
				: base(controller)
			{
			}

			protected override void PrepareShot()
			{
				if (_E039.FireMode.FireMode == Weapon.EFireMode.single)
				{
					Slot firstLoadedChamberSlot = _E039.FirstLoadedChamberSlot;
					int chamberIndex = Array.IndexOf(_E039.Chambers, firstLoadedChamberSlot);
					_E001(chamberIndex);
				}
				else
				{
					_E000();
				}
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
			}

			private new void _E000()
			{
				for (int i = 0; i < _E039.Chambers.Length; i++)
				{
					_E001(i);
				}
			}

			private new void _E001(int chamberIndex)
			{
				SingleShotData singleShotData = default(SingleShotData);
				singleShotData.AmmoCountInChamberBeforeShot = _E039.ChamberAmmoCount;
				SingleShotData item = singleShotData;
				Slot slot = _E039.Chambers[chamberIndex];
				if (slot.ContainedItem is _EA12 obj && !obj.IsUsed)
				{
					obj.IsUsed = true;
					slot.RemoveItem().OrElse(elseValue: false);
					_E02C.Add(chamberIndex);
					item.AmmoToFire = obj;
					_E02D.Add(item);
				}
			}

			public override void Reset()
			{
				base.Reset();
				_E02E = 0;
				_E02C.Clear();
				_E02D.Clear();
			}

			public override void RemoveAmmoFromChamber()
			{
				_E000();
			}

			public override void OnAddAmmoInChamber()
			{
				_E000();
			}

			public override void OnFireEvent()
			{
				_E030 = true;
				MakeMultiBarrelShot(_E02D, _E02C);
				for (int i = 0; i < _E02D.Count; i++)
				{
					((_E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(_E02D[i].AmmoToFire.IsUsed, _E02C[i]);
					_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
					SetTriggerPressed(pressed: false);
					_E039.ShellsInChambers[_E02C[i]] = (AmmoTemplate)_E02D[i].AmmoToFire.Template;
				}
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
				_E02E++;
			}
		}

		public class _E00C : _E00D
		{
			protected SingleShotData _E02F;

			protected bool _E030;

			private float _E031;

			private bool _E032;

			private bool _E033;

			private bool _E034 = true;

			public _E00C(FirearmController controller)
				: base(controller)
			{
			}

			public new virtual void Start()
			{
				_E037.SetBoltActionReload(boltActionReload: true);
				base.Start();
				PrepareShot();
				_E031 = 60f / (float)_E039.SingleFireRate;
				_E034 = _E039.CanQueueSecondShot;
				StartFireAnimation();
			}

			protected virtual void StartFireAnimation()
			{
				if (_E039.MalfState.State == Weapon.EMalfunctionState.None)
				{
					if (_E039 is _EAD1 && _E039.CylinderHammerClosed)
					{
						_E037.Animator.Play(_E037.FullDoubleActionFireStateName, 1, 0.2f);
					}
					else if (_E039.FireMode.FireMode == Weapon.EFireMode.semiauto)
					{
						_E037.Animator.Play(_E037.FullSemiFireStateName, 1, 0.2f);
					}
					else
					{
						_E037.Animator.Play(_E037.FullFireStateName, 1, 0.2f);
					}
				}
			}

			public override void Reset()
			{
				base.Reset();
				_E02F = default(SingleShotData);
				_E030 = false;
				_E031 = 0f;
				_E032 = false;
				_E033 = false;
			}

			public override void OnFireEvent()
			{
				_E030 = true;
				MakeShot(_E02F.AmmoToFire);
				if (_E039.HasChambers)
				{
					((_E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(_E02F.AmmoToFire.IsUsed);
				}
				((_E00E)this)._E002.IsTriggerPressed = false;
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Jam || _E039.MalfState.State == Weapon.EMalfunctionState.SoftSlide || _E039.MalfState.State == Weapon.EMalfunctionState.HardSlide || _E039.MalfState.State == Weapon.EMalfunctionState.Feed)
				{
					if (true)
					{
						((_E00E)this)._E001._E0DE.ExamineMalfunction(_E039, clearRest: true);
					}
					_E001();
				}
			}

			protected virtual void PrepareShot()
			{
				_E02F = _E000(out var malfState, out var malfSource);
				_E039.MalfState.State = malfState;
				if (malfState == Weapon.EMalfunctionState.None)
				{
					((_E00E)this)._E002.m__E00F = false;
					_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				}
				else
				{
					_E039.MalfState.LastMalfunctionTime = _E62A.PastTime;
					((_E00E)this)._E002.m__E00F = true;
					if (((_E00E)this)._E001.Skills.TroubleFixingDurElite.Value)
					{
						_E039.MalfState.AddMalfReduceChance(((_E00E)this)._E001.ProfileId, malfSource);
					}
					_E037.MisfireSlideUnknown(val: false);
					if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
					{
						((_E00E)this)._E001._E0DE.ExamineMalfunction(_E039, clearRest: true);
					}
					if (malfState == Weapon.EMalfunctionState.Misfire)
					{
						_E001();
					}
					else
					{
						_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
					}
				}
				ShowUncompatibleNotification();
				((_E00E)this)._E002._player.MouseLook();
			}

			public override void RemoveAmmoFromChamber()
			{
				_E037.SetAmmoInChamber(0f);
				if (_E02F.AmmoWillBeLoadedToChamber == null || _E039.MalfState.State != 0)
				{
					((_E00E)this)._E002.m__E00F = true;
				}
			}

			public override void OnAddAmmoInChamber()
			{
				_E037.SetAmmoInChamber(_E02F.AmmoCountInChamberAfterShot);
				_E037.SetAmmoOnMag(_E02F.AmmoCountInMagAfterShot);
				if (_E02F.AmmoWillBeLoadedToChamber != null)
				{
					((_E00E)this)._E002._E00C.SetRoundIntoWeapon(_E02F.AmmoWillBeLoadedToChamber);
				}
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
			}

			public override void OnShellEjectEvent()
			{
				if (!_E039.HasChambers && _E02F.AmmoToFire != null)
				{
					((_E00E)this)._E002._E00C.SetRoundIntoWeapon(_E02F.AmmoToFire);
					((_E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(_E02F.AmmoToFire.IsUsed);
				}
				_ = _E02F;
				base._E003();
			}

			public override void Update(float deltaTime)
			{
				_E031 -= deltaTime;
				if (_E031 <= 0f && _E032)
				{
					_E003();
				}
			}

			public override void SetTriggerPressed(bool pressed)
			{
				((_E00E)this)._E002.IsTriggerPressed &= pressed;
				_E033 |= pressed && _E034;
			}

			public override void OnFireEndEvent()
			{
				((_E00E)this)._E002.IsTriggerPressed = false;
				if (!_E039.HasChambers && _E02F.AmmoToFire != null)
				{
					((_E00E)this)._E002._E00C.DestroyPatronInWeapon(0);
				}
			}

			public override void OnIdleStartEvent()
			{
				if (_E030)
				{
					_E032 = true;
				}
			}

			protected new void _E000()
			{
				if (_E039.HasChambers)
				{
					_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				}
				_E037.SetAmmoOnMag(_E02F.AmmoCountInMagAfterShot);
				_E037.SetInventory(_E035);
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				State = EOperationState.Finished;
				((_E00E)this)._E002.InitiateOperation<_E011>().Start();
				if (_E033 && !_E035)
				{
					((_E00E)this)._E002.CurrentOperation.SetTriggerPressed(pressed: true);
					((_E00E)this)._E002.CurrentOperation.SetTriggerPressed(pressed: false);
				}
			}

			protected new void _E001()
			{
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				_E037.Malfunction((int)_E039.MalfState.State);
				State = EOperationState.Finished;
				_E039.MalfState.AmmoToFire = _E02F.AmmoToFire;
				_E039.MalfState.AmmoWillBeLoadedToChamber = _E02F.AmmoWillBeLoadedToChamber;
				_E039.MalfState.MalfunctionedAmmo = _E02F.FedAmmo ?? _E02F.AmmoToFire;
				((_E00E)this)._E002.InitiateOperation<_E031>().Start();
			}

			private new void _E002()
			{
				_E037.SetAmmoInChamber(_E02F.AmmoCountInChamberAfterShot);
				_E037.SetAmmoOnMag(_E02F.AmmoCountInMagAfterShot);
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				SetAiming(isAiming: false);
				State = EOperationState.Finished;
				_E036();
			}

			private new void _E003()
			{
				if (_E036 != null)
				{
					_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
					SetAiming(isAiming: false);
					_E002();
				}
				else
				{
					_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
					_E000();
				}
			}

			public override void FastForward()
			{
				base.FastForward();
				if (!_E030)
				{
					OnFireEvent();
				}
				_E033 = false;
				_E003();
				_E037.Animator.Play(_E037.FullIdleStateName, 1, 0.2f);
			}
		}

		public class _E00D : _E00E
		{
			protected struct SingleShotData
			{
				public _EA12 AmmoToFire;

				[CanBeNull]
				public _EA12 AmmoWillBeLoadedToChamber;

				public _EA12 FedAmmo;

				public int AmmoCountInChamberBeforeShot;

				public int AmmoCountInChamberAfterShot;

				public int AmmoCountInMagBeforeShot;

				public int AmmoCountInMagAfterShot;

				public bool IsAmmoCompatible;
			}

			[CompilerGenerated]
			private new sealed class _E000
			{
				public _E00D _003C_003E4__this;

				public _EA6A magazine;

				public _EB22 gridItemAddress;

				public Callback finishCallback;

				public Callback startCallback;

				internal void _E000()
				{
					_ECD8<_E021> sourceOption = FirearmController._E021.Run(((_E00E)_003C_003E4__this)._E002._player._E0DE, ((_E00E)_003C_003E4__this)._E002.Item, magazine, quickReload: false, ((_E00E)_003C_003E4__this)._E002.Item.MalfState.IsKnownMalfunction(((_E00E)_003C_003E4__this)._E002._player.ProfileId), gridItemAddress);
					if (sourceOption.Succeeded)
					{
						_003C_003E4__this.State = EOperationState.Finished;
						((_E00E)_003C_003E4__this)._E002.InitiateOperation<_E020>().Start(sourceOption.Value, finishCallback);
						startCallback?.Succeed();
					}
					else
					{
						finishCallback?.Invoke(sourceOption);
					}
				}
			}

			[CompilerGenerated]
			private new sealed class _E001
			{
				public _E00D _003C_003E4__this;

				public _E9CF ammoPack;

				public Callback finishCallback;

				public bool quickReload;

				public Callback startCallback;

				internal void _E000()
				{
					_003C_003E4__this.State = EOperationState.Finished;
					((_E00E)_003C_003E4__this)._E002.InitiateOperation<_E01F>().Start(ammoPack, finishCallback, quickReload);
					startCallback?.Succeed();
				}
			}

			[CompilerGenerated]
			private new sealed class _E002
			{
				public _E00D _003C_003E4__this;

				public _E9CF ammoPack;

				public Callback finishCallback;

				public Callback startCallback;

				internal void _E000()
				{
					_003C_003E4__this.State = EOperationState.Finished;
					((_E00E)_003C_003E4__this)._E002.InitiateOperation<_E022>().Start(ammoPack, finishCallback);
					startCallback?.Succeed();
				}
			}

			[CompilerGenerated]
			private sealed class _E003
			{
				public _E00D _003C_003E4__this;

				public _EA6A magazine;

				public Callback finishCallback;

				public Callback startCallback;

				internal void _E000()
				{
					_ECD8<_E021> sourceOption = FirearmController._E021.Run(((_E00E)_003C_003E4__this)._E002._player._E0DE, ((_E00E)_003C_003E4__this)._E002.Item, magazine, quickReload: true, ((_E00E)_003C_003E4__this)._E002.Item.MalfState.IsKnownMalfunction(((_E00E)_003C_003E4__this)._E002._player.ProfileId), null);
					if (sourceOption.Succeeded)
					{
						_003C_003E4__this.State = EOperationState.Finished;
						((_E00E)_003C_003E4__this)._E002.InitiateOperation<_E020>().Start(sourceOption.Value, finishCallback);
						startCallback?.Succeed();
					}
					else
					{
						finishCallback?.Invoke(sourceOption);
					}
				}
			}

			[CompilerGenerated]
			private sealed class _E004
			{
				public _E00D _003C_003E4__this;

				public Action onHidden;

				public bool fastDrop;

				public Item nextControllerItem;

				internal void _E000()
				{
					_003C_003E4__this.State = EOperationState.Finished;
					((_E00E)_003C_003E4__this)._E002.InitiateOperation<_E02B>().Start(onHidden, fastDrop, nextControllerItem);
				}
			}

			protected bool _E035;

			protected Action _E036;

			protected _E00D(FirearmController controller)
				: base(controller)
			{
			}

			protected SingleShotData _E000(out Weapon.EMalfunctionState malfState, out Weapon.EMalfunctionSource malfSource)
			{
				_E001(out var ammoToFire, out var _, out var ammoCountInMagBeforeShot);
				_E5CB instance = Singleton<_E5CB>.Instance;
				float modsCoolFactor;
				float currentOverheat = _E039.GetCurrentOverheat(_E62A.PastTime, instance.Overheat, out modsCoolFactor);
				malfState = base._E002.GetMalfunctionState(ammoToFire, ammoCountInMagBeforeShot > 0, _E039.IsBoltCatch, _E039.GetCurrentMagazine() != null, currentOverheat, instance.Overheat.FixSlideOverheat, out malfSource);
				_E039.MalfState.Source = malfSource;
				if (base._E001.IsAI && !instance.Malfunction.AllowMalfForBots)
				{
					malfState = Weapon.EMalfunctionState.None;
				}
				if (!_E039.ValidateMalfunction(malfState))
				{
					malfState = Weapon.EMalfunctionState.None;
				}
				return _E002(malfState);
			}

			private new void _E001(out _EA12 ammoToFire, out _EA12 ammoToChamber, out int ammoCountInMagBeforeShot)
			{
				Slot[] chambers = _E039.Chambers;
				ammoToFire = (_E039.HasChambers ? chambers[0] : null)?.ContainedItem as _EA12;
				ammoToChamber = null;
				_EA6A currentMagazine = _E039.GetCurrentMagazine();
				if (currentMagazine == null)
				{
					ammoCountInMagBeforeShot = 0;
					return;
				}
				ammoCountInMagBeforeShot = currentMagazine.Count;
				if (currentMagazine.IsAmmoCompatible(chambers) && ammoCountInMagBeforeShot > 0)
				{
					if (!_E039.HasChambers)
					{
						ammoToFire = (_EA12)currentMagazine.Cartridges.Peek();
					}
					else
					{
						ammoToChamber = (_EA12)currentMagazine.Cartridges.Peek();
					}
				}
			}

			protected new SingleShotData _E002(Weapon.EMalfunctionState malfState)
			{
				bool flag = malfState == Weapon.EMalfunctionState.Feed;
				Slot[] chambers = _E039.Chambers;
				SingleShotData singleShotData = default(SingleShotData);
				singleShotData.AmmoCountInChamberBeforeShot = ((!_E039.HasChambers) ? 1 : _E039.ChamberAmmoCount);
				SingleShotData result = singleShotData;
				Slot slot = (_E039.HasChambers ? chambers[0] : null);
				_EA12 obj = slot?.ContainedItem as _EA12;
				_EA6A currentMagazine = _E039.GetCurrentMagazine();
				if (_E039.HasChambers)
				{
					slot.RemoveItem().OrElse(elseValue: false);
					obj.IsUsed = true;
					result.AmmoToFire = obj;
				}
				if (currentMagazine == null)
				{
					result.AmmoCountInChamberAfterShot = 0;
					result.AmmoWillBeLoadedToChamber = null;
					result.IsAmmoCompatible = true;
					return result;
				}
				result.AmmoCountInMagBeforeShot = currentMagazine.Count;
				result.IsAmmoCompatible = currentMagazine.IsAmmoCompatible(chambers);
				if (result.IsAmmoCompatible && result.AmmoCountInMagBeforeShot > 0 && (malfState == Weapon.EMalfunctionState.None || malfState == Weapon.EMalfunctionState.Feed))
				{
					_EA12 obj2 = (_EA12)((_E039.HasChambers && !flag) ? currentMagazine.Cartridges.PopTo(base._E002._player._E0DE, new _EB20(_E039.Chambers[0])).Value.ResultItem : currentMagazine.Cartridges.PopToNowhere(base._E002._player._E0DE).Value.ResultItem);
					if (_E039.HasChambers)
					{
						if (flag)
						{
							result.FedAmmo = obj2;
						}
						else
						{
							result.AmmoWillBeLoadedToChamber = obj2;
						}
					}
					else
					{
						result.AmmoToFire = obj2;
						obj = obj2;
					}
					UncheckOnShot();
				}
				result.AmmoCountInChamberAfterShot = _E039.ChamberAmmoCount;
				result.AmmoCountInMagAfterShot = _E039.GetCurrentMagazineCount();
				return result;
			}

			public override void BlindFire(int b)
			{
				BlindFire_Internal(b);
			}

			public override void Reset()
			{
				_E035 = false;
				_E036 = null;
				base.Reset();
			}

			public override void SetAiming(bool isAiming)
			{
				base._E002.IsAiming = isAiming;
			}

			public override void ReloadMag(_EA6A magazine, _EB22 gridItemAddress, Callback finishCallback, Callback startCallback)
			{
				base._E002.IsTriggerPressed = false;
				if (_E036 == null)
				{
					_E036 = delegate
					{
						_ECD8<_E021> sourceOption = FirearmController._E021.Run(base._E002._player._E0DE, base._E002.Item, magazine, quickReload: false, base._E002.Item.MalfState.IsKnownMalfunction(base._E002._player.ProfileId), gridItemAddress);
						if (sourceOption.Succeeded)
						{
							State = EOperationState.Finished;
							base._E002.InitiateOperation<_E020>().Start(sourceOption.Value, finishCallback);
							startCallback?.Succeed();
						}
						else
						{
							finishCallback?.Invoke(sourceOption);
						}
					};
				}
				else
				{
					finishCallback?.Fail(_ED3E._E000(162019));
				}
			}

			public override void ReloadRevolverDrum(_E9CF ammoPack, Callback finishCallback, Callback startCallback, bool quickReload = false)
			{
				base._E002.IsTriggerPressed = false;
				if (_E039.GetCurrentMagazine() != null && _E036 == null)
				{
					_E036 = delegate
					{
						State = EOperationState.Finished;
						base._E002.InitiateOperation<_E01F>().Start(ammoPack, finishCallback, quickReload);
						startCallback?.Succeed();
					};
				}
			}

			public override void ReloadWithAmmo(_E9CF ammoPack, Callback finishCallback, Callback startCallback)
			{
				base._E002.IsTriggerPressed = false;
				if (_E039.GetCurrentMagazine() != null && _E036 == null)
				{
					_E036 = delegate
					{
						State = EOperationState.Finished;
						base._E002.InitiateOperation<_E022>().Start(ammoPack, finishCallback);
						startCallback?.Succeed();
					};
				}
			}

			public override void QuickReloadMag(_EA6A magazine, Callback finishCallback, Callback startCallback)
			{
				base._E002.IsTriggerPressed = false;
				if (_E036 == null)
				{
					_E036 = delegate
					{
						_ECD8<_E021> sourceOption = FirearmController._E021.Run(base._E002._player._E0DE, base._E002.Item, magazine, quickReload: true, base._E002.Item.MalfState.IsKnownMalfunction(base._E002._player.ProfileId), null);
						if (sourceOption.Succeeded)
						{
							State = EOperationState.Finished;
							base._E002.InitiateOperation<_E020>().Start(sourceOption.Value, finishCallback);
							startCallback?.Succeed();
						}
						else
						{
							finishCallback?.Invoke(sourceOption);
						}
					};
				}
				else
				{
					finishCallback?.Fail(_ED3E._E000(162019));
				}
			}

			public override bool CanStartReload()
			{
				return _E036 == null;
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				base._E002.IsTriggerPressed = false;
				_E036 = delegate
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E02B>().Start(onHidden, fastDrop, nextControllerItem);
				};
			}

			public override void OnOnOffBoltCatchEvent(bool isCaught)
			{
				if (_E039.IsBoltCatch)
				{
					_E037.SetBoltCatch(isCaught);
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E035 = opened;
			}

			protected void _E003()
			{
				base._E002._E00C.StartSpawnShell(base._E002._player.Velocity * 0.66f);
			}

			protected virtual void MakeMultiBarrelShot(List<SingleShotData> singleShotDatas, List<int> chambersForFire)
			{
				bool multiBarrelShot = singleShotDatas.Count > 1;
				for (int i = 0; i < singleShotDatas.Count; i++)
				{
					MakeShot(singleShotDatas[i].AmmoToFire, chambersForFire[i], multiBarrelShot);
				}
			}

			protected virtual void MakeShot(_EA12 ammo, int chamberIndex = 0, bool multiBarrelShot = false)
			{
				base._E002._E037(base._E002.Item, ammo, chamberIndex, multiBarrelShot);
			}

			protected virtual void UncheckOnShot()
			{
				Weapon item = base._E002.Item;
				_EA6A currentMagazine = item.GetCurrentMagazine();
				if (currentMagazine != null)
				{
					base._E002._player._E0DE.CheckChamber(item, base._E002._player.Profile.CheckedMagazines.ContainsKey(currentMagazine.Id));
					if (Singleton<_E5CB>.Instance.UncheckOnShot)
					{
						base._E002._player._E0DE.CheckMagazineAmmoDepend(currentMagazine, _E004, useOperation: false, allowUncheck: true);
					}
				}
				else
				{
					base._E002._player._E0DE.CheckChamber(item, status: false);
				}
			}

			private void _E004()
			{
			}
		}

		public abstract class _E00E : Player._E00E
		{
			protected readonly FirearmController _E002;

			protected new Player _E001;

			protected readonly FirearmsAnimator _E037;

			protected readonly _E6F9 _E038;

			protected readonly Weapon _E039;

			protected _E00E(FirearmController controller)
				: base(controller)
			{
				this._E002 = controller;
				this._E001 = this._E002._player;
				_E037 = this._E002._E001;
				_E038 = this._E002._E00C;
				_E039 = this._E002.Item;
			}

			public virtual bool CanChangeLightState(_E6C5[] lightsStates)
			{
				if (lightsStates != null)
				{
					return lightsStates.Length != 0;
				}
				return false;
			}

			public void SetLightsState(_E6C5[] lightsStates, bool force = false)
			{
				if (force || CanChangeLightState(lightsStates))
				{
					_E037.ModToggleTrigger();
					_E038.UpdateBeams();
				}
			}

			protected new void _E000()
			{
				this._E001.ExecuteSkill((Action)delegate
				{
					this._E001.Skills.WeaponFixAction.Complete();
				});
				this._E001._E0DE.CallUnknownMalfunctionStartRepair(_E039);
				this._E001._E0DE.CallMalfunctionRepaired(_E039);
				_E039.MalfState.Repair();
				_E037.MalfunctionRepair(val: false);
				_E037.Malfunction((int)_E039.MalfState.State);
				_E037.MisfireSlideUnknown(val: false);
				_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 0);
				_E039.MalfState.AmmoToFire = null;
				_E039.MalfState.AmmoWillBeLoadedToChamber = null;
				_E039.MalfState.MalfunctionedAmmo = null;
			}

			protected new void _E001()
			{
				int num = 0;
				for (int i = 0; i < _E039.ShellsInChambers.Length; i++)
				{
					if (_E039.ShellsInChambers[i] != null)
					{
						num++;
						_E037.SetChamberIndexWithShell(i);
					}
				}
				if (num == _E039.ShellsInChambers.Length)
				{
					_E037.SetChamberIndexWithShell(num);
				}
				if (num == 0)
				{
					_E037.SetChamberIndexWithShell(-1f);
				}
			}

			public void SetScopeMode(_E6C6[] scopeStates)
			{
				if (CanChangeScopeStates(scopeStates))
				{
					_E037.ModToggleTrigger();
					_E038.UpdateScopesMode();
				}
			}

			public virtual bool CanRemove()
			{
				return false;
			}

			public virtual void ShowGesture(EGesture gesture)
			{
				base._E000();
			}

			public virtual void BlindFire(int b)
			{
				BlindFire_Internal(0);
			}

			public virtual void OnFold(bool b)
			{
				base._E000();
			}

			public void BlindFire_Internal(int b)
			{
				this._E002.Blindfire = b != 0;
				if (b != 0 && this._E002.IsAiming)
				{
					this._E002.IsAiming = false;
				}
				this._E001.ProceduralWeaponAnimation.StartBlindFire(b);
			}

			public virtual void FastForward()
			{
			}

			public virtual bool CanChangeScopeStates(_E6C6[] scopeStates)
			{
				if (scopeStates != null)
				{
					return scopeStates.Length != 0;
				}
				return false;
			}

			public virtual void SetFirearmCompassState(bool active)
			{
				base._E000();
			}

			public virtual void OnMagPulledOutFromWeapon()
			{
				base._E000();
			}

			public virtual void OnMagPuttedToRig()
			{
				base._E000();
			}

			public virtual void OnMagAppeared()
			{
				base._E000();
			}

			public virtual void OnMagInsertedToWeapon()
			{
				base._E000();
			}

			public virtual void OnModChanged()
			{
				base._E000();
			}

			public virtual void OnAddAmmoInChamber()
			{
				base._E000();
			}

			public virtual void RemoveAmmoFromChamber()
			{
				base._E000();
			}

			public virtual void OnOnOffBoltCatchEvent(bool isCaught)
			{
				base._E000();
			}

			public virtual void OnBackpackDropEvent()
			{
				base._E000();
			}

			public virtual void OnFireEvent()
			{
				base._E000();
			}

			public virtual void OnFireEndEvent()
			{
				base._E000();
			}

			public virtual void OnIdleStartEvent()
			{
				base._E000();
			}

			public virtual void OnUtilityOperationStartEvent()
			{
				base._E000();
			}

			public virtual void SetTriggerPressed(bool pressed)
			{
				base._E000();
			}

			public virtual void ShowUncompatibleNotification()
			{
			}

			public virtual void SetInventoryOpened(bool opened)
			{
				base._E000();
			}

			public virtual void SetAiming(bool isAiming)
			{
				base._E000();
			}

			public virtual bool ChangeFireMode(Weapon.EFireMode fireMode)
			{
				base._E000();
				return true;
			}

			public virtual bool CheckFireMode()
			{
				base._E000();
				return false;
			}

			public virtual void OnSprintFinished()
			{
				base._E000();
			}

			public virtual void OnSprintStart()
			{
				base._E000();
			}

			public virtual void OnJumpOrFall()
			{
				base._E000();
			}

			public virtual void OnAimingDisabled()
			{
				base._E000();
			}

			public virtual bool ExamineWeapon()
			{
				base._E000();
				return true;
			}

			public virtual void RollCylinder(Callback callback, bool rollToZeroCamora)
			{
				base._E000();
			}

			public virtual void Execute(_EB72 operation, Callback callback)
			{
				base._E000();
				if (this._E002._E014(operation))
				{
					callback?.Succeed();
				}
				else
				{
					callback?.Fail(string.Format(_ED3E._E000(162053), GetType()));
				}
			}

			public virtual void ReloadMag(_EA6A magazine, [CanBeNull] _EB22 gridItemAddress, [CanBeNull] Callback finishCallback, [CanBeNull] Callback startCallback)
			{
				base._E000();
				finishCallback?.Fail(string.Format(_ED3E._E000(162135), GetType()));
			}

			public virtual void QuickReloadMag(_EA6A magazine, [CanBeNull] Callback finishCallback, [CanBeNull] Callback startCallback)
			{
				base._E000();
				finishCallback?.Fail(string.Format(_ED3E._E000(162145), GetType()));
			}

			public virtual void ReloadGrenadeLauncher(_E9CF ammoPack, [CanBeNull] Callback callback)
			{
				base._E000();
				callback?.Fail(string.Format(_ED3E._E000(162217), GetType()));
			}

			public virtual void ReloadWithAmmo(_E9CF ammoPack, [CanBeNull] Callback finishCallback, [CanBeNull] Callback startCallback)
			{
				base._E000();
				finishCallback?.Fail(string.Format(_ED3E._E000(162217), GetType()));
			}

			public virtual void ReloadRevolverDrum(_E9CF ammoPack, [CanBeNull] Callback finishCallback, [CanBeNull] Callback startCallback, bool quickReload = false)
			{
				base._E000();
				finishCallback?.Fail(string.Format(_ED3E._E000(162285), GetType()));
			}

			public virtual void ReloadBarrels(_E9CF ammoPack, _EB22 placeToPutContainedAmmoMagazine, [CanBeNull] Callback finishCallback, [CanBeNull] Callback startCallback)
			{
				base._E000();
				finishCallback?.Fail(string.Format(_ED3E._E000(162217), GetType()));
			}

			public virtual bool CanStartReload()
			{
				return false;
			}

			public virtual void OnRemoveShellEvent()
			{
				base._E000();
			}

			public virtual void OnShellEjectEvent()
			{
				base._E000();
			}

			public override void Update(float deltaTime)
			{
			}

			public virtual void AddAmmoToMag()
			{
				base._E000();
			}

			public virtual void OnShowAmmo(bool value)
			{
				base._E000();
			}

			public virtual void WeaponAppeared()
			{
				base._E000();
			}

			public virtual void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				base._E000();
			}

			public virtual void HideWeaponComplete()
			{
				base._E000();
			}

			public virtual bool CheckAmmo()
			{
				base._E000();
				return false;
			}

			public virtual bool CheckChamber()
			{
				base._E000();
				return false;
			}

			public virtual void OnMalfunctionOffEvent()
			{
				base._E000();
			}

			public virtual void Pickup(bool p)
			{
				if (_E037.IsIdling())
				{
					_E037.SetPickup(p);
				}
			}

			public virtual void Interact(bool isInteracting, int actionIndex)
			{
				if (_E037.IsIdling())
				{
					_E037.SetInteract(isInteracting, actionIndex);
				}
			}

			public virtual void Loot(bool p)
			{
				if (_E037.IsIdling())
				{
					_E037.SetLooting(p);
				}
			}

			public virtual void UnderbarrelSightingRangeUp()
			{
				base._E000();
			}

			public virtual void ForceSetUnderbarrelRangeIndex(int rangeIndex)
			{
				base._E000();
			}

			public virtual void UnderbarrelSightingRangeDown()
			{
				base._E000();
			}

			public virtual void UseSecondMagForReload()
			{
				base._E000();
			}

			public virtual void ReplaceSecondMag()
			{
				base._E000();
			}

			public virtual void PutMagToRig()
			{
				base._E000();
			}

			public virtual bool ToggleLauncher()
			{
				base._E000();
				return false;
			}

			public virtual bool CanNotBeInterrupted()
			{
				return false;
			}

			public virtual void LauncherAppeared()
			{
				base._E000();
			}

			public virtual void LauncherDisappeared()
			{
				base._E000();
			}

			public virtual void LauncherInventoryUnchamberFromMainWeapon(_EA12 ammo, int camoraIndex, ItemAddress itemAddress, Callback callback)
			{
				base._E000();
			}

			public virtual void LoadLauncherFromMainWeapon(_EA12 ammo, int camoraIndex, ItemAddress itemAddress, Callback callback)
			{
				base._E000();
			}

			public virtual void DropBackpackOperationInvoke(Item item, Callback callback)
			{
				base._E000();
			}

			public virtual void SprintStateChanged(bool value)
			{
				base._E000();
			}

			[CompilerGenerated]
			private void _E002()
			{
				this._E001.Skills.WeaponFixAction.Complete();
			}
		}

		public class _E00F : _E00C
		{
			protected _EA12 _E03A;

			public _E00F(FirearmController controller)
				: base(controller)
			{
			}

			public override void Start()
			{
				base.Start();
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				((_E00E)this)._E001._E0DE.RaiseEvent(new _EAF4(_E039, CommandStatus.Begin));
			}

			public override void Reset()
			{
				_E03A = null;
				base.Reset();
			}

			protected override void PrepareShot()
			{
			}

			public override void OnFireEvent()
			{
				_E030 = true;
				_E03A = _E039.FirstLoadedChamberSlot.ContainedItem as _EA12;
				if (_E03A != null && !_E03A.IsUsed)
				{
					_E03A.IsUsed = true;
					((_E00E)this)._E002._E035(_E03A);
					((_E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(_E03A.IsUsed);
					_E039.FirstLoadedChamberSlot.RemoveItem().OrElse(elseValue: false);
				}
			}

			public override void SetTriggerPressed(bool pressed)
			{
				((_E00E)this)._E002.IsTriggerPressed &= pressed;
			}

			public override void OnFireEndEvent()
			{
				((_E00E)this)._E001._E0DE.RaiseEvent(new _EAF4(_E039, CommandStatus.Succeed));
				SetTriggerPressed(pressed: false);
				_E037.SetFire(fire: false);
				_E039.ShellsInChambers[0] = (AmmoTemplate)_E03A.Template;
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
				State = EOperationState.Finished;
				((_E00E)this)._E002.InitiateOperation<_E011>().Start();
			}

			public override bool CanNotBeInterrupted()
			{
				return true;
			}
		}

		private sealed class _E010 : _E00E
		{
			private Callback _E03B;

			private _EB89 _E03C;

			private bool _E03D;

			private Action _E009;

			private bool _E03E;

			public _E010(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(_EB89 foldOperation, Callback callback)
			{
				_E03D = false;
				_E03C = foldOperation;
				FoldableComponent foldable = _E03C.Foldable;
				_E037.SetInventory(open: false);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				if (foldable.CanBeFolded)
				{
					_E03B = callback;
					_E037.TriggerFold();
				}
				else
				{
					UnityEngine.Debug.LogError(_ED3E._E000(162349));
					OnIdleStartEvent();
				}
			}

			public override void OnFold(bool b)
			{
				_E000();
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
			}

			private new void _E000()
			{
				State = EOperationState.Finished;
				_E002.RecalculateErgonomic();
				_E037.Fold(_E03C.NewValue);
				_E037.SetInventory(_E002.InventoryOpened);
				if (_E03D)
				{
					_E002.InitiateOperation<_E011>().HideWeapon(_E009, _E03E);
				}
				else
				{
					_E002.InitiateOperation<_E011>().Start();
				}
				_E03C = null;
				if (_E03B != null)
				{
					Callback callback = _E03B;
					_E03B = null;
					callback.Succeed();
				}
				base._E001.BodyAnimatorCommon.SetFloat(_E712.WEAPON_SIZE_MODIFIER_PARAM_HASH, _E039.CalculateCellSize().X);
				base._E001.ProceduralWeaponAnimation.UpdateWeaponVariables();
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				_E03D = true;
				_E009 = onHidden;
				_E03E = fastDrop;
			}

			public override void FastForward()
			{
				_E000();
				_E037.Animator.Play(_E037.FullIdleStateName, 1, 0.2f);
			}
		}

		public class _E011 : _E00E
		{
			[CompilerGenerated]
			private new sealed class _E000
			{
				public _E011 _003C_003E4__this;

				public _EA12 ammo;

				public _EB75 oneItemOperation;

				public Callback callback;

				public Action _003C_003E9__2;

				public Action _003C_003E9__3;

				internal void _E000()
				{
					((_E00E)_003C_003E4__this)._E002.InitiateOperation<_E032>().Start(isLauncherEnabled: true, delegate
					{
						((_E00E)_003C_003E4__this)._E002.CurrentOperation.LauncherInventoryUnchamberFromMainWeapon(ammo, 0, oneItemOperation.To1, callback);
					});
				}

				internal void _E001()
				{
					((_E00E)_003C_003E4__this)._E002.CurrentOperation.LauncherInventoryUnchamberFromMainWeapon(ammo, 0, oneItemOperation.To1, callback);
				}

				internal void _E002()
				{
					((_E00E)_003C_003E4__this)._E002.InitiateOperation<_E032>().Start(isLauncherEnabled: true, delegate
					{
						((_E00E)_003C_003E4__this)._E002.CurrentOperation.LoadLauncherFromMainWeapon(ammo, 0, oneItemOperation.To1, callback);
					});
				}

				internal void _E003()
				{
					((_E00E)_003C_003E4__this)._E002.CurrentOperation.LoadLauncherFromMainWeapon(ammo, 0, oneItemOperation.To1, callback);
				}
			}

			private float m__E004;

			private bool _E03F;

			private float _E040;

			private Action _E041;

			private Action _E042;

			public _E011(FirearmController controller)
				: base(controller)
			{
			}

			public override void BlindFire(int b)
			{
				BlindFire_Internal(b);
			}

			public void Start(Action callback = null)
			{
				base.Start();
				base._E001.ProceduralWeaponAnimation.TacticalReload = false;
				_E041 = callback;
				this.m__E004 = 0f;
				_E03F = false;
				_E040 = 0f;
				base._E002.SetAnimatorAndProceduralValues(checkMalf: false);
				if (_E039.IsUnderBarrelDeviceActive)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E032>().Start(isLauncherEnabled: true);
				}
				_E000();
			}

			private new void _E000()
			{
				if (_E041 != null)
				{
					_E041();
					_E041 = null;
				}
			}

			private new void _E001()
			{
				if (_E042 != null)
				{
					_E042();
					_E042 = null;
				}
			}

			public override void OnIdleStartEvent()
			{
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 0f);
				_E001();
			}

			public override void OnEnd()
			{
				base._E002.SetCompassState(active: false);
			}

			public override void Update(float deltaTime)
			{
				if (!base._E002.IsAiming && !base._E002.InventoryOpened && _E037.IsIdling())
				{
					this.m__E004 += deltaTime;
				}
				else
				{
					this.m__E004 = 0f;
				}
				if ((double)this.m__E004 > EFTHardSettings.Instance.IDLING_MAX_TIME && (double)base._E001.MovementIdlingTime > EFTHardSettings.Instance.IDLING_MAX_TIME)
				{
					_E037.Idle();
					this.m__E004 = 0f;
				}
				ProcessAutoshot();
				ProcessRemoveOneOffWeapon();
			}

			protected virtual void ProcessRemoveOneOffWeapon()
			{
				if (base._E002.Item.IsOneOff && base._E002.Item.Repairable.Durability == 0f && base._E001._E0DE.CanThrow(_E039))
				{
					base._E001._E0DE.TryThrowItem(_E039);
				}
			}

			protected virtual void ProcessAutoshot()
			{
				if (!base._E002.Item.MalfState.AutoshotChanceInited || !(base._E002.Item.MalfState.AutoshotTime > 0f))
				{
					return;
				}
				if (base._E002.Item.MalfState.State != 0)
				{
					base._E002.Item.MalfState.AutoshotTime = -1f;
				}
				else if (_E62A.PastTime >= base._E002.Item.MalfState.AutoshotTime)
				{
					base._E002.Item.MalfState.AutoshotTime = -1f;
					Weapon.EFireMode selectedFireMode = base._E002.Item.SelectedFireMode;
					base._E002.Item.FireMode.FireMode = Weapon.EFireMode.single;
					SetTriggerPressed(pressed: true);
					base._E002.Item.FireMode.FireMode = selectedFireMode;
					if (selectedFireMode == Weapon.EFireMode.semiauto)
					{
						SetTriggerPressed(pressed: false);
					}
				}
			}

			public override void OnSprintStart()
			{
				SetAiming(isAiming: false);
			}

			public override void OnSprintFinished()
			{
			}

			public override void OnAimingDisabled()
			{
				SetAiming(isAiming: false);
			}

			public override void OnJumpOrFall()
			{
				SetAiming(isAiming: false);
			}

			public void DisableAimingOnReload()
			{
				if (base._E002.CurrentMasteringLevel < 2)
				{
					SetAiming(isAiming: false);
				}
			}

			public override void SetAiming(bool isAiming)
			{
				if ((!isAiming || EFTHardSettings.Instance.CanAimInState(base._E001.CurrentState.Name)) && (!isAiming || !base._E002.Blindfire) && (!isAiming || !(base._E002.m__E010 > EFTHardSettings.Instance.STOP_AIMING_AT)) && (!isAiming || base._E002.FirearmsAnimator.IsIdling() || base._E002.Item.Template.ReloadMode != Weapon.EReloadMode.OnlyBarrel))
				{
					base._E002.IsAiming = isAiming;
					this.m__E004 = 0f;
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				SetAiming(isAiming: false);
				SetTriggerPressed(pressed: false);
				base._E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}

			public override void SetTriggerPressed(bool pressed)
			{
				if (pressed && !base._E002.SuitableForHandInput)
				{
					return;
				}
				if (!base._E002.CanPressTrigger())
				{
					if (!pressed)
					{
						base._E002.IsTriggerPressed = pressed;
						_E037.SetFire(pressed);
					}
					UnityEngine.Debug.Log(_ED3E._E000(162383));
					return;
				}
				if (pressed)
				{
					ShowUncompatibleNotification();
				}
				if (_E039.HasChambers)
				{
					_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				}
				_E037.SetAmmoOnMag(_E039.GetCurrentMagazineCount());
				base._E002.IsTriggerPressed = pressed;
				if (base._E002.IsTriggerPressed && _E039.MalfState.State == Weapon.EMalfunctionState.None && (_E039.ChamberAmmoCount > 0 || _E039.IsOneOff || (!_E039.HasChambers && _E039.GetCurrentMagazineCount() > 0)) && !(_E039 is _EAD1))
				{
					State = EOperationState.Finished;
					if (base._E002.Item.SelectedFireMode == Weapon.EFireMode.single || base._E002.Item.SelectedFireMode == Weapon.EFireMode.doublet || base._E002.Item.SelectedFireMode == Weapon.EFireMode.semiauto)
					{
						base._E002.InitiateOperation<_E00C>().Start();
					}
					else
					{
						base._E002.InitiateOperation<_E002>().Start();
					}
					return;
				}
				if (base._E002.IsTriggerPressed && _E039.MalfState.State == Weapon.EMalfunctionState.None && _E039 is _EAD1)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E00C>().Start();
					return;
				}
				_E037.SetFire(pressed);
				if (_E039.MalfState.State != Weapon.EMalfunctionState.None && pressed)
				{
					base._E002.FirearmsAnimator.MisfireSlideUnknown(val: false);
					base._E001._E0DE.ExamineMalfunction(_E039);
				}
				if (pressed)
				{
					base._E002.DryShot();
				}
			}

			public override void ShowUncompatibleNotification()
			{
				_EA6A currentMagazine = _E039.GetCurrentMagazine();
				bool flag = currentMagazine?.IsAmmoCompatible(_E039.Chambers) ?? false;
				if (currentMagazine != null && !flag)
				{
					_E857.DisplaySingletonWarningNotification(string.Format(_ED3E._E000(162442).Localized(), currentMagazine.Cartridges.Last.Name.Localized(), _E039.AmmoCaliber));
				}
				_E039.CompatibleAmmo = flag || currentMagazine == null;
			}

			public override bool ChangeFireMode(Weapon.EFireMode fireMode)
			{
				Weapon.EFireMode selectedFireMode = _E039.SelectedFireMode;
				if (_E039.IsBoltCatch && _E039.NoFiremodeOnBoltcatch && _E037.GetBoltCatch())
				{
					return false;
				}
				if (selectedFireMode != fireMode)
				{
					_E002(fireMode);
					base._E002.Item.FireMode.SetFireMode(fireMode);
					_E037.SetFireMode(base._E002.Item.SelectedFireMode);
					base._E002.SetCompassState(active: false);
					if (base._E001.ArmsAnimatorCommon.HasParameter(_E326.BOOL_FIREMODE_SPRINT))
					{
						base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
					}
					_E003();
					return true;
				}
				_E003();
				return false;
			}

			private new void _E002(Weapon.EFireMode fireMode)
			{
				if (_E039 is _EAD1 && fireMode == Weapon.EFireMode.single)
				{
					if (_E039.CylinderHammerClosed)
					{
						_EB13 obj = _E039.GetCurrentMagazine() as _EB13;
						obj.IncrementCamoraIndex();
						_E037.SetCamoraIndex(obj.CurrentCamoraIndex);
						_E037.HammerArmed();
					}
					_E037.SetDoubleAction(0f);
					_E039.CylinderHammerClosed = false;
				}
			}

			public override bool CheckFireMode()
			{
				_E003();
				if (_E039.FireMode.AvailableEFireModes.Length <= 1)
				{
					return false;
				}
				if (!base._E002._player._inventoryController.CheckItemAction(base._E002.Item, base._E002.Item.CurrentAddress))
				{
					return false;
				}
				SetAiming(isAiming: false);
				if (!(_E039 is _EAD1))
				{
					_E037.TriggerFiremodeCheck();
				}
				if (!(_E039 is _EAD1))
				{
					RunUtilityOperation(FirearmController._E034.EUtilityType.CheckFireMode);
				}
				return true;
			}

			private void _E003()
			{
				if (_E039 != null && base._E001.PointOfView == EPointOfView.FirstPerson)
				{
					Singleton<GameUI>.Instance.BattleUiScreen.ShowFireMode(_E039.SelectedFireMode);
				}
			}

			public override void ShowGesture(EGesture gesture)
			{
				_E037.Gesture(gesture);
			}

			public override void SetFirearmCompassState(bool active)
			{
				if (!active || !base._E002.Blindfire)
				{
					base._E002.CompassState.Value = active;
					if (active && base._E002.IsAiming)
					{
						base._E002.SetAim(value: false);
					}
				}
			}

			public override bool ExamineWeapon()
			{
				if (base._E002.Item.MalfState.State != 0)
				{
					if ((base._E002.Item.MalfState.State == Weapon.EMalfunctionState.Misfire || base._E002.Item.MalfState.State == Weapon.EMalfunctionState.SoftSlide || base._E002.Item.MalfState.State == Weapon.EMalfunctionState.HardSlide) && !base._E002.Item.MalfState.IsKnownMalfunction(base._E001.ProfileId))
					{
						if (base._E002.IsAiming)
						{
							return false;
						}
						RunUtilityOperation(FirearmController._E034.EUtilityType.ExamineWeapon);
					}
					else
					{
						State = EOperationState.Finished;
						base._E002.InitiateOperation<_E009>().Start();
					}
				}
				else
				{
					if (base._E002.IsAiming)
					{
						return false;
					}
					RunUtilityOperation(FirearmController._E034.EUtilityType.ExamineWeapon);
				}
				_E037.LookTrigger();
				return true;
			}

			public override void RollCylinder(Callback finishCallback, bool rollToZeroCamora)
			{
				if (_E039 is _EAD1)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E02E>().Start(finishCallback, rollToZeroCamora);
				}
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				BlindFire(0);
				SetAiming(isAiming: false);
				SetTriggerPressed(pressed: false);
				State = EOperationState.Finished;
				base._E002.InitiateOperation<_E02B>().Start(onHidden, fastDrop, nextControllerItem);
			}

			public override bool CanRemove()
			{
				return true;
			}

			public override void ReloadMag(_EA6A magazine, _EB22 gridItemAddress, Callback finishCallback, Callback startCallback)
			{
				DisableAimingOnReload();
				SetTriggerPressed(pressed: false);
				_ECD8<_E021> sourceOption = FirearmController._E021.Run(base._E001._E0DE, _E039, magazine, quickReload: false, _E039.MalfState.IsKnownMalfunction(base._E001.ProfileId), gridItemAddress);
				if (sourceOption.Succeeded)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E020>().Start(sourceOption.Value, finishCallback);
					startCallback?.Succeed();
				}
				else
				{
					finishCallback?.Invoke(sourceOption);
				}
			}

			public override void QuickReloadMag(_EA6A magazine, Callback finishCallback, Callback startCallback)
			{
				DisableAimingOnReload();
				SetTriggerPressed(pressed: false);
				_ECD8<_E021> sourceOption = FirearmController._E021.Run(base._E001._E0DE, _E039, magazine, quickReload: true, _E039.MalfState.IsKnownMalfunction(base._E001.ProfileId), null);
				if (sourceOption.Succeeded)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E020>().Start(sourceOption.Value, finishCallback);
					startCallback?.Succeed();
				}
				else
				{
					finishCallback?.Invoke(sourceOption);
				}
			}

			public override void ReloadWithAmmo(_E9CF ammoPack, Callback finishCallback, Callback startCallback)
			{
				DisableAimingOnReload();
				SetTriggerPressed(pressed: false);
				_EA6A currentMagazine = _E039.GetCurrentMagazine();
				if (currentMagazine != null && currentMagazine.Count < currentMagazine.MaxCount)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E022>().Start(ammoPack, finishCallback);
					startCallback?.Succeed();
				}
				else
				{
					finishCallback?.Fail(_ED3E._E000(162466));
				}
			}

			public override void ReloadRevolverDrum(_E9CF ammoPack, Callback finishCallback, Callback startCallback, bool quickReload = false)
			{
				DisableAimingOnReload();
				SetTriggerPressed(pressed: false);
				_EA6A currentMagazine = _E039.GetCurrentMagazine();
				if (currentMagazine != null && (quickReload || currentMagazine.Count < currentMagazine.MaxCount))
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E01F>().Start(ammoPack, finishCallback, quickReload);
					startCallback?.Succeed();
				}
				else
				{
					finishCallback?.Fail(_ED3E._E000(162466));
				}
			}

			public override void ReloadBarrels(_E9CF ammoPack, _EB22 placeToPutContainedAmmoMagazine, Callback finishCallback, Callback startCallback)
			{
				DisableAimingOnReload();
				SetTriggerPressed(pressed: false);
				if (_E039.Chambers.Length > 1)
				{
					_ECD8<_E027> sourceOption = FirearmController._E027.Run(base._E001._E0DE, base._E001._E0DE, _E039, ammoPack, placeToPutContainedAmmoMagazine);
					if (sourceOption.Error == null)
					{
						State = EOperationState.Finished;
						base._E002.InitiateOperation<_E025>().Start(sourceOption.Value, finishCallback);
						startCallback?.Succeed();
					}
					else
					{
						finishCallback?.Invoke(sourceOption);
					}
				}
				else
				{
					_ECD8<_E029> sourceOption2 = FirearmController._E029.Run(base._E001._E0DE, base._E001._E0DE, _E039, ammoPack.GetAmmoToReload(0), placeToPutContainedAmmoMagazine);
					if (sourceOption2.Error == null)
					{
						State = EOperationState.Finished;
						base._E002.InitiateOperation<_E028>().Start(sourceOption2.Value, finishCallback);
						startCallback?.Succeed();
					}
					else
					{
						finishCallback?.Invoke(sourceOption2);
					}
				}
			}

			public override bool CheckAmmo()
			{
				if (base._E002 == null || _E039 == null)
				{
					UnityEngine.Debug.LogError(_ED3E._E000(162549));
					return false;
				}
				if (_E039.GetCurrentMagazine() == null || !base._E002._player._inventoryController.CheckItemAction(base._E002.Item, base._E002.Item.CurrentAddress) || !base._E002._player._inventoryController.CheckItemAction(_E039.GetCurrentMagazine(), _E039.GetCurrentMagazine().CurrentAddress))
				{
					return false;
				}
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Feed || (_E039.MalfState.State != 0 && _E039.IsBoltCatch))
				{
					SetAiming(isAiming: false);
					base._E002.FirearmsAnimator.MisfireSlideUnknown(val: false);
					base._E001._E0DE.ExamineMalfunction(base._E002.Item);
					return false;
				}
				if (_E039 is _EAD1 || _E039.Chambers.Length > 1)
				{
					base._E001._E0DE.CheckChamber(_E039, status: true);
				}
				_EA6A currentMagazine = _E039.GetCurrentMagazine();
				if (base._E001.PointOfView == EPointOfView.FirstPerson)
				{
					_EA12 obj = ((!(_E039 is _EAD1)) ? (currentMagazine.Cartridges.Peek() as _EA12) : ((_EB13)currentMagazine)?.GetFirstAmmo(!_E039.CylinderHammerClosed));
					Singleton<GameUI>.Instance.BattleUiScreen.ShowAmmoDetails(currentMagazine.Count, currentMagazine.MaxCount, Mathf.Max(base._E001.Profile.MagDrillsMastering, currentMagazine.CheckOverride), obj?.Name.Localized(), _E039 is _EAD1 || _E039.Chambers.Length > 1);
					base._E001._E0DE.StrictCheckMagazine(currentMagazine, status: true);
				}
				SetAiming(isAiming: false);
				_E037.CheckAmmo();
				bool isExternalMag = _E039.ReloadMode == Weapon.EReloadMode.ExternalMagazine || _E039.ReloadMode == Weapon.EReloadMode.ExternalMagazineWithInternalReloadSupport || (_E039.ReloadMode == Weapon.EReloadMode.InternalMagazine && _E039.GetCurrentMagazine() == null);
				_E037.SetIsExternalMag(isExternalMag);
				if (base._E002._player.MovementContext.StationaryWeapon != null)
				{
					return true;
				}
				RunUtilityOperation(FirearmController._E034.EUtilityType.CheckMagazine);
				return true;
			}

			public override bool CheckChamber()
			{
				if (base._E002.IsTriggerPressed)
				{
					return false;
				}
				if (base._E002._player.MovementContext.StationaryWeapon != null)
				{
					return false;
				}
				if (_E039 is _EAD1)
				{
					return false;
				}
				if (_E039.MalfState.State == Weapon.EMalfunctionState.None)
				{
					if (!base._E002._player._inventoryController.CheckItemAction(base._E002.Item, base._E002.Item.CurrentAddress))
					{
						return false;
					}
					SetAiming(isAiming: false);
					_E037.CheckChamber();
					base._E001._E0DE.CheckChamber(_E039, status: true);
					RunUtilityOperation(FirearmController._E034.EUtilityType.CheckChamber);
				}
				else
				{
					SetAiming(isAiming: false);
					if (_E039.MalfState.IsKnownMalfType(base._E001.ProfileId))
					{
						State = EOperationState.Finished;
						base._E002.InitiateOperation<_E02D>().Start();
					}
					else
					{
						_E037.MisfireSlideUnknown(val: false);
						base._E001._E0DE.ExamineMalfunction(base._E002.Item);
					}
				}
				return true;
			}

			public override bool CanStartReload()
			{
				return true;
			}

			protected virtual void RunUtilityOperation(_E034.EUtilityType utilityType)
			{
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				State = EOperationState.Finished;
				base._E002.InitiateOperation<_E034>().Start(utilityType);
			}

			public override void Execute(_EB72 operation, Callback callback)
			{
				if (operation is _EB8D obj)
				{
					if (!(obj.InternalOperation is _EB75 obj2) || !_E039.SupportsInternalReload || !(obj2.Item1 is _EA12 item))
					{
						callback?.Fail(string.Format(_ED3E._E000(162575), _E039.ReloadMode));
						return;
					}
					base._E002.ReloadWithAmmo(new _E9CF(new List<_EA12> { item }), callback);
					return;
				}
				if (!(operation is _EB75 obj3))
				{
					callback.Succeed();
					return;
				}
				if (!base._E002._E015(operation))
				{
					if (obj3.From1 != null && base._E001._inventoryController.IsInAnimatedSlot(obj3.Item1))
					{
						State = EOperationState.Finished;
						base._E002.InitiateOperation<_E008>().Start(obj3.Item1, callback);
					}
					else
					{
						callback.Succeed();
					}
					return;
				}
				base._E002.IsTriggerPressed = false;
				if (operation is _EB89 foldOperation)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E010>().Start(foldOperation, callback);
				}
				else if (obj3.Item1 is _EA12 ammo)
				{
					_EA6A currentMagazine = _E039.GetCurrentMagazine();
					bool flag = _E039 is _EAD1;
					if (!_E039.HasChambers && !flag)
					{
						callback.Fail(string.Format(_ED3E._E000(162641), operation));
						return;
					}
					_EA62 underbarrelWeapon = base._E002.UnderbarrelWeapon;
					if (underbarrelWeapon != null && ((obj3.From1 != null && obj3.From1.Container == underbarrelWeapon.Chamber) || (obj3.To1 != null && obj3.To1.Container == underbarrelWeapon.Chamber)))
					{
						_E004(obj3, ammo, callback);
						return;
					}
					if (flag)
					{
						_E005(obj3, ammo, callback);
						return;
					}
					if (operation is _EBA0 obj4)
					{
						if (_E039.ReloadMode != Weapon.EReloadMode.OnlyBarrel)
						{
							State = EOperationState.Finished;
							base._E002.InitiateOperation<_E01E>().Start((_EA12)obj4.Item, callback);
						}
						else
						{
							callback.Fail(_ED3E._E000(162703));
						}
						return;
					}
					string text = null;
					for (int i = 0; i < _E039.Chambers.Length; i++)
					{
						Slot slot = _E039.Chambers[i];
						Item containedItem = slot.ContainedItem;
						if (obj3.From1 != null && obj3.From1.Container == slot && containedItem == obj3.Item1)
						{
							if ((_E039.ReloadMode == Weapon.EReloadMode.ExternalMagazine || _E039.ReloadMode == Weapon.EReloadMode.ExternalMagazineWithInternalReloadSupport) && currentMagazine != null)
							{
								text = string.Format(_ED3E._E000(162780), operation);
								continue;
							}
							if (_E039.ReloadMode != Weapon.EReloadMode.InternalMagazine || _E039.GetCurrentMagazineCount() <= 0)
							{
								ItemAddress destinationAddress = null;
								if (obj3 is _EB77 obj5 && obj5.BaseInventoryOperation is _EB91 obj6 && _E039.Chambers.Contains(obj6.To.Container))
								{
									destinationAddress = obj6.To;
								}
								State = EOperationState.Finished;
								base._E002.InitiateOperation<_E007>().Start((_EA12)obj3.Item1, i, destinationAddress, callback);
								base._E001.ExecuteSkill((Action)delegate
								{
									base._E001.Skills.WeaponChamberAction.Complete(_E039);
								});
								return;
							}
							text = string.Format(_ED3E._E000(162792), operation);
						}
						else
						{
							if (obj3.To1 == null || obj3.To1.Container != slot)
							{
								continue;
							}
							if (!_E039.CanLoadAmmoToChamber)
							{
								text = string.Format(_ED3E._E000(156731), operation);
								continue;
							}
							if ((_E039.ReloadMode == Weapon.EReloadMode.ExternalMagazine || _E039.ReloadMode == Weapon.EReloadMode.ExternalMagazineWithInternalReloadSupport) && currentMagazine != null)
							{
								text = string.Format(_ED3E._E000(162780), operation);
								continue;
							}
							if (_E039.ReloadMode != Weapon.EReloadMode.InternalMagazine || currentMagazine == null || currentMagazine.Cartridges.Count != currentMagazine.MaxCount || slot.CanAccept(currentMagazine.Cartridges.Last))
							{
								State = EOperationState.Finished;
								base._E002.InitiateOperation<_E01A>().Start((_EA12)obj3.Item1, i, callback);
								base._E001.ExecuteSkill((Action)delegate
								{
									base._E001.Skills.WeaponChamberAction.Complete(_E039);
								});
								return;
							}
							text = string.Format(_ED3E._E000(156826), operation);
						}
					}
					callback.Fail(text ?? string.Format(_ED3E._E000(156869), operation, obj3.Item1, obj3.From1, obj3.To1));
				}
				else if (obj3.Item1 is _EA6A)
				{
					if (_E039.ReloadMode == Weapon.EReloadMode.ExternalMagazine || _E039.ReloadMode == Weapon.EReloadMode.ExternalMagazineWithInternalReloadSupport)
					{
						if (obj3.To1 != null && obj3.To1.IsChildOf(_E039, notMergedWithThisItem: false))
						{
							_ECD9<_E013> sourceOption = FirearmController._E013.Run(base._E001._E0DE, _E039, base._E001.ProfileId);
							if (sourceOption.Succeeded)
							{
								State = EOperationState.Finished;
								base._E002.InitiateOperation<_E012>().Start(sourceOption.Value, callback);
							}
							else
							{
								callback.Invoke(sourceOption);
							}
							return;
						}
						if (obj3.From1 != null && obj3.From1.IsChildOf(_E039, notMergedWithThisItem: false))
						{
							State = EOperationState.Finished;
							base._E002.InitiateOperation<_E01D>().Start((_EA6A)obj3.Item1, (Slot)obj3.From1.Container, callback);
							return;
						}
					}
					UnityEngine.Debug.LogErrorFormat(_ED3E._E000(156959), operation);
					callback.Succeed();
				}
				else if (obj3.Item1 is Mod)
				{
					if (obj3.From1 != null && obj3.From1.IsChildOf(_E039, notMergedWithThisItem: false))
					{
						State = EOperationState.Finished;
						base._E002.InitiateOperation<_E02A>().Start(obj3.Item1, (Slot)obj3.From1.Container, callback);
					}
					else if (obj3.To1 != null && obj3.To1.IsChildOf(_E039, notMergedWithThisItem: false))
					{
						State = EOperationState.Finished;
						base._E002.InitiateOperation<_E001>().Start(obj3.Item1, (Slot)obj3.To1.Container, callback);
					}
					else
					{
						UnityEngine.Debug.LogErrorFormat(_ED3E._E000(156973), operation);
						callback.Succeed();
					}
				}
				else
				{
					UnityEngine.Debug.LogErrorFormat(_ED3E._E000(156959), operation);
					callback.Succeed();
				}
			}

			private void _E004(_EB75 oneItemOperation, _EA12 ammo, Callback callback)
			{
				State = EOperationState.Finished;
				_E037.SetInventory(open: false);
				if (oneItemOperation.From1 != null)
				{
					_E042 = delegate
					{
						base._E002.InitiateOperation<_E032>().Start(isLauncherEnabled: true, delegate
						{
							base._E002.CurrentOperation.LauncherInventoryUnchamberFromMainWeapon(ammo, 0, oneItemOperation.To1, callback);
						});
					};
				}
				else
				{
					_E042 = delegate
					{
						base._E002.InitiateOperation<_E032>().Start(isLauncherEnabled: true, delegate
						{
							base._E002.CurrentOperation.LoadLauncherFromMainWeapon(ammo, 0, oneItemOperation.To1, callback);
						});
					};
				}
				_E001();
			}

			private void _E005(_EB75 oneItemOperation, _EA12 ammo, Callback callback)
			{
				_EB13 obj = _E039.GetCurrentMagazine() as _EB13;
				if (oneItemOperation is _EBA0 obj2)
				{
					if (_E039.ReloadMode != Weapon.EReloadMode.OnlyBarrel)
					{
						State = EOperationState.Finished;
						base._E002.InitiateOperation<_E01E>().Start((_EA12)obj2.Item, callback);
					}
					else
					{
						callback.Fail(_ED3E._E000(162703));
					}
					return;
				}
				ItemAddress itemAddress = null;
				if (oneItemOperation is _EB77 obj3 && obj3.BaseInventoryOperation is _EB91 obj4 && obj.Camoras.Contains(obj4.To.Container))
				{
					itemAddress = obj4.To;
				}
				for (int i = 0; i < obj.Camoras.Length; i++)
				{
					Slot slot = obj.Camoras[i];
					Item containedItem = slot.ContainedItem;
					if (oneItemOperation.From1 != null && oneItemOperation.From1.Container == slot && containedItem == oneItemOperation.Item1)
					{
						if ((_E039.ReloadMode != 0 && _E039.ReloadMode != Weapon.EReloadMode.ExternalMagazineWithInternalReloadSupport) || _E039.GetCurrentMagazine() == null)
						{
							State = EOperationState.Finished;
							base._E002.InitiateOperation<_E005>().Start((_EA12)oneItemOperation.Item1, i, itemAddress, callback);
							base._E001.ExecuteSkill((Action)delegate
							{
								base._E001.Skills.WeaponChamberAction.Complete(_E039);
							});
							return;
						}
					}
					else
					{
						if (oneItemOperation.To1 == null || oneItemOperation.To1.Container != slot)
						{
							continue;
						}
						if (!_E039.CanLoadAmmoToChamber)
						{
							callback.Fail(_ED3E._E000(156993) + oneItemOperation);
							continue;
						}
						if ((_E039.ReloadMode != 0 && _E039.ReloadMode != Weapon.EReloadMode.ExternalMagazineWithInternalReloadSupport) || _E039.GetCurrentMagazine() == null)
						{
							State = EOperationState.Finished;
							base._E002.InitiateOperation<_E019>().Start((_EA12)oneItemOperation.Item1, i, callback);
							base._E001.ExecuteSkill((Action)delegate
							{
								base._E001.Skills.WeaponChamberAction.Complete(_E039);
							});
							return;
						}
						callback.Fail(string.Format(_ED3E._E000(162780), oneItemOperation));
					}
				}
				callback.Fail(string.Format(_ED3E._E000(157093), oneItemOperation));
			}

			public override bool ToggleLauncher()
			{
				if (base._E002.UnderbarrelWeapon != null)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E032>().Start(isLauncherEnabled: true);
					base._E001.ProceduralWeaponAnimation.IsGrenadeLauncher = true;
					return true;
				}
				return false;
			}

			public override void OnOnOffBoltCatchEvent(bool isCatched)
			{
				_E037.SetBoltCatch(isCatched);
			}

			public override void DropBackpackOperationInvoke(Item item, Callback callback)
			{
				State = EOperationState.Finished;
				base._E002.InitiateOperation<_E008>().Start(item, callback);
			}

			[CompilerGenerated]
			private void _E006()
			{
				base._E001.Skills.WeaponChamberAction.Complete(_E039);
			}

			[CompilerGenerated]
			private void _E007()
			{
				base._E001.Skills.WeaponChamberAction.Complete(_E039);
			}

			[CompilerGenerated]
			private void _E008()
			{
				base._E001.Skills.WeaponChamberAction.Complete(_E039);
			}

			[CompilerGenerated]
			private void _E009()
			{
				base._E001.Skills.WeaponChamberAction.Complete(_E039);
			}
		}

		public sealed class _E012 : _E00E
		{
			private new Callback m__E000;

			private _E013 _E043;

			private bool _E044;

			private bool _E00E;

			private bool _E045;

			public _E012(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(_E013 insertMagResult, Callback callback)
			{
				_E043 = insertMagResult;
				this.m__E000 = callback;
				Start();
				_E037.SetFire(fire: false);
				_E037.SetIsExternalMag(isExternalMag: true);
				_E037.SetCanReload(canReload: true);
				_E037.SetMagTypeCurrent(_E043.Magazine.magAnimationIndex);
				_E037.SetMagTypeNew(_E043.Magazine.magAnimationIndex);
				_E037.InsertMagInInventoryMode();
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				if (_E039.IsBoltCatch && _E039.ChamberAmmoCount == 1 && _E043.AddNewAmmoResult == null && !_E039.ManualBoltCatch && !_E039.MustBoltBeOpennedForExternalReload && !_E039.MustBoltBeOpennedForInternalReload)
				{
					_E037.SetBoltCatch(active: false);
				}
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire && _E039.MalfState.IsKnownMalfunction(base._E001.ProfileId) && _E043.Magazine.Count > 0 && _E043.AmmoCompatible)
				{
					_E037.SetAmmoInChamber(0f);
					_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 0);
				}
			}

			public override void Reset()
			{
				this.m__E000 = null;
				_E043 = null;
				_E044 = false;
				_E00E = false;
				_E045 = false;
				base.Reset();
			}

			public override void OnMagAppeared()
			{
				if (!_E00E)
				{
					_E00E = true;
					_E038.SetupMod(_E043.MagazineSlot.Slot, Singleton<_E760>.Instance.CreateItem(_E043.Magazine, isAnimated: true));
				}
			}

			public override void OnMagInsertedToWeapon()
			{
				if (!_E045)
				{
					_E045 = true;
					_E037.SetAmmoOnMag(_E043.MagazineAmmoCount + ((_E043.AddNewAmmoResult != null) ? 1 : 0));
					_E037.SetMagInWeapon(ok: true);
					_E037.SetAmmoCompatible(_E043.AmmoCompatible);
					if (_E043.AddNewAmmoResult == null && (_E039.MalfState.State != Weapon.EMalfunctionState.Misfire || !_E039.MalfState.IsKnownMalfunction(base._E001.ProfileId) || !_E043.AmmoCompatible || _E043.Magazine.Count <= 0))
					{
						_E000();
					}
				}
			}

			public override void OnOnOffBoltCatchEvent(bool isCaught)
			{
				_E037.SetBoltCatch(isCaught);
			}

			public override void OnAddAmmoInChamber()
			{
				if (!_E044)
				{
					_E044 = true;
					_E037.SetAmmoOnMag(_E043.Magazine.Count);
					if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
					{
						base._E000();
					}
					if (_E043.AddNewAmmoResult != null)
					{
						_E038.SetRoundIntoWeapon((_EA12)_E043.AddNewAmmoResult.ResultItem);
					}
					_E037.SetAmmoInChamber(_E043.Weapon.ChamberAmmoCount);
					_E000();
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}

			private new void _E000()
			{
				State = EOperationState.Finished;
				_E002.RecalculateErgonomic();
				_E002.InitiateOperation<_E011>().Start();
				this.m__E000.Succeed();
				_E002.WeaponModified();
			}

			public override bool CanChangeLightState(_E6C5[] lightsStates)
			{
				return false;
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					OnMagAppeared();
					OnMagInsertedToWeapon();
					OnAddAmmoInChamber();
					if (State != EOperationState.Finished)
					{
						_E000();
					}
				}
			}
		}

		public class _E013 : _EB2D, _E6A1
		{
			public readonly Weapon Weapon;

			public readonly _EA6A Magazine;

			public readonly int MagazineAmmoCount;

			public readonly _EB20 MagazineSlot;

			public readonly bool AmmoCompatible;

			[CanBeNull]
			public readonly _EB3A RemoveOldAmmoResult;

			[CanBeNull]
			public readonly _EB2E AddNewAmmoResult;

			public _E013(Weapon weapon, _EA6A magazine, bool ammoCompatible, [CanBeNull] _EB3A removeOldAmmoResult, [CanBeNull] _EB2E addNewAmmoResult)
			{
				Weapon = weapon;
				Magazine = magazine;
				MagazineAmmoCount = magazine.Count;
				MagazineSlot = (_EB20)magazine.Parent;
				AmmoCompatible = ammoCompatible;
				RemoveOldAmmoResult = removeOldAmmoResult;
				AddNewAmmoResult = addNewAmmoResult;
			}

			public void RollBack()
			{
				AddNewAmmoResult?.RollBack();
				RemoveOldAmmoResult?.RollBack();
			}

			public void RaiseEvents(_EB1E controller, CommandStatus status)
			{
				AddNewAmmoResult?.RaiseEvents(controller, status);
				RemoveOldAmmoResult?.RaiseEvents(controller, status);
			}

			public static _ECD9<_E013> Run(_EAED inventoryController, Weapon weapon, string playerId)
			{
				_EA6A currentMagazine = weapon.GetCurrentMagazine();
				Slot[] chambers = weapon.Chambers;
				_EB20 obj = (weapon.HasChambers ? new _EB20(chambers[0]) : null);
				bool flag = currentMagazine.IsAmmoCompatible(chambers);
				_ECD8<_EB3A> obj2 = ((obj != null && weapon.MustBoltBeOpennedForExternalReload && obj.Slot.ContainedItem != null) ? _EB29.Remove(obj.Slot.ContainedItem, inventoryController) : default(_ECD8<_EB3A>));
				if (obj2.Failed)
				{
					return obj2.Error;
				}
				bool flag2 = weapon.MalfState.State == Weapon.EMalfunctionState.None || (weapon.MalfState.State == Weapon.EMalfunctionState.Misfire && weapon.MalfState.IsKnownMalfunction(playerId));
				Weapon.EMalfunctionState state = weapon.MalfState.State;
				if (flag2 && weapon.MalfState.State != 0)
				{
					weapon.MalfState.ChangeStateSilent(Weapon.EMalfunctionState.None);
				}
				_ECD8<_EB2E> obj3 = ((obj != null && obj.Slot.ContainedItem == null && currentMagazine.Count > 0 && flag && flag2) ? currentMagazine.Cartridges.PopTo(inventoryController, obj) : default(_ECD8<_EB2E>));
				if (flag2 && state != 0)
				{
					weapon.MalfState.ChangeStateSilent(state);
				}
				if (obj3.Failed)
				{
					obj2.Value?.RollBack();
					return obj3.Error;
				}
				return new _E013(weapon, currentMagazine, flag, obj2.Value, obj3.Value);
			}

			public _ECD7 Replay()
			{
				throw new NotImplementedException();
			}

			public bool CanExecute(_EB1E itemController)
			{
				return true;
			}
		}

		public class _E014 : _E00C
		{
			private bool _E046;

			public _E014(FirearmController controller)
				: base(controller)
			{
			}

			public new void Start()
			{
				base.Start();
				_E037.Animator.Play(_E037.FullFireStateName, 1, 0.2f);
			}

			protected override void PrepareShot()
			{
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
			}

			public override void Reset()
			{
				_E046 = false;
				base.Reset();
			}

			protected new void _E000()
			{
				((_E00E)this)._E002.IsTriggerPressed = false;
				_E037.SetFire(fire: false);
				State = EOperationState.Finished;
				((_E00E)this)._E002.InitiateOperation<_E015>().Start();
			}

			public override void OnFireEndEvent()
			{
				_E000();
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E046 = opened;
			}

			public override void OnFireEvent()
			{
				if (!_E030)
				{
					FirearmsAnimator firearmsAnimator = ((_E00E)this)._E002.m__E00D.FirearmsAnimator;
					firearmsAnimator.ResetGestureTrigger();
					firearmsAnimator.ResetHandReadyTrigger();
					IAnimator animator = firearmsAnimator.Animator;
					animator.SetLayerWeight(animator.GetLayerIndex(_ED3E._E000(45316)), 0f);
					animator.Play(_ED3E._E000(51049), animator.GetLayerIndex(_ED3E._E000(45316)), 0f);
					_EA62 underbarrelWeapon = ((_E00E)this)._E002.UnderbarrelWeapon;
					_EA12 obj = underbarrelWeapon.Chamber.ContainedItem as _EA12;
					((_E00E)this)._E002._E036(underbarrelWeapon, obj);
					underbarrelWeapon.Chamber.RemoveItem();
					obj.IsUsed = true;
					_E030 = true;
					if (obj.AmmoTemplate.RemoveShellAfterFire)
					{
						((_E00E)this)._E002.m__E01F.DestroyPatronInWeapon();
					}
					else
					{
						((_E00E)this)._E002.m__E01F.MoveAmmoFromChamberToShellPort(obj.IsUsed);
					}
					if (!obj.AmmoTemplate.RemoveShellAfterFire)
					{
						underbarrelWeapon.ShellsInChambers[0] = obj.AmmoTemplate;
					}
					_E037.SetShellsInWeapon((!obj.AmmoTemplate.RemoveShellAfterFire) ? 1 : 0);
					_E037.SetAmmoInChamber(0f);
				}
			}
		}

		public class _E015 : _E00E
		{
			[CompilerGenerated]
			private new sealed class _E000
			{
				public _E015 _003C_003E4__this;

				public Item item;

				public Callback callback;

				internal void _E000()
				{
					((_E00E)_003C_003E4__this)._E002.CurrentOperation.DropBackpackOperationInvoke(item, callback);
				}
			}

			[CompilerGenerated]
			private new sealed class _E001
			{
				public _E015 _003C_003E4__this;

				public Action onHidden;

				public bool fastDrop;

				public Item nextControllerItem;

				internal void _E000()
				{
					((_E00E)_003C_003E4__this)._E002.CurrentOperation.HideWeapon(onHidden, fastDrop, nextControllerItem);
				}
			}

			protected _EA62 _E047;

			private new Action m__E000;

			private bool _E048;

			public _E015(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(Action callback = null)
			{
				base.Start();
				this.m__E000 = callback;
				_E047 = base._E002.UnderbarrelWeapon;
				_E326.SetUseLeftHand(_E037.Animator, useLeftHand: false);
				_E037.SetAmmoInChamber(_E047.ChamberAmmoCount);
				_E037.SetInventory(base._E002.m__E002);
				_E037.ResetCheckChamberTrigger();
				_E037.SetFire(fire: false);
				this.m__E000?.Invoke();
			}

			public override bool ToggleLauncher()
			{
				_E000();
				return true;
			}

			private new void _E000()
			{
				State = EOperationState.Finished;
				base._E002.InitiateOperation<_E032>().Start(isLauncherEnabled: false);
			}

			private new void _E001()
			{
				base._E001.UpdateLauncherBones(value: false, base._E002.m__E01F.LauncherWeaponPrefab);
				_E048 = true;
			}

			public override void ForceSetUnderbarrelRangeIndex(int rangeIndex)
			{
				_E047.ForceSetSightRangeIndex(rangeIndex);
			}

			public override void UnderbarrelSightingRangeDown()
			{
				_E047.SightingRangeDown();
				_E002();
			}

			public override void UnderbarrelSightingRangeUp()
			{
				_E047.SightingRangeUp();
				_E002();
			}

			private new void _E002()
			{
				if (Singleton<GameUI>.Instantiated && Singleton<GameUI>.Instance.BattleUiScreen != null)
				{
					Singleton<GameUI>.Instance.BattleUiScreen.ShowAmmoCountZeroingPanel(_E047.RangeValue.ToString() ?? "");
				}
			}

			public override void SprintStateChanged(bool value)
			{
				_E037.SetSprint(value);
			}

			public override void Update(float deltaTime)
			{
				if (_E048 && !_E037.IsIdling())
				{
					base._E001.UpdateLauncherBones(value: true, base._E002.m__E01F.LauncherWeaponPrefab);
					_E048 = false;
				}
				SetSightingRange(deltaTime);
			}

			protected virtual void SetSightingRange(float deltaTime)
			{
				int lastRangeValue = _E047.LastRangeValue;
				if (lastRangeValue == _E047.RangeValue)
				{
					return;
				}
				int num = 0;
				if (lastRangeValue > _E047.RangeValue)
				{
					num = (int)Mathf.Clamp((float)lastRangeValue - (float)_E047.RangeUpSpeed * deltaTime, _E047.RangeValue, lastRangeValue);
				}
				else
				{
					if (lastRangeValue >= _E047.RangeValue)
					{
						return;
					}
					num = (int)Mathf.Clamp((float)lastRangeValue + (float)_E047.RangeUpSpeed * deltaTime, lastRangeValue, _E047.RangeValue);
				}
				_E047.LastRangeValue = num;
				_E037.SetUnderbarrelSightingRange(num);
			}

			public override void SetTriggerPressed(bool pressed)
			{
				base._E002.IsTriggerPressed = pressed;
				if (pressed && !base._E001.StateIsSuitableForHandInput)
				{
					return;
				}
				if (pressed && _E047.Chamber.ContainedItem != null)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E014>().Start();
					return;
				}
				base._E002._E001.SetFire(pressed);
				if (pressed)
				{
					base._E002.DryShot(0, underbarrelShot: true);
				}
			}

			public override bool ExamineWeapon()
			{
				if (base._E002.IsAiming)
				{
					return false;
				}
				RunUtilityOperation(FirearmController._E034.EUtilityType.ExamineWeapon);
				_E037.LookTrigger();
				return true;
			}

			public override void OnIdleStartEvent()
			{
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 0f);
			}

			public override void ReloadGrenadeLauncher(_E9CF ammoPack, Callback callback)
			{
				if (_E047.Chamber.ContainedItem == null)
				{
					SetAiming(isAiming: false);
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E016>().Start(ammoPack, callback);
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				SetAiming(isAiming: false);
				SetTriggerPressed(pressed: false);
				base._E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}

			public override bool CanStartReload()
			{
				return true;
			}

			public override void SetAiming(bool isAiming)
			{
				if (!isAiming || EFTHardSettings.Instance.CanAimInState(base._E001.CurrentState.Name))
				{
					if (base._E002.m__E010 > EFTHardSettings.Instance.STOP_AIMING_AT && isAiming)
					{
						base._E002.AimingInterruptedByOverlap = false;
					}
					else
					{
						base._E002.IsAiming = isAiming;
					}
				}
			}

			public override void Interact(bool isInteracting, int actionIndex)
			{
				if (_E037.IsIdling())
				{
					base._E002.m__E00D.FirearmsAnimator.SetInteract(isInteracting, actionIndex);
					_E001();
				}
			}

			public override void Pickup(bool pickup)
			{
				if (_E037.IsIdling())
				{
					base._E002.m__E00D.FirearmsAnimator.SetPickup(pickup);
					_E001();
				}
			}

			public override void ShowGesture(EGesture gesture)
			{
				base._E002.m__E00D.FirearmsAnimator.Gesture(gesture);
				_E001();
			}

			public override void LoadLauncherFromMainWeapon(_EA12 ammo, int camoraIndex, ItemAddress itemAddress, Callback callback)
			{
				SetAiming(isAiming: false);
				SetTriggerPressed(pressed: false);
				base._E002.IsTriggerPressed = false;
				State = EOperationState.Finished;
				base._E002.InitiateOperation<_E018>().Start(ammo, 0, callback);
			}

			public override void LauncherInventoryUnchamberFromMainWeapon(_EA12 ammo, int camoraIndex, ItemAddress itemAddress, Callback callback)
			{
				SetAiming(isAiming: false);
				SetTriggerPressed(pressed: false);
				State = EOperationState.Finished;
				base._E002.InitiateOperation<FirearmController._E000>().Start(ammo, 0, itemAddress, callback);
			}

			protected void _E003(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				SetAiming(isAiming: false);
				SetTriggerPressed(pressed: false);
				State = EOperationState.Finished;
				base._E002.InitiateOperation<_E032>().Start(isLauncherEnabled: false);
				base._E002.FirearmsAnimator.Animator.Play(_ED3E._E000(157129), base._E002.FirearmsAnimator.Animator.GetLayerIndex(_ED3E._E000(45316)), 0f);
				base._E002.CurrentOperation.LauncherDisappeared();
				base._E002.CurrentOperation.HideWeapon(onHidden, fastDrop, nextControllerItem);
			}

			public override void DropBackpackOperationInvoke(Item item, Callback callback)
			{
				State = EOperationState.Finished;
				base._E002.InitiateOperation<_E032>().Start(isLauncherEnabled: false, delegate
				{
					base._E002.CurrentOperation.DropBackpackOperationInvoke(item, callback);
				});
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				SetAiming(isAiming: false);
				SetTriggerPressed(pressed: false);
				State = EOperationState.Finished;
				base._E002.InitiateOperation<_E032>().Start(isLauncherEnabled: false, delegate
				{
					base._E002.CurrentOperation.HideWeapon(onHidden, fastDrop, nextControllerItem);
				});
			}

			private void _E004(_EB75 oneItemOperation, _EA12 ammo, Callback callback)
			{
				ItemAddress itemAddress = null;
				Slot chamber = _E047.Chamber;
				_EA12 obj = chamber.ContainedItem as _EA12;
				if (oneItemOperation.From1 != null && oneItemOperation.From1.Container == chamber && obj == oneItemOperation.Item1)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<FirearmController._E000>().Start((_EA12)oneItemOperation.Item1, 0, itemAddress, callback);
					base._E001.ExecuteSkill((Action)delegate
					{
						base._E001.Skills.WeaponChamberAction.Complete(_E039);
					});
				}
				if (oneItemOperation.To1 != null && oneItemOperation.To1.Container == chamber)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E018>().Start((_EA12)oneItemOperation.Item1, 0, callback);
					base._E001.ExecuteSkill((Action)delegate
					{
						base._E001.Skills.WeaponChamberAction.Complete(_E039);
					});
				}
			}

			public override bool CheckChamber()
			{
				if (base._E002.IsTriggerPressed)
				{
					return false;
				}
				if (base._E002._player.MovementContext.StationaryWeapon != null)
				{
					return false;
				}
				if (_E047.UseAmmoWithoutShell && _E047.ChamberAmmoCount == 0)
				{
					return false;
				}
				if (!base._E002._player._inventoryController.CheckItemAction(base._E002.Item, base._E002.Item.CurrentAddress))
				{
					return false;
				}
				SetAiming(isAiming: false);
				_E037.CheckChamber();
				base._E001._E0DE.CheckChamber(_E039, status: true);
				RunUtilityOperation(FirearmController._E034.EUtilityType.CheckChamber);
				return true;
			}

			protected virtual void RunUtilityOperation(_E034.EUtilityType utilityType)
			{
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				State = EOperationState.Finished;
				base._E002.InitiateOperation<_E017>().Start(utilityType, _E047);
			}

			public override void Execute(_EB72 operation, Callback callback)
			{
				if (!(operation is _EB75 obj))
				{
					callback.Succeed();
				}
				else if (base._E002._E015(operation))
				{
					base._E002.IsTriggerPressed = false;
					if (operation is _EB89 foldOperation)
					{
						State = EOperationState.Finished;
						base._E002.InitiateOperation<_E010>().Start(foldOperation, callback);
					}
					else if (obj.Item1 is _EA12 ammo && base._E002.IsInLauncherMode())
					{
						_E004(obj, ammo, callback);
					}
					else
					{
						callback?.Fail(_ED3E._E000(157123));
					}
				}
				else if (obj.From1 != null && base._E001._inventoryController.IsInAnimatedSlot(obj.Item1))
				{
					DropBackpackOperationInvoke(obj.Item1, callback);
				}
				else
				{
					callback.Succeed();
				}
			}

			[CompilerGenerated]
			private void _E005()
			{
				base._E001.Skills.WeaponChamberAction.Complete(_E039);
			}

			[CompilerGenerated]
			private void _E006()
			{
				base._E001.Skills.WeaponChamberAction.Complete(_E039);
			}
		}

		public class _E016 : _E00E
		{
			private Callback _E049;

			private _E9CF _E04A;

			private bool _E046;

			private _EA12 _E00C;

			protected bool _E021;

			protected Action _E022;

			public _E016(FirearmController controller)
				: base(controller)
			{
			}

			public virtual void Start(_E9CF ammoPack, Callback callback)
			{
				Start();
				_E04A = ammoPack;
				_E04A.LockItems();
				_E049 = callback;
				_E037.SetFire(fire: false);
				_E037.Reload(b: true);
				_E000();
			}

			public override void Reset()
			{
				_E04A = null;
				_E049 = null;
				_E00C = null;
				_E046 = false;
				_E022 = null;
				_E021 = false;
				base.Reset();
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E046 = opened;
			}

			public override void OnMagAppeared()
			{
				_E002.m__E01F.SetRoundIntoWeapon(_E00C);
			}

			public override void OnAddAmmoInChamber()
			{
				State = EOperationState.Finished;
				Action action = _E022;
				bool fastDrop = _E021;
				_E015 obj = _E002.InitiateOperation<_E015>();
				obj.Start();
				_E049?.Succeed();
				if (action != null)
				{
					obj.HideWeapon(action, fastDrop);
				}
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				_E022 = onHidden;
				_E021 = fastDrop;
			}

			private new void _E000()
			{
				_E04A.UnlockItems();
				_E00C = _E04A.GetAmmoToReload(1);
				_E039.RaiseRefreshEvent();
				_EA62 underbarrelWeapon = _E002.UnderbarrelWeapon;
				_ECD8<_EB2E> obj = _E04A.LoadAmmo(base._E001._E0DE, base._E001._E0DE, new _EB20(underbarrelWeapon.Chamber));
				if (obj.Error != null)
				{
					UnityEngine.Debug.LogError(_ED3E._E000(157214) + obj.Error);
					return;
				}
				obj.Value.RaiseEvents(base._E001._E0DE, CommandStatus.Begin);
				obj.Value.RaiseEvents(base._E001._E0DE, CommandStatus.Succeed);
				_E00C = (_EA12)obj.Value.Item;
				if (_E00C == null)
				{
					UnityEngine.Debug.LogError(_ED3E._E000(157266));
				}
			}

			public override void OnShellEjectEvent()
			{
				_E002.m__E01F.StartSpawnShell(_E002._player.Velocity * 0.33f);
				for (int i = 0; i < _E002.UnderbarrelWeapon.ShellsInChambers.Length; i++)
				{
					_E002.UnderbarrelWeapon.ShellsInChambers[i] = null;
				}
				_E037.SetShellsInWeapon(_E002.UnderbarrelWeapon.ShellsInLauncherCount);
			}
		}

		public class _E017 : _E015
		{
			private const float _E04B = 2.5f;

			private float _E04C;

			private bool _E04D;

			private _E034.EUtilityType _E04E;

			private new _EA62 _E047;

			public _E017(FirearmController firearmController)
				: base(firearmController)
			{
			}

			public void Start(_E034.EUtilityType utilityType, _EA62 launcher)
			{
				_E04E = utilityType;
				_E047 = launcher;
				_E037.SetShellsInWeapon(_E047.ShellsInLauncherCount);
				State = EOperationState.Executing;
				_E04C = 0f;
			}

			public override void OnIdleStartEvent()
			{
				if (State == EOperationState.Ready)
				{
					base.OnIdleStartEvent();
					State = EOperationState.Finished;
					_E002.InitiateOperation<_E015>().Start();
				}
			}

			public override void OnUtilityOperationStartEvent()
			{
				State = EOperationState.Ready;
			}

			public override bool CanStartReload()
			{
				return false;
			}

			public override void Reset()
			{
				_E04E = FirearmController._E034.EUtilityType.None;
				base.Reset();
			}

			public override bool CheckAmmo()
			{
				return false;
			}

			public override bool CheckChamber()
			{
				return false;
			}

			public override bool CheckFireMode()
			{
				return false;
			}

			public override void ReloadMag(_EA6A magazine, _EB22 gridItemAddress, Callback finishCallback, Callback startCallback)
			{
			}

			public override void ReloadWithAmmo(_E9CF ammoPack, Callback finishCallback, Callback startCallback)
			{
			}

			public override void ReloadRevolverDrum(_E9CF ammoPack, Callback finishCallback, Callback startCallback, bool quickReload = false)
			{
			}

			public override void QuickReloadMag(_EA6A magazine, Callback finishCallback, Callback startCallback)
			{
			}

			public override void ReloadGrenadeLauncher(_E9CF ammoPack, Callback callback)
			{
			}

			public override void SetTriggerPressed(bool pressed)
			{
				if (_E04E == FirearmController._E034.EUtilityType.ExamineWeapon)
				{
					OnUtilityOperationStartEvent();
					OnIdleStartEvent();
					_E002.CurrentOperation.SetTriggerPressed(pressed);
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E04D = opened;
				if (!_E04D)
				{
					_E04C = 0f;
				}
				base.SetInventoryOpened(opened);
			}

			public override void Update(float deltaTime)
			{
				base.Update(deltaTime);
				if (State != EOperationState.Executing || _E04D)
				{
					return;
				}
				if (_E04C > 2.5f)
				{
					if (_E037 != null)
					{
						UnityEngine.Debug.LogError(_ED3E._E000(157311) + _E037.Animator.name);
					}
					else
					{
						UnityEngine.Debug.LogError(_ED3E._E000(157339));
					}
					State = EOperationState.Ready;
					OnIdleStartEvent();
				}
				else
				{
					_E04C += deltaTime;
				}
			}

			protected override void SetSightingRange(float deltaTime)
			{
			}

			public override void SetAiming(bool isAiming)
			{
			}

			public override bool ExamineWeapon()
			{
				return true;
			}

			public override void OnShellEjectEvent()
			{
				_E002.m__E01F.StartSpawnShell(_E002._player.Velocity * 0.33f);
				for (int i = 0; i < _E047.ShellsInChambers.Length; i++)
				{
					_E047.ShellsInChambers[i] = null;
				}
				_E037.SetShellsInWeapon(_E047.ShellsInLauncherCount);
			}
		}

		public class _E018 : _E00E
		{
			private _EA12 _E00C;

			private int _E010 = -1;

			private new Callback _E000;

			public _E018(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(_EA12 ammo, int chamberIndex, Callback callback)
			{
				_E00C = ammo;
				_E010 = chamberIndex;
				_E000 = callback;
				Start();
				_E002.IsAiming = false;
				_E037.SetFire(fire: false);
				_E037.LoadOneTrigger(loadOne: true);
				_E037.SetChamberIndexForLoadUnloadAmmo(chamberIndex);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}

			public override void Reset()
			{
				_E00C = null;
				_E010 = -1;
				_E000 = null;
				base.Reset();
			}

			public override void OnMagAppeared()
			{
				_E002.m__E01F.SetRoundIntoWeapon(_E00C);
			}

			public override void OnRemoveShellEvent()
			{
				_E002.m__E01F.RemoveShellInWeapon();
				_E037.SetShellsInWeapon(0);
			}

			public override void OnAddAmmoInChamber()
			{
				_E037.SetAmmoInChamber(1f);
				_E037.SetShellsInWeapon(0);
				_E037.SetCanReload(canReload: false);
				_E037.LoadOneTrigger(loadOne: false);
				State = EOperationState.Finished;
				_E002.InitiateOperation<_E015>().Start();
				_E000?.Succeed();
			}

			public override void OnOnOffBoltCatchEvent(bool isCaught)
			{
				_E037.SetBoltCatch(isCaught);
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}
		}

		public class _E019 : _E00E
		{
			private _EA12 _E00C;

			private _EB13 _E025;

			private int _E026 = -1;

			private new Callback m__E000;

			public _E019(FirearmController controller)
				: base(controller)
			{
			}

			public virtual void Start(_EA12 ammo, int camoraIndex, Callback callback)
			{
				_E00C = ammo;
				_E025 = _E039.GetCurrentMagazine() as _EB13;
				this.m__E000 = callback;
				_E026 = camoraIndex;
				Start();
				_E002.IsAiming = false;
				_E037.SetFire(fire: false);
				_E037.LoadOneTrigger(loadOne: true);
				if (_E039.ShellsInChambers[_E026] != null)
				{
					_E037.SetCamoraIndexWithShellForRemove(_E026);
				}
				else
				{
					_E037.SetCamoraIndexWithShellForRemove(-1);
				}
				_E037.SetShellsInWeapon(_E039.GetShellsInWeaponCount());
				_E037.SetCamoraIndexForLoadAmmo(_E026);
				_E037.SetAmmoOnMag(_E025.Count - 1);
				_E037.SetCanReload(canReload: true);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}

			public override void Reset()
			{
				_E00C = null;
				_E025 = null;
				this.m__E000 = null;
				_E026 = -1;
				base.Reset();
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					OnRemoveShellEvent();
					OnAddAmmoInChamber();
					AddAmmoToMag();
				}
			}

			public override void OnMagPuttedToRig()
			{
				_E000();
			}

			public override void OnRemoveShellEvent()
			{
				_E039.ShellsInChambers[_E026] = null;
				_E038.RemoveShellInWeapon(_E026);
				_E037.SetShellsInWeapon(_E039.GetShellsInWeaponCount());
			}

			public override void OnAddAmmoInChamber()
			{
				_E037.SetAmmoOnMag(_E025.Count);
				_E037.SetShellsInWeapon(_E039.GetShellsInWeaponCount());
				_E037.SetCanReload(canReload: true);
				_E037.LoadOneTrigger(loadOne: false);
				_E038.SetRoundIntoWeapon(_E00C, _E026);
			}

			public override void AddAmmoToMag()
			{
				_E037.SetCanReload(canReload: false);
			}

			public override void OnOnOffBoltCatchEvent(bool isCaught)
			{
				_E037.SetBoltCatch(isCaught);
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}

			private new void _E000()
			{
				_E025.ResetCamoraIndex();
				_E037.SetCamoraIndex(_E025.CurrentCamoraIndex);
				State = EOperationState.Finished;
				_E002.InitiateOperation<_E011>().Start();
				this.m__E000.Succeed();
			}
		}

		public class _E01A : _E00E
		{
			private _EA12 _E00C;

			private int _E010 = -1;

			private new Callback m__E000;

			public _E01A(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(_EA12 ammo, int chamberIndex, Callback callback)
			{
				_E00C = ammo;
				_E010 = chamberIndex;
				this.m__E000 = callback;
				Start();
				_E002.IsAiming = false;
				_E037.SetFire(fire: false);
				_E037.LoadOneTrigger(loadOne: true);
				_E037.SetChamberIndexForLoadUnloadAmmo(chamberIndex);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				base._E001();
			}

			public override void Reset()
			{
				_E00C = null;
				_E010 = -1;
				this.m__E000 = null;
				base.Reset();
			}

			public override void OnRemoveShellEvent()
			{
				for (int i = 0; i < _E039.ShellsInChambers.Length; i++)
				{
					_E039.ShellsInChambers[i] = null;
				}
				_E038.RemoveAllShells();
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
			}

			public override void OnMagAppeared()
			{
				if (!_E00C.IsUsed && !_E038.HasPatronInWeapon(_E010))
				{
					_E038.SetRoundIntoWeapon(_E00C, _E010);
				}
				if (_E00C.IsUsed && !_E038.HasShellInWeapon(_E010))
				{
					_E038.CreatePatronInShellPort(_E00C);
				}
			}

			public override void OnAddAmmoInChamber()
			{
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
				base._E001.ExecuteSkill((Action)delegate
				{
					base._E001.Skills.RaidLoadedAmmoAction.Complete();
				});
				_E037.SetCanReload(canReload: false);
				_E037.LoadOneTrigger(loadOne: false);
				if (!_E00C.IsUsed && !_E038.HasPatronInWeapon(_E010))
				{
					_E038.SetRoundIntoWeapon(_E00C, _E010);
				}
				if (_E00C.IsUsed && !_E038.HasShellInWeapon(_E010))
				{
					_E038.CreatePatronInShellPort(_E00C, _E010);
				}
				_E000();
			}

			public override void OnOnOffBoltCatchEvent(bool isCaught)
			{
				_E037.SetBoltCatch(isCaught);
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}

			private new void _E000()
			{
				State = EOperationState.Finished;
				_E002.InitiateOperation<_E011>().Start();
				this.m__E000.Succeed();
			}

			[CompilerGenerated]
			private new void _E001()
			{
				base._E001.Skills.RaidLoadedAmmoAction.Complete();
			}
		}

		public abstract class _E01B : _E00E
		{
			[CompilerGenerated]
			private new sealed class _E000
			{
				public _E01B _003C_003E4__this;

				public Action onHidden;

				public bool fastDrop;

				public Item nextControllerItem;

				internal void _E000()
				{
					_003C_003E4__this.State = EOperationState.Finished;
					_003C_003E4__this._E002.InitiateOperation<_E02B>().Start(onHidden, fastDrop, nextControllerItem);
				}
			}

			protected _EA12 _E03A;

			protected _EA12 _E04F;

			private Action _E036;

			protected _E01B(FirearmController controller)
				: base(controller)
			{
			}

			public new virtual void Start()
			{
				base.Start();
				_E03A = _E039.MalfState.AmmoToFire;
				_E04F = _E039.MalfState.AmmoWillBeLoadedToChamber;
			}

			public override void Reset()
			{
				_E03A = null;
				_E04F = null;
				_E036 = null;
				base.Reset();
			}

			public override void RemoveAmmoFromChamber()
			{
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount - ((_E04F != null) ? 1 : 0));
				_E038.SetupPatronInWeaponForJam();
			}

			protected new void _E000()
			{
				_E002.IsTriggerPressed = false;
				_E037.SetFire(_E002.IsTriggerPressed);
				State = EOperationState.Finished;
				if (_E036 != null)
				{
					_E036();
				}
				else
				{
					_E002.InitiateOperation<_E011>().Start();
				}
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				_E002.IsTriggerPressed = false;
				_E002.IsAiming = false;
				_E036 = delegate
				{
					State = EOperationState.Finished;
					_E002.InitiateOperation<_E02B>().Start(onHidden, fastDrop, nextControllerItem);
				};
			}
		}

		public class _E01C : _E00C
		{
			protected _EA12 _E03A;

			public _E01C(FirearmController controller)
				: base(controller)
			{
			}

			public override void Start()
			{
				base.Start();
				_E037.SetFire(((_E00E)this)._E002.IsTriggerPressed);
				((_E00E)this)._E002.SendStartOneShotFire();
				((_E00E)this)._E001._E0DE.RaiseEvent(new _EAF4(_E039, CommandStatus.Begin));
			}

			protected override void StartFireAnimation()
			{
				if (_E039.MalfState.State == Weapon.EMalfunctionState.None)
				{
					if (_E039 is _EAD1 && _E039.CylinderHammerClosed)
					{
						_E037.Animator.Play(_E037.FullDoubleActionFireStateName, 1, 0f);
					}
					else
					{
						_E037.Animator.Play(_E037.FullFireStateName, 1, 0f);
					}
				}
			}

			public override void Reset()
			{
				_E03A = null;
				base.Reset();
			}

			protected override void PrepareShot()
			{
			}

			public override void OnFireEvent()
			{
				_E03A = new _EA12(Guid.NewGuid().ToString(), _E039.Template.DefAmmoTemplate);
				if (_E03A != null)
				{
					_E03A.IsUsed = true;
					((_E00E)this)._E002._E035(_E03A);
					((_E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(_E03A.IsUsed);
					_E03A = null;
					_E039.Repairable.Durability = 0f;
				}
			}

			public override void SetTriggerPressed(bool pressed)
			{
				((_E00E)this)._E002.IsTriggerPressed &= pressed;
			}

			public override void OnFireEndEvent()
			{
				_E030 = true;
				SetTriggerPressed(pressed: false);
				_E037.SetFire(fire: false);
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
				((_E00E)this)._E001._E0DE.RaiseEvent(new _EAF4(_E039, CommandStatus.Succeed));
				SetAiming(isAiming: false);
				SetTriggerPressed(pressed: false);
				State = EOperationState.Finished;
				((_E00E)this)._E002.InitiateOperation<_E011>().Start();
			}

			public override void FastForward()
			{
				if (!_E030)
				{
					OnFireEvent();
				}
				OnFireEndEvent();
			}

			public override bool CanNotBeInterrupted()
			{
				return true;
			}
		}

		public sealed class _E01D : _E00E
		{
			private Slot _E050;

			private new Callback _E000;

			private bool _E051;

			private bool _E052;

			private bool _E053;

			private bool _E054;

			public _E01D(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(_EA6A magazine, Slot from, Callback callback)
			{
				_E050 = from;
				_E000 = callback;
				Start();
				_E037.PullOutMagInInventoryMode();
				_E037.SetCanReload(canReload: false);
				_E037.ResetInsertMagInInventoryMode();
				_E037.SetFire(fire: false);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				if (!_E039.MustBoltBeOpennedForExternalReload)
				{
					_E053 = true;
					_E054 = true;
					if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
					{
						_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 0);
					}
				}
				else if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					_E037.SetAmmoInChamber(1f);
					_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 0);
				}
				_E051 = false;
				_E052 = false;
				_E037.SetIsExternalMag(isExternalMag: true);
				_EA6A currentMagazine = _E039.GetCurrentMagazine();
				_E037.SetMagTypeCurrent(currentMagazine?.magAnimationIndex ?? 0);
				if (_E039.IsBoltCatch && _E039.ChamberAmmoCount == 1 && !_E039.ManualBoltCatch && !_E039.MustBoltBeOpennedForExternalReload && !_E039.MustBoltBeOpennedForInternalReload)
				{
					_E037.SetBoltCatch(active: false);
				}
			}

			public override void Reset()
			{
				_E050 = null;
				_E000 = null;
				base.Reset();
			}

			public override void OnMagPulledOutFromWeapon()
			{
				_E051 = true;
				_E037.SetAmmoOnMag(0);
				_E037.SetMagInWeapon(ok: false);
			}

			public override void OnMagPuttedToRig()
			{
				_E052 = true;
				_E038.RemoveMod(_E050);
				State = EOperationState.Finished;
				_E002.RecalculateErgonomic();
				_E002.InitiateOperation<_E011>().Start();
				_E000.Succeed();
				_E002.WeaponModified();
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}

			public override void OnShellEjectEvent()
			{
				_E053 = true;
				if (_E039.MustBoltBeOpennedForExternalReload && _E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					_E038.CreatePatronInShellPort(_E039.MalfState.MalfunctionedAmmo);
					_E038.StartSpawnMisfiredCartridge(base._E001.Velocity * 0.66f);
					return;
				}
				_EA12 obj = null;
				for (int i = 0; i < _E039.Chambers.Length; i++)
				{
					obj = (_EA12)_E039.Chambers[i].ContainedItem;
					if (obj != null && !obj.IsUsed)
					{
						break;
					}
				}
				_E038.MoveAmmoFromChamberToShellPort(obj.IsUsed);
				_E038.StartSpawnShell(base._E001.Velocity * 0.66f);
			}

			public override void RemoveAmmoFromChamber()
			{
				_E054 = true;
				if (_E039.MustBoltBeOpennedForExternalReload && _E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					base._E000();
					_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
					return;
				}
				bool flag = false;
				Slot[] chambers = _E039.Chambers;
				Slot slot = null;
				_EA12 obj = null;
				for (int i = 0; i < chambers.Length; i++)
				{
					if (flag)
					{
						break;
					}
					slot = chambers[i];
					obj = (_EA12)slot.ContainedItem;
					flag |= !obj.IsUsed && slot.RemoveItem().OrElse(elseValue: false);
				}
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
			}

			public override void OnOnOffBoltCatchEvent(bool isCaught)
			{
				_E037.SetBoltCatch(isCaught);
			}

			public override bool CanChangeLightState(_E6C5[] lightsStates)
			{
				return false;
			}

			public override void FastForward()
			{
				if (!_E054)
				{
					RemoveAmmoFromChamber();
				}
				if (!_E053)
				{
					OnShellEjectEvent();
				}
				if (!_E051)
				{
					OnMagPulledOutFromWeapon();
				}
				if (!_E052)
				{
					OnMagPuttedToRig();
				}
				_E037.Animator.Play(_E037.FullIdleStateName, 1, 0.1f);
			}
		}

		private class _E01E : _E00E
		{
			[CompilerGenerated]
			private new sealed class _E000
			{
				public _E01E _003C_003E4__this;

				public Action onHidden;

				public bool fastDrop;

				public Item nextControllerItem;

				internal void _E000()
				{
					_003C_003E4__this._E002.InitiateOperation<_E02B>().Start(onHidden, fastDrop, nextControllerItem);
				}
			}

			private Item _E055;

			private Item _E056;

			private Action _E036;

			private new Callback m__E000;

			private bool _E057;

			private bool _E058;

			private bool _E059;

			public _E01E(FirearmController controller)
				: base(controller)
			{
			}

			public virtual void Start(_EA12 ammo, Callback callback)
			{
				Start();
				this.m__E000 = callback;
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					base._E000();
				}
				if (_E039.ChamberAmmoCount == 0)
				{
					_E000();
					return;
				}
				if (_E039.HasChambers)
				{
					_ = _E039.Chambers[0];
				}
				_E055 = ammo;
				_EB29.Remove(_E055, base._E001._E0DE, simulate: true);
				_EA6A currentMagazine = _E039.GetCurrentMagazine();
				if (currentMagazine != null && currentMagazine.Count > 0)
				{
					_E056 = currentMagazine.Cartridges.Peek();
					base._E001.ExecuteSkill((Action)delegate
					{
						base._E001.Skills.WeaponChamberAction.Complete(_E039);
					});
				}
				_E002.SetAim(value: false);
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.Rechamber(val: true);
				_E037.SetInventory(open: false);
				_E037.SetFire(fire: false);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}

			public override void Reset()
			{
				_E055 = null;
				_E056 = null;
				_E036 = null;
				_E057 = false;
				_E058 = false;
				_E059 = false;
				base.Reset();
			}

			public override void RemoveAmmoFromChamber()
			{
				if (!_E057)
				{
					_E057 = true;
					_E038.RemovePatronInWeapon();
					_E037.SetAmmoInChamber(0f);
				}
			}

			public override void OnOnOffBoltCatchEvent(bool isCatched)
			{
				_E037.SetBoltCatch(isCatched);
			}

			public override void OnAddAmmoInChamber()
			{
				if (!_E058)
				{
					_E058 = true;
					if (_E056 != null)
					{
						_E038.SetRoundIntoWeapon((_EA12)_E056);
						_E000();
					}
				}
			}

			public override void OnShellEjectEvent()
			{
				if (!_E059)
				{
					_E059 = true;
					this.m__E000?.Succeed();
					_E038.ThrowPatronAsLoot(_E055, base._E001);
					if (_E056 == null)
					{
						_E000();
					}
				}
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				_E036 = delegate
				{
					_E002.InitiateOperation<_E02B>().Start(onHidden, fastDrop, nextControllerItem);
				};
			}

			public override void FastForward()
			{
				if (State == EOperationState.Finished)
				{
					return;
				}
				RemoveAmmoFromChamber();
				_EA6A currentMagazine = _E039.GetCurrentMagazine();
				bool flag = _E039.ReloadMode == Weapon.EReloadMode.ExternalMagazine || _E039.ReloadMode == Weapon.EReloadMode.ExternalMagazineWithInternalReloadSupport;
				bool flag2 = currentMagazine != null && currentMagazine.Count > 0;
				if (_E039.IsBoltCatch && _E056 == null && !flag2 && ((flag && currentMagazine != null) || !flag))
				{
					OnOnOffBoltCatchEvent(isCatched: true);
				}
				OnShellEjectEvent();
				if (State != EOperationState.Finished)
				{
					OnAddAmmoInChamber();
					if (State != EOperationState.Finished)
					{
						_E000();
					}
				}
			}

			private new void _E000()
			{
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.SetAmmoOnMag(_E039.GetCurrentMagazineCount());
				_E037.Rechamber(val: false);
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.SetInventory(_E002.m__E002);
				State = EOperationState.Finished;
				if (_E036 == null)
				{
					_E002.InitiateOperation<_E011>().Start();
				}
				else
				{
					_E036();
				}
			}

			[CompilerGenerated]
			private new void _E001()
			{
				base._E001.Skills.WeaponChamberAction.Complete(_E039);
			}
		}

		public class _E01F : _E022
		{
			protected bool _E05A;

			protected _EB13 _E025;

			protected List<int> _E05B = new List<int>();

			private List<int> _E05C = new List<int>();

			private int _E05D = -1;

			private bool _E05E;

			private int _E05F;

			private bool _E060;

			public _E01F(FirearmController controller)
				: base(controller)
			{
			}

			public virtual void Start(_E9CF ammoPack, Callback callback, bool quickReload = false)
			{
				base.Start(ammoPack, callback);
				_E05E = quickReload;
				Prepare();
				base._E000 = false;
			}

			public override void OnMagPuttedToRig()
			{
				_E002();
				SwitchToIdle();
			}

			public override void Reset()
			{
				base.Reset();
				_E05E = false;
				_E060 = false;
				_E05A = false;
				_E025 = null;
				_E05D = -1;
				_E05F = 0;
				_E05B.Clear();
				_E05C.Clear();
			}

			public void Prepare()
			{
				_E025 = _E039.GetCurrentMagazine() as _EB13;
				_E05F = _E025.Count;
				_E05A = (_E05E || _E025.Count == 0) && ((_E00E)this)._E002.CurrentMasteringLevel > 0;
				_E037.SetWeaponLevel(_E05A ? 1 : 0);
				_E05B = (_E05A ? _E025.GetCamorasIndexesList() : _E025.GetFreeCamorasIndexesFromCurrentActiveIndex(_E05E, !_E039.CylinderHammerClosed));
				_E039.GetShellsIndexes(_E05C);
				_E037.SetShellsInWeapon(_E05C.Count);
				_E037.SetAmmoOnMag(_E025.Count);
				if (_E025.Count != 0)
				{
					_E003();
					_E037.SetCamoraIndexWithShellForRemove(_E05D);
				}
				if (_E05E)
				{
					_E037.ReloadFast(_E05E);
					_E037.SetAmmoCountForRemove(_E05C.Count + _E025.Count);
					_E037.ResetReload();
					_E05F = 0;
				}
				else
				{
					_E037.SetAmmoCountForRemove(_E05C.Count);
				}
				_E001();
			}

			public override void OnOnOffBoltCatchEvent(bool isCatched)
			{
				_E037.SetBoltCatch(isCatched);
			}

			public override void OnMagAppeared()
			{
				_E000();
			}

			private new void _E000()
			{
				if (_E05A)
				{
					_E025.SetCurrentCamoraIndex(_E039.CylinderHammerClosed ? (_E025.MaxCount - 1) : 0);
					for (int i = 0; i < _E025.MaxCount && i < _E04A.AmmoCount; i++)
					{
						_EA12 ammoToReload = _E04A.GetAmmoToReload(i);
						_E038.SetRoundIntoWeapon(ammoToReload, i);
					}
				}
			}

			protected virtual void RemoveExtraPatronsInHandsAfterMasteringReload()
			{
				if (_E05A)
				{
					for (int i = _E06B; i < _E05B.Count && i < _E04A.AmmoCount; i++)
					{
						_E038.DestroyPatronInWeapon(_E05B[i]);
					}
				}
			}

			public override void RemoveAmmoFromChamber()
			{
				RemoveExtraPatronsInHandsAfterMasteringReload();
			}

			public override void OnRemoveShellEvent()
			{
				if (_E025.Count == 0)
				{
					_E004();
				}
				else if (_E05E)
				{
					_E004();
					_E005();
				}
				else
				{
					_E039.ShellsInChambers[_E05D] = null;
					_E038.StartSpawnShell(((_E00E)this)._E001.Velocity * 0.33f, _E05D);
					_E003();
				}
			}

			public override void OnShellEjectEvent()
			{
				_E037.SetCamoraIndexWithShellForRemove(_E05D);
			}

			public override void OnAddAmmoInChamber()
			{
				if (_E05A)
				{
					AddAmmoInHighMasteringReload();
				}
				else
				{
					SetCamoraIndexForLoadAmmo();
				}
				_E037.SetAmmoOnMag(_E025.Count + _E06B);
			}

			protected virtual void AddAmmoInHighMasteringReload()
			{
				if (_E06B < _E05B.Count && _E06B < _E04A.AmmoCount)
				{
					_E06B++;
					_E037.SetAmmoOnMag(_E06B);
					if (_E06B >= _E05B.Count || _E06B >= _E04A.AmmoCount)
					{
						AddAmmoToMag();
					}
				}
			}

			protected new void _E001()
			{
				if (_E06B < _E05B.Count)
				{
					int camoraIndexForLoadAmmo = _E05B[_E06B];
					_E037.SetCamoraIndexForLoadAmmo(camoraIndexForLoadAmmo);
				}
			}

			public override void AddAmmoToMag()
			{
				_E001();
				base._E000 = true;
				_E037.SetAmmoOnMag(_E025.Count + _E06B);
				if (!CanReload() || _E06A)
				{
					_E037.SetMasteringReloadAborted(_E06A);
					_E002();
				}
			}

			public static void CommitReloadWithAmmo(int ammoToLoadIntoMag, _E9CF ammoPack, Player player, _EB13 magazine, Weapon weapon, List<int> camorasIndexesForLoadAmmo)
			{
				for (int i = 0; i < ammoToLoadIntoMag; i++)
				{
					_ECD8<_EB2E> obj = ammoPack.LoadAmmo(player._E0DE, player._E0DE, new _EB20(magazine.Camoras[camorasIndexesForLoadAmmo[i]]));
					if (obj.Error == null)
					{
						obj.Value.RaiseEvents(player._E0DE, CommandStatus.Begin);
						obj.Value.RaiseEvents(player._E0DE, CommandStatus.Succeed);
						continue;
					}
					UnityEngine.Debug.LogError(_ED3E._E000(157353) + ammoPack.AmmoCount + _ED3E._E000(157437) + (ammoToLoadIntoMag - i) + _ED3E._E000(157419) + obj.Error);
				}
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				base.HideWeapon(onHidden, fastDrop, (Item)null);
				_E06A = true;
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished && State != EOperationState.Finished)
				{
					_E002();
					SwitchToIdle();
				}
			}

			protected new void _E002()
			{
				if (_E060)
				{
					return;
				}
				_E060 = true;
				if (_E06B > 0)
				{
					((_E00E)this)._E001.ExecuteSkill((Action)delegate
					{
						((_E00E)this)._E001.Skills.WeaponReloadAction.Complete(_E039);
					});
				}
				_E039.Parent.RaiseRemoveEvent(_E039, CommandStatus.Failed);
				_E037.SetCamoraIndex(_E025.CurrentCamoraIndex);
				_E037.SetCanReload(canReload: false);
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 1);
				}
			}

			protected virtual void SwitchToIdle()
			{
				_E04A.UnlockItems();
				_E006();
				_E037.SetMasteringReloadAborted(value: false);
				_E037.SetInventory(((_E00E)this)._E002.InventoryOpened);
				Action action = _E022;
				bool fastDrop = _E021;
				_E039.RaiseRefreshEvent();
				State = EOperationState.Finished;
				_E011 obj = ((_E00E)this)._E002.InitiateOperation<_E011>();
				obj.Start();
				base._E001();
				if (action != null)
				{
					obj.HideWeapon(action, fastDrop);
				}
			}

			protected virtual void SetCamoraIndexForLoadAmmo()
			{
				if (_E06B < _E05B.Count && _E06B < _E04A.AmmoCount)
				{
					_E037.SetCamoraIndex(_E025.CurrentCamoraIndex);
					_E037.SetCamoraIndexForLoadAmmo(_E05B[_E06B]);
					_EA12 ammoToReload = _E04A.GetAmmoToReload(_E06B);
					_E038.SetRoundIntoWeapon(ammoToReload, _E05B[_E06B]);
					_E06B++;
				}
			}

			protected void _E003()
			{
				_E037.SetShellsInWeapon(_E05C.Count);
				if (!_E05E && _E05C.Count != 0)
				{
					_E05D = _E05C.First();
					_E05C.RemoveAt(0);
				}
			}

			protected override bool CanReload()
			{
				if (_E04A.AmmoCount - _E06B > 0)
				{
					return _E05F + _E06B != _E069.MaxCount;
				}
				return false;
			}

			private void _E004()
			{
				for (int i = 0; i < _E039.ShellsInChambers.Length; i++)
				{
					_E039.ShellsInChambers[i] = null;
				}
				_E038.StartSpawnAllShells(Vector3.down);
				_E037.SetShellsInWeapon(0);
			}

			private void _E005()
			{
				_E038.DestroyAllPatronsInWeapon();
				for (int i = 0; i < _E025.Camoras.Length; i++)
				{
					Item containedItem = _E025.Camoras[i].ContainedItem;
					if (containedItem != null)
					{
						_E025.RemoveAmmoInCamora(containedItem, ((_E00E)this)._E001._E0DE);
						_E038.ThrowPatronAsLoot(containedItem, ((_E00E)this)._E001);
					}
				}
			}

			private void _E006()
			{
				_E9CF ammoPack = _E04A;
				_EB13 obj = _E025;
				Weapon weapon = _E039;
				int ammoToLoadIntoMag = _E06B;
				Player player = ((_E00E)this)._E001;
				List<int> camorasIndexesForLoadAmmo = _E05B;
				CommitReloadWithAmmo(ammoToLoadIntoMag, ammoPack, player, obj, weapon, camorasIndexesForLoadAmmo);
				if (!weapon.CylinderHammerClosed)
				{
					_E025.SetNotEmptyCamoraAsActive();
				}
				_E037.SetAmmoOnMag(obj.Count);
				((_E00E)this)._E001._E0DE.CheckChamber(_E039, status: true);
			}

			[CompilerGenerated]
			private void _E007()
			{
				((_E00E)this)._E001.Skills.WeaponReloadAction.Complete(_E039);
			}
		}

		public class _E020 : _E003
		{
			protected Callback _E061;

			private bool _E062;

			private bool _E063;

			private bool _E00F;

			private bool _E00E;

			private bool _E064;

			private bool _E065;

			private bool _E066;

			private bool _E067;

			protected _E021 _E068;

			public _E020(FirearmController controller)
				: base(controller)
			{
			}

			public override void SetAiming(bool isAiming)
			{
				_E000(isAiming);
			}

			public virtual void Start(_E021 reloadExternalMagResult, [CanBeNull] Callback callback)
			{
				_E061 = callback;
				Start(callback);
				_E068 = reloadExternalMagResult;
				_E037.SetAmmoCompatible(_E068.AmmoCompatible);
				_E037.SetIsExternalMag(isExternalMag: true);
				if (_E068.RemoveFromChamberResult == null)
				{
					_E062 = true;
				}
				if (_E068.PopNewAmmoResult == null)
				{
					_E065 = true;
				}
				_E037.SetCanReload(canReload: true);
				_E037.Reload((_E068.OldMagazine != null) ? _E068.OldMagazine.magAnimationIndex : (-1), _E068.NextMagazine.magAnimationIndex, reloadExternalMagResult.QuickReload);
				_E068.RaiseEvents(((_E00E)this)._E001._E0DE, CommandStatus.Begin);
				if (_E039.IsBoltCatch && _E039.ChamberAmmoCount == 1 && _E068.PopNewAmmoResult == null && !_E039.ManualBoltCatch && !_E039.MustBoltBeOpennedForExternalReload && !_E039.MustBoltBeOpennedForInternalReload)
				{
					_E037.SetBoltCatch(active: false);
				}
				((_E00E)this)._E001.Say(EPhraseTrigger.OnWeaponReload);
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire && _E039.MalfState.IsKnownMalfunction(((_E00E)this)._E001.ProfileId) && _E068.NextMagazine.Count > 0 && _E068.AmmoCompatible)
				{
					_E065 = false;
					_E037.SetAmmoInChamber(0f);
					_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 0);
				}
			}

			public override void Reset()
			{
				_E061 = null;
				_E062 = false;
				_E063 = false;
				_E00F = false;
				_E00E = false;
				_E064 = false;
				_E065 = false;
				_E067 = false;
				_E068 = null;
				base.Reset();
			}

			public override void UseSecondMagForReload()
			{
				_E001();
			}

			public override void OnShellEjectEvent()
			{
				_E038.StartSpawnShell(((_E00E)this)._E001.Velocity * 0.66f);
			}

			public override void RemoveAmmoFromChamber()
			{
				if (!_E062)
				{
					_E062 = true;
					if (_E068.RemoveFromChamberResult != null)
					{
						_E038.RemovePatronInWeapon();
					}
					_E037.SetAmmoInChamber(0f);
					if (_E068.RemoveFromChamberResult != null)
					{
						_E038.ThrowPatronAsLoot(_E068.RemoveFromChamberResult.Item, ((_E00E)this)._E001);
					}
				}
			}

			public override void OnMagPulledOutFromWeapon()
			{
				if (!_E063)
				{
					_E063 = true;
					_E002();
					_E037.SetMagInWeapon(ok: false);
				}
			}

			public override void OnMagPuttedToRig()
			{
				if (_E00F)
				{
					return;
				}
				_E00F = true;
				if (_E068.RemoveOldMagResult != null)
				{
					_EA6A magazineInWeapon = (_EA6A)_E068.RemoveOldMagResult.Item;
					LootItem lootItem = _E038.SpawnDroppedMagAndRemoveExisting(magazineInWeapon, EWeaponModType.mod_magazine);
					if (lootItem != null)
					{
						lootItem.LastOwner = ((_E00E)this)._E001;
					}
				}
				else
				{
					_E038.RemoveMod(_E068.MagazineSlot);
				}
			}

			public override void OnMagAppeared()
			{
				if (!_E00E)
				{
					_E00E = true;
					_E002();
					_E038.SetupMod(_E068.MagazineSlot, Singleton<_E760>.Instance.CreateItem(_E068.NextMagazine, isAnimated: true));
				}
			}

			public override void OnMagInsertedToWeapon()
			{
				if (!_E064)
				{
					_E064 = true;
					_E037.SetMagInWeapon(ok: true);
					_E037.SetAmmoOnMag(_E068.NextMagazine.Count + ((_E068.PopNewAmmoResult != null) ? 1 : 0));
					if (_E039.HasChambers && _E068.PopNewAmmoResult == null && (_E039.MalfState.State != Weapon.EMalfunctionState.Misfire || !_E039.MalfState.IsKnownMalfunction(((_E00E)this)._E001.ProfileId) || !_E068.AmmoCompatible))
					{
						_E000();
					}
					_E037.SetMagTypeCurrent(_E068.NextMagazine.magAnimationIndex);
				}
			}

			public override void OnOnOffBoltCatchEvent(bool isCatched)
			{
				if (isCatched || !_E067)
				{
					if (!isCatched)
					{
						_E067 = true;
					}
					_E037.SetBoltCatch(isCatched);
				}
			}

			public override void OnAddAmmoInChamber()
			{
				if (!_E065)
				{
					_E065 = true;
					if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
					{
						base._E000();
					}
					_E037.SetAmmoOnMag(_E068.NextMagazine.Count);
					if (_E039.HasChambers && _E068.PopNewAmmoResult != null)
					{
						_E038.SetRoundIntoWeapon((_EA12)_E068.PopNewAmmoResult.ResultItem);
					}
					_E037.SetAmmoInChamber(_E068.Weapon.ChamberAmmoCount);
					if (!_E067)
					{
						OnOnOffBoltCatchEvent(isCatched: false);
					}
					_E000();
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				((_E00E)this)._E002.InventoryOpened = opened;
				if (_E063 || _E00E)
				{
					_E037.SetInventory(opened);
				}
			}

			protected new void _E000()
			{
				if (State != EOperationState.Finished)
				{
					State = EOperationState.Finished;
					Action action = _E022;
					bool fastDrop = _E021;
					_E068.RaiseEvents(((_E00E)this)._E001._E0DE, CommandStatus.Succeed);
					((_E00E)this)._E002.RecalculateErgonomic();
					_E011 obj = ((_E00E)this)._E002.InitiateOperation<_E011>();
					obj.Start();
					_E061?.Succeed();
					((_E00E)this)._E002.WeaponModified();
					if (action != null)
					{
						obj.HideWeapon(action, fastDrop);
					}
				}
			}

			private new void _E001()
			{
				UnityEngine.Debug.LogError(_ED3E._E000(157469));
			}

			public override void OnIdleStartEvent()
			{
				if (!_E039.HasChambers && _E064)
				{
					_E000();
				}
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					RemoveAmmoFromChamber();
					OnMagPulledOutFromWeapon();
					OnMagPuttedToRig();
					OnMagAppeared();
					OnMagInsertedToWeapon();
					OnAddAmmoInChamber();
					OnOnOffBoltCatchEvent(isCatched: false);
					if (State != EOperationState.Finished)
					{
						_E000();
					}
					_E037.Animator.Play(_E037.FullIdleStateName, 1, 0.1f);
				}
			}
		}

		public sealed class _E021 : _EB2D, _E6A1
		{
			public readonly _EB1E ItemController;

			public readonly Weapon Weapon;

			public readonly bool AmmoCompatible;

			[CanBeNull]
			public readonly _EA6A OldMagazine;

			public readonly _EA6A NextMagazine;

			public readonly Slot MagazineSlot;

			public readonly bool QuickReload;

			public readonly bool IsKnownMalfunction;

			[CanBeNull]
			public readonly _EB3A RemoveFromChamberResult;

			[CanBeNull]
			public readonly _EB3A RemoveOldMagResult;

			[CanBeNull]
			public readonly _EB3B MoveOldMagResult;

			public readonly _EB3B InsertNextMagResult;

			[CanBeNull]
			public readonly _EB2E PopNewAmmoResult;

			private _E021(_EB1E itemController, [CanBeNull] _EB3A removeFromChamberResult, [CanBeNull] _EB3A removeOldMagResult, [CanBeNull] _EB3B moveOldMagResult, _EB3B insertNextMagResult, [CanBeNull] _EB2E popNewAmmoResult, Weapon weapon, bool ammoCompatible, bool quickReload, bool isKnownMalfunction)
			{
				ItemController = itemController;
				Weapon = weapon;
				AmmoCompatible = ammoCompatible;
				OldMagazine = ((removeOldMagResult != null) ? ((_EA6A)removeOldMagResult.Item) : ((_EA6A)(moveOldMagResult?.Item)));
				NextMagazine = (_EA6A)insertNextMagResult.Item;
				MagazineSlot = ((_EB20)insertNextMagResult.To).Slot;
				QuickReload = quickReload;
				IsKnownMalfunction = isKnownMalfunction;
				RemoveFromChamberResult = removeFromChamberResult;
				RemoveOldMagResult = removeOldMagResult;
				MoveOldMagResult = moveOldMagResult;
				InsertNextMagResult = insertNextMagResult;
				PopNewAmmoResult = popNewAmmoResult;
			}

			public void RollBack()
			{
				PopNewAmmoResult?.RollBack();
				InsertNextMagResult.RollBack();
				RemoveOldMagResult?.RollBack();
				MoveOldMagResult?.RollBack();
				RemoveFromChamberResult?.RollBack();
			}

			public void RaiseEvents(_EB1E controller, CommandStatus status)
			{
				Weapon.Parent.RaiseRemoveEvent(Weapon, (status != 0) ? CommandStatus.Failed : CommandStatus.Begin);
				RemoveFromChamberResult?.RaiseEvents(controller, status);
				RemoveOldMagResult?.RaiseEvents(controller, status);
				MoveOldMagResult?.RaiseEvents(controller, status);
				InsertNextMagResult.RaiseEvents(controller, status);
				PopNewAmmoResult?.RaiseEvents(controller, status);
			}

			public static _ECD8<_E021> Run(_EB1E itemController, Weapon weapon, _EA6A nextMagazine, bool quickReload, bool isKnownMalfunction, [CanBeNull] _EB22 vestTargetAddress)
			{
				_EB20 obj = (weapon.HasChambers ? new _EB20(weapon.Chambers[0]) : null);
				_EA12 obj2 = obj?.Slot.ContainedItem as _EA12;
				_EA6A currentMagazine = weapon.GetCurrentMagazine();
				Slot magazineSlot = weapon.GetMagazineSlot();
				Weapon.EMalfunctionState state = weapon.MalfState.State;
				if (state == Weapon.EMalfunctionState.Misfire)
				{
					weapon.MalfState.ChangeStateSilent(Weapon.EMalfunctionState.None);
				}
				_ECD8<_EB3A> obj3 = ((obj2 != null && weapon.MustBoltBeOpennedForExternalReload) ? _EB29.Remove(obj2, itemController) : default(_ECD8<_EB3A>));
				weapon.MalfState.ChangeStateSilent(state);
				if (obj3.Failed)
				{
					return obj3.Error;
				}
				_ECD8<_EB3A> obj4 = default(_ECD8<_EB3A>);
				_ECD8<_EB3B> obj5 = default(_ECD8<_EB3B>);
				if (currentMagazine != null)
				{
					if (vestTargetAddress != null)
					{
						obj5 = _EB29.Move(currentMagazine, vestTargetAddress, itemController);
						if (obj5.Failed)
						{
							obj3.Value?.RollBack();
							return obj5.Error;
						}
					}
					else
					{
						obj4 = _EB29.Remove(currentMagazine, itemController);
						if (obj4.Failed)
						{
							obj3.Value?.RollBack();
							return obj4.Error;
						}
					}
				}
				_ECD8<_EB3B> obj6 = _EB29.Move(nextMagazine, new _EB20(magazineSlot), itemController);
				if (obj6.Failed)
				{
					obj4.Value?.RollBack();
					obj5.Value?.RollBack();
					obj3.Value?.RollBack();
					return obj6.Error;
				}
				bool flag = nextMagazine.IsAmmoCompatible(weapon.Chambers);
				_ECD8<_EB2E> obj7 = default(_ECD8<_EB2E>);
				if (obj != null && obj.Slot.ContainedItem == null && nextMagazine.Count > 0 && flag)
				{
					bool num = weapon.MalfState.State == Weapon.EMalfunctionState.None || (weapon.MalfState.State == Weapon.EMalfunctionState.Misfire && isKnownMalfunction);
					Weapon.EMalfunctionState state2 = weapon.MalfState.State;
					if (num && weapon.MalfState.State != 0)
					{
						weapon.MalfState.ChangeStateSilent(Weapon.EMalfunctionState.None);
					}
					if (num)
					{
						obj7 = nextMagazine.Cartridges.PopTo(itemController, obj);
						if (state2 != 0)
						{
							weapon.MalfState.ChangeStateSilent(state2);
						}
						if (obj7.Failed)
						{
							obj6.Value.RollBack();
							obj4.Value?.RollBack();
							obj5.Value?.RollBack();
							obj3.Value?.RollBack();
							return obj7.Error;
						}
					}
				}
				return new _E021(itemController, obj3.Value, obj4.Value, obj5.Value, obj6.Value, obj7.Value, weapon, flag, quickReload, isKnownMalfunction);
			}

			public _ECD7 Replay()
			{
				return Run(ItemController, Weapon, NextMagazine, QuickReload, IsKnownMalfunction, (_EB22)(MoveOldMagResult?.To));
			}

			public bool CanExecute(_EB1E itemController)
			{
				if (!Weapon.CheckAction(null))
				{
					return false;
				}
				if (!NextMagazine.CheckAction(null))
				{
					return false;
				}
				if (OldMagazine != null && !OldMagazine.CheckAction(new _EB20(MagazineSlot)))
				{
					return false;
				}
				if (MoveOldMagResult != null && !MoveOldMagResult.Item.CheckAction(MoveOldMagResult.To))
				{
					return false;
				}
				return true;
			}
		}

		public class _E022 : _E003
		{
			protected _E9CF _E04A;

			protected _EA6A _E069;

			protected bool _E06A;

			protected int _E06B;

			private bool _E06C;

			protected new bool _E000
			{
				get
				{
					return _E06C;
				}
				set
				{
					_E06C = value;
					if (!_E06C && value)
					{
						_E002();
					}
				}
			}

			public _E022(FirearmController controller)
				: base(controller)
			{
			}

			public virtual void Start(_E9CF ammoPack, [CanBeNull] Callback callback)
			{
				Start(callback);
				_E04A = ammoPack;
				_E037.SetInventory(open: false);
				_E037.SetCanReload(canReload: true);
				_E037.Reload(b: true);
				_E037.SetIsExternalMag(isExternalMag: false);
				_E04A.LockItems();
				_E039.Parent.RaiseRemoveEvent(_E039, CommandStatus.Begin);
				_E069 = _E039.GetCurrentMagazine();
			}

			public override void Reset()
			{
				_E04A = null;
				_E069 = null;
				_E06B = 0;
				_E06A = false;
				_E000 = false;
				base.Reset();
			}

			protected virtual bool CanReload()
			{
				if (_E04A.AmmoCount - _E06B > 0)
				{
					return _E069.Count + _E06B != _E069.MaxCount;
				}
				return false;
			}

			public override void SetTriggerPressed(bool pressed)
			{
				_E06A |= pressed && _E000;
			}

			public override void SetInventoryOpened(bool opened)
			{
				((_E00E)this)._E002.InventoryOpened = opened;
				_E06A = true;
				if (_E000)
				{
					_E037.SetInventory(opened);
				}
			}
		}

		public class _E023 : _E022
		{
			[CompilerGenerated]
			private new sealed class _E000
			{
				public Player player;

				public Weapon weapon;

				public Action _003C_003E9__0;

				internal void _E000()
				{
					player.Skills.WeaponReloadAction.Complete(weapon);
				}
			}

			private _EA12 _E06D;

			private bool _E058;

			public _E023(FirearmController controller)
				: base(controller)
			{
			}

			public override void Start(_E9CF ammoPack, Callback callback)
			{
				base.Start(ammoPack, callback);
				_E000();
			}

			private new void _E000()
			{
				bool flag = _E039.MalfState.State == Weapon.EMalfunctionState.None || _E039.MalfState.State == Weapon.EMalfunctionState.Misfire;
				if (_E039.ChamberAmmoCount == 0 && flag)
				{
					Weapon.EMalfunctionState state = _E039.MalfState.State;
					if (state == Weapon.EMalfunctionState.Misfire)
					{
						_E039.MalfState.ChangeStateSilent(Weapon.EMalfunctionState.None);
					}
					_ECD8<_EB2E> obj = _E04A.LoadAmmo(((_E00E)this)._E001._E0DE, ((_E00E)this)._E001._E0DE, new _EB20(((_E003)this)._E000));
					_E039.MalfState.ChangeStateSilent(state);
					if (obj.Error != null)
					{
						UnityEngine.Debug.LogError(_ED3E._E000(157214) + obj.Error);
						return;
					}
					_E06D = (_EA12)obj.Value.ResultItem;
					if (obj.Value is _EB3B obj2)
					{
						obj2.From.RaiseRemoveEvent(_E06D, CommandStatus.Succeed);
					}
					if (obj.Value is _EB3D)
					{
						((_EB3D)obj.Value).From.RaiseRemoveEvent(_E06D, CommandStatus.Succeed);
					}
				}
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 0);
				}
			}

			public override void Reset()
			{
				_E06D = null;
				_E058 = false;
				base.Reset();
			}

			public override void OnMagAppeared()
			{
				base._E000 = true;
			}

			public override void OnAddAmmoInChamber()
			{
				if (_E058)
				{
					return;
				}
				_E058 = true;
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire && _E06D == null)
				{
					base._E000 = true;
					if (!CanReload() || _E06A)
					{
						SwitchToIdle();
					}
					return;
				}
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					base._E000();
				}
				base._E000 = true;
				_E038.SetRoundIntoWeapon(_E06D);
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.SetCanReload(CanReload());
				((_E00E)this)._E001.ExecuteSkill((Action)delegate
				{
					((_E00E)this)._E001.Skills.WeaponChamberAction.Complete(_E039);
				});
				if (!CanReload() || _E06A)
				{
					SwitchToIdle();
				}
			}

			public override void OnOnOffBoltCatchEvent(bool isCatched)
			{
				_E037.SetBoltCatch(isCatched);
			}

			public override void AddAmmoToMag()
			{
				base._E000 = true;
				_E06B++;
				_E037.SetAmmoOnMag(_E069.Count + _E06B);
				((_E00E)this)._E001.ExecuteSkill((Action)delegate
				{
					((_E00E)this)._E001.Skills.RaidLoadedAmmoAction.Complete();
				});
				if (!CanReload() || _E06A)
				{
					SwitchToIdle();
				}
			}

			protected virtual void SwitchToIdle()
			{
				_E037.SetCanReload(CanReload() && !_E06A);
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 1);
				}
				_E001();
				Action action = _E022;
				bool fastDrop = _E021;
				_E039.RaiseRefreshEvent();
				State = EOperationState.Finished;
				_E011 obj = ((_E00E)this)._E002.InitiateOperation<_E011>();
				obj.Start();
				base._E001();
				if (action != null)
				{
					obj.HideWeapon(action, fastDrop);
				}
			}

			private new void _E001()
			{
				_E9CF obj = _E04A;
				_EA6A obj2 = _E069;
				Weapon weapon = _E039;
				int ammoToLoadIntoMag = _E06B;
				Player player = ((_E00E)this)._E001;
				obj.UnlockItems();
				CommitReloadWithAmmo(ammoToLoadIntoMag, obj, player, obj2, weapon);
				_E037.SetAmmoOnMag(obj2.Count);
				weapon.Parent.RaiseRemoveEvent(weapon, CommandStatus.Failed);
			}

			public static void CommitReloadWithAmmo(int ammoToLoadIntoMag, _E9CF ammoPack, Player player, _EA6A magazine, Weapon weapon)
			{
				for (int i = 0; i < ammoToLoadIntoMag; i++)
				{
					_ECD8<_EB2E> obj = ammoPack.LoadAmmo(player._E0DE, player._E0DE, new _EB21(magazine.Cartridges));
					if (obj.Error == null)
					{
						obj.Value.RaiseEvents(player._E0DE, CommandStatus.Begin);
						obj.Value.RaiseEvents(player._E0DE, CommandStatus.Succeed);
					}
					else
					{
						UnityEngine.Debug.LogError(_ED3E._E000(157353) + ammoPack.AmmoCount + _ED3E._E000(157437) + (ammoToLoadIntoMag - i) + _ED3E._E000(157419) + obj.Error);
					}
					player.ExecuteSkill((Action)delegate
					{
						player.Skills.WeaponReloadAction.Complete(weapon);
					});
				}
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				base.HideWeapon(onHidden, fastDrop, (Item)null);
				_E06A = true;
			}

			public override void FastForward()
			{
				if (State == EOperationState.Finished)
				{
					return;
				}
				_E06A = true;
				if (_E06D != null)
				{
					OnAddAmmoInChamber();
					if (_E039.IsBoltCatch)
					{
						OnOnOffBoltCatchEvent(isCatched: false);
					}
				}
				if (State != EOperationState.Finished)
				{
					SwitchToIdle();
				}
			}

			[CompilerGenerated]
			private new void _E002()
			{
				((_E00E)this)._E001.Skills.WeaponChamberAction.Complete(_E039);
			}

			[CompilerGenerated]
			private void _E003()
			{
				((_E00E)this)._E001.Skills.RaidLoadedAmmoAction.Complete();
			}
		}

		public class _E024 : _E022
		{
			[CompilerGenerated]
			private new sealed class _E000
			{
				public Player player;

				public Weapon weapon;

				public Action _003C_003E9__0;

				internal void _E000()
				{
					player.Skills.WeaponReloadAction.Complete(weapon);
				}

				internal void _E001()
				{
					player.Skills.WeaponChamberAction.Complete(weapon);
				}
			}

			private Item _E055;

			private bool _E058;

			private bool _E057;

			public _E024(FirearmController controller)
				: base(controller)
			{
			}

			public override void Start(_E9CF ammoPack, Callback callback)
			{
				base.Start(ammoPack, callback);
				_E000();
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 0);
				}
			}

			public override void Reset()
			{
				_E055 = null;
				_E058 = false;
				_E057 = false;
				base.Reset();
			}

			private new void _E000()
			{
				if (_E039.ChamberAmmoCount == 0)
				{
					return;
				}
				_E055 = ((_E003)this)._E000.ContainedItem;
				if (_E055 == null)
				{
					UnityEngine.Debug.LogError(_ED3E._E000(157453));
					return;
				}
				Weapon.EMalfunctionState state = _E039.MalfState.State;
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					_E039.MalfState.ChangeStateSilent(Weapon.EMalfunctionState.None);
				}
				_ = _EB29.Remove(_E055, ((_E00E)this)._E001._E0DE).Succeeded;
				if (state == Weapon.EMalfunctionState.Misfire)
				{
					_E039.MalfState.ChangeStateSilent(state);
				}
			}

			public override void RemoveAmmoFromChamber()
			{
				if (!_E057)
				{
					_E057 = true;
					_E038.RemovePatronInWeapon();
					_E037.SetAmmoInChamber(0f);
					_E038.ThrowPatronAsLoot(_E055, ((_E00E)this)._E001);
				}
			}

			public override void OnOnOffBoltCatchEvent(bool isCatched)
			{
				_E037.SetBoltCatch(isCatched);
			}

			public override void AddAmmoToMag()
			{
				_E038.DestroyPatronInWeapon(0);
				base._E000 = true;
				_E06B++;
				_E037.SetAmmoOnMag(_E069.Count + _E06B);
				((_E00E)this)._E001.ExecuteSkill((Action)delegate
				{
					((_E00E)this)._E001.Skills.RaidLoadedAmmoAction.Complete();
				});
				_E037.SetCanReload(CanReload() && !_E06A);
			}

			public override void OnAddAmmoInChamber()
			{
				if (!_E058)
				{
					_E058 = true;
					base._E000 = true;
					if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
					{
						base._E000();
						_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
					}
					SwitchToIdle();
				}
			}

			protected virtual void SwitchToIdle()
			{
				_E04A.UnlockItems();
				CommitReloadWithAmmo(_E06B, _E04A, ((_E00E)this)._E001, _E069, _E039);
				((_E00E)this)._E001._E0DE.RaiseRemoveEvent(new _EAF3(_E039, _E039.Parent, CommandStatus.Failed));
				_E038.DestroyPatronInWeapon(0);
				if (((_E003)this)._E000.ContainedItem != null)
				{
					_E038.SetRoundIntoWeapon((_EA12)((_E003)this)._E000.ContainedItem);
				}
				_E037.SetAmmoOnMag(_E069.Count);
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.SetCanReload(canReload: false);
				Action action = _E022;
				bool fastDrop = _E021;
				_E039.RaiseRefreshEvent();
				State = EOperationState.Finished;
				_E011 obj = ((_E00E)this)._E002.InitiateOperation<_E011>();
				obj.Start();
				base._E001();
				if (action != null)
				{
					obj.HideWeapon(action, fastDrop);
				}
			}

			public static void CommitReloadWithAmmo(int ammoToLoadIntoMag, _E9CF ammoPack, Player player, _EA6A magazine, Weapon weapon)
			{
				for (int i = 0; i < ammoToLoadIntoMag; i++)
				{
					_ECD8<_EB2E> obj = ammoPack.LoadAmmo(player._E0DE, player._E0DE, new _EB21(magazine.Cartridges));
					if (obj.Succeeded)
					{
						obj.Value.RaiseEvents(player._E0DE, CommandStatus.Begin);
						obj.Value.RaiseEvents(player._E0DE, CommandStatus.Succeed);
						player.ExecuteSkill((Action)delegate
						{
							player.Skills.WeaponReloadAction.Complete(weapon);
						});
					}
					else
					{
						UnityEngine.Debug.LogError(_ED3E._E000(157353) + ammoPack.AmmoCount + _ED3E._E000(157437) + (ammoToLoadIntoMag - i) + _ED3E._E000(157419) + obj.Error);
					}
				}
				if (ammoToLoadIntoMag <= 0 || weapon.ChamberAmmoCount != 0)
				{
					return;
				}
				if (magazine.IsAmmoCompatible(weapon.Chambers))
				{
					_ECD8<_EB2E> obj2 = magazine.Cartridges.PopTo(player._E0DE, new _EB20(weapon.Chambers[0]));
					if (obj2.Failed)
					{
						UnityEngine.Debug.LogError(_ED3E._E000(157564) + obj2.Error);
					}
				}
				player.ExecuteSkill((Action)delegate
				{
					player.Skills.WeaponChamberAction.Complete(weapon);
				});
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				base.HideWeapon(onHidden, fastDrop, (Item)null);
				_E06A = true;
			}

			public override void OnShowAmmo(bool value)
			{
				if (value)
				{
					_EA12 ammoToReload = _E04A.GetAmmoToReload(_E06B);
					if (ammoToReload != null)
					{
						_E038.SetRoundIntoWeapon(ammoToReload);
					}
				}
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					if (_E055 != null)
					{
						RemoveAmmoFromChamber();
					}
					OnAddAmmoInChamber();
					if (_E039.IsBoltCatch)
					{
						OnOnOffBoltCatchEvent(isCatched: false);
					}
					if (State != EOperationState.Finished)
					{
						SwitchToIdle();
					}
				}
			}

			[CompilerGenerated]
			private new void _E001()
			{
				((_E00E)this)._E001.Skills.RaidLoadedAmmoAction.Complete();
			}
		}

		public class _E025 : _E003
		{
			private _E027 _E06E;

			private bool _E065;

			private bool _E06F;

			private int _E070;

			public _E025(FirearmController controller)
				: base(controller)
			{
			}

			public virtual void Start(_E027 reloadMultiBarrelResult, Callback callback)
			{
				Start(callback);
				_E06E = reloadMultiBarrelResult;
				_E06E.RaiseEvents(((_E00E)this)._E001._E0DE, CommandStatus.Begin);
				_E070 = _E039.ShellsInWeaponCount;
				_E000();
				_E001();
				_E037.SetCanReload(canReload: true);
				_E037.Reload(b: true);
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				((_E00E)this)._E001.Say(EPhraseTrigger.OnWeaponReload);
			}

			private new void _E000()
			{
				int num = 0;
				foreach (_E026 item in _E06E.ChambersForReloading)
				{
					num++;
					_E037.SetChamberIndexForLoadUnloadAmmo(item.ChamberIndex);
				}
				if (num == _E06E.ChambersInWeaponTotal)
				{
					_E037.SetChamberIndexForLoadUnloadAmmo(num);
				}
			}

			public override void Reset()
			{
				_E070 = 0;
				_E06F = false;
				_E065 = false;
				_E06E = null;
				base.Reset();
			}

			public override void FastForward()
			{
				if (!_E06F)
				{
					OnRemoveShellEvent();
					OnShellEjectEvent();
					OnAddAmmoInChamber();
				}
			}

			public override void RemoveAmmoFromChamber()
			{
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
			}

			public override void OnRemoveShellEvent()
			{
				for (int i = 0; i < _E039.ShellsInChambers.Length; i++)
				{
					_E039.ShellsInChambers[i] = null;
				}
				_E038.RemoveAllShells();
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
			}

			public override void OnShellEjectEvent()
			{
				_E037.SetCanReload(canReload: true);
				IReadOnlyCollection<_E026> chambersForReloading = _E06E.ChambersForReloading;
				bool discardOldAmmo = _E06E.DiscardOldAmmo;
				foreach (_E026 item in chambersForReloading)
				{
					if (item.OldAmmoResult == null)
					{
						continue;
					}
					if (discardOldAmmo)
					{
						_EA12 obj = (_EA12)item.OldAmmoResult.Item;
						if (!obj.IsUsed)
						{
							_E038.RemovePatronInWeapon(item.ChamberIndex);
							_E038.ThrowPatronAsLoot(obj, ((_E00E)this)._E001);
						}
					}
					else
					{
						_E038.DestroyPatronInWeapon(item.ChamberIndex);
					}
				}
				if (_E070 > 0)
				{
					_E038.StartSpawnAllShells(((_E00E)this)._E001.Velocity * 0.33f);
				}
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
			}

			public override void OnAddAmmoInChamber()
			{
				if (!_E065)
				{
					_E06F = true;
					_E065 = true;
					_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				}
			}

			public override void AddAmmoToMag()
			{
				_E06E.RaiseEvents(((_E00E)this)._E001._E0DE, CommandStatus.Succeed);
				State = EOperationState.Finished;
				_E037.SetCanReload(canReload: false);
				Action action = _E022;
				bool fastDrop = _E021;
				_E039.RaiseRefreshEvent();
				_E011 obj = ((_E00E)this)._E002.InitiateOperation<_E011>();
				obj.Start();
				_E001();
				if (action != null)
				{
					obj.HideWeapon(action, fastDrop);
				}
			}

			public override void OnMagAppeared()
			{
				foreach (_E026 item in _E06E.ChambersForReloading)
				{
					_E038.SetRoundIntoWeapon((_EA12)item.InsertResult.ResultItem, item.ChamberIndex);
				}
			}
		}

		public sealed class _E026
		{
			public readonly int ChamberIndex;

			public readonly _EB2E OldAmmoResult;

			public readonly _EB2E InsertResult;

			public _E026(int chamberIndex, _EB2E oldAmmoResult, _EB2E insertResult)
			{
				ChamberIndex = chamberIndex;
				OldAmmoResult = oldAmmoResult;
				InsertResult = insertResult;
			}

			public void RaiseEvents(_EB1E controller, CommandStatus status)
			{
				OldAmmoResult?.RaiseEvents(controller, status);
				InsertResult.RaiseEvents(controller, status);
			}

			public bool CheckAction()
			{
				_EB2E oldAmmoResult = OldAmmoResult;
				if (oldAmmoResult == null || oldAmmoResult.Item.CheckAction(null))
				{
					return InsertResult.ResultItem.CheckAction(null);
				}
				return false;
			}

			public void RollBack()
			{
				OldAmmoResult?.RollBack();
				InsertResult.RollBack();
			}
		}

		public sealed class _E027 : _EB2D, _E6A1
		{
			public readonly _EB1E ItemController;

			private readonly IDatabaseIdGenerator _E000;

			public readonly _E9CF AmmoPackToLoad;

			public readonly Weapon Weapon;

			public readonly _EB22 PlaceToPutContainedAmmoMagazine;

			public readonly IReadOnlyCollection<_E026> ChambersForReloading;

			public readonly int ChambersInWeaponTotal;

			public readonly bool DiscardOldAmmo;

			private _E027(_EB1E itemController, IDatabaseIdGenerator idGenerator, _E9CF ammoPackToLoad, Weapon weapon, int chambersInWeaponTotal, _EB22 placeToPutContainedAmmoMagazine, IReadOnlyCollection<_E026> chambersForReloading, bool discardOldAmmo)
			{
				ItemController = itemController;
				_E000 = idGenerator;
				AmmoPackToLoad = ammoPackToLoad;
				Weapon = weapon;
				ChambersInWeaponTotal = chambersInWeaponTotal;
				PlaceToPutContainedAmmoMagazine = placeToPutContainedAmmoMagazine;
				ChambersForReloading = chambersForReloading;
				DiscardOldAmmo = discardOldAmmo;
			}

			public static _ECD8<_E027> Run(IDatabaseIdGenerator idGenerator, _EB1E itemController, Weapon weapon, _E9CF ammoPack, _EB22 placeToPutContainedAmmoMagazine)
			{
				int num = weapon.Chambers.Length;
				if (num == 0)
				{
					return default(_ECD9<_E027>).Error;
				}
				bool flag = placeToPutContainedAmmoMagazine == null;
				List<_E026> list = new List<_E026>();
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					Slot slot = weapon.Chambers[i];
					_EA12 obj = slot.ContainedItem as _EA12;
					if (obj != null && !obj.IsUsed)
					{
						continue;
					}
					_EA12 ammoToReload = ammoPack.GetAmmoToReload(num2);
					if (ammoToReload == null)
					{
						break;
					}
					int stackObjectsCount = ammoToReload.StackObjectsCount;
					_EB20 to = new _EB20(slot);
					_ECD8<_EB2E> obj2 = default(_ECD8<_EB2E>);
					if (obj != null)
					{
						obj2 = (flag ? _EB29.Remove(obj, itemController).Cast<_EB3A, _EB2E>() : _EB29.Move(obj, placeToPutContainedAmmoMagazine, itemController).Cast<_EB3B, _EB2E>());
						if (obj2.Failed)
						{
							continue;
						}
					}
					_ECD8<_EB2E> obj3 = _EB29.ApplySingleItemToAddress(ammoToReload, idGenerator, itemController, to);
					if (obj3.Failed)
					{
						obj2.Value?.RollBack();
						continue;
					}
					_E026 item = new _E026(i, obj2.Value, obj3.Value);
					list.Add(item);
					if (stackObjectsCount <= 1)
					{
						num2++;
					}
				}
				return new _E027(itemController, idGenerator, ammoPack, weapon, num, placeToPutContainedAmmoMagazine, list, flag);
			}

			public void RollBack()
			{
				foreach (_E026 item in ChambersForReloading)
				{
					item.RollBack();
				}
			}

			public void RaiseEvents(_EB1E controller, CommandStatus status)
			{
				if (ChambersForReloading.Count == 0)
				{
					return;
				}
				Weapon.Parent.RaiseRemoveEvent(Weapon, (status != 0) ? CommandStatus.Failed : CommandStatus.Begin);
				foreach (_E026 item in ChambersForReloading)
				{
					item.RaiseEvents(controller, status);
				}
			}

			public _ECD7 Replay()
			{
				return Run(_E000, ItemController, Weapon, AmmoPackToLoad, PlaceToPutContainedAmmoMagazine);
			}

			public bool CanExecute(_EB1E itemController)
			{
				if (!Weapon.CheckAction(null))
				{
					return false;
				}
				foreach (_E026 item in ChambersForReloading)
				{
					if (item.CheckAction())
					{
						return true;
					}
				}
				return false;
			}
		}

		public class _E028 : _E003
		{
			private _E029 _E071;

			public _E028(FirearmController controller)
				: base(controller)
			{
			}

			public virtual void Start(_E029 reloadSingleBarrelResult, Callback callback)
			{
				Start(callback);
				_E071 = reloadSingleBarrelResult;
				_E071.RaiseEvents(((_E00E)this)._E001._E0DE, CommandStatus.Begin);
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
				_E037.SetAmmoInChamber(reloadSingleBarrelResult.HasOldAmmoInChamber ? _E039.ChamberAmmoCount : 0);
				_E037.SetCanReload(canReload: true);
				_E037.Reload(b: true);
				if (reloadSingleBarrelResult.HasOldAmmoInChamber)
				{
					for (int i = 0; i < _E039.ShellsInChambers.Length; i++)
					{
						_E039.ShellsInChambers[i] = null;
					}
				}
				((_E00E)this)._E001.Say(EPhraseTrigger.OnWeaponReload);
			}

			public override void Reset()
			{
				_E071 = null;
				base.Reset();
			}

			public override void RemoveAmmoFromChamber()
			{
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
			}

			public override void OnRemoveShellEvent()
			{
				for (int i = 0; i < _E039.ShellsInChambers.Length; i++)
				{
					_E039.ShellsInChambers[i] = null;
				}
				_E038.RemoveAllShells();
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
			}

			public override void OnShellEjectEvent()
			{
				_E037.SetCanReload(canReload: true);
				if (_E071.HasOldAmmoInChamber)
				{
					if (_E071.DiscardOldAmmoToInventory)
					{
						_E038.DestroyPatronInWeapon(0);
					}
					else
					{
						_EA12 obj = (_EA12)_E071.OldAmmoResult.Item;
						if (!obj.IsUsed)
						{
							_E038.RemovePatronInWeapon();
							_E038.ThrowPatronAsLoot(obj, ((_E00E)this)._E001);
						}
					}
				}
				else if (_E039.HasShellsInChamberBarrelOnlyWeapon)
				{
					_E038.StartSpawnShell(((_E00E)this)._E001.Velocity);
				}
				_E037.SetAmmoInChamber(0f);
			}

			public override void OnAddAmmoInChamber()
			{
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E071.RaiseEvents(((_E00E)this)._E001._E0DE, CommandStatus.Succeed);
				State = EOperationState.Finished;
				_E037.SetCanReload(canReload: false);
				Action action = _E022;
				bool fastDrop = _E021;
				_E039.RaiseRefreshEvent();
				_E037.SetInventory(((_E00E)this)._E002.InventoryOpened);
				_E011 obj = ((_E00E)this)._E002.InitiateOperation<_E011>();
				obj.Start();
				_E001();
				if (action != null)
				{
					obj.HideWeapon(action, fastDrop);
				}
			}

			public override void OnMagAppeared()
			{
				_E038.SetRoundIntoWeapon((_EA12)_E071.InsertNewAmmoResult.ResultItem);
			}
		}

		public sealed class _E029 : _EB2D, _E6A1
		{
			[CanBeNull]
			public readonly _EB2E OldAmmoResult;

			[NotNull]
			public readonly _EB2E InsertNewAmmoResult;

			public readonly _EB22 PlaceToPutContainedAmmoMagazine;

			private readonly _EB1E _E000;

			private readonly IDatabaseIdGenerator _E001;

			private readonly Weapon _E002;

			private readonly _EA12 _E003;

			public bool DiscardOldAmmoToInventory => PlaceToPutContainedAmmoMagazine != null;

			public bool HasOldAmmoInChamber => OldAmmoResult != null;

			private _E029(_EB1E itemController, IDatabaseIdGenerator idGenerator, _EA12 ammoToLoad, [CanBeNull] _EB2E oldAmmoResult, [NotNull] _EB2E insertNewAmmoResult, Weapon weapon, _EB22 placeToPutContainedAmmoMagazine)
			{
				_E000 = itemController;
				_E001 = idGenerator;
				_E003 = ammoToLoad;
				_E002 = weapon;
				PlaceToPutContainedAmmoMagazine = placeToPutContainedAmmoMagazine;
				OldAmmoResult = oldAmmoResult;
				InsertNewAmmoResult = insertNewAmmoResult;
			}

			public static _ECD8<_E029> Run(IDatabaseIdGenerator idGenerator, _EB1E itemController, Weapon weapon, _EA12 ammo, _EB22 placeToPutContainedAmmoMagazine)
			{
				Slot obj = weapon.Chambers[0];
				_EA12 obj2 = obj.ContainedItem as _EA12;
				_EB20 to = new _EB20(obj);
				_ECD8<_EB2E> obj3 = default(_ECD8<_EB2E>);
				if (obj2 != null)
				{
					obj3 = ((placeToPutContainedAmmoMagazine == null) ? _EB29.Remove(obj2, itemController).Cast<_EB3A, _EB2E>() : _EB29.Move(obj2, placeToPutContainedAmmoMagazine, itemController).Cast<_EB3B, _EB2E>());
					if (obj3.Failed)
					{
						return obj3.Error;
					}
				}
				_ECD8<_EB2E> obj4 = _EB29.ApplySingleItemToAddress(ammo, idGenerator, itemController, to);
				if (obj4.Failed)
				{
					obj3.Value?.RollBack();
					return obj4.Error;
				}
				return new _E029(itemController, idGenerator, ammo, obj3.Value, obj4.Value, weapon, placeToPutContainedAmmoMagazine);
			}

			public void RollBack()
			{
				OldAmmoResult?.RollBack();
				InsertNewAmmoResult.RollBack();
			}

			public void RaiseEvents(_EB1E controller, CommandStatus status)
			{
				_E002.Parent.RaiseRemoveEvent(_E002, (status != 0) ? CommandStatus.Failed : CommandStatus.Begin);
				OldAmmoResult?.RaiseEvents(controller, status);
				InsertNewAmmoResult.RaiseEvents(controller, status);
			}

			public _ECD7 Replay()
			{
				return Run(_E001, _E000, _E002, _E003, PlaceToPutContainedAmmoMagazine);
			}

			public bool CanExecute(_EB1E itemController)
			{
				if (!_E002.CheckAction(null))
				{
					return false;
				}
				if (OldAmmoResult != null && !OldAmmoResult.Item.CheckAction(null))
				{
					return false;
				}
				return _E003.CheckAction(null);
			}
		}

		public sealed class _E02A : _E00E
		{
			private Slot _E012;

			private new Callback m__E000;

			private bool _E013;

			public _E02A(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(Item item, Slot slot, Callback callback)
			{
				_E012 = slot;
				this.m__E000 = callback;
				Start();
				_E037.SetupMod(modSet: true);
				_E002.SetAim(value: false);
				_E037.SetFire(fire: false);
			}

			public override void Reset()
			{
				_E012 = null;
				this.m__E000 = null;
				_E013 = false;
				base.Reset();
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					OnModChanged();
				}
			}

			public override void SetAiming(bool isAiming)
			{
				if (!isAiming)
				{
					_E002.IsAiming = false;
				}
			}

			public override void OnModChanged()
			{
				if (!_E013)
				{
					_E013 = true;
					_E038.RemoveMod(_E012);
					_E037.SetupMod(modSet: false);
					State = EOperationState.Finished;
					_E002.InitiateOperation<_E011>().Start();
					_E000();
					this.m__E000.Succeed();
					_E038.ModFinallyRemoved(_E012);
					base._E001.BodyAnimatorCommon.SetFloat(_E712.WEAPON_SIZE_MODIFIER_PARAM_HASH, _E039.CalculateCellSize().X);
					base._E001.UpdateFirstPersonGrip(GripPose.EGripType.Common, _E002.HandsHierarchy);
					_E002.WeaponModified();
				}
			}

			private new void _E000()
			{
				if (_E012.ContainedItem is _EA62)
				{
					_E002._E008();
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}

			public override bool CanChangeLightState(_E6C5[] lightsStates)
			{
				return false;
			}
		}

		public sealed class _E02B : _E00E
		{
			private Action _E005;

			public _E02B(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(Action onHidden, bool fastDrop = false, Item nextControllerItem = null)
			{
				_E005 = onHidden;
				_E039.IsUnderBarrelDeviceActive = false;
				if (_E039.MalfState.IsKnownMalfunction(base._E001.ProfileId) && nextControllerItem != null && nextControllerItem is Weapon weapon && weapon.WeapClass.Equals(_ED3E._E000(32735)) && !weapon.MalfState.IsKnownMalfunction(base._E001.ProfileId) && nextControllerItem.Parent.Container == base._E001.Equipment.GetSlot(EquipmentSlot.Holster))
				{
					float pastTime = _E62A.PastTime;
					_E5CB instance = Singleton<_E5CB>.Instance;
					if (_E039.CanQuickdrawPistolAfterMalf(pastTime, instance.Malfunction))
					{
						float idleToOutSpeedMultOnMalf = instance.Malfunction.IdleToOutSpeedMultOnMalf;
						_E037.SetSpeedParameters(1f, idleToOutSpeedMultOnMalf);
						base._E001.MovementContext.PlayerAnimator._E000(1f, idleToOutSpeedMultOnMalf);
						base._E001.QuickdrawWeaponFast = true;
						base._E001.QuickdrawTime = pastTime;
						base._E001._E0B2?.Invoke();
					}
				}
				Start();
				_E037.SetActiveParam(active: false);
				_E037.SetFastHide(fastDrop);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}

			public override void Reset()
			{
				_E005 = null;
				base.Reset();
			}

			public override void HideWeaponComplete()
			{
				State = EOperationState.Finished;
				_E005?.Invoke();
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				_E005 = (Action)Delegate.Combine(_E005, onHidden);
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					HideWeaponComplete();
				}
			}

			public override bool CanChangeLightState(_E6C5[] lightsStates)
			{
				return false;
			}

			public override void BlindFire(int b)
			{
				base.BlindFire(0);
			}
		}

		public sealed class _E02C : _EB2D, _E6A1
		{
			public readonly _EB1E ItemController;

			public readonly Weapon Weapon;

			public readonly bool AmmoCompatible;

			public readonly _EB2E PopNewAmmoResult;

			private _E02C(_EB1E itemController, _EB2E popNewAmmoResult, Weapon weapon, bool ammoCompatible)
			{
				AmmoCompatible = ammoCompatible;
				ItemController = itemController;
				Weapon = weapon;
				PopNewAmmoResult = popNewAmmoResult;
			}

			public void RollBack()
			{
				PopNewAmmoResult?.RollBack();
			}

			public void RaiseEvents(_EB1E controller, CommandStatus status)
			{
				Weapon.Parent.RaiseRemoveEvent(Weapon, (status != 0) ? CommandStatus.Failed : CommandStatus.Begin);
				PopNewAmmoResult?.RaiseEvents(controller, status);
			}

			public static _ECD8<_E02C> Run(_EB1E itemController, Weapon weapon)
			{
				_EB20 obj = (weapon.HasChambers ? new _EB20(weapon.Chambers[0]) : null);
				_EA6A currentMagazine = weapon.GetCurrentMagazine();
				bool flag = currentMagazine.IsAmmoCompatible(weapon.Chambers);
				if (obj == null || obj.Slot.ContainedItem != null || !flag)
				{
					return default(_ECD9<_EB3B>).Error;
				}
				_ECD8<_EB2E> obj2 = currentMagazine.Cartridges.PopTo(itemController, obj);
				if (obj2.Failed)
				{
					return obj2.Error;
				}
				return new _E02C(itemController, obj2.Value, weapon, flag);
			}

			public _ECD7 Replay()
			{
				return Run(ItemController, Weapon);
			}

			public bool CanExecute(_EB1E itemController)
			{
				if (!Weapon.CheckAction(null))
				{
					return false;
				}
				return true;
			}
		}

		public sealed class _E02D : _E01B
		{
			private bool _E072;

			private bool _E059;

			private Weapon.EMalfunctionState _E073;

			public _E02D(FirearmController controller)
				: base(controller)
			{
			}

			public new void Start()
			{
				base.Start();
				_E002.IsAiming = false;
				((_E00E)this)._E001.StopBlindFire();
				_E037.MalfunctionRepair(val: true);
				_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 0);
				_E037.Malfunction((int)_E039.MalfState.State);
				((_E00E)this)._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				float num = 1f;
				float fixSpeed = _E002.m__E013.FixSpeed;
				bool flag = _E002._player.MovementContext.PhysicalConditionIs(EPhysicalCondition.LeftArmDamaged);
				bool flag2 = _E002._player.MovementContext.PhysicalConditionIs(EPhysicalCondition.RightArmDamaged);
				_E5CB._E02C malfunction = Singleton<_E5CB>.Instance.Malfunction;
				float num2 = ((_E039.MalfState.State == Weapon.EMalfunctionState.HardSlide) ? malfunction.MalfRepairHardSlideMult : 1f);
				if (flag && flag2)
				{
					num = malfunction.MalfRepairTwoHandsBrokenMult;
				}
				else if (flag || flag2)
				{
					num = malfunction.MalfRepairOneHandBrokenMult;
				}
				_E037.SetMalfRepairSpeed(fixSpeed * num * num2);
				((_E00E)this)._E001.MovementContext.PlayerAnimator._E001(fixSpeed * num * num2);
				((_E00E)this)._E001.ExecuteSkill((Action)delegate
				{
					((_E00E)this)._E001.Skills.WeaponFixAction.Complete();
				});
				((_E00E)this)._E001._E0DE.RaiseEvent(new _EAF5(_E039, CommandStatus.Begin));
				_E073 = _E039.MalfState.State;
				_E039.MalfState.Repair();
				bool flag3 = _E039.GetCurrentMagazine() != null;
				bool flag4 = flag3 && _E039.GetCurrentMagazine().Count > 0;
				if (flag3 && _E039.GetCurrentMagazine().IsAmmoCompatible(_E039.Chambers) && flag3 && flag4 && _E039.HasChambers && _E039.Chambers[0].ContainedItem == null)
				{
					_ECD8<_E02C> obj = FirearmController._E02C.Run(_E002._player._E0DE, _E002.Item);
					if (obj.Error != null)
					{
						UnityEngine.Debug.LogError(_ED3E._E000(157597) + obj.Error);
					}
					if (obj.Value.PopNewAmmoResult == null)
					{
						_E072 = true;
					}
					else
					{
						_E04F = obj.Value.PopNewAmmoResult.ResultItem as _EA12;
					}
				}
			}

			public override void FastForward()
			{
				OnAddAmmoInChamber();
				OnShellEjectEvent();
				OnMalfunctionOffEvent();
			}

			public override void SetTriggerPressed(bool pressed)
			{
				_E037.SetFire(pressed);
			}

			public override void Reset()
			{
				base.Reset();
				_E072 = false;
				_E059 = false;
			}

			public override void OnOnOffBoltCatchEvent(bool isCatched)
			{
				if (!isCatched)
				{
					UnityEngine.Debug.LogError(string.Format(_ED3E._E000(157571), isCatched));
				}
				if (!isCatched || _E039.ChamberAmmoCount != 1)
				{
					_E037.SetBoltCatch(isCatched);
				}
			}

			public override void RemoveAmmoFromChamber()
			{
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				if (_E073 == Weapon.EMalfunctionState.Jam)
				{
					_E038.SetupPatronInWeaponForJam();
					return;
				}
				if (_E073 == Weapon.EMalfunctionState.Feed)
				{
					_E038.RemoveShellInWeapon();
				}
				_E038.CreatePatronInShellPort(_E03A);
			}

			public override void OnAddAmmoInChamber()
			{
				if (!_E072)
				{
					_E072 = true;
					_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
					_E037.SetAmmoOnMag(_E039.GetCurrentMagazineCount());
					if (_E073 == Weapon.EMalfunctionState.Feed)
					{
						_E038.RemoveShellInWeapon();
					}
					if (_E04F != null)
					{
						_E038.SetRoundIntoWeapon(_E04F);
					}
					_E037.SetFire(_E002.IsTriggerPressed);
					_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				}
			}

			public override void OnMalfunctionOffEvent()
			{
				_E037.MalfunctionRepair(val: false);
				_E037.Malfunction((int)_E039.MalfState.State);
				_E037.MisfireSlideUnknown(val: false);
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.SetAmmoOnMag(_E039.GetCurrentMagazineCount());
				_E039.MalfState.AmmoToFire = null;
				_E039.MalfState.AmmoWillBeLoadedToChamber = null;
				_E039.MalfState.MalfunctionedAmmo = null;
				((_E00E)this)._E001._E0DE.CallMalfunctionRepaired(_E039);
				((_E00E)this)._E001._E0DE.RaiseEvent(new _EAF5(_E039, CommandStatus.Succeed));
				if (_E039.HasChambers && _E039.ChamberAmmoCount == 0)
				{
					_E002._E00C.DestroyPatronInWeapon(0);
				}
				base._E000();
			}

			public override void OnShellEjectEvent()
			{
				if (!_E059)
				{
					_E059 = true;
					switch (_E073)
					{
					case Weapon.EMalfunctionState.Feed:
						_E038.ThrowPatronAsLoot(_E039.MalfState.MalfunctionedAmmo, ((_E00E)this)._E001);
						break;
					case Weapon.EMalfunctionState.Jam:
						_E038.SpawnShellAfterJam();
						break;
					case Weapon.EMalfunctionState.Misfire:
					case Weapon.EMalfunctionState.HardSlide:
					case Weapon.EMalfunctionState.SoftSlide:
						_E038.StartSpawnMisfiredCartridge(((_E00E)this)._E001.Velocity);
						break;
					}
				}
			}

			[CompilerGenerated]
			private new void _E000()
			{
				((_E00E)this)._E001.Skills.WeaponFixAction.Complete();
			}
		}

		private sealed class _E02E : _E00E
		{
			private bool _E00F;

			private _EB13 _E025;

			private Callback _E074;

			private bool _E075;

			public _E02E(FirearmController controller)
				: base(controller)
			{
			}

			internal void Start(Callback finishCallback, bool rollToZeroCamora)
			{
				_E074 = finishCallback;
				_E025 = _E039.GetCurrentMagazine() as _EB13;
				_E075 = rollToZeroCamora;
				Start();
				_E002.IsAiming = false;
				if (rollToZeroCamora)
				{
					_E037.RollToZeroCamora(roll: true);
				}
				else
				{
					_E037.SetRollCylinder(roll: true);
				}
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}

			public override void Reset()
			{
				base.Reset();
				_E00F = false;
				_E025 = null;
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					OnMagPuttedToRig();
				}
			}

			public override void OnMagPuttedToRig()
			{
				if (!_E00F)
				{
					_E00F = true;
					State = EOperationState.Finished;
					_E002.InitiateOperation<_E011>().Start();
					_E025.IncrementCamoraIndex(_E075);
					_E037.SetCamoraIndex(_E025.CurrentCamoraIndex);
					_E037.SetRollCylinder(roll: false);
					_E037.RollToZeroCamora(roll: false);
					_E074?.Succeed();
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}
		}

		public sealed class _E02F : _E008
		{
			private const float _E006 = 0.5f;

			private float _E007;

			private bool _E008;

			public _E02F(FirearmController controller)
				: base(controller)
			{
			}

			public override void Start(Item item, Callback callback)
			{
				_E007 = 0f;
				_E008 = false;
				base.Start(item, callback);
			}

			public override void FastForward()
			{
				if (!_E008)
				{
					_E008 = true;
					OnBackpackDropEvent();
				}
			}

			public override void Update(float deltaTime)
			{
				base.Update(deltaTime);
				if (!_E008 && _E007 > 0.5f)
				{
					_E008 = true;
					OnBackpackDropEvent();
				}
				_E007 += deltaTime;
			}
		}

		public sealed class _E030 : _E00E
		{
			private Action _E00B;

			private Action _E076;

			private bool _E021;

			private _EA12 _E056;

			private _EA6A _E077;

			private bool _E078;

			public _E030(FirearmController controller)
				: base(controller)
			{
				controller._player.Logger.LogInfo(_ED3E._E000(157637));
			}

			public void Start(Action onWeaponAppear)
			{
				_E002._player.Logger.LogInfo(_ED3E._E000(157684));
				_E00B = onWeaponAppear;
				Start();
				_E037.SetActiveParam(active: true);
				_E037.SetLayerWeight(_E037.LACTIONS_LAYER_INDEX, 0);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.WEAPON_SIZE_MODIFIER_PARAM_HASH, _E039.CalculateCellSize().X);
				int chamberAmmoCount = _E039.ChamberAmmoCount;
				int currentMagazineCount = _E039.GetCurrentMagazineCount();
				_E077 = _E039.GetCurrentMagazine();
				if (_E039.HasChambers)
				{
					_E037.SetAmmoInChamber(chamberAmmoCount);
				}
				else
				{
					_E037.SetHammerArmed(_E039.Armed);
				}
				if (_E039.GetCurrentMagazine() is _EB13 obj)
				{
					bool hammerArmed = !_E039.CylinderHammerClosed;
					_E037.SetHammerArmed(hammerArmed);
					_E037.SetCamoraIndex(obj.CurrentCamoraIndex);
					for (int i = 0; i < obj.Count; i++)
					{
						if (obj.Camoras[i].ContainedItem != null)
						{
							_E039.ShellsInChambers[i] = null;
							_E038.RemoveShellInWeapon(i);
						}
					}
				}
				if (_E039.IsMultiBarrel)
				{
					for (int j = 0; j < _E039.Chambers.Length; j++)
					{
						if (_E039.Chambers[j].ContainedItem != null)
						{
							_E039.ShellsInChambers[j] = null;
							_E038.RemoveShellInWeapon(j);
						}
					}
				}
				_E037.SetAmmoOnMag(currentMagazineCount);
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				base._E001.Skills.OnWeaponDraw(_E039);
				bool flag = (_E078 = _E077 == null || _E077.IsAmmoCompatible(_E039.Chambers));
				_E037.SetAmmoCompatible(flag);
				if (_E078 && _E077 != null && _E077.Count > 0 && _E002.Item.Chambers.Length != 0 && _E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 0);
				}
				if (_E077 != null && chamberAmmoCount == 0 && currentMagazineCount > 0 && flag && _E002.Item.Chambers.Length != 0)
				{
					Weapon.EMalfunctionState state = _E002.Item.MalfState.State;
					if (state == Weapon.EMalfunctionState.Misfire)
					{
						_E002.Item.MalfState.ChangeStateSilent(Weapon.EMalfunctionState.None);
					}
					_ECD8<_EB2E> obj2 = _E077.Cartridges.PopTo(_E002._player._E0DE, new _EB20(_E002.Item.Chambers[0]));
					_E002.Item.MalfState.ChangeStateSilent(state);
					if (obj2.Value != null)
					{
						_E038.RemoveAllShells();
						base._E001.UpdatePhones();
						_E056 = (_EA12)obj2.Value.ResultItem;
					}
				}
			}

			public override void Reset()
			{
				base.Reset();
				_E00B = null;
				_E076 = null;
				_E077 = null;
				_E056 = null;
				_E078 = false;
			}

			public override void OnAddAmmoInChamber()
			{
				if (_E056 != null)
				{
					_E037.SetAmmoOnMag(_E077.Count);
					_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
					if (_E039.HasChambers)
					{
						_E038.SetRoundIntoWeapon(_E056);
					}
				}
			}

			public override void WeaponAppeared()
			{
				if (_E078 && _E077 != null && _E002.Item.Chambers.Length != 0 && _E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					_E000();
				}
				if (_E039.MalfState.State != 0)
				{
					base._E001.NeedRepairMalfPhraseSituation(_E039.MalfState.State, _E039.MalfState.IsKnownMalfType(_E002._player.ProfileId));
				}
				_E002.SetupProp();
				_E002._player.Logger.LogInfo(_ED3E._E000(157667));
				State = EOperationState.Finished;
				_E011 obj = _E002.InitiateOperation<_E011>();
				obj.Start();
				_E00B();
				Action<FirearmController> action = _E002.m__E007;
				_E002.m__E007 = null;
				action?.Invoke(_E002);
				if (_E076 != null)
				{
					obj.HideWeapon(_E076, _E021);
				}
				_E076 = null;
				base._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 0f);
				_E002._E00A();
			}

			public override void OnIdleStartEvent()
			{
				if (State != EOperationState.Finished)
				{
					WeaponAppeared();
				}
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				_E021 = fastDrop;
				_E076 = onHidden;
			}

			public override void SetInventoryOpened(bool opened)
			{
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					_E037.Animator.Play(_E037.FullIdleStateName, 1, 0.1f);
					WeaponAppeared();
				}
			}

			public override bool CanChangeLightState(_E6C5[] lightsStates)
			{
				return false;
			}
		}

		public sealed class _E031 : _E01B
		{
			public _E031(FirearmController controller)
				: base(controller)
			{
			}

			public override void Start()
			{
				base.Start();
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
				{
					_EA12 ammoToFire = _E039.MalfState.AmmoToFire;
					ammoToFire.IsUsed = false;
					_E5CB instance = Singleton<_E5CB>.Instance;
					float modsCoolFactor;
					float currentOverheat = _E039.GetCurrentOverheat(_E62A.PastTime, instance.Overheat, out modsCoolFactor);
					_E002.ShotMisfired(ammoToFire, _E039.MalfState.State, currentOverheat);
				}
			}

			public override void OnShellEjectEvent()
			{
				_E002._E00C.StartSpawnShell(_E002._player.Velocity * 0.66f);
				_E002._E00C.SetRoundIntoWeapon(_E039.MalfState.MalfunctionedAmmo);
				_E002._E00C.MoveAmmoFromChamberToShellPort(ammoIsUsed: false);
			}

			public override void FastForward()
			{
				OnMalfunctionOffEvent();
			}

			public override void OnMalfunctionOffEvent()
			{
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				_E037.SetLayerWeight(_E037.MALFUNCTION_LAYER_INDEX, 1);
				_E037.Malfunction(-1);
				_E000();
			}
		}

		public class _E032 : _E00E
		{
			private new Action m__E000;

			private WeaponPrefab _E079;

			private bool _E07A;

			private bool _E021;

			private Action _E022;

			public _E032(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(bool isLauncherEnabled, Action callback = null)
			{
				this.m__E000 = callback;
				_E07A = isLauncherEnabled;
				_E079 = _E002.m__E01F.LauncherWeaponPrefab;
				if (_E079.FirearmsAnimator == null)
				{
					_E079.Init(null, parent: true);
				}
				_E037.SetLauncher(isLauncherEnabled);
				if (isLauncherEnabled)
				{
					_E079.SetUnderbarrelFastAnimator(base._E001);
					_E001(val: true);
				}
				else
				{
					_E079.FirearmsAnimator.SetActiveParam(active: false);
				}
				_E039.IsUnderBarrelDeviceActive = isLauncherEnabled;
				if (isLauncherEnabled)
				{
					_E002._E009();
					base._E001.UpdateLauncherBones(isLauncherEnabled, _E079);
				}
				_E000();
				Start();
			}

			public override void Reset()
			{
				base.Reset();
				_E022 = null;
				_E021 = false;
				this.m__E000 = null;
				_E079 = null;
				_E07A = false;
			}

			public override void SetInventoryOpened(bool opened)
			{
				SetAiming(isAiming: false);
				SetTriggerPressed(pressed: false);
				_E002.InventoryOpened = opened;
				_E037.SetInventory(opened);
			}

			public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
			{
				_E022 = onHidden;
				_E021 = fastDrop;
			}

			public override void LauncherAppeared()
			{
				State = EOperationState.Finished;
				Action action = _E022;
				bool fastDrop = _E021;
				base._E001.UpdateFirstPersonGrip(GripPose.EGripType.Common, _E002.HandsHierarchy);
				base._E001.ProceduralWeaponAnimation._E007(_E002, _E002.UnderbarrelWeapon, null, _E002.m__E013);
				base._E001.ProceduralWeaponAnimation.IsGrenadeLauncher = true;
				_E015 obj = _E002.InitiateOperation<_E015>();
				if (action != null)
				{
					this.m__E000 = null;
				}
				obj.Start(this.m__E000);
				if (action != null)
				{
					obj.HideWeapon(action, fastDrop);
				}
			}

			public override void OnEnd()
			{
				base.OnEnd();
				_E079 = null;
			}

			public override void LauncherDisappeared()
			{
				Action action = _E022;
				bool fastDrop = _E021;
				State = EOperationState.Finished;
				base._E001.UpdateLauncherBones(value: false, _E079);
				base._E001.UpdateFirstPersonGrip(GripPose.EGripType.Common, _E002.HandsHierarchy);
				base._E001.ProceduralWeaponAnimation._E007(_E002, _E002.Item, _E002.m__E00D, _E002.m__E013);
				base._E001.ProceduralWeaponAnimation.IsGrenadeLauncher = false;
				_E001(val: false);
				_E079.ResetUnderbarrelFastAnimator(base._E001);
				_E011 obj = _E002.InitiateOperation<_E011>();
				if (action != null)
				{
					this.m__E000 = null;
				}
				obj.Start(this.m__E000);
				if (action != null)
				{
					obj.HideWeapon(action, fastDrop);
				}
			}

			public override void FastForward()
			{
				if (_E07A)
				{
					LauncherAppeared();
				}
				else
				{
					LauncherDisappeared();
				}
			}

			private new void _E000()
			{
				if (_E002.Blindfire)
				{
					_E002.Blindfire = false;
					base._E001.ProceduralWeaponAnimation.StartBlindFire(0);
				}
			}

			private new void _E001(bool val)
			{
				_E079.Animator.enabled = val;
				if (val)
				{
					_E079.FirearmsAnimator.AddEventsConsumer(_E002);
					_E002._E001 = _E079.FirearmsAnimator;
					_E002.AnimationEventsEmitter = _E079.AnimationEventsEmitter;
					_E079.FirearmsAnimator.SetActiveParam(active: true);
				}
				else
				{
					_E079.FirearmsAnimator.RemoveEventsConsumer(_E002);
					_E002._E001 = _E002.m__E00D.FirearmsAnimator;
					_E002.AnimationEventsEmitter = _E002.m__E00D.AnimationEventsEmitter;
				}
			}
		}

		public class _E033 : _E6FC
		{
			public const string SHELLPORT_TRANSFORM_NAME = "shellport";

			public const string PATRON_IN_WEAPON_TRANSFORM_NAME = "patron_in_weapon";

			private Player m__E000;

			private FirearmController m__E001;

			private WeaponPrefab m__E002;

			private _EA62 m__E003;

			private bool m__E004;

			private FirearmsEffects m__E005;

			private WeaponSoundPlayer m__E006;

			private Transform m__E007;

			private Vector3 m__E008;

			private Transform m__E009;

			private AmmoPoolObject m__E00A;

			private AmmoPoolObject m__E00B;

			private ShellExtractionData m__E00C;

			private BifacialTransform m__E00D = new BifacialTransform();

			private WaitForEndOfFrame _E00E = new WaitForEndOfFrame();

			public FirearmsEffects FirearmsEffects => this.m__E005;

			public WeaponSoundPlayer WeaponSoundPlayer => this.m__E006;

			private static WeaponSounds _E00F => Singleton<BetterAudio>.Instance.MiscCollisionSounds;

			public BifacialTransform Fireport => this.m__E00D;

			public WeaponPrefab LauncherWeaponPrefab => this.m__E002;

			public void Init(Player player, FirearmController controller, _EA62 launcher)
			{
				this.m__E002 = controller.m__E00D._objectInstance.gameObject.GetComponentInChildren<WeaponPrefab>();
				_E000(player, controller, launcher);
			}

			public void Init(Player player, FirearmController controller, _EA62 launcher, GameObject underbarrelPrefab)
			{
				this.m__E002 = underbarrelPrefab.GetComponent<WeaponPrefab>();
				_E000(player, controller, launcher);
			}

			public void InitWeaponSoundPlayer()
			{
				_E003();
			}

			private void _E000(Player player, FirearmController controller, _EA62 launcher)
			{
				this.m__E000 = player;
				this.m__E001 = controller;
				this.m__E003 = launcher;
				this.m__E00C = this.m__E002.GetComponent<ShellExtractionData>();
				_E001();
				_E002();
				_E004();
				_E005();
			}

			private void _E001()
			{
				this.m__E005 = this.m__E002.gameObject.AddComponent<FirearmsEffects>();
				this.m__E005.Init(this.m__E002.transform);
			}

			private void _E002()
			{
				this.m__E00D.Original = _E38B.FindTransformRecursive(this.m__E002.transform, _ED3E._E000(64493));
			}

			private void _E003()
			{
				if (!this.m__E004)
				{
					this.m__E006 = this.m__E002.transform.GetComponent<WeaponSoundPlayer>();
					this.m__E006.Init(this.m__E001, this.m__E00D, this.m__E000);
					this.m__E004 = true;
				}
			}

			public void StartSpawnShell(Vector3 playerVelocity, int shellPortIndex = 0)
			{
				if (_E007())
				{
					this.m__E002.StartCoroutine(_E00C(playerVelocity, shellPortIndex));
				}
			}

			private void _E004()
			{
				this.m__E007 = _E38B.FindTransformRecursive(this.m__E002.transform, _ED3E._E000(64030));
				this.m__E009 = _E38B.FindTransformRecursive(this.m__E002.transform, _ED3E._E000(155651));
				if (this.m__E007 != null)
				{
					this.m__E008 = this.m__E007.localPosition;
				}
			}

			private void _E005()
			{
				if (this.m__E003.Chamber.ContainedItem != null)
				{
					SetRoundIntoWeapon((_EA12)this.m__E003.Chamber.ContainedItem);
				}
				if (this.m__E003.ShellsInChambers == null)
				{
					return;
				}
				for (int i = 0; i < this.m__E003.ShellsInChambers.Length; i++)
				{
					AmmoTemplate ammoTemplate = this.m__E003.ShellsInChambers[i];
					if (ammoTemplate != null)
					{
						SetPatronInShellPort(Singleton<_E760>.Instance.CreateFromPool<AmmoPoolObject>(ammoTemplate.Prefab), i);
					}
				}
			}

			private bool _E006()
			{
				return _E8A8.Instance.Distance(this.m__E002.transform.position) < EFTHardSettings.Instance.PATRONS_MANIPULATIONS_VISIBLE_DISTANCE;
			}

			private bool _E007()
			{
				return _E8A8.Instance.Distance(this.m__E002.transform.position) < EFTHardSettings.Instance.FLYING_SHELLS_VISIBLE_DISTANCE;
			}

			public void MoveAmmoFromChamberToShellPort(bool ammoIsUsed, int chamberIndex = 0)
			{
				AmmoPoolObject ammoPoolObject = this.m__E00A;
				this.m__E00A = null;
				if (!(ammoPoolObject == null))
				{
					SetPatronInShellPort(ammoPoolObject, chamberIndex);
					if (_E006())
					{
						ammoPoolObject.SetUsed(ammoIsUsed);
					}
				}
			}

			public void SetRoundIntoWeapon(_EA12 ammo)
			{
				if (_E006())
				{
					if (this.m__E00A != null)
					{
						UnityEngine.Debug.LogWarning(_ED3E._E000(155692));
						DestroyPatronInWeapon();
					}
					Transform transform = this.m__E009;
					if (transform.childCount > 0)
					{
						AssetPoolObject.ReturnToPool(transform.GetChild(0).gameObject);
					}
					AmmoPoolObject ammoPoolObject = _E009(ammo);
					ammoPoolObject.SetUsed(ammo.IsUsed);
					ParentAmmoOrShellToTransform(ammoPoolObject.gameObject, transform);
					this.m__E00A = ammoPoolObject;
				}
			}

			public void SetPatronInShellPort(AmmoPoolObject ammoObject, int shellTransformIndex = 0)
			{
				if (this.m__E00B != null)
				{
					UnityEngine.Debug.LogError(_ED3E._E000(155724));
					AssetPoolObject.ReturnToPool(this.m__E00B.gameObject);
				}
				this.m__E00B = ammoObject;
				ParentAmmoOrShellToTransform(ammoObject.gameObject, this.m__E007);
			}

			public bool DestroyPatronInWeapon()
			{
				if (this.m__E00A == null)
				{
					return false;
				}
				AssetPoolObject.ReturnToPool(this.m__E00A.gameObject);
				this.m__E00A = null;
				return true;
			}

			public void RemoveShellInWeapon()
			{
				if (_E006())
				{
					_E008();
				}
			}

			private void _E008()
			{
				if (!(this.m__E00B == null))
				{
					AssetPoolObject.ReturnToPool(this.m__E00B.gameObject);
					this.m__E00B = null;
				}
			}

			public bool HasPatronInWeapon()
			{
				return this.m__E00A != null;
			}

			public bool HasShellInWeapon()
			{
				return this.m__E00B != null;
			}

			private static AmmoPoolObject _E009(Item ammo)
			{
				GameObject gameObject = Singleton<_E760>.Instance.CreateItem(ammo, isAnimated: true);
				AmmoPoolObject component = gameObject.GetComponent<AmmoPoolObject>();
				if (component == null)
				{
					throw new Exception(string.Concat(_ED3E._E000(155750), gameObject, _ED3E._E000(155793)));
				}
				return component;
			}

			public static void ParentAmmoOrShellToTransform(GameObject shell, Transform shellParent)
			{
				shell.transform.position = shellParent.position;
				shell.transform.rotation = shellParent.rotation;
				shell.transform.localRotation *= Quaternion.Euler(90f, 0f, 0f);
				shell.transform.SetParent(shellParent, worldPositionStays: true);
				shell.transform.localScale = Vector3.one;
				shell.transform.localPosition = Vector3.zero;
				shell.SetActive(value: true);
			}

			private AmmoPoolObject _E00A(Vector3 playerVelocity, AmmoPoolObject shell)
			{
				AmmoPoolObject ammoPoolObject = Singleton<GameWorld>.Instance.SpawnShellInTheWorld(ref shell);
				ammoPoolObject.transform.parent = null;
				Vector3 shotRotationVector = this.m__E00C.GetShotRotationVector();
				Vector3 shotAdditionalForce = this.m__E00C.GetShotAdditionalForce();
				Vector3 force = (this.m__E007.localPosition - this.m__E008) * this.m__E00C.GetShotShellForceMultiplier() + shotAdditionalForce;
				_E00B(force, shotRotationVector, ammoPoolObject, playerVelocity);
				ammoPoolObject.StartAutoDestroyCountDown();
				return ammoPoolObject;
			}

			private void _E00B(Vector3 force, Vector3 torque, AmmoPoolObject shell, Vector3 parentForce)
			{
				shell.EnablePhysics(force, torque, parentForce, this.m__E007.transform.forward);
				shell.gameObject.layer = _E37B.ShellsLayer;
				shell.Shell.CollisionListener = this;
			}

			private IEnumerator _E00C(Vector3 playerVelocity, int shellPortIndex = 0)
			{
				if (!(this.m__E00B == null))
				{
					AmmoPoolObject ammoPoolObject = this.m__E00B;
					this.m__E00B = null;
					yield return _E00E;
					if (!(ammoPoolObject == null))
					{
						_E00A(playerVelocity, ammoPoolObject).SetUsed(isUsed: true);
					}
				}
			}

			private void _E00D(Vector3 position, BaseBallistic.ESurfaceSound material, ECaliber caliber)
			{
				SoundBank soundBank = null;
				float volume = 1f;
				switch (material)
				{
				case BaseBallistic.ESurfaceSound.Concrete:
				case BaseBallistic.ESurfaceSound.Asphalt:
					soundBank = caliber switch
					{
						ECaliber.ShellHeavy => _E00F.ShellHeavyConcrete, 
						ECaliber.Shell12Cal => _E00F.Shell12calConcrete, 
						ECaliber.Shell556Mm => _E00F.Shell556mmConcrete, 
						ECaliber.Shell9Mm => _E00F.Shell9mmConcrete, 
						_ => null, 
					};
					break;
				case BaseBallistic.ESurfaceSound.Metal:
					soundBank = caliber switch
					{
						ECaliber.ShellHeavy => _E00F.ShellHeavyMetal, 
						ECaliber.Shell12Cal => _E00F.Shell12calMetal, 
						ECaliber.Shell556Mm => _E00F.Shell556mmMetal, 
						ECaliber.Shell9Mm => _E00F.Shell9mmMetal, 
						_ => null, 
					};
					break;
				case BaseBallistic.ESurfaceSound.Wood:
					soundBank = caliber switch
					{
						ECaliber.ShellHeavy => _E00F.ShellHeavyWood, 
						ECaliber.Shell12Cal => _E00F.Shell12calWood, 
						ECaliber.Shell556Mm => _E00F.Shell556mmWood, 
						ECaliber.Shell9Mm => _E00F.Shell9mmWood, 
						_ => null, 
					};
					break;
				case BaseBallistic.ESurfaceSound.Soil:
				case BaseBallistic.ESurfaceSound.Gravel:
					soundBank = caliber switch
					{
						ECaliber.ShellHeavy => _E00F.ShellHeavySoil, 
						ECaliber.Shell12Cal => _E00F.Shell12calSoil, 
						ECaliber.Shell556Mm => _E00F.Shell556mmSoil, 
						ECaliber.Shell9Mm => _E00F.Shell9mmSoil, 
						_ => null, 
					};
					break;
				case BaseBallistic.ESurfaceSound.Grass:
					soundBank = caliber switch
					{
						ECaliber.ShellHeavy => _E00F.ShellHeavySoil, 
						ECaliber.Shell12Cal => _E00F.Shell12calSoil, 
						ECaliber.Shell556Mm => _E00F.Shell556mmSoil, 
						ECaliber.Shell9Mm => _E00F.Shell9mmSoil, 
						_ => null, 
					};
					break;
				case BaseBallistic.ESurfaceSound.Plastic:
					soundBank = caliber switch
					{
						ECaliber.ShellHeavy => _E00F.ShellHeavyPlastic, 
						ECaliber.Shell12Cal => _E00F.Shell12calPlastic, 
						ECaliber.Shell556Mm => _E00F.Shell556mmPlastic, 
						ECaliber.Shell9Mm => _E00F.Shell9mmPlastic, 
						_ => null, 
					};
					volume = 0.6f;
					break;
				}
				if (soundBank != null)
				{
					Singleton<BetterAudio>.Instance.PlayAtPoint(position + Vector3.up / 4f, soundBank, (int)soundBank.SourceType, _E8A8.Instance.Distance(position), volume, -1f, EnvironmentType.Outdoor, EOcclusionTest.Fast, this.m__E000);
				}
			}

			public void InvokeShellCollision(Vector3 position, BaseBallistic.ESurfaceSound material, ECaliber caliber)
			{
				_E00D(position, material, caliber);
			}

			public void Clear()
			{
				RemoveShellInWeapon();
				DestroyPatronInWeapon();
			}
		}

		public class _E034 : _E011
		{
			public enum EUtilityType
			{
				None,
				ExamineWeapon,
				CheckChamber,
				CheckMagazine,
				CheckFireMode
			}

			private const float _E04B = 2.5f;

			private float _E04C;

			private bool _E04D;

			private EUtilityType _E04E;

			public _E034(FirearmController controller)
				: base(controller)
			{
			}

			public void Start(EUtilityType utilityType)
			{
				_E04E = utilityType;
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
				State = EOperationState.Executing;
				_E04C = 0f;
			}

			public override void OnIdleStartEvent()
			{
				if (State == EOperationState.Ready)
				{
					base.OnIdleStartEvent();
					State = EOperationState.Finished;
					_E002.InitiateOperation<_E011>().Start();
				}
			}

			public override void OnUtilityOperationStartEvent()
			{
				State = EOperationState.Ready;
			}

			public override bool CanStartReload()
			{
				return false;
			}

			public override void Reset()
			{
				_E04E = EUtilityType.None;
				base.Reset();
			}

			public override bool CheckAmmo()
			{
				return false;
			}

			public override bool CheckChamber()
			{
				return false;
			}

			public override bool CheckFireMode()
			{
				return false;
			}

			public override void ReloadMag(_EA6A magazine, _EB22 gridItemAddress, Callback finishCallback, Callback startCallback)
			{
			}

			public override void ReloadWithAmmo(_E9CF ammoPack, Callback finishCallback, Callback startCallback)
			{
			}

			public override void ReloadRevolverDrum(_E9CF ammoPack, Callback finishCallback, Callback startCallback, bool quickReload = false)
			{
			}

			public override void QuickReloadMag(_EA6A magazine, Callback finishCallback, Callback startCallback)
			{
			}

			public override void ReloadGrenadeLauncher(_E9CF ammoPack, Callback callback)
			{
			}

			public override void SetTriggerPressed(bool pressed)
			{
				if (_E04E == EUtilityType.ExamineWeapon)
				{
					OnUtilityOperationStartEvent();
					OnIdleStartEvent();
					_E002.CurrentOperation.SetTriggerPressed(pressed);
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E04D = opened;
				if (!_E04D)
				{
					_E04C = 0f;
				}
				base.SetInventoryOpened(opened);
			}

			public override void Update(float deltaTime)
			{
				base.Update(deltaTime);
				if (State != EOperationState.Executing || _E04D)
				{
					return;
				}
				if (_E04C > 2.5f)
				{
					if (_E037 != null)
					{
						UnityEngine.Debug.LogError(_ED3E._E000(157311) + _E037.Animator.name);
					}
					else
					{
						UnityEngine.Debug.LogError(_ED3E._E000(157339));
					}
					State = EOperationState.Ready;
					OnIdleStartEvent();
				}
				else
				{
					_E04C += deltaTime;
				}
			}

			public override void SetAiming(bool isAiming)
			{
			}

			public override bool ExamineWeapon()
			{
				return true;
			}

			public override void OnShellEjectEvent()
			{
				_E002._E00C.StartSpawnShell(_E002._player.Velocity * 0.66f);
				for (int i = 0; i < _E039.ShellsInChambers.Length; i++)
				{
					_E039.ShellsInChambers[i] = null;
				}
				_E037.SetShellsInWeapon(_E039.ShellsInWeaponCount);
			}
		}

		[CompilerGenerated]
		private sealed class _E036<_E077> where _E077 : FirearmController
		{
			public Player player;

			public _E077 controller;

			internal bool _E000()
			{
				if (player.AIData == null)
				{
					return false;
				}
				return player.AIData.IsNoOffsetShooting;
			}

			internal void _E001()
			{
				ProceduralWeaponAnimation proceduralWeaponAnimation = player.ProceduralWeaponAnimation;
				proceduralWeaponAnimation.AvailableScopesChanged = (Action)Delegate.Remove(proceduralWeaponAnimation.AvailableScopesChanged, new Action(controller.ValidateCurrentScopeIndex));
			}

			internal void _E002()
			{
				((FirearmController)controller)._E001.RemoveEventsConsumer(controller);
			}

			internal void _E003(bool visible)
			{
				((FirearmController)controller)._E00C.SetVisiblePatronInWeapon(visible);
			}

			internal void _E004()
			{
				((FirearmController)controller)._E001.SetPatronInWeaponVisibleEvent -= delegate(bool visible)
				{
					((FirearmController)controller)._E00C.SetVisiblePatronInWeapon(visible);
				};
			}

			internal void _E005()
			{
				controller._player.Skills.WeaponMastered -= controller.OnCurrentWeaponBeingMastered;
				controller._player.Skills.OnSkillLevelChanged -= controller._E003;
				controller._player.MovementContext.PhysicalConditionChanged -= controller._E004;
			}
		}

		[CompilerGenerated]
		private sealed class _E038
		{
			public Action callback;

			public FirearmController _003C_003E4__this;

			internal void _E000()
			{
				callback();
				_003C_003E4__this._player.MovementContext.OnStateChanged += _003C_003E4__this._E011;
				_003C_003E4__this._player.Physical.OnSprintStateChangedEvent += _003C_003E4__this._E010;
			}
		}

		[CompilerGenerated]
		private sealed class _E039
		{
			public Player._E013 inventoryOperation;

			public Action callback;

			internal void _E000()
			{
				inventoryOperation.Confirm();
				callback();
			}
		}

		private new List<_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>> m__E003 = new List<_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>>(4);

		private List<_E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>> m__E004 = new List<_E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>>(5);

		public const int CALCULATOR_SEED = 1;

		private const string m__E005 = "Cant StartReload";

		[CompilerGenerated]
		private Action m__E006;

		[CompilerGenerated]
		private Action<FirearmController> m__E007;

		[CompilerGenerated]
		private Action m__E008;

		protected static readonly List<_EA12> _preallocatedAmmoList = new List<_EA12>(10);

		private static readonly List<_EC26> m__E009 = new List<_EC26>(10);

		private static readonly RaycastHit[] m__E00A = new RaycastHit[8];

		public _E333 CCV;

		public Transform GunBaseTransform;

		public BifacialTransform Fireport;

		public BifacialTransform[] MultiBarrelsFireports;

		public bool _blindfire;

		public int CurrentChamberIndex;

		public float HipInaccuracy;

		public _EAA8[] AimingDevices;

		internal Func<bool> _E00B;

		internal _E6F9 _E00C;

		internal new FirearmsAnimator _E001;

		protected float WeaponLn;

		protected bool AimingInterruptedByOverlap;

		protected bool _isAiming;

		protected float _aimingSens = -1f;

		protected _EC1E BallisticsCalculator;

		private WeaponPrefab m__E00D;

		private _EC14 m__E00E;

		private new bool m__E002;

		private bool m__E00F;

		private float m__E010;

		private float m__E011 = 0.001f;

		private float m__E012;

		private _E74F._E000 m__E013;

		private bool m__E014;

		private bool m__E015;

		private bool m__E016;

		private int m__E017;

		private _E393<float> m__E018;

		private _E393<float> m__E019;

		private Func<RaycastHit, bool> m__E01A;

		private OneOffWeaponSettings m__E01B;

		private float m__E01C = 1f;

		private WeaponSoundPlayer m__E01D;

		private int m__E01E;

		private _E033 m__E01F;

		public _EA62 UnderbarrelWeapon;

		[CompilerGenerated]
		private bool m__E020;

		[CompilerGenerated]
		private bool m__E021;

		public new Weapon Item => base.Item as Weapon;

		public override FirearmsAnimator FirearmsAnimator => this._E001;

		public override string LoggerDistinctId => string.Format(_ED3E._E000(163638), _player.ProfileId, _player.Profile.Info.Nickname, this);

		public _E74F._E000 BuffInfo => this.m__E013;

		public bool IsOverlap => this.m__E010 > 0f;

		public float OverlapValue => this.m__E010;

		public bool IsSilenced
		{
			[CompilerGenerated]
			get
			{
				return this.m__E020;
			}
			[CompilerGenerated]
			private set
			{
				this.m__E020 = value;
			}
		}

		public int CurrentMasteringLevel => _player.Skills.GetMastering(Item.TemplateId)?.Level ?? 0;

		public float TotalErgonomics => this.m__E019.Value;

		public float ErgonomicWeight => this.m__E018.Value;

		public BifacialTransform CurrentFireport
		{
			get
			{
				if (!Item.IsMultiBarrel)
				{
					return Fireport;
				}
				return MultiBarrelsFireports[CurrentChamberIndex];
			}
		}

		public override float AimingSensitivity
		{
			get
			{
				if (!IsAiming)
				{
					return _player.GetAimingSensitivity();
				}
				return _aimingSens;
			}
		}

		public virtual Vector3 WeaponDirection => CurrentFireport.Original.TransformDirection(_player.LocalShotDirection);

		public WeaponSoundPlayer WeaponSoundPlayer => this.m__E01D;

		public bool IsBirstOf2Start
		{
			[CompilerGenerated]
			get
			{
				return this.m__E021;
			}
			[CompilerGenerated]
			set
			{
				this.m__E021 = value;
			}
		}

		protected bool IsStationaryWeapon => _player.MovementContext.StationaryWeapon != null;

		protected _E00E CurrentOperation => base.CurrentHandsOperation as _E00E;

		protected internal virtual bool IsTriggerPressed
		{
			get
			{
				return this.m__E014;
			}
			set
			{
				if (!value)
				{
					this.m__E008?.Invoke();
				}
				this.m__E014 = value;
			}
		}

		public override bool IsAiming
		{
			get
			{
				return _isAiming;
			}
			protected set
			{
				if (!value)
				{
					_player.Physical.HoldBreath(enable: false);
				}
				if (_isAiming != value)
				{
					_isAiming = value;
					_player.Skills.FastAimTimer.Target = (value ? 0f : 2f);
					_player.MovementContext.SetAimingSlowdown(IsAiming, 0.33f + this.m__E013.AimMovementSpeed);
					_player.Physical.Aim((_isAiming && _player.MovementContext.StationaryWeapon == null) ? ErgonomicWeight : 0f);
					this._E00C.SetAiming(_isAiming);
					UpdateSensitivity();
					AimingChanged(value);
				}
			}
		}

		public bool Malfunction
		{
			get
			{
				return this.m__E00F;
			}
			set
			{
				if (value)
				{
					this.m__E008?.Invoke();
				}
				this.m__E00F = value;
			}
		}

		public bool InventoryOpened
		{
			get
			{
				return this.m__E002;
			}
			set
			{
				this.m__E002 = value;
				if (this.m__E002)
				{
					SetCompassState(active: false);
					BlindFire(0);
				}
			}
		}

		public bool Blindfire
		{
			get
			{
				return _blindfire;
			}
			set
			{
				_blindfire = value;
				if (_blindfire)
				{
					SetCompassState(active: false);
				}
			}
		}

		public event Action OnShot
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m__E006;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E006, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m__E006;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E006, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action<FirearmController> OnReadyToOperate
		{
			[CompilerGenerated]
			add
			{
				Action<FirearmController> action = this.m__E007;
				Action<FirearmController> action2;
				do
				{
					action2 = action;
					Action<FirearmController> value2 = (Action<FirearmController>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E007, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<FirearmController> action = this.m__E007;
				Action<FirearmController> action2;
				do
				{
					action2 = action;
					Action<FirearmController> value2 = (Action<FirearmController>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E007, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action BreakLoop
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m__E008;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E008, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m__E008;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E008, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public float GetTotalMalfunctionChance(_EA12 ammoToFire, float overheat, out double durabilityMalfChance, out float magMalfChance, out float ammoMalfChance, out float overheatMalfChance, out float weaponDurability)
		{
			durabilityMalfChance = 0.0;
			magMalfChance = 0f;
			ammoMalfChance = 0f;
			overheatMalfChance = 0f;
			weaponDurability = 0f;
			if (!Item.AllowMalfunction)
			{
				return 0f;
			}
			_E5CB instance = Singleton<_E5CB>.Instance;
			_E5CB._E02C malfunction = instance.Malfunction;
			_E5CB._E02D overheat2 = instance.Overheat;
			_E5CB._E01D troubleShooting = instance.SkillsSettings.TroubleShooting;
			float ammoMalfChanceMult = malfunction.AmmoMalfChanceMult;
			float magazineMalfChanceMult = malfunction.MagazineMalfChanceMult;
			_EA6A currentMagazine = Item.GetCurrentMagazine();
			magMalfChance = ((currentMagazine == null) ? 0f : (currentMagazine.MalfunctionChance * magazineMalfChanceMult));
			ammoMalfChance = ((ammoToFire != null) ? ((ammoToFire.MalfMisfireChance + ammoToFire.MalfFeedChance) * ammoMalfChanceMult) : 0f);
			float num = Item.Repairable.Durability / (float)Item.Repairable.TemplateDurability * 100f;
			weaponDurability = Mathf.Floor(num);
			if (overheat >= overheat2.OverheatProblemsStart)
			{
				overheatMalfChance = Mathf.Lerp(overheat2.MinMalfChance, overheat2.MaxMalfChance, (overheat - overheat2.OverheatProblemsStart) / (overheat2.MaxOverheat - overheat2.OverheatProblemsStart));
			}
			overheatMalfChance *= (float)Item.Buff.MalfunctionProtections;
			if (weaponDurability > 59f)
			{
				durabilityMalfChance = (Math.Pow(Item.BaseMalfunctionChance + 1f, 3.0 + (double)(100f - weaponDurability) / (20.0 - 10.0 / Math.Pow((double)Item.FireRate / 10.0, 0.322))) - 1.0) / 1000.0;
			}
			else
			{
				durabilityMalfChance = (Math.Pow(Item.BaseMalfunctionChance + 1f, Math.Log10(Math.Pow(101f - weaponDurability, (50.0 - Math.Pow(weaponDurability, 1.286) / 4.8) / (Math.Pow(Item.FireRate, 0.17) / 2.9815 + 2.1)))) - 1.0) / 1000.0;
			}
			durabilityMalfChance *= (float)Item.Buff.MalfunctionProtections;
			if (Item.MalfState.HasMalfReduceChance(_player.ProfileId, Weapon.EMalfunctionSource.Durability))
			{
				durabilityMalfChance *= troubleShooting.EliteDurabilityChanceReduceMult;
			}
			if (Item.MalfState.HasMalfReduceChance(_player.ProfileId, Weapon.EMalfunctionSource.Magazine))
			{
				magMalfChance *= troubleShooting.EliteMagChanceReduceMult;
			}
			if (Item.MalfState.HasMalfReduceChance(_player.ProfileId, Weapon.EMalfunctionSource.Ammo))
			{
				ammoMalfChance *= troubleShooting.EliteAmmoChanceReduceMult;
			}
			if (num >= malfunction.DurRangeToIgnoreMalfs.x && num <= malfunction.DurRangeToIgnoreMalfs.y)
			{
				durabilityMalfChance = 0.0;
				ammoMalfChance = 0f;
				magMalfChance = 0f;
			}
			durabilityMalfChance = Mathf.Clamp01((float)durabilityMalfChance);
			return Mathf.Clamp01((float)Math.Round(durabilityMalfChance + (double)((ammoMalfChance + magMalfChance + overheatMalfChance) / 1000f), 5));
		}

		public float GetNextMalfunctionRandom()
		{
			return this.m__E00E.GetNextRandom();
		}

		public void GetMalfunctionSources(List<_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>> result, double durabilityMalfChance, float magMalfChance, float ammoMalfChance, float overheatMalfChance, bool hasAmmoInMag, bool isMagazineInserted)
		{
			result.Clear();
			result.Add(new _E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>((float)durabilityMalfChance, Weapon.EMalfunctionSource.Durability));
			if (ammoMalfChance > 0f)
			{
				result.Add(new _E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>(ammoMalfChance / 1000f, Weapon.EMalfunctionSource.Ammo));
			}
			if (magMalfChance > 0f && hasAmmoInMag && isMagazineInserted)
			{
				result.Add(new _E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>(magMalfChance / 1000f, Weapon.EMalfunctionSource.Magazine));
			}
			if (overheatMalfChance > 0f)
			{
				result.Add(new _E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>(overheatMalfChance / 1000f, Weapon.EMalfunctionSource.Overheat));
			}
		}

		public void GetSpecificMalfunctionVariants(List<_E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>> result, _EA12 ammo, Weapon.EMalfunctionSource malfunctionSource, float weaponDurability, bool hasAmmoInMag, bool isMagazineInserted, bool shouldCheckJam)
		{
			result.Clear();
			_E5CB._E02C malfunction = Singleton<_E5CB>.Instance.Malfunction;
			switch (malfunctionSource)
			{
			case Weapon.EMalfunctionSource.Durability:
				if (hasAmmoInMag && isMagazineInserted && Item.AllowFeed)
				{
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(malfunction.DurFeedWt, Weapon.EMalfunctionState.Feed));
				}
				if (Item.AllowMisfire)
				{
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(malfunction.DurMisfireWt, Weapon.EMalfunctionState.Misfire));
				}
				if (shouldCheckJam && Item.AllowJam)
				{
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(malfunction.DurJamWt, Weapon.EMalfunctionState.Jam));
				}
				if (hasAmmoInMag && Item.AllowSlide)
				{
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(malfunction.DurSoftSlideWt, Weapon.EMalfunctionState.SoftSlide));
				}
				if (weaponDurability <= 5f && hasAmmoInMag && Item.AllowSlide)
				{
					float first2 = Mathf.Lerp(malfunction.DurHardSlideMinWt, malfunction.DurHardSlideMaxWt, 1f - weaponDurability / 5f);
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(first2, Weapon.EMalfunctionState.HardSlide));
				}
				break;
			case Weapon.EMalfunctionSource.Ammo:
				if (Item.AllowMisfire)
				{
					float first = malfunction.AmmoMisfireWt / (ammo.MalfMisfireChance + ammo.MalfFeedChance);
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(first, Weapon.EMalfunctionState.Misfire));
				}
				if (hasAmmoInMag && isMagazineInserted && Item.AllowFeed)
				{
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(malfunction.AmmoFeedWt, Weapon.EMalfunctionState.Feed));
				}
				if (shouldCheckJam && Item.AllowJam)
				{
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(malfunction.AmmoJamWt, Weapon.EMalfunctionState.Jam));
				}
				break;
			case Weapon.EMalfunctionSource.Overheat:
				if (hasAmmoInMag && isMagazineInserted && Item.AllowFeed)
				{
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(malfunction.OverheatFeedWt, Weapon.EMalfunctionState.Feed));
				}
				if (shouldCheckJam && Item.AllowJam)
				{
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(malfunction.OverheatJamWt, Weapon.EMalfunctionState.Jam));
				}
				if (hasAmmoInMag && Item.AllowSlide)
				{
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(malfunction.OverheatSoftSlideWt, Weapon.EMalfunctionState.SoftSlide));
				}
				if (weaponDurability <= 5f && hasAmmoInMag && Item.AllowSlide)
				{
					float first3 = Mathf.Lerp(malfunction.OverheatHardSlideMinWt, malfunction.OverheatHardSlideMaxWt, 1f - weaponDurability / 5f);
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(first3, Weapon.EMalfunctionState.HardSlide));
				}
				break;
			case Weapon.EMalfunctionSource.Magazine:
				if (hasAmmoInMag && isMagazineInserted && Item.AllowFeed)
				{
					result.Add(new _E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>(1f, Weapon.EMalfunctionState.Feed));
				}
				break;
			case Weapon.EMalfunctionSource.Ammo | Weapon.EMalfunctionSource.Magazine:
				break;
			}
		}

		public Weapon.EMalfunctionState GetMalfunctionState(_EA12 ammoToFire, bool hasAmmoInMag, bool doesWeaponHaveBoltCatch, bool isMagazineInserted, float overheat, float fixSlideOverheat, out Weapon.EMalfunctionSource malfunctionSource)
		{
			malfunctionSource = Weapon.EMalfunctionSource.Durability;
			if (!Item.AllowMalfunction)
			{
				return Weapon.EMalfunctionState.None;
			}
			if (Item.MalfState.SlideOnOverheatReached && overheat > fixSlideOverheat && Item.AllowSlide && hasAmmoInMag)
			{
				malfunctionSource = Weapon.EMalfunctionSource.Overheat;
				return Weapon.EMalfunctionState.SoftSlide;
			}
			double durabilityMalfChance;
			float magMalfChance;
			float ammoMalfChance;
			float overheatMalfChance;
			float weaponDurability;
			float totalMalfunctionChance = GetTotalMalfunctionChance(ammoToFire, overheat, out durabilityMalfChance, out magMalfChance, out ammoMalfChance, out overheatMalfChance, out weaponDurability);
			float num = 0f;
			num = this.m__E00E.GetRandomFloat();
			if (num > totalMalfunctionChance)
			{
				return Weapon.EMalfunctionState.None;
			}
			List<_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>> list = this.m__E003;
			GetMalfunctionSources(list, durabilityMalfChance, magMalfChance, ammoMalfChance, overheatMalfChance, hasAmmoInMag, isMagazineInserted);
			malfunctionSource = _E375<Weapon.EMalfunctionSource>.GenerateDrop(list, num);
			bool shouldCheckJam = hasAmmoInMag || !doesWeaponHaveBoltCatch || !isMagazineInserted;
			List<_E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>> list2 = this.m__E004;
			GetSpecificMalfunctionVariants(list2, ammoToFire, malfunctionSource, weaponDurability, hasAmmoInMag, isMagazineInserted, shouldCheckJam);
			if (list2.Count == 0)
			{
				return Weapon.EMalfunctionState.None;
			}
			return _E375<Weapon.EMalfunctionState>.GenerateDrop(list2);
		}

		protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
		{
			return new Dictionary<Type, OperationFactoryDelegate>
			{
				{
					typeof(_E030),
					() => new _E030(this)
				},
				{
					typeof(_E02B),
					() => new _E02B(this)
				},
				{
					typeof(_E011),
					() => new _E011(this)
				},
				{
					typeof(_E020),
					() => new _E020(this)
				},
				{
					typeof(_E022),
					() => Item.MustBoltBeOpennedForInternalReload ? ((_E022)new _E024(this)) : ((_E022)new _E023(this))
				},
				{
					typeof(_E01F),
					() => new _E01F(this)
				},
				{
					typeof(_E028),
					() => new _E028(this)
				},
				{
					typeof(_E025),
					() => new _E025(this)
				},
				{
					typeof(_E01D),
					() => new _E01D(this)
				},
				{
					typeof(_E012),
					() => new _E012(this)
				},
				{
					typeof(_E01A),
					() => new _E01A(this)
				},
				{
					typeof(_E019),
					() => new _E019(this)
				},
				{
					typeof(_E005),
					() => new _E005(this)
				},
				{
					typeof(_E02A),
					() => new _E02A(this)
				},
				{
					typeof(_E008),
					() => new _E008(this)
				},
				{
					typeof(_E001),
					() => new _E001(this)
				},
				{
					typeof(_E000),
					() => new _E000(this)
				},
				{
					typeof(_E00C),
					() => (!Item.IsFlareGun) ? ((!Item.IsOneOff) ? ((Item.ReloadMode != Weapon.EReloadMode.OnlyBarrel) ? ((!(Item is _EAD1)) ? ((!Item.BoltAction) ? new _E00C(this) : new _E004(this)) : new _E00A(this)) : new _E00B(this)) : new _E01C(this)) : new _E00F(this)
				},
				{
					typeof(_E002),
					() => new _E002(this)
				},
				{
					typeof(_E031),
					() => new _E031(this)
				},
				{
					typeof(_E02D),
					() => new _E02D(this)
				},
				{
					typeof(_E01E),
					() => new _E01E(this)
				},
				{
					typeof(_E02E),
					() => new _E02E(this)
				},
				{
					typeof(_E032),
					() => new _E032(this)
				},
				{
					typeof(_E015),
					() => new _E015(this)
				},
				{
					typeof(_E014),
					() => new _E014(this)
				},
				{
					typeof(_E016),
					() => new _E016(this)
				},
				{
					typeof(_E007),
					() => (!Item.IsMultiBarrel) ? new _E007(this) : new _E006(this)
				},
				{
					typeof(_E018),
					() => new _E018(this)
				},
				{
					typeof(_E034),
					() => new _E034(this)
				},
				{
					typeof(_E017),
					() => new _E017(this)
				},
				{
					typeof(_E010),
					() => new _E010(this)
				},
				{
					typeof(_E009),
					() => new _E009(this)
				}
			};
		}

		internal static _E077 _E000<_E077>(Player player, Weapon weapon) where _E077 : FirearmController
		{
			_E077 val = ItemHandsController._E000<_E077>(player, weapon);
			_E002(val, player, weapon, async: false).HandleExceptions();
			return val;
		}

		internal static async Task<_E077> _E001<_E077>(Player player, Weapon weapon) where _E077 : FirearmController
		{
			_E077 val = await ItemHandsController._E002<_E077>(player, weapon);
			await _E002(val, player, weapon);
			return val;
		}

		private static async Task _E002<_E077>(_E077 controller, Player player, Weapon weapon, bool async = true) where _E077 : FirearmController
		{
			WeaponPrefab component = controller.ControllerGameObject.GetComponent<WeaponPrefab>();
			AssetPoolObject component2 = controller._controllerObject.GetComponent<AssetPoolObject>();
			component.ResetStatesToDefault();
			((FirearmController)controller)._E00B = () => player.AIData != null && player.AIData.IsNoOffsetShooting;
			((FirearmController)controller)._E00C = component.ObjectInHands as _E6F9;
			controller.Fireport = player.PlayerBones.FindFireport();
			((FirearmController)controller).m__E01A = controller._E00F;
			controller.MultiBarrelsFireports = player.PlayerBones.FindMultiBarrelsFireports(controller.Item.IsMultiBarrel);
			controller.GunBaseTransform = controller.HandsHierarchy.GetTransform(ECharacterWeaponBones.Weapon_root_anim);
			((FirearmController)controller).m__E00E = player.MalfRandoms;
			controller.CCV = component2.ContainerCollectionView;
			((FirearmController)controller).m__E013 = player.Skills.GetWeaponInfo(controller.Item);
			((FirearmController)controller).m__E018 = new _E393<float>(controller._E00D);
			((FirearmController)controller).m__E019 = new _E393<float>(controller._E00C);
			((FirearmController)controller).m__E00D = component;
			((FirearmController)controller)._E00C.Player = player;
			if (async)
			{
				await JobScheduler.Yield();
			}
			((FirearmController)controller)._E00C.AfterGetFromPoolInit(player.ProceduralWeaponAnimation, controller.CCV, player.IsYourPlayer);
			if (async)
			{
				await JobScheduler.Yield();
			}
			player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			player.ProceduralWeaponAnimation._E007(controller, controller.Item, component, ((FirearmController)controller).m__E013);
			ProceduralWeaponAnimation proceduralWeaponAnimation = player.ProceduralWeaponAnimation;
			proceduralWeaponAnimation.AvailableScopesChanged = (Action)Delegate.Combine(proceduralWeaponAnimation.AvailableScopesChanged, new Action(controller.ValidateCurrentScopeIndex));
			controller.CompositeDisposable.AddDisposable(delegate
			{
				ProceduralWeaponAnimation proceduralWeaponAnimation2 = player.ProceduralWeaponAnimation;
				proceduralWeaponAnimation2.AvailableScopesChanged = (Action)Delegate.Remove(proceduralWeaponAnimation2.AvailableScopesChanged, new Action(controller.ValidateCurrentScopeIndex));
			});
			if (async)
			{
				await JobScheduler.Yield();
			}
			player.ProceduralWeaponAnimation.InitTransforms(controller.HandsHierarchy, controller.CCV);
			if (async)
			{
				await JobScheduler.Yield();
			}
			((FirearmController)controller)._E00C.SetFireport(controller.CurrentFireport);
			if (async)
			{
				await JobScheduler.Yield();
			}
			((FirearmController)controller)._E00C.SetPointOfView(player.PointOfView);
			((FirearmController)controller)._E001 = component.FirearmsAnimator;
			((FirearmController)controller)._E001.AddEventsConsumer(controller);
			controller.CompositeDisposable.AddDisposable(delegate
			{
				((FirearmController)controller)._E001.RemoveEventsConsumer(controller);
			});
			((FirearmController)controller)._E001.SetPatronInWeaponVisibleEvent += delegate(bool visible)
			{
				((FirearmController)controller)._E00C.SetVisiblePatronInWeapon(visible);
			};
			controller.CompositeDisposable.AddDisposable(delegate
			{
				((FirearmController)controller)._E001.SetPatronInWeaponVisibleEvent -= delegate(bool visible)
				{
					((FirearmController)controller)._E00C.SetVisiblePatronInWeapon(visible);
				};
			});
			controller._player.Skills.WeaponMastered += controller.OnCurrentWeaponBeingMastered;
			controller._player.Skills.OnSkillLevelChanged += controller._E003;
			controller._player.MovementContext.PhysicalConditionChanged += controller._E004;
			controller.CompositeDisposable.AddDisposable(delegate
			{
				controller._player.Skills.WeaponMastered -= controller.OnCurrentWeaponBeingMastered;
				controller._player.Skills.OnSkillLevelChanged -= controller._E003;
				controller._player.MovementContext.PhysicalConditionChanged -= controller._E004;
			});
			((FirearmController)controller).m__E011 = weapon.GetTotalCenterOfImpact(includeAmmo: false);
			controller.AimingDevices = (from slot in weapon.AllSlots
				where slot.ContainedItem is _EAA8
				select slot into x
				select x.ContainedItem as _EAA8).ToArray();
			controller.UpdateHipInaccuracy();
			((FirearmController)controller).m__E012 = weapon.TotalShotgunDispersion;
			controller._E012();
			if (async)
			{
				await JobScheduler.Yield();
			}
			controller.SyncWithCharacterSkills();
			controller.InitBallisticCalculator();
			if (async)
			{
				await JobScheduler.Yield();
			}
			controller._player.HandsAnimator = ((FirearmController)controller)._E001;
			if (weapon.MalfState.State != 0)
			{
				component.InitMalfunctionState(weapon, hasPlayer: true, weapon.MalfState.IsKnownMalfunction(player.ProfileId), out var ammoPoolObject);
				((FirearmController)controller)._E00C.SetPatronInShellPort(ammoPoolObject);
			}
			controller._E006();
			((FirearmController)controller)._E001.SetBoltCatch(active: false);
			((FirearmController)controller)._E001.SkipTime(Time.fixedDeltaTime);
			controller._E00A();
			controller._E00E();
			controller._E005();
			for (int i = 0; i < weapon.Chambers.Length; i++)
			{
				Slot slot2 = weapon.Chambers[i];
				if (slot2.ContainedItem != null)
				{
					((FirearmController)controller)._E00C.SetRoundIntoWeapon((_EA12)slot2.ContainedItem, i);
				}
			}
			if (weapon.GetCurrentMagazine() is _EB13 obj)
			{
				for (int j = 0; j < obj.Camoras.Length; j++)
				{
					Slot slot3 = obj.Camoras[j];
					if (slot3?.ContainedItem != null)
					{
						((FirearmController)controller)._E00C.SetRoundIntoWeapon((_EA12)slot3.ContainedItem, j);
					}
				}
			}
			if (weapon.ShellsInChambers != null)
			{
				for (int k = 0; k < weapon.ShellsInChambers.Length; k++)
				{
					AmmoTemplate ammoTemplate = weapon.ShellsInChambers[k];
					if (ammoTemplate != null)
					{
						((FirearmController)controller)._E00C.SetPatronInShellPort(Singleton<_E760>.Instance.CreateFromPool<AmmoPoolObject>(ammoTemplate.Prefab), k);
						((FirearmController)controller)._E001.SetShellsInWeapon(weapon.ShellsInWeaponCount);
					}
				}
			}
			if (async)
			{
				await JobScheduler.Yield();
			}
		}

		public void ValidateCurrentScopeIndex()
		{
			if (Item.AimIndex.Value >= _player.ProceduralWeaponAnimation.ScopeAimTransforms.Count)
			{
				ChangeAimingMode();
			}
		}

		private void _E003(_E74E obj)
		{
			if (obj.Id == ESkillId.BotReload || obj is _E750 || ((_E751)obj).Class == ESkillClass.Combat)
			{
				SyncWithCharacterSkills();
			}
		}

		private void _E004(EPhysicalCondition condition, EPhysicalCondition full)
		{
			if (condition == EPhysicalCondition.LeftArmDamaged || condition == EPhysicalCondition.RightArmDamaged)
			{
				SetAnimatorAndProceduralValues();
				RecalculateErgonomic();
				_player.ProceduralWeaponAnimation.UpdateWeaponVariables();
			}
		}

		public void OnCurrentWeaponBeingMastered(_E750 m)
		{
			if (m.MasteringGroup.Templates.Contains(Item.TemplateId))
			{
				SyncWithCharacterSkills();
			}
		}

		public void SetAnimatorAndProceduralValues(bool checkMalf = true)
		{
			float num = _E62A.PastTime - _player.QuickdrawTime;
			bool flag = _player.Inventory.Equipment.GetSlot(EquipmentSlot.Holster).Equals(Item.Parent.Container);
			if (checkMalf && _player.QuickdrawWeaponFast && num < 1f && flag && !Item.MalfState.IsKnownMalfunction(_player.ProfileId))
			{
				float outToIdleSpeedMultForPistol = Singleton<_E5CB>.Instance.Malfunction.OutToIdleSpeedMultForPistol;
				this._E001.SetSpeedParameters(1f, outToIdleSpeedMultForPistol);
				_player.MovementContext.PlayerAnimator._E000(1f, outToIdleSpeedMultForPistol);
				_player.QuickdrawWeaponFast = false;
				return;
			}
			_player.QuickdrawWeaponFast = false;
			if (_player.MovementContext.PhysicalConditionIs(EPhysicalCondition.LeftArmDamaged) || _player.MovementContext.PhysicalConditionIs(EPhysicalCondition.RightArmDamaged))
			{
				this._E001.SetSpeedParameters();
				_player.MovementContext.PlayerAnimator._E000();
			}
			else
			{
				this._E001.SetSpeedParameters(this.m__E013.ReloadSpeed, this.m__E013.SwapSpeed);
				_player.MovementContext.PlayerAnimator._E000(this.m__E013.ReloadSpeed, this.m__E013.SwapSpeed);
			}
		}

		public void SyncWithCharacterSkills()
		{
			this._E001.SetWeaponLevel(CurrentMasteringLevel);
			_E74F._E000 weaponInfo = _player.Skills.GetWeaponInfo(Item);
			this.m__E013.AimMovementSpeed = weaponInfo.AimMovementSpeed;
			this.m__E013.SwapSpeed = weaponInfo.SwapSpeed;
			this.m__E013.DeltaErgonomics = weaponInfo.DeltaErgonomics;
			this.m__E013.FixSpeed = weaponInfo.FixSpeed;
			this.m__E013.RecoilSupression = weaponInfo.RecoilSupression;
			this.m__E013.ReloadSpeed = weaponInfo.ReloadSpeed;
			this.m__E013.Maxed = weaponInfo.Maxed;
			this.m__E013.DoubleActionRecoilReduce = weaponInfo.DoubleActionRecoilReduce;
			SetAnimatorAndProceduralValues();
		}

		private void _E005()
		{
			this.m__E01D = _controllerObject.GetComponent<WeaponSoundPlayer>();
			this.m__E01D.Init(this, CurrentFireport, _player);
			this.m__E01D.IsSilenced = IsSilenced;
		}

		public void UpdateSensitivity()
		{
			if (_isAiming)
			{
				float num = _player.ProceduralWeaponAnimation.CurrentAimingMod?.GetCurrentSensitivity ?? Item.Template.AimSensitivity;
				float num2 = _player.GetAimingSensitivity();
				_aimingSens = num * num2;
			}
		}

		private void _E006()
		{
			_EA62 underbarrelWeapon = Item.GetUnderbarrelWeapon();
			if (underbarrelWeapon != null)
			{
				UnderbarrelWeapon = underbarrelWeapon;
				this.m__E01F = new _E033();
				this.m__E01F.Init(_player, this, UnderbarrelWeapon);
			}
			else
			{
				UnderbarrelWeapon = null;
				this.m__E01F = null;
			}
		}

		private void _E007(_EA62 underbarrelWeapon, GameObject _weaponPrefab)
		{
			if (underbarrelWeapon != null)
			{
				UnderbarrelWeapon = underbarrelWeapon;
				this.m__E01F = new _E033();
				this.m__E01F.Init(_player, this, UnderbarrelWeapon, _weaponPrefab);
			}
			else
			{
				UnderbarrelWeapon = null;
				this.m__E01F = null;
			}
		}

		private void _E008()
		{
			ClearPreWarmOperationsDict();
			UnderbarrelWeapon = null;
			this.m__E01F?.Clear();
			this.m__E01F = null;
		}

		private void _E009()
		{
			this.m__E01F?.InitWeaponSoundPlayer();
		}

		private void _E00A()
		{
			this.m__E010 = 0f;
			WeaponLn = 0f;
			if (_player.MovementContext.StationaryWeapon != null && _player.MovementContext.StationaryWeapon.Item == Item)
			{
				return;
			}
			if (base.WeaponRoot == null || CurrentFireport.Original == null)
			{
				UnityEngine.Debug.LogError(_ED3E._E000(163700));
				return;
			}
			WeaponLn = Vector3.Distance(GunBaseTransform.position, CurrentFireport.Original.position);
			GameObject[] array = this._E00C.MuzzleTransforms();
			foreach (GameObject gameObject in array)
			{
				WeaponLn = Mathf.Max(WeaponLn, Vector3.Distance(GunBaseTransform.position, gameObject.transform.position));
			}
			if (CurrentFireport.Original.lossyScale.y < 1f)
			{
				WeaponLn /= CurrentFireport.Original.lossyScale.y;
			}
			this.m__E017 = LayerMask.NameToLayer(_ED3E._E000(60679));
		}

		private float _E00B(Vector3 origin, float ln, ref bool overlapsWithPlayer, Vector3? weaponUp = null)
		{
			Vector3 vector = weaponUp ?? base.WeaponRoot.up;
			Vector3 end = origin - vector * ln;
			if (_E320.Linecast(origin, end, out var bestHit, EFTHardSettings.Instance.WEAPON_OCCLUSION_LAYERS, reverseCheck: false, FirearmController.m__E00A, this.m__E01A))
			{
				overlapsWithPlayer = bestHit.collider.gameObject.layer == this.m__E017;
				return ln - bestHit.distance;
			}
			Vector3 lhs = origin - _player.Position;
			Vector3 up = Vector3.up;
			float num = Vector3.Dot(lhs, up);
			if (_E320.Linecast(_player.Position + num * up, origin, out bestHit, EFTHardSettings.Instance.WEAPON_OCCLUSION_LAYERS, reverseCheck: false, FirearmController.m__E00A, this.m__E01A))
			{
				overlapsWithPlayer = bestHit.collider.gameObject.layer == this.m__E017;
				return ln;
			}
			return 0f;
		}

		private float _E00C()
		{
			if (_player.MovementContext.PhysicalConditionContainsAny(EPhysicalCondition.LeftArmDamaged | EPhysicalCondition.RightArmDamaged))
			{
				return 0f;
			}
			return Mathf.Max(0f, Item.ErgonomicsTotal * (1f + this.m__E013.DeltaErgonomics + _player.ErgonomicsPenalty));
		}

		private float _E00D()
		{
			return Item.GetSingleItemTotalWeight() * (1f - Mathf.Sqrt(TotalErgonomics) / 25f);
		}

		public void ReloadMagNotFound()
		{
			_player._E0AF?.Invoke(EPhraseTrigger.NeedAmmo, 5);
		}

		private void _E00E()
		{
			IsSilenced = Item.GetItemComponentsInChildren<SilencerComponent>().Any();
		}

		public void UpdateHipInaccuracy()
		{
			bool flag = AimingDevices.Length != 0 && AimingDevices.Any((_EAA8 x) => x.Light != null && x.Light.IsActive);
			HipInaccuracy = (flag ? (0.3f - this.m__E019.Value / 400f) : (1f - Mathf.Clamp01(this.m__E019.Value / 250f - 0.15f)));
			_player.ProceduralWeaponAnimation.Breath.HipPenalty = HipInaccuracy;
		}

		public void SetUnderbarrelWeapon()
		{
			_E006();
		}

		public void WeaponModified()
		{
			_E00A();
			this.m__E011 = Item.GetTotalCenterOfImpact(includeAmmo: false);
			_E00E();
			RecalculateErgonomic();
			this.m__E00D.InitHotObjects(Item);
			_controllerObject.GetComponent<WeaponSoundPlayer>().IsSilenced = IsSilenced;
			_player.ProceduralWeaponAnimation.UpdateWeaponVariables();
			AimingDevices = (from slot in Item.AllSlots
				where slot.ContainedItem is _EAA8
				select slot into x
				select x.ContainedItem as _EAA8).ToArray();
			UpdateHipInaccuracy();
			this.m__E012 = Item.TotalShotgunDispersion;
		}

		public void RecalculateErgonomic()
		{
			this.m__E019.SetDirty();
			this.m__E018.SetDirty();
		}

		public override float GetAnimatorFloatParam(int hash)
		{
			return this._E001.GetAnimatorParameter(hash);
		}

		private bool _E00F(RaycastHit overlapHit)
		{
			GameObject gameObject = overlapHit.collider.gameObject;
			if (gameObject.layer == this.m__E017)
			{
				return gameObject == _player.gameObject;
			}
			return false;
		}

		public virtual void WeaponOverlapping()
		{
			if (WeaponLn <= 0f)
			{
				return;
			}
			try
			{
				float num = 1f;
				if (!_player.IsVisible || CurrentOperation is _E02B)
				{
					return;
				}
				float weaponOverlapDistanceCulling = Singleton<_E5B7>.Instance.WeaponOverlapDistanceCulling;
				if (!(weaponOverlapDistanceCulling > 0f) || _player.IsVisibleByCullingObject(weaponOverlapDistanceCulling))
				{
					Vector3 position = _player.ProceduralWeaponAnimation.HandsContainer.HandsPosition.Get();
					if (_player.ProceduralWeaponAnimation.BlindfireBlender.Value != 0f)
					{
						Vector3 position2 = (_player.ProceduralWeaponAnimation.BlindFireEndPosition + _player.ProceduralWeaponAnimation.PositionZeroSum) * 1.9f;
						position2 = _player.ProceduralWeaponAnimation.HandsContainer.WeaponRootAnim.parent.TransformPoint(position2);
						num = _E00B(position2, WeaponLn, ref _player.ProceduralWeaponAnimation.TurnAway.OverlapsWithPlayer);
					}
					if (num >= 0.02f)
					{
						position = _player.ProceduralWeaponAnimation.HandsContainer.WeaponRootAnim.parent.TransformPoint(position);
						num = _E00B(position, WeaponLn, ref _player.ProceduralWeaponAnimation.TurnAway.OverlapsWithPlayer);
					}
					SetWeaponOverlapValue(num);
					WeaponOverlapView();
				}
			}
			finally
			{
			}
		}

		private void _E010(bool value)
		{
			if (IsAiming && value)
			{
				_E01E();
			}
			if (FirearmsAnimator != null)
			{
				FirearmsAnimator.SetSprint(value);
			}
		}

		private void _E011(EPlayerState previousstate, EPlayerState nextstate)
		{
			if (!EFTHardSettings.Instance.CanAimInState(nextstate))
			{
				_E01E();
			}
		}

		public bool AudioDelegate()
		{
			if (IsTriggerPressed)
			{
				return !Malfunction;
			}
			return false;
		}

		protected virtual void InitBallisticCalculator()
		{
			BallisticsCalculator = Singleton<_E5CE>.Instance.CreateBallisticCalculator(1);
		}

		public virtual void SetWeaponOverlapValue(float overlap)
		{
			this.m__E010 = overlap;
		}

		public void WeaponOverlapView()
		{
			Vector3 vector = _player.ProceduralWeaponAnimation.HandsContainer.HandsPosition.Get();
			if (this.m__E010 < 0.02f)
			{
				_player.ProceduralWeaponAnimation.TurnAway.OverlapDepth = this.m__E010;
				_player.ProceduralWeaponAnimation.OverlappingAllowsBlindfire = true;
			}
			else
			{
				_player.ProceduralWeaponAnimation.OverlappingAllowsBlindfire = false;
				_player.ProceduralWeaponAnimation.TurnAway.OriginZShift = vector.y;
				_player.ProceduralWeaponAnimation.TurnAway.OverlapDepth = this.m__E010;
			}
			if (this.m__E010 > EFTHardSettings.Instance.STOP_AIMING_AT && IsAiming)
			{
				ToggleAim();
				AimingInterruptedByOverlap = true;
			}
			else if (this.m__E010 < EFTHardSettings.Instance.STOP_AIMING_AT && _player.ProceduralWeaponAnimation.TurnAway.OverlapValue < 0.2f && AimingInterruptedByOverlap && !IsAiming)
			{
				ToggleAim();
				AimingInterruptedByOverlap = false;
			}
		}

		private void _E012()
		{
			if (Item.WeapFireType.Length > 1)
			{
				this._E001.SetFireMode(Item.SelectedFireMode, skipAnimation: true);
			}
			if (Item.HasChambers)
			{
				this._E001.SetAmmoInChamber(Item.ChamberAmmoCount);
			}
			_EA6A currentMagazine = Item.GetCurrentMagazine();
			this._E001.SetMagInWeapon(currentMagazine != null);
			this._E001.SetAmmoOnMag(currentMagazine?.Count ?? 0);
			this._E001.SetMagTypeCurrent(currentMagazine?.magAnimationIndex ?? (-1));
			this._E001.Fold(Item.Folded);
			if (UnderbarrelWeapon != null)
			{
				this._E001.SetLauncher(isLauncherEnabled: false);
			}
		}

		protected override void IEventsConsumerOnWeapIn()
		{
			_E020();
		}

		protected override void IEventsConsumerOnWeapOut()
		{
			_E01F();
		}

		protected override void IEventsConsumerOnThirdAction(int intParam)
		{
			TranslateAnimatorParameter(intParam);
		}

		protected override void IEventsConsumerOnAddAmmoInChamber()
		{
			_E022();
		}

		protected override void IEventsConsumerOnRemoveShell()
		{
			_E023();
		}

		protected override void IEventsConsumerOnShellEject()
		{
			_E024();
		}

		protected override void IEventsConsumerOnAddAmmoInMag()
		{
			_E026();
		}

		protected override void IEventsConsumerOnDelAmmoFromMag()
		{
			_E025();
		}

		protected override void IEventsConsumerOnShowAmmo(bool boolParam)
		{
			_E027(boolParam);
		}

		protected override void IEventsConsumerOnDelAmmoChamber()
		{
			_E021();
		}

		protected override void IEventsConsumerOnMagIn()
		{
			_E032();
		}

		protected override void IEventsConsumerOnMagOut()
		{
			_E02F();
		}

		protected override void IEventsConsumerOnMagShow()
		{
			_E031();
		}

		protected override void IEventsConsumerOnMagHide()
		{
			_E030();
		}

		protected override void IEventsConsumerOnOffBoltCatch()
		{
			_E028(isCatched: false);
		}

		protected override void IEventsConsumerOnOnBoltCatch()
		{
			_E028(isCatched: true);
		}

		protected override void IEventsConsumerOnMalfunctionOff()
		{
			_E029();
		}

		protected override void IEventsConsumerOnFiringBullet()
		{
			_E02A();
		}

		protected override void IEventsConsumerOnFireEnd()
		{
			_E02B();
		}

		protected override void IEventsConsumerOnIdleStart()
		{
			_E02D();
		}

		protected override void IEventsConsumerOnUseSecondMagForReload()
		{
			_E018();
		}

		protected override void IEventsConsumerOnReplaceSecondMag()
		{
			_E019();
		}

		protected override void IEventsConsumerOnPutMagToRig()
		{
			_E01A();
		}

		protected override void IEventsConsumerOnModChanged()
		{
			_E02E();
		}

		protected override void IEventsConsumerOnLauncherAppeared()
		{
			_E017();
		}

		protected override void IEventsConsumerOnLauncherDisappeared()
		{
			_E016();
		}

		protected override void IEventsConsumerOnArm()
		{
			_E033(armed: true);
		}

		protected override void IEventsConsumerOnDisarm()
		{
			_E033(armed: false);
		}

		protected override void IEventsConsumerOnFoldOn()
		{
			_E013(b: true);
		}

		protected override void IEventsConsumerOnFoldOff()
		{
			_E013(b: false);
		}

		protected override void IEventsOnBackpackDrop()
		{
			_E02C();
		}

		protected override void IEventsConsumerOnStartUtilityOperation()
		{
			CurrentOperation?.OnUtilityOperationStartEvent();
		}

		protected override void IEventsConsumerOnOnUseProp(bool boolParam)
		{
			SetPropVisibility(boolParam);
		}

		private void _E013(bool b)
		{
			SetCompassState(active: false);
			CurrentOperation?.OnFold(b);
		}

		public override bool CanExecute(_EB72 operation)
		{
			if (_E014(operation))
			{
				return true;
			}
			if ((CurrentOperation is _E011 || CurrentOperation is _E015) && !(CurrentOperation is _E034))
			{
				return !(CurrentOperation is _E017);
			}
			return false;
		}

		private bool _E014(_EB72 operation)
		{
			if (!(operation is _EB75 obj))
			{
				return true;
			}
			if (!_E015(operation) && (obj.From1 == null || !_player._inventoryController.IsInAnimatedSlot(obj.Item1)))
			{
				return true;
			}
			return false;
		}

		public override void Execute(_EB72 operation, Callback callback)
		{
			CurrentOperation.Execute(operation, callback);
		}

		private bool _E015(_EB72 operation)
		{
			if (!(operation is _EB75 obj))
			{
				return false;
			}
			if (obj.Item1.IsChildOf(Item))
			{
				return true;
			}
			if ((obj.From1 != null && obj.From1.IsChildOf(Item, notMergedWithThisItem: false)) || (obj.From1 == null && obj.Item1 == Item))
			{
				return true;
			}
			if ((obj.To1 != null && obj.To1.IsChildOf(Item, notMergedWithThisItem: false)) || (obj.From1 == null && obj.Item1 == Item))
			{
				return true;
			}
			if (!(operation is _EB76 obj2))
			{
				return false;
			}
			if (obj2.Item2 != null && obj2.Item2.IsChildOf(Item))
			{
				return true;
			}
			if (obj2.From2 != null && obj2.From2.IsChildOf(Item, notMergedWithThisItem: false))
			{
				return true;
			}
			if (obj2.To2 != null && obj2.To2.IsChildOf(Item, notMergedWithThisItem: false))
			{
				return true;
			}
			return false;
		}

		private void _E016()
		{
			CurrentOperation.LauncherDisappeared();
		}

		private void _E017()
		{
			CurrentOperation.LauncherAppeared();
		}

		private void _E018()
		{
			CurrentOperation.UseSecondMagForReload();
		}

		private void _E019()
		{
			CurrentOperation.ReplaceSecondMag();
		}

		private void _E01A()
		{
			CurrentOperation.PutMagToRig();
		}

		private void _E01B()
		{
			CurrentOperation.OnJumpOrFall();
		}

		private void _E01C()
		{
			CurrentOperation.OnSprintFinished();
		}

		private void _E01D()
		{
			CurrentOperation.OnSprintStart();
		}

		private void _E01E()
		{
			CurrentOperation.OnAimingDisabled();
		}

		private void _E01F()
		{
			CurrentOperation.HideWeaponComplete();
		}

		private void _E020()
		{
			CurrentOperation.WeaponAppeared();
		}

		private void _E021()
		{
			CurrentOperation.RemoveAmmoFromChamber();
		}

		private void _E022()
		{
			CurrentOperation.OnAddAmmoInChamber();
		}

		private void _E023()
		{
			CurrentOperation.OnRemoveShellEvent();
		}

		private void _E024()
		{
			CurrentOperation.OnShellEjectEvent();
		}

		private void _E025()
		{
			UnityEngine.Debug.LogError(_ED3E._E000(163723));
		}

		private void _E026()
		{
			CurrentOperation.AddAmmoToMag();
		}

		private void _E027(bool b)
		{
			CurrentOperation.OnShowAmmo(b);
		}

		private void _E028(bool isCatched)
		{
			CurrentOperation.OnOnOffBoltCatchEvent(isCatched);
		}

		private void _E029()
		{
			CurrentOperation.OnMalfunctionOffEvent();
		}

		private void _E02A()
		{
			CurrentOperation.OnFireEvent();
		}

		private void _E02B()
		{
			CurrentOperation.OnFireEndEvent();
		}

		private void _E02C()
		{
			CurrentOperation.OnBackpackDropEvent();
		}

		private void _E02D()
		{
			CurrentOperation.OnIdleStartEvent();
		}

		private void _E02E()
		{
			CurrentOperation.OnModChanged();
		}

		private void _E02F()
		{
			CurrentOperation.OnMagPulledOutFromWeapon();
		}

		private void _E030()
		{
			CurrentOperation.OnMagPuttedToRig();
		}

		private void _E031()
		{
			CurrentOperation.OnMagAppeared();
		}

		private void _E032()
		{
			CurrentOperation.OnMagInsertedToWeapon();
		}

		private void _E033(bool armed)
		{
			this._E001.SetHammerArmed(armed);
			Item.Armed = armed;
		}

		public override void Spawn(float animationSpeed, Action callback)
		{
			this._E001.SetAnimationSpeed(animationSpeed);
			InitiateOperation<_E030>().Start(delegate
			{
				callback();
				_player.MovementContext.OnStateChanged += _E011;
				_player.Physical.OnSprintStateChangedEvent += _E010;
			});
		}

		public override void Drop(float animationSpeed, Action callback, bool fastDrop, Item nextControllerItem = null)
		{
			if (base.Destroyed)
			{
				CurrentOperation.HideWeapon(callback, fastDrop, nextControllerItem);
				return;
			}
			base.Destroyed = true;
			_player.MovementContext.OnStateChanged -= _E011;
			_player.Physical.OnSprintStateChangedEvent -= _E010;
			RemoveBallisticCalculator();
			Player._E013 inventoryOperation = _player._E03B(Item);
			this._E001.SetAnimationSpeed(animationSpeed);
			Action onHidden = delegate
			{
				inventoryOperation.Confirm();
				callback();
			};
			CurrentOperation.HideWeapon(onHidden, fastDrop, nextControllerItem);
		}

		protected virtual void RemoveBallisticCalculator()
		{
			Singleton<_E5CE>.Instance.RemoveBallisticCalculator(Item);
		}

		public override void Destroy()
		{
			this.m__E008?.Invoke();
			this.m__E008 = null;
			this.m__E00D = null;
			CCV?.Dispose();
			CCV = null;
			_player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			this.m__E01F?.Clear();
			base.Destroy();
			this._E001 = null;
			BallisticsCalculator = null;
			AssetPoolObject.ReturnToPool(_controllerObject.gameObject);
		}

		public override bool SupportPickup()
		{
			return true;
		}

		public override void Pickup(bool p)
		{
			CurrentOperation.Pickup(p);
		}

		public override void Interact(bool isInteracting, int actionIndex)
		{
			CurrentOperation.Interact(isInteracting, actionIndex);
		}

		public override bool CanInteract()
		{
			if (!(CurrentOperation is _E011) && !(CurrentOperation is _E015))
			{
				return CurrentOperation is _E00D;
			}
			return true;
		}

		public override bool InCanNotBeInterruptedOperation()
		{
			return CurrentOperation.CanNotBeInterrupted();
		}

		public override void Loot(bool p)
		{
			CurrentOperation.Loot(p);
		}

		public override bool IsInInteraction()
		{
			return this._E001.IsInInteraction;
		}

		public override bool IsInInteractionStrictCheck()
		{
			if (!IsInInteraction() && !(this._E001.GetLayerWeight(this._E001.LACTIONS_LAYER_INDEX) >= float.Epsilon))
			{
				return this._E001.Animator.IsInTransition(this._E001.LACTIONS_LAYER_INDEX);
			}
			return true;
		}

		public virtual void UnderbarrelSightingRangeDown()
		{
			CurrentOperation.UnderbarrelSightingRangeDown();
		}

		public virtual void UnderbarrelSightingRangeUp()
		{
			CurrentOperation.UnderbarrelSightingRangeUp();
		}

		public virtual bool IsInLauncherMode()
		{
			if (!(CurrentOperation is _E015) && !(CurrentOperation is _E014))
			{
				return CurrentOperation is _E016;
			}
			return true;
		}

		public virtual bool ToggleLauncher()
		{
			return CurrentOperation.ToggleLauncher();
		}

		public virtual void SetTriggerPressed(bool pressed)
		{
			CurrentOperation.SetTriggerPressed(pressed);
		}

		public virtual bool CanPressTrigger()
		{
			return true;
		}

		public virtual void ToggleAim()
		{
			SetCompassState(active: false);
			SetAim(!IsAiming);
		}

		public virtual void SetAim(int scopeIndex)
		{
			Item.AimIndex.Value = Mathf.Max(0, scopeIndex);
			SetAim(scopeIndex >= 0);
		}

		public virtual void SetAim(bool value)
		{
			if (Item.IsOneOff)
			{
				value = false;
			}
			AimingInterruptedByOverlap = false;
			bool isAiming = IsAiming;
			CurrentOperation.SetAiming(value);
			_player.ProceduralWeaponAnimation.CheckShouldMoveWeaponCloser();
			_player._E0DB &= !value;
			if (isAiming != IsAiming)
			{
				float num = TotalErgonomics / 100f - 1f;
				float volume = (1.5f * num * num + 0.25f) * (1f - (float)_player.Skills.DrawSound);
				_player._E028(volume);
			}
		}

		public override void SetInventoryOpened(bool opened)
		{
			CurrentOperation.SetInventoryOpened(opened);
			_player.CurrentState.OnInventory(opened);
			_player.InventoryOpenRaiseAction(opened);
		}

		public override bool IsInventoryOpen()
		{
			return InventoryOpened;
		}

		public virtual void ChangeAimingMode()
		{
			int num = Item.AimIndex.Value + 1;
			if (num >= this._E00C.ProceduralWeaponAnimation.ScopeAimTransforms.Count)
			{
				num = 0;
			}
			Item.AimIndex.Value = num;
			UpdateSensitivity();
			_player.RaiseSightChangedEvent(_player.ProceduralWeaponAnimation.CurrentAimingMod);
		}

		public virtual bool ChangeFireMode(Weapon.EFireMode fireMode)
		{
			return CurrentOperation.ChangeFireMode(fireMode);
		}

		public virtual bool CheckFireMode()
		{
			return CurrentOperation.CheckFireMode();
		}

		public virtual bool ExamineWeapon()
		{
			if ((!(CurrentOperation is _E011) && !(CurrentOperation is _E015)) || _player._E0DE.HasAnyHandsAction())
			{
				return false;
			}
			if (!CurrentOperation.ExamineWeapon())
			{
				return false;
			}
			if ((Item.MalfState.State == Weapon.EMalfunctionState.Jam || Item.MalfState.State == Weapon.EMalfunctionState.Feed) && !Item.MalfState.IsKnownMalfunction(_player.ProfileId))
			{
				this._E001.MisfireSlideUnknown(val: false);
				_player._E0DE.ExamineMalfunction(Item);
			}
			return true;
		}

		public virtual void RollCylinder(bool rollToZeroCamora)
		{
			CurrentOperation.RollCylinder(null, rollToZeroCamora);
		}

		public virtual bool CheckAmmo()
		{
			return CurrentOperation.CheckAmmo();
		}

		public virtual bool CheckChamber()
		{
			return CurrentOperation.CheckChamber();
		}

		public virtual void ReloadMag(_EA6A magazine, _EB22 gridItemAddress, Callback callback)
		{
			using (_ECC9.BeginSampleWithToken(_ED3E._E000(163755), _ED3E._E000(163780)))
			{
				if (CanStartReload())
				{
					CurrentOperation.ReloadMag(magazine, gridItemAddress, callback, null);
				}
				else
				{
					callback?.Fail(_ED3E._E000(163838));
				}
			}
		}

		public virtual void QuickReloadMag(_EA6A magazine, Callback callback)
		{
			using (_ECC9.BeginSampleWithToken(_ED3E._E000(163823), _ED3E._E000(161797)))
			{
				if (CanStartReload())
				{
					CurrentOperation.QuickReloadMag(magazine, callback, null);
				}
				else
				{
					callback?.Fail(_ED3E._E000(163838));
				}
			}
		}

		public virtual void ReloadGrenadeLauncher(_E9CF foundItem, Callback callback)
		{
			if (CanStartReload())
			{
				CurrentOperation.ReloadGrenadeLauncher(foundItem, callback);
			}
			else
			{
				callback?.Fail(_ED3E._E000(163838));
			}
		}

		public virtual void ReloadRevolverDrum(_E9CF ammoPack, Callback callback, bool quickReload = false)
		{
			if (Item.GetCurrentMagazine() != null)
			{
				if (CanStartReload())
				{
					CurrentOperation.ReloadRevolverDrum(ammoPack, callback, null, quickReload);
				}
				else
				{
					callback?.Fail(_ED3E._E000(163838));
				}
			}
		}

		public virtual void ReloadWithAmmo(_E9CF ammoPack, Callback callback)
		{
			if (Item.GetCurrentMagazine() == null)
			{
				return;
			}
			if (CanStartReload())
			{
				if (Item is _EAD1)
				{
					CurrentOperation.ReloadRevolverDrum(ammoPack, callback, null);
				}
				else
				{
					CurrentOperation.ReloadWithAmmo(ammoPack, callback, null);
				}
			}
			else
			{
				callback?.Fail(_ED3E._E000(163838));
			}
		}

		public virtual void ReloadBarrels(_E9CF ammoPack, _EB22 placeToPutContainedAmmoMagazine, Callback callback)
		{
			if (CanStartReload() && ammoPack.AmmoCount > 0)
			{
				CurrentOperation.ReloadBarrels(ammoPack, placeToPutContainedAmmoMagazine, callback, null);
			}
			else
			{
				callback?.Fail(_ED3E._E000(163838));
			}
		}

		public virtual bool CanStartReload()
		{
			_EA6A currentMagazine = Item.GetCurrentMagazine();
			if (currentMagazine != null && !_player._E0DE.Examined(currentMagazine))
			{
				_E857.DisplaySingletonWarningNotification(_ED3E._E000(161844).Localized());
				return false;
			}
			bool flag = (Item.MustBoltBeOpennedForExternalReload || Item.MustBoltBeOpennedForInternalReload) && Item.MalfState.IsAnyMalfExceptMisfire;
			if (Item.MalfState.State == Weapon.EMalfunctionState.Feed || flag)
			{
				_player.HandsController.FirearmsAnimator.MisfireSlideUnknown(val: false);
				_player._E0DE.ExamineMalfunction(Item);
				return false;
			}
			return CurrentOperation.CanStartReload();
		}

		public virtual bool ShouldForceQuickReload()
		{
			return _player.HealthController?.FindActiveEffect<_E9B3>(EBodyPart.Head) != null;
		}

		public override void ManualUpdate(float deltaTime)
		{
			base.ManualUpdate(deltaTime);
			this.m__E015 = true;
		}

		public virtual void SetLightsState(_E6C5[] lightsStates, bool force = false)
		{
			if (!force && !CurrentOperation.CanChangeLightState(lightsStates))
			{
				return;
			}
			Dictionary<string, LightComponent> dictionary = Item.AllSlots.Select((Slot x) => x.ContainedItem).GetComponents<LightComponent>().ToDictionary((LightComponent x) => x.Item.Id, (LightComponent x) => x);
			for (int i = 0; i < lightsStates.Length; i++)
			{
				_E6C5 obj = lightsStates[i];
				if (dictionary.ContainsKey(obj.Id))
				{
					LightComponent lightComponent = dictionary[obj.Id];
					lightComponent.IsActive = obj.IsActive;
					lightComponent.SelectedMode = obj.LightMode;
					if (lightComponent.IsActive && _player.AIData != null)
					{
						_player.AIData.TacticalModeChange(siActive: true);
					}
				}
				else
				{
					UnityEngine.Debug.LogErrorFormat(_ED3E._E000(161879), obj.Id);
				}
			}
			_player.AIData?.TacticalModeChange(dictionary.Any((KeyValuePair<string, LightComponent> x) => x.Value.IsActive));
			CurrentOperation.SetLightsState(lightsStates, force);
			if (_E2B6.Config.UseSpiritPlayer)
			{
				_player.Spirit.RecheckSwitch();
			}
			UpdateHipInaccuracy();
		}

		public override bool CanRemove()
		{
			return CurrentOperation.CanRemove();
		}

		public virtual void SetScopeMode(_E6C6[] scopeStates)
		{
			if (!CurrentOperation.CanChangeScopeStates(scopeStates))
			{
				return;
			}
			Dictionary<string, SightComponent> dictionary = Item.AllSlots.Select((Slot x) => x.ContainedItem).GetComponents<SightComponent>().ToDictionary((SightComponent x) => x.Item.Id, (SightComponent x) => x);
			for (int i = 0; i < scopeStates.Length; i++)
			{
				_E6C6 obj = scopeStates[i];
				if (dictionary.ContainsKey(obj.Id))
				{
					SightComponent sightComponent = dictionary[obj.Id];
					sightComponent.SetScopeMode(obj.ScopeIndexInsideSight, obj.ScopeMode);
					sightComponent.SetSelectedOpticCalibrationPoint(obj.ScopeIndexInsideSight, obj.ScopeCalibrationIndex);
				}
				else
				{
					UnityEngine.Debug.LogError(_ED3E._E000(161896) + obj.Id + _ED3E._E000(161894));
				}
			}
			_player.RaiseSightChangedEvent(_player.ProceduralWeaponAnimation.CurrentAimingMod);
			UpdateSensitivity();
			CurrentOperation.SetScopeMode(scopeStates);
		}

		public override bool IsHandsProcessing()
		{
			return this._E001.IsHandsProcessing();
		}

		public override void ShowGesture(EGesture gesture)
		{
			CurrentOperation.ShowGesture(gesture);
			if (Singleton<_E307>.Instantiated)
			{
				Singleton<_E307>.Instance.ShowGesture(_player, gesture);
			}
		}

		public override void BlindFire(int b)
		{
			CurrentOperation.BlindFire((!InventoryOpened) ? b : 0);
		}

		private float _E034(Weapon weapon)
		{
			return weapon.GetBarrelDeviation();
		}

		protected List<LightComponent> GetAllLightMods()
		{
			return Item.AllSlots.Select((Slot slot) => slot.ContainedItem).GetComponents<LightComponent>().ToList();
		}

		private void _E035(_EA12 flareItem)
		{
			Transform transform = _E38B.FindTransformRecursiveContains(base.WeaponRoot.transform, _ED3E._E000(64493));
			InitiateFlare(flareItem, transform.position, -transform.up);
		}

		protected void InitiateFlare(_EA12 flareItem, Vector3 shotPosition, Vector3 forward)
		{
			CreateFlareShot(flareItem, shotPosition, forward);
			_E038(this.m__E01D, flareItem, shotPosition, forward, multiShot: false);
			this._E00C.PlayShotEffects(_player.IsVisible, _player.SqrCameraDistance);
		}

		public virtual void AdjustShotVectors(ref Vector3 position, ref Vector3 direction)
		{
			position -= direction * WeaponLn / 5f;
			if (_player.ProceduralWeaponAnimation.ShotNeedsFovAdjustments && _player.RibcageScaleCurrent < 1f)
			{
				Transform self = HandsHierarchy.Self;
				Vector3 position2 = self.InverseTransformPoint(position);
				Vector3 direction2 = self.InverseTransformDirection(direction);
				position2.z *= _player.RibcageScaleCurrent;
				direction2.z *= _player.RibcageScaleCurrent;
				position = self.TransformPoint(position2);
				direction = self.TransformDirection(direction2).normalized;
			}
		}

		private void _E036(_EA62 launcher, _EA12 ammo)
		{
			Vector3 direction = this.m__E01F.Fireport.Original.TransformDirection(_player.LocalShotDirection);
			Vector3 position = this.m__E01F.Fireport.position;
			float ammoFactor = ammo.AmmoFactor;
			float num = 1f;
			AdjustShotVectors(ref position, ref direction);
			ammo.buckshotDispersion = launcher.TotalShotgunDispersion;
			float barrelDeviation = launcher.GetBarrelDeviation();
			Vector3 shotDirection = direction * 100f + launcher.CenterOfImpact * ammoFactor * num * barrelDeviation * UnityEngine.Random.insideUnitSphere;
			InitiateShot(launcher, ammo, position, shotDirection.normalized, position, 0, 0f);
			float num2 = 1f;
			this.m__E01C = num2 + (float)ammo.ammoRec / 100f;
			_E038(this.m__E01F.WeaponSoundPlayer, ammo, position, shotDirection, multiShot: false);
			if (ammo.AmmoTemplate.IsLightAndSoundShot)
			{
				_E039(position, direction);
				LightAndSoundShot(position, direction, ammo.AmmoTemplate);
			}
		}

		private void _E037(Weapon weapon, _EA12 ammo, int chamberIndex, bool multiShot = false)
		{
			Transform original = CurrentFireport.Original;
			Vector3 position = CurrentFireport.position;
			Vector3 direction = WeaponDirection;
			Vector3 position2 = position;
			float ammoFactor = ammo.AmmoFactor;
			float num = 1f;
			_E5CB instance = Singleton<_E5CB>.Instance;
			AdjustShotVectors(ref position2, ref direction);
			ammo.buckshotDispersion = this.m__E012;
			CurrentChamberIndex = chamberIndex;
			weapon.OnShot(ammo.DurabilityBurnModificator, ammo.HeatFactor, _player.Skills.WeaponDurabilityLosOnShotReduce.Value, instance.Overheat, _E62A.PastTime);
			float overheatProblemsStart = instance.Overheat.OverheatProblemsStart;
			if (weapon.MalfState.LastShotOverheat >= overheatProblemsStart)
			{
				num = Mathf.Lerp(1f, instance.Overheat.MaxCOIIncreaseMult, (weapon.MalfState.LastShotOverheat - overheatProblemsStart) / (instance.Overheat.MaxOverheat - overheatProblemsStart));
			}
			int num2;
			if (multiShot)
			{
				num2 = ((chamberIndex > 0) ? 1 : 0);
				if (num2 != 0)
				{
					float x = UnityEngine.Random.Range(this.m__E00D.DupletAccuracyPenaltyX.x, this.m__E00D.DupletAccuracyPenaltyX.y);
					float y = UnityEngine.Random.Range(this.m__E00D.DupletAccuracyPenaltyY.x, this.m__E00D.DupletAccuracyPenaltyY.y);
					Vector3 vector = new Vector3(x, y);
					float angle = vector.y * -1f;
					direction = Quaternion.AngleAxis(vector.x, original.forward) * direction;
					direction = Quaternion.AngleAxis(angle, original.right) * direction;
				}
			}
			else
			{
				num2 = 0;
			}
			float num3 = (weapon.CylinderHammerClosed ? (weapon.DoubleActionAccuracyPenalty * (1f - this.m__E013.DoubleActionRecoilReduce) * weapon.StockDoubleActionAccuracyPenaltyMult) : 0f);
			float num4 = _E034(weapon);
			double num5 = weapon.GetItemComponent<BuffComponent>().WeaponSpread;
			if (num5.ApproxEquals(0.0))
			{
				num5 = 1.0;
			}
			Vector3 shotDirection = (this._E00B() ? direction : (direction * 100f + (this.m__E011 + num3) * ammoFactor * num * num4 * (float)num5 * UnityEngine.Random.insideUnitSphere));
			InitiateShot(weapon, ammo, position2, shotDirection.normalized, position, chamberIndex, weapon.MalfState.LastShotOverheat);
			float num6 = ((num2 != 0) ? 1.5f : 1f);
			this.m__E01C = num6 + (float)ammo.ammoRec / 100f;
			_E038(this.m__E01D, ammo, position2, shotDirection, multiShot);
			if (ammo.AmmoTemplate.IsLightAndSoundShot)
			{
				_E039(position, direction);
				LightAndSoundShot(position, direction, ammo.AmmoTemplate);
			}
		}

		private void _E038(WeaponSoundPlayer weaponSoundPlayer, _EA12 ammo, Vector3 shotPosition, Vector3 shotDirection, bool multiShot)
		{
			_E5CB instance = Singleton<_E5CB>.Instance;
			float num = 1f;
			num = ((Item.FireMode.FireMode != 0 || !(Item.MalfState.OverheatFirerateMult > 0f)) ? (1f + UnityEngine.Random.Range(-0.03f, 0.03f)) : (1f + (1f - Item.MalfState.OverheatFirerateMult) * instance.Overheat.FireratePitchMult));
			if (Item.FireMode.FireMode != Weapon.EFireMode.burst || Item.FireMode.BurstShotsCount != 2 || IsBirstOf2Start || Item.ChamberAmmoCount <= 0)
			{
				weaponSoundPlayer.FireBullet(ammo, shotPosition, shotDirection.normalized, num, Malfunction, multiShot, IsBirstOf2Start);
			}
		}

		protected internal virtual void DryShot(int chamberIndex = 0, bool underbarrelShot = false)
		{
			SetCompassState(active: false);
		}

		protected virtual void ShotMisfired(_EA12 ammo, Weapon.EMalfunctionState malfunctionState, float overheat)
		{
		}

		protected virtual void RegisterShot(Item weapon, _EC26 shot)
		{
		}

		protected virtual void InitiateShot(IWeapon weapon, _EA12 ammo, Vector3 shotPosition, Vector3 shotDirection, Vector3 fireportPosition, int chamberIndex, float overheat)
		{
			_player.OnMakingShot(weapon, _player.PlayerBones.WeaponRoot.position - shotPosition);
			if (ammo.InitialSpeed > 0f)
			{
				if (ammo.ProjectileCount == 1)
				{
					_EC26 shot = BallisticsCalculator.Shoot(ammo, shotPosition, shotDirection, _player, weapon.Item, weapon.SpeedFactor, 0);
					RegisterShot(weapon.Item, shot);
				}
				else
				{
					FirearmController.m__E009.Clear();
					BallisticsCalculator.ShotMultiProjectileShot(ammo, shotPosition, shotDirection, weapon.SpeedFactor, FirearmController.m__E009, _player, weapon.Item);
					foreach (_EC26 item in FirearmController.m__E009)
					{
						RegisterShot(weapon.Item, item);
					}
					FirearmController.m__E009.Clear();
				}
			}
			this.m__E006?.Invoke();
			this.m__E016 = true;
			if (weapon.IsUnderbarrelWeapon)
			{
				this.m__E01F.FirearmsEffects.StartFireEffects(_player.IsVisible, _player.SqrCameraDistance);
			}
			else
			{
				this._E00C.PlayShotEffects(_player.IsVisible, _player.SqrCameraDistance);
			}
		}

		protected virtual void SendStartOneShotFire()
		{
		}

		protected virtual void CreateFlareShot(_EA12 flareItem, Vector3 shotPosition, Vector3 forward)
		{
			AmmoPoolObject ammoPoolObject = UnityEngine.Object.Instantiate(Singleton<_ED0A>.Instance.GetAsset<AmmoPoolObject>(flareItem.Template.Prefab));
			ammoPoolObject.transform.position = shotPosition;
			ammoPoolObject.transform.forward = forward;
			ammoPoolObject.gameObject.SetActive(value: true);
			FlareCartridge flareCartridge = ammoPoolObject.GetComponent<FlareCartridge>();
			if (flareCartridge == null)
			{
				flareCartridge = ammoPoolObject.gameObject.AddComponent<FlareCartridge>();
			}
			FlareCartridgeSettings flareCartridgeSettings = ammoPoolObject.GetComponent<FlareCartridgeSettings>();
			if (flareCartridgeSettings == null)
			{
				flareCartridgeSettings = ammoPoolObject.gameObject.AddComponent<FlareCartridgeSettings>();
			}
			flareCartridge.Init(flareCartridgeSettings, _player, flareItem, Item);
			flareCartridge.Launch();
			Singleton<_E5CE>.Instance.RegisterGrenade(flareCartridge);
			this.m__E016 = true;
		}

		public override void ManualLateUpdate(float deltaTime)
		{
			if (!_E2B6.Config.UseSpiritPlayer || !_player.Spirit.IsActive)
			{
				if (this.m__E015)
				{
					WeaponOverlapping();
					this.m__E015 = false;
				}
				if (this.m__E016)
				{
					this.m__E016 = false;
					_player.ProceduralWeaponAnimation.Shoot(this.m__E01C);
				}
			}
		}

		public override void OnPlayerDead()
		{
			_ECC9.ReleaseBeginSample(_ED3E._E000(161927), _ED3E._E000(161958));
			this.m__E008?.Invoke();
			CurrentOperation.FastForward();
			RemoveBallisticCalculator();
			base.OnPlayerDead();
		}

		public override void FastForwardCurrentState()
		{
			CurrentOperation.FastForward();
			base.FastForwardCurrentState();
		}

		public bool IsInSpawnOperation()
		{
			if (CurrentOperation != null)
			{
				return CurrentOperation is _E030;
			}
			return true;
		}

		public bool IsInReloadOperation()
		{
			if (!(CurrentOperation is _E003) && !(CurrentOperation is _E012) && !(CurrentOperation is _E01A) && !(CurrentOperation is _E01D))
			{
				return CurrentOperation is _E019;
			}
			return true;
		}

		public virtual void OpticCalibrationSwitchUp(_E6C6[] scopeStates)
		{
			this._E00C.OpticCalibrationSwitchUp();
		}

		public virtual void OpticCalibrationSwitchDown(_E6C6[] scopeStates)
		{
			this._E00C.OpticCalibrationSwitchDown();
		}

		public bool HasScopeAimBone(SightComponent sightComp)
		{
			List<ProceduralWeaponAnimation._E001> scopeAimTransforms = _player.ProceduralWeaponAnimation.ScopeAimTransforms;
			for (int i = 0; i < scopeAimTransforms.Count; i++)
			{
				if (scopeAimTransforms[i].Mod != null && scopeAimTransforms[i].Mod.Equals(sightComp))
				{
					return true;
				}
			}
			return false;
		}

		public override void SetCompassState(bool active)
		{
			if (CanChangeCompassState(active))
			{
				CurrentOperation.SetFirearmCompassState(active);
			}
		}

		protected List<_EA12> FindAmmoByIds(string[] ammoIds)
		{
			_preallocatedAmmoList.Clear();
			foreach (string itemId in ammoIds)
			{
				_ECD9<Item> obj = _player.FindItemById(itemId);
				if (obj.Succeeded && obj.Value is _EA12 item)
				{
					_preallocatedAmmoList.Add(item);
				}
			}
			return _preallocatedAmmoList;
		}

		private void _E039(Vector3 point, Vector3 direction)
		{
			Singleton<Effects>.Instance.EmitGrenade(_ED3E._E000(162011), point, direction);
		}

		protected virtual void LightAndSoundShot(Vector3 point, Vector3 direction, AmmoTemplate ammoTemplate)
		{
			_player.ActiveHealthController?.DoContusion(ammoTemplate.LightAndSoundShotSelfContusionTime, ammoTemplate.LightAndSoundShotSelfContusionStrength);
			Vector3 blindness = ammoTemplate.Blindness;
			float y = blindness.y;
			Collider[] array = Physics.OverlapSphere(point, y, _E37B.PlayerMask);
			List<Player> list = null;
			Dictionary<Player, _E6DF> dictionary = null;
			float num = Mathf.Cos(ammoTemplate.LightAndSoundShotAngle * 0.5f * ((float)Math.PI / 180f));
			Collider[] array2 = array;
			foreach (Collider col in array2)
			{
				Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
				if (!(playerByCollider == null) && !(playerByCollider == _player))
				{
					if (list == null)
					{
						list = new List<Player>();
						dictionary = new Dictionary<Player, _E6DF>();
					}
					Vector3 vector = playerByCollider.PlayerBones.Head.position - point;
					Vector3 normalized = vector.normalized;
					float magnitude = vector.magnitude;
					bool flag = Vector3.Dot(direction, normalized) >= num;
					list.Add(playerByCollider);
					dictionary.Add(playerByCollider, new _E6DF
					{
						Distance = magnitude,
						DirectionToEmitter = -normalized,
						TryToApplyStun = flag,
						TryToApplyBurnEyes = flag,
						TryToApplyContusion = true
					});
				}
			}
			if (list != null)
			{
				_E6E0.ApplyLightAndSoundHealthEffects(list, dictionary, point, blindness, ammoTemplate.Contusion);
			}
		}

		[CompilerGenerated]
		private Player._E00E _E03A()
		{
			return new _E030(this);
		}

		[CompilerGenerated]
		private Player._E00E _E03B()
		{
			return new _E02B(this);
		}

		[CompilerGenerated]
		private Player._E00E _E03C()
		{
			return new _E011(this);
		}

		[CompilerGenerated]
		private Player._E00E _E03D()
		{
			return new _E020(this);
		}

		[CompilerGenerated]
		private Player._E00E _E03E()
		{
			if (Item.MustBoltBeOpennedForInternalReload)
			{
				return new _E024(this);
			}
			return new _E023(this);
		}

		[CompilerGenerated]
		private Player._E00E _E03F()
		{
			return new _E01F(this);
		}

		[CompilerGenerated]
		private Player._E00E _E040()
		{
			return new _E028(this);
		}

		[CompilerGenerated]
		private Player._E00E _E041()
		{
			return new _E025(this);
		}

		[CompilerGenerated]
		private Player._E00E _E042()
		{
			return new _E01D(this);
		}

		[CompilerGenerated]
		private Player._E00E _E043()
		{
			return new _E012(this);
		}

		[CompilerGenerated]
		private Player._E00E _E044()
		{
			return new _E01A(this);
		}

		[CompilerGenerated]
		private Player._E00E _E045()
		{
			return new _E019(this);
		}

		[CompilerGenerated]
		private Player._E00E _E046()
		{
			return new _E005(this);
		}

		[CompilerGenerated]
		private Player._E00E _E047()
		{
			return new _E02A(this);
		}

		[CompilerGenerated]
		private Player._E00E _E048()
		{
			return new _E008(this);
		}

		[CompilerGenerated]
		private Player._E00E _E049()
		{
			return new _E001(this);
		}

		[CompilerGenerated]
		private Player._E00E _E04A()
		{
			return new _E000(this);
		}

		[CompilerGenerated]
		private Player._E00E _E04B()
		{
			if (!Item.IsFlareGun)
			{
				if (!Item.IsOneOff)
				{
					if (Item.ReloadMode != Weapon.EReloadMode.OnlyBarrel)
					{
						if (!(Item is _EAD1))
						{
							if (!Item.BoltAction)
							{
								return new _E00C(this);
							}
							return new _E004(this);
						}
						return new _E00A(this);
					}
					return new _E00B(this);
				}
				return new _E01C(this);
			}
			return new _E00F(this);
		}

		[CompilerGenerated]
		private Player._E00E _E04C()
		{
			return new _E002(this);
		}

		[CompilerGenerated]
		private Player._E00E _E04D()
		{
			return new _E031(this);
		}

		[CompilerGenerated]
		private Player._E00E _E04E()
		{
			return new _E02D(this);
		}

		[CompilerGenerated]
		private Player._E00E _E04F()
		{
			return new _E01E(this);
		}

		[CompilerGenerated]
		private Player._E00E _E050()
		{
			return new _E02E(this);
		}

		[CompilerGenerated]
		private Player._E00E _E051()
		{
			return new _E032(this);
		}

		[CompilerGenerated]
		private Player._E00E _E052()
		{
			return new _E015(this);
		}

		[CompilerGenerated]
		private Player._E00E _E053()
		{
			return new _E014(this);
		}

		[CompilerGenerated]
		private Player._E00E _E054()
		{
			return new _E016(this);
		}

		[CompilerGenerated]
		private Player._E00E _E055()
		{
			if (!Item.IsMultiBarrel)
			{
				return new _E007(this);
			}
			return new _E006(this);
		}

		[CompilerGenerated]
		private Player._E00E _E056()
		{
			return new _E018(this);
		}

		[CompilerGenerated]
		private Player._E00E _E057()
		{
			return new _E034(this);
		}

		[CompilerGenerated]
		private Player._E00E _E058()
		{
			return new _E017(this);
		}

		[CompilerGenerated]
		private Player._E00E _E059()
		{
			return new _E010(this);
		}

		[CompilerGenerated]
		private Player._E00E _E05A()
		{
			return new _E009(this);
		}
	}

	public interface _E008
	{
		void OnDrawCompleteAction();

		void OnHideCompleteActionAction();

		void OnDropGrenadeAction();

		void OnDropFinishedAction();

		void StartCountdown();

		void HideGrenade(Action onHidden, bool fastHide);

		void PutGrenadeBack();

		void ShowGesture(EGesture gesture);

		bool CanRemove();

		void FastForward();

		void OnBackpackDrop();

		void Execute(_EB72 operation, Callback callback);
	}

	internal abstract class BaseGrenadeController : ItemHandsController
	{
		[CompilerGenerated]
		private new sealed class _E001<_E077> where _E077 : BaseGrenadeController
		{
			public _E077 controller;

			public Player player;

			internal void _E000()
			{
				controller._E022.RemoveEventsConsumer(controller);
			}

			internal Transform _E001(string x)
			{
				return _E38B.FindTransformRecursive(player.PlayerBones.WeaponRoot.Original, x);
			}
		}

		[CompilerGenerated]
		private new sealed class _E002
		{
			public BaseGrenadeController _003C_003E4__this;

			public float animationSpeed;

			public Action callback;

			public bool fastDrop;

			internal void _E000(Result<_E6CC> result)
			{
				_003C_003E4__this.ActualDrop(result, animationSpeed, callback, fastDrop);
			}
		}

		[CompilerGenerated]
		private new sealed class _E003
		{
			public _E013 inventoryOperation;

			public Action callback;

			internal void _E000()
			{
				inventoryOperation.Confirm();
				callback();
			}
		}

		protected FirearmsAnimator _E022;

		private const string _E023 = "fireport";

		protected Transform _E024;

		protected Transform _E025;

		protected Transform[] _E026;

		protected GrenadePrefab _E027;

		protected GrenadeEmission _E028;

		private static readonly RaycastHit[] _E029 = new RaycastHit[8];

		private static Func<RaycastHit, bool> _E02A = (RaycastHit raycastHit) => false;

		public new _EADF Item => base.Item as _EADF;

		public override FirearmsAnimator FirearmsAnimator => _E022;

		protected new _E008 _E000 => base.CurrentHandsOperation as _E008;

		internal static _E077 _E000<_E077>(Player player, _EADF item, bool setQuickThrowParameters) where _E077 : BaseGrenadeController
		{
			_E077 val = ItemHandsController._E000<_E077>(player, item);
			_E002(val, player, setQuickThrowParameters);
			return val;
		}

		internal static async Task<_E077> _E001<_E077>(Player player, _EADF item, bool setQuickThrowParameters) where _E077 : BaseGrenadeController
		{
			_E077 obj = await ItemHandsController._E002<_E077>(player, item);
			_E002(obj, player, setQuickThrowParameters);
			return obj;
		}

		private static void _E002<_E077>(_E077 controller, Player player, bool setQuickThrowParameters) where _E077 : BaseGrenadeController
		{
			WeaponPrefab componentInChildren = controller.ControllerGameObject.GetComponentInChildren<WeaponPrefab>();
			controller._E022 = componentInChildren.FirearmsAnimator;
			controller._E022.AddEventsConsumer(controller);
			controller.CompositeDisposable.AddDisposable(delegate
			{
				controller._E022.RemoveEventsConsumer(controller);
			});
			controller._E024 = player.PlayerBones.WeaponRoot.Original;
			if (setQuickThrowParameters)
			{
				controller._E022.SetQuickFire(quickFire: true);
				controller._E022.SetActiveParam(active: false);
			}
			controller._E022.SkipTime(1f / 60f);
			controller._E027 = controller._controllerObject.GetComponent<GrenadePrefab>();
			controller._E025 = _E38B.FindTransformRecursive(player.PlayerBones.WeaponRoot.Original, controller._E027.ThrowingParts[0]);
			controller._E025.gameObject.SetActive(value: true);
			controller._E026 = controller._E027.ThrowingParts.Select((string x) => _E38B.FindTransformRecursive(player.PlayerBones.WeaponRoot.Original, x)).ToArray();
			for (int i = 0; i < controller._E026.Length; i++)
			{
				controller._E026[i].gameObject.SetActive(value: true);
			}
			controller._player.HandsAnimator = controller._E022;
			bool flag = controller._player.UpdateGrenadeAnimatorDuePoV();
			controller._E022.Animator.SetFloat(_ED3E._E000(155816), flag ? 1 : 0);
			player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			player.ProceduralWeaponAnimation.InitTransforms(controller.HandsHierarchy);
			componentInChildren.ObjectInHands.AfterGetFromPoolInit(player.ProceduralWeaponAnimation, null, player.IsYourPlayer);
			controller._controllerObject.GetComponent<BaseSoundPlayer>().Init(controller, player.PlayerBones.WeaponRoot, player);
		}

		protected override void IEventsConsumerOnWeapIn()
		{
			_E007();
		}

		protected override void IEventsConsumerOnWeapOut()
		{
			_E006();
		}

		protected override void IEventsConsumerOnFiringBullet()
		{
			_E005();
		}

		protected override void IEventsConsumerOnFireEnd()
		{
			_E004();
		}

		protected override void IEventsConsumerOnAddAmmoInChamber()
		{
			_E003();
		}

		protected override void IEventsConsumerOnDelAmmoChamber()
		{
			_E009();
		}

		protected override void IEventsConsumerOnThirdAction(int intParam)
		{
			TranslateAnimatorParameter(intParam);
		}

		protected override void IEventsConsumerOnCook()
		{
			_E00D();
		}

		protected override void IEventsOnBackpackDrop()
		{
			_E008();
		}

		public override bool SupportPickup()
		{
			return true;
		}

		protected override void IEventsConsumerOnOnUseProp(bool boolParam)
		{
			SetPropVisibility(boolParam);
		}

		public override void Pickup(bool p)
		{
			if (CanInteract())
			{
				_E022.SetPickup(p);
			}
		}

		public override void Interact(bool isInteracting, int actionIndex)
		{
			if (CanInteract())
			{
				_E022.SetInteract(isInteracting, actionIndex);
			}
		}

		public override bool CanInteract()
		{
			if (_E022.IsIdling())
			{
				return _E022.Animator.GetBool(_E326.BOOL_ACTIVE);
			}
			return false;
		}

		public override void Loot(bool p)
		{
			if (CanInteract())
			{
				_E022.SetLooting(p);
			}
		}

		public override float GetAnimatorFloatParam(int hash)
		{
			return _E022.GetAnimatorParameter(hash);
		}

		public override bool IsInInteraction()
		{
			return _E022.IsInInteraction;
		}

		public override bool IsInInteractionStrictCheck()
		{
			if (!IsInInteraction() && !(_E022.GetLayerWeight(_E022.LACTIONS_LAYER_INDEX) >= float.Epsilon))
			{
				return _E022.Animator.IsInTransition(_E022.LACTIONS_LAYER_INDEX);
			}
			return true;
		}

		public override void Spawn(float animationSpeed, Action callback)
		{
			_E022.SetAnimationSpeed(animationSpeed);
		}

		public override void Drop(float animationSpeed, Action callback, bool fastDrop, Item nextControllerItem = null)
		{
			Callback<_E6CC> callback2 = delegate(Result<_E6CC> result)
			{
				ActualDrop(result, animationSpeed, callback, fastDrop);
			};
			GrenadeController grenadeController = this as GrenadeController;
			if (CanRemove())
			{
				callback2((Result<_E6CC>)(_E6CC)grenadeController);
			}
			else if (grenadeController != null)
			{
				grenadeController.SetOnUsedCallback(callback2);
			}
		}

		public virtual void ActualDrop(Result<_E6CC> controller, float animationSpeed, Action callback, bool fastDrop)
		{
			if (base.Destroyed)
			{
				this._E000.HideGrenade(callback, fastDrop);
				return;
			}
			base.Destroyed = true;
			_E022.SetAnimationSpeed(animationSpeed);
			_E013 inventoryOperation = _player._E03B(Item);
			Action onHidden = delegate
			{
				inventoryOperation.Confirm();
				callback();
			};
			this._E000.HideGrenade(onHidden, fastDrop);
		}

		public override void Destroy()
		{
			_player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			base.Destroy();
			_E022 = null;
			AssetPoolObject.ReturnToPool(_controllerObject.gameObject);
		}

		protected void _E003()
		{
			this._E000.StartCountdown();
		}

		protected virtual void _E00D()
		{
			if (!(_E028 != null))
			{
				_E028 = Singleton<Effects>.Instance.GetEmissionEffect(_E027.GrenadeItself.EmmisionEffect);
				_E028.AttachTo(_E025, _E027.GrenadeItself.Offset);
				_E028.SetFillParams(0f, Item.EmitTime);
				_E028.StartEmission(0f);
			}
		}

		protected void _E004()
		{
			this._E000.OnDropFinishedAction();
		}

		protected void _E005()
		{
			this._E000.OnDropGrenadeAction();
		}

		protected void _E006()
		{
			this._E000.OnHideCompleteActionAction();
		}

		protected void _E007()
		{
			this._E000.OnDrawCompleteAction();
		}

		public override bool CanExecute(_EB72 operation)
		{
			return true;
		}

		public override void Execute(_EB72 operation, Callback callback)
		{
			this._E000.Execute(operation, callback);
		}

		public override void ShowGesture(EGesture gesture)
		{
			this._E000.ShowGesture(gesture);
		}

		private void _E008()
		{
			this._E000.OnBackpackDrop();
		}

		private void _E009()
		{
			this._E000.PutGrenadeBack();
		}

		protected virtual void _E00E(float timeSinceSafetyLevelRemoved, bool low = false)
		{
			if (_E2B6.Config.UseSpiritPlayer && _player.Spirit.IsActive)
			{
				_player.Spirit.PlayerSync();
			}
			float num = (low ? 0.66f : (1f + (float)_player.Skills.StrengthBuffThrowDistanceInc));
			Vector3 direction = Vector3.up;
			float forcePower = 3f;
			if (_player.HealthController.IsAlive)
			{
				if (_player.IsAI && _player.AIData.BotOwner.BotState == EBotState.Active)
				{
					try
					{
						_E14B grenades = _player.AIData.BotOwner.WeaponManager.Grenades;
						float num2 = ((grenades.Mass <= 0.01f) ? 0.6f : grenades.Mass);
						forcePower = grenades.AIGreanageThrowData.Force * num2;
						direction = _E39C.NormalizeFastSelf(grenades.ToThrowDirection);
					}
					catch (Exception)
					{
						return;
					}
				}
				else
				{
					forcePower = EFTHardSettings.Instance.GrenadeForce;
					if (!_player.Skills.ThrowingEliteBuff)
					{
						Vector3 vector = Mathf.Clamp01(0.5f - _player.Physical.HandsStamina.NormalValue) * UnityEngine.Random.onUnitSphere;
						direction = (-_E024.up * 5f + vector).normalized;
						num *= Mathf.Lerp(0.4f, 1f, _player.Physical.HandsStamina.NormalValue + 0.5f);
					}
					else
					{
						direction = -_E024.up;
					}
				}
			}
			_E00A(timeSinceSafetyLevelRemoved, num, direction, forcePower, low);
		}

		private void _E00A(float timeSinceSafetyLevelRemoved, float lowHighThrow, Vector3 direction, float forcePower, bool lowThrow)
		{
			Vector3 force = direction * (forcePower * lowHighThrow) + _player.Velocity;
			Vector3 vector = _E025.position + _E025.rotation * _E027.GrenadeItself.Offset;
			if (CheckHandsToBodyObstacles(_player, vector, out var _, out var correctedPoint))
			{
				vector = correctedPoint;
			}
			_E00F(timeSinceSafetyLevelRemoved, vector, _E025.rotation, force, lowThrow);
		}

		public static bool CheckHandsToBodyObstacles(Player player, Vector3 point, out RaycastHit hit, out Vector3 correctedPoint)
		{
			Vector3 projectionOnRealForwardSurface = player.MovementContext.GetProjectionOnRealForwardSurface(point);
			bool isForwardHit;
			bool num = _E320.LinecastInBothSides(projectionOnRealForwardSurface, point, out hit, out isForwardHit, _EC20.StaticObjectsHitMask, _EC20.StaticObjectsHitMask, _E029, _E02A);
			if (num)
			{
				correctedPoint = hit.point - (point - projectionOnRealForwardSurface).normalized * 0.1f;
				return num;
			}
			correctedPoint = point;
			return num;
		}

		private Grenade _E00B(Vector3 position, Quaternion rotation, Vector3 force, float prewarm = 0f)
		{
			GrenadeSettings grenadeSettings = UnityEngine.Object.Instantiate(_E027.GrenadeItself);
			Grenade grenade = Singleton<_E5CE>.Instance.GrenadeFactory.Create(grenadeSettings, position, rotation, force, Item, _player, prewarm);
			Singleton<_E5CE>.Instance.RegisterGrenade(grenade);
			return grenade;
		}

		protected virtual void _E00F(float timeSinceSafetyLevelRemoved, Vector3 position, Quaternion rotation, Vector3 force, bool lowThrow)
		{
			_player.ExecuteSkill((Action)delegate
			{
				_player.Skills.ThrowAction.Complete();
			});
			_ECD8<_E010> obj = _player.ThrowGrenade(Item, lowThrow, simulate: false);
			if (obj.Succeeded)
			{
				obj.Value.RaiseEvents(_player._E0DE, CommandStatus.Begin);
				obj.Value.RaiseEvents(_player._E0DE, CommandStatus.Succeed);
				Grenade grenade = _E00B(position, rotation, force, timeSinceSafetyLevelRemoved);
				SmokeGrenade smokeGrenade = grenade as SmokeGrenade;
				if (smokeGrenade != null)
				{
					if (_E028 == null)
					{
						_E00D();
					}
					if (_E028 != null)
					{
						_E028.AttachTo(smokeGrenade.transform, Vector3.zero);
						smokeGrenade.EmissionEnd = (Action<Grenade>)Delegate.Combine(smokeGrenade.EmissionEnd, new Action<Grenade>(_E028.StopEmission));
						smokeGrenade.VelocityBelowThreshold += _E028.Stall;
					}
				}
				Transform[] array = _E026;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].gameObject.SetActive(value: false);
				}
				if (Singleton<_E307>.Instantiated)
				{
					Singleton<_E307>.Instance.ThrowGrenade(grenade, position + Vector3.up, force, 0.6f);
				}
			}
			else
			{
				UnityEngine.Debug.LogError(_ED3E._E000(155861) + obj.Error);
			}
		}

		public override void FastForwardCurrentState()
		{
			this._E000.FastForward();
		}

		[CompilerGenerated]
		private void _E00C()
		{
			_player.Skills.ThrowAction.Complete();
		}
	}

	internal class GrenadeController : BaseGrenadeController, _E6CC, _E6CA, _E6C7, _E6C8
	{
		public new class _E000 : _E001
		{
			private new Callback m__E000;

			public _E000(GrenadeController controller)
				: base(controller)
			{
			}

			public virtual void Start(Item item, Callback callback)
			{
				m__E000 = callback;
				Start();
				_E002.FirearmsAnimator.SetInventory(open: false);
				_E002.FirearmsAnimator.Animator.SetLayerWeight(2, 1f);
				_E002.FirearmsAnimator.SetInteract(p: true, 300);
			}

			public override void Reset()
			{
				m__E000 = null;
				base.Reset();
			}

			public override void OnBackpackDrop()
			{
				State = EOperationState.Finished;
				_E002.FirearmsAnimator.SetInteract(p: false, 300);
				_E326.ResetTriggerHandReady(_E002.FirearmsAnimator.Animator);
				_E002.FirearmsAnimator.SetInventory(_E002.m__E002);
				_E002.InitiateOperation<_E003>().Start();
				m__E000.Succeed();
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.m__E002 = opened;
			}
		}

		public new abstract class _E001 : _E009<GrenadeController>
		{
			protected _E001(GrenadeController controller)
				: base(controller)
			{
			}

			public virtual void ExamineWeapon()
			{
				_E000();
			}

			public virtual void PullRingForHighThrow()
			{
				_E000();
			}

			public virtual void HighThrow()
			{
				_E000();
			}

			public virtual void PullRingForLowThrow()
			{
				_E000();
			}

			public virtual void LowThrow()
			{
				_E000();
			}

			public virtual void SetInventoryOpened(bool opened)
			{
				_E000();
			}

			public virtual void SetGrenadeCompassState(bool active)
			{
				_E000();
			}
		}

		protected new class _E002 : _E001
		{
			protected enum EThrowState
			{
				None,
				Idling,
				Throwing,
				Threw
			}

			protected new EThrowState _E001;

			private new Action m__E002;

			public virtual bool WaitingHighThrow => this._E001 == EThrowState.Idling;

			public _E002(GrenadeController controller)
				: base(controller)
			{
			}

			public new void Start()
			{
				base._E002._player.Say(EPhraseTrigger.OnGrenade);
				base.Start();
				_E005();
				this._E001 = EThrowState.Idling;
			}

			public override void Reset()
			{
				base.Reset();
				this._E001 = EThrowState.None;
				m__E002 = null;
			}

			protected virtual void _E005()
			{
				base._E002._E022.SetGrenadeFire(FirearmsAnimator.EGrenadeFire.Hold);
			}

			public override void HighThrow()
			{
				if (this._E001 != EThrowState.Throwing && this._E001 != EThrowState.Threw)
				{
					this._E001 = EThrowState.Throwing;
					base._E002._E022.SetGrenadeFire(FirearmsAnimator.EGrenadeFire.Throw);
				}
			}

			public override void OnDropGrenadeAction()
			{
				base._E002._E022.SetGrenadeFire(FirearmsAnimator.EGrenadeFire.Idle);
				_E000();
			}

			protected void _E000(bool low = false)
			{
				base._E002._E025.gameObject.SetActive(value: false);
				base._E002._E00E(0f, low);
			}

			public override void OnDropFinishedAction()
			{
				if (this._E001 == EThrowState.Threw)
				{
					return;
				}
				this._E001 = EThrowState.Threw;
				if (base._E002.Destroyed)
				{
					if (m__E002 != null)
					{
						m__E002();
					}
					m__E002 = null;
				}
				else if (base._E002._E02B != null)
				{
					base._E002._E02B((Result<_E6CC>)(_E6CC)base._E002);
				}
			}

			public override void HideGrenade(Action onHidden, bool fastHide)
			{
				if (this._E001 == EThrowState.Threw)
				{
					onHidden();
				}
				else if (this._E001 == EThrowState.Idling && base._E002.Item.CanBeHiddenDuringThrow)
				{
					m__E002 = onHidden;
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E005>().Start(onHidden);
				}
				else
				{
					m__E002 = onHidden;
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				if (this._E001 == EThrowState.Idling && base._E002.Item.CanBeHiddenDuringThrow)
				{
					base._E002._E022.SetInventory(opened);
					PutGrenadeBack();
				}
			}

			protected new void _E001()
			{
				State = EOperationState.Finished;
				base._E002.InitiateOperation<_E003>().Start();
			}

			public override bool CanRemove()
			{
				return this._E001 != EThrowState.Throwing;
			}

			public override void PutGrenadeBack()
			{
				base._E002._E022.SetGrenadeAltFire(FirearmsAnimator.EGrenadeFire.Idle);
				base._E002._E022.SetGrenadeFire(FirearmsAnimator.EGrenadeFire.Idle);
				_E001();
			}

			public override void FastForward()
			{
				base.FastForward();
				if (this._E001 == EThrowState.Idling)
				{
					base._E002._E008();
				}
				else if (this._E001 != EThrowState.Threw)
				{
					UnityEngine.Debug.LogErrorFormat(_ED3E._E000(155902), this._E001);
				}
			}
		}

		protected new class _E003 : _E001
		{
			public _E003(GrenadeController controller)
				: base(controller)
			{
			}

			public new void Start()
			{
				base.Start();
			}

			public override void HideGrenade(Action onHidden, bool fastHide)
			{
				State = EOperationState.Finished;
				_E002.InitiateOperation<_E005>().Start(onHidden);
			}

			public override bool CanRemove()
			{
				return true;
			}

			public override void OnEnd()
			{
				_E002.SetCompassState(active: false);
			}

			public override void ExamineWeapon()
			{
				_E002._E022.LookTrigger();
			}

			public override void PullRingForHighThrow()
			{
				State = EOperationState.Finished;
				_E002.InitiateOperation<_E002>().Start();
			}

			public override void PullRingForLowThrow()
			{
				State = EOperationState.Finished;
				_E002.InitiateOperation<_E004>().Start();
			}

			public override void LowThrow()
			{
			}

			public override void HighThrow()
			{
			}

			public override void SetGrenadeCompassState(bool active)
			{
				_E002.CompassState.Value = active;
			}

			public override void Execute(_EB72 operation, Callback callback)
			{
				if (!(operation is _EB75 obj))
				{
					callback.Succeed();
				}
				else if (obj.From1 != null && _E002._player._E0DE.IsInAnimatedSlot(obj.Item1))
				{
					State = EOperationState.Finished;
					_E002.InitiateOperation<_E000>().Start(obj.Item1, callback);
				}
				else
				{
					callback.Succeed();
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002._E022.SetInventory(opened);
			}

			public override void ShowGesture(EGesture gesture)
			{
				_E002._E022.Gesture(gesture);
			}
		}

		protected new class _E004 : _E002
		{
			public override bool WaitingHighThrow => false;

			public bool WaitingLowThrow => base.WaitingHighThrow;

			public _E004(GrenadeController controller)
				: base(controller)
			{
			}

			protected override void _E005()
			{
				_E002._E022.SetGrenadeAltFire(FirearmsAnimator.EGrenadeFire.Hold);
			}

			public override void HighThrow()
			{
				_E000();
			}

			public override void LowThrow()
			{
				if (base._E001 != EThrowState.Throwing && base._E001 != EThrowState.Threw)
				{
					base._E001 = EThrowState.Throwing;
					_E002._E022.SetGrenadeAltFire(FirearmsAnimator.EGrenadeFire.Throw);
				}
			}

			public override void OnDropGrenadeAction()
			{
				_E002._E022.SetGrenadeAltFire(FirearmsAnimator.EGrenadeFire.Idle);
				_E000(low: true);
			}
		}

		protected new class _E005 : _E001
		{
			private new Action _E000;

			public _E005(GrenadeController controller)
				: base(controller)
			{
			}

			public void Start(Action callback)
			{
				_E000 = callback;
				Start();
				_E002._E022.SetActiveParam(active: false);
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}

			public override void Reset()
			{
				_E000 = null;
				base.Reset();
			}

			public override void OnHideCompleteActionAction()
			{
				State = EOperationState.Finished;
				_E000();
			}

			public override void HideGrenade(Action onHidden, bool fastHide)
			{
				_E000 = (Action)Delegate.Combine(_E000, onHidden);
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					OnHideCompleteActionAction();
				}
			}
		}

		public new sealed class _E006 : _E000
		{
			private const float _E003 = 0.25f;

			private float _E004;

			private bool _E005;

			public _E006(GrenadeController controller)
				: base(controller)
			{
			}

			public override void Start(Item item, Callback callback)
			{
				_E004 = 0f;
				_E005 = false;
				base.Start(item, callback);
			}

			public override void FastForward()
			{
				if (!_E005)
				{
					_E005 = true;
					OnBackpackDrop();
				}
			}

			public override void Update(float deltaTime)
			{
				base.Update(deltaTime);
				if (!_E005 && _E004 > 0.25f)
				{
					_E005 = true;
					OnBackpackDrop();
				}
				_E004 += deltaTime;
			}
		}

		private new class _E007 : _E001
		{
			private new Action _E000;

			private Action _E006;

			public _E007(GrenadeController controller)
				: base(controller)
			{
			}

			public void Start(Action callback)
			{
				_E000 = callback;
				Start();
				_E002._E022.SetActiveParam(active: true);
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.WEAPON_SIZE_MODIFIER_PARAM_HASH, 1f);
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}

			public override void Reset()
			{
				_E000 = null;
				_E006 = null;
				base.Reset();
			}

			public override void OnDrawCompleteAction()
			{
				_E002.SetupProp();
				State = EOperationState.Finished;
				_E003 obj = _E002.InitiateOperation<_E003>();
				obj.Start();
				_E000();
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 0f);
				if (_E006 != null)
				{
					obj.HideGrenade(_E006, fastHide: false);
				}
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002._E022.SetInventory(opened);
			}

			public override void HideGrenade(Action onHidden, bool fastHide)
			{
				_E006 = onHidden;
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					OnDrawCompleteAction();
				}
			}
		}

		private new bool m__E002;

		private Callback<_E6CC> _E02B;

		public new _EADF Item => base.Item;

		protected internal new _E001 _E000 => base.CurrentHandsOperation as _E001;

		public bool WaitingForHighThrow
		{
			get
			{
				if (this._E000 is _E002 obj)
				{
					return obj.WaitingHighThrow;
				}
				return false;
			}
		}

		public bool WaitingForLowThrow
		{
			get
			{
				if (this._E000 is _E004 obj)
				{
					return obj.WaitingLowThrow;
				}
				return false;
			}
		}

		protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
		{
			return new Dictionary<Type, OperationFactoryDelegate>
			{
				{
					typeof(_E007),
					() => new _E007(this)
				},
				{
					typeof(_E003),
					() => new _E003(this)
				},
				{
					typeof(_E005),
					() => new _E005(this)
				},
				{
					typeof(_E002),
					() => new _E002(this)
				},
				{
					typeof(_E004),
					() => new _E004(this)
				},
				{
					typeof(_E000),
					() => new _E000(this)
				}
			};
		}

		internal new static _E077 _E000<_E077>(Player player, _EADF item) where _E077 : GrenadeController
		{
			return BaseGrenadeController._E000<_E077>(player, item, setQuickThrowParameters: false);
		}

		internal static Task<_E077> _E001<_E077>(Player player, _EADF item) where _E077 : GrenadeController
		{
			return BaseGrenadeController._E001<_E077>(player, item, setQuickThrowParameters: false);
		}

		public override bool CanExecute(_EB72 operation)
		{
			if (!(operation is _EB75 obj))
			{
				return true;
			}
			if (obj.From1 != null && _player._E0DE.IsInAnimatedSlot(obj.Item1))
			{
				return this._E000 is _E003;
			}
			return true;
		}

		public override void Execute(_EB72 operation, Callback callback)
		{
			base._E000.Execute(operation, callback);
		}

		public override void Spawn(float animationSpeed, Action callback)
		{
			InitiateOperation<_E007>().Start(callback);
			base.Spawn(animationSpeed, callback);
		}

		public virtual void ExamineWeapon()
		{
			this._E000.ExamineWeapon();
		}

		public virtual void PullRingForHighThrow()
		{
			this._E000.PullRingForHighThrow();
		}

		public virtual void HighThrow()
		{
			this._E000.HighThrow();
		}

		public virtual void PullRingForLowThrow()
		{
			this._E000.PullRingForLowThrow();
		}

		public virtual void LowThrow()
		{
			this._E000.LowThrow();
		}

		public virtual void SetOnUsedCallback(Callback<_E6CC> callback)
		{
			_E02B = callback;
		}

		public override void SetInventoryOpened(bool opened)
		{
			if (opened)
			{
				SetCompassState(active: false);
			}
			this._E000.SetInventoryOpened(opened);
		}

		public override bool IsInventoryOpen()
		{
			return _E022.IsInInventory;
		}

		public override bool CanRemove()
		{
			return this._E000.CanRemove();
		}

		protected virtual void _E008()
		{
			this._E000.PutGrenadeBack();
		}

		public virtual bool CanThrow()
		{
			return _player.StateIsSuitableForHandInput;
		}

		public override void SetCompassState(bool active)
		{
			if (CanChangeCompassState(active))
			{
				this._E000.SetGrenadeCompassState(active);
			}
		}

		[CompilerGenerated]
		private _E00E _E002()
		{
			return new _E007(this);
		}

		[CompilerGenerated]
		private new _E00E _E003()
		{
			return new _E003(this);
		}

		[CompilerGenerated]
		private new _E00E _E004()
		{
			return new _E005(this);
		}

		[CompilerGenerated]
		private new _E00E _E005()
		{
			return new _E002(this);
		}

		[CompilerGenerated]
		private new _E00E _E006()
		{
			return new _E004(this);
		}

		[CompilerGenerated]
		private new _E00E _E007()
		{
			return new _E000(this);
		}
	}

	internal abstract class _E009<_E0A2> : _E00E, _E008 where _E0A2 : BaseGrenadeController
	{
		protected readonly _E0A2 _E002;

		protected _E009(_E0A2 controller)
			: base(controller)
		{
			_E002 = controller;
		}

		public virtual void OnDrawCompleteAction()
		{
			_E000();
		}

		public virtual void OnHideCompleteActionAction()
		{
			_E000();
		}

		public virtual void OnDropGrenadeAction()
		{
			_E000();
		}

		public virtual void OnDropFinishedAction()
		{
			_E000();
		}

		public virtual void StartCountdown()
		{
			_E000();
		}

		public virtual void HideGrenade(Action onHidden, bool fastHide)
		{
			_E000();
		}

		public virtual void PutGrenadeBack()
		{
			_E000();
		}

		public virtual void ShowGesture(EGesture gesture)
		{
			_E000();
		}

		public virtual bool CanRemove()
		{
			return false;
		}

		public virtual void FastForward()
		{
		}

		public virtual void OnBackpackDrop()
		{
			_E000();
		}

		public virtual void Execute(_EB72 operation, Callback callback)
		{
			if (!(operation is _EB75 obj))
			{
				callback.Succeed();
			}
			else if (obj.From1 != null && _E002._player._E0DE.IsInAnimatedSlot(obj.Item1))
			{
				callback.Fail(string.Format(_ED3E._E000(163626), GetType()));
			}
			else
			{
				callback.Succeed();
			}
		}
	}

	internal class QuickGrenadeThrowController : BaseGrenadeController, _E6D2, _E6D1<_EADF>, _E6D0, _E6C7
	{
		public new class _E000 : _E009<QuickGrenadeThrowController>
		{
			private new Action m__E000;

			private new Action _E001;

			private new Callback<_E6D1<_EADF>> _E002;

			private bool _E003;

			private bool _E004;

			private float _E005 = -1f;

			public _E000(QuickGrenadeThrowController controller)
				: base(controller)
			{
			}

			public void Start(Action callback)
			{
				base._E002._player.Say(EPhraseTrigger.OnGrenade);
				m__E000 = callback;
				Start();
				_E003 = (_E004 = false);
			}

			public void SetOnUsedCallback(Callback<_E6D1<_EADF>> callback)
			{
				_E002 = callback;
			}

			public override void OnDropGrenadeAction()
			{
				_E004 = true;
				base._E002._E00E(_E005);
			}

			public override void OnDropFinishedAction()
			{
				_E003 = true;
				base._E002._E022.SetQuickFire(quickFire: false);
				if (base._E002.Destroyed)
				{
					_E001();
				}
				else if (_E002 != null)
				{
					_E002((Result<_E6D1<_EADF>>)(_E6D1<_EADF>)base._E002);
				}
			}

			public override void OnDrawCompleteAction()
			{
				m__E000();
			}

			public override void HideGrenade(Action onHidden, bool fastHide = false)
			{
				if (_E003)
				{
					onHidden();
				}
				else if (_E001 == null)
				{
					_E001 = onHidden;
				}
				else
				{
					_E001 = (Action)Delegate.Combine(_E001, onHidden);
				}
			}

			public override void StartCountdown()
			{
				_E005 = 0f;
			}

			public override void Update(float deltaTime)
			{
				base.Update(deltaTime);
				if (_E005 >= 0f)
				{
					_E005 += deltaTime;
				}
			}

			public override void Reset()
			{
				base.Reset();
				_E005 = -1f;
				_E003 = (_E004 = false);
				_E001 = null;
				m__E000 = null;
				_E002 = null;
			}

			public override void FastForward()
			{
				_ = _E004;
				if (!_E003)
				{
					OnDropFinishedAction();
				}
			}
		}

		[CompilerGenerated]
		private new sealed class _E001
		{
			public Action callback;

			internal void _E000()
			{
				callback();
			}
		}

		protected new _E000 _E000 => base.CurrentHandsOperation as _E000;

		protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
		{
			return new Dictionary<Type, OperationFactoryDelegate> { 
			{
				typeof(_E000),
				() => new _E000(this)
			} };
		}

		internal new static _E077 _E000<_E077>(Player player, _EADF item) where _E077 : QuickGrenadeThrowController
		{
			return BaseGrenadeController._E000<_E077>(player, item, setQuickThrowParameters: true);
		}

		internal static Task<_E077> _E001<_E077>(Player player, _EADF item) where _E077 : QuickGrenadeThrowController
		{
			return BaseGrenadeController._E001<_E077>(player, item, setQuickThrowParameters: true);
		}

		public void SetOnUsedCallback(Callback<_E6D1<_EADF>> callback)
		{
			this._E000.SetOnUsedCallback(callback);
		}

		public override void Spawn(float animationSpeed, Action callback)
		{
			Action callback2 = delegate
			{
				callback();
			};
			InitiateOperation<_E000>().Start(callback2);
			base.Spawn(animationSpeed, callback);
		}

		public override bool CanExecute(_EB72 operation)
		{
			return false;
		}

		public override bool CanRemove()
		{
			return true;
		}

		[CompilerGenerated]
		private _E00E _E002()
		{
			return new _E000(this);
		}
	}

	private interface _E00A
	{
		void OnDrawCompleteAction();

		void OnHideCompleteActionAction();

		void OnUseAction();

		void HideController(Action onHidden, bool fastHide);
	}

	public class QuickUseItemController : ItemHandsController, _E6D4, _E6C7
	{
		public new abstract class _E000 : _E00E, _E00A
		{
			protected readonly QuickUseItemController _E002;

			protected _E000(QuickUseItemController controller)
				: base(controller)
			{
				_E002 = controller;
			}

			public virtual void OnDrawCompleteAction()
			{
				_E000();
			}

			public virtual void OnHideCompleteActionAction()
			{
				_E000();
			}

			public virtual void OnUseAction()
			{
				_E000();
			}

			public virtual void HideController(Action onHidden, bool fastHide)
			{
				_E000();
			}
		}

		public new class _E001 : _E000
		{
			private Action _E07B;

			private Action _E07C;

			private Callback<_E6D4> _E07D;

			public _E001(QuickUseItemController controller)
				: base(controller)
			{
			}

			public void Start(Action callback)
			{
				_E07B = callback;
				_E002._objectInHandsAnimator.SetActiveParam(active: true);
				Start();
			}

			public void SetOnUsedCallback(Callback<_E6D4> callback)
			{
				_E07D = callback;
			}

			public Callback<_E6D4> GetOnUsedCallback()
			{
				return _E07D;
			}

			public override void OnUseAction()
			{
				_E002._objectInHandsAnimator.SetActiveParam(active: false);
				_E002._E004();
				if (_E002.Destroyed)
				{
					Action action = _E07C;
					_E07C = null;
					action();
				}
				else if (_E07D != null)
				{
					Callback<_E6D4> callback = _E07D;
					_E07D = null;
					callback((Result<_E6D4>)(_E6D4)_E002);
				}
			}

			public override void OnDrawCompleteAction()
			{
				_E07B();
			}

			public override void Reset()
			{
				base.Reset();
				_E07C = null;
				_E07B = null;
				_E07D = null;
			}

			public virtual void FastForward()
			{
				_E000();
			}
		}

		[CompilerGenerated]
		private new sealed class _E002<_E077> where _E077 : QuickUseItemController
		{
			public _E077 controller;

			internal void _E000()
			{
				controller._objectInHandsAnimator.RemoveEventsConsumer(controller);
			}
		}

		[CompilerGenerated]
		private new sealed class _E003
		{
			public Action callback;

			internal void _E000()
			{
				callback();
			}
		}

		[CompilerGenerated]
		private sealed class _E004
		{
			public Action callback;

			internal void _E000()
			{
				callback();
			}
		}

		protected GameObject _usableItemGameObject;

		protected new FirearmsAnimator _objectInHandsAnimator;

		public override FirearmsAnimator FirearmsAnimator => _objectInHandsAnimator;

		public override string LoggerDistinctId => string.Format(_ED3E._E000(163638), _player.ProfileId, _player.Profile.Info.Nickname, this);

		protected _E001 CurrentOperation => base.CurrentHandsOperation as _E001;

		internal new static _E077 _E000<_E077>(Player player, Item item) where _E077 : QuickUseItemController
		{
			_E077 controller = ItemHandsController._E001<_E077>(player, item, Singleton<_E760>.Instance.CreateItemUsablePrefab);
			UsableHandsPrefab component = controller._controllerObject.GetComponent<UsableHandsPrefab>();
			GameObject gameObject = Singleton<_E760>.Instance.CreateItem(item, isAnimated: true);
			gameObject.transform.SetParent(component.ItemSpawnTransform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.SetActive(value: true);
			controller._usableItemGameObject = gameObject;
			controller._objectInHandsAnimator = component.FirearmsAnimator;
			controller._objectInHandsAnimator.AddEventsConsumer(controller);
			controller.CompositeDisposable.AddDisposable(delegate
			{
				controller._objectInHandsAnimator.RemoveEventsConsumer(controller);
			});
			controller._objectInHandsAnimator.SkipTime(1f / 60f);
			controller._player.HandsAnimator = controller._objectInHandsAnimator;
			bool flag = controller._player.UpdateGrenadeAnimatorDuePoV();
			controller._objectInHandsAnimator.Animator.SetFloat(_ED3E._E000(155816), flag ? 1 : 0);
			player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			player.ProceduralWeaponAnimation.InitTransforms(controller.HandsHierarchy);
			component.ObjectInHands.AfterGetFromPoolInit(player.ProceduralWeaponAnimation, null, player.IsYourPlayer);
			controller._controllerObject.GetComponent<BaseSoundPlayer>().Init(controller, player.PlayerBones.WeaponRoot, player);
			return controller;
		}

		protected override void IEventsConsumerOnWeapIn()
		{
			_E001();
		}

		protected override void IEventsConsumerOnWeapOut()
		{
			_E002();
		}

		protected override void IEventsConsumerOnFiringBullet()
		{
			_E003();
		}

		protected override void IEventsConsumerOnThirdAction(int i)
		{
			TranslateAnimatorParameter(i);
		}

		protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
		{
			return new Dictionary<Type, OperationFactoryDelegate> { 
			{
				typeof(_E001),
				() => new _E001(this)
			} };
		}

		public override void Spawn(float animationSpeed, Action callback)
		{
			Action callback2 = delegate
			{
				callback();
			};
			InitiateOperation<_E001>().Start(callback2);
		}

		public override void Drop(float animationSpeed, Action callback, bool fastDrop, Item nextControllerItem = null)
		{
			if (!base.Destroyed)
			{
				base.Destroyed = true;
				_objectInHandsAnimator.SetAnimationSpeed(animationSpeed);
				((Action)delegate
				{
					callback();
				})();
			}
		}

		public override void Destroy()
		{
			_player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			base.Destroy();
			_objectInHandsAnimator = null;
			AssetPoolObject.ReturnToPool(_controllerObject.gameObject);
		}

		public override bool CanExecute(_EB72 operation)
		{
			if (operation is _EB95 obj)
			{
				return obj._E016 == base.Item;
			}
			return false;
		}

		public override void Execute(_EB72 operation, Callback callback)
		{
			callback.Succeed();
		}

		public override bool CanRemove()
		{
			return false;
		}

		public override bool CanInteract()
		{
			return false;
		}

		public override void Interact(bool isInteracting, int actionIndex)
		{
		}

		public void SetOnUsedCallback(Callback<_E6D4> callback)
		{
			CurrentOperation.SetOnUsedCallback(callback);
		}

		public Callback<_E6D4> GetOnUsedCallback()
		{
			return CurrentOperation.GetOnUsedCallback();
		}

		private void _E001()
		{
			CurrentOperation.OnDrawCompleteAction();
		}

		private void _E002()
		{
			CurrentOperation.OnHideCompleteActionAction();
		}

		private void _E003()
		{
			CurrentOperation.OnUseAction();
		}

		private void _E004()
		{
			AssetPoolObject.ReturnToPool(_usableItemGameObject);
			_usableItemGameObject = null;
		}

		public override void ShowGesture(EGesture gesture)
		{
		}

		public override void FastForwardCurrentState()
		{
			CurrentOperation.FastForward();
		}

		[CompilerGenerated]
		private _E00E _E005()
		{
			return new _E001(this);
		}
	}

	public enum EOperationState
	{
		Ready,
		Executing,
		Finished
	}

	public abstract class ItemHandsController : AbstractHandsController, _E6C7
	{
		internal delegate GameObject _E000(Item item, Player player);

		internal delegate Task<GameObject> _E001(Item item, Player player);

		protected delegate _E00E OperationFactoryDelegate();

		protected class HandsControllerLogger : _E315
		{
			private readonly string _E010;

			public HandsControllerLogger(LoggerMode loggerMode, ItemHandsController controller)
				: base(_ED3E._E000(155974), loggerMode)
			{
				_E010 = controller._player.ProfileId + _ED3E._E000(64014) + controller._player.Profile.Nickname;
			}

			public void TraceStateChange(_E00E currentOperation, _E00E nextOperation)
			{
				if (IsEnabled(NLog.LogLevel.Trace))
				{
					Log(_ED3E._E000(156027), _ED3E._E000(156061), NLog.LogLevel.Trace, currentOperation, nextOperation, _E010, Time.frameCount);
				}
			}

			[Conditional("UNITY_EDITOR")]
			public void TraceMethodCall()
			{
				if (IsEnabled(NLog.LogLevel.Trace))
				{
					StackFrame stackFrame = new StackFrame(1);
					Log(_ED3E._E000(156095), _ED3E._E000(156114), NLog.LogLevel.Trace, stackFrame.GetMethod().DeclaringType, stackFrame.GetMethod().Name, Time.frameCount, _E010);
				}
			}

			[Conditional("UNITY_EDITOR")]
			public void TraceMethodCall<T>(string argName, T argument1)
			{
				if (IsEnabled(NLog.LogLevel.Trace))
				{
					StackFrame stackFrame = new StackFrame(1);
					Log(_ED3E._E000(156169), _ED3E._E000(156245), NLog.LogLevel.Trace, stackFrame.GetMethod().DeclaringType, stackFrame.GetMethod().Name, argName, argument1, Time.frameCount, _E010);
				}
			}
		}

		protected FirearmsAnimator _objectInHandsAnimator;

		private readonly Dictionary<Type, _E00E> _E02C = new Dictionary<Type, _E00E>();

		protected GameObject _controllerObject;

		protected TransformLinks _handsHierarchy;

		private Item _E02D;

		private Dictionary<Type, OperationFactoryDelegate> _E02E;

		protected internal Player _player;

		protected _ECF5<bool> CompassState = new _ECF5<bool>(initialValue: false);

		protected _ECF5<bool> RadioTransmitterState = new _ECF5<bool>(initialValue: false);

		protected HandsControllerLogger Logger;

		[CompilerGenerated]
		private _E00E _E02F;

		public override FirearmsAnimator FirearmsAnimator => _objectInHandsAnimator;

		public bool SuitableForHandInput => _player.StateIsSuitableForHandInput;

		public bool CurrentCompassState => CompassState.Value;

		public bool CurrentRadioTransmitterState => RadioTransmitterState.Value;

		public override GameObject ControllerGameObject => _controllerObject;

		public override float AimingSensitivity => _player.GetAimingSensitivity();

		public override TransformLinks HandsHierarchy => _handsHierarchy;

		protected _E00E CurrentHandsOperation
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

		public string CurrentHandsOperationName
		{
			get
			{
				if (CurrentHandsOperation == null)
				{
					return string.Empty;
				}
				string text = CurrentHandsOperation.GetType().ToString();
				if (!text.Contains(_ED3E._E000(29692)))
				{
					return text;
				}
				string[] array = text.Split('+');
				return array[array.Length - 1];
			}
		}

		Item _E6C7.Item => _E02D;

		public override void BlindFire(int b)
		{
		}

		protected override Item GetItem()
		{
			return _E02D;
		}

		internal static _E077 _E000<_E077>(Player player, Item item) where _E077 : ItemHandsController
		{
			if (player.PlayerBody != null)
			{
				player.PlayerBody.GetSlotViewByItem(item)?.DestroyCurrentModel();
			}
			return _E001<_E077>(player, item, (Item item1, Player player1) => Singleton<_E760>.Instance.CreateItem(item1, player1, isAnimated: true));
		}

		internal static _E077 _E001<_E077>(Player player, Item item, _E000 itemObjectFactoryDelegate) where _E077 : ItemHandsController
		{
			_E077 val = player.gameObject.AddComponent<_E077>();
			val._controllerObject = itemObjectFactoryDelegate(item, player);
			_E004(val, player, item);
			return val;
		}

		internal static Task<_E077> _E002<_E077>(Player player, Item item) where _E077 : ItemHandsController
		{
			return _E003<_E077>(player, item, (Item item1, Player player1) => Singleton<_E760>.Instance.CreateItemAsync(item1, player1, isAnimated: true, _ECE3.General));
		}

		internal static async Task<_E077> _E003<_E077>(Player player, Item item, _E001 itemObjectAsyncFactoryDelegate) where _E077 : ItemHandsController
		{
			_E077 val = player.gameObject.AddComponent<_E077>();
			val._controllerObject = await itemObjectAsyncFactoryDelegate(item, player);
			_E004(val, player, item);
			return val;
		}

		private static void _E004<_E077>(_E077 controller, Player player, Item item) where _E077 : ItemHandsController
		{
			WeaponPrefab component = controller._controllerObject.GetComponent<WeaponPrefab>();
			controller._handsHierarchy = component.Hierarchy;
			controller._objectInHandsAnimator = component.FirearmsAnimator;
			controller.AnimationEventsEmitter = component.AnimationEventsEmitter;
			controller._player = player;
			controller._controllerObject.transform.SetPositionAndRotation(player.PlayerBones.Ribcage.Original.position, player.HandsRotation);
			player.UpdateBonesOnWeaponChange(controller._handsHierarchy);
			controller._E02D = item;
			controller.WeaponRoot = player.PlayerBones.WeaponRoot.Original;
			controller.Logger = new HandsControllerLogger(LoggerMode.Add, controller);
		}

		public void TranslateAnimatorParameter(int actionIndex)
		{
			_player.BodyAnimatorCommon.SetInteger(Player._E005.FIRST_PERSON_ACTION, actionIndex);
		}

		public override bool IsPlacingBeacon()
		{
			return CurrentHandsOperation is QuickUseItemController._E001;
		}

		protected void SetupProp()
		{
			if (_player.HealthController.IsAlive)
			{
				CompositeDisposable.BindState(CompassState, CompassStateHandler);
				CompositeDisposable.BindState(_player.MovementContext.CanUseProp, OnCanUsePropChanged);
			}
		}

		protected virtual void OnCanUsePropChanged(bool canUse)
		{
			if (!canUse)
			{
				SetCompassState(active: false);
			}
		}

		protected void SetPropVisibility(bool isVisible)
		{
			if (!isVisible)
			{
				SetCompassState(active: false);
			}
			_player.SetPropVisibility(CompassState.Value);
		}

		public override void ManualUpdate(float deltaTime)
		{
			CurrentHandsOperation.Update(deltaTime);
		}

		public void ToggleCompassState()
		{
			SetCompassState(!CompassState.Value);
		}

		public virtual void SetCompassState(bool active)
		{
			if (CanChangeCompassState(active))
			{
				CompassState.Value = active;
			}
		}

		public void ApplyCompassPacket(_E94A packet)
		{
			if (packet.Toggle)
			{
				CompassState.Value = packet.Status;
			}
		}

		protected virtual bool CanChangeCompassState(bool newState)
		{
			if (newState)
			{
				if (_player.MovementContext.CanUseProp.Value)
				{
					return !IsInInteractionStrictCheck();
				}
				return false;
			}
			return true;
		}

		protected virtual void CompassStateHandler(bool isActive)
		{
			_player.CreateCompass();
			_objectInHandsAnimator.ShowCompass(isActive);
			if (!isActive && _player.PointOfView == EPointOfView.FirstPerson && Singleton<GameUI>.Instantiated && Singleton<GameUI>.Instance.BattleUiScreen != null)
			{
				Singleton<GameUI>.Instance.BattleUiScreen.HideAzimuth();
			}
		}

		public override void BallisticUpdate(float deltaTime)
		{
		}

		public override void EmitEvents()
		{
			base.AnimationEventsEmitter.EmitEvents();
		}

		protected abstract Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates();

		protected internal void ClearPreWarmOperationsDict()
		{
			_E02C.Clear();
		}

		protected internal TCreateOperation InitiateOperation<TCreateOperation>() where TCreateOperation : _E00E
		{
			if (_E02E == null)
			{
				_E02E = GetOperationFactoryDelegates();
			}
			Type typeFromHandle = typeof(TCreateOperation);
			if (!_E02C.ContainsKey(typeFromHandle))
			{
				_E02C[typeFromHandle] = _E02E[typeFromHandle]();
			}
			_E00E obj = _E02C[typeFromHandle];
			obj.UpdateLoggerController(this);
			Logger.TraceStateChange(CurrentHandsOperation, obj);
			if (CurrentHandsOperation != null)
			{
				CurrentHandsOperation.OnEnd();
			}
			CurrentHandsOperation = obj;
			CurrentHandsOperation.Reset();
			return (TCreateOperation)CurrentHandsOperation;
		}

		public override string ToString()
		{
			return string.Format(_ED3E._E000(155955), base.ToString(), _E02D, CurrentHandsOperation);
		}
	}

	internal abstract class BaseKnifeController : ItemHandsController
	{
		[CompilerGenerated]
		private new sealed class _E002<_E077> where _E077 : BaseKnifeController
		{
			public _E077 controller;

			public KnifeComponent knife;

			internal void _E000()
			{
				((BaseKnifeController)controller)._E001.RemoveEventsConsumer(controller);
			}

			internal void _E001()
			{
				controller._player.BodyAnimatorCommon.SetLayerWeight(knife.Template.AdditionalAnimationLayer, 0f);
				controller._player.BodyAnimatorCommon.SetLayerWeight(knife.Template.AdditionalAnimationLayer + 1, 0f);
				controller._E033 = null;
			}
		}

		[CompilerGenerated]
		private new sealed class _E003
		{
			public BaseKnifeController _003C_003E4__this;

			public _E013 inventoryOperation;

			public Action callback;

			internal void _E000()
			{
				_003C_003E4__this._player.ProceduralWeaponAnimation.enabled = true;
				inventoryOperation.Confirm();
				callback();
				_003C_003E4__this._E033?.Invoke();
			}
		}

		public EKickType LastKickType;

		private float _E030;

		protected Vector3 _E031 = Vector3.zero;

		protected Vector3 _E032 = Vector3.zero;

		public const float LERP_DIRECTION_T = 0.9f;

		private Action _E033;

		protected bool _E034 = true;

		protected int _E035 = 1000;

		protected _E6FB _E036;

		protected new FirearmsAnimator _E001;

		protected Transform _E037;

		protected KnifeCollider _E038;

		private int _E039;

		private int _E03A;

		private int _E03B;

		public KnifeComponent Knife => base.Item.GetItemComponent<KnifeComponent>();

		public override string LoggerDistinctId => string.Format(_ED3E._E000(163638), _player.ProfileId, _player.Profile.Info.Nickname, this);

		protected new _E00B _E000 => base.CurrentHandsOperation as _E00B;

		public override FirearmsAnimator FirearmsAnimator => this._E001;

		protected async Task _E000()
		{
			_E034 = false;
			await Task.Delay(_E035);
			_E034 = true;
		}

		public override float GetAnimatorFloatParam(int hash)
		{
			return this._E001.GetAnimatorParameter(hash);
		}

		public override bool SupportPickup()
		{
			return true;
		}

		public override void Pickup(bool p)
		{
			if (CanInteract())
			{
				this._E001.SetPickup(p);
			}
		}

		public override void Interact(bool isInteracting, int actionIndex)
		{
			if (CanInteract())
			{
				this._E001.SetInteract(isInteracting, actionIndex);
			}
		}

		public override void Loot(bool p)
		{
			if (CanInteract())
			{
				this._E001.SetLooting(p);
			}
		}

		public override bool CanInteract()
		{
			if (this._E001.IsIdling())
			{
				return this._E001.Animator.GetBool(_E326.BOOL_ACTIVE);
			}
			return false;
		}

		public override bool IsInInteraction()
		{
			return this._E001.IsInInteraction;
		}

		public override bool IsInInteractionStrictCheck()
		{
			if (!IsInInteraction() && !(this._E001.GetLayerWeight(this._E001.LACTIONS_LAYER_INDEX) >= float.Epsilon))
			{
				return this._E001.Animator.IsInTransition(this._E001.LACTIONS_LAYER_INDEX);
			}
			return true;
		}

		public Vector3 GetPlayerOrientation()
		{
			return _player.LookDirection;
		}

		public Vector3 GetPlayerCastOrigin()
		{
			return _player.MovementContext.RibcagePosition();
		}

		public override void ShowGesture(EGesture gesture)
		{
			if (gesture != 0)
			{
				this._E001.Gesture(gesture);
			}
		}

		internal static _E077 _E001<_E077>(Player player, KnifeComponent knife) where _E077 : BaseKnifeController
		{
			_E077 val = ItemHandsController._E000<_E077>(player, knife.Item);
			_E003(val, player);
			return val;
		}

		internal static async Task<_E077> _E002<_E077>(Player player, KnifeComponent knife) where _E077 : BaseKnifeController
		{
			_E077 obj = await ItemHandsController._E002<_E077>(player, knife.Item);
			_E003(obj, player);
			return obj;
		}

		private static void _E003<_E077>(_E077 controller, Player player) where _E077 : BaseKnifeController
		{
			controller._E039 = LayerMask.NameToLayer(_ED3E._E000(25428));
			controller._E03A = LayerMask.GetMask(_ED3E._E000(60764), _ED3E._E000(25428), _ED3E._E000(60852), _ED3E._E000(60801));
			controller._E03B = LayerMask.GetMask(_ED3E._E000(60679));
			WeaponPrefab componentInChildren = controller._controllerObject.GetComponentInChildren<WeaponPrefab>();
			controller._E036 = componentInChildren.ObjectInHands;
			((BaseKnifeController)controller)._E001 = componentInChildren.FirearmsAnimator;
			controller._E038 = controller.HandsHierarchy.Self.GetComponentInChildren<KnifeCollider>();
			controller._E038._E000 = controller;
			controller._E038._E001 = controller._player;
			controller._E038._hitMask = controller._E03A;
			controller._E038._spiritMask = controller._E03B;
			((BaseKnifeController)controller)._E001.AddEventsConsumer(controller);
			controller.CompositeDisposable.AddDisposable(delegate
			{
				((BaseKnifeController)controller)._E001.RemoveEventsConsumer(controller);
			});
			controller._E037 = _E38B.FindTransformRecursive(player.PlayerBones.WeaponRoot.Original, _ED3E._E000(156293));
			controller._player.HandsAnimator = ((BaseKnifeController)controller)._E001;
			KnifeComponent knife = controller.Item.GetItemComponent<KnifeComponent>();
			if (knife != null && knife.Template.AdditionalAnimationLayer > 0)
			{
				controller._player.BodyAnimatorCommon.SetLayerWeight(knife.Template.AdditionalAnimationLayer, 1f);
				controller._player.BodyAnimatorCommon.SetLayerWeight(knife.Template.AdditionalAnimationLayer + 1, 1f);
				controller._E033 = delegate
				{
					controller._player.BodyAnimatorCommon.SetLayerWeight(knife.Template.AdditionalAnimationLayer, 0f);
					controller._player.BodyAnimatorCommon.SetLayerWeight(knife.Template.AdditionalAnimationLayer + 1, 0f);
					controller._E033 = null;
				};
			}
			player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			player.ProceduralWeaponAnimation.InitTransforms(controller.HandsHierarchy);
			controller._E036.AfterGetFromPoolInit(player.ProceduralWeaponAnimation, null, player.IsYourPlayer);
			BaseSoundPlayer component = controller._controllerObject.GetComponent<BaseSoundPlayer>();
			if (component != null)
			{
				component.Init(controller, player.PlayerBones.WeaponRoot, player);
			}
		}

		protected override void IEventsConsumerOnOnUseProp(bool boolParam)
		{
			SetPropVisibility(boolParam);
		}

		protected override void IEventsConsumerOnWeapIn()
		{
			_E006();
		}

		protected override void IEventsConsumerOnWeapOut()
		{
			_E005();
		}

		protected override void IEventsConsumerOnThirdAction(int intParam)
		{
			TranslateAnimatorParameter(intParam);
		}

		protected override void IEventsConsumerOnFireEnd()
		{
			OnFireEnd();
		}

		protected override void IEventsConsumerOnIdleStart()
		{
			this._E000.OnIdleStart();
		}

		protected override void IEventsConsumerOnComboPlanning()
		{
			OnComboPlanning();
		}

		protected override void IEventsConsumerOnFiringBullet()
		{
			_E004();
		}

		protected override void IEventsOnBackpackDrop()
		{
			_E007();
		}

		public override void Drop(float animationSpeed, Action callback, bool fastDrop, Item nextControllerItem = null)
		{
			SetDeflected(deflected: false);
			if (base.Destroyed)
			{
				this._E000.HideWeapon(callback, fastDrop);
				return;
			}
			base.Destroyed = true;
			_E013 inventoryOperation = _player._E03B(Knife.Item);
			Action onHidden = delegate
			{
				_player.ProceduralWeaponAnimation.enabled = true;
				inventoryOperation.Confirm();
				callback();
				_E033?.Invoke();
			};
			this._E000.HideWeapon(onHidden, fastDrop);
		}

		public override void Destroy()
		{
			_E033?.Invoke();
			_player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			base.Destroy();
			this._E001 = null;
			AssetPoolObject.ReturnToPool(_controllerObject.gameObject);
		}

		private void _E004()
		{
			this._E000.OnFire();
		}

		public virtual void OnFireEnd()
		{
			this._E000.OnFireEnd();
		}

		protected virtual void OnComboPlanning()
		{
		}

		private void _E005()
		{
			this._E000.HideWeaponComplete();
		}

		private void _E006()
		{
			this._E000.WeaponAppeared();
		}

		private void _E007()
		{
			this._E000.OnBackpackDrop();
		}

		public override bool CanExecute(_EB72 operation)
		{
			return true;
		}

		public override void Execute(_EB72 operation, Callback callback)
		{
			this._E000.Execute(operation, callback);
		}

		protected void _E008(_E00D other)
		{
			if (_E038.OnHit != null)
			{
				KnifeCollider knifeCollider = _E038;
				knifeCollider.OnHit = (Action<_E00D>)Delegate.Remove(knifeCollider.OnHit, new Action<_E00D>(_E008));
			}
			BallisticCollider component = other.collider.GetComponent<BallisticCollider>();
			if (component != null)
			{
				other.point = ((other.point.sqrMagnitude < 0.1f) ? other.collider.transform.position : other.point);
				_E00B(other, component);
				SetDeflected(deflected: true);
				if (other.collider.gameObject.layer == _E039)
				{
					_player.Physical.ConsumeAsMelee(Knife.Template.DeflectionConsumption);
				}
			}
		}

		public void SetDeflected(bool deflected)
		{
			this._E001.SetDeflected(deflected);
			_player.MovementContext.PlayerAnimatorSetDeflected(deflected);
		}

		public void SetMeleeSpeed(float speed)
		{
			this._E001.SetMeleeSpeed(speed);
			_player.MovementContext.PlayerAnimatorSetSwingSpeed(speed);
		}

		private bool _E009(out RaycastHit hitInfo, int layerMask)
		{
			return Physics.SphereCast(_E037.position, 0.15f, _player.LookDirection, out hitInfo, 0.5f, layerMask);
		}

		protected virtual _E6FF _E00B(_E00D hit, BallisticCollider ballisticCollider)
		{
			int num = ((LastKickType == EKickType.Slash) ? Knife.Template.KnifeHitSlashDam : Knife.Template.KnifeHitStabDam);
			num = (_player.Physical.HandsStamina.Exhausted ? ((int)((float)num * Singleton<_E5CB>.Instance.Stamina.ExhaustedMeleeDamageMultiplier)) : num);
			_E031 = Vector3.Lerp(_E031, (_E038.transform.position - _E032).normalized, 0.9f);
			UnityEngine.Debug.DrawLine(hit.point, hit.point + _E031, Color.magenta, 10f);
			_EC23 obj = default(_EC23);
			obj.DamageType = EDamageType.Melee;
			obj.Damage = (float)num * (1f + (float)_player.Skills.StrengthBuffMeleePowerInc);
			obj.PenetrationPower = ((LastKickType == EKickType.Slash) ? Knife.Template.SlashPenetration : Knife.Template.StabPenetration);
			obj.ArmorDamage = 1f;
			obj.Direction = _E031;
			obj.HitCollider = hit.collider;
			obj.HitPoint = hit.point;
			obj.Player = _player;
			obj.HittedBallisticCollider = ballisticCollider;
			obj.HitNormal = hit.normal;
			obj.Weapon = Knife.Item;
			obj.IsForwardHit = true;
			obj.StaminaBurnRate = Knife.Template.StaminaBurnRate;
			_EC23 damageInfo = obj;
			_E6FF result = Singleton<GameWorld>.Instance.HackShot(damageInfo);
			if (ballisticCollider as BodyPartCollider != null)
			{
				_player.ExecuteSkill((Action)delegate
				{
					_player.Skills.FistfightAction.Complete();
				});
			}
			return result;
		}

		public override void FastForwardCurrentState()
		{
			this._E000.FastForward();
		}

		[CompilerGenerated]
		private void _E00A()
		{
			_player.Skills.FistfightAction.Complete();
		}
	}

	internal interface _E00B
	{
		void HideWeaponComplete();

		void WeaponAppeared();

		void HideWeapon(Action onHidden, bool fastDrop);

		void OnFireEnd();

		void OnFire();

		void OnBackpackDrop();

		void Execute<TInventoryOperation>(TInventoryOperation operation, Callback callback) where TInventoryOperation : _EB72;

		void FastForward();

		void OnIdleStart();
	}

	internal abstract class _E00C<_E0A3> : _E00E, _E00B where _E0A3 : BaseKnifeController
	{
		protected readonly _E0A3 _E002;

		protected _E00C(_E0A3 controller)
			: base(controller)
		{
			_E002 = controller;
		}

		public virtual void HideWeaponComplete()
		{
			_E000();
		}

		public virtual void WeaponAppeared()
		{
			_E000();
		}

		public virtual void OnBackpackDrop()
		{
			_E000();
		}

		public virtual void Execute<TInventoryOperation>(TInventoryOperation operation, Callback callback) where TInventoryOperation : _EB72
		{
			_E000();
			if (!((object)operation is _EB75 obj))
			{
				callback.Succeed();
			}
			else if (obj.From1 != null && _E002._player._E0DE.IsInAnimatedSlot(obj.Item1))
			{
				callback.Fail(string.Format(_ED3E._E000(163626), GetType()));
			}
			else
			{
				callback.Succeed();
			}
		}

		public virtual void HideWeapon(Action onHidden, bool fastDrop)
		{
			_E000();
		}

		public virtual void OnFireEnd()
		{
			_E000();
		}

		public virtual void OnFire()
		{
			_E000();
		}

		public virtual void FastForward()
		{
			_E000();
		}

		public virtual void OnIdleStart()
		{
		}
	}

	internal class KnifeController : BaseKnifeController, _E6CD, _E6CA, _E6C7, _E6C8
	{
		public new class _E000 : _E003
		{
			private new Callback m__E000;

			public _E000(KnifeController controller)
				: base(controller)
			{
			}

			public virtual void Start(Item item, Callback callback)
			{
				m__E000 = callback;
				Start();
				_E002.FirearmsAnimator.SetInventory(open: false);
				((BaseKnifeController)_E002)._E001.Animator.SetLayerWeight(2, 1f);
				((BaseKnifeController)_E002)._E001.SetInteract(p: true, 300);
			}

			public override void Reset()
			{
				m__E000 = null;
				base.Reset();
			}

			public override void OnBackpackDrop()
			{
				State = EOperationState.Finished;
				((BaseKnifeController)_E002)._E001.SetInteract(p: false, 300);
				_E326.ResetTriggerHandReady(_E002.FirearmsAnimator.Animator);
				((BaseKnifeController)_E002)._E001.SetInventory(_E002.m__E002);
				_E002.InitiateOperation<_E001>().Start();
				m__E000.Succeed();
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.m__E002 = opened;
			}
		}

		public new class _E001 : _E003
		{
			private new const float m__E001 = 300f;

			private new float _E002;

			public _E001(KnifeController controller)
				: base(controller)
			{
			}

			public new void Start()
			{
				base.Start();
				_E002 = 0f;
			}

			public override void Reset()
			{
				_E002 = 0f;
				base.Reset();
			}

			public override void HideWeapon(Action onHidden, bool fastDrop)
			{
				State = EOperationState.Finished;
				base._E002.InitiateOperation<_E004>().Start(onHidden, fastDrop);
			}

			public override bool CanRemove()
			{
				return true;
			}

			public override void Execute<TInventoryOperation>(TInventoryOperation operation, Callback callback)
			{
				if (!((object)operation is _EB75 obj))
				{
					callback.Succeed();
				}
				else if (obj.From1 != null && _E005._inventoryController.IsInAnimatedSlot(obj.Item1))
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E000>().Start(obj.Item1, callback);
				}
				else
				{
					callback.Succeed();
				}
			}

			public override void Update(float deltaTime)
			{
				_E002 += deltaTime;
				if (_E002 > 300f)
				{
					((BaseKnifeController)base._E002)._E001.Idle();
					_E002 = 0f;
				}
			}

			public override void ExamineWeapon()
			{
				((BaseKnifeController)base._E002)._E001.LookTrigger();
			}

			public override void SetInventoryOpened(bool opened)
			{
				((BaseKnifeController)base._E002)._E001.SetInventory(opened);
			}

			public override bool MakeKnifeKick()
			{
				if (!_E005.StateIsSuitableForHandInput)
				{
					return false;
				}
				if (base._E002._player.Physical.CanMeleeHit)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E002>().Start(EKickType.Slash);
					return true;
				}
				base._E002._player.Physical.InvokeInsufficient();
				return false;
			}

			public override void OnEnd()
			{
				SetKnifeCompassState(active: false);
			}

			public override void SetKnifeCompassState(bool active)
			{
				base._E002.CompassState.Value = active;
			}

			public override bool MakeAlternativeKick()
			{
				if (!_E005.StateIsSuitableForHandInput)
				{
					return false;
				}
				if (base._E002._player.Physical.CanMeleeHit)
				{
					State = EOperationState.Finished;
					base._E002.InitiateOperation<_E002>().Start(EKickType.Stab);
					return true;
				}
				base._E002._player.Physical.InvokeInsufficient();
				return false;
			}

			public override void StopKnifeKick()
			{
			}

			public override void StopAlternativeKick()
			{
			}
		}

		protected new class _E002 : _E003
		{
			[CompilerGenerated]
			private new sealed class _E000
			{
				public _E002 _003C_003E4__this;

				public Action onHidden;

				public bool fastDrop;

				internal void _E000()
				{
					_003C_003E4__this._E002.InitiateOperation<_E004>().Start(onHidden, fastDrop);
				}
			}

			private Action _E003;

			private bool _E004;

			public _E002(KnifeController controller)
				: base(controller)
			{
			}

			public void Start(EKickType eKickType)
			{
				Start();
				_E002.LastKickType = eKickType;
				if (eKickType == EKickType.Slash)
				{
					((BaseKnifeController)_E002)._E001.SetFire(fire: true);
				}
				else
				{
					((BaseKnifeController)_E002)._E001.SetAlternativeFire(fire: true);
				}
				_E002.SetDeflected(deflected: false);
				_E002.SetMeleeSpeed(_E005.Physical.MeleeSpeed);
				_E004 = false;
			}

			public override void Update(float deltaTime)
			{
				base.Update(deltaTime);
				_E002._E038.ManualUpdate();
				Vector3 position = _E002._E038.transform.position;
				Vector3 normalized = (position - _E002._E032).normalized;
				UnityEngine.Debug.DrawLine(position, _E002._E032, Color.cyan, 10f);
				_E002._E031 = Vector3.Lerp(_E002._E031, normalized, 0.9f);
				_E002._E032 = position;
			}

			public override void Reset()
			{
				_E003 = null;
				base.Reset();
			}

			public override void OnComboPlanning()
			{
			}

			public override void ContinueCombo()
			{
			}

			public override void BrakeCombo()
			{
				_E004 = true;
				((BaseKnifeController)_E002)._E001.SetFire(fire: false);
				_E002.TranslateAnimatorParameter(0);
			}

			public override void OnIdleStart()
			{
				_E002.OnFireEnd();
			}

			public override void StopKnifeKick()
			{
			}

			public override void StopAlternativeKick()
			{
			}

			public override void OnFireEnd()
			{
				((BaseKnifeController)_E002)._E001.SetFire(fire: false);
				((BaseKnifeController)_E002)._E001.SetAlternativeFire(fire: false);
				if (_E003 != null)
				{
					_E000();
				}
				else
				{
					_E001();
				}
				if (_E002._E038.OnHit != null)
				{
					KnifeCollider knifeCollider = _E002._E038;
					knifeCollider.OnHit = (Action<_E00D>)Delegate.Remove(knifeCollider.OnHit, new Action<_E00D>(_E002._E008));
				}
				_E002._E038.OnFireEnd();
			}

			public override void OnFire()
			{
				_E002._player.Physical.ConsumeAsMelee((_E002.LastKickType == EKickType.Slash) ? _E002.Knife.Template.PrimaryConsumption : _E002.Knife.Template.SecondaryConsumption);
				if (_E002._E038.OnHit == null)
				{
					KnifeCollider knifeCollider = _E002._E038;
					knifeCollider.OnHit = (Action<_E00D>)Delegate.Combine(knifeCollider.OnHit, new Action<_E00D>(_E002._E008));
				}
				_E002._E038.MaxDistance = ((_E002.LastKickType == EKickType.Slash) ? _E002.Knife.Template.PrimaryDistance : _E002.Knife.Template.SecondaryDistance);
				_E002._E038.OnFire();
			}

			public override void HideWeapon(Action onHidden, bool fastDrop)
			{
				State = EOperationState.Finished;
				_E003 = delegate
				{
					_E002.InitiateOperation<_E004>().Start(onHidden, fastDrop);
				};
			}

			private new void _E000()
			{
				State = EOperationState.Finished;
				_E003();
			}

			private new void _E001()
			{
				State = EOperationState.Finished;
				_E002.InitiateOperation<_E001>().Start();
			}
		}

		public new abstract class _E003 : _E00C<KnifeController>
		{
			protected readonly Player _E005;

			public _E003(KnifeController controller)
				: base(controller)
			{
				_E005 = controller._player;
			}

			public virtual void ExamineWeapon()
			{
				_E000();
			}

			public virtual void SetInventoryOpened(bool opened)
			{
				_E000();
			}

			public virtual bool MakeKnifeKick()
			{
				_E000();
				return false;
			}

			public virtual void OnComboPlanning()
			{
				_E000();
			}

			public virtual void BrakeCombo()
			{
				_E000();
			}

			public virtual void ContinueCombo()
			{
				_E000();
			}

			public virtual void StopKnifeKick()
			{
				_E000();
			}

			public virtual bool CanRemove()
			{
				return false;
			}

			public virtual bool MakeAlternativeKick()
			{
				_E000();
				return false;
			}

			public virtual void StopAlternativeKick()
			{
				_E000();
			}

			public virtual void SetKnifeCompassState(bool active)
			{
				_E000();
			}
		}

		private class _E004 : _E003
		{
			private Action _E006;

			public _E004(KnifeController controller)
				: base(controller)
			{
			}

			public void Start(Action onHidden, bool fastDrop)
			{
				_E006 = onHidden;
				Start();
				((BaseKnifeController)_E002)._E001.SetActiveParam(active: false);
				((BaseKnifeController)_E002)._E001.SetFastHide(fastDrop);
				_E002.SetMeleeSpeed(_E005.Physical.MeleeSpeed);
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}

			public override void Reset()
			{
				_E006 = null;
				base.Reset();
			}

			public override void HideWeaponComplete()
			{
				State = EOperationState.Finished;
				_E006();
			}

			public override void HideWeapon(Action onHidden, bool fastDrop)
			{
				_E006 = (Action)Delegate.Combine(_E006, onHidden);
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					HideWeaponComplete();
				}
			}
		}

		public sealed class _E005 : _E000
		{
			private const float _E007 = 0.25f;

			private float _E008;

			private bool _E009;

			public _E005(KnifeController controller)
				: base(controller)
			{
			}

			public override void Start(Item item, Callback callback)
			{
				_E008 = 0f;
				_E009 = false;
				base.Start(item, callback);
			}

			public override void FastForward()
			{
				if (!_E009)
				{
					_E009 = true;
					OnBackpackDrop();
				}
			}

			public override void Update(float deltaTime)
			{
				base.Update(deltaTime);
				if (!_E009 && _E008 > 0.25f)
				{
					_E009 = true;
					OnBackpackDrop();
				}
				_E008 += deltaTime;
			}
		}

		public class _E006 : _E003
		{
			private Action _E00A;

			private Action _E00B;

			private bool _E00C;

			public _E006(KnifeController controller)
				: base(controller)
			{
			}

			public void Start(Action callback)
			{
				_E00B = callback;
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.WEAPON_SIZE_MODIFIER_PARAM_HASH, 1f);
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				Start();
				((BaseKnifeController)_E002)._E001.SetActiveParam(active: true);
				((BaseKnifeController)_E002)._E001.SetMeleeSpeed(_E005.Physical.MeleeSpeed);
			}

			public override void Reset()
			{
				base.Reset();
				_E00B = null;
				_E00A = null;
			}

			public override void WeaponAppeared()
			{
				_E002.SetupProp();
				State = EOperationState.Finished;
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 0f);
				_E001 obj = _E002.InitiateOperation<_E001>();
				obj.Start();
				_E00B();
				if (_E00A != null)
				{
					obj.HideWeapon(_E00A, _E00C);
				}
			}

			public override void HideWeapon(Action onHidden, bool fastDrop)
			{
				_E00A = onHidden;
				_E00C = fastDrop;
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					WeaponAppeared();
				}
			}
		}

		private new bool m__E002;

		[CompilerGenerated]
		private Action _E03C;

		[CompilerGenerated]
		private Action _E03D;

		public Action ComboPlanning
		{
			[CompilerGenerated]
			get
			{
				return _E03C;
			}
			[CompilerGenerated]
			set
			{
				_E03C = value;
			}
		}

		public Action OnAttackEnd
		{
			[CompilerGenerated]
			get
			{
				return _E03D;
			}
			[CompilerGenerated]
			set
			{
				_E03D = value;
			}
		}

		public new KnifeComponent Knife => base.Knife;

		protected new _E003 _E000 => base.CurrentHandsOperation as _E003;

		protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
		{
			return new Dictionary<Type, OperationFactoryDelegate>
			{
				{
					typeof(_E006),
					() => new _E006(this)
				},
				{
					typeof(_E001),
					() => new _E001(this)
				},
				{
					typeof(_E004),
					() => new _E004(this)
				},
				{
					typeof(_E002),
					() => new _E002(this)
				},
				{
					typeof(_E000),
					() => new _E000(this)
				}
			};
		}

		internal new static _E077 _E000<_E077>(Player player, KnifeComponent knife) where _E077 : KnifeController
		{
			return BaseKnifeController._E001<_E077>(player, knife);
		}

		internal new static Task<_E077> _E001<_E077>(Player player, KnifeComponent knife) where _E077 : KnifeController
		{
			return BaseKnifeController._E002<_E077>(player, knife);
		}

		public virtual void ExamineWeapon()
		{
			this._E000.ExamineWeapon();
		}

		public virtual bool MakeKnifeKick()
		{
			return this._E000.MakeKnifeKick();
		}

		protected override void OnComboPlanning()
		{
			if (_E038.OnHit != null)
			{
				KnifeCollider knifeCollider = _E038;
				knifeCollider.OnHit = (Action<_E00D>)Delegate.Remove(knifeCollider.OnHit, new Action<_E00D>(base._E008));
			}
			this._E000.OnComboPlanning();
			ComboPlanning?.Invoke();
		}

		public override void OnFireEnd()
		{
			base._E000.OnFireEnd();
			OnAttackEnd?.Invoke();
		}

		public virtual bool MakeAlternativeKick()
		{
			return this._E000.MakeAlternativeKick();
		}

		public virtual void BrakeCombo()
		{
			this._E000.BrakeCombo();
		}

		public virtual void ContinueCombo()
		{
			this._E000.ContinueCombo();
		}

		public override void SetCompassState(bool active)
		{
			if (CanChangeCompassState(active))
			{
				this._E000.SetKnifeCompassState(active);
			}
		}

		public override bool CanRemove()
		{
			return this._E000.CanRemove();
		}

		public override void Spawn(float animationSpeed, Action callback)
		{
			base._E001.SetAnimationSpeed(animationSpeed);
			InitiateOperation<_E006>().Start(callback);
			base._E001.SkipTime(Time.fixedDeltaTime);
		}

		public override void SetInventoryOpened(bool opened)
		{
			if (opened)
			{
				SetCompassState(active: false);
			}
			this._E000.SetInventoryOpened(opened);
			_player.CurrentState.OnInventory(opened);
		}

		public override bool IsInventoryOpen()
		{
			return _objectInHandsAnimator.IsInInventory;
		}

		public override bool CanExecute(_EB72 operation)
		{
			if (!(operation is _EB75 obj))
			{
				return true;
			}
			if (obj.From1 != null && _player._E0DE.IsInAnimatedSlot(obj.Item1))
			{
				return this._E000 is _E001;
			}
			return true;
		}

		public void SetBotParameters()
		{
			_E038.SetBotParameters(Knife.Template.ColliderScaleMultiplier);
		}

		[CompilerGenerated]
		private _E00E _E002()
		{
			return new _E006(this);
		}

		[CompilerGenerated]
		private _E00E _E003()
		{
			return new _E001(this);
		}

		[CompilerGenerated]
		private _E00E _E004()
		{
			return new _E004(this);
		}

		[CompilerGenerated]
		private _E00E _E005()
		{
			return new _E002(this);
		}

		[CompilerGenerated]
		private _E00E _E006()
		{
			return new _E000(this);
		}
	}

	public enum EKickType : byte
	{
		Slash,
		Stab
	}

	public struct _E00D
	{
		public Collider collider;

		public Vector3 point;

		public Vector3 normal;

		public _E00D(RaycastHit hit)
		{
			collider = hit.collider;
			point = hit.point;
			normal = hit.normal;
		}
	}

	internal class QuickKnifeKickController : BaseKnifeController, _E6D3, _E6D1<_EA60>, _E6D0, _E6C7
	{
		public new class _E000 : _E00C<QuickKnifeKickController>
		{
			private new Action m__E000;

			private new Action _E001;

			private new Callback<_E6D1<_EA60>> _E002;

			private bool _E003;

			public _E000(QuickKnifeKickController controller)
				: base(controller)
			{
			}

			public void Start(Action callback)
			{
				m__E000 = callback;
				((BaseKnifeController)base._E002)._E001.SetQuickFire(quickFire: true);
				((BaseKnifeController)base._E002)._E001.SetActiveParam(active: false);
				((BaseKnifeController)base._E002)._E001.SetMeleeSpeed(base._E002._player.Physical.MeleeSpeed);
				base._E002.SetDeflected(deflected: false);
				base._E002.SetMeleeSpeed(base._E002._player.Physical.MeleeSpeed);
			}

			public override void WeaponAppeared()
			{
				m__E000();
			}

			public override void HideWeapon(Action onHidden, bool fastHide)
			{
				onHidden();
				if (!_E38D.DisabledForNow)
				{
					if (_E003)
					{
						onHidden();
					}
					else
					{
						_E001 = onHidden;
					}
				}
			}

			public void SetOnUsedCallback(Callback<_E6D1<_EA60>> callback)
			{
				_E002 = callback;
			}

			public override void OnFireEnd()
			{
				_E003 = true;
				((BaseKnifeController)base._E002)._E001.SetQuickFire(quickFire: false);
				if (base._E002.Destroyed)
				{
					_E001();
				}
				else if (_E002 != null)
				{
					_E002((Result<_E6D1<_EA60>>)(_E6D1<_EA60>)base._E002);
				}
				if (base._E002._E038.OnHit != null)
				{
					KnifeCollider knifeCollider = base._E002._E038;
					knifeCollider.OnHit = (Action<_E00D>)Delegate.Remove(knifeCollider.OnHit, new Action<_E00D>(base._E002._E008));
				}
				base._E002._E038.OnFireEnd();
			}

			public override void OnFire()
			{
				base._E002._player.Physical.ConsumeAsMelee(base._E002.Knife.Template.SecondaryConsumption);
				KnifeCollider knifeCollider = base._E002._E038;
				knifeCollider.OnHit = (Action<_E00D>)Delegate.Combine(knifeCollider.OnHit, new Action<_E00D>(base._E002._E008));
				base._E002._E038.MaxDistance = base._E002.Knife.Template.PrimaryDistance;
				base._E002._E038.OnFire();
			}

			public override void Update(float deltaTime)
			{
				base._E002._E038.ManualUpdate();
				base.Update(deltaTime);
			}
		}

		public new _EA60 Item => (_EA60)base.Knife.Item;

		protected new _E000 _E000 => base.CurrentHandsOperation as _E000;

		protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
		{
			return new Dictionary<Type, OperationFactoryDelegate> { 
			{
				typeof(_E000),
				() => new _E000(this)
			} };
		}

		internal new static _E077 _E000<_E077>(Player player, KnifeComponent knife) where _E077 : QuickKnifeKickController
		{
			return BaseKnifeController._E001<_E077>(player, knife);
		}

		internal new static Task<_E077> _E001<_E077>(Player player, KnifeComponent knife) where _E077 : QuickKnifeKickController
		{
			return BaseKnifeController._E002<_E077>(player, knife);
		}

		public void SetOnUsedCallback(Callback<_E6D1<_EA60>> callback)
		{
			this._E000.SetOnUsedCallback(callback);
		}

		public override void Spawn(float animationSpeed, Action callback)
		{
			base._E001.SetAnimationSpeed(animationSpeed);
			InitiateOperation<_E000>().Start(callback);
			base._E001.SkipTime(Time.fixedDeltaTime);
		}

		public override bool CanExecute(_EB72 operation)
		{
			return false;
		}

		public override bool CanRemove()
		{
			return true;
		}

		[CompilerGenerated]
		private _E00E _E002()
		{
			return new _E000(this);
		}
	}

	internal class MedsController : ItemHandsController, _E6CF, _E6D4, _E6C7
	{
		private new sealed class _E000 : _E00E
		{
			private readonly MedsController _E07E;

			private Action _E005;

			private Callback<_E6D4> _E07F;

			public _E000(MedsController controller)
				: base(controller)
			{
				_E07E = controller;
			}

			public override void Reset()
			{
				_E005 = null;
				_E07F = null;
				base.Reset();
			}

			public void Start(EBodyPart bodyPart, float amount, Action callback)
			{
				Start();
				callback();
				if (_E07E._player.ActiveHealthController != null)
				{
					if (_E07E.Item is _EA72)
					{
						amount = 1f;
					}
					FoodDrinkComponent itemComponent = _E07E.Item.GetItemComponent<FoodDrinkComponent>();
					if (itemComponent != null)
					{
						amount = Mathf.Clamp(amount, 0f, itemComponent.HpPercent / itemComponent.MaxResource);
					}
					if (_E07E._player.ActiveHealthController.DoMedEffect(_E07E.Item, bodyPart, amount) == null)
					{
						State = EOperationState.Finished;
						_E07E.FailedToApply = true;
						Callback<_E6D4> callback2 = _E07F;
						_E07F = null;
						callback2?.Invoke((Result<_E6D4>)(_E6D4)_E07E);
						return;
					}
				}
				_E07E._player.HealthController.EffectRemovedEvent += _E000;
				_E07E.m__E001.SetActiveParam(active: true, resetLeftHand: false);
			}

			public void SetOnUsedCallback(Callback<_E6D4> callback)
			{
				_E07F = callback;
			}

			public Callback<_E6D4> GetOnUsedCallback()
			{
				return _E07F;
			}

			public void Remove()
			{
				_E07E._player.HealthController.CancelApplyingItem();
			}

			private void _E000(_E992 effect)
			{
				if (effect is _E9BF)
				{
					_E07E._player.HealthController.EffectRemovedEvent -= _E000;
					Callback<_E6D4> callback = _E07F;
					_E07F = null;
					callback?.Invoke((Result<_E6D4>)(_E6D4)_E07E);
				}
			}

			public void HideWeapon(Action onHiddenCallback)
			{
				_E07E._player.ActiveHealthController?.RemoveMedEffect();
				_E07E._player.HealthController.EffectRemovedEvent -= _E000;
				_E005 = onHiddenCallback;
				_E07E.m__E001.SetActiveParam(active: false, resetLeftHand: false);
				if (State == EOperationState.Finished)
				{
					_E005?.Invoke();
				}
			}

			public void HideWeaponComplete()
			{
				State = EOperationState.Finished;
				_E07E._player._E03B(_E07E.Item).Confirm();
				_E005?.Invoke();
			}

			public void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					HideWeaponComplete();
				}
			}
		}

		[CompilerGenerated]
		private new sealed class _E002<_E077> where _E077 : MedsController
		{
			public _E077 controller;

			internal void _E000()
			{
				((MedsController)controller).m__E001.RemoveEventsConsumer(controller);
			}
		}

		private float _E03E;

		private int _E03F;

		private EBodyPart _E040;

		private new FirearmsAnimator m__E001;

		[CompilerGenerated]
		private bool _E041;

		public override int AnimationVariant => _E03F;

		private new _E000 _E000 => base.CurrentHandsOperation as _E000;

		public override FirearmsAnimator FirearmsAnimator => this.m__E001;

		public override string LoggerDistinctId => string.Format(_ED3E._E000(163638), _player.ProfileId, _player.Profile.Info.Nickname, this);

		public bool FailedToApply
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

		protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
		{
			return new Dictionary<Type, OperationFactoryDelegate> { 
			{
				typeof(_E000),
				() => new _E000(this)
			} };
		}

		public override void ShowGesture(EGesture gesture)
		{
		}

		public override void Destroy()
		{
			_player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			base.Destroy();
			this.m__E001 = null;
			AssetPoolObject.ReturnToPool(_controllerObject.gameObject);
		}

		public void SetOnUsedCallback(Callback<_E6D4> callback)
		{
			this._E000.SetOnUsedCallback(callback);
		}

		public Callback<_E6D4> GetOnUsedCallback()
		{
			return this._E000.GetOnUsedCallback();
		}

		public void Remove()
		{
			this._E000.Remove();
		}

		public override bool CanExecute(_EB72 operation)
		{
			return true;
		}

		public override void Execute(_EB72 operation, Callback callback)
		{
			callback.Succeed();
		}

		public override void Pickup(bool p)
		{
		}

		public override void Loot(bool p)
		{
		}

		public override void Interact(bool isInteracting, int actionIndex)
		{
		}

		public override bool CanInteract()
		{
			return false;
		}

		public override bool CanRemove()
		{
			return true;
		}

		internal static _E077 _E000<_E077>(Player player, Item item, EBodyPart bodyPart, float amount, int animationVariant) where _E077 : MedsController
		{
			_E077 val = ItemHandsController._E001<_E077>(player, item, Singleton<_E760>.Instance.CreateItemUsablePrefab);
			_E002(val, player, item, animationVariant);
			val._E040 = bodyPart;
			val._E03E = amount;
			return val;
		}

		internal static async Task<_E077> _E001<_E077>(Player player, Item item, EBodyPart bodyPart, float amount, int animationVariant) where _E077 : MedsController
		{
			_E077 obj = await ItemHandsController._E003<_E077>(player, item, Singleton<_E760>.Instance.CreateItemUsablePrefabAsync);
			_E002(obj, player, item, animationVariant);
			obj._E040 = bodyPart;
			obj._E03E = amount;
			return obj;
		}

		private static void _E002<_E077>(_E077 controller, Player player, Item item, int animationVariant) where _E077 : MedsController
		{
			WeaponPrefab component = controller._controllerObject.GetComponent<WeaponPrefab>();
			_E6FB objectInHands = component.ObjectInHands;
			player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			player.ProceduralWeaponAnimation.InitTransforms(controller.HandsHierarchy);
			objectInHands.AfterGetFromPoolInit(player.ProceduralWeaponAnimation, null, player.IsYourPlayer);
			((MedsController)controller).m__E001 = component.FirearmsAnimator;
			((MedsController)controller).m__E001.AddEventsConsumer(controller);
			controller.CompositeDisposable.AddDisposable(delegate
			{
				((MedsController)controller).m__E001.RemoveEventsConsumer(controller);
			});
			((MedsController)controller).m__E001.SkipTime(0.0001f);
			controller._player.HandsAnimator = ((MedsController)controller).m__E001;
			controller._controllerObject.GetComponent<BaseSoundPlayer>().Init(controller, player.PlayerBones.WeaponRoot, player);
			AnimationVariantsComponent itemComponent = item.GetItemComponent<AnimationVariantsComponent>();
			animationVariant = ((itemComponent != null) ? (animationVariant % itemComponent.VariantsNumber) : 0);
			controller._E03F = animationVariant;
			((MedsController)controller).m__E001.SetAnimationVariant(animationVariant);
			((MedsController)controller).m__E001.SetUseTimeMultiplier(1f + (float)player.Skills.SurgerySpeed);
		}

		public override void ManualUpdate(float deltaTime)
		{
			base.ManualUpdate(deltaTime);
			this.m__E001?.SetAimAngle(_player.Pitch);
		}

		protected override void IEventsConsumerOnWeapOut()
		{
			this._E000.HideWeaponComplete();
		}

		protected override void IEventsConsumerOnThirdAction(int IntParam)
		{
			TranslateAnimatorParameter(IntParam);
		}

		public override void Spawn(float animationSpeed, Action callback)
		{
			this.m__E001.SetAnimationSpeed(animationSpeed);
			this.m__E001.SetPointOfViewOnSpawn(_player.PointOfView);
			InitiateOperation<_E000>().Start(_E040, _E03E, callback);
		}

		public override void Drop(float animationSpeed, Action callback, bool fastDrop = false, Item nextControllerItem = null)
		{
			base.Destroyed = true;
			this._E000.HideWeapon(callback);
		}

		public override void FastForwardCurrentState()
		{
			this._E000.FastForward();
		}

		[CompilerGenerated]
		private _E00E _E003()
		{
			return new _E000(this);
		}
	}

	public abstract class AbstractHandsController : MonoBehaviour, _E6C7, _E324
	{
		[CompilerGenerated]
		private Action<bool> _E042;

		[CompilerGenerated]
		private Transform _E043;

		[CompilerGenerated]
		private bool _E044;

		[CompilerGenerated]
		private _E570 _E045;

		protected readonly _E3A4 CompositeDisposable = new _E3A4();

		public Transform WeaponRoot
		{
			[CompilerGenerated]
			get
			{
				return _E043;
			}
			[CompilerGenerated]
			set
			{
				_E043 = value;
			}
		}

		public abstract GameObject ControllerGameObject { get; }

		public bool Destroyed
		{
			[CompilerGenerated]
			get
			{
				return _E044;
			}
			[CompilerGenerated]
			protected set
			{
				_E044 = value;
			}
		}

		public abstract TransformLinks HandsHierarchy { get; }

		public abstract FirearmsAnimator FirearmsAnimator { get; }

		public _E570 AnimationEventsEmitter
		{
			[CompilerGenerated]
			get
			{
				return _E045;
			}
			[CompilerGenerated]
			protected set
			{
				_E045 = value;
			}
		}

		public virtual bool IsAiming
		{
			get
			{
				return false;
			}
			protected set
			{
			}
		}

		public virtual float AimingSensitivity => 1f;

		public virtual string LoggerDistinctId => _ED3E._E000(50105);

		public Item Item => GetItem();

		public virtual int AnimationVariant => 0;

		public event Action<bool> OnAimingChanged
		{
			[CompilerGenerated]
			add
			{
				Action<bool> action = _E042;
				Action<bool> action2;
				do
				{
					action2 = action;
					Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref _E042, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<bool> action = _E042;
				Action<bool> action2;
				do
				{
					action2 = action;
					Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref _E042, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public virtual void ManualLateUpdate(float deltaTime)
		{
		}

		public virtual void Destroy()
		{
			CompositeDisposable.Dispose();
		}

		public abstract bool CanExecute(_EB72 operation);

		public abstract void Execute(_EB72 operation, Callback callback);

		public abstract bool CanRemove();

		public virtual bool IsHandsProcessing()
		{
			return false;
		}

		public virtual bool IsPlacingBeacon()
		{
			return false;
		}

		public virtual bool CanInteract()
		{
			return true;
		}

		public virtual bool InCanNotBeInterruptedOperation()
		{
			return false;
		}

		public abstract void ShowGesture(EGesture gesture);

		public abstract void BlindFire(int b);

		public virtual bool IsInInteraction()
		{
			return false;
		}

		public virtual bool IsInInteractionStrictCheck()
		{
			return IsInInteraction();
		}

		public virtual float GetAnimatorFloatParam(int hash)
		{
			return 0f;
		}

		public virtual bool SupportPickup()
		{
			return false;
		}

		public virtual void Pickup(bool p)
		{
			throw new NotImplementedException();
		}

		public virtual void Interact(bool isInteracting, int actionIndex)
		{
			throw new NotImplementedException();
		}

		public virtual void Loot(bool p)
		{
			throw new NotImplementedException();
		}

		public virtual void SetInventoryOpened(bool opened)
		{
		}

		public virtual bool IsInventoryOpen()
		{
			return false;
		}

		public virtual void OnPlayerDead()
		{
		}

		public virtual void OnGameSessionEnd()
		{
		}

		protected abstract Item GetItem();

		public abstract void ManualUpdate(float deltaTime);

		public abstract void BallisticUpdate(float deltaTime);

		public abstract void EmitEvents();

		public abstract void Spawn(float animationSpeed, Action callback);

		public abstract void Drop(float animationSpeed, Action callback, bool fastDrop, Item nextControllerItem = null);

		public virtual void FastForwardCurrentState()
		{
		}

		protected void AimingChanged(bool newValue)
		{
			_E042?.Invoke(newValue);
		}

		void _E324.OnAddAmmoInChamber()
		{
			IEventsConsumerOnAddAmmoInChamber();
		}

		protected virtual void IEventsConsumerOnAddAmmoInChamber()
		{
		}

		void _E324.OnAddAmmoInMag()
		{
			IEventsConsumerOnAddAmmoInMag();
		}

		protected virtual void IEventsConsumerOnAddAmmoInMag()
		{
		}

		void _E324.OnArm()
		{
			IEventsConsumerOnArm();
		}

		protected virtual void IEventsConsumerOnArm()
		{
		}

		void _E324.OnCook()
		{
			IEventsConsumerOnCook();
		}

		protected virtual void IEventsConsumerOnCook()
		{
		}

		void _E324.OnDelAmmoChamber()
		{
			IEventsConsumerOnDelAmmoChamber();
		}

		protected virtual void IEventsConsumerOnDelAmmoChamber()
		{
		}

		void _E324.OnDelAmmoFromMag()
		{
			IEventsConsumerOnDelAmmoFromMag();
		}

		protected virtual void IEventsConsumerOnDelAmmoFromMag()
		{
		}

		void _E324.OnDisarm()
		{
			IEventsConsumerOnDisarm();
		}

		protected virtual void IEventsConsumerOnDisarm()
		{
		}

		void _E324.OnFireEnd()
		{
			IEventsConsumerOnFireEnd();
		}

		protected virtual void IEventsConsumerOnFireEnd()
		{
		}

		void _E324.OnComboPlanning()
		{
			IEventsConsumerOnComboPlanning();
		}

		protected virtual void IEventsConsumerOnComboPlanning()
		{
		}

		void _E324.OnFiringBullet()
		{
			IEventsConsumerOnFiringBullet();
		}

		protected virtual void IEventsConsumerOnFiringBullet()
		{
		}

		void _E324.OnFoldOff()
		{
			IEventsConsumerOnFoldOff();
		}

		protected virtual void IEventsConsumerOnFoldOff()
		{
		}

		void _E324.OnFoldOn()
		{
			IEventsConsumerOnFoldOn();
		}

		protected virtual void IEventsConsumerOnFoldOn()
		{
		}

		void _E324.OnIdleStart()
		{
			IEventsConsumerOnIdleStart();
		}

		protected virtual void IEventsConsumerOnIdleStart()
		{
		}

		void _E324.OnLauncherAppeared()
		{
			IEventsConsumerOnLauncherAppeared();
		}

		protected virtual void IEventsConsumerOnLauncherAppeared()
		{
		}

		void _E324.OnLauncherDisappeared()
		{
			IEventsConsumerOnLauncherDisappeared();
		}

		protected virtual void IEventsConsumerOnLauncherDisappeared()
		{
		}

		void _E324.OnMagHide()
		{
			IEventsConsumerOnMagHide();
		}

		protected virtual void IEventsConsumerOnMagHide()
		{
		}

		void _E324.OnMagIn()
		{
			IEventsConsumerOnMagIn();
		}

		protected virtual void IEventsConsumerOnMagIn()
		{
		}

		void _E324.OnMagOut()
		{
			IEventsConsumerOnMagOut();
		}

		protected virtual void IEventsConsumerOnMagOut()
		{
		}

		void _E324.OnMagShow()
		{
			IEventsConsumerOnMagShow();
		}

		protected virtual void IEventsConsumerOnMagShow()
		{
		}

		void _E324.OnMessageName()
		{
			IEventsConsumerOnMessageName();
		}

		protected virtual void IEventsConsumerOnMessageName()
		{
		}

		void _E324.OnMalfunctionOff()
		{
			IEventsConsumerOnMalfunctionOff();
		}

		protected virtual void IEventsConsumerOnMalfunctionOff()
		{
		}

		void _E324.OnModChanged()
		{
			IEventsConsumerOnModChanged();
		}

		protected virtual void IEventsConsumerOnModChanged()
		{
		}

		void _E324.OnOffBoltCatch()
		{
			IEventsConsumerOnOffBoltCatch();
		}

		protected virtual void IEventsConsumerOnOffBoltCatch()
		{
		}

		void _E324.OnOnBoltCatch()
		{
			IEventsConsumerOnOnBoltCatch();
		}

		protected virtual void IEventsConsumerOnOnBoltCatch()
		{
		}

		void _E324.OnPutMagToRig()
		{
			IEventsConsumerOnPutMagToRig();
		}

		protected virtual void IEventsConsumerOnPutMagToRig()
		{
		}

		void _E324.OnRemoveShell()
		{
			IEventsConsumerOnRemoveShell();
		}

		protected virtual void IEventsConsumerOnRemoveShell()
		{
		}

		void _E324.OnReplaceSecondMag()
		{
			IEventsConsumerOnReplaceSecondMag();
		}

		protected virtual void IEventsConsumerOnReplaceSecondMag()
		{
		}

		void _E324.OnShellEject()
		{
			IEventsConsumerOnShellEject();
		}

		protected virtual void IEventsConsumerOnShellEject()
		{
		}

		void _E324.OnShowAmmo(bool BoolParam)
		{
			IEventsConsumerOnShowAmmo(BoolParam);
		}

		protected virtual void IEventsConsumerOnShowAmmo(bool BoolParam)
		{
		}

		void _E324.OnShowMag()
		{
			IEventsConsumerOnShowMag();
		}

		void _E324.OnSliderOut()
		{
		}

		protected virtual void IEventsConsumerOnShowMag()
		{
		}

		void _E324.OnSound(string StringParam)
		{
			IEventsConsumerOnSound(StringParam);
		}

		protected virtual void IEventsConsumerOnSound(string StringParam)
		{
		}

		public void OnSoundAtPoint(string StringParam)
		{
			IEventsConsumerOnSoundAtPoint(StringParam);
		}

		protected virtual void IEventsConsumerOnSoundAtPoint(string StringParam)
		{
		}

		void _E324.OnStartUtilityOperation()
		{
			IEventsConsumerOnStartUtilityOperation();
		}

		protected virtual void IEventsConsumerOnStartUtilityOperation()
		{
		}

		void _E324.OnThirdAction(int IntParam)
		{
			IEventsConsumerOnThirdAction(IntParam);
		}

		protected virtual void IEventsConsumerOnThirdAction(int IntParam)
		{
		}

		public void OnUseProp(bool BoolParam)
		{
			IEventsConsumerOnOnUseProp(BoolParam);
		}

		protected virtual void IEventsConsumerOnOnUseProp(bool BoolParam)
		{
		}

		void _E324.OnUseSecondMagForReload()
		{
			IEventsConsumerOnUseSecondMagForReload();
		}

		protected virtual void IEventsConsumerOnUseSecondMagForReload()
		{
		}

		void _E324.OnWeapIn()
		{
			IEventsConsumerOnWeapIn();
		}

		protected virtual void IEventsConsumerOnWeapIn()
		{
		}

		void _E324.OnWeapOut()
		{
			IEventsConsumerOnWeapOut();
		}

		protected virtual void IEventsConsumerOnWeapOut()
		{
		}

		public void OnBackpackDrop()
		{
			IEventsOnBackpackDrop();
		}

		protected virtual void IEventsOnBackpackDrop()
		{
		}

		void _E324.OnCurrentAnimStateEnded()
		{
		}

		void _E324.OnSetActiveObject(int objectID)
		{
		}

		void _E324.OnDeactivateObject(int objectID)
		{
		}
	}

	public abstract class _E00E
	{
		protected class HandsControllerOperationLogger : _E315
		{
			private AbstractHandsController _E017;

			private readonly _E00E _E018;

			public HandsControllerOperationLogger(LoggerMode loggerMode, _E00E objectInHandsOperation)
				: base(_ED3E._E000(155974), loggerMode)
			{
				_E018 = objectInHandsOperation;
			}

			public void SetHandsController(AbstractHandsController controller)
			{
				_E017 = controller;
			}

			public void TraceProhibitedCall()
			{
				if (IsEnabled(NLog.LogLevel.Trace))
				{
					StackFrame stackFrame = new StackFrame(2);
					Log(_ED3E._E000(156341), _ED3E._E000(156362), NLog.LogLevel.Debug, Time.frameCount, _E018.GetType().Name, stackFrame.GetMethod().Name, _E017.LoggerDistinctId);
				}
			}

			[Conditional("UNITY_EDITOR")]
			public void TraceMethodCall()
			{
				if (IsEnabled(NLog.LogLevel.Trace))
				{
					StackFrame stackFrame = new StackFrame(2);
					LogTrace(_ED3E._E000(156420), _E018.GetType().Name, stackFrame.GetMethod().Name);
				}
			}

			public void OperationStart()
			{
				if (IsEnabled(NLog.LogLevel.Trace))
				{
					Log(_ED3E._E000(156458), _ED3E._E000(156497), NLog.LogLevel.Trace, Time.frameCount, _E018.GetType().Name, _E017.LoggerDistinctId);
				}
			}
		}

		private readonly HandsControllerOperationLogger _E080;

		[CompilerGenerated]
		private EOperationState _E081;

		public virtual EOperationState State
		{
			[CompilerGenerated]
			get
			{
				return _E081;
			}
			[CompilerGenerated]
			protected set
			{
				_E081 = value;
			}
		}

		protected _E00E(AbstractHandsController handsController)
		{
			_E080 = new HandsControllerOperationLogger(LoggerMode.Add, this);
			UpdateLoggerController(handsController);
		}

		public void UpdateLoggerController(AbstractHandsController handsController)
		{
			_E080.SetHandsController(handsController);
		}

		protected void Start()
		{
			_E080.OperationStart();
			State = EOperationState.Executing;
		}

		public virtual void Reset()
		{
			State = EOperationState.Ready;
		}

		public virtual void Update(float deltaTime)
		{
		}

		protected void _E000()
		{
			_E080.TraceProhibitedCall();
		}

		[Conditional("UNITY_EDITOR")]
		protected void _E001()
		{
		}

		public virtual void OnEnd()
		{
		}
	}

	internal class UsableItemController : ItemHandsController, _E6CE, _E6CA, _E6C7, _E6C8
	{
		internal new abstract class _E000 : _E00E, _E00F
		{
			protected readonly UsableItemController _E002;

			protected new readonly Player _E001;

			protected _E000(UsableItemController controller)
				: base(controller)
			{
				_E002 = controller;
				_E001 = _E002._player;
			}

			public virtual bool CanRemove()
			{
				return false;
			}

			public virtual void HideWeaponComplete()
			{
				_E000();
			}

			public virtual void WeaponAppeared()
			{
				_E000();
			}

			public virtual void ExamineWeapon()
			{
				_E000();
			}

			public virtual void OnBackpackDrop()
			{
				_E000();
			}

			public virtual void SetInventoryOpened(bool opened)
			{
				_E000();
			}

			public virtual void Execute(_EB72 operation, Callback callback)
			{
				_E000();
				if (!(operation is _EB75 obj))
				{
					callback.Succeed();
				}
				else if (obj.From1 != null && _E002._player._E0DE.IsInAnimatedSlot(obj.Item1))
				{
					callback.Fail(string.Format(_ED3E._E000(163626), GetType()));
				}
				else
				{
					callback.Succeed();
				}
			}

			public virtual void HideWeapon(Action onHidden, bool fastDrop)
			{
				_E000();
			}

			public virtual void SetAiming(bool isAiming)
			{
				_E000();
			}

			public virtual void FastForward()
			{
				_E000();
			}

			public virtual void OnAimingDisabled()
			{
				_E000();
			}

			public virtual void SetCompassState(bool active)
			{
				_E000();
			}

			public virtual void OnIdleStart()
			{
			}
		}

		internal new class _E001 : _E000
		{
			private new Callback _E000;

			public _E001(UsableItemController controller)
				: base(controller)
			{
			}

			public virtual void Start(Item item, Callback callback)
			{
				_E000 = callback;
				Start();
				_E002.SetAim(value: false);
				_E002.FirearmsAnimator.SetInventory(open: false);
				_E002._E001.Animator.SetLayerWeight(2, 1f);
				_E002._E001.SetInteract(p: true, 300);
			}

			public override void Reset()
			{
				_E000 = null;
				base.Reset();
			}

			public override void OnBackpackDrop()
			{
				State = EOperationState.Finished;
				_E002._E001.SetInteract(p: false, 300);
				_E326.ResetTriggerHandReady(_E002.FirearmsAnimator.Animator);
				_E002._E001.SetInventory(_E002.m__E002);
				_E006();
				_E000.Succeed();
			}

			public override void SetInventoryOpened(bool opened)
			{
				_E002.m__E002 = opened;
			}

			public override void SetAiming(bool isAiming)
			{
				if (!isAiming || EFTHardSettings.Instance.CanAimInState(_E001.CurrentState.Name))
				{
					_E002.FirearmsAnimator.SetFire(isAiming);
					_E002.IsAiming = isAiming;
				}
			}

			protected virtual void _E006()
			{
				_E002.InitiateOperation<_E003>().Start();
			}
		}

		public new class _E002 : _E001
		{
			protected new const float _E006 = 0.25f;

			protected float _E007;

			protected bool _E008;

			public _E002(UsableItemController controller)
				: base(controller)
			{
			}

			public override void Start(Item item, Callback callback)
			{
				_E007 = 0f;
				_E008 = false;
				base.Start(item, callback);
			}

			public override void FastForward()
			{
				if (!_E008)
				{
					_E008 = true;
					OnBackpackDrop();
				}
			}

			public override void Update(float deltaTime)
			{
				base.Update(deltaTime);
				if (!_E008 && _E007 > 0.25f)
				{
					_E008 = true;
					OnBackpackDrop();
				}
				_E007 += deltaTime;
			}
		}

		internal new class _E003 : _E000
		{
			private const float m__E003 = 300f;

			private float _E004;

			public _E003(UsableItemController controller)
				: base(controller)
			{
			}

			public new void Start()
			{
				base.Start();
				_E004 = 0f;
			}

			public override void Reset()
			{
				_E004 = 0f;
				base.Reset();
			}

			public override void HideWeapon(Action onHidden, bool fastDrop)
			{
				State = EOperationState.Finished;
				_E002.InitiateOperation<_E004>().Start(onHidden, fastDrop);
				_E002.Hide();
			}

			public override bool CanRemove()
			{
				return true;
			}

			public override void Execute(_EB72 operation, Callback callback)
			{
				if (!(operation is _EB75 obj))
				{
					callback.Succeed();
				}
				else if (obj.From1 != null && _E001._inventoryController.IsInAnimatedSlot(obj.Item1))
				{
					State = EOperationState.Finished;
					_E007(obj, callback);
				}
				else
				{
					callback.Succeed();
				}
			}

			public override void Update(float deltaTime)
			{
				_E004 += deltaTime;
				if (_E004 > 300f)
				{
					_E002._E001.Idle();
					_E004 = 0f;
				}
			}

			public override void SetAiming(bool isAiming)
			{
				if ((!isAiming || EFTHardSettings.Instance.CanAimInState(_E001.CurrentState.Name)) && (!isAiming || !(_E002._E010 > EFTHardSettings.Instance.STOP_AIMING_AT)))
				{
					_E002.FirearmsAnimator.SetFire(isAiming);
					_E002.IsAiming = isAiming;
					_E004 = 0f;
				}
			}

			public override void ExamineWeapon()
			{
				_E002._E001.LookTrigger();
			}

			public override void OnAimingDisabled()
			{
				SetAiming(isAiming: false);
			}

			public override void SetInventoryOpened(bool opened)
			{
				SetAiming(isAiming: false);
				_E002.m__E002 = opened;
				_E002._E001.SetInventory(opened);
			}

			public override void OnEnd()
			{
				SetCompassState(active: false);
			}

			public override void SetCompassState(bool active)
			{
				_E002.CompassState.Value = active;
			}

			protected virtual void _E007(_EB75 oneItemOperation, Callback callback)
			{
				_E002.InitiateOperation<_E001>().Start(oneItemOperation.Item1, callback);
			}
		}

		internal class _E004 : _E000
		{
			protected Action _E005;

			public _E004(UsableItemController controller)
				: base(controller)
			{
			}

			public virtual void Start(Action onHidden, bool fastDrop)
			{
				_E005 = onHidden;
				Start();
				_E002._E001.SetActiveParam(active: false);
				_E002._E001.SetFastHide(fastDrop);
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
				_E002.IsAiming = false;
			}

			public override void Reset()
			{
				_E005 = null;
				base.Reset();
			}

			public override void HideWeaponComplete()
			{
				State = EOperationState.Finished;
				_E005?.Invoke();
			}

			public override void HideWeapon(Action onHidden, bool fastDrop)
			{
				_E005 = (Action)Delegate.Combine(_E005, onHidden);
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					HideWeaponComplete();
				}
			}
		}

		internal class _E005 : _E000
		{
			protected Action _E009;

			protected Action _E00B;

			protected bool _E03E;

			public _E005(UsableItemController controller)
				: base(controller)
			{
			}

			public void Start(Action callback)
			{
				_E00B = callback;
				Start();
				_E002._E001.SetActiveParam(active: true);
				_E002._E001.SetMeleeSpeed(_E001.Physical.MeleeSpeed);
				_E001.BodyAnimatorCommon.SetFloat(_E712.WEAPON_SIZE_MODIFIER_PARAM_HASH, 1f);
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}

			public override void Reset()
			{
				base.Reset();
				_E00B = null;
				this._E009 = null;
			}

			public override void WeaponAppeared()
			{
				_E002.SetupProp();
				State = EOperationState.Finished;
				_E002._player.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 0f);
				_E009();
			}

			public override void HideWeapon(Action onHidden, bool fastDrop)
			{
				this._E009 = onHidden;
				_E03E = fastDrop;
			}

			public override void FastForward()
			{
				if (State != EOperationState.Finished)
				{
					WeaponAppeared();
				}
			}

			protected virtual void _E009()
			{
				_E003 obj = _E002.InitiateOperation<_E003>();
				obj.Start();
				_E00B();
				if (this._E009 != null)
				{
					obj.HideWeapon(this._E009, _E03E);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _E007
		{
			public UsableItemController _003C_003E4__this;

			public _E013 inventoryOperation;

			public Action callback;

			internal void _E000()
			{
				_003C_003E4__this._player.MovementContext.OnStateChanged -= _003C_003E4__this._E015;
				_003C_003E4__this._player.Physical.OnSprintStateChangedEvent -= _003C_003E4__this._E007;
				_003C_003E4__this._player.ProceduralWeaponAnimation.enabled = true;
				inventoryOperation.Confirm();
				callback();
			}
		}

		private const float _E046 = 0.5f;

		private static readonly RaycastHit[] m__E00A = new RaycastHit[8];

		private int _E017;

		private new bool m__E002;

		private bool m__E015;

		protected float _E010;

		protected _E6FB _E036;

		protected new FirearmsAnimator _E001;

		protected Func<RaycastHit, bool> _E01A;

		protected bool _E047;

		protected float _E048;

		protected bool _E049;

		protected new _E00F _E000 => base.CurrentHandsOperation as _E00F;

		public override FirearmsAnimator FirearmsAnimator => this._E001;

		public override string LoggerDistinctId => string.Format(_ED3E._E000(163638), _player.ProfileId, _player.Profile.Info.Nickname, this);

		public override bool IsAiming
		{
			get
			{
				return _E049;
			}
			protected set
			{
				if (!value)
				{
					_player.Physical.HoldBreath(enable: false);
				}
				if (_E049 != value)
				{
					_E049 = value;
					_player.Skills.FastAimTimer.Target = (value ? 0f : 2f);
					_player.MovementContext.SetAimingSlowdown(IsAiming, 0.33f);
					_player.Physical.Aim(_E049 ? 1 : 0);
					AimingChanged(value);
					_player.ProceduralWeaponAnimation.IsAiming = _E049;
				}
			}
		}

		protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
		{
			return new Dictionary<Type, OperationFactoryDelegate>
			{
				{
					typeof(_E005),
					() => new _E005(this)
				},
				{
					typeof(_E003),
					() => new _E003(this)
				},
				{
					typeof(_E004),
					() => new _E004(this)
				},
				{
					typeof(_E001),
					() => new _E001(this)
				}
			};
		}

		internal new static _E077 _E000<_E077>(Player player, Item item) where _E077 : UsableItemController
		{
			_E077 val = ItemHandsController._E000<_E077>(player, item);
			_E002(val, player);
			return val;
		}

		internal static async Task<_E077> _E001<_E077>(Player player, Item item) where _E077 : UsableItemController
		{
			_E077 obj = await ItemHandsController._E002<_E077>(player, item);
			_E002(obj, player);
			return obj;
		}

		private static void _E002<_E077>(_E077 controller, Player player) where _E077 : UsableItemController
		{
			WeaponPrefab weaponPrefab = _E003(controller);
			controller._E013(player, weaponPrefab);
		}

		private static WeaponPrefab _E003<_E077>(_E077 controller) where _E077 : UsableItemController
		{
			return controller._controllerObject.GetComponentInChildren<WeaponPrefab>();
		}

		protected virtual void _E013(Player player, WeaponPrefab weaponPrefab)
		{
			_E01A = _E00B;
			_E036 = weaponPrefab.ObjectInHands;
			this._E001 = weaponPrefab.FirearmsAnimator;
			this._E001.AddEventsConsumer(this);
			CompositeDisposable.AddDisposable(delegate
			{
				this._E001.RemoveEventsConsumer(this);
			});
			_player.HandsAnimator = this._E001;
			player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			player.ProceduralWeaponAnimation.InitTransforms(HandsHierarchy);
			player.ProceduralWeaponAnimation._E008(weaponPrefab);
			player.ProceduralWeaponAnimation.FindAimTransformsWithoutSights();
			player.ProceduralWeaponAnimation.ResetScopeRotation();
		}

		protected override void IEventsConsumerOnWeapIn()
		{
			_E005();
		}

		protected override void IEventsConsumerOnWeapOut()
		{
			_E004();
		}

		protected override void IEventsConsumerOnThirdAction(int intParam)
		{
			TranslateAnimatorParameter(intParam);
		}

		protected override void IEventsOnBackpackDrop()
		{
			_E006();
		}

		protected override void IEventsConsumerOnIdleStart()
		{
			this._E000.OnIdleStart();
		}

		public override void ManualLateUpdate(float deltaTime)
		{
			if ((!_E2B6.Config.UseSpiritPlayer || !_player.Spirit.IsActive) && this.m__E015)
			{
				_E00C();
				this.m__E015 = false;
			}
		}

		public override void ManualUpdate(float deltaTime)
		{
			base.ManualUpdate(deltaTime);
			this.m__E015 = true;
		}

		protected override void IEventsConsumerOnOnUseProp(bool boolParam)
		{
			SetPropVisibility(boolParam);
		}

		public override bool SupportPickup()
		{
			return true;
		}

		public override bool IsInventoryOpen()
		{
			return this.m__E002;
		}

		public override void SetInventoryOpened(bool opened)
		{
			if (opened)
			{
				SetCompassState(active: false);
			}
			this._E000.SetInventoryOpened(opened);
			_player.CurrentState.OnInventory(opened);
		}

		public override void Pickup(bool p)
		{
			if (CanInteract())
			{
				this._E001.SetPickup(p);
			}
		}

		public override void Interact(bool isInteracting, int actionIndex)
		{
			if (CanInteract())
			{
				this._E001.SetInteract(isInteracting, actionIndex);
			}
		}

		public override void Loot(bool p)
		{
			if (CanInteract())
			{
				this._E001.SetLooting(p);
			}
		}

		public override bool CanRemove()
		{
			return true;
		}

		public override bool CanInteract()
		{
			if (this._E001.IsIdling())
			{
				return this._E001.Animator.GetBool(_E326.BOOL_ACTIVE);
			}
			return false;
		}

		public override void ShowGesture(EGesture gesture)
		{
			SetAim(value: false);
			if (gesture != 0)
			{
				this._E001.Gesture(gesture);
			}
		}

		public virtual bool ExamineWeapon()
		{
			if (!(this._E000 is _E003) || _player._E0DE.HasAnyHandsAction())
			{
				return false;
			}
			this._E000.ExamineWeapon();
			return true;
		}

		public override bool IsInInteraction()
		{
			return this._E001.IsInInteraction;
		}

		public virtual void ToggleAim()
		{
			SetCompassState(active: false);
			SetAim(!IsAiming);
		}

		public virtual void SetAim(bool value)
		{
			bool isAiming = IsAiming;
			this._E000.SetAiming(value);
			_player._E0DB &= !value;
			if (isAiming != IsAiming)
			{
				float volume = 1f - (float)_player.Skills.DrawSound;
				_player._E028(volume);
			}
		}

		public virtual void Hide()
		{
		}

		public override bool IsInInteractionStrictCheck()
		{
			if (!IsInInteraction() && !(this._E001.GetLayerWeight(this._E001.LACTIONS_LAYER_INDEX) >= float.Epsilon))
			{
				return this._E001.Animator.IsInTransition(this._E001.LACTIONS_LAYER_INDEX);
			}
			return true;
		}

		public override void Spawn(float animationSpeed, Action callback)
		{
			this._E001.SetAnimationSpeed(animationSpeed);
			_E014(callback);
			this._E001.SkipTime(Time.fixedDeltaTime);
			_player.MovementContext.OnStateChanged += _E015;
			_player.Physical.OnSprintStateChangedEvent += _E007;
		}

		public override void Drop(float animationSpeed, Action callback, bool fastDrop, Item nextControllerItem = null)
		{
			if (base.Destroyed)
			{
				this._E000.HideWeapon(callback, fastDrop);
				return;
			}
			base.Destroyed = true;
			_E013 inventoryOperation = _player._E03B(base.Item);
			Action onHidden = delegate
			{
				_player.MovementContext.OnStateChanged -= _E015;
				_player.Physical.OnSprintStateChangedEvent -= _E007;
				_player.ProceduralWeaponAnimation.enabled = true;
				inventoryOperation.Confirm();
				callback();
			};
			this._E000.HideWeapon(onHidden, fastDrop);
		}

		public override void Destroy()
		{
			_player.ProceduralWeaponAnimation.ClearPreviousWeapon();
			base.Destroy();
			this._E001 = null;
			_player.MovementContext.OnStateChanged -= _E015;
			_player.Physical.OnSprintStateChangedEvent -= _E007;
			AssetPoolObject.ReturnToPool(_controllerObject.gameObject);
		}

		protected virtual void _E014(Action callback)
		{
			InitiateOperation<_E005>().Start(callback);
		}

		private void _E004()
		{
			this._E000.HideWeaponComplete();
		}

		private void _E005()
		{
			this._E000.WeaponAppeared();
		}

		private void _E006()
		{
			this._E000.OnBackpackDrop();
		}

		private void _E007(bool obj)
		{
			if (IsAiming && obj)
			{
				_E008();
			}
		}

		protected virtual void _E015(EPlayerState previousstate, EPlayerState nextstate)
		{
			if (!EFTHardSettings.Instance.CanAimInState(nextstate))
			{
				_E008();
			}
		}

		private void _E008()
		{
			this._E000.OnAimingDisabled();
		}

		public override bool CanExecute(_EB72 operation)
		{
			return true;
		}

		public override void Execute(_EB72 operation, Callback callback)
		{
			this._E000.Execute(operation, callback);
		}

		public override void FastForwardCurrentState()
		{
			this._E000.FastForward();
		}

		protected void _E009()
		{
			this._E010 = 0f;
			_E048 = 0f;
			if (!(_player.MovementContext.StationaryWeapon != null) || _player.MovementContext.StationaryWeapon.Item != base.Item)
			{
				if (base.WeaponRoot == null)
				{
					UnityEngine.Debug.LogError(_ED3E._E000(163700));
					return;
				}
				_E048 = 0.5f;
				_E017 = LayerMask.NameToLayer(_ED3E._E000(60679));
			}
		}

		private float _E00A(Vector3 origin, float ln, ref bool overlapsWithPlayer)
		{
			Vector3 end = origin - base.WeaponRoot.up * ln;
			if (_E320.Linecast(origin, end, out var bestHit, EFTHardSettings.Instance.WEAPON_OCCLUSION_LAYERS, reverseCheck: false, UsableItemController.m__E00A, _E01A))
			{
				overlapsWithPlayer = bestHit.collider.gameObject.layer == _E017;
				return ln - bestHit.distance;
			}
			Vector3 lhs = origin - _player.Position;
			Vector3 up = Vector3.up;
			float num = Vector3.Dot(lhs, up);
			if (_E320.Linecast(_player.Position + num * up, origin, out bestHit, EFTHardSettings.Instance.WEAPON_OCCLUSION_LAYERS, reverseCheck: false, UsableItemController.m__E00A, _E01A))
			{
				overlapsWithPlayer = bestHit.collider.gameObject.layer == _E017;
				return ln;
			}
			return 0f;
		}

		protected bool _E00B(RaycastHit overlapHit)
		{
			GameObject gameObject = overlapHit.collider.gameObject;
			if (gameObject.layer == _E017)
			{
				return gameObject == _player.gameObject;
			}
			return false;
		}

		private void _E00C()
		{
			if (_player.IsVisible && !(_E048 <= 0f) && !(this._E000 is _E004))
			{
				Vector3 position = _player.ProceduralWeaponAnimation.HandsContainer.HandsPosition.Get();
				float num = 1f;
				if (_player.ProceduralWeaponAnimation.BlindfireBlender.Value != 0f)
				{
					Vector3 position2 = (_player.ProceduralWeaponAnimation.BlindFireEndPosition + _player.ProceduralWeaponAnimation.PositionZeroSum) * 1.9f;
					position2 = _player.ProceduralWeaponAnimation.HandsContainer.WeaponRootAnim.parent.TransformPoint(position2);
					num = _E00A(position2, _E048, ref _player.ProceduralWeaponAnimation.TurnAway.OverlapsWithPlayer);
				}
				if (num < 0.02f)
				{
					_player.ProceduralWeaponAnimation.TurnAway.OverlapDepth = num;
					_player.ProceduralWeaponAnimation.OverlappingAllowsBlindfire = true;
				}
				else
				{
					_player.ProceduralWeaponAnimation.OverlappingAllowsBlindfire = false;
					_player.ProceduralWeaponAnimation.TurnAway.OriginZShift = position.y;
					position = _player.ProceduralWeaponAnimation.HandsContainer.WeaponRootAnim.parent.TransformPoint(position);
					num = _E00A(position, _E048, ref _player.ProceduralWeaponAnimation.TurnAway.OverlapsWithPlayer);
					_player.ProceduralWeaponAnimation.TurnAway.OverlapDepth = num;
				}
				this._E010 = num;
				if (num > EFTHardSettings.Instance.STOP_AIMING_AT && IsAiming)
				{
					ToggleAim();
					_E047 = true;
				}
				else if (num < EFTHardSettings.Instance.STOP_AIMING_AT && _player.ProceduralWeaponAnimation.TurnAway.OverlapValue < 0.2f && _E047 && !IsAiming)
				{
					ToggleAim();
					_E047 = false;
				}
			}
		}

		[CompilerGenerated]
		private _E00E _E00D()
		{
			return new _E005(this);
		}

		[CompilerGenerated]
		private _E00E _E00E()
		{
			return new _E003(this);
		}

		[CompilerGenerated]
		private _E00E _E00F()
		{
			return new _E004(this);
		}

		[CompilerGenerated]
		private _E00E _E010()
		{
			return new _E001(this);
		}

		[CompilerGenerated]
		private void _E011()
		{
			this._E001.RemoveEventsConsumer(this);
		}
	}

	internal interface _E00F
	{
		void HideWeaponComplete();

		void WeaponAppeared();

		void HideWeapon(Action onHidden, bool fastDrop);

		void OnBackpackDrop();

		void SetAiming(bool isAiming);

		void Execute(_EB72 operation, Callback callback);

		void ExamineWeapon();

		void FastForward();

		void OnIdleStart();

		void OnAimingDisabled();

		void SetInventoryOpened(bool opened);
	}

	public enum EVoipState : byte
	{
		NotAvailable,
		Available,
		Off,
		Banned,
		MicrophoneFail
	}

	public enum EProcessStatus
	{
		None,
		Scheduled,
		Internal
	}

	public class _E010 : _EB2D, _E6A1
	{
		public _EB38 DiscardResult;

		public Player Player;

		public _EADF Grenade;

		public bool LowThrow;

		public _E010(_EB38 discardResult, _EADF grenade, Player player, bool lowThrow)
		{
			DiscardResult = discardResult;
			Grenade = grenade;
			Player = player;
			LowThrow = lowThrow;
		}

		public void RollBack()
		{
			DiscardResult.RollBack();
		}

		public void RaiseEvents(_EB1E controller, CommandStatus status)
		{
			DiscardResult.RaiseEvents(controller, status);
		}

		public _ECD7 Replay()
		{
			return Player.ThrowGrenade(Grenade, LowThrow, simulate: false);
		}

		public bool CanExecute(_EB1E itemController)
		{
			if (!Grenade.CheckAction(Grenade.CurrentAddress))
			{
				return false;
			}
			return true;
		}
	}

	internal abstract class _E011 : IDisposable
	{
		protected enum InternalState
		{
			Creating,
			Executed,
			Confirmed,
			Disposed
		}

		[CompilerGenerated]
		private Player _E000;

		[CompilerGenerated]
		private InternalState _E001;

		[CompilerGenerated]
		private CommandStatus _E002;

		[CompilerGenerated]
		private Item _E003;

		protected Player _E004
		{
			[CompilerGenerated]
			get
			{
				return _E000;
			}
			[CompilerGenerated]
			private set
			{
				_E000 = value;
			}
		}

		protected InternalState _E005
		{
			[CompilerGenerated]
			get
			{
				return _E001;
			}
			[CompilerGenerated]
			set
			{
				_E001 = value;
			}
		}

		public CommandStatus Status
		{
			[CompilerGenerated]
			get
			{
				return _E002;
			}
			[CompilerGenerated]
			protected set
			{
				_E002 = value;
			}
		}

		internal Item _E006
		{
			[CompilerGenerated]
			get
			{
				return _E003;
			}
			[CompilerGenerated]
			private set
			{
				_E003 = value;
			}
		}

		public bool Disposed => _E005 == InternalState.Disposed;

		protected _E011(Player player, Item item)
		{
			_E004 = player;
			_E006 = item;
		}

		~_E011()
		{
			Dispose(disposing: false);
		}

		public void Execute()
		{
			if (_E005 == InternalState.Creating)
			{
				_E005 = InternalState.Executed;
				_E00A();
			}
		}

		public void Confirm(bool succeed = true)
		{
			if (!Disposed && _E005 == InternalState.Executed)
			{
				_E005 = InternalState.Confirmed;
				_E00C(succeed);
			}
		}

		protected abstract void _E00A();

		protected abstract void _E00C(bool succeed);

		public void Dispose()
		{
			if (!Disposed)
			{
				_E005 = InternalState.Disposed;
				if (Status == CommandStatus.Begin)
				{
					Status = CommandStatus.Failed;
				}
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			_E004 = null;
			_E006 = null;
		}
	}

	internal sealed class _E012 : _E011
	{
		public _E012(Player player, Item item)
			: base(player, item)
		{
		}

		protected override void _E00A()
		{
			base._E004._E0DE.RaiseEvent(new _EAFA(base._E006, base.Status));
		}

		protected override void _E00C(bool succeed)
		{
			base.Status = (succeed ? CommandStatus.Succeed : CommandStatus.Failed);
			base._E004._E0DE.RaiseEvent(new _EAFA(base._E006, base.Status));
		}
	}

	private sealed class _E013 : _E011
	{
		public _E013(Player player, Item item)
			: base(player, item)
		{
		}

		protected override void _E00A()
		{
			base._E004._E0DE.RaiseEvent(new _EAFB(base._E006, base.Status));
		}

		protected override void _E00C(bool succeed)
		{
			base.Status = (succeed ? CommandStatus.Succeed : CommandStatus.Failed);
			base._E004._E0DE.RaiseEvent(new _EAFB(base._E006, base.Status));
		}
	}

	protected abstract class AbstractProcess
	{
		internal enum Completion
		{
			Sync,
			Async
		}

		internal enum Confirmation
		{
			Unknown,
			Succeed,
			Failed
		}

		protected static void Execute(AbstractProcess process)
		{
			process.Execute();
		}

		protected abstract void Execute();

		protected static bool TrySkip(AbstractProcess process)
		{
			return process.TrySkip();
		}

		protected abstract bool TrySkip();

		protected abstract void CreateController();

		protected abstract void Skip(string error);

		protected abstract void SkipToNext(string error);

		protected abstract void Begin(string error = null);

		protected abstract void Complete();

		protected abstract void Complete(string error);

		protected abstract void Abort();

		protected abstract void AbortAfterCompletion();

		protected abstract void ExecuteNext();
	}

	protected sealed class Process<TController, TResult> : AbstractProcess where TController : AbstractHandsController, TResult
	{
		[CompilerGenerated]
		private sealed class _E000
		{
			public Process<TController, TResult> _003C_003E4__this;

			public Action execute;

			internal void _E000()
			{
				_003C_003E4__this._E005 = false;
				if (_003C_003E4__this._E002 != null)
				{
					_003C_003E4__this._E000._E039(_003C_003E4__this._E002);
				}
				_003C_003E4__this.Begin();
				if (_003C_003E4__this._E004 == Confirmation.Failed)
				{
					_003C_003E4__this.SkipToNext(_ED3E._E000(159175));
				}
				else
				{
					_003C_003E4__this.CreateController();
				}
			}

			internal void _E001()
			{
				_003C_003E4__this._E000.Logger.LogInfo(_ED3E._E000(159221));
				_003C_003E4__this._E000.DestroyController();
				if (_003C_003E4__this._E000._E0DA == null || !_003C_003E4__this._E005)
				{
					execute();
				}
				else
				{
					_003C_003E4__this.SkipToNext(_ED3E._E000(159242));
				}
			}
		}

		[CompilerGenerated]
		private sealed class _E001
		{
			public Process<TController, TResult> _003C_003E4__this;

			public _E012 setInHandsOperation;

			internal void _E000()
			{
				_003C_003E4__this._E000.Logger.LogDebug(_ED3E._E000(159284), (_003C_003E4__this._E002 != null) ? _003C_003E4__this._E002.ShortName.Localized() : _ED3E._E000(158795), _003C_003E4__this._E004);
				_003C_003E4__this._E00B = true;
				if (setInHandsOperation != null)
				{
					setInHandsOperation.Confirm();
				}
				switch (_003C_003E4__this._E004)
				{
				case Confirmation.Failed:
					_003C_003E4__this.Abort();
					break;
				case Confirmation.Succeed:
					_003C_003E4__this.Complete();
					_003C_003E4__this._E000.Logger.LogInfo(_ED3E._E000(159328));
					_003C_003E4__this.ExecuteNext();
					break;
				case Confirmation.Unknown:
					if (_003C_003E4__this._E003 == Completion.Async)
					{
						_003C_003E4__this.Complete();
					}
					break;
				default:
					throw new ArgumentException(_ED3E._E000(158783));
				}
			}
		}

		private readonly Player m__E000;

		private readonly Func<TController> m__E001;

		[CanBeNull]
		private readonly Item m__E002;

		private readonly Completion _E003;

		private Confirmation _E004;

		private bool _E005;

		private bool _E006;

		private Callback _E007;

		private Callback<TResult> _E008;

		private TController _E009;

		private bool _E00A;

		private bool _E00B = true;

		internal Process(Player player, Func<TController> controllerFactory, [CanBeNull] Item item, bool fastHide = false, Completion completion = Completion.Sync, Confirmation confirmation = Confirmation.Succeed, bool skippable = true)
		{
			this._E000 = player;
			this._E001 = controllerFactory;
			this._E002 = item;
			_E003 = completion;
			_E004 = confirmation;
			_E005 = skippable;
			_E00A = fastHide;
		}

		internal void _E000([CanBeNull] Callback beginCallback, [CanBeNull] Callback<TResult> completeCallback, bool scheduled)
		{
			this._E000.Logger.LogInfo(_ED3E._E000(156572), this._E002);
			_E007 = beginCallback ?? ((Callback)delegate
			{
			});
			_E008 = completeCallback ?? ((Callback<TResult>)delegate
			{
			});
			if (this._E000._handsController != null && this._E000._handsController.Item == this._E002)
			{
				Skip(_ED3E._E000(156557));
				return;
			}
			switch (this._E000.ProcessStatus)
			{
			case EProcessStatus.None:
				this._E000.Logger.LogInfo(_ED3E._E000(156588), this, this._E002, this._E000.ProcessStatus);
				this._E000.ProcessStatus = EProcessStatus.Scheduled;
				Execute();
				break;
			case EProcessStatus.Scheduled:
				if (this._E000._E0DA == null || AbstractProcess.TrySkip(this._E000._E0DA))
				{
					this._E000._E0DA = this;
				}
				else
				{
					Skip(_ED3E._E000(156665));
				}
				break;
			case EProcessStatus.Internal:
				if (scheduled)
				{
					Skip(_ED3E._E000(158736));
				}
				else
				{
					Execute();
				}
				break;
			default:
				throw new ArgumentException(_ED3E._E000(158783));
			}
		}

		protected override void Execute()
		{
			this._E000.Logger.LogInfo(_ED3E._E000(158764), (this._E002 != null) ? this._E002.ShortName.Localized() : _ED3E._E000(158795));
			Action execute = delegate
			{
				_E005 = false;
				if (this._E002 != null)
				{
					this._E000._E039(this._E002);
				}
				Begin();
				if (_E004 == Confirmation.Failed)
				{
					SkipToNext(_ED3E._E000(159175));
				}
				else
				{
					CreateController();
				}
			};
			if (this._E000.HandsController == null)
			{
				execute();
				return;
			}
			if (this._E000.HandsController.Item == this._E002 && !this._E000.HandsController.Destroyed)
			{
				SkipToNext(_ED3E._E000(158844));
				return;
			}
			this._E000.Logger.LogInfo(_ED3E._E000(158833));
			this._E000.DropCurrentController(delegate
			{
				this._E000.Logger.LogInfo(_ED3E._E000(159221));
				this._E000.DestroyController();
				if (this._E000._E0DA == null || !_E005)
				{
					execute();
				}
				else
				{
					SkipToNext(_ED3E._E000(159242));
				}
			}, _E00A, this._E002);
		}

		protected override void CreateController()
		{
			this._E000.Logger.LogInfo(_ED3E._E000(158849), (this._E002 != null) ? this._E002.ShortName.Localized() : _ED3E._E000(158795));
			_E012 setInHandsOperation = ((this._E002 != null) ? this._E000._E03A(this._E002) : null);
			_E009 = this._E001();
			_E00B = false;
			this._E000.SpawnController(_E009, delegate
			{
				this._E000.Logger.LogDebug(_ED3E._E000(159284), (this._E002 != null) ? this._E002.ShortName.Localized() : _ED3E._E000(158795), _E004);
				_E00B = true;
				if (setInHandsOperation != null)
				{
					setInHandsOperation.Confirm();
				}
				switch (_E004)
				{
				case Confirmation.Failed:
					Abort();
					break;
				case Confirmation.Succeed:
					Complete();
					this._E000.Logger.LogInfo(_ED3E._E000(159328));
					ExecuteNext();
					break;
				case Confirmation.Unknown:
					if (_E003 == Completion.Async)
					{
						Complete();
					}
					break;
				default:
					throw new ArgumentException(_ED3E._E000(158783));
				}
			});
		}

		internal void _E001(bool succeed)
		{
			this._E000.Logger.LogInfo(_ED3E._E000(158970) + succeed + _ED3E._E000(158955) + _E00B);
			if (_E004 != 0)
			{
				UnityEngine.Debug.LogWarning(_ED3E._E000(159007));
				return;
			}
			_E004 = (succeed ? Confirmation.Succeed : Confirmation.Failed);
			if ((UnityEngine.Object)_E009 == (UnityEngine.Object)null || !_E00B)
			{
				return;
			}
			if (succeed)
			{
				if (_E003 == Completion.Sync)
				{
					Complete();
				}
				this._E000.Logger.LogInfo(_ED3E._E000(159039));
				ExecuteNext();
			}
			else if (_E003 == Completion.Sync)
			{
				Abort();
			}
			else
			{
				AbortAfterCompletion();
			}
		}

		protected override bool TrySkip()
		{
			if (_E006 || !_E005)
			{
				return false;
			}
			Skip(_ED3E._E000(159023));
			return true;
		}

		protected override void Skip(string error)
		{
			Begin(error);
			Complete(error);
		}

		protected override void SkipToNext(string error)
		{
			Skip(error);
			if (_E004 == Confirmation.Unknown)
			{
				_E004 = Confirmation.Failed;
			}
			this._E000.Logger.LogInfo(_ED3E._E000(159066) + error);
			ExecuteNext();
		}

		protected override void Begin(string error = null)
		{
			_E005 = false;
			if (string.IsNullOrEmpty(error))
			{
				_E007.Succeed();
			}
			else
			{
				_E007.Fail(error);
			}
		}

		protected override void Complete()
		{
			_E006 = true;
			_E008((TResult)_E009);
		}

		protected override void Complete([CanBeNull] string error)
		{
			_E006 = true;
			_E008(new Result<TResult>
			{
				Error = error
			});
		}

		protected override void Abort()
		{
			UnityEngine.Debug.LogError(_ED3E._E000(159044));
			_E6D4 obj = _E009 as _E6D4;
			Callback<_E6D4> callback2 = obj?.GetOnUsedCallback();
			this._E000.DestroyController();
			this._E000._E089 = EProcessStatus.None;
			if (callback2 != null)
			{
				Complete(null);
				callback2(new Result<_E6D4>(obj));
			}
			else
			{
				this._E000.SetEmptyHands(delegate
				{
					Complete(null);
				});
			}
			this._E000.Logger.LogInfo(_ED3E._E000(159094));
			ExecuteNext();
		}

		protected override void AbortAfterCompletion()
		{
			this._E000.DestroyController();
			this._E000.Logger.LogInfo(_ED3E._E000(159086));
			ExecuteNext();
		}

		protected override void ExecuteNext()
		{
			if (this._E000.ProcessStatus == EProcessStatus.Scheduled)
			{
				AbstractProcess abstractProcess = this._E000._E0DA;
				if (abstractProcess != null)
				{
					this._E000.Logger.LogInfo(_ED3E._E000(159125), this, abstractProcess.GetType());
					this._E000._E0DA = null;
					AbstractProcess.Execute(abstractProcess);
				}
				else
				{
					this._E000.Logger.LogInfo(_ED3E._E000(159159), this, this._E002, this._E000.ProcessStatus);
					this._E000.ProcessStatus = EProcessStatus.None;
				}
			}
		}

		[CompilerGenerated]
		private void _E002(Result<_E6C9> callback)
		{
			Complete(null);
		}
	}

	[Flags]
	public enum EAnimatorMask
	{
		Thirdperson = 1,
		Arms = 2,
		Procedural = 4,
		FBBIK = 8,
		IK = 0x10
	}

	protected class PlayerLogger : _E315
	{
		public PlayerLogger(LoggerMode loggerMode)
			: base(_ED3E._E000(159421), loggerMode)
		{
		}
	}

	public class _E014<_E077> where _E077 : class, IItemComponent
	{
		public readonly _ECEC Changed = new _ECEC();

		[CanBeNull]
		protected _E077 _E000;

		private readonly Slot _E001;

		private readonly Func<_E077, Action, Action> _E002;

		private Action _E003;

		[CanBeNull]
		public virtual _E077 Component => GetItemComponent();

		public _E014(Slot slot, Func<_E077, Action, Action> subscriber)
		{
			_E001 = slot;
			_E002 = subscriber;
			Set(GetItemComponent());
		}

		public void Update()
		{
			Set(Component);
		}

		public void Set([CanBeNull] _E077 value)
		{
			if (value != this._E000)
			{
				if (this._E000 != null)
				{
					_E003();
				}
				this._E000 = value;
				if (this._E000 != null)
				{
					_E003 = _E002(this._E000, _E000);
				}
				_E000();
			}
		}

		public _E077 GetItemComponent()
		{
			_EA40 obj = _E001.ContainedItem as _EA40;
			if (obj == null)
			{
				return null;
			}
			return obj.GetItemComponentsInChildren<_E077>().FirstOrDefault();
		}

		private void _E000()
		{
			Changed.Invoke();
		}

		public virtual void Dispose()
		{
			if (this._E000 != null)
			{
				_E003();
			}
			this._E000 = null;
		}
	}

	public enum EUpdateMode
	{
		Auto,
		Manual,
		None
	}

	public delegate float _E015();

	protected internal abstract class PlayerOwnerInventoryController : PlayerInventoryController
	{
		protected PlayerOwnerInventoryController(Player player, Profile profile, bool examined)
			: base(player, profile, examined)
		{
		}

		public override void ExamineMalfunction(Weapon weapon, bool clearRest = false)
		{
			if (!base._E000.IsAI && weapon.MalfState.IsKnownMalfunction(base.Profile.Id))
			{
				GameSetting<bool> malfunctionVisability = Singleton<_E7DE>.Instance.Game.Settings.MalfunctionVisability;
				bool num = MonoBehaviourSingleton<PreloaderUI>.Instance.MalfunctionGlow.ShowGlow(BattleUIMalfunctionGlow.GlowType.Examined, force: false, malfunctionVisability ? _E001() : 0f);
				base._E000.NeedRepairMalfPhraseSituation(weapon.MalfState.State, HasKnownMalfType(weapon));
				if (num)
				{
					if (HasKnownMalfType(weapon))
					{
						_E857.DisplayNotification(new _E892(weapon.MalfState.State));
					}
					else
					{
						_E857.DisplayNotification(new _E893());
					}
				}
			}
			else
			{
				base.ExamineMalfunction(weapon, clearRest);
				base._E000.NeedRepairMalfPhraseSituation(weapon.MalfState.State, HasKnownMalfType(weapon));
				if (!base._E000.IsAI)
				{
					_E000(weapon, 150).HandleExceptions();
				}
			}
		}

		private new async Task _E000(Weapon weapon, int delayInMilliseconds)
		{
			await Task.Delay(delayInMilliseconds);
			if ((bool)Singleton<_E7DE>.Instance.Game.Settings.MalfunctionVisability)
			{
				MonoBehaviourSingleton<PreloaderUI>.Instance.MalfunctionGlow.ShowGlow(BattleUIMalfunctionGlow.GlowType.Examined, force: true, _E001());
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MalfunctionExamined);
			}
			bool isKnown = weapon.MalfState.IsKnownMalfType(base._E000.ProfileId);
			base._E000.NeedRepairMalfPhraseSituation(weapon.MalfState.State, isKnown);
		}

		public override void ExamineMalfunctionType(Weapon weapon)
		{
			if (weapon.MalfState.IsKnownMalfType(base.Profile.Id))
			{
				_E857.DisplayNotification(new _E894(weapon.MalfState.State));
				return;
			}
			base.ExamineMalfunctionType(weapon);
			if (!base._E000.IsAI)
			{
				if ((bool)Singleton<_E7DE>.Instance.Game.Settings.MalfunctionVisability)
				{
					MonoBehaviourSingleton<PreloaderUI>.Instance.MalfunctionGlow.ShowGlow(BattleUIMalfunctionGlow.GlowType.TypeExamined, force: true, _E001());
				}
				_E857.DisplayNotification(new _E894(weapon.MalfState.State));
			}
		}

		public override void CallUnknownMalfunctionStartRepair(Weapon weapon)
		{
			base.CallUnknownMalfunctionStartRepair(weapon);
			if (!base._E000.IsAI)
			{
				if ((bool)Singleton<_E7DE>.Instance.Game.Settings.MalfunctionVisability)
				{
					MonoBehaviourSingleton<PreloaderUI>.Instance.MalfunctionGlow.ShowGlow(BattleUIMalfunctionGlow.GlowType.Repaired, force: true, _E001());
				}
				_E857.DisplayNotification(new _E894(weapon.MalfState.State));
			}
		}

		public override void CallMalfunctionRepaired(Weapon weapon)
		{
			base.CallMalfunctionRepaired(weapon);
			if (!base._E000.IsAI && (bool)Singleton<_E7DE>.Instance.Game.Settings.MalfunctionVisability)
			{
				MonoBehaviourSingleton<PreloaderUI>.Instance.MalfunctionGlow.ShowGlow(BattleUIMalfunctionGlow.GlowType.Repaired, force: true, _E001());
			}
		}

		private float _E001()
		{
			float result = 0.5f;
			if (base._E000.HealthController.FindActiveEffect<_E9AF>() != null)
			{
				result = 1f;
			}
			return result;
		}

		public override _ECD9<bool> TryThrowItem(Item item, Callback callback = null, bool silent = false)
		{
			if (item.Owner is _EAEA obj)
			{
				List<_EAE9> list = obj.GetItemsOverDiscardLimit(item).ToList();
				if (list.Any())
				{
					_EA0B obj2 = new _EA0B(item, list);
					if (!silent)
					{
						_E857.DisplayWarningNotification(obj2.GetLocalizedDescription());
					}
					return obj2;
				}
			}
			ThrowItem(item, null, callback);
			return true;
		}
	}

	protected sealed class SinglePlayerInventoryController : PlayerOwnerInventoryController
	{
		internal SinglePlayerInventoryController(Player player, Profile profile)
			: base(player, profile, examined: false)
		{
		}

		internal override void Execute(_EB73 operation, Callback callback)
		{
			_E000(operation, callback).HandleExceptions();
		}

		private new async Task _E000(_EB73 operation, [CanBeNull] Callback callback)
		{
			if (base._E000._healthController.IsAlive)
			{
				await Task.Yield();
			}
			base.Execute(operation, callback);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private void _E001(_EB73 operation, Callback callback)
		{
			base.Execute(operation, callback);
		}
	}

	[CompilerGenerated]
	private sealed class _E016
	{
		public Player _003C_003E4__this;

		public IWeapon weapon;

		internal void _E000()
		{
			_003C_003E4__this.Skills.RecoilAction.Complete(weapon.RecoilBase);
		}
	}

	[CompilerGenerated]
	private sealed class _E018
	{
		public GripPose.EGripType type;

		public Player _003C_003E4__this;

		internal bool _E000(GripPose x)
		{
			if (x.GripType != type)
			{
				return x.GripType == GripPose.EGripType.UnderbarrelWeapon;
			}
			return true;
		}

		internal int _E001(GripPose x)
		{
			return HandPoser.NumParents(x.transform, _003C_003E4__this.PlayerBones.WeaponRoot.Original);
		}

		internal int _E002(GripPose x)
		{
			return HandPoser.NumParents(x.transform, _003C_003E4__this.PlayerBones.WeaponRoot.Original);
		}
	}

	[CompilerGenerated]
	private sealed class _E01F
	{
		public VoiceBroadcastTrigger broadcastTrigger;

		internal void _E000(int value)
		{
			float volume = (float)value / 100f;
			broadcastTrigger.ActivationFader.Volume = volume;
		}
	}

	[CompilerGenerated]
	private sealed class _E020
	{
		public Player _003C_003E4__this;

		public AbstractHandsController controller;

		public Action callback;

		public TaskCompletionSource onControllerAppeared;

		internal void _E000()
		{
			_003C_003E4__this._E032(controller);
			callback?.Invoke();
			onControllerAppeared.SetResult(result: true);
		}
	}

	[CompilerGenerated]
	private sealed class _E021
	{
		public Player _003C_003E4__this;

		public Callback<_E6C7> completeCallback;

		internal Item _E000(EquipmentSlot x)
		{
			return _003C_003E4__this._E0DE.Inventory.Equipment.GetSlot(x).ContainedItem;
		}

		internal bool _E001(Item x)
		{
			return _003C_003E4__this._E033(x);
		}

		internal void _E002(Result<_E6C9> result)
		{
			completeCallback(result.Complete ? new Result<_E6C7>(result.Value) : new Result<_E6C7>(null, result.Error));
		}
	}

	[CompilerGenerated]
	private sealed class _E022
	{
		public Callback<_E6C7> callback;

		internal void _E000(Result<_E6C9> result)
		{
			callback((!string.IsNullOrEmpty(result.Error)) ? new Result<_E6C7>(null, result.Error) : new Result<_E6C7>(result.Value));
		}
	}

	[CompilerGenerated]
	private sealed class _E023
	{
		public Player _003C_003E4__this;

		public Weapon weapon;

		internal AIFirearmController _E000()
		{
			return FirearmController._E000<AIFirearmController>(_003C_003E4__this, weapon);
		}

		internal FirearmController _E001()
		{
			return FirearmController._E000<FirearmController>(_003C_003E4__this, weapon);
		}
	}

	[CompilerGenerated]
	private sealed class _E024
	{
		public Player _003C_003E4__this;

		public _EADF throwWeap;

		internal GrenadeController _E000()
		{
			return GrenadeController._E000<GrenadeController>(_003C_003E4__this, throwWeap);
		}
	}

	[CompilerGenerated]
	private sealed class _E025
	{
		public Player _003C_003E4__this;

		public _EA72 meds;

		public EBodyPart bodyPart;

		public int animationVariant;

		internal MedsController _E000()
		{
			return MedsController._E000<MedsController>(_003C_003E4__this, meds, bodyPart, 1f, animationVariant);
		}
	}

	[CompilerGenerated]
	private sealed class _E026
	{
		public Player _003C_003E4__this;

		public _EA48 foodDrink;

		public float amount;

		public int animationVariant;

		internal MedsController _E000()
		{
			return MedsController._E000<MedsController>(_003C_003E4__this, foodDrink, EBodyPart.Head, amount, animationVariant);
		}
	}

	[CompilerGenerated]
	private sealed class _E027
	{
		public Player _003C_003E4__this;

		public KnifeComponent knife;

		internal KnifeController _E000()
		{
			return KnifeController._E000<KnifeController>(_003C_003E4__this, knife);
		}
	}

	[CompilerGenerated]
	private sealed class _E028<_E077> where _E077 : UsableItemController
	{
		public Player _003C_003E4__this;

		public Item item;

		internal _E077 _E000()
		{
			return UsableItemController._E000<_E077>(_003C_003E4__this, item);
		}
	}

	[CompilerGenerated]
	private sealed class _E029
	{
		public Player _003C_003E4__this;

		public Item item;

		internal QuickUseItemController _E000()
		{
			return QuickUseItemController._E000<QuickUseItemController>(_003C_003E4__this, item);
		}
	}

	[CompilerGenerated]
	private sealed class _E02A
	{
		public Player _003C_003E4__this;

		public _EADF throwWeap;

		internal QuickGrenadeThrowController _E000()
		{
			return QuickGrenadeThrowController._E000<QuickGrenadeThrowController>(_003C_003E4__this, throwWeap);
		}
	}

	[CompilerGenerated]
	private sealed class _E02B
	{
		public Player _003C_003E4__this;

		public KnifeComponent knife;

		internal QuickKnifeKickController _E000()
		{
			return QuickKnifeKickController._E000<QuickKnifeKickController>(_003C_003E4__this, knife);
		}
	}

	[CompilerGenerated]
	private sealed class _E02C
	{
		public Callback<_E6C7> completeCallback;

		public Player _003C_003E4__this;

		internal void _E000()
		{
			completeCallback?.Invoke(new Result<_E6C7>
			{
				Error = _ED3E._E000(159412)
			});
		}

		internal void _E001(Result<_E6CC> result)
		{
			_E038(result, completeCallback);
		}

		internal void _E002(Result<_E6CF> result)
		{
			_E038(result, completeCallback);
		}

		internal void _E003(Result<_E6CF> result)
		{
			_E038(result, completeCallback);
		}

		internal void _E004(Result<_E6CD> result)
		{
			_E038(result, completeCallback);
		}

		internal void _E005(Result<_E6D4> result)
		{
			_E038(result, completeCallback);
		}
	}

	[CompilerGenerated]
	private sealed class _E02D
	{
		public Weapon _003Cweapon_003E5__2;

		public _E02C CS_0024_003C_003E8__locals2;

		internal void _E000(Result<_E6CB> result)
		{
			_E038(result, CS_0024_003C_003E8__locals2.completeCallback);
			if (result.Complete)
			{
				CS_0024_003C_003E8__locals2._003C_003E4__this.LastEquippedWeaponOrKnifeItem = _003Cweapon_003E5__2;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E02E
	{
		public Callback<_E6C7> completeCallback;

		internal void _E000(Result<_E6CE> result)
		{
			_E038(result, completeCallback);
		}

		internal void _E001(Result<_E6CE> result)
		{
			_E038(result, completeCallback);
		}

		internal void _E002(Result<_E6CE> result)
		{
			_E038(result, completeCallback);
		}
	}

	[CompilerGenerated]
	private sealed class _E02F
	{
		public Callback<_E6C7> completeCallback;

		internal void _E000(Result<_E6CE> result)
		{
			_E038(result, completeCallback);
		}

		internal void _E001(Result<_E6CE> result)
		{
			_E038(result, completeCallback);
		}

		internal void _E002(Result<_E6CE> result)
		{
			_E038(result, completeCallback);
		}
	}

	[CompilerGenerated]
	private sealed class _E030
	{
		public Player _003C_003E4__this;

		public Callback callback;

		internal void _E000(IResult result)
		{
			if ((object)_003C_003E4__this._removeFromHandsCallback == callback)
			{
				_003C_003E4__this._removeFromHandsCallback = null;
			}
			_003C_003E4__this._E0DE.RaiseInOutProcessEvents(new _EB08(_003C_003E4__this.HandsController.Item, CommandStatus.Succeed));
			callback(result);
		}
	}

	[CompilerGenerated]
	private sealed class _E031
	{
		public Player _003C_003E4__this;

		public Callback callback;

		internal void _E000(Result<_E6CD> result)
		{
			if ((object)_003C_003E4__this._removeFromHandsCallback == callback)
			{
				_003C_003E4__this._removeFromHandsCallback = null;
			}
			callback.Invoke(result);
		}

		internal void _E001(Result<_E6CE> result)
		{
			if ((object)_003C_003E4__this._removeFromHandsCallback == callback)
			{
				_003C_003E4__this._removeFromHandsCallback = null;
			}
			callback.Invoke(result);
		}

		internal void _E002(Result<_E6CE> result)
		{
			if ((object)_003C_003E4__this._removeFromHandsCallback == callback)
			{
				_003C_003E4__this._removeFromHandsCallback = null;
			}
			callback.Invoke(result);
		}

		internal void _E003(Result<_E6C9> result)
		{
			if ((object)_003C_003E4__this._removeFromHandsCallback == callback)
			{
				_003C_003E4__this._removeFromHandsCallback = null;
			}
			callback.Invoke(result);
		}
	}

	[CompilerGenerated]
	private sealed class _E032
	{
		public Player _003C_003E4__this;

		public Callback callback;

		internal void _E000(Result<_E6C7> result)
		{
			if ((object)_003C_003E4__this._E090 == callback)
			{
				_003C_003E4__this._E090 = null;
			}
			callback.Invoke(result);
		}

		internal void _E001(IResult error)
		{
			if ((object)_003C_003E4__this._E090 == callback)
			{
				_003C_003E4__this._E090 = null;
			}
			_003C_003E4__this._E0DE.RaiseInOutProcessEvents(new _EB08(_003C_003E4__this.HandsController.Item, CommandStatus.Succeed));
			callback(error);
		}
	}

	[CompilerGenerated]
	private sealed class _E034
	{
		public Player _003C_003E4__this;

		public EPointOfView pointOfView;

		internal void _E000(bool _)
		{
			_003C_003E4__this._E0AE?.Invoke();
		}

		internal void _E001(EPlayerState prevState, EPlayerState nextState)
		{
			_003C_003E4__this.ProceduralWeaponAnimation.WalkEffectorEnabled = nextState == EPlayerState.Run;
			_003C_003E4__this.ProceduralWeaponAnimation.DrawEffectorEnabled = nextState != EPlayerState.ProneMove;
			_003C_003E4__this.ProceduralWeaponAnimation.TiltBlender.Target = ((nextState == EPlayerState.Idle || nextState == EPlayerState.ProneIdle) ? 1 : 0);
			if (nextState == EPlayerState.Stationary)
			{
				_003C_003E4__this.ProceduralWeaponAnimation.SetStrategy(new _E3CE());
			}
			else if (prevState == EPlayerState.Stationary)
			{
				_003C_003E4__this.ProceduralWeaponAnimation.SetStrategy(pointOfView);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E036
	{
		public Action togglableSub;

		public Action hitSub;

		internal void _E000()
		{
			togglableSub?.Invoke();
			hitSub();
		}
	}

	[CompilerGenerated]
	private sealed class _E037
	{
		public Action togglableSub;

		public Action hitSub;

		internal void _E000()
		{
			togglableSub?.Invoke();
			hitSub();
		}
	}

	[CompilerGenerated]
	private sealed class _E038
	{
		public Player _003C_003E4__this;

		public _E992 effect;

		internal void _E000()
		{
			_003C_003E4__this.Skills.LowHPDuration.Begin();
		}

		internal void _E001()
		{
			_003C_003E4__this.Skills.HealthNegativeEffect.Complete(effect);
		}
	}

	[CompilerGenerated]
	private sealed class _E039
	{
		public Slot[] headSlots;

		public Slot[] armorSlots;

		internal bool _E000(ItemAddress loc)
		{
			return headSlots.Contains(loc.Container);
		}

		internal bool _E001(ItemAddress loc)
		{
			return armorSlots.Contains(loc.Container);
		}
	}

	[CompilerGenerated]
	private sealed class _E03B
	{
		public Player _003C_003E4__this;

		public float armorDamage;

		internal void _E000()
		{
			_003C_003E4__this.Skills.LightArmorDamageTakenAction.Complete(armorDamage);
		}

		internal void _E001()
		{
			_003C_003E4__this.Skills.HeavyArmorDamageTakenAction.Complete(armorDamage);
		}
	}

	[CompilerGenerated]
	private sealed class _E03D
	{
		public LootableContainer container;

		public Player _003C_003E4__this;

		internal void _E000()
		{
			container.Interact(new _EBFE(EInteractionType.Close));
			if (_003C_003E4__this.MovementContext.LevelOnApproachStart > 0f)
			{
				_003C_003E4__this.MovementContext.SetPoseLevel(_003C_003E4__this.MovementContext.LevelOnApproachStart);
				_003C_003E4__this.MovementContext.LevelOnApproachStart = -1f;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E03E
	{
		public Player _003C_003E4__this;

		public bool onCorpse;

		internal void _E000()
		{
			_003C_003E4__this.Skills.FindAction.Complete(onCorpse);
		}
	}

	[CompilerGenerated]
	private sealed class _E03F
	{
		public Player _003C_003E4__this;

		public int count;

		internal void _E000()
		{
			_003C_003E4__this.Skills.RaidLoadedAmmoAction.Complete(count);
		}
	}

	[CompilerGenerated]
	private sealed class _E040
	{
		public Player _003C_003E4__this;

		public int count;

		internal void _E000()
		{
			_003C_003E4__this.Skills.RaidUnloadedAmmoAction.Complete(count);
		}
	}

	[CompilerGenerated]
	private sealed class _E041
	{
		public Player _003C_003E4__this;

		public float diff;

		internal void _E000()
		{
			_003C_003E4__this.Skills.HydrationChanged.Complete(diff, diff);
		}
	}

	[CompilerGenerated]
	private sealed class _E042
	{
		public Player _003C_003E4__this;

		public float damage;

		internal void _E000()
		{
			_003C_003E4__this.Skills.DamageTakenAction.Complete(damage);
		}
	}

	[CompilerGenerated]
	private sealed class _E043
	{
		public Player _003C_003E4__this;

		public float diff;

		internal void _E000()
		{
			_003C_003E4__this.Skills.EnergyChanged.Complete(diff, diff);
		}
	}

	[CompilerGenerated]
	private sealed class _E045
	{
		public float distance;

		public Player _003C_003E4__this;

		internal void _E000()
		{
			_003C_003E4__this.Skills.SprintAction.Complete(new _E74F._E001
			{
				Overweight = _003C_003E4__this.Physical.Overweight,
				Fatigue = (_003C_003E4__this._E0A1?.Strength ?? 0f)
			}, distance);
		}
	}

	[CompilerGenerated]
	private sealed class _E046
	{
		public float distance;

		public Player _003C_003E4__this;

		internal void _E000()
		{
			_003C_003E4__this.Skills.MovementAction.Complete(new _E74F._E001
			{
				Noise = _003C_003E4__this.MovementContext.CovertNoiseLevel,
				Overweight = _003C_003E4__this.Physical.Overweight,
				Fatigue = (_003C_003E4__this._E0A1?.Strength ?? 0f)
			}, distance);
		}
	}

	[CompilerGenerated]
	private Action<float, float, int> m__E000;

	[CompilerGenerated]
	private Action<SightComponent> m__E001;

	public _E33A _characterController;

	protected TriggerColliderSearcher _triggerColliderSearcher;

	private bool m__E002;

	private WorldInteractiveObject m__E003;

	[CompilerGenerated]
	private _E70F m__E004;

	private float m__E005;

	private float m__E006;

	private bool m__E007;

	private bool m__E008;

	private bool m__E009;

	private bool m__E00A;

	private bool m__E00B;

	public _E6D7 Pedometer;

	public Vector3 HeadRotation;

	protected float _mouseSensitivityModifier;

	protected readonly Dictionary<EMouseSensitivityModifier, float> _mouseSensitivityModifiers = _E3A5<EMouseSensitivityModifier>.GetDictWith<float>();

	private Vector2 m__E00C = Player._E005.STAND_POSE_ROTATION_PITCH_RANGE;

	private Vector2 m__E00D = Player._E005.STAND_POSE_ROTATION_PITCH_RANGE;

	[CompilerGenerated]
	private Vector2 m__E00E;

	public float TrunkRotationLimit;

	public float PoseMemo = 1f;

	private float m__E00F;

	private float m__E010 = 0.5f;

	private float m__E011;

	public LeanType CurrentLeanType;

	private float m__E012;

	private bool m__E013;

	protected float _prevHeight;

	public float HeightSmoothTime = 0.066f;

	private float m__E014;

	private float m__E015;

	private float m__E016;

	private const float m__E017 = 0.2f;

	public const int GRIP_CULL_DISTANCE = 40;

	public const int IK_CULL_DISTANCE = 70;

	public const int MAX_IK_CULL_DISTANCE = 300;

	private const string m__E018 = "assets/content/weapons/additional_hands/item_compass.bundle";

	[CompilerGenerated]
	private _E006 m__E019;

	public GripPose LeftHandInteractionTarget;

	public GrounderFBBIK Grounder;

	public HitReaction HitReaction;

	public float RibcageScaleCurrent = 1f;

	public float RibcageScaleCurrentTarget = 1f;

	public Transform[] _elbowBends;

	public HandPoser[] HandPosers;

	public Vector2 UtilityLayerRange = new Vector2(0.5f, 0.2f);

	public float UtilityLayerLerpSpeed = 3f;

	public ValueBlender LMarkerRawBlender = new ValueBlender
	{
		Speed = 4f,
		Target = 0f
	};

	public ValueBlender LayerWeight = new ValueBlender
	{
		Speed = 4f
	};

	public readonly BetterValueBlender ThirdIkWeight = new BetterValueBlender
	{
		Speed = 3f
	};

	public bool GripAutoAdjust;

	public bool CustomAnimationsAreProcessing;

	protected FullBodyBipedIK _fbbik;

	protected PlayerBody _playerBody;

	protected float ThirdPersonWeaponRootAuthority;

	public const float HAND_ANIMATION_BLEND_THRESHOLD = 0.1f;

	private float m__E01A = 1f;

	private float m__E01B;

	private float m__E01C = 0.6f;

	private float m__E01D;

	private float m__E01E;

	private float m__E01F;

	private float m__E020;

	private float m__E021;

	private float m__E022;

	private float m__E023;

	private float m__E024;

	private bool m__E025;

	private bool m__E026;

	private bool m__E027 = true;

	private bool m__E028;

	private bool m__E029;

	private bool m__E02A;

	private bool m__E02B;

	private bool m__E02C;

	private Vector3[] m__E02D;

	private Vector3 m__E02E;

	private Vector3 m__E02F;

	private Vector3 m__E030;

	private Vector3 m__E031;

	private Vector3 m__E032;

	private Quaternion m__E033;

	private Quaternion m__E034;

	private Quaternion m__E035;

	private Quaternion[] m__E036;

	private readonly Transform[] m__E037 = new Transform[2];

	private readonly Transform[] m__E038 = new Transform[2];

	private Transform[] m__E039 = new Transform[2];

	private Transform m__E03A;

	private Transform m__E03B;

	private Transform m__E03C;

	private bool m__E03D;

	private TwistRelax[] m__E03E;

	private LimbIK[] m__E03F;

	private GameObject m__E040;

	private Action m__E041;

	private PreviewMaterialSetter m__E042;

	private BeaconPlacer m__E043;

	private FirearmsEffects m__E044;

	private FirearmsEffects m__E045;

	private CompassArrow m__E046;

	private RadioTransmitterView m__E047;

	private Transform[] m__E048 = new Transform[3];

	private Transform[] m__E049 = new Transform[3];

	private readonly List<_E3D2> m__E04A = new List<_E3D2>(10);

	[NonSerialized]
	public _ECEC PointOfViewChanged = new _ECEC();

	public readonly ValueBlender AuthorityBlender = new ValueBlender
	{
		Speed = 4f,
		Target = 0f
	};

	public readonly ValueBlender GrounderBlender = new ValueBlender
	{
		Speed = 4f,
		Target = 0f
	};

	private float m__E04B;

	private float m__E04C;

	protected bool _isDeadAlready;

	private bool m__E04D;

	private ObjectInHandsAnimator m__E04E;

	private GameObject m__E04F;

	private Action m__E050;

	[CompilerGenerated]
	private Action<_EC23, EBodyPart, float> m__E051;

	[CompilerGenerated]
	private Vector3 m__E052;

	[CompilerGenerated]
	private Quaternion m__E053;

	[CompilerGenerated]
	private GameObject m__E054;

	[CompilerGenerated]
	private Transform m__E055;

	[CompilerGenerated]
	private ProceduralWeaponAnimation m__E056;

	[CompilerGenerated]
	private bool m__E057;

	private _E007 m__E058;

	public Text DebugText;

	[CompilerGenerated]
	private bool m__E059;

	[CompilerGenerated]
	private bool m__E05A;

	public BaseBallistic.ESurfaceSound CurrentSurface;

	protected LayerMask _stepLayerMask;

	private const float m__E05B = 0.5f;

	private const float m__E05C = 2.5f;

	public const float DEFAULT_ROLL_OFF = 60f;

	public const int BUFFER_ZONE = 2;

	private const float m__E05D = 5f;

	private const float m__E05E = 3.5f;

	private const float m__E05F = 0.5f;

	private const float m__E060 = 1f;

	[CompilerGenerated]
	private EnvironmentType m__E061;

	public bool HeavyBreath;

	public bool Muffled;

	protected BetterSource NestedStepSoundSource;

	protected BetterSource _speechSource;

	protected bool OcclusionDirty;

	protected bool DistanceDirty;

	protected AudioClip FractureSound;

	public BaseSoundPlayer.SoundElement PropIn;

	public BaseSoundPlayer.SoundElement PropOut;

	protected bool PreviousFaceShield;

	protected bool PreviousNightVision;

	protected bool PreviousThermalVision;

	protected readonly Vector3 SpeechLocalPosition = new Vector3(0f, 1.2f, 0f);

	protected AudioClip FaceshieldOn;

	protected AudioClip FaceshieldOff;

	protected AudioClip NightVisionOn;

	protected AudioClip NightVisionOff;

	protected AudioClip ThermalVisionOn;

	protected AudioClip ThermalVisionOff;

	private AudioClip m__E062;

	private Dictionary<BaseBallistic.ESurfaceSound, SurfaceSet> m__E063;

	private SurfaceSet m__E064;

	private SoundBank m__E065;

	private SoundBank m__E066;

	private Coroutine m__E067;

	private Coroutine m__E068;

	private Coroutine m__E069;

	private Coroutine m__E06A;

	private Coroutine m__E06B;

	private Coroutine m__E06C;

	private bool m__E06D;

	private float m__E06E;

	private BetterSource m__E06F;

	private float m__E070;

	private AudioClip m__E071;

	private const int m__E072 = 14;

	private readonly List<BetterPropagationVolume> m__E073 = new List<BetterPropagationVolume>();

	private readonly List<BetterPropagationVolume> _E074 = new List<BetterPropagationVolume>();

	private BetterPropagationVolume _E075;

	private bool _E076;

	private Action _E077;

	private float _E078 = 60f;

	private float _E079 = 40f;

	private float _E07A = 40f;

	private float _E07B = 30f;

	private float _E07C;

	private float _E07D;

	private float _E07E;

	private float _E07F = 0.6f;

	private float _E080;

	private float _E081;

	private bool _E082;

	private WrappedVoipAudioSource _E083;

	private Action _E084;

	[CompilerGenerated]
	private _E7B3 _E085;

	[CompilerGenerated]
	private DissonanceComms _E086;

	protected static readonly TimeSpan HearingDetectionTime = TimeSpan.FromSeconds(2.0);

	[CompilerGenerated]
	private DateTime _E087;

	[CompilerGenerated]
	private EVoipState _E088;

	private EProcessStatus _E089;

	[CompilerGenerated]
	private AbstractProcess _E08A;

	[CompilerGenerated]
	private Slot _E08B;

	[CompilerGenerated]
	private Action _E08C;

	[CompilerGenerated]
	private Action<_E6C7> _E08D;

	private Item _E08E;

	private readonly EquipmentSlot[] _E08F = new EquipmentSlot[4]
	{
		EquipmentSlot.FirstPrimaryWeapon,
		EquipmentSlot.SecondPrimaryWeapon,
		EquipmentSlot.Holster,
		EquipmentSlot.Scabbard
	};

	protected Callback _removeFromHandsCallback;

	private Callback _E090;

	private float _E091;

	private int _E092;

	private const int _E093 = 3;

	public const EAnimatorMask EnabledAnimatorsPlayerDefault = EAnimatorMask.Thirdperson | EAnimatorMask.Arms | EAnimatorMask.Procedural | EAnimatorMask.FBBIK | EAnimatorMask.IK;

	public const EAnimatorMask FastAnimatorMask = EAnimatorMask.Thirdperson | EAnimatorMask.Arms | EAnimatorMask.FBBIK | EAnimatorMask.IK;

	public const EAnimatorMask EnabledAnimatorsSpiritDefault = EAnimatorMask.Thirdperson | EAnimatorMask.Arms;

	private const int _E094 = 2;

	private const int _E095 = 3;

	public const string LAYER_NAME_PLAYER = "Player";

	public PlayerOverlapManager POM;

	public List<string> TriggerZones = new List<string>();

	[NonSerialized]
	public _ECEC OnExitTriggerVisited = new _ECEC();

	public _ECF5<bool> InteractingWithExfiltrationPoint = new _ECF5<bool>();

	public EDamageType LastDamageType;

	public EBodyPart LastDamagedBodyPart;

	public bool Destroyed;

	public bool QuickdrawWeaponFast;

	public float QuickdrawTime;

	public bool IsInBufferZone;

	public bool CanManipulateWithHandsInBufferZone;

	public IAnimator[] _animators;

	public IAnimator _underbarrelFastAnimator;

	public _E76C Speaker;

	[CompilerGenerated]
	private _E6FD _E096;

	[CompilerGenerated]
	private static Action<Player, Player, _EC23, EBodyPart> _E097;

	[CompilerGenerated]
	private _E6FE _E098;

	public PlayerSpirit Spirit;

	[_E376(typeof(EAnimatorMask))]
	public EAnimatorMask EnabledAnimators = EAnimatorMask.Thirdperson | EAnimatorMask.Arms | EAnimatorMask.Procedural | EAnimatorMask.FBBIK | EAnimatorMask.IK;

	protected PlayerLogger Logger;

	protected Corpse Corpse;

	protected Player LastAggressor;

	protected _EC23 LastDamageInfo;

	protected EBodyPart LastBodyPart;

	protected float _corpseAppliedForce;

	protected Func<float> GetSensitivity;

	protected Func<float> GetAimingSensitivity;

	protected Action<Action> _openAction;

	protected _E639 recodableItemsHandler;

	private float _E099 = 1f;

	private float _E09A;

	private int _E09B;

	private float _E09C;

	private int _E09D;

	private bool _E09E;

	private bool _E09F;

	private bool _E0A0;

	private _E992 _E0A1;

	private Renderer[] _E0A2 = Array.Empty<Renderer>();

	private Camera _E0A3;

	private Coroutine _E0A4;

	private Coroutine _E0A5;

	private readonly _ECF5<Item> _E0A6 = new _ECF5<Item>();

	protected readonly _E3A4 CompositeDisposable = new _E3A4();

	private _EC19 _E0A7;

	private Action _E0A8;

	private readonly List<Item> _E0A9 = new List<Item>();

	private bool _E0AA = true;

	private Animator _E0AB;

	private RuntimeAnimatorController _E0AC;

	[CompilerGenerated]
	private Action<bool> _E0AD;

	[CompilerGenerated]
	private Action _E0AE;

	[CompilerGenerated]
	private Action<EPhraseTrigger, int> _E0AF;

	[CompilerGenerated]
	private Action<bool> _E0B0;

	[CompilerGenerated]
	private Action<Player, bool> _E0B1;

	[CompilerGenerated]
	private Action _E0B2;

	[CompilerGenerated]
	private GenericEventTranslator _E0B3;

	[CompilerGenerated]
	private _E014<NightVisionComponent> _E0B4;

	[CompilerGenerated]
	private _E014<ThermalVisionComponent> _E0B5;

	[CompilerGenerated]
	private _E014<FaceShieldComponent> _E0B6;

	[CompilerGenerated]
	private _E014<FaceShieldComponent> _E0B7;

	[CompilerGenerated]
	private string _E0B8;

	[CompilerGenerated]
	private ISpawnPoint _E0B9;

	[CompilerGenerated]
	private float _E0BA;

	[CompilerGenerated]
	private InteractableObject _E0BB;

	[CompilerGenerated]
	private bool _E0BC;

	[CompilerGenerated]
	private bool _E0BD;

	[CompilerGenerated]
	private Player _E0BE;

	[CompilerGenerated]
	private PlaceItemTrigger _E0BF;

	[CompilerGenerated]
	private ExfiltrationPoint _E0C0;

	[CompilerGenerated]
	private bool _E0C1;

	[CompilerGenerated]
	private _EC14 _E0C2 = new _EC14(0);

	[CompilerGenerated]
	private int _E0C3;

	[CompilerGenerated]
	private Profile _E0C4;

	[CompilerGenerated]
	private _E759 _E0C5;

	public _E338 Physical;

	[CompilerGenerated]
	private Action<string, int> _E0C6;

	[SerializeField]
	private EUpdateQueue _updateQueue;

	[SerializeField]
	protected EUpdateQueue _armsUpdateQueue;

	[SerializeField]
	protected EUpdateMode _armsUpdateMode;

	[SerializeField]
	protected EUpdateMode _bodyUpdateMode;

	[CompilerGenerated]
	private _E279 _E0C7;

	[CompilerGenerated]
	private _E27C _E0C8;

	[CompilerGenerated]
	private Dictionary<BodyPartType, _E1FF> _E0C9;

	protected _E9C4 _healthController;

	protected BodyPartCollider[] _hitColliders;

	protected _EAED _inventoryController;

	protected AbstractHandsController _handsController;

	protected _E935 _questController;

	[CompilerGenerated]
	private Action<AbstractHandsController, AbstractHandsController> _E0CA;

	private string _E0CB;

	public Transform Tracking;

	private float _E0CC;

	protected bool _armsupdated;

	protected float _armsTime;

	protected bool _bodyupdated;

	protected float _bodyTime;

	protected int _nFixedFrames;

	protected float _fixedTime;

	private static readonly _E015 _E0CD = () => Time.deltaTime;

	private _E015 _E0CE = _E0CD;

	private WaitForFixedUpdate _E0CF = new WaitForFixedUpdate();

	protected float LastDeltaTime;

	protected bool _manuallyUpdated;

	protected Transform _playerLookRaycastTransform;

	private EDoorState _E0D0;

	private bool _E0D1;

	private float _E0D2;

	public PlayerBones PlayerBones;

	protected readonly List<ArmorComponent> _preAllocatedArmorComponents = new List<ArmorComponent>(10);

	protected EquipmentPenaltyComponent _preAllocatedBackpackPenaltyComponent;

	[CompilerGenerated]
	private Action<ExfiltrationPoint, bool> _E0D3;

	[CompilerGenerated]
	private _E267 _E0D4;

	private bool _E0D5;

	private bool _E0D6;

	protected Action ExfilUnsubscribe;

	protected List<Action> SessionEndUnsubscribe;

	protected bool AggressorFound;

	[CompilerGenerated]
	private bool _E0D7;

	public IAnimator BodyAnimatorCommon => GetBodyAnimatorCommon();

	public IAnimator ArmsAnimatorCommon => GetArmsAnimatorCommon();

	public IAnimator UnderbarrelWeaponArmsAnimator => _underbarrelFastAnimator;

	public _E33A CharacterControllerCommon => GetCharacterControllerCommon();

	public _E70F MovementContext
	{
		[CompilerGenerated]
		get
		{
			return this.m__E004;
		}
		[CompilerGenerated]
		protected set
		{
			this.m__E004 = value;
		}
	}

	public bool IsResettingLook => this.m__E009;

	public bool IsLooking => this.m__E00B;

	public bool MouseLookControl => this.m__E008;

	public EPlayerPose Pose
	{
		get
		{
			if (!IsInPronePose)
			{
				if (!(MovementContext.SmoothedPoseLevel < 0.11f))
				{
					return EPlayerPose.Stand;
				}
				return EPlayerPose.Duck;
			}
			return EPlayerPose.Prone;
		}
	}

	public float PoseLevel => MovementContext.PoseLevel;

	public float Speed => MovementContext.CharacterMovementSpeed;

	public Vector2 Rotation
	{
		get
		{
			return MovementContext.Rotation;
		}
		protected set
		{
			MovementContext.Rotation = value;
		}
	}

	public float Yaw => MovementContext.Yaw;

	public bool IsInPronePose => MovementContext.IsInPronePose;

	public float Pitch => MovementContext.Pitch;

	public Vector3 Velocity => MovementContext.Velocity;

	public Vector3 Motion => MovementContext.InputMotion;

	public Vector2 RotationPitchLimit
	{
		get
		{
			return this.m__E00C;
		}
		set
		{
			this.m__E00D = value;
		}
	}

	public Vector2 InputDirection
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00E;
		}
		[CompilerGenerated]
		set
		{
			this.m__E00E = value;
		}
	}

	public MovementState CurrentState => MovementContext.CurrentState;

	public int CurrentAnimatorStateIndex => MovementContext.CurrentAnimatorStateIndex;

	public Vector3 Position
	{
		get
		{
			return PlayerBones.BodyTransform.position;
		}
		private set
		{
			PlayerBones.BodyTransform.position = value;
		}
	}

	private float _E0D8 => MovementContext.Tilt;

	public bool IsSprintEnabled => MovementContext.IsSprintEnabled;

	private EFTHardSettings _E0D9 => EFTHardSettings.Instance;

	public float MovementIdlingTime => Time.time - this.m__E012;

	public virtual bool OnHisWayToOperateStationaryWeapon => false;

	public bool OnScreen => _playerBody.IsVisible();

	public PlayerBody PlayerBody => _playerBody;

	protected float HandsToBodyAngle => MovementContext.HandsToBodyAngle;

	public bool HasGamePlayerOwner
	{
		get
		{
			return this.m__E026;
		}
		set
		{
			this.m__E026 = value;
		}
	}

	public virtual EPointOfView PointOfView
	{
		get
		{
			return _playerBody.PointOfView.Value;
		}
		set
		{
			if (_playerBody.PointOfView.Value != value || this.m__E027)
			{
				this.m__E027 = false;
				_playerBody.PointOfView.Value = value;
				CalculateScaleValueByFov((int)Singleton<_E7DE>.Instance.Game.Settings.FieldOfView);
				SetCompensationScale();
				if (value == EPointOfView.ThirdPerson)
				{
					PlayerBones.Ribcage.Original.localScale = new Vector3(1f, 1f, 1f);
				}
				MovementContext.PlayerAnimatorPointOfView(value);
				PointOfViewChanged?.Invoke();
				_playerBody.UpdatePlayerRenders(_playerBody.PointOfView.Value, Side);
				ProceduralWeaponAnimation.PointOfView = value;
			}
		}
	}

	public Vector3 BeaconPosition
	{
		[CompilerGenerated]
		get
		{
			return this.m__E052;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E052 = value;
		}
	}

	public Quaternion BeaconRotation
	{
		[CompilerGenerated]
		get
		{
			return this.m__E053;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E053 = value;
		}
	}

	public float ErgonomicsPenalty => this.m__E04B;

	public ObjectInHandsAnimator HandsAnimator
	{
		get
		{
			return this.m__E04E;
		}
		set
		{
			this.m__E04E = value;
		}
	}

	public GameObject CameraContainer
	{
		[CompilerGenerated]
		get
		{
			return this.m__E054;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E054 = value;
		}
	}

	public Transform CameraPosition
	{
		[CompilerGenerated]
		get
		{
			return this.m__E055;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E055 = value;
		}
	}

	public ProceduralWeaponAnimation ProceduralWeaponAnimation
	{
		[CompilerGenerated]
		get
		{
			return this.m__E056;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E056 = value;
		}
	}

	public bool AllowToPlantBeacon
	{
		[CompilerGenerated]
		get
		{
			return this.m__E057;
		}
		[CompilerGenerated]
		set
		{
			this.m__E057 = value;
		}
	}

	public Vector3 LookDirection => MovementContext.LookDirection;

	public BifacialTransform WeaponRoot => PlayerBones.WeaponRoot;

	public BifacialTransform Fireport => PlayerBones.Fireport;

	public BifacialTransform[] MultiBarrelFireports => PlayerBones.MultiBarrelsFireports;

	public Quaternion HandsRotation => Quaternion.Euler(MovementContext.Pitch, MovementContext.Yaw, 0f);

	public bool SurfaceHit
	{
		[CompilerGenerated]
		get
		{
			return this.m__E059;
		}
		[CompilerGenerated]
		protected set
		{
			this.m__E059 = value;
		}
	}

	public bool SurfaceHitOnTerrain
	{
		[CompilerGenerated]
		get
		{
			return this.m__E05A;
		}
		[CompilerGenerated]
		protected set
		{
			this.m__E05A = value;
		}
	}

	public EnvironmentType Environment
	{
		[CompilerGenerated]
		get
		{
			return this.m__E061;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E061 = value;
		}
	}

	protected virtual float LandingThreshold => 0.3f;

	public WrappedVoipAudioSource VoipAudioSource
	{
		get
		{
			return _E083;
		}
		set
		{
			_E083 = value;
			_E084?.Invoke();
			_E084 = null;
			if (_E083 == null)
			{
				return;
			}
			_E7E0 settings = Singleton<_E7DE>.Instance.Sound.Settings;
			_E084 = settings.VoipEnabled.Bind(delegate(bool enable)
			{
				if (_E083 != null)
				{
					_E083.Source.mute = !enable;
				}
			});
		}
	}

	protected virtual float MINStepSoundSpeedFactor => 0.15f;

	public ETagStatus Fraction
	{
		get
		{
			if (Profile.Info.Side != EPlayerSide.Bear)
			{
				if (Profile.Info.Side != EPlayerSide.Usec)
				{
					return ETagStatus.Scav;
				}
				return ETagStatus.Usec;
			}
			return ETagStatus.Bear;
		}
	}

	public float SinceLastStep => Time.time - _E07D;

	public virtual float ProtagonistHearing => 1f;

	protected virtual float Distance
	{
		get
		{
			if (DistanceDirty)
			{
				_E081 = _E8A8.Instance.Distance(Transform.position);
				DistanceDirty = false;
			}
			return _E081;
		}
	}

	protected BetterSource SpeechSource
	{
		get
		{
			if (_speechSource == null)
			{
				CreateSpeechSource();
			}
			return _speechSource;
		}
		set
		{
			_speechSource = value;
		}
	}

	string IDissonancePlayer.PlayerId => ProfileId;

	Vector3 IDissonancePlayer.Position => Transform.Original.position;

	Quaternion IDissonancePlayer.Rotation => Transform.Original.rotation;

	public _E7B3 VoipController
	{
		[CompilerGenerated]
		get
		{
			return _E085;
		}
		[CompilerGenerated]
		protected set
		{
			_E085 = value;
		}
	}

	NetworkPlayerType IDissonancePlayer.Type
	{
		get
		{
			if (!(this is ClientPlayer))
			{
				return NetworkPlayerType.Remote;
			}
			return NetworkPlayerType.Local;
		}
	}

	public bool IsTracking => !Destroyed;

	protected DissonanceComms DissonanceComms
	{
		[CompilerGenerated]
		get
		{
			return _E086;
		}
		[CompilerGenerated]
		private set
		{
			_E086 = value;
		}
	}

	protected DateTime HearingDateTime
	{
		[CompilerGenerated]
		get
		{
			return _E087;
		}
		[CompilerGenerated]
		set
		{
			_E087 = value;
		}
	}

	protected EVoipState VoipState
	{
		[CompilerGenerated]
		get
		{
			return _E088;
		}
		[CompilerGenerated]
		set
		{
			_E088 = value;
		}
	}

	public virtual bool CanBeSnapped => true;

	public EProcessStatus ProcessStatus
	{
		get
		{
			return _E089;
		}
		private set
		{
			_E089 = value;
			_E0DE.UpdateLockedStatus();
		}
	}

	private AbstractProcess _E0DA
	{
		[CompilerGenerated]
		get
		{
			return _E08A;
		}
		[CompilerGenerated]
		set
		{
			_E08A = value;
		}
	}

	public Slot ActiveSlot
	{
		[CompilerGenerated]
		get
		{
			return _E08B;
		}
		[CompilerGenerated]
		private set
		{
			_E08B = value;
		}
	}

	public bool StateIsSuitableForHandInput => Array.IndexOf(EFTHardSettings.Instance.UnsuitableStates, CurrentState.Name) < 0;

	public Item LastEquippedWeaponOrKnifeItem
	{
		get
		{
			return _E08E;
		}
		private set
		{
			if (value is Weapon || value is _EA60)
			{
				_E08E = value;
			}
		}
	}

	protected string VisitorId => _E0DE.ID;

	public static NetworkHash128 NetworkTypeId => NetworkHash128.Parse(_ED3E._E000(47794));

	public GenericEventTranslator EventTranslator
	{
		[CompilerGenerated]
		get
		{
			return _E0B3;
		}
		[CompilerGenerated]
		private set
		{
			_E0B3 = value;
		}
	}

	public _E014<NightVisionComponent> NightVisionObserver
	{
		[CompilerGenerated]
		get
		{
			return _E0B4;
		}
		[CompilerGenerated]
		protected set
		{
			_E0B4 = value;
		}
	}

	public _E014<ThermalVisionComponent> ThermalVisionObserver
	{
		[CompilerGenerated]
		get
		{
			return _E0B5;
		}
		[CompilerGenerated]
		protected set
		{
			_E0B5 = value;
		}
	}

	public _E014<FaceShieldComponent> FaceShieldObserver
	{
		[CompilerGenerated]
		get
		{
			return _E0B6;
		}
		[CompilerGenerated]
		protected set
		{
			_E0B6 = value;
		}
	}

	public _E014<FaceShieldComponent> FaceCoverObserver
	{
		[CompilerGenerated]
		get
		{
			return _E0B7;
		}
		[CompilerGenerated]
		protected set
		{
			_E0B7 = value;
		}
	}

	public string Location
	{
		[CompilerGenerated]
		get
		{
			return _E0B8;
		}
		[CompilerGenerated]
		set
		{
			_E0B8 = value;
		}
	}

	public ISpawnPoint SpawnPoint
	{
		[CompilerGenerated]
		get
		{
			return _E0B9;
		}
		[CompilerGenerated]
		set
		{
			_E0B9 = value;
		}
	}

	public float RayLength
	{
		[CompilerGenerated]
		get
		{
			return _E0BA;
		}
		[CompilerGenerated]
		private set
		{
			_E0BA = value;
		}
	}

	public InteractableObject InteractableObject
	{
		[CompilerGenerated]
		get
		{
			return _E0BB;
		}
		[CompilerGenerated]
		private set
		{
			_E0BB = value;
		}
	}

	public bool InteractableObjectIsProxy
	{
		[CompilerGenerated]
		get
		{
			return _E0BC;
		}
		[CompilerGenerated]
		private set
		{
			_E0BC = value;
		}
	}

	public bool IsAgressorInLighthouseTraderZone
	{
		[CompilerGenerated]
		get
		{
			return _E0BD;
		}
		[CompilerGenerated]
		set
		{
			_E0BD = value;
		}
	}

	public Player InteractablePlayer
	{
		[CompilerGenerated]
		get
		{
			return _E0BE;
		}
		[CompilerGenerated]
		private set
		{
			_E0BE = value;
		}
	}

	public PlaceItemTrigger PlaceItemZone
	{
		[CompilerGenerated]
		get
		{
			return _E0BF;
		}
		[CompilerGenerated]
		set
		{
			_E0BF = value;
		}
	}

	public ExfiltrationPoint ExfiltrationPoint
	{
		[CompilerGenerated]
		get
		{
			return _E0C0;
		}
		[CompilerGenerated]
		private set
		{
			_E0C0 = value;
		}
	}

	public bool ExitTriggerZone
	{
		[CompilerGenerated]
		get
		{
			return _E0C1;
		}
		[CompilerGenerated]
		private set
		{
			_E0C1 = value;
		}
	}

	public _EC14 MalfRandoms
	{
		[CompilerGenerated]
		get
		{
			return _E0C2;
		}
		[CompilerGenerated]
		protected set
		{
			_E0C2 = value;
		}
	}

	public string Infiltration => Profile.Info.EntryPoint;

	public string GroupId => Profile.Info.GroupId;

	public string TeamId => Profile.Info.TeamId;

	public float CarryingWeightRelativeModifier => Skills.CarryingWeightRelativeModifier * HealthController.CarryingWeightRelativeModifier;

	public float CarryingWeightAbsoluteModifier => HealthController.CarryingWeightAbsoluteModifier;

	protected _EAE7 Inventory => _E0DE.Inventory;

	protected _EB0B Equipment => Inventory.Equipment;

	public bool IsInventoryOpened => _E09F;

	public _E639 RecodableItemsHandler => recodableItemsHandler;

	public float BlindnessDuration
	{
		get
		{
			if (ThermalVisionObserver.Component?.Togglable != null && ThermalVisionObserver.Component.Togglable.On)
			{
				return 0f;
			}
			FaceShieldComponent component = FaceShieldObserver.Component;
			float num = ((component != null && (component.Togglable == null || component.Togglable.On)) ? (1f - component.BlindnessProtection) : 1f);
			float num2 = ((Equipment.GetSlot(EquipmentSlot.Eyewear).ContainedItem is _EAE3 obj) ? (1f - obj.BlindnessProtection) : 1f);
			if (!(Equipment.GetSlot(EquipmentSlot.FaceCover).ContainedItem is _EA44 obj2))
			{
				return num * num2;
			}
			FaceShieldComponent faceShield = obj2.FaceShield;
			bool flag = faceShield != null && (faceShield.Togglable == null || faceShield.Togglable.On);
			return num * num2 * (flag ? (1f - faceShield.BlindnessProtection) : 1f);
		}
	}

	public int CurrentHour
	{
		get
		{
			GameWorld instance = Singleton<GameWorld>.Instance;
			if (instance != null && instance.GameDateTime != null)
			{
				return instance.GameDateTime.Calculate().Hour;
			}
			return 0;
		}
	}

	private bool _E0DB
	{
		get
		{
			return _E09E;
		}
		set
		{
			_E09E = value;
			bool flag = HandsController != null && HandsController.IsAiming;
			bool flag2 = _E09E && !flag;
			if (_E0A0 != flag2)
			{
				_E0A0 = flag2;
				_E0AD?.Invoke(_E0A0);
			}
		}
	}

	public string KillerId
	{
		get
		{
			Player player = LastDamageInfo.Player;
			if (!(player != null) || (object)this == player)
			{
				return null;
			}
			return player.ProfileId;
		}
	}

	public string KillerAccountId
	{
		get
		{
			Player player = LastDamageInfo.Player;
			if (!(player != null) || (object)this == player)
			{
				return null;
			}
			return player.Profile.AccountId;
		}
	}

	public bool HasGlasses
	{
		get
		{
			if (!(Equipment.GetSlot(EquipmentSlot.Eyewear).ContainedItem is _EAE3))
			{
				return Equipment.GetSlot(EquipmentSlot.Headwear).ContainedItem is _EAE3;
			}
			return true;
		}
	}

	public bool HandsIsEmpty => HandsController is EmptyHandsController;

	public bool IsWeaponOrKnifeInHands
	{
		get
		{
			if (!(HandsController is FirearmController))
			{
				return HandsController is BaseKnifeController;
			}
			return true;
		}
	}

	public virtual Vector3 LocalShotDirection => ProceduralWeaponAnimation.ShotDirection;

	public ETagStatus HealthStatus
	{
		get
		{
			float normalized = HealthController.GetBodyPartHealth(EBodyPart.Common).Normalized;
			HealthController.GetBodyPartsInCriticalCondition(0.15f, out var all, out var vital);
			if (vital > 0 || normalized < 0.2f)
			{
				return ETagStatus.Dying;
			}
			if (all > 2 || normalized < 0.4f)
			{
				return ETagStatus.BadlyInjured;
			}
			if (normalized < 0.9f)
			{
				return ETagStatus.Injured;
			}
			return ETagStatus.Healthy;
		}
	}

	public string TryGetId => Profile?.Id ?? PlayerId.ToString();

	public int PlayerId
	{
		[CompilerGenerated]
		get
		{
			return _E0C3;
		}
		[CompilerGenerated]
		private set
		{
			_E0C3 = value;
		}
	}

	public string ProfileId => Profile.Id;

	public Profile Profile
	{
		[CompilerGenerated]
		get
		{
			return _E0C4;
		}
		[CompilerGenerated]
		private set
		{
			_E0C4 = value;
		}
	}

	public _E759 StatisticsManager
	{
		[CompilerGenerated]
		get
		{
			return _E0C5;
		}
		[CompilerGenerated]
		protected set
		{
			_E0C5 = value;
		}
	}

	public _E74F Skills => Profile?.Skills;

	public EUpdateQueue UpdateQueue => _updateQueue;

	public EUpdateQueue ArmsUpdateQueue => _armsUpdateQueue;

	public EUpdateMode ArmsUpdateMode => _armsUpdateMode;

	public EUpdateMode BodyUpdateMode => _bodyUpdateMode;

	public Player GetPlayer => this;

	public _E279 AIData
	{
		[CompilerGenerated]
		get
		{
			return _E0C7;
		}
		[CompilerGenerated]
		protected set
		{
			_E0C7 = value;
		}
	}

	public _E27C Loyalty
	{
		[CompilerGenerated]
		get
		{
			return _E0C8;
		}
		[CompilerGenerated]
		protected set
		{
			_E0C8 = value;
		}
	}

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

	public Dictionary<BodyPartType, _E1FF> MainParts
	{
		[CompilerGenerated]
		get
		{
			return _E0C9;
		}
		[CompilerGenerated]
		private set
		{
			_E0C9 = value;
		}
	}

	public virtual AbstractHandsController HandsController
	{
		get
		{
			return _handsController;
		}
		protected set
		{
			AbstractHandsController handsController = _handsController;
			_handsController = value;
			_E712.EWeaponAnimationType weaponAnimationType = GetWeaponAnimationType(_handsController);
			MovementContext.PlayerAnimatorSetWeaponId(weaponAnimationType);
			this.m__E04D = _handsController != null && weaponAnimationType != _E712.EWeaponAnimationType.Rifle;
			if (this.m__E04D)
			{
				_E012(MovementContext.CurrentState.Name, MovementContext.CurrentState.Name);
			}
			if ((object)handsController != _handsController && _E0CA != null)
			{
				_E0CA(handsController, _handsController);
			}
		}
	}

	public EPlayerState CurrentStateName => MovementContext.CurrentState.Name;

	internal virtual _EAED _E0DE => _inventoryController;

	internal _E935 _E0DC => _questController;

	public int Id => PlayerId;

	public string FullIdInfoClean => _E0CB ?? (_E0CB = _ED3E._E000(147704) + (Profile?.Nickname ?? _ED3E._E000(31633)) + _ED3E._E000(64014) + (Profile?.AccountId ?? _ED3E._E000(31633)) + _ED3E._E000(64014) + (Profile?.Id ?? _ED3E._E000(31633)) + _ED3E._E000(11164));

	public virtual string FullIdInfo => FullIdInfoClean;

	public EPlayerSide Side => Profile.Info.Side;

	public BifacialTransform Transform => PlayerBones.BodyTransform;

	public _E9C4 HealthController => _healthController;

	public _E97E ActiveHealthController => _healthController as _E97E;

	public _E9CA PlayerHealthController => _healthController as _E9CA;

	public virtual float Awareness
	{
		get
		{
			return _E0CC;
		}
		set
		{
			_E0CC = value;
		}
	}

	public float DeltaTime => _E0CE();

	protected virtual Ray InteractionRay => new Ray(_playerLookRaycastTransform.position - _playerLookRaycastTransform.forward * EFTHardSettings.Instance.BEHIND_CAST, _playerLookRaycastTransform.forward);

	private int _E0DD
	{
		get
		{
			return _E09D;
		}
		set
		{
			if (_E09D < 1 && value > 0)
			{
				ExecuteSkill((Action)delegate
				{
					Skills.StimulatorNegativeBuff.Begin();
				});
			}
			if (_E09D > 0 && value < 1)
			{
				ExecuteSkill((Action)delegate
				{
					Skills.StimulatorNegativeBuff.Complete();
				});
			}
			_E09D = value;
		}
	}

	public virtual bool IsVisible
	{
		get
		{
			if (PointOfView != 0)
			{
				return OnScreen;
			}
			return true;
		}
		set
		{
		}
	}

	public virtual float SqrCameraDistance
	{
		get
		{
			if (PointOfView == EPointOfView.FirstPerson)
			{
				return 0f;
			}
			return _E8A8.Instance.SqrDistance(Transform.position);
		}
	}

	public int OwnerId => PlayerId;

	public _E267 BotsGroup
	{
		[CompilerGenerated]
		get
		{
			return _E0D4;
		}
		[CompilerGenerated]
		set
		{
			_E0D4 = value;
		}
	}

	public bool IsYourPlayer
	{
		[CompilerGenerated]
		get
		{
			return _E0D7;
		}
		[CompilerGenerated]
		protected set
		{
			_E0D7 = value;
		}
	}

	[Obsolete("Use Player.Transform instead!", true)]
	public new Transform transform => base.transform;

	public event Action<float, float, int> OnSpeedChangedEvent
	{
		[CompilerGenerated]
		add
		{
			Action<float, float, int> action = this.m__E000;
			Action<float, float, int> action2;
			do
			{
				action2 = action;
				Action<float, float, int> value2 = (Action<float, float, int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<float, float, int> action = this.m__E000;
			Action<float, float, int> action2;
			do
			{
				action2 = action;
				Action<float, float, int> value2 = (Action<float, float, int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<SightComponent> OnSightChangedEvent
	{
		[CompilerGenerated]
		add
		{
			Action<SightComponent> action = this.m__E001;
			Action<SightComponent> action2;
			do
			{
				action2 = action;
				Action<SightComponent> value2 = (Action<SightComponent>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<SightComponent> action = this.m__E001;
			Action<SightComponent> action2;
			do
			{
				action2 = action;
				Action<SightComponent> value2 = (Action<SightComponent>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event _E006 OnDamageReceived
	{
		[CompilerGenerated]
		add
		{
			_E006 obj = this.m__E019;
			_E006 obj2;
			do
			{
				obj2 = obj;
				_E006 value2 = (_E006)Delegate.Combine(obj2, value);
				obj = Interlocked.CompareExchange(ref this.m__E019, value2, obj2);
			}
			while ((object)obj != obj2);
		}
		[CompilerGenerated]
		remove
		{
			_E006 obj = this.m__E019;
			_E006 obj2;
			do
			{
				obj2 = obj;
				_E006 value2 = (_E006)Delegate.Remove(obj2, value);
				obj = Interlocked.CompareExchange(ref this.m__E019, value2, obj2);
			}
			while ((object)obj != obj2);
		}
	}

	public event Action<_EC23, EBodyPart, float> BeingHitAction
	{
		[CompilerGenerated]
		add
		{
			Action<_EC23, EBodyPart, float> action = this.m__E051;
			Action<_EC23, EBodyPart, float> action2;
			do
			{
				action2 = action;
				Action<_EC23, EBodyPart, float> value2 = (Action<_EC23, EBodyPart, float>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E051, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_EC23, EBodyPart, float> action = this.m__E051;
			Action<_EC23, EBodyPart, float> action2;
			do
			{
				action2 = action;
				Action<_EC23, EBodyPart, float> value2 = (Action<_EC23, EBodyPart, float>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E051, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action HandsChangingEvent
	{
		[CompilerGenerated]
		add
		{
			Action action = _E08C;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E08C, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E08C;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E08C, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<_E6C7> HandsChangedEvent
	{
		[CompilerGenerated]
		add
		{
			Action<_E6C7> action = _E08D;
			Action<_E6C7> action2;
			do
			{
				action2 = action;
				Action<_E6C7> value2 = (Action<_E6C7>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E08D, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_E6C7> action = _E08D;
			Action<_E6C7> action2;
			do
			{
				action2 = action;
				Action<_E6C7> value2 = (Action<_E6C7>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E08D, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event _E6FD OnPlayerDead
	{
		[CompilerGenerated]
		add
		{
			_E6FD obj = _E096;
			_E6FD obj2;
			do
			{
				obj2 = obj;
				_E6FD value2 = (_E6FD)Delegate.Combine(obj2, value);
				obj = Interlocked.CompareExchange(ref _E096, value2, obj2);
			}
			while ((object)obj != obj2);
		}
		[CompilerGenerated]
		remove
		{
			_E6FD obj = _E096;
			_E6FD obj2;
			do
			{
				obj2 = obj;
				_E6FD value2 = (_E6FD)Delegate.Remove(obj2, value);
				obj = Interlocked.CompareExchange(ref _E096, value2, obj2);
			}
			while ((object)obj != obj2);
		}
	}

	public static event Action<Player, Player, _EC23, EBodyPart> OnPlayerDeadStatic
	{
		[CompilerGenerated]
		add
		{
			Action<Player, Player, _EC23, EBodyPart> action = _E097;
			Action<Player, Player, _EC23, EBodyPart> action2;
			do
			{
				action2 = action;
				Action<Player, Player, _EC23, EBodyPart> value2 = (Action<Player, Player, _EC23, EBodyPart>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E097, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Player, Player, _EC23, EBodyPart> action = _E097;
			Action<Player, Player, _EC23, EBodyPart> action2;
			do
			{
				action2 = action;
				Action<Player, Player, _EC23, EBodyPart> value2 = (Action<Player, Player, _EC23, EBodyPart>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E097, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event _E6FE OnPlayerDeadOrUnspawn
	{
		[CompilerGenerated]
		add
		{
			_E6FE obj = _E098;
			_E6FE obj2;
			do
			{
				obj2 = obj;
				_E6FE value2 = (_E6FE)Delegate.Combine(obj2, value);
				obj = Interlocked.CompareExchange(ref _E098, value2, obj2);
			}
			while ((object)obj != obj2);
		}
		[CompilerGenerated]
		remove
		{
			_E6FE obj = _E098;
			_E6FE obj2;
			do
			{
				obj2 = obj;
				_E6FE value2 = (_E6FE)Delegate.Remove(obj2, value);
				obj = Interlocked.CompareExchange(ref _E098, value2, obj2);
			}
			while ((object)obj != obj2);
		}
	}

	public event Action<bool> OnSenseChanged
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = _E0AD;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0AD, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = _E0AD;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0AD, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action PossibleInteractionsChanged
	{
		[CompilerGenerated]
		add
		{
			Action action = _E0AE;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0AE, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E0AE;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0AE, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<EPhraseTrigger, int> PhraseSituation
	{
		[CompilerGenerated]
		add
		{
			Action<EPhraseTrigger, int> action = _E0AF;
			Action<EPhraseTrigger, int> action2;
			do
			{
				action2 = action;
				Action<EPhraseTrigger, int> value2 = (Action<EPhraseTrigger, int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0AF, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<EPhraseTrigger, int> action = _E0AF;
			Action<EPhraseTrigger, int> action2;
			do
			{
				action2 = action;
				Action<EPhraseTrigger, int> value2 = (Action<EPhraseTrigger, int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0AF, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<bool> OnGlassesChanged
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = _E0B0;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0B0, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = _E0B0;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0B0, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<Player, bool> OnInventoryOpened
	{
		[CompilerGenerated]
		add
		{
			Action<Player, bool> action = _E0B1;
			Action<Player, bool> action2;
			do
			{
				action2 = action;
				Action<Player, bool> value2 = (Action<Player, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0B1, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Player, bool> action = _E0B1;
			Action<Player, bool> action2;
			do
			{
				action2 = action;
				Action<Player, bool> value2 = (Action<Player, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0B1, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnStartQuickdrawPistol
	{
		[CompilerGenerated]
		add
		{
			Action action = _E0B2;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0B2, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E0B2;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0B2, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string, int> OnSpecialPlaceVisited
	{
		[CompilerGenerated]
		add
		{
			Action<string, int> action = _E0C6;
			Action<string, int> action2;
			do
			{
				action2 = action;
				Action<string, int> value2 = (Action<string, int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0C6, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<string, int> action = _E0C6;
			Action<string, int> action2;
			do
			{
				action2 = action;
				Action<string, int> value2 = (Action<string, int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0C6, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<AbstractHandsController, AbstractHandsController> OnHandsControllerChanged
	{
		[CompilerGenerated]
		add
		{
			Action<AbstractHandsController, AbstractHandsController> action = _E0CA;
			Action<AbstractHandsController, AbstractHandsController> action2;
			do
			{
				action2 = action;
				Action<AbstractHandsController, AbstractHandsController> value2 = (Action<AbstractHandsController, AbstractHandsController>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0CA, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<AbstractHandsController, AbstractHandsController> action = _E0CA;
			Action<AbstractHandsController, AbstractHandsController> action2;
			do
			{
				action2 = action;
				Action<AbstractHandsController, AbstractHandsController> value2 = (Action<AbstractHandsController, AbstractHandsController>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0CA, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<ExfiltrationPoint, bool> OnEpInteraction
	{
		[CompilerGenerated]
		add
		{
			Action<ExfiltrationPoint, bool> action = _E0D3;
			Action<ExfiltrationPoint, bool> action2;
			do
			{
				action2 = action;
				Action<ExfiltrationPoint, bool> value2 = (Action<ExfiltrationPoint, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0D3, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<ExfiltrationPoint, bool> action = _E0D3;
			Action<ExfiltrationPoint, bool> action2;
			do
			{
				action2 = action;
				Action<ExfiltrationPoint, bool> value2 = (Action<ExfiltrationPoint, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0D3, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static _ECD9<ItemAddress> ToItemAddress(_E673 descriptor)
	{
		return Singleton<GameWorld>.Instance.ToItemAddress(descriptor);
	}

	public _ECD9<_EB1E> FindControllerById(string id)
	{
		_EB1E obj = Singleton<GameWorld>.Instance.FindControllerById(id);
		if (obj == null)
		{
			return new _E001(id);
		}
		return obj;
	}

	private _ECD9<IEnumerable<_EAE9>> _E000(IEnumerable<_E679> itemsData)
	{
		if (itemsData == null)
		{
			return default(_ECD9<IEnumerable<_EAE9>>);
		}
		List<_EAE9> list = null;
		foreach (var (itemId, numberToDestroy, numberToPreserve) in itemsData)
		{
			if (list == null)
			{
				list = new List<_EAE9>();
			}
			_ECD9<Item> obj2 = FindItemById(itemId);
			if (obj2.Failed)
			{
				return obj2.Error;
			}
			list.Add(new _EAE9(obj2.Value, numberToDestroy, numberToPreserve));
		}
		return (_ECD9<IEnumerable<_EAE9>>)(IEnumerable<_EAE9>)list;
	}

	public _ECD9<Item> FindItemById(string itemId, bool checkDistance = true, bool checkOwnership = true)
	{
		_ECD9<(Item, GameWorld._E000)> obj = Singleton<GameWorld>.Instance.FindItemWithWorldData(itemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		Item item = obj.Value.Item1;
		return item;
	}

	public bool IsOwnerCandidateValid(string itemId, IItemOwner ownerCandidate, out _ECD1 error)
	{
		error = null;
		return true;
	}

	[CanBeNull]
	public static LootItem GetLootItem(string itemId, Vector3 position)
	{
		LootItem lootItem = GameWorld.FindLootItem(itemId, position);
		if (lootItem == null)
		{
			UnityEngine.Debug.LogError(_ED3E._E000(161303) + itemId);
		}
		return lootItem;
	}

	public _ECD6<_EB7F> ToCreateItemsOperation(_EB7E descriptor)
	{
		return _EB7F.CreateFromDescriptor(descriptor, _E0DE);
	}

	public _ECD6<_EB79> ToAddOperation(_E67B descriptor)
	{
		_ECD9<ItemAddress> obj = ToItemAddress(descriptor.To);
		if (obj.Failed)
		{
			return obj.Error;
		}
		_ECD9<Item> obj2 = FindItemById(descriptor.ItemId);
		if (obj2.Failed)
		{
			return obj2.Error;
		}
		return new _EB79(descriptor.OperationId, _E0DE, obj2.Value, obj.Value);
	}

	public _ECD6<_EB8D> ToLoadMagOperation(_E67D descriptor)
	{
		return new _EB8D(descriptor.OperationId, _E0DE, ToInventoryOperation(descriptor.InternalOperationDescriptor).Value);
	}

	public _ECD6<_EB8E> ToUnloadMagOperation(_E67E descriptor)
	{
		return new _EB8E(descriptor.OperationId, _E0DE, ToInventoryOperation(descriptor.InternalOperationDescriptor).Value);
	}

	public _ECD6<_EB93> ToRemoveOperation(_E67F descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		return new _EB93(descriptor.OperationId, _E0DE, obj.Value);
	}

	public _ECD6<_EB91> ToMoveOperation(_E687 descriptor)
	{
		_ECD9<ItemAddress> obj = ToItemAddress(descriptor.From);
		if (obj.Failed)
		{
			return obj.Error;
		}
		_ECD9<ItemAddress> obj2 = ToItemAddress(descriptor.To);
		if (obj2.Failed)
		{
			return obj2.Error;
		}
		_ECD9<Item> obj3 = FindItemById(descriptor.ItemId);
		if (obj3.Failed)
		{
			return obj3.Error;
		}
		if (!obj.Value.Equals(obj3.Value.Parent))
		{
			return new _E003(obj3.Value, obj.Value);
		}
		IItemOwner ownerOrNull = obj2.Value.GetOwnerOrNull();
		if (ownerOrNull != null && !IsOwnerCandidateValid(descriptor.ItemId, ownerOrNull, out var error))
		{
			return error;
		}
		_ECD9<IEnumerable<_EAE9>> obj4 = _E000(descriptor.DestroyedItems);
		if (obj4.Failed)
		{
			return obj4.Error;
		}
		return new _EB91(descriptor.OperationId, _E0DE, obj3.Value, obj2.Value, obj4.Value);
	}

	public _ECD6<_EB90> ToMoveAllOperation(_E688 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		return new _EB90(descriptor.OperationId, _E0DE, (_EA91)obj.Value, _E0DE.Inventory.Equipment, _E0DE);
	}

	public _ECD6<_EB97> ToSplitOperation(_E689 descriptor)
	{
		_ECD9<ItemAddress> obj = ToItemAddress(descriptor.From);
		if (obj.Failed)
		{
			return obj.Error;
		}
		_ECD9<ItemAddress> obj2 = ToItemAddress(descriptor.To);
		if (obj2.Failed)
		{
			return obj2.Error;
		}
		_ECD9<Item> obj3 = FindItemById(descriptor.ItemId);
		if (obj3.Failed)
		{
			return obj3.Error;
		}
		if (!obj.Value.Equals(obj3.Value.Parent))
		{
			return new _E003(obj3.Value, obj.Value);
		}
		return new _EB97(descriptor.OperationId, _E0DE, obj3.Value, obj2.Value, descriptor.Count, _E0DE, descriptor.CloneId);
	}

	public _ECD6<_EB8F> ToMergeOperation(_E68A descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		_ECD9<Item> obj2 = FindItemById(descriptor.Item1Id);
		if (obj2.Failed)
		{
			return obj2.Error;
		}
		return new _EB8F(descriptor.OperationId, _E0DE, obj.Value, obj2.Value);
	}

	public _ECD6<_EB9E> ToTransferOperation(_E68B descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		_ECD9<Item> obj2 = FindItemById(descriptor.Item1Id);
		if (obj2.Failed)
		{
			return obj2.Error;
		}
		return new _EB9E(descriptor.OperationId, _E0DE, obj.Value, obj2.Value, descriptor.Count);
	}

	public _ECD6<_EB99> ToSwapOperation(_E68C descriptor)
	{
		_ECD9<ItemAddress> obj = ToItemAddress(descriptor.To);
		if (obj.Failed)
		{
			return obj.Error;
		}
		_ECD9<Item> obj2 = FindItemById(descriptor.ItemId);
		if (obj2.Failed)
		{
			return obj2.Error;
		}
		_ECD9<ItemAddress> obj3 = ToItemAddress(descriptor.To1);
		if (obj3.Failed)
		{
			return obj3.Error;
		}
		_ECD9<Item> obj4 = FindItemById(descriptor.Item1Id);
		if (obj4.Failed)
		{
			return obj4.Error;
		}
		_ECD9<IEnumerable<_EAE9>> obj5 = _E000(descriptor.DestroyedItems);
		if (obj5.Failed)
		{
			return obj5.Error;
		}
		return new _EB99(descriptor.OperationId, _E0DE, obj2.Value, obj.Value, obj4.Value, obj3.Value, obj5.Value);
	}

	public _ECD6<_EB9B> ToThrowOperation(_E68D descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		_ECD9<IEnumerable<_EAE9>> obj2 = _E000(descriptor.DestroyedItems);
		if (obj2.Failed)
		{
			return obj2.Error;
		}
		return new _EB9B(descriptor.OperationId, _E0DE, obj.Value, obj2.Value, this, descriptor.DownDirection);
	}

	public _ECD6<_EB9C> ToToggleOperation(_E68E descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		TogglableComponent itemComponent = obj.Value.GetItemComponent<TogglableComponent>();
		if (itemComponent == null)
		{
			return new _E002(obj.Value, typeof(TogglableComponent));
		}
		if (itemComponent.On == descriptor.Value)
		{
			return new _E004(itemComponent, descriptor.Value);
		}
		return new _EB9D(descriptor.OperationId, _E0DE, itemComponent, this);
	}

	public _ECD6<_EB89> ToFoldOperation(_E68F descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		if (!_EB29.CanFold(obj.Value, out var foldable))
		{
			if (foldable == null)
			{
				return new _E002(obj.Value, typeof(FoldableComponent));
			}
			return new _ECD2(_ED3E._E000(161316));
		}
		if (foldable.Folded == descriptor.Value)
		{
			return new _E004(foldable, descriptor.Value);
		}
		return new _EB89(descriptor.OperationId, _E0DE, foldable, descriptor.Value);
	}

	public _ECD6<_EB96> ToShotOperation(_E690 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId, checkDistance: false, checkOwnership: false);
		if (obj.Failed)
		{
			return obj.Error;
		}
		FaceShieldComponent itemComponent = obj.Value.GetItemComponent<FaceShieldComponent>();
		if (itemComponent == null)
		{
			return new _E002(obj.Value, typeof(FaceShieldComponent));
		}
		return new _EB96(descriptor.OperationId, _E0DE, itemComponent);
	}

	public _ECD6<_EB95> ToSetupItemOperation(_E691 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		return new _EB95(descriptor.OperationId, _E0DE, obj.Value, descriptor.ZoneId, descriptor.Position, descriptor.Rotation, this, descriptor.SetupTime);
	}

	public _ECD6<_EB9A> ToTagOperation(_E69A descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		TagComponent itemComponent = obj.Value.GetItemComponent<TagComponent>();
		if (itemComponent == null)
		{
			return new _E002(obj.Value, typeof(TagComponent));
		}
		return new _EB9A(descriptor.OperationId, _E0DE, itemComponent, descriptor.TagName, descriptor.TagColor);
	}

	public _ECD6<_EB98> ToStationaryOperation(_E69B descriptor)
	{
		StationaryWeapon stationaryWeapon = Singleton<GameWorld>.Instance.FindStationaryWeapon(descriptor.WeaponId);
		return new _EB98(descriptor.OperationId, _E0DE, stationaryWeapon);
	}

	public _ECD6<_EBA0> ToWeaponRechamberOperation(_E69C descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.WeaponId);
		return new _EBA0(descriptor.OperationId, _E0DE, obj.Value as Weapon);
	}

	public _ECD6<_EB7B> ToApplyKeyOperation(_E692 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		KeyComponent itemComponent = obj.Value.GetItemComponent<KeyComponent>();
		if (itemComponent == null)
		{
			return new _E002(obj.Value, typeof(KeyComponent));
		}
		_ECD9<Item> obj2 = FindItemById(descriptor.TargetItemId);
		if (obj2.Failed)
		{
			return obj2.Error;
		}
		LockableComponent itemComponent2 = obj2.Value.GetItemComponent<LockableComponent>();
		if (itemComponent2 == null)
		{
			return new _E002(obj2.Value, typeof(LockableComponent));
		}
		return new _EB7B(descriptor.OperationId, _E0DE, itemComponent, itemComponent2);
	}

	public _ECD6<_EB80> ToCreateMapMarkerOperation(_E694 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.MapItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		MapComponent itemComponent = obj.Value.GetItemComponent<MapComponent>();
		return new _EB80(descriptor.OperationId, _E0DE, itemComponent, descriptor.MapMarker);
	}

	public _ECD6<_EB84> ToEditMapMarkerOperation(_E695 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.MapItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		MapComponent itemComponent = obj.Value.GetItemComponent<MapComponent>();
		return new _EB84(descriptor.OperationId, _E0DE, itemComponent, descriptor.X, descriptor.Y, descriptor.MapMarker);
	}

	public _ECD6<_EB81> ToDeleteMapMarkerOperation(_E696 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.MapItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		MapComponent itemComponent = obj.Value.GetItemComponent<MapComponent>();
		return new _EB81(descriptor.OperationId, _E0DE, itemComponent, descriptor.X, descriptor.Y);
	}

	public _ECD6<_EB78> ToAddNoteOperation(_E697 descriptor)
	{
		return new _EB78(descriptor.OperationId, _E0DE, Profile.Notes, descriptor.Note);
	}

	public _ECD6<_EB85> ToEditNoteOperation(_E698 descriptor)
	{
		return new _EB85(descriptor.OperationId, _E0DE, Profile.Notes, descriptor.Index, descriptor.Note);
	}

	public _ECD6<_EB82> ToDeleteNoteOperation(_E699 descriptor)
	{
		return new _EB82(descriptor.OperationId, _E0DE, Profile.Notes, descriptor.Index);
	}

	public _ECD6<_EB88> ToExamineOperation(_E680 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		return new _EB88(descriptor.OperationId, _E0DE, Profile, obj.Value);
	}

	public _ECD6<_EBA2> ToQuestAcceptOperation(_E765 descriptor)
	{
		return _EBA2.FromDescriptor(descriptor, _E0DE, _E0DC);
	}

	public _ECD6<_EBA3> ToQuestFinishOperation(_E766 descriptor)
	{
		return _EBA3.FromDescriptor(descriptor, _E0DE, _E0DC);
	}

	public _ECD6<_EBA4> ToQuestHandoverOperation(_E767 descriptor)
	{
		return _EBA4.FromDescriptor(descriptor, _E0DE, _E0DC);
	}

	public _ECD6<_EB87> ToExamineMalfunctionOperation(_E681 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		return new _EB87(descriptor.OperationId, _E0DE, Profile.Id, obj.Value);
	}

	public _ECD6<_EB86> ToExamineMalfTypeOperation(_E682 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Error != null)
		{
			return obj.Error;
		}
		return new _EB86(descriptor.OperationId, _E0DE, Profile.Id, obj.Value);
	}

	public _ECD6<_EB7D> ToCheckMagazineOperation(_E683 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		return new _EB7D(descriptor.OperationId, _E0DE, descriptor.CheckStatus, descriptor.SkillLevel, obj.Value, Profile);
	}

	private _ECD6<_EB7C> _E001(_E684 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		return new _EB7C(descriptor.OperationId, _E0DE, obj.Value, descriptor.Index);
	}

	private _ECD6<_EB9F> _E002(_E685 descriptor)
	{
		_ECD9<Item> obj = FindItemById(descriptor.ItemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		return new _EB9F(descriptor.OperationId, _E0DE, obj.Value, descriptor.Index);
	}

	private _ECD6<_EB8B> _E003(_E686 descriptor)
	{
		return new _EB8B(descriptor.OperationId, _E0DE, descriptor.Items, descriptor.TraderId);
	}

	public _ECD5 ToInventoryOperation(_E67A descriptor)
	{
		if (descriptor != null)
		{
			if (descriptor is _E67B obj)
			{
				_E67B descriptor2 = obj;
				return ToAddOperation(descriptor2);
			}
			if (descriptor is _E67D obj2)
			{
				_E67D descriptor3 = obj2;
				return ToLoadMagOperation(descriptor3);
			}
			if (descriptor is _E67E obj3)
			{
				_E67E descriptor4 = obj3;
				return ToUnloadMagOperation(descriptor4);
			}
			if (descriptor is _E67F obj4)
			{
				_E67F descriptor5 = obj4;
				return ToRemoveOperation(descriptor5);
			}
			if (descriptor is _E687 obj5)
			{
				_E687 descriptor6 = obj5;
				return ToMoveOperation(descriptor6);
			}
			if (descriptor is _E688 obj6)
			{
				_E688 descriptor7 = obj6;
				return ToMoveAllOperation(descriptor7);
			}
			if (descriptor is _E689 obj7)
			{
				_E689 descriptor8 = obj7;
				return ToSplitOperation(descriptor8);
			}
			if (descriptor is _E68A obj8)
			{
				_E68A descriptor9 = obj8;
				return ToMergeOperation(descriptor9);
			}
			if (descriptor is _E68B obj9)
			{
				_E68B descriptor10 = obj9;
				return ToTransferOperation(descriptor10);
			}
			if (descriptor is _E68C obj10)
			{
				_E68C descriptor11 = obj10;
				return ToSwapOperation(descriptor11);
			}
			if (descriptor is _E68D obj11)
			{
				_E68D descriptor12 = obj11;
				return ToThrowOperation(descriptor12);
			}
			if (descriptor is _E68E obj12)
			{
				_E68E descriptor13 = obj12;
				return ToToggleOperation(descriptor13);
			}
			if (descriptor is _E68F obj13)
			{
				_E68F descriptor14 = obj13;
				return ToFoldOperation(descriptor14);
			}
			if (descriptor is _E690 obj14)
			{
				_E690 descriptor15 = obj14;
				return ToShotOperation(descriptor15);
			}
			if (descriptor is _E692 obj15)
			{
				_E692 descriptor16 = obj15;
				return ToApplyKeyOperation(descriptor16);
			}
			if (descriptor is _E694 obj16)
			{
				_E694 descriptor17 = obj16;
				return ToCreateMapMarkerOperation(descriptor17);
			}
			if (descriptor is _E695 obj17)
			{
				_E695 descriptor18 = obj17;
				return ToEditMapMarkerOperation(descriptor18);
			}
			if (descriptor is _E696 obj18)
			{
				_E696 descriptor19 = obj18;
				return ToDeleteMapMarkerOperation(descriptor19);
			}
			if (descriptor is _E697 obj19)
			{
				_E697 descriptor20 = obj19;
				return ToAddNoteOperation(descriptor20);
			}
			if (descriptor is _E698 obj20)
			{
				_E698 descriptor21 = obj20;
				return ToEditNoteOperation(descriptor21);
			}
			if (descriptor is _E699 obj21)
			{
				_E699 descriptor22 = obj21;
				return ToDeleteNoteOperation(descriptor22);
			}
			if (descriptor is _E680 obj22)
			{
				_E680 descriptor23 = obj22;
				return ToExamineOperation(descriptor23);
			}
			if (descriptor is _E681 obj23)
			{
				_E681 descriptor24 = obj23;
				return ToExamineMalfunctionOperation(descriptor24);
			}
			if (descriptor is _E682 obj24)
			{
				_E682 descriptor25 = obj24;
				return ToExamineMalfTypeOperation(descriptor25);
			}
			if (descriptor is _E683 obj25)
			{
				_E683 descriptor26 = obj25;
				return ToCheckMagazineOperation(descriptor26);
			}
			if (descriptor is _E684 obj26)
			{
				_E684 descriptor27 = obj26;
				return _E001(descriptor27);
			}
			if (descriptor is _E685 obj27)
			{
				_E685 descriptor28 = obj27;
				return _E002(descriptor28);
			}
			if (descriptor is _E686 obj28)
			{
				_E686 descriptor29 = obj28;
				return _E003(descriptor29);
			}
			if (descriptor is _E691 obj29)
			{
				_E691 descriptor30 = obj29;
				return ToSetupItemOperation(descriptor30);
			}
			if (descriptor is _E69A obj30)
			{
				_E69A descriptor31 = obj30;
				return ToTagOperation(descriptor31);
			}
			if (descriptor is _E69B obj31)
			{
				_E69B descriptor32 = obj31;
				return ToStationaryOperation(descriptor32);
			}
			if (descriptor is _E69C obj32)
			{
				_E69C descriptor33 = obj32;
				return ToWeaponRechamberOperation(descriptor33);
			}
			if (descriptor is _E765 obj33)
			{
				_E765 descriptor34 = obj33;
				return ToQuestAcceptOperation(descriptor34);
			}
			if (descriptor is _E766 obj34)
			{
				_E766 descriptor35 = obj34;
				return ToQuestFinishOperation(descriptor35);
			}
			if (descriptor is _E767 obj35)
			{
				_E767 descriptor36 = obj35;
				return ToQuestHandoverOperation(descriptor36);
			}
			if (descriptor is _EB7E obj36)
			{
				_EB7E descriptor37 = obj36;
				return ToCreateItemsOperation(descriptor37);
			}
		}
		throw new _E69D(_ED3E._E000(161400) + descriptor.GetType());
	}

	public IAnimator GetBodyAnimatorCommon()
	{
		if (_E2B6.Config.UseSpiritPlayer && Spirit.IsActive)
		{
			return Spirit.BodyAnimatorWrapper;
		}
		return _animators[0];
	}

	public IAnimator GetArmsAnimatorCommon()
	{
		if (_E2B6.Config.UseSpiritPlayer && Spirit.IsActive)
		{
			return Spirit.ArmsAnimator;
		}
		return _animators[1];
	}

	public RuntimeAnimatorController GetArmsAnimatorControllerCommon()
	{
		IAnimator armsAnimatorCommon = GetArmsAnimatorCommon();
		if (armsAnimatorCommon.runtimeAnimatorController is AnimatorOverrideController)
		{
			return ((AnimatorOverrideController)armsAnimatorCommon.runtimeAnimatorController).runtimeAnimatorController;
		}
		return armsAnimatorCommon.runtimeAnimatorController;
	}

	public _E33A GetCharacterControllerCommon()
	{
		if (_E2B6.Config.UseSpiritPlayer && Spirit.IsActive)
		{
			return Spirit.CharacterController;
		}
		return _characterController;
	}

	public TriggerColliderSearcher GetTriggerColliderSearcher()
	{
		if (_E2B6.Config.UseSpiritPlayer && Spirit.IsActive)
		{
			return Spirit.TriggerColliderSearcher;
		}
		return _triggerColliderSearcher;
	}

	public void AddMouseSensitivityModifier(EMouseSensitivityModifier type, float value)
	{
		_mouseSensitivityModifiers[type] = value;
		_E004();
	}

	public void RemoveMouseSensitivityModifier(EMouseSensitivityModifier type)
	{
		_mouseSensitivityModifiers.Remove(type);
		_E004();
	}

	private void _E004()
	{
		_mouseSensitivityModifier = 0f;
		foreach (float value in _mouseSensitivityModifiers.Values)
		{
			_mouseSensitivityModifier += value;
		}
	}

	public float GetRotationMultiplier()
	{
		if (HandsController != null && HandsController.IsAiming)
		{
			if (!(HandsController.AimingSensitivity >= float.Epsilon))
			{
				return GetAimingSensitivity();
			}
			return HandsController.AimingSensitivity;
		}
		return GetSensitivity() * (1f + _mouseSensitivityModifier);
	}

	public float GetCharacterSpeedMultiplier()
	{
		return 1f;
	}

	private void _E005()
	{
		CreateMovementContext();
		PoseMemo = MovementContext.PoseLevel;
		this.m__E010 = MovementContext.CharacterMovementSpeed;
	}

	protected virtual void CreateMovementContext()
	{
		LayerMask mOVEMENT_MASK = EFTHardSettings.Instance.MOVEMENT_MASK;
		MovementContext = _E70F.Create(this, GetBodyAnimatorCommon, GetCharacterControllerCommon, mOVEMENT_MASK);
	}

	internal void _E006(float dir)
	{
		if (CurrentLeanType != LeanType.SlowLean || Mathf.Approximately(this.m__E011, 0f))
		{
			CurrentLeanType = LeanType.NormalLean;
			float num = dir * 5f;
			float dir2 = ((_E0D8 * dir).Positive() ? 0f : num);
			_E008(dir2);
		}
	}

	internal void _E007(float fromDir)
	{
		if ((CurrentLeanType != LeanType.SlowLean || Mathf.Approximately(this.m__E011, 0f)) && (_E0D8 * fromDir).Positive())
		{
			_E008(0f);
		}
	}

	private void _E008(float dir)
	{
		this.m__E00F = dir;
		CurrentState.SetTilt(this.m__E00F);
	}

	public void SlowLean(float x)
	{
		if (Mathf.Abs(x) > 0f)
		{
			CurrentLeanType = LeanType.SlowLean;
		}
		x *= 0.1f;
		CurrentState.SetTilt(MovementContext.Tilt + x);
		this.m__E00F = MovementContext.Tilt;
		this.m__E011 = x;
	}

	public void ToggleBlindFire(float dir)
	{
		if (Math.Sign(MovementContext.BlindFire) == Math.Sign(dir))
		{
			StopBlindFire();
		}
		else
		{
			CurrentState.BlindFire(Math.Sign(dir));
		}
	}

	public void StopBlindFire()
	{
		CurrentState.BlindFire(0);
	}

	public void ToggleStep(int direction)
	{
		if (MovementContext.Step != direction)
		{
			CurrentState.SetStep(direction);
		}
		else
		{
			ReturnFromStep(direction);
		}
	}

	public void ReturnFromStep(int direction)
	{
		if (MovementContext.Step == direction)
		{
			CurrentState.SetStep(0);
		}
	}

	private void _E009(float fallHeight, float jumpHeight)
	{
		LandingAdjustments(fallHeight);
		PlayGroundedSound(fallHeight, jumpHeight);
	}

	protected virtual void LandingAdjustments(float d)
	{
		if (d < Singleton<_E5CB>.Instance.Inertia.FallThreshold)
		{
			return;
		}
		float num = EFTHardSettings.Instance.SpeedLimitDuration.Evaluate(d);
		if (!(num < float.Epsilon))
		{
			float num2 = _E0D9.SpeedLimitAfterFall.Evaluate(d);
			if (!(num2 >= 1f))
			{
				MovementContext.ChangeSpeedLimit(num2, ESpeedLimit.Fall, num);
			}
		}
	}

	public virtual void Move(Vector2 direction)
	{
		CurrentState.Move(direction);
		if (direction.sqrMagnitude >= float.Epsilon)
		{
			this.m__E012 = Time.time;
		}
		InputDirection = direction;
	}

	public void ChangePose(float poseDelta)
	{
		CurrentState.ChangePose(poseDelta);
		PoseMemo = PoseLevel;
		if (poseDelta != 0f)
		{
			this.m__E012 = Time.time;
		}
	}

	internal void _E00A()
	{
		if (!IsInPronePose && !(PoseLevel >= 0.5f))
		{
			float num = ((PoseMemo < 0.5f) ? 1f : PoseMemo);
			CurrentState.ChangePose(num - PoseLevel);
		}
	}

	internal void _E00B()
	{
		float num = ((!(Math.Abs(PoseMemo - PoseLevel) < 1E-06f)) ? PoseMemo : ((PoseLevel >= 0.5f) ? 0f : 1f));
		if (IsInPronePose)
		{
			num = PoseLevel;
		}
		CurrentState.ChangePose(num - PoseLevel);
	}

	public void ChangeSpeed(float speedDelta)
	{
		CurrentState.ChangeSpeed(speedDelta);
	}

	public void RaiseChangeSpeedEvent()
	{
		this.m__E000?.Invoke(Physical.Sprinting ? MovementContext.CharacterMovementSpeed : MovementContext.ClampedSpeed, MovementContext.MaxSpeed, MovementContext.CovertNoiseLevel);
	}

	public void RaiseSightChangedEvent(SightComponent sightComp)
	{
		this.m__E001?.Invoke(sightComp);
	}

	public virtual void AddStateSpeedLimit(float speedDelta, ESpeedLimit cause)
	{
		MovementContext?.AddStateSpeedLimit(speedDelta, cause);
	}

	public virtual void UpdateSpeedLimit(float speedDelta, ESpeedLimit cause)
	{
		MovementContext?.ChangeSpeedLimit(speedDelta, cause);
	}

	public virtual void UpdateSpeedLimit(float speedDelta, ESpeedLimit cause, float duration)
	{
		MovementContext?.ChangeSpeedLimit(speedDelta, cause, duration);
	}

	public virtual void RemoveStateSpeedLimit(ESpeedLimit cause)
	{
		MovementContext.RemoveStateSpeedLimit(cause);
	}

	internal void _E00C()
	{
		if (!MovementContext.ClampedSpeed.Positive())
		{
			ChangeSpeed(this.m__E010 - MovementContext.CharacterMovementSpeed);
		}
	}

	internal void _E00D()
	{
		if (MovementContext.ClampedSpeed.Positive())
		{
			this.m__E010 = MovementContext.CharacterMovementSpeed;
			ChangeSpeed(0f - this.m__E010);
		}
		else
		{
			_E00C();
		}
	}

	public virtual void Rotate(Vector2 deltaRotation, bool ignoreClamp = false)
	{
		if (deltaRotation.IsAnyComponentInfinity() || deltaRotation.IsAnyComponentNaN())
		{
			UnityEngine.Debug.LogErrorFormat(_ED3E._E000(161430), deltaRotation);
			return;
		}
		CurrentState.Rotate(deltaRotation, ignoreClamp);
		if (deltaRotation.sqrMagnitude >= float.Epsilon)
		{
			this.m__E012 = Time.time;
		}
	}

	public virtual void Look(float deltaLookY, float deltaLookX, bool withReturn = true)
	{
		bool num = HandsController != null && HandsController.IsAiming && !IsAI;
		EFTHardSettings instance = EFTHardSettings.Instance;
		Vector2 mOUSE_LOOK_HORIZONTAL_LIMIT = instance.MOUSE_LOOK_HORIZONTAL_LIMIT;
		Vector2 mOUSE_LOOK_VERTICAL_LIMIT = instance.MOUSE_LOOK_VERTICAL_LIMIT;
		if (num)
		{
			if (this.m__E013 != this.m__E008)
			{
				this.m__E013 = this.m__E008;
				int value = Singleton<_E7DE>.Instance.Game.Settings.FieldOfView.Value;
				float x = (this.m__E008 ? ((float)value) : (ProceduralWeaponAnimation.CurrentScope.IsOptic ? 35f : ((float)value - 15f)));
				_E8A8.Instance.SetFov(x, 1f);
			}
			mOUSE_LOOK_HORIZONTAL_LIMIT *= instance.MOUSE_LOOK_LIMIT_IN_AIMING_COEF;
		}
		Vector3 eulerAngles = ProceduralWeaponAnimation.HandsContainer.CameraTransform.eulerAngles;
		if (eulerAngles.x >= 50f && eulerAngles.x <= 90f && MovementContext.IsSprintEnabled)
		{
			mOUSE_LOOK_VERTICAL_LIMIT.y = 0f;
		}
		this.m__E005 = Mathf.Clamp(this.m__E005 - deltaLookY, mOUSE_LOOK_HORIZONTAL_LIMIT.x, mOUSE_LOOK_HORIZONTAL_LIMIT.y);
		this.m__E006 = Mathf.Clamp(this.m__E006 + deltaLookX, mOUSE_LOOK_VERTICAL_LIMIT.x, mOUSE_LOOK_VERTICAL_LIMIT.y);
		float x2 = ((this.m__E006 > 0f) ? (this.m__E006 * (1f - this.m__E005 / mOUSE_LOOK_HORIZONTAL_LIMIT.y * (this.m__E005 / mOUSE_LOOK_HORIZONTAL_LIMIT.y))) : this.m__E006);
		if (this.m__E00A)
		{
			this.m__E009 = false;
			this.m__E00A = false;
		}
		if (this.m__E007)
		{
			this.m__E008 = false;
			this.m__E007 = false;
			this.m__E009 = true;
			deltaLookY = 0f;
			deltaLookX = 0f;
		}
		if (Math.Abs(deltaLookY) >= float.Epsilon && Math.Abs(deltaLookX) >= float.Epsilon)
		{
			this.m__E008 = true;
		}
		if (!this.m__E008 && withReturn)
		{
			if (Mathf.Abs(this.m__E005) > 0.01f)
			{
				this.m__E005 = Mathf.Lerp(this.m__E005, 0f, Time.deltaTime * 15f);
			}
			else
			{
				this.m__E005 = 0f;
			}
			if (Mathf.Abs(this.m__E006) > 0.01f)
			{
				this.m__E006 = Mathf.Lerp(this.m__E006, 0f, Time.deltaTime * 15f);
			}
			else
			{
				this.m__E006 = 0f;
			}
		}
		if (!this.m__E009 && this.m__E005 != 0f && this.m__E006 != 0f)
		{
			this.m__E00B = true;
		}
		else
		{
			this.m__E00B = false;
		}
		if (this.m__E005 == 0f && this.m__E006 == 0f)
		{
			this.m__E00A = true;
		}
		HeadRotation = new Vector3(x2, this.m__E005, 0f);
		ProceduralWeaponAnimation.SetHeadRotation(HeadRotation);
	}

	public void ResetLookDirection()
	{
		this.m__E007 = true;
	}

	public void Jump()
	{
		CurrentState.Jump();
	}

	public void EnableSprint(bool enable)
	{
		CurrentState.EnableSprint(enable);
	}

	public void ToggleSprint()
	{
		bool enable = !Physical.Sprinting;
		CurrentState.EnableSprint(enable, isToggle: true);
	}

	public void ToggleHoldingBreath()
	{
		CurrentState.EnableBreath(!Physical.HoldingBreath);
	}

	public void StopHoldingBreath()
	{
		if (Physical.HoldingBreath)
		{
			CurrentState.EnableBreath(enable: false);
		}
	}

	private void _E00E(float deltaTime)
	{
		MovementContext.ManualUpdate(deltaTime);
	}

	public void HeightInterpolation(float timeDeltatime)
	{
		if (!Mathf.Approximately(timeDeltatime, 0f))
		{
			float num = (IsInPronePose ? 0f : (Transform.position.y - _prevHeight));
			float num2 = Mathf.SmoothDamp(PlayerBones.AnimatedTransform.localPosition.y - num, 0f, ref this.m__E014, this.m__E015, 1000000f, timeDeltatime);
			PlayerBones.AnimatedTransform.localPosition = new Vector3(PlayerBones.AnimatedTransform.localPosition.x, Mathf.Clamp(num2, -0.2f, 0.2f), PlayerBones.AnimatedTransform.localPosition.z);
			this.m__E015 = Mathf.Lerp(this.m__E015, (Mathf.Abs(num2) > this.m__E016) ? (HeightSmoothTime * 0.3f) : HeightSmoothTime, timeDeltatime);
			this.m__E016 = Mathf.Abs(num2);
		}
	}

	private void _E00F()
	{
		if (!Mathf.Approximately(PlayerBones.AnimatedTransform.localPosition.y, 0f))
		{
			PlayerBones.AnimatedTransform.localPosition = new Vector3(PlayerBones.AnimatedTransform.localPosition.x, 0f, PlayerBones.AnimatedTransform.localPosition.z);
		}
		_prevHeight = Transform.position.y;
		this.m__E016 = 0f;
	}

	public virtual void ToggleProne()
	{
		if (!MovementContext.IsAnimatorInteractionOn)
		{
			CurrentState.Prone();
		}
	}

	internal void _E010(bool isObserver = false)
	{
		if (_E0DE.Inventory.Equipment.GetSlot(EquipmentSlot.Headwear).ContainedItem is _EA40 thisItem)
		{
			TogglableComponent togglableComponent = thisItem.GetItemComponentsInChildren<TogglableComponent>().FirstOrDefault();
			if (togglableComponent != null)
			{
				_E0DE.TryRunNetworkTransaction(togglableComponent.Set(!togglableComponent.On, simulate: true));
			}
		}
	}

	internal virtual void _E06F(WorldInteractiveObject door, _EBFE interactionResult, Action callback)
	{
		EInteractionType interactionType = interactionResult.InteractionType;
		UnityEngine.Debug.LogFormat(_ED3E._E000(161449), interactionType);
		CurrentState.StartDoorInteraction(door, interactionResult, callback);
		UpdateInteractionCast();
	}

	internal virtual void _E070(WorldInteractiveObject door, _EBFE interactionResult)
	{
		if (!(door == null))
		{
			CurrentState.ExecuteDoorInteraction(door, interactionResult, null, this);
		}
	}

	internal virtual void _E071(string itemId, string zoneId, bool successful)
	{
		PlantItem(itemId, zoneId, successful);
	}

	protected void PlantItem(string itemId, string zoneId, bool successful)
	{
		if (successful)
		{
			Profile.ItemDroppedAtPlace(itemId, zoneId);
		}
	}

	internal virtual void _E072(Item item, string zone)
	{
		PlantItem(item.TemplateId, zone, successful: true);
	}

	public virtual void OperateStationaryWeapon(StationaryWeapon stationaryWeapon, _E6DB.EStationaryCommand command)
	{
		switch (command)
		{
		case _E6DB.EStationaryCommand.Occupy:
			if (Vector3.Distance(Position, stationaryWeapon.transform.position) > 2f)
			{
				UnityEngine.Debug.LogErrorFormat(GetPlayer, _ED3E._E000(161514), GetPlayer.FullIdInfo, Position.ToString(_ED3E._E000(56089)), stationaryWeapon.Item.ShortName, stationaryWeapon.Id, stationaryWeapon.transform.position);
			}
			if (stationaryWeapon.IsAvailable(ProfileId))
			{
				stationaryWeapon.SetOperator(ProfileId);
				MovementContext.StationaryWeapon = stationaryWeapon;
				MovementContext.InteractionParameters = stationaryWeapon.GetInteractionParameters();
				MovementContext.PlayerAnimatorSetApproached(b: false);
				MovementContext.PlayerAnimatorSetStationary(b: true);
				MovementContext.PlayerAnimatorSetStationaryAnimation((int)stationaryWeapon.Animation);
			}
			break;
		case _E6DB.EStationaryCommand.Leave:
			if (true)
			{
				if (ActiveSlot != null && ActiveSlot.ContainedItem != null)
				{
					SetInHands(ActiveSlot.ContainedItem, delegate
					{
					});
				}
				else
				{
					SetFirstAvailableItem(delegate
					{
					});
				}
			}
			break;
		default:
			MovementContext.PlayerAnimatorSetStationary(b: false);
			if (MovementContext.StationaryWeapon != null)
			{
				MovementContext.StationaryWeapon.Unlock(ProfileId);
			}
			break;
		}
	}

	public void FastForwardToStationaryWeapon(Item item, Vector2 stationaryRotation, Quaternion playerRotation, Quaternion stationaryPlayerRotation)
	{
		StationaryWeapon stationaryWeapon = Singleton<GameWorld>.Instance.FindStationaryWeaponByItemId(item.Id);
		if (!(stationaryWeapon == null))
		{
			stationaryWeapon.SetOperator(ProfileId);
			MovementContext.StationaryWeapon = stationaryWeapon;
			Teleport(stationaryWeapon.GetInteractionParameters().InteractionPosition);
			bool flag = BodyAnimatorCommon.enabled;
			bool keepAnimatorControllerStateOnDisable = BodyAnimatorCommon.keepAnimatorControllerStateOnDisable;
			BodyAnimatorCommon.keepAnimatorControllerStateOnDisable = true;
			BodyAnimatorCommon.enabled = false;
			MovementContext.PlayerAnimatorSetApproached(b: true);
			MovementContext.PlayerAnimatorSetStationary(b: true);
			MovementContext.PlayerAnimatorSetStationaryAnimation((int)stationaryWeapon.Animation);
			Transform.rotation = stationaryPlayerRotation;
			MovementContext.Rotation = stationaryRotation;
			MovementContext.UpdateStationaryDeltaAngle();
			for (int i = 0; i < 150; i++)
			{
				BodyAnimatorCommon.Update(0.01f);
			}
			BodyAnimatorCommon.enabled = flag;
			BodyAnimatorCommon.keepAnimatorControllerStateOnDisable = keepAnimatorControllerStateOnDisable;
			Teleport(stationaryWeapon.GetInteractionParameters().InteractionPosition);
			if (stationaryWeapon.Animation == StationaryWeapon.EStationaryAnimationType.AGS_17)
			{
				Vector3 eulerAngles = playerRotation.eulerAngles;
				Transform.rotation = Quaternion.Euler(0f, eulerAngles.y, eulerAngles.z);
			}
			else
			{
				Transform.rotation = stationaryPlayerRotation;
			}
		}
	}

	public void SetRadioTransmitterView(RadioTransmitterView rtView)
	{
		this.m__E047 = rtView;
	}

	protected void ReceiveDamage(float damage, EBodyPart part, EDamageType type, float absorbed, MaterialType special)
	{
		this.m__E019?.Invoke(damage, part, type, absorbed, special);
	}

	public void VisualPass()
	{
		if (CustomAnimationsAreProcessing)
		{
			return;
		}
		float num = 0f;
		if (PointOfView != 0)
		{
			num = _E8A8.Instance.Distance(Transform.position);
		}
		bool flag = PointOfView == EPointOfView.FirstPerson || (_E2B6.Config.UseSpiritPlayer && !Spirit.IsActive) || (IsVisible && num <= EFTHardSettings.Instance.CULL_GROUNDER);
		if ((_armsupdated || ArmsUpdateMode == EUpdateMode.Auto) && flag && (EnabledAnimators & EAnimatorMask.Procedural) != 0)
		{
			ProceduralWeaponAnimation.ProcessEffectors((_nFixedFrames > 0) ? _fixedTime : _armsTime, Mathf.Max(0, _nFixedFrames), Motion, Velocity);
			PlayerBones.Offset = ProceduralWeaponAnimation.HandsContainer.WeaponRootAnim.localPosition;
			PlayerBones.DeltaRotation = ProceduralWeaponAnimation.HandsContainer.WeaponRootAnim.localRotation;
		}
		if (_bodyupdated)
		{
			if (flag)
			{
				RestoreIKPos();
				HeightInterpolation(_bodyTime);
				FBBIKUpdate(num);
				MouseLook();
				if ((EnabledAnimators & EAnimatorMask.IK) != 0)
				{
					float num2 = ((PointOfView == EPointOfView.FirstPerson) ? _E019(_E712.FIRST_PERSON_CURVE_WEIGHT) : 1f);
					this.m__E01E = 1f - _E019(_E712.RIGHT_HAND_WEIGHT) * num2;
					this.m__E01F = 1f - _E019(_E712.LEFT_HAND_WEIGHT) * num2;
					ThirdPersonWeaponRootAuthority = _E019(_E712.WEAPON_ROOT_3RD) * num2;
					if (PointOfView == EPointOfView.FirstPerson)
					{
						this.m__E021 = ((this.m__E021 > this.m__E01F) ? this.m__E01F : Mathf.SmoothDamp(this.m__E021, this.m__E01F, ref this.m__E01B, 0.2f));
						PlayerBones.SetShoulders(1f - _E019(_E712.LEFT_SHOULDER_WEIGHT), 1f - _E019(_E712.RIGHT_SHOULDER_WEIGHT));
					}
					else
					{
						_E017(num);
					}
					if (_armsupdated || ArmsUpdateMode == EUpdateMode.Auto)
					{
						float str = ThirdPersonWeaponRootAuthority;
						if (PointOfView == EPointOfView.ThirdPerson && MovementContext.StationaryWeapon != null)
						{
							str = 0f;
						}
						PlayerBones.ShiftWeaponRoot(_bodyTime, PointOfView, str);
					}
					PlayerBones.RotateHead(ProceduralWeaponAnimation.ExternalHeadRotation, ProceduralWeaponAnimation.GetHeadRotation());
					HandPosers[0].weight = this.m__E01F;
					this.m__E03F[0].solver.IKRotationWeight = (this.m__E03F[0].solver.IKPositionWeight = this.m__E01F);
					this.m__E03F[1].solver.IKRotationWeight = (this.m__E03F[1].solver.IKPositionWeight = this.m__E01E);
					_E015(num);
					_E018(num2);
					_E014(num);
					if (this.m__E01E < 1f)
					{
						PlayerBones.Kinematics(this.m__E037[1], this.m__E01E);
					}
				}
				_prevHeight = Transform.position.y;
			}
			else
			{
				_E00F();
				MouseLook();
			}
		}
		if (num > EFTHardSettings.Instance.AnimatorCullDistance)
		{
			BodyAnimatorCommon.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			ArmsAnimatorCommon.cullingMode = ((!(_handsController is EmptyHandsController) && !(_handsController is KnifeController) && !(_handsController is UsableItemController)) ? AnimatorCullingMode.CullUpdateTransforms : AnimatorCullingMode.AlwaysAnimate);
		}
		else
		{
			BodyAnimatorCommon.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			ArmsAnimatorCommon.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		}
		if (_armsupdated || ArmsUpdateMode == EUpdateMode.Auto)
		{
			ProceduralWeaponAnimation.LateTransformations(Time.deltaTime);
			if (HandsController != null)
			{
				HandsController.ManualLateUpdate(Time.deltaTime);
			}
		}
	}

	protected virtual void LateUpdate()
	{
		MovementContext?.AnimatorStatesLateUpdate();
		DistanceDirty = true;
		OcclusionDirty = true;
		if (HealthController != null && HealthController.IsAlive)
		{
			Physical.LateUpdate();
			VisualPass();
			_armsupdated = false;
			_bodyupdated = false;
			if (_nFixedFrames > 0)
			{
				_nFixedFrames = 0;
				_fixedTime = 0f;
			}
			if (this.m__E040 != null)
			{
				if (Physics.Raycast(new Ray(_playerLookRaycastTransform.position + _playerLookRaycastTransform.forward / 2f, _playerLookRaycastTransform.forward), out var hitInfo, 1.5f, _E37B.HighPolyWithTerrainMask))
				{
					this.m__E040.transform.position = hitInfo.point;
					this.m__E040.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
					this.m__E042.SetAvailable(this.m__E043.Available);
					AllowToPlantBeacon = this.m__E043.Available;
					if (AllowToPlantBeacon)
					{
						BeaconPosition = this.m__E040.transform.position;
						BeaconRotation = this.m__E040.transform.rotation;
					}
				}
				else
				{
					this.m__E040.transform.position = _playerLookRaycastTransform.position + _playerLookRaycastTransform.forward;
					this.m__E040.transform.rotation = Quaternion.identity;
					this.m__E042.SetAvailable(isAvailable: false);
					AllowToPlantBeacon = false;
				}
			}
			ProceduralWeaponAnimation.StartFovCoroutine(this);
			PropUpdate();
		}
		ComplexLateUpdate(EUpdateQueue.Update, DeltaTime);
		if (POM != null && !IsAI)
		{
			POM.ExtrudeCamera();
		}
	}

	public void PropUpdate()
	{
		if (!this.m__E02A)
		{
			return;
		}
		if (this.m__E028 && this.m__E02A)
		{
			for (int i = 0; i < this.m__E048.Length; i++)
			{
				this.m__E049[i].SetPositionAndRotation(this.m__E048[i].position, this.m__E048[i].rotation);
			}
		}
		if (this.m__E01F < 1f)
		{
			Quaternion quaternion = Quaternion.Inverse(this.m__E037[0].rotation) * this.m__E048[0].rotation;
			Vector3 position = this.m__E037[0].InverseTransformPoint(this.m__E048[0].position);
			this.m__E049[0].position = PlayerBones.LeftPalm.TransformPoint(position);
			this.m__E049[0].rotation = PlayerBones.LeftPalm.rotation * quaternion;
		}
	}

	public void CalculateScaleValueByFov(float fov)
	{
		float t = Mathf.InverseLerp(50f, 75f, fov);
		this.m__E01A = Mathf.Lerp(1f, 0.65f, t);
	}

	public void RestoreRibcageScale()
	{
		RibcageScaleCurrentTarget = 1f;
	}

	public void SetCompensationScale(bool force = false)
	{
		RibcageScaleCurrentTarget = this.m__E01A;
		if (force)
		{
			RibcageScaleCurrent = RibcageScaleCurrentTarget;
			ProceduralWeaponAnimation.ResetFovAdjustments(this);
		}
		ProceduralWeaponAnimation.SetFovParams(this.m__E01A);
	}

	public void OnMakingShot([NotNull] IWeapon weapon, Vector3 force)
	{
		ExecuteSkill((Action)delegate
		{
			Skills.RecoilAction.Complete(weapon.RecoilBase);
		});
		IncreaseAwareness(15f);
		if (AIData != null)
		{
			FirearmController firearmController = HandsController as FirearmController;
			AISoundType soundType = AISoundType.gun;
			if (firearmController != null && firearmController.IsSilenced)
			{
				soundType = AISoundType.silencedGun;
			}
			AIData.TryPlayShootSound(GetPlayer, Position, soundType);
			if (AIData.IsAI)
			{
				AIData.BotOwner.ShootData.ShootDoneWeapon();
			}
		}
		if (PointOfView == EPointOfView.FirstPerson && IsYourPlayer && _E8AE.IsReflexAvailable() && _E8AE.IsAutomaticReflexAnalyzerSupported())
		{
			_E8A8.Instance.ReflexController.DoReflexTriggerFlash();
		}
		if (PointOfView == EPointOfView.FirstPerson)
		{
			return;
		}
		this.m__E01D = Time.time + this.m__E01C;
		_fbbik.solver.Quick = false;
		float num = Mathf.Sqrt(weapon.Weight) * weapon.RecoilForceBack / 600f;
		force *= num;
		foreach (HitReaction.HitPoint item in HitReaction.Recoil)
		{
			item.Hit(force, PlayerBones.WeaponRoot.position);
		}
	}

	public IEnumerator HitDelay(Action callback)
	{
		yield return new WaitForEndOfFrame();
		callback();
	}

	public virtual void ShotReactions(_EC23 shot, EBodyPart bodyPart)
	{
		Vector3 normalized = shot.Direction.normalized;
		if (PointOfView == EPointOfView.ThirdPerson)
		{
			this.m__E01D = Time.time + this.m__E01C;
			_fbbik.solver.Quick = false;
			HitReaction.Hit(shot.HitCollider, normalized, shot.HitPoint);
		}
		if (shot.Weapon is _EA60 obj)
		{
			KnifeComponent itemComponent = obj.GetItemComponent<KnifeComponent>();
			Vector3 normalized2 = (shot.Player.Transform.position - Transform.position).normalized;
			Vector3 lhs = Vector3.Cross(normalized2, Vector3.up);
			float y = normalized.y;
			float num = Vector3.Dot(lhs, normalized);
			float num2 = 1f - Mathf.Abs(Vector3.Dot(normalized2, normalized));
			num2 = ((bodyPart == EBodyPart.Head) ? num2 : Mathf.Sqrt(num2));
			Rotation += new Vector2(0f - num, 0f - y).normalized * itemComponent.Template.AppliedTrunkRotation.Random() * num2;
			ProceduralWeaponAnimation.ForceReact.AddForce(new Vector3(0f - y, num, 0f).normalized, num2, 1f, itemComponent.Template.AppliedHeadRotation.Random());
		}
		if (Singleton<Effects>.Instantiated)
		{
			this.m__E04A.Clear();
			_playerBody.GetBodyRenderersNonAlloc(this.m__E04A);
			Singleton<Effects>.Instance.EffectsCommutator.PlayerMeshesHit(this.m__E04A, shot.HitPoint, -shot.HitNormal);
		}
	}

	private void _E011(EPointOfView pointOfView)
	{
		MovementContext.OnStateChanged += _E012;
		this.m__E036 = new Quaternion[PlayerBones.FovSpecialTransforms.Length];
		this.m__E03F = PlayerBones.Ribcage.Original.GetComponentsInChildren<LimbIK>(includeInactive: true);
		this.m__E03F[0].enabled = false;
		this.m__E03F[1].enabled = false;
		this.m__E03E = PlayerBones.Ribcage.Original.GetComponentsInChildren<TwistRelax>();
		HandPosers = PlayerBones.Ribcage.Original.GetComponentsInChildren<HandPoser>();
		_E013();
		PointOfView = pointOfView;
		ProceduralWeaponAnimation.PointOfView = pointOfView;
		_fbbik.enabled = false;
		_fbbik.solver.Quick = true;
		Grounder.ik = _fbbik;
		Grounder.enabled = false;
		_E8A8.Instance.FoVUpdateAction += OnFovUpdatedEvent;
		OnFovUpdatedEvent((int)_E8A8.Instance.Fov);
		SubscribeVisualEvents();
	}

	private void _E012(EPlayerState previousState, EPlayerState nextState)
	{
		if (!this.m__E04D || HandsAnimator == null)
		{
			return;
		}
		if (PointOfView == EPointOfView.FirstPerson)
		{
			HandsAnimator.SetPlayerState(ObjectInHandsAnimator.PlayerState.None);
			return;
		}
		switch (nextState)
		{
		case EPlayerState.Jump:
			HandsAnimator.SetPlayerState(ObjectInHandsAnimator.PlayerState.Jump);
			break;
		case EPlayerState.ProneIdle:
		case EPlayerState.ProneMove:
		case EPlayerState.Transit2Prone:
			HandsAnimator.SetPlayerState(ObjectInHandsAnimator.PlayerState.Prone);
			break;
		case EPlayerState.Sprint:
			HandsAnimator.SetPlayerState(ObjectInHandsAnimator.PlayerState.Sprint);
			break;
		default:
			HandsAnimator.SetPlayerState(ObjectInHandsAnimator.PlayerState.Idle);
			break;
		}
	}

	protected virtual void OnFovUpdatedEvent(int fov)
	{
		if (HealthController.IsAlive)
		{
			CalculateScaleValueByFov(fov);
			SetCompensationScale(force: true);
		}
	}

	public virtual void OnHealthEffectVisualAdded(_E992 effect)
	{
		if (effect is _E9A0 && Singleton<Effects>.Instantiated)
		{
			Singleton<Effects>.Instance.EffectsCommutator.StartBleedingForPlayer(this);
		}
	}

	public virtual void OnHealthEffectVisualRemoved(_E992 effect)
	{
		if (effect is _E9A0 && Singleton<Effects>.Instantiated)
		{
			Singleton<Effects>.Instance.EffectsCommutator.StopBleedingForPlayer(this);
		}
	}

	protected virtual void OnPlayerVisualDied(EDamageType damageType)
	{
		if (Singleton<Effects>.Instantiated)
		{
			Singleton<Effects>.Instance.EffectsCommutator.StopBleedingForPlayer(this);
		}
	}

	public void SubscribeVisualEvents()
	{
		HealthController.EffectStartedEvent += OnHealthEffectVisualAdded;
		HealthController.EffectResidualEvent += OnHealthEffectVisualRemoved;
		HealthController.DiedEvent += OnPlayerVisualDied;
	}

	public void UnsubscribeVisualEvents()
	{
		HealthController.EffectStartedEvent -= OnHealthEffectVisualAdded;
		HealthController.EffectResidualEvent -= OnHealthEffectVisualRemoved;
		HealthController.DiedEvent -= OnPlayerVisualDied;
	}

	public virtual void MouseLook(bool forceApplyToOriginalRibcage = false)
	{
		if (!_E2B6.Config.UseSpiritPlayer || !Spirit.IsActive || forceApplyToOriginalRibcage)
		{
			MovementContext.RotationAction?.Invoke(this);
		}
	}

	private void _E013()
	{
		CameraContainer = Transform.Original.FindTransform(_ED3E._E000(161659)).gameObject;
		CameraPosition = Transform.Original.FindTransform(_ED3E._E000(161643));
		ProceduralWeaponAnimation = PlayerBones.Ribcage.Original.GetComponent<ProceduralWeaponAnimation>();
		ProceduralWeaponAnimation.CameraContainer = CameraContainer;
		ProceduralWeaponAnimation.Walk.Speed = MovementContext.CharacterMovementSpeed;
		ProceduralWeaponAnimation.Breath.Physical = Physical;
		ProceduralWeaponAnimation.OnPreCollision += IkStoreRaw;
		MovementContext.OnPoseChanged += delegate(int i)
		{
			ProceduralWeaponAnimation.Pose = i;
		};
		OnHealthEffectRemoved(null);
	}

	public void UpdateLauncherBones(bool value, WeaponPrefab weaponPrefab)
	{
		this.m__E03D = value;
		if (this.m__E03D)
		{
			Transform launcherRoot = weaponPrefab.transform;
			Transform propBone = PlayerBones.WeaponRoot.Original.GetComponentInChildren<AlternativePropBone>().transform;
			ProceduralWeaponAnimation.SetLauncherWeaponBone(weaponPrefab.transform, propBone);
			HandsController.HandsHierarchy.GatherUnderbarrelWeaponIK(launcherRoot, _elbowBends);
		}
		else
		{
			UpdateBonesOnWeaponChange(HandsController.HandsHierarchy);
		}
	}

	public void UpdateFirstPersonGrip(GripPose.EGripType type = GripPose.EGripType.Common, TransformLinks transforms = null)
	{
		HandPosers[0].GripWeight = 0f;
		if (transforms != null)
		{
			HandPosers[0].MapGrip(transforms.GetTransform(ECharacterWeaponBones.HumanLPalm));
			HandPosers[1].MapGrip(transforms.GetTransform(ECharacterWeaponBones.HumanRPalm));
			HandPosers[0].weight = (HandPosers[1].weight = 1f);
			ProceduralWeaponAnimation.HandsContainer.CameraAnimatedFP = transforms.GetTransform(ECharacterWeaponBones.Camera_animated);
		}
		else
		{
			HandPosers[0].weight = (HandPosers[1].weight = 0f);
		}
		GripPose[] source = (from x in PlayerBones.WeaponRoot.Original.GetComponentsInChildren<GripPose>()
			where x.GripType == type || x.GripType == GripPose.EGripType.UnderbarrelWeapon
			select x).ToArray();
		GripPose gripPose = (from x in source
			where x.Hand == GripPose.EHand.Left
			orderby x.GripType == GripPose.EGripType.UnderbarrelWeapon descending, HandPoser.NumParents(x.transform, PlayerBones.WeaponRoot.Original) descending
			select x).FirstOrDefault();
		GripPose gripPose2 = (from x in source
			where x.Hand == GripPose.EHand.Right
			orderby HandPoser.NumParents(x.transform, PlayerBones.WeaponRoot.Original) descending
			select x).FirstOrDefault();
		HandPosers[0].SetGrip(gripPose);
		HandPosers[1].SetGrip(gripPose2);
		HandPosers[1].IgnoreIndexFinger = gripPose2 != null;
		this.m__E039 = new Transform[2]
		{
			gripPose ? gripPose.transform : null,
			gripPose2 ? gripPose2.transform : null
		};
	}

	public void UpdateBonesOnWeaponChange(TransformLinks links)
	{
		PlayerBones.UpdateImportantBones(links);
		_E38B.SetLayersRecursively<MeshRenderer>(links.gameObject, LayerMask.NameToLayer(_ED3E._E000(60679)), new string[1] { _ED3E._E000(60752) });
		_elbowBends = new Transform[2];
		links.GatherIK(this.m__E037, this.m__E038, _elbowBends);
		this.m__E03C = links.GetTransformOutOfRangeSafe(ECharacterWeaponBones.prop);
		this.m__E028 = this.m__E03C != null;
		if (this.m__E028)
		{
			this.m__E048[0] = this.m__E03C;
			this.m__E048[1] = this.m__E03C.GetChild(0);
			this.m__E048[2] = this.m__E03C.GetChild(1);
		}
		this.m__E03A = ((links.Transforms.Length > 18) ? links.GetTransform(ECharacterWeaponBones.weapon_vest_IK_marker) : null);
		UpdateFirstPersonGrip(GripPose.EGripType.Common, links);
	}

	public void FBBIKUpdate(float distance)
	{
		if ((EnabledAnimators & EAnimatorMask.FBBIK) == 0)
		{
			return;
		}
		if (PointOfView == EPointOfView.ThirdPerson)
		{
			_fbbik.solver.iterations = (int)Mathf.Clamp(15f / distance, 0f, 2f);
			if (!_fbbik.solver.Quick && Time.time > this.m__E01D)
			{
				_fbbik.solver.Quick = true;
			}
		}
		_fbbik.solver.Update();
	}

	private void _E014(float d)
	{
		if (d > 300f)
		{
			return;
		}
		this.m__E03F[0].solver.Update();
		if (d > 70f)
		{
			return;
		}
		this.m__E03F[1].solver.Update();
		bool flag = d > 40f;
		if (!flag)
		{
			TwistRelax[] array = this.m__E03E;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Relax();
			}
		}
		HandPosers[0].ManualUpdate(flag);
		HandPosers[1].ManualUpdate(flag);
	}

	public Vector3 ProjectLocalPosition(Vector3 position)
	{
		return PlayerBones.WeaponRoot.TransformPoint(PlayerBones.Weapon_Root_Anim.InverseTransformPoint(position));
	}

	public void DropItemDead(Item item, GameObject prefab)
	{
		_ECC9.ReleaseBeginSample(_ED3E._E000(161639), _ED3E._E000(161683));
		int layer = LayerMask.NameToLayer(_ED3E._E000(55338));
		int num = LayerMask.NameToLayer(_ED3E._E000(60752));
		AssetPoolObject[] componentsInChildren = prefab.GetComponentsInChildren<AssetPoolObject>(includeInactive: true);
		Collider collider = null;
		AssetPoolObject[] array = componentsInChildren;
		foreach (AssetPoolObject assetPoolObject in array)
		{
			foreach (Collider collider2 in assetPoolObject.Colliders)
			{
				if (!collider2.isTrigger && collider2.gameObject.layer != num)
				{
					assetPoolObject.StoreCollider(collider2);
					collider2.enabled = true;
					collider2.gameObject.layer = layer;
					if (collider == null || collider.bounds.extents.sqrMagnitude < collider2.bounds.extents.sqrMagnitude)
					{
						collider = collider2;
					}
				}
			}
		}
		this.m__E058 = new _E007
		{
			Transform = prefab.transform
		};
		Rigidbody rigidbody = prefab.AddComponent<Rigidbody>();
		LootItem component = prefab.GetComponent<LootItem>();
		if (component != null)
		{
			component.SetItemAndRigidbody(item, rigidbody);
		}
		this.m__E058.Shift = ((rigidbody != null) ? rigidbody.centerOfMass : Vector3.zero);
		if ((bool)collider)
		{
			this.m__E058.Transportee = collider.gameObject.AddComponent<CommonTransportee>();
			this.m__E058.Transportee.ParentTransform = prefab.transform;
		}
		foreach (Transform item2 in prefab.transform)
		{
			item2.localPosition -= this.m__E058.Shift;
		}
		TransformLinks componentInChildren = prefab.GetComponentInChildren<TransformLinks>();
		bool flag = item is _EACD || item is _EADF || item.GetItemComponent<KnifeComponent>() != null;
		if (!flag)
		{
			HandPosers[1].Lerp2Target(EFTHardSettings.Instance.RIGHT_HAND_QTS, 5f);
		}
		RigidbodySpawner obj = (flag ? PlayerBones.Forearms[1].GetComponent<RigidbodySpawner>() : GetComponentInChildren<RigidbodySpawner>());
		Corpse.Ragdoll.AttachWeapon(rigidbody, base.gameObject, PlayerBones, componentInChildren, flag, Velocity);
		obj.RemoveEvent += RemoveAttachment;
	}

	public void RemoveAttachment(RigidbodySpawner spawner)
	{
		if (spawner != null)
		{
			spawner.RemoveEvent -= RemoveAttachment;
		}
		this.m__E058?.RemovePhysics();
	}

	public void ReleaseHand()
	{
		if (this.m__E058 != null)
		{
			this.m__E058.Destroy();
			this.m__E058 = null;
		}
		_E05A();
		HandPosers[1].Lerp2Target(EFTHardSettings.Instance.RIGHT_HAND_QTS, 5f);
		ProceduralWeaponAnimation.OnPreCollision -= IkStoreRaw;
	}

	public void SpawnInHands(Item item, string parentBone)
	{
		this.m__E04F = Singleton<_E760>.Instance.CreateItem(item, isAnimated: true);
		Transform transform = this.m__E04F.transform.FindTransform(_ED3E._E000(161664));
		Transform transform2 = this.m__E03F[0].solver.bone3.transform.FindTransform(parentBone);
		this.m__E04F.transform.SetParent(transform2, worldPositionStays: false);
		this.m__E04F.transform.localRotation = Quaternion.identity;
		this.m__E04F.transform.localPosition = Vector3.zero;
		this.m__E04F.SetActive(value: true);
		if (transform != null)
		{
			Quaternion quaternion = Quaternion.Inverse(transform.rotation) * transform2.rotation;
			this.m__E04F.transform.localRotation *= quaternion;
			Vector3 vector = transform2.position - transform.position;
			this.m__E04F.transform.position += vector;
		}
		else
		{
			UnityEngine.Debug.LogError(string.Concat(_ED3E._E000(161726), this.m__E04F, _ED3E._E000(161706), item.Id));
		}
		AudioClip itemClip = Singleton<GUISounds>.Instance.GetItemClip(item.ItemSound, EInventorySoundType.pickup);
		if (itemClip != null)
		{
			Singleton<BetterAudio>.Instance.PlayAtPoint(Transform.position, itemClip, 0f, BetterAudio.AudioSourceGroupType.Collisions, 30, 1f, (PointOfView != 0) ? EOcclusionTest.Fast : EOcclusionTest.None);
		}
	}

	public GameObject CreateBeacon(Item item, Vector3 position)
	{
		if (this.m__E040 == null)
		{
			GameObject original = Singleton<_E760>.Instance.CreateLootPrefab(item);
			this.m__E040 = UnityEngine.Object.Instantiate(original, position, Quaternion.identity);
			this.m__E040.gameObject.SetActive(value: true);
			this.m__E040.name = _ED3E._E000(161752);
			AssetPoolObject component = this.m__E040.GetComponent<AssetPoolObject>();
			foreach (Collider collider in component.Colliders)
			{
				collider.enabled = false;
			}
			BoxCollider boxCollider = this.m__E040.AddComponent<BoxCollider>();
			boxCollider.enabled = false;
			component.RegisteredComponentsToClean.Add(boxCollider);
			this.m__E042 = this.m__E040.AddComponent<PreviewMaterialSetter>();
			this.m__E042.SetAvailable(isAvailable: true);
			component.RegisteredComponentsToClean.Add(this.m__E042);
			this.m__E043 = this.m__E040.AddComponent<BeaconPlacer>();
		}
		return this.m__E040;
	}

	public void DestroyBeacon()
	{
		if (!(this.m__E040 == null))
		{
			UnityEngine.Object.Destroy(this.m__E040);
			this.m__E040 = null;
			this.m__E042 = null;
			this.m__E043 = null;
		}
	}

	public void ClearHands()
	{
		if (!(this.m__E04F == null))
		{
			AssetPoolObject.ReturnToPool(this.m__E04F);
			this.m__E04F = null;
		}
	}

	protected void RestoreIKPos()
	{
		if (!this.m__E025)
		{
			return;
		}
		this.m__E024 = HandsAnimator?.GetLayerWeight(HandsAnimator.LACTIONS_LAYER_INDEX) ?? 0f;
		this.m__E022 = ((this.m__E022 < this.m__E024) ? this.m__E024 : Mathf.SmoothDamp(this.m__E022, this.m__E024, ref this.m__E023, 0.2f));
		if (this.m__E022 > 0.005f)
		{
			if (this.m__E028 && this.m__E029)
			{
				this.m__E03C.position = this.m__E032;
				this.m__E03C.rotation = this.m__E034;
			}
			this.m__E037[0].position = Vector3.Lerp(this.m__E037[0].position, this.m__E02F, this.m__E022);
			this.m__E037[0].rotation = Quaternion.Slerp(this.m__E037[0].rotation, this.m__E033, this.m__E022);
			_elbowBends[0].position = this.m__E030;
		}
		this.m__E025 = false;
	}

	public void IkStoreRaw()
	{
		if (this.m__E028 && this.m__E029)
		{
			this.m__E032 = this.m__E03C.position;
			this.m__E034 = this.m__E03C.rotation;
		}
		if (!(this.m__E037[0] == null))
		{
			this.m__E02F = this.m__E037[0].position;
			this.m__E033 = this.m__E037[0].rotation;
			if (_elbowBends != null && _elbowBends.Length != 0)
			{
				this.m__E030 = _elbowBends[0].position;
				this.m__E031 = _elbowBends[1].position;
				this.m__E025 = true;
			}
		}
	}

	private void _E015(float distance2Camera)
	{
		for (int i = 0; i < 2; i++)
		{
			if (!(this.m__E037[i] == null) && !(Math.Abs(this.m__E03F[i].solver.IKPositionWeight) < float.Epsilon))
			{
				if (this.m__E039[i] != null && distance2Camera < 40f)
				{
					float value = Vector3.Distance(this.m__E037[i].position, this.m__E038[i].position);
					float num = Mathf.InverseLerp(0.1f, 0f, value);
					HandPosers[i].GripWeight = num;
					this.m__E02E = Vector3.Lerp(this.m__E037[i].position, this.m__E039[i].position, num);
					this.m__E035 = Quaternion.Lerp(this.m__E037[i].rotation, this.m__E039[i].rotation, num);
				}
				else
				{
					this.m__E02E = this.m__E037[i].position;
					this.m__E035 = this.m__E037[i].rotation;
				}
				if (LeftHandInteractionTarget != null && i == 0)
				{
					this.m__E02E = Vector3.Lerp(this.m__E02E, LeftHandInteractionTarget.transform.position, ThirdIkWeight.Value);
					this.m__E035 = Quaternion.Slerp(this.m__E035, LeftHandInteractionTarget.transform.rotation, ThirdIkWeight.Value);
				}
				this.m__E03F[i].solver.SetIKPosition(this.m__E02E);
				this.m__E03F[i].solver.SetIKRotation(this.m__E035);
			}
		}
	}

	protected RuntimeAnimatorController CreateAnimatorController()
	{
		return Singleton<_ED0A>.Instance.GetAsset<RuntimeAnimatorController>(_E5D2.PLAYER_DEFAULT_ANIMATOR_CONTROLLER);
	}

	private void _E016()
	{
		if (_animators[0].runtimeAnimatorController == null && !_E2B6.Config.UseBodyFastAnimator)
		{
			RuntimeAnimatorController runtimeAnimatorController = CreateAnimatorController();
			_animators[0].runtimeAnimatorController = runtimeAnimatorController;
		}
		if (_E2B6.Config.UseSpiritPlayer)
		{
			bool useFastAnimator = Profile.Info.Settings.Role.ShallUseFastAnimator() && _E2B6.Config.UseSpiritFastAnimator;
			Spirit.InitBodyAnimator(_animators[0].updateMode, useFastAnimator);
		}
	}

	protected virtual bool UpdateGrenadeAnimatorDuePoV()
	{
		return PointOfView == EPointOfView.ThirdPerson;
	}

	public virtual void SetEnvironment(EnvironmentType environmentType)
	{
		Environment = environmentType;
	}

	private void _E017(float distance)
	{
		if (!(distance > 70f))
		{
			if (this.m__E037[0] != null && this.m__E03A != null)
			{
				bool flag = ((_handsController is FirearmController firearmController && firearmController.IsInReloadOperation()) || _handsController.IsInventoryOpen()) && !IsSprintEnabled;
				this.m__E020 = (flag ? Mathf.InverseLerp(UtilityLayerRange.x, UtilityLayerRange.y, Vector3.Distance(this.m__E037[0].position, this.m__E03A.position)) : Mathf.Lerp(this.m__E020, 0f, Time.deltaTime * UtilityLayerLerpSpeed));
				BodyAnimatorCommon.SetLayerWeight(2, this.m__E020);
			}
			else
			{
				BodyAnimatorCommon.SetLayerWeight(2, 0f);
			}
		}
	}

	private void _E018(float curveWeight)
	{
		if (_elbowBends != null && !(_elbowBends[0] == null))
		{
			Quaternion quaternion = Quaternion.Euler(0.65f * MovementContext.Pitch, MovementContext.Yaw, 0f);
			Vector3 vector = quaternion * ProceduralWeaponAnimation.TurnAway.RElbowShift;
			Vector3 vector2 = quaternion * ProceduralWeaponAnimation.TurnAway.LElbowShift;
			float num = curveWeight * _E019(_E712.ELBOW_LEFT_WEIGHT);
			if (num < 1f)
			{
				PlayerBones.BendGoals[0].position = Vector3.Lerp(_elbowBends[0].position + vector2, PlayerBones.BendGoals[0].position, num);
			}
			float num2 = curveWeight * _E019(_E712.ELBOW_RIGHT_WEIGHT);
			if (num2 < 1f)
			{
				PlayerBones.BendGoals[1].position = Vector3.Lerp(_elbowBends[1].position + vector, PlayerBones.BendGoals[1].position, num2);
			}
		}
	}

	private float _E019(int hash)
	{
		return Mathf.Min(_animators[0].GetFloat(hash), 1f);
	}

	private bool _E01A()
	{
		foreach (KeyValuePair<EBoundItem, Item> boundItem in Inventory.FastAccess.BoundItems)
		{
			if (!(boundItem.Value is _EA3E))
			{
				continue;
			}
			return true;
		}
		return false;
	}

	public void CreateCompass()
	{
		if (!this.m__E02B && _E01A())
		{
			Transform transform = Singleton<_E760>.Instance.CreateFromPool<Transform>(new ResourceKey
			{
				path = _ED3E._E000(161740)
			});
			transform.SetParent(PlayerBones.Ribcage.Original, worldPositionStays: false);
			transform.localRotation = Quaternion.identity;
			transform.localPosition = Vector3.zero;
			_E01B(transform.gameObject);
			this.m__E02B = true;
		}
	}

	private void _E01B(GameObject obj)
	{
		this.m__E029 = obj != null;
		if (this.m__E029)
		{
			this.m__E046 = obj.GetComponentInChildren<CompassArrow>();
			this.m__E046.NorthDirection = Singleton<LevelSettings>.Instance.NorthVector;
			this.m__E046.enabled = true;
			this.m__E049[0] = obj.transform;
			this.m__E049[1] = obj.transform.FindTransform(_ED3E._E000(159760));
			this.m__E049[2] = obj.transform.FindTransform(_ED3E._E000(159750));
			obj.SetActive(value: false);
		}
		else
		{
			this.m__E049 = new Transform[3];
			this.m__E02A = false;
			if ((bool)this.m__E046)
			{
				this.m__E046.enabled = false;
				this.m__E046 = null;
			}
		}
	}

	public void SetPropVisibility(bool isVisible)
	{
		if (_playerBody != null && PointOfView == EPointOfView.FirstPerson)
		{
			if (isVisible)
			{
				if (Singleton<_E5CB>.Instance.AzimuthPanelShowsPlayerOrientation)
				{
					Singleton<GameUI>.Instance.BattleUiScreen.ShowAzimuth(() => (int)Vector3.SignedAngle(Singleton<LevelSettings>.Instance.NorthVector, Quaternion.Euler(0f, MovementContext.Yaw, 0f) * Vector3.forward, Vector3.up));
				}
				else if (this.m__E046 != null)
				{
					Singleton<GameUI>.Instance.BattleUiScreen.ShowAzimuth(this.m__E046.PanelValue);
				}
			}
			else if (Singleton<GameUI>.Instantiated)
			{
				Singleton<GameUI>.Instance.BattleUiScreen.HideAzimuth();
			}
		}
		if (this.m__E029 && this.m__E028)
		{
			this.m__E049[0].gameObject.SetActive(isVisible);
			this.m__E049[0].transform.SetPositionAndRotation(this.m__E048[0].position, this.m__E048[0].rotation);
			this.m__E046.enabled = isVisible;
			this.m__E02A = isVisible;
		}
	}

	public void OnRadiolocationZoneEnter()
	{
		UnityEngine.Debug.Log(Profile.Nickname + _ED3E._E000(159796));
	}

	public void OnRadiolocationZoneExit()
	{
		UnityEngine.Debug.Log(Profile.Nickname + _ED3E._E000(159826));
	}

	protected virtual bool IsVisibleByCullingObject(float cullingDistance)
	{
		return true;
	}

	protected virtual void InitAudioController()
	{
		SetAudioProtagonist();
		CompositeDisposable.AddDisposable(_E0DE.SearchOperations.CountChanged.Bind(_E022));
		_stepLayerMask = _E37B.AudioControllerStepLayerMask;
		this.m__E063 = new Dictionary<BaseBallistic.ESurfaceSound, SurfaceSet>();
		Sounds asset = Singleton<_ED0A>.Instance.GetAsset<Sounds>(_ED3E._E000(109909));
		this.m__E065 = asset.Gear;
		this.m__E066 = asset.GearFast;
		this.m__E062 = asset.TinnitusSound;
		FaceshieldOn = asset.FaceShieldOn;
		FaceshieldOff = asset.FaceShieldOff;
		NightVisionOn = asset.NightVisionOn;
		NightVisionOff = asset.NightVisionOff;
		ThermalVisionOn = asset.ThermalVisionOn;
		ThermalVisionOff = asset.ThermalVisionOff;
		FractureSound = asset.FractureSound;
		PropIn = (asset.PropIn ? new BaseSoundPlayer.SoundElement
		{
			SoundClips = new AudioClip[1] { asset.PropIn },
			RollOff = 10
		} : null);
		PropOut = (asset.PropOut ? new BaseSoundPlayer.SoundElement
		{
			SoundClips = new AudioClip[1] { asset.PropOut },
			RollOff = 10
		} : null);
		SurfaceSet[] sets = asset.Sets;
		foreach (SurfaceSet surfaceSet in sets)
		{
			if (!this.m__E063.ContainsKey(surfaceSet.Surface))
			{
				this.m__E063.Add(surfaceSet.Surface, surfaceSet);
			}
			else
			{
				UnityEngine.Debug.LogError(string.Concat(surfaceSet.Surface, _ED3E._E000(159849)));
			}
		}
		this.m__E064 = this.m__E063[BaseBallistic.ESurfaceSound.Concrete];
		MovementContext.OnStateChanged += _E026;
		_E70F movementContext = MovementContext;
		movementContext.OnGrounded = (Action<float, float>)Delegate.Combine(movementContext.OnGrounded, new Action<float, float>(_E009));
		_healthController.ApplyDamageEvent += _E02E;
		_healthController.DiedEvent += _E025;
		InitAudioSources();
		this.m__E067 = StartCoroutine(_E02C());
		_E077 = Physical.SubscribeToAudibleEffects(_E01D);
		GenericEventTranslator eventTranslator = EventTranslator;
		eventTranslator.OnSoundBankPlay = (Action<string>)Delegate.Combine(eventTranslator.OnSoundBankPlay, new Action<string>(PlaySoundBank));
		foreach (KeyValuePair<BaseBallistic.ESurfaceSound, SurfaceSet> item in this.m__E063)
		{
			_E079 = Math.Max(_E079, item.Value.RunSoundBank.Rolloff);
			_E078 = Math.Max(_E078, item.Value.SprintSoundBank.Rolloff);
			_E07A = Mathf.Max(_E07A, item.Value.LandingSoundBank.Rolloff);
			_E07B = Mathf.Max(_E07B, item.Value.ProneSoundBank.Rolloff);
		}
		Speaker.OnRelease += OnSpeakerRelease;
		FaceShieldObserver.Changed.Bind(PlayFaceShieldSound);
		NightVisionObserver.Changed.Bind(PlayNightVisionSound);
		ThermalVisionObserver.Changed.Bind(PlayThermalVisionSound);
		_E01F();
		_E020();
	}

	protected virtual void PlayToggleSound(ref bool previousState, bool isOn, AudioClip toggleOn, AudioClip toggleOff)
	{
		if (previousState != isOn)
		{
			Singleton<BetterAudio>.Instance.PlayAtPoint(Transform.Original.position + SpeechLocalPosition, isOn ? toggleOn : toggleOff, Distance, BetterAudio.AudioSourceGroupType.Character, 5);
		}
		previousState = isOn;
	}

	public void PlayFaceShieldSound()
	{
		FaceShieldComponent component = FaceShieldObserver.Component;
		bool isOn = component != null && (component.Togglable == null || component.Togglable.On);
		PlayToggleSound(ref PreviousFaceShield, isOn, FaceshieldOn, FaceshieldOff);
	}

	public void PlayNightVisionSound()
	{
		NightVisionComponent component = NightVisionObserver.Component;
		bool isOn = component != null && (component.Togglable == null || component.Togglable.On);
		PlayToggleSound(ref PreviousNightVision, isOn, NightVisionOn, NightVisionOff);
	}

	public void PlayThermalVisionSound()
	{
		ThermalVisionComponent component = ThermalVisionObserver.Component;
		bool isOn = component != null && (component.Togglable == null || component.Togglable.On);
		PlayToggleSound(ref PreviousThermalVision, isOn, ThermalVisionOn, ThermalVisionOff);
	}

	public virtual void SetAudioProtagonist()
	{
	}

	public void AddVolume(BetterPropagationVolume volume)
	{
		this.m__E073.Add(volume);
		if (volume.MutuallyExclusive)
		{
			_E075 = volume;
		}
	}

	public void RemoveVolume(BetterPropagationVolume volume)
	{
		int num = this.m__E073.IndexOf(volume);
		if (num >= 0)
		{
			this.m__E073.RemoveAt(num);
		}
		_E075 = this.m__E073.FirstOrDefault((BetterPropagationVolume x) => x.MutuallyExclusive);
	}

	public List<BetterPropagationVolume> GetPropagationVolume()
	{
		_E074.Clear();
		if (_E075 != null)
		{
			_E074.Add(_E075);
		}
		else
		{
			_E074.AddRange(this.m__E073);
		}
		return _E074;
	}

	public IEnumerator OutOfRangeSpeakingCoroutine(float distance)
	{
		yield return new WaitForSeconds(Mathf.Max(1f, distance / 5f));
		while (Speaker.Speaking)
		{
			distance = DistanceFromProtagonistHearingEdge(Speaker.Clip.Falloff, buffer: false);
			if (distance < 0f)
			{
				StopOutOfRangeSpeakingCoroutine();
				PlaySpeechFromTime(Speaker.Clip, Speaker.Clip.Length - Speaker.TimeLeft);
			}
			yield return new WaitForSeconds(Mathf.Max(1f, distance / 5f));
		}
		this.m__E06B = null;
	}

	public void StopOutOfRangeSpeakingCoroutine()
	{
		if (this.m__E06B != null)
		{
			StopCoroutine(this.m__E06B);
			this.m__E06B = null;
		}
	}

	public void ToggleMuteSpeechSource(bool muteSpeech)
	{
		SpeechSource.source1.mute = muteSpeech;
	}

	public void PlaySpeechFromTime(TaggedClip clip, float time)
	{
		UpdateOcclusion();
		SpeechSource.gameObject.SetActive(value: true);
		SpeechSource.source1.spatialBlend = ((PointOfView == EPointOfView.ThirdPerson) ? 1 : 0);
		SpeechSource.SetRolloff((float)clip.Falloff * ProtagonistHearing);
		SpeechSource.source1.time = time;
		float volume = clip.Volume;
		SpeechSource.Play(clip.Clip, null, 1f, volume, PointOfView == EPointOfView.FirstPerson, oneShot: false);
	}

	private void _E01C(TaggedClip clip)
	{
		StopOutOfRangeSpeakingCoroutine();
		float num = DistanceFromProtagonistHearingEdge(clip.Falloff);
		if (num <= 0f)
		{
			PlaySpeechFromTime(clip, 0f);
			return;
		}
		if (num > 5f)
		{
			_E01E();
		}
		this.m__E06B = StartCoroutine(OutOfRangeSpeakingCoroutine(num));
	}

	public virtual bool OutOfProtagonistsRange(float spreadRange, bool buffer = true)
	{
		return DistanceFromProtagonistHearingEdge(spreadRange, buffer) > 0f;
	}

	public virtual float DistanceFromProtagonistHearingEdge(float spreadRange, bool buffer = true)
	{
		spreadRange *= ProtagonistHearing;
		float num = (buffer ? (spreadRange + 2f) : spreadRange);
		return Distance - num;
	}

	public virtual void UpdateOcclusion()
	{
		if (!OcclusionDirty || !MonoBehaviourSingleton<BetterAudio>.Instantiated)
		{
			return;
		}
		OcclusionDirty = false;
		AudioMixerGroup mixerGroup = MonoBehaviourSingleton<BetterAudio>.Instance.VeryStandartMixerGroup;
		if (PointOfView == EPointOfView.ThirdPerson)
		{
			if (Muffled)
			{
				mixerGroup = Singleton<BetterAudio>.Instance.SimpleOccludedMixerGroup;
			}
		}
		else
		{
			mixerGroup = (Muffled ? Singleton<BetterAudio>.Instance.SelfSpeechReverb : Singleton<BetterAudio>.Instance.VeryStandartMixerGroup);
		}
		if (SpeechSource != null)
		{
			SpeechSource.SetMixerGroup(mixerGroup);
		}
		UpdateVoipOcclusion(MonoBehaviourSingleton<BetterAudio>.Instance.VoipMixer);
	}

	protected virtual void UpdateVoipOcclusion(AudioMixerGroup group)
	{
		if (!(VoipAudioSource == null))
		{
			if ((object)group != VoipAudioSource.Source.outputAudioMixerGroup)
			{
				VoipAudioSource.Source.outputAudioMixerGroup = group;
			}
			VoipAudioSource.Source.maxDistance = 60f * ProtagonistHearing;
			VoipAudioSource.Source.spatialBlend = ((PointOfView == EPointOfView.ThirdPerson) ? 1 : 0);
		}
	}

	protected virtual bool CheckSurface(float range, float delayToNextCheck = 1f)
	{
		if (OutOfProtagonistsRange(range))
		{
			return false;
		}
		if (PointOfView == EPointOfView.ThirdPerson && _E080 > Time.time)
		{
			return true;
		}
		_E080 = Time.time + delayToNextCheck;
		(bool hit, bool hitOnTerrain, BaseBallistic.ESurfaceSound? surfaceSound) tuple = CalculateMovementSurface();
		var (hit, hitOnTerrain, _) = tuple;
		UpdateSurfaceData(hit, hitOnTerrain, tuple.surfaceSound ?? BaseBallistic.ESurfaceSound.Concrete);
		return true;
	}

	private void _E01D()
	{
		bool breathIsAudible = Physical.BreathIsAudible;
		if (breathIsAudible != _E076)
		{
			_E076 = breathIsAudible;
			HeavyBreath = false;
			UpdateBreathStatus();
		}
	}

	protected virtual void UpdateBreathStatus()
	{
		ETagStatus healthStatus = HealthStatus;
		bool flag = (healthStatus == ETagStatus.BadlyInjured || healthStatus == ETagStatus.Dying) && HealthController.FindActiveEffect<_E9B1>() == null;
		bool flag2 = flag || _E076 || Muffled;
		if (!HeavyBreath && flag2)
		{
			ETagStatus eTagStatus = (flag ? healthStatus : ETagStatus.Healthy);
			ETagStatus eTagStatus2 = ((!_E076) ? ETagStatus.Unaware : ETagStatus.Aware);
			if (eTagStatus == ETagStatus.Healthy && eTagStatus2 == ETagStatus.Unaware)
			{
				Speaker.Play(EPhraseTrigger.OnBreath, eTagStatus | eTagStatus2, demand: true, -1);
			}
			else
			{
				Speaker.Play(EPhraseTrigger.OnBreath, eTagStatus | eTagStatus2, demand: true);
			}
		}
		HeavyBreath = flag2;
	}

	protected void OnSpeakerRelease(bool force)
	{
		StopOutOfRangeSpeakingCoroutine();
		HeavyBreath = false;
		if (_healthController.IsAlive)
		{
			UpdateBreathStatus();
		}
		if (!HeavyBreath || force)
		{
			_E01E();
		}
	}

	private void _E01E()
	{
		if (!(_speechSource == null))
		{
			_speechSource.transform.parent = null;
			_speechSource.Release();
			_speechSource = null;
		}
	}

	private void _E01F()
	{
		FaceShieldComponent component = FaceShieldObserver.Component;
		_E021(component, EquipmentSlot.Headwear);
	}

	private void _E020()
	{
		FaceShieldComponent component = FaceCoverObserver.Component;
		_E021(component, EquipmentSlot.FaceCover);
	}

	private void _E021(FaceShieldComponent fs, EquipmentSlot quipmentSlot)
	{
		Muffled = false;
		bool flag = false;
		bool flag2 = fs != null && (fs.Togglable == null || fs.Togglable.On);
		if (flag2)
		{
			flag = Equipment.GetSlot(quipmentSlot).ContainedItem is _EA40 thisItem && thisItem.GetItemComponentsInChildren<CompositeArmorComponent>().SelectMany((CompositeArmorComponent x) => x.HeadSegments).Contains(EHeadSegment.Jaws);
		}
		Muffled = flag2 && flag;
		if (PointOfView == EPointOfView.FirstPerson)
		{
			UpdateBreathStatus();
			if (!flag2 && Speaker != null && !HeavyBreath && Speaker.Importance == 0)
			{
				Speaker.Shut();
			}
		}
		UpdateOcclusion();
	}

	protected virtual void InitAudioSources()
	{
		CreateNestedSource();
	}

	protected virtual void CreateNestedSource()
	{
		NestedStepSoundSource = Singleton<BetterAudio>.Instance.GetSource(BetterAudio.AudioSourceGroupType.Character);
		if ((object)NestedStepSoundSource != null)
		{
			if (PointOfView == EPointOfView.ThirdPerson && MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
			{
				MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ProcessSourceOcclusion(this, NestedStepSoundSource);
			}
			NestedStepSoundSource.transform.parent = Transform.Original;
			NestedStepSoundSource.transform.localPosition = new Vector3(0f, 0.1f, 0f);
			NestedStepSoundSource.SetMixerGroup(Singleton<BetterAudio>.Instance.VeryStandartMixerGroup);
			UpdateStepSoundRolloff();
		}
	}

	protected virtual void CreateSpeechSource()
	{
		_speechSource = Singleton<BetterAudio>.Instance.GetSource(BetterAudio.AudioSourceGroupType.Speech, activateSource: false);
		if ((object)_speechSource != null)
		{
			if (PointOfView == EPointOfView.ThirdPerson && MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
			{
				MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ProcessSourceOcclusion(this, _speechSource);
			}
			_speechSource.transform.position = Position;
			_speechSource.transform.parent = Transform.Original;
			_speechSource.transform.localPosition = SpeechLocalPosition;
			_speechSource.SetMixerGroup(Singleton<BetterAudio>.Instance.VeryStandartMixerGroup);
		}
	}

	public void UpdateStepSoundRolloff()
	{
		if (NestedStepSoundSource != null)
		{
			float rolloff = 60f * ProtagonistHearing * Physical.SoundRadius;
			NestedStepSoundSource.SetRolloff(rolloff);
		}
	}

	public IEnumerator SupportAudioSourceCoroutine()
	{
		while (this.m__E070 > 0f && _healthController.IsAlive)
		{
			float num = _E8A8.Instance.Distance(Transform.position);
			if (num <= 14f)
			{
				float volume = 0.35f;
				BetterSource betterSource = _E023(this.m__E071);
				AudioMixerGroup mixerGroup = _E487.VolumeDependentOcclusion(Transform.position, ref volume, num, this, Environment, EFTHardSettings.Instance.GunshotMask);
				betterSource.SetMixerGroup(mixerGroup);
				betterSource.SetBaseVolume(volume);
				if (!betterSource.source1.isPlaying)
				{
					betterSource.gameObject.SetActive(value: true);
					betterSource.source1.Play();
				}
			}
			else
			{
				ReleaseSearchSource();
			}
			yield return new WaitForSeconds(0.5f);
		}
		ReleaseSearchSource();
	}

	private void _E022(int count)
	{
		this.m__E070 = count;
		if (this.m__E070 > 0f)
		{
			if (this.m__E06C != null)
			{
				StopCoroutine(this.m__E06C);
			}
			AudioClip audioClip = null;
			try
			{
				audioClip = Singleton<GUISounds>.Instance.GetLootingClip(_E0DE.SearchOperations.Last()._E016.SearchSound);
			}
			catch (Exception)
			{
				_ECEF<_EB94> searchOperations = _E0DE.SearchOperations;
				_EA91 obj = ((searchOperations != null && searchOperations.Count > 0) ? searchOperations.Last()._E016 : null);
				string text = obj?.SearchSound;
				string text2 = obj?.ShortName;
				UnityEngine.Debug.LogError(string.Format(_ED3E._E000(159880), searchOperations.Count, obj == null, text, text2));
				return;
			}
			if (!(audioClip == null))
			{
				BetterSource betterSource = _E023(audioClip);
				try
				{
					betterSource.Loop = true;
					betterSource.transform.position = MovementContext.PlayerColliderCenter + MovementContext.TransformForwardVector / 4f;
					betterSource.SetRolloff(14f);
				}
				catch (Exception)
				{
					UnityEngine.Debug.LogError(string.Format(_ED3E._E000(159938), betterSource == null, MovementContext == null));
					return;
				}
				this.m__E06C = StartCoroutine(SupportAudioSourceCoroutine());
				this.m__E071 = audioClip;
			}
		}
		else
		{
			ReleaseSearchSource();
			if (this.m__E06C != null)
			{
				StopCoroutine(this.m__E06C);
				this.m__E06C = null;
			}
		}
	}

	private BetterSource _E023(AudioClip clip)
	{
		if (this.m__E06F == null)
		{
			this.m__E06F = MonoBehaviourSingleton<BetterAudio>.Instance.GetSource(BetterAudio.AudioSourceGroupType.Environment, activateSource: false);
			if (this.m__E06F != null)
			{
				if (PointOfView == EPointOfView.ThirdPerson && MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
				{
					MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ProcessSourceOcclusion(this, this.m__E06F);
				}
				this.m__E06F.source1.clip = clip;
			}
		}
		return this.m__E06F;
	}

	public void ReleaseSearchSource()
	{
		if (this.m__E06F != null)
		{
			this.m__E06F.source1.Stop();
			this.m__E06F.Release();
			this.m__E06F = null;
		}
	}

	protected virtual void PlayGroundedSound(float fallHeight, float jumpHeight)
	{
		if (!(Time.realtimeSinceStartup < this.m__E06E))
		{
			float num = Mathf.Max(fallHeight, jumpHeight);
			if (num > LandingThreshold && CheckSurface(_E07A * Physical.SoundRadius))
			{
				DefaultPlay(this.m__E064.LandingSoundBank, Mathf.InverseLerp(0.1f, LandingThreshold * 2.5f, num));
				this.m__E06E = Time.realtimeSinceStartup + 0.5f;
			}
		}
	}

	private void _E024()
	{
		if (NestedStepSoundSource != null && !_E082)
		{
			NestedStepSoundSource.transform.parent = null;
			_E082 = true;
			NestedStepSoundSource.WaitSeconds(1f, delegate
			{
				NestedStepSoundSource.Release();
				NestedStepSoundSource = null;
			});
		}
		_E077?.Invoke();
		_E077 = null;
	}

	private void _E025(EDamageType damageType)
	{
		Coroutine[] array = new Coroutine[3] { this.m__E069, this.m__E068, this.m__E067 };
		foreach (Coroutine coroutine in array)
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
			}
		}
		_E024();
	}

	private void _E026(EPlayerState previousState, EPlayerState nextstate)
	{
		switch (previousState)
		{
		case EPlayerState.Idle:
			if (this.m__E067 != null)
			{
				StopCoroutine(this.m__E067);
			}
			break;
		case EPlayerState.Run:
			if (this.m__E068 == null)
			{
				break;
			}
			StopCoroutine(this.m__E068);
			if (!this.m__E06D && SinceLastStep > 0.66f)
			{
				if (CheckSurface(_E079 * Physical.SoundRadius * MovementContext.CovertMovementVolumeBySpeed))
				{
					PlayStepSound();
				}
				_E07D = Time.time;
			}
			break;
		case EPlayerState.Sprint:
			if (this.m__E069 != null)
			{
				StopCoroutine(this.m__E069);
				if (!this.m__E06D && CheckSurface(_E078))
				{
					DefaultPlay(this.m__E064.SprintSoundBank);
				}
			}
			if (nextstate == EPlayerState.Transition || nextstate == EPlayerState.Idle)
			{
				float num = ((PointOfView == EPointOfView.FirstPerson) ? this.m__E064.StopSoundBank.BaseVolume : 1f);
				DefaultPlay(this.m__E064.StopSoundBank, num * MovementContext.CovertMovementVolume);
			}
			break;
		}
		switch (nextstate)
		{
		case EPlayerState.Prone2Stand:
			DefaultPlay(this.m__E066, 0.7f * MovementContext.CovertEquipmentNoise);
			break;
		case EPlayerState.Transit2Prone:
			DefaultPlay((previousState == EPlayerState.Sprint) ? this.m__E064.ProneDropSoundBank : this.m__E066, 0.7f * MovementContext.CovertMovementVolume);
			break;
		case EPlayerState.Idle:
			this.m__E067 = StartCoroutine(_E02C());
			break;
		case EPlayerState.Run:
			this.m__E068 = StartCoroutine(_E02D());
			break;
		case EPlayerState.Sprint:
			this.m__E069 = StartCoroutine(_E02B());
			break;
		case EPlayerState.Jump:
			DefaultPlay(this.m__E064.JumpSoundBank);
			DefaultPlay(this.m__E066, MovementContext.CovertEquipmentNoise * MovementContext.CovertEquipmentNoise);
			break;
		}
	}

	public void DefaultPlay(SoundBank bank, float volume = 1f, string signature = "")
	{
		if (bank == null)
		{
			UnityEngine.Debug.LogError(string.Format(_ED3E._E000(160003), signature, (this.m__E064 != null) ? this.m__E064.ToString() : _ED3E._E000(160088)));
			return;
		}
		if (OutOfProtagonistsRange(bank.Rolloff * Physical.SoundRadius))
		{
			NestedStepSoundSource.SetPriority(200);
			return;
		}
		NestedStepSoundSource.SetPriority((Distance < 30f) ? 64 : 128);
		UpdateOcclusion();
		float num = 1f - (float)Skills.BotSoundCoef;
		volume *= num;
		bank.PlayWithConstantRolloffDistance(NestedStepSoundSource, EnvironmentType.Outdoor, Distance, volume, Distance);
	}

	public void PlayStepSound()
	{
		SoundBank soundBank = ((Pose == EPlayerPose.Duck) ? this.m__E064.DuckSoundBank : this.m__E064.RunSoundBank);
		float clampedSpeed = MovementContext.ClampedSpeed;
		float covertMovementVolumeBySpeed = MovementContext.CovertMovementVolumeBySpeed;
		clampedSpeed = Mathf.Max(Physical.MinStepSound, clampedSpeed);
		clampedSpeed *= covertMovementVolumeBySpeed;
		UpdateOcclusion();
		float num = ((PointOfView == EPointOfView.FirstPerson) ? (covertMovementVolumeBySpeed * soundBank.BaseVolume) : covertMovementVolumeBySpeed);
		float num2 = _E027();
		float num3 = 1f - (float)Skills.BotSoundCoef;
		num = num * num3 * num2;
		soundBank.PlayWithConstantRolloffDistance(NestedStepSoundSource, EnvironmentType.Outdoor, Distance, num, clampedSpeed, PointOfView == EPointOfView.FirstPerson);
		if (PointOfView == EPointOfView.FirstPerson)
		{
			float num4 = ((Pose == EPlayerPose.Duck) ? Mathf.Clamp(clampedSpeed, 0f, 0.3f) : Mathf.Clamp(clampedSpeed * 0.75f * Mathf.Sqrt(MovementContext.PoseLevel), 0.1f, 0.5f));
			if (num4 > 0f)
			{
				StartCoroutine(_E02A(num4));
			}
		}
	}

	private float _E027()
	{
		float actualLinearSpeed = MovementContext.ActualLinearSpeed;
		return Mathf.Clamp(Mathf.InverseLerp(0.5f, 3.5f, actualLinearSpeed), MINStepSoundSpeedFactor, 1f);
	}

	private void _E028(float volume = 1f)
	{
		if (!OutOfProtagonistsRange(this.m__E065.Rolloff * MovementContext.CovertEquipmentNoise, buffer: false))
		{
			UpdateOcclusion();
			float num = 1f - (float)Skills.BotSoundCoef;
			volume *= num;
			if (NestedStepSoundSource != null)
			{
				NestedStepSoundSource.gameObject.SetActive(value: true);
				this.m__E065.PlayWithConstantRolloffDistance(NestedStepSoundSource, EnvironmentType.Outdoor, Distance, volume * MovementContext.CovertEquipmentNoise, Distance, PointOfView == EPointOfView.FirstPerson);
			}
		}
	}

	private void _E029(float speed = 55f)
	{
		if (Time.time - _E07E >= _E07F)
		{
			float num = Mathf.InverseLerp(1f, 360f + (1f - MovementContext.PoseLevel) * 360f, Mathf.Abs(MovementContext.AverageRotationSpeed.Avarage));
			SoundBank turnSoundBank = this.m__E064.TurnSoundBank;
			float volume = num * MovementContext.CovertMovementVolume * ((PointOfView == EPointOfView.FirstPerson) ? turnSoundBank.BaseVolume : 1f);
			DefaultPlay(turnSoundBank, volume, _ED3E._E000(160085));
			_E07E = Time.time;
			if (num > 0.4f)
			{
				_E028(num);
			}
		}
	}

	public void PlaySoundBank(string soundBank)
	{
		if (soundBank == _ED3E._E000(127273) && !(SinceLastStep < 0.5f) && CheckSurface(_E07B * Physical.SoundRadius, 2.5f))
		{
			UpdateOcclusion();
			float num = Mathf.Max(0.4f, MovementContext.CovertMovementVolume * MovementContext.ClampedSpeed * this.m__E064.ProneSoundBank.BaseVolume);
			NestedStepSoundSource.SetPriority((Distance < 30f) ? 64 : 128);
			float num2 = 1f - (float)Skills.BotSoundCoef;
			num *= num2;
			this.m__E064.ProneSoundBank.PlayWithConstantRolloffDistance(NestedStepSoundSource, EnvironmentType.Outdoor, Distance, num, Distance);
			_E07D = Time.time;
		}
	}

	private IEnumerator _E02A(float v = 1f)
	{
		yield return new WaitForSeconds(EFTHardSettings.Instance.GEAR_SOUND_DELAY);
		if (NestedStepSoundSource != null)
		{
			_E028(v);
		}
	}

	private IEnumerator _E02B()
	{
		this.m__E06D = false;
		while (CurrentState.Name == EPlayerState.Sprint)
		{
			float num = Mathf.Sign(BodyAnimatorCommon.GetFloat(_ED3E._E000(130814)));
			if (Math.Abs(_E07C - num) >= float.Epsilon)
			{
				_E07C = num;
				float num2 = Time.time - _E07D;
				if (num2 > 0.2f && MovementContext.FreefallTime < 0.6f)
				{
					this.m__E06D = true;
					_E07D = Time.time;
					if (CheckSurface(_E078))
					{
						NestedStepSoundSource.SetPriority((Distance < 30f) ? 64 : 128);
						UpdateOcclusion();
						float num3 = ((PointOfView == EPointOfView.FirstPerson) ? this.m__E064.SprintSoundBank.BaseVolume : 1f);
						float num4 = 1f - (float)Skills.BotSoundCoef;
						num3 *= num4;
						this.m__E064.SprintSoundBank.PlayWithCustomRolloffDistance(NestedStepSoundSource, EnvironmentType.Outdoor, Distance, num3, Distance, PointOfView == EPointOfView.FirstPerson, 60f * (0.5f + 3f * MovementContext.Overweight));
						_E028();
						if (num2 < 1.2f && PointOfView == EPointOfView.FirstPerson)
						{
							ProceduralWeaponAnimation.Walk.StepFrequency = 0.5f / Mathf.Clamp(num2, 0.3f, 0.8f);
						}
					}
					else
					{
						NestedStepSoundSource.SetPriority(200);
					}
				}
			}
			yield return null;
		}
	}

	private IEnumerator _E02C()
	{
		while (CurrentState.Name == EPlayerState.Idle && HealthController.IsAlive)
		{
			float num = Math.Abs(HandsToBodyAngle);
			if (num > EFTHardSettings.Instance.TURN_ANGLE)
			{
				_E029(num);
				yield return new WaitForSeconds(EFTHardSettings.Instance.TURN_SOUND_DELAY);
			}
			yield return null;
		}
	}

	private IEnumerator _E02D()
	{
		this.m__E06D = false;
		while (CurrentState.Name == EPlayerState.Run)
		{
			float num = Mathf.Sign(BodyAnimatorCommon.GetFloat(_ED3E._E000(130814)));
			if (Math.Abs(_E07C - num) >= float.Epsilon)
			{
				_E07C = num;
				float sinceLastStep = SinceLastStep;
				if (sinceLastStep > 0.2f && MovementContext.FreefallTime < 1f)
				{
					_E07D = Time.time;
					this.m__E06D = true;
					if (CheckSurface(_E079 * Physical.SoundRadius * MovementContext.CovertMovementVolumeBySpeed))
					{
						if (sinceLastStep < 1.2f && PointOfView == EPointOfView.FirstPerson)
						{
							ProceduralWeaponAnimation.Walk.StepFrequency = 0.5f / Mathf.Clamp(sinceLastStep, 0.7f - MovementContext.SmoothedCharacterMovementSpeed / 2f, 1.2f);
						}
						PlayStepSound();
					}
				}
			}
			yield return null;
		}
	}

	private void _E02E(EBodyPart bodyPart, float damage, _EC23 damageInfo)
	{
		if (damageInfo.DamageType == EDamageType.Fall && damage > 1f && !IsAI)
		{
			Say(EPhraseTrigger.OnBeingHurt, demand: true);
		}
	}

	protected (bool hit, bool hitOnTerrain, BaseBallistic.ESurfaceSound? surfaceSound) CalculateMovementSurface()
	{
		if (!Physics.Raycast(new Ray(MovementContext.PlayerColliderCenter, Vector3.down), out var hitInfo, MovementContext.CharacterController.height + 0.5f, _stepLayerMask))
		{
			return (false, false, null);
		}
		bool item = hitInfo.collider != null && hitInfo.collider is TerrainCollider;
		BaseBallistic.ESurfaceSound? item2 = null;
		BaseBallistic component = hitInfo.collider.GetComponent<BaseBallistic>();
		if (component != null)
		{
			item2 = component.GetSurfaceSound(hitInfo.point);
		}
		return (true, item, item2);
	}

	protected void UpdateSurfaceData(bool hit, bool hitOnTerrain, BaseBallistic.ESurfaceSound surfaceSound)
	{
		SurfaceHit = hit;
		SurfaceHitOnTerrain = hitOnTerrain;
		if (SurfaceHit)
		{
			if (CurrentSurface != surfaceSound)
			{
				CurrentSurface = surfaceSound;
				this.m__E064 = this.m__E063[surfaceSound];
			}
			MovementContext.SoftSurface = CurrentSurface == BaseBallistic.ESurfaceSound.Asphalt || CurrentSurface == BaseBallistic.ESurfaceSound.Concrete || CurrentSurface == BaseBallistic.ESurfaceSound.Gravel || CurrentSurface == BaseBallistic.ESurfaceSound.Soil || CurrentSurface == BaseBallistic.ESurfaceSound.Wood || CurrentSurface == BaseBallistic.ESurfaceSound.WoodThick || CurrentSurface == BaseBallistic.ESurfaceSound.Puddle;
		}
	}

	protected virtual void InitVoip(EVoipState voipState)
	{
		VoipState = voipState;
		if (voipState != 0 && voipState != EVoipState.MicrophoneFail)
		{
			if (this is ObservedPlayer)
			{
				_E02F();
			}
			DissonanceComms = DissonanceComms.Instance;
		}
	}

	private void _E02F()
	{
		VoiceBroadcastTrigger broadcastTrigger = base.gameObject.AddComponent<VoiceBroadcastTrigger>();
		broadcastTrigger.ChannelType = CommTriggerTarget.Self;
		_E7E0 settings = Singleton<_E7DE>.Instance.Sound.Settings;
		CompositeDisposable.BindState(settings.VoipDeviceSensitivity, delegate(int value)
		{
			float volume = (float)value / 100f;
			broadcastTrigger.ActivationFader.Volume = volume;
		});
	}

	protected void TrackPlayerPosition()
	{
		DissonanceComms?.TrackPlayerPosition(this);
	}

	public (bool, bool) IsHeard(in Vector3 voicePos, float sqrDistance)
	{
		if (DissonanceComms == null)
		{
			return (false, false);
		}
		bool num = _E030(in voicePos, sqrDistance);
		bool item = num && (VoipState == EVoipState.Available || VoipState == EVoipState.MicrophoneFail);
		return (num, item);
	}

	private bool _E030(in Vector3 voicePos, float sqrDistance)
	{
		return (Position - voicePos).sqrMagnitude <= sqrDistance;
	}

	internal void _E031()
	{
		_E08C?.Invoke();
	}

	private void _E032(_E6C7 controller)
	{
		_E08D?.Invoke(controller);
	}

	protected Task SpawnController(AbstractHandsController controller, Action callback = null)
	{
		TaskCompletionSource onControllerAppeared = new TaskCompletionSource();
		HandsController = controller;
		controller.Spawn(1f, delegate
		{
			_E032(controller);
			callback?.Invoke();
			onControllerAppeared.SetResult(result: true);
		});
		return onControllerAppeared.Task;
	}

	public void FastForwardCurrentOperations()
	{
		if (!(HandsController == null))
		{
			AbstractHandsController handsController;
			do
			{
				handsController = HandsController;
				HandsController.FastForwardCurrentState();
			}
			while (!(handsController == HandsController));
		}
	}

	protected void DestroyController()
	{
		Item item = HandsController.Item;
		FastForwardCurrentOperations();
		HandsController.Destroy();
		_EAFB[] array = _E0DE.SelectEvents<_EAFB>(item).ToArray();
		foreach (_EAFB activeEvent in array)
		{
			_E0DE.RemoveActiveEvent(activeEvent);
		}
		UnityEngine.Object.Destroy(HandsController);
		HandsController = null;
	}

	protected virtual void DropCurrentController(Action callback, bool fastDrop, Item nextControllerItem = null)
	{
		HandsController.Drop(1f, callback, fastDrop, nextControllerItem);
	}

	public void SetEmptyHands(Callback<_E6C9> callback)
	{
		Proceed(withNetwork: true, callback);
	}

	public void SetStationaryWeapon(Weapon weapon)
	{
		Proceed(weapon, delegate
		{
		}, scheduled: false);
	}

	public void SetInHands(Weapon weapon, Callback<_E6CB> callback)
	{
		Proceed(weapon, callback);
	}

	public void SetInHands(_EADF throwWeap, Callback<_E6CC> callback)
	{
		Proceed(throwWeap, callback);
	}

	public void SetInHands(_EA72 meds, EBodyPart bodyPart, int animationVariant, Callback<_E6CF> callback)
	{
		Proceed(meds, bodyPart, callback, animationVariant);
	}

	public void SetInHands(_EA48 foodDrink, float amount, int animationVariant, Callback<_E6CF> callback)
	{
		Proceed(foodDrink, amount, callback, animationVariant);
	}

	public void SetInHands(KnifeComponent knife, Callback<_E6CD> callback)
	{
		Proceed(knife, callback);
	}

	public void SetInHandsUsableItem(Item item, Callback<_E6CE> callback)
	{
		if (item is _EA82)
		{
			Proceed<PortableRangeFinderController>(item, callback);
		}
		if (item is _EA87)
		{
			Proceed<RadioTransmitterController>(item, callback);
		}
	}

	public void SetInHandsForQuickUse(Item quickUseItem, Callback<_E6D4> callback)
	{
		Proceed(quickUseItem, callback);
	}

	public void SetInHandsForQuickUse(_EADF throwWeap, Callback<_E6D2> callback)
	{
		Proceed(throwWeap, callback);
	}

	public void SetInHandsForQuickUse(KnifeComponent knife, Callback<_E6D3> callback)
	{
		Proceed(knife, callback);
	}

	public void TrySetLastEquippedWeapon(bool equipFirstAvaliableOnFail = true)
	{
		if (_E033(LastEquippedWeaponOrKnifeItem))
		{
			TryProceed(LastEquippedWeaponOrKnifeItem, delegate
			{
			});
		}
		else if (equipFirstAvaliableOnFail)
		{
			SetFirstAvailableItem(delegate
			{
			});
		}
	}

	public void SetFirstAvailableItem(Callback<_E6C7> completeCallback)
	{
		Item item = _E08F.Select((EquipmentSlot x) => _E0DE.Inventory.Equipment.GetSlot(x).ContainedItem).FirstOrDefault((Item x) => _E033(x));
		if (item != null)
		{
			SetInHands(item, completeCallback);
			return;
		}
		SetEmptyHands(delegate(Result<_E6C9> result)
		{
			completeCallback(result.Complete ? new Result<_E6C7>(result.Value) : new Result<_E6C7>(null, result.Error));
		});
	}

	private bool _E033(Item itemToCheck)
	{
		if (itemToCheck != null && _E0DE.IsAtReachablePlace(itemToCheck))
		{
			if (itemToCheck.CurrentAddress != null)
			{
				return itemToCheck.CheckAction(null);
			}
			return true;
		}
		return false;
	}

	public void SetInHands(Item item, Callback<_E6C7> callback)
	{
		if (item.CurrentAddress == null || item.CheckAction(null))
		{
			TryProceed(item, callback);
			return;
		}
		SetEmptyHands(delegate(Result<_E6C9> result)
		{
			callback((!string.IsNullOrEmpty(result.Error)) ? new Result<_E6C7>(null, result.Error) : new Result<_E6C7>(result.Value));
		});
	}

	public void SetQuickSlotItem(EBoundItem quickSlot, Callback<_E6C7> callback)
	{
		Item boundItem = _E0DE.Inventory.FastAccess.GetBoundItem(quickSlot);
		if (boundItem == null || (HealthController.IsItemForHealing(boundItem) && !HealthController.CanApplyItem(boundItem, EBodyPart.Common)))
		{
			callback((Result<_E6C7>)(_E6C7)null);
		}
		else if (boundItem.CheckAction(null) && !_E0DE.IsChangingWeapon && (!IsInBufferZone || CanManipulateWithHandsInBufferZone || (IsInBufferZone && HealthController.IsItemForHealing(boundItem))))
		{
			TryProceed(boundItem, callback);
		}
		else
		{
			callback((Result<_E6C7>)(_E6C7)null);
		}
	}

	public void SetSlotItem(EquipmentSlot equipmentSlot, Callback<_E6C7> callback)
	{
		Item containedItem = _E0DE.Inventory.Equipment.GetSlot(equipmentSlot).ContainedItem;
		SetItemInHands(containedItem, callback);
	}

	public void DropBackpack()
	{
		Item containedItem = _E0DE.Inventory.Equipment.GetSlot(EquipmentSlot.Backpack).ContainedItem;
		if (containedItem != null)
		{
			ItemHandsController itemHandsController = HandsController as ItemHandsController;
			if (itemHandsController != null && itemHandsController.CurrentCompassState)
			{
				itemHandsController.SetCompassState(active: false);
			}
			else if (!(MovementContext.StationaryWeapon != null) && _inventoryController.CanThrow(containedItem) && !HandsController.IsPlacingBeacon() && !HandsController.IsInInteractionStrictCheck() && CurrentStateName != EPlayerState.BreachDoor && !IsSprintEnabled)
			{
				_inventoryController.TryThrowItem(containedItem);
			}
		}
	}

	public void SetItemInHands(Item item, Callback<_E6C7> callback)
	{
		if (item != null && item.CheckAction(null) && !_E0DE.IsChangingWeapon && (!IsInBufferZone || CanManipulateWithHandsInBufferZone))
		{
			TryProceed(item, callback);
			return;
		}
		UnityEngine.Debug.LogError(_ED3E._E000(160075));
		callback((Result<_E6C7>)(_E6C7)null);
	}

	internal bool _E034(EquipmentSlot slot)
	{
		return _E0DE.Inventory.Equipment.GetSlot(slot).ContainedItem != null;
	}

	internal bool _E035(EBoundItem slot)
	{
		return _E0DE.Inventory.FastAccess.GetBoundItem(slot) != null;
	}

	public virtual void Interact(IItemOwner loot, Callback callback)
	{
		callback.Succeed();
	}

	protected virtual void Proceed(bool withNetwork, Callback<_E6C9> callback, bool scheduled = true)
	{
		Func<EmptyHandsController> controllerFactory = () => EmptyHandsController._E000<EmptyHandsController>(this);
		new Process<EmptyHandsController, _E6C9>(this, controllerFactory, null)._E000(null, callback, scheduled);
	}

	protected virtual void Proceed(Weapon weapon, Callback<_E6CB> callback, bool scheduled = true)
	{
		Func<FirearmController> controllerFactory = ((!IsAI) ? ((Func<FirearmController>)(() => FirearmController._E000<FirearmController>(this, weapon))) : ((Func<AIFirearmController>)(() => FirearmController._E000<AIFirearmController>(this, weapon))));
		new Process<FirearmController, _E6CB>(this, controllerFactory, weapon)._E000(null, callback, scheduled);
	}

	protected virtual void Proceed(_EADF throwWeap, Callback<_E6CC> callback, bool scheduled = true)
	{
		Func<GrenadeController> controllerFactory = () => GrenadeController._E000<GrenadeController>(this, throwWeap);
		new Process<GrenadeController, _E6CC>(this, controllerFactory, throwWeap)._E000(null, callback, scheduled);
	}

	protected virtual void Proceed(_EA72 meds, EBodyPart bodyPart, Callback<_E6CF> callback, int animationVariant, bool scheduled = true)
	{
		Func<MedsController> controllerFactory = () => MedsController._E000<MedsController>(this, meds, bodyPart, 1f, animationVariant);
		new Process<MedsController, _E6CF>(this, controllerFactory, meds)._E000(null, callback, scheduled);
	}

	protected virtual void Proceed(_EA48 foodDrink, float amount, Callback<_E6CF> callback, int animationVariant, bool scheduled = true)
	{
		Func<MedsController> controllerFactory = () => MedsController._E000<MedsController>(this, foodDrink, EBodyPart.Head, amount, animationVariant);
		new Process<MedsController, _E6CF>(this, controllerFactory, foodDrink)._E000(null, callback, scheduled);
	}

	protected virtual void Proceed(KnifeComponent knife, Callback<_E6CD> callback, bool scheduled = true)
	{
		Func<KnifeController> controllerFactory = () => KnifeController._E000<KnifeController>(this, knife);
		new Process<KnifeController, _E6CD>(this, controllerFactory, knife.Item, fastHide: true)._E000(null, callback, scheduled);
	}

	internal virtual void Proceed<T>(Item item, Callback<_E6CE> callback, bool scheduled = true) where T : UsableItemController
	{
		Func<T> controllerFactory = () => UsableItemController._E000<T>(this, item);
		new Process<T, _E6CE>(this, controllerFactory, item, fastHide: true)._E000(null, callback, scheduled);
	}

	protected virtual void Proceed(Item item, Callback<_E6D4> callback, bool scheduled = true)
	{
		Func<QuickUseItemController> controllerFactory = () => QuickUseItemController._E000<QuickUseItemController>(this, item);
		new Process<QuickUseItemController, _E6D4>(this, controllerFactory, item, fastHide: true, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Succeed, skippable: false)._E000(null, callback, scheduled);
	}

	protected virtual void Proceed(_EADF throwWeap, Callback<_E6D2> callback, bool scheduled = true)
	{
		Func<QuickGrenadeThrowController> controllerFactory = () => QuickGrenadeThrowController._E000<QuickGrenadeThrowController>(this, throwWeap);
		new Process<QuickGrenadeThrowController, _E6D2>(this, controllerFactory, throwWeap, fastHide: true, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Succeed, skippable: false)._E000(null, callback, scheduled);
	}

	protected virtual void Proceed(KnifeComponent knife, Callback<_E6D3> callback, bool scheduled = true)
	{
		Func<QuickKnifeKickController> controllerFactory = () => QuickKnifeKickController._E000<QuickKnifeKickController>(this, knife);
		new Process<QuickKnifeKickController, _E6D3>(this, controllerFactory, knife.Item, fastHide: true, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Succeed, skippable: false)._E000(null, callback, scheduled);
	}

	protected void TryProceed(Item item, Callback<_E6C7> completeCallback, bool scheduled = true)
	{
		_E02C obj = new _E02C();
		obj.completeCallback = completeCallback;
		obj._003C_003E4__this = this;
		if (_handsController is ItemHandsController itemHandsController && item is _EA3E)
		{
			itemHandsController.ToggleCompassState();
			return;
		}
		_E031();
		if (item == null)
		{
			obj._E000();
			return;
		}
		if (!_E0DE.IsAtReachablePlace(item))
		{
			SetFirstAvailableItem(obj.completeCallback);
			return;
		}
		_E02C obj2 = obj;
		if (item != null)
		{
			if (item is Weapon weapon)
			{
				Weapon weapon2 = weapon;
				Proceed(weapon2, delegate(Result<_E6CB> result)
				{
					_E038(result, obj2.completeCallback);
					if (result.Complete)
					{
						obj2._003C_003E4__this.LastEquippedWeaponOrKnifeItem = weapon2;
					}
				}, scheduled);
				return;
			}
			if (item is _EADF obj3)
			{
				_EADF throwWeap = obj3;
				Proceed(throwWeap, delegate(Result<_E6CC> result)
				{
					_E038(result, obj2.completeCallback);
				}, scheduled);
				return;
			}
			if (item is _EA72 obj4)
			{
				_EA72 meds = obj4;
				Proceed(meds, EBodyPart.Common, delegate(Result<_E6CF> result)
				{
					_E038(result, obj2.completeCallback);
				}, item.GetRandomAnimationVariant(), scheduled);
				return;
			}
			if (item is _EA48 obj5)
			{
				_EA48 foodDrink = obj5;
				Proceed(foodDrink, 1f, delegate(Result<_E6CF> result)
				{
					_E038(result, obj2.completeCallback);
				}, item.GetRandomAnimationVariant(), scheduled);
				return;
			}
		}
		KnifeComponent itemComponent = item.GetItemComponent<KnifeComponent>();
		if (itemComponent != null)
		{
			Proceed(itemComponent, delegate(Result<_E6CD> result)
			{
				_E038(result, obj2.completeCallback);
			}, scheduled);
		}
		else if (item is _EA82)
		{
			_E036(item as _EA82, obj2.completeCallback, scheduled);
		}
		else if (item is _EA87)
		{
			_E037(item as _EA87, obj2.completeCallback, scheduled);
		}
		else if (item.UsePrefab != null)
		{
			Proceed(item, delegate(Result<_E6D4> result)
			{
				_E038(result, obj2.completeCallback);
			}, scheduled);
		}
		else
		{
			obj2._E000();
		}
	}

	private void _E036(_EA82 item, Callback<_E6C7> completeCallback, bool scheduled = true)
	{
		if (this is ClientPlayer)
		{
			Proceed<ClientPortableRangeFinderController>(item, delegate(Result<_E6CE> result)
			{
				_E038(result, completeCallback);
			}, scheduled);
		}
		else if (this is ObservedPlayer)
		{
			Proceed<ObservedPortableRangeFinderController>(item, delegate(Result<_E6CE> result)
			{
				_E038(result, completeCallback);
			}, scheduled);
		}
		else
		{
			Proceed<PortableRangeFinderController>(item, delegate(Result<_E6CE> result)
			{
				_E038(result, completeCallback);
			}, scheduled);
		}
	}

	private void _E037(_EA87 item, Callback<_E6C7> completeCallback, bool scheduled = true)
	{
		if (this is ClientPlayer)
		{
			Proceed<ClientRadioTransmitterController>(item, delegate(Result<_E6CE> result)
			{
				_E038(result, completeCallback);
			}, scheduled);
		}
		else if (this is ObservedPlayer)
		{
			Proceed<ObservedRadioTransmitterController>(item, delegate(Result<_E6CE> result)
			{
				_E038(result, completeCallback);
			}, scheduled);
		}
		else
		{
			Proceed<RadioTransmitterController>(item, delegate(Result<_E6CE> result)
			{
				_E038(result, completeCallback);
			}, scheduled);
		}
	}

	private static void _E038<_E077>(Result<_E077> result, Callback<_E6C7> callback) where _E077 : _E6C7
	{
		callback?.Invoke(new Result<_E6C7>(result.Value, result.Error));
	}

	private void _E039(Item item)
	{
		if (item == null)
		{
			return;
		}
		EquipmentSlot[] array = new EquipmentSlot[4]
		{
			EquipmentSlot.FirstPrimaryWeapon,
			EquipmentSlot.SecondPrimaryWeapon,
			EquipmentSlot.Holster,
			EquipmentSlot.Scabbard
		};
		_EB0B equipment = _E0DE.Inventory.Equipment;
		foreach (EquipmentSlot slotName in array)
		{
			if (item == equipment.GetSlot(slotName).ContainedItem)
			{
				ActiveSlot = equipment.GetSlot(slotName);
				break;
			}
		}
	}

	protected bool CanPerformAnimatedOperation(Item item, _EB73 operation)
	{
		if (!HealthController.IsAlive)
		{
			return true;
		}
		if (HandsController.CanExecute(operation))
		{
			return true;
		}
		if (HandsController.Item != item)
		{
			return true;
		}
		if (HandsController is BaseGrenadeController && !HandsController.CanRemove())
		{
			return false;
		}
		return true;
	}

	protected void TryRemoveFromHands(Item item, _EB72 abstractOperation, Callback callback)
	{
		if (HandsController == null)
		{
			UnityEngine.Debug.LogFormat(_ED3E._E000(160108), item);
			callback.Succeed();
		}
		else if (!HealthController.IsAlive)
		{
			callback.Succeed();
		}
		else if (HandsController.Item == item)
		{
			if (HandsController is BaseGrenadeController && !HandsController.CanRemove())
			{
				callback.Fail(_ED3E._E000(160171));
			}
			else
			{
				SetControllerInsteadRemovedOne(item, callback);
			}
		}
		else if (HandsController.CanExecute(abstractOperation))
		{
			_removeFromHandsCallback = callback;
			_E0DE.RaiseInOutProcessEvents(new _EB08(HandsController.Item, CommandStatus.Begin));
			HandsController.Execute(abstractOperation, delegate(IResult result)
			{
				if ((object)_removeFromHandsCallback == callback)
				{
					_removeFromHandsCallback = null;
				}
				_E0DE.RaiseInOutProcessEvents(new _EB08(HandsController.Item, CommandStatus.Succeed));
				callback(result);
			});
		}
		else
		{
			callback.Fail(_ED3E._E000(160195));
		}
	}

	protected virtual void SetControllerInsteadRemovedOne(Item removingItem, Callback callback)
	{
		_removeFromHandsCallback = callback;
		Item containedItem = _E0DE.Inventory.Equipment.GetSlot(EquipmentSlot.Scabbard).ContainedItem;
		KnifeComponent knifeComponent = containedItem?.GetItemComponent<KnifeComponent>();
		bool flag = containedItem is _EA82;
		bool flag2 = containedItem is _EA87;
		if (knifeComponent != null && removingItem != knifeComponent.Item)
		{
			Proceed(knifeComponent, delegate(Result<_E6CD> result)
			{
				if ((object)_removeFromHandsCallback == callback)
				{
					_removeFromHandsCallback = null;
				}
				callback.Invoke(result);
			}, scheduled: false);
			return;
		}
		if (flag && removingItem != containedItem)
		{
			Proceed<PortableRangeFinderController>(containedItem, delegate(Result<_E6CE> result)
			{
				if ((object)_removeFromHandsCallback == callback)
				{
					_removeFromHandsCallback = null;
				}
				callback.Invoke(result);
			}, scheduled: false);
			return;
		}
		if (flag2 && removingItem != containedItem)
		{
			Proceed<RadioTransmitterController>(containedItem, delegate(Result<_E6CE> result)
			{
				if ((object)_removeFromHandsCallback == callback)
				{
					_removeFromHandsCallback = null;
				}
				callback.Invoke(result);
			}, scheduled: false);
			return;
		}
		Proceed(withNetwork: false, delegate(Result<_E6C9> result)
		{
			if ((object)_removeFromHandsCallback == callback)
			{
				_removeFromHandsCallback = null;
			}
			callback.Invoke(result);
		}, scheduled: false);
	}

	protected void TrySetInHands(Item item, ItemAddress to, _EB72 operation, Callback originalCallback)
	{
		if (!HealthController.IsAlive || (IsInBufferZone && !CanManipulateWithHandsInBufferZone))
		{
			originalCallback.Succeed();
		}
		else if (!(operation is _EB89) && ActiveSlot == to.Container)
		{
			_E090 = originalCallback;
			TryProceed(item, delegate(Result<_E6C7> result)
			{
				if ((object)_E090 == originalCallback)
				{
					_E090 = null;
				}
				originalCallback.Invoke(result);
			}, scheduled: false);
		}
		else if ((item.Parent != to || operation is _EB89) && HandsController.CanExecute(operation))
		{
			_E090 = originalCallback;
			_E0DE.RaiseInOutProcessEvents(new _EB08(HandsController.Item, CommandStatus.Begin));
			HandsController.Execute(operation, delegate(IResult error)
			{
				if ((object)_E090 == originalCallback)
				{
					_E090 = null;
				}
				_E0DE.RaiseInOutProcessEvents(new _EB08(HandsController.Item, CommandStatus.Succeed));
				originalCallback(error);
			});
		}
		else if (operation is _EB89 && !HandsController.CanExecute(operation))
		{
			originalCallback.Fail(_ED3E._E000(160273));
		}
		else
		{
			originalCallback.Succeed();
		}
	}

	public IEnumerator FakeCallbackCoroutine()
	{
		while (HealthController.IsAlive)
		{
			yield return null;
		}
		if (_removeFromHandsCallback != null)
		{
			_removeFromHandsCallback.Succeed();
			_removeFromHandsCallback = null;
		}
		if (_E090 != null)
		{
			_E090.Succeed();
			_E090 = null;
		}
	}

	public virtual void FaceshieldMarkOperation(FaceShieldComponent armor, bool hasServerOrigin)
	{
		if (Time.time > _E091 + Time.fixedDeltaTime)
		{
			_E091 = Time.time;
			_E092 = 0;
		}
		if (_E092 <= 3)
		{
			_E0DE.ExecuteShotOperation(armor);
			_E092++;
		}
	}

	protected IContainer FindContainer(_E9F7 containerId, IItemOwner owner = null)
	{
		if (!(FindItemById(containerId.ParentId).OrElse(null) is ContainerCollection containerCollection))
		{
			return null;
		}
		return containerCollection.GetContainer(containerId.Id);
	}

	internal _E012 _E03A(Item item)
	{
		if (item != null)
		{
			_E012 obj = new _E012(this, item);
			obj.Execute();
			return obj;
		}
		UnityEngine.Debug.LogWarning(_ED3E._E000(160313));
		return null;
	}

	private _E013 _E03B(Item item)
	{
		if (item != null)
		{
			_E013 obj = new _E013(this, item);
			obj.Execute();
			return obj;
		}
		UnityEngine.Debug.LogWarning(_ED3E._E000(160323));
		return null;
	}

	public _ECD8<_E010> ThrowGrenade(_EADF grenade, bool lowThrow, bool simulate)
	{
		if (!grenade.IsChildOf(_E0DE.Inventory.Equipment))
		{
			return new _ECD2(_ED3E._E000(160386) + grenade.Id);
		}
		_ECD8<_EB38> obj = _EB29.Discard(HandsController.Item, _E0DE, simulate);
		if (obj.Failed)
		{
			return obj.Error;
		}
		if (!simulate)
		{
			Physical.OnThrow(lowThrow);
		}
		return new _E010(obj.Value, grenade, this, lowThrow);
	}

	protected static TPlayer Create<TPlayer>(ResourceKey assetName, int playerId, Vector3 position, EUpdateQueue updateQueue, EUpdateMode armsUpdateMode, EUpdateMode bodyUpdateMode, CharacterControllerSpawner.Mode characterControllerMode, Func<float> getSensitivity, Func<float> getAimingSensitivity, string prefix, bool isThirdPerson) where TPlayer : Player
	{
		GameObject gameObject = Singleton<_E760>.Instance.CreatePlayerObject(assetName);
		gameObject.name = prefix + gameObject.name;
		gameObject.transform.parent = null;
		Animator componentInChildren = gameObject.GetComponentInChildren<Animator>(includeInactive: true);
		gameObject.SetActive(value: true);
		return _E03C<TPlayer>(gameObject, componentInChildren, playerId, position, updateQueue, armsUpdateMode, bodyUpdateMode, characterControllerMode, getSensitivity, getAimingSensitivity, isThirdPerson);
	}

	private static _E0A1 _E03C<_E0A1>(GameObject poolObject, Animator animator, int playerId, Vector3 position, EUpdateQueue updateQueue, EUpdateMode armsUpdateMode, EUpdateMode bodyUpdateMode, CharacterControllerSpawner.Mode characterControllerMode, Func<float> getSensitivity, Func<float> getAimingSensitivity, bool isThirdPerson) where _E0A1 : Player
	{
		PlayerPoolObject component = poolObject.GetComponent<PlayerPoolObject>();
		_E0A1 val = poolObject.AddComponent<_E0A1>();
		val.PlayerId = playerId;
		component.RegisteredComponentsToClean.Add(val);
		val._updateQueue = updateQueue;
		val.GetSensitivity = getSensitivity;
		val.GetAimingSensitivity = getAimingSensitivity;
		val.MalfRandoms = new _EC14(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
		val._E0A7 = new _EC19(512, 0);
		val._armsUpdateMode = armsUpdateMode;
		val.TrunkRotationLimit = EFTHardSettings.Instance.HANDS_TO_BODY_MAX_ANGLE;
		val._bodyUpdateMode = bodyUpdateMode;
		val.PlayerBones = component.PlayerBones;
		val.PlayerBones.Player = val;
		val._animators = new IAnimator[2];
		val.CreateBodyAnimator(animator, updateQueue);
		foreach (Collider collider in component.Colliders)
		{
			collider.enabled = true;
		}
		val.Transform.Original.position = position;
		val._characterController = component.CharacterControllerSpawner.Spawn(characterControllerMode, val, val.gameObject, isSpirit: false, isThirdPerson);
		val._triggerColliderSearcher = component.CharacterControllerSpawner.TriggerColliderSearcher;
		val.POM = component.PlayerOverlapManager;
		if (val.POM != null)
		{
			val.POM.Init(val._characterController.GetCollider());
		}
		IKAuthority[] behaviours = val.BodyAnimatorCommon.GetBehaviours<IKAuthority>();
		for (int i = 0; i < behaviours.Length; i++)
		{
			behaviours[i].PlayerBones = val.PlayerBones;
		}
		val.Grounder = component.GrounderFbbik;
		val.Grounder.enabled = true;
		val._fbbik = component.FullBodyBipedIk;
		val.HitReaction = component.HitReaction;
		val.HitReaction.enabled = true;
		((Player)val).m__E03F = component.LimbIks;
		LimbIK[] array = ((Player)val).m__E03F;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		if (_E2B6.Config.UseSpiritPlayer)
		{
			PlayerSpirit playerSpirit = Singleton<_E760>.Instance.CreateFromPool<PlayerSpirit>(_E5D2.PLAYER_SPIRIT_RESOURCE_KEY);
			playerSpirit.transform.position = Vector3.zero;
			playerSpirit.gameObject.SetActive(value: true);
			val.Spirit = playerSpirit;
			val.Transform.Original.SetParent(playerSpirit.transform, worldPositionStays: false);
			playerSpirit.Init(val, position, val._bodyUpdateMode != EUpdateMode.None, characterControllerMode, null);
		}
		val.Logger = new PlayerLogger(LoggerMode.Add);
		return val;
	}

	public virtual _E338 CreatePhysical()
	{
		return new _E336();
	}

	public virtual void CreateBodyAnimator(Animator animator, EUpdateQueue updateQueue)
	{
		if (!_E2B6.Config.UseBodyFastAnimator)
		{
			_animators[0] = _E563.CreateAnimator(animator);
			_animators[0].cullingMode = AnimatorCullingMode.AlwaysAnimate;
			_animators[0].updateMode = ((updateQueue == EUpdateQueue.FixedUpdate) ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal);
			return;
		}
		_E4EB fastAnimatorController = _E508.Deserialize(Singleton<_ED0A>.Instance.GetAsset<TextAsset>(_E5D2.PLAYER_FAST_ANIMATOR_CONTROLLER).bytes);
		RootMotionBlendTable asset = Singleton<_ED0A>.Instance.GetAsset<RootMotionBlendTable>(_E5D2.PLAYER_ROOTMOTION_TABLE);
		asset.LoadNodes();
		_bodyUpdateMode = EUpdateMode.Manual;
		_animators[0] = _E563.CreateAnimator(fastAnimatorController, asset._loadedNodes, PlayerBones.BodyTransform.Original, PlayerBones.PlayableAnimator);
		_animators[0].cullingMode = AnimatorCullingMode.AlwaysAnimate;
		_animators[0].updateMode = ((updateQueue == EUpdateQueue.FixedUpdate) ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal);
		CharacterClipsKeeper asset2 = Singleton<_ED0A>.Instance.GetAsset<CharacterClipsKeeper>(_E5D2.PLAYER_ANIMATION_CLIPS_KEEPER);
		_E502 obj = _animators[0] as _E502;
		PlayerBones.PlayableAnimator.Init(_animators[0], obj.GetParametersCache(), asset, asset2, manualUpdate: false);
		PlayerBones.PlayableAnimator.SetCuller(new _E500(PlayerBones.PlayableAnimator));
		PlayerBones.PlayableAnimator.Play();
		for (int i = 0; i < PlayerBones.PlayableAnimator.initialLayerInfo.Length; i++)
		{
			_animators[0].SetLayerWeight(i, PlayerBones.PlayableAnimator.initialLayerInfo[i].weight);
		}
	}

	private void _E03D()
	{
		if (!(_E0AB == null) && !(_E0AC == null) && !_E2B6.Config.UseBodyFastAnimator)
		{
			_animators[0].runtimeAnimatorController = _E0AC;
			if (_E0AB.runtimeAnimatorController == null)
			{
				_E0AB.runtimeAnimatorController = _E0AC;
			}
			_E0AB = null;
			_E0AC = null;
		}
	}

	protected internal async Task Init(Quaternion rotation, string layerName, EPointOfView pointOfView, Profile profile, _EAED inventoryController, _E9C4 healthController, _E759 statisticsManager, _E935 questController, _E60F filter, EVoipState voipState, bool aiControlled = false, bool async = true)
	{
		_E03D();
		if (async)
		{
			await JobScheduler.Yield();
		}
		Profile = profile;
		_E016();
		if (async)
		{
			await JobScheduler.Yield();
		}
		StatisticsManager = statisticsManager;
		InitialProfileExamineAll();
		MainParts = _E1FF.Create(GetPlayer, PlayerBones);
		_inventoryController = inventoryController;
		_E0DE.RegisterView(this);
		_E0A6.Value = _E0DE.ItemInHands;
		ExfilUnsubscribe = InteractingWithExfiltrationPoint.Bind(delegate
		{
			_E0AE?.Invoke();
		});
		CreateSlotObservers();
		DogtagComponent dogtagComponent = Equipment.GetSlot(EquipmentSlot.Dogtag).ContainedItem?.GetItemComponent<DogtagComponent>();
		if (dogtagComponent != null)
		{
			dogtagComponent.GroupId = Profile.Info.GroupId;
		}
		FaceShieldObserver.Changed.Subscribe(_E01F);
		FaceCoverObserver.Changed.Subscribe(_E020);
		_questController = questController;
		_playerBody = PlayerBones.AnimatedTransform.Original.gameObject.GetComponent<PlayerBody>();
		UpdatePhones();
		Task task = _playerBody.Init(filter.FilterCustomization(profile.Customization), Equipment, _E0A6, LayerMask.NameToLayer(_ED3E._E000(60679)), Side);
		if (async)
		{
			await task;
		}
		_healthController = healthController;
		if (async)
		{
			await JobScheduler.Yield();
		}
		Physical = CreatePhysical();
		_E005();
		_E011(pointOfView);
		Physical.Init(this);
		Physical.EncumberedChanged += HealthController.SetEncumbered;
		Physical.OverEncumberedChanged += HealthController.SetOverEncumbered;
		Physical.OnWeightUpdated();
		_E38B.SetLayersRecursively(base.gameObject, LayerMask.NameToLayer(layerName));
		SetupHitColliders();
		EventTranslator = Transform.Original.GetChild(0).gameObject.AddComponent<GenericEventTranslator>();
		MovementContext.OnStateChanged += delegate(EPlayerState prevState, EPlayerState nextState)
		{
			ProceduralWeaponAnimation.WalkEffectorEnabled = nextState == EPlayerState.Run;
			ProceduralWeaponAnimation.DrawEffectorEnabled = nextState != EPlayerState.ProneMove;
			ProceduralWeaponAnimation.TiltBlender.Target = ((nextState == EPlayerState.Idle || nextState == EPlayerState.ProneIdle) ? 1 : 0);
			if (nextState == EPlayerState.Stationary)
			{
				ProceduralWeaponAnimation.SetStrategy(new _E3CE());
			}
			else if (prevState == EPlayerState.Stationary)
			{
				ProceduralWeaponAnimation.SetStrategy(pointOfView);
			}
		};
		MovementContext.PhysicalConditionChanged += ProceduralWeaponAnimation.PhysicalConditionUpdated;
		HealthController.EffectStartedEvent += OnHealthEffectAdded;
		HealthController.EffectResidualEvent += OnHealthEffectRemoved;
		HealthController.HealthChangedEvent += _E040;
		HealthController.BodyPartDestroyedEvent += _E03F;
		HealthController.BodyPartRestoredEvent += _E03E;
		HealthController.PropagateAllEffects();
		profile.OnTraderStandingChanged += TraderStandingHandler;
		Speaker = new _E76C
		{
			OnDemandOnly = !aiControlled
		};
		Speaker.Init(profile.Info.Side, PlayerId, profile.Info.Voice);
		Speaker.TrackTransform = Transform;
		Speaker.OnPhraseTold += OnPhraseTold;
		Singleton<GameWorld>.Instance.SpeakerManager.AssignToGroup(profile.Info.Side, this);
		InitAudioController();
		MovementContext.Rotation = new Vector2(rotation.eulerAngles.y, 0f);
		_playerLookRaycastTransform = PlayerBones.LootRaycastOrigin;
		StartCoroutine(FakeCallbackCoroutine());
		Pedometer = new _E6D7(this);
		ConnectSkillManager();
		float current = HealthController.Temperature.Current;
		PlayerBody.SetTemperatureForBody(current);
		RecalculateEquipmentParams();
		statisticsManager.Init(this);
		StartCoroutine(_E059());
		if (_E2B6.Config.UseSpiritPlayer)
		{
			Spirit.InitAfterPlayerInit();
		}
		Loyalty = new _E27C(this);
		OnGameSessionBegin();
		_healthController.DiedEvent += OnDead;
		Singleton<GameWorld>.Instance.RegisterPlayer(this);
		if (_triggerColliderSearcher != null)
		{
			_triggerColliderSearcher.ConnectToCharacterController(_characterController);
			_triggerColliderSearcher.IsEnabled = true;
		}
		InitVoip(voipState);
		InitializeRecodableItemHandlers();
		StartCoroutine(_E041());
	}

	protected virtual void CreateSlotObservers()
	{
		NightVisionObserver = new _E014<NightVisionComponent>(Equipment.GetSlot(EquipmentSlot.Headwear), (NightVisionComponent nv, Action handler) => nv.Togglable.OnChanged.Subscribe(handler));
		ThermalVisionObserver = new _E014<ThermalVisionComponent>(Equipment.GetSlot(EquipmentSlot.Headwear), (ThermalVisionComponent tv, Action handler) => tv.Togglable.OnChanged.Subscribe(handler));
		FaceShieldObserver = new _E014<FaceShieldComponent>(Equipment.GetSlot(EquipmentSlot.Headwear), delegate(FaceShieldComponent fs, Action handler)
		{
			Action togglableSub2 = fs.Togglable?.OnChanged.Subscribe(handler);
			Action hitSub2 = fs.HitsChanged.Subscribe(handler);
			return delegate
			{
				togglableSub2?.Invoke();
				hitSub2();
			};
		});
		FaceCoverObserver = new _E014<FaceShieldComponent>(Equipment.GetSlot(EquipmentSlot.FaceCover), delegate(FaceShieldComponent fs, Action handler)
		{
			Action togglableSub = fs.Togglable?.OnChanged.Subscribe(handler);
			Action hitSub = fs.HitsChanged.Subscribe(handler);
			return delegate
			{
				togglableSub?.Invoke();
				hitSub();
			};
		});
	}

	protected virtual void InitialProfileExamineAll()
	{
		Profile.UncoverAll();
	}

	private void _E03E(EBodyPart arg1, ValueStruct arg2)
	{
		ExecuteSkill(Skills.SurgeryAction.Complete);
		UpdateSpeedLimitByHealth();
	}

	private void _E03F(EBodyPart arg1, EDamageType arg2)
	{
		if ((arg1 == EBodyPart.LeftLeg || arg1 == EBodyPart.RightLeg) && CurrentState.Name == EPlayerState.Sprint)
		{
			StartInflictSelfDamageCoroutine();
		}
	}

	public virtual void Say(EPhraseTrigger @event, bool demand = false, float delay = 0f, ETagStatus mask = (ETagStatus)0, int probability = 100, bool aggressive = false)
	{
		if (@event == EPhraseTrigger.Cooperation)
		{
			_E073(EGesture.Hello);
		}
		if (@event == EPhraseTrigger.MumblePhrase)
		{
			@event = ((aggressive || Time.time < Awareness) ? EPhraseTrigger.OnFight : EPhraseTrigger.OnMutter);
		}
		if (Speaker.OnDemandOnly && !demand)
		{
			_E0AF?.Invoke(@event, 5);
			return;
		}
		if (Singleton<_E307>.Instantiated)
		{
			Singleton<_E307>.Instance.SayPhrase(this, @event);
		}
		if (demand || probability > 99 || probability > UnityEngine.Random.Range(0, 100))
		{
			ETagStatus eTagStatus = ((!aggressive && !(Awareness > Time.time)) ? ETagStatus.Unaware : ETagStatus.Combat);
			if (delay > 0f)
			{
				Speaker.Queue(@event, HealthStatus | mask | eTagStatus, delay, demand);
			}
			else
			{
				Speaker.Play(@event, HealthStatus | mask | eTagStatus, demand);
			}
		}
	}

	public void NeedRepairMalfPhraseSituation(Weapon.EMalfunctionState malfState, bool isKnown)
	{
		if (isKnown && (malfState == Weapon.EMalfunctionState.SoftSlide || malfState == Weapon.EMalfunctionState.HardSlide))
		{
			_E0AF?.Invoke(EPhraseTrigger.OnWeaponJammed, 5);
		}
		else
		{
			_E0AF?.Invoke(EPhraseTrigger.WeaponBroken, 5);
		}
	}

	protected virtual void OnPhraseTold(EPhraseTrigger @event, TaggedClip clip, TagBank bank, _E76C speaker)
	{
		_E01C(clip);
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		MovementContext.OnControllerColliderHit(hit);
	}

	protected virtual void OnDestroy()
	{
		Destroyed = true;
		_E8A8.Instance.FoVUpdateAction -= OnFovUpdatedEvent;
		if (Speaker != null)
		{
			Speaker.OnPhraseTold -= OnPhraseTold;
			Speaker.OnDestroy();
		}
		_E024();
		Dispose();
		if (_E2B6.Config.UseSpiritPlayer && Spirit != null)
		{
			Spirit.IsStub = true;
			UnityEngine.Object.Destroy(Spirit.gameObject);
		}
	}

	protected virtual void TraderStandingHandler(Profile._E001 traderInfo)
	{
	}

	public virtual void OnInteractWithLightHouseTraderZone(_EC06[] AllowedPlayers, _EC06[] UnallowedPlayers)
	{
	}

	public virtual void OnLighthouseTraderZoneDebugToolSwitch(bool active)
	{
	}

	public virtual void ShowStringNotification(string message)
	{
	}

	private void _E040(EBodyPart bodyPart, float diff, _EC23 damageInfo)
	{
		if (!(Mathf.Abs(diff) < 0.01f))
		{
			switch (bodyPart)
			{
			case EBodyPart.LeftLeg:
			case EBodyPart.RightLeg:
				UpdateSpeedLimitByHealth();
				break;
			case EBodyPart.LeftArm:
			case EBodyPart.RightArm:
				UpdateArmsCondition();
				break;
			}
			UpdateBreathStatus();
		}
	}

	public virtual void UpdateArmsCondition()
	{
		bool val = HealthController.IsBodyPartBroken(EBodyPart.LeftArm) || HealthController.IsBodyPartDestroyed(EBodyPart.LeftArm);
		bool val2 = HealthController.IsBodyPartBroken(EBodyPart.RightArm) || HealthController.IsBodyPartDestroyed(EBodyPart.RightArm);
		MovementContext.SetPhysicalCondition(EPhysicalCondition.LeftArmDamaged, val);
		MovementContext.SetPhysicalCondition(EPhysicalCondition.RightArmDamaged, val2);
	}

	public virtual void OnChangeRadioTransmitterState(bool isEncoded, RadioTransmitterStatus status, bool isAgressor)
	{
		if (RecodableItemsHandler.TryToGetRecodableComponent<RadioTransmitterRecodableComponent>(out var component))
		{
			component.SetStatus(status);
		}
		IsAgressorInLighthouseTraderZone = isAgressor;
	}

	public virtual void OnHealthEffectAdded(_E992 effect)
	{
		bool flag = true;
		if (effect is _E9B0)
		{
			if (MovementContext.PhysicalConditionIs(EPhysicalCondition.OnPainkillers))
			{
				flag = false;
			}
		}
		else if (effect is _E9B1 || effect is _E9AA || effect is _E9A2)
		{
			UpdateSpeedLimitByHealth();
			UpdateArmsCondition();
			if (effect is _E9AA)
			{
				Physical.BerserkRestorationFactor = true;
				Say(EPhraseTrigger.OnFight, demand: true);
			}
			if (effect is _E9A2 obj && !obj.WasPaused && FractureSound != null && Singleton<BetterAudio>.Instantiated)
			{
				Singleton<BetterAudio>.Instance.PlayAtPoint(Position, FractureSound, _E8A8.Instance.Distance(Position), BetterAudio.AudioSourceGroupType.Impacts, 15, 1f, EOcclusionTest.Fast);
			}
		}
		else if (effect is _E9B4)
		{
			MovementContext.SetPhysicalCondition(EPhysicalCondition.Tremor, val: true);
		}
		else if (effect is _E9BF obj2)
		{
			MovementContext.SetPhysicalCondition(EPhysicalCondition.UsingMeds, val: true);
			if (obj2.NoMove)
			{
				MovementContext.SetPhysicalCondition(EPhysicalCondition.HealingLegs, val: true);
			}
		}
		else if (effect is _E9AF)
		{
			ExecuteSkill((Action)delegate
			{
				Skills.LowHPDuration.Begin();
			});
		}
		else if (effect is _E9B9)
		{
			_E0A1 = effect;
		}
		else if (effect is _E9AC && !(Equipment.GetSlot(EquipmentSlot.Earpiece).ContainedItem is _EA54))
		{
			Singleton<BetterAudio>.Instance.StartTinnitusEffect(effect.WorkStateTime, this.m__E062);
		}
		if (flag)
		{
			ExecuteSkill((Action)delegate
			{
				Skills.HealthNegativeEffect.Complete(effect);
			});
		}
	}

	public virtual void OnHealthEffectRemoved(_E992 effect)
	{
		if (effect is _E9B1 || effect is _E9AA || effect is _E9A2)
		{
			UpdateSpeedLimitByHealth();
			UpdateArmsCondition();
			if (effect is _E9AA)
			{
				Physical.BerserkRestorationFactor = false;
			}
		}
		else if (effect is _E9B4 && _healthController.FindActiveEffect<_E9B4>() == null)
		{
			MovementContext.SetPhysicalCondition(EPhysicalCondition.Tremor, val: false);
		}
		else if (effect is _E9BF && _healthController.FindActiveEffect<_E9BF>() == null)
		{
			MovementContext.SetPhysicalCondition(EPhysicalCondition.UsingMeds, val: false);
			MovementContext.SetPhysicalCondition(EPhysicalCondition.HealingLegs, val: false);
			MovementContext.SetPhysicalCondition(EPhysicalCondition.RightLegDamaged, HealthController.IsBodyPartBroken(EBodyPart.RightLeg) || HealthController.IsBodyPartDestroyed(EBodyPart.RightLeg));
			MovementContext.SetPhysicalCondition(EPhysicalCondition.LeftLegDamaged, HealthController.IsBodyPartBroken(EBodyPart.LeftLeg) || HealthController.IsBodyPartDestroyed(EBodyPart.LeftLeg));
		}
		else if (effect is _E9AF)
		{
			ExecuteSkill((Action)delegate
			{
				Skills.LowHPDuration.Complete();
			});
		}
		else if (effect is _E9B9)
		{
			_E0A1 = null;
		}
	}

	protected virtual void UpdateSpeedLimitByHealth()
	{
		MovementContext.SetPhysicalCondition(EPhysicalCondition.OnPainkillers, HealthController.FindActiveEffect<_E9B1>() != null || HealthController.FindActiveEffect<_E9AA>() != null);
		MovementContext.SetPhysicalCondition(EPhysicalCondition.RightLegDamaged, HealthController.IsBodyPartBroken(EBodyPart.RightLeg) || HealthController.IsBodyPartDestroyed(EBodyPart.RightLeg));
		MovementContext.SetPhysicalCondition(EPhysicalCondition.LeftLegDamaged, HealthController.IsBodyPartBroken(EBodyPart.LeftLeg) || HealthController.IsBodyPartDestroyed(EBodyPart.LeftLeg));
		RemoveStateSpeedLimit(ESpeedLimit.HealthCondition);
		if (!MovementContext.PhysicalConditionIs(EPhysicalCondition.RightLegDamaged) && !MovementContext.PhysicalConditionIs(EPhysicalCondition.LeftLegDamaged))
		{
			return;
		}
		if (!MovementContext.PhysicalConditionIs(EPhysicalCondition.OnPainkillers))
		{
			MovementContext.EnableSprint(enable: false);
			if (MovementContext.PhysicalConditionIs(EPhysicalCondition.RightLegDamaged) && MovementContext.PhysicalConditionIs(EPhysicalCondition.LeftLegDamaged))
			{
				AddStateSpeedLimit(0.2f, ESpeedLimit.HealthCondition);
			}
			else
			{
				AddStateSpeedLimit(0.3f, ESpeedLimit.HealthCondition);
			}
		}
		if (CurrentState.Name == EPlayerState.Sprint)
		{
			StartInflictSelfDamageCoroutine();
		}
	}

	public void OnItemRemoved(_EAF3 eventArgs)
	{
		if (eventArgs.Status == CommandStatus.Succeed)
		{
			OnItemAddedOrRemoved(eventArgs.Item, eventArgs.From, added: false);
		}
	}

	public void OnSetInHands(_EAFA eventArgs)
	{
		if (eventArgs.Status == CommandStatus.Succeed)
		{
			_E0A6.Value = eventArgs.Item;
		}
	}

	public void OnItemAdded(_EAF2 eventArgs)
	{
		if (eventArgs.Status == CommandStatus.Succeed)
		{
			OnItemAddedOrRemoved(eventArgs.Item, eventArgs.To, added: true);
			ArmorComponent itemComponent = eventArgs.Item.GetItemComponent<ArmorComponent>();
			if (itemComponent != null)
			{
				OnArmorPointsChanged(itemComponent, children: true);
			}
			SideEffectComponent itemComponent2 = eventArgs.Item.GetItemComponent<SideEffectComponent>();
			if (itemComponent2 != null)
			{
				OnSideEffectApplied(itemComponent2);
			}
		}
	}

	protected void UpdatePhonesReally()
	{
		_EA40 obj = Equipment.GetSlot(EquipmentSlot.Headwear).ContainedItem as _EA40;
		_EA40 obj2 = Equipment.GetSlot(EquipmentSlot.FaceCover).ContainedItem as _EA40;
		_EA54 obj3 = (Equipment.GetSlot(EquipmentSlot.Earpiece).ContainedItem as _EA54) ?? obj?.GetAllItemsFromCollection().OfType<_EA54>().FirstOrDefault();
		_EA53 obj4 = null;
		if (obj3 != null)
		{
			obj4 = obj3.Template;
		}
		if (obj != null && obj4 == null)
		{
			obj4 = (from x in obj.GetItemComponentsInChildren<CompositeArmorComponent>()
				select x.Deaf into d
				orderby (int)d descending
				select d).FirstOrDefault() switch
			{
				EDeafStrength.Low => BetterAudio.LowMute, 
				EDeafStrength.High => BetterAudio.StrongMute, 
				_ => null, 
			};
		}
		if (obj2 != null && obj4 == null)
		{
			obj4 = (from x in obj2.GetItemComponentsInChildren<CompositeArmorComponent>()
				select x.Deaf into d
				orderby (int)d descending
				select d).FirstOrDefault() switch
			{
				EDeafStrength.Low => BetterAudio.LowMute, 
				EDeafStrength.High => BetterAudio.StrongMute, 
				_ => null, 
			};
		}
		Singleton<BetterAudio>.Instance.SetCompressor(obj4);
	}

	protected virtual void UpdatePhones()
	{
		if (IsYourPlayer)
		{
			UpdatePhonesReally();
		}
	}

	protected virtual void OnItemAddedOrRemoved(Item item, ItemAddress location, bool added)
	{
		if (location is _EB22)
		{
			return;
		}
		Slot[] headSlots = new Slot[3]
		{
			Equipment.GetSlot(EquipmentSlot.Eyewear),
			Equipment.GetSlot(EquipmentSlot.Headwear),
			Equipment.GetSlot(EquipmentSlot.FaceCover)
		};
		IEnumerable<ItemAddress> allParentLocations = location.GetAllParentLocations(onlyMerged: true);
		if (allParentLocations.Any((ItemAddress loc) => headSlots.Contains(loc.Container)))
		{
			_E0B0?.Invoke(HasGlasses);
			if (item.GetItemComponentsInChildren<NightVisionComponent>().Any())
			{
				NightVisionObserver.Update();
			}
			if (item.GetItemComponentsInChildren<ThermalVisionComponent>().Any())
			{
				ThermalVisionObserver.Update();
			}
			if (item.GetItemComponentsInChildren<FaceShieldComponent>().Any())
			{
				FaceShieldObserver.Update();
				FaceCoverObserver.Update();
			}
		}
		Slot[] armorSlots = _EAE7.ArmorSlots.Select(Equipment.GetSlot).ToArray();
		if ((item.GetItemComponentsInChildren<ArmorComponent>().Any() && allParentLocations.Any((ItemAddress loc) => armorSlots.Contains(loc.Container))) || item.GetItemComponent<EquipmentPenaltyComponent>() != null)
		{
			RecalculateEquipmentParams();
		}
		UpdatePhones();
	}

	[CanBeNull]
	public TItem TryGetItemInHands<TItem>() where TItem : Item
	{
		if (_handsController == null)
		{
			return null;
		}
		return _handsController.Item as TItem;
	}

	protected _E712.EWeaponAnimationType GetWeaponAnimationType(AbstractHandsController handsController)
	{
		if (handsController == null || handsController.Item == null || handsController is EmptyHandsController)
		{
			return _E712.EWeaponAnimationType.EmptyHands;
		}
		Item item = handsController.Item;
		if (item is _EACD || item is _EA82 || item is _EA87)
		{
			return _E712.EWeaponAnimationType.Pistol;
		}
		if (item is _EAD1 obj)
		{
			if (!obj.WeapClass.Equals(_ED3E._E000(32735)))
			{
				return _E712.EWeaponAnimationType.Rifle;
			}
			return _E712.EWeaponAnimationType.Pistol;
		}
		if (item is _EABF obj2 && obj2.IsFlareGun)
		{
			return _E712.EWeaponAnimationType.Pistol;
		}
		if (item.GetItemComponent<KnifeComponent>() != null)
		{
			return _E712.EWeaponAnimationType.Knife;
		}
		if (item is _EADF)
		{
			return _E712.EWeaponAnimationType.ThrowWeapon;
		}
		return _E712.EWeaponAnimationType.Rifle;
	}

	public void SetDeltaTimeDelegate(_E015 deltaTimeDelegate)
	{
		_E0CE = deltaTimeDelegate ?? _E0CD;
	}

	private void FixedUpdate()
	{
		_nFixedFrames++;
		_fixedTime += Time.fixedDeltaTime;
	}

	public virtual void FixedUpdateTick()
	{
		ComplexUpdate(EUpdateQueue.FixedUpdate, Time.fixedUnscaledDeltaTime);
	}

	public virtual void AfterMainTick()
	{
	}

	public virtual void UpdateTick()
	{
		float deltaTime = DeltaTime;
		ComplexUpdate(EUpdateQueue.Update, deltaTime);
	}

	private IEnumerator _E041()
	{
		while (true)
		{
			yield return _E0CF;
			ComplexLateUpdate(EUpdateQueue.FixedUpdate, Time.fixedUnscaledDeltaTime);
		}
	}

	public void ComplexUpdate(EUpdateQueue queue, float deltaTime)
	{
		if (HealthController != null && HealthController.IsAlive)
		{
			if (UpdateQueue == queue)
			{
				ManualUpdate(deltaTime);
				_bodyupdated = true;
				_bodyTime = deltaTime;
			}
			if (ArmsUpdateQueue == queue)
			{
				ArmsUpdate(deltaTime);
				_armsupdated = true;
				_armsTime = deltaTime;
			}
		}
	}

	protected virtual void ComplexLateUpdate(EUpdateQueue queue, float deltaTime)
	{
		if (ArmsUpdateQueue == queue && ArmsUpdateMode == EUpdateMode.Auto)
		{
			_E042(deltaTime);
		}
	}

	public virtual void ArmsUpdate(float deltaTime)
	{
		if (_handsController != null)
		{
			_handsController.ManualUpdate(deltaTime);
		}
		if (ArmsUpdateMode == EUpdateMode.Manual)
		{
			ArmsAnimatorCommon.Update(deltaTime);
			UnderbarrelWeaponArmsAnimator?.Update(deltaTime);
		}
		_armsupdated = true;
		_armsTime = deltaTime;
		if (ArmsUpdateMode == EUpdateMode.Manual)
		{
			_E042(deltaTime);
		}
	}

	public virtual void BodyUpdate(float deltaTime, int loop = 1)
	{
		if (BodyUpdateMode == EUpdateMode.Manual)
		{
			for (int i = 0; i < loop; i++)
			{
				float dt = deltaTime / (float)loop;
				BodyAnimatorCommon.Update(dt);
			}
		}
		if (_E2B6.Config.UseBodyFastAnimator && HealthController != null && HealthController.IsAlive)
		{
			PlayerBones.PlayableAnimator.Process(IsVisible, deltaTime);
		}
	}

	public virtual void ManualUpdate(float deltaTime, int loop = 1)
	{
		LastDeltaTime = deltaTime;
		if (Mathf.Approximately(deltaTime, 0f))
		{
			UnityEngine.Debug.LogErrorFormat(_ED3E._E000(160418), deltaTime);
			return;
		}
		Physical.Update(deltaTime);
		for (int i = 0; i < loop; i++)
		{
			_E00E(deltaTime / (float)loop);
		}
		HealthControllerUpdate(deltaTime);
		BodyUpdate(deltaTime);
		if (Vector2.Distance(this.m__E00D, this.m__E00C) < 0.1f)
		{
			this.m__E00C = this.m__E00D;
		}
		else
		{
			this.m__E00C = Vector2.Lerp(this.m__E00C, this.m__E00D, 0.1f);
		}
		UpdateTriggerColliderSearcher(deltaTime);
	}

	protected virtual void HealthControllerUpdate(float deltaTime)
	{
		if (!(_healthController is _E981))
		{
			_healthController.ManualUpdate(deltaTime);
		}
	}

	protected virtual void UpdateTriggerColliderSearcher(float deltaTime, bool isCloseToCamera = true)
	{
		GetTriggerColliderSearcher().ManualUpdate(deltaTime, isCloseToCamera);
	}

	private void _E042(float deltaTime)
	{
		if (_handsController != null)
		{
			_handsController.EmitEvents();
			if (IsYourPlayer)
			{
				_E320._E001.SyncTransforms();
			}
			_handsController.BallisticUpdate(deltaTime);
		}
	}

	public void SetOwnerToAIData(BotOwner bot)
	{
		if (AIData == null)
		{
			AIData = new _E279(bot, this);
		}
		else
		{
			AIData.BotOwner = bot;
		}
		Physical.EncumberDisabled = true;
	}

	public virtual void InteractionRaycast()
	{
		if (_playerLookRaycastTransform == null || !HealthController.IsAlive)
		{
			return;
		}
		InteractableObject interactableObject = null;
		InteractableObjectIsProxy = false;
		Player player = null;
		Ray interactionRay = InteractionRay;
		if (CurrentState.CanInteract && (bool)HandsController && HandsController.CanInteract())
		{
			RaycastHit hit;
			GameObject gameObject = GameWorld.FindInteractable(interactionRay, out hit);
			if (gameObject != null)
			{
				InteractiveProxy interactiveProxy = null;
				interactableObject = gameObject.GetComponentInParent<InteractableObject>();
				if (interactableObject == null)
				{
					interactiveProxy = gameObject.GetComponent<InteractiveProxy>();
					if (interactiveProxy != null && hit.distance < EFTHardSettings.Instance.DOOR_RAYCAST_DISTANCE + EFTHardSettings.Instance.BEHIND_CAST)
					{
						InteractableObjectIsProxy = true;
						interactableObject = interactiveProxy.Link;
					}
				}
				if (interactableObject != null && interactiveProxy == null)
				{
					if (interactableObject.InteractsFromAppropriateDirection(LookDirection))
					{
						if (hit.distance > EFTHardSettings.Instance.LOOT_RAYCAST_DISTANCE + EFTHardSettings.Instance.BEHIND_CAST || !interactableObject.isActiveAndEnabled)
						{
							interactableObject = null;
						}
						else if (hit.distance > EFTHardSettings.Instance.DOOR_RAYCAST_DISTANCE + EFTHardSettings.Instance.BEHIND_CAST && interactableObject is Door)
						{
							interactableObject = null;
						}
					}
					else
					{
						interactableObject = null;
					}
				}
				player = ((interactableObject == null) ? gameObject.GetComponent<Player>() : null);
			}
			RayLength = hit.distance;
		}
		if (interactableObject is WorldInteractiveObject worldInteractiveObject)
		{
			if (worldInteractiveObject is BufferGateSwitcher bufferGateSwitcher)
			{
				_ = bufferGateSwitcher.BufferGatesState;
				if (interactableObject == InteractableObject)
				{
					_E0D1 = true;
				}
			}
			else
			{
				EDoorState doorState = worldInteractiveObject.DoorState;
				if (doorState == EDoorState.Interacting || !worldInteractiveObject.Operatable)
				{
					interactableObject = null;
				}
				else if (interactableObject == InteractableObject && _E0D0 != doorState)
				{
					_E0D1 = true;
				}
			}
		}
		else if (interactableObject is LootItem lootItem)
		{
			if (lootItem.Item is Weapon weapon && weapon.IsOneOff && weapon.Repairable.Durability == 0f)
			{
				interactableObject = null;
			}
		}
		else if (interactableObject is StationaryWeapon stationaryWeapon)
		{
			if (stationaryWeapon.Locked)
			{
				interactableObject = null;
			}
			else if (interactableObject == InteractableObject && _E0D0 != stationaryWeapon.State)
			{
				_E0D1 = true;
			}
		}
		else if (interactableObject != null)
		{
			if (_E0D2 != interactableObject.StateUpdateTime)
			{
				_E0D1 = true;
			}
			_E0D2 = interactableObject.StateUpdateTime;
		}
		if (interactableObject != InteractableObject || _E0D1)
		{
			_E0D1 = false;
			InteractableObject = interactableObject;
			if (InteractableObject is WorldInteractiveObject worldInteractiveObject2)
			{
				_E0D0 = worldInteractiveObject2.DoorState;
			}
			else if (InteractableObject is StationaryWeapon stationaryWeapon2)
			{
				_E0D0 = stationaryWeapon2.State;
			}
			_E0AE?.Invoke();
		}
		if (player != InteractablePlayer || _E0D1)
		{
			_E0D1 = false;
			InteractablePlayer = ((player != this) ? player : null);
			if (player == this)
			{
				UnityEngine.Debug.LogWarning(Profile.Nickname + _ED3E._E000(160502));
			}
			_E0AE?.Invoke();
		}
		if (player == null && interactableObject == null)
		{
			float radius = 0.1f * (1f + (float)Skills.PerceptionLootDot);
			float distance = 1.5f;
			if ((bool)Skills.PerceptionEliteNoIdea)
			{
				distance = 2.35f;
				radius = 1.1f;
				interactionRay.origin = Transform.position + Vector3.up * 3f;
				interactionRay.direction = Vector3.down;
			}
			_E0DB = GameWorld.InteractionSense(interactionRay.origin, interactionRay.direction, radius, distance);
		}
		else
		{
			_E0DB = false;
		}
	}

	public virtual void PauseAllEffectsOnPlayer()
	{
	}

	public virtual void UnpauseAllEffectsOnPlayer()
	{
	}

	public virtual void ShowHelloNotification(string sender)
	{
		_E857.DisplayMessageNotification(string.Format(_ED3E._E000(160532).Localized(), sender));
	}

	public void ResetInteractionRaycast(_E2B8 @object)
	{
		if (@object == InteractableObject)
		{
			InteractableObject = null;
			_E0AE?.Invoke();
		}
	}

	public void OnPlaceItemTriggerChanged([CanBeNull] PlaceItemTrigger zone)
	{
		PlaceItemZone = zone;
		if (zone == null)
		{
			DestroyBeacon();
		}
		_E0AE?.Invoke();
	}

	public void AddTriggerZone(TriggerWithId zone)
	{
		string id = zone.Id;
		if (!TriggerZones.Contains(id))
		{
			TriggerZones.Add(id);
		}
	}

	public void RemoveTriggerZone(TriggerWithId zone)
	{
		string id = zone.Id;
		if (TriggerZones.Contains(id))
		{
			TriggerZones.Remove(id);
		}
	}

	public void UpdateInteractionCast()
	{
		_E0D1 = true;
	}

	protected virtual void SetupHitColliders()
	{
		_hitColliders = GetComponentsInChildren<BodyPartCollider>();
		BodyPartCollider[] hitColliders = _hitColliders;
		foreach (BodyPartCollider obj in hitColliders)
		{
			obj.Player = this;
			obj.gameObject.layer = LayerMask.NameToLayer(_ED3E._E000(60764));
		}
	}

	public virtual void SetInventoryOpened(bool opened)
	{
		if (opened)
		{
			MovementContext.SetBlindFire(0);
		}
		_E09F = opened;
		InventoryOpenRaiseAction(opened);
		if (_handsController != null)
		{
			_handsController.SetInventoryOpened(opened);
		}
	}

	public void InventoryOpenRaiseAction(bool opened)
	{
		_E0B1?.Invoke(this, opened);
	}

	public virtual void ExecuteShotSkill(Item weapon)
	{
		if (!(weapon is _EADF) && !IsAI)
		{
			Type type = weapon.GetType();
			if (typeof(_EAAF).IsAssignableFrom(type))
			{
				type = typeof(_EAAF);
			}
			float val = (Skills.WeaponBuffs.ContainsKey(type) ? Skills.WeaponBuffs[type][EBuffId.WeaponDoubleMastering].Value : 1f);
			Skills.WeaponShotAction.Complete(weapon, val);
		}
	}

	protected virtual void ManageAggressor(_EC23 damageInfo, EBodyPart bodyPart, EHeadSegment? headSegment)
	{
		if (_isDeadAlready)
		{
			return;
		}
		if (!HealthController.IsAlive)
		{
			_isDeadAlready = true;
		}
		Player player = damageInfo.Player;
		if ((object)player == this)
		{
			return;
		}
		if (player == null)
		{
			Profile.Stats.Aggressor = null;
			LastAggressor = null;
			LastDamageInfo = damageInfo;
			LastBodyPart = bodyPart;
			return;
		}
		if (damageInfo.Weapon != null && !player.Profile.Info.GroupId.EqualsAndNotNull(Profile.Info.GroupId))
		{
			player.ExecuteShotSkill(damageInfo.Weapon);
		}
		bool isHeavyDamage = damageInfo.DidBodyDamage / HealthController.GetBodyPartHealth(bodyPart).Maximum >= 0.6f && HealthController.FindActiveEffect<_E9A1>(bodyPart) != null;
		player.StatisticsManager.OnEnemyDamage(damageInfo, bodyPart, Profile.Info.Side, Profile.Info.Settings.Role.ToString(), Profile.Info.GroupId, HealthController.GetBodyPartHealth(EBodyPart.Common).Maximum, isHeavyDamage, Vector3.Distance(player.Transform.position, Transform.position), CurrentHour, Inventory.EquippedInSlotsTemplateIds, HealthController.BodyPartEffects, TriggerZones);
		if (!string.IsNullOrEmpty(player.Profile.Info.Nickname) && !(player == this) && !(player.ProfileId == ProfileId))
		{
			_E043(damageInfo, bodyPart, player);
			LastAggressor = player;
			LastDamageInfo = damageInfo;
			LastBodyPart = bodyPart;
			_E279 aIData = player.AIData;
			Profile profile = ((aIData != null && aIData.IsAI) ? null : player.Profile);
			Profile.Stats.Aggressor = new AggressorStats(profile?.AccountId, profile?.Id, player.Profile.Nickname, player.Profile.Info.MainProfileNickname, player.Profile.Info.Side, bodyPart, (damageInfo.Weapon != null) ? damageInfo.Weapon.ShortName : string.Empty, player.Profile.Info.MemberCategory, headSegment);
			if (!HealthController.IsAlive)
			{
				player.Say(EPhraseTrigger.OnEnemyDown, demand: false, UnityEngine.Random.Range(0f, 1f), Speaker.SideTag, 70);
				return;
			}
			player.Say(EPhraseTrigger.OnEnemyShot, demand: false, UnityEngine.Random.Range(0f, 1f), Speaker.SideTag, 30);
			Singleton<GameWorld>.Instance.SpeakerManager.GroupEvent(PlayerId, EPhraseTrigger.Hit, Transform.position, player.Speaker.SideTag, 30);
		}
	}

	private void _E043(_EC23 damageInfo, EBodyPart bodyPart, Player aggressor)
	{
		if (aggressor != null && aggressor.AIData.IsAI)
		{
			BotOwner botOwner = aggressor.AIData.BotOwner;
			botOwner.EnemiesController.HitTarget(this, damageInfo, bodyPart);
			botOwner.BotPersonalStats.HitTarget(this, damageInfo, bodyPart);
		}
	}

	public virtual void ApplyExplosionDamageToArmor(Dictionary<EBodyPart, float> armorDamage, _EC23 damageInfo)
	{
		_preAllocatedArmorComponents.Clear();
		Inventory.GetPutOnArmorsNonAlloc(_preAllocatedArmorComponents);
		foreach (ArmorComponent preAllocatedArmorComponent in _preAllocatedArmorComponents)
		{
			float num = 0f;
			EBodyPart[] armorZone = preAllocatedArmorComponent.ArmorZone;
			foreach (EBodyPart key in armorZone)
			{
				if (armorDamage.TryGetValue(key, out var value))
				{
					num += value;
				}
			}
			if (num > 0f)
			{
				num = preAllocatedArmorComponent.ApplyExplosionDurabilityDamage(num, damageInfo);
				_E045(num, preAllocatedArmorComponent);
			}
		}
	}

	public bool IsShotDeflectedByHeavyArmor(EBodyPart bodyPart, int shotSeed)
	{
		if (!Skills.HeavyVestNoBodyDamageDeflectChance)
		{
			return false;
		}
		_E5CB._E01F heavyVests = Singleton<_E5CB>.Instance.SkillsSettings.HeavyVests;
		_preAllocatedArmorComponents.Clear();
		Inventory.GetPutOnArmorsNonAlloc(_preAllocatedArmorComponents);
		foreach (ArmorComponent preAllocatedArmorComponent in _preAllocatedArmorComponents)
		{
			RepairableComponent repairable = preAllocatedArmorComponent.Repairable;
			if (preAllocatedArmorComponent.ArmorType != EArmorType.Heavy || repairable.Durability < heavyVests.RicochetChanceHVestsCurrentDurabilityThreshold * repairable.MaxDurability || repairable.Durability < heavyVests.RicochetChanceHVestsMaxDurabilityThreshold * (float)repairable.TemplateDurability)
			{
				continue;
			}
			EBodyPart[] armorZone = preAllocatedArmorComponent.ArmorZone;
			for (int i = 0; i < armorZone.Length; i++)
			{
				if (armorZone[i] == bodyPart && _E0A7.GetRandom(shotSeed) < heavyVests.RicochetChanceHVestsEliteLevel)
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool _E044(EBodyPart bodyPart)
	{
		if (!Skills.LightVestBleedingProtection)
		{
			return false;
		}
		_preAllocatedArmorComponents.Clear();
		Inventory.GetPutOnArmorsNonAlloc(_preAllocatedArmorComponents);
		foreach (ArmorComponent preAllocatedArmorComponent in _preAllocatedArmorComponents)
		{
			if (preAllocatedArmorComponent.IsDestroyed || preAllocatedArmorComponent.ArmorType != EArmorType.Light)
			{
				continue;
			}
			EBodyPart[] armorZone = preAllocatedArmorComponent.ArmorZone;
			for (int i = 0; i < armorZone.Length; i++)
			{
				if (armorZone[i] == bodyPart)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void _E045(float armorDamage, ArmorComponent armorComponent)
	{
		if (!(armorDamage > 0.1f) || armorComponent.IsDestroyed)
		{
			return;
		}
		switch (armorComponent.ArmorType)
		{
		case EArmorType.Light:
			ExecuteSkill((Action)delegate
			{
				Skills.LightArmorDamageTakenAction.Complete(armorDamage);
			});
			break;
		case EArmorType.Heavy:
			ExecuteSkill((Action)delegate
			{
				Skills.HeavyArmorDamageTakenAction.Complete(armorDamage);
			});
			break;
		}
	}

	[CanBeNull]
	protected List<ArmorComponent> ProceedDamageThroughArmor(ref _EC23 damageInfo, EBodyPart bodyPartType, int pitch, int yaw, bool damageInfoIsLocal = true)
	{
		_preAllocatedArmorComponents.Clear();
		Inventory.GetPutOnArmorsNonAlloc(_preAllocatedArmorComponents);
		List<ArmorComponent> list = null;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (ArmorComponent preAllocatedArmorComponent in _preAllocatedArmorComponents)
		{
			flag3 = preAllocatedArmorComponent.Item.Template._id == _EB1D.InvincibleBalaclava;
			if (flag3)
			{
				break;
			}
		}
		foreach (ArmorComponent preAllocatedArmorComponent2 in _preAllocatedArmorComponents)
		{
			float num = 0f;
			if (preAllocatedArmorComponent2.ShotMatches(bodyPartType, pitch, yaw))
			{
				if (flag || flag2)
				{
					float num2 = preAllocatedArmorComponent2.BluntThroughput;
					if (preAllocatedArmorComponent2.ArmorType == EArmorType.Heavy)
					{
						num2 *= 1f - (float)Skills.HeavyVestBluntThroughputDamageReduction;
					}
					damageInfo.Damage *= num2;
				}
				else
				{
					if (list == null)
					{
						list = new List<ArmorComponent>();
					}
					list.Add(preAllocatedArmorComponent2);
					if (_healthController.IsAlive)
					{
						num = preAllocatedArmorComponent2.ApplyDamage(ref damageInfo, bodyPartType, damageInfoIsLocal, Skills.LightVestMeleeWeaponDamageReduction, Skills.HeavyVestBluntThroughputDamageReduction);
						_E045(num, preAllocatedArmorComponent2);
					}
					flag = preAllocatedArmorComponent2.Item.Id == damageInfo.BlockedBy;
					flag2 = preAllocatedArmorComponent2.Item.Id == damageInfo.DeflectedBy;
				}
			}
			if (num > 0.1f)
			{
				OnArmorPointsChanged(preAllocatedArmorComponent2);
			}
		}
		if (flag3)
		{
			damageInfo.Damage = 0f;
		}
		return list;
	}

	public void SetDogtagInfo(_E94B deathPacket)
	{
		if (_E0D6)
		{
			return;
		}
		_E0D6 = true;
		EPlayerSide side = (EPlayerSide)deathPacket.Side;
		if (side == EPlayerSide.Savage)
		{
			return;
		}
		Item containedItem = Equipment.GetSlot(EquipmentSlot.Dogtag).ContainedItem;
		if (containedItem == null)
		{
			UnityEngine.Debug.LogErrorFormat(_ED3E._E000(160513), side, FullIdInfo);
			return;
		}
		DogtagComponent itemComponent = containedItem.GetItemComponent<DogtagComponent>();
		if (itemComponent != null)
		{
			itemComponent.Item.SpawnedInSession = true;
			itemComponent.AccountId = deathPacket.AccountId;
			itemComponent.ProfileId = deathPacket.ProfileId;
			itemComponent.Nickname = deathPacket.Nickname;
			itemComponent.KillerAccountId = deathPacket.KillerAccountId;
			itemComponent.KillerProfileId = deathPacket.KillerProfileId;
			itemComponent.KillerName = deathPacket.KillerName;
			itemComponent.Side = side;
			itemComponent.Level = deathPacket.Level;
			itemComponent.Time = _E5AD.UniversalDateTimeFromUnixTime(deathPacket.Time).ToLocalTime();
			itemComponent.Status = deathPacket.Status;
			itemComponent.WeaponName = deathPacket.WeaponName;
		}
		else
		{
			UnityEngine.Debug.LogError(_ED3E._E000(160585));
		}
	}

	private void _E046(_EC23 damageInfo)
	{
		Player player = damageInfo.Player;
		Vector3 direction = damageInfo.Direction * -1f;
		_E3BF.HitArmor(this, player, direction);
	}

	protected virtual void OnArmorPointsChanged(ArmorComponent armor, bool children = false)
	{
	}

	protected virtual void OnSideEffectApplied(SideEffectComponent sideEffect)
	{
	}

	protected void RecalculateEquipmentParams()
	{
		float num = 0f;
		float num2 = 0f;
		float ergonomicsPenalty = ErgonomicsPenalty;
		_preAllocatedArmorComponents.Clear();
		Inventory.GetPutOnArmorsNonAlloc(_preAllocatedArmorComponents);
		this.m__E04B = 0f;
		for (int i = 0; i < _preAllocatedArmorComponents.Count; i++)
		{
			ArmorComponent armorComponent = _preAllocatedArmorComponents[i];
			float num3 = 0f;
			if (armorComponent.ArmorType == EArmorType.Light)
			{
				num3 = Skills.LightVestMoveSpeedPenaltyReduction;
			}
			if (armorComponent.ArmorType == EArmorType.Heavy)
			{
				num3 = Skills.HeavyVestMoveSpeedPenaltyReduction;
			}
			num += (float)armorComponent.SpeedPenalty * (1f - num3);
			num2 += (float)armorComponent.MousePenalty * (1f - num3);
			this.m__E04B += armorComponent.WeaponErgonomicPenalty;
		}
		_preAllocatedBackpackPenaltyComponent = Inventory.GetPutOnBackpack();
		if (_preAllocatedBackpackPenaltyComponent != null)
		{
			num += (float)_preAllocatedBackpackPenaltyComponent.Template.SpeedPenaltyPercent;
			num2 += (float)_preAllocatedBackpackPenaltyComponent.Template.MousePenalty;
			this.m__E04B += _preAllocatedBackpackPenaltyComponent.Template.WeaponErgonomicPenalty;
		}
		_preAllocatedArmorComponents.Clear();
		this.m__E04B /= 100f;
		RemoveStateSpeedLimit(ESpeedLimit.Armor);
		if (num < 0f)
		{
			AddStateSpeedLimit((100f + num) / 100f * MovementContext.MaxSpeed, ESpeedLimit.Armor);
		}
		if (Math.Abs(this.m__E04B - ergonomicsPenalty) > 0f)
		{
			ProceduralWeaponAnimation.UpdateWeaponVariables();
		}
		RemoveMouseSensitivityModifier(EMouseSensitivityModifier.Armor);
		if (num2 < 0f)
		{
			AddMouseSensitivityModifier(EMouseSensitivityModifier.Armor, num2 / 100f);
		}
	}

	public virtual void ApplyHitDebuff(float damage, float staminaBurnRate, EBodyPart bodyPartType, EDamageType damageType)
	{
		if (damageType.IsEnemyDamage())
		{
			IncreaseAwareness(20f);
		}
		if (HealthController.IsAlive && (!MovementContext.PhysicalConditionIs(EPhysicalCondition.OnPainkillers) || damage > 4f) && !IsAI)
		{
			if (Speaker != null)
			{
				Speaker.Play(EPhraseTrigger.OnBeingHurt, HealthStatus, demand: true);
			}
			else
			{
				UnityEngine.Debug.LogError(_ED3E._E000(160703));
			}
		}
		if (damageType.IsWeaponInduced())
		{
			_E09C = ((_E09B == Time.frameCount) ? (_E09C + staminaBurnRate) : staminaBurnRate);
			float num = Mathf.InverseLerp(55f, 10f, _E09C);
			if (num < 1f)
			{
				UpdateSpeedLimit(num, ESpeedLimit.Shot, 0.66f);
			}
			_E09B = Time.frameCount;
			Physical.BulletHit(staminaBurnRate);
			if ((bodyPartType == EBodyPart.LeftLeg || bodyPartType == EBodyPart.RightLeg) && !MovementContext.PhysicalConditionIs(EPhysicalCondition.OnPainkillers))
			{
				Physical.Sprint(target: false);
			}
		}
	}

	public virtual bool SetShotStatus(BodyPartCollider bodypart, _EC26 shot, Vector3 hitpoint, Vector3 shotNormal, Vector3 shotDirection)
	{
		_preAllocatedArmorComponents.Clear();
		Inventory.GetPutOnArmorsNonAlloc(_preAllocatedArmorComponents);
		int pitch = 60;
		int yaw = 0;
		if (bodypart.BodyPartType == EBodyPart.Head)
		{
			Vector3 normalized = (PlayerBones.Head.Original.InverseTransformPoint(hitpoint) - (bodypart.Collider as SphereCollider).center).normalized;
			pitch = (int)(Mathf.Asin(0f - normalized.x) * 57.29578f);
			yaw = (int)(Mathf.Atan2(normalized.z, normalized.y) * 57.29578f);
			yaw = ((yaw > 180) ? (360 - yaw) : ((yaw < 0) ? (-yaw) : yaw));
		}
		for (int i = 0; i < _preAllocatedArmorComponents.Count; i++)
		{
			ArmorComponent armorComponent = _preAllocatedArmorComponents[i];
			if (armorComponent.ShotMatches(bodypart.BodyPartType, pitch, yaw))
			{
				if (armorComponent.Deflects(shotDirection, shotNormal, shot))
				{
					return true;
				}
				if (string.IsNullOrEmpty(shot.BlockedBy))
				{
					armorComponent.SetPenetrationStatus(shot);
				}
			}
		}
		return false;
	}

	public bool CheckArmorHitByDirection(BodyPartCollider bodypart, Vector3 hitpoint, Vector3 shotNormal, Vector3 shotDirection)
	{
		_preAllocatedArmorComponents.Clear();
		Inventory.GetPutOnArmorsNonAlloc(_preAllocatedArmorComponents);
		int pitch = 60;
		int yaw = 0;
		if (bodypart.BodyPartType == EBodyPart.Head)
		{
			Vector3 normalized = (PlayerBones.Head.Original.InverseTransformPoint(hitpoint) - (bodypart.Collider as SphereCollider).center).normalized;
			pitch = (int)(Mathf.Asin(0f - normalized.x) * 57.29578f);
			yaw = (int)(Mathf.Atan2(normalized.z, normalized.y) * 57.29578f);
			yaw = ((yaw > 180) ? (360 - yaw) : ((yaw < 0) ? (-yaw) : yaw));
		}
		for (int i = 0; i < _preAllocatedArmorComponents.Count; i++)
		{
			if (_preAllocatedArmorComponents[i].ShotMatches(bodypart.BodyPartType, pitch, yaw))
			{
				return true;
			}
		}
		return false;
	}

	public virtual _E6FF ApplyShot(_EC23 damageInfo, EBodyPart bodyPartType, _EC22 shotId)
	{
		_E97E activeHealthController = ActiveHealthController;
		if (activeHealthController != null && !activeHealthController.IsAlive)
		{
			return null;
		}
		int pitch = 60;
		int yaw = 0;
		if (bodyPartType == EBodyPart.Head)
		{
			Vector3 normalized = (PlayerBones.Head.Original.InverseTransformPoint(damageInfo.HitPoint) - (damageInfo.HitCollider as SphereCollider).center).normalized;
			pitch = (int)(Mathf.Asin(0f - normalized.x) * 57.29578f);
			yaw = (int)(Mathf.Atan2(normalized.z, normalized.y) * 57.29578f);
			yaw = ((yaw > 180) ? (360 - yaw) : ((yaw < 0) ? (-yaw) : yaw));
		}
		bool num = !string.IsNullOrEmpty(damageInfo.DeflectedBy);
		float damage = damageInfo.Damage;
		List<ArmorComponent> list = ProceedDamageThroughArmor(ref damageInfo, bodyPartType, pitch, yaw);
		MaterialType material = (num ? MaterialType.HelmetRicochet : ((list == null || list.Count < 1) ? MaterialType.Body : list[0].Material));
		_E6FF obj = new _E6FF
		{
			PoV = PointOfView,
			Penetrated = (string.IsNullOrEmpty(damageInfo.BlockedBy) || string.IsNullOrEmpty(damageInfo.DeflectedBy)),
			Material = material,
			Pitch = pitch,
			Yaw = yaw
		};
		EHeadSegment? headSegment = ((bodyPartType == EBodyPart.Head) ? _E9D3.GetSegment(pitch, yaw) : null);
		ApplyDamageInfo(damageInfo, bodyPartType, 0f, headSegment);
		ShotReactions(damageInfo, bodyPartType);
		float absorbed = damage - damageInfo.Damage;
		ReceiveDamage(damageInfo.Damage, bodyPartType, damageInfo.DamageType, absorbed, obj.Material);
		return obj;
	}

	public virtual void ApplyDamageInfo(_EC23 damageInfo, EBodyPart bodyPartType, float absorbed, EHeadSegment? headSegment = null)
	{
		_E97E activeHealthController = ActiveHealthController;
		if (activeHealthController != null && !activeHealthController.IsAlive)
		{
			return;
		}
		EDamageType damageType = damageInfo.DamageType;
		LastDamagedBodyPart = bodyPartType;
		Player player = damageInfo.Player;
		if (ActiveHealthController != null)
		{
			ActiveHealthController.DoWoundRelapse(damageInfo.Damage, bodyPartType);
			LastAggressor = player;
			LastDamageInfo = damageInfo;
			LastBodyPart = bodyPartType;
			damageInfo.BleedBlock = _E044(bodyPartType);
			float value = (damageInfo.DidBodyDamage = ActiveHealthController.ApplyDamage(bodyPartType, damageInfo.Damage, damageInfo));
			ActiveHealthController.BluntContusion(bodyPartType, absorbed);
			if (value.Positive() && ActiveHealthController.TryApplySideEffects(damageInfo, bodyPartType, out var sideEffectComponent))
			{
				damageInfo.Player.OnSideEffectApplied(sideEffectComponent);
			}
		}
		else
		{
			damageInfo.DidBodyDamage = 0f;
		}
		player?.Loyalty.MarkAsAggressor(this);
		ManageAggressor(damageInfo, bodyPartType, headSegment);
		ApplyHitDebuff(damageInfo.Damage, damageInfo.StaminaBurnRate * damageInfo.Damage, bodyPartType, damageType);
		if (!damageType.IsWeaponInduced())
		{
			ReceiveDamage(damageInfo.Damage, bodyPartType, damageType, 0f, MaterialType.None);
		}
		this.m__E051?.Invoke(damageInfo, bodyPartType, 0f);
		if (Singleton<_E307>.Instantiated)
		{
			Singleton<_E307>.Instance.BeingHitAction(damageInfo, this);
		}
		if (!(player == null) && !HealthController.IsAlive && Singleton<_E307>.Instantiated)
		{
			Singleton<_E307>.Instance.Kill(player, GetPlayer);
		}
	}

	public virtual void AddDetailedHitInfo(EDamageType damageType, int d, int absorbed, int staminaLoss, EBodyPart part, MaterialType special)
	{
	}

	protected virtual bool ShouldVocalizeDeath(EBodyPart bodyPart)
	{
		return bodyPart != EBodyPart.Head;
	}

	protected virtual void OnBeenKilledByAggressor(Player aggressor, _EC23 damageInfo, EBodyPart bodyPart, EDamageType lethalDamageType)
	{
		if (!AggressorFound && (object)this != aggressor)
		{
			AggressorFound = true;
			float distance = Vector3.Distance(aggressor.Position, Position);
			aggressor.AIData?.KillEnemy(this);
			aggressor.StatisticsManager.OnEnemyKill(damageInfo, lethalDamageType, bodyPart, Profile.Info.Side, Profile.Info.Settings.Role, Profile.AccountId, Profile.Id, Profile.Nickname, Profile.Info.GroupId, Profile.Info.Level, Profile.Info.Settings.Experience, distance, CurrentHour, Inventory.EquippedInSlotsTemplateIds, HealthController.BodyPartEffects, TriggerZones);
		}
	}

	protected virtual void OnDead(EDamageType damageType)
	{
		_ECC9.ReleaseBeginSample(_ED3E._E000(160678), _ED3E._E000(160724));
		float time = 0f;
		if (LastAggressor != null)
		{
			OnBeenKilledByAggressor(LastAggressor, LastDamageInfo, LastBodyPart, damageType);
		}
		if (_E2B6.Config.UseSpiritPlayer)
		{
			Spirit.Die();
		}
		LastDamageType = damageType;
		_ECC9.ReleaseBeginSample(_ED3E._E000(160723), _ED3E._E000(160724));
		_E096?.Invoke(this, LastAggressor, LastDamageInfo, LastBodyPart);
		_E097?.Invoke(this, LastAggressor, LastDamageInfo, LastBodyPart);
		_E098?.Invoke(this);
		_ECC9.ReleaseBeginSample(_ED3E._E000(160748), _ED3E._E000(160724));
		if (ShouldVocalizeDeath(LastDamagedBodyPart))
		{
			EPhraseTrigger trigger = (LastDamageType.IsWeaponInduced() ? EPhraseTrigger.OnDeath : EPhraseTrigger.OnAgony);
			TagBank tagBank = Speaker.Play(trigger, HealthStatus, demand: true);
			if (tagBank != null)
			{
				TaggedClip taggedClip = tagBank.Match((int)HealthStatus);
				if (taggedClip != null)
				{
					time = taggedClip.Length;
				}
				else
				{
					time = 0f;
					UnityEngine.Debug.LogErrorFormat(_ED3E._E000(162836), tagBank.name, _ED3E._E000(162863), HealthStatus);
				}
			}
		}
		else
		{
			Speaker.Shut();
		}
		MovementContext.ReleaseDoorIfInteractingWithOne();
		if (base.gameObject.GetComponent<_E6D5>() != null)
		{
			StaticManager.Instance.WaitSeconds(time, delegate
			{
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.PlayerIsDead);
			});
		}
		MovementContext.OnStateChanged -= _E012;
		MovementContext.PhysicalConditionChanged -= ProceduralWeaponAnimation.PhysicalConditionUpdated;
		_E0DE.UnregisterView(this);
		_ECC9.ReleaseBeginSample(_ED3E._E000(162908), _ED3E._E000(160724));
		ExfilUnsubscribe();
		EnabledAnimators = (EAnimatorMask)0;
		_ECC9.ReleaseBeginSample(_ED3E._E000(162940), _ED3E._E000(160724));
		BodyAnimatorCommon.enabled = false;
		if (_E2B6.Config.UseBodyFastAnimator)
		{
			PlayerBones.PlayableAnimator.Stop();
		}
		ArmsAnimatorCommon.enabled = false;
		_ECC9.ReleaseBeginSample(_ED3E._E000(162915), _ED3E._E000(160724));
		_characterController.isEnabled = false;
		Skills?.Terminate();
		Physical.Unsubscribe();
		if (POM != null)
		{
			POM.Off();
		}
		if (HandsController != null)
		{
			HandsController.OnPlayerDead();
		}
		FastForwardCurrentOperations();
		_ECC9.ReleaseBeginSample(_ED3E._E000(162996), _ED3E._E000(160724));
		MovementContext.InteractionInfo.Callback?.Invoke();
		_healthController.DiedEvent -= OnDead;
		if (this.m__E02A)
		{
			this.m__E049[0].parent = PlayerBones.LeftPalm;
			if ((bool)this.m__E046)
			{
				this.m__E046.enabled = false;
			}
			this.m__E02A = false;
		}
		else
		{
			HandPosers[0].Lerp2Target(EFTHardSettings.Instance.LEFT_HAND_QTS, 5f);
		}
		_EB29.DestroyOverLimit(Equipment, _E0DE);
		Corpse = CreateCorpse();
		ApplyCorpseImpulse();
		if (Singleton<GameWorld>.Instantiated)
		{
			Singleton<GameWorld>.Instance.UnregisterPlayer(this);
		}
		if (_triggerColliderSearcher != null)
		{
			_triggerColliderSearcher.IsEnabled = false;
		}
		_ECC9.ReleaseBeginSample(_ED3E._E000(163026), _ED3E._E000(160724));
		if (MovementContext.StationaryWeapon != null)
		{
			MovementContext.StationaryWeapon.Unlock(ProfileId);
		}
		if (MovementContext.StationaryWeapon != null && MovementContext.StationaryWeapon.Item == _handsController.Item)
		{
			MovementContext.StationaryWeapon.Show();
			ReleaseHand();
		}
		else
		{
			Corpse.SetItemInHandsLootedCallback(ReleaseHand);
			StartCoroutine(_E047());
		}
	}

	private IEnumerator _E047()
	{
		yield return null;
		DropItemDead(_handsController.Item, _handsController.ControllerGameObject);
	}

	protected virtual Corpse CreateCorpse()
	{
		return CreateCorpse<Corpse>(Velocity);
	}

	protected T CreateCorpse<T>(Vector3 velocity) where T : Corpse
	{
		FirearmController firearmController = HandsController as FirearmController;
		_E333 containerCollectionView = null;
		if (firearmController != null)
		{
			containerCollectionView = firearmController.CCV;
		}
		return Corpse.CreateCorpse<T>(base.gameObject, Equipment, Profile.Customization, reinitBody: false, Singleton<GameWorld>.Instance, Side, velocity, PlayerBones.Pelvis.Original, _E0A6, containerCollectionView, _E0DE.CurrentId);
	}

	protected virtual void ApplyCorpseImpulse()
	{
		float hIT_FORCE = EFTHardSettings.Instance.HIT_FORCE;
		hIT_FORCE = (_corpseAppliedForce = hIT_FORCE * (0.3f + 0.7f * Mathf.InverseLerp(50f, 20f, LastDamageInfo.PenetrationPower)));
		Corpse.Ragdoll.ApplyImpulse(LastDamageInfo.HitCollider, LastDamageInfo.Direction, LastDamageInfo.HitPoint, hIT_FORCE);
	}

	public void MumbleToggle()
	{
		MonoBehaviourSingleton<GameUI>.Instance.GesturesQuickPanel.ToggleBindPanel(this);
	}

	public void MumbleEnd()
	{
		MonoBehaviourSingleton<GameUI>.Instance.GesturesQuickPanel.CloseBindPanel();
	}

	public void QuickMumbleStart()
	{
		if (!MonoBehaviourSingleton<GameUI>.Instance.GesturesQuickPanel.DropdownPanelActive && _E0AA)
		{
			EPhraseTrigger ePhraseTrigger = MonoBehaviourSingleton<GameUI>.Instance.GesturesQuickPanel.ActivateCommand();
			if (ePhraseTrigger != EPhraseTrigger.PhraseNone)
			{
				Say(ePhraseTrigger, demand: true);
			}
		}
	}

	public void ToggleMumbleDropdown()
	{
		_E048(!MonoBehaviourSingleton<GameUI>.Instance.GesturesQuickPanel.DropdownPanelActive);
	}

	public void CloseMumbleDropdown()
	{
		_E048(value: false);
	}

	private void _E048(bool value)
	{
		if (_E0A4 != null)
		{
			StopCoroutine(_E0A4);
			_E0A4 = null;
		}
		if (value)
		{
			_E0A4 = this.WaitSeconds(0.2f, delegate
			{
				_E0AA = false;
				MonoBehaviourSingleton<GameUI>.Instance.GesturesQuickPanel.ShowDropdown();
			});
			return;
		}
		if (MonoBehaviourSingleton<GameUI>.Instance.GesturesQuickPanel.DropdownPanelActive)
		{
			MonoBehaviourSingleton<GameUI>.Instance.GesturesQuickPanel.CloseDropdown(delegate(EPhraseTrigger x)
			{
				Say(x, demand: true);
			});
		}
		this.WaitSeconds(0.3f, delegate
		{
			_E0AA = true;
		});
	}

	public void ExitTriggerStatusChanged(bool status)
	{
		ExitTriggerZone = status;
		OnExitTriggerVisited.Invoke();
	}

	public virtual void SetExfiltrationPoint(ExfiltrationPoint point, bool entered)
	{
		_E0D3?.Invoke(point, entered);
		ExfiltrationPoint = (entered ? point : null);
		_E0AE?.Invoke();
	}

	public void SearchForInteractions()
	{
		_E0D1 = true;
		_E0AE?.Invoke();
	}

	public void MakeBindAction(int index, bool aggressive)
	{
		if (MonoBehaviourSingleton<GameUI>.Instance != null && MonoBehaviourSingleton<GameUI>.Instance.BattleUIGesturesMenu != null && MonoBehaviourSingleton<GameUI>.Instance.BattleUIGesturesMenu.Binds.TryGetValue((ECommand)index, out var value))
		{
			PlayPhraseOrGesture(value, aggressive);
		}
	}

	public void PlayPhraseOrGesture(int actionId, bool aggressive)
	{
		if (actionId > 9)
		{
			if (GesturesQuickPanel.IsPhraseAvailable((EPhraseTrigger)actionId))
			{
				Say((EPhraseTrigger)actionId, demand: true, 0f, (ETagStatus)0, 100, aggressive);
			}
			else
			{
				UnityEngine.Debug.Log(string.Concat(_ED3E._E000(163056), (EPhraseTrigger)actionId, _ED3E._E000(27308)));
			}
		}
		else
		{
			_E073((EGesture)actionId);
		}
	}

	internal virtual void _E073(EGesture gesture)
	{
		if (!HandsController.IsInInteractionStrictCheck())
		{
			HandsController.ShowGesture(gesture);
		}
	}

	protected virtual void TriggerPhraseCommand(EPhraseTrigger phrase, int netPhraseId)
	{
	}

	public void AddAlly(_E5B4 enemy)
	{
	}

	public void SetGroup(_E267 newBotsGroup)
	{
	}

	public virtual void KillMe(EBodyPart part, float damage)
	{
		if (HealthController.IsAlive)
		{
			_EC23 obj = default(_EC23);
			obj.DamageType = EDamageType.Sniper;
			obj.Damage = damage;
			obj.Direction = Transform.forward;
			obj.HitCollider = ((part != 0) ? null : _hitColliders.FirstOrDefault((BodyPartCollider partCollider) => partCollider.BodyPartColliderType == EBodyPartColliderType.Head)?.Collider);
			_EC23 damageInfo = obj;
			ApplyShot(damageInfo, part, _EC22.EMPTY_SHOT_ID);
		}
	}

	public virtual void DevelopResetDiscardLimits()
	{
		if (Profile.Info.MemberCategory.Is(EMemberCategory.Developer) && _E0DE is _EAEA obj)
		{
			obj.ResetDiscardLimits();
		}
	}

	public virtual void DevelopSetEncodedRadioTransmitter(bool value)
	{
	}

	public virtual void DevelopSetActiveLighthouseTraderZoneDebug(bool value)
	{
	}

	public virtual void GetRadioTransmitterStatusFromServer()
	{
	}

	public virtual void KillAIs()
	{
	}

	public virtual void SpawnAI(int count)
	{
	}

	public virtual void DevelopUnlockDoors(bool openDoors)
	{
		Door[] array = _E3AA.FindUnityObjectsOfType<Door>();
		foreach (Door door in array)
		{
			if (door.DoorState == EDoorState.Locked && !string.IsNullOrEmpty(door.KeyId) && !openDoors)
			{
				door.DoorState = EDoorState.Shut;
			}
			if ((door.DoorState == EDoorState.Locked || door.DoorState == EDoorState.Shut) && openDoors)
			{
				door.DoorState = EDoorState.Open;
			}
		}
	}

	public virtual void Heal(EBodyPart bodyPart, float value)
	{
		ActiveHealthController?.Heal(bodyPart, value);
	}

	public virtual void DebugSnapshotAllPlayers()
	{
	}

	public virtual void DebugSpawnAirdrop(bool spawnNearPlayer, Vector3 playerPosition)
	{
	}

	public void SetCallbackForInteraction(Action<Action> cb)
	{
		_openAction = cb;
	}

	public virtual void TryInteractionCallback(LootableContainer container)
	{
		if (container != null)
		{
			_openAction?.Invoke(delegate
			{
				container.Interact(new _EBFE(EInteractionType.Close));
				if (MovementContext.LevelOnApproachStart > 0f)
				{
					MovementContext.SetPoseLevel(MovementContext.LevelOnApproachStart);
					MovementContext.LevelOnApproachStart = -1f;
				}
			});
		}
		_openAction = null;
	}

	public void OnGameSessionBegin()
	{
		Pedometer.Start();
		StatisticsManager.BeginStatisticsSession();
	}

	public virtual void OnGameSessionEnd(ExitStatus exitStatus, float pastTime, string locationId, string exitName)
	{
		if (!_E0D5)
		{
			_E0D5 = true;
			Pedometer.Stop();
			_E0DD = 0;
			ExecuteSkill((Action)delegate
			{
				Skills.LowHPDuration.Complete();
			});
			ExecuteSkill((Action)delegate
			{
				Skills.OnlineAction.Complete((float)StatisticsManager.CurrentSessionLength.TotalHours);
			});
			Profile.Stats.LastPlayerState = null;
			StatisticsManager.EndStatisticsSession(exitStatus, pastTime);
			if (_questController != null)
			{
				_questController.CheckExitConditionCounters(exitStatus, pastTime, locationId, exitName, HealthController.BodyPartEffects, TriggerZones);
				_questController.ResetCurrentNullableCounters();
			}
			_E05A();
			if (MovementContext.Platform != null)
			{
				GetOff(MovementContext.Platform);
			}
			Skills.OnSkillLevelChanged -= OnSkillLevelChanged;
			Skills.OnSkillExperienceChanged -= OnSkillExperienceChanged;
			Skills.WeaponMastered -= OnWeaponMastered;
			Skills.OnMasteringExperienceChanged -= OnMasteringExperienceChanged;
			Skills.ImmunityAvoidPoisonChance.OnResult -= _E049;
			Skills.ImmunityAvoidMiscEffectsChance.OnResult -= _E049;
			StatisticsManager.OnUniqueLoot -= _E052;
			MovementContext.OnStateChanged -= _E058;
			HealthController.ApplyDamageEvent -= _E054;
			HealthController.EnergyChangedEvent -= _E057;
			HealthController.HydrationChangedEvent -= _E053;
			HealthController.EffectResidualEvent -= _E055;
			HealthController.StimulatorBuffActivationEvent -= _E04C;
			HealthController.TemperatureChangedEvent -= _E04D;
			_E0DE.OnItemFound -= _E04E;
			_E0DE.OnAmmoLoaded -= _E04F;
			_E0DE.OnAmmoUnloaded -= _E050;
			_E0DE.OnMagazineCheck -= _E051;
			_E0DE.SearchOperations.ItemRemoved -= SearchOperationsOnItemRemoved;
			HealthController.EffectStartedEvent -= OnHealthEffectAdded;
			HealthController.EffectResidualEvent -= OnHealthEffectRemoved;
			HealthController.HealthChangedEvent -= _E040;
			HealthController.BodyPartDestroyedEvent -= _E03F;
			HealthController.BodyPartRestoredEvent -= _E03E;
			Profile.OnTraderStandingChanged -= TraderStandingHandler;
			_E0A8?.Invoke();
			_E0A8 = null;
			UnsubscribeVisualEvents();
		}
	}

	protected virtual void ConnectSkillManager()
	{
		Skills.OnSkillLevelChanged += OnSkillLevelChanged;
		Skills.OnSkillExperienceChanged += OnSkillExperienceChanged;
		Skills.WeaponMastered += OnWeaponMastered;
		Skills.OnMasteringExperienceChanged += OnMasteringExperienceChanged;
		Skills.ImmunityAvoidPoisonChance.OnResult += _E049;
		Skills.ImmunityAvoidMiscEffectsChance.OnResult += _E049;
		StatisticsManager.OnUniqueLoot += _E052;
		MovementContext.OnStateChanged += _E058;
		HealthController.ApplyDamageEvent += _E054;
		HealthController.EnergyChangedEvent += _E057;
		HealthController.HydrationChangedEvent += _E053;
		HealthController.EffectResidualEvent += _E055;
		HealthController.StimulatorBuffActivationEvent += _E04C;
		HealthController.TemperatureChangedEvent += _E04D;
		_E0DE.OnItemFound += _E04E;
		_E0DE.OnAmmoLoaded += _E04F;
		_E0DE.OnAmmoUnloaded += _E050;
		_E0DE.OnMagazineCheck += _E051;
		_E0DE.SearchOperations.ItemRemoved += SearchOperationsOnItemRemoved;
		_E0A8 = _EBAF.Instance.SubscribeOnEvent(delegate(_EBB9 invokedEvent)
		{
			_E056(invokedEvent);
		});
	}

	private void _E049(bool result)
	{
		HealthController.AddImmunityNotificationEffect();
	}

	private void _E04A(_E992 obj)
	{
		if (obj is _E9A3)
		{
			Skills.Dehydration.Begin();
		}
		else if (obj is _E9A4)
		{
			Skills.Exhaustion.Begin();
		}
	}

	private void _E04B(_E992 obj)
	{
		if (obj is _E9A3)
		{
			ExecuteSkill((Action)delegate
			{
				Skills.Dehydration.Complete();
			});
		}
		else if (obj is _E9A4)
		{
			ExecuteSkill((Action)delegate
			{
				Skills.Exhaustion.Complete();
			});
		}
	}

	private void _E04C(_E986 buff)
	{
		if (!buff.Settings.BuffType.IsBuff(buff.Settings.Value))
		{
			_E0DD += (buff.Active ? 1 : (-1));
		}
		Skills.Endurance.OnLevelUp?.Invoke();
	}

	private void _E04D(float tempCelsio)
	{
		PlayerBody.SetTemperatureForBody(tempCelsio);
	}

	protected virtual void SearchOperationsOnItemRemoved(_EB94 obj)
	{
		IfItemFullySearched(obj._E016);
	}

	protected void IfItemFullySearched(_EA91 item)
	{
		if (item.GetSearchState(_E0DE.ID).Value == SearchedState.FullySearched)
		{
			ExecuteSkill((Action)delegate
			{
				Skills.SearchAction.Complete();
			});
		}
	}

	private void _E04E(Item item)
	{
		if (!_E0A9.Contains(item))
		{
			_E0A9.Add(item);
			IItemOwner owner = item.Parent.GetOwner();
			bool onCorpse = owner.RootItem is _EB0B;
			ExecuteSkill((Action)delegate
			{
				Skills.FindAction.Complete(onCorpse);
			});
		}
	}

	private void _E04F(int count)
	{
		ExecuteSkill((Action)delegate
		{
			Skills.RaidLoadedAmmoAction.Complete(count);
		});
	}

	private void _E050(int count)
	{
		ExecuteSkill((Action)delegate
		{
			Skills.RaidUnloadedAmmoAction.Complete(count);
		});
	}

	private void _E051()
	{
		ExecuteSkill((Action)delegate
		{
			Skills.MagazineCheckAction.Complete();
		});
	}

	private void _E052()
	{
		ExecuteSkill((Action)delegate
		{
			Skills.UniqueLoot.Complete();
		});
	}

	private void _E053(float diff)
	{
		ExecuteSkill((Action)delegate
		{
			Skills.HydrationChanged.Complete(diff, diff);
		});
	}

	private void _E054(EBodyPart bodyPart, float damage, _EC23 damageInfo)
	{
		if (!damageInfo.DamageType.IsSelfInflicted())
		{
			ExecuteSkill((Action)delegate
			{
				Skills.DamageTakenAction.Complete(damage);
			});
		}
	}

	private void _E055(_E992 healthEffect)
	{
		if (healthEffect is _E9BF obj && _E0DC != null)
		{
			_E0DC.CheckUseItemCounter(obj.MedItem.TemplateId, obj.Amount, Location, TriggerZones);
		}
	}

	private void _E056(_EBB9 flareEvent)
	{
		if (IsYourPlayer && _E0DC != null && flareEvent.PlayerProfileID == ProfileId && flareEvent.ZoneEventType == _EBB9.EZoneEventType.FiredPlayerAddedInShotList && flareEvent.FlareEventType == FlareEventType.Quest)
		{
			_E0DC.CheckShootFlareCounter(flareEvent.ZoneID);
		}
	}

	private void _E057(float diff)
	{
		ExecuteSkill((Action)delegate
		{
			Skills.EnergyChanged.Complete(diff, diff);
		});
	}

	public virtual void ExecuteSkill(Action action)
	{
		action();
	}

	public virtual void ExecuteSkill(Action<float> action)
	{
		action(1f);
	}

	public Task ManageGameQuests()
	{
		_questController?.Run();
		return Singleton<GameWorld>.Instance._E00E(this);
	}

	public void InitializeRecodableItemHandlers()
	{
		recodableItemsHandler = new _E639(this);
	}

	protected virtual void StartInflictSelfDamageCoroutine()
	{
		if (_E0A5 == null)
		{
			_E0A5 = StartCoroutine(InflictSelfDamage());
		}
	}

	public IEnumerator InflictSelfDamage()
	{
		while (_healthController.IsAlive)
		{
			_E099 -= Time.deltaTime;
			if (_E099 <= 0f && CurrentState.Name == EPlayerState.Sprint)
			{
				_E099 = UnityEngine.Random.Range(1f, 1.5f);
				if (MovementContext.PhysicalConditionIs(EPhysicalCondition.LeftLegDamaged))
				{
					ActiveHealthController.ApplyDamage(EBodyPart.LeftLeg, 2f, _E98A.FallDamage);
				}
				if (MovementContext.PhysicalConditionIs(EPhysicalCondition.RightLegDamaged))
				{
					ActiveHealthController.ApplyDamage(EBodyPart.RightLeg, 2f, _E98A.FallDamage);
				}
			}
			yield return null;
		}
	}

	private void _E058(EPlayerState previousState, EPlayerState nextState)
	{
		Pedometer.CurrentState = nextState;
		switch (nextState)
		{
		case EPlayerState.Sprint:
			MovementContext.CheckGroundedRayDistance = 0.15f;
			Pedometer.MakeMark(EPlayerState.Sprint);
			if (MovementContext.PhysicalConditionIs(EPhysicalCondition.LeftLegDamaged) || MovementContext.PhysicalConditionIs(EPhysicalCondition.RightLegDamaged))
			{
				StartInflictSelfDamageCoroutine();
			}
			break;
		case EPlayerState.Run:
			MovementContext.CheckGroundedRayDistance = 0.08f;
			Pedometer.MakeMark(EPlayerState.Run);
			break;
		case EPlayerState.Jump:
			MovementContext.CheckGroundedRayDistance = 0.03f;
			ActiveHealthController?.DoWoundRelapse(1f, EBodyPart.Common);
			break;
		case EPlayerState.ProneMove:
			MovementContext.CheckGroundedRayDistance = 0.08f;
			Pedometer.MakeMark(EPlayerState.ProneMove);
			break;
		}
		switch (previousState)
		{
		case EPlayerState.Sprint:
			if (MovementContext.IsGrounded)
			{
				float distance2 = Pedometer.GetDistanceFromMark(EPlayerState.Sprint);
				ExecuteSkill((Action)delegate
				{
					Skills.SprintAction.Complete(new _E74F._E001
					{
						Overweight = Physical.Overweight,
						Fatigue = (_E0A1?.Strength ?? 0f)
					}, distance2);
				});
				ActiveHealthController?.DoWoundRelapse(distance2 / 10f, EBodyPart.Common);
			}
			break;
		case EPlayerState.Run:
			if (MovementContext.IsGrounded)
			{
				float distance = Pedometer.GetDistanceFromMark(EPlayerState.Run);
				ExecuteSkill((Action)delegate
				{
					Skills.MovementAction.Complete(new _E74F._E001
					{
						Noise = MovementContext.CovertNoiseLevel,
						Overweight = Physical.Overweight,
						Fatigue = (_E0A1?.Strength ?? 0f)
					}, distance);
				});
			}
			break;
		case EPlayerState.ProneMove:
			if (MovementContext.IsGrounded)
			{
				ExecuteSkill((Action)delegate
				{
					Skills.ProneAction.Complete(Pedometer.GetDistanceFromMark(EPlayerState.ProneMove));
				});
			}
			break;
		case EPlayerState.Jump:
		{
			bool flag = false;
			if (MovementContext.PhysicalConditionIs(EPhysicalCondition.LeftLegDamaged))
			{
				ActiveHealthController?.ApplyDamage(EBodyPart.LeftLeg, 3f, _E98A.FallDamage);
				flag = true;
			}
			if (MovementContext.PhysicalConditionIs(EPhysicalCondition.RightLegDamaged))
			{
				ActiveHealthController?.ApplyDamage(EBodyPart.RightLeg, 3f, _E98A.FallDamage);
				flag = true;
			}
			if (flag && !MovementContext.PhysicalConditionIs(EPhysicalCondition.OnPainkillers) && !IsAI)
			{
				Say(EPhraseTrigger.OnBeingHurt, demand: true);
			}
			break;
		}
		}
	}

	protected virtual void OnSkillLevelChanged(_E74E skill)
	{
	}

	protected virtual void OnSkillExperienceChanged(_E74E skill)
	{
	}

	protected virtual void OnWeaponMastered(_E750 masterSkill)
	{
	}

	protected virtual void OnMasteringExperienceChanged(_E750 masterSkill)
	{
	}

	public void SpecialPlaceVisited(string id, int experience)
	{
		_E0C6?.Invoke(id, experience);
	}

	private IEnumerator _E059()
	{
		yield return new WaitForSeconds(2f);
		_E0A2 = (from x in GetComponentsInChildren<Renderer>(includeInactive: true)
			where x.enabled
			select x).ToArray();
	}

	public void SwitchRenderer(bool @switch)
	{
		Renderer[] array = _E0A2;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = @switch;
		}
	}

	public virtual void Teleport(Vector3 position, bool onServerToo = false)
	{
		MovementContext.TransformPosition = position;
		_E00F();
		this.m__E014 = 0f;
		MovementContext.ResetFlying();
		if ((bool)EnvironmentManager.Instance)
		{
			EnvironmentManager.Instance.UpdateEnvironmentForPlayer(this);
		}
	}

	public void IncreaseAwareness(float aware = 5f)
	{
		Awareness = Mathf.Max(Awareness, Time.time + aware);
	}

	public virtual void Sleep(bool value)
	{
	}

	public bool HasBodyPartCollider(Collider collider)
	{
		return PlayerBones.BodyPartCollidersHashSet.Contains(collider);
	}

	public virtual void Dispose()
	{
		_questController?.Dispose();
		_questController = null;
		if (Singleton<GameWorld>.Instantiated)
		{
			GameWorld instance = Singleton<GameWorld>.Instance;
			instance.UnregisterPlayer(this);
			instance.RemovePlayerObject(this);
			if (instance.SpeakerManager != null)
			{
				instance.SpeakerManager.RemoveFromGroup(this);
			}
		}
		_E084?.Invoke();
		_E084 = null;
		if (_playerBody != null && Corpse == null)
		{
			_playerBody.Dispose();
			_playerBody = null;
		}
		this.m__E02B = false;
		Physical?.Unsubscribe();
		if (HandsController != null)
		{
			AbstractHandsController handsController = HandsController;
			_E05A();
			UnityEngine.Object.Destroy(handsController);
		}
		MovementContext?.Dispose();
		if (ExfiltrationPoint != null)
		{
			ExfiltrationPoint.Entered.Remove(this);
		}
		if (MovementContext != null && MovementContext.Platform != null)
		{
			GetOff(MovementContext.Platform);
		}
		_E098?.Invoke(this);
		CompositeDisposable.Dispose();
	}

	private void _E05A()
	{
		if (!(_handsController == null))
		{
			_handsController.Destroy();
			_handsController = null;
		}
	}

	public void Board(MovingPlatform platform)
	{
		if (MovementContext.Platform != null && MovementContext.Platform != platform)
		{
			GetOff(MovementContext.Platform);
		}
		if (!platform.Passengers.Contains(this))
		{
			platform.Passengers.Add(this);
		}
		MovementContext.Platform = platform;
	}

	public void GetOff(MovingPlatform platform)
	{
		if (!(MovementContext.Platform != platform))
		{
			platform.Passengers.Remove(this);
			MovementContext.Platform = null;
		}
	}

	public void HandleFlareSuccessEvent(Vector3 position, FlareEventType eventType)
	{
		Singleton<_E307>.Instance.SuccessFlare(this, position, eventType);
	}

	[CompilerGenerated]
	private void _E05B(int i)
	{
		ProceduralWeaponAnimation.Pose = i;
	}

	[CompilerGenerated]
	private int _E05C()
	{
		return (int)Vector3.SignedAngle(Singleton<LevelSettings>.Instance.NorthVector, Quaternion.Euler(0f, MovementContext.Yaw, 0f) * Vector3.forward, Vector3.up);
	}

	[CompilerGenerated]
	private void _E05D(bool enable)
	{
		if (_E083 != null)
		{
			_E083.Source.mute = !enable;
		}
	}

	[CompilerGenerated]
	private void _E05E()
	{
		NestedStepSoundSource.Release();
		NestedStepSoundSource = null;
	}

	[CompilerGenerated]
	private EmptyHandsController _E05F()
	{
		return EmptyHandsController._E000<EmptyHandsController>(this);
	}

	[CompilerGenerated]
	private void _E060()
	{
		Skills.LowHPDuration.Complete();
	}

	[CompilerGenerated]
	private void _E061()
	{
		Skills.StimulatorNegativeBuff.Begin();
	}

	[CompilerGenerated]
	private void _E062()
	{
		Skills.StimulatorNegativeBuff.Complete();
	}

	[CompilerGenerated]
	private void _E063()
	{
		_E0AA = false;
		MonoBehaviourSingleton<GameUI>.Instance.GesturesQuickPanel.ShowDropdown();
	}

	[CompilerGenerated]
	private void _E064(EPhraseTrigger x)
	{
		Say(x, demand: true);
	}

	[CompilerGenerated]
	private void _E065()
	{
		_E0AA = true;
	}

	[CompilerGenerated]
	private void _E066()
	{
		Skills.LowHPDuration.Complete();
	}

	[CompilerGenerated]
	private void _E067()
	{
		Skills.OnlineAction.Complete((float)StatisticsManager.CurrentSessionLength.TotalHours);
	}

	[CompilerGenerated]
	private void _E068(_EBB9 invokedEvent)
	{
		_E056(invokedEvent);
	}

	[CompilerGenerated]
	private void _E069()
	{
		Skills.Dehydration.Complete();
	}

	[CompilerGenerated]
	private void _E06A()
	{
		Skills.Exhaustion.Complete();
	}

	[CompilerGenerated]
	private void _E06B()
	{
		Skills.SearchAction.Complete();
	}

	[CompilerGenerated]
	private void _E06C()
	{
		Skills.MagazineCheckAction.Complete();
	}

	[CompilerGenerated]
	private void _E06D()
	{
		Skills.UniqueLoot.Complete();
	}

	[CompilerGenerated]
	private void _E06E()
	{
		Skills.ProneAction.Complete(Pedometer.GetDistanceFromMark(EPlayerState.ProneMove));
	}
}
