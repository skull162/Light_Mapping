using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Target yang akan diikuti oleh kamera (biasanya karakter pemain)
    public Transform target;

    // Kecepatan smoothing (semakin tinggi nilai, semakin cepat kamera mengikuti)
    public float smoothSpeed = 0.125f;

    // Offset kamera dari target
    public Vector3 offset;

    void LateUpdate()
    {
        // Jika target tidak diset, keluar dari fungsi
        if (target == null)
            return;

        // Tentukan posisi target dengan offset
        Vector3 desiredPosition = target.position + offset;

        // Lerp untuk membuat gerakan kamera halus
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set posisi kamera ke posisi yang telah dihaluskan
        transform.position = smoothedPosition;
    }
}