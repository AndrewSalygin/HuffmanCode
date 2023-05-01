using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HuffmanCode
{
    class Program
    {
        public static void codingFile()
        {
            Dictionary<char, int> dictOfNodes = new Dictionary<char, int>();
            Dictionary<char, string> table = new Dictionary<char, string>();

            int chInt;
            char ch;
            using (StreamReader fileIn = new StreamReader("input.txt"))
            {
                //читаем поток посимвольно, до тех пор пока он не пуст
                while ((chInt = fileIn.Read()) != -1)
                {
                    // заносим символы в словарь
                    ch = (char)chInt;
                    if (dictOfNodes.ContainsKey(ch))
                        dictOfNodes[ch]++;
                    else
                        dictOfNodes.Add(ch, 1);
                }
            }

            SortedSet<Node> nodes = new SortedSet<Node>();
            foreach (var value in dictOfNodes)
            {
                nodes.Add(new Node(value.Key, 0, value.Value));
            }

            var enumerator = nodes.GetEnumerator();
            while (nodes.Count > 1)
            {
                enumerator.MoveNext();
                Node leftChild = enumerator.Current;
                enumerator.MoveNext();
                Node rightChild = enumerator.Current;
                Node subRoot = Node.joinSubtree(leftChild, rightChild);

                nodes.Remove(leftChild);
                nodes.Remove(rightChild);

                nodes.Add(subRoot);
                enumerator = nodes.GetEnumerator();
            }

            enumerator = nodes.GetEnumerator();
            enumerator.MoveNext();
            Node root = enumerator.Current;

            StringBuilder code = new StringBuilder();

            // создание таблицы
            Node.PreorderSetCode(root, ref code, ref table);

            StringBuilder strToPrint = new StringBuilder();

            // записываем количество нулей
            strToPrint.Append("000");

            // мощность алфавита
            strToPrint.Append(Convert.ToString(table.Count, 2).PadLeft(8, '0'));

            foreach (var val in table)
            {
                strToPrint.Append(Convert.ToString(val.Key, 2).PadLeft(16, '0'));
                strToPrint.Append(Convert.ToString(val.Value.Length, 2).PadLeft(4, '0'));
                strToPrint.Append(val.Value);
            }

            using (StreamReader fileIn = new StreamReader("input.txt"))
            {
                //читаем поток посимвольно, до тех пор пока он не пуст
                while ((chInt = fileIn.Read()) != -1)
                {
                    ch = (char)chInt;
                    strToPrint.Append(table[ch]);
                }

            }

            byte zeros = (byte)(8 - strToPrint.Length % 8);
            if (zeros % 8 != 0)
            {
                strToPrint.Append('0', zeros);
                strToPrint.Replace("000", Convert.ToString(zeros, 2), 0, 3);
            }
            using (BinaryWriter binWriter = new BinaryWriter(File.Open("output.slg", FileMode.Create)))
            {
                for (int i = 0; i < strToPrint.Length; i += 8)
                {
                    binWriter.Write(Convert.ToByte(strToPrint.ToString(i, 8), 2));
                }
            }
        }

        public static void decodingFile()
        {
            byte readByte;
            char ch;
            byte chLength;
            string code;

            StringBuilder str = new StringBuilder();

            using (BinaryReader binReader = new BinaryReader(File.Open("output.slg", FileMode.Open), Encoding.ASCII))
            {
                while (binReader.PeekChar() != -1)
                {
                    readByte = binReader.ReadByte();
                    str.Append(Convert.ToString(readByte, 2).PadLeft(8, '0'));
                }

                byte countOfZeros = Convert.ToByte(str.ToString(0, 3), 2);
                str.Length -= countOfZeros;

                UInt16 powerOfAlphbt = Convert.ToUInt16(str.ToString(3, 8), 2);

                Dictionary<string, char> table = new Dictionary<string, char>(powerOfAlphbt);

                int pos = 11;

                for (int i = 0; i < powerOfAlphbt; i++)
                {
                    ch = (char)Convert.ToInt16(str.ToString(pos, 16), 2);
                    pos += 16;
                    chLength = Convert.ToByte(str.ToString(pos, 4), 2);
                    pos += 4;
                    code = str.ToString(pos, chLength);
                    pos += chLength;
                    table.Add(code, ch);
                }

                StringBuilder buffer = new StringBuilder();
                StringBuilder toOutput = new StringBuilder();

                while (pos < str.Length)
                {
                    while (!table.ContainsKey(buffer.ToString()))
                    {
                        buffer.Append(str[pos]);
                        pos++;
                    }

                    toOutput.Append(table[buffer.ToString()]);
                    buffer.Clear();
                }

                using (StreamWriter fileOut = new StreamWriter("output.txt", false))
                {
                    fileOut.Write(toOutput);
                }
            }
        }

        static void Main(string[] args)
        {
            byte mode;

            Console.WriteLine("Введите (0 - закодировать, 1 - декодировать):");
            mode = Byte.Parse(Console.ReadLine());
            try
            {
                switch (mode)
                {
                    case 0:
                        codingFile();
                        break;
                    case 1:
                        decodingFile();
                        break;
                    default: throw new modeException("Ошибка: выбран неправильный тип работы программы.");
                }
            }
            catch (modeException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}