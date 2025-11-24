using System.IO;
using Newtonsoft.Json;

namespace ComfyLauncher
{
    public class ComfyConfig
    {
        public string Port { get; set; } = "8188";
        public string ListenIp { get; set; } = "127.0.0.1";
        public bool LowVram { get; set; } = false;
        public bool FastMode { get; set; } = false;
        public bool CpuMode { get; set; } = false;
        public bool Fp16Vae { get; set; } = false;
        public string ExtraArgs { get; set; } = "";
        public bool AutoLaunchGui { get; set; } = true;

        private const string ConfigFile = "launcher_config_csharp.json";

        public static void Save(ComfyConfig cfg)
        {
            File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(cfg, Formatting.Indented));
        }

        public static ComfyConfig Load()
        {
            if (File.Exists(ConfigFile))
                return JsonConvert.DeserializeObject<ComfyConfig>(File.ReadAllText(ConfigFile));
            return new ComfyConfig();
        }
    }
}