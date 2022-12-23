using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Checkers")]
    public Collider2D headCollider;
    public Transform headChecker;
    public float headCheckerRadius;
    public Transform groundChecker;
    public float groundCheckerRadius;
    public LayerMask groundLayer;


    [Header("Movement")]
    public Vector3 respawnPosition;
    public Rigidbody2D rigidBody;
    public MoveStyle moveStyle = MoveStyle.Velocity;
    public float jumpPower = 5f;
    public float speedModifier = 1f;
    
    
    [Header("Animation")]
    public Animator animator;
    public string movingConditionName;
    public string crouchDetectorName;
    public string jumpDetectorName;
    public string groundedDetectorName;
    public string ySpeedName;
    
    [Header("UI")]
    public Slider slider;
    public TMP_Text text;
    
    private bool _faceRight = true;
    private bool _grounded = true;

    private int _healthPoints;
    private int _maxHealthPoints = 20;

    // Start is called before the first frame update
    void Start()
    {
        _healthPoints = 20;
        transform.position = respawnPosition;
        slider.maxValue = _maxHealthPoints;
        slider.value = _healthPoints;
        text.text = $"{_healthPoints} HP";
    }

    // Update is called once per frame
    void Update()
    {
        
        //change style
        ChangeMoveStyle();

        //Movement
        Vector2 velocity = rigidBody.velocity;
        _grounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, groundLayer);
            
        //get horizontal axis for movement
        float x = Input.GetAxisRaw("Horizontal");
        
        //Animation
        animator.SetBool(movingConditionName, x != 0 && _grounded);
        animator.SetBool(groundedDetectorName, _grounded);
        animator.SetFloat(ySpeedName, rigidBody.velocity.y);
        
        //Crouch
        if (Input.GetKeyDown(KeyCode.C) && x == 0)
        {
            headCollider.enabled = !headCollider.enabled;
        }
        animator.SetBool(crouchDetectorName, !headCollider.enabled);
        
        //Movement
        if(_grounded && headCollider.enabled)
        {
            Movement(x);
        }
        
        //Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (_grounded && headCollider.enabled)
            {
                animator.SetBool(jumpDetectorName, true);
                Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius);
                rigidBody.velocity = new Vector2(velocity.x, jumpPower);
            }
        }
        else
        {
            animator.SetBool(jumpDetectorName, false);
        }
        
        //Reset
        if (Input.GetKeyDown((KeyCode.R)))
        {
            transform.position = respawnPosition;
        }
    }
    
    //Getting damage
    private void OnTriggerEnter2D(Collider2D col)
    {
        ArrowScript arrowScript = col.GetComponent<ArrowScript>();
        if (arrowScript != null)
        {
            int hit = arrowScript.getDamage();
            _healthPoints -= hit;
            showUI();
            Destroy(arrowScript.gameObject);
        }

        HealScript healScript = col.GetComponent<HealScript>();
        if (healScript != null)
        {
            _healthPoints += healScript.Heal();
            if (_healthPoints > _maxHealthPoints)
            {
                _healthPoints = _maxHealthPoints;
            }
            showUI();
            Destroy(healScript.gameObject);
        }

        if (_healthPoints <= 0)
        {
            Dead();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TeleporterScript teleport = other.GetComponent<TeleporterScript>();
        if (teleport != null && Input.GetKeyDown(KeyCode.E))
        {
            transform.position = teleport.getTeleporterPosition();
        }
    }

    void showUI()
    {
        slider.value = _healthPoints;
        text.text = $"{(_healthPoints < 0 ? 0 : _healthPoints)} HP";
    }

    void Dead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(headChecker.position, headCheckerRadius);
    }

    private void Movement(float x)
    {
        if (x > 0 && !_faceRight)
        {
            Flip();
        }

        if (x < 0 && _faceRight)
        {
            Flip();
        }

        switch (moveStyle)
        {
            case MoveStyle.Position:
                transform.position += transform.right * speedModifier * x;
                break;
            case MoveStyle.Velocity:
                rigidBody.velocity = new Vector2(speedModifier * x, rigidBody.velocity.y);
                break;
            case MoveStyle.Force:
                rigidBody.AddForce(Vector2.right * speedModifier * x);
                break;
            case MoveStyle.Translate:
                transform.Translate(Vector2.right * speedModifier * x);
                break;
            case MoveStyle.MovePosition:
                rigidBody.MovePosition(rigidBody.position + Vector2.right * speedModifier * x);
                break;
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        _faceRight = !_faceRight;
    }

    private void ChangeMoveStyle()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            speedModifier = 5f;
            moveStyle = MoveStyle.Velocity;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            speedModifier = 1f;
            moveStyle = MoveStyle.Force;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            speedModifier = 0.05f;
            moveStyle = MoveStyle.Position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            speedModifier = 0.05f;
            moveStyle = MoveStyle.Translate;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            speedModifier = 0.1f;
            moveStyle = MoveStyle.MovePosition;
        }
    }
}

public enum MoveStyle
{
    MovePosition,
    Position,
    Velocity,
    Force,
    Translate
}
