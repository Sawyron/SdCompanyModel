using Domain;
using Solution.Common;
using Solution.Production;
using Solution.Sales;

namespace Solution.Company;
public class CompanyInitialConditionsProvider
{
    private readonly ParameterInputService _inputService;
    private readonly SalesParametersProvider _salesParametersProvider;
    private readonly ProductionParametersProvider _productionParametersProvider;

    public CompanyInitialConditionsProvider(
        ParameterInputService inputService,
        SalesParametersProvider salesParametersProvider,
        ProductionParametersProvider productionParametersProvider)
    {
        _inputService = inputService;
        _salesParametersProvider = salesParametersProvider;
        _productionParametersProvider = productionParametersProvider;
    }

    public IDictionary<string, double> GetInitialConditions()
    {
        var input = _inputService.GetParameters();
        var salesParameters = _salesParametersProvider.GetSalesParameters();
        var productionParameters = _productionParametersProvider.GetProductionParameters();
        double ui = input["ui"];
        double x3 = ui;
        double x1 = (salesParameters.T2 * salesParameters.T3) * x3;
        double x2 = salesParameters.K * x3;
        double x4 = salesParameters.T6 * ui;
        double x5 = salesParameters.T7 * ui;
        double x6 = salesParameters.T8 * ui;
        double w5 = SalesSystem.W5(salesParameters.K, x3);
        double w11 = ui;
        double y3 = ui;
        double y1 = (productionParameters.T2 + productionParameters.T3) * y3;
        double y2 = productionParameters.K * y3;
        double y4 = productionParameters.T6 * w11;
        double y5 = productionParameters.T7 * w11;
        double v5 = ProductionSystem.V5(productionParameters.K, y3);
        double v4 = ProductionSystem.V4(0.05, y2);
        double v3 = ProductionSystem.V3(productionParameters.T2, productionParameters.T3, v5, y2);
        double v2 = ProductionSystem.V2(y1, v3);
        return new Dictionary<string, double>
        {
            {"x1", x1 },
            {"x2", x2 },
            {"x3", x3 },
            {"x4", x4 },
            {"x5", x5 },
            {"x6", x6 },
            {"f1", x3 },
            {"w1", input["w1"] },
            {"w3", SalesSystem.W3(
                salesParameters.T2,
                salesParameters.T3,
                x2,
                w5) },
            {"w5", SalesSystem.W5(salesParameters.K, x3) },
            {"w6", x3 },
            {"w7", SalesSystem.W7(
                x3,
                v3,
                salesParameters.T6,
                salesParameters.T7,
                salesParameters.T8) },
            {"w8", SalesSystem.W8(x4, x5, x6, y1) },
            {"w9", SalesSystem.W9(x3, salesParameters.T2, salesParameters.T3) },
            {"w10", x1 },
            {"w11", w11 },
            {"y1", y1 },
            {"y2", y2 },
            {"y3", y3 },
            {"y4", y4 },
            {"y5", y5 },
            {"v1", input["v1"] },
            {"v2", v2 },
            {"v3", v3 },
            {"v4", v4 },
            {"v5", v5 },
            {"v6", input["v6"] },
            {"v7", ProductionSystem.V7(productionParameters.T6, productionParameters.T7, y3) },
            {"v8", ProductionSystem.V8(y4, y5) },
            {"v9", ProductionSystem.V9(productionParameters.T2, productionParameters.T7, y3) },
            {"v10", input["v10"] },
            {"v11", input["v11"] },
            {"f3", ProductionSystem.F3(v2, v4) }
        };
    }
}
