using UnityEngine;

public abstract class Parabola<T> : UISingletonBehaviour<T> where T : Parabola<T> {
    public float focusDistance = 100f;
    [Space(10f)]
    public int width = 1500;

    public abstract int steps { get; }

    protected int arcDistance { get { return width / (1+steps); } }

    protected Vector2 vertex { get { return new Vector2(0,0); } }
    protected float h { get { return vertex.x; } }
    protected float k { get { return vertex.y; } }
    protected float p { get { return focusDistance; } }
    protected Vector2 focus { get { return new Vector2(h, k+p); } }

    protected float sq4P { get { return Mathf.Sqrt(4 * p); } }
    protected float sqH { get { return Mathf.Sqrt(h); } }

    protected float h2 { get { return h*h; } }
    protected float p4 { get { return 4*p; } }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        // veretx
        Gizmos.DrawSphere(vertex, 10f);
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(focus, 10f);

        Gizmos.color = Color.red;
        for (int i = 0; i < steps; i++) {
            Gizmos.DrawSphere(GetPositionAtIndex(i), 20f);
        }

        Gizmos.color = Color.cyan;
        for (int x = -(width/2); x <= width/2; x++) {
            float tx = x + h;
            Gizmos.DrawLine(yParabolaPoint(tx - 1), yParabolaPoint(tx));
        }
        if (width <= 10) return;
        Gizmos.color = Color.magenta;
        for (int i = -(width/2); i <= width/2; i+=arcDistance) {
            float tx = i+h;
            Vector3 p = yParabolaPoint(tx);
            Gizmos.DrawLine(p, p+((p-new Vector3(focus.x, focus.y, 0)).normalized * 40f));
        }
    }

    protected Vector2 GetPositionAtIndex(int index) {
        if (width == 0) return Vector2.zero;
        float tx = h + ((-width / 2) + ((1+index) * arcDistance));
        Vector2 p = yParabolaPoint(tx);
        return p;
    }
    protected Vector2 GetNormalAtIndex(int index) {
        if (width == 0) return Vector2.up;
        float tx = h + ((-width / 2) + ((1 + index) * arcDistance));
        Vector2 p = yParabolaPoint(tx);
        Vector2 n = (p - new Vector2(focus.x, focus.y)).normalized;
        return n;
    }

    private float yParabola(float x) {
        float sqX = Mathf.Sqrt(x);
        float y = k + ((Mathf.Pow(x-h, 2))/p4);
        return y;
    }
    protected Vector3 yParabolaPoint(float x) {
        return new Vector3(x, yParabola(x), 0);
    }

    private float xParabola(float y)
    {
        float sqY = Mathf.Sqrt(y);
        float x = h + (sq4P * (sqY - sqH));
        return x;
    }
    protected Vector3 xParabolaPoint(float y) {
        return new Vector3(xParabola(y), y, 0);
    }
}
