namespace Deserialization
{
    public interface IJsonSerializer
    {
        public string SerializeToJson<T>(T instance);
        public T DeserializeJson<T>(string serialized);
    }
}