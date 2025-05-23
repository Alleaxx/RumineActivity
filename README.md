﻿# Анализ форумной активности Румине
Приложение визуализации серой и неприглядной статистики форумной активности сайта в прекрасные таблицы и диаграммы с разумной степенью настройки.

Дает уверенное представление об активности форума https://ru-minecraft.ru за всё время его существования с точностью до дня.

Доступно по следующему адресу: <https://alleaxxrmca.github.io/RumineActivity/>.

Инструкция по использованию находится там же, прямо на главной странице. А в меню можно найти список изменений последних версий.

![Диаграмма активности](https://i.ibb.co/B5g9L2Ds/NVIDIA-Overlay-6hwf2yw5l-U.gif)

## Возможности
- Хранение данных активности форума ru-minecraft.ru;
- Преобразование непонятных данных в читабельную статистику;
- Анализ полученной статистики;
    - Разделение по конкретным периодам;
    - Форматирование выводимых значений;
    - Визуализация с помощью таблиц, списков, гистограмм и графиков.
- Сравнение статистики разных периодов друг с другом

## Технологии
C#, .NET 8 (изначально .NET 5), Blazor WebAssembly и чуток SVG для диаграмм.

Обоснование выбора:
- C# + Blazor доступен сразу в браузере;
- Идея приложения хорошо ложится на интерактивно одностраничное приложение. Здесь нет необходимости в сервере, а вот продвинутый UI ой как пригодится.

### Проекты
- ```RumineActivity.Core``` - основная библиотека с алгоритмом расчета активности и общими объектами.
- ```RumineActivity.View``` - Blazor WASM приложение с визуализацией данных.
- ```RumineActivity.Console``` - вспомогательный проект для конвертации исходных данных в удобный для анализа вид.

## Зачем это нужно?
Информация по статистике нужна каждому, кто хоть раз оставлял сообщение на форуме ru-minecraft.ru.
Шутка.
И тем не менее посетителям сайта вполне интересно узнать активность форума, на который они тратят своё время и формируют своими сообщениями ту самую активность. Автору приложения это тоже интересно + кайф разбираться с циферками. Вот и ответ.

## Как работает
В обработанном файле данных хранится список форумных постов, у каждого указано два свойства:
- ID поста
- Дата написания

Этого вполне достаточно для анализа.

#### Расчет активности
ID форумного сообщения соответствует его порядковому номеру на форуме. Если из ID позднего поста вычесть ID более раннего, то получится общее количество написанных сообщений на *всем форуме* за прошедший период. Будут учтены даже удаленные сообщения.

Точность определения активности зависит от числа "пропущенных" сообщений, т.е. постов с неизвестной датой. Периодизация в один день как правило даёт идеальную точность, кроме совсем уж древних времён.
### Источник данных
В изначальной версии приложения данные по постам были некогда собраны и введены вручную (ужас-то какой!) в оригинальную эксель-таблицу, откуда они перекочевали в актуальную версию приложения.

Сейчас же все данные о постах получены куда более эффективным путём, который запросто уделывает по точности любые ручные Excel-таблицы (парсинг и не только). Так что оригинальная Excel-таблица окончательно перешла в разряд наследия.

## Предыстория
С 2016 года статистика по активности форума Румине базировалась в эксель-таблице. В ней хранились посчитанные вручную числа написанных постов в условном месяце, затем стали проводиться кое-какие базовые вычисления средствами экселя (на основе разницы ID двух постов). Там же строились и графики, затем они публиковались картинкой. В 2018 / 2019 была сделана веб-версия, которая представляла обычную html-таблицу с перенесенными значениями.

Эксель таблица обновлялась раз в несколько месяцев и почти никому не показывалась, веб-вариант не обновлялся вообще (ибо занятие весьма нудное). На том дело почти и кончилось.
![Часть исходной таблицы](https://i.ibb.co/NjD36kB/image.png)
В ходе работы над историческим симулятором Румине (2020+) появилось множество новых статистических данных, которые можно было бы использовать для автоматического расчета активности. Однако исходную экселевскую книгу для такого пришлось бы полностью переделывать, и всё равно бы остались ограничения с неудобствами. Появился хороший повод написать полноценное приложение для анализа и визуализации статистики, заодно предоставив возможность всем желающим посмотреть статистику в удобном, настраиваемом виде. Сказано - сделано, и вот мы здесь.

## Планы на будущее
- Статистика пиков активности по часам дня.
- Возможность сравнения статистики на одной гистограмме или на нескольких рядом.
- Добавление актуальной статистики по ходу её появления.
- Адаптация интерфейса под мобильные устройства по возможности.
- Использование цветовых переменных в стилях для возможной смены светлой / тёмной темы.
- ?...