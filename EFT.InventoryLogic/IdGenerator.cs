using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EFT.InventoryLogic;

[Serializable]
public sealed class IdGenerator : IDatabaseIdGenerator
{
	[JsonProperty("IdCache")]
	private List<MongoID> _idCache = new List<MongoID>();

	private int _idIndex;

	public bool Ready => _idCache.Count > _idIndex;

	[JsonIgnore]
	private MongoID _E000 => _idCache[_idIndex - 1];

	[JsonIgnore]
	public MongoID NextId
	{
		get
		{
			MongoID result = _idCache[_idIndex];
			_idIndex++;
			return result;
		}
	}

	public void Replenish(IEnumerable<MongoID> newIds)
	{
		_idCache.AddRange(newIds);
	}

	public void Reset(IEnumerable<MongoID> newIds)
	{
		if (_idCache.Count > _idIndex)
		{
			_idCache.RemoveRange(_idIndex, _idCache.Count - _idIndex);
		}
		_idCache.InsertRange(_idIndex, newIds);
	}

	public void RollBack()
	{
		_idIndex--;
	}
}
