using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class StatsListElement : MonoBehaviour
{
	[SerializeField]
	private CustomTextMeshProUGUI _caption;

	[SerializeField]
	private CustomTextMeshProUGUI _statFieldId;

	[SerializeField]
	private CustomTextMeshProUGUI _xp;

	[SerializeField]
	private CustomTextMeshProUGUI _value;

	[SerializeField]
	private Image _xpIcon;

	public void Show(_E34D._E000 statInfo)
	{
		switch (statInfo.Type)
		{
		case _E34D.EStatType.DateTime:
			Show(_E7AD._E010._E004(statInfo.Caption), null, statInfo.Value.ToString());
			break;
		case _E34D.EStatType.Date:
			Show(_E7AD._E010._E004(statInfo.Caption), null, statInfo.Value.ToString());
			break;
		case _E34D.EStatType.Experience:
			Show(_E7AD._E010._E004(statInfo.Caption), statInfo.Value.ToString(), null);
			break;
		default:
			Show(_E7AD._E010._E004(statInfo.Caption), null, statInfo.Value.ToString());
			break;
		}
	}

	public void Show(string caption)
	{
		_caption.gameObject.SetActive(value: true);
		_caption.text = caption;
		Image component = GetComponent<Image>();
		if (component != null)
		{
			component.color = Color.black;
		}
		base.gameObject.SetActive(value: true);
	}

	public void Show(string statFieldId, string xp, [CanBeNull] string value)
	{
		_statFieldId.gameObject.SetActive(value: true);
		_statFieldId.text = statFieldId;
		if (!string.IsNullOrEmpty(xp))
		{
			_xp.gameObject.SetActive(value: true);
			_xp.text = xp;
			_xpIcon.gameObject.SetActive(value: true);
		}
		if (!string.IsNullOrEmpty(value))
		{
			_value.gameObject.SetActive(value: true);
			_value.text = value;
		}
		Image component = GetComponent<Image>();
		if (component != null)
		{
			component.color = new Color(0.0627451f, 0.0627451f, 0.0627451f, 1f);
		}
		base.gameObject.SetActive(value: true);
	}
}
