using BowlingCoacher.Shared.DTO;

namespace BowlingCoacher.API.Interfaces;

public interface IGameStatisticsRepository {
    IEnumerable<GameStatistics> GetAll();
    GameStatistics? GetById(int id);
    GameStatistics Add(GameStatistics stats);
    bool Update(int id, GameStatistics stats);
    bool Delete(int id);
}