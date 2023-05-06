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
    public partial class Dialog : Window
    {
        public Product Product { get; set; }
        public Dialog()
        {
            InitializeComponent();
            Product = new Product();
            DataContext = Product;
            Product.ID = Guid.NewGuid();
        }
        //Button Add
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DatabaseContext databaseContext = new DatabaseContext();
                Product product = new Product()
                {
                    Name = Product.Name,
                    Description = Product.Description,
                    Price = Product.Price,
                    ID = Product.ID,
                };
                //Save info  
                databaseContext.Products.Add(product);
                databaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.ShowDialog();
        }

        //Exit

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.ShowDialog();
        }
    }
}
