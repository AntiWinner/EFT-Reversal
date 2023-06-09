using UnityEngine;
using UnityEngine.UI;

namespace GPUInstancer;

public class TreeDemoSceneController : MonoBehaviour
{
	public GPUInstancerTreeManager manager;

	public Text GPUIStateText;

	public Text FPSCountTextText;

	private FPS _E000;

	private void Awake()
	{
		_E000 = GetComponent<FPS>();
	}

	private void Start()
	{
		QualitySettings.shadowDistance = 450f;
	}

	private void Update()
	{
		FPSCountTextText.text = _ED3E._E000(40298) + _E000.FPSCount;
	}

	public void ToggleManager()
	{
		manager.gameObject.SetActive(!manager.gameObject.activeSelf);
		GPUIStateText.text = (manager.gameObject.activeSelf ? _ED3E._E000(76939) : _ED3E._E000(76953));
		GPUIStateText.color = (manager.gameObject.activeSelf ? Color.green : Color.red);
	}
}
