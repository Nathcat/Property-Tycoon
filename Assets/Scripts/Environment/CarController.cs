using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 1f;

    void Update()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, UnityEngine.Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CarWaypoint"))
        {
            Vector3[] vec = other.GetComponent<CarWaypoint>().turnVectors;
            transform.rotation = Quaternion.Euler(vec[Random.Range(0, vec.Length)]);
            transform.position = other.transform.position;
        }
    }
}
