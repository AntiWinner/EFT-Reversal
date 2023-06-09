using UnityEngine;

[ExecuteInEditMode]
public class CheatShotLogVisualizer : MonoBehaviour
{
	private Vector3 _E000;

	private Vector3 _E001;

	private Vector3 _E002;

	private Vector3 _E003;

	private Vector3 _E004;

	private string _E005;

	private Vector3[] _E006;

	private Vector3[] _E007;

	private float _E008;

	[SerializeField]
	private float _radius1 = 0.025f;

	[SerializeField]
	private float _radius2 = 1f / 160f;

	[SerializeField]
	private float _radius3 = 1f / 160f;

	public static CheatShotLogVisualizer Create(string name, Vector3 aggressorPosition, Vector3 shotDirection, Vector3 shotStart, Vector3 shotEnd, Vector3 victimPosition, Vector3 victimPartPosition, string bodyPart, Vector3[] collisions, Vector3[] invalidCollisions)
	{
		GameObject obj = new GameObject(_ED3E._E000(55238) + name);
		obj.transform.position = aggressorPosition;
		CheatShotLogVisualizer cheatShotLogVisualizer = obj.AddComponent<CheatShotLogVisualizer>();
		cheatShotLogVisualizer.Init(aggressorPosition, shotDirection, shotStart, shotEnd, victimPosition, victimPartPosition, bodyPart, collisions, invalidCollisions);
		return cheatShotLogVisualizer;
	}

	public void Init(Vector3 aggressorPosition, Vector3 shotDirection, Vector3 shotStart, Vector3 shotEnd, Vector3 victimPosition, Vector3 victimPartPosition, string bodyPart, Vector3[] collisions, Vector3[] invalidCollisions)
	{
		_E000 = aggressorPosition;
		_E001 = shotStart;
		_E002 = shotEnd;
		_E003 = victimPosition;
		_E004 = victimPartPosition;
		_E005 = bodyPart;
		_E006 = collisions;
		_E007 = invalidCollisions;
		_E008 = Vector3.Distance(_E001, _E002);
		Debug.Log(_ED3E._E000(55286) + _E008, base.gameObject);
	}
}
