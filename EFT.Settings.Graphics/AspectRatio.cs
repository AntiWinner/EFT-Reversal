using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace EFT.Settings.Graphics;

[Serializable]
public struct AspectRatio : IEquatable<AspectRatio>
{
	private const float ASPECT_RATIO_THRESHOLD = 0.04f;

	private static ReadOnlyCollection<AspectRatio> _availableAspectRatios;

	private static ReadOnlyCollection<AspectRatio> _sortedAspectRatios;

	private static readonly AspectRatio[] _landscapeAspectRatios = new AspectRatio[9]
	{
		new AspectRatio(16, 9),
		new AspectRatio(4, 3),
		new AspectRatio(16, 10),
		new AspectRatio(8, 5),
		new AspectRatio(5, 3),
		new AspectRatio(5, 4),
		new AspectRatio(25, 16),
		new AspectRatio(21, 9),
		new AspectRatio(32, 9)
	};

	private static readonly AspectRatio[] _portraitAspectRatios = new AspectRatio[9]
	{
		new AspectRatio(9, 16),
		new AspectRatio(3, 4),
		new AspectRatio(10, 16),
		new AspectRatio(5, 8),
		new AspectRatio(3, 5),
		new AspectRatio(4, 5),
		new AspectRatio(16, 25),
		new AspectRatio(9, 21),
		new AspectRatio(9, 32)
	};

	public static ReadOnlyCollection<AspectRatio> AvailableAspectRatios
	{
		get
		{
			Init();
			return _availableAspectRatios;
		}
	}

	public static ReadOnlyCollection<AspectRatio> SortedAspectRatios
	{
		get
		{
			Init();
			return _sortedAspectRatios;
		}
	}

	public int X { get; private set; }

	public int Y { get; private set; }

	[JsonIgnore]
	public float Division => (float)X / (float)Y;

	public static void Init()
	{
		HashSet<AspectRatio> hashSet = new HashSet<AspectRatio>();
		foreach (EftResolution availableResolution in EftResolution.AvailableResolutions)
		{
			AspectRatio aspectRatio = availableResolution.GetAspectRatio();
			hashSet.Add(aspectRatio);
		}
		_availableAspectRatios = hashSet.ToList().AsReadOnly();
		_sortedAspectRatios = _availableAspectRatios.OrderByDescending((AspectRatio x) => x.X).ToList().AsReadOnly();
	}

	public AspectRatio(int x, int y)
	{
		X = x;
		Y = y;
	}

	public bool IsClose(in AspectRatio ratio)
	{
		return Math.Abs(Division - ratio.Division) < 0.04f;
	}

	public static bool TryToFindCorrectAspectRatio(ref AspectRatio ratio)
	{
		AspectRatio[] array = ((Screen.currentResolution.width > Screen.currentResolution.height) ? _landscapeAspectRatios : _portraitAspectRatios);
		for (int i = 0; i < array.Length; i++)
		{
			AspectRatio aspectRatio = array[i];
			if (aspectRatio.IsClose(in ratio))
			{
				ratio = aspectRatio;
				return true;
			}
		}
		return false;
	}

	public bool Equals(AspectRatio other)
	{
		if (X.Equals(other.X))
		{
			return Y.Equals(other.Y);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is AspectRatio other)
		{
			return Equals(other);
		}
		return false;
	}

	public static bool operator ==(AspectRatio value1, AspectRatio value2)
	{
		return value1.Equals(value2);
	}

	public static bool operator !=(AspectRatio value1, AspectRatio value2)
	{
		return !value1.Equals(value2);
	}

	public override string ToString()
	{
		return string.Format(_ED3E._E000(71425), X, Y);
	}

	public override int GetHashCode()
	{
		return (X.GetHashCode() * 397) ^ Y.GetHashCode();
	}

	public static AspectRatio FindCloseRatio(EftResolution resolution)
	{
		AspectRatio ratio = resolution.GetAspectRatio();
		TryToFindCorrectAspectRatio(ref ratio);
		foreach (AspectRatio availableAspectRatio in AvailableAspectRatios)
		{
			if (!availableAspectRatio.IsClose(in ratio))
			{
				continue;
			}
			return availableAspectRatio;
		}
		return AvailableAspectRatios.First();
	}
}
