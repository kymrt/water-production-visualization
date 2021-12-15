using WaterProductionVisualization.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace WaterProductionVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string BaseAddress = "https://openapi.izmir.bel.tr/";
        private readonly string WaterProductionRequestUri = "api/izsu/suuretiminindagilimi/";
        private List<WaterProductionEntity> WaterProductionEntity;

        public MainWindow()
        {
            InitializeComponent();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(WaterProductionRequestUri).Result;

                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var waterProductionResponse = response.Content.ReadAsStringAsync().Result;
                    WaterProductionEntity = JsonConvert.DeserializeObject<List<WaterProductionEntity>>(waterProductionResponse);
                    List<string> barrageList = WaterProductionEntity.Select(x => x.UretimKaynagi).Distinct().ToList();
                    BarrageCombo.ItemsSource = barrageList;
                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (BarrageCombo.SelectedItem != null && !string.IsNullOrWhiteSpace(BarrageCombo.SelectedItem.ToString()))
            {
                int startMonth = 1;
                int endMonth = DateTime.Now.Month;
                int startYear = 2009;
                int endYear = DateTime.Now.Year;
                if (MinDate.SelectedDate.HasValue)
                {
                    startMonth = MinDate.SelectedDate.Value.Month;
                    startYear = MinDate.SelectedDate.Value.Year;
                }
                if (MaxDate.SelectedDate.HasValue)
                {
                    endMonth = MaxDate.SelectedDate.Value.Month;
                    endYear = MaxDate.SelectedDate.Value.Year;
                }
                var resultOfFilter = WaterProductionEntity.Where(x => x.UretimKaynagi == BarrageCombo.SelectedItem.ToString()
                                                                && new DateTime(x.Yil, x.Ay, 1) > new DateTime(startYear, startMonth, 1)
                                                                && new DateTime(x.Yil, x.Ay, 1) < new DateTime(endYear, endMonth, 1))
                                                            .Select(x => new
                                                            {
                                                                Key = string.Format("{0}-{1}", x.Yil, x.Ay),
                                                                Value = x.UretimMiktari
                                                            }).ToList();
                ((LineSeries)mcChart.Series[0]).ItemsSource = resultOfFilter;
            }
        }
    }
}
