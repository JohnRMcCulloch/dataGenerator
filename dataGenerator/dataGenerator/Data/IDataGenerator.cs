namespace dataGenerator.Data;

public interface IDataGenerator<T>
{
    public T GenerateData();
}