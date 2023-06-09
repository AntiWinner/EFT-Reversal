using System;
using System.Collections.Generic;

namespace UnityEngine.AI;

[AddComponentMenu("Navigation/NavMeshLink", 33)]
[ExecuteInEditMode]
[DefaultExecutionOrder(-101)]
[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
public class NavMeshLink : MonoBehaviour
{
	[SerializeField]
	private int m_AgentTypeID;

	[SerializeField]
	private Vector3 m_StartPoint = new Vector3(0f, 0f, -2.5f);

	[SerializeField]
	private Vector3 m_EndPoint = new Vector3(0f, 0f, 2.5f);

	[SerializeField]
	private float m_Width;

	[SerializeField]
	private int m_CostModifier = -1;

	[SerializeField]
	private bool m_Bidirectional = true;

	[SerializeField]
	private bool m_AutoUpdatePosition;

	[SerializeField]
	private int m_Area;

	private NavMeshLinkInstance m__E000;

	private Vector3 m__E001 = Vector3.zero;

	private Quaternion m__E002 = Quaternion.identity;

	private static readonly List<NavMeshLink> m__E003 = new List<NavMeshLink>();

	public int agentTypeID
	{
		get
		{
			return m_AgentTypeID;
		}
		set
		{
			m_AgentTypeID = value;
			UpdateLink();
		}
	}

	public Vector3 startPoint
	{
		get
		{
			return m_StartPoint;
		}
		set
		{
			m_StartPoint = value;
			UpdateLink();
		}
	}

	public Vector3 endPoint
	{
		get
		{
			return m_EndPoint;
		}
		set
		{
			m_EndPoint = value;
			UpdateLink();
		}
	}

	public float width
	{
		get
		{
			return m_Width;
		}
		set
		{
			m_Width = value;
			UpdateLink();
		}
	}

	public int costModifier
	{
		get
		{
			return m_CostModifier;
		}
		set
		{
			m_CostModifier = value;
			UpdateLink();
		}
	}

	public bool bidirectional
	{
		get
		{
			return m_Bidirectional;
		}
		set
		{
			m_Bidirectional = value;
			UpdateLink();
		}
	}

	public bool autoUpdate
	{
		get
		{
			return m_AutoUpdatePosition;
		}
		set
		{
			_E002(value);
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
			UpdateLink();
		}
	}

	private void OnEnable()
	{
		_E003();
		if (m_AutoUpdatePosition && this.m__E000.valid)
		{
			_E000(this);
		}
	}

	private void OnDisable()
	{
		_E001(this);
		this.m__E000.Remove();
	}

	public void UpdateLink()
	{
		this.m__E000.Remove();
		_E003();
	}

	private static void _E000(NavMeshLink link)
	{
		if (NavMeshLink.m__E003.Count == 0)
		{
			NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Combine(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(_E006));
		}
		NavMeshLink.m__E003.Add(link);
	}

	private static void _E001(NavMeshLink link)
	{
		NavMeshLink.m__E003.Remove(link);
		if (NavMeshLink.m__E003.Count == 0)
		{
			NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Remove(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(_E006));
		}
	}

	private void _E002(bool value)
	{
		if (m_AutoUpdatePosition != value)
		{
			m_AutoUpdatePosition = value;
			if (value)
			{
				_E000(this);
			}
			else
			{
				_E001(this);
			}
		}
	}

	private void _E003()
	{
		NavMeshLinkData link = default(NavMeshLinkData);
		link.startPosition = m_StartPoint;
		link.endPosition = m_EndPoint;
		link.width = m_Width;
		link.costModifier = m_CostModifier;
		link.bidirectional = m_Bidirectional;
		link.area = m_Area;
		link.agentTypeID = m_AgentTypeID;
		this.m__E000 = NavMesh.AddLink(link, base.transform.position, base.transform.rotation);
		if (this.m__E000.valid)
		{
			this.m__E000.owner = this;
		}
		this.m__E001 = base.transform.position;
		this.m__E002 = base.transform.rotation;
	}

	private bool _E004()
	{
		if (this.m__E001 != base.transform.position)
		{
			return true;
		}
		if (this.m__E002 != base.transform.rotation)
		{
			return true;
		}
		return false;
	}

	private void _E005()
	{
		UpdateLink();
	}

	private static void _E006()
	{
		foreach (NavMeshLink item in NavMeshLink.m__E003)
		{
			if (item._E004())
			{
				item.UpdateLink();
			}
		}
	}
}
