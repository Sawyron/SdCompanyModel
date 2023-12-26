namespace Solution.Conditions;

/// <summary>
/// 
/// </summary>
/// <param name="Demands">uk</param>
/// <param name="OrderFulfillmentDelay">T2</param>
/// <param name="AbsenceDelay">T3</param>
/// <param name="K">k</param>
/// <param name="AveragingDelay">T5</param>
/// <param name="StockControlDelay">T5</param>
/// <param name="OrderProcessDelay">T6</param>
/// <param name="LinkDelay">T7</param>
/// <param name="ShipmentDelay">T8</param>
public record SalesParameters(
    double Demands,
    double OrderFulfillmentDelay,
    double AbsenceDelay,
    double K,
    double AveragingDelay,
    double StockControlDelay,
    double OrderProcessDelay,
    double LinkDelay,
    double ShipmentDelay);