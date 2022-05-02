using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharChangeCell : MonoBehaviour
{
    public int num = 0;

    public void OnClick()
    {
        transform.parent.GetComponent<CharChangeCellGrid>().ChangeCostume(num);
    }
}
