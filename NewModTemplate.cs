using BepInEx;
using BepInEx.Configuration;
using NewModTemplate.Core;
using TheKartersModdingAssistant;
using TheKartersModdingAssistant.Event;

namespace NewModTemplate;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class NewModTemplate: AbstractPlugin {
    public static NewModTemplate instance;

    /// <summary>
    /// Get the plugin instance.
    /// </summary>
    /// 
    /// <returns>NewModTemplate</returns>
    public static NewModTemplate Get() {
        return NewModTemplate.instance;
    }

    public ConfigData data = new();

    /// <summary>
    /// NewModTemplate constructor.
    /// </summary>
    public NewModTemplate(): base() {
        this.pluginGuid = MyPluginInfo.PLUGIN_GUID;
        this.pluginName = MyPluginInfo.PLUGIN_NAME;
        this.pluginVersion = MyPluginInfo.PLUGIN_VERSION;

        this.harmony = new(this.pluginGuid);
        this.logger = new(this.Log);

        NewModTemplate.instance = this;
    }

    /// <summary>
    /// Patch all the methods with Harmony.
    /// </summary>
    public override void ProcessPatching() {
        this.BindFromConfig();

        if (this.data.isModEnabled) {
            this.logger.Info($"{this.pluginName} has been enabled.", true);

            // Put all methods that should patched by Harmony here.
            // Eg:
            this.harmony.PatchAll(typeof(Ant_CurrentGameConfiguration__Start));

            // Then, add methods to the SDK actions.
            // Eg:
            GameEvent.onGameStart += () => this.logger.Log("(From action) The game has been started.");
        }
    }

    /// <summary>
    /// Bind configurations from the config file.
    /// </summary>
    public void BindFromConfig() {
        this.BindGeneralConfig();
        this.BindCustomizationConfig();
    }

    /// <summary>
    /// Bind general configurations from the config file.
    /// </summary>
    protected void BindGeneralConfig() {
        ConfigEntry<bool> isModEnabled = Config.Bind(
            ConfigCategory.General,
            nameof(isModEnabled),
            true,
            "Whether the mod is enabled."
        );

        this.data.isModEnabled = isModEnabled.Value;
    }

    /// <summary>
    /// Bind customization configurations from the config file.
    /// </summary>
    protected void BindCustomizationConfig() {
        /*ConfigEntry<int> myIntegerCustomizationConfig = Config.Bind(
            ConfigCategory.Customization,
            nameof(myIntegerCustomizationConfig),
            10,
            "Whether the mod is enabled."
        );

        this.data.myIntegerCustomizationConfig = myIntegerCustomizationConfig.Value;*/
    }
}
