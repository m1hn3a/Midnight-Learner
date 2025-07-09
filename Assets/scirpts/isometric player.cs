using UnityEngine;

public class IsoPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Collider2D freezeZone;
    public Sprite idleSprite;
    public SpriteRenderer freezeIndicator;

    public AnimationClip walkLeft;
    public AnimationClip walkRight;
    public AnimationClip walkUp;
    public AnimationClip walkDown;
    public AnimationClip idle;

    private Vector2 input;
    private bool isFrozen = false;
    private bool inFreezeZone = false;
    private Rigidbody2D rb;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private string currentAnimation = "";
    private string lastHorizontal = "Right";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (freezeIndicator != null)
            freezeIndicator.enabled = false;
    }

    void Update()
    {
        // Toggle freeze only if inside freeze zone
        if (inFreezeZone && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isFrozen = !isFrozen;

            if (isFrozen)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                animator.enabled = false;
                spriteRenderer.enabled = false;

                if (freezeIndicator != null)
                    freezeIndicator.enabled = true;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                spriteRenderer.enabled = true;

                if (freezeIndicator != null)
                    freezeIndicator.enabled = false;
            }
        }

        if (!isFrozen)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            input.Normalize();

            bool isMoving = input.magnitude > 0.01f;

            if (isMoving)
            {
                animator.enabled = true;

                if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                {
                    if (input.x > 0)
                    {
                        lastHorizontal = "Right";
                        PlayAnimation(walkRight.name);
                    }
                    else
                    {
                        lastHorizontal = "Left";
                        PlayAnimation(walkLeft.name);
                    }
                }
                else
                {
                    if (input.y > 0)
                        PlayAnimation(walkUp.name);
                    else
                        PlayAnimation(walkDown.name);
                }
            }
            else
            {
                SetIdleSprite();
            }
        }
        else
        {
            input = Vector2.zero;
            SetIdleSprite();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == freezeZone)
        {
            inFreezeZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == freezeZone)
        {
            inFreezeZone = false;

            // Auto unfreeze if player exits zone
            if (isFrozen)
            {
                isFrozen = false;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                spriteRenderer.enabled = true;

                if (freezeIndicator != null)
                    freezeIndicator.enabled = false;
            }
        }
    }

    private void PlayAnimation(string animationName)
    {
        if (currentAnimation != animationName)
        {
            animator.Play(animationName);
            currentAnimation = animationName;
        }
    }

    private void SetIdleSprite()
    {
        animator.enabled = false;
        spriteRenderer.sprite = idleSprite;
        currentAnimation = "";
    }
}
