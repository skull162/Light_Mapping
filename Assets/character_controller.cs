using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Variabel publik untuk mengatur kecepatan gerak dan kekuatan lompatan dari Inspector
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    // Komponen Rigidbody karakter
    private Rigidbody2D rb;

    // Komponen Animator untuk mengelola animasi
    private Animator animator;

    // Mengecek apakah karakter berada di tanah
    private bool isGrounded;

    // Variabel untuk menyimpan arah pandangan karakter
    private bool isFacingRight = true;

    // Variabel untuk input horizontal
    private float moveInput;

    // Layer untuk tanah
    public LayerMask groundLayer;

    // Start dipanggil sebelum frame pertama
    void Start()
    {
        // Mendapatkan komponen Rigidbody2D dan Animator
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update dipanggil setiap frame untuk menangani input
    void Update()
    {
        // Mendapatkan input horizontal (A/D atau panah kiri/kanan)
        moveInput = Input.GetAxis("Horizontal");

        // Tetapkan parameter "isWalking" di Animator hanya jika input signifikan
        animator.SetBool("isWalking", Mathf.Abs(moveInput) > 0.1f);

        // Melompat jika tombol Space atau panah atas ditekan dan karakter berada di tanah
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Mengatur parameter "isJump" di Animator
        animator.SetBool("isJump", !isGrounded);

        // Membalik arah pandangan karakter jika bergerak ke kiri atau kanan
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // FixedUpdate dipanggil secara konsisten untuk logika fisika
    void FixedUpdate()
    {
        // Mengatur kecepatan horizontal langsung dengan Time.deltaTime untuk pergerakan yang konsisten
        float targetVelocity = moveInput * moveSpeed;
        rb.velocity = new Vector2(targetVelocity * Time.deltaTime * 60f, rb.velocity.y);

        // Menggunakan raycast untuk mendeteksi tanah hanya jika karakter berada di bawah tanah (untuk menembus)
        if (rb.velocity.y < 0)  // Karakter bergerak ke bawah
        {
            if (Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer))
            {
                // Jika terkena tanah di bawah, maka biarkan karakter menembus tanah
                isGrounded = true;
            }
        }
    }

    // Fungsi untuk membalik arah pandangan karakter
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Mengecek apakah karakter menyentuh tanah menggunakan collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
