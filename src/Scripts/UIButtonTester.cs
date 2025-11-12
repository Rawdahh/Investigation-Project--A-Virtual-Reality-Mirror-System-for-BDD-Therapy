using UnityEngine;

public class UIButtonTester : MonoBehaviour
{
    // function is public so a Button can see it.
    public void TestClick()
    {
        Debug.Log("BUTTON CLICK DETECTED! The button itself is working.");
    }
}