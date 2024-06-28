namespace dataGenerator.Data;

/// <summary>
/// Defines a generic interface for generating data of type <typeparamref name="T"/>.
/// </summary>
public interface IDataGenerator<T>
{
    public T GenerateData();
}