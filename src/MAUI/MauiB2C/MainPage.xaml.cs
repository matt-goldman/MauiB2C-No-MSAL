using MauiB2C.Services;
using MauiB2CNoMsal.Shared;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace MauiB2C;

public partial class MainPage : ContentPage
{
    private readonly IAuthService _authService;

    private readonly HttpClient _http;

    public ObservableCollection<WeatherForecast> Forecasts { get; set; } = new();

    public MainPage(IAuthService authService, IHttpClientFactory clientFactory)
    {
        InitializeComponent();
        _authService = authService;
        _http = clientFactory.CreateClient(AuthService.AuthenticatedClient);
        ForecastsCollection.ItemsSource = Forecasts;
        RunningIndicator.IsVisible = false;
    }

    private async void Login_Clicked(object sender, EventArgs e)
    {
        RunningIndicator.IsVisible = true;

        var loggedIn = await _authService.LoginAsync();

        if (loggedIn)
        {
            var forecasts = await _http.GetFromJsonAsync<List<WeatherForecast>>("WeatherForecast");

            foreach(var forecast in forecasts)
            {
                Forecasts.Add(forecast);
                LoginButton.IsVisible = false;
            }
        }
        else
        {
            await DisplayAlert("Error", "You are not logged in, please try again.", "OK");
        }

        RunningIndicator.IsVisible = false;
    }
}