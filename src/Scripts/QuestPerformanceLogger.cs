using System;
using System.IO;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Profiling;
using System.Collections.Generic;

public class QuestPerformanceLogger : MonoBehaviour
{
    private string logFilePath;
    private float logInterval = 2f; // seconds
    private float nextLogTime;

    private float previousFrameTime;
    private int frameSpikeCount;

    // XR Devices
    private InputDevice headset;
    private InputDevice leftController;
    private InputDevice rightController;

    void Start()
    {
        // Prepare log file
        string folderPath = Path.Combine(Application.persistentDataPath, "PerformanceLogs");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        logFilePath = Path.Combine(folderPath, $"QuestPerformanceLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
        File.WriteAllText(logFilePath, "Quest Performance Log\n-----------------------\n");

        // Get XR devices correctly
        var headsetDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, headsetDevices);
        if (headsetDevices.Count > 0) headset = headsetDevices[0];

        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, leftHandDevices);
        if (leftHandDevices.Count > 0) leftController = leftHandDevices[0];

        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, rightHandDevices);
        if (rightHandDevices.Count > 0) rightController = rightHandDevices[0];
    }

    void Update()
    {
        if (Time.time >= nextLogTime)
        {
            LogPerformanceData();
            nextLogTime = Time.time + logInterval;
        }

        // Detect frame spikes (simple check)
        float frameTime = Time.unscaledDeltaTime * 1000f;
        if (previousFrameTime > 0 && Mathf.Abs(frameTime - previousFrameTime) > 10f) // 10ms jump threshold
        {
            frameSpikeCount++;
        }
        previousFrameTime = frameTime;
    }

    private void LogPerformanceData()
    {
        // Gather performance stats
        string appFPS = (Time.unscaledDeltaTime > 0) ? (1f / Time.unscaledDeltaTime).ToString("F1") : "0.0";
        string fpd = GetRefreshRate().ToString("F1");
        string totalMem = (Profiler.GetTotalAllocatedMemoryLong() / (1024f * 1024f)).ToString("F1") + " MB";
        string cpuUtilization = SystemInfo.processorCount + " cores @ " + SystemInfo.processorFrequency + "MHz";

        // Use suggested performance levels (newer API)
        string cpuLevel = OVRManager.suggestedCpuPerfLevel.ToString();
        string gpuLevel = OVRManager.suggestedGpuPerfLevel.ToString();

        // Headset tracking (ensure device is valid before using)
        Vector3 headAngVel = Vector3.zero, headAcc = Vector3.zero, headPos = Vector3.zero;
        Quaternion headRot = Quaternion.identity;
        if (headset.isValid)
        {
            headset.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out headAngVel);
            headset.TryGetFeatureValue(CommonUsages.deviceAcceleration, out headAcc);
            headset.TryGetFeatureValue(CommonUsages.devicePosition, out headPos);
            headset.TryGetFeatureValue(CommonUsages.deviceRotation, out headRot);
        }

        // Hand tracking status
        string handTrackingStatus = OVRInput.IsControllerConnected(OVRInput.Controller.Hands) ? "Enabled" : "Disabled";

        // Rendering stats
        string renderScale = XRSettings.eyeTextureResolutionScale.ToString("F2");
        string gpuOverdraw = "Approx. " + (XRSettings.renderViewportScale * 100f).ToString("F0") + "%";

        // Get thermal and performance info if available
        string thermal = "N/A";
        string perfHeadroom = "N/A";

        // Try to get performance metrics from OVRPlugin if available
        if (OVRPlugin.initialized)
        {
            perfHeadroom = "Normal"; // Default value since powerSavingMode is deprecated
        }

        // Compose the log
        string logEntry = $@"
Timestamp: {DateTime.Now:HH:mm:ss}
FPS: {appFPS}
Display Refresh Rate: {fpd} Hz
Frame Time Spikes: {frameSpikeCount}
CPU Level: {cpuLevel}
GPU Level: {gpuLevel}
Thermal Level: {thermal}
Performance State: {perfHeadroom}
CPU Info: {cpuUtilization}
GPU: {SystemInfo.graphicsDeviceName}
Graphics API: {SystemInfo.graphicsDeviceType}
Total Allocated Memory: {totalMem}
Render Scale: {renderScale}
Estimated Viewport Scale: {gpuOverdraw}
Hand Tracking: {handTrackingStatus}
Headset AngVel: {headAngVel.magnitude:F2}, Accel: {headAcc.magnitude:F2}
Headset Pose: pos={headPos}, rot={headRot.eulerAngles}
------------------------------------------------------------";

        File.AppendAllText(logFilePath, logEntry + "\n");
    }

    private float GetRefreshRate()
    {
        // Use XRDevice.refreshRate or fallback to OVRManager
        if (XRDevice.refreshRate > 0)
        {
            return XRDevice.refreshRate;
        }

        // Fallback: Try to get from display
        if (OVRPlugin.initialized)
        {
            return OVRPlugin.systemDisplayFrequency;
        }

        return 72f; // Default Quest refresh rate
    }
}

// FPS
//float fps = (Time.unscaledDeltaTime > 0) ? (1f / Time.unscaledDeltaTime) : 0f;

//using UnityEngine;
//using System.IO;
//using System;
//using System.Text;
//using UnityEngine.Profiling;
//using UnityEngine.SceneManagement;
//using UnityEngine.XR;
//using System.Collections.Generic;

//public class EnhancedQuestPerformanceLogger : MonoBehaviour
//{
//    [Tooltip("How often to log performance data, in seconds.")]
//    public float loggingInterval = 5.0f;

//    private string logFilePath;
//    private StringBuilder logBuilder;
//    private float timeSinceLastLog = 0f;

//    private Vector3 lastHeadAngVel;
//    private Vector3 lastRightControllerVel;
//    private Vector3 lastLeftControllerVel;

//    private float lastCpuTime;
//    private float lastGpuTime;
//    private float lastFrameTime;

//    public static EnhancedQuestPerformanceLogger Instance;

//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//            InitializeLogger();
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void InitializeLogger()
//    {
//        logBuilder = new StringBuilder();
//        string fileName = $"QuestPerformanceLog_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
//        logFilePath = Path.Combine(Application.persistentDataPath, fileName);

//        string header = "--- Meta Quest Performance Log ---\n\n";
//        header += $"Session Start Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n";
//        header += $"Device Model: {SystemInfo.deviceModel}\n";
//        header += $"Operating System: {SystemInfo.operatingSystem}\n";
//        header += $"Graphics API: {SystemInfo.graphicsDeviceType}\n";
//        header += $"GPU: {SystemInfo.graphicsDeviceName}\n";
//        header += $"Display Refresh Rate: {GetRefreshRate()} Hz\n\n";
//        header += $"Processor: {SystemInfo.processorType} ({SystemInfo.processorCount} cores)\n";
//        header += $"System Memory: {SystemInfo.systemMemorySize} MB\n\n";

//        logBuilder.AppendLine(header);

//        logBuilder.AppendLine(
//            "Timestamp | FPS | FrameTime(ms) | CPU Perf | GPU Perf | MemAlloc(MB) | CPUUtil(%) | HandTracking | " +
//            "HeadVel | HeadAcc | RCtrlVel | RCtrlAcc | LCtrlVel | LCtrlAcc | " +
//            "AngularVel | AngularAcc | Overdraw | Heat | " +
//            "SetPassCalls | DrawCalls | Triangles | Vertices | DroppedFrame?"
//        );

//        File.WriteAllText(logFilePath, logBuilder.ToString());
//        Debug.Log($"Performance logger initialized. Saving data to: {logFilePath}");
//    }

//    void Update()
//    {
//        timeSinceLastLog += Time.deltaTime;
//        if (timeSinceLastLog >= loggingInterval)
//        {
//            LogPerformanceMetrics();
//            timeSinceLastLog = 0f;
//        }
//    }

//    private void LogPerformanceMetrics()
//    {
//        string timestamp = DateTime.Now.ToString("HH:mm:ss");

//        // FPS & frame time
//        float fps = 1.0f / Time.deltaTime;
//        float frameTimeMs = Time.deltaTime * 1000f;
//        bool droppedFrame = frameTimeMs > (1000f / GetRefreshRate() * 1.5f); // drop threshold

//        long allocatedMemory = Profiler.GetTotalAllocatedMemoryLong() / 1048576;

//        // Use OVRManager performance levels
//        int cpuPerfLevel = (int)OVRManager.cpuLevel;
//        int gpuPerfLevel = (int)OVRManager.gpuLevel;

//        // Hand tracking status
//        bool isHandTracking = OVRInput.GetActiveController() == OVRInput.Controller.Hands;

//        // Headset motion
//        Vector3 headAngVel = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.None);
//        Vector3 headAcc = (headAngVel - lastHeadAngVel) / loggingInterval;
//        lastHeadAngVel = headAngVel;

//        // Controller motion
//        Vector3 rVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
//        Vector3 rAcc = (rVel - lastRightControllerVel) / loggingInterval;
//        lastRightControllerVel = rVel;

//        Vector3 lVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
//        Vector3 lAcc = (lVel - lastLeftControllerVel) / loggingInterval;
//        lastLeftControllerVel = lVel;

//        // Approximate CPU Utilization
//        float cpuUtil = (float)(System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds / (Environment.ProcessorCount * Time.realtimeSinceStartup * 10));

//        // GPU Overdraw (approximate, editor-only)
//        float overdraw = 0f;
//#if UNITY_EDITOR
//        overdraw = UnityEditor.UnityStats.screenOverdraw;
//#endif

//        // Heat (mocked for now)
//        float deviceHeat = 0f;
//        if (OVRPlugin.initialized)
//        {
//            deviceHeat = (float)OVRPlugin.GetSystemTemperatureTrend();
//        }

//        // Rendering stats (editor only)
//        int setPassCalls = 0;
//        int drawCalls = 0;
//        int triangles = 0;
//        int vertices = 0;
//#if UNITY_EDITOR
//        setPassCalls = UnityEditor.UnityStats.setPassCalls;
//        drawCalls = UnityEditor.UnityStats.batches;
//        triangles = UnityEditor.UnityStats.triangles;
//        vertices = UnityEditor.UnityStats.vertices;
//#endif

//        // Log entry
//        string logEntry =
//            $"{timestamp} | {fps:F1} | {frameTimeMs:F1} | {cpuPerfLevel} | {gpuPerfLevel} | {allocatedMemory} | {cpuUtil:F1} | {(isHandTracking ? "YES" : "NO")} | " +
//            $"HVel:{headAngVel.magnitude:F2} | HAcc:{headAcc.magnitude:F2} | " +
//            $"RVel:{rVel.magnitude:F2} | RAcc:{rAcc.magnitude:F2} | " +
//            $"LVel:{lVel.magnitude:F2} | LAcc:{lAcc.magnitude:F2} | " +
//            $"{headAngVel.magnitude:F2} | {headAcc.magnitude:F2} | {overdraw:F2} | {deviceHeat:F2} | " +
//            $"{setPassCalls} | {drawCalls} | {triangles} | {vertices} | {(droppedFrame ? "YES" : "NO")}";

//        File.AppendAllText(logFilePath, logEntry + "\n");
//    }

//    private float GetRefreshRate()
//    {
//        // Modern XR API method
//        List<XRDisplaySubsystem> displays = new List<XRDisplaySubsystem>();
//        SubsystemManager.GetSubsystems(displays);

//        if (displays.Count > 0 && displays[0].TryGetDisplayRefreshRate(out float rate))
//            return rate;

//        if (OVRPlugin.initialized)
//            return OVRPlugin.systemDisplayFrequency;

//        return 72f; // default fallback
//    }

//    void OnApplicationQuit()
//    {
//        try
//        {
//            logBuilder.AppendLine($"\n--- Session Ended: {DateTime.Now:yyyy-MM-dd HH:mm:ss} ---");
//            File.AppendAllText(logFilePath, logBuilder.ToString());
//            Debug.Log("Performance log successfully saved on application quit.");
//        }
//        catch (Exception e)
//        {
//            Debug.LogError($"Failed to save final performance log: {e.Message}");
//        }
//    }
//}
