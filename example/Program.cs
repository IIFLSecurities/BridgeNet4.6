using BridgeNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePOC
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            IConnect CS = Connect.Instance;
            ConnectReq connectReq = new ConnectReq();

            //Event handlers
            CS.OnDisconnect += (resultCode, message) =>
            {
                Console.WriteLine("Disconnected with result code: " + resultCode + " and message: " + message);
            };

            //On error event handler
            CS.OnError += (errorCode, errMsg) =>
            {
                Console.WriteLine("Error code: " + errorCode + " and message: " + errMsg);
            };

            //On feed data received event handler
            CS.OnFeedDataReceived += (data, topic) =>
            {
                MWBOCombined markertFeed = BuildStruct<MWBOCombined>(data);
                Console.WriteLine("LTP : " + markertFeed.Ltp);
                Console.WriteLine("Data received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };


            //On open interest data received event handler
            CS.OnOpenInterestDataReceived += (data, topic) =>
            {
                OpenInterestData openInterestData = BuildStruct<OpenInterestData>(data);
                Console.WriteLine("Open Interest: " + openInterestData.OpenInterest);
                Console.WriteLine("Open interest data received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };

            //On market status data received event handler
            CS.OnMarketStatusDataReceived += (data, topic) =>
            {
                MarketStatusData marketStatusData = BuildStruct<MarketStatusData>(data);
                Console.WriteLine("Market status code: " + marketStatusData.MarketStatusCode);
                Console.WriteLine("Market status data received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };

            //On LPP data received event handler
            CS.OnLppDataReceived += (data, topic) =>
            {
                LppData lppData = BuildStruct<LppData>(data);
                Console.WriteLine("LPP High: " + lppData.LppHigh);
                Console.WriteLine("LPP data received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };

            //On high 52 week data received event handler
            CS.OnHigh52WeekDataReceived += (data, topic) =>
            {
                High52WeekData high52WeekData = BuildStruct<High52WeekData>(data);
                Console.WriteLine("High 52 week: " + high52WeekData.High52Week);
                Console.WriteLine("High 52 week data received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };

            //On low 52 week data received event handler
            CS.OnLow52WeekDataReceived += (data, topic) =>
            {
                Low52WeekData low52WeekData = BuildStruct<Low52WeekData>(data);
                Console.WriteLine("Low 52 week: " + low52WeekData.Low52Week);
                Console.WriteLine("Low 52 week data received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };

            //On upper circuit data received event handler
            CS.OnUpperCircuitDataReceived += (data, topic) =>
            {
                UpperCircuitData upperCircuitData = BuildStruct<UpperCircuitData>(data);
                Console.WriteLine("Upper circuit: " + upperCircuitData.UpperCircuit);
                Console.WriteLine("Upper circuit data received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };

            //On lower circuit data received event handler
            CS.OnLowerCircuitDataReceived += (data, topic) =>
            {
                LowerCircuitData lowerCircuitData = BuildStruct<LowerCircuitData>(data);    
                Console.WriteLine("Lower circuit: " + lowerCircuitData.LowerCircuit);
                Console.WriteLine("Lower circuit data received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };

            //On order updates received event handler
            CS.OnOrderUpdatesReceived += (data, topic) =>
            {
                Console.WriteLine("Order updates received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };

            //On trade updates received event handler
            CS.OnTradeUpdatesReceived += (data, topic) =>
            {
                Console.WriteLine("Trade updates received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };


            connectReq.host = "bridge.iiflcapital.com";
            connectReq.port = 9906;
            connectReq.token = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICIxVks4TEhlRnRvSmp6YWk1RmJlSGNPbDI3ekpGanBScTE2Vmt4eGJBZ0ZjIn0.eyJleHAiOjE3NTgzNDE3MzEsImlhdCI6MTc0Mjc4OTczMSwianRpIjoiNzFhNjYzMzgtODU0OC00MDcxLTgxMDctZmRmNjdjMjBlYWIyIiwiaXNzIjoiaHR0cHM6Ly9rZXljbG9hay5paWZsc2VjdXJpdGllcy5jb20vcmVhbG1zL0lJRkwiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiNDk4NTU2NTAtMDcyNS00ZGUzLWFlMmQtN2ExMGYxYzIwODI4IiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiSUlGTCIsInNpZCI6ImQwZDM1MTZhLTkxOWUtNGU5OC1iZmJmLTI3ZTg3OTJiZjI3MSIsImFjciI6IjEiLCJhbGxvd2VkLW9yaWdpbnMiOlsiaHR0cDovLzEwLjEyNS42OC4xNDQ6ODA4MC8iXSwicmVhbG1fYWNjZXNzIjp7InJvbGVzIjpbImRlZmF1bHQtcm9sZXMtaWlmbCIsIm9mZmxpbmVfYWNjZXNzIiwidW1hX2F1dGhvcml6YXRpb24iXX0sInJlc291cmNlX2FjY2VzcyI6eyJJSUZMIjp7InJvbGVzIjpbIkdVRVNUX1VTRVIiLCJBQ1RJVkVfVVNFUiJdfSwiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwic2NvcGUiOiJvcGVuaWQgZW1haWwgcHJvZmlsZSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJ1Y2MiOiI5MzA4MDA0OCIsInNvbGFjZV9ncm91cCI6IlNVQlNDUklCRVJfQ0xJRU5UIiwibmFtZSI6IlNBVEhFRVNIIEtVTUFSIEdPUElTRVRUWSBOQSIsInByZWZlcnJlZF91c2VybmFtZSI6IjkzMDgwMDQ4IiwiZ2l2ZW5fbmFtZSI6IlNBVEhFRVNIIEtVTUFSIEdPUElTRVRUWSIsImZhbWlseV9uYW1lIjoiTkEiLCJlbWFpbCI6InNhdGlzaGt1bWFyZ29waXNldHR5MTExQGdtYWlsLmNvbSJ9.8t9A2KBDWb5sqi8M9ObhThKea0VCPrsC3E85iK472ff0KDVpXt9UY9nTIHie9Q4oJeIKzZraXwfZodWlkWFNPVzX2uH4DZjRVwTkxmaMFO3f4vMy4sBZPTprTmvB89qHlmLfEamnoB6C0wfm507QyAQC4PTuek4qVFdb8lvPcOEm0E9yGvzAbs64RMx8J1M8aql3OrfKqPdTfMb3GE3Qh4drBjRWBBWLhpbcwJmyYGFs7Wc-UXKH1xTjl0idyxCyyxMgWp9V4YlACTovoail5_a8rpQL03GzsVt5E4xwwVZXYPHLd2bvmax6cITEomzAC6rxpd4jgLf5gd4CiyY3RA";
            string JsonReq = JsonConvert.SerializeObject(connectReq);

            //Connect to the host
            string connectRes = await CS.ConnectHost(JsonReq);
            Console.WriteLine(connectRes);



            //Subscribe to the feed
            SubscribeReq subscribeReq = new SubscribeReq(new List<string> { "bseeq/999904", });
            String jSubReq= JsonConvert.SerializeObject(subscribeReq);
            string subscribeRes = await CS.SubscribeFeed(jSubReq);


            //Subscribe to the open interest
            string jSubOpenInterestReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "nseeq/234656" }));
            string subscribeOpenInterestRes = await CS.SubscribeOpenInterest(jSubOpenInterestReq);
            Console.WriteLine(subscribeOpenInterestRes);

            //Subscribe to the LPP
            string jSubLppReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "nsefo/234656" }));
            string subscribeLppRes = await CS.SubscribeLpp(jSubLppReq);
            Console.WriteLine(subscribeLppRes);

            //Subscribe to the market status
            string jSubMarketStatusReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "nseeq","bsefo" }));
            string subscribeMarketStatusRes = await CS.SubscribeMarketStatus(jSubMarketStatusReq);
            Console.WriteLine(subscribeMarketStatusRes);

            //Subscribe to the high 52 week
            string jSubHigh52WeekReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "nseeq" }));
            string subscribeHigh52WeekRes = await CS.SubscribeHigh52week(jSubHigh52WeekReq);
            Console.WriteLine(subscribeHigh52WeekRes);

            //Subscribe to the low 52 week
            string jSubLow52WeekReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "bseeq" }));
            string subscribeLow52WeekRes = await CS.SubscribeLow52week(jSubLow52WeekReq);
            Console.WriteLine(subscribeLow52WeekRes);

            //Subscribe to the upper circuit
            string jSubUpperCircuitReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "bsefo","ncdexcomm" }));
            string subscribeUpperCircuitRes = await CS.SubscribeUpperCircuit(jSubUpperCircuitReq);  
            Console.WriteLine(subscribeUpperCircuitRes);

            //Subscribe to the lower circuit
            string jSubLowerCircuitReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "mcxcomm" }));
            string subscribeLowerCircuitRes = await CS.SubscribeLowerCircuit(jSubLowerCircuitReq);
            Console.WriteLine(subscribeLowerCircuitRes);

            //Subscribe to the order updates
            string jSubOrderUpdatesReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "93080048" }));
            string subscribeOrderUpdatesRes = await CS.SubscribeOrderUpdates(jSubOrderUpdatesReq);
            Console.WriteLine(subscribeOrderUpdatesRes);

            //Subscribe to the trade updates
            string jSubTradeUpdatesReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "93080048" }));
            string subscribeTradeUpdatesRes = await CS.SubscribeTradeUpdates(jSubTradeUpdatesReq);
            Console.WriteLine(subscribeTradeUpdatesRes);

            Thread.Sleep(10000);

            //Unsubscribe to the feed
            UnSubscribeReq unSubscribeReq = new UnSubscribeReq(new List<string> { "bseeq/999904" });
            string jUnSubReq = JsonConvert.SerializeObject(unSubscribeReq);
            string unSubscribeRes = await CS.UnsubscribeFeed(jUnSubReq);
            Console.WriteLine(unSubscribeRes);

            //Unsubscribe to the open interest
            string jUnSubOpenInterestReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "nseeq/234656" }));
            string unSubscribeOpenInterestRes = await CS.UnsubscribeOpenInterest(jUnSubOpenInterestReq);
            Console.WriteLine(unSubscribeOpenInterestRes);

            //Unsubscribe to the LPP
            string jUnSubLppReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "nsefo/234656" }));
            string unSubscribeLppRes = await CS.UnsubscribeLpp(jUnSubLppReq);
            Console.WriteLine(unSubscribeLppRes);

            //Unsubscribe to the market status
            string jUnSubMarketStatusReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "nseeq", "bsefo" }));
            string unSubscribeMarketStatusRes = await CS.UnsubscribeMarketStatus(jUnSubMarketStatusReq);
            Console.WriteLine(unSubscribeMarketStatusRes);

            //Unsubscribe to the high 52 week
            string jUnSubHigh52WeekReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "nseeq"}));
            string unSubscribeHigh52WeekRes = await CS.UnsubscribeHigh52week(jUnSubHigh52WeekReq);
            Console.WriteLine(unSubscribeHigh52WeekRes);

            //Unsubscribe to the low 52 week
            string jUnSubLow52WeekReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "bseeq" }));
            string unSubscribeLow52WeekRes = await CS.UnsubscribeLow52week(jUnSubLow52WeekReq);
            Console.WriteLine(unSubscribeLow52WeekRes);

            //Unsubscribe to the upper circuit
            string jUnSubUpperCircuitReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "bsefo", "ncdexcomm" }));
            string unSubscribeUpperCircuitRes = await CS.UnsubscribeUpperCircuit(jUnSubUpperCircuitReq);
            Console.WriteLine(unSubscribeUpperCircuitRes);

            //Unsubscribe to the lower circuit
            string jUnSubLowerCircuitReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "mcxcomm" }));
            string unSubscribeLowerCircuitRes = await CS.UnsubscribeLowerCircuit(jUnSubLowerCircuitReq);
            Console.WriteLine(unSubscribeLowerCircuitRes);

            //Unsubscribe to the order updates
            string jUnSubOrderUpdatesReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "93080048" }));
            string unSubscribeOrderUpdatesRes = await CS.UnSubscribeOrderUpdates(jUnSubOrderUpdatesReq);
            Console.WriteLine(unSubscribeOrderUpdatesRes);

            //Unsubscribe to the trade updates
            string jUnSubTradeUpdatesReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "93080048" }));
            string unSubscribeTradeUpdatesRes = await CS.UnSubscribeTradeUpdates(jUnSubTradeUpdatesReq);
            Console.WriteLine(unSubscribeTradeUpdatesRes);


            Thread.Sleep(100000);

            //Disconnect from the host
            string disconnectRes = await CS.DisconnectHost();
            Console.WriteLine(disconnectRes);

        }

        private static T BuildStruct<T>(byte[] Bytes)
        {
            T myStruct;

            // Convert byte array to struct
            GCHandle handle = GCHandle.Alloc(Bytes, GCHandleType.Pinned);
            try
            {
                myStruct = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                return myStruct;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
            finally
            {
                handle.Free();
            }
        }
    }

    [Serializable]
    internal class ConnectReq
    {
        public string host { get; set; }
        public int port { get; set; }
        public string token { get; set; }

    }

    internal class SubscribeReq
    {
        private List<string> _subscriptionList = null;

        public SubscribeReq(List<string> topicList)
        {
            _subscriptionList = topicList;
        }

        public List<string> subscriptionList
        {
            get { return _subscriptionList; }
        }

    }

    internal class UnSubscribeReq
    {
        private List<string> _topicList = null;

        public UnSubscribeReq(List<string> topicList)
        {
            _topicList = topicList;
        }

        public List<string> unsubscriptionList
        {
            get { return _topicList; }
        }

    }


    public struct MWBOCombined
    {
        public int Ltp { get; set; }
        public uint LastTradedQuantity { get; set; }
        public uint TradedVolume { get; set; }
        public int High { get; set; }
        public int Low { get; set; }
        public int Open { get; set; }
        public int Close { get; set; }
        public int AverageTradedPrice { get; set; }
        public ushort Reserved { get; set; }
        public uint BestBidQuantity { get; set; }
        public int BestBidPrice { get; set; }
        public uint BestAskQuantity { get; set; }
        public int BestAskPrice { get; set; }
        public uint TotalBidQuantity { get; set; }
        public uint TotalAskQuantity { get; set; }
        public int PriceDivisor { get; set; }
        public int LastTradedTime { get; set; }
        public Depth[] MarketDepth { get; set; }
    }

    public struct Depth
    {
        public uint Quantity { get; set; }
        public int Price { get; set; }
        public short Orders { get; set; }
        public short TransactionType { get; set; }
    }

    public struct OpenInterestData
    {
        public int OpenInterest { get; set; }
        public int DayHighOi { get; set; }
        public int DayLowOi { get; set; }
        public int PreviousOi { get; set; }
    }

    public struct LppData
    {
        public uint LppHigh { get; set; }
        public uint LppLow { get; set; }
        public int PriceDivisor { get; set; }
    }

    public struct UpperCircuitData
    {
        public uint InstrumentId { get; set; }
        public uint UpperCircuit { get; set; }
        public int PriceDivisor { get; set; }
    }

    public struct LowerCircuitData
    {
        public uint InstrumentId { get; set; }
        public uint LowerCircuit { get; set; }
        public int PriceDivisor { get; set; }
    }

    public struct MarketStatusData
    {
        public ushort MarketStatusCode { get; set; }
    }

    public struct High52WeekData
    {
        public uint InstrumentId { get; set; }
        public uint High52Week { get; set; }
        public int PriceDivisor { get; set; }
    }

    public struct Low52WeekData
    {
        public uint InstrumentId { get; set; }
        public uint Low52Week { get; set; }
        public int PriceDivisor { get; set; }
    }   


}
