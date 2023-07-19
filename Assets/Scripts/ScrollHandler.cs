using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ScrollHandler : MonoBehaviour
{
    public Vector2 limitY;
    public Scrollbar scrollbar;

    public float scrollAmount;

    public void OnScroll()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(limitY.y, limitY.x, scrollbar.value), transform.localPosition.z);
    }

    void Update()
    {
        Vector2 scrollValue = Mouse.current.scroll.ReadValue().normalized;
        scrollbar.value = Mathf.Clamp01(scrollbar.value + scrollAmount / (limitY.y - limitY.x) * scrollValue.y);
    }
}
