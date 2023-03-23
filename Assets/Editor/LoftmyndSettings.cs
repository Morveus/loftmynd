using UnityEngine;
using UnityEditor;

public class LoftmyndSettingsProvider
{
    private const string SETTINGS_PATH = "Project/Loftmynd";

    [SettingsProvider]
    public static SettingsProvider CreateLoftmyndSettingsProvider()
    {
        SettingsProvider settingsProvider = new SettingsProvider(SETTINGS_PATH, SettingsScope.Project)
        {
            label = "Loftmynd",
            guiHandler = (searchContext) =>
            {
                GUILayout.Label("Home Assistant API Settings", EditorStyles.boldLabel);

                string apiUrl = EditorGUILayout.TextField("API URL", LoftmyndSettings.ApiUrl);
                PlayerPrefs.SetString(LoftmyndSettings.PREFS_API_URL_KEY, apiUrl);

                string apiToken = EditorGUILayout.TextField("API Token", LoftmyndSettings.ApiToken);
                PlayerPrefs.SetString(LoftmyndSettings.PREFS_API_TOKEN_KEY, apiToken);
            }
        };

        return settingsProvider;
    }
}

public static class LoftmyndSettings
{
    public const string PREFS_API_URL_KEY = "Loftmynd_API_URL";
    public const string PREFS_API_TOKEN_KEY = "Loftmynd_API_TOKEN";

    public static string ApiUrl
    {
        get => PlayerPrefs.GetString(PREFS_API_URL_KEY, "");
        set => PlayerPrefs.SetString(PREFS_API_URL_KEY, value);
    }

    public static string ApiToken
    {
        get => PlayerPrefs.GetString(PREFS_API_TOKEN_KEY, "");
        set => PlayerPrefs.SetString(PREFS_API_TOKEN_KEY, value);
    }
}