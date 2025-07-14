using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public string SceneName;
    public Image fillAmount;
    public TMP_Text progressText;
    private void Start()
    {
        LoadScene();
    }
    public void LoadScene()
    {
        SceneController.instance.LoadSceneAsync(SceneName, fillAmount, progressText);
    }
}
