using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Trainings2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int id = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RadioButton_trener_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            WorkClass.role = 1;
        }

        private void RadioButton_sportsman_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            WorkClass.role = 0;
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                SqlConnection cn;
                if (WorkClass.role == 1)
                {
                    cn = Connections.TrenerConnection.GetConnection();
                }
                else if (WorkClass.role == 0)
                {
                    cn = Connections.SportsmanConnection.GetConnection();
                }
                else
                {
                    throw new Exception("Не выбрана роль");
                }

                if (string.IsNullOrEmpty(Login.Text))
                { 
                    throw new Exception("Поля не заполнены");
                }

                string loginParam = Login.Text;

                using (cn)
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("checkRole", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter login = new SqlParameter
                    {
                        ParameterName = "@login",
                        Value = loginParam
                    };

                    SqlParameter typerole = new SqlParameter
                    {
                        ParameterName = "@typerole",
                        Value = WorkClass.role
                    };

                    SqlParameter iduser = new SqlParameter
                    {
                        ParameterName = "@iduser",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    };

                    cmd.Parameters.Add(login);
                    cmd.Parameters.Add(typerole);
                    cmd.Parameters.Add(iduser);

                    cmd.ExecuteNonQuery();

                    

                    id = Convert.ToInt32(cmd.Parameters["@iduser"].Value);
                    if (id < 0)
                    {
                        throw new Exception("Пользователь не найден");
                    }
                    else
                    {
                        if(WorkClass.role == 1)
                        {
                            WorkClass.trener.trenerid = id;
                            TrenerWindow trenerWindow = new TrenerWindow();
                            trenerWindow.Show();
                            Close();
                        }
                        else if (WorkClass.role == 0)
                        {
                            WorkClass.sportsman.sportsmanid = id;
                            SportsmanWindow sportsmanWindow = new SportsmanWindow();
                            sportsmanWindow.Show();
                            Close();
                        }
                    }

                    cn.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
                
        private void Registrationtrener_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            Close();
        }

        private void Registrationsportsman_Click(object sender, RoutedEventArgs e)
        {
            RegistrationSportsmanWindow registrationSportsmanWindow = new RegistrationSportsmanWindow();
            registrationSportsmanWindow.Show();
            Close();
        }
    }
}
