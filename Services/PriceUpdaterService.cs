using AutoMapper;
using BCrypt.Net;
using CryptoWallet.Dto;
using CryptoWallet.Model;
using CryptoWallet.Repository;
using Microsoft.Identity.Client;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CryptoWallet.Services
{
    public class PriceUpdaterService : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public PriceUpdaterService(HttpClient httpClient, IServiceScopeFactory scopeFactory)
        {
            _httpClient = httpClient;
            _scopeFactory = scopeFactory;
        }

        //ASP.NET-ben az ExecuteAsync az automatikusan induló metódus a háttérszolgáltatás indulásakor
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var _limitOrderService = scope.ServiceProvider.GetRequiredService<ILimitOrderService>();

                    var cryptos = await _unitOfWork.CryptocurrencyRepository.GetAllAsync();
                    foreach (var crypto in cryptos)
                    {
                        Cryptocurrency updatedCrypto = null;
                        switch (crypto.Name)
                        {                            
                            case "Avalanche":
                                await PriceUpdateAsync("avalanche-2");

                                updatedCrypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(crypto.Id);
                                await _limitOrderService.ExecuteLimitOrdersAsync(updatedCrypto);
                                break;
                            case "BNB":
                                await PriceUpdateAsync("binancecoin");

                                updatedCrypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(crypto.Id);
                                await _limitOrderService.ExecuteLimitOrdersAsync(updatedCrypto);
                                break;        
                            case "Near Protocol":
                                await PriceUpdateAsync("near");

                                updatedCrypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(crypto.Id);
                                await _limitOrderService.ExecuteLimitOrdersAsync(updatedCrypto);
                                break;
                            case "Artificial Superintelligence Alliance":
                                await PriceUpdateAsync("fetch-ai");

                                updatedCrypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(crypto.Id);
                                await _limitOrderService.ExecuteLimitOrdersAsync(updatedCrypto);
                                break;
                            default:
                                await PriceUpdateAsync(crypto.Name.ToLower());
                                updatedCrypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(crypto.Id);

                                if(updatedCrypto != null)
                                {
                                    await _limitOrderService.ExecuteLimitOrdersAsync(updatedCrypto);
                                }                               
                                break;
                        }                        
                    }
                    
                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
            }                                            
        }
        public async Task PriceUpdateAsync(string cryptocurrencyName)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&ids={cryptocurrencyName}&x_cg_demo_api_key=CG-bwNkXijofCCDxiWQ8sAMR5dY";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<List<Cryptocurrency>>(responseString);

            //Mivel a háttérszolgáltatás Singleton (IHostedService), a unitOfWork Scoped, nem injektálható a konstruktorba
            //->Violation of the dependency injection lifetime rules
            using (var scope = _scopeFactory.CreateScope())
            {
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var crypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync((data[0].Id).ToUpper());
                if (crypto != null)
                {
                    crypto.Price = data[0].Price;
                    _unitOfWork.CryptocurrencyRepository.Update(crypto);

                    var priceHistory = new PriceHistory
                    {
                        Price = data[0].Price,
                        Timestamp = DateTime.UtcNow,
                        CryptocurrencyId = crypto.Id,
                    };
                    await _unitOfWork.PriceHistoryRepository.PostAsync(priceHistory);

                    await _unitOfWork.SaveAsync();
                }
            }            
        }
    }
}
