using System.Data;
using System.Linq.Expressions;
using VendingMachineAPI.InfraStructure.DataServices;
using VendingMachineAPI.InfraStructure.Entity;
using VendingMachineAPI.Presentation.ViewModels;

namespace VendingMachineAPI.BusinessLogic.Services
{
    public class VendingMachineManager : IVendingMachineManager
    {
        private readonly IUserManager _userManager;
        private readonly IVendingMachineRepository _vendingMachineRepository;
        private readonly IProductsManager _productsManager;

        public VendingMachineManager(IUserManager cache, IVendingMachineRepository vendingMachineRepository, IProductsManager productsManager)
        {
            _userManager = cache;
            _vendingMachineRepository = vendingMachineRepository;
            _productsManager = productsManager;
        }
        
        public virtual async Task<VendingMachineCoins> AddAsync(VendingMachineCoins entity)
        {
            await _vendingMachineRepository.AddAsync(entity);
            return entity;
        }

        public virtual IEnumerable<VendingMachineCoins> GetAll() => _vendingMachineRepository.GetAll();

        public async Task UpdateAsync(Coins entity)
        {
            var trx = await _vendingMachineRepository.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                var coin = await _vendingMachineRepository.FindOneAsync(s => s.Coin == entity.Cent);
                coin.AvailableAmount += entity.Amount;
                await SaveChangesAsync();
                trx.Commit();
            }
            catch (Exception ex)
            {
                trx.Rollback();
            }
        }

        public async Task SaveChangesAsync() => await _vendingMachineRepository.SaveChangesAsync();

        public async Task<VendingMachineCoins> DeleteAsync(VendingMachineCoins entity)
        {
            await _vendingMachineRepository.DeleteAsync(entity);
            return entity;
        }

        public async Task<VendingMachineCoins?> FindOneAsync(Expression<Func<VendingMachineCoins, bool>> predicate)
            => await _vendingMachineRepository.FindOneAsync(predicate);
        
        public async Task<bool> IsExistAsync(Expression<Func<VendingMachineCoins, bool>> predicate)
            => await _vendingMachineRepository.IsExistAsync(predicate);
        
        public async Task<BuyModel?> BuyAsync(int productId, int amount, string currentUserId)
        {
            var trx = await _vendingMachineRepository.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                var product = await _productsManager.GetByIdAsync(productId);

                if (product == null)
                {
                    throw new InvalidOperationException($"this product not found with id = {productId}");
                }
                if (amount > product.AvailableAmount)
                {
                    throw new InvalidOperationException($"this this product doesn't have this amount, the current amount for product = {product.AvailableAmount}");
                }
                var currentUser = await _userManager.GetByIdAsync(currentUserId);
                if (amount * product.Cost > currentUser?.Deposit)
                {
                    throw new InvalidOperationException($"this current buyer doesn't have deposit for current transaction, the current deposit for buyer = {currentUser.Deposit}");
                }
                decimal totalAmount = amount * product.Cost;
                decimal remainingDeposit = currentUser.Deposit - totalAmount;
                var coins = GetAll().OrderByDescending(s => s.Coin);
                var buyModel = new BuyModel();
                buyModel.TotalAmount = (decimal)totalAmount;
                List<Coins> coinsList = new List<Coins>();
                foreach (var coin in coins)
                {
                    var coinChange = new Coins
                    {
                        Cent = coin.Coin,
                        Amount = (int)(remainingDeposit / coin.Coin),
                    };
                    coinsList.Add(coinChange);
                    remainingDeposit %= coin.Coin;
                }
                currentUser.Deposit += remainingDeposit;
                await _userManager.UpdateAsync(currentUser, currentUserId);
                product.AvailableAmount -= amount;
                await _productsManager.SaveChangesAsync();
                trx.Commit();
                return buyModel;
            }
            catch (Exception ex)
            {
                trx.Rollback();
            }
            return null;
        }

        public async Task<List<Coins>> ResetAsync(string currentUserId)
        {
            var currentUser = await _userManager.GetByIdAsync(currentUserId);

            var coins = GetAll().OrderByDescending(s => s.Coin);
            List<Coins> coinsList = new List<Coins>();
            decimal userDeposit = currentUser.Deposit;
            foreach (var coin in coins)
            {
                var coinChange = new Coins
                {
                    Cent = coin.Coin,
                    Amount = (int)(userDeposit / coin.Coin),
                };
                coinsList.Add(coinChange);
                userDeposit %= coin.Coin;
            }
            currentUser.Deposit -= userDeposit;
            await _userManager.UpdateAsync(currentUser, currentUserId);
            await SaveChangesAsync();
            return coinsList;
        }
    }
}
