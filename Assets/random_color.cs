using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Mendapatkan komponen Renderer dari GameObject
        Renderer renderer = GetComponent<Renderer>();

        // Jika Renderer ditemukan, atur warna acak
        if (renderer != null)
        {
            renderer.material.color = GenerateRandomColor();
        }
    }

    // Fungsi untuk menghasilkan warna acak
    Color GenerateRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
