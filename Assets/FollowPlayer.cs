using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Referensi ke pemain
    public Transform player;

    // Kecepatan pergerakan objek mengikuti pemain
    public float followSpeed = 5f;

    // Offset posisi (agar objek tidak berada tepat di posisi pemain, tapi sedikit bergeser)
    public Vector3 offset;

    // ID pemain yang ingin diikuti
    public string targetPlayerId;

    void Start()
    {
        // Cari pemain secara otomatis tanpa memerlukan ID manual
        AssignPlayerToFollow();
    }

    // Metode untuk mencari pemain pertama yang ditemukan dengan NetworkController
    public void AssignPlayerToFollow()
    {
        NetworkController[] players = FindObjectsOfType<NetworkController>();

        // Cari pemain pertama yang ditemukan
        if (players.Length > 0)
        {
            player = players[0].transform;  // Ambil transform dari pemain pertama
            targetPlayerId = players[0].Id; // Set ID pemain yang pertama
            Debug.Log($"Assigned to follow player with ID {targetPlayerId}");
        }
        else
        {
            Debug.LogWarning("No players found in the scene!");
        }
    }

    // Update dipanggil setiap frame
    void Update()
    {
        // Pastikan objek mengikuti pemain dengan posisi yang diberi offset
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;

            // Menggerakkan objek ke posisi target dengan kecepatan tertentu
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Player reference is missing!");
        }
    }
}
