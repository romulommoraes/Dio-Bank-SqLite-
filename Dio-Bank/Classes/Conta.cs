using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dio_Bank
{
    class Conta
    {
        //atributos devem ficar como privados por questão de encapsulamento. se tiver q mudar, deve-se fazer por meio de um método.
        private TipoConta TipoConta { get; set; }
        private double Saldo { get; set; }
        private double Credito { get; set; }
        private string Nome { get; set; }

        //construtor
        public Conta(TipoConta tipoConta, double saldo, double credito, string nome)
        {
            this.TipoConta = tipoConta;
            this.Saldo = saldo;
            this.Credito = credito;
            this.Nome = nome;
        }

        //----métodos----//

        public bool Sacar(double valorSaque) //SAQUE
        {
            if (this.Saldo - valorSaque < (this.Credito *-1))
            {
                Program.WriteLineCor(ConsoleColor.Red, "Saldo insuficiente");
                return false;
            }

            this.Saldo -= valorSaque;
            Program.WriteLineCor(ConsoleColor.Blue, $"Saldo atual da conta de {this.Nome} é de {this.Saldo}");
            return true;
        }



        public void Depositar (double valorDeposito) //DEPÓSITO
        {
            this.Saldo += valorDeposito;
            Program.WriteLineCor(ConsoleColor.Blue, $"Saldo atual da conta de {this.Nome} é de {this.Saldo}");
        }

        public void Transferir (double valorTransferencia, Conta contaDestino) //Transferencia
        {
            if (this.Sacar(valorTransferencia))//se o saque retornar true (tem dinheiro na conta) ele faz a transferência
            {
                contaDestino.Depositar(valorTransferencia);
            }

        }

        public override string ToString()
        {
            string retorno = $"Tipo de Conta: {this.TipoConta}, Nome: {this.Nome}, Saldo: {this.Saldo}, Crédito: {this.Credito}";
            return retorno;
        }

    }
}
