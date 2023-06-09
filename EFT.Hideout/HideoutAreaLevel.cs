using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.Hideout;

public sealed class HideoutAreaLevel : SerializedMonoBehaviour
{
	[SerializeField]
	private Dictionary<ELightingLevel, LightLevel> _lightingLevels = new Dictionary<ELightingLevel, LightLevel>();

	[SerializeField]
	private AreaLevelAudio _audioAmbiance;

	[SerializeField]
	private ParticleSystem _constructionParticlesTemplate;

	[SerializeField]
	private GameObject _immediateEnable;

	[SerializeField]
	private Transform _highlightTransform;

	[SerializeField]
	private Transform[] _producedObjectsSpawnPoints;

	private Stage m__E000;

	private List<GameObject> _E001 = new List<GameObject>();

	private IReadOnlyCollection<ELightingLevel> _E002 = (IReadOnlyCollection<ELightingLevel>)(object)new ELightingLevel[1];

	public bool CanSetProducedItemsOnPlace
	{
		get
		{
			Transform[] producedObjectsSpawnPoints = _producedObjectsSpawnPoints;
			if (producedObjectsSpawnPoints == null)
			{
				return false;
			}
			return producedObjectsSpawnPoints.Length != 0;
		}
	}

	public Dictionary<ELightingLevel, LightLevel> LightingLevels => _lightingLevels;

	public AreaLevelAudio AudioAmbiance => _audioAmbiance;

	public Transform HighlightTransform
	{
		get
		{
			if (_highlightTransform == null)
			{
				_highlightTransform = base.transform;
			}
			return _highlightTransform;
		}
		set
		{
			_highlightTransform = value;
		}
	}

	public RelatedSounds Sounds => this.m__E000.Sounds;

	public void Init(Stage stage)
	{
		this.m__E000 = stage;
		AudioAmbiance.Init(this.m__E000.Sounds);
	}

	public void Enable(bool immediate)
	{
		base.gameObject.SetActive(value: true);
		_immediateEnable.SmartEnable();
		HighlightTransform.gameObject.SetActive(value: true);
		foreach (Transform item in HighlightTransform)
		{
			item.gameObject.SetActive(value: true);
			if (!immediate)
			{
				_E000(item).HandleExceptions();
			}
		}
	}

	private async Task _E000(Transform arg)
	{
		ParticleSystem particleSystem = _E80C.Pop(_constructionParticlesTemplate, arg);
		particleSystem.gameObject.SetActive(value: true);
		particleSystem.Play();
		await TasksExtensions.Delay(particleSystem.main.duration * 2f);
		particleSystem.gameObject.SetActive(value: false);
		_E80C.Push(particleSystem);
	}

	public void SetAvailableLightingLevels(IReadOnlyCollection<ELightingLevel> availableLevels)
	{
		_E002 = availableLevels;
		ELightingLevel key;
		LightLevel value;
		foreach (KeyValuePair<ELightingLevel, LightLevel> lightingLevel in _lightingLevels)
		{
			_E39D.Deconstruct(lightingLevel, out key, out value);
			ELightingLevel value2 = key;
			LightLevel lightLevel = value;
			if (lightLevel != null && !_E002.Contains(value2))
			{
				lightLevel.gameObject.SetActive(value: false);
			}
		}
		foreach (KeyValuePair<ELightingLevel, LightLevel> lightingLevel2 in _lightingLevels)
		{
			_E39D.Deconstruct(lightingLevel2, out key, out value);
			ELightingLevel value3 = key;
			LightLevel lightLevel2 = value;
			if (lightLevel2 != null && _E002.Contains(value3))
			{
				lightLevel2.gameObject.SetActive(value: true);
			}
		}
	}

	public void SetLightLevel(ELightingLevel level)
	{
		if (!_E002.Contains(level))
		{
			_E815._E000.Instance.LogError(string.Format(_ED3E._E000(171146), level, _E002));
			return;
		}
		if (!_lightingLevels.TryGetValue(level, out var value) || value == null)
		{
			level = ELightingLevel.Off;
		}
		ELightingLevel key;
		LightLevel value2;
		foreach (KeyValuePair<ELightingLevel, LightLevel> lightingLevel in _lightingLevels)
		{
			_E39D.Deconstruct(lightingLevel, out key, out value2);
			ELightingLevel eLightingLevel = key;
			LightLevel lightLevel = value2;
			if (lightLevel != null && eLightingLevel != level)
			{
				lightLevel.Enabled = false;
			}
		}
		foreach (KeyValuePair<ELightingLevel, LightLevel> lightingLevel2 in _lightingLevels)
		{
			_E39D.Deconstruct(lightingLevel2, out key, out value2);
			ELightingLevel eLightingLevel2 = key;
			LightLevel lightLevel2 = value2;
			if (lightLevel2 != null && eLightingLevel2 == level)
			{
				lightLevel2.Enabled = true;
			}
		}
	}

	public void SetItemsOnPlace(IEnumerable<GameObject> dynamicObjects)
	{
		_E001.Clear();
		if (dynamicObjects == null || _producedObjectsSpawnPoints == null || _producedObjectsSpawnPoints.Length == 0)
		{
			return;
		}
		int num = 0;
		foreach (GameObject dynamicObject in dynamicObjects)
		{
			Transform transform = _producedObjectsSpawnPoints[num % _producedObjectsSpawnPoints.Length];
			dynamicObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
			num++;
		}
	}

	public void Disable()
	{
		AudioAmbiance.Pause();
		_immediateEnable.SmartDisable();
		HighlightTransform.gameObject.SmartDisable();
		_E001.Clear();
		base.gameObject.SetActive(value: false);
	}
}
