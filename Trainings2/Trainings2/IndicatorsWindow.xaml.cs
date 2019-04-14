using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Trainings2.Models;

namespace Trainings2
{
    /// <summary>
    /// Логика взаимодействия для IndicatorsWindow.xaml
    /// </summary>
    public partial class IndicatorsWindow : Window
    {
        public IndicatorsWindow()
        {
            InitializeComponent();
        }

        Indicator indicator = new Indicator();

        public async Task UpdateIdicatorsAsync()
        {
            try
            {
                using (SqlConnection connection = Connections.TrenerConnection.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("UpdateIndicator", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter id = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = WorkClass.training.idtraining
                    };

                    SqlParameter pulsunwork = new SqlParameter
                    {
                        ParameterName = "@pulsunwork",
                        Value = WorkClass.indicator.pulseUnWork
                    };

                    SqlParameter pulswork = new SqlParameter
                    {
                        ParameterName = "@pulswork",
                        Value = WorkClass.indicator.pulseWork
                    };

                    SqlParameter pressuresistol = new SqlParameter
                    {
                        ParameterName = "@pressuresistol",
                        Value = WorkClass.indicator.pressureSistol
                    };

                    SqlParameter pressurediastol = new SqlParameter
                    {
                        ParameterName = "@pressurediastol",
                        Value = WorkClass.indicator.pressureDiastol
                    };

                    cmd.Parameters.Add(id);
                    cmd.Parameters.Add(pulsunwork);
                    cmd.Parameters.Add(pulswork);
                    cmd.Parameters.Add(pressuresistol);
                    cmd.Parameters.Add(pressurediastol);

                    await cmd.ExecuteNonQueryAsync();
                    MessageBox.Show("Update");
                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task ReadIndicatorAsync()
        {
            string sqlExpression = "SELECT * FROM IndicatorInfo (@id)";
            using (SqlConnection connection = Connections.TrenerConnection.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlParameter id = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = WorkClass.training.idtraining
                };

                command.Parameters.Add(id);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Indicator indicator = new Indicator()
                        {
                            pulseWork = Convert.ToInt32(reader.GetValue(1)),
                            pulseUnWork = Convert.ToInt32(reader.GetValue(0)),
                            pressureSistol = Convert.ToInt32(reader.GetValue(2)),
                            pressureDiastol = Convert.ToInt32(reader.GetValue(3))

                        };

                        PulseWorkText.Text = indicator.pulseWork.ToString();
                        PulseUnWorkText.Text = indicator.pulseUnWork.ToString();
                        PressureSistolText.Text = indicator.pressureSistol.ToString();
                        PressureDiastolText.Text = indicator.pressureDiastol.ToString();

                    }
                    connection.Close();
                }
            }
        }

        public async Task AddIndicatorAsync()
        {
            try
            {                
                using (SqlConnection cn = Connections.TrenerConnection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("addNewIndicators", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter id = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = WorkClass.training.idtraining
                    };
                    SqlParameter pulsunwork = new SqlParameter
                    {
                        ParameterName = "@pulsunwork",
                        Value = indicator.pulseUnWork
                    };

                    SqlParameter pulswork = new SqlParameter
                    {
                        ParameterName = "@pulswork",
                        Value = indicator.pulseWork
                    };

                    SqlParameter pressuresistol = new SqlParameter
                    {
                        ParameterName = "@pressuresistol",
                        Value = indicator.pressureSistol
                    };

                    SqlParameter pressurediastol = new SqlParameter
                    {
                        ParameterName = "@pressurediastol",
                        Value = indicator.pressureDiastol
                    };

                    cmd.Parameters.Add(id);
                    cmd.Parameters.Add(pulsunwork);
                    cmd.Parameters.Add(pulswork);
                    cmd.Parameters.Add(pressuresistol);
                    cmd.Parameters.Add(pressurediastol);

                    await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            try
            {
                TrenerWindow trenerWindow = new TrenerWindow();
                trenerWindow.Show();
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Button_create_indicators_Click(object sender, RoutedEventArgs e)
        {
            try
            {    
                if (!string.IsNullOrEmpty(PulseUnWork.Text) && !string.IsNullOrEmpty(PulseWork.Text) &&
                    !string.IsNullOrEmpty(PressureSistol.Text) && !string.IsNullOrEmpty(PressureDiastol.Text))
                {
                    indicator.pulseUnWork = Convert.ToInt32(PulseUnWork.Text);
                    indicator.pulseWork = Convert.ToInt32(PulseWork.Text);
                    indicator.pressureSistol = Convert.ToInt32(PressureSistol.Text);
                    indicator.pressureDiastol = Convert.ToInt32(PressureDiastol.Text);

                    await AddIndicatorAsync();

                    PulseUnWork.Clear();
                    PulseWork.Clear();
                    PressureSistol.Clear();
                    PressureDiastol.Clear();

                    await ReadIndicatorAsync();
                }
                else
                {
                    throw new Exception("Поля не заполнены");
                }                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReadIndicatorAsync().GetAwaiter();
        }

        private async void Button_update_indicators_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (!string.IsNullOrEmpty(PulseUnWork.Text) && !string.IsNullOrEmpty(PulseWork.Text) &&
                    !string.IsNullOrEmpty(PressureSistol.Text) && !string.IsNullOrEmpty(PressureDiastol.Text))
                {
                    WorkClass.indicator.pulseUnWork = Convert.ToInt32(PulseUnWork.Text);
                    WorkClass.indicator.pulseWork = Convert.ToInt32(PulseWork.Text);
                    WorkClass.indicator.pressureSistol = Convert.ToInt32(PressureSistol.Text);
                    WorkClass.indicator.pressureDiastol = Convert.ToInt32(PressureDiastol.Text);

                    await UpdateIdicatorsAsync();

                    PulseUnWork.Clear();
                    PulseWork.Clear();
                    PressureSistol.Clear();
                    PressureDiastol.Clear();

                    await ReadIndicatorAsync();
                }
                else
                {
                    throw new Exception("Поля не заполнены");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Number_reg_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (!(Regex.IsMatch(PulseUnWork.Text, @"^\d*$")))
                {
                    PulseUnWork.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
                else if (!(Regex.IsMatch(PulseWork.Text, @"^\d*$")))
                {
                    PulseWork.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
                else if (!(Regex.IsMatch(PressureSistol.Text, @"^\d*$")))
                {
                    PressureSistol.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
                else if (!(Regex.IsMatch(PressureDiastol.Text, @"^\d*$")))
                {
                    PressureDiastol.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
