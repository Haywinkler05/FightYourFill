using UnityEngine;

public class movementScript : MonoBehaviour
{
    GameObject player;

    public float moveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");


        Vector3 move = new Vector3(moveX, 0f, moveZ);


        transform.Translate(move * moveSpeed * Time.deltaTime);
    }
}
