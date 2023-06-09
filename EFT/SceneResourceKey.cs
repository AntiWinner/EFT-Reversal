using System;
using UnityEngine;

namespace EFT;

[Serializable]
[_EBED]
public class SceneResourceKey : ResourceKey, ISceneResource
{
	[SerializeField]
	private bool _onlyOffline;

	public bool onlyOffline
	{
		get
		{
			return _onlyOffline;
		}
		set
		{
			_onlyOffline = value;
		}
	}

	protected bool Equals(SceneResourceKey other)
	{
		if (Equals((ResourceKey)other))
		{
			return onlyOffline == other.onlyOffline;
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
		return Equals((SceneResourceKey)obj);
	}
}
