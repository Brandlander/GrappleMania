using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour
{
    [SerializeField] float rateOfFire;
    [SerializeField] Projectile projectile;
    [SerializeField] Transform hand;
    [SerializeField] AudioController audioFire;

    private LineRenderer lr;

    private Transform AimTarget;
    private Vector3 AimTargetOffset;

    public Reloader reloader;

    float nextFireAllowed;
    public Transform muzzle;

    public bool canFire;

    void Awake()
    {
        muzzle = transform.Find("Muzzle");
        reloader = GetComponent<Reloader>();

        transform.SetParent(hand);
    }

    public void Reload()
    {
        if (reloader == null)
            return;
        reloader.Reload();
    }

    public virtual void Fire()
    {
        canFire = false;

        if (Time.time < nextFireAllowed)
            return;

        if (Paused.GameIsPaused)
            return;

        if(reloader != null)
        {
            if (reloader.IsReloading)
                return;
            if (reloader.RoundsRemainingInClip == 0)
                return;

            reloader.TakeFromClip(1);
            print("Remaining : " + reloader.RoundsRemainingInClip);
        }

        nextFireAllowed = Time.time + rateOfFire;

        bool isLocalPlayerControlled = AimTarget == null;

        // useful in case additional players are added, don't have them shoot to the center of the local player's screen
        if (!isLocalPlayerControlled)
            muzzle.LookAt(AimTarget.position + AimTargetOffset);

        // instantiate the projectile;
        GrappleHook newHook = (GrappleHook)Instantiate(projectile, muzzle.position, muzzle.rotation);
        // shoots the projectile to the center of the screen accurately for at least 500 meters
        if(isLocalPlayerControlled)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            RaycastHit hit;
            Vector3 targetPosition = ray.GetPoint(500);
            Debug.DrawRay(ray.origin, ray.direction*100, Color.blue);
            //if the raycast hits something within 500 meters then it will send the projectile there
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag != "Player")
                targetPosition = hit.point;
            newHook.transform.LookAt(targetPosition);
        }

        audioFire.Play();
        canFire = true;
    }
}
