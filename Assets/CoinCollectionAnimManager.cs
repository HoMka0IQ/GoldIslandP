using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ValutaData;

public class CoinCollectionAnimManager : MonoBehaviour
{
    [SerializeField] GameObject collectAnimPrefab;
    [SerializeField] List<CoinCollctionAnim> allAnim;

    [SerializeField] AudioSource[] flaskSounds;
    [SerializeField] AudioSource[] coinSounds;
    AudioSource[] currentSound;

    public static CoinCollectionAnimManager instance;

    private void Awake()
    {
        instance = this;
    }
    public void InitCollectCoinAnim(Vector3 startPos, ValutaData.ValutaType valutaType)
    {
        Vector3 endPos = new Vector3();
        Vector3 size = new Vector3();
        if (valutaType == ValutaType.Flask)
        {
            endPos = GlobalData.instance.flaskPos.position;
            size = GlobalData.instance.flaskPos.localScale;
            currentSound = flaskSounds;
        }
        if (valutaType == ValutaType.Money)
        {
            endPos = GlobalData.instance.coinPos.position;
            size = GlobalData.instance.coinPos.localScale;
            currentSound = coinSounds;
        }

        Valuta valuta = ValutaData.instance.GetValutaDataByType(valutaType);

        for (int i = 0; i < allAnim.Count; i++)
        {
            if (allAnim[i].gameObject.activeSelf == false)
            {
                allAnim[i].gameObject.SetActive(true);
                allAnim[i].PlayAnim(startPos, endPos, valuta.icon, size, currentSound);
                return;
            }
        }
        CoinCollctionAnim anim = Instantiate(collectAnimPrefab, transform).GetComponent<CoinCollctionAnim>();
        anim.PlayAnim(startPos, endPos, valuta.icon, size, currentSound);
        allAnim.Add(anim);
    }
}
