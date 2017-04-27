using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSTimer : MonoBehaviour {

    float deltaTime = 0.0f;
    float fps = 0;
    float totalFPS = 0.0f;
    float totalFramesCounted;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        totalFPS += fps;
        totalFramesCounted++;
    }

    public void printFPS()
    {
        Debug.Log("Total FPS = " + totalFPS);
        Debug.Log("Total Frames Counted" + totalFramesCounted);

        float averageFPS = totalFPS / totalFramesCounted;
        Debug.Log("Average FPS = " + averageFPS);
    }
}
