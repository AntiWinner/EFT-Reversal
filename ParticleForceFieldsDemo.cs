using MirzaBeig.Scripting.Effects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParticleForceFieldsDemo : MonoBehaviour
{
	[Header("Overview")]
	public CustomTextMeshProUGUI FPSText;

	public CustomTextMeshProUGUI particleCountText;

	public Toggle postProcessingToggle;

	public MonoBehaviour postProcessing;

	[Header("Particle System Settings")]
	public ParticleSystem particleSystem;

	private ParticleSystem.MainModule _E000;

	private ParticleSystem.EmissionModule _E001;

	public CustomTextMeshProUGUI maxParticlesText;

	public CustomTextMeshProUGUI particlesPerSecondText;

	public Slider maxParticlesSlider;

	public Slider particlesPerSecondSlider;

	[Header("Attraction Particle Force Field Settings")]
	public AttractionParticleForceField attractionParticleForceField;

	public CustomTextMeshProUGUI attractionParticleForceFieldRadiusText;

	public CustomTextMeshProUGUI attractionParticleForceFieldMaxForceText;

	public CustomTextMeshProUGUI attractionParticleForceFieldArrivalRadiusText;

	public CustomTextMeshProUGUI attractionParticleForceFieldArrivedRadiusText;

	public CustomTextMeshProUGUI attractionParticleForceFieldPositionTextX;

	public CustomTextMeshProUGUI attractionParticleForceFieldPositionTextY;

	public CustomTextMeshProUGUI attractionParticleForceFieldPositionTextZ;

	public Slider attractionParticleForceFieldRadiusSlider;

	public Slider attractionParticleForceFieldMaxForceSlider;

	public Slider attractionParticleForceFieldArrivalRadiusSlider;

	public Slider attractionParticleForceFieldArrivedRadiusSlider;

	public Slider attractionParticleForceFieldPositionSliderX;

	public Slider attractionParticleForceFieldPositionSliderY;

	public Slider attractionParticleForceFieldPositionSliderZ;

	[Header("Vortex Particle Force Field Settings")]
	public VortexParticleForceField vortexParticleForceField;

	public CustomTextMeshProUGUI vortexParticleForceFieldRadiusText;

	public CustomTextMeshProUGUI vortexParticleForceFieldMaxForceText;

	public CustomTextMeshProUGUI vortexParticleForceFieldRotationTextX;

	public CustomTextMeshProUGUI vortexParticleForceFieldRotationTextY;

	public CustomTextMeshProUGUI vortexParticleForceFieldRotationTextZ;

	public CustomTextMeshProUGUI vortexParticleForceFieldPositionTextX;

	public CustomTextMeshProUGUI vortexParticleForceFieldPositionTextY;

	public CustomTextMeshProUGUI vortexParticleForceFieldPositionTextZ;

	public Slider vortexParticleForceFieldRadiusSlider;

	public Slider vortexParticleForceFieldMaxForceSlider;

	public Slider vortexParticleForceFieldRotationSliderX;

	public Slider vortexParticleForceFieldRotationSliderY;

	public Slider vortexParticleForceFieldRotationSliderZ;

	public Slider vortexParticleForceFieldPositionSliderX;

	public Slider vortexParticleForceFieldPositionSliderY;

	public Slider vortexParticleForceFieldPositionSliderZ;

	private void Start()
	{
		if ((bool)postProcessing)
		{
			postProcessingToggle.isOn = postProcessing.enabled;
		}
		_E000 = particleSystem.main;
		_E001 = particleSystem.emission;
		maxParticlesSlider.value = _E000.maxParticles;
		particlesPerSecondSlider.value = _E001.rateOverTime.constant;
		maxParticlesText.text = _ED3E._E000(40150) + maxParticlesSlider.value;
		particlesPerSecondText.text = _ED3E._E000(40134) + particlesPerSecondSlider.value;
		attractionParticleForceFieldRadiusSlider.value = attractionParticleForceField.radius;
		attractionParticleForceFieldMaxForceSlider.value = attractionParticleForceField.force;
		attractionParticleForceFieldArrivalRadiusSlider.value = attractionParticleForceField.arrivalRadius;
		attractionParticleForceFieldArrivedRadiusSlider.value = attractionParticleForceField.arrivedRadius;
		Vector3 position = attractionParticleForceField.transform.position;
		attractionParticleForceFieldPositionSliderX.value = position.x;
		attractionParticleForceFieldPositionSliderY.value = position.y;
		attractionParticleForceFieldPositionSliderZ.value = position.z;
		attractionParticleForceFieldRadiusText.text = _ED3E._E000(40173) + attractionParticleForceFieldRadiusSlider.value;
		attractionParticleForceFieldMaxForceText.text = _ED3E._E000(40166) + attractionParticleForceFieldMaxForceSlider.value;
		attractionParticleForceFieldArrivalRadiusText.text = _ED3E._E000(40218) + attractionParticleForceFieldArrivalRadiusSlider.value;
		attractionParticleForceFieldArrivedRadiusText.text = _ED3E._E000(40203) + attractionParticleForceFieldArrivedRadiusSlider.value;
		attractionParticleForceFieldPositionTextX.text = _ED3E._E000(40244) + attractionParticleForceFieldPositionSliderX.value;
		attractionParticleForceFieldPositionTextY.text = _ED3E._E000(40233) + attractionParticleForceFieldPositionSliderY.value;
		attractionParticleForceFieldPositionTextZ.text = _ED3E._E000(40286) + attractionParticleForceFieldPositionSliderZ.value;
		vortexParticleForceFieldRadiusSlider.value = vortexParticleForceField.radius;
		vortexParticleForceFieldMaxForceSlider.value = vortexParticleForceField.force;
		Vector3 eulerAngles = vortexParticleForceField.transform.eulerAngles;
		vortexParticleForceFieldRotationSliderX.value = eulerAngles.x;
		vortexParticleForceFieldRotationSliderY.value = eulerAngles.y;
		vortexParticleForceFieldRotationSliderZ.value = eulerAngles.z;
		Vector3 position2 = vortexParticleForceField.transform.position;
		vortexParticleForceFieldPositionSliderX.value = position2.x;
		vortexParticleForceFieldPositionSliderY.value = position2.y;
		vortexParticleForceFieldPositionSliderZ.value = position2.z;
		vortexParticleForceFieldRadiusText.text = _ED3E._E000(40173) + vortexParticleForceFieldRadiusSlider.value;
		vortexParticleForceFieldMaxForceText.text = _ED3E._E000(40166) + vortexParticleForceFieldMaxForceSlider.value;
		vortexParticleForceFieldRotationTextX.text = _ED3E._E000(40275) + vortexParticleForceFieldRotationSliderX.value;
		vortexParticleForceFieldRotationTextY.text = _ED3E._E000(40256) + vortexParticleForceFieldRotationSliderY.value;
		vortexParticleForceFieldRotationTextZ.text = _ED3E._E000(40309) + vortexParticleForceFieldRotationSliderZ.value;
		vortexParticleForceFieldPositionTextX.text = _ED3E._E000(40244) + vortexParticleForceFieldPositionSliderX.value;
		vortexParticleForceFieldPositionTextY.text = _ED3E._E000(40233) + vortexParticleForceFieldPositionSliderY.value;
		vortexParticleForceFieldPositionTextZ.text = _ED3E._E000(40286) + vortexParticleForceFieldPositionSliderZ.value;
	}

	private void Update()
	{
		FPSText.text = _ED3E._E000(40298) + 1f / Time.deltaTime;
		particleCountText.text = _ED3E._E000(40288) + particleSystem.particleCount;
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void SetMaxParticles(float value)
	{
		_E000.maxParticles = (int)value;
		maxParticlesText.text = _ED3E._E000(40150) + value;
	}

	public void SetParticleEmissionPerSecond(float value)
	{
		_E001.rateOverTime = value;
		particlesPerSecondText.text = _ED3E._E000(40134) + value;
	}

	public void SetAttractionParticleForceFieldRadius(float value)
	{
		attractionParticleForceField.radius = value;
		attractionParticleForceFieldRadiusText.text = _ED3E._E000(40173) + value;
	}

	public void SetAttractionParticleForceFieldMaxForce(float value)
	{
		attractionParticleForceField.force = value;
		attractionParticleForceFieldMaxForceText.text = _ED3E._E000(40166) + value;
	}

	public void SetAttractionParticleForceFieldArrivalRadius(float value)
	{
		attractionParticleForceField.arrivalRadius = value;
		attractionParticleForceFieldArrivalRadiusText.text = _ED3E._E000(40218) + value;
	}

	public void SetAttractionParticleForceFieldArrivedRadius(float value)
	{
		attractionParticleForceField.arrivedRadius = value;
		attractionParticleForceFieldArrivedRadiusText.text = _ED3E._E000(40203) + value;
	}

	public void SetAttractionParticleForceFieldPositionX(float value)
	{
		Vector3 position = attractionParticleForceField.transform.position;
		position.x = value;
		attractionParticleForceField.transform.position = position;
		attractionParticleForceFieldPositionTextX.text = _ED3E._E000(40244) + value;
	}

	public void SetAttractionParticleForceFieldPositionY(float value)
	{
		Vector3 position = attractionParticleForceField.transform.position;
		position.y = value;
		attractionParticleForceField.transform.position = position;
		attractionParticleForceFieldPositionTextY.text = _ED3E._E000(40233) + value;
	}

	public void SetAttractionParticleForceFieldPositionZ(float value)
	{
		Vector3 position = attractionParticleForceField.transform.position;
		position.z = value;
		attractionParticleForceField.transform.position = position;
		attractionParticleForceFieldPositionTextZ.text = _ED3E._E000(40286) + value;
	}

	public void SetVortexParticleForceFieldRadius(float value)
	{
		vortexParticleForceField.radius = value;
		vortexParticleForceFieldRadiusText.text = _ED3E._E000(40173) + value;
	}

	public void SetVortexParticleForceFieldMaxForce(float value)
	{
		vortexParticleForceField.force = value;
		vortexParticleForceFieldMaxForceText.text = _ED3E._E000(40166) + value;
	}

	public void SetVortexParticleForceFieldRotationX(float value)
	{
		Vector3 eulerAngles = vortexParticleForceField.transform.eulerAngles;
		eulerAngles.x = value;
		vortexParticleForceField.transform.eulerAngles = eulerAngles;
		vortexParticleForceFieldRotationTextX.text = _ED3E._E000(40275) + value;
	}

	public void SetVortexParticleForceFieldRotationY(float value)
	{
		Vector3 eulerAngles = vortexParticleForceField.transform.eulerAngles;
		eulerAngles.y = value;
		vortexParticleForceField.transform.eulerAngles = eulerAngles;
		vortexParticleForceFieldRotationTextY.text = _ED3E._E000(40256) + value;
	}

	public void SetVortexParticleForceFieldRotationZ(float value)
	{
		Vector3 eulerAngles = vortexParticleForceField.transform.eulerAngles;
		eulerAngles.z = value;
		vortexParticleForceField.transform.eulerAngles = eulerAngles;
		vortexParticleForceFieldRotationTextZ.text = _ED3E._E000(40309) + value;
	}

	public void SetVortexParticleForceFieldPositionX(float value)
	{
		Vector3 position = vortexParticleForceField.transform.position;
		position.x = value;
		vortexParticleForceField.transform.position = position;
		vortexParticleForceFieldPositionTextX.text = _ED3E._E000(40244) + value;
	}

	public void SetVortexParticleForceFieldPositionY(float value)
	{
		Vector3 position = vortexParticleForceField.transform.position;
		position.y = value;
		vortexParticleForceField.transform.position = position;
		vortexParticleForceFieldPositionTextY.text = _ED3E._E000(40233) + value;
	}

	public void SetVortexParticleForceFieldPositionZ(float value)
	{
		Vector3 position = vortexParticleForceField.transform.position;
		position.z = value;
		vortexParticleForceField.transform.position = position;
		vortexParticleForceFieldPositionTextZ.text = _ED3E._E000(40286) + value;
	}
}
