using System;
using System.IO;
using JetBrains.Annotations;

namespace EFT;

[Serializable]
[_EBED]
public class ResourceKey
{
	[_EBEE]
	public string path;

	[_EBEE]
	public string rcid;

	public string FileName => Path.GetFileNameWithoutExtension(path);

	[CanBeNull]
	public string ToAssetName()
	{
		if (!string.IsNullOrEmpty(rcid))
		{
			return rcid;
		}
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		string text = Path.GetDirectoryName(path);
		if (!string.IsNullOrEmpty(text))
		{
			text += _ED3E._E000(30703);
		}
		return (text + Path.GetFileNameWithoutExtension(path)).ToLower();
	}

	protected bool Equals(ResourceKey other)
	{
		if (string.Equals(path, other.path))
		{
			if (!string.Equals(rcid, other.rcid))
			{
				if (string.IsNullOrEmpty(rcid))
				{
					return string.IsNullOrEmpty(other.rcid);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((ResourceKey)obj);
	}

	public override int GetHashCode()
	{
		return (((!string.IsNullOrEmpty(path)) ? path.GetHashCode() : 0) * 397) ^ ((!string.IsNullOrEmpty(rcid)) ? rcid.GetHashCode() : 0);
	}

	public static bool operator ==(ResourceKey left, ResourceKey right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(ResourceKey left, ResourceKey right)
	{
		return !object.Equals(left, right);
	}

	public override string ToString()
	{
		return string.Format(_ED3E._E000(134850), GetType(), path, rcid);
	}

	public bool IsEmpty()
	{
		return string.IsNullOrEmpty(path);
	}
}
