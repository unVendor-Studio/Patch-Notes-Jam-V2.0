using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    private BoxCollider boxCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Open()
    {
        boxCollider.isTrigger = true;
        animator.SetBool("Close", false);
        animator.SetBool("Open", true);
    }

    public void Close()
    {
        boxCollider.isTrigger = false;
        animator.SetBool("Close", true);
        animator.SetBool("Open", false);
    }
}
