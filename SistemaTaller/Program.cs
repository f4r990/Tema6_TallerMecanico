using System;
using FacturacionTaller;

namespace SistemaTaller
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int opcion = 0;
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=====================================================");
                    Console.WriteLine("   CENTRO DE SERVICIO TÉCNICO MECÁNICO - FACTURACIÓN ");
                    Console.WriteLine("=====================================================");
                    Console.WriteLine("1. Registrar atención");
                    Console.WriteLine("2. Buscar atención por Placa");
                    Console.WriteLine("3. Reporte general de atenciones");
                    Console.WriteLine("4. Salir");
                    Console.Write("Seleccione una opción (1-4): ");

                    opcion = int.Parse(Console.ReadLine());

                    switch (opcion)
                    {
                        case 1:
                            RegistrarAtencion();
                            break;
                        case 2:
                            BuscarAtencionPorPlaca();
                            break;
                        case 3:
                            ReporteGeneralAtenciones();
                            break;
                        case 4:
                            Console.WriteLine("\nSaliendo del sistema...");
                            break;
                        default:
                            Console.WriteLine("\nOpción fuera de rango. Ingrese un número del 1 al 4.");
                            Console.WriteLine("Presione una tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (FormatException)
                {
                    opcion = 0;
                    Console.WriteLine("\nError: Debe ingresar un valor numérico válido.");
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    opcion = 0;
                    Console.WriteLine($"\nOcurrió un error inesperado: {ex.Message}");
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                }
            } while (opcion != 4);
        }

        static void RegistrarAtencion()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- 1. REGISTRAR ATENCIÓN VEHICULAR ---");

                Console.Write("Ingrese placa del vehículo: ");
                string placa = Console.ReadLine()?.Trim().ToUpper();
                if (string.IsNullOrWhiteSpace(placa))
                    throw new ArgumentException("La placa del vehículo no puede estar vacía.");

                Console.Write("Ingrese nombre del propietario: ");
                string propietario = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(propietario))
                    throw new ArgumentException("El nombre del propietario no puede estar vacío.");

                Console.Write("Tipo de servicio (PREVENTIVO / CORRECTIVO / OTRO): ");
                string tipoServicio = Console.ReadLine()?.Trim().ToUpper();
                if (string.IsNullOrWhiteSpace(tipoServicio))
                    tipoServicio = "OTRO";

                Console.Write("¿Es cliente frecuente? (SI / NO): ");
                string esFrecuente = Console.ReadLine()?.Trim().ToUpper();

                // Cálculos mediante ModuloTaller
                double tarifaBase = ModuloTaller.ObtenerTarifaBase(tipoServicio);
                double descuento = ModuloTaller.CalcularDescuentoCliente(tarifaBase, esFrecuente);
                double tarifaConDescuento = tarifaBase - descuento;
                double igv = ModuloTaller.CalcularImpuestoIGV(tarifaConDescuento);
                double totalPagar = ModuloTaller.CalcularTotalPagar(tarifaBase, descuento, igv);

                // Fecha y hora exacta mediante la biblioteca estándar del sistema
                string fechaHoraActual = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                // Construcción de trama PIPE y guardado en atenciones.txt mediante ModuloArchivo
                string registroPipe = ModuloTaller.ConstruirRegistroPipe(
                    placa,
                    propietario,
                    tipoServicio,
                    totalPagar,
                    fechaHoraActual
                );

                ModuloArchivo.GuardarAtencion(registroPipe);

                Console.WriteLine("\n--- COMPROBANTE DE ATENCIÓN ---");
                Console.WriteLine($"Fecha/Hora    : {fechaHoraActual}");
                Console.WriteLine($"Placa         : {placa}");
                Console.WriteLine($"Propietario   : {propietario}");
                Console.WriteLine($"Servicio      : {tipoServicio}");
                Console.WriteLine($"Tarifa Base   : S/ {tarifaBase:F2}");
                Console.WriteLine($"Descuento     : S/ {descuento:F2}");
                Console.WriteLine($"IGV (18%)     : S/ {igv:F2}");
                Console.WriteLine($"Total a Pagar : S/ {totalPagar:F2}");
                Console.WriteLine($"\nRegistro guardado en atenciones.txt: {registroPipe}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al registrar la atención: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\nPresione una tecla para volver al menú principal...");
                Console.ReadKey();
            }
        }

        static void BuscarAtencionPorPlaca()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- 2. BUSCAR ATENCIÓN POR PLACA ---");
                Console.Write("Ingrese la placa a buscar: ");
                string placaBuscada = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(placaBuscada))
                    throw new ArgumentException("Debe ingresar una placa para realizar la búsqueda.");

                ModuloArchivo.BuscarAtencion(placaBuscada);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError durante la búsqueda: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\nPresione una tecla para volver al menú principal...");
                Console.ReadKey();
            }
        }

        static void ReporteGeneralAtenciones()
        {
            try
            {
                Console.Clear();
                ModuloArchivo.ListarTodasLasAtenciones();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al generar el reporte: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\nPresione una tecla para volver al menú principal...");
                Console.ReadKey();
            }
        }
    }
}
