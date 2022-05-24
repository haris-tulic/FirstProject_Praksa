using System.Text.Json.Serialization;

namespace FirstProject_Praksa.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RPGClass
    {
        Knight,
        Mage,
        Cleric
    }
}
