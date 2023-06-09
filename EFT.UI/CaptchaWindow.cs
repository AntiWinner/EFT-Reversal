using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class CaptchaWindow : HandoverItemsWindow
{
	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private ScrollRect _scroll;

	private readonly List<string> m__E000 = new List<string>();

	private Action<List<string>> m__E001;

	protected override int GridWidth => 6;

	protected override int GridHeight => 5;

	protected override bool CanStretchHorizontally => true;

	protected override bool CloseOnAccept => false;

	public _EC7B Show(string title, string description, ICollection<Item> items, Profile profile, _EB1E controller, Action<List<string>> acceptAction, Action cancelAction)
	{
		_EC7B result = Show(title, description, profile, controller, new _EB66(EItemViewType.Captcha, this), cancelAction);
		this.m__E001 = acceptAction;
		SetAcceptActive(value: false);
		UpdateItems(items);
		return result;
	}

	protected override bool IsSelected(Item item)
	{
		return ItemsList.Contains(item);
	}

	protected override bool IsActive(Item item, out string tooltip)
	{
		tooltip = string.Empty;
		return true;
	}

	public override void Accept()
	{
		_loader.SetActive(value: true);
		SetAcceptActive(value: false);
		this.m__E001?.Invoke(this.m__E000);
		base.Accept();
	}

	public void InvalidAnswer()
	{
		_loader.SetActive(value: false);
		AnimateText(Color.red).HandleExceptions();
		ClearSelectedList();
	}

	public async Task ValidAnswer()
	{
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ChatSelect);
		_loader.SetActive(value: false);
		await AnimateText(Color.green);
		CloseSilent();
	}

	protected override void UpdateItems(ICollection<Item> items)
	{
		_loader.SetActive(value: false);
		this.m__E000.Clear();
		base.UpdateItems(items);
	}

	protected override void TrySelectItemToHandover(Item item)
	{
		if (ItemsList.Contains(item))
		{
			_E001(item);
		}
		else
		{
			_E000(item);
		}
		SetAcceptActive(ItemsList.Count > 0);
	}

	protected override void SetAcceptActive(bool value)
	{
		base.AcceptButton.Interactable = value;
		base.SetAcceptActive(value);
	}

	private void _E000(Item item)
	{
		this.m__E000.Add(item.TemplateId);
		ItemsList.Add(item);
		SelectView(item);
	}

	private void _E001(Item item)
	{
		this.m__E000.Remove(item.TemplateId);
		ItemsList.Remove(item);
		DeselectView(item);
	}

	protected override void ClearSelectedList()
	{
		for (int num = ItemsList.Count - 1; num >= 0; num--)
		{
			_E001(ItemsList[num]);
		}
		base.ClearSelectedList();
		_E002();
	}

	private void _E002()
	{
		if (_scroll != null)
		{
			_scroll.verticalNormalizedPosition = 1f;
			_scroll.horizontalNormalizedPosition = 0f;
		}
	}

	public override void Close()
	{
		ClearSelectedList();
		base.Close();
	}
}
