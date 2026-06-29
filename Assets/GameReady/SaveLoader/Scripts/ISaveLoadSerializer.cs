namespace GameReady.SaveLoad
{
    public interface ISaveLoadSerializer
    {
        string Serialize(object data);
        void Deserialize(string serializedData, object target);
    }
}