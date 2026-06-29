using GameReady.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadTester : MonoBehaviour
{
    [SerializeField] private SaveData _saveData;
    [SerializeField] private SaveLoadSetting _saveLoadSetting;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    private SaveLoader<SaveData> _saveLoader;

    private void Awake()
    {
        ISaveLoadSerializer saveSerializer = new JsonSaveDataSerializer();
        _saveLoader = new SaveLoader<SaveData>(_saveData, saveSerializer, _saveLoadSetting);
        Load();
    }

    [ContextMenu("Save")]
    public async void Save()
    {
        _saveData.PlayerName = _inputField.text;
        await _saveLoader.SaveDataAsync();

        Debug.Log($"Save Path: {_saveLoader.SaveDirectoryPath}");
    }

    [ContextMenu("Load")]
    public async void Load()
    {
        await _saveLoader.LoadDataAsync();
        _inputField.text = _saveData.PlayerName;
    }
}
