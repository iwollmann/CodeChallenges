using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace ChooseFlask
{
    class ChooseFlask
    {
        public static int chooseFlask(List<int> requirements, int flaskTypes, List<List<int>> markings)
        {
            requirements.Sort();
            var maxValue = requirements.Last();
            long minValue = long.MaxValue;
            var minIndex = int.MaxValue;

            markings = markings.GroupBy(x => x[0]).Select(x => x.Select(y => y[1]).ToList()).ToList();
            for (int i = 0; i < flaskTypes; i++)
            {
                if (markings[i].Last() < maxValue)
                    continue;

                long count = 0;

                var value = 0;
                for (int m = 0; m < requirements.Count; m++)
                {
                    if (value < requirements[m])
                    {
                        value = markings[i]
                            .Where(x => x >= requirements[m])
                            .FirstOrDefault();
                    }

                    if (value != 0)
                    {
                        count += value - requirements[m];
                    }
                }

                if (count < minValue)
                {
                    minValue = count;
                    minIndex = i;
                }
            }

            return minIndex == int.MaxValue ? -1 : minIndex;
        }
    }

    public class TestClass
    {

        [Fact]
        public void TestCase0()
        {
            var result = ChooseFlask.chooseFlask(new List<int> { 4, 6 }, 2,
                new List<List<int>> {
                    new List<int> { 0, 5 },
                    new List<int> { 0, 7 },
                    new List<int> { 0, 10 },
                    new List<int> { 1, 4 },
                    new List<int> { 1, 10 }
                });

            Assert.Equal(0, result);
        }

        [Fact]
        public void TestCase1()
        {
            var result = ChooseFlask.chooseFlask(new List<int> { 10, 15 }, 3,
                new List<List<int>> {
                    new List<int> { 0, 11 },
                    new List<int> { 0, 20 },
                    new List<int> { 1, 11 },
                    new List<int> { 1, 17 },
                    new List<int> { 2, 12 },
                    new List<int> { 2, 16 }
                });

            Assert.Equal(1, result);
        }

        [Fact]
        public void TestCase3()
        {
            List<int> requirements;
            int flaskTypes;
            List<List<int>> markings;
            LoadTestFile("./ChooseFlask_input003.txt", out requirements, out flaskTypes, out markings);

            var result = ChooseFlask.chooseFlask(requirements, flaskTypes, markings);

            Assert.Equal(4, result);
        }


        [Fact]
        public void TestCaseSample()
        {
            var result = ChooseFlask.chooseFlask(new List<int> { 4, 6, 6, 7 }, 3,
                new List<List<int>> {
                    new List<int> { 0, 3 },
                    new List<int> { 0, 5 },
                    new List<int> { 0, 7 },
                    new List<int> { 1, 6 },
                    new List<int> { 1, 8 },
                    new List<int> { 1, 9 },
                    new List<int> { 2, 3 },
                    new List<int> { 2, 5 },
                    new List<int> { 2, 6 }
                });

            Assert.Equal(0, result);
        }

        private static void LoadTestFile(string file, out List<int> requirements, out int flaskTypes, out List<List<int>> markings)
        {
            var lines = File.ReadAllLines(file);
            int requirementsCount = Convert.ToInt32(lines[0]);

            requirements = new List<int>();
            for (int i = 1; i < requirementsCount + 1; i++)
            {
                int requirementsItem = Convert.ToInt32(lines[i]);
                requirements.Add(requirementsItem);
            }

            var currentLine = 1 + requirementsCount;

            flaskTypes = Convert.ToInt32(lines[currentLine]);
            currentLine++;

            int markingsRows = Convert.ToInt32(lines[currentLine]);
            currentLine++;

            int markingsColumns = Convert.ToInt32(lines[currentLine]);
            currentLine++;

            markings = new List<List<int>>();
            for (int i = 0; i < markingsRows; i++)
                markings.Add(lines[currentLine + i].Split(' ').ToList().Select(markingsTemp => Convert.ToInt32(markingsTemp)).ToList());
        }

    }
}
