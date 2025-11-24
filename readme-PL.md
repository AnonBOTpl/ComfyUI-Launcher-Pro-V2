# 🇵🇱 ComfyUI Launcher Pro - Wersja Polska

Lekki, niezależny launcher napisany w C# oraz zaawansowany generator (HTML/JS) dla **ComfyUI Portable**.
Zaprojektowany, aby zastąpić standardowe pliki `.bat` i zapewnić wygodną pracę bez skomplikowanej konfiguracji.

## ✨ Główne Funkcje

*   **🚀 Natywny Launcher:** Szybki i stabilny program `.exe`, nie obciąża systemu.
*   **🎨 Generator Workflow:** Wbudowany, nowoczesny interfejs do generowania.
*   **⚡ Optymalizacja:** Łatwe przełączniki dla trybów `--lowvram`, `--fast` (dla kart RTX).
*   **📦 Generowanie Seryjne (Batch):** Twórz serie obrazów z losowym ziarnem (seed).
*   **🌐 Tłumacz Offline:** Wbudowany tłumacz AI (Polski -> Angielski), który działa w przeglądarce bez internetu.
*   **🛑 Skuteczne Zatrzymywanie:** Przycisk, który faktycznie czyści kolejkę i przerywa pracę.
*   **🛠️ Konserwacja:** Narzędzia do czyszczenia logów i cache'u.

## 📥 Instalacja i Użycie

1.  Pobierz najnowszą wersję z zakładki **[Releases (Wydania)](../../releases)**.
2.  Wypakuj pliki (`ComfyLauncher.exe`, `WebView2Loader.dll` oraz folder `gui`).
3.  Umieść je w głównym folderze **ComfyUI Portable** (tam, gdzie zwykle są pliki `run_nvidia_gpu.bat`).
    *   *Struktura folderów:*
    ```text
    ComfyUI_windows_portable/
    ├── ComfyUI/
    ├── python_embeded/ (lub python_standalone)
    ├── ComfyLauncher.exe  <-- Tutaj wrzuć
    ├── WebView2Loader.dll
    └── gui/
        └── index.html
    ```
4.  Uruchom `ComfyLauncher.exe`.
