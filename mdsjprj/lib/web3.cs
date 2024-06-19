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
using prj202405;
using prj202405.lib;
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

namespace mdsj.lib
{

    //go语言写一个函数，
    internal class web3
    {
        private const string InfuraProjectId = "your_infura_project_id";
        private const string PrivateKey = "your_private_key";
        private const string USDTContractAddress = "0x1234567890abcdef1234567890abcdef12345678"; // Replace with actual USDT contract address on the selected chain
        private const string UniswapRouterAddress = "0xUniswapRouterAddress"; // Replace with actual Uniswap router address

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

        //    Console.WriteLine("Swap successful, transaction hash: " + receipt.TransactionHash);
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

        //    Console.WriteLine("Approval successful, transaction hash: " + receipt.TransactionHash);
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
        //        Console.WriteLine($"Error retrieving ETH price: {priceResult.Error}");
        //        return;
        //    }

        //    var ethPrice = priceResult.Data.Price;
        //    Console.WriteLine($"Current ETH price: {ethPrice}");

        //    // 计算需要购买的ETH数量
        //    var ethQuantity = usdAmount / ethPrice;
        //    Console.WriteLine($"ETH quantity to buy: {ethQuantity}");

        //    // 购买ETH
        //    var orderResult = await client.Spot.Order.PlaceOrderAsync(
        //        symbol: "ETHUSDT",
        //        side: OrderSide.Buy,
        //        type: OrderType.Market,
        //        quantity: ethQuantity
        //    );

        //    if (orderResult.Success)
        //    {
        //        Console.WriteLine("Order placed successfully");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Error placing order: {orderResult.Error}");
        //    }
        //}
        public static void rdCnPrs()
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod()));
            Console.WriteLine(DateTime.Now);
            string url = "https://coinmarketcap.com/";
            //string htm = GetHtmlContent(url);
            //file_put_contents("cn2004.htm", htm);
            Console.WriteLine("GetEthPrice()");
            decimal prs = GetEthPrice();
            if (prs < 3480 || prs>3600 )
            {
                try
                {
                    Program.botClient.SendTextMessageAsync(879006550, "快来回复本信息,快来回复本信息快来回复本信息", parseMode: ParseMode.Html);
                }
                catch (Exception e)
                {
                    Console.WriteLine("告知@回复本信息,搜商家联系方式时出错:" + e.Message);
                }

                try
                {
                    Program.botClient.SendTextMessageAsync(2078535546, "快来回复本信息,快来回复本信息快来回复本信息", parseMode: ParseMode.Html);
                }
                catch (Exception e)
                {
                    Console.WriteLine("告知@回复本信息,搜商家联系方式时出错:" + e.Message);
                }
                
            }
            Console.WriteLine(prs);
            dbgCls.setDbgValRtval(__METHOD__, prs);
        }


        public static decimal GetEthPrice()
        {
            string url = "https://api.coingecko.com/api/v3/simple/price?ids=ethereum&vs_currencies=usd";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    JObject json = JObject.Parse(responseBody);
                    decimal ethPrice = json["ethereum"]["usd"].Value<decimal>();

                    return ethPrice;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                    return 0;
                }
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
