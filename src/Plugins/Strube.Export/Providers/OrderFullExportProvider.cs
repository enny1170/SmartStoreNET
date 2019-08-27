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
using System.IO;
using SmartStore.Core.Domain.Orders;

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

        public static string SystemName
        {
            get { return "Strube.OrdersExportCSV"; }
        }

        public override string FileExtension
        {
            get { return "txt"; }
        }

        protected override void Export(ExportExecuteContext context)
        {
            dynamic currency = context.Currency;
            string _FormatString = "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}";

            //Create Streamwriter
            StreamWriter _sw = new StreamWriter(context.DataStream);
            //Add Header Line
            _sw.WriteLine(String.Format(_FormatString, 
                "ID", 
                "OrderId", 
                "Comment",
                "Company",
                "Name",
                "Surname",
                "Address1",
                "Address2",
                "Zip-Code",
                "City",
                "Country",
                "ItemId",
                "Description",
                "Count"));
            // export the lines
            while (context.Abort==DataExchangeAbortion.None && context.DataSegmenter.ReadNextSegment())
            {
                var segment = context.DataSegmenter.CurrentSegment;
                foreach (dynamic order in segment)
                {
                    Order orderEntity = order.Entity;
                    List<OrderItem> orderItem = orderEntity.OrderItems.ToList();

                    if (context.Abort!= DataExchangeAbortion.None)
                    {
                        break;
                    }

                    try
                    {
                        foreach (OrderItem item in orderItem)
                        {
                            Product itemProduct = item.Product;
                            _sw.WriteLine(String.Format(_FormatString,
                                orderEntity.OrderGuid,
                                orderEntity.GetOrderNumber(),
                                orderEntity.CustomerOrderComment,
                                orderEntity.ShippingAddress.Company,
                                orderEntity.ShippingAddress.LastName,
                                orderEntity.ShippingAddress.FirstName,
                                orderEntity.ShippingAddress.Address1,
                                orderEntity.ShippingAddress.Address2,
                                orderEntity.ShippingAddress.ZipPostalCode,
                                orderEntity.ShippingAddress.City,
                                orderEntity.ShippingAddress.Country.Name,
                                itemProduct.Sku,
                                itemProduct.Name,
                                item.Quantity
                                )) ;

                            ++context.RecordsSucceeded;

                        }
                    }
                    catch(Exception ex)
                    {
                        context.RecordException(ex, orderEntity.Id);
                    }
                }
            }
            _sw.Flush();
            //throw new NotImplementedException();
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