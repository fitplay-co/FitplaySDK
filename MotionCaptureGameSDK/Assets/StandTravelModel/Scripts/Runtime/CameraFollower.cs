using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private float speedMove;
    [SerializeField] private float speedRotate;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private Transform target;

    private Vector3 offset;

    private void Awake() {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            var followPos = target.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, followPos, speedMove);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, speedRotate);
            transform.rotation = Quaternion.AngleAxis(rotation.x, transform.right) * transform.rotation;
            transform.rotation = Quaternion.AngleAxis(rotation.y, transform.up) * transform.rotation;
        }
    }
}
