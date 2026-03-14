using UnityEngine;

public class spiderling : spiderEnemy
{
    protected override void intializeStates()
    {
        base.intializeStates();
    }
    protected override void Start()
    {
        base.Start();
        hasCocooned = true;
    }

    protected override void Update()
    {
        base.Update();
    }
}
