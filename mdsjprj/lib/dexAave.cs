using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class AaveCollateralInfo
    {
        //
        public static string _subgraphUrl = "https://api.thegraph.com/subgraphs/name/aave/protocol-multy-raw";

        public static async Task GetCollateralInfo(string userAddress)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var query = $@"
                {{
                    users(where: {{id: ""{userAddress.ToLower()}"", }} ) {{
                        deposits {{
                            id
                            amount
                            reserve {{
                                symbol
                                reserveFactor
                            }}
                        }}
                    }}
                }}";
                    Console.WriteLine(query);



                    _subgraphUrl = "https://api.thegraph.com/subgraphs/name/aave/protocol";

                    // 替换为最新的 Aave Subgraph API URL
                    _subgraphUrl = "https://api.thegraph.com/subgraphs/name/aave/protocol-v2";
                    _subgraphUrl = "https://api.thegraph.com/subgraphs/name/aave/protocol-v3";
                    //https://api.thegraph.com/subgraphs/name/aave/protocol-v3
                    _subgraphUrl = "https://thegraph.com/hosted-service/subgraph/aave/protocol-v3";
                //    https://thegraph.com/hosted-service/subgraph/aave/protocol-v3

                https://gateway-arbitrum.network.thegraph.com/api/[api-key]/subgraphs/id/Cd2gEDVeqnjBn1hSeqFMitw8Q1iiyV9FYUZkLNRcL87g
                    var content = new StringContent(
                 JsonConvert.SerializeObject(new { query = query }),
                 Encoding.UTF8,
                 "application/json"
             );

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(_subgraphUrl),
                        Content = content
                    };

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        File.WriteAllText("aave.json", responseContent);
                        var responseData = JsonConvert.DeserializeObject<JObject>(responseContent);

                        // Parse the response and display collateral information
                        // Parse the response and display collateral information
                        var user = responseData["data"]["users"].FirstOrDefault();
                        if (user != null)
                        {
                            foreach (var deposit in user["deposits"])
                            {
                                var collateralAmount = deposit["amount"].Value<decimal>();
                                var collateralAssetSymbol = deposit["reserve"]["symbol"].Value<string>();
                                var reserveFactor = deposit["reserve"]["reserveFactor"].Value<decimal>();

                                Console.WriteLine($"Collateral Asset: {collateralAssetSymbol}");
                                Console.WriteLine($"Collateral Amount: {collateralAmount}");
                                Console.WriteLine($"Collateral Reserve Factor: {reserveFactor}");
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data found for the user.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving data: {ex.Message}");
            }
        }
    }

}
