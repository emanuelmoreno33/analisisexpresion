using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data;

namespace Detectarvalores
{
    class Program
    {
        static void Main(string[] args)
        {
            char letra;
            do
            {
                Console.WriteLine("Ingrese operacion matematica sin parentesis");
                string operacion = Console.ReadLine();


                //ingreso letras
                Regex letras = new Regex(@"[^0-9,'+','\-','*','/']+");
                //ingreso simbolos duplicados
                Regex duplicados = new Regex(@"['+','\-','*','/']{2}");
                //termino con simbolo
                Regex terminacionmal = new Regex(@"['+',-.*,/]$");
                //Inicia con simbolo
                Regex iniciamal = new Regex(@"^['+',-.*,/]");
                Match encontrado = letras.Match(operacion);
                Match duplica = duplicados.Match(operacion);
                Match nofin = terminacionmal.Match(operacion);
                Match noinicio = iniciamal.Match(operacion);

                if(operacion==""||operacion==null)
                {
                    Console.WriteLine("Escribio un enter sin contenido, no es valido");
                }
                else if(nofin.Success)
                {
                    Console.WriteLine("Termina en simbolo, no es valido");
                }
                else if (noinicio.Success)
                {
                    Console.WriteLine("Inicia con simbolo, no es valido");
                }
                else if (encontrado.Success)
                {
                    Console.WriteLine("contiene caracteres no validas: ' " + encontrado.Value + " '");
                }
                else if (duplica.Success)
                {
                    Console.WriteLine("contiene simbolos operacionales duplicados: '" + duplica.Value + " '");
                }
                else
                {
                    //hace la operacion, no es necesario
                    //double result = Convert.ToDouble(new DataTable().Compute(operacion, null));
                    //Console.WriteLine("Resultado = " + result.ToString());

                    Regex num = new Regex(@"\d+");
                    Regex rex = new Regex(@"['+','\-','/','*']+");
                    string[] valores;
                    string[,] simb;

                    MatchCollection numero = num.Matches(operacion.ToString());
                    valores = new string[numero.Count + 1];

                    MatchCollection simbolo = rex.Matches(operacion.ToString());
                    simb = new string[numero.Count + 1,2];

                    int contadornum = 0;
                    int contadorsimbolos = 0;

                    Console.Write("Los operandos son:");

                    foreach (Match item in numero)
                    {
                        foreach (Capture cap in item.Captures)
                        {
                            valores[contadornum] = cap.Value;
                            Console.Write(valores[contadornum] + ",");
                            contadornum++;
                        }

                    }
                    Console.WriteLine();

                    Console.Write("Los operadores son:");

                    foreach (Match item in simbolo)
                    {
                        foreach (Capture cap in item.Captures)
                        {
                            simb[contadorsimbolos,0] = cap.Value;
                            simb[contadorsimbolos, 1] = "False";
                            Console.Write(simb[contadorsimbolos,0] + ",");
                            contadorsimbolos++;
                        }
                    }
                    Console.WriteLine("\nEl proceso es:");
                    string anterior = "";
                    int posicion = 0;
                    for (int i = 0; i < contadorsimbolos; i++)
                    {
                        if (i == 0 || anterior == "")
                        {
                            string primordial = validacion(simb[i, 0], simb[i + 1, 0]);
                            if (simb[i, 0] == simb[i + 1, 0])
                            {
                                Console.WriteLine(simb[i, 0]);
                                simb[i, 1] = "True";
                            }
                            else if (primordial == simb[i, 0])
                            {
                                Console.WriteLine(simb[i, 0]);
                                simb[i, 1] = "True";
                            }
                            else
                            {
                                Console.WriteLine(simb[i + 1, 0]);
                                simb[i + 1, 1] = "True";
                                anterior = simb[i, 0];
                                posicion = i;
                            }
                        }
                        else
                        {
                            string primordial = validacion(anterior, simb[i + 1, 0]);
                            if (posicion != 0&&simb[posicion - 1, 1] == "True" && simb[posicion + 1, 1] == "True")
                            {
                                Console.WriteLine(anterior);
                                simb[posicion, 1] = "True";
                                anterior = "";
                            }
                            else if (anterior == simb[i + 1, 0])
                            {
                                Console.WriteLine(anterior);
                                simb[posicion, 1] = "True";
                                anterior = "";
                            }
                            else if (primordial == anterior)
                            {
                                Console.WriteLine(anterior);
                                simb[posicion, 1] = "True";
                                anterior = "";
                            }
                            else if (simb[i + 1, 0] == null)
                            {
                                Console.WriteLine(simb[i, 0]);
                            }
                            else
                            {
                                Console.WriteLine(simb[i + 1,0]);
                            }
                        }
                        
                    }
                }
                Console.ReadKey();
                try
                {
                    Console.WriteLine("\n Desea continuar? (Y/N)");
                    letra = Convert.ToChar(Console.ReadLine());
                    if (letra != 'Y' && letra != 'y')
                    {
                        Console.WriteLine("Saliendo del programa, presione cualquier tecla");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ingresaste espacio, saliendo del programa");
                    letra = 'n';
                    Console.ReadKey();
                }
            } while (letra == 'Y' || letra == 'y');
        }

        static string validacion(string valor1, string valor2)
        {
            int posicion1 = 0;
            int posicion2 = 0;

                switch (valor1)
                {
                    case "-":
                        posicion1 = 1;
                        break;
                    case "+":
                        posicion1 = 2;
                        break;
                    case "/":
                        posicion1 = 3;
                        break;
                    case "*":
                        posicion1 = 4;
                        break;
                }
                switch (valor2)
                {
                    case "-":
                        posicion2 = 1;
                        break;
                    case "+":
                        posicion2 = 2;
                        break;
                    case "/":
                        posicion2 = 3;
                        break;
                    case "*":
                        posicion2 = 4;
                        break;
                }
                if (posicion1 > posicion2)
                {
                    return valor1.ToString();
                }
                else if (posicion2 > posicion1)
                {
                    return valor2.ToString();
                }
                return "";
            
        }
    }
}
