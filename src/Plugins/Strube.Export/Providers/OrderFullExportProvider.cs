using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartStore.Collections;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.DataExchange;
using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Localization;
using SmartStore.Core.Logging;
using SmartStore.Core.Plugins;
using SmartStore.Services;
using SmartStore.Services.Catalog;
using SmartStore.Services.DataExchange.Export;
using SmartStore.Services.Directory;
using SmartStore.Services.Localization;

namespace Strube.Export.Providers
{
    [SystemName("Strube.OrdersExportCSV")]
    [FriendlyName("Strube Full Order csv-Export")]
    [DisplayOrder(1)]
    [ExportFeatures(Features =
    ExportFeatures.CreatesInitialPublicDeployment |
    ExportFeatures.CanOmitGroupedProducts |
    ExportFeatures.CanProjectAttributeCombinations |
    ExportFeatures.CanProjectDescription |
    ExportFeatures.UsesSkuAsMpnFallback |
    ExportFeatures.OffersBrandFallback |
    ExportFeatures.UsesSpecialPrice |
    ExportFeatures.UsesAttributeCombination)]
    public class OrderFullExportProvider : ExportProviderBase
    {
        public override ExportEntityType EntityType
        {
            get { return ExportEntityType.Order; }
        }

        public override string FileExtension
        {
            get { return "txt"; }
        }

        protected override void Export(ExportExecuteContext context)
        {
            throw new NotImplementedException();
        }

        //public override ExportConfigurationInfo ConfigurationInfo
        //{
        //    get { return null; }
        //}

        public override void OnExecuted(ExportExecuteContext context)
        {

        }
    }
}