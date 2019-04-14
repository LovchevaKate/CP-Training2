using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trainings2.Models;
using System.Data;
using System.Text.RegularExpressions;

namespace Trainings2
{
    /// <summary>
    /// Логика взаимодействия для TrenerWindow.xaml
    /// </summary>
    public partial class TrenerWindow : Window
    {       

        public TrenerWindow()
        {
            InitializeComponent();
        }

        public async Task ReadAllSportsmanAsync()
        {
            string sqlExpression = "SELECT * FROM allSportsman (@idCoach)";
            using (SqlConnection connection = Connections.TrenerConnection.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlParameter idcoach = new SqlParameter
                {
                    ParameterName = "@idCoach",
                    Value = WorkClass.trener.trenerid
                };

                command.Parameters.Add(idcoach);

                SqlDataReader reader = await command.ExecuteReaderAsync();
              
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Sportsman sportsman = new Sportsman()
                        {
                            sportsmanid = Convert.ToInt32(reader.GetValue(0)),
                            name = reader.GetValue(1).ToString(),
                            sname = reader.GetValue(2).ToString(),
                            phone = Convert.ToInt32(reader.GetValue(3)),
                            age = Convert.ToInt32(reader.GetValue(5))
                        };

                        SportsmanDataGrid.Items.Add(sportsman);
                    }
                    connection.Close();
                }
            }
        }

        public async Task ReadAllTrainingAsync()
        {
            string sqlExpression = "SELECT * FROM trainingTrener (@idTrener)";
            using (SqlConnection connection = Connections.TrenerConnection.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlParameter idcoach = new SqlParameter
                {
                    ParameterName = "@idTrener",
                    Value = WorkClass.trener.trenerid
                };

                command.Parameters.Add(idcoach);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime date = Convert.ToDateTime(reader.GetValue(2));
                        DateTime time = Convert.ToDateTime(reader.GetValue(2));

                        Training training = new Training
                        {
                            idtraining = Convert.ToInt32(reader.GetValue(0)),
                            idsportsman = Convert.ToInt32(reader.GetValue(1)),
                            date = date.Date,
                            time = date.TimeOfDay,
                            type = Convert.ToString(reader.GetValue(3))
                        };

                        TrainingsDataGrid.Items.Add(training);
                    }
                    connection.Close();
                }
            }
        }

        public async Task DeleteSportsmanAsync()
        {
            try
            {
                using (SqlConnection cn = Connections.TrenerConnection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("deleteSportsman", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter idsportsman = new SqlParameter
                    {
                        ParameterName = "@idsportsman",
                        Value = WorkClass.sportsman.sportsmanid
                    };
                
                    cmd.Parameters.Add(idsportsman);

                    await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task AddTrainingAsync()
        {
            try
            {
                using (SqlConnection cn = Connections.TrenerConnection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("addNewTraining", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter idcoach = new SqlParameter
                    {
                        ParameterName = "@idcoach",
                        Value = WorkClass.trener.trenerid
                    };

                    SqlParameter idsportsman = new SqlParameter
                    {
                        ParameterName = "@idsportsman",
                        Value = WorkClass.sportsman.sportsmanid
                    };

                    DateTime d = WorkClass.training.date;
                    d = d.Add(WorkClass.training.time);

                    SqlParameter date = new SqlParameter
                    {
                        ParameterName = "@date",
                        Value = d
                    };

                    SqlParameter type = new SqlParameter
                    {
                        ParameterName = "@type",
                        Value = Type.Text
                    };

                    cmd.Parameters.Add(idcoach);
                    cmd.Parameters.Add(idsportsman);
                    cmd.Parameters.Add(date);
                    cmd.Parameters.Add(type);

                    await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task DeleteTrainingAsync()
        {
            try
            {
                using (SqlConnection cn = Connections.TrenerConnection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("deleteTraining", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter idtraining = new SqlParameter
                    {
                        ParameterName = "@idtraining",
                        Value = WorkClass.training.idtraining
                    };

                    cmd.Parameters.Add(idtraining);

                    await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                    MessageBox.Show("Delete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadAllSportsmanAsync().GetAwaiter();
                ReadAllTrainingAsync().GetAwaiter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //выход
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow mainwindow = new MainWindow();
                mainwindow.Show();
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SportsmanDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                WorkClass.sportsman = SportsmanDataGrid.SelectedItem as Sportsman;
                if (WorkClass.sportsman == null)
                {
                    throw new Exception("Спортсмен не выбран.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //удаление спортсмена
        private async void Button_Click_DeleteSportsman(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WorkClass.sportsman.sportsmanid != 0)
                {
                    await DeleteSportsmanAsync();
                    SportsmanDataGrid.Items.Clear();
                    await ReadAllSportsmanAsync();
                }
                else
                {
                    throw new Exception("Спортсмен не выбран!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
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

        private void Button_Click_Info(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WorkClass.sportsman.sportsmanid != 0)
                {
                    TrenerSportsmanInfoWindow trenerSportsmanInfoWindow = new TrenerSportsmanInfoWindow();
                    trenerSportsmanInfoWindow.Show();
                    Close();
                }
                else
                {
                    throw new Exception("Спортсмен не выбран!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Button_Click_AddTraining(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Calendar.Text) && !string.IsNullOrEmpty(Time.Text) &&
                    !string.IsNullOrEmpty(Type.Text) && WorkClass.sportsman.sportsmanid != 0)
                {
                    await AddTrainingAsync();
                    TrainingsDataGrid.Items.Clear();
                    await ReadAllTrainingAsync();
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

        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {    
                if (Calendar.SelectedDate.Value < DateTime.Today.Date)
                {
                    throw new Exception("Неверная дата");

                }
                else if (Calendar.SelectedDate.Value == DateTime.Today.Date)
                {
                    throw new Exception("На сегодня тренировку нельзя поставить!");
                }
                else
                {
                    WorkClass.training.date = Calendar.SelectedDate.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Time_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            try
            {                
                WorkClass.training.time = Time.SelectedTime.Value.TimeOfDay;               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Button_Click_DeleteTraining(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WorkClass.training.idtraining != 0)
                {
                    await DeleteTrainingAsync();
                    TrainingsDataGrid.Items.Clear();
                    await ReadAllTrainingAsync();
                }
                else
                {
                    throw new Exception("Тренировка не выбрана!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void Button_Click_InfoTraining(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WorkClass.training.idtraining != 0)
                {
                    IndicatorsWindow indicatorsWindow = new IndicatorsWindow();
                    indicatorsWindow.Show();
                    Close();
                }
                else
                {
                    throw new Exception("Тренировка не выбрана!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Button_Click_Import(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection cn = Connections.Connection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("ImportFromXML", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter path = new SqlParameter
                    {
                        ParameterName = "@path",
                        Value = "D:\\Универ\\БД\\Курсовой проект\\Food_XML.xml"
                    };

                    SqlParameter rc = new SqlParameter
                    {
                        ParameterName = "@rc",
                        Value = 1
                    };

                    cmd.Parameters.Add(path);
                    cmd.Parameters.Add(rc);

                    MessageBox.Show("Create");

                    await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
                
        private void Button_Click_Export(object sender, RoutedEventArgs e)
        {
            try
            {

                using (SqlConnection conection = Connections.TrenerConnection.GetConnection())
                {
                    conection.Open();

                    string str1 = "exec TakeXMLBodyParameters";
                    string str2 = "exec TakeXMLCoach";
                    string str3 = "exec TakeXMLDiet";
                    string str4 = "exec TakeXMLFood";
                    string str5 = "exec TakeXMLIndicators";
                    string str6 = "exec TakeXMLMeal";
                    string str7 = "exec TakeXMLMenu";
                    string str8 = "exec TakeXMLPerson";
                    string str9 = "exec TakeXMLSportParam";
                    string str10 = "exec TakeXMLSportsman";
                    string str11 = "exec TakeXMLTrainings";

                    SqlDataAdapter da = new SqlDataAdapter(str1, conection);
                    SqlDataAdapter da2 = new SqlDataAdapter(str2, conection);
                    SqlDataAdapter da3 = new SqlDataAdapter(str3, conection);
                    SqlDataAdapter da4 = new SqlDataAdapter(str4, conection);
                    SqlDataAdapter da5 = new SqlDataAdapter(str5, conection);
                    SqlDataAdapter da6 = new SqlDataAdapter(str6, conection);
                    SqlDataAdapter da7 = new SqlDataAdapter(str7, conection);
                    SqlDataAdapter da8 = new SqlDataAdapter(str8, conection);
                    SqlDataAdapter da9 = new SqlDataAdapter(str9, conection);
                    SqlDataAdapter da10 = new SqlDataAdapter(str10, conection);
                    SqlDataAdapter da11 = new SqlDataAdapter(str11, conection);

                    DataSet ds = new DataSet();
                    DataSet ds2 = new DataSet();
                    DataSet ds3 = new DataSet();
                    DataSet ds4 = new DataSet();
                    DataSet ds5 = new DataSet();
                    DataSet ds6 = new DataSet();
                    DataSet ds7 = new DataSet();
                    DataSet ds8 = new DataSet();
                    DataSet ds9 = new DataSet();
                    DataSet ds10 = new DataSet();
                    DataSet ds11 = new DataSet();


                    da.Fill(ds);
                    da2.Fill(ds2);
                    da3.Fill(ds3);
                    da4.Fill(ds4);
                    da5.Fill(ds5);
                    da6.Fill(ds6);
                    da7.Fill(ds7);
                    da8.Fill(ds8);
                    da9.Fill(ds9);
                    da10.Fill(ds10);
                    da11.Fill(ds11);

                    conection.Close();

                    ds.WriteXml(@"..\..\..\xml\BodyParameters.xml");
                    ds2.WriteXml(@"..\..\..\xml\Coach.xml");
                    ds3.WriteXml(@"..\..\..\xml\Diet.xml");
                    ds4.WriteXml(@"..\..\..\xml\Food.xml");
                    ds5.WriteXml(@"..\..\..\xml\Indicators.xml");
                    ds6.WriteXml(@"..\..\..\xml\Meal.xml");
                    ds7.WriteXml(@"..\..\..\xml\Menu.xml");
                    ds8.WriteXml(@"..\..\..\xml\Person.xml");
                    ds9.WriteXml(@"..\..\..\xml\SportParam.xml");
                    ds10.WriteXml(@"..\..\..\xml\Sportsman.xml");
                    ds11.WriteXml(@"..\..\..\xml\Trainings.xml");

                    MessageBox.Show("Successfully..");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_info_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PersonInfo personInfo = new PersonInfo();
                personInfo.Show();
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Type_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!(Regex.IsMatch(Type.Text, @"^[а-яА-ЯёЁa-zA-Z]*$")))
                {
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
