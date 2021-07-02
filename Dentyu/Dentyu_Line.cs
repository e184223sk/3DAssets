using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dentyu_Line : MonoBehaviour
{
    public Dentyu_Line[] next;
    public float Dripping; 
    [SerializeField,Range(3,20)]
    uint CurvePolyCount;
    Vector3 getV(Vector3 a, Vector3 b, float v = 0.5f) => Vector3.Lerp(a, b, v) - Vector3.up * Dripping * Mathf.Abs((0.5f - v) * 2);

    void Start ()=> OnValidate();

    /// <summary>
    /// 処理軽減の為, 不要な子オブジェクトを破棄してこのコードも消す
    /// </summary>
    void Update()
    { 
        linerPoint me = new linerPoint(transform);
        DestroyImmediate(me.T0.gameObject);
        DestroyImmediate(me.T1.gameObject);
        DestroyImmediate(me.T2.gameObject);
        DestroyImmediate(me.T3.gameObject);
        DestroyImmediate(me.B0.gameObject);
        DestroyImmediate(me.B1.gameObject);
        DestroyImmediate(me.B2.gameObject);
        DestroyImmediate(me.DD.gameObject);
        if (transform.Find("Liner").GetComponent<LineRenderer>().positionCount <= 0)
            Destroy(transform.Find("Liner").gameObject); //自身のLineRenderから線が出てないなら破棄する
        Destroy(this);
    }



    void OnValidate()
    {
        if (next != null && next.Length > 0)
        {
            LineRenderer e = transform.Find("Liner").GetComponent<LineRenderer>();
            float v0 = 0.4f, v1 = 0.6f;
            List<Vector3> ne = new List<Vector3>();
            foreach (var i in next)
            {
                if (i != null)
                { 
                    linerPoint me = new linerPoint(transform);
                    linerPoint tar = new linerPoint(i.transform);

                    for (int v = 0; v < 5; v++) ne.Add(getV(me.T0.position, tar.T0.position, v * 0.25f));
                    for (int v = 0; v < 5; v++) ne.Add(getV(tar.T1.position, me.T1.position, v * 0.25f));
                    for (int v = 0; v < 5; v++) ne.Add(getV(me.T2.position, tar.T2.position, v * 0.25f));
                    for (int v = 0; v < 5; v++) ne.Add(getV(tar.T3.position, me.T3.position, v * 0.25f));
                    ne.Add(me.DD.position);
                    for (int v = 0; v < 5; v++) ne.Add(getV(me.B0.position, tar.B0.position, v * 0.25f));
                    for (int v = 0; v < 5; v++) ne.Add(getV(tar.B1.position, me.B1.position, v * 0.25f));
                    for (int v = 0; v < 5; v++) ne.Add(getV(me.B2.position, tar.B2.position, v * 0.25f)); 
                    for (int v = 0; v < 5; v++) ne.Add(getV(tar.B2.position, me.B2.position, v * 0.25f));
                    ne.Add(me.DD.position);
                    ne.Add(me.T0.position);
                }
            }

            e.positionCount = ne.Count;
            e.SetPositions(ne.ToArray());
            e.numCornerVertices = (int)CurvePolyCount;
        }
    } 

    
    struct linerPoint
    {
        public Transform T0, T1, T2, T3, B0, B1, B2,DD;

        public linerPoint(Transform t)
        {
            T0 = t.Find("pointT0");
            T1 = t.Find("pointT1");
            T2 = t.Find("pointT2");
            T3 = t.Find("pointT3");
            B0 = t.Find("pointB0");
            B1 = t.Find("pointB1");
            B2 = t.Find("pointB2");
            DD = t.Find("DowmPointer");
        }
    }
}


