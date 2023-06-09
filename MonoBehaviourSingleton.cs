using Comfort.Common;
using UnityEngine;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
	public static T Instance => Singleton<T>.Instance;

	public static bool Instantiated => Singleton<T>.Instantiated;

	public virtual void Awake()
	{
		Singleton<T>.Create((T)this);
	}

	public virtual void OnDestroy()
	{
		Singleton<T>.TryRelease((T)this);
	}
}
