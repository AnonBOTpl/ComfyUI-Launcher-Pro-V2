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

5.  ## 🎨 Instrukcja obsługi Generatora

1.  **Przygotuj Workflow:**
    *   ⚠️ **WAŻNE:** W ustawieniach ComfyUI musisz włączyć "Dev mode Options", a następnie wyeksportować plik używając przycisku **"Save (API Format)"**. Standardowy zapis workflow nie zadziała!
    *   📥 **[Pobierz przykładowy Workflow](sample_workflows/workflow_api.json)** aby przetestować generator od razu.
2.  **Konfiguracja:**
    *   Kliknij **"Wybierz plik JSON"**, aby wczytać swój plik w formacie API.
    *   *(Opcjonalnie)* Kliknij **"Testuj"**, aby sprawdzić połączenie z serwerem.
3.  **Dostosowanie Interfejsu:**
    *   Kliknij **"🔳 Wybierz Nody"**, aby zdecydować, które parametry chcesz edytować (np. Prompty, Seed, Model). Odznaczone nody zostaną ukryte, aby zachować czystość interfejsu.
4.  **Ustawienia Generowania:**
    *   **Liczba obrazów (Batch):** Ustal, ile obrazków ma zostać wygenerowanych w serii.
    *   **Losowy Seed:** Zaznacz, aby każdy obrazek w serii był unikalny.
    *   **🌐 Tłumacz (AI):** Użyj wbudowanego narzędzia, aby przetłumaczyć prompt z polskiego na angielski (działa offline).
5.  **Wyniki:**
    *   **Lewy przycisk myszy** na obrazku otwiera go w trybie **Pełnoekranowym**.
    *   Kliknij **"💾 Zapisz"**, aby zapisać plik na dysku.
    *   Kliknij **"⛔ Zatrzymaj wszystko"**, aby natychmiast przerwać generowanie i wyczyścić kolejkę.
