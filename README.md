# iMotions Meta Unity Plugin

This Unity plugin simplifies the process of exporting eye-tracking data and in-app recordings for integration with the iMotions research platform.

## ✅ Features

- ✅ Captures eye-tracking data from Meta Quest PRO headsets using Meta SDK  
- ✅ Records in-app footage using AVPro Movie Capture (Free Trial supported)  
- ✅ Automatically formats and saves data for iMotions ingestion  
- ✅ Designed for Android builds (e.g. Meta Quest Pro)

## 📂 Export Location

All exported files are saved to the following path on the Android device:

```
storage/Android/data/<your.app.package.name>/files/imotions/
```

> 📌 Note: This location is accessible via `adb pull` or through device file explorers with proper permissions.

## 📦 Installation

1. Ensure your Unity project uses:
   - Unity **2022.3.x** or later
2. Import this plugin via Git or Unity Package Manager.
   - **AVPro Movie Capture** (Free Trial allowed and included in the package)

## 🧰 Usage

1. Add the plugin to your Unity project.
2. Ensure your project is configured for Meta OpenXR.
3. Drag the "iMotions-Systems-Meta" prefab from the package/Runtime/Prefabs to your scene.
4. Build to an Android device with eye tracking support.
5. Run the app — use the input method you defined (controller input or UI buttons) and data will be captured and stored automatically in the `/files/imotions/` directory.

## 📝 Licensing

This plugin includes third-party components. See [`Third Party Notices.md`](./Third%20Party%20Notices.md) for license details.

---

### 🧑‍💻 Author

**CoFlowVisuals**  
For support or collaboration, please contact: https://www.coflowvisuals.com/  contact@coflowvisuals.com