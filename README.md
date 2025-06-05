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

## 📦 Installation and Usage

- Unity version 2022.3 or higher (fully tested on 2022.3.40f1)
- Build settings platform set to Android
- Inside Unity go to: "Window/Package Manager"
- Click the "+" sign on top left upper corner
- Then "Add package from git URL"
- Paste "https://github.com/imotions/iMotions-UnityPlugin.git#meta"

Follow the iMotions-Unity-Plugin_vMeta Doc for step by step process.

## 📝 Licensing

This plugin includes third-party components. See [`Third Party Notices.md`](./Third%20Party%20Notices.md) for license details.