using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stage : MonoBehaviour
{
    public abstract void Entrance();

    public abstract void Exit();

    public abstract void Pause();

    public abstract void Resume();

    public abstract void HandleOutOfBoundBody(GameObject outOfBoundBody);

    public abstract void HandleHit(GameObject hitBy);
}
