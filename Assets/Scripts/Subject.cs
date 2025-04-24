using System.Collections.Generic;
using UnityEngine;

public abstract class Subject<T> : MonoBehaviour
{
    private readonly List<IObserver<T>> observers = new List<IObserver<T>>();

    public void Subscribe(IObserver<T> observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
            // Notifica o novo observador com o estado atual (opcional, dependendo da lógica)
            NotifyObserver(observer, GetState());
        }
    }

    public void Unsubscribe(IObserver<T> observer)
    {
        observers.Remove(observer);
    }

    protected void NotifyObservers(T data)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(data);
        }
    }

    // Método abstrato para obter o estado atual do Subject
    protected abstract T GetState();

    // Método para notificar um único observador com um dado específico
    protected void NotifyObserver(IObserver<T> observer, T data)
    {
        observer?.OnNotify(data);
    }
}