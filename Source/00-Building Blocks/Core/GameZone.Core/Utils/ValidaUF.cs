namespace GameZone.Core.Utils
{
    public static class ValidaUF
    {
        public static bool ValidarUF(string uf)
        {
            bool ufExistente = true;
            if (!string.IsNullOrEmpty(uf))
            {
                switch (uf.ToUpper())
                {
                    case "RO":
                    case "AC":
                    case "AM":
                    case "RR":
                    case "PA":
                    case "AP":
                    case "TO":
                    case "MA":
                    case "PI":
                    case "CE":
                    case "RN":
                    case "PB":
                    case "PE":
                    case "AL":
                    case "SE":
                    case "BA":
                    case "MG":
                    case "ES":
                    case "RJ":
                    case "SP":
                    case "PR":
                    case "SC":
                    case "RS":
                    case "MS":
                    case "MT":
                    case "GO":
                    case "DF":
                        ufExistente = true;
                        break;
                    default:
                        ufExistente = false;
                        break;
                }
            }

            return ufExistente;
        }
    }
}
