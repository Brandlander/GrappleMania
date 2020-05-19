using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPCamera : MonoBehaviour
{
    [System.Serializable]
    public class CameraRig
    {
        public Vector3 CameraOffset;
        public float damping;
    }
    // 0.24 0.85 -2.87
    // 5
    [SerializeField]
    CameraRig defaultCamera;

    [SerializeField]
    CameraRig aimCamera;

    Transform cameraLookTarget;
    Player localPlayer;


    private PlayerAim m_PlayerAim;
    private PlayerAim PlayerAim
    {
        get
        {
            if (m_PlayerAim == null)
                m_PlayerAim = GameManager.Instance.LocalPlayer.playerAim;
            return m_PlayerAim;
        }
    }

    void Awake()
    {
        GameManager.Instance.OnLocalPlayerJoined += HandleOnLocalPlayerJoined;
    }

    // hooks camera onto player once they join through the gameManager
    void HandleOnLocalPlayerJoined(Player player)
    {
        localPlayer = player;
        cameraLookTarget = localPlayer.transform.Find("AimingPivot");

        if (cameraLookTarget == null)
        {
            cameraLookTarget = localPlayer.transform;
            print("No look target");
        }

    }

    void LateUpdate()
    {
        if (localPlayer == null)
            return;

        CameraRig cameraRig = defaultCamera;

        if (localPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMING || localPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMEDFIRING)
            cameraRig = aimCamera;

        float targetHeight = cameraRig.CameraOffset.y;

        Vector3 targetPos = cameraLookTarget.position + 
            localPlayer.transform.forward * cameraRig.CameraOffset.z + //forward offset
            localPlayer.transform.up * targetHeight + // vertical offset
            localPlayer.transform.right * cameraRig.CameraOffset.x; // horizontal offset

        Vector3 collisionDestination = cameraLookTarget.position + localPlayer.transform.up * targetHeight - localPlayer.transform.forward * .5f;

        HandleCameraCollision(collisionDestination, ref targetPos);

        transform.position = Vector3.Lerp(transform.position, targetPos, cameraRig.damping * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraLookTarget.rotation, cameraRig.damping * Time.deltaTime);

        // value is between .99 and -.99, lookTarget pos should be between 1.5 and 0
    }

    private void HandleCameraCollision(Vector3 toTarget, ref Vector3 fromTarget)
    {
        RaycastHit hit;
        if (Physics.Linecast(toTarget, fromTarget, out hit))
        {
            //deals with camera clipping through walls
            Vector3 hitPoint = new Vector3(hit.point.x + hit.normal.x * .2f, hit.point.y, hit.point.z + hit.normal.z * .2f);
            //deals with camera collision
            fromTarget = new Vector3(hitPoint.x, fromTarget.y, hitPoint.z);
        }
    }
}
