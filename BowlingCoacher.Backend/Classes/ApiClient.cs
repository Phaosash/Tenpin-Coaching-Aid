using BowlingCoacher.Shared.DTO;
using ErrorLogging;
using System.Net.Http.Json;
using System.Text.Json;

namespace BowlingCoacher.Backend.Classes;

public class ApiClient (HttpClient http){
    private readonly HttpClient _http = http;
    private const string BaseRoute = "api/scores";

    //  This method is used to get get a json response via the API.
    public async Task<IEnumerable<GameStatistics>?> GetAllAsync (){
        try {
            return await _http.GetFromJsonAsync<IEnumerable<GameStatistics>>(BaseRoute);
        } catch (HttpRequestException ex){
            LoggingManager.Instance.LogError(ex, "Failed to get all game statistics.");
            return null;
        } catch (NotSupportedException ex){
            LoggingManager.Instance.LogError(ex, "The content type is not supported.");
            return null;
        } catch (JsonException ex){
            LoggingManager.Instance.LogError(ex, "Invalid JSON received.");
            return null;
        }
    }

    //  This method is used to get a json response from an API based off the supplied ID.
    public async Task<GameStatistics?> GetByIdAsync (int id){
        try {
            return await _http.GetFromJsonAsync<GameStatistics>($"{BaseRoute}/{id}");
        } catch (HttpRequestException ex){
            LoggingManager.Instance.LogError(ex, $"Failed to get game statistics with ID={id}.");
        } catch (NotSupportedException ex){
            LoggingManager.Instance.LogError(ex, "The content type is not supported.");
        } catch (JsonException ex){
            LoggingManager.Instance.LogError(ex, "Invalid JSON received.");
        }
        return null;
    }

    //  This method is used to create a new record via the API.
    public async Task<GameStatistics?> CreateAsync (GameStatistics stats){
        try {
            var response = await _http.PostAsJsonAsync(BaseRoute, stats);
            
            if (!response.IsSuccessStatusCode){
                LoggingManager.Instance.LogWarning($"Failed to create game stats. Status code: {response.StatusCode}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<GameStatistics>();
        } catch (HttpRequestException ex){
            LoggingManager.Instance.LogError(ex, "Failed to create game statistics.");
        }
        return null;
    }

    //  This method is used to update an existing data record based on the supplied ID and the modified data.
    public async Task<bool> UpdateAsync (int id, GameStatistics stats){
        try {
            var response = await _http.PutAsJsonAsync($"{BaseRoute}/{id}", stats);
            
            if (!response.IsSuccessStatusCode){
                LoggingManager.Instance.LogWarning($"Failed to update game stats with ID={id}. Status code: {response.StatusCode}");
            }

            return response.IsSuccessStatusCode;
        } catch (HttpRequestException ex){
            LoggingManager.Instance.LogError(ex, $"Failed to update game statistics with ID={id}.");
            return false;
        }
    }

    //  This method is used to tell the API to delete a record based off the supplied ID.
    public async Task<bool> DeleteAsync (int id){
        try {
            var response = await _http.DeleteAsync($"{BaseRoute}/{id}");
            
            if (!response.IsSuccessStatusCode){
                LoggingManager.Instance.LogWarning($"Failed to delete game stats with ID={id}. Status code: {response.StatusCode}");
            }

            return response.IsSuccessStatusCode;
        } catch (HttpRequestException ex){
            LoggingManager.Instance.LogError(ex, $"Failed to delete game statistics with ID={id}.");
            return false;
        }
    }
}