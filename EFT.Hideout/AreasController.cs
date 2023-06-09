using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.Hideout;

public sealed class AreasController : SerializedMonoBehaviour, _E813, IEventSystemHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E815 hideout;

		public AreasController _003C_003E4__this;

		internal void _E000()
		{
			hideout.OnInited -= _003C_003E4__this._E002;
			_003C_003E4__this.m__E001.OnEnergyGenerationChanged -= _003C_003E4__this._E003;
			_003C_003E4__this.m__E000.OnLightingLevelChanged -= _003C_003E4__this._E000;
			_003C_003E4__this.m__E000 = null;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public AreaData data;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			ELightingLevel value = _E3A5<ELightingLevel>.GetValue(data.CurrentLevel);
			CS_0024_003C_003E8__locals1._003C_003E4__this._E001(value, switchToNewLevel: true);
		}

		internal void _E001()
		{
			if (data.CurrentLevel > 0)
			{
				CS_0024_003C_003E8__locals1._003C_003E4__this._E001(ELightingLevel.ChristmasLights, switchToNewLevel: true);
			}
		}

		internal void _E002(bool value)
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this._E005(data, value);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _E831 behaviour;

		internal void _E000()
		{
			if (behaviour != null)
			{
				behaviour.Controller = null;
			}
		}
	}

	public static readonly IReadOnlyCollection<ELightingLevel> LightingPriority = (IReadOnlyCollection<ELightingLevel>)(object)new ELightingLevel[6]
	{
		ELightingLevel.FluorescentLamps,
		ELightingLevel.LightBulbs,
		ELightingLevel.Candles,
		ELightingLevel.ChristmasLights,
		ELightingLevel.HalloweenLights,
		ELightingLevel.Off
	};

	[SerializeField]
	private HideoutCameraController _hideoutCameraController;

	[SerializeField]
	private AmbianceController _ambianceController;

	[SerializeField]
	private Dictionary<ELightingLevel, GameObject> _globalLighting = new Dictionary<ELightingLevel, GameObject>();

	[SerializeField]
	private Dictionary<EAreaType, HideoutArea> _areas;

	private HideoutScreenOverlay m__E000;

	private _E80F m__E001;

	private _E3A4 m__E002 = new _E3A4();

	private readonly HashSet<ELightingLevel> m__E003 = new HashSet<ELightingLevel>();

	private ELightingLevel m__E004;

	private IEnumerable<ELightingLevel> _E005 => LightingPriority.Where((ELightingLevel level) => this.m__E003.Contains(level));

	public Dictionary<EAreaType, HideoutArea> Areas => _areas;

	private void Awake()
	{
		foreach (KeyValuePair<EAreaType, HideoutArea> area in Areas)
		{
			_E39D.Deconstruct(area, out var _, out var value);
			HideoutAreaLevel[] areaLevels = value.AreaLevels;
			for (int i = 0; i < areaLevels.Length; i++)
			{
				areaLevels[i].Disable();
			}
		}
		if (Singleton<CommonUI>.Instantiated)
		{
			this.m__E000 = Singleton<CommonUI>.Instance.HideoutScreenOverlay;
			Singleton<CommonUI>.Instance.HideoutScreenRear.HideoutCameraController = _hideoutCameraController;
		}
	}

	public void HideoutAwake()
	{
		using (_E069.StartWithToken(_ED3E._E000(171039)))
		{
			_E815 hideout = Singleton<_E815>.Instance;
			List<AreaData> areaDatas = hideout.AreaDatas;
			this.m__E001 = hideout.EnergyController;
			this.m__E000.ClearLightLevels();
			hideout.OnInited += _E002;
			this.m__E001.OnEnergyGenerationChanged += _E003;
			this.m__E000.OnLightingLevelChanged += _E000;
			this.m__E002.AddDisposable(delegate
			{
				hideout.OnInited -= _E002;
				this.m__E001.OnEnergyGenerationChanged -= _E003;
				this.m__E000.OnLightingLevelChanged -= _E000;
				this.m__E000 = null;
			});
			_E001(ELightingLevel.Off, switchToNewLevel: false);
			if (hideout.Events.Contains(EEventType.HalloweenIllumination))
			{
				_E001(ELightingLevel.HalloweenLights, switchToNewLevel: false);
			}
			foreach (AreaData data in areaDatas)
			{
				EAreaType type = data.Template.Type;
				Areas[type].Data = data;
				if (!data.Enabled)
				{
					continue;
				}
				switch (type)
				{
				case EAreaType.Illumination:
				{
					for (int i = 1; i <= data.CurrentLevel; i++)
					{
						ELightingLevel value2 = _E3A5<ELightingLevel>.GetValue(i);
						_E001(value2, switchToNewLevel: false);
					}
					this.m__E002.AddDisposable(data.LevelUpdated.Subscribe(delegate
					{
						ELightingLevel value4 = _E3A5<ELightingLevel>.GetValue(data.CurrentLevel);
						_E001(value4, switchToNewLevel: true);
					}));
					break;
				}
				case EAreaType.ChristmasIllumination:
					if (!data.Enabled)
					{
						break;
					}
					if (data.CurrentLevel > 0)
					{
						_E001(ELightingLevel.ChristmasLights, switchToNewLevel: false);
						break;
					}
					this.m__E002.AddDisposable(data.LevelUpdated.Subscribe(delegate
					{
						if (data.CurrentLevel > 0)
						{
							_E001(ELightingLevel.ChristmasLights, switchToNewLevel: true);
						}
					}));
					break;
				case EAreaType.Generator:
					this.m__E001.SetSwitchedStatus(data.IsActive);
					break;
				}
				_E831 behaviour = data.Template.AreaBehaviour;
				if (behaviour == null)
				{
					throw new Exception(_ED3E._E000(171012) + data.Template.Name);
				}
				behaviour.Controller = this;
				this.m__E002.AddDisposable(data.OnHover.Subscribe(delegate(bool value)
				{
					_E005(data, value);
				}));
				this.m__E002.AddDisposable(delegate
				{
					if (behaviour != null)
					{
						behaviour.Controller = null;
					}
				});
			}
			List<AreaData> list = new List<AreaData>();
			foreach (KeyValuePair<EAreaType, HideoutArea> area in Areas)
			{
				_E39D.Deconstruct(area, out var _, out var value3);
				HideoutArea hideoutArea = value3;
				AreaData data2 = hideoutArea.Data;
				if (data2 != null && data2.Enabled)
				{
					list.Add(hideoutArea.Data);
				}
			}
			if (hideout.Events.Contains(EEventType.HalloweenIllumination))
			{
				this.m__E003.Add(ELightingLevel.HalloweenLights);
			}
			ELightingLevel eLightingLevel = _E838.GlobalLightingLevel;
			if (!this.m__E003.Contains(eLightingLevel))
			{
				eLightingLevel = this._E005.First();
			}
			_ambianceController.Show(list, this.m__E001.IsEnergyGenerationOn);
			_E000(eLightingLevel);
		}
	}

	private void _E000(ELightingLevel level)
	{
		if (level == this.m__E004)
		{
			return;
		}
		this.m__E004 = level;
		_E838.GlobalLightingLevel = level;
		_E004();
		foreach (KeyValuePair<EAreaType, HideoutArea> area in Areas)
		{
			_E39D.Deconstruct(area, out var _, out var value);
			value.SetLightingLevel(this.m__E004);
		}
		if (this.m__E000 != null)
		{
			this.m__E000.SetCurrentLightingLevel(level);
		}
		ELightingLevel key2;
		GameObject value2;
		foreach (KeyValuePair<ELightingLevel, GameObject> item in _globalLighting)
		{
			_E39D.Deconstruct(item, out key2, out value2);
			ELightingLevel num = key2;
			GameObject gameObject = value2;
			if (num != level)
			{
				gameObject.SetActive(value: false);
			}
		}
		foreach (KeyValuePair<ELightingLevel, GameObject> item2 in _globalLighting)
		{
			_E39D.Deconstruct(item2, out key2, out value2);
			ELightingLevel num2 = key2;
			GameObject gameObject2 = value2;
			if (num2 == level)
			{
				gameObject2.SetActive(value: true);
			}
		}
	}

	private void _E001(ELightingLevel level, bool switchToNewLevel)
	{
		if (!Enum.IsDefined(typeof(ELightingLevel), level) || this.m__E003.Contains(level))
		{
			return;
		}
		this.m__E003.Add(level);
		foreach (KeyValuePair<EAreaType, HideoutArea> area in Areas)
		{
			_E39D.Deconstruct(area, out var _, out var value);
			value.EnableIlluminationLevels(this.m__E003);
		}
		if (this.m__E000 != null)
		{
			this.m__E000.AddLightingLevel(level, switchToNewLevel);
		}
	}

	private void _E002()
	{
		_E003(this.m__E001.IsEnergyGenerationOn);
	}

	public void Select(AreaData data, bool wait)
	{
		if (this.m__E000 != null)
		{
			this.m__E000.AreaSelected(data, wait).HandleExceptions();
		}
	}

	private void _E003(bool value)
	{
		_ambianceController.EnergySupplyChanged(value);
		_E004();
	}

	private void _E004()
	{
		_hideoutCameraController.SurroundingIllumination = this.m__E004 != ELightingLevel.Candles && this.m__E004 != ELightingLevel.HalloweenLights && _ambianceController.GlobalLightStatus == ELightStatus.Working;
	}

	private void _E005(AreaData data, bool value)
	{
		HighLightMesh meshHighlighter = _E8A8.Instance.MeshHighlighter;
		if (!(meshHighlighter == null))
		{
			if (value)
			{
				meshHighlighter.Color = data.HighlightColor;
			}
			meshHighlighter.Target = (value ? data.HighlightTransform : null);
			meshHighlighter.enabled = value;
		}
	}

	private void OnDestroy()
	{
		this.m__E002.Dispose();
	}

	[CompilerGenerated]
	private bool _E006(ELightingLevel level)
	{
		return this.m__E003.Contains(level);
	}
}
