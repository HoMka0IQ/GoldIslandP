using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoldPresent : MonoBehaviour
{


    private void OnMouseUp()
    {
        if (IslandBuildMode.instance.buildMode || CameraMovement.instance.drag == true || EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }
        ConfirmationDialog.instance.SetDialog(() =>
        {
            RewardAdController.instance.ShowAd(Reward);
        }, "Watch the ads for<color=yellow> " + GameData.instance.maxMoneyAmount / 5 + "<sprite=0></color>?");
       

        
    }

    public void Reward()
    {
        GameData.instance.IncreaseMoney(GameData.instance.maxMoneyAmount / 5);
        CoinCollectionAnimManager.instance.InitCollectCoinAnim(transform.position, ValutaData.ValutaType.Money);
        gameObject.SetActive(false);
        SoundController.instance.chestSound.Play();
    }
}
