using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTilingAnim : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] float speed;
    private void Start()
    {
        mat.mainTextureOffset = new Vector2(0, 0);
    }

    void Update()
    {
        mat.mainTextureOffset += new Vector2(speed * Time.deltaTime, speed * Time.deltaTime);
    }
}
