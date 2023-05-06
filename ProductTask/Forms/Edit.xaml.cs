using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProductTask.Models;

namespace ProductTask.Forms
{
    public partial class Edit : Window
    {
        public Product Product { get; set; }
        public Edit(Product product)
        {
            InitializeComponent();
            Product = product;
            DataContext = Product;
        }
        //Button save new info
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Product product = new Product()
            {
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price,
                ID = Product.ID,
            };
        }
        //Button remove
        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
