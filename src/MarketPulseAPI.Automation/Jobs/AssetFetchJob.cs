using MarketPriceAPI.Data.Entities;
using MarketPriceAPI.Data.Entities.Extsnions;
using MarketPulseAPI.Fintacharts.Models.Queries;
using MarketPulseAPI.Interfaces.Clients.Fintacharts;
using MarketPulseAPI.Interfaces.Data.Repositories;
using Quartz;

namespace MarketPulseAPI.Automation.Jobs
{
    public class AssetFetchJob : IJob
    {
        private readonly IFintachartsHttpClient _fintachartsHttpClient;
        private readonly IAssetsRepository _assetsRepository;
        public AssetFetchJob(IFintachartsHttpClient fintachartsHttpClient, IAssetsRepository assetsRepository)
        {
            _fintachartsHttpClient = fintachartsHttpClient;
            _assetsRepository = assetsRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var cancellationToken = context.CancellationToken;

            int currentPage = 1;
            int totalPages = 1;

            var newAssets = new List<Asset>();
            var updatedAssets = new List<Asset>();

            var existingAssetsIds = new HashSet<Guid>(await _assetsRepository.GetAllIdsAsync());

            while (currentPage <= totalPages)
            {
                var instrumentsPage = await _fintachartsHttpClient.GetInstrumentsAsync(
                    new InstrumentsQuery(page: currentPage, size: 100),
                    cancellationToken);

                if (instrumentsPage == null)
                {
                    break;
                }

                foreach (var instrument in instrumentsPage.Data)
                {
                    if (!existingAssetsIds.Contains(instrument.Id))
                    {
                        newAssets.Add(new Asset
                        {
                            Id = instrument.Id,
                            Symbol = instrument.Symbol,
                            Description = instrument.Description,
                            TickSize = instrument.TickSize,
                            Currency = instrument.Currency,
                            BaseCurrency = instrument.BaseCurrency,
                            Kind = instrument.Kind
                        });
                    }
                    else
                    {
                        var existingAsset = await _assetsRepository.GetByIdAsync(instrument.Id);
                        if (existingAsset.HasChanged(instrument))
                        {
                            updatedAssets.Add(new Asset
                            {
                                Id = instrument.Id,
                                Symbol = instrument.Symbol,
                                Description = instrument.Description,
                                TickSize = instrument.TickSize,
                                Currency = instrument.Currency,
                                BaseCurrency = instrument.BaseCurrency
                            });
                        }
                    }
                }

                if (newAssets.Count != 0)
                {
                    await _assetsRepository.AddManyAsync(newAssets);
                    newAssets.Clear();
                }

                if (updatedAssets.Count != 0)
                {
                    await _assetsRepository.UpdateManyAsync(updatedAssets);
                    updatedAssets.Clear();
                }

                totalPages = instrumentsPage.Paging.Pages;
                currentPage++;
            }
        }
    }
}
