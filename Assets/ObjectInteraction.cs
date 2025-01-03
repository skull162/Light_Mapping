using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject objectToDisable; // Objek yang akan dinonaktifkan
    public GameObject objectToSpawn;   // Objek baru yang akan dimunculkan
    public GameObject fKeyImage;       // Sprite gambar tombol F

    private bool isPlayerNearby = false;
    private MovementController controlPlayer;
    private NetworkController controlNetwork;

    public List<string> idPlayer = new List<string>();
    public List<string> lampId = new List<string>();
    public string currentId;


    void Start()
    {
        if (fKeyImage != null)
        {
            fKeyImage.SetActive(false); // Sembunyikan sprite tombol F saat awal
        }
    }

    void Update()
    {
        /*if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            // Nonaktifkan objek
            if (objectToDisable != null)
            {
                objectToDisable.SetActive(false);
            }

            // Munculkan objek baru
            if (objectToSpawn != null)
            {
                objectToSpawn.SetActive(true);
            }

            // Sembunyikan tombol F
            if (fKeyImage != null)
            {
                fKeyImage.SetActive(false);
            }
        }*/
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            controlPlayer = other.gameObject.GetComponent<MovementController>();
            controlNetwork = other.gameObject.GetComponent<NetworkController>();
            controlPlayer.insideLamp = true;
            currentId = controlNetwork.Id;
            isPlayerNearby = true;
            if (fKeyImage != null)
            {
                fKeyImage.SetActive(true); // Tampilkan sprite tombol F
            }

            if(!idPlayer.Contains(controlNetwork.Id))
            {
                idPlayer.Add(controlNetwork.Id);
            }
            else{
                Debug.Log("Id Ada didalam List");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (fKeyImage != null)
            {
                fKeyImage.SetActive(false); // Sembunyikan sprite tombol F
            }
            currentId = null;
        }
        Debug.Log(idPlayer[idPlayer.Count - 1]);
    }

    public void PickedItem()
    {
        // Nonaktifkan objek
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }

        // Munculkan objek baru
        if (objectToSpawn != null)
        {
            objectToSpawn.SetActive(true);
        }

        // Sembunyikan tombol F
        if (fKeyImage != null)
        {
            fKeyImage.SetActive(false);
        }

        if(!lampId.Contains(currentId))
        {
            GameObject lampObject;
            lampObject =  Instantiate(objectToSpawn, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            lampObject.gameObject.GetComponent<FollowPlayer>().targetPlayerId = currentId;
            lampId.Add(lampObject.gameObject.GetComponent<FollowPlayer>().targetPlayerId);
        }
    }
}
