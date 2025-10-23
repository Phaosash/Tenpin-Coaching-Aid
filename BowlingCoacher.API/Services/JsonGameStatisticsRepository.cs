using BowlingCoacher.API.Interfaces;
using BowlingCoacher.Shared.DTO;
using System.Text.Json;

namespace BowlingCoacher.API.Services;

public class JsonGameStatisticsRepository: IGameStatisticsRepository {
    private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "gamestats.json");
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    private List<GameStatistics> LoadData (){
        if (!File.Exists(_filePath)){
            return [];
        }

        var json = File.ReadAllText(_filePath);

        return JsonSerializer.Deserialize<List<GameStatistics>>(json) ?? [];
    }

    private void SaveData(List<GameStatistics> data){
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }

    public IEnumerable<GameStatistics> GetAll() => LoadData();

    public GameStatistics? GetById(int id) => LoadData().FirstOrDefault(x => x.Id == id);

    public GameStatistics Add (GameStatistics stats){
        var data = LoadData();
        stats.Id = data.Any() ? data.Max(x => x.Id) + 1 : 1;
        data.Add(stats);
        SaveData(data);
        return stats;
    }

    public bool Update (int id, GameStatistics stats){
        var data = LoadData();
        var existing = data.FirstOrDefault(x => x.Id == id);
        if (existing == null) return false;

        existing.Score = stats.Score;
        existing.Games = stats.Games;
        existing.Strikes = stats.Strikes;
        existing.Spares = stats.Spares;
        existing.Opens = stats.Opens;

        SaveData(data);
        return true;
    }

    public bool Delete (int id){
        var data = LoadData();
        var toRemove = data.FirstOrDefault(x => x.Id == id);
        if (toRemove == null) return false;

        data.Remove(toRemove);
        SaveData(data);
        return true;
    }
}