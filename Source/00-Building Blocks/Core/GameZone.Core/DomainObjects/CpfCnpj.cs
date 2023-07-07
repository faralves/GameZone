using GameZone.Core.Utils;

namespace GameZone.Core.DomainObjects
{
    public class CpfCnpj
    {
        public const int CpfCnpjMaxLength = 15;
        public string Numero { get; private set; }

        //Construtor do EntityFramework
        protected CpfCnpj() { }

        public CpfCnpj(string numero)
        {
            if (!Validar(numero)) throw new DomainException("CPF / CNPJ inválido");
            Numero = numero;
        }

        public static bool Validar(string cpfCnpj)
        {
            cpfCnpj = cpfCnpj.ApenasNumeros(cpfCnpj);

            if (cpfCnpj.Length > 15)
                return false;

            while (cpfCnpj.Length < 11)
                cpfCnpj = '0' + cpfCnpj;
            
            if(cpfCnpj.Length == 11)
                return ValidarCPF(cpfCnpj);
            else
                return ValidarCNPJ(cpfCnpj);
        }

        private static bool ValidarCPF(string cpfCnpj)
        {
            var igual = true;
            for (var i = 1; i < 11 && igual; i++)
                if (cpfCnpj[i] != cpfCnpj[0])
                    igual = false;

            if (igual || cpfCnpj == "12345678909")
                return false;

            var numeros = new int[11];

            for (var i = 0; i < 11; i++)
                numeros[i] = int.Parse(cpfCnpj[i].ToString());

            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            var resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }
        private static bool ValidarCNPJ(string cpfCnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cpfCnpj = cpfCnpj.Trim();
            cpfCnpj = cpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cpfCnpj.Length != 14)
                return false;
            tempCnpj = cpfCnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpfCnpj.EndsWith(digito);
        }
    }
}