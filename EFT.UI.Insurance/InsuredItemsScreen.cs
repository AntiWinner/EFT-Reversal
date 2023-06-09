using Comfort.Common;
using EFT.UI.DragAndDrop;
using TMPro;
using UnityEngine;

namespace EFT.UI.Insurance;

public class InsuredItemsScreen : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _insuredItemsCount;

	[SerializeField]
	private InsuredItemPanel _insuredItemPanel;

	[SerializeField]
	private RectTransform _insuredItemsContainer;

	private _EC71<_ECB4, InsuredItemPanel> _E07E;

	private _ECB1 _E18F;

	public void Show(_ECB1 insurance)
	{
		ShowGameObject();
		_E18F = insurance;
		UI.AddDisposable(_E18F.InsuredItems.ItemsChanged.Bind(_E000));
		_E07E?.Dispose();
		_E07E = UI.AddDisposable(new _EC71<_ECB4, InsuredItemPanel>(_E18F.InsuredItems, _insuredItemPanel, _insuredItemsContainer, delegate(_ECB4 item, InsuredItemPanel view)
		{
			view.Show(item.Name, Singleton<_E5CB>.Instance.TradersSettings[item.InsurerId].Nickname.Localized(), ItemViewFactory.GetItemType(item.Type));
		}));
	}

	private void _E000()
	{
		_insuredItemsCount.text = string.Format(_ED3E._E000(233112).Localized(), _E18F.InsuredItems.Count);
	}
}
