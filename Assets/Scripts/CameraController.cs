using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera cam;

    public Transform followObject;
    public float moveSpeed;
    public Vector2 limitX;
    public float normalViewSize;
    public float timeZoomNormal;

    public Transform computer;
    public float computerViewSize;
    public float timeZoomComputer;

    public Transform tablet;
    public float tabletViewSize;
    public float timeZoomTablet;

    public GameObject onZoom;
    public GameObject onUnZoom;


    private float currentViewSize;
    private Vector3 normalPosition;
    private bool isZoomed = false;
    private bool isZooming = false;
    private GameObject zoomButton;
    private float z;

    private bool isMoving = false;
    private int moveDirection = 0;

    private bool isStuck = false;
    
    void Start()
    {
        Physics2D.queriesHitTriggers = true;

        currentViewSize = normalViewSize;
        cam.m_Lens.OrthographicSize = currentViewSize;
        
        cam.Follow = followObject;

        z = followObject.localPosition.z;

        normalPosition = followObject.localPosition;
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 position = followObject.localPosition;
            position += Vector3.right * moveDirection * moveSpeed * Time.deltaTime;
            position = new Vector3(Mathf.Clamp(position.x, limitX.x, limitX.y), position.y, position.z);
            followObject.localPosition = position;
        }
    }

    public void ZoomToComputer(GameObject button)
    {
        if (!isZooming && !isStuck)
        {
            isZooming = true;

            button.SetActive(false);
            zoomButton = button;

            isMoving = false;

            StartCoroutine(objectZoom(computer, computerViewSize));
        }
    }

    public void ZoomToTablet(GameObject button)
    {
        if (!isZooming && !isStuck)
        {
            isZooming = true;
            
            button.SetActive(false);
            zoomButton = button;

            isMoving = false;

            StartCoroutine(objectZoom(tablet, tabletViewSize));
        }
    }

    public void OnCancel()
    {
        if (isZoomed && !isZooming && !isStuck)
        {
            isZoomed = false;
            isZooming = true;

            StartCoroutine(unZoom());
        }
    }

    public void MoveCamera(int direction)
    {
        if (!isZoomed && !isZooming && !isStuck)
        {
            moveDirection = direction;
            isMoving = true;
        }
    }

    public void StopCamera()
    {
        isMoving = false;
    }

    public void GetStuck()
    {
        isStuck = true;

        isMoving = false;

        if (isZoomed)
        {
            isZoomed = false;
            isZooming = true;

            StartCoroutine(unZoom());
        }
    }
    
    public void Unstuck()
    {
        isStuck = false;
    }

    public void AddZoomGameObject(GameObject d)
    {
        onZoom = d;
    }

    public void AddUnZoomGameObject(GameObject d)
    {
        onUnZoom = d;
    }

    IEnumerator objectZoom(Transform obj, float viewSize)
    {
        float time = 0.0f;
        normalPosition = new Vector3(obj.localPosition.x, followObject.localPosition.y, z);
        followObject.localPosition = new Vector3(obj.localPosition.x, obj.localPosition.y, z);

        while (time <= timeZoomComputer)
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(currentViewSize, viewSize, Mathf.SmoothStep(0.0f, 1.0f, time / timeZoomComputer));

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        cam.m_Lens.OrthographicSize = viewSize;

        if (onZoom != null) onZoom.SetActive(true);
        onZoom = null;

        currentViewSize = viewSize;

        isZooming = false;
        isZoomed = true;
    }

    public IEnumerator unZoom()
    {
        float time = 0.0f;
        followObject.localPosition = new Vector3(normalPosition.x, normalPosition.y, z);

        while (time <= timeZoomNormal)
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(currentViewSize, normalViewSize, Mathf.SmoothStep(0.0f, 1.0f, time / timeZoomNormal));

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        cam.m_Lens.OrthographicSize = normalViewSize;

        if (onUnZoom != null) onUnZoom.SetActive(false);
        onUnZoom = null;

        zoomButton.SetActive(true);
        currentViewSize = normalViewSize;

        isZooming = false;
    }

    public IEnumerator FinishingUnZoom()
    {
        if (isZoomed && !isZooming && !isStuck)
        {
            isZoomed = false;
            isZooming = true;

            yield return StartCoroutine(unZoom());
        }
    }
}
