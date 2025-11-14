using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEdgeVisual : MonoBehaviour
{
    private float z = 0;
    private float width = 0.25f;

    public void UpdateVisual(Vector3 Apos, Vector3 Bpos) {
        transform.localPosition = new Vector3((Apos.x + Bpos.x) / 2, (Apos.y + Bpos.y) / 2, z);

        transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(Apos.y - Bpos.y, Apos.x - Bpos.x));

        transform.localScale = new Vector3(Mathf.Pow(Mathf.Pow(Apos.y - Bpos.y, 2) + Mathf.Pow(Apos.x - Bpos.x, 2), 0.5f), width, 1f);
    }
}
