using TMPro.Examples;
using UnityEngine;

public class Bow : MonoBehaviour
{

    public Transform arrowSpawnPoint;
    public GameObject arrowPrefab;
    public float arrowSpeed = 15f;
    public float arrowGravity = -9.8f;
    public int arrowPierce = 0;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootArrow()
    {
        var arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        arrow.GetComponent<Rigidbody>().linearVelocity = arrowSpawnPoint.forward * arrowSpeed;
    }
}
