using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace EFT.Settings.Graphics;

[Serializable]
public struct EftResolution : IEquatable<EftResolution>
{
	internal class _E000 : IComparer<EftResolution>
	{
		public virtual int Compare(EftResolution r0, EftResolution r1)
		{
			int num = r0.Width.CompareTo(r1.Width);
			if (num == 0)
			{
				num = r0.Height.CompareTo(r1.Height);
			}
			return num;
		}
	}

	internal sealed class _E001 : _E000
	{
		public override int Compare(EftResolution r0, EftResolution r1)
		{
			if (r0.Width > r0.Height)
			{
				return base.Compare(r0, r1);
			}
			int num = r0.Height.CompareTo(r1.Height);
			if (num == 0)
			{
				num = r0.Width.CompareTo(r1.Width);
			}
			return num;
		}
	}

	private static ReadOnlyCollection<EftResolution> _availableResolutions;

	private static ReadOnlyCollection<EftResolution> _sortedResolutions;

	public int Width;

	public int Height;

	public static ReadOnlyCollection<EftResolution> AvailableResolutions
	{
		get
		{
			Init();
			return _availableResolutions;
		}
	}

	public static ReadOnlyCollection<EftResolution> SortedResolutions
	{
		get
		{
			Init();
			return _sortedResolutions;
		}
	}

	public static void Init()
	{
		HashSet<EftResolution> hashSet = new HashSet<EftResolution>();
		Resolution[] resolutions = Screen.resolutions;
		for (int i = 0; i < resolutions.Length; i++)
		{
			Resolution resolution = resolutions[i];
			bool num;
			if (resolution.width <= resolution.height)
			{
				if (resolution.height <= 800)
				{
					continue;
				}
				num = resolution.width > 600;
			}
			else
			{
				if (resolution.width <= 800)
				{
					continue;
				}
				num = resolution.height > 600;
			}
			if (num)
			{
				hashSet.Add(new EftResolution(resolution));
			}
		}
		if (Screen.fullScreenMode == FullScreenMode.Windowed || Screen.fullScreenMode == FullScreenMode.MaximizedWindow)
		{
			Vector2Int windowMaxResolution = EftDisplay.GetWindowMaxResolution();
			if (windowMaxResolution.x > 0 && windowMaxResolution.y > 0)
			{
				hashSet.Add(new EftResolution(windowMaxResolution.x, windowMaxResolution.y));
			}
		}
		_availableResolutions = hashSet.ToList().AsReadOnly();
		_sortedResolutions = _availableResolutions.OrderByDescending((EftResolution r) => r, new _E000()).ToList().AsReadOnly();
	}

	public EftResolution(Resolution resolution)
		: this(resolution.width, resolution.height)
	{
	}

	public EftResolution(int width, int height)
	{
		Width = width;
		Height = height;
	}

	public AspectRatio GetAspectRatio()
	{
		AspectRatio ratio = GetAspectRatio(Width, Height);
		AspectRatio.TryToFindCorrectAspectRatio(ref ratio);
		return ratio;
	}

	public static AspectRatio GetAspectRatio(int width, int height)
	{
		int num = _E000(width, height);
		return new AspectRatio(width / num, height / num);
	}

	private static int _E000(int a, int b)
	{
		while (b != 0)
		{
			int num = a;
			a = b;
			b = num % b;
		}
		return a;
	}

	public bool Equals(EftResolution other)
	{
		if (Width == other.Width)
		{
			return Height == other.Height;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is EftResolution other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (Width * 397) ^ Height;
	}

	public override string ToString()
	{
		return string.Format(_ED3E._E000(191263), Width, Height, GetAspectRatio());
	}

	public static bool operator ==(EftResolution left, EftResolution right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(EftResolution left, EftResolution right)
	{
		return !left.Equals(right);
	}
}
