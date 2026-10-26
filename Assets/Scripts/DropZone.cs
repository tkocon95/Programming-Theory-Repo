using UnityEngine;
using UnityEngine.InputSystem;

public class DropZone : MonoBehaviour
{
    private Camera mainCamera;
    private Collider2D dropZoneCollider;

    private GameObject currentBall;
    private Rigidbody2D currentRb;
    private Vector2 targetPosition;
    private ObjectManager objectManager;
    private GameManager gameManager;

    void Start()
    {
        mainCamera = Camera.main;
        dropZoneCollider = GetComponent<Collider2D>();

        objectManager = ObjectManager.Instance;
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (gameManager == null || !gameManager.GameActive)
            return;
        
        if (Time.timeScale == 0f)
            return;

        if (mainCamera == null || Mouse.current == null || dropZoneCollider == null)
            return;

        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, -mainCamera.transform.position.z));
        Vector2 mouseWorld2D = new Vector2(mouseWorld.x, mouseWorld.y);

        bool mouseInside = dropZoneCollider.OverlapPoint(mouseWorld2D);

        // Tạo hoặc lấy Ball từ Pool khi chuột đi vào vùng
        if (mouseInside && currentBall == null)
        {
            SpawnBall(mouseWorld2D);
        }

        if (currentBall == null)
            return;

        targetPosition = mouseWorld2D;

        // Chuột rời vùng trước khi thả: trả Ball về Pool
        if (!mouseInside)
        {
            DeactivateBall();
            return;
        }

        // Chỉ thả khi click bên trong DropZone
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            DropBall();
        }
    }

    void FixedUpdate()
    {
        if (currentBall == null || currentRb == null)
            return;

        if (currentRb.bodyType == RigidbodyType2D.Kinematic)
        {
            currentRb.MovePosition(targetPosition);
        }
    }

    void SpawnBall(Vector2 position)
    {
        if (objectManager == null)
        {
            Debug.LogError("Không tìm thấy ObjetManager trong scene!");
            return;
        }

        // Không còn Ball chưa thả nếu Pool đã hết
        if (objectManager.GetRemainObj() <= 0)
        {
            Debug.Log("Bạn đã hết Ball!");
            return;
        }

        currentBall = objectManager.GetObject();

        if (currentBall == null)
            return;

        currentBall.transform.position = new Vector3(position.x, position.y, 0f);

        //currentBall.SetActive(true);

        currentRb = currentBall.GetComponent<Rigidbody2D>();

        if (currentRb != null)
        {
            currentRb.linearVelocity = Vector2.zero;
            currentRb.angularVelocity = 0f;
            currentRb.gravityScale = 0f;
            currentRb.bodyType = RigidbodyType2D.Kinematic;
            currentRb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        targetPosition = position;
    }

    void DropBall()
    {
        if (currentBall == null)
            return;

        GameObject droppedBall = currentBall;
        Rigidbody2D rb = currentRb;

        // Đặt đúng vị trí chuột cuối cùng
        if (rb != null)
        {
            rb.position = targetPosition;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.gravityScale = 1f;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.WakeUp();
        }
        else
        {
            droppedBall.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);
        }

         // Chỉ trừ số lượng khi Ball được thả
        objectManager.DecreaseObj();

        // Update GameManager
        gameManager.UpdateBall();

        // Giải phóng quyền điều khiển Ball
        // Ball tiếp tục rơi, không trả về Pool
        currentBall = null;
        currentRb = null;
    }

    void DeactivateBall()
    {
        if (currentBall == null)
            return;

        if (currentRb != null)
        {
            currentRb.linearVelocity = Vector2.zero;
            currentRb.angularVelocity = 0f;
            currentRb.gravityScale = 0f;
            currentRb.bodyType = RigidbodyType2D.Kinematic;
        }

        currentBall.SetActive(false);

        currentBall = null;
        currentRb = null;
    }
}