using TheKartersModdingAssistant;

namespace AtLifePace;

public class ConfigData {
    // General
    public bool isModEnabled;

    // Customization
    public int reservePercentageAtMinimumHealth;
    public int reservePercentageAtMaximumHealth;
    public bool isAlternativeVersionEnabled;

    public void Log(Logger logger) {
        logger.Log($"reservePercentageAtMinimumHealth: {this.reservePercentageAtMinimumHealth}");
        logger.Log($"reservePercentageAtMinimumHealth: {this.reservePercentageAtMaximumHealth}");
        logger.Log($"isAlternateVersionEnabled: {this.isAlternativeVersionEnabled}");
    }
}