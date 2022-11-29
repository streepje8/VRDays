using SG;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public SG_BasicGesture fingerGuns;
    public float moveSpeed = 10f;
    public Transform handOrientation;

    private CharacterController cc;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (fingerGuns)
        {
            if (fingerGuns.IsGesturing)
            {
                Quaternion rotation = Quaternion.Euler(0, handOrientation.eulerAngles.y, 0);
                cc.Move(rotation * Vector3.forward * (moveSpeed * Time.deltaTime));
            }
        }
    }
}
