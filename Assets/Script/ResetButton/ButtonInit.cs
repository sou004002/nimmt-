using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInit : MonoBehaviour
{
    private int ButtonNumber;
    private bool isClicked=false;

    public int GetButtonNumber()
    {
        return ButtonNumber;
    }

    public void SetButtonNumber(int num)
    {
        ButtonNumber=num;
    }

    public bool GetIsClicked()
    {
        return isClicked;
    }

    public void ToTrueIsClicked()
    {
        isClicked=true;
    }
}
