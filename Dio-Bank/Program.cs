using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Dio_Bank
{
    class Program
    {
        //lista com as contas cadastradas
        static List<Conta> listContas = new List<Conta>();

        static void Main(string[] args)
        {
            AppInfo(); //informações do app

            CarregarBanco();

            string opcaoUsuario = ObterOpcaoUsuario();

            while (opcaoUsuario.ToUpper() != "X")
            {
                switch (opcaoUsuario)
                {
                    case "1":
                        ListarContas();
                        break;
                    case "2":
                        InserirConta();
                        break;
                    case "3":
                        Transferir();
                        break;
                    case "4":
                        Sacar();
                        break;
                    case "5":
                        Depositar();
                        break;
                    case "C":
                        Console.Clear();
                        break;
                    default:
                        WriteLineCor(ConsoleColor.Red, "Digite uma opção válida");
                        break;
                }
                opcaoUsuario = ObterOpcaoUsuario();
            }

            WriteLineCor(ConsoleColor.Blue, "Obrigado por utilizar nossos serviços");
            Console.ReadLine();
        }

        //-------MÉTODOS------//

        // ----------------------------- LISTAR CONTAS --------------------------- //
        private static void ListarContas()
        {
            WriteLineCor(ConsoleColor.DarkGreen, "Listar contas");
            if (listContas.Count == 0)
            {
                WriteLineCor(ConsoleColor.Red, "Nenhuma conta cadastrada");
                return;
            }

            for (int i = 0; i < listContas.Count; i++)
            {
                Conta conta = listContas[i];
                WriteCor(ConsoleColor.Blue, $"# {i + 1} ");
                WriteLineCor(ConsoleColor.Blue, $"{conta}");
            }
        }

        // --------------------------- INSERIR CONTA -----------------------------//

        private static void InserirConta()
        {
            WriteCor(ConsoleColor.DarkGreen, "Inserir nova conta");

            //enquanto não entrar o tipo certo, continuará solicitando -- é bom botar uma opçao X para sair.

            int entradaTipoConta = SolicitaPessoa();
            while (entradaTipoConta == 0) { entradaTipoConta = SolicitaPessoa(); }
            string entradaNome = SolicitaNome();
            while (entradaNome == " ") { entradaNome = SolicitaNome(); }
            double entradaSaldo = SolicitaSaldo();
            while (entradaSaldo == -1) { entradaSaldo = SolicitaSaldo(); }
            double entradaCredito = SolicitaCredito();
            while (entradaCredito == -1) { entradaCredito = SolicitaCredito(); }


            static int SolicitaPessoa()
            {
                WriteCor(ConsoleColor.DarkGreen, "Digite 1 para Conta de Pessoa Física ou 2 para Conta de Pessoa Jurídica: ");
                int parseresultado;
                string tipoContaEntrada = Console.ReadLine();
                bool parsing = int.TryParse(tipoContaEntrada, out parseresultado);
                if (parsing)
                {
                    int validaTipoConta = parseresultado;
                    if (validaTipoConta != 1 && validaTipoConta != 2)
                    {
                        WriteCor(ConsoleColor.Red, "Tipo de Pessoa inválido");
                        return 0;
                    }
                    else
                    {
                        return validaTipoConta;
                    }
                }
                else
                {
                    WriteCor(ConsoleColor.Red, "Tipo de Pessoa inválido");
                    return 0;
                }
            }

             
            static string SolicitaNome() // 
            {
                WriteCor(ConsoleColor.DarkGreen, "Digite o nome do Cliente: ");
                string validaNome = Console.ReadLine();

                //Regex pra nome válido -- até 4 nomes, com pontuações letras e números (não pode ser só números ou pontos e caracteres especias)
                Regex reg = new Regex("^[A-Za-z]{1,20}[ ]{0,1}[A-Za-z.0-9]{0,20}[ ]{0,1}[A-Za-z.0-9]{0,20}[ ]{0,1}[A-Za-z.0-9]{0,20}[.!@$&]{0,1}$");

                if (reg.IsMatch(validaNome) == false)
                {
                    WriteCor(ConsoleColor.Red, "Tipo de Pessoa inválido");
                    return " ";
                }
                else
                {
                    return validaNome;
                }
            }

            static double SolicitaSaldo()
            {
                WriteCor(ConsoleColor.DarkGreen, "Digite o Saldo inicial: ");
                string entradaSaldo = Console.ReadLine();
                return ParseValores(entradaSaldo);
            }
            static double SolicitaCredito()
            {
                WriteCor(ConsoleColor.DarkGreen, "Digite o Crédito: ");
                string entradaCredito = Console.ReadLine();
                return ParseValores(entradaCredito);
            }


            Conta novaConta = new Conta
                (
                tipoConta: (TipoConta)entradaTipoConta,
                saldo: entradaSaldo,
                credito: entradaCredito,
                nome: entradaNome
                );

            listContas.Add(novaConta);
            Banco.novaConta(entradaTipoConta, entradaSaldo, entradaCredito, entradaNome);
            WriteLineCor(ConsoleColor.Blue, $"Conta criada com sucesso: {novaConta.ToString()}");

        }


        //---------------------------------SAQUE----------------------------------//

        private static void Sacar()
        {
            double indiceConta = SolicitaIndice();
            int totalContas = listContas.Count;

            while (indiceConta == 0 || indiceConta == -1)
            {
                indiceConta = SolicitaIndice();
                while (indiceConta > totalContas)
                {
                    WriteCor(ConsoleColor.Red, "Digite um número de conta válido ou X para sair");
                    indiceConta = SolicitaIndice();
                }
            }

            if (indiceConta == -9) // SAÍDA PELO "X"
            {
                return;
            }

            double valorSaque = SolicitaValor();
            while (valorSaque == -1) { valorSaque = SolicitaValor(); }


            EfetuaSaque(indiceConta, valorSaque);


            static double SolicitaIndice()
            {
                WriteCor(ConsoleColor.DarkGreen, "Digite o número da conta: ");
                string entradaIndice = Console.ReadLine();

                return ParseValores(entradaIndice);
            }

            static double SolicitaValor()
            {
                WriteCor(ConsoleColor.DarkGreen, "Digite o valor a ser sacado: ");
                string entradaValor = Console.ReadLine();
                return ParseValores(entradaValor);
            }

            static void EfetuaSaque(double conta, double valor)
            {
                int numConta = (int)conta - 1;
                listContas[numConta].Sacar(valor);
            }
        }

        //--------------------------------------DEPOSITAR-----------------------------------//
        private static void Depositar()
        {
            double indiceConta = SolicitaIndice();
            int totalContas = listContas.Count;

            while (indiceConta == 0 || indiceConta == -1)
            {
                indiceConta = SolicitaIndice();
                while (indiceConta > totalContas)
                {
                    WriteCor(ConsoleColor.Red, "Digite um número de conta válido ou X para sair");
                    indiceConta = SolicitaIndice();
                }
            }

            if (indiceConta == -9) // SAÍDA PELO "X"
            {
                return;
            }

            double valorDepo = SolicitaValor();
            while (valorDepo == -1) { valorDepo = SolicitaValor(); }

            EfetuarDeposito(indiceConta, valorDepo);



            static double SolicitaIndice()
            {
                WriteCor(ConsoleColor.DarkGreen, "Digite o número da conta: ");
                string entradaIndice = Console.ReadLine();

                return ParseValores(entradaIndice);
            }

            static double SolicitaValor()
            {
                WriteCor(ConsoleColor.DarkGreen, "Digite o valor a ser Depositado: ");
                string entradaDepo = Console.ReadLine();
                return ParseValores(entradaDepo);
            }

            static void EfetuarDeposito(double conta, double valor)
            {
                int numConta = (int)conta - 1;
                listContas[numConta].Depositar(valor);
            }

        }


        //----------------------------------------TRANSFERIR-------------------------------------//
        private static void Transferir()
        {

            double indiceContaOrigem = SolicitaIndice("origem");
            int totalContas = listContas.Count;

            while (indiceContaOrigem == 0 || indiceContaOrigem == -1)
            {
                indiceContaOrigem = SolicitaIndice("origem");
                while (indiceContaOrigem > totalContas)
                {
                    WriteCor(ConsoleColor.Red, "Digite um número de conta válido ou X para sair");
                    indiceContaOrigem = SolicitaIndice("origem");
                }
            }

            double indiceContaDestino = SolicitaIndice("destino");

            while (indiceContaDestino == 0 || indiceContaDestino == -1)
            {
                indiceContaDestino = SolicitaIndice("destino");
                while (indiceContaDestino > totalContas)
                {
                    WriteCor(ConsoleColor.Red, "Digite um número de conta válido ou X para sair");
                    indiceContaDestino = SolicitaIndice("destino");
                }
            }

            if (indiceContaOrigem == -9 || indiceContaDestino == -9) // SAÍDA PELO "X"
            {
                return;
            }

            double valorTransf = SolicitaValor();
            while (valorTransf == -1) { valorTransf = SolicitaValor(); }

            EfetuarTransf(indiceContaOrigem, indiceContaDestino, valorTransf);


            static double SolicitaIndice(string conta)
            {
                WriteCor(ConsoleColor.DarkGreen, $"Digite o número da conta de {conta}: ");
                string entradaIndice = Console.ReadLine();

                return ParseValores(entradaIndice);
            }

            static double SolicitaValor()
            {
                WriteCor(ConsoleColor.DarkGreen, "Digite o valor a ser transferido: ");
                string entradaDepo = Console.ReadLine();
                return ParseValores(entradaDepo);
            }

            static void EfetuarTransf(double contaO, double contaD, double valor)
            {
                int numContaO = (int)contaO - 1;
                int numContaD = (int)contaD - 1;
                listContas[numContaO].Transferir(valor, listContas[numContaD]);
            }

        }

        // -----------------------------------------FUNÇÃO INICIAL ------------------------------------//
        private static string ObterOpcaoUsuario()
        {
            Console.WriteLine();
            WriteLineCor(ConsoleColor.DarkGreen, "DIO Bank a seu dispor!");
            WriteLineCor(ConsoleColor.DarkGreen, "Informe a opção desejada:");

            WriteLineCor(ConsoleColor.DarkGreen, "1 - Listar contas");
            WriteLineCor(ConsoleColor.DarkGreen, "2 - Inserir nova conta");
            WriteLineCor(ConsoleColor.DarkGreen, "3 - Transferir");
            WriteLineCor(ConsoleColor.DarkGreen, "4 - Sacar");
            WriteLineCor(ConsoleColor.DarkGreen, "5 - Depositar");
            WriteLineCor(ConsoleColor.DarkGreen, "C - Limpar Tela");
            WriteLineCor(ConsoleColor.DarkGreen, "X - Sair");
            Console.WriteLine("");


            string opcaoUsuario = Console.ReadLine().ToUpper();
            Console.WriteLine();
            return opcaoUsuario;
        }

        //------------------------- FUNÇÃO PARA TRATAR DE INPUTS ERRADOS -----------------------------//

        static double ParseValores(string entradaParse)
        {
            if (entradaParse.ToUpper() == "X")
            {
                return -9;
            }
            double parseresultado;
            bool parsing = double.TryParse(entradaParse, out parseresultado);
            if (parsing)
            {
                double valida = parseresultado;
                if (valida < 0)
                {
                    WriteCor(ConsoleColor.Red, "Digite um número válido ou X para sair");
                    return -1;
                }
                else
                {
                    return valida;
                }
            }
            else
            {
                WriteCor(ConsoleColor.Red, "Digite um número válido ou X para sair");
                return -1;
            }
        }

        // ---------------------------------FUNÇÕES PRA COLORIR O TEXTO -----------------------------------//
        public static void WriteLineCor(ConsoleColor cor, string mensagem) //colore o texto do WriteLine
        {
            //muda cor do texto
            Console.ForegroundColor = cor;
            Console.WriteLine(mensagem);
            //reseta cor do texto
            Console.ResetColor();
            //pergunta nome do usuário
        }

        public static void WriteCor(ConsoleColor cor, string mensagem) //colore o texto do Write
        {
            //muda cor do texto
            Console.ForegroundColor = cor;
            Console.WriteLine(mensagem);
            //reseta cor do texto
            Console.ResetColor();
            //pergunta nome do usuário
        }

        //----------------------------------------FUNÇÃO INFORMAÇÕES DO APP-------------------------------//
        static void AppInfo()
        {
            string NomeApp = "DIO.BANK";
            string versao = "1.0.0";
            string autor = "Romulus";

            // Muda cor do texto
            Console.ForegroundColor = ConsoleColor.Blue;
            // informações sobre o programa
            Console.WriteLine("{0}: Versão {1} por {2}", NomeApp, versao, autor);
            Console.WriteLine();
            //reseta cor do texto
            Console.ResetColor();

        }

        //---------------------------------------CARREGAR BANCO DE DADOS SQLITE -------------------------//
        static void CarregarBanco()
        {
            DataTable data = Banco.CarregarContas();
            CarregarLista(data);
        }

        public static void CarregarLista(DataTable data)
        {
            string TConta;
            string valor = "";
            string credito = "";
            string nome = "";

            foreach (DataRow row in data.Rows)//acessa todos os itens e sub itens da data table
            {
                TConta = row[0].ToString();

                for (int i = 1; i < data.Columns.Count; i++)
                {
                    valor = row[1].ToString();
                    credito = row[2].ToString();
                    nome = row[3].ToString();
                    
                }
                Conta conta = new Conta(tipoConta: (TipoConta)int.Parse(TConta), double.Parse(valor), double.Parse(credito), nome);//a cada iteração um objeto é criado
                listContas.Add(conta);
            }

        }

    }

}
