# 🚀 ComfyUI Launcher Pro

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)](https://www.microsoft.com/windows)


A lightweight, standalone C# launcher and advanced HTML/JS Generator UI for **ComfyUI Portable**. 
Designed to replace the default bat files and provide a user-friendly experience without complex installation.

![Screenshot](https://placehold.co/800x450?text=Add+Your+Screenshot+Here)
*(Add a screenshot of your app here)*

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

## 🔧 Requirements

*   Windows 10/11
*   ComfyUI Portable version
*   WebView2 Runtime (usually pre-installed on Windows)

---

## 🔧 Wymagania

*   System Windows 10 lub 11.
*   Zainstalowane ComfyUI w wersji Portable.
