using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class UIAnimatedToggleSpawner : UISpawner<AnimatedToggle>
{
	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private ToggleGroup _toggleGroup;

	[SerializeField]
	private bool _unavailable;

	private UISpawnableToggle _E000;

	internal bool _E001
	{
		get
		{
			return SpawnableToggle._E000;
		}
		set
		{
			SpawnableToggle._E000 = value;
		}
	}

	public UISpawnableToggle SpawnableToggle
	{
		get
		{
			if (_E000 == null)
			{
				_E000 = base.SpawnedObject.GetComponent<UISpawnableToggle>();
			}
			return _E000;
		}
	}

	protected override AnimatedToggle SpawnObject()
	{
		AnimatedToggle animatedToggle = base.SpawnObject();
		SpawnableToggle._E000(_toggleGroup);
		animatedToggle.interactable = !_unavailable;
		return animatedToggle;
	}

	protected override void SetEllipsis(bool useEllipsis)
	{
		SpawnableToggle._E002(useEllipsis);
	}

	public void ToggleSilently(bool show)
	{
		base.SpawnedObject.ToggleSilent(show);
	}

	internal override void _E02E(string caption, int size)
	{
		_headerCaption = caption;
		if (caption != null && _headerFontSize != 0)
		{
			SpawnableToggle._E001(caption, _headerFontSize, null, null);
		}
	}

	public void SetActive(bool active)
	{
		if (_canvasGroup == null)
		{
			Debug.LogError(_ED3E._E000(255325) + base.gameObject.name + _ED3E._E000(255301));
		}
		else
		{
			_canvasGroup.SetUnlockStatus(active);
		}
	}
}
