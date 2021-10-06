using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFX : MonoBehaviour
{
   public void Finish()
    {
        // 在动画的最后一帧，不展示动画
        gameObject.SetActive(false);
    }
}
