using EFT.Interactive;
using UnityEngine;

public class SpecificDoorStateChangeEventGenerator : MonoBehaviour
{
	[SerializeField]
	private Door _targetDoor;

	private void Awake()
	{
		_targetDoor.OnDoorStateChanged += _E000;
	}

	private void OnDestroy()
	{
		_targetDoor.OnDoorStateChanged -= _E000;
	}

	private void _E000(WorldInteractiveObject obj, EDoorState prevState, EDoorState nextState)
	{
		_EBAF.Instance.CreateCommonEvent<_EBB7>().Invoke(obj as Door);
	}
}
