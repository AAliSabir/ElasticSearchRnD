using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("! Application Started !");

            try
            {
                var node = new Uri("http://40.74.55.129:9200");

                var settings = new ConnectionSettings(node);

                Console.WriteLine("Connecting to Elastic Search ...");

                var client = new ElasticClient(settings);

                Console.WriteLine("Elastic Search Connected Successfully !");
                
                queryDocuments2(client);
                //addNewDocument(client);
                //addListOfDocs(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured : " + ex);
            }

            Console.ReadLine();
        }

        static void addNewDocument(ElasticClient client)
        {
            Console.WriteLine("Adding new document");
            Console.WriteLine();

            Laptop newLaptop = new Laptop();

            TransactionLog transactionLog = new TransactionLog
            {
                instrument = "card",
                instrumentNumber = "1234",
                purpose = "Lenovo Buy",
                status = "COMPLETED",
                statusCode = "200",
                responseCode = "000",
                responseDescription = "Success"
            };

            var index = client.Index(transactionLog, i => i
                .Index("dummytransactionlog")
                .Id("12"));

            Console.WriteLine("Added");


        }

        static void queryDocuments1(ElasticClient client)
        {
            Console.WriteLine("Querying Elastic Search ...");

            QueryContainer query = new TermQuery
            {
                Field = "core",
                Value = "i5"
            };

            var searchRequest = new SearchRequest
            {
                Query = query
            };

            // var searchResults = client.Search<Laptop>(searchRequest);

            var searchResults = client.Search<object>(s => s.AllIndices().QueryOnQueryString("lenovo"));
            
            // var searchResults = client.Search<Laptop>(s => s.AllIndices().MatchAll());

            Console.WriteLine("Query Results Started");
            Console.WriteLine();

            foreach (Laptop laptop in searchResults.Documents)
            {
                //Console.WriteLine("core : " + laptop["core"] + ", ssd : " + laptop["ssd"] + ", ram" + laptop["ram"] + ", gen : " + laptop["gen"] + ", make : " + laptop["make"]);
                Console.WriteLine("core : " + laptop.core + ", ssd : " + laptop.ssd + ", ram" + laptop.ram + ", gen : " + laptop.gen + ", make : " + laptop.make);
            }

            Console.WriteLine();
            Console.WriteLine("Query Results Ended");

            Console.WriteLine();
        }

        static void queryDocuments2(ElasticClient client)
        {
            Console.WriteLine("Querying Elastic Search ...");

            //search by specific text field
            QueryContainer matchQuery = new MatchQuery
            {
                Field = "CstmrCdtTrfInitn.GrpHdr.MsgId",
                Query = "ABC"
            };

            //search by date range
            QueryContainer dateRangequery = new TermRangeQuery
            {
                Field = "CstmrCdtTrfInitn.GrpHdr.MsgId",
                GreaterThanOrEqualTo = "2009-09-28T14:07:00",
                LessThanOrEqualTo = "2009-09-28T14:07:00"                
            };

            //search by amount range
            QueryContainer amountRangequery = new TermRangeQuery
            {
                Field = "CstmrCdtTrfInitn.GrpHdr.NbOfTxs",
                GreaterThanOrEqualTo = "2",
                LessThanOrEqualTo = "5"
            };

            var searchRequest = new SearchRequest
            {
                Query = matchQuery
            };

            //get all documents of an index
            var searchResults = client.Search<object>(s => s.Index("iso_dev_field"));

            //search in all fields of all docs
            //var searchResults = client.Search<object>(s => s.AllIndices().QueryOnQueryString("lenovo"));

            //get all documents of an index
            // var searchResults = client.Search<Laptop>(searchRequest);

            Console.WriteLine("Query Results Started");
            Console.WriteLine();

            foreach (Dictionary<string, object> b in searchResults.Documents)
            {
                Console.WriteLine("----------");
                parseKeyValuePairs(b);
                Console.WriteLine("----------");
            }

            Console.WriteLine();
            Console.WriteLine("Query Results Ended");

            Console.WriteLine();
        }

        static void parseKeyValuePairs(Dictionary<string, object> dict)
        {

            foreach (KeyValuePair<string, object> x in dict)
            {
                if(x.Value.GetType().Equals(typeof(Dictionary<string, object>)))
                {
                    Console.WriteLine(x.Key + " : " );
                    dynamic newDict = x.Value;
                    parseKeyValuePairs(newDict);
                }
                else
                {
                    Console.WriteLine(x.Key + " : " + x.Value);
                }
            }

        }

        static void addListOfDocs(ElasticClient client)
        {
            List<TransactionLog> transactionLogs = new List<TransactionLog>();

            #region adding data to list
            TransactionLog transactionLog = new TransactionLog
            {
                instrument = "account",
                instrumentNumber = "PK1234",
                purpose = "Electricity Bill Payment",
                status = "COMPLETED",
                statusCode = "200",
                responseCode = "000",
                responseDescription = "Success"
            };
            transactionLogs.Add(transactionLog);

            transactionLog = new TransactionLog();
            transactionLog.instrument = "account";
            transactionLog.instrumentNumber = "PK1234";
            transactionLog.purpose = "SSGC Bill Payment";
            transactionLog.status = "PENDING";
            transactionLog.statusCode = "200";
            transactionLog.responseCode = "000";
            transactionLog.responseDescription = "Success";
            transactionLogs.Add(transactionLog);

            transactionLog = new TransactionLog();
            transactionLog.instrument = "card";
            transactionLog.instrumentNumber = "1234";
            transactionLog.purpose = "Restaurant Bill Payment";
            transactionLog.status = "FAILED";
            transactionLog.statusCode = "400";
            transactionLog.responseCode = "047";
            transactionLog.responseDescription = "Validation Failed";
            transactionLogs.Add(transactionLog);

            transactionLog = new TransactionLog();
            transactionLog.instrument = "card";
            transactionLog.instrumentNumber = "1234";
            transactionLog.purpose = "Mart Bill Payment";
            transactionLog.status = "COMPLETED";
            transactionLog.statusCode = "200";
            transactionLog.responseCode = "000";
            transactionLog.responseDescription = "Success";
            transactionLogs.Add(transactionLog);

            transactionLog = new TransactionLog();
            transactionLog.instrument = "card";
            transactionLog.instrumentNumber = "1234";
            transactionLog.purpose = "Fuel";
            transactionLog.status = "COMPLETED";
            transactionLog.statusCode = "200";
            transactionLog.responseCode = "000";
            transactionLog.responseDescription = "Success";
            transactionLogs.Add(transactionLog);

            transactionLog = new TransactionLog();
            transactionLog.instrument = "account";
            transactionLog.instrumentNumber = "PK2468";
            transactionLog.purpose = "SSGC Bill Payment";
            transactionLog.status = "FAILED";
            transactionLog.statusCode = "500";
            transactionLog.responseCode = "500";
            transactionLog.responseDescription = "Internal Server Error";
            transactionLogs.Add(transactionLog);

            transactionLog = new TransactionLog();
            transactionLog.instrument = "account";
            transactionLog.instrumentNumber = "PK2468";
            transactionLog.purpose = "Electricity Bill Payment";
            transactionLog.status = "COMPLETED";
            transactionLog.statusCode = "200";
            transactionLog.responseCode = "000";
            transactionLog.responseDescription = "Success";
            transactionLogs.Add(transactionLog);

            transactionLog = new TransactionLog();
            transactionLog.instrument = "card";
            transactionLog.instrumentNumber = "2468";
            transactionLog.purpose = "Fuel";
            transactionLog.status = "FAILED";
            transactionLog.statusCode = "400";
            transactionLog.responseCode = "999";
            transactionLog.responseDescription = "Business Validation Failure";
            transactionLogs.Add(transactionLog);

            transactionLog = new TransactionLog();
            transactionLog.instrument = "card";
            transactionLog.instrumentNumber = "2468";
            transactionLog.purpose = "Clothes Shopping";
            transactionLog.status = "COMPLETED";
            transactionLog.statusCode = "200";
            transactionLog.responseCode = "000";
            transactionLog.responseDescription = "Success";
            transactionLogs.Add(transactionLog);

            transactionLog = new TransactionLog();
            transactionLog.instrument = "card";
            transactionLog.instrumentNumber = "2468";
            transactionLog.purpose = "Grocery Shopping";
            transactionLog.status = "COMPLETED";
            transactionLog.statusCode = "200";
            transactionLog.responseCode = "000";
            transactionLog.responseDescription = "Success";
            transactionLogs.Add(transactionLog);

            transactionLog = new TransactionLog();
            transactionLog.instrument = "card";
            transactionLog.instrumentNumber = "2468";
            transactionLog.purpose = "Grocery Shopping";
            transactionLog.status = "COMPLETED";
            transactionLog.statusCode = "200";
            transactionLog.responseCode = "000";
            transactionLog.responseDescription = "Success";
            transactionLogs.Add(transactionLog);
            #endregion

            int a = 1;

            BulkDescriptor descriptor = new BulkDescriptor();
            foreach (var doc in transactionLogs)
            {
                descriptor.Index<object>(i => i
                    .Index("dummytransactionlog")
                    .Id(a.ToString())
                    .Document(doc));
                a++;
            }

            client.Bulk(descriptor);
        }
    }

    class TransactionLog
    {
        public string instrument { get; set; }
        public string instrumentNumber { get; set; }
        public string status { get; set; }
        public string purpose { get; set; }
        public string statusCode { get; set; }
        public string responseCode { get; set; }
        public string responseDescription { get; set; }
    }

    class Laptop
    {
        public string core { get; set; }
        public string ram { get; set; }
        public string ssd { get; set; }
        public string gen { get; set; }
        public string make { get; set; }
    }
}
