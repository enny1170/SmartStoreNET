using SmartStore.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Strube.Export.Models
{
    public class OrderDetail
    {
        [FieldOrder(0)]
        public string Id { get; set; }
        [FieldOrder(1)]
        public string OrderId { get; set; }
        [FieldOrder(2)]
        public string Comment { get; set; }
        [FieldOrder(3)]
        public string Company { get; set; }
        [FieldOrder(4)]
        public string Name { get; set; }
        [FieldOrder(5)]
        public string Surname { get; set; }
        [FieldOrder(6)]
        public string Address1 { get; set; }
        [FieldOrder(7)]
        public string Address2 { get; set; }
        [FieldOrder(8)]
        public string ZipCode { get; set; }
        [FieldOrder(9)]
        public string City { get; set; }
        [FieldOrder(10)]
        public string Country { get; set; }
        [FieldOrder(11)]
        public string ItemId { get; set; }
        [FieldOrder(12)]
        public string Description { get; set; }
        [FieldOrder(13)]
        public int Count { get; set; }
        [FieldOrder(14)]
        public string TrackingId { get; set; }
        [FieldOrder(15)]
        public DateTime? ShipDateTime { get; set; }

        public OrderDetail()
        {

        }

        public OrderDetail(OrderItem orderItem)
        {
            this.Id = orderItem.Order.OrderGuid.ToString();
            this.OrderId = orderItem.Order.GetOrderNumber();
            this.Comment = orderItem.Order.CustomerOrderComment;
            this.Company = orderItem.Order.ShippingAddress.Company;
            this.Name = orderItem.Order.ShippingAddress.LastName;
            this.Surname = orderItem.Order.ShippingAddress.FirstName;
            this.Address1 = orderItem.Order.ShippingAddress.Address1;
            this.Address2 = orderItem.Order.ShippingAddress.Address2;
            this.ZipCode = orderItem.Order.ShippingAddress.ZipPostalCode;
            this.City = orderItem.Order.ShippingAddress.City;
            this.Country = orderItem.Order.ShippingAddress.Country.Name;
            this.ItemId = orderItem.Product.Sku;
            this.Description = orderItem.Product.Name;
            this.Count = orderItem.Quantity;
            this.TrackingId = "";
            this.ShipDateTime = null;
        }

        /// <summary>
        /// Creates A Header Line for CSV depending on Properties and FieldOrder
        /// </summary>
        /// <param name="Seperator">Seperator to use default ';'</param>
        /// <returns>a Seperator sperated String with property names</returns>
        public string GetCSVHeader(string Seperator=";")
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] props = this.GetType().GetProperties();
            List<PropertyInfo> propertyInfos = props.Where(p => p.GetCustomAttribute<FieldOrderAttribute>() != null).OrderBy(a => a.GetCustomAttribute<FieldOrderAttribute>().Index).ToList();

            foreach (var item in propertyInfos)
            {
                sb.Append(item.Name);
                sb.Append(Seperator);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a Line for CSV depending on Properties and FieldOrder
        /// </summary>
        /// <param name="Seperator">Seperator to use default ';'</param>
        /// <returns>a Seperator sperated String with property values</returns>
        public string GetCSVLine(string Seperator=";")
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] props = this.GetType().GetProperties();
            List<PropertyInfo> propertyInfos = props.Where(p => p.GetCustomAttribute<FieldOrderAttribute>() != null).OrderBy(a => a.GetCustomAttribute<FieldOrderAttribute>().Index).ToList();

            foreach (var item in propertyInfos)
            {
                if (item.GetValue(this) != null)
                {
                    sb.Append(item.GetValue(this).ToString());
                    sb.Append(Seperator);
                }
                else
                {
                    sb.Append("");
                    sb.Append(Seperator);
                }
            }
            return sb.ToString();
        }
    }
}