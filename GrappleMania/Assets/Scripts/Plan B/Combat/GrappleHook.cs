using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : Projectile
{
    Player player;
    PlayerShoot playerShoot;
    LineRenderer lr;
    [SerializeField]
    AudioClip rappel;
    AudioSource source;

    Firing grappleGun;

    [SerializeField]
    public float playerTravelSpeed;

    public static bool hooked;

    private float currentDistance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.LocalPlayer;
        playerShoot = player.PlayerShoot;
        grappleGun = playerShoot.GrappleGun;
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.clip = rappel;
        // hook's decay starts
        base.Decay();
    
    }

    // Update is called once per frame
    void Update()
    {
        // hook is travelling if not hooked on something
        if(hooked == false)
            base.Travel();

        // keep track of distance between player and hook
        currentDistance = Vector3.Distance(player.transform.position, transform.position);

        DrawRope();

        // while hooked, move player towards hook and stop moving hook
        if (hooked == true)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, Time.deltaTime * playerTravelSpeed);
            float distanceToHook = Vector3.Distance(player.transform.position, transform.position);
            // deletes the grapple if the player reaches it before the decay finishes and releases the hook
            if (distanceToHook < 1)
            {
                hooked = false;
                EndLine();
                Destroy(gameObject);

            }
        }
        // After 3 seconds, remove the line since the hook is destroyed
        StartCoroutine(ExecuteAfterTime(2.7f));
    }

    void OnTriggerEnter(Collider other)
    {
        
        // if hookable, mark as hooked
        if (other.CompareTag("Grappleable"))
        {
            hooked = true;
            print("Grappled: " + other.name);
            source.Play();
        }
    }

    void DrawRope()
    {
        lr = grappleGun.GetComponent<LineRenderer>();

        lr.positionCount = 2;

        lr.SetPosition(0, grappleGun.muzzle.position);
        lr.SetPosition(1, transform.position);
    }

    void EndLine()
    {
        lr.positionCount = 0;
    }

    void PlayRappel()
    {

    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        EndLine();
        print("Line should be destroyed");
    }
}
