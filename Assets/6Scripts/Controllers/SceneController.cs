using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   // для UI елементів
using System.Collections;
using TMPro;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Асинхронне завантаження сцени з індикатором (Image fillAmount)
    public void LoadSceneAsync(string sceneName, Image progressImage = null, TMP_Text progressText = null)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, progressImage, progressText));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Image progressImage, TMP_Text progressText)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Затримуємо активацію сцени, поки не буде 100%

        while (asyncLoad.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressImage != null)
                progressImage.fillAmount = progress;

            if (progressText != null)
                progressText.text = Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }

        // Завантаження завершене (progress == 0.9), оновлюємо UI на 100%
        if (progressImage != null)
            progressImage.fillAmount = 1f;

        if (progressText != null)
            progressText.text = "100%";

        yield return new WaitForSeconds(0.5f);

        asyncLoad.allowSceneActivation = true;
    }
}
