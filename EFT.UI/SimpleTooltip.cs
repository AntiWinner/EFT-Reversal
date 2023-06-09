using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace EFT.UI;

public class SimpleTooltip : Tooltip
{
	private readonly Vector2 _E187 = new Vector2(10f, 10f);

	[FormerlySerializedAs("_text")]
	[SerializeField]
	private CustomTextMeshProUGUI _label;

	private LayoutElement _E143;

	private float _E188;

	protected override void Awake()
	{
		base.Awake();
		_E143 = base.gameObject.GetComponent<LayoutElement>();
		if (_E143 != null)
		{
			_E188 = _E143.preferredWidth;
		}
	}

	public void Show(string text, Vector2? offset = null, float delay = 0f, float? maxWidth = null, bool isInteractable = true)
	{
		SetText(text);
		Show(offset ?? _E187, delay);
		_label.color = new Color(_label.color.r, _label.color.g, _label.color.b, isInteractable ? 1f : 0.3f);
		if (_E143 != null)
		{
			_E143.preferredWidth = maxWidth ?? _E188;
		}
	}

	public void ShowInventoryError(InventoryError error)
	{
		Show(_ED3E._E000(103088) + error.GetLocalizedDescription() + _ED3E._E000(59467));
	}

	public void ShowWarning(InventoryError warning)
	{
		Show(_ED3E._E000(258148) + warning.GetLocalizedDescription() + _ED3E._E000(59467));
	}

	public void SetText(string text)
	{
		_label.text = text;
	}
}
