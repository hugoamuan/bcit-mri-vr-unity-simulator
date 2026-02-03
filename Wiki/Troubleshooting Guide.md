# 🛠️ Common Troubleshooting Tips
- Also in the [Installation & Setup page](https://bitbucket.org/vie_bcit/mri-vr-room4/wiki/Installation)

- **APK not visible**:  
    → Ensure the headset was in file transfer mode when connected.

- **Installation blocked**:  
    → Enable installation from unknown sources under `Settings > Developer Options`.

- **App not launching correctly**:  
    → Check that the headset is running the latest firmware.

- **APK not installing**:
    1. Reconnect the headset to your computer
    2. From your computer, move the APK in the headsets `Download` folder to your desktop
    3. On your Headset, delete the old APK
    4. On your headset, go to `Settings > Storage`, find and select **Room4** and select **Uninstall**
    5. From your desktop, copy the new APK into the headsets `Download` folder
    6. On your headset go to the `Download` folder, select the new APK and press **Install** 

- **Simulation shows a black screen**:
    1. Press the **menu button** on the right controller and select **Quit**
    2. If that doesn't work, press the **power button** to put the headset to sleep, then press it again to wake it up

- **Gradle not building**: Many different fixes for this.
  - Check that you are building to Android.
    1. Open **File** settings on the top left corner
    2. Select **Build Settings**
    3. On the **Platform**, on the left side, select **Android**
  - Check your player settings and make sure you are selecting the proper minimum and maximum Android version
    1. Open **File** settings on the top left corner
    2. Select **Build Settings**
    3. On the bottom left corner, select **Player Settings**
    4. Under **Cursor Hotspot**, select **Android** if not selected
    5. Under **Settings for Android**, search and expand **Other Settings**
    6. Under the **Identification** sub-section select **Minimum API Level** as **Android 7.1 ‘Nougat’ (API Level 25)**
    7. Under the **Identification** sub-section select **Target API Level** as **Automatic (highest installed)**
  - If, after all this, if gradle is still not building, check and delete your **Gradle cache**.
    1. On Windows: delete `C:\Users\<your-user>\.gradle\caches`
    2. On macOS/Linux: delete `~/.gradle/caches`
    3. Additionally, you can also delete Unity’s `Library/` to force Unity to reimport any gradle dependency

- **Project building to 2D instead of 3D, or not building to VR**: Make sure that the **OpenXR** plugin is being activated when building.
  1. Open **Edit** settings on the top left corner
  2. Select **Project Settings**
  3. On the left side, open **XR Plug-In Management**
  4. Make sure that the **Android** icon is selected
  5. Enable **Initialize XR on Startup**
  6. Under **Plug-In Providers**, enable **OpenXR** and **VIVE XR Support feature group**
  7. On the left side, under **XR Plug-In Management**, click on **OpenXR**
  8. Under the **Android icon** tab, under **OpenXR Feature Groups**, make sure that **VIVE XR Support** on the left and right tabs (scroll down on the right tab)