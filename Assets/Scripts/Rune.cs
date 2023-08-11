using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public Player player;
    [SerializeField] private int requireOrbToWin = 0;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (player.getOrb() >= requireOrbToWin)
            {
                GameManager.Instance.LevelComplete();
            }
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (player.getOrb() >= requireOrbToWin)
            {
                GameManager.Instance.LevelComplete();
            }
        }
    }
    */
}
