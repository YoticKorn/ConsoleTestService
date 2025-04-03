using Korn.Pipes;
using System;
using System.Threading.Tasks;

static class TestPipes
{
    public static void Execute()
    {
        var configuration = new PipeConfiguration("pipes_test");

        Task.Run(Task1);
        Task.Run(Task2);
        Task.Delay(100000).Wait();

        void Task1()
        {
            var output = new OutputPipe(configuration);

            Task.Delay(100000).Wait();
        }

        void Task2()
        {
            using (var input = new InputPipe(configuration))
            {
                for (var i = 1; i < 10; i++)
                {
                    var bytes = new byte[i];
                    input.Send(bytes);
                    Task.Delay(10).Wait();
                }

                Console.ReadLine();
            }

            Task.Run(Task2);
        }
    }
}