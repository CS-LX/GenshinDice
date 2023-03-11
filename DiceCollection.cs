using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DiceCollection : MonoBehaviour
{
    public List<GameObject> dices = new List<GameObject>();
    private bool haveRecorded;
    public List<ElementType> types = new List<ElementType>();
    [ContextMenu("开始模拟")]
    public void StartRecode()
    {
        foreach (GameObject obj in dices)
        {
            obj.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(10, 40), Random.Range(10, 40), Random.Range(10, 40));
            obj.GetComponent<Dice>().StartRecode();
        }
        SetColor();
        haveRecorded = true;
    }
    [ContextMenu("开始播放")]
    public void StartPaly()
    {
        foreach (GameObject obj in dices)
        {
            obj.GetComponent<Dice>().StartPlay();
        }
    }
    [ContextMenu("初始化")]
    public void SCH()
    {
        foreach (GameObject obj in dices)
        {
            obj.GetComponent<Dice>().初始化();
        }
        haveRecorded = false;
    }
    [ContextMenu("上色")]
    public void SetColor()
    {
        for (int i = 0; i < dices.Count; i++)
        {
            dices[i].GetComponent<Dice>().SetElement(types[Mathf.Clamp(i, 0, types.Count - 1)]);
            dices[i].GetComponent<Dice>().SetColor();
        }
    }
    private void FixedUpdate()
    {
        if (haveRecorded)
        {
            if (IsAllFinish())
            {
                haveRecorded = false;
                StartPaly();
            }
        }
    }
    private bool IsAllFinish()
    {
        foreach (GameObject dice in dices)
        {
            if (!dice.GetComponent<Dice>().recordEnd)
            {
                return false;
            }
        }
        return true;
    }
}
