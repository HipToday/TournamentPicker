using System.Text.Json;

namespace GoogleSheets
{
    /// <summary>
    /// Service for interacting with Google Sheets API.
    /// <see href="https://developers.google.com/sheets/api/reference/rest"/>
    /// </summary>
    public class GoogleSheetsService
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleSheetsService"/> class.
        /// </summary>
        /// <param name="apiKey"></param>
        public GoogleSheetsService(string apiKey)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-Goog-API-Key", apiKey);
        }

        /// <summary>
        /// Get the spreadsheet data from the given spreadsheet ID.
        /// </summary>
        /// <param name="spreadsheetId">The ID of the spreadsheet.</param>
        /// <returns>The spreadsheet data.</returns>
        public async Task<Spreadsheet> GetSpreadsheetAsync(string spreadsheetId)
        {
            var url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch spreadsheet: {response.ReasonPhrase}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var spreadsheet = JsonSerializer.Deserialize<Spreadsheet>(jsonResponse, _jsonSerializerOptions);

            if (spreadsheet == null)
            {
                throw new InvalidOperationException("Failed to deserialize the spreadsheet.");
            }

            return spreadsheet;
        }

        /// <summary>
        /// Get the list of sheet titles from the given spreadsheet.
        /// </summary>
        /// <param name="spreadsheetId">The ID of the spreadsheet.</param>
        /// <returns>The list of sheet titles.</returns>
        public async Task<IList<string>> GetSheetTitlesAsync(string spreadsheetId)
        {
            var spreadsheet = await GetSpreadsheetAsync(spreadsheetId);

            return spreadsheet.Sheets?.Select(sheet => sheet.Properties?.Title ?? string.Empty).ToList() ?? [];
        }

        /// <summary>
        /// Get the values from the given sheet and range in the spreadsheet.
        /// </summary>
        /// <param name="spreadsheetId">The ID of the spreadsheet.</param>
        /// <param name="sheetTitle">The title of the sheet.</param>
        /// <param name="range">The range of the cells to get values for. E.g. "A2:B17".</param>
        /// <returns>The values in the range.</returns>
        public async Task<IList<IList<object>>> GetValuesAsync(string spreadsheetId, string sheetTitle, string range)
        {
            var a1Notation = $"'{sheetTitle}'!{range}";
            var url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{a1Notation}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch values: {response.ReasonPhrase}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var valueRange = JsonSerializer.Deserialize<ValueRange>(jsonResponse, _jsonSerializerOptions);

            if (valueRange == null)
            {
                throw new InvalidOperationException("Failed to deserialize the value range.");
            }

            return valueRange.Values ?? [];
        }
    }

    /// <summary>
    /// Represents the spreadsheet data.
    /// <see href="https://developers.google.com/sheets/api/reference/rest/v4/spreadsheets#Spreadsheet"/>
    /// </summary>
    public class Spreadsheet
    {
        /// <summary>
        /// The ID of the spreadsheet.
        /// </summary>
        public string? SpreadsheetId { get; set; }
        /// <summary>
        /// The properties of the spreadsheet.
        /// </summary>
        public SpreadsheetProperties? Properties { get; set; }
        /// <summary>
        /// The sheets in the spreadsheet.
        /// </summary>
        public IList<Sheet>? Sheets { get; set; }
        /// <summary>
        /// The URL of the spreadsheet.
        /// </summary>
        public string? SpreadSheetUrl { get; set; }
    }

    /// <summary>
    /// Represents the properties of the spreadsheet.
    /// <see href="https://developers.google.com/sheets/api/reference/rest/v4/spreadsheets#SpreadsheetProperties"/>
    /// </summary>
    public class SpreadsheetProperties
    {
        /// <summary>
        /// The title of the spreadsheet.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// The locale of the spreadsheet.
        /// </summary>
        public string? Locale { get; set; }
        /// <summary>
        /// The time zone of the spreadsheet.
        /// </summary>
        public string? TimeZone { get; set; }
    }

    /// <summary>
    /// Represents a sheet in the spreadsheet.
    /// <see href="https://developers.google.com/sheets/api/reference/rest/v4/spreadsheets/sheets#Sheet"/>
    /// </summary>
    public class Sheet
    {
        /// <summary>
        /// The properties of the sheet.
        /// </summary>
        public SheetProperties? Properties { get; set; }
    }

    /// <summary>
    /// Represents the properties of a sheet in the spreadsheet.
    /// <see href="https://developers.google.com/sheets/api/reference/rest/v4/spreadsheets/sheets#SheetProperties"/>
    /// </summary>
    public class SheetProperties
    {
        /// <summary>
        /// The ID of the sheet.
        /// </summary>
        public int SheetId { get; set; }
        /// <summary>
        /// The title of the sheet.
        /// </summary>
        public required string Title { get; set; }
        /// <summary>
        /// The index of the sheet.
        /// </summary>
        public int Index { get; set; }
    }

    /// <summary>
    /// Represents the value range in the spreadsheet.
    /// <see href="https://developers.google.com/sheets/api/reference/rest/v4/spreadsheets.values#ValueRange"/>
    /// </summary>
    public class ValueRange
    {
        /// <summary>
        /// The range of the values in A1 notation.
        /// </summary>
        public string? Range { get; set; }
        /// <summary>
        /// The major dimension of the values.
        /// </summary>
        public string? MajorDimension { get; set; }
        /// <summary>
        /// The values in the range.
        /// </summary>
        public IList<IList<object>>? Values { get; set; }
    }
}