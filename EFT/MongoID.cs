using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace EFT;

[Serializable]
[_EBEC("ReadMongoId", "WriteMongoId")]
[JsonConverter(typeof(_E69F))]
public readonly struct MongoID : IComparable<MongoID>, IEquatable<MongoID>
{
	private static System.Random _random = new System.Random();

	private static readonly ulong _processId = MongoID._E000;

	private static uint _newIdCounter;

	private readonly uint _timeStamp;

	private readonly ulong _counter;

	private readonly string _stringID;

	public static uint TimeStamp => Convert.ToUInt32(_E5AD.UtcNowUnixInt);

	private static ulong _E000 => (ulong)(((long)_random.Next(0, int.MaxValue) << 8) ^ _random.Next(0, int.MaxValue));

	[JsonConstructor]
	public MongoID([JsonProperty("$value")] string id)
	{
		_timeStamp = Convert.ToUInt32(id.Substring(0, 8), 16);
		_counter = Convert.ToUInt64(id.Substring(8, 16), 16);
		_stringID = id;
		_E000();
	}

	private MongoID(NetworkReader reader)
	{
		_timeStamp = reader.ReadUInt32();
		_counter = reader.ReadUInt64();
		_stringID = null;
		_stringID = _E001();
		_E000();
	}

	private MongoID(BinaryReader reader)
	{
		_timeStamp = reader.ReadUInt32();
		_counter = reader.ReadUInt64();
		_stringID = null;
		_stringID = _E001();
		_E000();
	}

	public MongoID(bool newProcessId)
	{
		_timeStamp = 0u;
		_stringID = null;
		if (newProcessId)
		{
			_counter = MongoID._E000 << 24;
		}
		else
		{
			_newIdCounter++;
			_counter = (_processId << 24) + _newIdCounter;
		}
		_timeStamp = TimeStamp;
		_stringID = _E001();
	}

	private MongoID(MongoID source, int increment, bool newTimestamp)
	{
		_timeStamp = (newTimestamp ? TimeStamp : source._timeStamp);
		_counter = ((increment > 0) ? (source._counter + Convert.ToUInt32(increment)) : (source._counter - Convert.ToUInt32(Mathf.Abs(increment))));
		_stringID = null;
		_stringID = _E001();
	}

	private void _E000()
	{
		ulong num = Convert.ToUInt64(_counter >> 24);
		if (_processId == num)
		{
			uint val = Convert.ToUInt32(_counter << 40 >> 40);
			_newIdCounter = Math.Max(_newIdCounter, val);
		}
	}

	public MongoID Next()
	{
		return new MongoID(this, 1, newTimestamp: true);
	}

	public static MongoID Read(NetworkReader reader)
	{
		return new MongoID(reader);
	}

	public static MongoID Read(BinaryReader reader)
	{
		return new MongoID(reader);
	}

	public void Write(NetworkWriter writer)
	{
		writer.Write(_timeStamp);
		writer.Write(_counter);
	}

	public void Write(BinaryWriter writer)
	{
		writer.Write(_timeStamp);
		writer.Write(_counter);
	}

	public bool Equals(MongoID other)
	{
		if (_timeStamp == other._timeStamp)
		{
			return _counter == other._counter;
		}
		return false;
	}

	public int CompareTo(MongoID other)
	{
		if (this == other)
		{
			return 0;
		}
		if (this > other)
		{
			return 1;
		}
		return -1;
	}

	private string _E001()
	{
		return _timeStamp.ToString(_ED3E._E000(159482)).ToLower() + _counter.ToString(_ED3E._E000(159477)).ToLower();
	}

	public override string ToString()
	{
		return _stringID;
	}

	public byte[] ToBytes()
	{
		byte[] array = new byte[12];
		for (int i = 0; i < 4; i++)
		{
			array[i] = (byte)(_timeStamp >> (3 - i) * 8);
		}
		for (int j = 0; j < 8; j++)
		{
			array[j + 4] = (byte)(_counter >> (7 - j) * 8);
		}
		return array;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (obj is MongoID mongoID)
			{
				return mongoID == this;
			}
			if (obj is string text)
			{
				return text == ToString();
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		uint num = Convert.ToUInt32(_counter >> 32) * 3637;
		uint num2 = Convert.ToUInt32(_counter << 32 >> 32) * 5807;
		return (int)(_timeStamp ^ num ^ num2);
	}

	public static implicit operator string(MongoID mongoId)
	{
		return mongoId.ToString();
	}

	public static implicit operator MongoID(string id)
	{
		return new MongoID(id);
	}

	public static MongoID operator ++(MongoID id)
	{
		return new MongoID(id, 1, newTimestamp: false);
	}

	public static MongoID operator --(MongoID id)
	{
		return new MongoID(id, -1, newTimestamp: false);
	}

	public static bool operator ==(MongoID a, MongoID b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(MongoID a, MongoID b)
	{
		return !a.Equals(b);
	}

	public static bool operator >(MongoID a, MongoID b)
	{
		if (a._timeStamp <= b._timeStamp)
		{
			if (a._timeStamp == b._timeStamp)
			{
				return a._counter > b._counter;
			}
			return false;
		}
		return true;
	}

	public static bool operator <(MongoID a, MongoID b)
	{
		if (a._timeStamp >= b._timeStamp)
		{
			if (a._timeStamp == b._timeStamp)
			{
				return a._counter < b._counter;
			}
			return false;
		}
		return true;
	}

	public static bool operator >=(MongoID a, MongoID b)
	{
		if (!(a == b))
		{
			return a > b;
		}
		return true;
	}

	public static bool operator <=(MongoID a, MongoID b)
	{
		if (!(a == b))
		{
			return a < b;
		}
		return true;
	}
}
