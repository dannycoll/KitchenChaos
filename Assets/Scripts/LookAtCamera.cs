using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
  private enum Mode
  {
    LookAt,
    LookAtInverted,
    CameraForward,
    CameraForwardInverted
  }
  [SerializeField] private Mode mode;
  private void LateUpdate()
  {
    switch (mode)
    {
      case Mode.LookAt:
        if (Camera.main != null) transform.LookAt(Camera.main.transform);
        break;
      case Mode.LookAtInverted:
        if (Camera.main != null)
        {
          var transform1 = transform;
          var position = transform1.position;
          Vector3 dirFromCamera = position - Camera.main.transform.position;
          transform.LookAt(position + dirFromCamera);
        }

        break;
      case Mode.CameraForward:
        if (Camera.main != null) transform.forward = Camera.main.transform.forward;
        break;
      case Mode.CameraForwardInverted:
        if (Camera.main != null) transform.forward = -Camera.main.transform.forward;
        break;
    }
  }
}
