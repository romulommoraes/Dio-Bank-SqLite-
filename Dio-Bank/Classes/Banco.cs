using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Dio_Bank
{
    class Banco
    {
        private static SQLiteConnection conexao; //cria a variavel pra conexao com o SQlite 

        private static SQLiteConnection ConexaoBd()//método pra criar a conexão com o banco e retorna a variável conexao
        {
            string filepathRaiz = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            conexao = new SQLiteConnection(@$"Data Source={filepathRaiz}\Database\bd.db");
            conexao.Open();
            return conexao;
        }

        public static DataTable CarregarContas()
        {
            SQLiteDataAdapter da = null; //data adapter que vai receber os dados do Banco
            DataTable dt = new DataTable();// data table que vai servir pra transferir os dados do banco 
            try
            {
                var vcon = ConexaoBd();//variavel com o caminho pra conexao com o bd
                var cmd = vcon.CreateCommand();//variavel com os comandos a serem realizados
                cmd.CommandText = "SELECT * FROM Contas";//comando especifico de cada função
                da = new SQLiteDataAdapter(cmd.CommandText, vcon);//carrega o data adapter com os dados do banco
                da.Fill(dt); //transfere os dados pra um datatable
                vcon.Close();//fecha a conexão

                return dt;
            }
            catch 
            {
                throw;
            }

        }

        public static void novaConta(int t, double v, double c, string n)

        {
            var vcon = ConexaoBd();
            var cmd = vcon.CreateCommand();
            try //adiciona os itens nas caixas de texto ao banco de dados
            {
                cmd.CommandText = "INSERT INTO Contas (TipoConta, Saldo, Credito, Nome) VALUES (@tipoconta, @valor, @credito, @nome)";
                cmd.Parameters.AddWithValue("@tipoconta", t);
                cmd.Parameters.AddWithValue("@valor", v);
                cmd.Parameters.AddWithValue("@credito", c);
                cmd.Parameters.AddWithValue("@nome", n);

                 cmd.ExecuteNonQuery();
                vcon.Close();
            }
            catch 
            {
               vcon.Close();
            }
        }


    }
        

}
