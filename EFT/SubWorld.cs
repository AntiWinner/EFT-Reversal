using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using ComponentAce.Compression.Libs.zlib;
using Diz.Utils;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.Networking;

namespace EFT;

internal sealed class SubWorld : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public byte[] bytes;

		public byte[] lootJsonZip;

		public SubWorld _003C_003E4__this;

		internal Func<TimeSpan> _E000()
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001
			{
				CS_0024_003C_003E8__locals1 = this,
				stopwatch = Stopwatch.StartNew()
			};
			_E518 stream = new _E518(bytes);
			CS_0024_003C_003E8__locals0.info = _E5D5.Deserialize(stream);
			using (MemoryStream input = new MemoryStream(SimpleZlib.DecompressToBytes(lootJsonZip)))
			{
				using BinaryReader reader = new BinaryReader(input);
				CS_0024_003C_003E8__locals0.loot = _E672.DeserializeLootData(Singleton<_E63B>.Instance, reader.ReadEFTLootDataDescriptor());
			}
			CS_0024_003C_003E8__locals0.stopwatch.Stop();
			return () => CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._E002(CS_0024_003C_003E8__locals0.stopwatch, CS_0024_003C_003E8__locals0.loot, CS_0024_003C_003E8__locals0.info.Data);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Stopwatch stopwatch;

		public _E548 loot;

		public _E5D6 info;

		public _E000 CS_0024_003C_003E8__locals1;

		internal TimeSpan _E000()
		{
			return CS_0024_003C_003E8__locals1._003C_003E4__this._E002(stopwatch, loot, info.Data);
		}
	}

	private GameWorld m__E000;

	private World m__E001;

	private int m__E002;

	internal static readonly int _E003 = 32;

	private ArrayPool<byte> _E004;

	internal static NetworkHash128 _E005 => NetworkHash128.Parse(_ED3E._E000(47877));

	internal static SubWorld _E000(GameWorld gameWorld, World world, int index, ArrayPool<byte> serializationPool)
	{
		GameObject obj = new GameObject(_ED3E._E000(110219) + index);
		obj.transform.parent = gameWorld.gameObject.transform;
		SubWorld subWorld = obj.AddComponent<SubWorld>();
		subWorld.m__E000 = gameWorld;
		subWorld.m__E001 = world;
		subWorld.m__E002 = index;
		subWorld._E004 = serializationPool;
		return subWorld;
	}

	public void OnSerialize(NetworkWriter writer, _E545[] loot)
	{
		int num = loot.Length;
		int num2 = (int)Math.Ceiling((double)num / (double)_E003);
		int num3 = this.m__E002 * num2;
		int num4 = num3 + num2;
		int num5 = num4 - num3;
		if (num > num3)
		{
			if (num < num4)
			{
				num4 = num;
				num5 = num4 - num3;
			}
		}
		else
		{
			num5 = 0;
		}
		bool flag = num5 > 0;
		writer.Write(flag);
		if (flag)
		{
			loot = loot.Skip(num3).Take(num5).ToArray();
			_E001(writer, loot);
		}
	}

	private void _E001(NetworkWriter writer, _E545[] loot)
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using BinaryWriter writer2 = new BinaryWriter(memoryStream);
			writer2.Write(_E672.SerializeLootData(loot));
			byte[] array = memoryStream.ToArray();
			byte[] buffer = SimpleZlib.CompressToBytes(array, array.Length, 9);
			writer.WriteBytesFull(buffer);
		}
		IEnumerable<Item> allItemsFromCollections = loot.Select((_E545 x) => x.Item).OfType<ContainerCollection>().GetAllItemsFromCollections();
		byte[] array2 = _E004.Rent(65536);
		_E51A obj = new _E51A(array2);
		_E5D5.Serialize(obj, allItemsFromCollections);
		obj.Flush();
		writer.WriteBytesAndSize(obj.Buffer, obj.BytesWritten);
		_E004.Return(array2);
	}

	public void OnDeserialize(NetworkReader reader)
	{
		reader = this.m__E001._E001(reader);
		if (!reader.ReadBoolean())
		{
			this.m__E001._E005(null);
			this.m__E001.EmptySubWorld();
			return;
		}
		byte[] lootJsonZip = reader.ReadBytesAndSize();
		byte[] bytes = reader.ReadBytesAndSize();
		Task<Func<TimeSpan>> task = AsyncWorker.RunOnBackgroundThread((Func<Func<TimeSpan>>)delegate
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			_E518 stream = new _E518(bytes);
			_E5D6 info = _E5D5.Deserialize(stream);
			_E548 loot;
			using (MemoryStream input = new MemoryStream(SimpleZlib.DecompressToBytes(lootJsonZip)))
			{
				using BinaryReader reader2 = new BinaryReader(input);
				loot = _E672.DeserializeLootData(Singleton<_E63B>.Instance, reader2.ReadEFTLootDataDescriptor());
			}
			stopwatch.Stop();
			return () => _E002(stopwatch, loot, info.Data);
		});
		this.m__E001.SpawnSubWorld(task);
	}

	private TimeSpan _E002(Stopwatch stopwatch, _E548 loot, _E5D7[] info)
	{
		_ = stopwatch.ElapsedMilliseconds;
		stopwatch.Start();
		_EA91[] items = loot.Select((_E545 x) => x.Item).OfType<ContainerCollection>().GetAllItemsFromCollections()
			.OfType<_EA91>()
			.ToArray();
		_E5D5.FillSearchInfo(info, items);
		this.m__E000._E005(loot, initial: false);
		this.m__E001._E005(loot);
		stopwatch.Stop();
		return stopwatch.Elapsed;
	}
}
