using EFT.Interactive;
using UnityEngine;

namespace EFT.BufferZone;

public class BufferZoneContainer : MonoBehaviour
{
	[SerializeField]
	private BufferGates _gates;

	[SerializeField]
	private BufferInnerZone _innerZone;

	[SerializeField]
	private BufferOuterBattleZone _outerBattleZone;

	[SerializeField]
	private BufferGateSwitcher _innerSwitcher;

	[SerializeField]
	private BufferGateSwitcher _outerSwitcher;

	public BufferGates Gates => _gates;

	public BufferInnerZone InnerZone => _innerZone;

	public BufferOuterBattleZone BattleOuterZone => _outerBattleZone;

	public BufferGateSwitcher InnerSwitcher => _innerSwitcher;

	public BufferGateSwitcher OuterSwitcher => _outerSwitcher;

	private void Awake()
	{
		_innerZone.SetUpReferences(this);
		_innerSwitcher.SetUpReferences(this);
		_outerSwitcher.SetUpReferences(this);
	}

	private void Start()
	{
		if (_EBEB.Instance != null)
		{
			_E000();
		}
		else
		{
			_EBEB.OnInitialized += _E000;
		}
	}

	private void OnDestroy()
	{
		_EBEB.OnInitialized -= _E000;
	}

	private void _E000()
	{
		_EBEB.OnInitialized -= _E000;
		_EBEB.Instance.RegisterContainer(this);
	}
}
