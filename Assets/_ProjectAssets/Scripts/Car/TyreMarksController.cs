using UnityEngine;

public class TyreMarksController : MonoBehaviour
{
    [SerializeField] private TrailRenderer[] trailRenderer;
    [SerializeField] private LayerMask groundLayer;

    void OnEnable()
    {
        CarController.onDriftStateChanged += CheckDrift;
    }

    void OnDisable()
    {
        CarController.onDriftStateChanged -= CheckDrift;
    }

    void LateUpdate()
    {
        foreach (TrailRenderer trailObj in trailRenderer)
        {
            Vector3 pos = trailObj.transform.position;
            float yRotation = transform.eulerAngles.y;
            trailObj.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            trailObj.transform.position = new Vector3(pos.x, GetGroundY(pos), pos.z);
        }
    }

    float GetGroundY(Vector3 pos)
    {
        if (Physics.Raycast(pos + Vector3.up, Vector3.down, out RaycastHit hit, 5f, groundLayer))
            return hit.point.y + 0.01f;
        return pos.y;
    }

    private void CheckDrift(bool drifting)
    {
        if (drifting)
        {
            StartEmitter();
        }
        else
        {
            StopEmitter();
        }
    }

    private void StartEmitter()
    {
        foreach (var trail in trailRenderer)
        {
            if (trail != null)
            {
                trail.emitting = true;
            }
        }
    }

    private void StopEmitter()
    {
        foreach (var trail in trailRenderer)
        {
            if (trail != null)
            {
                trail.emitting = false;
            }
        }
    }


}
