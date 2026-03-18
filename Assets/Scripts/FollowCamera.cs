using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField]
    private float speed = 1.0f;
    
    [SerializeField]
    GameObject target;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lerp2d = Vector2.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.position = new Vector3(lerp2d.x, lerp2d.y, transform.position.z);    
    }
}