using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionAnim : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] float scrollSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Обчислення нового офсету
        float offset = Time.time * scrollSpeed;
        // Встановлення нового офсету
        mat.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
