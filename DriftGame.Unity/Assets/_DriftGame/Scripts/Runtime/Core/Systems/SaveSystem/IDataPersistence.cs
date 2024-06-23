using DriftGame.Systems.SaveSystem;

public interface IDataPersistence
{
    public void LoadData(GameData data);
    public void SaveData(ref GameData data);
}