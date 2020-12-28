using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingPhysics : MonoBehaviour
{
    public float appliedForce;
    public float rotationPerInput;
    public float landVelocityLimit;

    private Rigidbody2D rb;
    private Collider2D cd;

    private bool isInZone;

    public LayerMask LandingLayer;
    public LayerMask NotInZone;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
    }



    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector2.up * appliedForce * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z - rotationPerInput);
            //rb.AddTorque();   not doing this because it will continue to spin and i want to rotate it in another way
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + rotationPerInput);
            //rb.AddTorque();   not doing this because it will continue to spin and i want to rotate it in another way
        }

    }

    void FixedUpdate()
    {
        if (IsSuccessfullyLanded() && rb.velocity.y < landVelocityLimit || !isInZone)
        {
            StartCoroutine(MissionFailed());
        }
        else if (IsSuccessfullyLanded() && rb.velocity.y > landVelocityLimit)
        {
            Debug.Log("YOU LANDED!");
        }

    }

    bool IsSuccessfullyLanded()
    {
        float extraHeightText = 0.1f;
        RaycastHit2D raycasthit = Physics2D.BoxCast(cd.bounds.center, cd.bounds.size, 0f, Vector2.down, extraHeightText, LandingLayer);

        if (Physics2D.BoxCast(cd.bounds.center, cd.bounds.size, 0f, Vector2.down, extraHeightText, NotInZone))
        {
            isInZone = false;
        }
        else
        {
            isInZone = true;
        }
        return raycasthit.collider != null;
    }

    IEnumerator MissionFailed()
    {
        rb.AddForce(new Vector2(2000f, 3000f));
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}

