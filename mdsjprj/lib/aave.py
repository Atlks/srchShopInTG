from web3 import Web3
import requests

# Aave LendingPool contract address and ABI
AAVE_LENDING_POOL_ADDRESS = "0x398EC7346DcD622eDc5ae82352F02Be94C62d119"  # Aave LendingPool contract on Ethereum mainnet
AAVE_LENDING_POOL_ABI_URL = "https://raw.githubusercontent.com/aave/protocol-v2/master/abi/LendingPool.json"

# Connect to an Ethereum node
web3 = Web3(Web3.HTTPProvider("https://mainnet.infura.io/v3/YOUR_INFURA_PROJECT_ID"))

# Load the LendingPool contract ABI
response = requests.get(AAVE_LENDING_POOL_ABI_URL)
lending_pool_abi = response.json()

# Create the LendingPool contract object
lending_pool_contract = web3.eth.contract(address=AAVE_LENDING_POOL_ADDRESS, abi=lending_pool_abi)

def get_user_reserves_data(user_address):
    user_data = lending_pool_contract.functions.getUserAccountData(user_address).call()
    total_collateral_eth = user_data[0] / 10**18
    total_debt_eth = user_data[1] / 10**18
    available_borrow_eth = user_data[2] / 10**18
    current_liquidation_threshold = user_data[3] / 100
    ltv = user_data[4] / 100
    health_factor = user_data[5] / 10**18

    return {
        "total_collateral_eth": total_collateral_eth,
        "total_debt_eth": total_debt_eth,
        "available_borrow_eth": available_borrow_eth,
        "current_liquidation_threshold": current_liquidation_threshold,
        "ltv": ltv,
        "health_factor": health_factor,
    }

def get_staking_ratio(user_address):
    user_data = get_user_reserves_data(user_address)
    total_collateral_eth = user_data["total_collateral_eth"]
    total_debt_eth = user_data["total_debt_eth"]

    if total_collateral_eth == 0:
        return 0

    staking_ratio = total_collateral_eth / (total_collateral_eth + total_debt_eth)
    return staking_ratio

# Example usage
user_address = "0xYourEthereumAddress"
staking_ratio = get_staking_ratio(user_address)
print(f"The staking ratio for the address {user_address} is {staking_ratio:.2%}")
