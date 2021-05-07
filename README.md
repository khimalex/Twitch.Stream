﻿### Проект для получения m3u8-плейлистов сервиса Twitch.

Для сборки проекта необходимо выполнить команду PowerShell:

```powershell
.\build.ps1 --target Publish
```


Произойдёт следующее:

1. Будет скачан и установлен .Net Core SDK если его нет в ОС.
2. Будет произведена компиляция и сборка проекта в exe-файлы для `win-x64` и `win-x86` систем.  

 
Расположение получившихся ехе-файлов в директориях `/output/win-x64` или `/output/win-x86`.

Для получения описания доступных функций проекта нужно набрать в консоли или PowerShell команду:
```powershell 
.\Twitch.Stream.exe -h
```

Если просто запустить `Twitch.Stream.exe`, двойным кликом, то буду скачаны плейлисты (файлы с расширением `.m3u8`) для идущих на данный момент стримов из списка заданного в appsettings.json

Плейлисты для стримов которые перезапускались стримерами необходимо будет перекачивать заново - т.е. ещё раз запустить `Twitch.Stream.exe`.

Плейлисты будут сохранены в директории расположения `Twitch.Stream.exe`, называться полученные файлы будут по имени канала, например: `visualstudio.m3u8`.

Файлы с расширением m3u8 можно просматривать с помощью любого видеопроигрывателя, например, [MPC-HC](https://mpc-hc.org/).
