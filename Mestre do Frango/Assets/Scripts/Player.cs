using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed, jumpForce, moveSpeed, swipeY, swipeX, limiteX;
    [SerializeField] SoundSystem audioSystem;
    [SerializeField] Transform moto;
    [SerializeField] ParticleSystem penas;

    [SerializeField] GameObject restart;
    [SerializeField] GameObject globalVolume;

    float gravity = 1;
    float limiteMove;
    int score, nivel, scoreTemp;

    Rigidbody body;

    Vector2 startPos, endPos, direction2;

    bool isGround, isMovedLeft, isMovedRight, isDead, isRamp;

    private Vector3 verticalTargetPosition;

    [SerializeField] GameObject uiCoxinha;


    /// <summary>
    /// Sounds Player
    /// </summary>
    [SerializeField] AudioClip[] statusSound;

    [SerializeField] private UI active;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        limiteMove = limiteX;
        limiteX = 0;
        isDead = false;
    }

    private void FixedUpdate()
    {
        body.velocity = Vector3.forward * speed;
    }
    // Update is called once per frame
    void Update()
    {       
        if (!isDead)
        {
            Swipe();

            Moviment();
        }   

        //Manipulação de sons
        if(score <= 100 && nivel == 0)
        {
            GetComponent<AudioSource>().clip = statusSound[0];
            nivel = 1;
        }
        else if (score >= 101 && score <= 200 && nivel == 1)
        {
            GetComponent<AudioSource>().clip = statusSound[1];
            GetComponent<AudioSource>().Play();
            speed = 15;
            nivel = 2;
        }
        else if(score >= 201 && score <= 300 && nivel == 2)
        {
            GetComponent<AudioSource>().clip = statusSound[2];
            GetComponent<AudioSource>().Play();
            speed = 20;
            nivel = 3;
            scoreTemp = score;
        }
        else if (score >= scoreTemp + 100 && nivel == 3 && speed < 36)
        {
            scoreTemp = score;
            speed = speed + 5;

            if (speed > 30)
            {
                globalVolume.SetActive(true);
            }

            if ( speed > 36)
            {
                speed = 36;
                nivel = 4;
            }
        }
    }

    void Leftmove()
    {
        if (transform.position.x <= -limiteX)
            isMovedLeft = false;
    }

    void RightMove()
    {
        if (transform.position.x >= limiteX)
            isMovedRight = false;
    }

    void Moviment()
    {
        transform.position += new Vector3(0, 0, speed * Time.deltaTime);

        //chamada de movimento
        if (isMovedRight)
        {
            RightMove();
        }

        if (isMovedLeft)
        {
            Leftmove();
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(limiteX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);

        //JUMP  GRAVITY
        if(!isGround && !isRamp)
        {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
            Vector3 targetPosition = new Vector3(transform.position.x, verticalTargetPosition.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 15 * Time.deltaTime);
        }
    }

    private void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;

            direction2 = startPos - endPos;

            if (direction2.x < -swipeX)
            {
                limiteX = limiteMove;
                isMovedRight = true;
                isMovedLeft = false;

                if (transform.position.x == 0)
                {
                    limiteX = limiteMove;
                }
                else if (transform.position.x == -limiteMove)
                {
                    limiteX = 0;
                }

            }
            else
                if (direction2.x > swipeX)
                {
                    limiteX = -limiteMove;
                    isMovedLeft = true;
                    isMovedRight = false;

                    if (transform.position.x == 0)
                    {
                        limiteX = -limiteMove;
                    }
                    else if(transform.position.x == limiteMove)
                    {
                        limiteX = 0;
                    }
                }
                

            //Jump
            if (direction2.y < -swipeY)
            {
                if (isGround)
                {
                    //body.AddForce(transform.up * jumpForce);
                    verticalTargetPosition.y = jumpForce;

                    GetComponent<AudioSource>().clip = statusSound[5];
                    GetComponent<AudioSource>().Play();
                    GetComponent<AudioSource>().loop = false;

                    isGround = false;
                    isRamp = false;
                }
                else if (isRamp)
                {
                    //body.AddForce(transform.up * jumpForce);
                    verticalTargetPosition.y = jumpForce;

                    GetComponent<AudioSource>().clip = statusSound[5];
                    GetComponent<AudioSource>().Play();
                    GetComponent<AudioSource>().loop = false;

                    isGround = false;
                    isRamp = false;
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ramp"))
        {
            isRamp = false;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") /*|| collision.gameObject.CompareTag("Ramp")*/)
        {
            isRamp = false;
            isGround = true;
            collisionGrounds(0);
        }

        if(collision.gameObject.CompareTag("Ramp"))
        {
            isRamp = true;
            isGround = false;
            collisionGrounds(-19);
        }

        if (collision.gameObject.CompareTag("Damage"))
        {
            penas.Play();
            GetComponent<AudioSource>().clip = statusSound[3];
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().loop = false;
            speed = 0;

            StartCoroutine("Win");            
        }
    }

    void collisionGrounds(float angle)
    {
        transform.GetChild(0).transform.eulerAngles = new Vector3(angle, 0, 0);
        isGround = true;
        if (nivel > 2)
        {
            GetComponent<AudioSource>().clip = statusSound[nivel-1];
        }
        else
            GetComponent<AudioSource>().clip = statusSound[nivel];

        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = true;
    }
    
    IEnumerator Win()
    {
        yield return new WaitForSeconds(1);
        active.active = true;
        GetComponent<AudioSource>().clip = statusSound[4];
        uiCoxinha.SetActive(false);
        GetComponent<AudioSource>().Play();
        isDead = true;
        restart.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coxinha"))
        {
            score += 5;
            //other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            audioSystem.PlayAudioCoxinha();
        }

        if (other.CompareTag("CxCoxinha"))
        {
            score += 15;
            //other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            audioSystem.PlayAudioCx();
        }
    }

    public int SetScore()
    {
        return score;
    }
}
