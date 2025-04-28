using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveDirection;
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A-D veya Sol-Sağ
        float vertical = Input.GetAxis("Vertical");     // W-S veya Yukarı-Aşağı

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Yerçekimi
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Hafif negatif yapıyoruz yere yapışsın diye
        }
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 gravityMove = new Vector3(0, verticalVelocity, 0);
        controller.Move(gravityMove * Time.deltaTime);
    }
}
