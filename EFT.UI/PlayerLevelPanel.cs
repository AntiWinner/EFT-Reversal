using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class PlayerLevelPanel : MonoBehaviour
{
	private const int _E000 = 80;

	[SerializeField]
	private CustomTextMeshProUGUI _level;

	[SerializeField]
	private Image _icon;

	public void Set(int level, ESideType side)
	{
		bool flag = side == ESideType.Pmc;
		base.gameObject.SetActive(flag);
		if (flag)
		{
			_level.text = string.Format(_ED3E._E000(248078), level);
			SetLevelIcon(_icon, level);
		}
	}

	public static void SetLevelIcon(Image image, int level)
	{
		int num = ((level > 80) ? 80 : (((int)((float)level / 5f) + 1) * 5));
		image.sprite = _E905.Pop<Sprite>(_ED3E._E000(248069) + num);
		image.preserveAspect = true;
	}
}
