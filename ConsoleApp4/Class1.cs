using System;

namespace ConsoleApp4
{
    // Интерфейс для цифровых часов
    public interface IDigitalClock
    {
        void SetTime(string time); // Метод для установки времени в цифровом формате HH:MM
        string GetTime(); // Метод для получения времени в цифровом формате HH:MM
    }

    // Интерфейс для аналоговых часов
    public interface IAnalogClock
    {
        void SetHands(double hourAngle, double minuteAngle); // Метод для установки углов поворота часовой и минутной стрелок
        (double HourAngle, double MinuteAngle) GetHands(); // Метод для получения углов поворота часовой и минутной стрелок
    }

    // Реализация аналоговых часов
    public class AnalogClock : IAnalogClock
    {
        private double hourAngle; // Угол поворота часовой стрелки
        private double minuteAngle; // Угол поворота минутной стрелки

        // Установка углов поворота стрелок
        public void SetHands(double hourAngle, double minuteAngle)
        {
            this.hourAngle = hourAngle; // Устанавливаем угол для часовой стрелки
            this.minuteAngle = minuteAngle; // Устанавливаем угол для минутной стрелки
        }

        // Получение текущих углов поворота стрелок
        public (double HourAngle, double MinuteAngle) GetHands()
        {
            return (hourAngle, minuteAngle); // Возвращаем текущие углы для часовой и минутной стрелок
        }
    }

    // Класс-адаптер, позволяющий использовать аналоговые часы как цифровые
    public class ClockAdapter : IDigitalClock
    {
        private readonly IAnalogClock analogClock;

        // Конструктор, принимающий объект аналоговых часов
        public ClockAdapter(IAnalogClock analogClock)
        {
            this.analogClock = analogClock;
        }

        // Установка времени в цифровом формате
        public void SetTime(string time)
        {
            // Разделение времени на часы и минуты
            var parts = time.Split(':');
            if (parts.Length != 2 || // Проверка, что формат времени корректен
                !int.TryParse(parts[0], out int hours) || // Проверка, что часы являются числом
                !int.TryParse(parts[1], out int minutes) || // Проверка, что минуты являются числом
                hours < 0 || hours > 23 || // Проверка, что часы находятся в допустимом диапазоне
                minutes < 0 || minutes > 59) // Проверка, что минуты находятся в допустимом диапазоне
            {
                throw new ArgumentException("Время должно быть в формате HH:MM и в допустимых пределах.");
            }

            // Вычисление угла поворота часовой стрелки
            double hourAngle = (hours % 12) * 30 + minutes * 0.5; // 30 градусов за каждый час + 0.5 градуса за каждую минуту

            // Вычисление угла поворота минутной стрелки
            double minuteAngle = minutes * 6; // 6 градусов за каждую минуту

            // Установка углов поворота стрелок на аналоговых часах
            analogClock.SetHands(hourAngle, minuteAngle);
        }

        // Получение времени в цифровом формате
        public string GetTime()
        {
            var (hourAngle, minuteAngle) = analogClock.GetHands();

            // Нормализуем углы
            hourAngle = hourAngle % 360;
            minuteAngle = minuteAngle % 360;

            // Вычисление времени
            int hours = (int)(hourAngle / 30);
            int minutes = (int)(minuteAngle / 6);

            // Корректировка времени для формата 12-часов
            if (hours == 0)
            {
                hours = 12;
            }

            return $"{hours:D2}:{minutes:D2}";
        }

    }
}
