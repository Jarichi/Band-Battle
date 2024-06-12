using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexableButton : MonoBehaviour
{
    public int Index { get; private set; }
    public void SetIndex(int index)
    {
        Index = index;
    }
}
