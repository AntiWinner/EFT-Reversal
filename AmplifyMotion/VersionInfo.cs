using System;
using UnityEngine;

namespace AmplifyMotion;

[Serializable]
public class VersionInfo
{
	public const byte Major = 1;

	public const byte Minor = 8;

	public const byte Release = 1;

	private static string StageSuffix = _ED3E._E000(239089);

	private static string TrialSuffix = "";

	[SerializeField]
	private int m_major;

	[SerializeField]
	private int m_minor;

	[SerializeField]
	private int m_release;

	public int Number => m_major * 100 + m_minor * 10 + m_release;

	public static string StaticToString()
	{
		return string.Format(_ED3E._E000(239101), (byte)1, (byte)8, (byte)1) + StageSuffix + TrialSuffix;
	}

	public override string ToString()
	{
		return string.Format(_ED3E._E000(239101), m_major, m_minor, m_release) + StageSuffix + TrialSuffix;
	}

	private VersionInfo()
	{
		m_major = 1;
		m_minor = 8;
		m_release = 1;
	}

	private VersionInfo(byte major, byte minor, byte release)
	{
		m_major = major;
		m_minor = minor;
		m_release = release;
	}

	public static VersionInfo Current()
	{
		return new VersionInfo(1, 8, 1);
	}

	public static bool Matches(VersionInfo version)
	{
		if (1 == version.m_major && 8 == version.m_minor)
		{
			return 1 == version.m_release;
		}
		return false;
	}
}
