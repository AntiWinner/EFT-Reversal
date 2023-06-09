using System.Linq;
using Comfort.Common;
using UnityEngine;

namespace EFT.Utilities;

public sealed class EventObject : MonoBehaviour
{
	[SerializeField]
	private EEventType _eventType;

	[SerializeField]
	private EMissingEventActionType _eventAction = EMissingEventActionType.HideIfNotFound;

	private bool _E000
	{
		get
		{
			if (Singleton<_E5CB>.Instantiated)
			{
				return Singleton<_E5CB>.Instance.EventType.Contains(_eventType);
			}
			return true;
		}
	}

	private void OnEnable()
	{
		switch ((int)_eventAction)
		{
		case 1:
			if (!_E000)
			{
				base.gameObject.SetActive(value: false);
			}
			break;
		case 2:
			if (!_E000)
			{
				Object.Destroy(base.gameObject);
			}
			break;
		case 3:
			if (_E000)
			{
				base.gameObject.SetActive(value: false);
			}
			break;
		case 4:
			if (_E000)
			{
				Object.Destroy(base.gameObject);
			}
			break;
		}
	}
}
