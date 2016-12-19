using System.Text.RegularExpressions;

namespace Utilities.Validations
{
    public static class ModelValidations
    {   
        /// <summary>
        /// Rut validation of Persona
        /// </summary>
        /// <param name="rut"></param>
        /// <returns>Without dots, with verification number and dash e.g. 11502391-5</returns>
        public static bool isValidRut(string rut) {
            rut = rut.Replace(".", "").ToUpper();
            Regex expresion = new Regex("^([0-9]+-[0-9K])$");
            string dv = rut.Substring(rut.Length - 1, 1);
            if (!expresion.IsMatch(rut)) {
                return false;
            }
            char[] charCorte = { '-' };
            string[] rutTemp = rut.Split(charCorte);
            if (dv != Digito(int.Parse(rutTemp[0]))) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// método que calcula el digito verificador a partir
        /// de la mantisa del rut
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        public static string Digito(int rut) {
            int suma = 0;
            int multiplicador = 1;
            while (rut != 0) {
                multiplicador++;
                if (multiplicador == 8)
                multiplicador = 2;
                suma += (rut % 10) * multiplicador;
                rut = rut / 10;
            }
            suma = 11 - (suma % 11);
            if (suma == 11)	{
                return "0";
            } else if (suma == 10) {
                return "K";
            } else {
                return suma.ToString();
            }
        }
    }
}