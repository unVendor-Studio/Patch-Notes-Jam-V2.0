using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    public static DoorUnlock Instance;
    private Animator animator;
    private BoxCollider boxCollider;

    public int UnlockCount;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }
    void Update()
    {
        if (UnlockCount == 2)
        {
            Open();
        }
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
