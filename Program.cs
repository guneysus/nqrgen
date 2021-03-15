using CommandLine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using static ZXing.Rendering.SvgRenderer;

namespace nqrgen
{
    internal class Program
    {
        static void Main(string[] args)
        {

#if DEBUG
            Console.WriteLine("Waiting for debugger");
            do
            {
                Thread.Sleep(100);
            } while (!System.Diagnostics.Debugger.IsAttached);
            Console.WriteLine("Debugger attached");
#endif

            Parser.Default.ParseArguments<Options>(args)
                .WithNotParsed(errors => {
                    foreach (var err in errors)
                    {
                        Console.Error.WriteLine(err);
                    }
                })
                .WithParsed(options =>
                {
                    if (Console.IsInputRedirected)
                    {
                        foreach (var line in readLines())
                        {
                            switch (options.RenderType)
                            {
                                case RenderType.Console:
                                    {
                                        var writer = QR.createBarcodeWriterForConsole();
                                        _ = writer.Write(line);
                                        break;
                                    }
                                case RenderType.File:
                                    {
                                        var task = QR.New(line, options);
                                        task.Process();
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                    } else
                    {
                        switch (options.RenderType)
                        {
                            case RenderType.Console:
                                {
                                    var writer = QR.createBarcodeWriterForConsole();
                                    _ = writer.Write(options.Content);
                                    break;
                                }
                            case RenderType.File:
                                {
                                    var task = QR.New(options.Content, options);
                                    task.Process();
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                });
        }

        public static IEnumerable<string> readLines()
        {
            string line;

            if (Console.IsInputRedirected)
            {
                using var stream = Console.OpenStandardInput();
                using var sr = new StreamReader(stream);
                do
                {
                    if (sr.Peek() == 0) break;

                    line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) break;

                    yield return line;
                } while (true);
            }
        }
    }

}
