using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float moveUpSpeed = 40f; // Pixels per second
    public float duration = 1f;

    private float timer;
    private RectTransform rectTransform;
    private Vector3 moveDirection = Vector3.up;
    private Canvas parentCanvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas == null)
        {
            parentCanvas = FindObjectOfType<Canvas>();
            if (parentCanvas != null)
            {
                transform.SetParent(parentCanvas.transform, false);

            }

        }
    }

    public void Setup(int damage, Vector3 worldPosition)
    {
        textMesh.text = $"Damage Dealt: {damage}";

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition + Vector3.up * 1f);
        rectTransform.position = screenPos;
        Debug.Log($"[DamagePopup] Setup called. Damage: {damage}, ScreenPos: {screenPos}");
    }

    void Update()
    {
        rectTransform.position += moveDirection * moveUpSpeed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > duration)
            Destroy(gameObject);
    }
}