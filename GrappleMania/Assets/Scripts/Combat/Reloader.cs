using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reloader : MonoBehaviour
{
    [SerializeField] int maxAmmo;
    [SerializeField] float reloadTime;
    [SerializeField] int clipSize;
    [SerializeField] Container inventory;

    public int shotsFiredInClip;
    bool isReloading;
    System.Guid containerItemId;

    //when there is a change in ammo count
    public event System.Action OnAmmoChanged;

    public int RoundsRemainingInClip
    {
        get
        {
            return clipSize - shotsFiredInClip;
        }
    }

    public int RoundsRemainingInInventory
    {
        get
        {
            return inventory.GetAmountRemaining(containerItemId);
        }
    }

    public bool IsReloading
    {
        get
        {
            return isReloading;
        }
    }

    void Awake()
    {
        //add gun and ammo to inventory
        containerItemId = inventory.Add(this.name, maxAmmo);
    }

    public void Reload()
    {
        if (isReloading)
            return;

        isReloading = true;

        print("Reload started!");
        GameManager.Instance.Timer.Add(() => {
            ExecuteReload(inventory.TakeFromContainer(containerItemId, clipSize - RoundsRemainingInClip));
            },reloadTime);
    }

    private void ExecuteReload(int amount)
    {
        print("Reload executed!");
        isReloading = false;

        print("Amount = " + amount);
        // add shots back to the clip
        shotsFiredInClip -= amount;

        if (OnAmmoChanged != null)
            OnAmmoChanged();
    }

    public void TakeFromClip(int amount)
    {
        shotsFiredInClip += amount;

        if (OnAmmoChanged != null)
            OnAmmoChanged();
    }

    public int currentAmmo()
    {
        return (maxAmmo + clipSize) - shotsFiredInClip;
    }
}
