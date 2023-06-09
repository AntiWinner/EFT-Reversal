namespace EFT;

public interface ISerializer<T>
{
	T Deserialize();

	object Serialize(T t);
}
