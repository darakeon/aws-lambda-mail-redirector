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

When the project is build, it creates a file called lambda.zip at the
source root folder. You need to update this zip to your lambda so you
can use it.

Given I configured it some time ago, if you need help to configure all
the stuff at AWS, [open an issue at this repository] and we can make it
together. When this happen, I will be able to add all the instructions
here, so the next person will have it.

[Don't fly Money project]: https://github.com/darakeon/dfm
[DK library]: https://github.com/darakeon/dk-lib
[Nuget Package Manager]: https://www.nuget.org/packages/Keon.Eml/
[open an issue at this repository]: ../../issues
