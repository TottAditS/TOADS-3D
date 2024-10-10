using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Twin : MonoBehaviour
{
    public GameObject orang;

    public float duration;
    public Vector3 endval;

    public float duration_move;
    public Vector3 endval_move;

    private void Update()
    {
        orang.transform.DORotate(endval, duration,RotateMode.FastBeyond360);
        orang.transform.DOMove(endval_move, duration_move);
    }
}
