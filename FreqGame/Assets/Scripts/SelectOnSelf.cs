using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectOnSelf : MonoBehaviour
{
    void Start()
    {
        // 自分を選択状態にする
        Selectable sel = GetComponent<Selectable>();
        sel.Select();
    }
}
