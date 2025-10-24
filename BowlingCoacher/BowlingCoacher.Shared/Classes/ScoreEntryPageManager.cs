using BowlingCoacher.Shared.DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingCoacher.Shared.Classes;

internal class ScoreEntryPageManager {
    private readonly List<StatisticsObject> _statsObject = [];

    public void AddObjectToList (StatisticsObject statsObject){
        _statsObject.Add(statsObject);
    }
}