using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class DestroyBrokenCrate : MonoBehaviour
{
    
    void Start()
    {
       
        Destroy(gameObject, 5f);
    }
    /*
    void FadeOut()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Renderer>(out Renderer rend))
            {
                float alpha  = rend.material.color.a;
                alpha -= 0.3 * Time.deltaTime;
                
            }
        }
    }*/
    
}
