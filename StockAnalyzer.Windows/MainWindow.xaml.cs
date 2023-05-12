﻿using StockAnalyzer.Core;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace StockAnalyzer.Windows;

public partial class MainWindow : Window
{
    private static string API_URL = "https://ps-async.fekberg.com/api/stocks";
    private Stopwatch stopwatch = new Stopwatch();

    public MainWindow()
    {
        InitializeComponent();
    }



    private async void Search_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            BeforeLoadingStockData();
            await GetStockPrices();
        }



        //using (var client = new HttpClient())
        //{
        //    var responseTask = client.GetAsync($"{API_URL}/{StockIdentifier.Text}");

        //    var response = await responseTask;

        //    var content = await response.Content.ReadAsStringAsync();

        //    var data = JsonConvert.DeserializeObject<IEnumerable<StockPrice>>(content);

        //    Stocks.ItemsSource = data;

        //}

        catch(Exception ex)
        {
            Notes.Text = ex.Message;
        }
            
        

        finally
        {
            AfterLoadingStockData();
        }
        
    }

    private async Task GetStockPrices()
    {
        try
        {

            var dataStore = new DataStore();

            var responseTask = await dataStore.GetStockPrices(StockIdentifier.Text);

            Stocks.ItemsSource = responseTask;

        }

        catch (System.Exception ex)
        {
            //Notes.Text = ex.Message;
            throw;
        }
    }

    private void BeforeLoadingStockData()
    {
        stopwatch.Restart();
        StockProgress.Visibility = Visibility.Visible;
        StockProgress.IsIndeterminate = true;
    }

    private void AfterLoadingStockData()
    {
        StocksStatus.Text = $"Loaded stocks for {StockIdentifier.Text} in {stopwatch.ElapsedMilliseconds}ms";
        StockProgress.Visibility = Visibility.Hidden;
    }

    private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo { FileName = e.Uri.AbsoluteUri, UseShellExecute = true });

        e.Handled = true;
    }

    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}