using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

[ExecuteAlways]
public class TriangleController : MonoBehaviour
{
    [Header("Text Settings")]
    public string pointText = "100";
    public float fontSize = 3f;
    public Color textColor = Color.white;
    public Vector3 textOffset = Vector3.zero;

    private TextMeshPro textMesh;
    private GameManager gameManager;

    private void OnEnable()
    {
        UpdateText();
    }

    private void OnValidate()
    {
        UpdateText();
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void UpdateText()
    {
        if (Application.isPlaying)
            return;

        // Tìm chữ đã tạo để tránh tạo nhiều object
        Transform child = transform.Find("PointText");

        GameObject textObject;

        if (child == null)
        {
            textObject = new GameObject("PointText");
            textObject.transform.SetParent(transform, false);
        }
        else
        {
            textObject = child.gameObject;
        }

        textObject.transform.localPosition = textOffset;
        textObject.transform.localRotation = Quaternion.identity;
        textObject.transform.localScale = Vector3.one;

        textMesh = textObject.GetComponent<TextMeshPro>();

        if (textMesh == null)
            textMesh = textObject.AddComponent<TextMeshPro>();

        textMesh.text = pointText;
        textMesh.fontSize = fontSize;
        textMesh.color = textColor;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.rectTransform.sizeDelta = new Vector2(5f, 2f);

        textMesh.gameObject.hideFlags = HideFlags.None;
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
        if (!Application.isPlaying)
            return;

        if (!other.gameObject.CompareTag("Ball")) 
        {
            return;
        }

        BallController ball = other.gameObject.GetComponent<BallController>();

        if (ball == null || ball.hasScored)
        {
            return;
        }
        if (!int.TryParse(pointText, out int points))
        {
            Debug.LogWarning("PointText phải là số nguyên!", this);
            return;
        }

        if (GameManager.Instance == null)
            return;

        // Đánh dấu quả banh này đã ghi điểm
        ball.hasScored = true;

        // Cộng điểm vào GameManager
        GameManager.Instance.AddPoint(points);
    }
}