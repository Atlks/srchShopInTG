using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Hex.HexTypes;
using System.Collections.Generic;
using Nethereum.Hex.HexTypes;
using Nethereum.StandardTokenEIP20;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json.Linq;
using prjx;
using prjx.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static mdsj.lib.web3;
using static prjx.timerCls;
using static mdsj.biz_other;
using static mdsj.clrCls;
using static mdsj.lib.exCls;
using static prjx.lib.arrCls;//  prj202405.lib
using static prjx.lib.dbgCls;
using static mdsj.lib.logCls;
using static prjx.lib.corex;
using static prjx.lib.db;
using static prjx.lib.filex;
using static prjx.lib.ormJSonFL;
using static prjx.lib.strCls;
using static mdsj.lib.bscEncdCls;
using static mdsj.lib.net_http;
using static mdsj.lib.util;
using static mdsj.lib.web3;
using static mdsj.libBiz.tgBiz;
using System.Collections;
namespace mdsj.lib
{

    //go语言写一个函数，
    internal class web3
    {
        private const string InfuraProjectId = "your_infura_project_id";
        private const string PrivateKey = "your_private_key";
        private const string USDTContractAddress = "0x1234567890abcdef1234567890abcdef12345678"; // Replace with actual USDT contract address on the selected chain
        private const string UniswapRouterAddress = "0xUniswapRouterAddress"; // Replace with actual Uniswap router address


        public static async Task<Dictionary<string, double>> GetCryptoPricesAsync(string cryptoSymbols)
        {
            try
            {
                var prices = new Dictionary<string, double>();
                using (var client = new HttpClient())
                {
                    string[] symbols = cryptoSymbols.Split(',');
                    foreach (var symbol in symbols)
                    {
                        string url = $"https://api.coingecko.com/api/v3/simple/price?ids={symbol}&vs_currencies=usd";
                       Print(url);
                        var response = await client.GetStringAsync(url);
                       Print(response);
                        var json = JObject.Parse(response);
                        if (json[symbol] != null && json[symbol]["usd"] != null)
                        {
                            prices[symbol] = json[symbol]["usd"].Value<double>();
                        }
                        else
                        {
                           Print($"未能获取 {symbol} 的价格。");
                        }
                    }
                }
                return prices;
            }
            catch (Exception) {
                return null;
            }
         
        }
        //public async Task BuyEthereumAsync(decimal usdAmount, string network)
        //{
        //    string url = network switch
        //    {
        //        "optimism" => $"https://optimism.infura.io/v3/{InfuraProjectId}",
        //        "arbitrum" => $"https://arbitrum.infura.io/v3/{InfuraProjectId}",
        //        _ => throw new ArgumentException("Unsupported network")
        //    };

        //    var account = new Account(PrivateKey);
        //    var web3 = new Web3(account, url);

        //    // Get USDT balance
        //    var usdtService = new StandardTokenService(web3, USDTContractAddress);
        //    var usdtBalance = await usdtService.BalanceOfQueryAsync(account.Address);
        //    var usdAmountInWei = Web3.Convert.ToWei(usdAmount);

        //    if (usdtBalance < usdAmountInWei)
        //    {
        //        throw new Exception("Insufficient USDT balance");
        //    }

        //    // Approve the Uniswap router to spend USDT
        //    var approveFunction = usdtService.GetFunction("approve");
        //    var approveReceipt = await approveFunction.SendTransactionAndWaitForReceiptAsync(account.Address, UniswapV3RouterAddress, usdAmountInWei);

        //    // Get ETH price from Uniswap V3
        //    var router = new UniswapV3Router(web3, UniswapV3RouterAddress);
        //    var path = new List<string> { USDTContractAddress, "0xETHAddress" }; // Replace with actual ETH contract address
        //    var amountsOut = await router.GetAmountsOutQueryAsync(usdAmountInWei, path);
        //    var ethAmount = amountsOut[1];

        //    // Perform the swap
        //    var deadline = new HexBigInteger(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 120); // 2 minutes from now
        //    var swapFunction = router.SwapExactTokensForETHSupportingFeeOnTransferTokensFunction(usdAmountInWei, ethAmount, path, account.Address, deadline);
        //    var gas = new HexBigInteger(300000);
        //    var receipt = await web3.Eth.Transactions.SendTransactionAndWaitForReceiptAsync(swapFunction.CreateTransactionInput(gas));

        //   print("Swap successful, transaction hash: " + receipt.TransactionHash);
        //}


        //public async Task ApproveUniswapV2Async(decimal amountInUSDT)
        //{
        //    string url = $"https://mainnet.infura.io/v3/{InfuraProjectId}"; // Mainnet URL

        //    var account = new Account(PrivateKey);
        //    var web3 = new Web3(account, url);

        //    // Create StandardTokenService instance for USDT
        //    var usdtService = new StandardTokenService(web3, USDTContractAddress);

        //    // Approve UniswapV2 Router contract address
        //    var uniswapV2RouterAddress = "0x7a250d5630B4cF539739dF2C5dAcb4c659F2488D"; // Example Uniswap V2 Router address
        //    var approveFunction = new FunctionBuilder()
        //        .Name("approve")
        //        .InputParameter("spender", uniswapV2RouterAddress)
        //        .InputParameter("amount", amountInUSDT * (decimal)Math.Pow(10, 6)) // Convert to USDT's decimals (6)
        //        .Build();

        //    var gas = new HexBigInteger(300000);
        //    var receipt = await web3.Eth.Transactions.SendTransactionAndWaitForReceiptAsync(approveFunction.CreateTransactionInput(account.Address, gas));

        //   print("Approval successful, transaction hash: " + receipt.TransactionHash);
        //}

        private const string ApiKey = "your_api_key";
        private const string ApiSecret = "your_api_secret";

        //public async Task BuyEthereumAsync(decimal usdAmount)
        //{
        //    // 创建客户端实例
        //    using var client = new BinanceClient(new BinanceClientOptions
        //    {
        //        ApiCredentials = new ApiCredentials(ApiKey, ApiSecret),
        //        LogVerbosity = LogVerbosity.Info,
        //        LogWriters = new List<TextWriter> { Console.Out }
        //    });

        //    // 获取ETH/USDT的当前价格
        //    var priceResult = await client.Spot.Market.GetPriceAsync("ETHUSDT");
        //    if (!priceResult.Success)
        //    {
        //       print($"Error retrieving ETH price: {priceResult.Error}");
        //        return;
        //    }

        //    var ethPrice = priceResult.Data.Price;
        //   print($"Current ETH price: {ethPrice}");

        //    // 计算需要购买的ETH数量
        //    var ethQuantity = usdAmount / ethPrice;
        //   print($"ETH quantity to buy: {ethQuantity}");

        //    // 购买ETH
        //    var orderResult = await client.Spot.Order.PlaceOrderAsync(
        //        symbol: "ETHUSDT",
        //        side: OrderSide.Buy,
        //        type: OrderType.Market,
        //        quantity: ethQuantity
        //    );

        //    if (orderResult.Success)
        //    {
        //       print("Order placed successfully");
        //    }
        //    else
        //    {
        //       print($"Error placing order: {orderResult.Error}");
        //    }
        //}



        public static void rdCnPrs()
        {
            return;
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod()));
           Print(DateTime.Now);
            string url = "https://coinmarketcap.com/";
            //string htm = GetHtmlContent(url);
            //file_put_contents("cn2004.htm", htm);
           Print("GetEthPrice()");
            double prs = (double)GetEthPrice();
            double bijiaoPrc = 3400;
            double pre = bijiaoPrc * 0.85;
            double next = bijiaoPrc * 1.015;
           Print(json_encode((prs: prs, pre: pre, next: next)));
            if (prs < pre || prs > next)
            {
                sendNotyfy2me();
                playMp3(mp3FilePathEmgcy);

            }

            prs = (double)GetBitcoinPrice();
            bijiaoPrc = 64255;
            pre = bijiaoPrc * 0.85;
            next = bijiaoPrc * 1.015;
           Print("GetBitcoinPrice()");
           Print(json_encode((prs: prs, pre: pre, next: next)));
            if (prs < pre || prs > next)
            {
                sendNotyfy2me();
                playMp3(mp3FilePathEmgcy);
            }


            //if (prs < 3480 || prs > 3650)
            //{
            //    sendNotyfy2me();

            //}
           Print(prs);
            dbgCls.PrintRet(__METHOD__, prs);
        }

        private static void sendNotyfy2me()
        {
            try
            {
                Program.botClient.SendTextMessageAsync(879006550, "prs快来回复本信息,快来回复本信息快来回复本信息prs", parseMode: ParseMode.Html);
            }
            catch (Exception e)
            {
               Print("告知@回复本信息,搜商家联系方式时出错:" + e.Message);
            }

            try
            {
                Program.botClient.SendTextMessageAsync(2078535546, "prs快来回复本信息,快来回复本信息快来回复本信息", parseMode: ParseMode.Html);
            }
            catch (Exception e)
            {
               Print("告知@回复本信息,搜商家联系方式时出错:" + e.Message);
            }
        }

        static decimal GetBitcoinPrice()
        {
            try
            {
               Print("FUN GetBitcoinPrice()");
                using (HttpClient client = new HttpClient())
                {
                    // 请求CoinGecko API获取比特币当前价格
                    HttpResponseMessage response = client.GetAsync("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd").Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Failed to fetch Bitcoin price.");
                    }

                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    JObject json = JObject.Parse(jsonResponse);
                    decimal price = json["bitcoin"]["usd"].Value<decimal>();
                   Print(json_encode(json));
                   Print("end FUN GetBitcoinPrice");
                    return price;
                }
            }
            catch (Exception e)
            {
               Print($"Error: {e.Message}"); return 0;
            }

        }
        //        {
        //  "ethereum": {
        //    "usd": 3604.69
        //  }
        //}
        public static decimal GetEthPrice()
        {
            try
            {
               Print("FUN GetEthPrice()");
                string url = "https://api.coingecko.com/api/v3/simple/price?ids=ethereum&vs_currencies=usd";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = client.GetAsync(url).Result;
                        response.EnsureSuccessStatusCode();
                        string responseBody = response.Content.ReadAsStringAsync().Result;

                        JObject json = JObject.Parse(responseBody);
                        file_put_contents("cns.json", json_encode(json));
                        decimal ethPrice = json["ethereum"]["usd"].Value<decimal>();
                       Print(json_encode(json));
                       Print("end FUN GetEthPric");
                        return ethPrice;
                    }
                    catch (HttpRequestException e)
                    {
                       Print($"Request error: {e.Message}");
                        return 0;
                    }
                }

            }
            catch (Exception e)
            {
               Print($"Error: {e.Message}"); return 0;
            }
        }

    }


    public class UniswapV3Router
    {
        private readonly Web3 _web3;
        private readonly Contract _contract;

        public UniswapV3Router(Web3 web3, string contractAddress)
        {
            _web3 = web3;
            _contract = _web3.Eth.GetContract(UniswapV3RouterABI, contractAddress);
        }

        public async Task<BigInteger[]> GetAmountsOutQueryAsync(BigInteger amountIn, List<string> path)
        {
            var function = _contract.GetFunction("getAmountsOut");
            return await function.CallAsync<BigInteger[]>(amountIn, path);
        }

        public Function SwapExactTokensForETHFunction(BigInteger amountIn, BigInteger amountOutMin, List<string> path, string to, HexBigInteger deadline)
        {
            var function = _contract.GetFunction("swapExactTokensForETH");
            return function;
        }

        private const string UniswapV3RouterABI = @"[
        {
            'inputs': [
                { 'internalType': 'uint256', 'name': 'amountIn', 'type': 'uint256' },
                { 'internalType': 'uint256', 'name': 'amountOutMin', 'type': 'uint256' },
                { 'internalType': 'address[]', 'name': 'path', 'type': 'address[]' },
                { 'internalType': 'address', 'name': 'to', 'type': 'address' },
                { 'internalType': 'uint256', 'name': 'deadline', 'type': 'uint256' }
            ],
            'name': 'swapExactTokensForETH',
            'outputs': [],
            'stateMutability': 'nonpayable',
            'type': 'function'
        },
        {
            'inputs': [
                { 'internalType': 'uint256', 'name': 'amountIn', 'type': 'uint256' },
                { 'internalType': 'address[]', 'name': 'path', 'type': 'address[]' }
            ],
            'name': 'getAmountsOut',
            'outputs': [
                { 'internalType': 'uint256[]', 'name': 'amounts', 'type': 'uint256[]' }
            ],
            'stateMutability': 'view',
            'type': 'function'
        }
    ]";
    }

}
