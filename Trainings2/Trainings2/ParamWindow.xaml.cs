using System;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Trainings2.Models;

namespace Trainings2
{
    /// <summary>
    /// Логика взаимодействия для ParamWindow.xaml
    /// </summary>
    public partial class ParamWindow : Window
    {
        Person person = new Person();
        public ParamWindow()
        {
            InitializeComponent();
            ReadInfoAsync().GetAwaiter();
        }

        public async Task ReadInfoAsync()
        {
            try
            {
                SqlConnection connection;
                if (WorkClass.role == 1)
                {
                    connection = Connections.TrenerConnection.GetConnection();
                }
                else if (WorkClass.role == 0)
                {
                    connection = Connections.SportsmanConnection.GetConnection();
                }
                else
                {
                    throw new Exception("Не выбрана роль");
                }

                string sqlExpression = "SELECT * FROM PersonInfo (@id)";
                using (connection)
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    SqlParameter id = new SqlParameter();
                    id.ParameterName = "@id";


                    if (WorkClass.role == 1)
                    {
                        id.Value = WorkClass.trener.trenerid;
                    }
                    else if (WorkClass.role == 0)
                    {
                        id.Value = WorkClass.sportsman.sportsmanid;
                    }
                    else
                    {
                        throw new Exception("Не выбрана роль");
                    }

                    command.Parameters.Add(id);

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Person person = new Person()
                            {
                                name = reader.GetValue(0).ToString(),
                                sname = reader.GetValue(1).ToString(),
                                phone = Convert.ToInt32(reader.GetValue(2)),
                                sex = Convert.ToChar(reader.GetValue(3)),
                                age = Convert.ToInt32(reader.GetValue(4))
                            };
                            this.person = person;
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CreateFile(Param param)
        {
            try
            {
                string path = @"D:\Универ\БД\Курсовой проект\Trainings2\Info\" + WorkClass.sportsman.sportsmanid +person.name+person.sname + "_param_info.txt";
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("Дата " + WorkClass.param.date.Day + " / " + WorkClass.param.date.Month + " / " + WorkClass.param.date.Year);
                        sw.WriteLine("Рост:" + param.height);
                        sw.WriteLine("Вес: " + param.weight);
                        sw.WriteLine("arms: " + param.arms);
                        sw.WriteLine("waist: " + param.waist);
                        sw.WriteLine("chest: " + param.chest);
                        sw.WriteLine("leg: " + param.leg);
                        sw.WriteLine("hip: " + param.hip);

                        sw.WriteLine();
                        sw.WriteLine();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("Дата " + DateTime.Today.Day + " / " + DateTime.Today.Month + " / " + DateTime.Today.Year);
                        sw.WriteLine("Рост:" + param.height);
                        sw.WriteLine("Вес: " + param.weight);
                        sw.WriteLine("arms: " + param.arms);
                        sw.WriteLine("waist: " + param.waist);
                        sw.WriteLine("chest: " + param.chest);
                        sw.WriteLine("leg: " + param.leg);
                        sw.WriteLine("hip: " + param.hip);

                        sw.WriteLine();
                        sw.WriteLine();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            SportsmanWindow sportsmanWindow = new SportsmanWindow();
            sportsmanWindow.Show();
            Close();
        }

        public async Task AddParamAsync()
        {
            try
            {
                using (SqlConnection cn = Connections.SportsmanConnection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("addNewBodyParam", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    SqlParameter idsportsman = new SqlParameter
                    {
                        ParameterName = "@idsportsman",
                        Value = WorkClass.sportsman.sportsmanid
                    };

                    SqlParameter date = new SqlParameter
                    {
                        ParameterName = "@date",
                        Value = WorkClass.param.date
                    };

                    SqlParameter height = new SqlParameter
                    {
                        ParameterName = "@height",
                        Value = WorkClass.param.height
                    };

                    SqlParameter weight = new SqlParameter
                    {
                        ParameterName = "@weight",
                        Value = WorkClass.param.weight
                    };

                    SqlParameter arms = new SqlParameter
                    {
                        ParameterName = "@arms",
                        Value = WorkClass.param.arms
                    };

                    SqlParameter waist = new SqlParameter
                    {
                        ParameterName = "@waist",
                        Value = WorkClass.param.waist
                    };

                    SqlParameter chest = new SqlParameter
                    {
                        ParameterName = "@chest",
                        Value = WorkClass.param.chest
                    };

                    SqlParameter leg = new SqlParameter
                    {
                        ParameterName = "@leg",
                        Value = WorkClass.param.leg
                    };

                    SqlParameter hip = new SqlParameter
                    {
                        ParameterName = "@hip",
                        Value = WorkClass.param.hip
                    };
                    
                    cmd.Parameters.Add(idsportsman);
                    cmd.Parameters.Add(date);
                    cmd.Parameters.Add(height);
                    cmd.Parameters.Add(weight);
                    cmd.Parameters.Add(arms);
                    cmd.Parameters.Add(waist);
                    cmd.Parameters.Add(chest);
                    cmd.Parameters.Add(leg);
                    cmd.Parameters.Add(hip);

                    await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Button_create_indicators_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Height.Text) && !string.IsNullOrEmpty(Weight.Text) &&
                    !string.IsNullOrEmpty(Arms.Text) && !string.IsNullOrEmpty(Waist.Text) &&
                    !string.IsNullOrEmpty(Chest.Text) && !string.IsNullOrEmpty(Leg.Text) &&
                    !string.IsNullOrEmpty(Hip.Text))
                {
                    WorkClass.param.height = Convert.ToInt32(Height.Text);
                    WorkClass.param.weight = Convert.ToInt32(Weight.Text);
                    WorkClass.param.arms = Convert.ToInt32(Arms.Text);
                    WorkClass.param.waist = Convert.ToInt32(Waist.Text);
                    WorkClass.param.chest = Convert.ToInt32(Chest.Text);
                    WorkClass.param.leg = Convert.ToInt32(Leg.Text);
                    WorkClass.param.hip = Convert.ToInt32(Hip.Text);

                    await AddParamAsync();

                    CreateFile(WorkClass.param);
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
                if (!(Regex.IsMatch(Height.Text, @"^\d*$")))
                {
                    Height.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
                else if (!(Regex.IsMatch(Weight.Text, @"^\d*$")))
                {
                    Weight.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
                else if (!(Regex.IsMatch(Arms.Text, @"^\d*$")))
                {
                    Arms.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
                else if (!(Regex.IsMatch(Waist.Text, @"^\d*$")))
                {
                    Waist.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
                else if (!(Regex.IsMatch(Chest.Text, @"^\d*$")))
                {
                    Chest.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
                else if (!(Regex.IsMatch(Leg.Text, @"^\d*$")))
                {
                    Leg.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
                else if (!(Regex.IsMatch(Hip.Text, @"^\d*$")))
                {
                    Hip.Clear();
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
