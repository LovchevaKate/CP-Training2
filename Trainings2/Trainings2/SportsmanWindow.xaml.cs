using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trainings2.Models;
using System.IO;

namespace Trainings2
{
    /// <summary>
    /// Логика взаимодействия для SportsmanWindow.xaml
    /// </summary>
    public partial class SportsmanWindow : Window
    {
        public SportsmanWindow()
        {
            InitializeComponent();
        }

        public async Task ReadAllTrainingAsync()
        {
            string sqlExpression = "SELECT * FROM TrainingSportsman (@idSportsman)";
            using (SqlConnection connection = Connections.SportsmanConnection.GetConnection())
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
                        DateTime date = Convert.ToDateTime(reader.GetValue(1));
                        DateTime time = Convert.ToDateTime(reader.GetValue(1));

                        Training training = new Training
                        {
                            idtraining = Convert.ToInt32(reader.GetValue(0)),
                            date = date.Date,
                            time = date.TimeOfDay,
                            type = Convert.ToString(reader.GetValue(2))
                        };

                        TrainingsDataGrid.Items.Add(training);
                    }
                    connection.Close();
                }
            }
        }

        public async Task DeleteParamAsync()
        {
            try
            {
                using (SqlConnection cn = Connections.SportsmanConnection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("deleteBodyParam", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    SqlParameter date = new SqlParameter
                    {
                        ParameterName = "@date",
                        Value = WorkClass.param.date
                    };
                    
                    cmd.Parameters.Add(date);

                    await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReadAllTrainingAsync().GetAwaiter();
            WorkClass.param.date = DateTime.Today;
            ReadParamAsync().GetAwaiter();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            Close();
        }

        private void TrainingsDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                WorkClass.training = TrainingsDataGrid.SelectedItem as Training;
                if (WorkClass.training == null)
                {
                    throw new Exception("Тренировка не выбрана.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_AddParam(object sender, RoutedEventArgs e)
        {
            ParamWindow paramWindow = new ParamWindow();
            paramWindow.Show();
            Close();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                WorkClass.param.date = Calendar.SelectedDate.Value;
                HeightText.Text = String.Empty;
                WeightText.Text = String.Empty;
                ArmsText.Text = String.Empty;
                WaistText.Text = String.Empty;
                ChestText.Text = String.Empty;
                LegText.Text = String.Empty;
                HipText.Text = String.Empty;
                ReadParamAsync().GetAwaiter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_EatInfo(object sender, RoutedEventArgs e)
        {
            SportsmanInfoWindow sportsmanInfoWindow = new SportsmanInfoWindow();
            sportsmanInfoWindow.Show();
            Close();
        }

        private void Button_info_Click(object sender, RoutedEventArgs e)
        {
            PersonInfo personInfo = new PersonInfo();
            personInfo.Show();
            Close();
        }

        private void Button_Click_DeleteParam(object sender, RoutedEventArgs e)
        {
            try
            {
                DeleteParamAsync().GetAwaiter();
                HeightText.Text = String.Empty;
                WeightText.Text = String.Empty;
                ArmsText.Text = String.Empty;
                WaistText.Text = String.Empty;
                ChestText.Text = String.Empty;
                LegText.Text = String.Empty;
                HipText.Text = String.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}