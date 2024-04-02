using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Follow : MonoBehaviour
{
    public float speed = 2;
    public Transform player;
    public float stoppingDistance = 2;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);

            if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
            {
                Destroy(player.gameObject);
                GameOver();
            }
        }
    }
    void GameOver()
    {
        Debug.Log("Игра окончена!");
        SceneManager.LoadSceneAsync(0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
