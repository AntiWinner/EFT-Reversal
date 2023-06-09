using System;

namespace Diz.Binding;

public interface IBindable<out T>
{
	T Value { get; }

	Action Bind(Action<T> handler);

	Action Subscribe(Action<T> handler);

	Action BindWithoutValue(_ED00 handler);
}
