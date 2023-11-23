using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delegate : MonoBehaviour
{

    public DelegateExample _delegateExample;

    void Start()
    {
        print("Wait");
        _delegateExample.onDied += onDied;
    }

    private void onDied(string name, int age)
    {
        print($"Имя:{name}, Возвраст:{age}");
    }

}


public interface IMovable
{
    float Speed { get; }
    void Move();
}

public class PlayerMove : IMovable
{
    public float Speed { get; set; }

    public void Move()
    {
    
    }
}
