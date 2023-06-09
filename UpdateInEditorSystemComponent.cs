using UnityEngine;

[ExecuteInEditMode]
public abstract class UpdateInEditorSystemComponent<T> : BaseSystemComponent<T> where T : BaseSystemComponent<T>, _E3A7
{
	protected virtual void OnDestroy()
	{
	}

	public abstract void ManualUpdate(float deltaTime);
}
