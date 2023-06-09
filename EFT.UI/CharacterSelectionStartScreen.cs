using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class CharacterSelectionStartScreen : MonoBehaviour
{
	public enum PMC
	{
		USEC,
		BEAR
	}

	[SerializeField]
	private Button _acceptButton;

	[SerializeField]
	private Button _backButton;

	[SerializeField]
	private AnimatedToggle _usecButton;

	[SerializeField]
	private AnimatedToggle _bearButton;

	private Action<PMC> m__E000;

	private Action m__E001;

	private PMC m__E002;

	private void Awake()
	{
		_acceptButton.onClick.AddListener(delegate
		{
			this.m__E000(this.m__E002);
		});
		_backButton.onClick.AddListener(delegate
		{
			this.m__E001();
		});
		_usecButton.onValueChanged.AddListener(delegate
		{
			this.m__E002 = PMC.USEC;
		});
		_bearButton.onValueChanged.AddListener(delegate
		{
			this.m__E002 = PMC.BEAR;
		});
	}

	public void Show(Action<PMC> acceptAction, Action backAction)
	{
		this.m__E000 = acceptAction;
		this.m__E001 = backAction;
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	[CompilerGenerated]
	private void _E000()
	{
		this.m__E000(this.m__E002);
	}

	[CompilerGenerated]
	private void _E001()
	{
		this.m__E001();
	}

	[CompilerGenerated]
	private void _E002(bool arg)
	{
		this.m__E002 = PMC.USEC;
	}

	[CompilerGenerated]
	private void _E003(bool arg)
	{
		this.m__E002 = PMC.BEAR;
	}
}
