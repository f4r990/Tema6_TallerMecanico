using System;
using System.Globalization;

namespace FacturacionTaller
{
    public static class ModuloTaller
    {
        // 1. Determina y retorna la tarifa base según el tipo de servicio
        public static double ObtenerTarifaBase(string tipoServicio)
        {
            if (string.IsNullOrWhiteSpace(tipoServicio))
                return 120.00;

            switch (tipoServicio.Trim().ToUpper())
            {
                case "PREVENTIVO":
                    return 180.00;
                case "CORRECTIVO":
                    return 320.00;
                default: // "OTRO" (cualquier otro texto)
                    return 120.00;
            }
        }

        // 2. Retorna el 15% de descuento si es cliente frecuente ("SI"), caso contrario 0
        public static double CalcularDescuentoCliente(double tarifaBase, string esFrecuente)
        {
            if (!string.IsNullOrWhiteSpace(esFrecuente) &&
                esFrecuente.Trim().ToUpper() == "SI")
            {
                return tarifaBase * 0.15;
            }
            return 0.0;
        }

        // 3. Calcula y retorna el 18% (0.18) de IGV sobre la tarifa base luego de restar el descuento
        public static double CalcularImpuestoIGV(double tarifabaseConDescuento)
        {
            return tarifabaseConDescuento * 0.18;
        }

        // 4. Retorna el importe final (tarifaBase - descuento + igv)
        public static double CalcularTotalPagar(double tarifaBase, double descuento, double igv)
        {
            return tarifaBase - descuento + igv;
        }

        // 5. Genera la trama delimitada por PIPE (|) con los 5 campos y total formateado a 2 decimales (F2)
        public static string ConstruirRegistroPipe(string placa, string propietario, string tipoServicio, double totalPagar, string fecha)
        {
            string totalFormateado = totalPagar.ToString("F2", CultureInfo.InvariantCulture);
            return $"{placa}|{propietario}|{tipoServicio}|{totalFormateado}|{fecha}";
        }
    }
}
