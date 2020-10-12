using System;

using UnityEngine;

public class MortalObject : MonoBehaviour
{
    public event Action Die = delegate { };

    protected virtual void OnDie()
    {
        Die?.Invoke();
    }
}