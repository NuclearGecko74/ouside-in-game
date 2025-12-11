using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 targetPostition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPostition);
    }
}
