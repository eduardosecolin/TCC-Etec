﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using BarberSystem.Dados;

namespace BarberSystem.Janelas
{
    /// <summary>
    /// Lógica interna para Menu.xaml
    /// </summary>
    public  partial class Menu : Window
    {
        BancoDados conexao = new BancoDados();
        public Menu()
        {
            InitializeComponent();
            dgAgenda.RowBackground = null;
            carregaGrig();
        }

        //Botao de sair menuitem
        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        //Quando form carregado validar usuario(admin ou user) e mostrar items do statusBar
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            
            sbHora.Content = sbHora.Content + " " + DateTime.Now.ToShortTimeString();
            sbData.Content = sbData.Content + " " + DateTime.Now.ToLongDateString();
            sbUsuario.Content = sbUsuario.Content + " " + Login.usuarioLogado;


            BancoDados bd = new BancoDados();
            var sql = from u in bd.USUARIOS where u.nome_usuario == Login.usuarioLogado select u.tipo;
            if(sql.FirstOrDefault() == "admin"){
                return;
            }else{
                esconderBotoes();
            }
        }

        //Metodo para validar usuario deixando inacessivel os botoes para user
        public void esconderBotoes(){
            btnPagar.IsEnabled = false;
            btnReceber.IsEnabled = false;
            btnCaixa.IsEnabled = false;
            btnFuncionarios.IsEnabled = false;
            btnConfig.IsEnabled = false;
        }

        //Botao agenda
        private void btnAgenda_Click(object sender, RoutedEventArgs e) {
            Agenda janela = new Agenda(this);
            janela.Show(); 
        }


        //Popular o dataGrid
        public void carregaGrig() {
            List<AGENDA> listaAgenda = conexao.AGENDA.ToList();
            dgAgenda.ItemsSource = null;
            dgAgenda.ItemsSource = listaAgenda.OrderBy(user => user.hora_inicio);
        }


        // botao Barbeiros
        private void btnBarbeiros_Click(object sender, RoutedEventArgs e) {
            Barbeiros janela = new Barbeiros();
            janela.Show();
        }

        // botao Clientes
        private void btnClientes_Click(object sender, RoutedEventArgs e) {
            Clientes janela = new Clientes();
            janela.Show();
        }

        // botao config. usuarios
        private void btnConfig_Click(object sender, RoutedEventArgs e) {
            Configurações_de_Usuários janela = new Configurações_de_Usuários();
            janela.Show();           
        }

        // botao menuitem agenda
        private void MenuItem_Click_1(object sender, RoutedEventArgs e) {
            btnAgenda_Click(sender, e);
        }

        // botao menuitem usuarios
        private void MenuItem_Click_2(object sender, RoutedEventArgs e) {
            btnConfig_Click(sender, e);
        }

        // botao menuitem clientes
        private void MenuItem_Click_3(object sender, RoutedEventArgs e) {
            btnClientes_Click(sender, e);
        }

        // botao menuitem barbeiros
        private void MenuItem_Click_4(object sender, RoutedEventArgs e) {
            btnBarbeiros_Click(sender, e);
        }

        // botao contas pagar
        private void btnPagar_Click(object sender, RoutedEventArgs e) {
            ContasPagar janela = new ContasPagar();
            janela.Show();
        }

        // botao menuitem contas pagar
        private void MenuItem_Click_5(object sender, RoutedEventArgs e) {
            btnPagar_Click(sender, e);
        }
    }
}

