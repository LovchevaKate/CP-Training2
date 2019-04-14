using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Trainings2.Models;

namespace Trainings2
{
    /// <summary>
    /// Логика взаимодействия для TrenerSportsmanInfoWindow.xaml
    /// </summary>
    public partial class TrenerSportsmanInfoWindow : Window
    {
        public TrenerSportsmanInfoWindow()
        {
            InitializeComponent();
        }

        public async Task ReadInfoAsync()
        {
            string sqlExpression = "SELECT * FROM SportsmanInfo (@idSportsman)";
            using (SqlConnection connection = Connections.TrenerConnection.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlParameter idsportsman = new SqlParameter
                {
                    ParameterName = "@idSportsman",
                    Value = WorkClass.sportsman.sportsmanid
                };

                command.Parameters.Add(idsportsman);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Sportsman sportsman = new Sportsman()
                        {
                            name = reader.GetValue(0).ToString(),
                            sname = reader.GetValue(1).ToString(),
                            phone = Convert.ToInt32(reader.GetValue(2)),
                            sex = Convert.ToChar(reader.GetValue(3)),
                            age = Convert.ToInt32(reader.GetValue(4))
                        };
                        Name.Text = sportsman.name;
                        Sname.Text = sportsman.sname;
                        Phone.Text = sportsman.phone.ToString();
                        Gender.Text = sportsman.sex.ToString();
                        Age.Text = sportsman.age.ToString();
                    }
                    connection.Close();
                }
            }
        }

        public async Task ReadParamAsync()
        {
            string sqlExpression = "SELECT * FROM SportsmanParamRead(@idSportsman, @date)";
            using (SqlConnection connection = Connections.SportsmanConnection.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlParameter idsportsman = new SqlParameter
                {
                    ParameterName = "@idSportsman",
                    Value = WorkClass.sportsman.sportsmanid
                };

                SqlParameter date = new SqlParameter
                {
                    ParameterName = "@date",
                    Value = WorkClass.param.date
                };

                command.Parameters.Add(idsportsman);
                command.Parameters.Add(date);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Param param = new Param
                        {
                            height = Convert.ToInt32(reader.GetValue(0)),
                            weight = Convert.ToInt32(reader.GetValue(1)),
                            arms = Convert.ToInt32(reader.GetValue(2)),
                            waist = Convert.ToInt32(reader.GetValue(3)),
                            chest = Convert.ToInt32(reader.GetValue(4)),
                            leg = Convert.ToInt32(reader.GetValue(5)),
                            hip = Convert.ToInt32(reader.GetValue(6))
                        };
                        HeightText.Text = param.height.ToString();
                        WeightText.Text = param.weight.ToString();
                        ArmsText.Text = param.arms.ToString();
                        WaistText.Text = param.waist.ToString();
                        ChestText.Text = param.chest.ToString();
                        LegText.Text = param.leg.ToString();
                        HipText.Text = param.hip.ToString();
                    }
                    connection.Close();
                }
            }
        }

        public async Task ReadDietAsync()
        {
            string sqlExpression = "SELECT * FROM readDiet2(@idsportsman, @dietdate)";
            using (SqlConnection connection = Connections.SportsmanConnection.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlParameter idsportsman = new SqlParameter
                {
                    ParameterName = "@idsportsman",
                    Value = WorkClass.sportsman.sportsmanid
                };

                SqlParameter dietdate = new SqlParameter
                {
                    ParameterName = "@dietdate",
                    Value = WorkClass.diet.date
                };

                command.Parameters.Add(idsportsman);
                command.Parameters.Add(dietdate);


                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Diet diet = new Diet
                    {
                        dietid = Convert.ToInt32(reader.GetValue(0)),
                        date = Convert.ToDateTime(reader.GetValue(1)),
                        dietperiod = reader.GetValue(2).ToString()
                    };
                    if (diet.dietperiod == "утро")
                        diet.morning = reader.GetValue(3).ToString();
                    else if (diet.dietperiod == "день")
                        diet.lunch = reader.GetValue(3).ToString();
                    else if (diet.dietperiod == "вечер")
                        diet.diner = reader.GetValue(3).ToString();

                    EatingDataGrid.Items.Add(diet);
                }
                connection.Close();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WorkClass.param.date = DateTime.Today;
            await ReadInfoAsync();
            await ReadParamAsync();
            WorkClass.diet.date = DateTime.Today;
            await ReadDietAsync();
        }

        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                WorkClass.param.date = Calendar.SelectedDate.Value;
                ReadParamAsync().GetAwaiter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            TrenerWindow trenerWindow = new TrenerWindow();
            trenerWindow.Show();
            Close();
        }

        private async void Calendar2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                WorkClass.diet.date = Calendar2.SelectedDate.Value;
                EatingDataGrid.Items.Clear();
                await ReadDietAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
