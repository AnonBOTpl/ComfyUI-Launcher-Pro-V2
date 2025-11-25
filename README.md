# 🚀 ComfyUI Launcher Pro

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)](https://www.microsoft.com/windows)


A lightweight, standalone C# launcher and advanced HTML/JS Generator UI for **ComfyUI Portable**. 
Designed to replace the default bat files and provide a user-friendly experience without complex installation.

![Screenshot](https://github.com/AnonBOTpl/ComfyUI-Launcher-Pro-V2/blob/main/screen-Launcher.png)
![Screenshot](https://github.com/AnonBOTpl/ComfyUI-Launcher-Pro-V2/blob/main/screen-Generator.png)

## ✨ Key Features

*   **🚀 Native C# Launcher:** Fast, stable, no python dependencies for the launcher itself.
*   **🎨 Advanced Generator UI:** A custom HTML interface integrated directly into the app.
*   **⚡ Performance Tuning:** Easy toggles for `--lowvram`, `--fast`, `--fp16-vae`.
*   **📦 Batch Generation:** Generate multiple images in a sequence with random seeds.
*   **🌐 Offline Translator:** Built-in AI translator (PL -> EN) running locally in the browser.
*   **🛑 Real "Stop":** Force stop generation and clear queue immediately.
*   **🛠️ Maintenance Tools:** Built-in log cleaner and cache purger.
*   **📂 Portable Friendly:** Designed specifically for `ComfyUI_windows_portable`.

## 📥 Installation & Usage

1.  Download the latest release from the **[Releases Page](../../releases)**.
2.  Extract the files (`ComfyLauncher.exe`, `WebView2Loader.dll` and `gui` folder).
3.  Place them inside your root **ComfyUI Portable** folder (where `run_nvidia_gpu.bat` usually is).
    *   *Structure should look like this:*
    ```text
    ComfyUI_windows_portable/
    ├── ComfyUI/
    ├── python_embeded/ (or python_standalone)
    ├── ComfyLauncher.exe  <-- Place here
    ├── WebView2Loader.dll
    └── gui/
        └── index.html
    ```
4.  Run `ComfyLauncher.exe`.

## 🎨 How to use the Generator

1.  **Configuration:**
    *   Click **"Choose JSON file"** to load your ComfyUI workflow (API format).
    *   *(Optional)* Click **"Test Connection"** to verify if ComfyUI is running.
2.  **Customize Interface:**
    *   Click **"🔳 Select Nodes"** to choose which parameters you want to edit (e.g., Prompts, Seed, Checkpoint). Unselected nodes will remain hidden to keep the UI clean.
3.  **Generation Settings:**
    *   **Batch Count:** Set how many images to generate in a row.
    *   **Random Seed:** Check this to get a unique variation for every image in the batch.
    *   **🌐 Translate (AI):** Use the built-in offline tool to translate your prompts from Polish to English.
4.  **Results:**
    *   **Left Click** on a generated image to view it in **Fullscreen**.
    *   Click **"💾 Save PNG"** to save the image to your disk.
    *   Click **"⛔ Stop All"** to immediately cancel generation and clear the queue.

## 🔧 Requirements

*   Windows 10/11
*   ComfyUI Portable version
*   WebView2 Runtime (usually pre-installed on Windows)

