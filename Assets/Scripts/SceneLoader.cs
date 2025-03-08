using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadScene(SceneName sceneName)
    {
        SceneManager.LoadScene((int)sceneName);
    }
}