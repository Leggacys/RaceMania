using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private float normalXDamping = 1.5f;
    [SerializeField] private float driftXDamping = 0.2f;
    [SerializeField] private float dampingChangeSpeed = 3f;

    private CinemachineTransposer transposer;
    private Coroutine dampingRoutine;

    void OnEnable()
    {
        CarController.onDriftStateChanged += OnDriftStateChanged;
    }

    void OnDisable()
    {
        CarController.onDriftStateChanged -= OnDriftStateChanged;
        if (dampingRoutine != null)
        {
            StopCoroutine(dampingRoutine);
        }
    }

    void Start()
    {
        transposer = virtualCam.GetCinemachineComponent<CinemachineTransposer>();
    }



    public void OnDriftStateChanged(bool isDrifting)
    {
        float targetDamping = isDrifting ? driftXDamping : normalXDamping;

        if (dampingRoutine != null)
            StopCoroutine(dampingRoutine);

        dampingRoutine = StartCoroutine(SmoothXDampingChange(targetDamping));
    }

    private IEnumerator SmoothXDampingChange(float target)
    {
        float start = transposer.m_XDamping;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * dampingChangeSpeed;
            transposer.m_XDamping = Mathf.Lerp(start, target, t);
            yield return null;
        }

        transposer.m_XDamping = target;
    }
}
