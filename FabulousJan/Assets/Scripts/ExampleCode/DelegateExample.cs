using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateExample : MonoBehaviour
{

    public delegate void Died(string name, int age);
    public Died onDied;
    /*private IEnumerator Start()
    {
        int age = 50;
        int yearsDead = Random.Range(0, 6);
        yield return new WaitForSeconds(yearsDead);
        onDied?.Invoke("Leon", age + yearsDead);

    }*/

    public delegate void MathOperation(int num1, int num2);

    void Start()
    {
        MathOperation addNumAnswer, multiplyNumAnswer;

        addNumAnswer = addNum;
        multiplyNumAnswer = multiplyNum;

    }

    private void addNum(int num1, int num2)
    {
        print($"{num1} * {num2} = {num1 + num2}");
    }

    private void multiplyNum(int num1, int num2)
    {
        print($"{num1} * {num2} = {num1 * num2}");
    }


}
