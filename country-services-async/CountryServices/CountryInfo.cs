using System.Text.Json.Serialization;

namespace CountryServices;

/// <summary>
/// Contains information about capital of the country.
/// </summary>
internal sealed class CountryInfo
{
    /// <summary>
    /// Gets or sets the country name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the capital name of the country.
    /// </summary>
    [JsonPropertyName("capital")]
    public string? CapitalName { get; set; }

    /// <summary>
    /// Gets or sets the area of the country.
    /// </summary>
    [JsonPropertyName("area")]
    public double Area { get; set; }

    /// <summary>
    /// Gets or sets the population of the country.
    /// </summary>
    [JsonPropertyName("population")]
    public int Population { get; set; }

    /// <summary>
    /// Gets or sets the flag url (png or svg if png does not exist) of the country.
    /// </summary>
    [JsonPropertyName("flag")]
    public string? Flag { get; set; }
}
