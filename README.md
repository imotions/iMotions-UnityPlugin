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

- Unity version 2022.3 or higher (fully tested on 2022.3.40f1)
- Build settings platform set to Android
- Inside Unity go to: "Window/Package Manager"
- Click the "+" sign on top left upper corner
- Then "Add package from git URL"
- Paste "https://github.com/imotions/iMotions-UnityPlugin.git#vive"

Follow the iMotions-Unity-Plugin_vVive Doc for step by step process.


## 📝 Licensing

This plugin includes third-party components. See [`Third Party Notices.md`](./Third%20Party%20Notices.md) for license details.


## Support
If you contact iMotions support we can help you with:
- Advice on how the Unity plugin can be useful for your research goals and study design
- Troubleshooting specific problems with the plugin

Unfortunately we do not have the resources to help you with:
- Learning programming
- Setting up your computer for software development
- Debugging your programs
- Untangling AI-generated code that does not work

If you need this type of help we recommend finding a colleague or graduate student with some programming knowledge to assist you instead.
