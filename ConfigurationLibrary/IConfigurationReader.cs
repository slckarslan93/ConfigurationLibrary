using ConfigurationLibrary.Models;

public interface IConfigurationReader
    {
        T GetValue<T>(string key);
        List<ConfigurationSetting> GetAllSettings();
    }




