using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public class UIEventSystem : MonoBehaviour
{
	private static UIEventSystem m__E000;

	private EventSystem _E001;

	private StandaloneInputModule _E002;

	private bool _E003;

	private bool _E004;

	public static UIEventSystem Instance
	{
		get
		{
			if (UIEventSystem.m__E000 != null)
			{
				return UIEventSystem.m__E000;
			}
			GameObject obj = new GameObject(_ED3E._E000(255427));
			obj.AddComponent<EventSystem>();
			obj.AddComponent<StandaloneInputModule>();
			UIEventSystem.m__E000 = obj.AddComponent<UIEventSystem>();
			return UIEventSystem.m__E000;
		}
	}

	private EventSystem _E005
	{
		get
		{
			if (!_E001)
			{
				return _E001 = Instance.GetComponent<EventSystem>();
			}
			return _E001;
		}
	}

	private StandaloneInputModule _E006
	{
		get
		{
			if (!_E002)
			{
				return _E002 = Instance.GetComponent<StandaloneInputModule>();
			}
			return _E002;
		}
	}

	private void Awake()
	{
		UIEventSystem.m__E000 = this;
		UIEventSystem.m__E000.Enable();
	}

	public void SetTemporaryStatus(bool isActive)
	{
		if (_E003 != isActive)
		{
			_E003 = isActive;
			_E000();
		}
	}

	public void Enable()
	{
		if (!_E004)
		{
			_E004 = true;
			_E000();
		}
	}

	public void Disable()
	{
		if (_E004)
		{
			_E004 = false;
			_E000();
		}
	}

	private void _E000()
	{
		bool flag = _E004 || _E003;
		_E005.enabled = flag;
		_E006.enabled = flag;
	}
}
