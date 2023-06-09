using System.Collections.Generic;

namespace UnityEngine.AI;

[ExecuteInEditMode]
[AddComponentMenu("Navigation/NavMeshModifierVolume", 31)]
[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
public class NavMeshModifierVolume : MonoBehaviour
{
	[SerializeField]
	private Vector3 m_Size = new Vector3(4f, 3f, 4f);

	[SerializeField]
	private Vector3 m_Center = new Vector3(0f, 1f, 0f);

	[SerializeField]
	private int m_Area;

	[SerializeField]
	private List<int> m_AffectedAgents = new List<int>(new int[1] { -1 });

	private static readonly List<NavMeshModifierVolume> _E000 = new List<NavMeshModifierVolume>();

	public Vector3 size
	{
		get
		{
			return m_Size;
		}
		set
		{
			m_Size = value;
		}
	}

	public Vector3 center
	{
		get
		{
			return m_Center;
		}
		set
		{
			m_Center = value;
		}
	}

	public int area
	{
		get
		{
			return m_Area;
		}
		set
		{
			m_Area = value;
		}
	}

	public static List<NavMeshModifierVolume> activeModifiers => _E000;

	private void OnEnable()
	{
		if (!_E000.Contains(this))
		{
			_E000.Add(this);
		}
	}

	private void OnDisable()
	{
		_E000.Remove(this);
	}

	public bool AffectsAgentType(int agentTypeID)
	{
		if (m_AffectedAgents.Count == 0)
		{
			return false;
		}
		if (m_AffectedAgents[0] == -1)
		{
			return true;
		}
		return m_AffectedAgents.IndexOf(agentTypeID) != -1;
	}
}
