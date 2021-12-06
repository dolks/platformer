using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            FindObjectOfType<GameSession>().IncreaseScore(100);
        }
    }
}
