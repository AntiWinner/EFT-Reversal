using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class UiElementBlocker : MonoBehaviour
{
	[SerializeField]
	private CanvasGroup Group;

	[SerializeField]
	private Graphic Graphic;

	[SerializeField]
	private HoverTooltipArea Tooltip;

	public Transform Container => Group.transform;

	public void StartBlock()
	{
		StartBlock(null);
	}

	public void SetBlock(bool block, string reason = null)
	{
		if (block)
		{
			StartBlock(reason);
		}
		else
		{
			RemoveBlock();
		}
	}

	public void StartBlock(string reason)
	{
		Group.SetUnlockStatus(value: false);
		if (Graphic != null)
		{
			Graphic.enabled = true;
		}
		if (Tooltip != null)
		{
			Tooltip.enabled = !string.IsNullOrEmpty(reason);
			Tooltip.SetMessageText(reason);
		}
	}

	public void RemoveBlock()
	{
		Group.SetUnlockStatus(value: true);
		if (Graphic != null)
		{
			Graphic.enabled = false;
		}
		if (Tooltip != null)
		{
			Tooltip.enabled = false;
		}
	}
}
