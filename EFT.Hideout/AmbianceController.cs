using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.Hideout;

public sealed class AmbianceController : SerializedMonoBehaviour
{
	[SerializeField]
	private List<_E809> _ambianceObjects;

	[SerializeField]
	private HideoutAudioBackground _audioBackground;

	[SerializeField]
	private MultiFlare _multiFlare;

	private IEnumerable<AreaData> m__E000;

	[CompilerGenerated]
	private ELightStatus m__E001;

	public ELightStatus GlobalLightStatus
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E001 = value;
		}
	}

	private void Awake()
	{
		if (_multiFlare != null)
		{
			_multiFlare.RemoveAllLights();
		}
	}

	public void Show(IEnumerable<AreaData> datas, bool isEnergyOn)
	{
		this.m__E000 = datas;
		EnergySupplyChanged(isEnergyOn);
		Dictionary<EAreaType, AreaTemplate> dictionary = new Dictionary<EAreaType, AreaTemplate>();
		foreach (AreaData item in this.m__E000)
		{
			_audioBackground.InitArea(item);
			dictionary.Add(item.Template.Type, item.Template);
		}
		foreach (_E809 ambianceObject in _ambianceObjects)
		{
			if (ambianceObject != null)
			{
				EAreaType areaType = ambianceObject.AreaType;
				if (areaType != EAreaType.NotSet && dictionary.ContainsKey(areaType))
				{
					ambianceObject.Init(dictionary[areaType]);
				}
			}
		}
		_audioBackground.Play();
	}

	public void EnergySupplyChanged(bool isOn)
	{
		_E000(isOn ? ELightStatus.Working : ELightStatus.OutOfFuel).HandleExceptions();
	}

	private async Task _E000(ELightStatus status)
	{
		if (this.m__E000 != null && GlobalLightStatus != status)
		{
			GlobalLightStatus = status;
			await _E001();
		}
	}

	private async Task _E001()
	{
		List<Task<bool>> list = new List<Task<bool>>(_ambianceObjects.Count);
		foreach (_E809 ambianceObject in _ambianceObjects)
		{
			if (ambianceObject != null)
			{
				list.Add(ambianceObject.PerformInteraction(GlobalLightStatus));
			}
		}
		await Task.WhenAll(list);
	}
}
