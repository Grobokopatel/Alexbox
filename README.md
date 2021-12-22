# AlexBox
# Участники проекта
  1. Усов Егор
  2. Нестеров Дмитрий
  3. Пухир Михаил
  4. Миков Кирилл
# Решаемая проблема
Если вы любитель играть в настольные игры с друзьями, но боязнь коронавируса останавливает вас от любимого занятия, то наш проект это решение вашей проблемы.
# Основной сценарий использования
Компания людей с ноутбуками хочет весело провести время, скачивает нашу игру и играет
# Компоненты
## Пользовательский интерфейс
  - Формы `Windows forms`
## Домен
  - Сущность игры `GameBase`
  - Сущность игрока `Player`
## Инфраструктура
  - интерфейс `IGamePage`, представляет сущность игровой страницы
  - интерфейс `IGame`, представляет сущность игры
# Точки расширения
  1. Создавая объекты класса CustomGame можно создавать новые игры, в стиле вопрос-ответ.
  2. Можно легко менять дизайн страниц.
