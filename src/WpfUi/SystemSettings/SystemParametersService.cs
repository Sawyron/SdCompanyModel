using WpfUi.SystemSettings.Production;

namespace WpfUi.SystemSettings;

public class SystemParametersService
{
    private ProductionInput _productionInput;

    public SystemParametersService(ProductionInput productionInput)
    {
        _productionInput = productionInput;
    }

    public ProductionInput GetProductionInput() => _productionInput;
    public void UpdateProductionInput(ProductionInput input) => _productionInput = input;
}
