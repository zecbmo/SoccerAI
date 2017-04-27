using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBase : MonoBehaviour {

    static float TotalMeasuredTime = 0.0f;
    static int FucntionTests = 0;

    static System.Diagnostics.Stopwatch SW = null;

    public static void AddToMeassuredTime(float TimeDifference)
    { 

        TotalMeasuredTime += TimeDifference;
        FucntionTests++;
    }

    public static void PrintFindings()
    {

        float GameTime = SW.ElapsedMilliseconds;

        float TimeUsedPersecond = TotalMeasuredTime/ GameTime;
        float TimePercentageUse = TimeUsedPersecond * 100.0f;

        Debug.Log("Game Time  = " + GameTime);
        Debug.Log("Total Time Calculating Fuzzy System  = " + TotalMeasuredTime);
        Debug.Log("Time Used Per Second by Fuzzy System  = " + TimeUsedPersecond);
        Debug.Log("Performance use by Fuzzy System in %  = " + TimePercentageUse);
        Debug.Log("Total Funciton calls %  = " + FucntionTests);
    }

    static public float GetStartTime()
    {
        if (SW == null)
        {
            SW = System.Diagnostics.Stopwatch.StartNew();
        }

        return SW.ElapsedMilliseconds;
        
    }

    static public float GetEndTime()
    {
        return SW.ElapsedMilliseconds;
    }


}
