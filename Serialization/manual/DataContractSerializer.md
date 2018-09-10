﻿_______________________________________________________________________________
# Сериализатор контрактов данных
_______________________________________________________________________________

#### Использование: ####

1. Выбрать класс сериализатор – ```DataContractSerializer``` или
```NetDataContractSerializer```:
* ```DataContractSerializer``` обеспечивает слабую привязку типов .NET к типам
контрактов данных, поэтому требуется предварительная явная регистрация
сериализируемых подтипов, для сопоставления имени контракта данных с
корректным типом .NET.
* ```DataContractSerializer``` предохраняет эквивалентность ссылок только по
требованию.
* ```NetDataContractSerializer``` осуществляет тесную привязку типов .NET к типам
контрактов данных, записывая полные имена типов и сборок для сериализируемых
типов, не требуется пререгистрация
(подобный вывод является патентованным, следовательно, при десериализации он
также полагается на наличие определённого типа .NET в конкретном пространстве
имён и сборке).
* ```NetDataContractSerializer``` всегда предохраняет эквивалентность ссылок.

2. Декорировать типы и члены, подлежащие сериализации, атрибутами ```DataContract```
и ```DataMember``` соответственно (это установит тип в неявно сериализуемый).

3. Создать экземпляр сериализатора и вызвать его метод ```WriteObject``` или
```ReadObject``` для явной сериализации и десериализации:
```c#
using System.IO;
using System.Runtime.Serialization;
using static System.Console;

class Program
{
    static void Main()
    {
        var person = new Person { Name = "Alexander", Age = 32 };
        var dcs = new DataContractSerializer(typeof(Person));
        using (var stream = File.Create("person.xml"))
            dcs.WriteObject(stream, person); // сериализировать

        Person p;
        using (var stream = File.OpenRead("person.xml"))
            p = (Person)dcs.ReadObject(stream); // десериализировать

        WriteLine("{0} {1}", p.Name, p.Age); // Alexander 32
    }
}

[DataContract]
public class Person
{
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public int Age { get; set; }
}
```
*конструктор ```DataContractSerializer``` (в отличие от ```NetDataContractSerializer```)
требует указания типа корневого объекта, который явно сериализируется, в данном
примере – ```Person```
_______________________________________________________________________________
# Работа с сериализатором
_______________________________________________________________________________

Сериализаторы ```DataContractSerializer``` и ```NetDataContractSerializer``` по умолчанию
примениют форматер XML.

Работая с классом ```XmlWriter``` можно добавить в вывод отступы для лучшей
читаемости:
```c#
var person = new Person { Name = "Alexander", Age = 32 };
var dcs = new System.Runtime.Serialization.DataContractSerializer(typeof(Person));

var settings = new System.Xml.XmlWriterSettings() { Indent = true };
using (var writer = System.Xml.XmlWriter.Create("person.xml", settings))
    dcs.WriteObject(writer, person); // сериализировать

System.Diagnostics.Process.Start("person.xml");
/* Output:
<?xml version="1.0" encoding="ISO-8859-1"?>
<Person xmlns="http://schemas.datacontract.org/2004/07/"
        xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
    <Age>32</Age>
    <Name>Alexander</Name>
</Person>
*/
```

Где имя XML-элемента ```<Person>``` есть имя контракта данных, по умолчанию
соответствующее имени типа .NET, можно переопределить явно задав имя контракта
данных:
```c#
[DataContract(Name = "Candidate")]
public class Person { ... }
```

Пространство имён XML отражает пространство имён контракта данных
(http://schemas.datacontract.org/2004/07/), а также пространство имён типа .NET.
Можно переопределять в известной форме:
```c#
[DataContract(Namespace = "http://.../...")]
public class Person { ... }
```
*указание имени и пространства имён отменяет связь между идентичностью
контракта и именем типа .NET, гарантируя что сериализация не будет затронута,
при изменении имени или пространства имён

Переопределяются имена данных-членов:
```c#
[DataContract(Name = "Candidate", Namespace = "http://.../...")]
public class Person
{
    [DataMember(Name = "FirstName")]
    public string Name { get; set; }
    [DataMember(Name = "ClaimedAge")]
    public int Age { get; set; }
}
```