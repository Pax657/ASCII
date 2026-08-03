using UnityEngine;
using System;

public class debouncedValue<T> 
{
    private readonly float delay; //Tiempo de espera antes de aplicar el cambio
    private readonly Action<T> onCommit; //Acción a ejecutar cuando se confirme el cambio

    private T pendingValue; //Valor pendiente de cambio
    private float timer; //Temporizador para el retraso
    private bool hasPendingChange; //Indica si hay un cambio pendiente

    //constructor
    public debouncedValue(float delay, Action<T> onCommit)
    {
        this.delay = delay;
        this.onCommit = onCommit;
        this.hasPendingChange = false;
    }

    //se llama cada que el valor cambia (por ejemplo, desde un slider o input field)
    public void SetValue(T newValue)
    {
        pendingValue = newValue;
        timer = delay;
        hasPendingChange = true;
    }

    //Se llama una vez por frame, típicamente desde el Update() del MonoBehaviour
    public void Tick(float deltaTime)
    {
        if (!hasPendingChange) return;

        timer -= deltaTime;
        if (timer <= 0f)
        {
            hasPendingChange = false;
            onCommit?.Invoke(pendingValue);
        }
    }
}
