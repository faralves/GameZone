using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace GameZone.Core.Utils
{
    public static class StringUtils
    {
        public static string ApenasNumeros(this string str, string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }

        public static string RemoverAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }

        public static string FormatarDocumento(string documento)
        {
            if (!string.IsNullOrEmpty(documento))
            {
                if (documento.Substring(0, 3) == "000")
                {
                    return Convert.ToUInt64(documento).ToString(@"000\.000\.000\-00");
                }
                else
                {
                    return Convert.ToUInt64(documento).ToString(@"00\.000\.000\/0000\-00");
                }
            }
            else
                return documento;
        }

        public static String DateTimeToString(DateTime data, String formato)
        {
            //DefinirCulturaBR();
            if (data == null || data == DateTime.MinValue)
            {
                return "00000000";
            }
            else
            {
                return data.ToString(formato, Thread.CurrentThread.CurrentUICulture);
            }
        }

        public static String DateTimeToString_ddMMyyyy(DateTime data)
        {
            return DateTimeToString(data, "ddMMyyyy");
        }

        public static DateTime? String_ddMMyyyyToDateTime(string strdata)
        {
            DateTime? data = null;

            int dia = 0;
            int mes = 0;
            int ano = 0;
            if (!string.IsNullOrEmpty(strdata) && strdata.Length >= 6 )
            {
                switch (strdata.Length)
                {
                    case 6:
                        dia = Convert.ToInt32(strdata.Substring(0, 1));
                        mes = Convert.ToInt32(strdata.Substring(1, 1));
                        ano = Convert.ToInt32(strdata.PadRight(4));

                        break;
                    case 7:
                        dia = Convert.ToInt32(strdata.Substring(0, 1));
                        mes = Convert.ToInt32(strdata.Substring(1, 2));
                        ano = Convert.ToInt32(strdata.PadRight(4));

                        break;
                    case 8:
                        dia = Convert.ToInt32(strdata.Substring(0, 2));
                        mes = Convert.ToInt32(strdata.Substring(2, 2));
                        ano = Convert.ToInt32(strdata.PadRight(4));

                        break;
                    default:
                        break;
                }

                data = new DateTime().AddDays(dia).AddMonths(mes).AddYears(ano);
            }


            return data;
        }

        public static String DateTimeToString_dd_MM_yyyySemZero(String valor)
        {
            try
            {
                //DefinirCulturaAppConfig();

                if (valor != null)
                    valor = valor.Trim();

                if (valor.Contains(" "))
                    valor = valor.Replace(" ", "/");

                if (!valor.Contains("/"))
                    valor = StringToDateTime(valor).ToString();

                DateTime data = DateTime.Parse(valor);

                return DateTimeToString_dd_MM_yyyySemZero(data);

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static String DateTimeToString_dd_MM_yyyySemZero(DateTime data)
        {
            try
            {
                //DefinirCulturaAppConfig();

                if (data == null || data == DateTime.MinValue)
                    return string.Empty;
                //return DateTime.MinValue.ToString("dd/MM/yyyy");
                else
                    return data.ToString("ddMMyyyy");
            }
            catch
            {
                //return DateTime.MinValue.ToString("dd/MM/yyyy");
                return string.Empty;
            }
        }

        public static String DateTimeToString_dd_MM_yyyySemZero_v2(DateTime data)
        {
            try
            {
                if (data == null || data == DateTime.MinValue)
                    return string.Empty;
                //return DateTime.MinValue.ToString("dd/MM/yyyy");
                else
                    return data.ToString("dd/MM/yyyy");
            }
            catch
            {
                //return DateTime.MinValue.ToString("dd/MM/yyyy");
                return string.Empty;
            }
        }

        public static String DateTimeToString_dd_MM_yyyy(DateTime data)
        {
            return DateTimeToString(data, "dd/MM/yyyy");
        }



        public static String DateTimeToString_yyyyMMdd(DateTime data)
        {
            return DateTimeToString(data, "yyyyMMdd");
        }

        public static String DateTimeToString_yyyy_MM_dd(DateTime data)
        {
            return data.Year.ToString().PadLeft(4, '0') + "-" + data.Month.ToString().PadLeft(2, '0') + "-" + data.Day.ToString().PadLeft(2, '0');
        }

        public static String YYYYMMDDToDD_MM_YYYY(String str)
        {
            try
            {
                if (IsNullOrWhiteSpaces(str))
                    return string.Empty;
                else
                    return str.Substring(6, 2) + "/" + str.Substring(4, 2) + "/" + str.Substring(0, 4);
            }
            catch
            {
                return "";
            }
        }

        public static String DateTimeToString_HH_mm(String valor)
        {
            try
            {
                if (valor.Contains(" "))
                    valor = valor.Replace(" ", ":");
                DateTime data = DateTime.Parse(valor);
                return DateTimeToString_HH_mm(data);
            }
            catch
            { return string.Empty; }
        }
        public static String DateTimeToString_HH_mm(DateTime data)
        {
            if (data == null || data == DateTime.MinValue)
                return "";
            else
            {
                string hora = data.ToString("HH:mm");
                if (IsNullOrWhiteSpaces(hora.Replace(":", "")))
                    hora = string.Empty;
                return hora;
            }
        }

        public static DateTime StringToDateTime(String str)
        {
            try
            {
                if (IsNullOrWhiteSpaces(str))
                    str = DateTime.MinValue.ToString();
                else if (str.Contains("/"))
                {
                    string strAux = str.Replace("/", "").Replace(":", "").Replace(" ", "");
                    Int64 data = ValidaNumero.ToInteger64(strAux);
                    if (IsNullOrWhiteSpaces(data))
                        str = DateTime.MinValue.ToString();
                }

                str = str.Trim();
                if (str.Length == 8 && !str.Contains("/"))
                {
                    str = str.Substring(0, 2) + "/" + str.Substring(2, 2) + "/" + str.Substring(4, 4);
                }

                return Convert.ToDateTime(str);
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }


        public static DateTime StringToDateTimePTBR(String str)
        {
            try
            {
                //DefinirCulturaBR();

                if (IsNullOrWhiteSpaces(str))
                    str = DateTime.MinValue.ToString();

                if (str.Contains("T"))
                {
                    str = str.Replace("T", " ");
                }

                if (str.Contains(" AS"))
                {
                    str = str.Replace(" AS", "");
                }

                str = str.Trim();
                if (str.Length == 8 && !str.Contains("/"))
                {
                    //str = str.Substring(0, 2) + "/" + str.Substring(2, 2) + "/" + str.Substring(4, 4);

                    int ano = ValidaNumero.ToInteger(str.Substring(4, 4));
                    if (ano > 1970 && ano <= DateTime.Now.Year)
                    {
                        str = str.Substring(0, 2) + "/" + str.Substring(2, 2) + "/" + str.Substring(4, 4);
                    }
                    else
                    {
                        str = str.Replace("/", "");
                        str = str.Substring(6, 2) + "/" + str.Substring(4, 2) + "/" + str.Substring(0, 4);

                        ano = ValidaNumero.ToInteger(str.Substring(6, 4));
                        if (ano < 1970 || ano > DateTime.Now.Year)
                            str = DateTime.MinValue.ToString();

                    }
                }

                return Convert.ToDateTime(str, Thread.CurrentThread.CurrentUICulture);

            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime StrtoDateDDMMANO(string data)
        {
            DateTime ret;
            ret = DateTime.MinValue;
            if (!IsNullOrWhiteSpaces(data))
            {
                try
                {
                    ret = Convert.ToDateTime(FomataDataConversao(data));
                }
                catch
                {
                    ret = DateTime.MinValue;

                }
            }

            return ret;

        }

        private static string FomataDataConversao(string data)
        {
            string dataformatada = String.Empty;
            try
            {
                if (!IsNullOrWhiteSpaces(data))
                {
                    if (data.IndexOf("/") > -1)
                    {
                        if (data.Trim().Length == 10 || data.Trim().Length == 8)
                        {
                            dataformatada = data + " 00:00:00";

                        }
                    }
                    else
                    {

                        if (data.Trim().Length == 8)
                        {
                            dataformatada = data.Substring(0, 2) + "/" + data.Substring(2, 2) + "/" + data.Substring(4, 4) + " 00:00:00";

                        }
                        if (data.Trim().Length == 6)
                        {
                            dataformatada = data.Substring(0, 2) + "/" + data.Substring(2, 2) + "/" + "20" + data.Substring(4, 2) + " 00:00:00";

                        }

                    }

                }
            }
            catch (Exception e) { };
            return dataformatada;

        }


        public static bool IsNullOrWhiteSpaces(String str)
        {
            Boolean vazio = false;
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string strAux = str;
                    TirarOutrosCaracteres(ref strAux);
                    if (string.IsNullOrEmpty(strAux.Trim()))
                        vazio = true;
                    else
                    {
                        if (Regex.IsMatch(str.Trim(), "^[0-9]+$"))
                        {
                            //long val_0 = ToLong(str.Replace(",", "").Replace(".", ""));
                            Double val_0 = Convert.ToDouble(str.Replace(",", "").Replace(".", ""));
                            if (val_0 == 0)
                                vazio = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ((string.IsNullOrEmpty(str) || str.Trim().Equals(String.Empty) || vazio) ? true : false);
        }

        public static bool IsNullOrWhiteSpaces(Object obj)
        {
            try
            {
                if (obj == null)
                    return true;
                else
                    return IsNullOrWhiteSpaces(obj.ToString());
            }
            catch { }
            return false;
        }

        public static bool IsDBNullOrWhiteSpaces(Object obj)
        {
            if (obj is DBNull || obj == null)
            {
                return true;
            }
            else if (obj is int)
            {
                return ((int)obj) == 0 ? true : false;
            }
            else if (obj is DateTime)
            {
                return ((DateTime)obj) == DateTime.MinValue ? true : false;
            }
            else if (obj is String)
            {
                string texto = ((String)obj).ToString().Trim();
                return (texto.Equals(String.Empty) || texto.Equals("NULL") ? true : false);
            }
            else if (obj is List<string>)
            {
                var texto = ((List<string>)obj);
                return (texto == null || texto.Count == 0 ? true : false);
            }
            else
            {
                return true;
            }
        }

        public static bool IsDBNullOrWhiteSpaces(DataTable dt, string coluna)
        {
            bool eNulo = true;
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName == coluna.ToUpper())
                {
                    eNulo = false;
                }
            }

            if (!eNulo)
            {
                if (dt.Rows[0][coluna] is DBNull)
                    eNulo = true;
            }

            return eNulo;

        }

        #region -- Acentuação e Pontuação --

        public static void RemoverAcentosDoArquivo(String filePath)
        {
            if (File.Exists(filePath))
            {
                String conteudo;
                using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding((new CultureInfo("pt-BR")).TextInfo.ANSICodePage)))
                {
                    conteudo = sr.ReadToEnd();
                }
                TirarAcento(ref conteudo);
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(conteudo);
                }
            }
        }

        public static void TirarAcento(ref String str)
        {
            str = TirarAcento(str);
        }

        public static String TirarAcento(String str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                str = str.Replace("ã", "a");
                str = str.Replace("á", "a");
                str = str.Replace("à", "a");
                str = str.Replace("â", "a");
                str = str.Replace("ä", "a");
                str = str.Replace("Ã", "A");
                str = str.Replace("Á", "A");
                str = str.Replace("À", "A");
                str = str.Replace("Â", "A");
                str = str.Replace("Ä", "A");

                str = str.Replace("é", "e");
                str = str.Replace("è", "e");
                str = str.Replace("ê", "e");
                str = str.Replace("ë", "e");
                str = str.Replace("É", "E");
                str = str.Replace("È", "E");
                str = str.Replace("Ê", "E");
                str = str.Replace("Ë", "E");

                str = str.Replace("í", "i");
                str = str.Replace("ì", "i");
                str = str.Replace("î", "i");
                str = str.Replace("ï", "i");
                str = str.Replace("Í", "I");
                str = str.Replace("Ì", "I");
                str = str.Replace("Î", "I");
                str = str.Replace("Ï", "I");

                str = str.Replace("õ", "o");
                str = str.Replace("ó", "o");
                str = str.Replace("ò", "o");
                str = str.Replace("ô", "o");
                str = str.Replace("ö", "o");
                str = str.Replace("Õ", "O");
                str = str.Replace("Ó", "O");
                str = str.Replace("Ò", "O");
                str = str.Replace("Ô", "O");
                str = str.Replace("Ö", "O");

                str = str.Replace("ú", "u");
                str = str.Replace("ù", "u");
                str = str.Replace("û", "u");
                str = str.Replace("ü", "u");
                str = str.Replace("Ú", "U");
                str = str.Replace("Ù", "U");
                str = str.Replace("Û", "U");
                str = str.Replace("Ü", "U");


                str = str.Replace("Ç", "C");
                str = str.Replace("ç", "c");

                str = TirarSimboloNumeroOrdinal(str);

                str = str.Replace("ª", "");
                str = str.Replace("~", "");
                str = str.Replace("´", "");
                str = str.Replace("`", "");
                str = str.Replace("^", "");
                str = str.Replace("¨", "");
            }
            else
            {
                str = String.Empty;
            }
            return str;
        }

        public static void TirarOutrosCaracteres(ref String str)
        {
            str = str.Replace(".", " ");
            str = str.Replace(",", " ");
            str = str.Replace(";", " ");
            str = str.Replace(":", " ");
            str = str.Replace("/", " ");
            str = str.Replace(@"\", " ");
            str = str.Replace("  ", " ");
            str = str.Replace("'", " ");
            str = str.Replace("*", " ");
            str = str.Replace("(", " ");
            str = str.Replace(")", " ");
            str = str.Replace("{", " ");
            str = str.Replace("}", " ");
            str = str.Replace("[", " ");
            str = str.Replace("]", " ");
            str = str.Replace("-", " ");
            str = str.Replace(">", " ");
            str = str.Replace("<", " ");
            str = str.Replace("", " ");
            str = str.Replace("", " ");
            str = str.Replace("", " ");
            str = str.Replace("", " ");
            str = str.Replace("", " ");
            str = str.Replace("¨", " ");
        }

        public static String TirarOutrosCaracteres(String str)
        {
            if (!IsNullOrWhiteSpaces(str))
                TirarOutrosCaracteres(ref str);
            return str;
        }


        public static String TirarOutrosCaracteres2(String str)
        {
            str = str.Replace("  ", " ");
            str = str.Replace(">", " ");
            str = str.Replace("<", " ");
            str = str.Replace("", " ");
            str = str.Replace("", " ");
            str = str.Replace("", " ");
            str = str.Replace("", " ");
            str = str.Replace("", " ");

            return str;
        }

        public static String TirarCaracteres(String str)
        {
            TirarOutrosCaracteres(ref str);
            return str;
        }

        public static String TirarOutrosCaracteres3(String str)
        {

            string input = str;
            string pattern = @"[^0-9a-z!¡$%&/\()=?¿*+-_{};:,áéíóúàèìòùâêîôûãõç'.\s][^>][^<]";
            string replacement = "";
            Regex rgx = new Regex(pattern);
            str = rgx.Replace(str, replacement);

            return str;
        }

        public static String TirarOutrosCaracteres4(String str)
        {

            string input = str;
            string pattern = @"[^0-9a-zA-Z]";
            string replacement = "";
            Regex rgx = new Regex(pattern);
            str = rgx.Replace(str, replacement);

            return str;
        }

        public static String TirarSimboloNumeroOrdinal(String str)
        {
            str = str.Replace("º", "");
            str = str.Replace("°", "");
            return str;
        }

        public static String TirarLetras(String str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                str = str.Replace("ã", "");
                str = str.Replace("á", "");
                str = str.Replace("à", "");
                str = str.Replace("â", "");
                str = str.Replace("ä", "");
                str = str.Replace("Ã", "");
                str = str.Replace("Á", "");
                str = str.Replace("À", "");
                str = str.Replace("Â", "");
                str = str.Replace("Ä", "");

                str = str.Replace("é", "");
                str = str.Replace("è", "");
                str = str.Replace("ê", "");
                str = str.Replace("ë", "");
                str = str.Replace("É", "");
                str = str.Replace("È", "");
                str = str.Replace("Ê", "");
                str = str.Replace("Ë", "");

                str = str.Replace("í", "");
                str = str.Replace("ì", "");
                str = str.Replace("î", "");
                str = str.Replace("ï", "");
                str = str.Replace("Í", "");
                str = str.Replace("Ì", "");
                str = str.Replace("Î", "");
                str = str.Replace("Ï", "");
                str = str.Replace("I", "");
                str = str.Replace("i", "");

                str = str.Replace("õ", "");
                str = str.Replace("ó", "");
                str = str.Replace("ò", "");
                str = str.Replace("ô", "");
                str = str.Replace("ö", "");
                str = str.Replace("Õ", "");
                str = str.Replace("Ó", "");
                str = str.Replace("Ò", "");
                str = str.Replace("Ô", "");
                str = str.Replace("Ö", "");
                str = str.Replace("O", "");
                str = str.Replace("o", "");

                str = str.Replace("ú", "");
                str = str.Replace("ù", "");
                str = str.Replace("û", "");
                str = str.Replace("ü", "");
                str = str.Replace("Ú", "");
                str = str.Replace("Ù", "");
                str = str.Replace("Û", "");
                str = str.Replace("Ü", "");
                str = str.Replace("U", "");
                str = str.Replace("u", "");

                str = str.Replace("Ç", "");
                str = str.Replace("ç", "");
                str = str.Replace("Ç", "");
                str = str.Replace("ç", "");

                str = str.Replace("º", "");
                str = str.Replace("ª", "");
                str = str.Replace("~", "");
                str = str.Replace("´", "");
                str = str.Replace("`", "");
                str = str.Replace("^", "");
                str = str.Replace("¨", "");

                str = str.Replace("B", "");




            }
            else
            {
                str = String.Empty;
            }
            return str;
        }

        public static string AdicionarPontuacaoCpf(string cpf)
        {
            if (cpf.Length == 11)
            {
                return cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);
            }
            else
            {
                return cpf;
            }
        }

        public static string AdicionarPontuacaoCnpj(string cnpj)
        {
            if (cnpj.Length == 14)
            {
                return cnpj.Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
            }
            else
            {
                return cnpj;
            }
        }

        public static bool IsDocumentoValido(string documento)
        {

            if (documento.Trim().Length == 14)
            {
                return IsCnpj(documento);
            }
            else if (documento.Trim().Length == 11)
            {
                return IsCpf(documento);
            }
            else
            {
                return false;
            }
        }

        public static bool IsCnpj(string cnpj)
        {
            //00000534922007
            if (cnpj.Substring(0, 5).Equals("00000"))
                return false;

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
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
            return cnpj.EndsWith(digito);
        }

        public static bool IsCpf(string cpf)
        {
            cpf = cpf.TrimStart('0'); // remove zeros a esquerda
            cpf = cpf.PadLeft(11, '0'); // adiciona de volta caso cpf inicie com 0
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (System.Text.RegularExpressions.Regex.IsMatch(cpf, @"[a-zA-Z]"))
                return false;
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static bool IsCnpjMacorati(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
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
            return cnpj.EndsWith(digito);
        }



        public static string RemoverPontuacaoCpf(string cpf)
        {
            return cpf.Replace(".", "").Replace("-", "");
        }

        public static string RemoverPontuacaoCpfCnpj(string cpf)
        {
            return cpf.Replace(".", "").Replace("-", "").Replace("/", "");
        }

        public static string RemoverCaracterEspecial(String str)
        {
            RemoverCodigoCaracterEspecial(ref str);
            return str;
        }

        public static void RemoverCodigoCaracterEspecial(ref String str)
        {
            str = str.Replace("&aacute;", "a");
            str = str.Replace("&acirc;", "a");
            str = str.Replace("&atilde;", "a");
            str = str.Replace("&agrave;", "a");
            str = str.Replace("&Aacute;", "A");
            str = str.Replace("&Acirc;", "A");
            str = str.Replace("&Atilde;", "A");
            str = str.Replace("&Agrave;", "A");

            str = str.Replace("&eacute;", "e");
            str = str.Replace("&ecirc;", "e");
            str = str.Replace("&Eacute;", "E");
            str = str.Replace("&Ecirc;", "E");

            str = str.Replace("&iacute;", "i");
            str = str.Replace("&icirc;", "i");
            str = str.Replace("&igrave;", "i");
            str = str.Replace("&iuml;", "i");
            str = str.Replace("&Iacute;", "I");
            str = str.Replace("&Icirc;", "I");
            str = str.Replace("&Igrave;", "I");
            str = str.Replace("&Iuml;", "I");

            str = str.Replace("&oacute;", "o");
            str = str.Replace("&otilde;", "o");
            str = str.Replace("&ocirc;", "o");
            str = str.Replace("&ograve;", "o");
            str = str.Replace("&ouml;", "o");
            str = str.Replace("&Oacute;", "O");
            str = str.Replace("&Otilde;", "O");
            str = str.Replace("&Ocirc;", "O");
            str = str.Replace("&Ograve;", "O");
            str = str.Replace("&Ouml;", "O");

            str = str.Replace("&uacute;", "u");
            str = str.Replace("&uuml;", "u");
            str = str.Replace("&ucirc;", "u");
            str = str.Replace("&ugrave;", "u");
            str = str.Replace("&Uacute", "U");
            str = str.Replace("&Uuml;", "U");
            str = str.Replace("&Ucirc;", "U");
            str = str.Replace("&Ugrave;", "U");

            str = str.Replace("&Ccedil;", "C");
            str = str.Replace("&ccedil;", "c");

            str = str.Replace("\u001a", " "); // char desconhecido
            str = str.Replace("&#26;", " ");
            str = str.Replace("&deg;", " ");
            str = str.Replace("&ordm;", "");
            str = str.Replace("&ordf;", "");
            str = str.Replace("&tilde;", "");
            str = str.Replace("&cute;", "");
            str = str.Replace("&circ;", "");
            str = str.Replace("&uml;", "");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&amp;", " ");
            str = str.Replace("&AMP;", " ");
            str = str.Replace("\n", "");
            str = str.Replace("\r", " ");
            str = str.Replace("\t", " ");

        }

        public static void RemoverCodigoCaracterEspecialComAcento(ref String str)
        {

            str = str.Replace("&Aacute;", "Á");
            str = str.Replace("&aacute;", "á");
            str = str.Replace("&Acirc;", "Â");
            str = str.Replace("&acirc;", "â");
            str = str.Replace("&Agrave;", "À");
            str = str.Replace("&agrave;", "à");
            str = str.Replace("&Aring;", "Å");
            str = str.Replace("&aring;", "å");
            str = str.Replace("&Atilde;", "Ã");
            str = str.Replace("&atilde;", "ã");
            str = str.Replace("&Auml;", "Ä");
            str = str.Replace("&auml;", "ä");
            str = str.Replace("&AElig;", " ");
            str = str.Replace("&aelig;", " ");
            str = str.Replace("&Eacute;", "É");
            str = str.Replace("&eacute;", "é");
            str = str.Replace("&Ecirc;", "Ê");
            str = str.Replace("&ecirc;", "ê");
            str = str.Replace("&Egrave;", "È");
            str = str.Replace("&egrave;", "è");
            str = str.Replace("&Euml;", "Ë");
            str = str.Replace("&euml;", "ë");
            str = str.Replace("&Iacute;", "Í");
            str = str.Replace("&iacute;", "í");
            str = str.Replace("&Icirc;", "Î");
            str = str.Replace("&icirc;", "î");
            str = str.Replace("&Igrave;", "Ì");
            str = str.Replace("&igrave;", "ì");
            str = str.Replace("&Iuml;", "Ï");
            str = str.Replace("&iuml;", "ï");
            str = str.Replace("&Oacute;", "Ó");
            str = str.Replace("&oacute;", "ó");
            str = str.Replace("&Ocirc;", "Ô");
            str = str.Replace("&ocirc;", "ô");
            str = str.Replace("&Ograve;", "Ò");
            str = str.Replace("&ograve;", "ò");
            str = str.Replace("&Oslash;", " ");
            str = str.Replace("&oslash;", " ");
            str = str.Replace("&Otilde;", "Õ");
            str = str.Replace("&otilde;", "õ");
            str = str.Replace("&Ouml;", "Ö");
            str = str.Replace("&ouml;", "ö");
            str = str.Replace("&Uacute;", "Ú");
            str = str.Replace("&uacute;", "ú");
            str = str.Replace("&Ucirc;", "Û");
            str = str.Replace("&ucirc;", "û");
            str = str.Replace("&Ugrave;", "Ù");
            str = str.Replace("&ugrave;", "ù");
            str = str.Replace("&Uuml;", "Ü");
            str = str.Replace("&uuml;", "ü");
            str = str.Replace("&Ccedil;", "Ç");
            str = str.Replace("&ccedil;", "ç");
            str = str.Replace("&Ntilde;", "Ñ");
            str = str.Replace("&ntilde;", "ñ");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&amp;", "&");
            str = str.Replace("&quot;", "\"");
            str = str.Replace("&reg;", "®");
            str = str.Replace("&copy;", "©");
            str = str.Replace("&Yacute;", "Ý");
            str = str.Replace("&yacute;", "ý");

            //Þ .............. &THORN;
            str = str.Replace("&THORN;", " ");
            //þ ................ &thorn;
            str = str.Replace("&thorn;", " ");
            //ß ................ &szlig;
            str = str.Replace("&szlig;", " ");
            //Ð ................... &ETH;
            str = str.Replace("&ETH;", " ");
            //ð .................. &eth;
            str = str.Replace("&eth;", " ");

            str = str.Replace("\u001a", " "); // char desconhecido
            str = str.Replace("&#26;", " ");
            str = str.Replace("&deg;", "º");
            str = str.Replace("&ordm;", " ");
            str = str.Replace("&ordf;", "");
            str = str.Replace("&tilde;", "");
            str = str.Replace("&cute;", "");
            str = str.Replace("&circ;", "");
            str = str.Replace("&uml;", "");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&amp;", " ");
            str = str.Replace("&AMP;", " ");

            str = str.Replace("Ã§Ãµ", "õe");
            str = str.Replace("Ã£", "ã");

        }

        public static String RemoverAcentoAndCaracterEspecial(String str)
        {
            TirarAcento(ref str);
            TirarOutrosCaracteres(ref str);
            return str;
        }

        public static String RemoverPontoVirgula(String str)
        {
            return str.Replace(".", "").Replace(",", "");
        }

        public static String CodificarCaracteresEspeciais(String str)
        {
            str = str.Replace("$", "%24");
            str = str.Replace("(", "%28");
            str = str.Replace(")", "%29");
            str = str.Replace("<", "%3C");
            str = str.Replace(">", "%3E");
            str = str.Replace("#", "%23");
            str = str.Replace("{", "%7B");
            str = str.Replace("}", "%7D");
            str = str.Replace("^", "%5E");
            str = str.Replace("~", "%7E");
            str = str.Replace("[", "%5B");
            str = str.Replace("]", "%5D");
            str = str.Replace("`", "%60");
            str = str.Replace("+", "%2B");
            str = str.Replace(":", "%3A");



            return str;
        }

        /// <summary>
        /// Codifica o conteúdo em UTF-8
        /// Útil para páginas html que retornam conteúdo com codificação diferente e perde-se as letras acentuadas
        /// </summary>
        /// <param name="conteudo">Conteúdo a ser codificado</param>
        /// <returns></returns>
        public static String CodificarEmUtf8(String conteudo)
        {
            return Encoding.GetEncoding("UTF-8").GetString(Encoding.Default.GetBytes(conteudo));
        }

        #endregion

        public static string ExtrairCodigo(string str, int tam)
        {
            string retorno = String.Empty;
            for (int idx = 0; idx < tam && idx < str.Length; idx++)
            {
                if (!char.IsNumber(Convert.ToChar(str.Substring(idx, 1))))
                {
                    break;
                }
                else
                {
                    retorno = retorno + str.Substring(idx, 1);
                }
            }
            return retorno.PadLeft(tam, '0');
        }

        public static String ToString12(Decimal valor)
        {
            return ToStringN(valor, 12);
        }

        public static String ToStringN(Decimal valor, int numeroDeCaracteres)
        {
            String strValor = ToStringSemPontuacao(valor);
            if (strValor.Length > numeroDeCaracteres)
            {
                return strValor.Substring(strValor.Length - numeroDeCaracteres);
            }
            else
            {
                return strValor.PadLeft(numeroDeCaracteres, '0');
            }
        }

        public static String ToStringSemPontuacao(Decimal valor)
        {
            return valor.ToString("N2").Replace(",", "").Replace(".", "");
        }


    }
}