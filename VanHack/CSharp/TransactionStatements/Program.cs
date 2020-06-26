using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TransactionStatements
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionType
    {
        Credit,
        Debit
    }

    public class Location
    {
        public int Id { get; set; }
    }
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public TransactionType TxnType { get; set; }
        [JsonIgnore]
        public double ParsedAmount => double.Parse(Amount, System.Globalization.NumberStyles.Currency);
        public string Amount { get; set; }
        public Location Location { get; set; }
    }

    public class ResponseTransactionsDto
    {
        public string Page { get; set; }
        [JsonProperty("per_page")]

        public int PageSize { get; set; }
        public int Total { get; set; }
        [JsonProperty("total_pages")]

        public int TotalPages { get; set; }

        [JsonProperty("data")]

        public IEnumerable<Transaction> Transactions { get; set; }
    }

    class Result
    {
        public static async Task<List<List<int>>> totalTransactions(int locationId, string transactionType)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://jsonmock.hackerrank.com")
            };

            if (Enum.TryParse(transactionType, true, out TransactionType transType))
            {
                var transactions = await GetAllTransactionsByType(client, transType);

                var test = transactions
                    .Where(x => x.Location.Id == locationId)
                    .OrderBy(x => x.UserId);

                var result = transactions
                    .Where(x => x.Location.Id == locationId)
                    .OrderBy(x=> x.UserId)
                    .GroupBy(x => x.UserId)
                    .Select(x => new List<int> { x.Key, (int)Math.Floor(x.Sum(s => s.ParsedAmount))}).ToList();

                if (result.Any())
                    return result;
            }
            else
            {
                throw new InvalidOperationException("Invalid transaction type");
            }

            return new List<List<int>> { new List<int> { -1, -1 } };
        }

        private static async Task<IEnumerable<Transaction>> GetAllTransactionsByType(HttpClient client, TransactionType transactionType)
        {
            var transactions = new List<Transaction>();

            int page_number = 1;
            var pageCountDown = page_number;

            while (pageCountDown > 0)
            {
                var response = await client.GetAsync($"/api/transactions/search?txnType={transactionType}&page={page_number}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var responseContent = JsonConvert.DeserializeObject<ResponseTransactionsDto>(content);

                    transactions.AddRange(responseContent.Transactions);
                    pageCountDown = responseContent.TotalPages - page_number;
                    page_number++;
                }
                else
                {
                    break;
                }
            }

            return transactions;
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            //TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            //int locationId = Convert.ToInt32(Console.ReadLine().Trim());

            //string transactionType = Console.ReadLine();

            List<List<int>> result = Result.totalTransactions(1, "debit").Result;

            //textWriter.WriteLine(String.Join("\n", result.Select(x => String.Join(" ", x))));

            //textWriter.Flush();
            //textWriter.Close();
        }
    }
}
