using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerPhysicsModifier : MonoBehaviour
{
    [SerializeField] private Transform characterModel;
    [SerializeField] private CinemachineCamera cmCamera;
    [SerializeField] private CinemachineInputAxisController inputController;
    public int GravityDirection { get; private set; } = 1;
    public bool IsIceFriction { get; private set; } = false;
    private bool isLevel2;
    private bool isLevel5;
    private float defaultXGain;
    private float defaultYGain;

    private CharacterController cc;

    private void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        isLevel2 = (sceneName == "Level2");
        isLevel5 = (sceneName == "Level5");

        cc = FindFirstObjectByType<CharacterController>();

        if (inputController != null && inputController.Controllers.Count >= 2)
        {
            defaultXGain = inputController.Controllers[0].Input.Gain;
            defaultYGain = inputController.Controllers[1].Input.Gain;
        }
    }

    private void OnEnable() => TimeManager.OnTimeSwapped += HandlePhysicsChange;
    private void OnDisable() => TimeManager.OnTimeSwapped -= HandlePhysicsChange;

    private void Start()
    {
        HandlePhysicsChange(TimeManager.Instance.IsFuture);
    }

    private void HandlePhysicsChange(bool isFuture)
    {
        if (isLevel2)
        {
            GravityDirection = isFuture ? 1 : -1;
            MusicManager.Instance.PlayGravityInversion(transform.position);
            if (characterModel != null)
            {
                characterModel.localScale = new Vector3(1, GravityDirection, 1);
            }

            if (cc != null)
            {
                Vector3 newCenter = cc.center;
                newCenter.y = isFuture ? 1f : -1f;
                cc.center = newCenter;
            }

            if (cmCamera != null)
            {
                var lens = cmCamera.Lens;
                lens.Dutch = isFuture ? 0 : 180;
                cmCamera.Lens = lens;
            }

            if (inputController != null && inputController.Controllers.Count >= 2)
            {
                var controllerX = inputController.Controllers[0];
                var inputX = controllerX.Input;
                inputX.Gain = isFuture ? defaultXGain : -defaultXGain;
                controllerX.Input = inputX;
                inputController.Controllers[0] = controllerX;

                var controllerY = inputController.Controllers[1];
                var inputY = controllerY.Input;
                inputY.Gain = isFuture ? defaultYGain : -defaultYGain;
                controllerY.Input = inputY;
                inputController.Controllers[1] = controllerY;
            }
        }

        if (isLevel5)
        {
            IsIceFriction = !isFuture;
        }
    }
}