# E-mail redirector

This is a project made to redirect e-mails from AWS SES to your mailbox.

SES is acronym for Simple E-mail Service. When you configure the e-mail
boxes you want to use, you can use SNS (Simple Notification Service) to
redirect it to another mailbox, for example, your personal e-mail.

To implement some rules in the e-mail redirecting, I preffered to create
this project. Originally, it was inside [Don't fly Money project]. But
with received its own repository because:

- I'm using it for more than DfM project;
- It can be made generic so another people can use.

## AWS Techonologies used

- SES: to receive and send e-mails;
- Lambda: to upload this code to handle the e-mails;
- S3: where the SES store the e-mails that arrive;

## Eml Reader

I tried to find a C# library to read Eml files, but couldn't. So I
created one at [DK library]. It is available at [Nuget Package Manager].

## Upload to lamda

There is a Makefile at the root of the project to build the project and
create the zip to upload to lambda. You also can use Visual Studio to
build a Release version of the project and zip it manually checking the
logic inside the Makefile, or even use Visual Studio to build and the
Makefile to create the zip only.

To use Makefile for every step, run at the root:

```
make all
```

If you only want to zip:

```
make create-zip
```

Given I configured it some time ago, if you need help to configure all
the stuff at AWS, [open an issue at this repository] and we can make it
together. When this happen, I will be able to add all the instructions
here, so the next person will have it.

[Don't fly Money project]: https://github.com/darakeon/dfm
[DK library]: https://github.com/darakeon/dk-lib
[Nuget Package Manager]: https://www.nuget.org/packages/Keon.Eml/
[open an issue at this repository]: ../../issues
