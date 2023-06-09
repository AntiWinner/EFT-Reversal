using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GPUInstancer;

public class DetailDemoSceneController : MonoBehaviour
{
	private enum CameraModes
	{
		FPMode,
		SpaceshipMode,
		MowerMode
	}

	private enum QualityMode
	{
		Low,
		Mid,
		High
	}

	public GameObject fpController;

	public GameObject spaceshipCamera;

	public GameObject grassMowerCamera;

	public GPUInstancerDetailManager detailManager;

	public bool persistRemoval;

	private GameObject m__E000;

	private GameObject m__E001;

	private GameObject m__E002;

	private GameObject m__E003;

	private Text m__E004;

	private Transform _E005;

	private Transform _E006;

	private GameObject _E007;

	private CameraModes _E008;

	private ParticleSystem _E009;

	private QualityMode _E00A = QualityMode.High;

	private List<int[,]> _E00B;

	private void Awake()
	{
		this.m__E000 = GameObject.Find(_ED3E._E000(66300));
		this.m__E001 = GameObject.Find(_ED3E._E000(74375));
		this.m__E002 = GameObject.Find(_ED3E._E000(74414));
		this.m__E003 = GameObject.Find(_ED3E._E000(74454));
		this.m__E004 = GameObject.Find(_ED3E._E000(74439)).GetComponent<Text>();
		this.m__E004.text = string.Concat(_ED3E._E000(74474), _E00A, _ED3E._E000(74513));
		_E005 = Object.FindObjectOfType<SpaceshipController>().transform;
		_E009 = _E005.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
		_E006 = Object.FindObjectOfType<GrassMowerController>().transform;
		_E001(CameraModes.FPMode);
		_E004(_E00A);
		_E4BD.StartListeningGPUIEvent(GPUInstancerEventType.DetailInitializationFinished, _E002);
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.C))
		{
			_E000();
		}
		if (Input.GetKeyUp(KeyCode.U))
		{
			this.m__E000.gameObject.SetActive(!this.m__E000.gameObject.activeSelf);
		}
		if (Input.GetKeyUp(KeyCode.F1))
		{
			if (detailManager.gameObject.activeSelf && persistRemoval)
			{
				_E00B = _E4BD.GetDetailMapData(detailManager);
			}
			detailManager.gameObject.SetActive(!detailManager.gameObject.activeSelf);
			if (detailManager.gameObject.activeSelf)
			{
				if (persistRemoval && _E00B != null)
				{
					_E4BD.SetDetailMapData(detailManager, _E00B);
				}
				this.m__E003.SetActive(value: true);
				_E4BD.SetCamera(_E007.GetComponentInChildren<Camera>());
				_E4BD.StartListeningGPUIEvent(GPUInstancerEventType.DetailInitializationFinished, _E002);
			}
			_E003(_E00A);
		}
		if (Input.GetKeyUp(KeyCode.F2))
		{
			_E003(QualityMode.Low);
		}
		if (Input.GetKeyUp(KeyCode.F3))
		{
			_E003(QualityMode.Mid);
		}
		if (Input.GetKeyUp(KeyCode.F4))
		{
			_E003(QualityMode.High);
		}
	}

	private void _E000()
	{
		_E001((CameraModes)((int)(_E008 + 1) % 3));
	}

	private void _E001(CameraModes cameraMode)
	{
		if (!fpController || !spaceshipCamera || !grassMowerCamera)
		{
			Debug.Log(_ED3E._E000(74506));
			return;
		}
		fpController.SetActive(value: false);
		spaceshipCamera.SetActive(value: false);
		grassMowerCamera.SetActive(value: false);
		_E005.GetComponent<SpaceshipController>().enabled = false;
		_E009.gameObject.SetActive(value: false);
		this.m__E001.gameObject.SetActive(value: false);
		_E006.GetComponent<GrassMowerController>().enabled = false;
		_E006.GetComponent<GPUInstancerInstanceRemover>().enabled = false;
		this.m__E002.gameObject.SetActive(value: false);
		switch (cameraMode)
		{
		case CameraModes.FPMode:
			fpController.SetActive(value: true);
			_E007 = fpController;
			break;
		case CameraModes.SpaceshipMode:
			spaceshipCamera.SetActive(value: true);
			_E005.GetComponent<SpaceshipController>().enabled = true;
			_E009.gameObject.SetActive(value: true);
			this.m__E001.gameObject.SetActive(value: true);
			_E007 = spaceshipCamera;
			break;
		case CameraModes.MowerMode:
			grassMowerCamera.SetActive(value: true);
			_E006.GetComponent<GrassMowerController>().enabled = true;
			_E006.GetComponent<GPUInstancerInstanceRemover>().enabled = true;
			this.m__E002.gameObject.SetActive(value: true);
			_E007 = grassMowerCamera;
			break;
		}
		_E008 = cameraMode;
		_E4BD.SetCamera(_E007.GetComponentInChildren<Camera>());
	}

	private void _E002()
	{
		this.m__E003.SetActive(value: false);
		_E4BD.StopListeningGPUIEvent(GPUInstancerEventType.DetailInitializationFinished, _E002);
	}

	private void _E003(QualityMode qualityMode)
	{
		if (!detailManager.gameObject.activeSelf)
		{
			this.m__E004.text = _ED3E._E000(74617);
			return;
		}
		this.m__E004.text = string.Concat(_ED3E._E000(74474), qualityMode, _ED3E._E000(74513));
		if (_E00A != qualityMode)
		{
			_E00A = qualityMode;
			_E004(qualityMode);
			_E4BD.UpdateDetailInstances(detailManager, updateMeshes: true);
		}
	}

	private void _E004(QualityMode qualityMode)
	{
		for (int i = 0; i < detailManager.prototypeList.Count; i++)
		{
			GPUInstancerDetailPrototype gPUInstancerDetailPrototype = (GPUInstancerDetailPrototype)detailManager.prototypeList[i];
			switch (qualityMode)
			{
			case QualityMode.Low:
				gPUInstancerDetailPrototype.isBillboard = !gPUInstancerDetailPrototype.usePrototypeMesh;
				gPUInstancerDetailPrototype.useCrossQuads = false;
				gPUInstancerDetailPrototype.isShadowCasting = false;
				gPUInstancerDetailPrototype.maxDistance = 150f;
				break;
			case QualityMode.Mid:
				gPUInstancerDetailPrototype.isBillboard = false;
				gPUInstancerDetailPrototype.useCrossQuads = !gPUInstancerDetailPrototype.usePrototypeMesh;
				gPUInstancerDetailPrototype.quadCount = 2;
				gPUInstancerDetailPrototype.isShadowCasting = gPUInstancerDetailPrototype.usePrototypeMesh;
				gPUInstancerDetailPrototype.maxDistance = 250f;
				break;
			case QualityMode.High:
				gPUInstancerDetailPrototype.isBillboard = false;
				gPUInstancerDetailPrototype.useCrossQuads = !gPUInstancerDetailPrototype.usePrototypeMesh;
				gPUInstancerDetailPrototype.quadCount = 4;
				gPUInstancerDetailPrototype.isShadowCasting = true;
				gPUInstancerDetailPrototype.maxDistance = 500f;
				break;
			}
		}
	}
}
