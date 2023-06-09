using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Comfort.Common;
using ComponentAce.Compression.Libs.zlib;
using EFT.Airdrop;
using EFT.Interactive;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.Networking;

namespace EFT.SynchronizableObjects;

public class AirdropSynchronizableObject : SynchronizableObject
{
	private const int m__E000 = 65536;

	public EAirdropType AirdropType;

	public TaggedClip SqueakClip;

	public GameObject AirdropFlare;

	public GameObject AirdropDust;

	public GameObject Parachute;

	public GameObject ParachuteJoint;

	public BoxCollider CollisionCollider;

	public MeshRenderer[] DecalRenderers;

	public Material GunDecal;

	public Material MedicineDecal;

	public Material SupplyDecal;

	public override void Serialize(NetworkWriter writer)
	{
		base.Serialize(writer);
		Item rootItem = GetComponentInChildren<LootableContainer>().ItemOwner.RootItem;
		ArrayPool<byte> shared = ArrayPool<byte>.Shared;
		shared.Preallocate(65536, 2);
		writer.Write((byte)AirdropType);
		writer.Write((byte)ObjectId);
		writer.Write(rootItem.Id);
		writer.Write(IsStatic);
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using BinaryWriter writer2 = new BinaryWriter(memoryStream);
			writer2.Write(_E672.SerializeItem(rootItem));
			byte[] array = memoryStream.ToArray();
			byte[] buffer = SimpleZlib.CompressToBytes(array, array.Length, 9);
			writer.WriteBytesFull(buffer);
		}
		byte[] array2 = shared.Rent(65536);
		_E51A obj = new _E51A(array2);
		_E5D5.Serialize(obj, rootItem.ToEnumerable());
		obj.Flush();
		writer.WriteBytesAndSize(obj.Buffer, obj.BytesWritten);
		shared.Return(array2);
	}

	public override void Deserialize(NetworkReader reader)
	{
		base.Deserialize(reader);
		AirdropType = (EAirdropType)reader.ReadByte();
		ObjectId = reader.ReadByte();
		string id = reader.ReadString();
		IsStatic = reader.ReadBoolean();
		Item item = _E000(reader);
		LootableContainer componentInChildren = GetComponentInChildren<LootableContainer>().gameObject.GetComponentInChildren<LootableContainer>();
		componentInChildren.enabled = true;
		componentInChildren.Id = id;
		Singleton<GameWorld>.Instance.RegisterWorldInteractionObject(componentInChildren);
		LootItem.CreateLootContainer(componentInChildren, item, item.ShortName.Localized(), Singleton<GameWorld>.Instance);
		if (!IsStatic)
		{
			Singleton<GameWorld>.Instance.SynchronizableObjectLogicProcessor.InitSyncObject(this, base.transform.position, base.transform.rotation.eulerAngles, ObjectId);
		}
		else
		{
			Singleton<GameWorld>.Instance.SynchronizableObjectLogicProcessor.InitStaticObject(this);
		}
	}

	private Item _E000(NetworkReader reader)
	{
		Item item;
		using (MemoryStream input = new MemoryStream(SimpleZlib.DecompressToBytes(reader.ReadBytesAndSize())))
		{
			using BinaryReader reader2 = new BinaryReader(input);
			item = _E672.DeserializeItem(Singleton<_E63B>.Instance, new Dictionary<string, Item>(), reader2.ReadEFTItemDescriptor());
		}
		_E5D6 obj = _E5D5.Deserialize(new _E518(reader.ReadBytesAndSize()));
		_E548 obj2 = new _E548();
		obj2.Add(new _E545());
		obj2[0].Item = item;
		_E5D5.FillSearchInfo(items: obj2.Select((_E545 x) => x.Item).OfType<ContainerCollection>().GetAllItemsFromCollections()
			.OfType<_EA91>()
			.ToArray(), searchableItemInfos: obj.Data);
		Item[] source = obj2.Select((_E545 x) => x.Item).ToArray();
		ResourceKey[] resources = source.OfType<ContainerCollection>().GetAllItemsFromCollections().Concat(source.Where((Item x) => !(x is ContainerCollection)))
			.SelectMany((Item x) => x.Template.AllResources)
			.ToArray();
		Singleton<_E760>.Instance.LoadBundlesAndCreatePools(_E760.PoolsCategory.Raid, _E760.AssemblyType.Online, resources, _ECE3.Immediate).HandleExceptions();
		return item;
	}
}
