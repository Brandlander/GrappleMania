using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ammoText;

    PlayerShoot playerShoot;
    Reloader reloader;

    private void Awake()
    {
        GameManager.Instance.OnLocalPlayerJoined += HandleOnLocalPlayerJoined;
    }

    void HandleOnLocalPlayerJoined(Player player)
    {
        playerShoot = player.PlayerShoot;
        reloader = playerShoot.GrappleGun.reloader;
        reloader.OnAmmoChanged += HandleOnAmmoChanged;
    }

    void HandleOnAmmoChanged()
    {
        int amountInInventory = reloader.RoundsRemainingInInventory;
        int amountInClip = reloader.RoundsRemainingInClip;
        ammoText.text = string.Format("{0}/{1}", amountInClip, amountInInventory);
    }
}
