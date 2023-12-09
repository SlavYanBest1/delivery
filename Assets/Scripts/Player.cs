using UnityEngine;

public class NewBehaviourScript : MonoBehaviour{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;
    private void Update() {    
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;

    }

    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, moveDir, out RaycastHit raycastHit, interactDistance)) {
            Debug.Log(raycastHit.transform);          
        } else {
            Debug.Log("-");
        }
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove) {
            // CAnnot move towards moveDir
            //only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) {
                //Can move only on the x 
                moveDir = moveDirX;
            } else {
                // Cannot move only on the X
                // Attempt only z movement 
                 Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                 canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                 if (canMove) {
                    // can move only on the z 
                    moveDir = moveDirZ;
                 } else {
                    // Cannot move in any direction
                 }
            }
        }

        if (canMove) {
          transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 14f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }
}
