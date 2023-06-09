using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BitPacking;
using Comfort.Common;
using ComponentAce.Compression.Libs.zlib;
using Dissonance;
using Dissonance.Networking.Client;
using Diz.Utils;
using EFT.Ballistics;
using EFT.Communications;
using EFT.Interactive;
using EFT.InventoryLogic;
using EFT.NetworkPackets;
using EFT.Quests;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

namespace EFT;

public class ClientPlayer : NetworkPlayer
{
	public new interface _E000
	{
		ulong BytesWritten { get; }

		event _E001._E000 DataSentEvent;

		void Send(ref _E6C2 client2ServerPacket, float deltaTime, float time, float lastServerTime);

		void Dispatch();
	}

	public new sealed class _E001 : _E000
	{
		public delegate void _E000(ClientPlayer player, int bytes, ulong totalBytes);

		private readonly ClientPlayer m__E001;

		private readonly _E004 _E002;

		private readonly _E528 _E003;

		private _E6C2 _E004 = _E6C2.DEFAULT_CLIENT2_SERVER_PACKET;

		private readonly _E37C<_E6C2> _E005 = new _E37C<_E6C2>(127);

		private ulong _E006;

		private readonly float _E007;

		private float _E008;

		private float _E009;

		private int _E00A;

		[CompilerGenerated]
		private _E000 _E00B;

		public bool PreventDispatch;

		private bool _E00C;

		ulong ClientPlayer._E000.BytesWritten => _E006;

		private event _E000 _E000
		{
			[CompilerGenerated]
			add
			{
				_E000 obj = _E00B;
				_E000 obj2;
				do
				{
					obj2 = obj;
					_E000 value2 = (_E000)Delegate.Combine(obj2, value);
					obj = Interlocked.CompareExchange(ref _E00B, value2, obj2);
				}
				while ((object)obj != obj2);
			}
			[CompilerGenerated]
			remove
			{
				_E000 obj = _E00B;
				_E000 obj2;
				do
				{
					obj2 = obj;
					_E000 value2 = (_E000)Delegate.Remove(obj2, value);
					obj = Interlocked.CompareExchange(ref _E00B, value2, obj2);
				}
				while ((object)obj != obj2);
			}
		}

		event _E000 ClientPlayer._E000.DataSentEvent
		{
			add
			{
				this._E000 += value;
			}
			remove
			{
				this._E000 -= value;
			}
		}

		internal _E001(ClientPlayer player, _E004 sendDelegate, int sendRateFramesLimit)
		{
			m__E001 = player;
			_E002 = sendDelegate;
			_E003 = new _E51A(new byte[10000]);
			if (sendRateFramesLimit <= 10)
			{
				sendRateFramesLimit = 120;
			}
			_E007 = 1f / (float)sendRateFramesLimit;
		}

		void ClientPlayer._E000.Send(ref _E6C2 client2ServerPacket, float deltaTime, float time, float lastServerTime)
		{
			bool flag = (client2ServerPacket.HasCriticalData = client2ServerPacket.HasCriticalData || _E00A <= 0 || client2ServerPacket.HasImportantData);
			client2ServerPacket.TempDeltaTime = deltaTime;
			client2ServerPacket.LastServerTime = lastServerTime;
			client2ServerPacket.ClientTime = time;
			if (flag)
			{
				_E00A = 5;
			}
			else
			{
				_E00A--;
			}
			_E005.Enqueue(client2ServerPacket);
			_E00C |= flag;
		}

		void ClientPlayer._E000.Dispatch()
		{
			if (_E005.Size == 0 || (PreventDispatch && _E005.Size <= _E005.ReservedSize))
			{
				return;
			}
			float time = Time.time;
			if (!_E00C && time < _E008)
			{
				return;
			}
			_E008 = time + _E007;
			_E00C = false;
			_E003.Reset();
			int size = _E005.Size;
			_E003.WriteLimitedInt32(size, 0, 127, BitPackingTag.IClientPlayerDataSenderPacketsCount);
			bool flag = false;
			for (int i = 0; i < size; i++)
			{
				_E6C2 current = _E005[i];
				flag |= current.HasCriticalData;
				current.DeltaTimeFromLastCriticalPacket = _E009 + current.TempDeltaTime;
				_E6C2.SerializeDiffUsing(_E003, ref current, _E004);
				_E009 = current.DeltaTimeFromLastCriticalPacket;
				if (_E004.FrameId >= current.FrameId)
				{
					UnityEngine.Debug.LogErrorFormat(_ED3E._E000(193883), _E004.FrameId, current.FrameId);
				}
				if (current.HasCriticalData)
				{
					_E009 = 0f;
					_E004 = current;
				}
			}
			_E003.Flush();
			_E005.Clear();
			byte[] buffer = _E003.Buffer;
			int bytesWritten = _E003.BytesWritten;
			if (bytesWritten > 1200)
			{
				_E36E.LogErrorFormat(_ED3E._E000(193907), bytesWritten, size, _E068.LastValues(), _E068.AVGStats());
			}
			ArraySegment<byte> segment = new ArraySegment<byte>(buffer, 0, bytesWritten);
			PacketPriority priority = (flag ? PacketPriority.Critical : PacketPriority.Low);
			_E002(m__E001.ChannelIndex, in segment, priority);
			_E006 += (ulong)bytesWritten;
			_E00B?.Invoke(m__E001, bytesWritten, _E006);
		}
	}

	internal new sealed class _E002 : PlayerOwnerInventoryController
	{
		[CompilerGenerated]
		private new sealed class _E000
		{
			public int finishedInventoryHash;

			public EOperationStatus? serverOperationStatus;

			public EOperationStatus? localOperationStatus;

			public _EB73 operation;

			public Callback callback;

			public ClientPlayer._E002 _003C_003E4__this;

			internal void _E000(Result<int, bool, EOperationStatus> result)
			{
				_E001 CS_0024_003C_003E8__locals0 = new _E001
				{
					CS_0024_003C_003E8__locals1 = this,
					result = result
				};
				if (CS_0024_003C_003E8__locals0.result.Succeed)
				{
					switch (CS_0024_003C_003E8__locals0.result.Value2)
					{
					case EOperationStatus.Finished:
						finishedInventoryHash = CS_0024_003C_003E8__locals0.result.Value0;
						serverOperationStatus = EOperationStatus.Finished;
						if (localOperationStatus == serverOperationStatus)
						{
							operation.Dispose();
							callback.Succeed();
						}
						break;
					case EOperationStatus.Started:
						localOperationStatus = EOperationStatus.Started;
						serverOperationStatus = EOperationStatus.Started;
						_003C_003E4__this._E000(CS_0024_003C_003E8__locals0.result.Value0, CS_0024_003C_003E8__locals0.result.Value1, operation);
						operation._E02A(delegate(IResult executeResult)
						{
							if (!executeResult.Succeed)
							{
								CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this.Logger.LogError(_ED3E._E000(194137), CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this.ID, CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.operation.Id, CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.operation, executeResult.Error);
							}
							CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.localOperationStatus = EOperationStatus.Finished;
							if (CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.serverOperationStatus == CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.localOperationStatus)
							{
								CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._E000(CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.finishedInventoryHash, CS_0024_003C_003E8__locals0.result.Value1, CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.operation);
								CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.operation.Dispose();
								CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.callback.Invoke(CS_0024_003C_003E8__locals0.result);
							}
							else if (CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.serverOperationStatus.HasValue && CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.serverOperationStatus == EOperationStatus.Failed)
							{
								CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.operation.Dispose();
								CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.callback.Invoke(CS_0024_003C_003E8__locals0.result);
							}
						}, requiresExternalFinalization: true);
						break;
					}
				}
				else
				{
					_003C_003E4__this.Logger.LogError(_ED3E._E000(194079), _003C_003E4__this.ID, operation.Id, operation, CS_0024_003C_003E8__locals0.result.Error);
					serverOperationStatus = EOperationStatus.Failed;
					if (localOperationStatus != EOperationStatus.Started)
					{
						operation.Dispose();
						callback.Invoke(CS_0024_003C_003E8__locals0.result);
					}
				}
			}
		}

		[CompilerGenerated]
		private new sealed class _E001
		{
			public Result<int, bool, EOperationStatus> result;

			public _E000 CS_0024_003C_003E8__locals1;

			internal void _E000(IResult executeResult)
			{
				if (!executeResult.Succeed)
				{
					CS_0024_003C_003E8__locals1._003C_003E4__this.Logger.LogError(_ED3E._E000(194137), CS_0024_003C_003E8__locals1._003C_003E4__this.ID, CS_0024_003C_003E8__locals1.operation.Id, CS_0024_003C_003E8__locals1.operation, executeResult.Error);
				}
				CS_0024_003C_003E8__locals1.localOperationStatus = EOperationStatus.Finished;
				if (CS_0024_003C_003E8__locals1.serverOperationStatus == CS_0024_003C_003E8__locals1.localOperationStatus)
				{
					CS_0024_003C_003E8__locals1._003C_003E4__this._E000(CS_0024_003C_003E8__locals1.finishedInventoryHash, result.Value1, CS_0024_003C_003E8__locals1.operation);
					CS_0024_003C_003E8__locals1.operation.Dispose();
					CS_0024_003C_003E8__locals1.callback.Invoke(result);
				}
				else if (CS_0024_003C_003E8__locals1.serverOperationStatus.HasValue && CS_0024_003C_003E8__locals1.serverOperationStatus == EOperationStatus.Failed)
				{
					CS_0024_003C_003E8__locals1.operation.Dispose();
					CS_0024_003C_003E8__locals1.callback.Invoke(result);
				}
			}
		}

		[CompilerGenerated]
		private new sealed class _E002
		{
			public _EB94 operation;

			public Callback callback;

			internal void _E000(Result<int, bool, EOperationStatus> result)
			{
				operation.Dispose();
				if (result.Succeed)
				{
					callback.Succeed();
				}
				else
				{
					callback.Invoke(result);
				}
			}
		}

		private new ClientPlayer _E000 => (ClientPlayer)base._E000;

		internal _E002(ClientPlayer player, Profile profile, MongoID firstId, bool examined = false)
			: base(player, profile, examined)
		{
			_E027 = firstId;
		}

		private new void _E000(int inventoryHashSum, bool badBeforeExecuting, _EB73 operation)
		{
		}

		private uint _E001<_E077>(_E077 operation, Callback<int, bool, EOperationStatus> action) where _E077 : _EB73
		{
			ushort id = operation.Id;
			this._E000.OperationCallbacks.Add(id, action);
			return id;
		}

		public override void ExecuteStationaryOperation(StationaryWeapon stationaryWeapon, Callback callback = null)
		{
			this._E000.LastStationaryWeaponOperationId = _E026;
			base.ExecuteStationaryOperation(stationaryWeapon, callback);
		}

		internal override void Execute(_EB73 operation, Callback callback)
		{
			if (callback == null)
			{
				callback = delegate
				{
				};
			}
			EOperationStatus? localOperationStatus = null;
			if (!_E01E(operation))
			{
				operation.Dispose();
				callback.Fail(_ED3E._E000(193959));
				return;
			}
			int finishedInventoryHash = 0;
			EOperationStatus? serverOperationStatus;
			uint callbackId = _E001(operation, delegate(Result<int, bool, EOperationStatus> result)
			{
				if (result.Succeed)
				{
					switch (result.Value2)
					{
					case EOperationStatus.Finished:
						finishedInventoryHash = result.Value0;
						serverOperationStatus = EOperationStatus.Finished;
						if (localOperationStatus == serverOperationStatus)
						{
							operation.Dispose();
							callback.Succeed();
						}
						break;
					case EOperationStatus.Started:
						localOperationStatus = EOperationStatus.Started;
						serverOperationStatus = EOperationStatus.Started;
						_E000(result.Value0, result.Value1, operation);
						operation._E02A(delegate(IResult executeResult)
						{
							if (!executeResult.Succeed)
							{
								Logger.LogError(_ED3E._E000(194137), base.ID, operation.Id, operation, executeResult.Error);
							}
							localOperationStatus = EOperationStatus.Finished;
							if (serverOperationStatus == localOperationStatus)
							{
								_E000(finishedInventoryHash, result.Value1, operation);
								operation.Dispose();
								callback.Invoke(result);
							}
							else if (serverOperationStatus.HasValue && serverOperationStatus == EOperationStatus.Failed)
							{
								operation.Dispose();
								callback.Invoke(result);
							}
						}, requiresExternalFinalization: true);
						break;
					}
				}
				else
				{
					Logger.LogError(_ED3E._E000(194079), base.ID, operation.Id, operation, result.Error);
					serverOperationStatus = EOperationStatus.Failed;
					if (localOperationStatus != EOperationStatus.Started)
					{
						operation.Dispose();
						callback.Invoke(result);
					}
				}
			});
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter writer = new BinaryWriter(memoryStream);
			writer.WritePolymorph(_E69E.FromInventoryOperation(operation));
			this._E000._E0DE.AddInventoryCommand(new _E6F7
			{
				CallbackId = callbackId,
				CommandBytes = memoryStream.ToArray(),
				InventoryHashSum = base.Inventory.CreateInventoryHashSum()
			});
		}

		protected override void NotifyMagazineChecked(string name)
		{
			_E857.DisplayMessageNotification(_ED3E._E000(194028).Localized() + name.Localized());
		}

		protected override void Execute(_EB94 operation, Callback callback)
		{
			uint callbackId = _E001(operation, delegate(Result<int, bool, EOperationStatus> result)
			{
				operation.Dispose();
				if (result.Succeed)
				{
					callback.Succeed();
				}
				else
				{
					callback.Invoke(result);
				}
			});
			this._E000._E0DE.StartSearchContentPacket = new _E6AD
			{
				ItemId = operation._E016.Id,
				CallbackId = callbackId
			};
		}

		protected override void ExecuteStop(_EB94 operation)
		{
			this._E000._E0DE.StopSearchContentPacket = new _E6AE
			{
				ItemId = operation._E016.Id
			};
		}
	}

	private new sealed class _E003 : _E7B3, IDisposable
	{
		internal readonly struct _E000
		{
			[CompilerGenerated]
			private readonly DateTime m__E000;

			[CompilerGenerated]
			private readonly DateTime _E001;

			internal DateTime _E002
			{
				[CompilerGenerated]
				get
				{
					return this.m__E000;
				}
			}

			internal DateTime _E003
			{
				[CompilerGenerated]
				get
				{
					return _E001;
				}
			}

			internal bool _E004 => _E005 <= TimeSpan.Zero;

			internal TimeSpan _E005
			{
				get
				{
					TimeSpan timeSpan = _E002 - _E5AD.UtcNow;
					if (!(timeSpan > TimeSpan.Zero))
					{
						return TimeSpan.Zero;
					}
					return timeSpan;
				}
			}

			internal _E000(in DateTime dateTime, in DateTime created)
			{
				_E001 = created;
				this.m__E000 = dateTime;
			}

			internal _E000(in DateTime dateTime)
			{
				_E001 = _E5AD.UtcNow;
				this.m__E000 = dateTime;
			}

			internal _E000(in TimeSpan timeSpan)
			{
				_E001 = _E5AD.UtcNow;
				this.m__E000 = _E003 + timeSpan;
			}

			internal void _E000()
			{
			}
		}

		internal sealed class _E001
		{
			private struct _E000
			{
				public DateTime From;

				public DateTime To;

				public TimeSpan Duration => To - From;
			}

			private TimeSpan m__E000;

			private List<_E000> m__E001;

			private byte m__E002;

			private TimeSpan m__E003;

			internal static _E001 _E000(byte activationsLimit, float activationsInterval, float speakingInterval)
			{
				int capacity = Math.Max(10, (int)activationsLimit);
				return new _E001
				{
					_E000 = TimeSpan.FromSeconds(activationsInterval),
					_E001 = ((activationsLimit > 0) ? new List<_E000>(capacity) : null),
					_E002 = activationsLimit,
					_E003 = TimeSpan.FromSeconds(Math.Max(activationsInterval, speakingInterval))
				};
			}

			internal void _E001(DateTime from, DateTime to)
			{
				if (this.m__E001 != null)
				{
					DateTime dateTime = _E5AD.UtcNow - this.m__E003;
					int num = this.m__E001.Count - 1;
					while (num >= 0 && !(this.m__E001[num].To >= dateTime))
					{
						this.m__E001.RemoveAt(num);
						num--;
					}
					this.m__E001.Insert(0, new _E000
					{
						From = from,
						To = to
					});
				}
			}

			internal TimeSpan _E002(DateTime dateTime)
			{
				if (this.m__E001 == null)
				{
					return TimeSpan.Zero;
				}
				TimeSpan zero = TimeSpan.Zero;
				for (int i = 0; i < this.m__E001.Count; i++)
				{
					_E000 obj = this.m__E001[i];
					if (obj.To <= dateTime)
					{
						break;
					}
					if (obj.From <= dateTime)
					{
						zero += obj.To - dateTime;
						break;
					}
					zero += obj.Duration;
				}
				return zero;
			}

			internal bool _E003()
			{
				if (this.m__E001 == null)
				{
					return true;
				}
				if (this.m__E001.Count < this.m__E002)
				{
					return true;
				}
				DateTime dateTime = _E5AD.UtcNow - this.m__E000;
				DateTime from = this.m__E001[this.m__E002 - 1].From;
				return dateTime >= from;
			}
		}

		private abstract class _E002
		{
			[CompilerGenerated]
			private ClientPlayer._E003 _E000;

			internal abstract EVoipControllerStatus _E005 { get; }

			internal ClientPlayer._E003 _E001
			{
				[CompilerGenerated]
				get
				{
					return _E000;
				}
				[CompilerGenerated]
				set
				{
					_E000 = value;
				}
			}

			internal _E001 _E002 => _E001.m__E002;

			internal bool _E003 => this == _E001?.m__E009;

			protected DissonanceComms _E004 => _E001._E015;

			internal abstract TimeSpan _E006 { get; }

			internal virtual EVoipControllerStatus _E01B()
			{
				return _E005;
			}

			internal virtual EVoipControllerStatus _E01D()
			{
				return _E005;
			}

			internal virtual EVoipControllerStatus _E01F()
			{
				return _E005;
			}

			internal virtual void _E020()
			{
			}

			internal virtual void _E021()
			{
				_E001._E003(_E001.m__E008);
			}

			internal virtual void Update()
			{
			}
		}

		private abstract class _E003 : _E002
		{
			internal override EVoipControllerStatus _E01B()
			{
				base._E004.IsMuted = true;
				return base._E01B();
			}
		}

		private sealed class _E004 : _E003
		{
			internal override EVoipControllerStatus _E005 => EVoipControllerStatus.Off;

			internal override TimeSpan _E006 => TimeSpan.Zero;

			internal override EVoipControllerStatus _E01B()
			{
				return base._E01B();
			}

			internal override void _E020()
			{
				base._E001.TalkDetected.Value = false;
				if (base._E001.m__E007._E000)
				{
					base._E001._E003(base._E001.m__E003);
				}
				else
				{
					base._E001._E003(base._E001.m__E007);
				}
			}

			internal override void Update()
			{
				base._E001._E00C();
			}
		}

		private sealed class _E005 : _E003
		{
			internal override EVoipControllerStatus _E005 => EVoipControllerStatus.Ready;

			internal override TimeSpan _E006 => TimeSpan.Zero;

			internal override EVoipControllerStatus _E01B()
			{
				base._E001.TalkDetected.Value = false;
				if ((bool)base._E001.HasInteraction)
				{
					base._E001._E002();
				}
				return base._E01B();
			}

			internal override EVoipControllerStatus _E01D()
			{
				if (!base._E002._E003())
				{
					return _E000();
				}
				return base._E001._E003(base._E001.m__E004);
			}

			private EVoipControllerStatus _E000()
			{
				return base._E001._E003(base._E001.m__E006);
			}
		}

		private sealed class _E006 : _E002
		{
			private new _E000 _E001;

			internal override EVoipControllerStatus _E005 => EVoipControllerStatus.Talking;

			internal override TimeSpan _E006 => _E001._E005;

			internal override EVoipControllerStatus _E01B()
			{
				_E001 = base._E001._E008();
				if (_E001._E004)
				{
					UnityEngine.Debug.LogError(string.Format(_ED3E._E000(194514), _E5AD.UtcNow.TimeOfDay, _E001._E002.TimeOfDay, _E001._E003.TimeOfDay));
				}
				if (_E001._E004)
				{
					return base._E001._E003(base._E001.m__E005);
				}
				base._E004.IsMuted = false;
				base._E001._E00E = null;
				base._E001.HasInteraction.Value = true;
				return base._E01B();
			}

			internal override EVoipControllerStatus _E01D()
			{
				return _E01F();
			}

			internal override EVoipControllerStatus _E01F()
			{
				base._E002._E001(_E001._E003, _E5AD.UtcNow);
				return base._E001._E003(base._E001.m__E003);
			}

			internal override void Update()
			{
				if (_E001._E004)
				{
					_E000();
				}
			}

			private void _E000()
			{
				DateTime utcNow = _E5AD.UtcNow;
				base._E002._E001(_E001._E003, utcNow);
				base._E001._E003(base._E001.m__E005);
			}
		}

		private abstract class _E007 : _E003
		{
			protected new _E000 _E002;

			internal override TimeSpan _E006 => _E002._E005;

			internal override void Update()
			{
				if (_E002._E004)
				{
					base._E001._E003(base._E001.m__E003);
				}
			}
		}

		private sealed class _E008 : _E007
		{
			internal override EVoipControllerStatus _E005 => EVoipControllerStatus.Limited;

			internal override EVoipControllerStatus _E01B()
			{
				_E002 = base._E001._E009();
				return base._E01B();
			}
		}

		private sealed class _E009 : _E007
		{
			internal override EVoipControllerStatus _E005 => EVoipControllerStatus.Blocked;

			internal override EVoipControllerStatus _E01B()
			{
				_E002 = base._E001._E00B();
				base._E001._E00E = _E002._E002;
				base._E001.HasInteraction.Value = true;
				return base._E01B();
			}
		}

		private sealed class _E00A : _E007
		{
			internal override EVoipControllerStatus _E005 => EVoipControllerStatus.Banned;

			internal bool _E000 => _E002._E004;

			internal override TimeSpan _E006 => _E002._E005;

			internal static _E00A _E000(ClientPlayer._E003 controller)
			{
				return new _E00A
				{
					_E002 = controller._E00A()
				};
			}

			internal override EVoipControllerStatus _E01B()
			{
				_E002 = base._E001._E00A();
				base._E001._E002();
				base._E001.HasInteraction.Value = true;
				return base._E01B();
			}
		}

		private sealed class _E00B : _E007
		{
			internal override EVoipControllerStatus _E005 => EVoipControllerStatus.MicrophoneFail;

			internal override TimeSpan _E006 => _E002._E005;

			internal override EVoipControllerStatus _E01B()
			{
				return _E005;
			}

			internal override void Update()
			{
				base._E001._E00C();
			}
		}

		private ClientPlayer m__E000;

		private readonly _E3A4 m__E001 = new _E3A4(2);

		private readonly _E001 m__E002;

		private readonly _E005 m__E003;

		private readonly _E006 m__E004;

		private readonly _E008 m__E005;

		private readonly _E009 m__E006;

		private readonly _E00A m__E007;

		private readonly _E004 m__E008;

		private _E002 m__E009;

		[CompilerGenerated]
		private readonly TimeSpan m__E00A;

		[CompilerGenerated]
		private readonly TimeSpan m__E00B;

		[CompilerGenerated]
		private readonly TimeSpan m__E00C;

		[CompilerGenerated]
		private readonly TimeSpan _E00D;

		private DateTime? _E00E;

		private bool _E00F = true;

		private bool _E010;

		[CompilerGenerated]
		private readonly _ECF5<EVoipControllerStatus> _E011;

		[CompilerGenerated]
		private readonly _ECF5<bool> _E012;

		[CompilerGenerated]
		private readonly _ECF5<bool> _E013;

		[CompilerGenerated]
		private Action<string> _E014;

		private DissonanceComms _E015 => this.m__E000.DissonanceComms;

		private TimeSpan _E016
		{
			[CompilerGenerated]
			get
			{
				return this.m__E00A;
			}
		}

		private TimeSpan _E017
		{
			[CompilerGenerated]
			get
			{
				return this.m__E00B;
			}
		}

		private TimeSpan _E018
		{
			[CompilerGenerated]
			get
			{
				return this.m__E00C;
			}
		}

		private TimeSpan _E019
		{
			[CompilerGenerated]
			get
			{
				return _E00D;
			}
		}

		public _ECF5<EVoipControllerStatus> Status
		{
			[CompilerGenerated]
			get
			{
				return _E011;
			}
		}

		public _ECF5<bool> HasInteraction
		{
			[CompilerGenerated]
			get
			{
				return _E012;
			}
		}

		public _ECF5<bool> TalkDetected
		{
			[CompilerGenerated]
			get
			{
				return _E013;
			}
		}

		TimeSpan _E7B3.TimeToNextStatus => this.m__E009._E006;

		public event Action<string> AbuseNotification
		{
			[CompilerGenerated]
			add
			{
				Action<string> action = _E014;
				Action<string> action2;
				do
				{
					action2 = action;
					Action<string> value2 = (Action<string>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref _E014, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<string> action = _E014;
				Action<string> action2;
				do
				{
					action2 = action;
					Action<string> value2 = (Action<string>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref _E014, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		internal _E003([NotNull] ClientPlayer clientPlayer, [NotNull] _E7E0 soundSettings)
		{
			EVoipState voipState = clientPlayer.VoipState;
			this.m__E000 = clientPlayer;
			_E75F pushToTalkSettings = this.m__E000._E0F1.PushToTalkSettings;
			this.m__E00A = TimeSpan.FromSeconds(pushToTalkSettings.BlockingTime);
			this.m__E00B = TimeSpan.FromSeconds(pushToTalkSettings.SpeakingSecondsInterval);
			this.m__E00C = TimeSpan.FromSeconds(pushToTalkSettings.SpeakingSecondsLimit);
			_E00D = _E017 - _E018;
			if (_E015 == null)
			{
				this.m__E009 = new _E00B();
			}
			else
			{
				this.m__E002 = ClientPlayer._E003._E001._E000(pushToTalkSettings.ActivationsLimit, pushToTalkSettings.ActivationsInterval, pushToTalkSettings.SpeakingSecondsInterval);
				this.m__E008 = new _E004();
				this.m__E003 = new _E005();
				this.m__E004 = new _E006();
				this.m__E005 = new _E008();
				this.m__E006 = new _E009();
				this.m__E007 = ClientPlayer._E003._E00A._E000(this);
				switch (voipState)
				{
				case EVoipState.Off:
					this.m__E009 = this.m__E008;
					break;
				case EVoipState.Available:
					this.m__E009 = this.m__E003;
					break;
				case EVoipState.Banned:
					this.m__E009 = this.m__E007;
					break;
				default:
					UnityEngine.Debug.LogError(string.Format(_ED3E._E000(194200), voipState));
					this.m__E009 = this.m__E008;
					break;
				}
			}
			_E011 = new _ECF5<EVoipControllerStatus>(this.m__E009._E005, _E3A5<EVoipControllerStatus>.EqualityComparer);
			_E012 = new _ECF5<bool>();
			_E013 = new _ECF5<bool>();
			this.m__E009._E001 = this;
			this.m__E009._E01B();
			if (this.m__E009._E005 == EVoipControllerStatus.MicrophoneFail)
			{
				return;
			}
			try
			{
				this.m__E001.BindState(soundSettings.VoipEnabled, _E005);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(_ED3E._E000(194228));
				UnityEngine.Debug.LogException(exception);
			}
			try
			{
				this.m__E001.BindState(soundSettings.VoipDevice, _E006);
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogError(_ED3E._E000(194248));
				UnityEngine.Debug.LogException(exception2);
			}
		}

		public void Dispose()
		{
			this.m__E001.Dispose();
			ClientPlayer clientPlayer = Interlocked.Exchange(ref this.m__E000, null);
			if (clientPlayer != null)
			{
				clientPlayer.VoipController = null;
			}
		}

		internal void _E000(string reporterId)
		{
			_E014?.Invoke(reporterId);
		}

		internal void Update()
		{
			if (!_E010)
			{
				this.m__E009.Update();
				_E001();
			}
		}

		private void _E001()
		{
			if (_E00E.HasValue && !(_E00E.Value > _E5AD.UtcNow))
			{
				_E00E = null;
				HasInteraction.Value = false;
			}
		}

		private void _E002()
		{
			_E00E = _E5AD.UtcNow;
		}

		EVoipControllerStatus _E7B3.ToggleTalk()
		{
			return this.m__E009._E01D();
		}

		EVoipControllerStatus _E7B3.StopTalk()
		{
			return this.m__E009._E01F();
		}

		void _E7B3.ReportAbuse()
		{
			ClientHearingTable.Instance?.ReportAbuse();
		}

		public void ForceMuteVoIP(bool enable)
		{
			_E010 = enable;
			if (_E010)
			{
				if (this.m__E009._E005 != EVoipControllerStatus.MicrophoneFail)
				{
					this.m__E009._E021();
				}
			}
			else if (_E00F && this.m__E009._E005 != EVoipControllerStatus.MicrophoneFail)
			{
				this.m__E009._E020();
			}
		}

		private EVoipControllerStatus _E003(_E002 state)
		{
			if (this.m__E009 == state)
			{
				return state._E005;
			}
			state._E001 = this;
			_E002 obj = Interlocked.Exchange(ref this.m__E009, state);
			if (obj != null)
			{
				obj._E001 = null;
			}
			_E004(state._E005);
			Status.Value = state._E005;
			if (this.m__E009 != state)
			{
				UnityEngine.Debug.LogError(_ED3E._E000(194275));
			}
			if (!state._E003)
			{
				return this.m__E009._E005;
			}
			return state._E01B();
		}

		private void _E004(EVoipControllerStatus status)
		{
			EVoipState voipState = this.m__E000.VoipState;
			switch (status)
			{
			case EVoipControllerStatus.Off:
				voipState = EVoipState.Off;
				break;
			case EVoipControllerStatus.Banned:
				voipState = EVoipState.Banned;
				break;
			case EVoipControllerStatus.MicrophoneFail:
				voipState = EVoipState.MicrophoneFail;
				break;
			default:
				if (voipState == EVoipState.Available)
				{
					return;
				}
				voipState = EVoipState.Available;
				break;
			}
			this.m__E000.VoipState = voipState;
		}

		private void _E005(bool enabled)
		{
			_E00F = enabled;
			if (!_E010)
			{
				if (enabled)
				{
					this.m__E009._E020();
				}
				else
				{
					this.m__E009._E021();
				}
			}
		}

		private void _E006(string device)
		{
			if (!(device == _ED3E._E000(194336)))
			{
				if (device == _ED3E._E000(30808))
				{
					_E007();
				}
				else if (_E7E0.IsValidMicrophone(device))
				{
					UnityEngine.Debug.Log(_ED3E._E000(194374) + device);
					_E015.MicrophoneName = device;
				}
				else
				{
					UnityEngine.Debug.LogError(_ED3E._E000(194409) + device + _ED3E._E000(194441));
					_E007();
				}
			}
			else
			{
				_E005(enabled: false);
			}
		}

		private void _E007()
		{
			string defaultMicrophone = _E7E0.DefaultMicrophone;
			if (defaultMicrophone != null)
			{
				UnityEngine.Debug.Log(_ED3E._E000(194478) + defaultMicrophone);
				_E015.MicrophoneName = defaultMicrophone;
			}
			else
			{
				_E005(enabled: false);
			}
		}

		private _E000 _E008()
		{
			int num = 0;
			DateTime created = _E5AD.UtcNow;
			DateTime dateTime = created;
			DateTime dateTime2 = dateTime;
			TimeSpan zero = TimeSpan.Zero;
			do
			{
				num++;
				DateTime dateTime3 = dateTime - _E017;
				TimeSpan timeSpan = this.m__E002._E002(dateTime3);
				TimeSpan timeSpan2 = _E018 - timeSpan;
				if (timeSpan2.TotalSeconds < 0.1)
				{
					break;
				}
				timeSpan2 -= zero;
				if (timeSpan2 <= TimeSpan.Zero)
				{
					break;
				}
				dateTime2 += timeSpan2;
				if (dateTime2 - created >= _E018)
				{
					dateTime2 = created + _E018;
					break;
				}
				zero += timeSpan2;
				dateTime = dateTime2;
			}
			while (num < 20);
			_E000 result = new _E000(in dateTime2, in created);
			result._E000();
			return result;
		}

		private _E000 _E009()
		{
			TimeSpan timeSpan = _E019;
			_E000 result = new _E000(in timeSpan);
			result._E000();
			return result;
		}

		private _E000 _E00A()
		{
			_E72E ban = this.m__E000.Profile.Info.GetBan(EBanType.Voip);
			if (ban == null)
			{
				return default(_E000);
			}
			DateTime dateTime = ban.BanUntil;
			_E000 result = new _E000(in dateTime);
			result._E000();
			return result;
		}

		private _E000 _E00B()
		{
			TimeSpan timeSpan = _E016;
			_E000 result = new _E000(in timeSpan);
			result._E000();
			return result;
		}

		private void _E00C()
		{
			TimeSpan timeSpan = _E5AD.UtcNow - this.m__E000.HearingDateTime;
			bool flag = timeSpan <= Player.HearingDetectionTime;
			TalkDetected.Value = flag;
			if (flag)
			{
				TimeSpan timeSpan2 = Player.HearingDetectionTime - timeSpan;
				_E00E = _E5AD.UtcNow + timeSpan2;
				HasInteraction.Value = true;
			}
		}
	}

	public new delegate void _E004(byte channelIndex, in ArraySegment<byte> segment, PacketPriority priority);

	private new struct _E005
	{
		public string calledOnSceneName;

		public int cutseneID;
	}

	[CompilerGenerated]
	private new sealed class _E006
	{
		public Callback callback;

		internal void _E000(bool succeed)
		{
			if (succeed)
			{
				callback.Succeed();
			}
			else
			{
				callback.Fail(_ED3E._E000(192542));
			}
		}
	}

	[CompilerGenerated]
	private new sealed class _E007
	{
		public ClientPlayer _003C_003E4__this;

		public bool withNetwork;

		public Process<EmptyHandsController, _E6C9> process;

		public Action confirmCallback;

		internal EmptyHandsController _E000()
		{
			return ClientEmptyHandsController._E000(_003C_003E4__this);
		}

		internal void _E001()
		{
			if (withNetwork)
			{
				uint callbackId = _003C_003E4__this._E001(process._E001);
				_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateEmptyHands;
				_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			}
		}

		internal void _E002(IResult result)
		{
			ClientPlayer._E000(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E008
	{
		public ClientPlayer _003C_003E4__this;

		public Weapon weapon;

		public Process<FirearmController, _E6CB> process;

		public Action confirmCallback;

		internal FirearmController _E000()
		{
			return ClientFirearmController._E000(_003C_003E4__this, weapon);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E001(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateFirearm;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = weapon.Id;
		}

		internal void _E002(IResult result)
		{
			ClientPlayer._E000(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E009
	{
		public ClientPlayer _003C_003E4__this;

		public _EADF throwWeap;

		public Process<GrenadeController, _E6CC> process;

		public Action confirmCallback;

		internal GrenadeController _E000()
		{
			return ClientGrenadeController._E000(_003C_003E4__this, throwWeap);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E001(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateGrenade;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = throwWeap.Id;
		}

		internal void _E002(IResult result)
		{
			ClientPlayer._E000(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E00A
	{
		public ClientPlayer _003C_003E4__this;

		public _EA72 meds;

		public EBodyPart bodyPart;

		public int animationVariant;

		public Process<MedsController, _E6CF> process;

		public Action confirmCallback;

		internal MedsController _E000()
		{
			return ClientMedsController._E000(_003C_003E4__this, meds, bodyPart, 1f, animationVariant);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E001(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateMeds;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = meds.Id;
			_003C_003E4__this._handsChangePacket.MedsBodyPart = bodyPart;
			_003C_003E4__this._handsChangePacket.MedsAmount = 1f;
			_003C_003E4__this._handsChangePacket.AnimationVariant = animationVariant;
		}

		internal void _E002(IResult result)
		{
			ClientPlayer._E000(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E00B
	{
		public ClientPlayer _003C_003E4__this;

		public _EA48 foodDrink;

		public float amount;

		public int animationVariant;

		public Process<MedsController, _E6CF> process;

		public Action confirmCallback;

		internal MedsController _E000()
		{
			return ClientMedsController._E000(_003C_003E4__this, foodDrink, EBodyPart.Head, amount, animationVariant);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E001(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateMeds;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = foodDrink.Id;
			_003C_003E4__this._handsChangePacket.MedsBodyPart = EBodyPart.Head;
			_003C_003E4__this._handsChangePacket.MedsAmount = amount;
			_003C_003E4__this._handsChangePacket.AnimationVariant = animationVariant;
		}

		internal void _E002(IResult result)
		{
			ClientPlayer._E000(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E00C
	{
		public ClientPlayer _003C_003E4__this;

		public KnifeComponent knife;

		public Process<KnifeController, _E6CD> process;

		public Action confirmCallback;

		internal KnifeController _E000()
		{
			return ClientKnifeController._E000(_003C_003E4__this, knife);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E001(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateKnife;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = knife.Item.Id;
		}

		internal void _E002(IResult result)
		{
			ClientPlayer._E000(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E00D<_E077> where _E077 : UsableItemController
	{
		public ClientPlayer _003C_003E4__this;

		public Item item;

		public Process<_E077, _E6CE> process;

		public Action confirmCallback;

		internal _E077 _E000()
		{
			return UsableItemController._E000<_E077>(_003C_003E4__this, item);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E001(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateUsableItem;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = item.Id;
		}

		internal void _E002(IResult result)
		{
			ClientPlayer._E000(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E00E
	{
		public ClientPlayer _003C_003E4__this;

		public _EADF throwWeap;

		public Process<QuickGrenadeThrowController, _E6D2> process;

		public Action confirmCallback;

		internal QuickGrenadeThrowController _E000()
		{
			return ClientQuickGrenadeThrowController._E000(_003C_003E4__this, throwWeap);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E001(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateQuickGrenadeThrow;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = throwWeap.Id;
		}

		internal void _E002(IResult result)
		{
			ClientPlayer._E000(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E00F
	{
		public ClientPlayer _003C_003E4__this;

		public KnifeComponent knife;

		public Process<QuickKnifeKickController, _E6D3> process;

		public Action confirmCallback;

		internal QuickKnifeKickController _E000()
		{
			return ClientQuickKnifeKickController._E000(_003C_003E4__this, knife);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E001(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateQuickKnifeKick;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = knife.Item.Id;
		}

		internal void _E002(IResult result)
		{
			ClientPlayer._E000(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E010
	{
		public ClientPlayer _003C_003E4__this;

		public Item item;

		public Process<QuickUseItemController, _E6D4> process;

		public Action confirmCallback;

		internal QuickUseItemController _E000()
		{
			return QuickUseItemController._E000<QuickUseItemController>(_003C_003E4__this, item);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E001(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateQuickUseItem;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = item.Id;
		}

		internal void _E002(IResult result)
		{
			ClientPlayer._E000(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E011
	{
		public uint id;

		public Action<bool> confirmAction;

		internal void _E000(Result<int, bool> result)
		{
			UnityEngine.Debug.Log(_ED3E._E000(192533) + id + ((!string.IsNullOrEmpty(result.Error)) ? (_ED3E._E000(192568) + result.Error) : ""));
			confirmAction(result.Succeed);
		}
	}

	[CompilerGenerated]
	private new sealed class _E012
	{
		public ClientPlayer player;

		internal void _E000(_EBC2 tpEvent)
		{
			player._E004(tpEvent);
		}

		internal void _E001(_EBB5 tpEvent)
		{
			player._E005(tpEvent);
		}
	}

	[CompilerGenerated]
	private sealed class _E013
	{
		public byte[] searchInfoSerializationBytes;

		public byte[] profileZip;

		public Profile profile;

		public byte[] inventoryZip;

		internal void _E000()
		{
			_E5D6 obj = _E5D5.Deserialize(new _E518(searchInfoSerializationBytes));
			string json = SimpleZlib.Decompress(profileZip);
			profile = json.ParseJsonTo<Profile>(Array.Empty<JsonConverter>());
			using (MemoryStream input = new MemoryStream(SimpleZlib.DecompressToBytes(inventoryZip)))
			{
				using BinaryReader reader = new BinaryReader(input);
				profile.Inventory = _E672.DeserializeInventory(Singleton<_E63B>.Instance, reader.ReadEFTInventoryDescriptor());
			}
			_EA91[] items = profile.Inventory.NonQuestItems.OfType<_EA91>().ToArray();
			_E5D5.FillSearchInfo(obj.Data, items);
		}
	}

	[CompilerGenerated]
	private sealed class _E016
	{
		public ClientPlayer _003C_003E4__this;

		public LootableContainer container;

		internal void _E000()
		{
			_003C_003E4__this._E0DB.Id = container.Id;
			_003C_003E4__this._E0DB.EInteractionType = EInteractionType.Close;
			_003C_003E4__this._E0DB.Execute = EInteractionStage.Execute;
			_003C_003E4__this._E0DB.HasInteraction = true;
			container.Interact(new _EBFE(EInteractionType.Close));
			if (_003C_003E4__this.MovementContext.LevelOnApproachStart > 0f)
			{
				_003C_003E4__this.MovementContext.SetPoseLevel(_003C_003E4__this.MovementContext.LevelOnApproachStart);
				_003C_003E4__this.MovementContext.LevelOnApproachStart = -1f;
			}
		}
	}

	protected _E6E5 _handsChangePacket;

	private _E6DA _E0DB;

	protected _E6DE _lootInteractionPacket;

	private new _E6DB _E0DC;

	private _E6DC _E0DD;

	private new _E6C2 _E0DE;

	private _E6C2 _E0DF;

	private bool _E0E0;

	private bool _E0E1;

	private const int _E0E2 = 20;

	private static readonly TimeSpan _E0E3 = TimeSpan.FromSeconds(1.0);

	[CompilerGenerated]
	private DateTime _E0E4;

	private const float _E0E5 = 2.17f;

	private byte _E0E6;

	private static Queue<Tuple<ushort, DateTime>> _E0E7;

	private float _E0E8;

	private float _E0E9;

	[CompilerGenerated]
	private float _E0EA = 0.3f;

	protected _E000 _dataSender;

	private readonly Dictionary<uint, Callback<int, bool>> _E0EB = new Dictionary<uint, Callback<int, bool>>();

	private uint _E0EC;

	public int ScavExfilMask;

	private _E6C3 _E0ED;

	private long _E0EE;

	private long _E0EF;

	private _E386 _E0F0 = new _E386(8);

	private _E75D _E0F1;

	private _E003 _E0F2;

	private ClientGameWorld _E0F3;

	private Action _E0F4;

	private bool _E0F5;

	private bool _E0F6;

	private _E005 _E0F7;

	private _E7AF _E0F8;

	public bool TranslateNetCommands;

	private Vector3 _E0F9;

	private _E6D9 _E0FA;

	[CompilerGenerated]
	private bool _E0FB;

	[CompilerGenerated]
	private Action<bool> _E0FC;

	[CompilerGenerated]
	private Action<_E634> _E0FD;

	private readonly _E37C<ClientShot> _E0FE = new _E37C<ClientShot>(30);

	private _E795 _E0FF;

	private bool _E100;

	private bool _E101;

	private float _E102;

	private new bool _E000 => _E5AD.UtcNow - TalkDateTime <= _E0E3;

	public DateTime TalkDateTime
	{
		[CompilerGenerated]
		get
		{
			return _E0E4;
		}
		[CompilerGenerated]
		set
		{
			_E0E4 = value;
		}
	}

	public override byte ChannelIndex => _E0E6;

	public float RTTDelay
	{
		[CompilerGenerated]
		get
		{
			return _E0EA;
		}
		[CompilerGenerated]
		set
		{
			_E0EA = value;
		}
	}

	public static bool RTTEnabled
	{
		get
		{
			return _E0E7 != null;
		}
		set
		{
			if (value)
			{
				if (_E0E7 == null)
				{
					_E0E7 = new Queue<Tuple<ushort, DateTime>>();
				}
			}
			else
			{
				_E0E7 = null;
			}
		}
	}

	public long UnprocessedCriticalPackets => _E0EE - _E0EF;

	private new float _E001 => _E0F0.Sum;

	public int UnprocessedCriticalPacketsTimeMs => (int)(this._E001 * 1000f);

	public override bool IsVisible
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public bool IsWaitingForNetworkCallback
	{
		get
		{
			if (_E0EB.Count <= 0)
			{
				return OperationCallbacks.Count > 0;
			}
			return true;
		}
	}

	public override AbstractHandsController HandsController
	{
		get
		{
			return base.HandsController;
		}
		protected set
		{
			base.HandsController = value;
			_E712.EWeaponAnimationType weaponAnimationType = GetWeaponAnimationType(_handsController);
			base.MovementContext.PlayerAnimatorSetWeaponId(weaponAnimationType);
		}
	}

	public bool IsBot
	{
		[CompilerGenerated]
		get
		{
			return _E0FB;
		}
		[CompilerGenerated]
		set
		{
			_E0FB = value;
		}
	}

	public override bool OnHisWayToOperateStationaryWeapon => OperationCallbacks.ContainsKey(LastStationaryWeaponOperationId);

	public override Vector3 LocalShotDirection => base.ProceduralWeaponAnimation.ShotDirection;

	protected override float Distance => 0f;

	public event Action<bool> SetActiveLighthouseTraderZoneDebug
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = _E0FC;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0FC, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = _E0FC;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0FC, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<_E634> OnSyncLighthouseTraderZoneData
	{
		[CompilerGenerated]
		add
		{
			Action<_E634> action = _E0FD;
			Action<_E634> action2;
			do
			{
				action2 = action;
				Action<_E634> value2 = (Action<_E634>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0FD, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_E634> action = _E0FD;
			Action<_E634> action2;
			do
			{
				action2 = action;
				Action<_E634> value2 = (Action<_E634>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0FD, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event _E001._E000 OnPacketSent
	{
		add
		{
			_dataSender.DataSentEvent += value;
		}
		remove
		{
			_dataSender.DataSentEvent -= value;
		}
	}

	public override void Interact(IItemOwner loot, Callback callback)
	{
		string id = loot.RootItem.Id;
		uint callbackId = _E001(delegate(bool succeed)
		{
			if (succeed)
			{
				callback.Succeed();
			}
			else
			{
				callback.Fail(_ED3E._E000(192542));
			}
		});
		_lootInteractionPacket.Interact = true;
		_lootInteractionPacket.LootId = id;
		_lootInteractionPacket.CallbackId = callbackId;
	}

	protected override void DropCurrentController(Action callback, bool fastDrop, Item nextControllerItem = null)
	{
		_handsChangePacket.OperationType = ((!fastDrop) ? _E6E5.EOperationType.Drop : _E6E5.EOperationType.FastDrop);
		_handsChangePacket.ItemId = nextControllerItem?.Id;
		base.DropCurrentController(callback, fastDrop, nextControllerItem);
	}

	protected override void Proceed(bool withNetwork, Callback<_E6C9> callback, bool scheduled = true)
	{
		Func<EmptyHandsController> controllerFactory = () => ClientEmptyHandsController._E000(this);
		Process<EmptyHandsController, _E6C9> process = new Process<EmptyHandsController, _E6C9>(this, controllerFactory, null);
		Action confirmCallback = delegate
		{
			if (withNetwork)
			{
				uint callbackId = _E001(process._E001);
				_handsChangePacket.OperationType = _E6E5.EOperationType.CreateEmptyHands;
				_handsChangePacket.CallbackId = callbackId;
			}
		};
		process._E000(delegate(IResult result)
		{
			_E000(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(Weapon weapon, Callback<_E6CB> callback, bool scheduled = true)
	{
		Func<FirearmController> controllerFactory = () => ClientFirearmController._E000(this, weapon);
		Process<FirearmController, _E6CB> process = new Process<FirearmController, _E6CB>(this, controllerFactory, weapon, fastHide: false, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Unknown);
		Action confirmCallback = delegate
		{
			uint callbackId = _E001(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateFirearm;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = weapon.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E000(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(_EADF throwWeap, Callback<_E6CC> callback, bool scheduled = true)
	{
		Func<GrenadeController> controllerFactory = () => ClientGrenadeController._E000(this, throwWeap);
		Process<GrenadeController, _E6CC> process = new Process<GrenadeController, _E6CC>(this, controllerFactory, throwWeap, fastHide: false, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Unknown);
		Action confirmCallback = delegate
		{
			uint callbackId = _E001(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateGrenade;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = throwWeap.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E000(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(_EA72 meds, EBodyPart bodyPart, Callback<_E6CF> callback, int animationVariant, bool scheduled = true)
	{
		Func<MedsController> controllerFactory = () => ClientMedsController._E000(this, meds, bodyPart, 1f, animationVariant);
		Process<MedsController, _E6CF> process = new Process<MedsController, _E6CF>(this, controllerFactory, meds, fastHide: false, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Unknown);
		Action confirmCallback = delegate
		{
			uint callbackId = _E001(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateMeds;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = meds.Id;
			_handsChangePacket.MedsBodyPart = bodyPart;
			_handsChangePacket.MedsAmount = 1f;
			_handsChangePacket.AnimationVariant = animationVariant;
		};
		process._E000(delegate(IResult result)
		{
			_E000(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(_EA48 foodDrink, float amount, Callback<_E6CF> callback, int animationVariant, bool scheduled = true)
	{
		Func<MedsController> controllerFactory = () => ClientMedsController._E000(this, foodDrink, EBodyPart.Head, amount, animationVariant);
		Process<MedsController, _E6CF> process = new Process<MedsController, _E6CF>(this, controllerFactory, foodDrink, fastHide: false, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Unknown);
		Action confirmCallback = delegate
		{
			uint callbackId = _E001(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateMeds;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = foodDrink.Id;
			_handsChangePacket.MedsBodyPart = EBodyPart.Head;
			_handsChangePacket.MedsAmount = amount;
			_handsChangePacket.AnimationVariant = animationVariant;
		};
		process._E000(delegate(IResult result)
		{
			_E000(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(KnifeComponent knife, Callback<_E6CD> callback, bool scheduled = true)
	{
		Func<KnifeController> controllerFactory = () => ClientKnifeController._E000(this, knife);
		Process<KnifeController, _E6CD> process = new Process<KnifeController, _E6CD>(this, controllerFactory, knife.Item, fastHide: false, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Unknown);
		Action confirmCallback = delegate
		{
			uint callbackId = _E001(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateKnife;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = knife.Item.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E000(result, confirmCallback);
		}, callback, scheduled);
	}

	internal override void Proceed<T>(Item item, Callback<_E6CE> callback, bool scheduled = true)
	{
		Func<T> controllerFactory = () => UsableItemController._E000<T>(this, item);
		Process<T, _E6CE> process = new Process<T, _E6CE>(this, controllerFactory, item, fastHide: false, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Unknown);
		Action confirmCallback = delegate
		{
			uint callbackId = _E001(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateUsableItem;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = item.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E000(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(_EADF throwWeap, Callback<_E6D2> callback, bool scheduled = true)
	{
		Func<QuickGrenadeThrowController> controllerFactory = () => ClientQuickGrenadeThrowController._E000(this, throwWeap);
		Process<QuickGrenadeThrowController, _E6D2> process = new Process<QuickGrenadeThrowController, _E6D2>(this, controllerFactory, throwWeap, fastHide: false, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Unknown, skippable: false);
		Action confirmCallback = delegate
		{
			uint callbackId = _E001(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateQuickGrenadeThrow;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = throwWeap.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E000(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(KnifeComponent knife, Callback<_E6D3> callback, bool scheduled = true)
	{
		Func<QuickKnifeKickController> controllerFactory = () => ClientQuickKnifeKickController._E000(this, knife);
		Process<QuickKnifeKickController, _E6D3> process = new Process<QuickKnifeKickController, _E6D3>(this, controllerFactory, knife.Item, fastHide: true, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Unknown, skippable: false);
		Action confirmCallback = delegate
		{
			uint callbackId = _E001(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateQuickKnifeKick;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = knife.Item.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E000(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(Item item, Callback<_E6D4> callback, bool scheduled = true)
	{
		Func<QuickUseItemController> controllerFactory = () => QuickUseItemController._E000<QuickUseItemController>(this, item);
		Process<QuickUseItemController, _E6D4> process = new Process<QuickUseItemController, _E6D4>(this, controllerFactory, item, fastHide: true, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Succeed, skippable: false);
		Action confirmCallback = delegate
		{
			uint callbackId = _E001(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateQuickUseItem;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = item.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E000(result, confirmCallback);
		}, callback, scheduled);
	}

	private static void _E000(IResult result, Action confirmAction)
	{
		if (result.Succeed)
		{
			confirmAction();
		}
	}

	private uint _E001(Action<bool> confirmAction)
	{
		uint id = _E003();
		UnityEngine.Debug.Log(_ED3E._E000(182965) + id);
		_E0EB[id] = delegate(Result<int, bool> result)
		{
			UnityEngine.Debug.Log(_ED3E._E000(192533) + id + ((!string.IsNullOrEmpty(result.Error)) ? (_ED3E._E000(192568) + result.Error) : ""));
			confirmAction(result.Succeed);
		};
		return id;
	}

	protected override void ProcessStopSearching(string itemId)
	{
		UnityEngine.Debug.Log(_ED3E._E000(183004));
		((_E002)((Player)this)._E0DE).StopSearching(itemId);
	}

	protected override void InitVoip(EVoipState voipState)
	{
		if (_E0F1.VoipEnabled && voipState != 0)
		{
			_E7E0 settings = Singleton<_E7DE>.Instance.Sound.Settings;
			if (!settings.VoipEnabled)
			{
				voipState = EVoipState.Off;
			}
			if (!_E0F1.MicrophoneChecked)
			{
				voipState = EVoipState.MicrophoneFail;
			}
			base.InitVoip(voipState);
			_E0F2 = new _E003(this, settings);
			base.VoipController = _E0F2;
		}
	}

	public void VoipAbuseNotification(string reporterId)
	{
		_E0F2?._E000(reporterId);
	}

	internal static ClientPlayer _E002(int playerId, Vector3 position, _E62D frameIndexer, _E004 sendDelegate, int sendRateFramesLimit, EUpdateQueue updateQueue, EUpdateMode armsUpdateMode, EUpdateMode bodyUpdateMode, CharacterControllerSpawner.Mode characterControllerMode, _E795 session, _E75D voipSettings)
	{
		ClientPlayer player = NetworkPlayer._E000<ClientPlayer>(_E5D2.PLAYER_BUNDLE_NAME, playerId, position, _ED3E._E000(183038), frameIndexer, updateQueue, armsUpdateMode, bodyUpdateMode, characterControllerMode, () => Singleton<_E7DE>.Instance.Control.Settings.MouseSensitivity, () => Singleton<_E7DE>.Instance.Control.Settings.MouseAimingSensitivity, isThirdPerson: false);
		player.IsYourPlayer = true;
		player._dataSender = new _E001(player, sendDelegate, sendRateFramesLimit);
		player._fbbik.solver.Quick = true;
		player._E0FF = session;
		player._E0F9 = position;
		player._E0F1 = voipSettings;
		player._E0F3 = (ClientGameWorld)Singleton<GameWorld>.Instance;
		player._E0F4 = _EBAF.Instance.SubscribeOnEvent(delegate(_EBC2 tpEvent)
		{
			player._E004(tpEvent);
		});
		ClientPlayer clientPlayer = player;
		clientPlayer._E0F4 = (Action)Delegate.Combine(clientPlayer._E0F4, _EBAF.Instance.SubscribeOnEvent(delegate(_EBB5 tpEvent)
		{
			player._E005(tpEvent);
		}));
		return player;
	}

	public _E000 GetDataSender()
	{
		return _dataSender;
	}

	private uint _E003()
	{
		uint num;
		do
		{
			num = ++_E0EC;
		}
		while (_E0EB.ContainsKey(num));
		return num;
	}

	public override void FixedUpdateTick()
	{
		if (_E0ED == null)
		{
			base.FixedUpdateTick();
		}
	}

	protected override void ComplexLateUpdate(EUpdateQueue queue, float deltaTime)
	{
		base.ComplexLateUpdate(queue, deltaTime);
		if (TranslateNetCommands && base.UpdateQueue == queue)
		{
			_E008();
			_dataSender.Dispatch();
		}
	}

	protected override void LateUpdate()
	{
		if (_E0ED == null)
		{
			base.LateUpdate();
		}
	}

	protected override void CreateMovementContext()
	{
		LayerMask mOVEMENT_MASK = EFTHardSettings.Instance.MOVEMENT_MASK;
		_E0F8 = _E7AF.Create(this, base.GetBodyAnimatorCommon, base.GetCharacterControllerCommon, mOVEMENT_MASK);
		base.MovementContext = _E0F8;
		_E0F8.OnCharacterControllerSpeedLimitChanged += _E01E;
	}

	public override void KillMe(EBodyPart part, float damage)
	{
		base.KillMe(part, damage);
		_E0DE.DevelopKillMePacket = new _E6BA
		{
			Part = part,
			Damage = damage
		};
	}

	public override void DevelopResetDiscardLimits()
	{
		base.DevelopResetDiscardLimits();
		_E0DE.DevelopResetDiscardLimits = default(_E6BB);
	}

	public override void DevelopSetEncodedRadioTransmitter(bool value)
	{
		_E0DE.DevelopSetEncodedRadioTransmitter = new _E6BE
		{
			IsEncoded = value
		};
	}

	public override void GetRadioTransmitterStatusFromServer()
	{
		(Singleton<AbstractGame>.Instance as _E7A6).GetRadioTransmitterData(base.Profile.Id);
	}

	public override void DevelopSetActiveLighthouseTraderZoneDebug(bool value)
	{
		_E0DE.DevelopSetActiveLighthouseTraderZoneDebug = new _E6BF
		{
			Active = value
		};
	}

	public override void KillAIs()
	{
		_E0DE.DevelopKillAllAIs = default(_E6B5);
	}

	public override void SpawnAI(int count)
	{
		_E0DE.DevelopSpawnAI = new _E6B6
		{
			Count = count
		};
	}

	public override void DevelopUnlockDoors(bool open)
	{
		_E0DE.DevelopUnlockAllDoors = new _E6B7
		{
			OpenDoors = open
		};
	}

	public override void PauseAllEffectsOnPlayer()
	{
		base.NetworkHealthController.PauseAllEffects();
	}

	public override void UnpauseAllEffectsOnPlayer()
	{
		base.NetworkHealthController.UnpauseAllEffects();
	}

	protected override void TriggerPhraseCommand(EPhraseTrigger phrase, int netPhraseId)
	{
		_E0DE.SetPhraseCommand(phrase, netPhraseId);
		base.TriggerPhraseCommand(phrase, netPhraseId);
	}

	protected override void OnPhraseTold(EPhraseTrigger @event, TaggedClip clip, TagBank bank, _E76C speaker)
	{
		_E0DE.SetPhraseCommand(@event, clip.NetId);
		base.OnPhraseTold(@event, clip, bank, speaker);
	}

	protected override void ManageAggressor(_EC23 damageInfo, EBodyPart bodyPart, EHeadSegment? headSegment)
	{
	}

	public override void ShowStringNotification(string message)
	{
		_E857.DisplayMessageNotification(message, ENotificationDurationType.Infinite);
		UnityEngine.Debug.Log(_ED3E._E000(183030) + message);
	}

	private void _E004(_EBC2 endCutsceneEvent)
	{
		_E0F5 = false;
		_E0F6 = true;
		_E0F7.calledOnSceneName = endCutsceneEvent.SceneName;
		_E0F7.cutseneID = endCutsceneEvent.CutsceneID;
	}

	private void _E005(_EBB5 cutsceneEvent)
	{
		_E0F5 = true;
		_E0DE.CutsceneEndedPacket = new _E6B0
		{
			CutsceneSceneNameHash = 0,
			CutsceneID = 0,
			IsStarted = true
		};
	}

	[Conditional("DEBUG")]
	[Conditional("CONSOLE")]
	private void _E006()
	{
		float speedLimit = _characterController.SpeedLimit;
		if (speedLimit >= 0f && !_ECC9.enabled && !base.MovementContext.IsOnPlatformWithCooldown && _characterController.IsSpeedLimitWasEnabledAtTheFrame && !_E0F5 && !_E0F6)
		{
			float num = _E717.CalculateDistance(_E0F9, base.MovementContext.TransformPosition);
			float num2 = _E717.CalculateSpeed(num, LastDeltaTime);
			if ((double)num2 > (double)speedLimit + 0.01)
			{
				UnityEngine.Debug.LogError(string.Format(_ED3E._E000(183015), _E0DE.FrameId, _E0DE.HasCriticalData, num2, speedLimit, num, LastDeltaTime, base.MovementContext.CurrentState.Name, base.Skills.Strength.Level, Physical.WalkSpeedLimit, _E0F9.ToString(_ED3E._E000(183251)), base.MovementContext.TransformPosition.ToString(_ED3E._E000(183251)), _E0DE.IsExtraPrecisionMovement));
			}
		}
		_E0F9 = base.MovementContext.TransformPosition;
	}

	private void _E007()
	{
		if (_E0E0 && !_E0DF.HasCriticalData)
		{
			_E6C2 obj = _E0DE;
			_E0DE = default(_E6C2);
			_E0DE.HasCriticalData = true;
			_E0DE.MovementInfoPacket = _E0DF.MovementInfoPacket;
			_E0DE.MovementInfoPacket.ClearCommandRelatedData();
			_E00C(0f);
			_E0DE = obj;
		}
	}

	private void _E008()
	{
		_E00D();
		_E0DE.Set(_handsChangePacket);
		ClientFirearmController clientFirearmController = HandsController as ClientFirearmController;
		ClientGrenadeController clientGrenadeController = HandsController as ClientGrenadeController;
		ClientKnifeController clientKnifeController = HandsController as ClientKnifeController;
		ClientQuickGrenadeThrowController clientQuickGrenadeThrowController = HandsController as ClientQuickGrenadeThrowController;
		ClientQuickKnifeKickController clientQuickKnifeKickController = HandsController as ClientQuickKnifeKickController;
		ClientEmptyHandsController clientEmptyHandsController = HandsController as ClientEmptyHandsController;
		if (clientFirearmController != null)
		{
			_E0DE.Set(clientFirearmController.FirearmPacket);
			_E0DE.ViewPacket.WeaponOverlap = Mathf.Clamp(clientFirearmController.OverlapValue, 0f, 1f);
			clientFirearmController.FirearmPacket = default(_E6EE);
		}
		else if (clientGrenadeController != null)
		{
			_E0DE.Set(clientGrenadeController.GrenadePacket);
			clientGrenadeController.GrenadePacket = default(_E953);
		}
		else if (clientKnifeController != null)
		{
			_E0DE.Set(clientKnifeController.KnifePacket);
			clientKnifeController.KnifePacket = default(_E959);
		}
		else if (clientQuickGrenadeThrowController != null)
		{
			_E0DE.Set(clientQuickGrenadeThrowController.GrenadePacket);
			clientQuickGrenadeThrowController.GrenadePacket = default(_E953);
		}
		else if (clientQuickKnifeKickController != null)
		{
			_E0DE.Set(clientQuickKnifeKickController.KnifePacket);
			clientQuickKnifeKickController.KnifePacket = default(_E959);
		}
		else if (clientEmptyHandsController != null)
		{
			_E0DE.Set(clientEmptyHandsController.EmptyHandPacket);
			clientEmptyHandsController.EmptyHandPacket = default(_E94E);
		}
		else
		{
			_E009();
		}
		_E0DE.VoipState = base.VoipState;
		_E0DE.IsTalked = this._E000;
		if (_E0F6)
		{
			_E0DE.CutsceneEndedPacket = new _E6B0
			{
				CutsceneSceneNameHash = _E0F7.calledOnSceneName.GetHashCode(),
				CutsceneID = _E0F7.cutseneID,
				IsStarted = false
			};
			_E0F6 = false;
		}
		_handsChangePacket = default(_E6E5);
		_E00C(LastDeltaTime);
	}

	private void _E009()
	{
		if (HandsController is ClientPortableRangeFinderController)
		{
			_E00A();
		}
		if (HandsController is ClientRadioTransmitterController)
		{
			_E00B();
		}
	}

	private new void _E00A()
	{
		ClientPortableRangeFinderController clientPortableRangeFinderController = HandsController as ClientPortableRangeFinderController;
		_E0DE.Set(clientPortableRangeFinderController.UsableItemPacket);
		clientPortableRangeFinderController.UsableItemPacket = default(_E967);
	}

	private new void _E00B()
	{
		ClientRadioTransmitterController clientRadioTransmitterController = HandsController as ClientRadioTransmitterController;
		_E0DE.Set(clientRadioTransmitterController.UsableItemPacket);
		clientRadioTransmitterController.UsableItemPacket = default(_E967);
	}

	private void _E00C(float deltaTime)
	{
		base.FrameIndexer.LocalIndex++;
		_E0DE.FrameId = base.FrameIndexer.LocalIndex;
		_E0DE.RTT = _E011();
		if (!_E0E1 && deltaTime > Mathf.Epsilon && deltaTime < 1f / 120f)
		{
			_E0E1 = true;
		}
		_E0DE.IsExtraPrecisionMovement = _E0E1 || _E0DE.MovementInfoPacket.FullPositionSync;
		_dataSender.Send(ref _E0DE, deltaTime, _E0E9, _E0F3.LastServerWorldTime);
		if (_E0DE.HasCriticalData)
		{
			_E0E1 = false;
			_E0EE++;
			_E0F0.Enqueue(_E0DE.DeltaTimeFromLastCriticalPacket);
		}
		_E0DF = _E0DE;
		_E0E0 = true;
		_E0DE = default(_E6C2);
	}

	private new void _E00D()
	{
		if (_E0F5)
		{
			_E0F8.ClearCommandMask();
			_E101 = true;
			_E102 = Time.time;
			_E0FA.CommandMask = 0;
			_E0FA.AnimatorStateIndex = 6;
			_E0FA.EPlayerState = EPlayerState.Idle;
			_E0DE.Set(_E0FA);
			_E100 = true;
			_E101 = true;
			_E0DB = default(_E6DA);
			_E0DC = default(_E6DB);
			_lootInteractionPacket = default(_E6DE);
			_E0DD = default(_E6DC);
			return;
		}
		byte commandMask = _E0F8.CommandMask;
		_E0F8.ClearCommandMask();
		if (!_E101 && Time.time - _E102 > 2.17f)
		{
			_E101 = true;
		}
		_E6D9 obj = new _E6D9(base.MovementContext.TransformPosition, base.MovementContext.Rotation, commandMask, base.MovementContext.CurrentState.Name, base.CurrentAnimatorStateIndex, base.MovementContext.MovementDirection, base.MovementContext.SmoothedPoseLevel, base.MovementContext.ClampSpeed(base.MovementContext.SmoothedCharacterMovementSpeed), base.MovementContext.SmoothedTilt, base.MovementContext.Step, base.MovementContext.BlindFire, _E0DB, _lootInteractionPacket, _E0DC, _E0DD, base.MovementContext.SoftSurface, HeadRotation, Physical.SerializationStruct, _E100, (int)base.MovementContext.DiscreteDirection, base.MovementContext.IsGrounded, base.MovementContext.SurfaceNormal, base.MovementContext.PlayerSurfaceUpAlignNormal);
		if (_E101)
		{
			obj.FullPositionSync = true;
			_E102 = Time.time;
		}
		_E0FA = obj;
		_E0DE.Set(obj);
		_E100 = false;
		_E101 = false;
		_E0DB = default(_E6DA);
		_E0DC = default(_E6DB);
		_lootInteractionPacket = default(_E6DE);
		_E0DD = default(_E6DC);
	}

	public void ResetHands()
	{
		UnityEngine.Debug.LogWarning(_ED3E._E000(183246));
	}

	public override void ApplyDamageInfo(_EC23 damageInfo, EBodyPart bodyPartType, float absorbed, EHeadSegment? headSegment = null)
	{
		EDamageType damageType = damageInfo.DamageType;
		if (!damageType.IsWeaponInduced())
		{
			ReceiveDamage(damageInfo.Damage, bodyPartType, damageType, absorbed, MaterialType.None);
		}
	}

	public override _E6FF ApplyShot(_EC23 damageInfo, EBodyPart bodyPartType, _EC22 shotId)
	{
		ShotReactions(damageInfo, bodyPartType);
		_preAllocatedArmorComponents.Clear();
		((Player)this)._E0DE.Inventory.GetPutOnArmorsNonAlloc(_preAllocatedArmorComponents);
		int pitch = 60;
		int yaw = 0;
		if (bodyPartType == EBodyPart.Head)
		{
			Vector3 normalized = (PlayerBones.Head.Original.InverseTransformPoint(damageInfo.HitPoint) - ((SphereCollider)damageInfo.HitCollider).center).normalized;
			pitch = (int)(Mathf.Asin(0f - normalized.x) * 57.29578f);
			yaw = (int)(Mathf.Atan2(normalized.z, normalized.y) * 57.29578f);
			yaw = ((yaw > 180) ? (360 - yaw) : ((yaw < 0) ? (-yaw) : yaw));
		}
		_E6FF obj = new _E6FF
		{
			PoV = PointOfView,
			Material = MaterialType.Body,
			Penetrated = true
		};
		for (int i = 0; i < _preAllocatedArmorComponents.Count; i++)
		{
			ArmorComponent armorComponent = _preAllocatedArmorComponents[i];
			if (armorComponent.ShotMatches(bodyPartType, pitch, yaw))
			{
				obj.Penetrated = false;
				obj.Material = armorComponent.Material;
				obj.Silent = bodyPartType == EBodyPart.Head;
				break;
			}
		}
		_preAllocatedArmorComponents.Clear();
		return obj;
	}

	public override void MouseLook(bool forceApplyToOriginalRibcage = false)
	{
		base.MovementContext.RotationAction?.Invoke(this);
	}

	public void OnDeserializeInitialState(NetworkReader networkReader, _E6C3 deferredData, Callback callback)
	{
		_E0ED = deferredData;
		_E0E6 = networkReader.ReadByte();
		if (_E0E6 < _E6A8._E006)
		{
			Logger.LogError(string.Format(_ED3E._E000(183280), _E0E6));
		}
		bool isAlive = networkReader.ReadBoolean();
		Vector3 position = networkReader.ReadVector3();
		Quaternion rotation = networkReader.ReadQuaternion();
		bool isInPronePose = networkReader.ReadBoolean();
		float poseLevel = networkReader.ReadSingle();
		EVoipState voipState = (EVoipState)networkReader.ReadByte();
		bool isInBufferZone = networkReader.ReadBoolean();
		int bufferZoneUsageTimeLeft = networkReader.ReadInt32();
		base.MalfRandoms.Deserialize(networkReader);
		base.Transform.position = position;
		_E00E(callback, networkReader, isAlive, rotation, isInPronePose, poseLevel, voipState, isInBufferZone, bufferZoneUsageTimeLeft).HandleExceptions();
	}

	private async Task _E00E(Callback callback, NetworkReader reader, bool isAlive, Quaternion rotation, bool isInPronePose, float poseLevel, EVoipState voipState, bool isInBufferZone, int bufferZoneUsageTimeLeft)
	{
		byte[] healthState = null;
		string itemId = null;
		EController eController = EController.None;
		bool isInSpawnOperation = true;
		bool flag = false;
		Vector2 stationaryRotation = Vector2.zero;
		Quaternion identity = Quaternion.identity;
		int animationVariant = 0;
		Profile profile = null;
		byte[] inventoryZip = reader.SafeReadSizeAndBytes();
		byte[] profileZip = reader.SafeReadSizeAndBytes();
		byte[] searchInfoSerializationBytes = reader.SafeReadSizeAndBytes();
		MongoID firstId = reader.ReadMongoId();
		if (isAlive)
		{
			reader.ReadBoolean();
			ScavExfilMask = reader.ReadInt32();
			healthState = reader.SafeReadSizeAndBytes();
			animationVariant = reader.ReadInt32();
			eController = (EController)reader.ReadByte();
			if (reader.ReadBoolean())
			{
				itemId = reader.ReadString();
			}
			if (eController == EController.Firearm)
			{
				isInSpawnOperation = reader.ReadBoolean();
				flag = reader.ReadBoolean();
				if (flag)
				{
					stationaryRotation = reader.ReadVector2();
					identity.y = reader.ReadSingle();
					identity.w = reader.ReadSingle();
				}
			}
			byte b = reader.ReadByte();
			string[] array = new string[b];
			for (int i = 0; i < b; i++)
			{
				array[i] = reader.ReadString();
			}
			if (eController == EController.None)
			{
				UnityEngine.Debug.LogError(_ED3E._E000(192561));
				callback.Fail(_ED3E._E000(192561));
			}
		}
		await AsyncWorker.RunOnBackgroundThread(delegate
		{
			_E5D6 obj = _E5D5.Deserialize(new _E518(searchInfoSerializationBytes));
			string json = SimpleZlib.Decompress(profileZip);
			profile = json.ParseJsonTo<Profile>(Array.Empty<JsonConverter>());
			using (MemoryStream input = new MemoryStream(SimpleZlib.DecompressToBytes(inventoryZip)))
			{
				using BinaryReader reader2 = new BinaryReader(input);
				profile.Inventory = _E672.DeserializeInventory(Singleton<_E63B>.Instance, reader2.ReadEFTInventoryDescriptor());
			}
			_EA91[] items = profile.Inventory.NonQuestItems.OfType<_EA91>().ToArray();
			_E5D5.FillSearchInfo(obj.Data, items);
		});
		await _E00F(profile, firstId, rotation, isAlive, eController, isInSpawnOperation, itemId, healthState, isInPronePose, poseLevel, flag, stationaryRotation, identity, animationVariant, voipState, isInBufferZone, bufferZoneUsageTimeLeft, callback);
	}

	private async Task _E00F(Profile profile, MongoID firstId, Quaternion rotation, bool isAlive, EController type, bool isInSpawnOperation, string itemId, byte[] healthState, bool isInPronePose, float poseLevel, bool isStationaryWeapon, Vector2 stationaryRotation, Quaternion playerStationaryRotation, int animationVariant, EVoipState voipState, bool isInBufferZone, int bufferZoneUsageTimeLeft, Callback callback)
	{
		if (_E0ED == null)
		{
			_E0ED = _E6C3._E000(0);
		}
		_E002 obj = new _E002(this, profile, firstId);
		_E942 obj2 = new _E942(profile, obj, _E0FF);
		obj2.Run();
		EPointOfView pointOfView = (IsBot ? EPointOfView.ThirdPerson : EPointOfView.FirstPerson);
		await Init(rotation, _ED3E._E000(60679), pointOfView, profile, obj, new _E9CB(healthState, this, obj, profile.Skills), new _E757(), obj2, new _E610(), voipState, IsBot, async: false);
		profile.Skills.StartClientMode();
		foreach (_EA6A item2 in profile.Inventory.NonQuestItems.OfType<_EA6A>())
		{
			obj.StrictCheckMagazine(item2, status: true, profile.MagDrillsMastering, notify: false, useOperation: false);
		}
		Task onCurrentHandsControllerSpawned;
		if (isAlive)
		{
			AbstractHandsController controller = null;
			switch (type)
			{
			case EController.Empty:
				controller = await ClientEmptyHandsController._E001(this);
				break;
			case EController.Firearm:
				controller = await ClientFirearmController._E001(this, itemId, isStationaryWeapon);
				break;
			case EController.Meds:
				controller = await ClientMedsController._E001(this, itemId, EBodyPart.Head, 1f, animationVariant);
				break;
			case EController.Grenade:
				controller = await ClientGrenadeController._E001(this, itemId);
				break;
			case EController.Knife:
				controller = await ClientKnifeController._E001(this, itemId);
				break;
			case EController.UsableItem:
			{
				Item item = obj.FindItem(itemId);
				if (item is _EA82)
				{
					controller = await UsableItemController._E001<ClientPortableRangeFinderController>(this, item);
				}
				if (item is _EA87)
				{
					controller = await UsableItemController._E001<ClientRadioTransmitterController>(this, item);
				}
				break;
			}
			case EController.QuickGrenade:
				controller = await ClientQuickGrenadeThrowController._E001(this, itemId);
				break;
			case EController.QuickKnife:
				controller = await ClientQuickKnifeKickController._E001(this, itemId);
				break;
			case EController.None:
				UnityEngine.Debug.LogError(_ED3E._E000(192561));
				callback.Fail(_ED3E._E000(192561));
				return;
			default:
				throw new Exception(_ED3E._E000(192606) + type);
			}
			onCurrentHandsControllerSpawned = SpawnController(controller);
			if (isStationaryWeapon)
			{
				FastForwardToStationaryWeapon(HandsController.Item, stationaryRotation, rotation, playerStationaryRotation);
			}
		}
		else
		{
			onCurrentHandsControllerSpawned = SpawnController(await ClientEmptyHandsController._E001(this));
			OnDead(EDamageType.Undefined);
		}
		((Player)this)._E0DE.RaiseEvent(new _EAFA(HandsController.Item, CommandStatus.Begin));
		((Player)this)._E0DE.RaiseEvent(new _EAFA(HandsController.Item, CommandStatus.Succeed));
		if (!isInSpawnOperation)
		{
			if (HandsController != null)
			{
				if (HandsController is _E6CB obj3)
				{
					Weapon.EMalfunctionState state = obj3.Item.MalfState.State;
					obj3.Item.MalfState.ChangeStateSilent(Weapon.EMalfunctionState.None);
					HandsController.FastForwardCurrentState();
					obj3.Item.MalfState.ChangeStateSilent(state);
				}
				else
				{
					HandsController.FastForwardCurrentState();
				}
			}
			else
			{
				UnityEngine.Debug.LogError(_ED3E._E000(192638));
			}
		}
		if (isAlive)
		{
			base.MovementContext.SetPoseLevel(poseLevel, force: true);
			base.MovementContext.IsInPronePose = isInPronePose;
			if (isInPronePose)
			{
				base.MovementContext.SetProneStateForce();
			}
			UpdateSpeedLimitByHealth();
			UpdateArmsCondition();
			_E0F8.ClearCommandMask();
		}
		_animators[0].enabled = true;
		_E0ED._E003(_E010);
		_E0ED = null;
		if (_E0FF is _E8B3 obj4)
		{
			_ECB1 insuranceCompany = obj4.InsuranceCompany;
			_E541[] insuredItems = profile.InsuredItems;
			if (insuranceCompany != null && insuredItems != null)
			{
				insuranceCompany.ClearInsuredItems();
				insuranceCompany.RegisterInsuredItems(insuredItems, ((Player)this)._E0DE.Inventory.NonQuestItems);
				((Player)this)._E0DE.RegisterView(insuranceCompany);
			}
		}
		_EBEB.Instance.HandleReconnectedPlayer(isInBufferZone, this, bufferZoneUsageTimeLeft, onCurrentHandsControllerSpawned);
		callback.Succeed();
	}

	public override void OnDeserializeFromServer(byte channelId, _E524 reader, int rtt)
	{
		if (_E0ED == null)
		{
			_E010(reader);
		}
		else if ((int)channelId % 2 != 0)
		{
			_E0ED._E002(reader.Buffer);
		}
	}

	private void _E010(_E524 reader)
	{
		_E734 selfPlayerInfo = default(_E734);
		_E734.Deserialize(reader, ref selfPlayerInfo);
		_E0EF += selfPlayerInfo.CriticalPacketsProcessed;
		_E0F0.DequeueCount(selfPlayerInfo.CriticalPacketsProcessed);
		if (selfPlayerInfo.RTT.HasValue)
		{
			_E012(selfPlayerInfo.RTT.Value, selfPlayerInfo.ServerFixedUpdate, selfPlayerInfo.ServerTime);
		}
		List<_E956> hitInfos = selfPlayerInfo.HitInfos;
		List<_E949> armorUpdates = selfPlayerInfo.ArmorUpdates;
		List<_E961> poisonUpdates = selfPlayerInfo.PoisonUpdates;
		List<_E94D> detailedHitInfo = selfPlayerInfo.DetailedHitInfo;
		List<_E957> operationStatuses = selfPlayerInfo.OperationStatuses;
		if (hitInfos != null)
		{
			for (int i = 0; i < hitInfos.Count; i++)
			{
				_E956 obj = hitInfos[i];
				if (!obj.DamageType.IsWeaponInduced())
				{
					try
					{
						ReceiveDamage(obj.Damage, obj.BodyPart, obj.DamageType, 0f, MaterialType.None);
					}
					catch (Exception ex)
					{
						_E7A8.TraceError(ETraceCode.ClientDamageReceiveActionException, ex);
						UnityEngine.Debug.LogException(ex);
					}
				}
			}
		}
		if (armorUpdates != null)
		{
			for (int j = 0; j < armorUpdates.Count; j++)
			{
				try
				{
					ProcessArmorPointsChange(armorUpdates[j].Id, armorUpdates[j].Durability);
				}
				catch (Exception ex2)
				{
					_E7A8.TraceError(ETraceCode.ClientProcessArmorPointsChangeException, ex2);
					UnityEngine.Debug.LogException(ex2);
				}
			}
		}
		if (poisonUpdates != null)
		{
			for (int k = 0; k < poisonUpdates.Count; k++)
			{
				try
				{
					ProcessPoisonResourceChange(poisonUpdates[k].Id, poisonUpdates[k].Resource);
				}
				catch (Exception ex3)
				{
					_E7A8.TraceError(ETraceCode.ClientProcessPoisonResourceChangeException, ex3);
					UnityEngine.Debug.LogException(ex3);
				}
			}
		}
		if (detailedHitInfo != null)
		{
			for (int l = 0; l < detailedHitInfo.Count; l++)
			{
				_E94D obj2 = detailedHitInfo[l];
				try
				{
					ApplyHitDebuff(obj2.Damage, obj2.StaminaLoss, obj2.Part, obj2.DamageType);
				}
				catch (Exception ex4)
				{
					_E7A8.TraceError(ETraceCode.ClientApplyHitDebuffException, ex4);
					UnityEngine.Debug.LogException(ex4);
				}
				try
				{
					ReceiveDamage(obj2.Damage, obj2.Part, obj2.DamageType, obj2.Absorbed, (obj2.Special != 0) ? ((obj2.Special == EHitSpecial.Ricochet) ? MaterialType.HelmetRicochet : ((obj2.Special == EHitSpecial.Helmet) ? MaterialType.Helmet : MaterialType.GlassVisor)) : MaterialType.None);
				}
				catch (Exception ex5)
				{
					_E7A8.TraceError(ETraceCode.ClientDamageReceiveActionException, ex5);
					UnityEngine.Debug.LogException(ex5);
				}
			}
		}
		if (operationStatuses != null)
		{
			for (int m = 0; m < operationStatuses.Count; m++)
			{
				try
				{
					_E013(operationStatuses[m].Id, operationStatuses[m].Status, operationStatuses[m].Error, 0, badBeforeExecuting: false);
				}
				catch (Exception ex6)
				{
					_E7A8.TraceError(ETraceCode.ClientProcessRpcClientOperationStatusException, ex6);
					UnityEngine.Debug.LogException(ex6);
				}
			}
		}
		if (selfPlayerInfo.CommonPacket != null)
		{
			ProcessCommonPacket(selfPlayerInfo.CommonPacket);
		}
		_E746? obj3 = selfPlayerInfo.AcceptHitDebugDataPacket;
		while (obj3.HasValue)
		{
			_E746 valueOrDefault = obj3.GetValueOrDefault();
			try
			{
				_E019(valueOrDefault.ClientShots);
			}
			catch (Exception ex7)
			{
				_E7A8.TraceError(ETraceCode.ClientProcessRpcAcceptHitDebugDataException, ex7);
				UnityEngine.Debug.LogException(ex7);
			}
			obj3 = valueOrDefault.GetNested();
		}
		_E747? obj4 = selfPlayerInfo.QuestConditionValueChangedPacket;
		while (obj4.HasValue)
		{
			_E747 valueOrDefault2 = obj4.GetValueOrDefault();
			int questIdHash = obj4.Value.QuestData.QuestIdHash;
			_E95C<_E749> conditions = obj4.Value.QuestData.Conditions;
			for (int n = 0; n < conditions.Length; n++)
			{
				_E749 obj5 = conditions[n];
				try
				{
					_E01A(questIdHash, obj5.Status, obj5.ConditionId, obj5.Value, obj5.Notify);
				}
				catch (Exception ex8)
				{
					_E7A8.TraceError(ETraceCode.ClientProcessRpcQuestConditionValueChangedException, ex8);
					UnityEngine.Debug.LogException(ex8);
				}
			}
			obj4 = valueOrDefault2.GetNested();
		}
		_E74A? obj6 = selfPlayerInfo.ShowStatNotificationPacket;
		while (obj6.HasValue)
		{
			_E74A valueOrDefault3 = obj6.GetValueOrDefault();
			try
			{
				_E857.DisplayNotification(new _E8A5((LocalizationKey)valueOrDefault3.LocalizationKey1, (LocalizationKey)valueOrDefault3.LocalizationKey2, valueOrDefault3.Value));
			}
			catch (Exception ex9)
			{
				_E7A8.TraceError(ETraceCode.ClientShowStatNotificationException, ex9);
				UnityEngine.Debug.LogException(ex9);
			}
			obj6 = valueOrDefault3.GetNested();
		}
		_E74C? obj7 = selfPlayerInfo.ClientConfirmCallbackPacket;
		while (obj7.HasValue)
		{
			_E74C valueOrDefault4 = obj7.GetValueOrDefault();
			try
			{
				_E014(valueOrDefault4.CallbackId, valueOrDefault4.Error, valueOrDefault4.InventoryHashSum, valueOrDefault4.BadBeforeExecuting);
			}
			catch (Exception ex10)
			{
				_E7A8.TraceError(ETraceCode.ClientProcessClientConfirmCallbackException, ex10);
				UnityEngine.Debug.LogException(ex10);
			}
			obj7 = valueOrDefault4.GetNested();
		}
		_E74D? obj8 = selfPlayerInfo.WeaponOverheatPacket;
		while (obj8.HasValue)
		{
			_E74D valueOrDefault5 = obj8.GetValueOrDefault();
			try
			{
				_E018(valueOrDefault5.WeaponId, valueOrDefault5.LastOverheat, valueOrDefault5.LastShotTime, valueOrDefault5.SlideOnOverheatReached);
			}
			catch (Exception ex11)
			{
				_E7A8.TraceError(ETraceCode.ClientProcessWeaponOverheatException, ex11);
				UnityEngine.Debug.LogException(ex11);
			}
			obj8 = valueOrDefault5.GetNested();
		}
		ProcessChangeSkillExperience(selfPlayerInfo.ChangeSkillExperiencePacket, silent: true);
		ProcessChangeMasteringExperience(selfPlayerInfo.ChangeMasteringExperiencePacket, silent: true);
		_E73F? obj9 = selfPlayerInfo.TradersInfoPacket;
		while (obj9.HasValue)
		{
			_E73F valueOrDefault6 = obj9.GetValueOrDefault();
			try
			{
				_E015(valueOrDefault6.TraderId, valueOrDefault6.Standing);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			obj9 = valueOrDefault6.GetNested();
		}
		_E741? obj10 = selfPlayerInfo.RadioTransmitterPacket;
		while (obj10.HasValue)
		{
			_E741 valueOrDefault7 = obj10.GetValueOrDefault();
			try
			{
				_E016(valueOrDefault7.IsEncoded, valueOrDefault7.Status, valueOrDefault7.IsAgressor);
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogException(exception2);
			}
			obj10 = valueOrDefault7.GetNested();
		}
		_E742? obj11 = selfPlayerInfo.LighthouseTraderZoneDebugToolPacket;
		while (obj11.HasValue)
		{
			_E742 valueOrDefault8 = obj11.GetValueOrDefault();
			try
			{
				_E017(valueOrDefault8.Active);
			}
			catch (Exception exception3)
			{
				UnityEngine.Debug.LogException(exception3);
			}
			obj11 = valueOrDefault8.GetNested();
		}
		_E740? obj12 = selfPlayerInfo.StringNotificationPacket;
		while (obj12.HasValue)
		{
			_E740 valueOrDefault9 = obj12.GetValueOrDefault();
			try
			{
				ShowStringNotification(valueOrDefault9.Message);
			}
			catch (Exception exception4)
			{
				UnityEngine.Debug.LogException(exception4);
			}
			obj12 = valueOrDefault9.GetNested();
		}
		_E9C7? obj13 = selfPlayerInfo.SyncHealthPacket;
		while (obj13.HasValue)
		{
			base.NetworkHealthController.HandleSyncPacket(obj13.Value);
			obj13 = obj13.Value.GetNested();
		}
		if (selfPlayerInfo.PlayerDiedPacket.HasValue)
		{
			_E74B valueOrDefault10 = selfPlayerInfo.PlayerDiedPacket.GetValueOrDefault();
			try
			{
				ProcessNetworkDeath(valueOrDefault10.Aggressor, valueOrDefault10.AggressorMainProfileName, valueOrDefault10.AggressorSide, valueOrDefault10.BodyPart, valueOrDefault10.WeaponName, valueOrDefault10.MemberCategory);
			}
			catch (Exception ex12)
			{
				_E7A8.TraceError(ETraceCode.ClientProcessNetworkDeathException, ex12);
				UnityEngine.Debug.LogException(ex12);
			}
		}
		if (selfPlayerInfo.TalkDetected)
		{
			base.HearingDateTime = _E5AD.UtcNow;
		}
	}

	public override void UpdateTick()
	{
		if (_E0ED == null)
		{
			_E0E9 += base.DeltaTime;
			_E0F2?.Update();
			base.UpdateTick();
		}
	}

	private ushort? _E011()
	{
		if (!RTTEnabled)
		{
			return null;
		}
		if (Time.time - _E0E8 < RTTDelay)
		{
			return null;
		}
		_E0E8 = Time.time;
		ushort num = (ushort)(Time.frameCount % 1024);
		Tuple<ushort, DateTime> item = new Tuple<ushort, DateTime>(num, _E5AD.UtcNow);
		_E0E7.Enqueue(item);
		return num;
	}

	private void _E012(ushort rttKey, int singleFixedUpdate, int serverTime)
	{
		if (!RTTEnabled)
		{
			return;
		}
		while (_E0E7.Count > 0)
		{
			var (num2, dateTime2) = _E0E7.Dequeue();
			if (num2 == rttKey)
			{
				if (Singleton<_E50D>.Instantiated)
				{
					_E50D instance = Singleton<_E50D>.Instance;
					instance.PlayerRTT = (int)(_E5AD.UtcNow - dateTime2).TotalMilliseconds;
					instance.ServerFixedUpdateTime = singleFixedUpdate;
					instance.ServerTime = serverTime;
				}
				break;
			}
		}
	}

	protected override void UpdatePhones()
	{
		UpdatePhonesReally();
	}

	public override void TryInteractionCallback(LootableContainer container)
	{
		if (container != null && _openAction != null)
		{
			_openAction(delegate
			{
				_E0DB.Id = container.Id;
				_E0DB.EInteractionType = EInteractionType.Close;
				_E0DB.Execute = EInteractionStage.Execute;
				_E0DB.HasInteraction = true;
				container.Interact(new _EBFE(EInteractionType.Close));
				if (base.MovementContext.LevelOnApproachStart > 0f)
				{
					base.MovementContext.SetPoseLevel(base.MovementContext.LevelOnApproachStart);
					base.MovementContext.LevelOnApproachStart = -1f;
				}
			});
		}
		_openAction = null;
	}

	internal override void _E06F(WorldInteractiveObject door, _EBFE interactionResult, Action callback)
	{
		_E0DB.HasInteraction = true;
		_E0DB.Id = door.Id;
		_E0DB.EInteractionType = interactionResult.InteractionType;
		_E0DB.Execute = EInteractionStage.Start;
		_E0DB.ItemId = ((interactionResult is _EBFF) ? ((_EBFF)interactionResult).Key.Item.Id : string.Empty);
		base.CurrentState.StartDoorInteraction(door, interactionResult, callback);
		UpdateInteractionCast();
	}

	internal override void _E070(WorldInteractiveObject door, _EBFE interactionResult)
	{
		base._E070(door, interactionResult);
		if (!door.ForceLocalInteraction)
		{
			_E0DB.HasInteraction = true;
			_E0DB.Id = door.Id;
			_E0DB.EInteractionType = interactionResult.InteractionType;
			_E0DB.Execute = EInteractionStage.Execute;
			_E0DB.ItemId = ((interactionResult is _EBFF) ? ((_EBFF)interactionResult).Key.Item.Id : string.Empty);
		}
		UpdateInteractionCast();
	}

	public override void OperateStationaryWeapon(StationaryWeapon stationaryWeapon, _E6DB.EStationaryCommand command)
	{
		if (command == _E6DB.EStationaryCommand.Occupy)
		{
			if (IsWaitingForNetworkCallback || !HandsController.CanRemove())
			{
				return;
			}
			((Player)this)._E0DE.ExecuteStationaryOperation(stationaryWeapon, delegate(IResult result)
			{
				if (!result.Succeed)
				{
					base.MovementContext.PlayerAnimatorSetStationary(b: false);
					base.MovementContext.PlayerAnimatorSetApproached(b: true);
					base.CurrentState.DropStationary();
					if (base.MovementContext.StationaryWeapon != null)
					{
						base.MovementContext.StationaryWeapon.Unlock(base.ProfileId);
					}
				}
			});
		}
		base.OperateStationaryWeapon(stationaryWeapon, command);
		_E0DC.HasInteraction = true;
		_E0DC.StationaryCommand = command;
		_E0DC.Id = ((stationaryWeapon != null) ? stationaryWeapon.Id : string.Empty);
	}

	internal override void _E071(string itemId, string zoneId, bool successful)
	{
		base._E071(itemId, zoneId, successful);
		_E0DD.ItemId = itemId;
		_E0DD.ZoneId = zoneId;
		_E0DD.Successful = successful;
	}

	private void _E013(uint operationId, EOperationStatus status, string error, int inventoryHashSum, bool badBeforeExecuting)
	{
		if (OperationCallbacks.TryGetValue(operationId, out var value))
		{
			if (status != 0)
			{
				OperationCallbacks.Remove(operationId);
			}
			value(string.IsNullOrEmpty(error) ? new Result<int, bool, EOperationStatus>(inventoryHashSum, badBeforeExecuting, status) : new Result<int, bool, EOperationStatus>(inventoryHashSum, badBeforeExecuting, status)
			{
				Error = error
			});
		}
		else
		{
			UnityEngine.Debug.LogError(_ED3E._E000(193557) + operationId);
		}
	}

	private void _E014(uint callbackId, string error, int inventoryHashSum, byte badBeforeExecuting)
	{
		UnityEngine.Debug.LogFormat(_ED3E._E000(193581) + callbackId + ((!string.IsNullOrEmpty(error)) ? (_ED3E._E000(193663) + error) : ""));
		if (!string.IsNullOrEmpty(error))
		{
			UnityEngine.Debug.LogError(_ED3E._E000(193649) + error + _ED3E._E000(193673));
		}
		if (_E0EB.TryGetValue(callbackId, out var value))
		{
			_E0EB.Remove(callbackId);
			value(string.IsNullOrEmpty(error) ? new Result<int, bool>(inventoryHashSum, badBeforeExecuting != 0) : new Result<int, bool>(inventoryHashSum, badBeforeExecuting != 0)
			{
				Error = error
			});
		}
		else
		{
			UnityEngine.Debug.LogError(_ED3E._E000(193557) + callbackId);
		}
	}

	private void _E015(string traderId, double standing)
	{
		if (!base.Profile.TryGetTraderInfo(traderId, out var traderInfo))
		{
			Logger.LogError(_ED3E._E000(193723) + traderId.Localized() + _ED3E._E000(135679));
		}
		else
		{
			traderInfo.SetStanding(standing);
		}
	}

	private void _E016(bool isEncoded, RadioTransmitterStatus status, bool isAgressor)
	{
		if (base.RecodableItemsHandler.TryToGetRecodableComponent<RadioTransmitterRecodableComponent>(out var component))
		{
			component.SetStatus(status);
		}
		base.IsAgressorInLighthouseTraderZone = isAgressor;
	}

	private void _E017(bool active)
	{
		_E0FC?.Invoke(active);
	}

	public void SyncLighthouseTraderZoneData(_E634 data)
	{
		_E0FD?.Invoke(data);
	}

	private void _E018(string weaponId, float lastOverheat, float lastShotTime, bool slideReached)
	{
		Weapon weapon = TryGetItemInHands<Weapon>();
		if (weapon != null && weapon.Id.Equals(weaponId))
		{
			weapon.MalfState.LastShotOverheat = lastOverheat;
			weapon.MalfState.LastShotTime = lastShotTime;
			weapon.MalfState.SlideOnOverheatReached = slideReached;
		}
	}

	protected override void OnSkillLevelChanged(_E74E skill)
	{
		base.OnSkillLevelChanged(skill);
		_E857.DisplayMessageNotification(string.Format(_ED3E._E000(193715).Localized(), skill.Id.ToString().Localized(), skill.Level.ToString()));
	}

	protected override void OnWeaponMastered(_E750 masterSkill)
	{
		base.OnWeaponMastered(masterSkill);
		_E857.DisplayMessageNotification(string.Format(_ED3E._E000(193759).Localized(), masterSkill.MasteringGroup.Id.Localized(), masterSkill.Level.ToString()));
	}

	protected override void OnDestroy()
	{
		if (((Player)this)._E0DE != null && _E0FF is _E8B3 obj)
		{
			_ECB1 insuranceCompany = obj.InsuranceCompany;
			if (insuranceCompany != null)
			{
				((Player)this)._E0DE.UnregisterView(insuranceCompany);
			}
			else
			{
				UnityEngine.Debug.LogError(_ED3E._E000(193735));
			}
		}
		else
		{
			UnityEngine.Debug.LogErrorFormat(_ED3E._E000(193800), ((Player)this)._E0DE == null, _E0FF == null);
		}
		_E0F4?.Invoke();
		base.OnDestroy();
	}

	private void _E019(ClientShot[] clientShots)
	{
		for (int i = 0; i < clientShots.Length; i++)
		{
			ClientShot packet = clientShots[i];
			UnityEngine.Debug.Log(packet.ToString());
			if (_E0FE.Size > 15)
			{
				_E0FE.Dequeue();
			}
			_E0FE.Enqueue(packet);
		}
	}

	private void _E01A(int questId, EQuestStatus status, int conditionId, double value, bool notify)
	{
		if (_questController is _E942 obj)
		{
			obj.QuestConditionValueChanged(questId, status, conditionId, (float)value, notify);
		}
	}

	public ulong OutgoingPacketsBytes()
	{
		return _dataSender.BytesWritten;
	}

	protected override void OnDead(EDamageType damageType)
	{
		base.OnDead(damageType);
		Singleton<BetterAudio>.Instance.UnsubscribeProtagonist();
	}

	protected override bool ShouldVocalizeDeath(EBodyPart bodyPart)
	{
		return true;
	}

	public override void OnGameSessionEnd(ExitStatus exitStatus, float pastTime, string locationId, string exitName)
	{
		base.OnGameSessionEnd(exitStatus, pastTime, locationId, exitName);
		Singleton<BetterAudio>.Instance.UnsubscribeProtagonist();
	}

	public void WriteCancelApplyingItemPacket()
	{
		_E0DE.CancelApplyingItemPacket = default(_E6AF);
	}

	public override void Teleport(Vector3 position, bool onServerToo = false)
	{
		base.Teleport(position, onServerToo);
		if (onServerToo)
		{
			_E0DE.DevelopTeleportPacket = new _E6B2
			{
				Position = position
			};
		}
		_E0F9 = position;
	}

	protected override void ApplyTeleportPacket(_E963 packet)
	{
		base.ApplyTeleportPacket(packet);
		_E100 = true;
		_E101 = true;
	}

	protected override void StartInflictSelfDamageCoroutine()
	{
	}

	protected override void PlayToggleSound(ref bool previousState, bool isOn, AudioClip toggleOn, AudioClip toggleOff)
	{
		if (previousState != isOn)
		{
			Singleton<BetterAudio>.Instance.PlayNonspatial(isOn ? toggleOn : toggleOff, BetterAudio.AudioSourceGroupType.Character);
		}
		previousState = isOn;
	}

	protected override void CreateNestedSource()
	{
		base.CreateNestedSource();
		NestedStepSoundSource.SetBaseVolume(0.9f);
	}

	public override bool OutOfProtagonistsRange(float spreadRange, bool buffer = true)
	{
		return false;
	}

	public override void SetAudioProtagonist()
	{
		Singleton<BetterAudio>.Instance.SetProtagonist(this);
	}

	public override void UpdateOcclusion()
	{
		if (!(base.SpeechSource == null))
		{
			AudioMixerGroup mixerGroup = (Muffled ? Singleton<BetterAudio>.Instance.SelfSpeechReverb : Singleton<BetterAudio>.Instance.VeryStandartMixerGroup);
			base.SpeechSource.SetMixerGroup(mixerGroup);
		}
	}

	internal void _E01B(float coeff)
	{
		_E0DE.DevelopSetDamageCoeffPacket = new _E6B3
		{
			Value = coeff
		};
	}

	internal void _E01C(int accessLevel)
	{
		_E0DE.DevelopSetBufferZoneAccess = new _E6BC
		{
			AccessLevel = accessLevel
		};
	}

	internal void _E01D()
	{
		_E0DE.DevelopResetBufferZoneUsageTime = default(_E6BD);
	}

	protected override void InitialProfileExamineAll()
	{
	}

	private void _E01E()
	{
		_E007();
	}

	public override void SetInventoryOpened(bool opened)
	{
		if (opened)
		{
			_E0DE.SyncSkills = true;
		}
		base.SetInventoryOpened(opened);
	}

	public override void Dispose()
	{
		Interlocked.Exchange(ref _E0F2, null)?.Dispose();
		base.Dispose();
		_dataSender = null;
	}

	[CompilerGenerated]
	private void _E01F(IResult result)
	{
		if (!result.Succeed)
		{
			base.MovementContext.PlayerAnimatorSetStationary(b: false);
			base.MovementContext.PlayerAnimatorSetApproached(b: true);
			base.CurrentState.DropStationary();
			if (base.MovementContext.StationaryWeapon != null)
			{
				base.MovementContext.StationaryWeapon.Unlock(base.ProfileId);
			}
		}
	}
}
