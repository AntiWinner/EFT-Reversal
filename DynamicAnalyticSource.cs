public class DynamicAnalyticSource : AnalyticSource
{
	protected override void Start()
	{
		AmbientLight.AddDynamicAnalyticSource(this);
		UpdateSettings();
	}

	protected override void OnEnable()
	{
		AmbientLight.AddDynamicAnalyticSource(this);
		UpdateSettings();
	}

	protected override void OnDisable()
	{
		AmbientLight.RemoveDynamicAnalyticSource(this);
	}

	public void UpdateDynamicValues()
	{
		if (base.transform.hasChanged)
		{
			Bounds.center = base.transform.position;
			LocalToWorldMatrix = base.transform.localToWorldMatrix;
		}
	}
}
