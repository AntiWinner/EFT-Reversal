using System;
using UnityEngine;

namespace EFT.UI;

public interface IUIView : IDisposable
{
	GameObject GameObject { get; }

	Transform Transform { get; }
}
