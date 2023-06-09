using System;
using UnityEngine;

namespace Diz.Skinning;

public abstract class AbstractSkin : MonoBehaviour
{
	protected Action _unsubscribe;

	public abstract SkinnedMeshRenderer SkinnedMeshRenderer { get; }

	public abstract void ApplySkin();

	public abstract void Unskin();

	protected void Unsubscribe()
	{
		if (_unsubscribe != null)
		{
			Action unsubscribe = _unsubscribe;
			_unsubscribe = null;
			unsubscribe();
		}
	}
}
