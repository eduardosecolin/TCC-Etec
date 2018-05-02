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
using System.Globalization;
using BarberSystem.Utils;
using BarberSystem.Dados;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Data.Entity.Migrations;

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Agenda.xaml
    /// </summary>
    public partial class Agenda : Microsoft.Office.Interop.Excel.Window {

        AGENDA agendamento = new AGENDA();
        BancoDados conexao = new BancoDados();
        public List<AGENDA> listaAgenda = new List<AGENDA>();
        private Menu janela;

        //Construtor
        public Agenda(Menu window) {
            janela = window;
            InitializeComponent();
            dgAgendamento.RowBackground = null;
            carrgearGrid();
            carregarComboBox();
            carregaComboCodClient();
        }

        // metodo para campos vazios
        public void verificaCampos(){
          if(cbCodCliente.Equals(string.Empty)){
                agendamento.codcliente = null;
          }else{
                string codigo = cbCodCliente.Text.Substring(0, 1);
                agendamento.codcliente = int.Parse(codigo);
          }
        }

        //Botao de voltar
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            var sql = from a in conexao.AGENDA 
                      where a.data == DateTime.Today 
                      select new { a.cliente, a.descricao, a.hora_inicio, a.hora_fim, a.data, a.nome_barbeiro };
            janela.dgAgenda.ItemsSource = sql.ToList().OrderBy(user => user.hora_inicio);
            this.Close();
        }

        //Botao de Novo
        private void btnCadastrar_Click(object sender, RoutedEventArgs e) {
            cbCodCliente.Focus();
            limpaCampos();
        }

        //Metodo para limpar os campos(textBox)
        public void limpaCampos(){
            txtCodigo.Clear();
            cbCodCliente.Text = string.Empty;
            txtCliente.Clear();
            txtDescricao.Clear();
            MtxtHinicio.Clear();
            MtxtHfim.Clear();
            txtCodBarbeiro.Clear();
            txtPesquisar.Clear();
            MtxtHinicio.Clear();
            dpData.Text = "";
            cbBarbeiro.Text = "";
            txtCodCliente.Clear();
        }

        //Botao limpar
        private void btnLimpar_Click(object sender, RoutedEventArgs e) {
            limpaCampos();
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {
                verificaCampos();
                agendamento.cliente = Util.VerificarCamposVazios(txtCliente.Text);
                agendamento.descricao = Util.VerificarCamposVazios(txtDescricao.Text);
                agendamento.hora_inicio = DateTime.Parse(MtxtHinicio.Text);
                agendamento.hora_fim = DateTime.Parse(MtxtHfim.Text);
                agendamento.data = DateTime.Parse(dpData.SelectedDate.ToString());
                agendamento.codbarbeiro = int.Parse(txtCodBarbeiro.Text);
                agendamento.nome_barbeiro = Util.VerificarCamposVazios(cbBarbeiro.Text);

                if (Util.vazio == true) {
                    return;
                }

                conexao.AGENDA.Add(agendamento);
                conexao.SaveChanges();

                txtCodigo.Text = agendamento.codigo.ToString();
                carrgearGrid();

                MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
            }catch(Exception a){
                MessageBox.Show(a.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        // carregar a grid
        public void carrgearGrid(){
                listaAgenda = conexao.AGENDA.ToList();
                dgAgendamento.ItemsSource = null;
                dgAgendamento.ItemsSource = listaAgenda.OrderBy(user => user.hora_inicio);
        }

        // pesquisar
        private void BtnPesquisar_Click(object sender, RoutedEventArgs e) {
            btnGravar.IsEnabled = false;           
            try {
                if (txtPesquisar.Text != "") {
                    agendamento = conexao.AGENDA.Find(int.Parse(txtPesquisar.Text));
                    cbCodCliente.Text = agendamento.codcliente.ToString();
                    txtCodCliente.Text = agendamento.codcliente.ToString();
                    txtCodigo.Text = agendamento.codigo.ToString();
                    txtCliente.Text = agendamento.cliente;
                    txtDescricao.Text = agendamento.descricao;
                    MtxtHinicio.Text = DateTime.Parse(agendamento.hora_inicio.ToString()).ToShortTimeString();
                    MtxtHfim.Text = DateTime.Parse(agendamento.hora_fim.ToString()).ToShortTimeString();
                    dpData.Text = agendamento.data.ToString();
                    txtCodBarbeiro.Text = agendamento.codbarbeiro.ToString();
                    cbBarbeiro.Text = agendamento.nome_barbeiro;
                }
                else {
                    MessageBox.Show("Agendamento não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                }
            }catch(Exception a){
                MessageBox.Show("Campo vazio ou código invalido!" + "\n" + a.Message, "Erro", MessageBoxButton.OK, 
                                 MessageBoxImage.Exclamation);
                limpaCampos();
                return;
            }
        }

        // excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes) {
                agendamento = conexao.AGENDA.Remove(agendamento);
                limpaCampos();
                agendamento.cliente = null;
                agendamento.descricao = null;
                agendamento.hora_inicio = null;
                agendamento.hora_fim = null;
                agendamento.data = null;
                agendamento.nome_barbeiro = null;
                conexao.SaveChanges();
                MessageBox.Show("Registro excluido com sucesso!", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                carrgearGrid();
                limpaCampos();
            }else{
                limpaCampos();
                return;
            }
            btnGravar.IsEnabled = true;
        }

        // exportar para o excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true; 
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < dgAgendamento.Columns.Count; j++) 
            {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true; 
                sheet1.Columns[j + 1].ColumnWidth = 15; 
                myRange.Value2 = dgAgendamento.Columns[j].Header;
            }
            for (int i = 0; i < dgAgendamento.Columns.Count; i++) { 
                for (int j = 0; j < dgAgendamento.Items.Count; j++) {
                    TextBlock b = dgAgendamento.Columns[i].GetCellContent(dgAgendamento.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }

        public void carregarComboBox(){
           List<BARBEIROS> listaBarbeiros = conexao.BARBEIROS.ToList();
            cbBarbeiro.ItemsSource = null;
            cbBarbeiro.ItemsSource = listaBarbeiros.OrderBy(user => user.nome);
            cbBarbeiro.DisplayMemberPath = "nome";
        }

        // carregar comboBox com o codigo do cliente
        public void carregaComboCodClient(){
            var sql = from a in conexao.CLIENTES
                      where a.codigo > 0
                      select a.codigo + " - " + a.nome;

            cbCodCliente.ItemsSource = null;
            cbCodCliente.ItemsSource = sql.ToList();
        }

        //mostrar barbeiro automatico
        private void txtCodBarbeiro_LostFocus(object sender, RoutedEventArgs e) {
            BARBEIROS barber = new BARBEIROS();
            try{
              if(txtCodBarbeiro.Text != ""){
                    barber = conexao.BARBEIROS.Find(int.Parse(txtCodBarbeiro.Text));
                    cbBarbeiro.Text = barber.nome.ToString();
              }
            }catch(Exception){
                MessageBox.Show("Código do barbeiro invalido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCodBarbeiro.Clear();
                cbBarbeiro.Text = "";
                txtCodBarbeiro.Focus();
            }

        }

        // mostrar cliente automatico
        private void txtCliente_GotFocus(object sender, RoutedEventArgs e) {
            try {
              if(cbCodCliente.SelectedItem != null){
                    string codigo = cbCodCliente.Text.Substring(0, 1);
                    CLIENTES cliente = new CLIENTES();
                    cliente = conexao.CLIENTES.Find(int.Parse(codigo));
                    txtCliente.Text = cliente.nome;

              }
            }
            catch (Exception) {
                MessageBox.Show("Código do cliente invalido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                cbCodCliente.Text = "";
                txtCliente.Clear();
                cbCodCliente.Focus();
            }
        }

        //botao alterar
        private void btnAlterar_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtCodigo.Text != "") {
                    agendamento.codcliente = int.Parse(txtCodCliente.Text);
                    agendamento.cliente = txtCliente.Text;
                    agendamento.descricao = txtDescricao.Text;
                    agendamento.hora_inicio = DateTime.Parse(MtxtHinicio.Text);
                    agendamento.hora_fim = DateTime.Parse(MtxtHfim.Text);
                    agendamento.data = DateTime.Parse(dpData.SelectedDate.ToString());
                    agendamento.codbarbeiro = int.Parse(txtCodBarbeiro.Text);
                    agendamento.nome_barbeiro = cbBarbeiro.Text;
                    conexao.AGENDA.AddOrUpdate(agendamento);
                    conexao.SaveChanges();
                    MessageBox.Show("Dados alterados com sucesso!", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    carrgearGrid();
                }
                else {
                    MessageBox.Show("Insira um código ou pesquise para alterar", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    return;
                }
            }catch(Exception a){
                MessageBox.Show("Alguns campos não podem ficar vazios" + "\n" + a.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                limpaCampos();
                return;
            }
            btnGravar.IsEnabled = true;
        }

        // mostrar registros da grid pela data selecionada
        private void dpCurrent_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
            var sql = from a in conexao.AGENDA
                      where a.data == dpCurrent.SelectedDate
                      select new {a.codigo, a.codcliente, a.cliente, a.descricao, a.hora_inicio, a.hora_fim, a.data, a.codbarbeiro, a.nome_barbeiro };
            dgAgendamento.ItemsSource = null;
            dgAgendamento.ItemsSource = sql.ToList().OrderBy(user => user.hora_inicio);
        }

        private void txtCodCliente_GotFocus(object sender, RoutedEventArgs e) {
            try {
                if (cbCodCliente.SelectedItem != null) {
                    string codigo = cbCodCliente.Text.Substring(0, 1);
                    CLIENTES cliente = new CLIENTES();
                    cliente = conexao.CLIENTES.Find(int.Parse(codigo));
                    txtCodCliente.Text = cliente.codigo.ToString();

                }
            }
            catch (Exception) {
                MessageBox.Show("Código do cliente invalido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                cbCodCliente.Text = "";
                txtCliente.Clear();
                cbCodCliente.Focus();
            }
        }













        //---------------- REFERENCIAS PARA EXCEL -------------------------------------------------------------------------------
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
