using System;
using System.Globalization;
using System.IO;

namespace FacturacionTaller
{
    public static class ModuloArchivo
    {
        private static readonly string RutaArchivo = "atenciones.txt";

        // 1. Almacena el nuevo registro al final del archivo atenciones.txt
        public static void GuardarAtencion(string linea)
        {
            using (StreamWriter sw = new StreamWriter(RutaArchivo, true))
            {
                sw.WriteLine(linea);
            }
        }

        // 2. Lee cada registro línea por línea, separa por '|' y muestra coincidencias por Placa
        public static void BuscarAtencion(string placaBuscada)
        {
            if (!File.Exists(RutaArchivo))
            {
                Console.WriteLine("El archivo atenciones.txt aún no existe. No hay registros.");
                return;
            }

            string placaObjetivo = (placaBuscada ?? string.Empty).Trim().ToUpper();
            bool encontrado = false;

            Console.WriteLine($"\n--- RESULTADOS DE BÚSQUEDA PARA PLACA: {placaObjetivo} ---");

            using (StreamReader sr = new StreamReader(RutaArchivo))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    string[] campos = linea.Split('|');
                    if (campos.Length >= 5)
                    {
                        string placa = campos[0].Trim().ToUpper();
                        if (placa == placaObjetivo)
                        {
                            encontrado = true;
                            Console.WriteLine($"Placa: {campos[0]} | Propietario: {campos[1]} | Servicio: {campos[2]} | Total: S/ {campos[3]} | Fecha: {campos[4]}");
                        }
                    }
                }
            }

            if (!encontrado)
            {
                Console.WriteLine($"No se encontraron atenciones registradas para la placa '{placaObjetivo}'.");
            }
        }

        // 3. Lee todo atenciones.txt, muestra todos los vehículos atendidos y la suma total recaudada
        public static void ListarTodasLasAtenciones()
        {
            if (!File.Exists(RutaArchivo))
            {
                Console.WriteLine("El archivo atenciones.txt aún no existe. No hay atenciones registradas.");
                return;
            }

            double sumaTotalRecaudada = 0.0;
            int contador = 0;

            Console.WriteLine("\n--- REPORTE GENERAL DE ATENCIONES DEL TALLER ---");
            Console.WriteLine("PLACA      | PROPIETARIO          | SERVICIO     | TOTAL (S/) | FECHA/HORA");
            Console.WriteLine("-------------------------------------------------------------------------------");

            using (StreamReader sr = new StreamReader(RutaArchivo))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    string[] campos = linea.Split('|');
                    if (campos.Length >= 5)
                    {
                        contador++;
                        double monto = 0.0;
                        double.TryParse(campos[3].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out monto);
                        sumaTotalRecaudada += monto;

                        Console.WriteLine($"{campos[0],-10} | {campos[1],-20} | {campos[2],-12} | {monto,10:F2} | {campos[4]}");
                    }
                }
            }

            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine($"Total de vehículos atendidos : {contador}");
            Console.WriteLine($"SUMA TOTAL RECAUDADA         : S/ {sumaTotalRecaudada:F2}");
        }
    }
}
