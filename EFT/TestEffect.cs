using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT;

public class TestEffect : MonoBehaviour
{
	public static bool IsOfflineMode;

	public _E97E PlayerHealth;

	public Transform AreasParent;

	public Transform ButtonParent;

	public Transform ShootArea;

	public Transform BloodlossArea;

	public Transform BreackPartArea;

	public Transform ContusionArea;

	public Transform ShootOnArmorArea;

	public Transform SandInEyeArea;

	public Transform DisorientArea;

	public Transform StunArea;

	public Transform FlashArea;

	public Transform HydraTionArea;

	public Transform EnergyArea;

	public Transform HydrationSettings;

	public Transform EnergySettings;

	public Transform ToxicationArea;

	public Transform RadiationArea;

	public Transform HealingArea;

	public Transform CureBreackPartArea;

	public GameObject _currentEffectsText;

	public GameObject _destroyebleEffectsText;

	public GameObject _innerEffectsText;

	public GameObject Hydration;

	public GameObject Energy;

	public GameObject Head;

	public GameObject Breast;

	public GameObject Stomatch;

	public GameObject RightHand;

	public GameObject LeftHand;

	public GameObject RightLeg;

	public GameObject LeftLeg;

	public GameObject UseSpeed;

	public GameObject LootSpeed;

	public GameObject Precision;

	public GameObject WeaponGetSpeed;

	public GameObject ReloadSpeed;

	public GameObject Sense;

	public GameObject WalkSpeed;

	public GameObject WorkingState;

	public Slider _bloodlossHeadSlider;

	public Slider _dehydrationSlider;

	public Slider _exhaustionSlider;

	public Slider _breackPartSlider;

	[HideInInspector]
	public Dictionary<_E992, float> _bloodlossEffectsDict = new Dictionary<_E992, float>();

	[HideInInspector]
	public Dictionary<EBodyPart, Slider> _bodyPartSlidersDict = new Dictionary<EBodyPart, Slider>();

	private Dictionary<EBodyPart, List<_E992>> m__E000;

	public float GetBloodlossValue()
	{
		return _bloodlossHeadSlider.normalizedValue;
	}

	public float GetDehydrationValue()
	{
		return _dehydrationSlider.normalizedValue;
	}

	public float GetExhausionValue()
	{
		return _exhaustionSlider.normalizedValue;
	}

	public float GetBreackPartValue()
	{
		return _breackPartSlider.normalizedValue;
	}

	private void Start()
	{
		Player player = null;
		player = _E3AA.FindUnityObjectsOfType<ClientPlayer>().First();
		PlayerHealth = player.ActiveHealthController;
		_bodyPartSlidersDict.Add(EBodyPart.Head, _bloodlossHeadSlider);
	}

	private void OnEnable()
	{
		Player player = null;
		Player[] array = _E3AA.FindUnityObjectsOfType<Player>();
		foreach (Player player2 in array)
		{
			if (player2.GetComponent<BotOwner>() == null)
			{
				player = player2;
			}
		}
		if (player != null)
		{
			PlayerHealth = player.ActiveHealthController;
		}
	}

	public void CloseAllButThis(Transform trans)
	{
		Transform[] array = new Transform[AreasParent.childCount];
		for (int i = 0; i < AreasParent.childCount; i++)
		{
			array[i] = AreasParent.GetChild(i);
		}
		Transform[] array2 = array;
		foreach (Transform transform in array2)
		{
			if (transform != trans && transform != ButtonParent)
			{
				transform.gameObject.SetActive(value: false);
			}
		}
	}

	public void Shoot()
	{
		TextMeshProUGUI[] componentsInChildren = ShootArea.GetComponentsInChildren<TextMeshProUGUI>();
		List<string> list = new List<string>();
		EBodyPart bodyPart = EBodyPart.Head;
		TextMeshProUGUI[] array = componentsInChildren;
		foreach (TextMeshProUGUI textMeshProUGUI in array)
		{
			if (textMeshProUGUI.transform.parent.GetComponent<Toggle>() != null && textMeshProUGUI.transform.parent.GetComponent<Toggle>().isOn)
			{
				list.Add(textMeshProUGUI.text);
			}
		}
		foreach (string item in list)
		{
			switch (_ED3C._E000(item))
			{
			case 2996251363u:
				if (item == _ED3E._E000(48545))
				{
					bodyPart = EBodyPart.Head;
				}
				break;
			case 3646599052u:
				if (item == _ED3E._E000(189271))
				{
					bodyPart = EBodyPart.Chest;
				}
				break;
			case 4139903152u:
				if (item == _ED3E._E000(189262))
				{
					bodyPart = EBodyPart.Stomach;
				}
				break;
			case 787944353u:
				if (item == _ED3E._E000(189255))
				{
					bodyPart = EBodyPart.LeftArm;
				}
				break;
			case 1634889654u:
				if (item == _ED3E._E000(189305))
				{
					bodyPart = EBodyPart.RightArm;
				}
				break;
			case 129641964u:
				if (item == _ED3E._E000(189292))
				{
					bodyPart = EBodyPart.LeftLeg;
				}
				break;
			case 1300964345u:
				if (item == _ED3E._E000(189285))
				{
					bodyPart = EBodyPart.RightLeg;
				}
				break;
			}
			int num = 0;
			try
			{
				num = int.Parse(ShootArea.GetComponentInChildren<TMP_InputField>().text);
			}
			catch (Exception)
			{
				Debug.Log(_ED3E._E000(189343));
			}
			PlayerHealth.ApplyDamage(bodyPart, num, _E98A.UndefinedDamage);
		}
	}

	public void AddBloodloss()
	{
		TextMeshProUGUI[] componentsInChildren = BloodlossArea.GetComponentsInChildren<TextMeshProUGUI>();
		List<string> list = new List<string>();
		TextMeshProUGUI[] array = componentsInChildren;
		foreach (TextMeshProUGUI textMeshProUGUI in array)
		{
			if (textMeshProUGUI.transform.parent.GetComponent<Toggle>() != null && textMeshProUGUI.transform.parent.GetComponent<Toggle>().isOn)
			{
				list.Add(textMeshProUGUI.text);
			}
		}
	}

	public void Fracture()
	{
		TextMeshProUGUI[] componentsInChildren = BreackPartArea.GetComponentsInChildren<TextMeshProUGUI>();
		string text = "";
		EBodyPart eBodyPart = EBodyPart.LeftArm;
		TextMeshProUGUI[] array = componentsInChildren;
		foreach (TextMeshProUGUI textMeshProUGUI in array)
		{
			if (textMeshProUGUI.transform.parent.GetComponent<Toggle>() != null && textMeshProUGUI.transform.parent.GetComponent<Toggle>().isOn)
			{
				text = textMeshProUGUI.text;
			}
		}
		if (!(text == _ED3E._E000(189255)))
		{
			if (!(text == _ED3E._E000(189305)))
			{
				if (!(text == _ED3E._E000(189292)))
				{
					if (text == _ED3E._E000(189285))
					{
						eBodyPart = EBodyPart.RightLeg;
						PlayerHealth.DoFracture(eBodyPart);
					}
				}
				else
				{
					eBodyPart = EBodyPart.LeftLeg;
					PlayerHealth.DoFracture(eBodyPart);
				}
			}
			else
			{
				eBodyPart = EBodyPart.RightArm;
				PlayerHealth.DoFracture(eBodyPart);
			}
		}
		else
		{
			eBodyPart = EBodyPart.LeftArm;
			PlayerHealth.DoFracture(eBodyPart);
		}
	}

	public void AddContusion()
	{
		int num = 0;
		try
		{
			num = int.Parse(ContusionArea.GetComponentInChildren<TMP_InputField>().text);
		}
		catch (Exception)
		{
			Debug.Log(_ED3E._E000(189318));
		}
		Debug.Log(PlayerHealth.DamageCoeff);
		PlayerHealth.DoContusion(num, 1f);
	}

	public void PainOnOff()
	{
	}

	public void ShootOnArmor()
	{
		int num = 0;
		try
		{
			num = int.Parse(ShootOnArmorArea.GetComponentInChildren<TMP_InputField>().text);
		}
		catch (Exception)
		{
			Debug.Log(_ED3E._E000(189343));
		}
		PlayerHealth.DoShootOnArmor(num);
	}

	public void SandInEye()
	{
		int num = 0;
		try
		{
			num = int.Parse(SandInEyeArea.GetComponentInChildren<TMP_InputField>().text);
		}
		catch (Exception)
		{
			Debug.Log(_ED3E._E000(189318));
		}
		PlayerHealth.DoSandingScreen(num);
	}

	public void Disorient()
	{
		int num = 0;
		try
		{
			num = int.Parse(DisorientArea.GetComponentInChildren<TMP_InputField>().text);
		}
		catch (Exception)
		{
			Debug.Log(_ED3E._E000(189318));
		}
		PlayerHealth.DoDisorientation(num);
	}

	public void Intoxication()
	{
		if (ToxicationArea.GetComponentInChildren<Toggle>().isOn)
		{
			PlayerHealth.DoIntoxication();
		}
	}

	public void Radiation()
	{
		if (RadiationArea.GetComponentInChildren<Toggle>().isOn)
		{
			PlayerHealth.DoRadExposure();
		}
	}

	public void SetHydration()
	{
		if (HydraTionArea.GetComponentInChildren<Toggle>().isOn)
		{
			PlayerHealth.ChangeHydration(-100f);
		}
		else
		{
			PlayerHealth.ChangeHydration(100f);
		}
	}

	public void SetEnergySettings()
	{
	}

	public void Healing()
	{
	}

	public void Load()
	{
		_E000();
		StreamReader streamReader = new StreamReader(_ED3E._E000(189363));
		PlayerHealth = streamReader.ReadToEnd().ParseJsonTo<_E97E>(Array.Empty<JsonConverter>());
	}

	private bool _E000()
	{
		string text = "";
		TextMeshProUGUI[] componentsInChildren = WorkingState.GetComponentsInChildren<TextMeshProUGUI>();
		foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
		{
			if (textMeshProUGUI.transform.parent.GetComponent<Toggle>() != null && textMeshProUGUI.transform.parent.GetComponent<Toggle>().isOn)
			{
				text = textMeshProUGUI.text.ToString();
			}
		}
		if (!(text == _ED3E._E000(189407)))
		{
			if (text == _ED3E._E000(189398))
			{
				IsOfflineMode = true;
			}
		}
		else
		{
			IsOfflineMode = false;
		}
		return IsOfflineMode;
	}

	private void Update()
	{
	}
}
