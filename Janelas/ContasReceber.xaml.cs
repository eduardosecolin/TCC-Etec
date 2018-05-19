﻿using System;
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
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using BarberSystem.Controle;


namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para ContasReceber.xaml
    /// </summary>
    public partial class ContasReceber : Excel.Window {

        BancoDados conexao = new BancoDados();
        CONTAS_RECEBER cr = new CONTAS_RECEBER();
        private List<CONTAS_RECEBER> listaReceber = new List<CONTAS_RECEBER>();
      
        public ContasReceber() {
            InitializeComponent();
            dgReceber.RowBackground = null;
            carregaGrid();
        }

        // metodo para limpar os campos
        public void limpaCampos(){
            txtCodigo.Clear();
            txtDescricao.Clear();
            txtUnitario.Clear();
            txtPesquisar.Clear();
            dpPagto.Text = "";
            dpVencto.Text = "";
            lblTotal.Content = "0";
        }

        // verificar campos vazios
        public void verificaVazios() {
            if (txtDescricao.Text == "") {
                MessageBox.Show("O campo descrição não pode ser vazio!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
                return;
            }
            else {
                cr.descricao = txtDescricao.Text;
            }
            if (dpPagto.SelectedDate.ToString() == "") {
                cr.data_pagto = null;
            }
            else {
                cr.data_pagto = DateTime.Parse(dpPagto.SelectedDate.ToString());
            }
            if (dpVencto.SelectedDate.ToString() == "") {
                cr.data_vencto = null;
            }
            else {
                cr.data_vencto = DateTime.Parse(dpVencto.SelectedDate.ToString());
            }
            if(txtUnitario.Text == ""){
                cr.vl_unitario = null;
            }else{
                cr.vl_unitario = double.Parse(txtUnitario.Text);
            }
        }

        // metodo para carregar o dataGrid
        public void carregaGrid(){
            listaReceber = conexao.CONTAS_RECEBER.ToList();
            dgReceber.ItemsSource = null;
            dgReceber.ItemsSource = listaReceber.OrderBy(user => user.codigo);
        }

        // calcular valor total e mostrar na Label
        public void calculaValorTotal() {
            cr.vl_total = 0.0;
            foreach (CONTAS_RECEBER item in listaReceber) {
                cr.vl_total += item.vl_unitario;
            }
            lblTotal.Content = cr.vl_total.ToString();
        }

        // botao novo
        private void btnNovo_Click(object sender, RoutedEventArgs e) {
            txtDescricao.Focus();
            limpaCampos();
        }

        // botao alterar
        private void btnAlterar_Click(object sender, RoutedEventArgs e) {
            if (txtCodigo.Text != "") {
                verificaVazios();
                if(txtDescricao.Text == ""){
                    return;
                }
                cr.vl_total = cr.vl_unitario;
                double? temp = 0.0;
                foreach (CONTAS_RECEBER item in listaReceber) {
                    item.vl_total = temp;
                    item.vl_total += item.vl_unitario;
                    temp = item.vl_total;
                }
                MessageBox.Show("Dados alterados com sucesso!", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
                carregaGrid();
            }
            else {
                MessageBox.Show("Insira um código ou pesquise para alterar", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
                return;
            }
            btnGravar.IsEnabled = true;
        }

        // botao pesquisar
        private void btnPesquisar_Click(object sender, RoutedEventArgs e) {
            btnGravar.IsEnabled = false;
            try {
                if (txtPesquisar.Text != "") {
                    cr = conexao.CONTAS_RECEBER.Find(int.Parse(txtPesquisar.Text));
                    txtCodigo.Text = cr.codigo.ToString();
                    txtDescricao.Text = cr.descricao;
                    dpPagto.Text = cr.data_pagto.ToString();
                    dpVencto.Text = cr.data_vencto.ToString();
                    txtUnitario.Text = cr.vl_unitario.ToString();
                    lblTotal.Content = cr.vl_total.ToString();
                }
                else {
                    MessageBox.Show("Registro não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                }
            }
            catch (Exception a) {
                MessageBox.Show("Campo vazio ou código invalido!" + "\n" + a.Message, "Erro", MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                limpaCampos();
                Log.logException(a);
                Log.logMessage(a.Message);
                return;
            }
        }

        // botao excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e) {
         MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro ? ", "Excluir",
                                                         MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes) {
                cr = conexao.CONTAS_RECEBER.Remove(cr);
                cr.descricao = null;
                cr.data_pagto = null;
                cr.data_vencto = null;
                cr.vl_unitario = null;
                cr.vl_total = null;
                conexao.SaveChanges();
                MessageBox.Show("Registro excluido com sucesso!", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                carregaGrid();
                limpaCampos();
            }else{
                limpaCampos();
                return;
            }
            btnGravar.IsEnabled = true;
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {
                calculaValorTotal();
                verificaVazios();
                if (txtDescricao.Text == "") {
                    return;
                }
                cr.vl_total += cr.vl_unitario;

                conexao.CONTAS_RECEBER.Add(cr);
                conexao.SaveChanges();

                txtCodigo.Text = cr.codigo.ToString();

                MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
                carregaGrid();
                limpaCampos();
            }catch(Exception a){
                MessageBox.Show(a.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.logException(a);
                Log.logMessage(a.Message);
                return;
            }
        }

        // botao limpar
        private void btnLimpar_Click(object sender, RoutedEventArgs e) {
            limpaCampos();
        }

        // botao voltar
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        // botao calcular valor total
        private void btnCalcularValorTotal_Click(object sender, RoutedEventArgs e) {
            calculaValorTotal();
        }

        // mostrar a calculadora do windows
        private void btnCalcWindows_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("calc.exe");
        }

        // botao excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < dgReceber.Columns.Count; j++) {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = dgReceber.Columns[j].Header;
            }
            for (int i = 0; i < dgReceber.Columns.Count; i++) {
                for (int j = 0; j < dgReceber.Items.Count; j++) {
                    TextBlock b = dgReceber.Columns[i].GetCellContent(dgReceber.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }











        //----------------- REFERENCIAS PARA EXCEL ---------------------------------------------------------------------
        dynamic Excel.Window.Activate() {
            throw new NotImplementedException();
        }

        public dynamic ActivateNext() {
            throw new NotImplementedException();
        }

        public dynamic ActivatePrevious() {
            throw new NotImplementedException();
        }

        public bool Close(object SaveChanges, object Filename, object RouteWorkbook) {
            throw new NotImplementedException();
        }

        public dynamic LargeScroll(object Down, object Up, object ToRight, object ToLeft) {
            throw new NotImplementedException();
        }

        public Excel.Window NewWindow() {
            throw new NotImplementedException();
        }

        public dynamic _PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName) {
            throw new NotImplementedException();
        }

        public dynamic PrintPreview(object EnableChanges) {
            throw new NotImplementedException();
        }

        public dynamic ScrollWorkbookTabs(object Sheets, object Position) {
            throw new NotImplementedException();
        }

        public dynamic SmallScroll(object Down, object Up, object ToRight, object ToLeft) {
            throw new NotImplementedException();
        }

        public int PointsToScreenPixelsX(int Points) {
            throw new NotImplementedException();
        }

        public int PointsToScreenPixelsY(int Points) {
            throw new NotImplementedException();
        }

        public dynamic RangeFromPoint(int x, int y) {
            throw new NotImplementedException();
        }

        public void ScrollIntoView(int Left, int Top, int Width, int Height, object Start) {
            throw new NotImplementedException();
        }

        public dynamic PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName) {
            throw new NotImplementedException();
        }

        public Excel.Application Application => throw new NotImplementedException();

        public XlCreator Creator => throw new NotImplementedException();

        dynamic Excel.Window.Parent => throw new NotImplementedException();

        public Range ActiveCell => throw new NotImplementedException();

        public Chart ActiveChart => throw new NotImplementedException();

        public Pane ActivePane => throw new NotImplementedException();

        public dynamic ActiveSheet => throw new NotImplementedException();

        public dynamic Caption { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayFormulas { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayGridlines { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayHeadings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayHorizontalScrollBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayOutline { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool _DisplayRightToLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayVerticalScrollBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayWorkbookTabs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayZeros { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnableResize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezePanes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int GridlineColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public XlColorIndex GridlineColorIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Index => throw new NotImplementedException();

        public string OnWindow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Panes Panes => throw new NotImplementedException();

        public Range RangeSelection => throw new NotImplementedException();

        public int ScrollColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ScrollRow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Sheets SelectedSheets => throw new NotImplementedException();

        public dynamic Selection => throw new NotImplementedException();

        public bool Split { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SplitColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SplitHorizontal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SplitRow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SplitVertical { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double TabRatio { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public XlWindowType Type => throw new NotImplementedException();

        public double UsableHeight => throw new NotImplementedException();

        public double UsableWidth => throw new NotImplementedException();

        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Range VisibleRange => throw new NotImplementedException();

        public int WindowNumber => throw new NotImplementedException();

        XlWindowState Excel.Window.WindowState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public dynamic Zoom { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public XlWindowView View { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayRightToLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SheetViews SheetViews => throw new NotImplementedException();

        public dynamic ActiveSheetView => throw new NotImplementedException();

        public bool DisplayRuler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoFilterDateGrouping { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayWhitespace { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
