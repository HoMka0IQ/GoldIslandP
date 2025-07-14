using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollctionAnim : MonoBehaviour
{
    [SerializeField] Image[] allCoins;

    public float moveDuration = 0.7f;
    Camera mainCamera;

    [SerializeField] RectTransform rec;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void PlayAnim(Vector3 startPos, Vector3 endPos, Sprite icon, Vector3 size, AudioSource[] sounds)
    {
        
        StartCoroutine(CollectCoinsAnim(startPos, endPos, icon, size, sounds));
    }
    IEnumerator CollectCoinsAnim(Vector3 startPos, Vector3 endPos, Sprite icon, Vector3 size,AudioSource[] sounds)
    {
        for (int i = 0; i < allCoins.Length; i++)
        {
            allCoins[i].sprite = icon;
            allCoins[i].gameObject.SetActive(true);

            Vector3 initPos = mainCamera.WorldToScreenPoint(startPos);
            allCoins[i].transform.position = new Vector3(Random.Range(-120, 120) + initPos.x, Random.Range(-120, 120) + initPos.y, 0);
            allCoins[i].transform.localScale = size;

            allCoins[i].transform.DOMove(endPos, moveDuration);

            
            sounds[Random.Range(0, sounds.Length)].Play();
            
            yield return new WaitForSeconds(Random.Range(0.03f,0.07f));
        }
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
