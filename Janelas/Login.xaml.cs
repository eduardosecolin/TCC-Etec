using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Data.SqlClient;

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Login.xaml
    /// </summary>
    public partial class Login : Window {
        public Login() {
            InitializeComponent();
        }

        private void lblCadastro_MouseMove(object sender, MouseEventArgs e) => lblCadastro.Foreground = Brushes.LightSeaGreen;

        private void lblCadastro_MouseLeave(object sender, MouseEventArgs e) => lblCadastro.Foreground = Brushes.White;

        private void button_Click(object sender, RoutedEventArgs e) {
            BancoDados bd = new BancoDados();
            var sql = from u in bd.USUARIOS where u.nome_usuario == txtUsuario.Text && u.senha == txtSenha.Password.ToString()
            select u.codigo;
            if (txtUsuario.Text == "" || txtSenha.Password.ToString() == "") {
                MessageBox.Show("Campo usuário ou senha vazio!");
                txtUsuario.Focus();
                return;
            }
            if (sql.FirstOrDefault() == 0){
                MessageBox.Show("Usuário ou senha inválidos!");
                txtUsuario.Clear();
                txtSenha.Clear();
                txtUsuario.Focus();
            }
            else{
                MessageBox.Show("Login realizado com sucesso!!!");
                Menu janela = new Menu();
                janela.Show();
                this.Hide();
                Close();
            }
                    
        }
    }
}
