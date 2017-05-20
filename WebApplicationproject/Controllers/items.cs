using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationproject.Models;

namespace WebApplicationproject.Controllers
{
    public class items
    {
        private product product = new product();
        public product Product
        {
            get { return product; }
            set { product = value; }
        }
        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public items()
        { }
        public items(product product, int quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }
    }
}