# Proyecto de ejemplo Webpay para Transbank SDK .NET

## Requerimientos

Para poder ejecutar el proyecto de ejemplo necesitas tener instalada las siguientes herramientas
en tu computador:

1. git ([como instalar git][git_install])
2. Visual Studio ([instalar VisualStudio 2017][visualstudio_install])

[git_install]: https://git-scm.com/book/en/v2/Getting-Started-Installing-Git
[visualstudio_install]: https://visualstudio.microsoft.com/es/downloads/

## Clonar repositorio

Primero deberás clonar este repositorio en tu computador:

````batch
git clone https://github.com/TransbankDevelopers/transbank-sdk-dotnet-webpay-example.git
````


## Ejecutar ejemplo

El ejemplo viene listo para correr usando Visual Studio:

1. Abrir la solución `TransbankWebpayExample.sln` con Visual Studio
2. Hacer clic derecho sobre el proyecto `WebpayASPNetExample`.
3. Seleccionar la opción Establecer como proyecto de inicio.
4. Clic derecho nuevamente sobre el proyecto `WebpayASPNetExample` y seleccionar la opción Compilar.
5. Iniciar la aplicación (`ctrl + F5`) `Menú` -> `Debug` -> `Iniciar sin debug`

Si todo ha salido bien deberías poder acceder al ejemplo en la URL  `http://localhost:54128/` y probar los distintos productos de Webpay en ambiente de pruebas. Para hacer pruebas en dicho ambiente debes usar los siguientes datos:

- Tarjeta: VISA
- Número: 4051885600446623
- Fecha de Expiración: Cualquiera
- CVV: 123
- RUT autenticación con emisor: 11.111.111-1
- Contraseña autenticación con emisor: 123
