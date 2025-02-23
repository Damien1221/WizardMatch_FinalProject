using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject[] ObjPrefab;

    public float Movespeed = 5.0f;
    public bool firstBall;
    public Transform newposition;
    private Rigidbody2D rb;
    [SerializeField] private CoolDown _cooldown;

    public float x, y;

    protected GameObject currentObj;

    // Start is called before the first frame update
    void Start()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb = GetComponent<Rigidbody2D>();

        Spawning();
        firstBall = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (currentObj != null)
        {
            currentObj.transform.position = newposition.transform.position;
        }

        if (Input.GetKey(KeyCode.A) && transform.position.x > x)
        {
            rb.velocity = new Vector2(-Movespeed, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D) && transform.position.x < y)
        {
            rb.velocity = new Vector2(Movespeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_cooldown.CurrentProgress == CoolDown.Progress.Ready && firstBall == true)
            {
                Drop();
                _cooldown.StartCooldown();
                firstBall = false;
                Spawning();
            }

            else if (_cooldown.CurrentProgress == CoolDown.Progress.Ready && firstBall == false)
            {
                Drop();
                _cooldown.StartCooldown();
                Spawning();
            }

            if (_cooldown.CurrentProgress == CoolDown.Progress.Finished)
            {
                _cooldown.StopCooldown();
            }
        }
    }

    public void Spawning()
    {
        currentObj = Instantiate(ObjPrefab[Random.Range(0, ObjPrefab.Length)], newposition.transform.position, Quaternion.identity);
        currentObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;

    }

    public void Drop()
    {
        currentObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        currentObj = null;
    }
}
