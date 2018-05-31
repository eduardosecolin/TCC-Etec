using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarberSystem.Dados;
using BarberSystem.Controle;
using System.Data.SqlClient;
using System.Data;

namespace BarberSystem.Controle {
    public class SqlServer {

        // criar banco de dados se nao existir
        public string ConnectionString { get; protected set; }
        public SqlServer(string connectionString) {
            ConnectionString = connectionString;
        }

        public static void createIfNotExists(string connectionString) {
            new SqlServer(connectionString).createIfNotExists();
        }

        public void createIfNotExists() {
            try {
                var connectionStringBuilder = new SqlConnectionStringBuilder(ConnectionString);
                var databaseName = connectionStringBuilder.InitialCatalog;
                connectionStringBuilder.InitialCatalog = "master";
                using (var connection = new SqlConnection(connectionStringBuilder.ToString())) {
                    connection.Open();
                    using (var cmd = connection.CreateCommand()) {
                        cmd.CommandText = string.Format("select * from master.dbo.sysdatabases where name='{0}'", databaseName);
                        using (var reader = cmd.ExecuteReader()) {
                            if (reader.HasRows) {
                                return;
                            }
                            reader.Close();
                            cmd.CommandText = string.Format("CREATE DATABASE {0}", databaseName);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e) {
                Log.logException(e);
                Log.logMessage(e.Message);
            }

        }

        // verificar se existe as tabelas
        public static bool existeTabela(string nomeTabela) {
            BancoDados dados = new BancoDados();
            try {
                string sql = string.Format("select * from " + nomeTabela);
                dados.Database.ExecuteSqlCommand(sql);
                return false;
            }
            catch (Exception e) {
                return true;
            }
        }

        // inserir usuario e senha padrao
        public static void acesso() {
            BancoDados conexao = new BancoDados();
            string usuario = "admin";
            string senha = "eKkqAeJvo3t60TtipgWMjg==";
            string end = "sem endereço";
            string bairro = "sem bairro";
            string cidade = "mogi mirim";
            string estado = "SP";
            string cpf = "33322211100";
            string email = "email@email.com";
            string tipo = "admin";
            string sql = string.Format("INSERT INTO [dbo].[USUARIOS]([nome_usuario],[senha],[endereco],[bairro],[cidade],[estado],[cpf],[email],[tipo])" +
                                       "VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", usuario, senha, end, bairro, cidade, estado, cpf, email, tipo);

            conexao.Database.ExecuteSqlCommand(sql);
        }

        // verificar se a tabela esta fazia
        public static bool existeDados() {
            string connection = "Data Source="+getServer()+"\\"+getInstance()+";Initial Catalog=BARBER_DATABASE;Integrated Security=True";
            SqlConnection con = new SqlConnection(connection);
            string sql = string.Format("select * from dbo.USUARIOS");
            SqlCommand cmd = new SqlCommand(sql, con);
            DataTable dados = new DataTable();
            con.Open();
            dados.Load(cmd.ExecuteReader());
            if (dados.Rows.Count > 0) {
                con.Close();
                return true;
            }
            else {
                con.Close();
                return false;
            }
        }

        // pegar o nome do servidor sql server
        public static string getServer(){
            string servidor = string.Empty;
            System.Data.Sql.SqlDataSourceEnumerator instancia = System.Data.Sql.SqlDataSourceEnumerator.Instance;
            System.Data.DataTable dados = instancia.GetDataSources();
            foreach (DataRow item in dados.Rows) {
                servidor = item["ServerName"].ToString();
            }
            return servidor;
        }

        // pegar o nome do instancia sql server
        public static string getInstance() {
            string instancia = string.Empty;
            System.Data.Sql.SqlDataSourceEnumerator instance = System.Data.Sql.SqlDataSourceEnumerator.Instance;
            System.Data.DataTable dados = instance.GetDataSources();
            foreach (DataRow item in dados.Rows) {
                instancia = item["InstanceName"].ToString();
            }
            return instancia;
        }
    }
}
