# iMotions VIVE OpenXR Unity Plugin

This Unity plugin simplifies the process of exporting eye-tracking data and in-app recordings for integration with the iMotions research platform.

## ✅ Features

- ✅ Captures eye-tracking data from VIVE headsets using VIVE OpenXR SDK  
- ✅ Records in-app footage using AVPro Movie Capture (Free Trial supported)  
- ✅ Automatically formats and saves data for iMotions ingestion  
- ✅ Designed for Android builds (e.g. VIVE Focus 3)

## 📂 Export Location

All exported files are saved to the following path on the Android device:

```
storage/Android/data/<your.app.package.name>/files/imotions/
```

> 📌 Note: This location is accessible via `adb pull` or through device file explorers with proper permissions.

## 📦 Installation and Usage

Follow the iMotions-Unity-Plugin_vVive Doc for step by step process.


## 📝 Licensing

This plugin includes third-party components. See [`Third Party Notices.md`](./Third%20Party%20Notices.md) for license details.

---

### 🧑‍💻 Author

**CoFlowVisuals**  
For support or collaboration, please contact: https://www.coflowvisuals.com/  contact@coflowvisuals.com