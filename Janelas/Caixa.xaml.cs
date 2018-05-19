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
using BarberSystem.Dados;
using System.Data.Entity.Migrations;
using System.Globalization;
using BarberSystem.Controle;

namespace BarberSystem.Janelas
{
    /// <summary>
    /// Lógica interna para Caixa.xaml
    /// </summary>
    public partial class Caixa : Window
    {

        BancoDados conexao = new BancoDados();
        CAIXA caixa = new CAIXA();
        List<CAIXA> listaCaixa = new List<CAIXA>();
       
        public Caixa()
        {
            InitializeComponent();
            mostrarValorTotal();
            mudaCorRetangulo();
        }

        // limpar os campos
        private void limpaCampos(){
            txtEntrada.Clear();
            txtSaida.Clear();
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {

                conexao.CAIXA.AddOrUpdate(caixa);
                conexao.SaveChanges();

                MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
            }catch(Exception a){
                MessageBox.Show(a.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.logException(a);
                Log.logMessage(a.Message);
                return;
            }
        }

        // botao entrada
        private void btnEntrada_Click(object sender, RoutedEventArgs e) {
            if (txtEntrada.Text == "") {
                MessageBox.Show("Entre com uma valor", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
                return;
            }
            int codigo = 1;
            caixa = conexao.CAIXA.Find(codigo);
            caixa.codigo = codigo;
            caixa.entrada = double.Parse(txtEntrada.Text);
            caixa.entradaCaixa(decimal.Parse(txtEntrada.Text));
            lblTotal.Content = caixa.vl_total.ToString();
            caixa.vl_total = decimal.Parse(lblTotal.Content.ToString());
            txtEntrada.Clear();
            mudaCorRetangulo();
        }

        // botao saida
        private void btnSaida_Click(object sender, RoutedEventArgs e) {
            if (txtSaida.Text == "") {
                MessageBox.Show("Entre com uma valor", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
                return;
            }
            int codigo = 1;
            caixa = conexao.CAIXA.Find(codigo);
            caixa.codigo = codigo;
            caixa.saida = double.Parse(txtSaida.Text);
            caixa.saidaCaixa(decimal.Parse(txtSaida.Text));
            lblTotal.Content = caixa.vl_total.ToString();
            caixa.vl_total = decimal.Parse(lblTotal.Content.ToString());
            txtSaida.Clear();
            mudaCorRetangulo();
        }

        // botao limpar
        private void btnLimpar_Click(object sender, RoutedEventArgs e) {
            limpaCampos();
        }

        // botao voltar
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        // botao calculadora windows
        private void btnCalcWindows_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("calc.exe");
        }

        // colorir retangulo
        public void mudaCorRetangulo(){
          if(double.Parse(lblTotal.Content.ToString()) < 0.0){
                retangulo.Fill = Brushes.Red;
          }else if(double.Parse(lblTotal.Content.ToString()) > 0.0){
                retangulo.Fill = Brushes.Green;
          }else{
                retangulo.Fill = Brushes.Blue;
          }
        }

        // mostrar valor total ao iniciar
        public void mostrarValorTotal(){
            listaCaixa = conexao.CAIXA.ToList();
            decimal? valor = 0;
            foreach (CAIXA item in listaCaixa) {
                valor = item.vl_total;
            }
            lblTotal.Content = valor.ToString();
        }
    }
    
}
