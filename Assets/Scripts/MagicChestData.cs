using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class MagicChestData
{
    public DateTime LastDateChecked;
    public float RestockTime;
    public float Timer;
    public bool Restocking = false;
    public bool ReadyToCollect = false;

}
