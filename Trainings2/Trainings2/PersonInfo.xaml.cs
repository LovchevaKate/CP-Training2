using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Trainings2.Models;

namespace Trainings2
{
    /// <summary>
    /// Логика взаимодействия для PersonInfo.xaml
    /// </summary>
    /// 

    public partial class PersonInfo : Window
    {
        private Person person = new Person();

        public async Task ReadInfoAsync()
        {
            try
            {
                string sqlExpression;
                SqlConnection connection;
                if (WorkClass.role == 1)
                {
                    connection = Connections.TrenerConnection.GetConnection();
                    sqlExpression = "SELECT * FROM TrenerInfo (@id)";
                }
                else if (WorkClass.role == 0)
                {
                    connection = Connections.SportsmanConnection.GetConnection();
                    sqlExpression = "SELECT * FROM SportsmanInfo (@id)";
                }
                else
                {
                    throw new Exception("Не выбрана роль");
                }
                               
               
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
                                name = reader.GetValue(1).ToString(),
                                sname = reader.GetValue(0).ToString(),
                                phone = Convert.ToInt32(reader.GetValue(2)),
                                sex = Convert.ToChar(reader.GetValue(3)),
                                age = Convert.ToInt32(reader.GetValue(4))
                            };
                            Name.Text = person.name;
                            Sname.Text = person.sname;
                            Phone.Text = person.phone.ToString();
                            Sex.Text = person.sex.ToString();
                            Age.Text = person.age.ToString();
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

        public async Task UpdateInfoAsync()
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

                using (connection)
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("UpdatePerson", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    SqlParameter id = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = person.personid
                    };

                    SqlParameter name = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = person.name
                    };

                    SqlParameter sname = new SqlParameter
                    {
                        ParameterName = "@sname",
                        Value = person.sname
                    };

                    SqlParameter phone = new SqlParameter
                    {
                        ParameterName = "@phone",
                        Value = person.phone
                    };

                    SqlParameter age = new SqlParameter
                    {
                        ParameterName = "@age",
                        Value = person.age
                    };

                    cmd.Parameters.Add(id);
                    cmd.Parameters.Add(name);
                    cmd.Parameters.Add(sname);
                    cmd.Parameters.Add(phone);
                    cmd.Parameters.Add(age);

                    await cmd.ExecuteNonQueryAsync();
                    MessageBox.Show("Update");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public PersonInfo()
        {
            InitializeComponent();
            ReadInfoAsync().GetAwaiter();
        }

       private async void Button_update_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WorkClass.role == 1)
                {
                    person.personid = WorkClass.trener.trenerid;
                }
                else if (WorkClass.role == 0)
                {
                    person.personid = WorkClass.sportsman.sportsmanid;
                }
                else
                {
                    throw new Exception("Не выбрана роль");
                }
                if (!string.IsNullOrEmpty(NameText.Text) && !string.IsNullOrEmpty(SnameText.Text) &&
                    !string.IsNullOrEmpty(PhoneText.Text) && !string.IsNullOrEmpty(AgeText.Text))
                {
                    person.name = NameText.Text;
                    person.sname = SnameText.Text;
                    person.phone = Convert.ToInt32(PhoneText.Text);
                    person.age = Convert.ToInt32(AgeText.Text);

                    await UpdateInfoAsync();

                    NameText.Clear();
                    SnameText.Clear();
                    PhoneText.Clear();
                    AgeText.Clear();
                    Name.Text = String.Empty;
                    Sname.Text = String.Empty;
                    Age.Text = String.Empty;
                    Phone.Text = String.Empty;

                    await ReadInfoAsync();
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

        private void Button_exit_Click(object sender, RoutedEventArgs e)
        {
            if (WorkClass.role == 1)
            {
                TrenerWindow trenerWindow = new TrenerWindow();
                trenerWindow.Show();
            }
            else if (WorkClass.role == 0)
            {
                SportsmanWindow sportsmanWindow = new SportsmanWindow();
                sportsmanWindow.Show();
            }
            else
            {
                throw new Exception("Не выбрана роль");
            }

            Close();
        }

        private void Number_reg_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!(Regex.IsMatch(PhoneText.Text, @"^\d*$")))
                {
                    PhoneText.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
                else if (!(Regex.IsMatch(AgeText.Text, @"^\d*$")))
                {
                    AgeText.Clear();
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