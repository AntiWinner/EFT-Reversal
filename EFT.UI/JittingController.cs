using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class JittingController : MonoBehaviour
{
	private void Awake()
	{
		_E000();
	}

	private void _E000()
	{
		ScrollRect[] componentsInChildren = GetComponentsInChildren<ScrollRect>(includeInactive: true);
		foreach (ScrollRect scrollRect in componentsInChildren)
		{
			if (!(scrollRect is ScrollRectNoDrag))
			{
				scrollRect.gameObject.GetOrAddComponent<UIJittingStabilizer>();
			}
		}
	}
}
