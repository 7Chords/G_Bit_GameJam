using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    public int maxSteps = 10;
    private int remainingSteps;

    private void Start()
    {
        remainingSteps = maxSteps;
    }

    // ����Ƿ��в���
    public bool CanTakeStep()
    {
        return remainingSteps > 0;
    }

    // ���Ĳ���
    public void UseStep()
    {
        if (remainingSteps > 0)
        {
            remainingSteps--;
        }
    }

    // ���ò���
    public void ResetSteps()
    {
        remainingSteps = maxSteps;
        Debug.Log("����������");
    }

    // ��ȡ��ǰʣ�ಽ��
    public int GetRemainingSteps()
    {
        return remainingSteps;
    }

    //���õ�ǰʣ�ಽ��
    public void SetRemainSteps(int step)
    {
        remainingSteps = Mathf.Min(maxSteps, step);
        
    }
}
