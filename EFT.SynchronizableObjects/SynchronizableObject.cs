using UnityEngine;
using UnityEngine.Networking;

namespace EFT.SynchronizableObjects;

public class SynchronizableObject : MonoBehaviour
{
	private _E90D _E001;

	public SynchronizableObjectType Type;

	public int ObjectId;

	public int UniqueId;

	public bool IsActive;

	public bool IsInited;

	public bool IsStatic;

	public void SetLogic(_E90D synchronizableObject)
	{
		_E001 = synchronizableObject;
		_E001.SetSyncObject(this);
	}

	public void Init(int objectId, Vector3 position, Vector3 rotation)
	{
		_E001.Init(objectId, position, rotation);
	}

	public void InitStaticObject()
	{
		_E001.InitStaticObject();
	}

	public void UpdateSyncObjectData(ref _E358 packet)
	{
		_E001.UpdateSyncObjectData(ref packet);
	}

	public void ManualUpdate(float deltaTime)
	{
		_E001.ManualUpdate(deltaTime);
	}

	public virtual void Serialize(NetworkWriter writer)
	{
		writer.Write((ushort)ObjectId);
		writer.Write(base.transform.position);
		writer.Write(base.transform.rotation.eulerAngles);
		writer.Write((byte)UniqueId);
	}

	public virtual void Deserialize(NetworkReader reader)
	{
		ObjectId = reader.ReadUInt16();
		base.transform.position = reader.ReadVector3();
		base.transform.rotation = Quaternion.Euler(reader.ReadVector3());
		UniqueId = reader.ReadByte();
	}

	public void CollisionEnter(Collider collider)
	{
		_E001.CollisionEnter(collider);
	}

	public void TakeFromPool()
	{
		_E001.TakeFromPool();
	}

	public void ReturnToPool()
	{
		_E001.ReturnToPool();
	}
}
