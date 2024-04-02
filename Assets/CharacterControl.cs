using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterControl : MonoBehaviour
{
    [Header("Персонаж")]
    [SerializeField][Range(2, 8)][Space] public float Speed;
    [SerializeField] private float DirectionX, DirectionY;
    [Space] public SpriteRenderer sr;
    public Rigidbody2D rb;
    public int Counter;
    [HideInInspector]
    public GameObject Arrow;
    [HideInInspector]
    public TextMeshProUGUI ShowCounter;

    [HideInInspector]
    public bool isGround;

    [SerializeField] private AnimationCurve _speedCurve;
    private float currentTime;
    private float totalTime;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        ShowCounter.text = $"Количество собранных звёзд {Counter}";

        totalTime = _speedCurve.keys[_speedCurve.keys.Length - 1].time;
    }

    // Update is called once per frame
    void Update()
    {
        MoveAndJump();
        ShootArrow();

        // Управление скоростью с помощью AnimationCurve
        float speedMultiplier = _speedCurve.Evaluate(currentTime);
        Vector2 movement = new Vector2(DirectionX, DirectionY) * Speed * speedMultiplier;
        transform.Translate(movement * Time.deltaTime);

        currentTime += Time.fixedDeltaTime;
        if (currentTime > totalTime)
        {
            currentTime = 0;
        }
    }

    void MoveAndJump()
    {
        DirectionX = Input.GetAxisRaw("Horizontal");
        DirectionY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(DirectionX, DirectionY);

        transform.Translate(movement * Speed * Time.deltaTime);

        if (DirectionX < 0)
        {
            sr.flipX = true;
        }
        else if (DirectionX > 0)
        {
            sr.flipX = false;
        }

        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.up * 10;
            isGround = false;
        }
    }

    void ShootArrow()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newArrow = Instantiate(Arrow) as GameObject;
            newArrow.transform.position = this.transform.position;
            Rigidbody2D rb = newArrow.GetComponent<Rigidbody2D>();
            if (this.transform.localScale.x > 0)
            {
                rb.AddForce(transform.up * 8, ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Stars")
        {
            Destroy(collision.gameObject);
            Counter++;
            ShowCounter.text = $"Количество собранных звёзд {Counter}";

            if (Counter >= 8)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        Debug.Log("Вы завершили уровень!");
        SceneManager.LoadSceneAsync(0);
    }
}
