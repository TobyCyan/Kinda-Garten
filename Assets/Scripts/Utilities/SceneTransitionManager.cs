using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTransitionManager
{
    public static async void LoadHome()
    {
        await LoadSceneAsync(0);
    }

    public static async void ReloadCurrentScene()
    {
        await LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public static async void LoadGameScene()
    {
        await LoadSceneAsync(1);
    }

    public static async void LoadGameScene(int sceneIndex)
    {
        await LoadSceneAsync(sceneIndex);
    }

    private static async Task LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            await Task.Yield();
        }
    }
}
