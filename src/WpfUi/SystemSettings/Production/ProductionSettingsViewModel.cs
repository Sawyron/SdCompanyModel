using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfUi.SystemSettings.Production;

public class ProductionSettingsViewModel : ObservableObject
{
    public ProductionSettingsViewModel(SystemParametersService parametersService)
    {
        var values = parametersService.GetProductionInput();
        _y3 = values.Y3;
        _v1 = values.V1;
        _v10 = values.V10;
        _v11 = values.V11;
        _k = values.K;
        _beta = values.Beta;
        _t2 = values.T2;
        _t3 = values.T3;
        _t4 = values.T4;
        _t5 = values.T5;
        _t6 = values.T6;
        _t7 = values.T7;
    }

    private double _y3;
	public double Y3
    {
        get => _y3;
        set => SetProperty(ref _y3, value);
    }

	private double _v1;
	public double V1
    {
        get => _v1;
        set => SetProperty(ref _v1, value);
    }

    private double _v6;
    public double V6
    {
        get => _v6;
        set => SetProperty(ref _v6, value);
    }

    private double _v10;
    public double V10
    {
        get => _v10;
        set => SetProperty(ref _v10, value);
    }

    private double _v11;
    public double V11
    {
        get => _v11;
        set => SetProperty(ref _v11, value);
    }

    private double _k;
    public double K
    {
        get => _k;
        set => SetProperty(ref _k, value);
    }

    private double _beta;
    public double Beta
    {
        get => _beta;
        set => SetProperty(ref _beta, value);
    }

    private double _t2;
    public double T2
    {
        get => _t2;
        set => SetProperty(ref _t2, value);
    }

    private double _t3;
    public double T3
    {
        get => _t3;
        set => SetProperty(ref _t3, value);
    }

    private double _t4;
    public double T4
    {
        get => _t4;
        set => SetProperty(ref _t4, value);
    }

    private double _t5;
    public double T5
    {
        get => _t5;
        set => SetProperty(ref _t5, value);
    }

    private double _t6;
    public double T6
    {
        get => _t6;
        set => SetProperty(ref _t6, value);
    }

    private double _t7;
    public double T7
    {
        get => _t7;
        set => SetProperty(ref _t7, value);
    }

    private double _t8;

    public double T8
    {
        get => _t8;
        set => SetProperty(ref _t8, value);
    }

    private ProductionInput ToInput() => new(
        _y3,
        _v1,
        _v6,
        _v10,
        _v11,
        _t2,
        _t3,
        _t4,
        _t5,
        _t6,
        _t7,
        _k,
        _beta
        );
}
