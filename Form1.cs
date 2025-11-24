using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Collections.Generic;

namespace ComfyLauncher
{
    // Enum poziomów logów
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Success // Dodane dla operacji czyszczenia
    }

    public partial class Form1 : Form
    {
        // Kolory
        private readonly Color ColBack = Color.FromArgb(30, 30, 46);
        private readonly Color ColPanel = Color.FromArgb(45, 45, 60);
        private readonly Color ColAccent = Color.FromArgb(76, 175, 80);
        private readonly Color ColText = Color.White;
        private readonly Color ColConsole = Color.Black;
        private readonly Color ColButtonSecondary = Color.FromArgb(60, 60, 80);
        private readonly Color ColWarning = Color.FromArgb(255, 152, 0);
        private readonly Color ColError = Color.FromArgb(244, 67, 54);
        private readonly Color ColClean = Color.FromArgb(33, 150, 243); // Kolor dla czyszczenia

        // Fonty
        private Font FontBold = new Font("Segoe UI", 10, FontStyle.Bold);
        private Font FontHeader = new Font("Segoe UI", 18, FontStyle.Bold);
        private Font FontConsole = new Font("Consolas", 10, FontStyle.Regular);

        private TabControl tabs;
        private RichTextBox consoleBox;
        private Button btnStart, btnReopen, btnClearLogs, btnExportLogs;
        
        // Checkboxy konfiguracyjne
        private CheckBox chkLowVram, chkFast, chkCpu, chkFp16, chkGui, chkAutoScroll;
        
        // Checkboxy czyszczenia (NOWE)
        private CheckBox chkCleanFolders, chkCleanLogs, chkCleanPython;
        private Button btnRunClean;

        private ComboBox cmbLogFilter;
        private TextBox txtPort, txtIp, txtExtra;

        private Process comfyProcess;
        private ComfyConfig config;
        private Form generatorWindow;
        private bool isGenerating = false;
        
        // System tłumaczeń
        private string currentLanguage = "en";
        private Dictionary<string, Dictionary<string, string>> translations;

        public Form1()
        {
            // Auto-wykrywanie języka z Windows
            DetectSystemLanguage();
            InitializeTranslations();
            
            this.Text = T("app_title");
            this.Size = new Size(600, 950); // Zwiększona wysokość
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColBack;
            this.ForeColor = ColText;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;

            config = ComfyConfig.Load();
            InitializeCustomUI();
        }

        private void DetectSystemLanguage()
        {
            var culture = CultureInfo.CurrentUICulture;
            if (culture.TwoLetterISOLanguageName == "pl" || culture.Name.StartsWith("pl"))
            {
                currentLanguage = "pl";
            }
            else
            {
                currentLanguage = "en";
            }
        }

        private void InitializeTranslations()
        {
            translations = new Dictionary<string, Dictionary<string, string>>
            {
                ["en"] = new Dictionary<string, string>
                {
                    ["app_title"] = "ComfyUI Launcher Pro",
                    ["header"] = "ComfyUI Launcher",
                    ["tab_performance"] = "Performance",
                    ["tab_network"] = "Network",
                    ["tab_advanced"] = "Advanced & Tools",
                    ["lowvram"] = "--lowvram (Less VRAM)",
                    ["fast"] = "--fast (Faster for RTX)",
                    ["fp16"] = "--fp16-vae (Save VRAM)",
                    ["cpu"] = "--cpu (CPU only - slow)",
                    ["port"] = "Port:",
                    ["ip"] = "IP:",
                    ["autostart"] = "Autostart window",
                    ["extra_args"] = "Arguments (e.g. --enable-cors-header):",
                    ["btn_start"] = "START",
                    ["btn_stop"] = "STOP",
                    ["btn_reopen"] = "Open Generator Window ↗",
                    ["btn_clear_logs"] = "Clear Logs",
                    ["btn_export_logs"] = "Export Logs",
                    ["logs_label"] = "System Logs:",
                    ["log_filter"] = "Filter:",
                    ["filter_all"] = "All",
                    ["filter_errors"] = "Errors Only",
                    ["filter_warnings"] = "Warnings + Errors",
                    ["autoscroll"] = "Auto-scroll",
                    ["searching_python"] = "Searching for Python environment...",
                    ["found_in"] = "Found in:",
                    ["found_python"] = "Found Python:",
                    ["arguments"] = "Arguments:",
                    ["started"] = "ComfyUI started!",
                    ["waiting"] = "Waiting 3s for server start...",
                    ["window_opened"] = "Generator window opened",
                    ["stopping"] = "Stopping ComfyUI...",
                    ["stopped"] = "ComfyUI stopped",
                    ["error_python"] = "ERROR: python.exe not found!",
                    ["error_main"] = "ERROR: ComfyUI/main.py not found!",
                    ["error_start"] = "Startup error:",
                    ["error_port"] = "Port must be a number between 1-65535",
                    ["error_ip"] = "Invalid IP address\nExamples: 127.0.0.1, 0.0.0.0",
                    ["confirm_close"] = "ComfyUI is running. Are you sure you want to close?",
                    ["confirm"] = "Confirmation",
                    ["logs_exported"] = "Logs exported successfully!",
                    ["logs_cleared"] = "Logs cleared",
                    ["window_exists"] = "Generator window is already open",
                    ["no_image"] = "No image to save",
                    ["image_saved"] = "Image saved successfully!",
                    ["success"] = "Success",
                    
                    // Maintenance Section
                    ["maint_header"] = "Maintenance / Cleaning",
                    ["clean_folders"] = "Clean Folders (Output, Temp, WebView)",
                    ["clean_logs"] = "Clean Log Files (*.log)",
                    ["clean_python"] = "Clean Python (Cache, PIP, .pyc)",
                    ["btn_clean"] = "EXECUTE CLEANING",
                    ["clean_warn_running"] = "Cannot clean while ComfyUI is running!",
                    ["clean_start"] = "--- Starting Cleanup ---",
                    ["clean_done"] = "--- Cleanup Finished ---",
                    ["deleted"] = "Deleted:",
                    ["pip_purge"] = "Executing PIP Cache Purge...",
                    ["clean_error"] = "Clean Error:"
                },
                ["pl"] = new Dictionary<string, string>
                {
                    ["app_title"] = "ComfyUI Launcher Pro",
                    ["header"] = "ComfyUI Launcher",
                    ["tab_performance"] = "Wydajność",
                    ["tab_network"] = "Sieć",
                    ["tab_advanced"] = "Zaawansowane i Narzędzia",
                    ["lowvram"] = "--lowvram (Mniej VRAM)",
                    ["fast"] = "--fast (Szybciej dla RTX)",
                    ["fp16"] = "--fp16-vae (Oszczędność VRAM)",
                    ["cpu"] = "--cpu (Tylko procesor - wolne)",
                    ["port"] = "Port:",
                    ["ip"] = "IP:",
                    ["autostart"] = "Autostart okna",
                    ["extra_args"] = "Argumenty (np. --enable-cors-header):",
                    ["btn_start"] = "URUCHOM",
                    ["btn_stop"] = "ZATRZYMAJ",
                    ["btn_reopen"] = "Otwórz okno Generatora ↗",
                    ["btn_clear_logs"] = "Wyczyść logi",
                    ["btn_export_logs"] = "Eksportuj logi",
                    ["logs_label"] = "Logi Systemowe:",
                    ["log_filter"] = "Filtr:",
                    ["filter_all"] = "Wszystkie",
                    ["filter_errors"] = "Tylko błędy",
                    ["filter_warnings"] = "Ostrzeżenia + Błędy",
                    ["autoscroll"] = "Auto-przewijanie",
                    ["searching_python"] = "Szukanie środowiska Python...",
                    ["found_in"] = "Znaleziono w:",
                    ["found_python"] = "Znaleziono Python:",
                    ["arguments"] = "Argumenty:",
                    ["started"] = "ComfyUI uruchomione!",
                    ["waiting"] = "Czekam 3s na start serwera...",
                    ["window_opened"] = "Okno generatora otwarte",
                    ["stopping"] = "Zatrzymywanie ComfyUI...",
                    ["stopped"] = "ComfyUI zatrzymane",
                    ["error_python"] = "BŁĄD: Nie znaleziono python.exe!",
                    ["error_main"] = "BŁĄD: Nie znaleziono ComfyUI/main.py!",
                    ["error_start"] = "Błąd startu:",
                    ["error_port"] = "Port musi być liczbą z zakresu 1-65535",
                    ["error_ip"] = "Nieprawidłowy adres IP\nPrzykłady: 127.0.0.1, 0.0.0.0",
                    ["confirm_close"] = "ComfyUI jest uruchomione. Czy na pewno chcesz zamknąć?",
                    ["confirm"] = "Potwierdzenie",
                    ["logs_exported"] = "Logi wyeksportowane pomyślnie!",
                    ["logs_cleared"] = "Logi wyczyszczone",
                    ["window_exists"] = "Okno generatora już jest otwarte",
                    ["no_image"] = "Brak obrazu do zapisania",
                    ["image_saved"] = "Obraz zapisany pomyślnie!",
                    ["success"] = "Sukces",

                    // Maintenance Section
                    ["maint_header"] = "Konserwacja / Czyszczenie",
                    ["clean_folders"] = "Czyść Foldery (Output, Temp, WebView)",
                    ["clean_logs"] = "Czyść Logi (*.log)",
                    ["clean_python"] = "Czyść Python (Cache, PIP, .pyc)",
                    ["btn_clean"] = "WYKONAJ CZYSZCZENIE",
                    ["clean_warn_running"] = "Nie można czyścić podczas pracy ComfyUI!",
                    ["clean_start"] = "--- Rozpoczynanie Czyszczenia ---",
                    ["clean_done"] = "--- Czyszczenie Zakończone ---",
                    ["deleted"] = "Usunięto:",
                    ["pip_purge"] = "Wykonywanie PIP Cache Purge...",
                    ["clean_error"] = "Błąd czyszczenia:"
                }
            };
        }

        private string T(string key)
        {
            if (translations.ContainsKey(currentLanguage) && 
                translations[currentLanguage].ContainsKey(key))
            {
                return translations[currentLanguage][key];
            }
            return key;
        }

        private void InitializeCustomUI()
        {
            Label header = new Label { 
                Text = T("header"), 
                Font = FontHeader, 
                ForeColor = ColAccent, 
                AutoSize = true, 
                Location = new Point(20, 15) 
            };
            this.Controls.Add(header);

            tabs = new TabControl { 
                Location = new Point(20, 60), 
                Size = new Size(545, 340) 
            };
            this.Controls.Add(tabs);

            // --- Performance Tab ---
            Panel pPerf = CreatePage(T("tab_performance"));
            chkLowVram = CreateCheck(pPerf, T("lowvram"), 20, 25, config.LowVram);
            chkFast = CreateCheck(pPerf, T("fast"), 20, 65, config.FastMode);
            chkFp16 = CreateCheck(pPerf, T("fp16"), 20, 105, config.Fp16Vae);
            chkCpu = CreateCheck(pPerf, T("cpu"), 20, 145, config.CpuMode);

            // --- Network Tab ---
            Panel pNet = CreatePage(T("tab_network"));
            CreateLabel(pNet, T("port"), 20, 25); 
            txtPort = CreateInput(pNet, config.Port, 80, 22, 100);
            CreateLabel(pNet, T("ip"), 20, 75); 
            txtIp = CreateInput(pNet, config.ListenIp, 80, 72, 150);
            chkGui = CreateCheck(pNet, T("autostart"), 20, 125, config.AutoLaunchGui);

            // --- Advanced Tab (With Cleaning) ---
            Panel pAdv = CreatePage(T("tab_advanced"));
            CreateLabel(pAdv, T("extra_args"), 20, 25);
            txtExtra = CreateInput(pAdv, config.ExtraArgs, 20, 50, 480);

            // Sekcja konserwacji (Maintenance)
            GroupBox grpMaint = new GroupBox
            {
                Text = T("maint_header"),
                ForeColor = Color.LightGray,
                Font = FontBold,
                Location = new Point(15, 90),
                Size = new Size(495, 190)
            };
            
            chkCleanFolders = CreateCheck(grpMaint, T("clean_folders"), 15, 30, true);
            chkCleanLogs = CreateCheck(grpMaint, T("clean_logs"), 15, 65, true);
            chkCleanPython = CreateCheck(grpMaint, T("clean_python"), 15, 100, true);

            btnRunClean = new Button
            {
                Text = T("btn_clean"),
                Font = FontBold,
                BackColor = ColClean,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(15, 140),
                Size = new Size(465, 35),
                Cursor = Cursors.Hand
            };
            btnRunClean.FlatAppearance.BorderSize = 0;
            btnRunClean.Click += async (s, e) => await PerformCleaning();
            
            grpMaint.Controls.Add(btnRunClean);
            pAdv.Controls.Add(grpMaint);

            // --- Main Buttons ---
            btnStart = new Button { 
                Text = T("btn_start"), 
                Font = new Font("Segoe UI", 14, FontStyle.Bold), 
                BackColor = ColAccent, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Location = new Point(20, 420), 
                Size = new Size(545, 60), 
                Cursor = Cursors.Hand 
            };
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Click += async (s, e) => await BtnStart_Click();
            this.Controls.Add(btnStart);

            btnReopen = new Button { 
                Text = T("btn_reopen"), 
                Font = FontBold, 
                BackColor = ColButtonSecondary, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Location = new Point(20, 490), 
                Size = new Size(545, 40), 
                Cursor = Cursors.Hand, 
                Enabled = false 
            };
            btnReopen.FlatAppearance.BorderSize = 0;
            btnReopen.Click += async (s, e) => await OpenGeneratorWindow();
            this.Controls.Add(btnReopen);

            // --- Logs Control Panel ---
            Panel logControlPanel = new Panel
            {
                Location = new Point(20, 540),
                Size = new Size(545, 40),
                BackColor = ColPanel
            };

            Label lblLogs = new Label { 
                Text = T("logs_label"), 
                ForeColor = Color.LightGray, 
                Location = new Point(0, 10), 
                AutoSize = true, 
                Font = FontBold 
            };
            logControlPanel.Controls.Add(lblLogs);

            Label lblFilter = new Label
            {
                Text = T("log_filter"),
                ForeColor = Color.LightGray,
                Location = new Point(220, 10),
                AutoSize = true,
                Font = FontBold
            };
            logControlPanel.Controls.Add(lblFilter);

            cmbLogFilter = new ComboBox
            {
                Location = new Point(270, 7),
                Width = 140,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(60, 60, 80),
                ForeColor = Color.White,
                Font = FontBold,
                FlatStyle = FlatStyle.Flat
            };
            cmbLogFilter.Items.AddRange(new object[] { 
                T("filter_all"), 
                T("filter_errors"), 
                T("filter_warnings") 
            });
            cmbLogFilter.SelectedIndex = 0;
            logControlPanel.Controls.Add(cmbLogFilter);

            chkAutoScroll = new CheckBox
            {
                Text = T("autoscroll"),
                Location = new Point(420, 10),
                AutoSize = true,
                ForeColor = ColText,
                Checked = true,
                Font = FontBold
            };
            logControlPanel.Controls.Add(chkAutoScroll);

            this.Controls.Add(logControlPanel);

            // --- Console Box ---
            consoleBox = new RichTextBox { 
                Location = new Point(20, 590), 
                Size = new Size(545, 230), 
                BackColor = ColConsole, 
                ForeColor = Color.White, 
                Font = FontConsole, 
                ReadOnly = true, 
                BorderStyle = BorderStyle.None 
            };
            this.Controls.Add(consoleBox);

            // --- Bottom Buttons ---
            btnClearLogs = new Button
            {
                Text = T("btn_clear_logs"),
                Font = FontBold,
                BackColor = ColWarning,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 830),
                Size = new Size(265, 35),
                Cursor = Cursors.Hand
            };
            btnClearLogs.FlatAppearance.BorderSize = 0;
            btnClearLogs.Click += (s, e) => ClearLogs();
            this.Controls.Add(btnClearLogs);

            btnExportLogs = new Button
            {
                Text = T("btn_export_logs"),
                Font = FontBold,
                BackColor = ColButtonSecondary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(300, 830),
                Size = new Size(265, 35),
                Cursor = Cursors.Hand
            };
            btnExportLogs.FlatAppearance.BorderSize = 0;
            btnExportLogs.Click += (s, e) => ExportLogs();
            this.Controls.Add(btnExportLogs);
        }

        // --- LOGIKA CZYSZCZENIA (NOWE) ---

        private async Task PerformCleaning()
        {
            if (isGenerating)
            {
                MessageBox.Show(T("clean_warn_running"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Przełącz na zakładkę logów, żeby użytkownik widział co się dzieje
            consoleBox.Clear();
            Log(T("clean_start"), Color.Cyan);
            btnRunClean.Enabled = false;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string comfyPath = Path.Combine(basePath, "ComfyUI");
            string webViewPath = Path.Combine(basePath, "webview_data");

            await Task.Run(() =>
            {
                // 1. Clean Folders (Output, Temp, Input, WebView)
                if (chkCleanFolders.Checked)
                {
                    CleanFolderContent(Path.Combine(comfyPath, "output"));
                    CleanFolderContent(Path.Combine(comfyPath, "temp"));
                    // CleanFolderContent(Path.Combine(comfyPath, "input")); // Input zostawmy, często ludzie tam trzymają ważne pliki
                    CleanFolderContent(webViewPath);
                }

                // 2. Clean Logs
                if (chkCleanLogs.Checked)
                {
                    DeleteFilePattern(basePath, "*.log");
                    DeleteFilePattern(comfyPath, "*.log");
                    DeleteFilePattern(Path.Combine(comfyPath, "user"), "comfyui.log");
                }

                // 3. Clean Python (Cache & PIP)
                if (chkCleanPython.Checked)
                {
                    string pythonPath = FindPythonExecutable(basePath);
                    string pythonFolder = string.IsNullOrEmpty(pythonPath) ? "" : Path.GetDirectoryName(pythonPath);
                    string libPath = string.IsNullOrEmpty(pythonFolder) ? "" : Path.GetDirectoryName(pythonFolder); // Wyjście z python_embedded
                    
                    // Usuwanie __pycache__ i .pyc
                    if (!string.IsNullOrEmpty(libPath))
                    {
                        CleanPythonCacheRecursive(libPath);
                    }

                    // PIP Purge
                    if (!string.IsNullOrEmpty(pythonPath))
                    {
                        RunPipPurge(pythonPath);
                    }
                }
            });

            Log(T("clean_done"), Color.Lime);
            btnRunClean.Enabled = true;
            MessageBox.Show(T("clean_done"), T("success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CleanFolderContent(string path)
        {
            if (!Directory.Exists(path)) return;

            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                
                foreach (FileInfo file in di.GetFiles())
                {
                    // Pomijanie plików .json w output (workflowy)
                    if (path.EndsWith("output") && file.Extension.ToLower() == ".json") continue;
                    
                    try { 
                        file.Delete();
                        LogFiltered($"{T("deleted")} {file.Name}", Color.Gray, LogLevel.Info);
                    } catch { }
                }

                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    try { 
                        dir.Delete(true); 
                        LogFiltered($"{T("deleted")} [DIR] {dir.Name}", Color.Gray, LogLevel.Info);
                    } catch { }
                }
            }
            catch (Exception ex)
            {
                LogFiltered($"{T("clean_error")} {path} - {ex.Message}", Color.Red, LogLevel.Error);
            }
        }

        private void DeleteFilePattern(string path, string pattern)
        {
            if (!Directory.Exists(path)) return;
            try
            {
                string[] files = Directory.GetFiles(path, pattern);
                foreach (string f in files)
                {
                    try { 
                        File.Delete(f); 
                        LogFiltered($"{T("deleted")} {Path.GetFileName(f)}", Color.Gray, LogLevel.Info);
                    } catch { }
                }
            }
            catch { }
        }

        private void CleanPythonCacheRecursive(string path)
        {
            if (!Directory.Exists(path)) return;

            try
            {
                // Usuń pliki .pyc .pyo
                foreach (string file in Directory.GetFiles(path, "*.py?", SearchOption.AllDirectories))
                {
                    string ext = Path.GetExtension(file).ToLower();
                    if (ext == ".pyc" || ext == ".pyo")
                    {
                        try { File.Delete(file); } catch { }
                    }
                }

                // Usuń foldery __pycache__
                foreach (string dir in Directory.GetDirectories(path, "__pycache__", SearchOption.AllDirectories))
                {
                    try { Directory.Delete(dir, true); } catch { }
                }
                LogFiltered("Python compiled files (.pyc) cleaned.", Color.Gray, LogLevel.Success);
            }
            catch (Exception ex)
            {
                LogFiltered($"Python clean error: {ex.Message}", Color.Orange, LogLevel.Warning);
            }
        }

        private void RunPipPurge(string pythonExe)
        {
            try
            {
                Log(T("pip_purge"), Color.Cyan);
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pythonExe,
                    Arguments = "-m pip cache purge",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process p = Process.Start(psi))
                {
                    string output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        LogFiltered(output.Trim(), Color.Gray, LogLevel.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                LogFiltered($"PIP Error: {ex.Message}", Color.Red, LogLevel.Error);
            }
        }

        // --- RESZTA FUNKCJI (Start, Stop, Logs...) ---

        private void ClearLogs()
        {
            consoleBox.Clear();
            Log(T("logs_cleared"), Color.Cyan);
        }

        private void ExportLogs()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Text File|*.txt|Log File|*.log|All Files|*.*",
                    FileName = $"comfyui_logs_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
                    DefaultExt = "txt",
                    Title = T("btn_export_logs")
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, consoleBox.Text, Encoding.UTF8);
                    Log($"✅ {T("logs_exported")}", Color.Green);
                    MessageBox.Show($"{T("logs_exported")}\n\n{sfd.FileName}", T("success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Log($"{T("error_start")} {ex.Message}", Color.Red);
            }
        }

        private async Task BtnStart_Click()
        {
            if (isGenerating)
            {
                await StopComfyProcess();
                return;
            }

            if (!ValidateInputs()) return;

            config.LowVram = chkLowVram.Checked;
            config.FastMode = chkFast.Checked;
            config.Fp16Vae = chkFp16.Checked;
            config.CpuMode = chkCpu.Checked;
            config.Port = txtPort.Text.Trim();
            config.ListenIp = txtIp.Text.Trim();
            config.ExtraArgs = txtExtra.Text.Trim();
            config.AutoLaunchGui = chkGui.Checked;
            
            try { ComfyConfig.Save(config); } catch { }

            consoleBox.Clear();
            Log(T("searching_python"), Color.Yellow);

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string pythonPath = FindPythonExecutable(basePath);

            if (string.IsNullOrEmpty(pythonPath))
            {
                Log(T("error_python"), Color.Red);
                MessageBox.Show(T("error_python"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            Log($"{T("found_python")} {pythonPath}", Color.Green);

            string mainScript = Path.Combine(basePath, "ComfyUI", "main.py");
            if (!File.Exists(mainScript))
            {
                Log(T("error_main"), Color.Red);
                return;
            }

            string args = BuildComfyArguments(mainScript, config);
            Log($"{T("arguments")} {args}", Color.Cyan);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = basePath
            };

            comfyProcess = new Process { StartInfo = psi };
            
            comfyProcess.OutputDataReceived += (s, d) => 
            {
                if (!string.IsNullOrWhiteSpace(d.Data) && !d.Data.Contains("Bounds.Empty"))
                    LogFiltered(d.Data, Color.White, LogLevel.Info);
            };
            
            comfyProcess.ErrorDataReceived += (s, d) => 
            {
                if (!string.IsNullOrWhiteSpace(d.Data) && !d.Data.Contains("Bounds.Empty"))
                {
                    if (d.Data.ToLower().Contains("error")) LogFiltered(d.Data, Color.Red, LogLevel.Error);
                    else LogFiltered(d.Data, Color.Yellow, LogLevel.Warning);
                }
            };

            try
            {
                comfyProcess.Start();
                comfyProcess.BeginOutputReadLine();
                comfyProcess.BeginErrorReadLine();

                isGenerating = true;
                btnStart.Text = T("btn_stop");
                btnStart.BackColor = ColError;
                btnReopen.Enabled = true;
                btnReopen.BackColor = Color.FromArgb(33, 150, 243);

                Log($"✅ {T("started")}", Color.Green);

                if (chkGui.Checked)
                {
                    Log(T("waiting"), Color.Cyan);
                    await Task.Delay(3000);
                    await OpenGeneratorWindow();
                }
            }
            catch (Exception ex)
            {
                Log($"{T("error_start")} {ex.Message}", Color.Red);
                isGenerating = false;
            }
        }

        private bool ValidateInputs()
        {
            if (!int.TryParse(txtPort.Text.Trim(), out int port) || port < 1 || port > 65535)
            {
                MessageBox.Show(T("error_port"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            string ip = txtIp.Text.Trim();
            if (!System.Net.IPAddress.TryParse(ip, out _) && ip != "0.0.0.0")
            {
                MessageBox.Show(T("error_ip"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private string FindPythonExecutable(string basePath)
        {
            string[] checkPaths = { "python_standalone", "python_embedded", "python_embeded" };
            foreach (var p in checkPaths)
            {
                string full = Path.Combine(basePath, p, "python.exe");
                if (File.Exists(full))
                {
                    Log($"  ↳ {T("found_in")} {p}/", Color.Cyan);
                    return full;
                }
            }
            return null;
        }

        private string BuildComfyArguments(string mainScript, ComfyConfig cfg)
        {
            string args = $"-s \"{mainScript}\" --windows-standalone-build --enable-cors-header --disable-auto-launch";
            if (cfg.LowVram) args += " --lowvram";
            if (cfg.FastMode) args += " --fast";
            if (cfg.CpuMode) args += " --cpu";
            if (cfg.Fp16Vae) args += " --fp16-vae";
            args += $" --port {cfg.Port} --listen {cfg.ListenIp}";
            if (!string.IsNullOrWhiteSpace(cfg.ExtraArgs)) args += $" {cfg.ExtraArgs}";
            return args;
        }

        private async Task OpenGeneratorWindow()
        {
            if (generatorWindow != null && !generatorWindow.IsDisposed)
            {
                generatorWindow.BringToFront();
                return;
            }

            try
            {
                Form webForm = new Form
                {
                    Text = "ComfyUI Generator",
                    Size = new Size(1600, 900),
                    BackColor = ColBack,
                    Icon = this.Icon,
                    StartPosition = FormStartPosition.CenterScreen
                };

                WebView2 webView = new WebView2 { Dock = DockStyle.Fill, DefaultBackgroundColor = ColBack };
                webForm.Controls.Add(webView);

                webForm.FormClosing += (s, e) => { webView?.Dispose(); generatorWindow = null; };

                string userDataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "webview_data");
                var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
                await webView.EnsureCoreWebView2Async(env);

                string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gui", "index.html");
                if (File.Exists(htmlPath)) webView.Source = new Uri(htmlPath);

                webView.CoreWebView2.WebMessageReceived += async (s, args) =>
                {
                    try
                    {
                        string base64 = args.TryGetWebMessageAsString();
                        if (!string.IsNullOrWhiteSpace(base64)) await SaveImageFromWebView(base64);
                    }
                    catch { }
                };

                generatorWindow = webForm;
                webForm.Show();
                Log($"✅ {T("window_opened")}", Color.Green);
            }
            catch (Exception ex)
            {
                Log($"WebView2 error: {ex.Message}", Color.Red);
            }
        }

        private async Task SaveImageFromWebView(string base64Data)
        {
            await Task.Run(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        SaveFileDialog sfd = new SaveFileDialog
                        {
                            Filter = "PNG Image|*.png|All Files|*.*",
                            FileName = $"comfy_{DateTimeOffset.Now.ToUnixTimeSeconds()}.png",
                            DefaultExt = "png",
                            AddExtension = true
                        };

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            if (base64Data.Contains(",")) base64Data = base64Data.Split(',')[1];
                            byte[] imageBytes = Convert.FromBase64String(base64Data);
                            File.WriteAllBytes(sfd.FileName, imageBytes);
                            Log($"✅ {sfd.FileName}", Color.Green);
                            MessageBox.Show($"{T("image_saved")}\n\n{sfd.FileName}", T("success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch { }
                });
            });
        }

        private async Task StopComfyProcess()
        {
            btnStart.Enabled = false;
            Log(T("stopping"), Color.Orange);

            try
            {
                if (comfyProcess != null && !comfyProcess.HasExited)
                {
                    comfyProcess.Kill(entireProcessTree: true);
                    await Task.Delay(1000);
                    comfyProcess.Dispose();
                    comfyProcess = null;
                }
                Log($"✅ {T("stopped")}", Color.Green);
            }
            catch (Exception ex) { Log($"{T("error_start")} {ex.Message}", Color.Red); }
            finally
            {
                isGenerating = false;
                btnStart.Text = T("btn_start");
                btnStart.BackColor = ColAccent;
                btnStart.Enabled = true;
                btnReopen.Enabled = false;
                btnReopen.BackColor = ColButtonSecondary;
            }
        }

        private Panel CreatePage(string title)
        {
            var p = new TabPage(title);
            tabs.TabPages.Add(p);
            Panel panel = new Panel { Dock = DockStyle.Fill, BackColor = ColPanel };
            p.Controls.Add(panel);
            return panel;
        }

        private CheckBox CreateCheck(Control p, string t, int x, int y, bool c)
        {
            var k = new CheckBox { Text = t, Location = new Point(x, y), AutoSize = true, ForeColor = ColText, Checked = c, Font = FontBold };
            p.Controls.Add(k);
            return k;
        }

        private void CreateLabel(Control p, string t, int x, int y)
        {
            p.Controls.Add(new Label { Text = t, Location = new Point(x, y), AutoSize = true, ForeColor = Color.LightGray, Font = FontBold });
        }

        private TextBox CreateInput(Control p, string v, int x, int y, int w)
        {
            var t = new TextBox { Text = v, Location = new Point(x, y), Width = w, BackColor = Color.FromArgb(60, 60, 80), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Font = FontBold };
            p.Controls.Add(t);
            return t;
        }

        private void LogFiltered(string m, Color c, LogLevel level)
        {
            int filterIndex = 0;
            if (cmbLogFilter != null) 
            {
                if (cmbLogFilter.InvokeRequired) cmbLogFilter.Invoke((MethodInvoker)delegate { filterIndex = cmbLogFilter.SelectedIndex; });
                else filterIndex = cmbLogFilter.SelectedIndex;
            }

            bool shouldLog = filterIndex switch
            {
                0 => true,
                1 => level == LogLevel.Error,
                2 => level == LogLevel.Error || level == LogLevel.Warning,
                _ => true
            };

            if (shouldLog) Log(m, c);
        }

        private void Log(string m, Color c)
        {
            if (string.IsNullOrWhiteSpace(m)) return;
            if (consoleBox.InvokeRequired) { consoleBox.Invoke(new Action(() => Log(m, c))); return; }

            consoleBox.SelectionStart = consoleBox.TextLength;
            consoleBox.SelectionLength = 0;
            consoleBox.SelectionColor = c;
            consoleBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {m}\n");
            
            if (chkAutoScroll != null && chkAutoScroll.Checked) consoleBox.ScrollToCaret();
            if (consoleBox.Lines.Length > 1000) consoleBox.Lines = consoleBox.Lines.Skip(500).ToArray();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isGenerating)
            {
                var result = MessageBox.Show(T("confirm_close"), T("confirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) { e.Cancel = true; return; }
            }
            try
            {
                if (comfyProcess != null && !comfyProcess.HasExited) comfyProcess.Kill(entireProcessTree: true);
                generatorWindow?.Dispose();
            }
            catch { }
            base.OnFormClosing(e);
        }
    }
}