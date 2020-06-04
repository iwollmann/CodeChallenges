using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using Xunit;

namespace LeadALife
{
    class PriorityQueue
    {
        private List<Node> _list = new List<Node>();

        public void Add(Node v)
        {
            if (v.gain < 0 || _list.Where(x => x.Equals(v)).Any()) return;

            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].gain < v.gain || (_list[i].gain == v.gain && _list[i].index > v.index))
                {
                    _list.Insert(i, v);
                    return;
                }
            }

            _list.Add(v);
        }

        public bool IsNotEmpty => _list.Count > 0;

        public Node Pop()
        {
            Node node = _list.First();
            _list.Remove(node);
            return node;
        }

        public Node Peak()
        {
            Node node = _list.First();
            return node;
        }
    }


    class Node
    {
        public int index;
        public int gain;
        public int energy;

        public Node(int i, int value, int e)
        {
            gain = value;
            index = i;
            energy = e;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Node))
                return false;

            return this.index == ((Node)obj).index &&
                    this.gain == ((Node)obj).gain &&
                    this.energy == ((Node)obj).energy;
        }

        public override int GetHashCode()
        {
            return this.index.GetHashCode() ^ this.energy.GetHashCode() ^ this.gain.GetHashCode();
        }
    }
    class LeadALife
    {
        public static int calculateProfit(int n, List<int> earning, List<int> cost, int e)
        {
            Func<int, int> work = (int earn) => e * earn;
            Func<int, int> eat = (int c) => -1 * (e * c);


            PriorityQueue queue = new PriorityQueue();

            Node bestNode = new Node(0, 0, e);

            queue.Add(bestNode);

            while (queue.IsNotEmpty)
            {
                Node node = queue.Pop();

                if (node.gain > bestNode.gain)
                    bestNode = node;

                if (node.index == n)
                    continue;

                var currentEarn = work(earning[node.index]);
                var currentEatCost = eat(cost[node.index]);

                if (bestNode.index == n && node.index < bestNode.index
                    && node.energy == 0)
                {
                    queue.Add(new Node(node.index + 1, node.gain + currentEatCost, e));
                    continue;
                }

                if (node.energy != 0)
                {
                    if (currentEarn >= currentEatCost)
                    {
                        queue.Add(new Node(node.index + 1, node.gain + currentEarn + currentEatCost, e)); //work and eat
                        queue.Add(new Node(node.index + 1, node.gain + currentEarn, 0)); // just work
                    }

                    queue.Add(new Node(node.index + 1, node.gain + 0, node.energy)); // do nothing
                }
                else
                {
                    queue.Add(new Node(node.index + 1, node.gain + currentEatCost, e)); // eat
                    queue.Add(new Node(node.index + 1, node.gain, node.energy)); // do nothing
                }
            }

            return bestNode.gain;
        }
    }

    //class Data
    //{
    //    public int e;
    //    public int m;

    //    public Data(int en, int mn)
    //    {
    //        e = en;
    //        m = mn;
    //    }

    //    public static Data Max(Data a, Data b)
    //    {
    //        if (a.m > b.m)
    //            return a;
    //        else
    //            return b;
    //    }
    //}

    //public class LeadALife
    //{
    //    public static int calculateProfit(int n, List<int> earning, List<int> cost, int start_energy)
    //    {
    //        var t = new Data[start_energy + 1, n + 1];

    //        for (var day = 0; day <= n; ++day)
    //        {
    //            for (var e = start_energy; e <= start_energy; ++e)
    //            {
    //                if (day == 0)
    //                {
    //                    t[e, day] = new Data(e, 0);
    //                    continue;
    //                }

    //                var last_best = t[e, day - 1];
    //                var money = last_best.m;
    //                var j = 1;

    //                var qtdFood = day < 2 ? 0 : Math.Min(start_energy, money / (cost[day - j - 1]));
    //                var costFood = day < 2 ? 0 : qtdFood * cost[day - j - 1];

    //                while (day - j - 1 >= 0 && earning[day - j - 1] <= cost[day - j - 1])
    //                {
    //                    var qtdFood2 = Math.Min(start_energy, money / (cost[day - j - 1]));
    //                    var costFood2 = qtdFood2 * cost[day - j - 1];
    //                    if (costFood2 < costFood) {
    //                        qtdFood = qtdFood2;
    //                        costFood = costFood2;
    //                    }

    //                    j++;
    //                }

    //                //if (day - j - 1 <= 0)
    //                //{
    //                //    t[e, day] = Data.Max(last_best, new Data(0, e * earning[day - 1]));
    //                //    continue;
    //                //};

    //                //var max_food_yesterday = Math.Min(start_energy, money / costX);
    //                var change = money - costFood;

    //                var energy = last_best.e;

    //                var try_improve = new Data(0,
    //                         energy * earning[day - 1] +
    //                           (change) + (qtdFood * earning[day - 1]));

    //                var keep_energy_and_work_today = new Data(0, e * earning[day - 1]);

    //                t[e, day] = Data.Max(
    //                    last_best,    // Do nothing
    //                    try_improve); // buy most food yesterday to work today
    //                t[e, day] = Data.Max(
    //                    t[e, day],                   // Do nothing
    //                    keep_energy_and_work_today); // buy most food yesterday to work today
    //            }
    //        }

    //        return t[start_energy, n].m;
    //    }
    //}


    //public class LeadALife
    //{
    //    public static int calculateProfit(int n, List<int> earning, List<int> cost, int e)
    //    {
    //        Func<int, int, int> work = (int en, int earn) => en * earn;
    //        Func<int, int, int> eat = (int en, int c) => -1 * (en * c);

    //        var data = new Data[n + 1, e + 1];
    //        for (int day = 0; day <= n; day++)
    //        {
    //            for (int energy = 0; energy <= e; energy++)
    //            {
    //                if (day == 0)
    //                {
    //                    data[day, energy] = new Data(energy,0);
    //                    continue;
    //                }

    //                var last_best = new Data(energy, day - 1);


    //                var max_food_yesterday = Math.Min(e, (day < 2) ? 0 : (last_best.m / cost[day - 2]));
    //                var change = last_best.m - (max_food_yesterday * ((day < 2) ? 0 : cost[day - 2]));

    //                int currEn = last_best.e;

    //                var currentEatCost = eat(max_food_yesterday, cost[day - 1]);
    //                var workAndEat = new Data(0, change + work(currEn, earning[day - 1]) + currentEatCost); // workAndEat

    //                var currentEarn = work(energy, earning[day - 1]);
    //                var justWork = new Data(0, data[day - 1, energy].m + currentEarn);
    //                //var justEat = data[day - 1, energy] + currentEatCost;
    //                var doNothing = data[day - 1, energy];

    //                data[day, energy] = Data.Max(Data.Max(doNothing, workAndEat), justWork);
    //            }
    //        }

    //        return data[n, e].m;
    //    }
    //}

    //public class Choose
    //{
    //    public int energy;
    //    public int earn;

    //    public Choose(int e, int gain)
    //    {
    //        energy = e;
    //        earn = gain;
    //    }
    //}


    //public class LeadALife
    //{
    //    public static int calculateProfit(int n, List<int> earning, List<int> cost, int e)
    //    {
    //        Func<int, int, int> work = (int en, int earn) => en * earn;
    //        Func<int, int, int> eat = (int en, int c) => -1 * (en * c);

    //        var afterWork = new int[n + 1, 2];
    //        var afterEat = new int[n + 1, 2];
    //        int currentEnergy = e;
    //        for (int day = 0; day < n; day++)
    //        {
    //            if (day == 0)
    //            {
    //                afterWork[day, 0] = work(currentEnergy * 0, earning[day]);
    //                afterWork[day, 1] = work(currentEnergy * 1, earning[day]);
    //                afterEat[day, 1] = Math.Max(afterWork[day, 0] - 0 * cost[day], afterWork[day, 1] - e * cost[day]);
    //                continue;
    //            }


    //            afterWork[day, 0] = Math.Max(afterEat[day - 1, 0] + 0 * earning[day], afterEat[day - 1, 0] + e * earning[day]);
    //            afterWork[day, 1] = Math.Max(afterEat[day - 1, 1] + 0 * earning[day], afterEat[day - 1, 1] + e * earning[day]);

    //            afterEat[day, 1] = Math.Max(afterWork[day, 0] - 0 * cost[day], afterWork[day, 1] - e * cost[day]);

    //            //var workToday = day == n ? 0 : work(earning[day]);
    //            //var eatYestarday = eat(e, cost[day - 1]);
    //            //var testWorkToday = data[day - 1, 0] + eatYestarday + workToday;
    //            //var testEatYestarday = data[day - 1, 0] + eatYestarday;
    //            //var didntWorkYestarday = data[day - 1, 0] - work(earning[day - 1]) + workToday;
    //            //var doNothing = data[day - 1, 0];

    //            //data[day, 0] = Math.Max(doNothing, Math.Max(didntWorkYestarday, Math.Max(testEatYestarday, testWorkToday)));
    //        }

    //        return afterWork[n, 1];
    //    }
    //}

    public class TestClass
    {

        [Fact]
        public void TestCase0()
        {
            var result = LeadALife.calculateProfit(2, new List<int> { 1, 2 }, new List<int> { 1, 4 }, 5);

            Assert.Equal(10, result);
        }

        [Fact]
        public void TestCase1()
        {
            var result = LeadALife.calculateProfit(3, new List<int> { 1, 5, 5 }, new List<int> { 2, 1, 4 }, 4);

            Assert.Equal(36, result);
        }

        [Fact]
        public void TestCase2()
        {
            var result = LeadALife.calculateProfit(4, new List<int> { 1, 8, 6, 7 }, new List<int> { 1, 3, 4, 1 }, 5);

            Assert.Equal(70, result);
        }

        [Fact]
        public void TestCase3()
        {
            var result = LeadALife.calculateProfit(18, new List<int> { 9, 7, 9, 2, 7, 7, 9, 6, 7, 6, 6, 3, 4, 2, 8, 4, 6, 10 }, new List<int> { 4, 1, 1, 5, 2, 3, 3, 3, 2, 4, 4, 3, 3, 4, 4, 1, 2, 5 }, 5);

            Assert.Equal(345, result);
        }

        [Fact]
        public void TestCase4()
        {
            var result = LeadALife.calculateProfit(11, new List<int> { 2, 5, 10, 8, 5, 3, 1, 4, 6, 8, 6 }, new List<int> { 4, 4, 4, 1, 5, 1, 4, 4, 3, 5, 1 }, 15);

            Assert.Equal(450, result);
        }

        [Fact]
        public void TestCase5()
        {
            var result = LeadALife.calculateProfit(11, new List<int> { 9, 9, 10, 10, 1, 2, 1, 10, 2, 4, 2 }, new List<int> { 2, 4, 1, 1, 5, 1, 5, 1, 3, 5, 5 }, 15);

            Assert.Equal(660, result);
        }

        //[Fact]
        public void TestCase6()
        {
            var result = LeadALife.calculateProfit(50,
                new List<int> { 1, 2, 3, 2, 3, 3, 5, 1, 5, 4, 22, 18, 35, 4, 11, 12, 9, 39, 32, 11, 46, 5, 49, 40, 10, 25, 17, 16, 13, 2, 7, 6, 21, 19, 24, 10, 26, 3, 10, 50, 21, 1, 3, 2, 1, 3, 4, 5, 3, 2 },
                new List<int> { 2, 3, 1, 2, 4, 5, 3, 2, 3, 1, 3, 4, 3, 5, 5, 1, 4, 5, 4, 3, 5, 1, 5, 4, 3, 1, 3, 3, 2, 2, 4, 5, 4, 4, 1, 2, 1, 4, 3, 5, 1, 2, 5, 3, 2, 4, 3, 5, 1, 3 },
                15);

            Assert.Equal(660, result);
        }

        //[Fact]
        public void TestCase7()
        {
            var result = LeadALife.calculateProfit(47,
                new List<int> { 2, 4, 1, 3, 1, 2, 4, 1, 2, 4, 43, 5, 43, 17, 47, 36, 32, 13, 26, 6, 17, 6, 7, 30, 20, 8, 46, 47, 5, 3, 13, 13, 18, 20, 40, 28, 8, 28, 10, 11, 11, 2, 2, 5, 3, 3, 1 },
                new List<int> { 1, 5, 1, 3, 2, 2, 2, 1, 3, 4, 1, 1, 5, 5, 5, 2, 2, 4, 3, 1, 3, 5, 1, 3, 1, 4, 1, 2, 4, 4, 2, 1, 5, 5, 4, 1, 1, 2, 3, 5, 5, 5, 5, 2, 1, 4, 3 },
                15);

            Assert.Equal(660, result);
        }

        //[Fact]
        public void TestCase8()
        {
            var result = LeadALife.calculateProfit(48,
                new List<int> { 5, 5, 5, 2, 3, 1, 2, 4, 3, 2, 50, 46, 32, 44, 24, 40, 15, 26, 8, 14, 6, 10, 8, 13, 26, 6, 22, 24, 40, 16, 31, 50, 12, 43, 13, 19, 33, 46, 7, 7, 33, 3, 2, 1, 4, 2, 2, 3 },
                new List<int> { 4, 1, 1, 4, 5, 1, 3, 1, 3, 2, 4, 3, 2, 2, 4, 3, 4, 1, 3, 3, 4, 4, 1, 3, 4, 4, 3, 2, 1, 1, 1, 1, 4, 1, 2, 3, 3, 4, 5, 1, 5, 4, 5, 3, 2, 3, 5, 2 },
                15);

            Assert.Equal(660, result);
        }

        //[Fact]
        public void TestCase9()
        {
            var result = LeadALife.calculateProfit(98,
                new List<int> { 4, 2, 5, 3, 1, 5, 3, 1, 2, 5, 82, 18, 49, 99, 40, 39, 70, 25, 58, 7, 38, 36, 54, 50, 13, 27, 93, 100, 17, 72, 8, 60, 3, 99, 93, 4, 88, 80, 19, 60, 26, 100, 77, 26, 51, 68, 64, 72, 92, 73, 78, 81, 61, 84, 30, 73, 62, 74, 72, 79, 45, 31, 90, 48, 81, 82, 3, 69, 14, 73, 80, 91, 72, 8, 17, 74, 75, 80, 98, 18, 5, 27, 98, 65, 62, 79, 37, 24, 52, 60, 54, 1, 3, 3, 5, 5, 2, 2 },
                new List<int> { 3, 2, 4, 4, 2, 3, 3, 3, 3, 2, 5, 5, 1, 4, 4, 3, 3, 5, 1, 1, 5, 2, 5, 3, 5, 4, 3, 4, 4, 1, 3, 3, 2, 3, 4, 3, 2, 1, 3, 5, 5, 2, 1, 5, 5, 4, 3, 2, 1, 5, 2, 5, 2, 3, 5, 3, 4, 2, 1, 2, 4, 3, 1, 5, 1, 4, 4, 2, 5, 1, 3, 4, 2, 1, 3, 1, 1, 2, 4, 1, 4, 5, 1, 5, 5, 5, 4, 3, 3, 4, 1, 1, 4, 1, 5, 1, 2, 3 },
                15);

            Assert.Equal(660, result);
        }

        //[Fact]
        public void TestCase10()
        {
            var result = LeadALife.calculateProfit(96,
                new List<int> { 2, 5, 4, 1, 4, 3, 5, 2, 1, 2, 1, 4, 4, 4, 4, 1, 5, 1, 2, 2, 4, 4, 3, 3, 3, 3, 5, 3, 2, 2, 5, 3, 2, 5, 5, 2, 2, 1, 5, 4, 2, 5, 2, 2, 5, 2, 3, 1, 2, 1, 2, 2, 4, 2, 4, 2, 4, 5, 1, 5, 4, 2, 5, 2, 1, 1, 3, 5, 2, 4, 3, 5, 1, 2, 2, 2, 5, 1, 3, 4, 1, 1, 5, 2, 4, 1, 5, 3, 2, 5, 4, 5, 1, 3, 1, 3 },
                new List<int> { 1, 5, 2, 4, 4, 5, 3, 1, 3, 1, 2, 2, 1, 1, 2, 4, 4, 4, 5, 2, 4, 4, 1, 2, 5, 5, 2, 2, 4, 4, 4, 4, 4, 3, 4, 4, 2, 4, 1, 1, 4, 2, 4, 2, 5, 1, 5, 5, 4, 1, 1, 4, 1, 2, 5, 5, 3, 3, 3, 1, 2, 3, 2, 5, 5, 5, 5, 3, 3, 5, 3, 4, 3, 2, 2, 4, 4, 3, 3, 4, 3, 4, 2, 3, 2, 3, 4, 4, 1, 3, 1, 4, 5, 2, 5, 5 },
                15);

            Assert.Equal(660, result);
        }

        //[Fact]
        public void TestCase11()
        {
            var result = LeadALife.calculateProfit(99,
                new List<int> { 3, 1, 5, 3, 4, 2, 5, 4, 1, 1, 9, 93, 76, 9, 96, 6, 46, 100, 18, 94, 84, 70, 38, 19, 50, 33, 33, 62, 12, 79, 62, 41, 39, 32, 75, 49, 30, 9, 19, 37, 36, 79, 29, 12, 87, 24, 69, 32, 75, 86, 77, 11, 7, 14, 81, 56, 98, 13, 69, 61, 91, 31, 1, 81, 14, 75, 81, 43, 84, 99, 31, 71, 29, 59, 82, 67, 35, 2, 50, 61, 87, 27, 71, 45, 92, 51, 100, 42, 16, 21, 2, 1, 1, 3, 4, 4, 2, 4, 3 },
                new List<int> { 2, 5, 3, 5, 3, 3, 3, 5, 2, 5, 1, 3, 3, 4, 5, 3, 1, 1, 4, 4, 1, 4, 2, 3, 4, 1, 1, 4, 3, 5, 3, 1, 1, 2, 5, 3, 5, 5, 4, 3, 4, 5, 2, 1, 3, 2, 3, 5, 4, 2, 5, 1, 5, 2, 3, 1, 2, 1, 1, 1, 2, 4, 4, 2, 5, 5, 4, 1, 4, 5, 1, 2, 1, 2, 5, 3, 5, 2, 5, 3, 5, 4, 5, 2, 5, 5, 4, 2, 5, 4, 4, 1, 2, 2, 2, 4, 2, 2, 1 },
                15);

            Assert.Equal(660, result);
        }


        //[Fact]
        public void TestCase12()
        {
            var result = LeadALife.calculateProfit(100,
                new List<int> { 5, 3, 1, 4, 1, 3, 4, 1, 1, 3, 63, 60, 20, 28, 74, 15, 41, 83, 30, 12, 18, 85, 37, 71, 46, 70, 79, 19, 80, 37, 38, 15, 61, 43, 95, 32, 93, 4, 94, 15, 66, 57, 74, 37, 84, 47, 3, 76, 81, 85, 40, 99, 21, 76, 69, 66, 45, 47, 85, 76, 35, 74, 42, 96, 16, 37, 79, 60, 40, 24, 74, 57, 80, 99, 45, 16, 98, 47, 43, 78, 83, 82, 28, 3, 57, 48, 69, 53, 46, 5, 29, 1, 3, 2, 3, 5, 3, 3, 5, 4 },
                new List<int> { 1, 5, 2, 3, 4, 1, 5, 1, 5, 2, 5, 2, 1, 3, 2, 4, 5, 2, 2, 1, 1, 2, 3, 5, 3, 2, 4, 3, 4, 5, 1, 1, 5, 3, 5, 3, 5, 1, 5, 4, 5, 4, 3, 5, 1, 4, 3, 3, 5, 1, 5, 2, 4, 2, 1, 4, 5, 4, 1, 5, 4, 1, 2, 5, 5, 2, 2, 5, 4, 3, 3, 3, 1, 2, 4, 4, 5, 4, 1, 4, 4, 5, 2, 5, 3, 2, 3, 4, 3, 3, 3, 3, 5, 4, 2, 2, 2, 5, 1, 3 },
                15);

            Assert.Equal(345, result);
        }

        [Fact]
        public void TestCaseCustom()
        {
            var result = LeadALife.calculateProfit(3, new List<int> { 7, 2, 4 }, new List<int> { 7, 3, 6 }, 5);

            Assert.Equal(40, result);
        }

        [Fact]
        public void TestCaseCustom2()
        {
            var result = LeadALife.calculateProfit(4, new List<int> { 7, 2, 2, 4 }, new List<int> { 7, 3, 2, 6 }, 5);

            Assert.Equal(45, result);
        }


        //[Fact]
        public void TestCaseX()
        {
            var result = LeadALife.calculateProfit(20, new List<int> { 5, 1, 9, 7, 9, 2, 7, 7, 9, 6, 7, 6, 6, 3, 4, 2, 8, 4, 6, 10 }, new List<int> { 3, 1, 4, 1, 1, 5, 2, 3, 3, 3, 2, 4, 4, 3, 3, 4, 4, 1, 2, 5 }, 5);

            Assert.Equal(345, result);
        }
    }

}
