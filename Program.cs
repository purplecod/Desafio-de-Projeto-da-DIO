// Declaração de variaveis de preços e apelidando nameespace.
using System;
using Cdp = ClassesDoPrograma;
double preçoInicial, preçoPorHora;
int opção;

// fase inicial do programa, definições de preços.

Console.WriteLine("Seja bem vindo ao sistema de estacionamento!");

// Definindo o preço por estacionar.
while (true)
{
    try 
    {
        Console.Write("Digite o preço inicial: ");
        preçoInicial = Convert.ToDouble(Console.ReadLine());
        break;
    }
    catch (FormatException)
    {
        Console.WriteLine("ERRO: Por favor digite o preço inicial do estacionamento.");
        Thread.Sleep(1500);
        Console.Clear();
    }

}
// Definindo o preço por hora.
while (true)
{
    try
    {
        Console.Write("Digite o preço por hora: ");
        preçoPorHora = Convert.ToDouble(Console.ReadLine());
        break;
    }
    catch (FormatException)
    {
        Console.WriteLine("ERRO: Por favor digite o preço inicial do estacionamento.");
        Thread.Sleep(1500);
        Console.Clear();
    }
}


Cdp.Estacionamento Estacionamento = new(preçoInicial, preçoPorHora);// Preços ficam amazernados aqui.

while (true)
{//Segunda fase do programa, genrenciamento do estacionamento.
 //Menu principal.
    while (true)
    {
        try
        {
            Console.Clear();
            opção = Cdp.Visual.Menu("Estacionamento", "Digite o numero da opção desejada", "Cadastrar Veiculos", "Listar Veiculos", "Remover Veiculos", "Encerrar");
            break;
        }
        catch (FormatException)
        {
            Console.WriteLine("ERRO: Por favor, digite o numero da opção desejada.");
            Thread.Sleep(1500);
        }
    }
    Console.Clear();
    switch (opção)
    {
        case 1:
            // Se o menu principal retornar 1, este case será executado, onde acontece acontece a adição de veiculos.

            Cdp.Visual.Titulo("Cadastrar Veiculos");
            Console.Write("Digite a placa reverente ao veiculo: ");
            string? veiculoDigitado = Console.ReadLine();
            Estacionamento.AdicionarVeiculo(veiculoDigitado);

            break;

        case 2:
            // Se o menu principal retornar 2, este case será executado, onde acontece a listagem das placas dos veiculos.

            int i = 0;
            Cdp.Visual.Titulo("Veiculos Estacionados");
            foreach (string? placa in Estacionamento.ListaDePlacasDosVeiculosEstacionados())
            {
                i++;
                Console.WriteLine($"{i} - {placa}");
            }
            Cdp.Visual.Linha();

            break;

        case 3:
            //Se o menu principal retornar 3, este case será executado, onde acontecerá a cobrança e a remoção dos veiculos do estacionamento.
            byte horasQueOVeiculoFicouEstacionado;
            Console.Write("Digite a placa do veiculo para remove-lo: ");
            string? veiculoASerVerificadoERemovido = Console.ReadLine();
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.Write("Digite a quantidade de horas que o veiculo permaneceu no estacionamento: ");
                    horasQueOVeiculoFicouEstacionado = Convert.ToByte(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("ERRO: digite o numero de horas que o veiculo ficou estacionado.");
                    Thread.Sleep(1500);
                }
                catch (OverflowException)
                {
                    Console.WriteLine("ERRO: Digite um valor valido.");
                    Thread.Sleep(1500);
                }
            }
            // Verificação acontece aqui.
            bool veiculoEstaNoEstacionamento/*?*/ = Estacionamento.VerificaçãoEExclusãoDoVeiculo(veiculoASerVerificadoERemovido);


            if (veiculoEstaNoEstacionamento)

            // Se o Veiculo expecificado foi encontrado nos registros, a remoção e a cobrança vai acontecer

            {
                double preçoACobrar = Estacionamento.RealizarCobrança(horasQueOVeiculoFicouEstacionado);
                Console.WriteLine($"Remoção do veiculo realizada com sucesso! Preço a cobrar: {preçoACobrar:c}");
            }
            else

            // Senão a mensagem a baixo vai ser exibida.

            {
                Console.WriteLine("Veiculo não encontrado.");
            }

            break;

        case 4:
            // Se menu principal retornar 4, o programa será encerrado.

            return 0;
                

        default:
            // Se o usuario digitar uma opção que o menu principal não tem, a mensagem abaixo vai ser exibida.

            Console.WriteLine("Opção indisponivel");

            break;
    }
        
    
    Console.WriteLine("Aperte enter para continuar.");
    Console.ReadLine();
}


namespace ClassesDoPrograma
{
    public class Estacionamento
    {
        /// <summary>
        /// Classe que vai gerenciar o estacionamento.
        /// </summary>
        public List<string?> ListaDasPlacasDosVeiculos = new(); //Lista de placas refenrente aos veiculos estacionados.
        public double PreçoBase; //Preço Minimo - Preço cobrado por usar o estacionamento.
        public double PreçoHoras;//Preço Extra - Preço cobrado pela quantidade de horas por usar o estacionamento.


        public Estacionamento(double preçoBase, double preçoHoras)// Método construtor.

        // Preços digitados pelo usuario.
        {
            this.PreçoBase = preçoBase;
            this.PreçoHoras = preçoHoras;
        }

        public bool VerificaçãoEExclusãoDoVeiculo(string? placaDoVeiculo)

        ///<summary>
        ///<param name="placaDoVeiculo">O método vai verificar se a propriedade ListaDasPlacasDosVeiculos contém registro especificado no parametro.</param>>
        ///Retorna "true" se o valor do parametro for encontrado na propriendade ListaDasPlacasDosVeiculos, senão retornará "false".
        ///</summary>>
     
        {
            foreach (var placa in this.ListaDasPlacasDosVeiculos)// Aqui acontece a verificação.
            {
                if (placa == placaDoVeiculo)
                {
                    this.ListaDasPlacasDosVeiculos.Remove(placa);
                    return true;
                }
            }
            return false;
        }

        public double RealizarCobrança(short horasNoEstacionamento)

        ///<summary>
        ///<param name="horasNoEstacionamento">Digite a quantidade de horas que o veiculo ficou estacionado - Entrada que vai ser usada para o calculo do preço total</param>>
        /// O método irá retornar o preço que o cliente deverá pagar.
        ///</summary>
        
        {
            double preçoTotal = PreçoHoras * horasNoEstacionamento;
            preçoTotal += PreçoBase;
            return preçoTotal;
        }

        public void AdicionarVeiculo(string? placaDoVeiculo)

        ///<summary>Esse método adiciona placa do veiculo na lista de placas referente aos veiculos do estacionamento. Não tem retorno</summary>>
        ///
        {
            ListaDasPlacasDosVeiculos.Add(placaDoVeiculo);
        }

        public List<string?> ListaDePlacasDosVeiculosEstacionados()
        ///<summary>Retorna a propreidade "ListaDasPlacasDosVeiculos". </summary>>
        {
            return ListaDasPlacasDosVeiculos;
        }
    }

    static class Visual
    {
        ///<summary>
        ///Classe responsável pela estética do programa.
        ///</summary>>

        public static void Linha()
        {
            ///<summary>Faz uma linha de correntes (=-=-=-=-=)</summary>>

            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
        }
        public static void Titulo(string titulo)
        {
            ///<summary>
            ///Esse método destaca uma string separando-a por correntes (=-=-=-=-=).
            ///</summary>>
            Console.WriteLine($"=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n\t{titulo}\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
        }
        public static byte Menu(string titulo, string mensagemDeEscolha,params string[] opçõesDoMenu)
        ///<summary>
        ///<param name="titulo">Titulo, para ficar claro do que o menu se trata.</param>>
        ///<param name="opçõesDoMenu">Um array que contém as opções que o menu terá</param>>
        ///<param name="mensagemDeEscolha">Mensagem auxiliando o usuario como deve ser feita a escolha das opções</param>>
        ///Exemplo:
        ///     =-=-=-=-=-=-=-=-=-=-=-=-=
        ///               titulo
        ///     =-=-=-=-=-=-=-=-=-=-=-=-=
        ///     1 - opçõesDoMenu
        ///     2 - opçõesDoMenu
        ///     3 - opçõesDoMenu
        ///     =-=-=-=-=-=-=-=-=-=-=-=-=
        ///     mensagemDeEscolha:
        ///     
        ///Se o valor for muito grande, o método retornará 0.
        ///</summary>>
        {
            byte i = 0;
            byte escolha;
            Titulo(titulo);
            foreach (string? opções in opçõesDoMenu)
            {
                i++;
                Console.WriteLine($"{i} - {opções}");
            }
            Linha();
            Console.Write($"{mensagemDeEscolha}: ");
            try
            {
                escolha = Convert.ToByte(Console.ReadLine());
            }
            catch (OverflowException)
            {
                escolha = 0;
            }
            return escolha;
        }
    }
}
