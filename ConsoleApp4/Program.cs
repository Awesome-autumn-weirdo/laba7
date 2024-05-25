using System;

namespace ConsoleApp4
{
    class Program
    {
        static void Main()
        {
            IAnalogClock analogClock = new AnalogClock();
            IDigitalClock digitalClock = new ClockAdapter(analogClock);

            Console.WriteLine("Введите время в формате HH:MM");
            string inputTime = Console.ReadLine();

            try
            {
                digitalClock.SetTime(inputTime);

                string currentTime = digitalClock.GetTime();
                var (hourAngle, minuteAngle) = analogClock.GetHands();

                Console.WriteLine($"Углы поворота стрелок: часовая - {hourAngle} градусов, минутная - {minuteAngle} градусов");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}

