using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    // Objek respawn yang akan menjadi tempat respawn pemain
    private Transform respawnPoint;

    // Batas bawah posisi pemain sebelum respawn
    public float fallLimit = -10f;

    // Start dipanggil sebelum frame pertama
    void Start()
    {
        // Cari objek dengan tag "Respawn" untuk digunakan sebagai respawn point
        GameObject respawnObject = GameObject.FindGameObjectWithTag("Respawn");
        if (respawnObject != null)
        {
            respawnPoint = respawnObject.transform;
        }
        else
        {
            Debug.LogError("Respawn object with tag 'Respawn' not found in the scene!");
        }
    }

    // Update dipanggil setiap frame
    void Update()
    {
        // Cek apakah pemain jatuh di bawah batas
        if (transform.position.y < fallLimit)
        {
            RespawnPlayer();
        }
    }

    // Fungsi untuk mereset posisi pemain ke titik respawn
    void RespawnPlayer()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            Debug.Log("Player respawned at: " + respawnPoint.position);
        }
        else
        {
            Debug.LogError("Respawn point not set!");
        }
    }
}
