﻿using static System.Console;

class Program
{
    static void Main()
    {
        int[] array = new int[3] { 10, 20, 30 };
        unsafe
        {
            // необходимо закрепить объект в динамической памяти (куче)
            // чтобы он не перемещался при использовании внутренних указателей
            fixed (int* p = &array[0])
            {
                // p инициализирована в fixed - только для чтения
                // необхоимо создать второй указатель для его инкремента
                int* p2 = p;
                WriteLine("*p2 @ {0}", *p2); // 10

                // инкремент p2 увеличивает внутренний указатель на 4 байта для его типа
                p2++;
                WriteLine("*p2 @ {0}", *p2); // 20
                p2++;
                WriteLine("*p2 @ {0}", *p2); // 30


                WriteLine("*p @ {0}", *p);   // 10

                // доступ через указатель на изменение значения array[0]
                *p += 1;
                WriteLine("*p @ {0}", *p);   // 11
                *p += 1;
                WriteLine("*p @ {0}", *p);   // 12
            }
        }
        Write("array[0] @ {0}", array[0]);   // 12
        // *p2 @ 10
        // *p2 @ 20
        // *p2 @ 30
        // *p @ 10
        // *p @ 11
        // *p @ 12
        // array[0] @ 12
    }
}
