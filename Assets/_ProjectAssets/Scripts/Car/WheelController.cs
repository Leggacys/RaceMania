using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private WheelCollider wheelCollider;
    [SerializeField] private Transform wheelMesh;

    void Update()
    {
        UpdateWheelPose();
    }

    private void UpdateWheelPose()
    {
        if (wheelCollider == null || wheelMesh == null) return;
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelMesh.position = pos;
        wheelMesh.rotation = rot;
    }
}
