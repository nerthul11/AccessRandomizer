using System;

namespace AccessRandomizer.Settings
{
    public class GlobalSettings
    {
        public AccessSettings Settings { get; set; } = new();
    }

    public class LocalSettings
    {
        public int ChainsBroken { get; set; } = 0;
        public bool GraveyardKey { get; set;} = false;
        public bool WaterwaysKey { get; set;} = false;
        public bool PleasureKey { get; set;} = false;
        public bool CoffinKey { get; set;} = false;

        public T GetVariable<T>(string propertyName) {
            var property = typeof(LocalSettings).GetProperty(propertyName);
            if (property == null) {
                throw new ArgumentException($"Property '{propertyName}' not found in LocalSettings class.");
            }
            return (T)property.GetValue(this);
        }

        public void SetVariable<T>(string propertyName, T value) {
            var property = typeof(LocalSettings).GetProperty(propertyName);
            if (property == null) {
                throw new ArgumentException($"Property '{propertyName}' not found in LocalSettings class.");
            }
            property.SetValue(this, value);
        }
    }
}