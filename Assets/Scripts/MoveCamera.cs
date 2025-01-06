using UnityEngine;

public class MoveCamera : MonoBehaviour{
    public Transform playerTarget;  
    public Vector3 playerOffset = new Vector3(0, 0, 0);  

    public Transform rocketTarget;  
    public Vector3 rocketOffset = new Vector3(0, 2, -5);  

    private enum CameraMode { FollowPlayer, FollowRocket }
    private CameraMode currentMode = CameraMode.FollowPlayer; 

    private void LateUpdate(){
        if (currentMode == CameraMode.FollowPlayer){
            if (playerTarget != null){
                transform.position = playerTarget.position + playerOffset;
                transform.LookAt(playerTarget);
            }
        }
        else if (currentMode == CameraMode.FollowRocket){
            if (rocketTarget != null){
                Vector3 dynamicOffset = rocketTarget.TransformDirection(rocketOffset);
                transform.position = rocketTarget.position + dynamicOffset;
                transform.LookAt(rocketTarget.position + rocketTarget.forward * 10f);
            }
        }
    }

    public void SetCameraModeToRocket(Transform rocket){
        currentMode = CameraMode.FollowRocket;
        rocketTarget = rocket; 
    }

    public void SetCameraModeToPlayer(){
        currentMode = CameraMode.FollowPlayer;
        rocketTarget = null; 
    }
}
