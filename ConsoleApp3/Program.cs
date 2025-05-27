using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace cajero_hecho_por_mi
{
    public enum Menu
    {
        Consultar = 1, Deposito, Retiro, HistorialDepositos, HistorialRetiros, salir, DepositosoRetiros
    }
    class Program
    {
        static double saldo = 0;
        static Dictionary<DateTime, double> historialDepositos = new Dictionary<DateTime, double>();
        static Dictionary<DateTime, double> historialRetiros = new Dictionary<DateTime, double>();
        static void Main(string[] args)
        {

            int intentos = 3;
            do
            {
                if (loggin())
                {
                    Console.WriteLine("Bienvenido a tu banco del Bienestar");
                    while (true)
                    {
                        switch (MostrarMenu())
                        {
                            case Menu.Consultar:
                                Console.WriteLine($"Tu saldo es: {saldo}");
                                break;
                            case Menu.Deposito:
                                Depositar();
                                break;
                            case Menu.Retiro:
                                Retirar();
                                break;
                            case Menu.DepositosoRetiros:
                                break;
                            case Menu.HistorialDepositos:
                                Console.WriteLine("Historial de depósitos");
                                if (historialDepositos.Count == 0)
                                {
                                    Console.WriteLine("No hay depósitos registrados");
                                }
                                else
                                    foreach (var d in historialDepositos)
                                    {
                                        Console.WriteLine($"{d.Key}: +${d.Value}");
                                    }
                                break;
                            case Menu.HistorialRetiros:
                                Console.WriteLine("Historial de retiros");
                                if (historialRetiros.Count == 0)
                                {
                                    Console.WriteLine("No hay retiros registrados");
                                }
                                else
                                    foreach (var r in historialRetiros)
                                    {
                                        Console.WriteLine($"{r.Key}: -${r.Value}");
                                    }
                                break;
                            case Menu.salir:
                                Console.WriteLine("Gracias por usar el cajero.");
                                return;
                        }

                        Console.Write("¿Deseas hacer otra operación? (s/n): ");
                        string respuesta = Console.ReadLine().ToLower();
                        if (respuesta != "s") break;
                    }
                    break;
                }
                else
                {
                    intentos--;
                    Console.WriteLine($"Credenciales incorrectas. Te quedan {intentos} intento(s).");
                }
            } while (intentos > 0);

            if (intentos == 0)
            {
                Console.WriteLine("Has excedido el número de intentos.");
            }
        }

        static void Depositar()
        {
            Console.Write("¿Cuánto vas a depositar? ");
            double dep = Convert.ToDouble(Console.ReadLine());
            saldo += dep;
            historialDepositos[DateTime.Now] = dep;
            Console.WriteLine("Depósito exitoso.");
        }

        static void Retirar()
        {
            Console.Write("¿Cuánto deseas retirar? ");
            if (double.TryParse(Console.ReadLine(), out double ret) && ret > 0)
            {
                if (ret <= saldo)
                {
                    saldo -= ret;
                    historialRetiros[DateTime.Now] = ret;
                    Console.WriteLine("Retiro exitoso.");

                    Dictionary<int, string> tareas = new Dictionary<int, string>
            {
                { 1, $"{DateTime.Now}: Retiro de ${ret}" }
            };
                    enviarCorreo(tareas);
                }
                else
                {
                    Console.WriteLine("Saldo insuficiente.");
                }
            }
            else
            {
                Console.WriteLine("Cantidad inválida.");
            }
        }

        static void DepositosoRetiros()
        {
            if (historialDepositos.Count > 0)
        }
        Console.WriteLine($"Cantidad de depósitos: {historialDepositos.Count}");
        double totalDepositos = 0;
        foreach (var d in historialDepositos.Values)
        {
        totalDepositos += d;
        }
        Console.WriteLine($"Total depositado: ${totalDepositos}");
        {
        else
        }
         Console.WriteLine("No se han realizado depósitos.");
        {
        if (historialRetiros.Count > 0)
        {
         Console.WriteLine($"Cantidad de retiros: {historialRetiros.Count}");
         double totalRetiros = 0;
         foreach (var r in historialRetiros.Values)
        }
        totalRetiros += r;
        
         Console.WriteLine($"Total retirado: ${totalRetiros}");
        {
        else
        {
         Console.WriteLine("No se han realizado retiros.");
        }



        static bool loggin()
        {
            Console.WriteLine("Dame tu user");
            string user = Console.ReadLine();
            Console.WriteLine("dame tu password");
            string password = Console.ReadLine();
            Console.WriteLine("Dame tu fecha nacimiento dd/MM/yyyy");
            DateTime fecha = Convert.ToDateTime(Console.ReadLine());
            DateTime fechaactual = DateTime.Now;
            int anios = fechaactual.Year - fecha.Year;
            if (fechaactual < fecha.AddYears(anios)) anios--;

            if (anios < 18)
            {
                Console.WriteLine("Debes ser mayor de edad para usar el cajero.");
                return false;
            }

            if (user == "joshua" && password == "1234")
                return true;
            else
                return false;
        }

        static Menu MostrarMenu()
        {
            Console.WriteLine("--- MENÚ ---");
            Console.WriteLine("1 Consultar saldo");
            Console.WriteLine("2 Depositar");
            Console.WriteLine("3 Retirar");
            Console.WriteLine("4 Revisar historial de depositos");
            Console.WriteLine("5 Revisar historial de retiros");
            Console.WriteLine("6 Salir");
            Console.WriteLine("7 Depositos o Retiros");
            Console.Write("Selecciona una opción:");

            if (int.TryParse(Console.ReadLine(), out int opcion) && Enum.IsDefined(typeof(Menu), opcion))
            {
                return (Menu)opcion;
            }
            else
            {
                Console.WriteLine("Opción inválida. Intenta de nuevo.");
                return MostrarMenu();
            }
        }


        static bool enviarCorreo(Dictionary<int, string> tareaspendientes)
        {
            string servidorSmtp = "smtp.office365.com";//OFFICE CORREO
            int puerto = 587;//PUERTO
            string usuario = "113340@alumnouninter.mx";  // Tu correo de Office 365
            string contrasena = "Paladins1234";  // Tu contraseña de Office 365

            // Crear el cliente SMTP
            SmtpClient smtp = new SmtpClient(servidorSmtp)
            {
                Port = puerto,
                Credentials = new NetworkCredential(usuario, contrasena),
                EnableSsl = true
            };
            // Crear el mensaje de correo
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(usuario),  // Remitente
                Subject = "Tareas Pendientes",   // Asunto del correo
                IsBodyHtml = false  // El cuerpo no es HTML
            };

            // Construir el cuerpo del mensaje con las tareas pendientes
            string cuerpoMensaje = "Las tareas pendientes son:\n\n";
            foreach (var tarea in tareaspendientes)
            {
                cuerpoMensaje += $"{tarea.Key}. {tarea.Value}\n";
            }
            mail.Body = cuerpoMensaje;  // Asignar el cuerpo del mensaje

            mail.To.Add("joshuamendozabobadilla@gmail.com"); // Agregar destinatario

            // Enviar el correo
            smtp.Send(mail);
            return true;
        }
    }
}
