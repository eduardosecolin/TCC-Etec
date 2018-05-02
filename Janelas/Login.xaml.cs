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
using BarberSystem.Utils;
using BarberSystem.Dados;

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Login.xaml
    /// </summary>
    public partial class Login : Window {

        public static string usuarioLogado;

        public Login() {
            InitializeComponent();
        }

        //Botao de entrar
        private void button_Click(object sender, RoutedEventArgs e) {
            try {
                BancoDados bd = new BancoDados();
               /* var sql = from u in bd.USUARIOS
                          where u.nome_usuario == txtUsuario.Text && u.senha == txtSenha.Password.ToString()
                          select u.codigo;*/
                if (txtUsuario.Text == "" || txtSenha.Password.ToString() == "") {
                    MessageBox.Show("Campo usuário ou senha vazio!");
                    txtUsuario.Focus();
                    return;
                }
                var query = bd.USUARIOS.Where(usuario => usuario.nome_usuario == txtUsuario.Text);
                USUARIOS usu = query.FirstOrDefault();
                string usuResult = usu.nome_usuario;
                string usuSenhaResult = usu.senha;

                if((txtUsuario.Text == usu.nome_usuario) && Util.descrypt(txtSenha.Password.ToString(), usu.senha)){
                    usuarioLogado = txtUsuario.Text;
                    Menu janela = new Menu();
                    janela.Show();
                    this.Hide();
                    Close();
                }else{
                    MessageBox.Show("Usuário ou senha inválidos!");
                    txtUsuario.Clear();
                    txtSenha.Clear();
                    txtUsuario.Focus();
                }
           

                /*  if (sql.FirstOrDefault() == 0) {
                      MessageBox.Show("Usuário ou senha inválidos!");
                      txtUsuario.Clear();
                      txtSenha.Clear();
                      txtUsuario.Focus();
                  }
                  else {
                      usuarioLogado = txtUsuario.Text;
                      Menu janela = new Menu();
                      janela.Show();
                      this.Hide();
                      Close();
                  }*/
            }
            catch(Exception a){
                MessageBox.Show(a.Message);
            }
        }

        //Setar foco no usuarios quando o form for carregado
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            txtUsuario.Focus();
        }

        //Metodo para limpar campos(textBox)
        public void limparTextBox(){
            txtUsuario.Clear();
            txtSenha.Clear();
            txtUsuario.Focus();
        }

        //Botao limpar
        private void button_Copy_Click(object sender, RoutedEventArgs e) {
            limparTextBox();
        }

        //Botao sair
        private void button_Copy1_Click(object sender, RoutedEventArgs e) {
            App.Current.Shutdown();
        }

        // entrar com a tecla enter
        private void txtSenha_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                button_Click(sender, e);
            }
        }
    }
}
