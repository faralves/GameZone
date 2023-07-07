namespace GameZone.Core.Utils
{
    public static class ValidaData
    {
        public static bool SejaNumero(int? value)
        {

            if (value != null)
            {
                int _value = 0;
                if (int.TryParse(value.ToString(), out _value))
                {
                    return true;
                }
                else
                {
                    return false;
                } 
            }
            else
            {
                return false;
            }
        }

        public static bool SejaData(DateTime? value)
        {
            if (value != null)
            {
                return value.Equals(default(DateTime));
            }

            return false;
        }

        public static bool SejaDataNaoNulable(DateTime value)
        {
            if (value != null)
            {
                return value.Equals(default(DateTime));
            }

            return false;
        }

        public static bool EhDataMinimaDotNet(DateTime data)
        {
            if (data == DateTime.MinValue)
                return false;

            return true;
        }

        public static bool EhDataMinimaDotNet(DateTime? data)
        {
            if (data.Value == DateTime.MinValue)
                return false;

            return true;
        }

        public static bool EhDataMaximaDotNet(DateTime? data)
        {
            if (data.Value == DateTime.MaxValue)
                return false;

            return true;
        }

        public static bool ValidarDataMenorQueDateTimeNow(DateTime? data)
        {
            if (data.Value > DateTime.Now)
                return false;

            return true;
        }

        public static bool ValidarDataMenorQueDateTimeNow(DateTime data)
        {
            if (data > DateTime.Now)
                return false;

            return true;
        }

        /// <summary>
        /// Se Mês nulo ou vazio retorna por referencia o mês atual
        /// </summary>
        /// <param name="mes"></param>
        /// <returns>true</returns>
        public static string ValidarMesVazioOuNulo(String mes)
        {
            if(string.IsNullOrEmpty(mes))
                mes = DateTime.Now.Month.ToString().PadLeft(2, '0');

            return mes;
        }

        public static string ValidarAnoVazioOuNulo(String ano)
        {
            if(string.IsNullOrEmpty(ano))
                ano = DateTime.Now.Year.ToString();

            return ano;
        }

        public static bool ValidarMes(String mes)
        {
            int mesInt = Convert.ToInt32(mes);
            if (mesInt >= 1 && mesInt <= 12)
                return true;

            return false;
        }

        public static bool ValidarAno(String ano)
        {
            int anoInt = Convert.ToInt32(ano);
            if (anoInt >= (DateTime.Now.Year - 100) && anoInt <= DateTime.Now.Year)
                return true;

            return false;
        }

        public static bool ValidarMesAnoInicialAndFinalRange2Meses(string mesInicial, string anoInicial, string mesFinal, string anoFinal)
        {
            try
            {
                int mesInicialInt = Convert.ToInt32(mesInicial);
                int anoInicialInt = Convert.ToInt32(anoInicial);
                DateTime dataInicial = new DateTime(anoInicialInt, mesInicialInt, 1);

                int mesFinalInt = Convert.ToInt32(mesFinal);
                int anoFinalInt = Convert.ToInt32(anoFinal);
                DateTime dataFinal = new DateTime(anoFinalInt, mesFinalInt, 1);

                if (dataInicial >= dataFinal.AddMonths(-2))
                    return true;

                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public static bool ValidarMesAnoInicialMaiorQueMesAnoFinal(string mesInicial, string anoInicial, string mesFinal, string anoFinal)
        {
            try
            {
                int mesInicialInt = Convert.ToInt32(mesInicial);
                int anoInicialInt = Convert.ToInt32(anoInicial);
                DateTime dataInicial = new DateTime(anoInicialInt, mesInicialInt, 1);

                int mesFinalInt = Convert.ToInt32(mesFinal);
                int anoFinalInt = Convert.ToInt32(anoFinal);
                DateTime dataFinal = new DateTime(anoFinalInt, mesFinalInt, 1);

                if (dataInicial > dataFinal)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
