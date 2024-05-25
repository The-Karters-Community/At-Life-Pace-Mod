using BepInEx;
using BepInEx.Configuration;
using TheKartersModdingAssistant;
using AtLifePace.EventHandler;
using UnityEngine;

namespace AtLifePace;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class AtLifePace: AbstractPlugin {
    public static AtLifePace instance;

    /// <summary>
    /// Get the plugin instance.
    /// </summary>
    /// 
    /// <returns>AtLifePace</returns>
    public static AtLifePace Get() {
        return AtLifePace.instance;
    }

    public ConfigData data = new();

    /// <summary>
    /// AtLifePace constructor.
    /// </summary>
    public AtLifePace(): base() {
        this.pluginGuid = MyPluginInfo.PLUGIN_GUID;
        this.pluginName = MyPluginInfo.PLUGIN_NAME;
        this.pluginVersion = MyPluginInfo.PLUGIN_VERSION;

        this.harmony = new(this.pluginGuid);
        this.logger = new(this.Log);

        AtLifePace.instance = this;
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
            //this.harmony.PatchAll(typeof(Ant_CurrentGameConfiguration__Start));

            // Then, add methods to the SDK actions.
            PlayerEventHandler.Initialize();
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
        ConfigEntry<int> reservePercentageAtMinimumHealth = Config.Bind(
            ConfigCategory.Customization,
            nameof(reservePercentageAtMinimumHealth),
            100,
            "The reserve percentage value when a player is at his minimum health. A value below 100 will be clamped."
        );

        ConfigEntry<int> reservePercentageAtMaximumHealth = Config.Bind(
            ConfigCategory.Customization,
            nameof(reservePercentageAtMaximumHealth),
            150,
            "The reserve percentage value when a player is at his maximum health."
        );

        this.data.reservePercentageAtMinimumHealth = Mathf.Max(100, reservePercentageAtMinimumHealth.Value);
        this.data.reservePercentageAtMaximumHealth = reservePercentageAtMaximumHealth.Value;
    }
}
