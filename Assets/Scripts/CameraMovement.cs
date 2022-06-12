using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float positionX;
    [SerializeField] float positionY;
    [SerializeField] float positionZ;  

    void Update()
    {
        transform.position = target.transform.position + new Vector3(positionX, positionY, positionZ);

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(
                transform.eulerAngles.x,
                transform.eulerAngles.y - 1,
                transform.eulerAngles.z
            );
        }

        if (Input.GetKey(KeyCode.D))
        {  
            transform.rotation = Quaternion.Euler(
                transform.eulerAngles.x,
                transform.eulerAngles.y + 1,
                transform.eulerAngles.z
            );
        }
        
    }
    
}
