using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeDisplayState {
    NONE,
    PLAY,
    REWIND,
    END
}
public class TimeDisplay : MonoBehaviour
{
    Material mat;
    MeshRenderer mr;
    static TimeDisplay instance;
    public Texture play;
    public Texture rewind;
    public Texture end;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        mr = GetComponent<MeshRenderer>();
        mr.material = new Material(mr.material);
        mat = mr.material;
    }
    public static void SetState(TimeDisplayState state) {
        if(state== TimeDisplayState.NONE) {
            instance.mr.enabled = false;
            return;
        }
        instance.mr.enabled = true;
        switch (state) {
            case TimeDisplayState.PLAY:
                instance.mat.mainTexture = instance.play;
                break;
            case TimeDisplayState.REWIND:
                instance.mat.mainTexture = instance.rewind;
                break;
            case TimeDisplayState.END:
                instance.mat.mainTexture = instance.end;
                break;
        }


    }


}
