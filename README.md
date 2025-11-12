# Perceptual Pathways: A VR Platform for Body Dysmorphic Disorder Research

This repository contains the final build and demonstration assets for **Perceptual Pathways**, a virtual reality (VR) system designed to support cognitive reframing therapy in individuals with body dysmorphic disorder (BDD). The system presents socially ambiguous scenarios in a controlled VR environment, allowing users to engage in real-time interpretation tasks and receive structured cognitive feedback.

The project was developed using Unity cross-platform game engine and deployed to the Meta Quest 2 headset. It includes features such as:
- First-person embodiment with full-body avatar tracking.
- Avatar customisation and third-person "mirror" perspective.
- Interactive decision-making tasks with cognitive reframing.
- Real-time behavioural data logging for clinical analysis.

#### The APK build and a full walkthrough video recorded directly from the Meta Quest 2 headset are available via Google Drive:
- 🔗 [Download APK and Watch Walkthrough](https://drive.google.com/drive/folders/1VF9Yam-7mecqGjuPditFbFVN0Wfpp9D5)
  
## 🛠 Requirements

- **Meta Quest 2** headset (with developer mode enabled)
- USB-C cable or wireless ADB connection
- A computer with [Android Debug Bridge (ADB)](https://developer.android.com/tools/adb) installed

## 🚀 Installation Instructions

To install and run the APK on your Meta Quest 2:

### 1. Enable Developer Mode on Meta Quest 2
- Open the **Meta Quest mobile app** on your phone.
- Go to **Menu > Devices > Developer Mode**.
- Toggle **Developer Mode** ON.
- Reboot the headset.

### 2. Install ADB on Your Computer
- Download the [Android SDK Platform Tools](https://developer.android.com/studio/releases/platform-tools).
- Extract the zip file to a known location (e.g., `C:\platform-tools` or `~/platform-tools`).

### 3. Connect the Headset
- Plug your Meta Quest 2 into your computer via USB-C.
- Put on the headset and **allow USB debugging** when prompted.

### 4. Install the APK
Open a terminal or command prompt and navigate to the platform-tools directory. Then run:

```bash
adb install PerceptualPathways.apk
```

### 4. Launch the application
- Put on your headset.
- Open the Apps menu.
- From the dropdown in the top-right, select Unknown Sources.
- Launch Perceptual Pathways.

