using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTransitionManager
{
    private static int currentBuildIndex = 0;
    public static async void LoadHome()
    {
        currentBuildIndex = 0;
        await LoadSceneAsync(0);
    }

    public static async void ReloadCurrentScene()
    {
        await LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public static async void LoadNextScene()
    {
        currentBuildIndex++;
        await LoadSceneAsync(currentBuildIndex);
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
