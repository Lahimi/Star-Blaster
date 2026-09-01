using UnityEngine;
using UnityEngine.EventSystems; // Required for Hover events

public class ButtonJuice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float scaleAmount = 1.1f;
    [SerializeField] float transitionSpeed = 10f;
    Vector3 originalScale;
    Vector3 targetScale;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // Smoothly lerp the scale for a professional feel
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);
    }

    // This runs when the mouse enters the button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleAmount;
    }

    // This runs when the mouse leaves the button area
    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}