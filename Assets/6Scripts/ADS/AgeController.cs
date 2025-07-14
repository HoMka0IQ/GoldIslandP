using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AgeController : MonoBehaviour
{
    public RectTransform content;

    public TMP_Text age;

    public GameObject numberPrefab;
    public List<GameObject> numbers;

    public int numbersCount;

    int oldNumber = -1;
    [SerializeField] int number;


    private void Start()
    {
        FillNumbers();
    }
    private void Update()
    {
        number = (int)Mathf.Abs(Mathf.Floor((content.anchoredPosition.x - 75f) / 200));
        if (oldNumber != number)
        {
            age.text = number + "";
            for (int i = 0; i < numbers.Count; i++)
            {
                numbers[i].transform.GetChild(0).GetChild(0).DOScale(new Vector3(1, 1f, 1f), 0.1f);
                numbers[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().DOFade(0.5f,0.1f);
                numbers[i].transform.GetChild(0).GetComponent<Image>().DOFade(0.5f, 0.1f);
                numbers[i].transform.GetChild(0).DOScale(new Vector3(1, 1, 1), 0.1f);
            }

            numbers[number - 1].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().DOFade(1f, 0.1f);
            numbers[number - 1].transform.GetChild(0).GetComponent<Image>().DOFade(1f, 0.1f);
            numbers[number - 1].transform.GetChild(0).GetChild(0).DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f);
            numbers[number - 1].transform.GetChild(0).DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.1f);
            oldNumber = number;
            age.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0);
            age.transform.DOScale(new Vector3(1f, 1f, 1f), 0.075f);
        }

    }

    public void FillNumbers()
    {
        //scontent.anchoredPosition = new Vector2(150f * numbersCount, content.anchoredPosition.y);
        for (int i = 0; i < numbersCount; i++)
        {
            GameObject number = Instantiate(numberPrefab, content);
            number.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = i + 1 + "";
            numbers.Add(number);
        }
    }

    public void AcceptBtn()
    {
        PlayerPrefs.SetInt("AgeForAds", number);
        SceneController.instance.LoadScene("Load Game");
    }
}
