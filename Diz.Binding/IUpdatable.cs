namespace Diz.Binding;

public interface IUpdatable<in T> where T : IUpdatable<T>
{
	bool Compare(T other);

	void UpdateFromAnotherItem(T other);
}
