using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponTestController : MonoBehaviour
{
	public Animator Animator;

	public bool Active;

	public float AmmoInChamber = 1f;

	public float AmmoInMag = 30f;

	public bool MagInWeapon = true;

	public bool Inventory;

	public float Idle = 1f;

	public bool Fire;

	public int FireModes = 2;

	private float FireMode;

	private int MagTypeOld;

	public int MagTypes = 3;

	private int MagTypeNew;

	private bool inv;

	public float IdleTime;

	public float IdleTimeMax = 30f;

	public float IdleRandomValue;

	public float IdleRandomMin;

	public float IdleRandomMax = 3f;

	public List<int> AltFireVars = new List<int>();

	public int CurrentAltFire;

	protected Dictionary<string, int> LayerIntDict = new Dictionary<string, int>();

	private void Start()
	{
		for (int i = 0; i < 10; i++)
		{
			try
			{
				string layerName = Animator.GetLayerName(i);
				if (string.IsNullOrEmpty(layerName))
				{
					break;
				}
				LayerIntDict.Add(layerName, i);
			}
			catch (Exception)
			{
			}
		}
	}

	private int _E000(string name)
	{
		int value = 0;
		LayerIntDict.TryGetValue(name, out value);
		return value;
	}

	private void OnEnable()
	{
		IdleTime = 0f;
	}

	public void FiringBullet()
	{
		Debug.Log(_ED3E._E000(46658));
		StartCoroutine(_E001(_ED3E._E000(50481), 0.01f));
		Animator.SetFloat(_ED3E._E000(49473), Animator.GetFloat(_ED3E._E000(49473)) + 1f);
	}

	public void DelAmmoChamber()
	{
		Debug.Log(_ED3E._E000(46706));
		StartCoroutine(_E001(_ED3E._E000(50460), 0.01f));
		Animator.SetFloat(_ED3E._E000(49291), Animator.GetFloat(_ED3E._E000(49291)) - 1f);
	}

	public void ShellEject()
	{
		Debug.Log(_ED3E._E000(46741));
		StartCoroutine(_E001(_ED3E._E000(50657), 0.01f));
	}

	public void DelAmmoFromMag()
	{
		Debug.Log(_ED3E._E000(46724));
		StartCoroutine(_E001(_ED3E._E000(50451), 0.01f));
		Animator.SetFloat(_ED3E._E000(49337), Animator.GetFloat(_ED3E._E000(49337)) - 1f);
	}

	public void AddAmmoInChamber()
	{
		Debug.Log(_ED3E._E000(46771));
		StartCoroutine(_E001(_ED3E._E000(50373), 0.01f));
		Animator.SetFloat(_ED3E._E000(49291), Animator.GetFloat(_ED3E._E000(49291)) + 1f);
	}

	public void OnBoltCatch()
	{
		Debug.Log(_ED3E._E000(46810));
		StartCoroutine(_E001(_ED3E._E000(50644), 0.1f));
		Animator.SetBool(_ED3E._E000(50979), value: true);
	}

	public void OffBoltCatch()
	{
		Debug.Log(_ED3E._E000(46784));
		StartCoroutine(_E001(_ED3E._E000(50599), 0.1f));
		Animator.SetBool(_ED3E._E000(50979), value: false);
	}

	public void FiremodeSwitch()
	{
		Debug.Log(_ED3E._E000(46829));
		StartCoroutine(_E001(_ED3E._E000(51174), 0.01f));
		FireMode = ((int)FireMode + 1) % FireModes;
		Animator.SetFloat(_ED3E._E000(46875), FireMode);
	}

	public void MagOut()
	{
		Debug.Log(_ED3E._E000(46860));
		StartCoroutine(_E001(_ED3E._E000(50586), 0.01f));
		Animator.SetFloat(_ED3E._E000(49337), 0f);
		Animator.SetBool(_ED3E._E000(51165), value: false);
	}

	public void MagIn()
	{
		Debug.Log(_ED3E._E000(46910));
		StartCoroutine(_E001(_ED3E._E000(50588), 0.01f));
		Animator.SetFloat(_ED3E._E000(49337), 10f);
		Animator.SetBool(_ED3E._E000(51165), value: true);
	}

	public void Arm()
	{
		Debug.Log(_ED3E._E000(46889));
		StartCoroutine(_E001(_ED3E._E000(50411), 0.01f));
		Animator.SetBool(_ED3E._E000(50996), value: true);
	}

	public void Disarm()
	{
		Debug.Log(_ED3E._E000(46943));
		StartCoroutine(_E001(_ED3E._E000(50434), 0.01f));
		Animator.SetBool(_ED3E._E000(50996), value: false);
	}

	public void MisfireOff()
	{
		Debug.Log(_ED3E._E000(46920));
		StartCoroutine(_E001(_ED3E._E000(46960), 0.01f));
		Animator.SetBool(_ED3E._E000(46955), value: false);
	}

	public void AddAmmoInMag()
	{
		Debug.Log(_ED3E._E000(46947));
		StartCoroutine(_E001(_ED3E._E000(50422), 0.01f));
		Animator.SetFloat(_ED3E._E000(49337), Animator.GetFloat(_ED3E._E000(49337)) + 1f);
	}

	public void MagSwapEnd()
	{
		Debug.Log(_ED3E._E000(46987));
		StartCoroutine(_E001(_ED3E._E000(49994), 0.01f));
		Animator.SetBool(_ED3E._E000(51153), value: false);
	}

	public void RemoveShell()
	{
		Debug.Log(_ED3E._E000(47035));
		Animator.SetFloat(_ED3E._E000(49473), Animator.GetFloat(_ED3E._E000(49473)) - 1f);
	}

	public void NullShell()
	{
		Debug.Log(_ED3E._E000(47018));
		Animator.SetFloat(_ED3E._E000(49473), 0f);
	}

	private IEnumerator _E001(string name, float time)
	{
		Animator.SetBool(name, value: true);
		yield return new WaitForSeconds(time);
		Animator.SetBool(name, value: false);
	}

	private void Update()
	{
		if (Animator.GetBool(_ED3E._E000(50979)))
		{
			Animator.SetLayerWeight(2, 1f);
		}
		else
		{
			Animator.SetLayerWeight(2, 0f);
		}
		if (!Animator.GetBool(_ED3E._E000(50996)))
		{
			Animator.SetLayerWeight(_E000(_ED3E._E000(47071)), 1f);
		}
		else
		{
			Animator.SetLayerWeight(_E000(_ED3E._E000(47071)), 0f);
		}
		bool num = FireMode == 0f;
		bool flag = false;
		if ((!num) ? Input.GetKeyDown(KeyCode.Mouse0) : Input.GetKey(KeyCode.Mouse0))
		{
			Animator.SetBool(_ED3E._E000(51066), value: true);
		}
		else
		{
			Animator.SetBool(_ED3E._E000(51066), value: false);
		}
		if (Input.GetKey(KeyCode.Keypad7))
		{
			Animator.SetBool(_ED3E._E000(49959), value: true);
		}
		else
		{
			Animator.SetBool(_ED3E._E000(49959), value: false);
		}
		if (Input.GetKey(KeyCode.Keypad8))
		{
			Animator.SetBool(_ED3E._E000(50004), value: true);
		}
		else
		{
			Animator.SetBool(_ED3E._E000(50004), value: false);
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (AmmoInChamber > 0f)
			{
				Animator.SetBool(_ED3E._E000(46955), value: true);
			}
		}
		else
		{
			Animator.SetBool(_ED3E._E000(46955), value: false);
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			FiremodeSwitch();
		}
		Animator.SetBool(_ED3E._E000(50029), Input.GetKeyDown(KeyCode.R));
		Animator.SetBool(_ED3E._E000(49949), Input.GetKeyDown(KeyCode.X));
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			MagTypeNew = (MagTypeNew + 1) % MagTypes;
			Animator.SetInteger(_ED3E._E000(47062), MagTypeNew);
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			MagTypeOld = (MagTypeOld + 1) % MagTypes;
			Animator.SetInteger(_ED3E._E000(47049), MagTypeOld);
		}
		Animator.SetBool(_ED3E._E000(50020), Input.GetKeyDown(KeyCode.T));
		if (Input.GetKeyDown(KeyCode.Y))
		{
			Animator.SetBool(_ED3E._E000(51117), value: true);
		}
		else
		{
			Animator.SetBool(_ED3E._E000(51117), value: false);
		}
		Animator.SetBool(_ED3E._E000(49962), Input.GetKeyDown(KeyCode.L));
		Animator.SetBool(_ED3E._E000(51095), inv);
		if (Input.GetKeyDown(KeyCode.I))
		{
			inv = !inv;
		}
		Animator.SetBool(_ED3E._E000(49920), Input.GetKeyDown(KeyCode.B));
		Animator.SetBool(_ED3E._E000(51199), Input.GetKeyDown(KeyCode.V));
		if (Input.GetKey(KeyCode.Mouse1))
		{
			Debug.Log(_ED3E._E000(47100));
			CurrentAltFire++;
			if (CurrentAltFire < 0)
			{
				CurrentAltFire = 0;
			}
			if (CurrentAltFire >= AltFireVars.Count)
			{
				CurrentAltFire = 0;
			}
			if (AltFireVars.Count > 0)
			{
				Animator.SetBool(_ED3E._E000(51004), value: true);
				Animator.SetInteger(_ED3E._E000(47099), AltFireVars[CurrentAltFire]);
			}
		}
		else
		{
			Animator.SetBool(_ED3E._E000(51004), value: false);
		}
		AnimatorStateInfo currentAnimatorStateInfo = Animator.GetCurrentAnimatorStateInfo(1);
		IdleTime += Time.deltaTime;
		if (!currentAnimatorStateInfo.IsName(_ED3E._E000(52028)))
		{
			IdleTime = 0f;
		}
		if (IdleTime > IdleTimeMax)
		{
			IdleTime = 0f;
			IdleRandomValue = Mathf.RoundToInt(UnityEngine.Random.Range(IdleRandomMin, IdleRandomMax + 1f));
			Animator.SetFloat(_ED3E._E000(49418), IdleRandomValue);
			Animator.SetTrigger(_ED3E._E000(49969));
		}
		Animator.SetBool(_ED3E._E000(51109), (int)Animator.GetFloat(_ED3E._E000(49337)) >= Animator.GetInteger(_ED3E._E000(47086)));
		if (Math.Abs(Input.mouseScrollDelta.y) >= float.Epsilon)
		{
			Animator.SetBool(_ED3E._E000(50949), !Animator.GetBool(_ED3E._E000(50949)));
		}
	}
}
