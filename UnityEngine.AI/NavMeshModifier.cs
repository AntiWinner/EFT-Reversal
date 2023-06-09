using System.Collections.Generic;

namespace UnityEngine.AI;

[ExecuteInEditMode]
[AddComponentMenu("Navigation/NavMeshModifier", 32)]
[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
public class NavMeshModifier : MonoBehaviour
{
	[SerializeField]
	private bool m_OverrideArea;

	[SerializeField]
	private int m_Area;

	[SerializeField]
	private bool m_IgnoreFromBuild;

	[SerializeField]
	private List<int> m_AffectedAgents = new List<int>(new int[1] { -1 });

	private static readonly List<NavMeshModifier> _E000 = new List<NavMeshModifier>();

	public bool overrideArea
	{
		get
		{
			return m_OverrideArea;
		}
		set
		{
			m_OverrideArea = value;
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

	public bool ignoreFromBuild
	{
		get
		{
			return m_IgnoreFromBuild;
		}
		set
		{
			m_IgnoreFromBuild = value;
		}
	}

	public static List<NavMeshModifier> activeModifiers => _E000;

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
