using System;

public class TaskStr  // Переименуем класс, чтобы избежать конфликта с ключевым словом C#
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;  // Обновим стиль свойств
    public bool IsCompleted { get; set; }

    public int Position { get; set; } = 0;
}
