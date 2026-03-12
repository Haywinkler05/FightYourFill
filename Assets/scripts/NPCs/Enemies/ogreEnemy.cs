using UnityEngine;

public class ogreEnemy : Enemy
{
    [Header("StateInfo")]
    public AnimationClip rageState;
    private float ogreRageHealthBonus = 20f;
    public float ogre_rage_health_bonus => ogreRageHealthBonus;
    [Header("StateMachine")]
    [SerializeField]
    private string CurrentStateName;

    protected override void intializeStates()
    {
        currentState = new spawnState(this);
        CurrentStateName = currentState.GetType().Name;
    }

    protected override void Update()
    {
        base.Update();
        CurrentStateName = currentState.GetType().Name;
    }

    private void onFootFall()
    {
        if(wanderSFX == null)
        {
            return;
        }
        audioPlayer.pitch = Random.Range(pitchMin, pitchMax);
        audioPlayer.PlayOneShot(wanderSFX);
    }

}
