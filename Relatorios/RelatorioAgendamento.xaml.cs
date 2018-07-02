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
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Microsoft.Reporting.WinForms;

namespace BarberSystem.Relatorios {
    /// <summary>
    /// Lógica interna para RelatorioAgendamento.xaml
    /// </summary>
    public partial class RelatorioAgendamento : Window {

        BancoDados conexao = new BancoDados();
        public static string textBarbeiro;
        private bool ativo = false;
        public RelatorioAgendamento() {
            InitializeComponent();
            carregarCombo();
        }

        private void RelatorioAgendaJanela_Load(object sender, EventArgs e) {
            if (ativo) {
                sqlAgendamento(textBarbeiro);
            }
        }

        private void sqlAgendamento(string barbeiro){
            try {
                barbeiro = cbBarbeiros.Text;
                if (dpInicio.SelectedDate == null || dpFim.SelectedDate == null) {
                   var dadosRelatorio = (from a in conexao.AGENDA
                                          where a.nome_barbeiro == barbeiro
                                          select new { a.cliente, a.descricao, a.data, a.hora_inicio, a.hora_fim, a.nome_barbeiro }).ToList();

                    this.RelatorioAgendaJanela.LocalReport.DataSources.Clear();
                    this.RelatorioAgendaJanela.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("RelatorioAgendamentoDataSet", dadosRelatorio));
                    this.RelatorioAgendaJanela.LocalReport.ReportEmbeddedResource = "BarberSystem.Relatorios.RelatorioAgendaGUI.rdlc";
                }
                else{
                   var dadosRelatorio = (from a in conexao.AGENDA
                                          where a.nome_barbeiro == barbeiro && a.data >= dpInicio.SelectedDate && a.data <= dpFim.SelectedDate
                                          select new { a.cliente, a.descricao, a.data, a.hora_inicio, a.hora_fim, a.nome_barbeiro }).ToList();

                    this.RelatorioAgendaJanela.LocalReport.DataSources.Clear();
                    this.RelatorioAgendaJanela.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("RelatorioAgendamentoDataSet", dadosRelatorio));
                    this.RelatorioAgendaJanela.LocalReport.ReportEmbeddedResource = "BarberSystem.Relatorios.RelatorioAgendaGUI.rdlc";
                }

                this.RelatorioAgendaJanela.RefreshReport();                
            }
            catch (Exception) {

                throw;
            }
        }

        private void btnMostrar_Click(object sender, RoutedEventArgs e) {
            ativo = true;
            RelatorioAgendaJanela_Load(sender, e);
        }

        private void carregarCombo(){
           List<BARBEIROS> listaBarbeiro = conexao.BARBEIROS.ToList();
            cbBarbeiros.ItemsSource = null;
            cbBarbeiros.ItemsSource = listaBarbeiro;
            cbBarbeiros.DisplayMemberPath = "nome";
        }
    }
}
