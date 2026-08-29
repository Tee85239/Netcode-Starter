using StarterAssets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClient : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] PlayerInput playerInput;
    [SerializeField] StarterAssetsInputs starterAssetsInputs;
    [SerializeField] ThirdPersonController thirdPersonController;

    private void Awake()
    {
        playerInput.enabled = false;
        starterAssetsInputs.enabled = false;
        thirdPersonController.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            playerInput.enabled = true;
            starterAssetsInputs.enabled = true;
            
        }

        if (IsServer)
        {
            thirdPersonController.enabled = true;
        }
    }

    [Rpc(SendTo.Server)]
    private void UpdateServerRPC(Vector2 move, Vector2 look, bool jump, bool sprint)
    {
        starterAssetsInputs.MoveInput(move);
        starterAssetsInputs.LookInput(look);
        starterAssetsInputs.JumpInput(jump);
        starterAssetsInputs.SprintInput(sprint);
    }

    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
         }
        UpdateServerRPC(starterAssetsInputs.move,starterAssetsInputs.look,starterAssetsInputs.jump,starterAssetsInputs.sprint);
    }
}
