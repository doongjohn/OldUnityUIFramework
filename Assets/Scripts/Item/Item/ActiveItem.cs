using UnityEngine;

public abstract class ActiveItem : Item
{
    [SerializeField] private float cooldown;

    private float curCooldown = 0f;

    public abstract void Activate();
}
