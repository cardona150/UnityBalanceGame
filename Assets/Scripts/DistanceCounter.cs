using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class DistanceCounter : MonoBehaviour
{
    public Text distance;
    private Stopwatch stopwatch = new Stopwatch();
    // Start is called before the first frame update
    void Start()
    {
        stopwatch.Start();
    }

    // Update is called once per frame
    void Update()
    {
        distance.text = stopwatch.Elapsed.TotalSeconds.ToString();
    }
}
