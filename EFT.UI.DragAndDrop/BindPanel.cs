using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

public sealed class BindPanel : MonoBehaviour
{
	[SerializeField]
	private CustomTextMeshProUGUI _hotKey;

	public void Show(EBoundItem slotName)
	{
		_hotKey.text = Singleton<_E7DE>.Instance.Control.Settings.GetBoundItemNames(slotName);
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
