namespace GameZone.Core.Utils
{
    public static class ValidaNumero
    {
        public static bool SejaNumero(string value)
        {
            if (value != null)
            {
                return value.All(char.IsNumber);
            }

            return false;
        }

        public static Int32 ToInteger(object numero)
        {
            if (numero != null)
                return ToInteger(numero.ToString());
            else
                return 0;
        }
        public static Int32 ToInteger(double numero)
        {
            return ToInteger(numero.ToString());
        }

        public static Int32 ToInteger(String numero)
        {
            if (StringUtils.IsNullOrWhiteSpaces(numero))
                numero = string.Empty;

            Int32 inteiro = 0;
            Int32.TryParse(numero.Replace(" ", ""), out inteiro);
            return inteiro;
        }

        public static Int64 ToInteger64(double numero)
        {
            return ToInteger64(numero.ToString());
        }

        public static Int64 ToInteger64(String numero)
        {
            Int64 inteiro;
            Int64.TryParse(numero, out inteiro);
            return inteiro;
        }

        public static long ToLong(String valor)
        {
            try
            {
                if (StringUtils.IsNullOrWhiteSpaces(valor))
                    return 0;
                else
                    return long.Parse(valor);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static Decimal ToDecimalSemPonto(String valor)
        {
            string varanalise = valor;
            try
            {
                if (varanalise.IndexOf(",") < 0)
                {

                    valor = varanalise.Substring(0, varanalise.Length - 2) + "," + varanalise.Substring(varanalise.Length - 2);

                }
                return Convert.ToDecimal(valor);
            }
            catch (Exception EX)
            {
                return 0;
            }
        }

        public static Decimal ToDecimal(String valor)
        {
            try
            {
                return ToDecimal2(valor);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static Decimal ToDecimal2(String valor)
        {
            try
            {
                if (String.IsNullOrEmpty(valor))
                    return 0;

                if (!String.IsNullOrEmpty(valor) && valor.Length > 2 && (valor.Contains(",") && !valor.Contains(".")))
                {
                    string centavos = valor.Substring(valor.IndexOf(",") + 1);
                    if (centavos.Length < 2)
                    {
                        valor += "0";
                    }
                }

                if (!String.IsNullOrEmpty(valor) && valor.Length > 2 && (!valor.Contains(",") && valor.Contains(".")))
                {
                    string centavos = valor.Substring(valor.IndexOf(".") + 1);
                    if (centavos.Length < 2)
                    {
                        valor = valor + "0";
                    }
                }

                valor = valor.Replace("R$", "").Replace(".", "").Replace(",", "").Trim();
                if (valor.Length < 3)
                {
                    valor = valor.PadLeft(3, '0');
                }
                return Convert.ToDecimal(valor.Substring(0, valor.Length - 2) + "," + valor.Substring(valor.Length - 2, 2));
            }
            catch
            {
                return 0;
            }
        }

        public static Decimal Porcentagem(string valor, string porcentagem)
        {
            decimal retorno = 0;
            decimal valortotal;
            decimal porcentagemrecebida;
            valortotal = Convert.ToDecimal(valor);
            porcentagemrecebida = Convert.ToDecimal(porcentagem);
            try
            {
                retorno = ((valortotal / 100) * (porcentagemrecebida * 100));
            }
            catch
            {
                return 0;
            }
            return retorno;
        }


    }
}
