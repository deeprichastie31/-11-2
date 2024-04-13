using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace лаба_11_задание_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random random = new Random();
        bool iscollapsed = true;
        string god;
        struct Goods
        {
            public string name;
            public string nameOfShop;
            public int price;

            public override string ToString()
            {
                return $"{name} - {nameOfShop} - {price}";
            }
        }
        string[] names = new string[13] { "Milk", "Eggs", "Bananas", "Chicken", "Oatmeal", "Pasta", "Backwheat", "Liquid", "Water", "Curd", "Apple", "Grape", "Bread" };
        string[] nameOfShops = new string[5] { "Lenta", "Metro", "Yarche!", "Pyatorochka", "Magnit" }; // нет смысла здесь держать списки, если они все равно не будут расширяться 
        // price будет генерироваться с помощью метода random;
        List<Goods> goods = new List<Goods>();
        string path = @"C:\\Users\\79521\\OneDrive\\Рабочий стол\\Для учебы\\ВКП\\лабораторная работа ОП 1\\11 лаба\resource.txt";
        private void Generation()
        {
            for (int i = 0; i < 10; i++)// можно отдельным перменным присвоить рандомные значения каждого из массивов, которые содержат информацию о наименовании продукта, имени магазина и стоимости товара, а потом 
                                        // в список с типом данных Goods, в котором 3 переменные, присвоить их объекту структуры, т.е создать ее, а потом добавить в заранее созданный список, содержащий объекты данной структуры
            {
                string nameOfGoods = names[random.Next(names.Length)];
                string nameOfshop = nameOfShops[random.Next(nameOfShops.Length)];
                int price = random.Next(50, 1000);
                // все переменные выше образуют одну позицию товара, включая имя товара, название магазина, в котором он есть, и цену за этот товар в этом магазине 
                Goods posistion = new Goods { name = nameOfGoods, nameOfShop = nameOfshop, price = price }; // Создан объект структуры Goods, в котором находится одна позиция. Теперь этот объект можно добавить в список, который как раз принимает эти объекты
                // для этого обязательно было создавать новый объект структуры
                goods.Add(posistion);
            }
            
            using (var write = new StreamWriter(path, false)) // false - потому что мне не нужно каждый раз добавлять в файл новые данные, которые по сути могут совпадать
            {
                for (int i = 0; i < goods.Count; i++)
                {
                    write.WriteLine(goods[i]);
                }
            }
            using (var sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    listBox1.Items.Add(sr.ReadLine()); // позовляет добавить элемент с листбокс
                }
            }
            
        }

        private void SortInNames(List<Goods> goods)
        {
            listBox2.Items.Clear();
            IEnumerable<Goods> sortlist =
            from names in goods
            orderby names.name //"ascending" is default
            select names;
            foreach (Goods name in sortlist)
            {
                listBox2.Items.Add(name);
            }
        }
        private void SortInPrice(List<Goods> goods) // данный метод сортировки спокойно подходит и для числовых значений
        {
            listBox2.Items.Clear();
            IEnumerable <Goods> sortlist =
                from price in goods
                orderby price.price 
                select price;
            foreach (Goods name in sortlist)
            {
                listBox2.Items.Add(name);
            }
        }
        private bool Search(string god, List<Goods> goods)
        {
            int count = 0;
            listBox2.Items.Clear();
            foreach (Goods name in goods) 
            {
                if (name.name == god) 
                {
                    listBox2.Items.Add(name);
                    count++;
                }
            }
            if (count == 0) 
            {
                listBox2.Items.Clear();
                MessageBox.Show("Такого нет в списках");
                return false;
            }
            return true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            listBox1.Items.Clear();
            Generation();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (iscollapsed)
            {
                sorting.Height += 10;
                if (sorting.Size == sorting.MaximumSize)
                {
                    timer1.Stop();
                    iscollapsed = false;
                }
            }
            else
            {
                sorting.Height -= 10;
                if (sorting.Size == sorting.MinimumSize)
                {
                    timer1.Stop();
                    iscollapsed = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button3_Click(object sender, EventArgs e) // price
        {
            SortInPrice(goods);
        }

        private void namess_Click(object sender, EventArgs e)
        {
            SortInNames(goods);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void Searh_Click(object sender, EventArgs e)
        {
            god = textBox1.Text;
            bool finder = Search(god, goods);
            if (!finder)
            {
                Thread.Sleep(2000);
                listBox2.Items.Clear();
                textBox1.Text = null;
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
            listBox2.Items.Clear();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string generateName = "abcdefghijklmnopqrstuvwxyz";
            string nameOfFile = null;
            if (listBox2 != null) 
            {
                for (int i = 0; i < 5; i++)
                {
                    nameOfFile += Convert.ToString(generateName[random.Next(generateName.Length)]);
                }
                using(var file = new StreamWriter($@"C:\Users\79521\OneDrive\Рабочий стол\Для учебы\ВКП\лабораторная работа ОП 1\11 лаба\{nameOfFile}.txt"))
                {
                    foreach (Goods item in listBox2.Items)
                    {
                        file.WriteLine(item);
                    }
                }
                
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
