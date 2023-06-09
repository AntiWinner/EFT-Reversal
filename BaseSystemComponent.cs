using UnityEngine;

public abstract class BaseSystemComponent<T> : MonoBehaviour, _E3A7 where T : BaseSystemComponent<T>, _E3A7
{
	protected virtual void OnEnable()
	{
		_E3A3.RegisterInSystem((T)this);
	}

	protected virtual void OnDisable()
	{
		_E3A3.UnregisterInSystem((T)this);
	}
}
