using UnityEngine;

public class TurretView : TowerViewBase
{
    protected override void OnDied(object sender)
    {
        
    }

    protected override void OnAttacked()
    {
        
    }

    protected override void OnAttacking(object args)
    {
        var target = (ITarget)args;
        Head.LookAt(target.Transform);
    }

    protected override void OnDamaged()
    {
        
    }
}
